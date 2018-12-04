using QuickEffect.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickEffect.View.UserControls
{
    /// <summary>
    /// Interaction logic for DragAndDropUserControl.xaml
    /// </summary>
    public partial class DragAndDropUserControl : UserControl
    {

        private DragAndDropViewModel viewModel;

        public DragAndDropUserControl()
        {
            InitializeComponent();

            viewModel = new DragAndDropViewModel();
            this.DataContext = viewModel;
        }

        private void DropArea_Drop(object sender, DragEventArgs e)
        {
            string fileName = this.viewModel.GetFileNamesFromDrop(e)[0];

            ProcessImageWindow processImageWindow = new ProcessImageWindow(fileName);
            processImageWindow.Show();

            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            //this.DropArea.Background = brush;
        }

        private void DropArea_DragEnter(object sender, DragEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Green);
            //this.DropArea.Background = brush;
        }

        private void DropArea_DragLeave(object sender, DragEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            //this.DropArea.Background = brush;
        }
    }
}
