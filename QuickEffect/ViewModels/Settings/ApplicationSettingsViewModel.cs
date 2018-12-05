namespace QuickEffect.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for Application settings.
    /// </summary>
    public class ApplicationSettingsViewModel : BaseViewModel
    {
        #region Private Members

        private bool openProcessingInNewWindow;

        #endregion

        #region Public members

        // Template properties for view
        public string Name { get { return "Application"; } }
        public string Icon { get { return "Application"; } }

        #endregion

        #region Properties

        public bool OpenProcessingInNewWindow
        {
            get { return openProcessingInNewWindow; }
            set
            {
                openProcessingInNewWindow = value;
                NotifyPropertyChanged();

                // Save changes
                Properties.Settings.Default.OpenProcessingInNewWindow = value;
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        #region Constructor

        public ApplicationSettingsViewModel()
        {
            // Get settings
            openProcessingInNewWindow = Properties.Settings.Default.OpenProcessingInNewWindow;
        }

        #endregion
    }
}
