using Microsoft.Win32;
using QuickEffect.Commands;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
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

        // Collections
        private ObservableCollection<string> fileNames;

        // Message service
        private string message;
        private bool messageActive;

        // Selected image
        private BitmapImage selectedImage;
        private string selectedImagePath;
        private bool resettingUI;

        // Effects
        private CurrentImageHandler imageHandler;
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

        public ObservableCollection<string> FileNames
        {
            get { return fileNames; }
            set
            {
                fileNames = value;
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
                    for (int i = 0; i < FileNames.Count; i++)
                    {
                        if (!File.Exists(FileNames[i]))
                        {
                            FileNames.Remove(FileNames[i]);

                            SetMessage("One or more files could not be found.");
                        }
                    }
                }                
                    
                selectedImage = value;

                // If initial loading, load file into handler
                if (selectedImage.UriSource != null)
                {
                    string path = selectedImage.UriSource.LocalPath;
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
                            flipY = false;
                            flipX = false;

                            resettingUI = false;
                        }
                    }

                    // Save path to original file
                    selectedImagePath = selectedImage.UriSource.LocalPath;                                    
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

        private ICommand closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (closeWindowCommand == null)
                    closeWindowCommand = new RelayCommand(p =>
                    {
                        // Close window
                        ((Window)p).Close();
                    });

                return closeWindowCommand;
            }
            set
            {
                closeWindowCommand = value;
            }
        }

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

        #endregion

        #region Constructor

        public ImageEditorViewModel(ObservableCollection<string> fileNames)
        {
            // Initialize save file dialog
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "C:\\";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Filter = "jpg Files (*.jpg)|*.jpg|gif Files (*.gif)|*.gif|png Files (*.png)|*.png |bmp Files (*.bmp)|*.bmp";

            FileNames = fileNames;

            imageHandler = new CurrentImageHandler();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set message to be displayed.
        /// </summary>
        /// <param name="message"></param>
        public void SetMessage(string message)
        {
            MessageActive = true;
            Message = message;
            MessageActive = false;
        }

        /// <summary>
        /// Save file to disk.
        /// </summary>
        private void SaveFile()
        {
            if (saveFileDialog.ShowDialog().Value)
            {
                imageHandler.CurrentFileHandler.Save(saveFileDialog.FileName);
            }
        }

        /// <summary>
        /// Apply original effect.
        /// </summary>
        private void ApplyOriginal()
        {
            SelectedImage = new BitmapImage(new Uri(selectedImagePath));
        }

        /// <summary>
        /// Apply grayscale effect.
        /// </summary>
        private async void ApplyGrayscale()
        {
            await PaintImageAsync(new Action(imageHandler.CurrentGrayscaleHandler.SetGrayscale));
        }

        /// <summary>
        /// Apply grayscale effect.
        /// </summary>
        private async void ApplySepia()
        {
            await PaintImageAsync(new Action(imageHandler.CurrentSepiaToneHandler.SetSepiaTone));
        }

        /// <summary>
        /// Apply color filter.
        /// </summary>
        /// <param name="color"></param>
        private async void ApplyColorFilter(ImageFunctions.ColorFilterTypes color)
        {
            await PaintImageColorAsync(new Action<ImageFunctions.ColorFilterTypes>(imageHandler.CurrentFilterHandler.SetColorFilter), color);
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
        private async Task PaintImageAsync(Action processMethod)
        {
            IsBusy = true;

            try
            {
                SelectedImage = await Task.Run(() =>
                {
                    lock (SelectedImage)
                    {
                        imageHandler.CurrentFileHandler.Load(selectedImagePath);
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

            IsBusy = false;
        }

        /// <summary>
        /// Paint image asynchronously with specific color filter.
        /// </summary>
        /// <param name="processMethod"></param>
        /// <param name="color"></param>
        private async Task PaintImageColorAsync(Action<ImageFunctions.ColorFilterTypes> processMethod, ImageFunctions.ColorFilterTypes color)
        {
            IsBusy = true;

            try
            {
                SelectedImage = await Task.Run(() =>
                {
                    lock (SelectedImage)
                    {
                        imageHandler.CurrentFileHandler.Load(selectedImagePath);
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

            IsBusy = false;
        }

        #endregion
    }
}
