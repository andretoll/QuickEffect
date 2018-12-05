using QuickEffect.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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
        }

        #endregion

        #region Events

        private void DropArea_Drop(object sender, DragEventArgs e)
        {
            _viewModel.GetDroppedFiles(e);
        }

        #endregion
    }
}
