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
        private Point? dragStart = null;
        private bool dragActive = false;

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
            // If drag is active, dragstart is registered and user is holding mouse button pressed
            if (dragStart != null && e.LeftButton == MouseButtonState.Pressed && dragActive)
            {
                // Get sender as UIElement and its position
                var element = (UIElement)sender;
                var p2 = e.GetPosition(this);

                // If outside boundries, return
                if (p2.X - 5 < dragStart.Value.X || 
                    p2.Y - 5 < dragStart.Value.Y || 
                    (p2.X - dragStart.Value.X + ToolbarContainer.ActualWidth) + 5 > (Parent as FrameworkElement).ActualWidth || 
                    (p2.Y - dragStart.Value.Y + ToolbarContainer.ActualHeight + 5 > (Parent as FrameworkElement).ActualHeight))
                {
                    return;
                }

                // Move element
                Canvas.SetLeft(element, p2.X - dragStart.Value.X);
                Canvas.SetTop(element, p2.Y - dragStart.Value.Y);           
            }
        }

        /// <summary>
        /// Upon toggling move on and off.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            // Toggle drag
            dragActive = (sender as ToggleButton).IsChecked ?? true;

            // Show message if active and change icon
            if (dragActive)
            {
                (this.DataContext as ViewModels.ImageEditorViewModel).SetMessage("Drag and drop enabled.");

                MoveHandleToggleIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.LockOpen;
            }
            else
                MoveHandleToggleIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.Lock;
        }

        #endregion
    }
}
