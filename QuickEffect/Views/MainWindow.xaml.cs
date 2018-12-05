using MahApps.Metro.Controls;
using QuickEffect.ViewModels;
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

namespace QuickEffect.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructor

        public MainWindow()
        {
            // Load all settings
            Helpers.SettingsHelper.LoadSettings();

            InitializeComponent();
        }

        #endregion

        #region Events

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settingsWindow.ShowDialog();
        }

        #endregion
    }
}
