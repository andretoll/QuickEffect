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
using System.Windows.Shapes;

namespace QuickEffect.Views
{
    /// <summary>
    /// Interaction logic for ProcessImageWindow.xaml
    /// </summary>
    public partial class ProcessImageWindow : Window
    {
        private List<string> _fileNames;

        public ProcessImageWindow(List<string> fileNames)
        {
            InitializeComponent();
            _fileNames = fileNames;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            BitmapImage bitmap = new BitmapImage(new Uri(_fileNames[0]));
            this.Image.Source = bitmap;
        }
    }
}
