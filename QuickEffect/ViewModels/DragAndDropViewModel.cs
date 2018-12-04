using NotebookWPF.Commands;
using NotebookWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuickEffect.ViewModel
{
    /// <summary>
    /// ViewModel for DropArea.
    /// </summary>
    public class DragAndDropViewModel : BaseViewModel
    {
        #region Private members

        private ObservableCollection<string> fileNames;

        #endregion

        #region Public members

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
                FileNames = new ObservableCollection<string>();

                // Loop through each file
                foreach (var file in openFileDialog.FileNames)
                {
                    // Validate extension
                    if (Path.GetExtension(file) == ".png" && 
                        Path.GetExtension(file) == ".jpg" &&
                        Path.GetExtension(file) == ".jpeg")
                    {
                        // Add path to list
                        FileNames.Add(file);
                    }                    
                }
            }
        }

        #endregion
    }
}
