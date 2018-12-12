using QuickEffect.Models;
using System;
using System.Globalization;
using System.IO;
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
            // Treat object as image item
            if (value == null || !File.Exists((value as ImageItem).FileName))
                return null;

            BitmapImage bitmap = new BitmapImage(new Uri((value as ImageItem).FileName));
            bitmap.DecodePixelWidth = 200;

            return bitmap;
        }
    }
}
