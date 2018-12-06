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
        
        // Collections
        private ObservableCollection<string> orientationList;

        #endregion

        #region Public members

        // Template properties for view
        public string Name { get { return "Application"; } }
        public string Icon { get { return "Application"; } }

        #endregion

        #region Properties

        public ObservableCollection<string> OrientationList
        {
            get { return orientationList; }
            private set
            {
                orientationList = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public ApplicationSettingsViewModel()
        {
            // Populate lists
            orientationList = new ObservableCollection<string>();
            var enumList = Enum.GetNames(typeof(Orientation));

            foreach (var item in enumList)
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
