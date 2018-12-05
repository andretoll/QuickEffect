﻿using QuickEffect.Commands;
using System;
using System.Windows.Input;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for MainWindow. Acts as a shell.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region Private Members

        // Current ViewModel + View
        private BaseViewModel viewModel;

        // New window event
        public EventHandler OpenProcessingWindow;

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
                // If new window is enabled
                if (Properties.Settings.Default.OpenProcessingInNewWindow)
                {
                    OpenProcessingWindow.Invoke(files, null);
                }
                else
                    this.ViewModel = new ImageProcessorViewModel();
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