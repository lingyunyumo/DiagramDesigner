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
    public interface IDiagramItem
    {
        int Id { get; set; }

        string Name { get; set; }

        bool IsSelected { get; set; }

        bool Disabled { get; set; }

        double Left { get; set; }

        double Top { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        Color Color { get; set; }

        IList<IDiagramItem> Children { get; set; }

        IDiagramItem Parent { get; set; }

        object Data { get; set; }

        [DisplayName("备注")]
        string Comment { get; set; }
    }
}
