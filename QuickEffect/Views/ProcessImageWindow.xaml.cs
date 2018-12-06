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
            ImageProcessorViewModel viewModel = new ImageProcessorViewModel(fileNames);
            this.DataContext = viewModel;
        }

        #endregion

        private void CloseProcessingWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
