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

namespace QuickEffect.View
{
    /// <summary>
    /// Interaction logic for ProcessImageWindow.xaml
    /// </summary>
    public partial class ProcessImageWindow : Window
    {
        private string _fileName;

        public ProcessImageWindow(string fileName)
        {
            InitializeComponent();
            _fileName = fileName;

            this.Title = _fileName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            BitmapImage bitmap = new BitmapImage(new Uri(_fileName));
            this.Image.Source = bitmap;
        }
    }
}
