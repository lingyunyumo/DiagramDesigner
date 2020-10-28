using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DiagramDesigner.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class NodeViewModel : IDiagramItem
    { 
        public string ImagePath{ get; set; }

        public double ImageRotation { get; set; }


        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool Disabled { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Color Color { get; set; } = Colors.White;
        public IList<IDiagramItem> Children { get; set; }
        public IDiagramItem Parent { get; set; }
        public object Data { get; set; }

        [DisplayName("备注")] 
        public string Comment { get; set; }
    }

}
