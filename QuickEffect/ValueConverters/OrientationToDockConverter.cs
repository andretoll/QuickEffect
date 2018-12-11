using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace QuickEffect.ValueConverters
{
    /// <summary>
    /// Null to visibility converter. Returns collapsed if value is null.
    /// </summary>
    public class OrientationToDockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Left";

            return value.ToString() == "Horizontal" ? "Top" : "Left";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
