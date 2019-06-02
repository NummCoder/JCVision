using Basler.Pylon;
using MvCamCtrl.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VisionUtil.NLog;

namespace VisionCameraManager
{
    /// <summary>
    /// 相机通用的静态函数
    /// </summary>
    public static class CameraGeneral
    {
        /// <summary>
        /// 巴斯勒相机
        /// </summary>
        /// <returns></returns>
        public static List<string> GetBaslerCameraListInfo()
        {
            List<string> cameralist = new List<string>();
            try
            {
                List<ICameraInfo> allCameraInfos = CameraFinder.Enumerate();
                foreach (ICameraInfo cameraInfo in allCameraInfos)
                {
                    cameralist.Add(cameraInfo[CameraInfoKey.UserDefinedName]);
                }
            }
            catch (Exception e)
            {
                LogFileManager.Error("Camera", e.ToString());
            }
            return cameralist;
        }
        /// <summary>
        /// 海康相机
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHikCameraListInfo()
        {
            List<string> cameralist = new List<string>();
            try
            {
                MyCamera.MV_CC_DEVICE_INFO_LIST stDevList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
                int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE, ref stDevList);
                if (MyCamera.MV_OK != nRet)
                {
                    return cameralist;
                }
                ///设备信息
                MyCamera.MV_CC_DEVICE_INFO stDevInfo;
                for (int i = 0; i < stDevList.nDeviceNum; i++)
                {
                    stDevInfo = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                    if (MyCamera.MV_GIGE_DEVICE == stDevInfo.nTLayerType)
                    {
                        MyCamera.MV_GIGE_DEVICE_INFO stGigEDeviceInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                        if (!string.IsNullOrEmpty(stGigEDeviceInfo.chUserDefinedName))
                        {
                            cameralist.Add(stGigEDeviceInfo.chUserDefinedName);
                        }
                    }
                    else if (MyCamera.MV_USB_DEVICE == stDevInfo.nTLayerType)
                    {
                        MyCamera.MV_USB3_DEVICE_INFO stUsb3DeviceInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                        if (!string.IsNullOrEmpty(stUsb3DeviceInfo.chUserDefinedName))
                        {
                            cameralist.Add(stUsb3DeviceInfo.chUserDefinedName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileManager.Error("Camera", e.ToString());
            }
            return cameralist;
        }
    }
}
