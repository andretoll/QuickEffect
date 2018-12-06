using QuickEffect.Helpers;
using System.Collections.ObjectModel;

namespace QuickEffect.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for Appearance settings.
    /// </summary>
    public class AppearanceSettingsViewModel : BaseViewModel
    {
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
                // Save changes
                SettingsHelper.WriteToSettings(SettingsHelper.Settings.MetroTheme.ToString(), value);
                SettingsHelper.SetAppTheme();
            }
        }

        public string SelectedAccent
        {
            get { return SettingsHelper.GetCurrentAccent(); }
            set
            {
                // Save changes
                SettingsHelper.WriteToSettings(SettingsHelper.Settings.MetroAccent.ToString(), value);
                SettingsHelper.SetAppTheme();
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
