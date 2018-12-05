using MahApps.Metro.Controls;
using QuickEffect.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace QuickEffect.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Private Members

        private MainViewModel _viewModel;

        #endregion

        #region Constructor

        public MainWindow()
        {
            // Load all settings
            Helpers.SettingsHelper.LoadSettings();

            InitializeComponent();

            // Set datacontext
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;

            // Subscribe to events
            _viewModel.OpenProcessingWindow += OpenWindow;
        }

        #endregion

        #region Events

        private void OpenWindow(object sender, EventArgs e)
        {
            if (sender != null)
            {
                ProcessImageWindow processImageWindow = new ProcessImageWindow((ObservableCollection<string>)sender);
                processImageWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                processImageWindow.ShowDialog();

            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settingsWindow.ShowDialog();
        }

        #endregion
    }
}
