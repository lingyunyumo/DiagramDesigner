using DiagramDesigner.View;
using DiagramDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DiagramDesigner.Adorners
{
    /// <summary>
    /// 橡皮筋装饰器
    /// </summary>
    class RubberBandAdorner : DragAdorner
    {
        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="view">视图</param>
        /// <param name="startPoint">起始点</param>
        public RubberBandAdorner(DiagramControl view, Point startPoint, Point realStartPoint)
            : base(view, startPoint)
        {
            this.m_RealStartPoint = realStartPoint;
            this.m_Pen = new Pen(Brushes.Black, 1);// 画笔颜色：黑色，线宽：1
        }

        #endregion Construction

        #region Members

        private Point m_RealStartPoint;

        /// <summary>
        /// 画笔
        /// </summary>
        private Pen m_Pen;

        /// <summary>
        /// 画笔颜色，默认为：黑色
        /// </summary>
        public Brush PenBrush
        {
            get { return this.m_Pen.Brush; }
            set
            {
                if (this.m_Pen.Brush != value)
                {
                    this.m_Pen.Brush = value;
                }
            }
        }

        /// <summary>
        /// 画笔粗细，默认线宽为：1
        /// </summary>
        public double PenThickness
        {
            get { return this.m_Pen.Thickness; }
            set
            {
                if (this.m_Pen.Thickness != value)
                {
                    this.m_Pen.Thickness = value;
                }
            }
        }

        /// <summary>
        /// 矩形填充画笔颜色
        /// </summary>
        private Brush m_RectBrush = new SolidColorBrush(Color.FromArgb(50, 0, 100, 255));
        /// <summary>
        /// 矩形填充画笔颜色
        /// </summary>
        public Brush RectBrush
        {
            get { return this.m_RectBrush; }
            set
            {
                this.m_RectBrush = value;
            }
        }

        #endregion Members

        #region Methods

        protected override bool DoDrag()
        {
            this.InvalidateVisual();
            return true;
        }

        /// <summary>
        /// 结束拖拽
        /// </summary>
        protected override void EndDrag()
        {
            if (this.IsCommit)
            {
                var vm = this.DiagramControl.DataContext as DiagramViewModel;// .DiagramItems.Where(p => p.CanSelect && rect.Contains(p.Bounds));
                if (vm == null)
                {
                    return;
                }

                double xOffset = m_RealStartPoint.X - StartPoint.X;
                double yOffset = m_RealStartPoint.Y - StartPoint.Y;

                var rect = new Rect(
                    Math.Min(EndPoint.X, StartPoint.X) + xOffset,
                    Math.Min(EndPoint.Y, StartPoint.Y) + yOffset,
                    Math.Abs(EndPoint.X - StartPoint.X) / vm.Scale,
                    Math.Abs(EndPoint.Y - StartPoint.Y) / vm.Scale
                );
                
                vm.UnselectAll();
                var selectedItems = vm.Children.Where(a => 
                    rect.Contains(new Rect(a.Left, 
                        a.Top, 
                        a.Width, 
                        a.Height)
                ));
                foreach (var item in selectedItems)
                {
                    item.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// 绘制选择矩形框
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(this.m_RectBrush, this.m_Pen, new Rect(this.StartPoint, this.EndPoint));
        }

        #endregion Methods
    }
}
