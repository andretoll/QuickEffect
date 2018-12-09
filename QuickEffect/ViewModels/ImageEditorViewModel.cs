using QuickEffect.Commands;
using QuickEffect.Helpers;
using System.Collections.ObjectModel;
using System.IO;
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

        // Collections
        private ObservableCollection<string> fileNames;

        // Message service
        private string message;
        private bool messageActive;

        // Selected image
        private BitmapImage selectedImage;

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
            FileNames = fileNames;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set message to be displayed.
        /// </summary>
        /// <param name="message"></param>
        public void SetMessage(string message)
        {
            MessageActive = false;
            Message = message;
            MessageActive = true;
        }

        #endregion
    }
}
