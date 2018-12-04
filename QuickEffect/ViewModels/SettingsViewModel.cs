using NotebookWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for SettingsWindow
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
