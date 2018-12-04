using NotebookWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for Application settings.
    /// </summary>
    public class ApplicationSettingsViewModel : BaseViewModel
    {
        #region Public members

        // Template properties for view
        public string Name { get { return "Application"; } }
        public string Icon { get { return "Application"; } }

        #endregion
    }
}
