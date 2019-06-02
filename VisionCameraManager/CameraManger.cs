using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;
using VisionUtil;
using VisionUtil.EnumStrucks;

namespace VisionCameraManager
{
    /// <summary>
    /// 相机管理类，用于管理相机的初始化以及相机的增删改查
    /// </summary>
   public class CameraManger
    {
        /// <summary>
        /// 存储所有相机
        /// </summary>
        private static Dictionary<string, CameraBase> Cameras { get; set; }

       
        #region files operator 
        private static CameraDoc doc;
        public static void LoadDoc()
        {
            doc = CameraDoc.LoadObj();
        }
        public static void SaveDoc()
        {
            doc.SaveDoc();
        }
        public static List<IVisionCameraInfo> GetCameraInfoList()
        {
            if (doc != null)
            {
                return doc.CamerasInfoList;
            }
            return new List<IVisionCameraInfo>();
        }
        public static IVisionCameraInfo GetCameraInfoInstance(string key)
        {
            if (doc != null)
            {
                if (doc.CamerasInfoDic.ContainsKey(key))
                {
                    return doc.CamerasInfoDic[key];
                }
            }
            return null;
        }
        public static bool AddCameraInfo(string cameraName,string camType)
        {
            if (doc != null)
            {
                if (!doc.CamerasInfoDic.ContainsKey(cameraName))
                {
                    IVisionCameraInfo info = CreatingHelper<IVisionCameraInfo>.CreateInstance(@".//CamerasDll/"+ "Vision" + camType + ".dll","Vision"+camType, camType+"Info");
                    info.UserID = cameraName;
                    doc.CamerasInfoList.Add(info);
                    doc.CamerasInfoDic.Add(info.UserID, info);
                    return true;
                }
            }
            return false;
        }

        public static bool RemoveCameraInfo(string cameraName)
        {
            if (doc != null)
            {
                if (doc.CamerasInfoDic.ContainsKey(cameraName))
                {
                    doc.CamerasInfoList.Remove(doc.CamerasInfoList.Find(p=>p.UserID==cameraName));
                    doc.CamerasInfoDic.Remove(cameraName);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Camera Operator
        public static void InitCamera()
        {
            LoadDoc();
            try
            {
                Cameras = new Dictionary<string, CameraBase>();
                if (doc == null)
                {
                    return;
                }
                if (doc.CamerasInfoList.Count != 0)
                {

                    foreach (var info in doc.CamerasInfoList)
                    {
                        object[] paras = new object[] { info };
                        var camera = CreatingHelper<CameraBase>.CreateInstance(@".//CamerasDll/" + "Vision" + info._CameraType + "Camera" + ".dll", "Vision" + info._CameraType + "Camera", info._CameraType.ToString() + "Camera", paras);
                        if (camera != null)
                        {
                            Cameras.Add(camera.UserID, camera);
                        }
                    }
                }
                else
                {
                    #region 枚举巴斯勒相机
                    List<string> baslerCams = CameraGeneral.GetBaslerCameraListInfo();
                    if (baslerCams.Count > 0)
                    {
                        foreach (var cam in baslerCams)
                        {
                            AddCameraInfo(cam, "Basler");
                            IVisionCameraInfo info = GetCameraInfoInstance(cam);
                            object[] paras = new object[] { info };
                            var camera = CreatingHelper<CameraBase>.CreateInstance(@".//CamerasDll/" + "Vision" + info._CameraType + "Camera" + ".dll", "Vision" + info._CameraType + "Camera", info._CameraType.ToString() + "Camera", paras);
                            if (camera != null)
                            {
                                Cameras.Add(camera.UserID, camera);
                            }
                        }
                    }
                    #endregion

                    #region 枚举海康相机
                    List<string> hikCams = CameraGeneral.GetHikCameraListInfo();
                    if (hikCams.Count > 0)
                    {
                        foreach (var cam in hikCams)
                        {
                            AddCameraInfo(cam, "Hik");
                            IVisionCameraInfo info = GetCameraInfoInstance(cam);
                            object[] paras = new object[] { info };
                            var camera = CreatingHelper<CameraBase>.CreateInstance(@".//CamerasDll/" + "Vision" + info._CameraType + "Camera" + ".dll", "Vision" + info._CameraType + "Camera", info._CameraType.ToString() + "Camera", paras);
                            if (camera != null)
                            {
                                Cameras.Add(camera.UserID, camera);
                            }
                        }

                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex.ToString());
            }
            SaveDoc();
        }
        public static bool OpenCameras()
        {
            bool bOpenCamsOk = true;
            foreach (var cam in Cameras.Values)
            {
                bOpenCamsOk &= cam.OpenCamera();
            }
            return bOpenCamsOk;
        }
        public static bool OpenSingleCam(string name)
        {
            if (!Cameras.ContainsKey(name))
            {
                return false;
            }
            return Cameras[name].OpenCamera();
        }

        public static bool CloseCameras()
        {
            bool bCloseCamsOk = true;
            foreach (var cam in Cameras.Values)
            {
                bCloseCamsOk &= cam.CloseCamera();
            }
            return bCloseCamsOk;
        }

        public static bool CloseSingleCamera(string camName)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].CloseCamera();
                }
            }
            return false;
        }
        public static bool SetCamSoftTrigger(string camName)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].SetSoftwareTrigger();
                }
            }
            return false;
        }

        public static bool GrabOne(string camName)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].GrabOne();
                }
            }
            return false;
        }

        public static bool StartLive(string camName)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].StartGrabbing();
                }
            }
            return false;
        }
        public static bool StopLive(string camName)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].StopGrabbing();
                }
            }
            return false;
        }

        public static void BindingCameraImageProcessEvent(string camName, CameraBase.delegateProcessHImage func, bool bAdd = true)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (bAdd)
                {
                    Cameras[camName].EventProcessImage += func;
                }
                else
                {
                    Cameras[camName].EventProcessImage -= func;
                }
            }
        }
        public static void BindingCameraGrabImageUseTime(string camName, CameraBase.delegateComputeGrabTime func, bool bAdd = true)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (bAdd)
                {
                    Cameras[camName].EventComputeGrabTime += func;
                }
                else
                {
                    Cameras[camName].EventComputeGrabTime -= func;
                }
            }
        }       

        public static HImage GetImage(string camName)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].GetImage();
                }
            }
            return null;
        }

        public static bool SetGain(string camName,string gainValue)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].SetGain(gainValue);
                }
            }
            return false;
        }
        public static bool SetShutter(string camName, string shutterValue)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].SetExposureTime(shutterValue);
                }
            }
            return false;
        }

        public static bool SetExternTrigger(string camName)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].SetExternTrigger();
                }
            }
            return false;
        }
        public static bool SetHearBeatTime(string camName,string value)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].SetHearBeatTime(value);
                }
            }
            return false;
        }
        public static bool SetFreerun(string camName)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].SetFreerun();
                }
            }
            return false;
        }
        public static bool SetImageFormat(string camName,ImageFormat format)
        {
            if (Cameras.ContainsKey(camName))
            {
                if (Cameras[camName].bConnectOk)
                {
                    return Cameras[camName].SetImageFormat(format);
                }
            }
            return false;
        }
        #endregion
    }
}
