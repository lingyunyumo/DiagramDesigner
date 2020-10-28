using DiagramDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DiagramDesigner.Converters
{
    class ColorToBrushConverter : IValueConverter
    {
        static ColorToBrushConverter()
        {
            Instance = new ColorToBrushConverter();
        }

        public static ColorToBrushConverter Instance
        {
            get;
            private set;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var c = (Color)value;
            return new SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
