using QuickEffect.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickEffect.Views.UserControls
{
    /// <summary>
    /// Interaction logic for DragAndDropUserControl.xaml
    /// </summary>
    public partial class DragAndDropUserControl : UserControl
    {
        #region Private members

        private DragAndDropViewModel _viewModel;

        #endregion

        #region Constructor

        public DragAndDropUserControl()
        {
            InitializeComponent();

            // Set datacontext
            _viewModel = new DragAndDropViewModel();
            this.DataContext = _viewModel;

            _viewModel.FilesReady += FilenamesReady;
        }

        #endregion

        #region Events

        private void FilenamesReady(object sender, EventArgs e)
        {
            List<string> fileNamesList = (List<string>)sender;

            OpenProcessingWindow(fileNamesList);
        }

        private void DropArea_Drop(object sender, DragEventArgs e)
        {
            List<string> fileNamesList = new List<string>();

            // Get file names
            string[] fileNames = (string[])((DragEventArgs)e).Data.GetData(DataFormats.FileDrop);

            foreach (var fileName in fileNames)
            {
                fileNamesList.Add(fileName);
            }

            // Open processing window
            OpenProcessingWindow(fileNamesList);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Open processing window and provide file names.
        /// </summary>
        /// <param name="fileNames"></param>
        private void OpenProcessingWindow(List<string> fileNames)
        {
            if (fileNames != null)
            {
                // Open process window
                ProcessImageWindow processImageWindow = new ProcessImageWindow(fileNames);
                processImageWindow.ShowDialog();
            }
        }

        #endregion
    }
}
