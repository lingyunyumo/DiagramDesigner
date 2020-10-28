using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DiagramDesigner.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class LinkViewModel : IDiagramItem
    {
        public LinkViewModel()
        {
            //PathFinder = new StraightLinePathFinder();
            PathFinder = new OrthogonalLinePathFinder();
        }

        public PortViewModel PortFrom { get; set; }

        public PortViewModel PortTo { get; set; }

        public IPathFinder PathFinder { get; set; }

        [JsonIgnore]
        [DoNotCheckEquality]
        public IList<Point> PathPoints
        {
            get
            {
                if (PortFrom == null || PortTo == null)
                {
                    return null;
                }
                var points = PathFinder.FindPath(PortFrom, PortTo);
               
                return points;
            }
            private set
            {
                // Only for triggering INotifyPropertyChanged
            }
        }

        public void Update()
        {
            PathPoints = null;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool Disabled { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Color Color { get; set; } = Colors.Black;
        public IList<IDiagramItem> Children { get; set; }
        public IDiagramItem Parent { get; set; }
        public object Data { get; set; }
        public string Comment { get; set; }
    }
}
