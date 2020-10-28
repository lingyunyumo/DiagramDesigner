using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiagramDesigner.ViewModel
{
    public interface IPathFinder
    {
        IList<Point> FindPath(PortViewModel portFrom, PortViewModel portTo);
    }
}
