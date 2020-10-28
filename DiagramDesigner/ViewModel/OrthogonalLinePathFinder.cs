using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace DiagramDesigner.ViewModel
{
    public class OrthogonalLinePathFinder : IPathFinder
    {
        public IList<Point> FindPath(PortViewModel portFrom, PortViewModel portTo)
        {
            var points = new ObservableCollection<Point>();
            var pointFrom = new Point(portFrom.Left + portFrom.Parent.Left, portFrom.Top + portFrom.Parent.Top);
            var pointTo = new Point(portTo.Left + portTo.Parent.Left, portTo.Top + portTo.Parent.Top);
            points.Add(pointFrom);

            // 同向的
            if (portFrom.Orientation == portTo.Orientation)
            {
                if (portFrom.Orientation == Orientation.Left)
                {
                    double alignedX = Math.Min(pointFrom.X, pointTo.X) - 20;
                    points.Add(new Point(alignedX, pointFrom.Y));
                    points.Add(new Point(alignedX, pointTo.Y));
                }
                if (portFrom.Orientation == Orientation.Right)
                {
                    double alignedX = Math.Max(pointFrom.X, pointTo.X) + 20;
                    points.Add(new Point(alignedX, pointFrom.Y));
                    points.Add(new Point(alignedX, pointTo.Y));
                }
                if (portFrom.Orientation == Orientation.Up)
                {
                    double alignedY = Math.Min(pointFrom.Y, pointTo.Y) - 20;
                    points.Add(new Point(pointFrom.X, alignedY));
                    points.Add(new Point(pointTo.X, alignedY));
                }
                if (portFrom.Orientation == Orientation.Down)
                {
                    double alignedY = Math.Max(pointFrom.Y, pointTo.Y) + 20;
                    points.Add(new Point(pointFrom.X, alignedY));
                    points.Add(new Point(pointTo.X, alignedY));
                }
            }

            // 反向的
            if ((portFrom.Orientation | portTo.Orientation) == (Orientation.Up | Orientation.Down))
            {
                double alignedY = (pointFrom.Y + pointTo.Y) / 2;
                points.Add(new Point(pointFrom.X, alignedY));
                points.Add(new Point(pointTo.X, alignedY));
            }
            if ((portFrom.Orientation | portTo.Orientation) == (Orientation.Left | Orientation.Right))
            {
                double alignedX = (pointFrom.X + pointTo.X) / 2;
                points.Add(new Point(alignedX, pointFrom.Y));
                points.Add(new Point(alignedX, pointTo.Y));
            }

            // 正交的
            if ((portFrom.Orientation | portTo.Orientation) == (Orientation.Up | Orientation.Right)
                || (portFrom.Orientation | portTo.Orientation) == (Orientation.Left | Orientation.Up)
                || (portFrom.Orientation | portTo.Orientation) == (Orientation.Down | Orientation.Left)
                || (portFrom.Orientation | portTo.Orientation) == (Orientation.Right | Orientation.Down))
            {
                double alignedX = 0;
                double alignedY = 0;
                if (portFrom.Orientation == Orientation.Up || portFrom.Orientation == Orientation.Down)
                {
                    alignedX = pointFrom.X;
                }
                if (portFrom.Orientation == Orientation.Left || portFrom.Orientation == Orientation.Right)
                {
                    alignedY = pointFrom.Y;
                }
                if (portTo.Orientation == Orientation.Up || portTo.Orientation == Orientation.Down)
                {
                    alignedX = pointTo.X;
                }
                if (portTo.Orientation == Orientation.Left || portTo.Orientation == Orientation.Right)
                {
                    alignedY = pointTo.Y;
                }
                points.Add(new Point(alignedX, alignedY));
            }



            points.Add(pointTo);
            return points;

        }
    }
}
