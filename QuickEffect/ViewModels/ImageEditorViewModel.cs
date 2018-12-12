using Microsoft.Win32;
using QuickEffect.Commands;
using QuickEffect.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for image editor view.
    /// </summary>
    public class ImageEditorViewModel : BaseViewModel
    {
        #region Private Members

        // Dialogs
        SaveFileDialog saveFileDialog;

        // UI
        private bool isBusy;
        private bool multiSelect;
        private bool allSelected;
        private int multiSelectCount = 0;
        private string imageBeingProcessed;

        // Multisave options
        private bool copyToFolder;

        // Collections
        private ObservableCollection<ImageItem> imageItems;

        // Message service
        private string message;
        private bool messageActive;

        // Selected image
        private BitmapImage selectedImage;
        private string selectedImagePath;
        private bool resettingUI;

        // Effects
        private CurrentImageHandler imageHandler;
        private Effect currentEffect;
        private bool original = true;
        private bool grayscale;
        private bool sepia;
        private bool redFilter;
        private bool greenFilter;
        private bool blueFilter;

        // Image specific
        private int rotationAngle = 0;
        private bool flipY;
        private bool flipX;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;

                NotifyPropertyChanged();
            }
        }

        public bool MultiSelect
        {
            get { return multiSelect && SelectedImage != null; }
            set
            {
                multiSelect = value;

                NotifyPropertyChanged();
            }
        }

        public bool AllSelected
        {
            get { return allSelected; }
            set
            {
                allSelected = value;

                // Set all image items to selected
                foreach (var imageItem in ImageItems)
                {
                    if (imageItem.IsSelected == !value)
                        imageItem.IsSelected = value;
                }

                NotifyPropertyChanged();
            }
        }

        public int MultiSelectCount
        {
            get { return multiSelectCount; }
            set
            {
                multiSelectCount = value;

                NotifyPropertyChanged();
            }
        }

        public string ImageBeingProcessed
        {
            get { return imageBeingProcessed; }
            set
            {
                imageBeingProcessed = value;

                NotifyPropertyChanged();
            }
        }

        public bool CopyToFolder
        {
            get { return copyToFolder; }
            set
            {
                copyToFolder = value;

                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<ImageItem> ImageItems
        {
            get { return imageItems; }
            set
            {
                imageItems = value;

                NotifyPropertyChanged();
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged();
            }
        }

        public bool MessageActive
        {
            get { return messageActive; }
            set
            {
                messageActive = value;
                NotifyPropertyChanged();
            }
        }

        public BitmapImage SelectedImage
        {
            get
            {
                return selectedImage;
            }
            set
            {
                // If value is null, check if it exists. If not, remove it and display message
                if (value == null)
                {
                    for (int i = 0; i < ImageItems.Count; i++)
                    {
                        if (!File.Exists(ImageItems[i].FileName))
                        {
                            ImageItems.Remove(ImageItems[i]);

                            SetMessage("One or more files could not be found.");
                        }
                    }
                }

                selectedImage = value;

                // If initial loading, load file into handler

                if (selectedImage != null)
                {
                    if (selectedImage.UriSource != null)
                    {
                        // Get absolute path
                        string path = selectedImage.UriSource.LocalPath;
                        // Set current file for processing (Locks resource!)
                        imageHandler.CurrentFileHandler.Load(path);

                        // Reset image properties if changing image source
                        if (!string.IsNullOrEmpty(selectedImagePath))
                        {
                            // If new image selected
                            if (selectedImagePath != selectedImage.UriSource.LocalPath)
                            {
                                resettingUI = true;

                                // Reset image and UI properties
                                Original = true;
                                currentEffect = Effect.nofilter;
                                flipY = false;
                                flipX = false;

                                resettingUI = false;
                            }
                        }

                        // Save path to original file
                        selectedImagePath = selectedImage.UriSource.LocalPath;
                    }
                }

                NotifyPropertyChanged();
            }
        }

        public bool Original
        {
            get { return original; }
            set
            {
                original = value;

                // If set to true and same file is selected, apply original effect
                if (original && !resettingUI)
                    ApplyOriginal();

                NotifyPropertyChanged();
            }
        }

        public bool Grayscale
        {
            get { return grayscale; }
            set
            {
                grayscale = value;

                if (grayscale)
                    ApplyGrayscale();

                NotifyPropertyChanged();
            }
        }

        public bool Sepia
        {
            get { return sepia; }
            set
            {
                sepia = value;

                if (sepia)
                    ApplySepia();

                NotifyPropertyChanged();
            }
        }

        public bool RedFilter
        {
            get { return redFilter; }
            set
            {
                redFilter = value;

                if (redFilter)
                    ApplyColorFilter(ImageFunctions.ColorFilterTypes.Red);

                NotifyPropertyChanged();
            }
        }

        public bool GreenFilter
        {
            get { return greenFilter; }
            set
            {
                greenFilter = value;

                if (greenFilter)
                    ApplyColorFilter(ImageFunctions.ColorFilterTypes.Green);

                NotifyPropertyChanged();
            }
        }

        public bool BlueFilter
        {
            get { return blueFilter; }
            set
            {
                blueFilter = value;

                if (blueFilter)
                    ApplyColorFilter(ImageFunctions.ColorFilterTypes.Blue);

                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private ICommand rotateImageCommand;
        public ICommand RotateImageCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (rotateImageCommand == null)
                    rotateImageCommand = new RelayCommand(async p => { await RotateAsync(Convert.ToSingle(p)); rotationAngle = Convert.ToInt32(p); });

                return rotateImageCommand;
            }
            set
            {
                rotateImageCommand = value;
            }
        }

        private ICommand flipXCommand;
        public ICommand FlipXCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (flipXCommand == null)
                    flipXCommand = new RelayCommand(async p => { flipX = !flipX; await FlipAsync(RotateFlipType.RotateNoneFlipX); });

                return flipXCommand;
            }
            set
            {
                flipXCommand = value;
            }
        }

        private ICommand flipYCommand;
        public ICommand FlipYCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (flipYCommand == null)
                    flipYCommand = new RelayCommand(async p => { flipY = !flipY; await FlipAsync(RotateFlipType.RotateNoneFlipY); });

                return flipYCommand;
            }
            set
            {
                flipYCommand = value;
            }
        }

        private ICommand saveImageCommand;
        public ICommand SaveImageCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (saveImageCommand == null)
                    saveImageCommand = new RelayCommand(async p => { await SaveImageAsync(); }, p => SelectedImage != null);

                return saveImageCommand;
            }
            set
            {
                saveImageCommand = value;
            }
        }

        private ICommand saveSelectedImagesCommand;
        public ICommand SaveSelectedImagesCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (saveSelectedImagesCommand == null)
                    saveSelectedImagesCommand = new RelayCommand(async p => { await SaveSelectedImagesAsync(); }, p => MultiSelect);

                return saveSelectedImagesCommand;
            }
            set
            {
                saveSelectedImagesCommand = value;
            }
        }


        #endregion

        #region Constructor

        public ImageEditorViewModel(ObservableCollection<string> fileNames)
        {
            // Initialize save file dialog
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "C:\\";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";

            // Initialize list
            ImageItems = new ObservableCollection<ImageItem>();
            foreach (var fileName in fileNames)
            {
                if (!File.Exists(fileName))
                    continue;

                ImageItems.Add(new ImageItem()
                {
                    FileName = fileName
                });
            }

            // Subscribe to property changes
            foreach (var imageItem in ImageItems)
            {
                imageItem.PropertyChanged += ImageItem_PropertyChanged;
            }

            imageHandler = new CurrentImageHandler();

            // Start with first image selected
            if (ImageItems.Count > 0)
                SelectedImage = new BitmapImage(new Uri(ImageItems[0].FileName));
        }

        #endregion

        #region Events

        private void ImageItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MultiSelectCount = 0;

            // Count selected images
            foreach (var imageItem in ImageItems)
            {
                if (imageItem.IsSelected)
                {
                    MultiSelectCount++;
                }
            }

            // Determine if multiselect is active
            if (MultiSelectCount > 0)
                MultiSelect = true;
            else MultiSelect = false;

            // Determine if all items are selected and notify property change to UI
            if (MultiSelectCount == ImageItems.Count)
                allSelected = true;
            else allSelected = false;
            NotifyPropertyChanged(nameof(AllSelected));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save currently selected image.
        /// </summary>
        private async Task SaveImageAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    IsBusy = true;

                    // Create new copy
                    string imagePath = Path.Combine(
                        Path.GetDirectoryName(selectedImagePath),
                        Path.GetFileNameWithoutExtension(selectedImagePath) +
                        " - (" + currentEffect.ToString() + ")" +
                        Path.GetExtension(selectedImagePath));

                    imageHandler.CurrentFileHandler.Save(imagePath);

                    // Open directory to the new copy
                    Process.Start(Path.GetDirectoryName(imagePath));
                });                
            }
            catch (Exception ex)
            {
                SetMessage("Error: " + ex.Message);
            }

            IsBusy = false;

            SetMessage("Image saved");

            return;
        }

        /// <summary>
        /// Apply current effect to selected images and save to disk.
        /// </summary>
        private async Task SaveSelectedImagesAsync()
        {
            try
            {
                await Task.Run(async () =>
                {
                    IsBusy = true;

                    // Iterate through all selected images
                    foreach (var imageItem in ImageItems)
                    {
                        if (imageItem.IsSelected)
                        {
                            // Notify UI about image being processed
                            ImageBeingProcessed = imageItem.FileName;

                            // Apply effect
                            BitmapImage image = await Task.Run(async () =>
                            {
                                // Load image into handler and return image with effect
                                imageHandler.CurrentFileHandler.Load(imageItem.FileName);
                                switch (currentEffect)
                                {
                                    case Effect.nofilter:                                        
                                        return await PaintImageAsync(imageHandler.CurrentGrayscaleHandler.SetGrayscale, imageItem.FileName);

                                    case Effect.grayscale:
                                        return await PaintImageAsync(imageHandler.CurrentGrayscaleHandler.SetGrayscale, imageItem.FileName);

                                    case Effect.sepia:
                                        return await PaintImageAsync(imageHandler.CurrentSepiaToneHandler.SetSepiaTone, imageItem.FileName);

                                    case Effect.redfilter:
                                        return await PaintImageColorAsync(
                                            new Action<ImageFunctions.ColorFilterTypes>(imageHandler.CurrentFilterHandler.SetColorFilter), 
                                            ImageFunctions.ColorFilterTypes.Red,
                                            imageItem.FileName);

                                    case Effect.greenfilter:
                                        return await PaintImageColorAsync(
                                            new Action<ImageFunctions.ColorFilterTypes>(imageHandler.CurrentFilterHandler.SetColorFilter),
                                            ImageFunctions.ColorFilterTypes.Red,
                                            imageItem.FileName);

                                    case Effect.bluefilter:
                                        return await PaintImageColorAsync(
                                            new Action<ImageFunctions.ColorFilterTypes>(imageHandler.CurrentFilterHandler.SetColorFilter),
                                            ImageFunctions.ColorFilterTypes.Red,
                                            imageItem.FileName);

                                    default:
                                        return null;
                                }
                            });

                            // Check if image is null
                            if (image == null)
                                return;

                            // Create new path
                            string imagePath = Path.Combine(
                                Path.GetDirectoryName(imageItem.FileName),
                                Path.GetFileNameWithoutExtension(imageItem.FileName) +
                                " - (" + currentEffect.ToString() + ")" +
                                Path.GetExtension(imageItem.FileName));

                            // Save image
                            imageHandler.CurrentFileHandler.Save(imagePath);                      
                        }
                    }
                });

                IsBusy = false;

                // Reset image being processed
                ImageBeingProcessed = null;

                SetMessage("All files saved successfully.");
            }
            catch (Exception ex)
            {
                SetMessage("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Set message to be displayed.
        /// </summary>
        /// <param name="message"></param>
        private void SetMessage(string message)
        {
            MessageActive = true;
            Message = message;
            MessageActive = false;
        }

        /// <summary>
        /// Apply original effect.
        /// </summary>
        private void ApplyOriginal()
        {
            SelectedImage = new BitmapImage(new Uri(selectedImagePath));

            // Set current effect
            currentEffect = Effect.nofilter;
        }

        /// <summary>
        /// Apply grayscale effect.
        /// </summary>
        private async void ApplyGrayscale()
        {
            // Get image asynchronously
            BitmapImage image = await Task.Run(() =>
            {
                IsBusy = true;

                // Get image
                return PaintImageAsync(imageHandler.CurrentGrayscaleHandler.SetGrayscale, selectedImagePath).Result;
            });

            // If image is not null, set it
            if (image != null)
                SelectedImage = image;

            IsBusy = false;

            // Set current effect
            currentEffect = Effect.grayscale;
        }

        /// <summary>
        /// Apply grayscale effect.
        /// </summary>
        private async void ApplySepia()
        {
            // Get image asynchronously
            BitmapImage image = await Task.Run(() =>
            {
                IsBusy = true;

                // Get image
                return PaintImageAsync(imageHandler.CurrentSepiaToneHandler.SetSepiaTone, selectedImagePath).Result;
            });

            // If image is not null, set it
            if (image != null)
                SelectedImage = image;

            IsBusy = false;

            // Set current effect
            currentEffect = Effect.sepia;
        }

        /// <summary>
        /// Apply color filter.
        /// </summary>
        /// <param name="color"></param>
        private async void ApplyColorFilter(ImageFunctions.ColorFilterTypes color)
        {
            // Get image asynchronously
            BitmapImage image = await Task.Run(() =>
            {
                IsBusy = true;

                // Get image
                return PaintImageColorAsync(new Action<ImageFunctions.ColorFilterTypes>(imageHandler.CurrentFilterHandler.SetColorFilter), color, selectedImagePath).Result;
            });

            // If image is not null, set it
            if (image != null)
                SelectedImage = image;

            IsBusy = false;

            // Set current effect
            switch (color)
            {
                case ImageFunctions.ColorFilterTypes.Red:
                    currentEffect = Effect.redfilter;
                    break;
                case ImageFunctions.ColorFilterTypes.Green:
                    currentEffect = Effect.greenfilter;
                    break;
                case ImageFunctions.ColorFilterTypes.Blue:
                    currentEffect = Effect.bluefilter;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Rotate image.
        /// </summary>
        /// <param name="loadMode"></param>
        /// <param name="angle"></param>
        private async Task RotateAsync(float angle)
        {
            IsBusy = true;

            try
            {
                SelectedImage = await Task.Run(() =>
                {
                    lock (SelectedImage)
                    {
                        imageHandler.CurrentRotationHandler.Rotate(angle);
                        MemoryStream stream = new MemoryStream();
                        imageHandler.CurrentBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        stream.Position = 0;
                        byte[] data = new byte[stream.Length];
                        stream.Read(data, 0, Convert.ToInt32(stream.Length));
                        BitmapImage bmapImage = new BitmapImage();
                        bmapImage.BeginInit();
                        bmapImage.StreamSource = stream;
                        bmapImage.EndInit();
                        bmapImage.Freeze();
                        return bmapImage;
                    }
                });
            }
            catch (Exception ex)
            {
                SetMessage(ex.Message);
            }

            IsBusy = false;
        }

        /// <summary>
        /// Flip image.
        /// </summary>
        /// <param name="rotateFlipType"></param>
        private async Task FlipAsync (RotateFlipType rotateFlipType)
        {
            IsBusy = true;

            try
            {
                SelectedImage = await Task.Run(() =>
                {
                    imageHandler.CurrentRotationHandler.Flip(rotateFlipType);
                    MemoryStream stream = new MemoryStream();
                    imageHandler.CurrentBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    stream.Position = 0;
                    byte[] data = new byte[stream.Length];
                    stream.Read(data, 0, Convert.ToInt32(stream.Length));
                    BitmapImage bmapImage = new BitmapImage();
                    bmapImage.BeginInit();
                    bmapImage.StreamSource = stream;
                    bmapImage.EndInit();
                    bmapImage.Freeze();
                    return bmapImage;
                });
            }
            catch (Exception ex)
            {
                SetMessage(ex.Message);
            }

            IsBusy = false;
        }

        /// <summary>
        /// Paint image asynchronously according to current settings.
        /// </summary>
        private async Task<BitmapImage> PaintImageAsync(Action processMethod, string path)
        {
            try
            {
                return await Task.Run(() =>
                {
                    lock (SelectedImage)
                    {
                        imageHandler.CurrentFileHandler.Load(path);
                        processMethod();
                        MemoryStream stream = new MemoryStream();
                        imageHandler.CurrentBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        stream.Position = 0;
                        byte[] data = new byte[stream.Length];
                        stream.Read(data, 0, Convert.ToInt32(stream.Length));
                        BitmapImage bmapImage = new BitmapImage();
                        bmapImage.BeginInit();
                        bmapImage.StreamSource = stream;
                        bmapImage.EndInit();
                        bmapImage.Freeze();
                        return bmapImage;
                    }
                });
            }
            catch (Exception ex)
            {
                SetMessage(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Paint image asynchronously with specific color filter.
        /// </summary>
        /// <param name="processMethod"></param>
        /// <param name="color"></param>
        private async Task<BitmapImage> PaintImageColorAsync(Action<ImageFunctions.ColorFilterTypes> processMethod, ImageFunctions.ColorFilterTypes color, string path)
        {
            try
            {
                return await Task.Run(() =>
                {
                    lock (SelectedImage)
                    {
                        imageHandler.CurrentFileHandler.Load(path);
                        processMethod(color);
                        MemoryStream stream = new MemoryStream();
                        imageHandler.CurrentBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        stream.Position = 0;
                        byte[] data = new byte[stream.Length];
                        stream.Read(data, 0, Convert.ToInt32(stream.Length));
                        BitmapImage bmapImage = new BitmapImage();
                        bmapImage.BeginInit();
                        bmapImage.StreamSource = stream;
                        bmapImage.EndInit();
                        bmapImage.Freeze();
                        return bmapImage;
                    }
                });
            }
            catch (Exception ex)
            {
                SetMessage(ex.Message);
            }

            return null;
        }

        #endregion
    }

    /// <summary>
    /// Current effect applied.
    /// </summary>
    public enum Effect
    {
        nofilter,
        grayscale,
        sepia,
        redfilter,
        greenfilter,
        bluefilter
    }
}
