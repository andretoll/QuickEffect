using QuickEffect.ViewModel;
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

namespace QuickEffect.View.UserControls
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
        }

        #endregion

        #region Events

        private void DropArea_Drop(object sender, DragEventArgs e)
        {
            string fileName = this._viewModel.GetFileNamesFromDrop(e)[0];

            ProcessImageWindow processImageWindow = new ProcessImageWindow(fileName);
            processImageWindow.Show();
        }

        private void DropArea_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void DropArea_DragLeave(object sender, DragEventArgs e)
        {
        }

        #endregion
    }
}
