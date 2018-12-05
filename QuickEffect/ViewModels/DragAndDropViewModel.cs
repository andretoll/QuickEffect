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

        private ObservableCollection<string> fileNames;

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

        private ICommand clearFilesCommand;
        public ICommand ClearFilesCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (clearFilesCommand == null)
                    clearFilesCommand = new RelayCommand(p => { FileNames = null; });
                return clearFilesCommand;
            }
            set
            {
                clearFilesCommand = value;
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
                foreach (var file in openFileDialog.FileNames)
                {
                    // Validate extension
                    if (Path.GetExtension(file) == ".png" || 
                        Path.GetExtension(file) == ".jpg" ||
                        Path.GetExtension(file) == ".jpeg")
                    {
                        // Add path to list
                        AddFile(file);
                    }                    
                }
            }
        }        

        /// <summary>
        /// Add file to collection.
        /// </summary>
        /// <param name="fileName"></param>
        private void AddFile(string fileName)
        {
            if (!FileNames.Contains(fileName))
            {
                FileNames.Add(fileName);
            }
            else
            {
                // TODO: Show message
            }
        }

        /// <summary>
        /// Retrieve filenames from dropped files.
        /// </summary>
        /// <param name="e"></param>
        public void GetDroppedFiles(DragEventArgs e)
        {
            if (FileNames == null)
                FileNames = new ObservableCollection<string>();

            // Get file names from event
            string[] fileNames = (string[])((DragEventArgs)e).Data.GetData(DataFormats.FileDrop);

            // Populate collection
            foreach (var fileName in fileNames)
            {
                AddFile(fileName);
            }
        }

        #endregion
    }
}
