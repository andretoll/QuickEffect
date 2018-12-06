using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuickEffect.Helpers
{
    /// <summary>
    /// A helper class to load, save and retrieve settings.
    /// </summary>
    public static class SettingsHelper
    {
        #region Members

        // Default settings
        private static readonly string defaultTheme = "BaseLight";
        private static readonly string defaultAccent = "Blue";

        #endregion

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
        /// Load settings
        /// </summary>
        public static void LoadSettings()
        {
            try
            {
                SetAppTheme(Properties.Settings.Default.MetroTheme, Properties.Settings.Default.MetroAccent);
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
            LoadSettings();
        }

        /// <summary>
        /// Set App Theme and Accent
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="accent"></param>
        public static void SetAppTheme(string theme, string accent)
        {
            // If either theme or accent is empty
            if ((string.IsNullOrEmpty(theme)) || (string.IsNullOrEmpty(accent)))
            {
                // Get current theme or accent from ThemeManager
                Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);

                // Apply either theme or accent
                if (string.IsNullOrEmpty(theme))
                    theme = appStyle.Item1.Name;

                if (string.IsNullOrEmpty(accent))
                    accent = appStyle.Item2.Name;
            }

            try
            {
                // Try applying theme and accent
                ThemeManager.ChangeAppStyle(
                    Application.Current,
                    ThemeManager.GetAccent(accent),
                    ThemeManager.GetAppTheme(theme)
                );
            }
            catch
            {
                // If any errors occur, set base theme and accent
                ThemeManager.ChangeAppStyle(
                    Application.Current,
                    ThemeManager.GetAccent(defaultAccent),
                    ThemeManager.GetAppTheme(defaultTheme)
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
