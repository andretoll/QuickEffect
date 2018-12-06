using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace QuickEffect.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for Application settings.
    /// </summary>
    public class ApplicationSettingsViewModel : BaseViewModel
    {
        #region Private Members

        private bool openProcessingInNewWindow;
        private ObservableCollection<string> orientationList;
        private string selectedOrientation;

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

        public ObservableCollection<string> OrientationList
        {
            get { return orientationList; }
            private set
            {
                orientationList = value;
                NotifyPropertyChanged();
            }
        }

        public string SelectedOrientation
        {
            get { return selectedOrientation; }
            set
            {
                selectedOrientation = value;
                NotifyPropertyChanged();

                // Save changes
                Properties.Settings.Default.ImageListOrientation = value;
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        #region Constructor

        public ApplicationSettingsViewModel()
        {
            // Get settings
            openProcessingInNewWindow = Properties.Settings.Default.OpenProcessingInNewWindow;
            selectedOrientation = Properties.Settings.Default.ImageListOrientation;

            // Populate lists
            orientationList = new ObservableCollection<string>();
            var test = Enum.GetNames(typeof(Orientation));

            foreach (var item in test)
            {
                orientationList.Add(item);
            }
        }

        #endregion

        public enum Orientation
        {
            Horizontal,
            Vertical
        }
    }
}
