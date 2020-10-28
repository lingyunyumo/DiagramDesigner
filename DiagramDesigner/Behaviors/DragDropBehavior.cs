using DiagramDesigner.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiagramDesigner.Behaviors
{
    public class DragDropBehavior
    {
        #region EnableDrag

        public static readonly DependencyProperty EnableDragProperty =
            DependencyProperty.RegisterAttached("EnableDrag", typeof(bool), typeof(DragDropBehavior),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnEnableDragChanged)));

        public static bool GetEnablDrag(DependencyObject d)
        {
            return (bool)d.GetValue(EnableDragProperty);
        }

        public static void SetEnableDrag(DependencyObject d, bool value)
        {
            d.SetValue(EnableDragProperty, value);
        }

        private static void OnEnableDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)d;

            if ((bool)e.NewValue)
            {
                fe.PreviewMouseDown += DragPreviewMouseDown;
                fe.MouseMove += DragMouseMove;
            }
            else
            {
                fe.PreviewMouseDown -= DragPreviewMouseDown;
                fe.MouseMove -= DragMouseMove;
            }
        }

        static void DragMouseMove(object sender, MouseEventArgs e)
        {
            Point? dragStartPoint = GetDragStartPoint((DependencyObject)sender);

            if (e.LeftButton != MouseButtonState.Pressed)
                dragStartPoint = null;

            if (dragStartPoint.HasValue)
            {
                var obj = ((FrameworkElement)sender).DataContext as IDiagramItem;
                DragDrop.DoDragDrop((DependencyObject)sender, obj, DragDropEffects.Copy);
                e.Handled = true;
            }
        }

        static void DragPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetDragStartPoint((DependencyObject)sender, e.GetPosition((IInputElement)sender));
        }

        #endregion 

        #region EnableDrop

        public static readonly DependencyProperty EnableDropProperty =
            DependencyProperty.RegisterAttached("EnableDrop", typeof(bool), typeof(DragDropBehavior),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnEnableDropChanged)));

        public static bool GetEnableDrop(DependencyObject d)
        {
            return (bool)d.GetValue(EnableDropProperty);
        }

        public static void SetEnableDrop(DependencyObject d, bool value)
        {
            d.SetValue(EnableDropProperty, value);
        }

        private static void OnEnableDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (Canvas)d;

            if ((bool)e.NewValue)
            {
                DragDrop.AddDropHandler(fe, DropOnDrop);
            }
            else
            {
                DragDrop.RemoveDropHandler(fe, DropOnDrop);
            }
        }

        private static void DropOnDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(e.Data.GetFormats()[0]) as IDiagramItem;
            if (data != null)
            {
                string jsonText = JsonConvert.SerializeObject(data);
                var dragObject = JsonConvert.DeserializeObject<IDiagramItem>(jsonText);

                var canvas = sender as Canvas;

                Point position = e.GetPosition(canvas);

                var dvm = canvas.DataContext as DiagramViewModel;
                dragObject.Parent = dvm;
                dragObject.Left += position.X;
                dragObject.Top += position.Y;
                dvm.Children.Add(dragObject);
                //if (dragObject.Children != null)
                //{
                //    for (int i = 0; i < dragObject.Children.Count; i++)
                //    {
                //        dragObject.Children[i].Left += position.X;
                //        dragObject.Children[i].Top += position.Y;
                //        //dvm.Children.Add(dragObject.Children[i]);
                //    }
    
                //}
            }
        }

        #endregion

        #region DragStartPoint

        public static readonly DependencyProperty DragStartPointProperty =
            DependencyProperty.RegisterAttached("DragStartPoint", typeof(Point?), typeof(DragDropBehavior));

        public static Point? GetDragStartPoint(DependencyObject d)
        {
            return (Point?)d.GetValue(DragStartPointProperty);
        }


        public static void SetDragStartPoint(DependencyObject d, Point? value)
        {
            d.SetValue(DragStartPointProperty, value);
        }

        #endregion

        
    }
}
