﻿using QuickEffect.Commands;
using QuickEffect.Helpers;
using System.Collections.ObjectModel;
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
    }
}