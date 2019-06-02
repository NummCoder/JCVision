using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VisionUtil;
using VisionUtil.EnumStrucks;
using VisionUtil.NLog;
using VisionDisplayTool;

namespace VisionInterface
{
    public class CameraBase : ICamera, IDisposable
    {
        #region Properties

        public string UserID { get; set; }
        protected IntPtr latestFrameAddress { get; set; } = IntPtr.Zero;

        protected long grabTime { get; set; }

        protected Stopwatch stopWatch = new Stopwatch();

        protected HiPerfTimer timer { get; set; }
       
        public delegate void delegateComputeGrabTime(long time);

        public event delegateComputeGrabTime EventComputeGrabTime;
        public bool bConnectOk { get; set; } = false;
        public HImage Image { get; set; }

        public delegate void delegateProcessHImage(HImage hImage);

        public event delegateProcessHImage EventProcessImage;

        public IVisionCameraInfo Info { get; set; }
        #endregion

        #region 事件触发
        protected virtual void ProcessImageCallBack(HImage image)
        {
            EventProcessImage?.Invoke(image);
        }
        protected virtual void ProcessGrabTimeCallback(long time)
        {
            EventComputeGrabTime?.Invoke(time);
        }
        #endregion

        public CameraBase()
        {

        }
        public CameraBase(IVisionCameraInfo info)
        {
            Info = info;
        }
        #region Cam Methods
        public virtual bool CloseCamera()
        {
            throw new NotImplementedException();
        }

        public virtual string GetCameraIPAddress()
        {
            throw new NotImplementedException();
        }

        public virtual string GetCameraMacAddress()
        {
            throw new NotImplementedException();
        }

        public virtual string GetMaxExposureTime()
        {
            throw new NotImplementedException();
        }

        public virtual string GetMaxGain()
        {
            throw new NotImplementedException();
        }

        public virtual string GetMinExposureTime()
        {
            throw new NotImplementedException();
        }

        public virtual string GetMinGain()
        {
            throw new NotImplementedException();
        }

        public virtual HImage GetImage()
        {
            throw new NotImplementedException();
        }
        public virtual bool GrabOne()
        {
            throw new NotImplementedException();
        }

        public virtual bool OpenCamera()
        {
            throw new NotImplementedException();
        }

        public virtual bool SendSoftwareExecute()
        {
            throw new NotImplementedException();
        }

        public virtual bool SetExposureTime(string value)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetExternTrigger()
        {
            throw new NotImplementedException();
        }

        public virtual bool SetFreerun()
        {
            throw new NotImplementedException();
        }

        public virtual bool SetGain(string value)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetGrabOverTime(int value)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetHearBeatTime(string value)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetImageFormat(ImageFormat format)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetSoftwareTrigger()
        {
            throw new NotImplementedException();
        }

        public virtual bool StartGrabbing()
        {
            throw new NotImplementedException();
        }

        public virtual bool StopGrabbing()
        {
            throw new NotImplementedException();
        } 

        public virtual bool SetTriggerDelayTime(string value)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetOutLineTime(string value)
        {
            throw new NotImplementedException();
        }
        public virtual bool SetLineDebouncerTime(string value)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Tool Methods
        public override string ToString()
        {
            return "相机工具";
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        bConnectOk = false;
                        if (Image != null)
                        {
                            Image.Dispose();
                        }

                        if (latestFrameAddress != null)
                        {
                            Marshal.FreeHGlobal(latestFrameAddress);
                            latestFrameAddress = IntPtr.Zero;
                        }
                    }
                    catch (Exception e)
                    {
                        LogFileManager.Error("Camera", e.ToString());
                    }
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~CameraBase() {
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

        #region 记录Log信息
        protected  void WriteInfoLog(string infoMessage)
        {
            LogFileManager.Info("Camera", infoMessage);
        }
        protected  void WriteDebugLog(string debugMessage)
        {
            LogFileManager.Debug("Camera", debugMessage);
        }
        protected  void WriteErrorLog(string errorMessage)
        {
            LogFileManager.Error("Camera", errorMessage);
        }
        protected  void WriteFatalLog(string fatalMessage)
        {
            LogFileManager.Fatal("Camera", fatalMessage);
        }

        public ToolResult GetResult()
        {
            throw new NotImplementedException();
        }

        public void SetImage(HImage image)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
