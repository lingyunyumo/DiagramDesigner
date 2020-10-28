using DiagramDesigner.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace DiagramDesigner.Adorners
{
    public abstract class DragAdorner : Adorner
    {
        #region Construction

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="view">视图</param>
        /// <param name="startPoint">起始点</param>
        protected DragAdorner(DiagramControl view, Point startPoint)
            : base(view)
        {
            this.DiagramControl = view;
            this.EndPoint = this.StartPoint = startPoint;
            this.Loaded += DragAdorner_Loaded;
        }

        #endregion Construction

        #region Members

        /// <summary>
        /// 视图
        /// </summary>
        public DiagramControl DiagramControl
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否提交
        /// </summary>
        protected bool IsCommit
        {
            get;
            set;
        }

        /// <summary>
        /// 是否落下
        /// </summary>
        private bool IsDrop
        {
            get;
            set;
        }

        /// <summary>
        /// 起始点
        /// </summary>
        protected Point StartPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 结束点
        /// </summary>
        protected Point EndPoint
        {
            get;
            set;
        }

        #endregion Members

        #region Methods

        /// <summary>
        /// 拖拽，如果在此位置放置，则返回true
        /// </summary>
        /// <returns></returns>
        protected abstract bool DoDrag();

        /// <summary>
        /// 结束拖拽
        /// </summary>
        protected abstract void EndDrag();

        private void DragAdorner_Loaded(object sender, RoutedEventArgs e)
        {
            this.IsCommit = false;
            this.CaptureMouse();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.EndPoint = e.GetPosition(this.DiagramControl);
            this.IsDrop = this.DoDrag();
            Mouse.OverrideCursor = this.IsDrop ? this.Cursor : Cursors.Cross;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                this.IsCommit = this.IsDrop;
                this.ReleaseMouseCapture();
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            this.DiagramControl.DragAdorner = null;
            Mouse.OverrideCursor = null;
            this.EndDrag();
        }

        #endregion Methods
    }
}
