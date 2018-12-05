using QuickEffect.Helpers;
using QuickEffect.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickEffect.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for Appearance settings.
    /// </summary>
    public class AppearanceSettingsViewModel : BaseViewModel
    {
        #region Private members

        private string selectedAccent;
        private string selectedTheme;

        #endregion

        #region Public members

        // Template properties for view
        public string Name { get { return "Appearance"; } }
        public string Icon { get { return "FormatColorFill"; } }

        // Collection of available themes
        public ObservableCollection<string> Themes { get; set; }

        // Collection of available accents
        public ObservableCollection<string> Accents { get; set; }

        #endregion

        #region Properties

        public string SelectedTheme
        {
            get { return SettingsHelper.GetCurrentTheme(); }
            set
            {
                selectedTheme = value;
                NotifyPropertyChanged();

                // Save changes
                Properties.Settings.Default.MetroTheme = value;
                Properties.Settings.Default.Save();
                SettingsHelper.LoadSettings();
            }
        }

        public string SelectedAccent
        {
            get { return SettingsHelper.GetCurrentAccent(); }
            set
            {
                selectedAccent = value;
                NotifyPropertyChanged();

                // Save changes
                Properties.Settings.Default.MetroAccent = value;
                Properties.Settings.Default.Save();
                SettingsHelper.LoadSettings();
            }
        }

        #endregion

        #region Constructor

        public AppearanceSettingsViewModel()
        {
            // Load themes and accents
            Themes = new ObservableCollection<string>(SettingsHelper.GetAllThemes());
            Accents = new ObservableCollection<string>(SettingsHelper.GetAllAccents());
        }

        #endregion
    }
}
