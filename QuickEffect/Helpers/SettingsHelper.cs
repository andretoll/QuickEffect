using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace QuickEffect.Helpers
{
    /// <summary>
    /// A helper class to load, save and retrieve settings.
    /// </summary>
    public static class SettingsHelper
    {
        #region Methods

        /// <summary>
        /// Write to a specific property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void WriteToSettings(string property, object value)
        {
            Properties.Settings.Default[property] = value;
        }

        /// <summary>
        /// Read the value from a specific property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object ReadFromSettings(string property)
        {
            return Properties.Settings.Default[property];
        }

        /// <summary>
        /// Load settings and restore defaults if necessary.
        /// </summary>
        public static void ValidateSettings()
        {
            try
            {
                // Validate settings
                var prop = Properties.Settings.Default.Properties;

                // Iterate through each property. If property value is empty, restore default value
                foreach (var setting in prop)
                {
                    string name = prop[((SettingsProperty)setting).Name].Name;
                    string value = Properties.Settings.Default[name].ToString();

                    // If value is empty
                    if (string.IsNullOrEmpty(value))
                    {
                        // Restore default value
                        Properties.Settings.Default[name] = ((SettingsProperty)setting).DefaultValue;
                    }
                }
            }
            catch
            {
                // TODO: If any errors occurs on loading settings, restore defaults
            }
        }

        /// <summary>
        /// Save Settings
        /// </summary>
        public static void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Discard pending settings.
        /// </summary>
        public static void DiscardSettings()
        {

            Properties.Settings.Default.Reload();
            SetAppTheme();
        }

        /// <summary>
        /// Set App Theme and Accent
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="accent"></param>
        public static void SetAppTheme()
        {            
            try
            {
                // Try applying theme and accent
                ThemeManager.ChangeAppStyle(
                    Application.Current,
                    ThemeManager.GetAccent(Properties.Settings.Default.MetroAccent),
                    ThemeManager.GetAppTheme(Properties.Settings.Default.MetroTheme)
                );
            }
            catch
            {
                // If any errors occur, set default theme and accent
                ThemeManager.ChangeAppStyle(
                    Application.Current,
                    ThemeManager.GetAccent(Properties.Settings.Default.Properties["MetroTheme"].DefaultValue.ToString()),
                    ThemeManager.GetAppTheme(Properties.Settings.Default.Properties["MetroAccent"].DefaultValue.ToString())
                );
            }
        }

        /// <summary>
        /// Get current theme
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTheme()
        {
            // Get current theme
            return ThemeManager.DetectAppStyle(Application.Current).Item1.Name;
        }

        /// <summary>
        /// Get current accent
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentAccent()
        {
            // Get current accent
            return ThemeManager.DetectAppStyle(Application.Current).Item2.Name;
        }

        /// <summary>
        /// Load available themes
        /// </summary>
        public static List<string> GetAllThemes()
        {
            // Populate collection
            List<string> themes = new List<string>();
            themes.Add("BaseLight");
            themes.Add("BaseDark");

            return themes;
        }

        /// <summary>
        /// Load available accents
        /// </summary>
        public static List<string> GetAllAccents()
        {
            // Populate collection
            List<string> accents = new List<string>();
            accents.Add("Blue");
            accents.Add("Red");
            accents.Add("Green");
            accents.Add("Orange");
            accents.Add("Yellow");
            accents.Add("Pink");
            accents.Add("Purple");
            accents.Add("Lime");
            accents.Add("Emerald");
            accents.Add("Teal");
            accents.Add("Cobalt");
            accents.Add("Indigo");
            accents.Add("Violet");
            accents.Add("Magenta");
            accents.Add("Crimson");
            accents.Add("Amber");
            accents.Add("Brown");
            accents.Add("Olive");
            accents.Add("Steel");
            accents.Add("Mauve");
            accents.Add("Taupe");
            accents.Add("Sienna");

            // Return list ordered alphabetically
            return accents.OrderBy(a => a).ToList();
        }

        #endregion

        public enum Settings
        {
            MetroTheme,
            MetroAccent,
            OpenEditorInNewWindow,
            ImageListOrientation
        }
    }
}
