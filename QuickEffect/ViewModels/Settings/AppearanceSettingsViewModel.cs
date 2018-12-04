using NotebookWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickEffect.ViewModels
{
    public class AppearanceSettingsViewModel : BaseViewModel
    {
        public string Name { get { return "Appearance"; } }
        public string Icon { get { return "FormatColorFill"; } }
    }
}
