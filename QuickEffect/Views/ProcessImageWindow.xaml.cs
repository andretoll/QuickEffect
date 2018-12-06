using QuickEffect.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace QuickEffect.Views
{
    /// <summary>
    /// Interaction logic for ProcessImageWindow.xaml
    /// </summary>
    public partial class ProcessImageWindow
    {
        #region Constructor

        public ProcessImageWindow(ObservableCollection<string> fileNames)
        {
            InitializeComponent();

            // Set datacontext and insert files
            ImageEditorViewModel viewModel = new ImageEditorViewModel(fileNames);
            this.DataContext = viewModel;
        }

        #endregion
    }
}
