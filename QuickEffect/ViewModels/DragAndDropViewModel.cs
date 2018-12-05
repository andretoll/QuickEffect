using QuickEffect.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for DropArea.
    /// </summary>
    public class DragAndDropViewModel : BaseViewModel
    {
        // Event that fires when files have been opened
        public event EventHandler<EventArgs> FilesReady;

        #region Private members

        private List<string> fileNames;

        #endregion

        #region Public members

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Commands

        private ICommand browseFilesCommand;
        public ICommand BrowseFilesCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (browseFilesCommand == null)
                    browseFilesCommand = new RelayCommand(p => { BrowseFiles(); }, p => true);
                return browseFilesCommand;
            }
            set
            {
                browseFilesCommand = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Browse files and add file names.
        /// </summary>
        public void BrowseFiles()
        {
            // Open file dialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            var result = openFileDialog.ShowDialog();

            // If one or more files were selected
            if (result == true)
            {
                fileNames = new List<string>();

                // Loop through each file
                foreach (var file in openFileDialog.FileNames)
                {
                    // Validate extension
                    if (Path.GetExtension(file) == ".png" || 
                        Path.GetExtension(file) == ".jpg" ||
                        Path.GetExtension(file) == ".jpeg")
                    {
                        // Add path to list
                        fileNames.Add(file);
                    }                    
                }

                // Invoke event
                FilesReady?.Invoke(fileNames, null);
            }
        }

        #endregion
    }
}
