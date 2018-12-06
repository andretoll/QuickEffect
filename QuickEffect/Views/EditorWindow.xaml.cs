using QuickEffect.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace QuickEffect.Views
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow
    {
        #region Constructor

        public EditorWindow(ObservableCollection<string> fileNames)
        {
            InitializeComponent();

            // Set datacontext and insert files
            ImageEditorViewModel viewModel = new ImageEditorViewModel(fileNames);
            this.DataContext = viewModel;
        }

        #endregion
    }
}
