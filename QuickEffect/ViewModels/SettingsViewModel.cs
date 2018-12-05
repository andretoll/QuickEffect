using QuickEffect.ViewModels.Settings;
using System.Collections.ObjectModel;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for SettingsWindow. Acts as a shell.
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        #region Properties

        // ViewModels for settings
        public ObservableCollection<BaseViewModel> Settings { get; }

        #endregion

        #region Constructor

        public SettingsViewModel()
        {
            // Add settings ViewModels
            this.Settings = new ObservableCollection<BaseViewModel>
            {
                new AppearanceSettingsViewModel(),
                new ApplicationSettingsViewModel()
            };
        }

        #endregion
    }
}
