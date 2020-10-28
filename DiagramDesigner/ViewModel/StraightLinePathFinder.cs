using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiagramDesigner.ViewModel
{
    public class StraightLinePathFinder : IPathFinder
    {
        public IList<Point> FindPath(PortViewModel portFrom, PortViewModel portTo)
        {
            var points = new ObservableCollection<Point>();
            points.Add(new Point(portFrom.Left + portFrom.Parent.Left, portFrom.Top + portFrom.Parent.Top));
            points.Add(new Point(portTo.Left + portTo.Parent.Left, portTo.Top + portTo.Parent.Top));
            return points;
        }
    }
}
