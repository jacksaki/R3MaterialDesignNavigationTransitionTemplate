using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace R3MaterialDesignNavigationTransitionTemplate.Converters;

[ValueConversion(typeof(Color), typeof(string))]
public class ColorToStringConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }
        if(!(value is Color))
        {
            return null;
        }
        var color = (Color)value;
        return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
