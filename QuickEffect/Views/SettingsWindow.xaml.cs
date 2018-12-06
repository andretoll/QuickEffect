using QuickEffect.ViewModels;
using System;
using System.Windows;

namespace QuickEffect.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        #region Constructor

        public SettingsWindow()
        {
            InitializeComponent();

            // Set datacontext
            this.DataContext = new SettingsViewModel();
        }

        #endregion
    }
}
