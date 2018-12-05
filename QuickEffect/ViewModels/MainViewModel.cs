using QuickEffect.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for MainWindow. Acts as a shell.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region Private Members

        private ObservableCollection<string> files;

        // Current ViewModel + View
        private BaseViewModel viewModel;

        #endregion

        #region Properties

        public ObservableCollection<string> Files
        {
            get { return files; }
            set
            {
                files = value;
                NotifyPropertyChanged();
            }
        }

        public BaseViewModel ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private ICommand startProcessingCommand;
        public ICommand StartProcessingCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (startProcessingCommand == null)
                    startProcessingCommand = new RelayCommand(p => { OpenProcessingView(p); });
                return startProcessingCommand;
            }
            set
            {
                startProcessingCommand = value;
            }
        }

        private ICommand stopProcessingCommand;
        public ICommand StopProcessingCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (stopProcessingCommand == null)
                    stopProcessingCommand = new RelayCommand(p => { OpenDragDropView(); });
                return stopProcessingCommand;
            }
            set
            {
                stopProcessingCommand = value;
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            // Start with Drag & Drop ViewModel and View
            ViewModel = new DragAndDropViewModel();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Switches to processing ViewModel if any files exists.
        /// </summary>
        /// <param name="files"></param>
        private void OpenProcessingView(object files)
        {
            if (files != null)
            {
                ViewModel = new ImageProcessorViewModel();
            }
        }

        /// <summary>
        /// Switches to Drag & Drop ViewModel
        /// </summary>
        private void OpenDragDropView()
        {
            ViewModel = new DragAndDropViewModel();
        }

        #endregion
    }
}
