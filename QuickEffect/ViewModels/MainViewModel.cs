using QuickEffect.Commands;
using QuickEffect.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for MainWindow. Acts as a shell.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region Eventhandlers

        // New window event
        public EventHandler OpenEditorWindow;

        #endregion

        #region Private Members

        // Current ViewModel + View
        private BaseViewModel viewModel;

        #endregion

        #region Properties

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

        private ICommand openEditorCommand;
        public ICommand OpenEditorCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (openEditorCommand == null)
                    openEditorCommand = new RelayCommand(p => { OpenEditorView(p); });
                return openEditorCommand;
            }
            set
            {
                openEditorCommand = value;
            }
        }

        private ICommand openDragDropCommand;
        public ICommand OpenDragDropCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (openDragDropCommand == null)
                    openDragDropCommand = new RelayCommand(p => { OpenDragDropView(); });
                return openDragDropCommand;
            }
            set
            {
                openDragDropCommand = value;
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
        /// Switches to editor ViewModel if any files exists.
        /// </summary>
        /// <param name="files"></param>
        private void OpenEditorView(object files)
        {
            if (files != null)
            {
                // If new window is enabled
                if ((bool)SettingsHelper.ReadFromSettings(SettingsHelper.Settings.OpenEditorInNewWindow.ToString()))
                {
                    // Alert view to open new window
                    OpenEditorWindow.Invoke(files, null);
                }
                // Else open editor in same window
                else
                    this.ViewModel = new ImageEditorViewModel((ObservableCollection<string>)files);
            }
        }

        /// <summary>
        /// Switches to Drag & Drop ViewModel
        /// </summary>
        private void OpenDragDropView()
        {
            this.ViewModel = new DragAndDropViewModel();
        }

        #endregion
    }
}
