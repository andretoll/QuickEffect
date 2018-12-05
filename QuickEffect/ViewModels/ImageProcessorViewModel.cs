using System.Collections.ObjectModel;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for image processing view.
    /// </summary>
    public class ImageProcessorViewModel : BaseViewModel
    {
        #region Private Members

        private ObservableCollection<string> fileNames;

        private string message;
        private bool messageActive;

        #endregion

        #region Public Members

        public ObservableCollection<string> FileNames
        {
            get { return fileNames; }
            set
            {
                fileNames = value;
                NotifyPropertyChanged();
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged();
            }
        }

        public bool MessageActive
        {
            get { return messageActive; }
            set
            {
                messageActive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public ImageProcessorViewModel(ObservableCollection<string> fileNames)
        {
            FileNames = fileNames;
        }

        #endregion
    }
}
