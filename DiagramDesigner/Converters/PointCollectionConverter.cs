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
    class PointCollectionConverter : IValueConverter
    {
        static PointCollectionConverter()
        {
            Instance = new PointCollectionConverter();
        }

        public static PointCollectionConverter Instance
        {
            get;
            private set;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var points = (IList<Point>)value;


            PointCollection pointCollection = new PointCollection();
            foreach (var p in points)
            {
                pointCollection.Add(p);
            }
            return pointCollection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
