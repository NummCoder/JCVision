using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionUtil.EnumStrucks;

namespace VisionInterface
{
   public interface ICamera
    {      
        #region 相机操作
        bool OpenCamera();
        bool CloseCamera();
        bool GrabOne();
        bool StartGrabbing();
        bool StopGrabbing();

        string UserID { get; set; }
        HImage GetImage();
        #endregion

        #region 参数设置
        bool SetHearBeatTime(string value);
        string GetMaxExposureTime();
        string GetMinExposureTime();
        string GetMinGain();
        string GetMaxGain();
        bool SetExposureTime(string value);
        bool SetGain(string value);
        bool SetImageFormat(ImageFormat format);
        bool SetFreerun();
        bool SetSoftwareTrigger();
        bool SendSoftwareExecute();
        bool SetExternTrigger();
        bool SetGrabOverTime(int value);
        string GetCameraIPAddress();
        string GetCameraMacAddress();
        #endregion
    }
}
