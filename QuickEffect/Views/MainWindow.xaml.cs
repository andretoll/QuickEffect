using MahApps.Metro.Controls;
using QuickEffect.View;
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

namespace QuickEffect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Private members

        private DragAndDropViewModel _viewModel;

        #endregion

        #region Constructor

        public MainWindow()
        {
            // Load all settings
            Helpers.SettingsHelper.LoadSettings();

            InitializeComponent();

            // Set datacontext
            _viewModel = new DragAndDropViewModel();
            this.DataContext = _viewModel;
        }

        #endregion

        #region Events

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        #endregion
    }
}
