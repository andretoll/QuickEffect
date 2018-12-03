using QuickEffect.View;
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

namespace QuickEffect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DragAndDropViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new DragAndDropViewModel();
            this.DataContext = viewModel;
        }

        private void DropArea_Drop(object sender, DragEventArgs e)
        {
            string fileName = this.viewModel.DragFile(e);

            ProcessImageWindow processImageWindow = new ProcessImageWindow(fileName);
            processImageWindow.ShowDialog();
        }
    }
}
