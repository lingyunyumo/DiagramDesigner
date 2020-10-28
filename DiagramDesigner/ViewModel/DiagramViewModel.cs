using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using PropertyChanged;

namespace DiagramDesigner.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class DiagramViewModel : IDiagramItem
    {
        public DiagramViewModel()
        { 
            if (Children == null)
            {
                Children = new ObservableCollection<IDiagramItem>();
            }            
        }

        public bool ShowKeyValuesOnDiagram { get; set; }

        public double CanvasWidth { get; set; } = 2000;
        
        public double CanvasHeight { get; set; } = 1000;

        public double Scale { get; set; } = 1.0;

        public double ViewboxWidth => CanvasWidth * Scale;

        public double ViewboxHeight => CanvasHeight * Scale;

        public void UnselectAll(IDiagramItem item = null)
        {
            IList<IDiagramItem> children;
            if (item == null)
            {
                children = Children;
            }
            else
            {
                children = item.Children;
            }

            if (item != null)
            {
                item.IsSelected = false;
            }
            if (children == null || children.Count == 0)
            {
                IsSelected = false;
                return;
            }

            foreach (var x in children)
            {
                UnselectAll(x);
            }
        }

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
}
