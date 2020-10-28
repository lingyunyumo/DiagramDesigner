using DiagramDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DiagramDesigner.Converters
{
    class PathPointsMarginConverter : IValueConverter
    {
        static PathPointsMarginConverter()
        {
            Instance = new PathPointsMarginConverter();
        }

        public static PathPointsMarginConverter Instance
        {
            get;
            private set;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var points = (IList<Point>)value;

            Point middlePoint;
            if (points.Count % 2 == 1)
            {
                middlePoint = points[points.Count / 2];
            }
            else
            {
                var p1 = points[points.Count / 2 - 1];
                var p2 = points[points.Count / 2];
                middlePoint = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            }

            var margin = new Thickness(middlePoint.X-14, middlePoint.Y-14, 0, 0);
            return margin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
