using QuickEffect.Commands;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for DropArea.
    /// </summary>
    public class DragAndDropViewModel : BaseViewModel
    {
        #region Private members

        // Collections
        private ObservableCollection<string> fileNames;

        // Message service
        private string message;
        private bool messageActive;

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

        #endregion

        #region Commands

        private ICommand browseFilesCommand;
        public ICommand BrowseFilesCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (browseFilesCommand == null)
                    browseFilesCommand = new RelayCommand(p => { BrowseFiles(); });
                return browseFilesCommand;
            }
            set
            {
                browseFilesCommand = value;
            }
        }

        private ICommand cancelFileSelectionCommand;
        public ICommand CancelFileSelectionCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (cancelFileSelectionCommand == null)
                    cancelFileSelectionCommand = new RelayCommand(p => { FileNames = null; SetMessage("Operation cancelled."); });
                return cancelFileSelectionCommand;
            }
            set
            {
                cancelFileSelectionCommand = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Browse files and add selected files to collection.
        /// </summary>
        private void BrowseFiles()
        {
            // Open file dialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            var result = openFileDialog.ShowDialog();

            // If one or more files were selected
            if (result == true)
            {
                if (FileNames == null)
                    FileNames = new ObservableCollection<string>();

                // Loop through each file
                foreach (var fileName in openFileDialog.FileNames)
                {
                    // Add filename to collection
                    AddFile(fileName);
                }
            }
        }        

        /// <summary>
        /// Add file to collection.
        /// </summary>
        /// <param name="fileName"></param>
        private void AddFile(string fileName)
        {
            // If invalid file type, display message
            if (Path.GetExtension(fileName) != ".png" &&
                Path.GetExtension(fileName) != ".jpg" &&
                Path.GetExtension(fileName) != ".jpeg")
            {
                SetMessage("Invalid file. Only images are accepted.");

                return;
            }

            if (FileNames == null)
                FileNames = new ObservableCollection<string>();

            // If collection doesn't contain file yet, add it
            if (!FileNames.Contains(fileName))
            {
                FileNames.Add(fileName);

                SetMessage("File(s) added.");
            }
            // Else, display message
            else
            {
                SetMessage("File(s) already added.");
            }

        }

        /// <summary>
        /// Set message to be displayed.
        /// </summary>
        /// <param name="message"></param>
        private void SetMessage(string message)
        {
            MessageActive = false;
            Message = message;
            MessageActive = true;
        }

        /// <summary>
        /// Retrieve filenames from dropped files.
        /// </summary>
        /// <param name="e"></param>
        public void GetDroppedFiles(DragEventArgs e)
        {           
            // Get file names from event
            string[] fileNames = (string[])((DragEventArgs)e).Data.GetData(DataFormats.FileDrop);

            // Populate collection
            foreach (var fileName in fileNames)
            {
                // Add filename to collection
                AddFile(fileName);
            }
        }

        #endregion
    }
}
