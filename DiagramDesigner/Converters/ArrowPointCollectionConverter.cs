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
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace DiagramDesigner.Converters
{
    class ArrowPointCollectionConverter : IValueConverter
    {
        static ArrowPointCollectionConverter()
        {
            Instance = new ArrowPointCollectionConverter();
        }

        public static ArrowPointCollectionConverter Instance
        {
            get;
            private set;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var points = (IList<Point>)value;

            // https://www.cnblogs.com/zhoug2020/p/7842808.html
            // define a triangle
            // (0,0) (-15,5) (-15,-5) direction (1,0)
            var m0 = DenseMatrix.OfArray(new double[,] {
            {  0, -15, -15 },
            {  0,   5,  -5 },
            {  1,   1,   1 } // 齐次
            });

            var startPoint = points[points.Count - 2];
            var endPoint = points.Last();
            double length = Math.Sqrt(Math.Pow(endPoint.Y - startPoint.Y, 2) + Math.Pow(endPoint.X - startPoint.X, 2));
            double deltaY = endPoint.Y - startPoint.Y;
            double deltaX = endPoint.X - startPoint.X;
            double sin = deltaY / length;
            double cos = deltaX / length;

            var translationMatrix = DenseMatrix.OfArray(new double[,] {
            { 1, 0, endPoint.X },
            { 0, 1, endPoint.Y },
            { 0, 0,          1 }
            });

            var rotationMatrix = DenseMatrix.OfArray(new double[,] {
            { cos, -sin, 0 },
            { sin,  cos, 0 },
            {   0,    0, 1 }
            });

            var scaleMatrix = DenseMatrix.OfArray(new double[,] {
            { 0.5,   0, 0 },
            {   0, 0.5, 0 },
            {   0,   0, 1 }
            });

            var m1 = translationMatrix.Multiply(rotationMatrix.Multiply(scaleMatrix.Multiply(m0))).ToArray();

            PointCollection pointCollection = new PointCollection();
            for (int i = 0; i < m0.ColumnCount; i++)
            {
                Point p = new Point(m1[0, i], m1[1, i]);
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
