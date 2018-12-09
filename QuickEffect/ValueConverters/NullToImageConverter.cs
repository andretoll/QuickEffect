using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace QuickEffect.ValueConverters
{
    /// <summary>
    /// Null to image converter. Returns 'dependency property unset value' if value is null.
    /// </summary>
    public class NullToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            BitmapImage bitmap = new BitmapImage(new Uri(value as string));
            bitmap.DecodePixelWidth = 200;

            return bitmap;
        }
    }
}
