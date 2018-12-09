using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace QuickEffect.ValueConverters
{
    /// <summary>
    /// URI to image converter. Returns thumbnail bitmap image of URI.
    /// </summary>
    public class UriToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !File.Exists(value as string))
                return null;

            BitmapImage bitmap = new BitmapImage(new Uri(value.ToString()));

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !File.Exists(value as string))
                return null;

            BitmapImage bitmap = new BitmapImage(new Uri(value as string));

            return bitmap;
        }
    }
}
