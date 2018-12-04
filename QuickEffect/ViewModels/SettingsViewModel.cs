using NotebookWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickEffect.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ObservableCollection<BaseViewModel> settings;

        public ObservableCollection<BaseViewModel> Settings
        {
            get { return this.settings; }
        }

        public SettingsViewModel()
        {
            this.settings = new ObservableCollection<BaseViewModel>
            {
                new AppearanceSettingsViewModel(),
                new ApplicationSettingsViewModel()
            };
        }
    }
}
