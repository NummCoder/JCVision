using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionUtil.NLog;

namespace VisionDisplayTool.DrawROI
{
    /// <summary>
    /// 泛型ROI基类，提供五个可选的参数，用于更新图形参数
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    public class DrawBase<T1, T2, T3, T4, T5> : IDisposable
    {
        /// <summary>
        /// 图形第一个参数
        /// </summary>
        public T1 Content1 { get; set; }
        /// <summary>
        /// 图形第二个参数
        /// </summary>
        public T2 Content2 { get; set; }
        /// <summary>
        /// 图形第三个参数
        /// </summary>
        public T3 Content3 { get; set; }
        /// <summary>
        /// 图形第四个参数
        /// </summary>
        public T4 Content4 { get; set; }
        /// <summary>
        /// 图形第五个参数
        /// </summary>
        public T5 Content5 { get; set; }
        public HImage Image { get; set; }

        public HWindow Window { get; set; }

        private HDrawingObject.HDrawingObjectCallback onDrawCallBack;
        private HDrawingObject.HDrawingObjectCallback onResizeCallBack;
        private HDrawingObject.HDrawingObjectCallback onSelectCallBack;
       

        public delegate void ParametersHandler(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);

        /// <summary>
        /// 图形参数传递事件
        /// </summary>
        public event ParametersHandler ProcessParameters;
        protected object lockObj { get; set; }

        protected HDrawingObject drawingObj { get; set; }
        public HDrawingObject.HDrawingObjectCallback OnDrawCallBack
        {
            get { return onDrawCallBack; }
            set { onDrawCallBack = value; }
        }
        public HDrawingObject.HDrawingObjectCallback OnResizeCallBack
        {
            get { return onResizeCallBack; }
            set { onResizeCallBack = value; }
        }
        public HDrawingObject.HDrawingObjectCallback OnSelectCallBack
        {
            get { return onSelectCallBack; }
            set { onSelectCallBack = value; }
        }

        public DrawBase()
        {
            OnDrawCallBack = new HDrawingObject.HDrawingObjectCallback(DrawingItemActioning);
            OnResizeCallBack = new HDrawingObject.HDrawingObjectCallback(DrawingItemActioning);
            OnSelectCallBack = new HDrawingObject.HDrawingObjectCallback(DrawingItemActioning);
            lockObj = new object();
            drawingObj = new HDrawingObject();
        }
        public DrawBase(HWindow window, HImage image)
        {
            OnDrawCallBack = new HDrawingObject.HDrawingObjectCallback(DrawingItemActioning);
            OnResizeCallBack = new HDrawingObject.HDrawingObjectCallback(DrawingItemActioning);
            OnSelectCallBack = new HDrawingObject.HDrawingObjectCallback(DrawingItemActioning);
            this.Window = window;
            this.Image = image;
            lockObj = new object();
            drawingObj = new HDrawingObject();
        }
        protected void DrawingItemActioning(IntPtr drawid, IntPtr windowHandle, string type)
        {

        }
        protected virtual void SetDefaultSetting()
        {
            drawingObj.SetDrawingObjectParams("color", "green");
            HOperatorSet.SetDrawingObjectParams(drawingObj, "line_width", 1);
            drawingObj.OnSelect(OnSelectCallBack);
            drawingObj.OnDrag(OnDrawCallBack);
            drawingObj.OnResize(OnResizeCallBack);
        }
        public virtual void CreateROI()
        {
            return;
        }
        public virtual void DrawROIComplete()
        {
            HOperatorSet.SetSystem("flush_graphic", "false");
            Window.ClearWindow();
            HOperatorSet.SetSystem("flush_graphic", "true");
            if (Image != null)
            {
                Window.DispImage(Image);
            }
        }

        protected virtual void WriteInfoLog(string infoMessage)
        {
            LogFileManager.Info("ROI", infoMessage);
        }

        protected virtual void WriteErrorLog(string errorMessage)
        {
            LogFileManager.Error("ROI", errorMessage);
        }

        protected virtual void WriteFatalLog(string fatalMessage)
        {
            LogFileManager.Fatal("ROI", fatalMessage);
        }
        protected virtual void WriteDebugLog(string debugMessage)
        {
            LogFileManager.Debug("ROI", debugMessage);
        }

        /// <summary>
        /// 可以通过绑定事件来图形参数，目前只提供五个泛型参数
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        protected void RaiseProcessROIParameter(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            ProcessParameters?.Invoke(t1, t2, t3, t4, t5);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用，防止出现多次释放

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.drawingObj != null)
                    {
                        drawingObj = null;
                    }
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ROIBase() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
