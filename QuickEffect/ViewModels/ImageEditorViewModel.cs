using Microsoft.Win32;
using QuickEffect.Commands;
using QuickEffect.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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

        // Collections
        private ObservableCollection<string> fileNames;

        // Message service
        private string message;
        private bool messageActive;

        // Selected image
        private BitmapImage selectedImage;
        private string selectedImagePath;

        // Effects
        private CurrentImageHandler imageHandler;
        private bool original = true;
        private bool grayscale;
        private bool sepia;
        private bool redFilter;
        private bool greenFilter;
        private bool blueFilter;
        private int brightness;
        private int contrast;

        #endregion

        #region Properties

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
                // If image is null, check if it exists. If not, remove it and display message
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
                    selectedImagePath = selectedImage.UriSource.LocalPath;
                    string path = selectedImage.UriSource.LocalPath;
                    imageHandler.CurrentFileHandler.Load(path);
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

                if (original)
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

        public int Brightness
        {
            get { return brightness; }
            set
            {
                brightness = value;

                AdjustBrightness();

                NotifyPropertyChanged();
            }
        }

        public int Contrast
        {
            get { return contrast; }
            set
            {
                contrast = value;

                AdjustContrast();

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
        private void ApplyGrayscale()
        {
            ApplyOriginal();
            imageHandler.CurrentGrayscaleHandler.SetGrayscale();
            PaintImage();
        }

        /// <summary>
        /// Apply grayscale effect.
        /// </summary>
        private void ApplySepia()
        {
            ApplyOriginal();
            imageHandler.CurrentSepiaToneHandler.SetSepiaTone();
            PaintImage();
        }

        /// <summary>
        /// Apply color filter.
        /// </summary>
        /// <param name="color"></param>
        private void ApplyColorFilter(ImageFunctions.ColorFilterTypes color)
        {
            ApplyOriginal();
            imageHandler.CurrentFilterHandler.SetColorFilter(color);
            PaintImage();
        }

        /// <summary>
        /// Adjust brightness.
        /// </summary>
        private void AdjustBrightness()
        {
            ApplyOriginal();
            imageHandler.CurrentBrightnessHandler.SetBrightness(Brightness);
            PaintImage();
        }

        /// <summary>
        /// Adjust contrast.
        /// </summary>
        private void AdjustContrast()
        {
            ApplyOriginal();
            imageHandler.CurrentContrastHandler.SetContrast(Contrast);
            PaintImage();
        }

        /// <summary>
        /// Paint image according to current settings.
        /// </summary>
        private void PaintImage()
        {
            MemoryStream stream = new MemoryStream();
            imageHandler.CurrentBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Position = 0;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, Convert.ToInt32(stream.Length));
            BitmapImage bmapImage = new BitmapImage();
            bmapImage.BeginInit();
            bmapImage.StreamSource = stream;
            bmapImage.EndInit();

            SelectedImage = bmapImage;            
        }

        #endregion
    }
}
