using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickEffect.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ImageToolbarUserControl.xaml
    /// </summary>
    public partial class ImageToolbarUserControl : Canvas
    {
        #region Private members

        private Point? dragStart = null;

        #endregion

        #region Constructor

        public ImageToolbarUserControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Capture mouse down for toolbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolbarContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Get element and its position
            var element = (UIElement)sender;
            dragStart = e.GetPosition(element);

            // Capture mouse to this element
            element.CaptureMouse();
        }

        /// <summary>
        /// Capture mouse up for toolbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolbarContainer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Get element and its position
            var element = (UIElement)sender;
            dragStart = e.GetPosition(element);

            // Release mouse of this element
            element.ReleaseMouseCapture();
        }

        /// <summary>
        /// Capture mouse move for toolbar. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolbarContainer_MouseMove(object sender, MouseEventArgs e)
        {
            // If dragstart is registered and user is holding mouse button pressed
            if (dragStart != null && e.LeftButton == MouseButtonState.Pressed)
            {
                // Get sender as UIElement and its position
                var element = (UIElement)sender;
                var p2 = e.GetPosition(this);

                // If outside boundries, return
                if (p2.X < dragStart.Value.X || 
                    p2.Y < dragStart.Value.Y || 
                    (p2.X - dragStart.Value.X + ToolbarContainer.ActualWidth)> (Parent as FrameworkElement).ActualWidth || 
                    (p2.Y - dragStart.Value.Y + ToolbarContainer.ActualHeight> (Parent as FrameworkElement).ActualHeight))
                {
                    return;
                }

                // Move element
                Canvas.SetLeft(element, p2.X - dragStart.Value.X);
                Canvas.SetTop(element, p2.Y - dragStart.Value.Y);

                // Close popups if open
                if (BrightnessToggleButton.IsChecked ?? true) { BrightnessToggleButton.IsChecked = false; }
                if (RotateToggleButton.IsChecked ?? true) { RotateToggleButton.IsChecked = false; }
            }
        }

        private void BrightnessToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            bool open = BrightnessToggleButton.IsChecked ?? true;

            BrightnessPopup.IsOpen = open;
            RotateToggleButton.IsChecked = false;
        }

        private void RotateToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            bool open = RotateToggleButton.IsChecked ?? true;

            RotatePopup.IsOpen = open;
            BrightnessToggleButton.IsChecked = false;
        }

        #endregion
    }
}
