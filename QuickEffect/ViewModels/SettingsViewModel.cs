using QuickEffect.Commands;
using QuickEffect.Helpers;
using QuickEffect.ViewModels.Settings;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace QuickEffect.ViewModels
{
    /// <summary>
    /// ViewModel for SettingsWindow. Acts as a shell.
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        #region Properties

        // ViewModels for settings
        public ObservableCollection<BaseViewModel> Settings { get; }

        #endregion

        #region Commands

        private ICommand saveSettingsCommand;
        public ICommand SaveSettingsCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (saveSettingsCommand == null)
                    saveSettingsCommand = new RelayCommand(p => 
                    {
                        // Save changes
                        SettingsHelper.SaveSettings();

                        // Close window
                        ((Window)p).Close();
                    });

                return saveSettingsCommand;
            }
            set
            {
                saveSettingsCommand = value;
            }
        }

        private ICommand closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                // Create new RelayCommand and pass method to be executed and a boolean value whether or not to execute
                if (closeWindowCommand == null)
                    closeWindowCommand = new RelayCommand(p => 
                    {
                        // Discard any unsaved settings
                        SettingsHelper.DiscardSettings();

                        // If a window was passed in, close it
                        if (p != null)
                            ((Window)p).Close();
                    });

                return closeWindowCommand;
            }
            set
            {
                closeWindowCommand = value;
            }
        }

        #endregion

        #region Constructor

        public SettingsViewModel()
        {
            // Add settings ViewModels
            this.Settings = new ObservableCollection<BaseViewModel>
            {
                new AppearanceSettingsViewModel(),
                new ApplicationSettingsViewModel()
            };
        }

        #endregion
    }
}
