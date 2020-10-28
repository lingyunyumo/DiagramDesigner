using DiagramDesigner.ViewModel;
using Newtonsoft.Json;
using Serilog;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiagramDesigner.View
{
    /// <summary>
    /// DiagramControl.xaml 的交互逻辑
    /// </summary>
    public partial class DiagramControl : UserControl
    {
        #region NodeAddedEvent

        public static readonly RoutedEvent NodeAddedEvent = EventManager.RegisterRoutedEvent(
            "NodeAdded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DiagramControl));

        public event RoutedEventHandler NodeAdded
        {
            add { AddHandler(NodeAddedEvent, value); }
            remove { RemoveHandler(NodeAddedEvent, value); }
        }

        void RaiseNodeAddedEvent(object source = null)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NodeAddedEvent, source);
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region NodeRemovedEvent

        public static readonly RoutedEvent NodeRemovedEvent = EventManager.RegisterRoutedEvent(
            "NodeRemoved", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DiagramControl));

        public event RoutedEventHandler NodeRemoved
        {
            add { AddHandler(NodeRemovedEvent, value); }
            remove { RemoveHandler(NodeRemovedEvent, value); }
        }

        void RaiseNodeRemovedEvent(object source = null)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NodeRemovedEvent, source);
            RaiseEvent(newEventArgs);
        }

        #endregion


        #region SelectedItemChangedEvent

        public static readonly RoutedEvent SelectedItemChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedItemChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DiagramControl));
        
        public event RoutedEventHandler SelectedItemChanged
        {
            add { AddHandler(SelectedItemChangedEvent, value); }
            remove { RemoveHandler(SelectedItemChangedEvent, value); }
        }

        void RaiseSelectedItemChangedEvent(object source = null)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SelectedItemChangedEvent, source);
            RaiseEvent(newEventArgs);
        }

        #endregion


        #region DiagramSelectedEvent

        public static readonly RoutedEvent DiagramSelectedEvent = EventManager.RegisterRoutedEvent(
            "DiagramSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DiagramControl));

        public event RoutedEventHandler DiagramSelected
        {
            add { AddHandler(DiagramSelectedEvent, value); }
            remove { RemoveHandler(DiagramSelectedEvent, value); }
        }

        void RaiseDiagramSelectedEvent(object source = null)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(DiagramSelectedEvent, source);
            RaiseEvent(newEventArgs);
        }

        #endregion


        #region LinkAddedEvent
        public static readonly RoutedEvent LinkAddedEvent = EventManager.RegisterRoutedEvent(
            "LinkAdded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DiagramControl));

        public event RoutedEventHandler LinkAdded
        {
            add { AddHandler(LinkAddedEvent, value); }
            remove { RemoveHandler(LinkAddedEvent, value); }
        }

        void RaiseLinkAddedEvent(object source = null)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(LinkAddedEvent, source);
            RaiseEvent(newEventArgs);
        }
        #endregion

        #region LinkRemovedEvent
        public static readonly RoutedEvent LinkRemovedEvent = EventManager.RegisterRoutedEvent(
            "LinkRemoved", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DiagramControl));

        public event RoutedEventHandler LinkRemoved
        {
            add { AddHandler(LinkRemovedEvent, value); }
            remove { RemoveHandler(LinkRemovedEvent, value); }
        }

        void RaiseLinkRemovedEvent(object source = null)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(LinkRemovedEvent, source);
            RaiseEvent(newEventArgs);
        }
        #endregion


        #region RotateClockwiseClickedEvent
        public static readonly RoutedEvent RotateClockwiseClickedEvent = EventManager.RegisterRoutedEvent(
            "RotateClockwiseClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DiagramControl));

        public event RoutedEventHandler RotateClockwiseClicked
        {
            add { AddHandler(RotateClockwiseClickedEvent, value); }
            remove { RemoveHandler(RotateClockwiseClickedEvent, value); }
        }

        void RaiseRotateClockwiseClickedEvent(object source = null)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(RotateClockwiseClickedEvent, source);
            RaiseEvent(newEventArgs);
        }
        #endregion

        #region NodeMoreClickedEvent
        public static readonly RoutedEvent NodeMoreClickedEvent = EventManager.RegisterRoutedEvent(
            "NodeMoreClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DiagramControl));

        public event RoutedEventHandler NodeMoreClickedClicked
        {
            add { AddHandler(NodeMoreClickedEvent, value); }
            remove { RemoveHandler(NodeMoreClickedEvent, value); }
        }

        void RaiseNodeMoreClickedEvent(object source = null)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NodeMoreClickedEvent, source);
            RaiseEvent(newEventArgs);
        }
        #endregion


        public DiagramControl()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.All,
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };

            InitializeComponent();
        }


        /// <summary>
        /// 创建一个Node。
        /// Node可能含有一个或多个Port，也可能未含有Port。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(e.Data.GetFormats()[0]) as IDiagramItem;
            if (data != null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                string jsonText = JsonConvert.SerializeObject(data);
                var dragObject = JsonConvert.DeserializeObject<NodeViewModel>(jsonText);

                var canvas = sender as Canvas;

                Point position = e.GetPosition(canvas);

                var dvm = canvas.DataContext as DiagramViewModel;
                dragObject.Parent = dvm;
                dragObject.Left = position.X;
                dragObject.Top = position.Y;

                dvm.Children.Add(dragObject);
                RaiseNodeAddedEvent(dragObject);
                Mouse.OverrideCursor = null;
            }
        }


        /// <summary>
        /// 移动一个或多个Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nodeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var node = (sender as FrameworkElement).DataContext as NodeViewModel;
            if (node == null)
            {
                return;
            }

            var diagram = DataContext as DiagramViewModel;

            foreach (var item in diagram.Children)
            {
                if (!item.IsSelected)
                {
                    continue;
                }


                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;

                double left = item.Left;
                double top = item.Top;
                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                item.Left += deltaHorizontal;
                item.Top += deltaVertical;

                if (item.Children != null)
                {
                    foreach (var p in item.Children)
                    {
                        var pvm = p as PortViewModel;
                        if (pvm != null && pvm.OwnerLink != null)
                        {
                            pvm.OwnerLink.Update();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 移动一个Port。
        /// 只有按住Alt键是才移动Port。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void portThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            bool isAltPressed = (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
            if (!isAltPressed)
            {
                return;
            }

            var port = (sender as FrameworkElement).DataContext as PortViewModel;
            if (port == null)
            {
                return;
            }

            double newLeft = port.Left + e.HorizontalChange;
            double newTop = port.Top + e.VerticalChange;
            newLeft = Math.Max(newLeft, 0);
            newLeft = Math.Min(newLeft, port.Parent.Width);
            newTop = Math.Max(newTop, 0);
            newTop = Math.Min(newTop, port.Parent.Height);

            port.Left = newLeft;
            port.Top = newTop;

            if (port.OwnerLink != null)
            {
                port.OwnerLink.Update();
            }
            e.Handled = true;
        }


        /// <summary>
        /// 修改一个Node的尺寸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var node = (sender as Thumb).DataContext as NodeViewModel;
            if (node == null)
            {
                return;
            }

            double oldNodeWidth = node.Width;
            double oldNodeHeight = node.Height;
            double newNodeWidth = Math.Max(32, node.Width + e.HorizontalChange);
            double newNodeHeight = Math.Max(32, node.Height + e.VerticalChange);

            node.Width = newNodeWidth;
            node.Height = newNodeHeight;
            
            if (node.Children != null)
            {
                foreach (PortViewModel p in node.Children)
                {
                    p.Left = p.Left / oldNodeWidth * newNodeWidth;
                    p.Top = p.Top / oldNodeHeight * newNodeHeight;
                    //if (p.Left > node.Width)
                    //{
                    //    p.Left = node.Width;
                    //}
                    //if (p.Top > node.Height)
                    //{
                    //    p.Top = node.Height;
                    //}

                    //if (p.Left == oldNodeWidth)
                    //{
                    //    p.Left = newNodeWidth;
                    //}
                    //if (p.Top == oldNodeHeight)
                    //{
                    //    p.Top = newNodeHeight;
                    //}
                    if (p.OwnerLink != null)
                    {
                        p.OwnerLink.Update();
                    }
                }
            }

            e.Handled = true;
        }


        #region 创建一个Link

        PortViewModel _portFrom;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void portThumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isAltPressed = (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
            if (isAltPressed)
            {
                _portFrom = null;
                return;
            }
            else
            {
                var port = (sender as FrameworkElement).DataContext as PortViewModel;
                _portFrom = port;

                port.IsSelected = true;
                RaiseSelectedItemChangedEvent(port);

                // 如果没有按住Alt键，结束路由事件
                // 否则，让路由事件继续传播，portThumb_DragDelta将处理拖动Port的逻辑
                e.Handled = true;
            }

        }

        private void portThumb_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool isAltPressed = (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
            var diagram = DataContext as DiagramViewModel;
            
            var portTo = (sender as FrameworkElement).DataContext as PortViewModel;

            if (_portFrom == null || portTo == null
                || _portFrom.IsLinked || portTo.IsLinked
                || diagram == null
                || isAltPressed
                || _portFrom == portTo)
            {
                _portFrom = null;
                return;
            }

            if (_portFrom.Direction != PortDirection.Outlet || portTo.Direction != PortDirection.Inlet)
            {
                _portFrom = null;
                Log.Error("非法连接：必须从出口(圆形)到进口(正方形)");
                return;
            }

            if (_portFrom.Data.GetType() != portTo.Data.GetType())
            {
                _portFrom = null;
                Log.Error("非法连接：不能连接不同类型的进出口");
                return;
            }

            if (_portFrom.Parent == portTo.Parent)
            {
                _portFrom = null;
                Log.Error("非法连接：不能连接同一部件上的进出口");
                return;
            }

            var link = new LinkViewModel
            {
                PortFrom = _portFrom,
                PortTo = portTo,
                Parent = diagram
            };
            _portFrom.OwnerLink = link;
            portTo.OwnerLink = link;
            diagram.Children.Add(link);
            link.Update();

            _portFrom = null;

            RaiseLinkAddedEvent(link);
        }

        #endregion 创建一个Link


        /// <summary>
        /// 选择Node。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nodeThumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 使用PreviewMouseLeftButtonDown事件，而不用MouseLeftButtonDown事件
            // 因为MouseLeftButtonDown事件已经在内部的Port处理并Handle=ture了
            //
            // WPF路由事件知识点:
            // Preview事件是Tunneling传播的，非Preview事件是Bubble传播的

            var node = (sender as FrameworkElement).DataContext as NodeViewModel;
            if (node == null)
            {
                return;
            }

            bool isCtrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            var diagram = DataContext as DiagramViewModel;
            if (!isCtrlPressed && !node.IsSelected)
            {
                diagram.UnselectAll();
                node.IsSelected = true;
            }
            else if (isCtrlPressed)
            {
                node.IsSelected = !node.IsSelected;
            }

            RaiseSelectedItemChangedEvent(node);
            //e.Handled = true;
        }


        Point _canvasClickPosition;

        /// <summary>
        /// 反选所有Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canvasClickPosition = e.GetPosition(sender as IInputElement);

            var diagram = DataContext as DiagramViewModel;
            diagram.UnselectAll();

            RaiseDiagramSelectedEvent(diagram);

            DragAdorner = new Adorners.RubberBandAdorner(this, e.GetPosition(this), e.GetPosition(sender as IInputElement));

            //e.Handled = true;
        }


        private void miAddToLibrary_Click(object sender, RoutedEventArgs e)
        {
            var diagram = DataContext as DiagramViewModel;
            var node = (sender as FrameworkElement).DataContext as NodeViewModel;
            // TODO: 需要自定义路由事件，外层主窗口处理添加到组件库的逻辑
        }

        private void Remove(IDiagramItem item)
        {
            var diagram = DataContext as DiagramViewModel;

            if (item.GetType() == typeof(NodeViewModel))
            {
                var node = item as NodeViewModel;
                diagram.Children.Remove(node);
                if (node.Children == null) return;
                foreach (var child in node.Children)
                {
                    Remove(child);
                }

                RaiseNodeRemovedEvent(node);
            }
            else if (item.GetType() == typeof(PortViewModel))
            {
                var port = item as PortViewModel;
                if (port.OwnerLink != null)
                {
                    Remove(port.OwnerLink);
                }
            }
            else // if (item.GetType() == typeof(LinkViewModel))
            {
                var link = item as LinkViewModel;
                diagram.Children.Remove(link);
                link.PortFrom.OwnerLink = null;
                link.PortTo.OwnerLink = null;

                RaiseLinkRemovedEvent(link);
            }
        }

        private void miRemove_Click(object sender, RoutedEventArgs e)
        {
            //var item = (sender as FrameworkElement).DataContext as IDiagramItem;
            //if (item != null)
            //{
            //    Remove(item);
            //}

            var vm = DataContext as DiagramViewModel;
            while (true)
            {
                var firstSelectedItem = vm.Children.FirstOrDefault(a => a.IsSelected);
                if (firstSelectedItem == null)
                {
                    break;
                }
                Remove(firstSelectedItem);
            }
        }

        Point mousePosition;

        

        private void nodeThumb_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            mousePosition = e.GetPosition(this);
        }

        private void miAddPort_Click(object sender, RoutedEventArgs e)
        {
            var node = (sender as FrameworkElement).DataContext as NodeViewModel;
            if (node == null)
            {
                return;
            }

            var port = new PortViewModel
            {
                Left = mousePosition.X - node.Left,
                Top = mousePosition.Y - node.Top,
                Width = 10,
                Height = 10,
                Parent = node
            };
            if (node.Children == null)
            {
                node.Children = new ObservableCollection<IDiagramItem>();
            }
            node.Children.Add(port);
        }

        private void line_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as FrameworkElement).Focus();

            var link = (sender as FrameworkElement).DataContext as LinkViewModel;
            if (link == null)
            {
                return;
            }

            bool isCtrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            var diagram = DataContext as DiagramViewModel;
            if (!isCtrlPressed && !link.IsSelected)
            {
                diagram.UnselectAll();
                link.IsSelected = true;
            }
            else if (isCtrlPressed)
            {
                link.IsSelected = !link.IsSelected;
            }

            RaiseSelectedItemChangedEvent(link);
            e.Handled = true;
        }

        private void Rotate(NodeViewModel node, double deg)
        {
            node.ImageRotation += deg;

            double w0 = node.Width;
            double h0 = node.Height;

            double w1 = node.Height;
            double h1 = node.Width;

            node.Width = w1;
            node.Height = h1;

            // Node的几何中心
            double xo = node.Left + w0 / 2;
            double yo = node.Top + h0 / 2;

            node.Left = Math.Max(xo - w1 / 2, 0);
            node.Top = yo - h1 / 2;

            foreach (PortViewModel port in node.Children)
            {
                // 归一化
                double x0 = port.Left / w0;
                double y0 = port.Top / h0;
                // 移动原点
                double x00 = x0 - 0.5;
                double y00 = 0.5 - y0;

                double x11 = 0;
                double y11 = 0;
                if (deg == 90)
                {
                    // 顺时针旋转90度
                    x11 = y00;
                    y11 = -x00;
                }
                else if (deg == -90)
                {
                    // 逆时针旋转90度
                    x11 = -y00;
                    y11 = x00;
                }
                else
                {
                    throw new NotImplementedException($"未实现此旋转角度: {deg}");
                }

                // 恢复原点
                double x1 = x11 + 0.5;
                double y1 = 0.5 - y11;
                // 反归一化
                port.Left = x1 * w1;
                port.Top = y1 * h1;
                if (port.OwnerLink != null)
                {
                    port.OwnerLink.Update();
                }
            }
        }

        private void miRotateClockwise_Click(object sender, RoutedEventArgs e)
        {
            var node = (sender as FrameworkElement).DataContext as NodeViewModel;
            if (node == null)
            {
                return;
            }

            Rotate(node, 90);

            RaiseRotateClockwiseClickedEvent(node);
            e.Handled = true;
        }

        private void miRotateCounterClockwise_Click(object sender, RoutedEventArgs e)
        {
            var node = (sender as FrameworkElement).DataContext as NodeViewModel;
            if (node == null)
            {
                return;
            }

            Rotate(node, -90);

            e.Handled = true;
        }

        private void miMirrorHorizontally_Click(object sender, RoutedEventArgs e)
        {
            var node = (sender as FrameworkElement).DataContext as NodeViewModel;
            if (node == null)
            {
                return;
            }

            double w0 = node.Width;
            double h0 = node.Height;

            foreach (PortViewModel port in node.Children)
            {
                port.Left = w0 - port.Left;
                if (port.OwnerLink != null)
                {
                    port.OwnerLink.Update();
                }
            }

            e.Handled = true;
        }

        private void miMirrorVertically_Click(object sender, RoutedEventArgs e)
        {
            var node = (sender as FrameworkElement).DataContext as NodeViewModel;
            if (node == null)
            {
                return;
            }

            double w0 = node.Width;
            double h0 = node.Height;

            foreach (PortViewModel port in node.Children)
            {
                port.Top = h0 - port.Top;
                if (port.OwnerLink != null)
                {
                    port.OwnerLink.Update();
                }
            }

            e.Handled = true;
        }

        private void miMore_Click(object sender, RoutedEventArgs e)
        {
            var node = (sender as FrameworkElement).DataContext as NodeViewModel;
            if (node == null)
            {
                return;
            }
            RaiseNodeMoreClickedEvent(node);
            e.Handled = true;
        }

        private Adorner dragAdorner;
        public Adorner DragAdorner
        {
            get { return this.dragAdorner; }
            set
            {
                if (this.dragAdorner != value)
                {
                    var adornerLayer = AdornerLayer.GetAdornerLayer(this);
                    if (this.dragAdorner != null)
                    {
                        adornerLayer.Remove(this.dragAdorner);
                    }
                    this.dragAdorner = value;
                    if (this.dragAdorner != null)
                    {
                        adornerLayer.Add(this.dragAdorner);
                    }
                }
            }
        }

        private void miCopy_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext as NodeViewModel;
            CopyNode(item);
        }

        private void CopyNode(NodeViewModel node)
        {
            if (node == null)
            {
                Log.Error("复制部件失败");
                return;
            }

            var jsonText = JsonConvert.SerializeObject(node);
            Clipboard.SetData("mcool2.node", jsonText);
            //Log.Information("复制部件成功");
        }

        private void miPaste_Click(object sender, RoutedEventArgs e)
        {
            PasteNode();
        }

        private void PasteNode()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                var jsonText = Clipboard.GetData("mcool2.node") as string;
                if (string.IsNullOrEmpty(jsonText))
                {
                    Log.Error("粘贴部件失败：未复制部件？");
                    return;
                }

                var node = JsonConvert.DeserializeObject<NodeViewModel>(jsonText);
                if (node == null)
                {
                    Log.Error("粘贴部件失败");
                    return;
                }
                //node.Name = node.Name + " 复制";
                foreach (PortViewModel port in node.Children)
                {
                    if (port.OwnerLink != null)
                    {
                        port.Data = port.OwnerLink.Data;
                        port.OwnerLink = null;
                    }
                }

                //MenuItem mnu = sender as MenuItem;
                //Canvas canvas = null;
                //if (mnu != null)
                //{
                //    canvas = ((ContextMenu)mnu.Parent).PlacementTarget as Canvas;
                //}

                Point position = _canvasClickPosition;

                node.Left = position.X;
                node.Top = position.Y;

                var dvm = (DataContext as DiagramViewModel);
                node.Parent = dvm;
                node.IsSelected = true;
                foreach (var item in dvm.Children)
                {
                    item.IsSelected = false;
                }
                dvm.Children.Add(node);
                RaiseNodeAddedEvent(node);
            }
            catch (Exception ex)
            {
                Log.Error("粘贴部件失败");
                Log.Debug(ex.Message);
                return;
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canvasClickPosition = e.GetPosition(sender as IInputElement);
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                var vm = DataContext as DiagramViewModel;
                int selectedNodeCount = vm.Children.Count(a => a.IsSelected && a is NodeViewModel);
                if (selectedNodeCount > 1)
                {
                    Log.Information("请选中一个部件");
                    return;
                }
                var firstSelectedNode = vm.Children.FirstOrDefault(a => a.IsSelected && a is NodeViewModel) as NodeViewModel;
                if (firstSelectedNode == null)
                {
                    Log.Error("复制部件失败");
                }

                CopyNode(firstSelectedNode);

                e.Handled = true;
            }
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                PasteNode();
                e.Handled = true;
            }
            if (e.Key == Key.Delete)
            {
                var vm = DataContext as DiagramViewModel;
                while (true)
                {
                    var firstSelectedItem = vm.Children.FirstOrDefault(a => a.IsSelected);
                    if (firstSelectedItem == null)
                    {
                        break;
                    }
                    Remove(firstSelectedItem);
                }
                e.Handled = true;
            }
        }

        private void canvas_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

    }
}
