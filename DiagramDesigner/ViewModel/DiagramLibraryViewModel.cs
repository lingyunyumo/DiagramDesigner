using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using PropertyChanged;

namespace DiagramDesigner.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class DiagramLibraryViewModel
    { 
        public DiagramLibraryViewModel()
        {
            if (Items == null)
            {
                Items = new ObservableCollection<IDiagramItem>();
            }
        }

        public IList<IDiagramItem> Items { get; set; }
    }
}
