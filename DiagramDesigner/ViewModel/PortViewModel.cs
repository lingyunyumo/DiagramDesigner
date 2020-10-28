using Newtonsoft.Json;
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
    public class PortViewModel : IDiagramItem
    {
        /// <summary>
        /// 是否有与此Port相连的Link
        /// </summary>
        [JsonIgnore]
        public bool IsLinked => OwnerLink != null;

        /// <summary>
        /// 与此Port相连的Link
        /// </summary>
        public LinkViewModel OwnerLink { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        [DependsOn("Left", "Top")]
        public Orientation Orientation 
        {
            get
            {
                var pNode = Parent as NodeViewModel;
                if (pNode == null)
                {
                    return Orientation.None;
                }

                if (Left == 0)
                {
                    return Orientation.Left;
                }

                if (Top == 0)
                {
                    return Orientation.Up;
                }

                if (Left == pNode.Width)
                {
                    return Orientation.Right;
                }

                if (Top == pNode.Height)
                {
                    return Orientation.Down;
                }

                return Orientation.None;
            }
        }

        public PortDirection Direction { get; set; } = PortDirection.Inlet;

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
        public string Comment { get; set; }
    }

    public enum Orientation
    {
        None = 0, // 0000
        Left = 1, // 0001
        Up = 2,   // 0010
        Right = 4,// 0100
        Down = 8  // 1000
    }

    public enum PortDirection
    {
        Inlet,
        Outlet,
        InletAndOutlet, // 既是前一部件的出口，又是后一部件的进口
        None
    }
}
