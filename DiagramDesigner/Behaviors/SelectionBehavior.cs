using DiagramDesigner.View;
using DiagramDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DiagramDesigner.Behaviors
{
    public class SelectionBehavior
    {
        public static bool GetEnableSelection(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableSelectionProperty);
        }

        public static void SetEnableSelection(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableSelectionProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnableSelection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableSelectionProperty =
            DependencyProperty.RegisterAttached("EnableSelection", typeof(bool), typeof(SelectionBehavior), 
                new FrameworkPropertyMetadata((bool)false, new PropertyChangedCallback(OnEnableSelectionChanged)));

        private static void OnEnableSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)d;
            if ((bool)e.NewValue)
            {
                fe.PreviewMouseDown += Fe_PreviewMouseDown;
            }
            else
            {
                fe.PreviewMouseDown -= Fe_PreviewMouseDown;
            }
        }

        private static void Fe_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var vm = ((FrameworkElement)sender).DataContext as IDiagramItem;
            if (vm == null)
            {
                return;
            }

            bool isCtrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            if (vm.Parent == null)
            { // if it's the root diagram item -- the canvas, unselect all items
                if (!isCtrlPressed)
                {
                    foreach (var child in vm.Children)
                    {
                        child.IsSelected = false;
                    }
                }
                return;
            }

            if (!isCtrlPressed)
            {

                if (vm.IsSelected == false)
                {
                    vm.Parent.IsSelected = false;
                    foreach (var child in vm.Parent.Children)
                    {
                        child.IsSelected = false;
                    }

                    if (vm.Parent.Parent != null)
                    {
                        vm.Parent.Parent.IsSelected = false;
                        foreach (var child in vm.Parent.Parent.Children)
                        {
                            child.IsSelected = false;
                        }
                    }

                    if (vm.Children != null)
                    {
                        foreach (var child in vm.Children)
                        {
                            child.IsSelected = false;
                        }
                    }
                }

                vm.IsSelected = true;
                
            }
            else
            { // Ctrl is pressed
                vm.IsSelected = !vm.IsSelected;
            }


            if (vm.IsSelected)
            {
                var args = new RoutedEventArgs(DiagramControl.SelectedItemChangedEvent, vm);
                (sender as UIElement)?.RaiseEvent(args);
            }
            else
            {
                var args = new RoutedEventArgs(DiagramControl.SelectedItemChangedEvent, null);
                (sender as UIElement)?.RaiseEvent(args);
            }

        }
    }
}
