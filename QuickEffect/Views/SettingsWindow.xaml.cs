using QuickEffect.ViewModels;

namespace QuickEffect.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();

            // Set datacontext
            this.DataContext = new SettingsViewModel();
        }

        private void CloseSettingsWindowButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
