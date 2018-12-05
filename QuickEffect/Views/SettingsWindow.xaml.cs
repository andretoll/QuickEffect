using QuickEffect.ViewModels;
using System.Windows;

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

        private void CloseSettingsWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
