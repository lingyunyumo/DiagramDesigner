using DiagramDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DiagramDesigner.Selectors
{
    class DiagramElementStyleSelector : StyleSelector
    {
        static DiagramElementStyleSelector()
        {
            Instance = new DiagramElementStyleSelector();
        }

        public static DiagramElementStyleSelector Instance
        {
            get;
            private set;
        }


        public override Style SelectStyle(object item, DependencyObject container)
        {
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
            if (itemsControl == null || itemsControl.Resources.Count == 0)
                return null;

            if (item is NodeViewModel)
            {
                return (Style)itemsControl.FindResource("nodeStyle");
            }

            if (item is LinkViewModel)
            {
                return (Style)itemsControl.FindResource("linkStyle");
            }

            return null;
        }
    
    }
}
