using MahApps.Metro.Controls;
using QuickEffect.Helpers;
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
            // Validate settings
            SettingsHelper.ValidateSettings();
            SettingsHelper.SetAppTheme();

            InitializeComponent();

            // Set datacontext
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;

            // Subscribe to events
            _viewModel.OpenEditorWindow += OpenEditorWindow;
        }

        #endregion

        #region Events

        private void OpenEditorWindow(object sender, EventArgs e)
        {
            if (sender != null)
            {
                EditorWindow editorWindow = new EditorWindow((ObservableCollection<string>)sender);
                editorWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                editorWindow.ShowDialog();

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
