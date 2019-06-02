using HalconDotNet;
using MvCamCtrl.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;
using VisionUtil.EnumStrucks;

namespace VisionHikCamera
{

    /// <summary>
    /// 海康相机
    /// </summary>
    public class HikCamera:CameraBase
    {
        #region Properties
        private MyCamera camera { get; set; }
        /// <summary>
        /// 回调函数
        /// </summary>
        private static MyCamera.cbOutputExdelegate ImageCallback;
        private static MyCamera.cbExceptiondelegate CameraReconnectCallback;

        private HImage image { get; set; }
        #endregion

        #region Constructor
        public HikCamera()
        {
            try
            {
                Image = new HImage();
                ImageCallback = new MyCamera.cbOutputExdelegate(ImageCallbackFunc);
                CameraReconnectCallback = new MyCamera.cbExceptiondelegate(ReconnectCallbackFunc);
            }
            catch (Exception e)
            {
                WriteErrorLog(e.ToString());
            }
        }
        public HikCamera(IVisionCameraInfo info) : base(info)
        {
            try
            {
                Image = new HImage();
                camera = new MyCamera();
                ImageCallback = new MyCamera.cbOutputExdelegate(ImageCallbackFunc);
                CameraReconnectCallback = new MyCamera.cbExceptiondelegate(ReconnectCallbackFunc);
                if (camera == null)
                {
                    WriteErrorLog("It can not recognite the camera of  " + UserID);
                }
            }
            catch (Exception e)
            {
                WriteErrorLog(e.ToString());
            }
        }
        #endregion

        #region  相机操作
        public override bool OpenCamera()
        {
            MyCamera.MV_CC_DEVICE_INFO_LIST stDevList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE, ref stDevList);

            if (MyCamera.MV_OK != nRet)
            {
                WriteInfoLog("{this.Info.UserID} Camera Can not find device");
                return false;
            }
            if (0 == stDevList.nDeviceNum)
            {
                return false;
            }

            MyCamera.MV_CC_DEVICE_INFO stDevInfo;                            // 通用设备信息

            // ch:打印设备信息 en:Print device info
            for (Int32 i = 0; i < stDevList.nDeviceNum; i++)
            {
                //赋值结构体的一种方式
                stDevInfo = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));

                //judge camera is gige
                if (MyCamera.MV_GIGE_DEVICE == stDevInfo.nTLayerType)
                {
                    MyCamera.MV_GIGE_DEVICE_INFO stGigEDeviceInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    // ch:显示IP | en:Display IP
                    //UInt32 nIp1 = (stGigEDeviceInfo.nCurrentIp & 0xFF000000) >> 24;
                    //UInt32 nIp2 = (stGigEDeviceInfo.nCurrentIp & 0x00FF0000) >> 16;
                    //UInt32 nIp3 = (stGigEDeviceInfo.nCurrentIp & 0x0000FF00) >> 8;
                    //UInt32 nIp4 = (stGigEDeviceInfo.nCurrentIp & 0x000000FF);
                    //string tempCameraIp = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();

                    if (stGigEDeviceInfo.chUserDefinedName == Info.UserID)
                    {
                        nRet = camera.MV_CC_CreateDevice_NET(ref stDevInfo);
                        if (MyCamera.MV_OK != nRet)
                        {
                            WriteInfoLog($"{this.Info.UserID} Camera Create device failed");
                            return false;
                        }
                        // ch:打开设备 | en:Open device
                        nRet = camera.MV_CC_OpenDevice_NET();
                        if (MyCamera.MV_OK != nRet)
                        {
                            WriteInfoLog($"{this.Info.UserID} Camera Open device failed");
                            return false;
                        }
                        // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                        if (stDevInfo.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                        {
                            int nPacketSize = camera.MV_CC_GetOptimalPacketSize_NET();
                            if (nPacketSize > 0)
                            {
                                nRet = camera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                                if (nRet != MyCamera.MV_OK)
                                {
                                    WriteInfoLog($"{this.Info.UserID} Camera Warning: Set Packet Size failed");
                                }
                            }
                            else
                            {
                                WriteInfoLog($"{this.Info.UserID} Camera Warning: Get Packet Size failed");
                            }
                        }
                        // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
                        camera.MV_CC_SetEnumValue_NET("AcquisitionMode", 2);// ch:工作在连续模式 | en:Acquisition On Continuous Mode
                        camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);    // ch:连续模式 | en:Continuous
                                                                            ///绑定回调函数
                        nRet = camera.MV_CC_RegisterImageCallBackEx_NET(ImageCallback, IntPtr.Zero);

                        if (MyCamera.MV_OK != nRet)
                        {
                            WriteInfoLog($"{this.Info.UserID} Camera Register image callback failed!");
                            return false;
                        }
                        //注册掉线回调函数
                        nRet = camera.MV_CC_RegisterExceptionCallBack_NET(ReconnectCallbackFunc, IntPtr.Zero);
                        if (MyCamera.MV_OK != nRet)
                        {
                            WriteInfoLog($"{this.Info.UserID} Camera Register exception callback failed!");
                            return false;
                        }
                        bConnectOk = true;
                    }
                }
            }
            return true;
        }
        public override bool CloseCamera()
        {
            StopGrabbing();
            // ch:关闭设备 | en:Close device
            int nRet = camera.MV_CC_CloseDevice_NET();
            if (MyCamera.MV_OK != nRet)
            {
                WriteInfoLog($"{this.Info.UserID} camera Close device failed");
                return false;
            }
            // ch:销毁设备 | en:Destroy device
            nRet = camera.MV_CC_DestroyDevice_NET();
            if (MyCamera.MV_OK != nRet)
            {
                WriteInfoLog($"{this.Info.UserID} camera Destroy device failed");
                return false;
            }
            WriteInfoLog($"{this.Info.UserID} camera Close device success!");
            bConnectOk = false;
            return true;
        }
        public override bool GrabOne()
        {
            try
            {
                ///设置为单次触发方式
                camera.MV_CC_SetEnumValue_NET("TriggerMode", 1);

                // ch:开启抓图 | en:start grab
                int nRet = camera.MV_CC_StartGrabbing_NET();
                stopWatch.Restart();
                if (MyCamera.MV_OK != nRet)
                {
                    WriteInfoLog($"{this.Info.UserID} Camera Start grabbing failed:{nRet}");
                    return false;
                }
            }
            catch (Exception e)
            {
                bConnectOk = false;
                WriteErrorLog($"{this.Info.UserID} camera" + e.ToString());
                return false;
            }
            return true;
        }
        public override bool StartGrabbing()
        {
            // ch:开启抓图 | en:start grab
            try
            {
                int nRet = camera.MV_CC_StartGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    WriteInfoLog($"{this.Info.UserID} Camera Start grabbing failed:{nRet}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteInfoLog($"{this.Info.UserID}" + ex.ToString());
                bConnectOk = false;
                return false;
            }
            return true;
        }
        public override bool StopGrabbing()
        {
            try
            {
                int nRet = camera.MV_CC_StopGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    WriteInfoLog($"{this.Info.UserID} Camera Stop grabbing failed{nRet}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteInfoLog($"{this.Info.UserID}" + ex.ToString());
                bConnectOk = false;
                return false;
            }
            return true;
        }
        #endregion


        #region 相机参数设置
        public override string GetMaxExposureTime()
        {
            try
            {
                MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
                int nRet = camera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
                this.Info.MaxExposureTime = Convert.ToInt64(stParam.fMax);
            }
            catch (Exception ex)
            {
                WriteErrorLog($"{this.Info.UserID}" + ex.ToString());
                bConnectOk = false;
            }
            return base.GetMaxExposureTime();
        }
        public override string GetMinExposureTime()
        {
            try
            {
                MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
                int nRet = camera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
                this.Info.MaxExposureTime = Convert.ToInt64(stParam.fMin);
            }
            catch (Exception ex)
            {
                WriteErrorLog($"{this.Info.UserID}" + ex.ToString());
                bConnectOk = false;
            }
            return base.GetMinExposureTime();
        }
        public override string GetMinGain()
        {
            try
            {
                MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
                int nRet = camera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
                this.Info.MaxExposureTime = Convert.ToInt64(stParam.fMin);
            }
            catch (Exception ex)
            {
                WriteErrorLog($"{this.Info.UserID}" + ex.ToString());
                bConnectOk = false;
            }
            return base.GetMinGain();
        }
        public override string GetMaxGain()
        {
            try
            {
                MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
                int nRet = camera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
                this.Info.MaxExposureTime = Convert.ToInt64(stParam.fMax);
            }
            catch (Exception ex)
            {
                WriteErrorLog($"{this.Info.UserID}" + ex.ToString());
                bConnectOk = false;
            }
            return base.GetMaxGain();
        }
        public override bool SendSoftwareExecute()
        {
            try
            {
                // ch:触发源设为软触发 | en:Set trigger source as Software
                camera.MV_CC_SetEnumValue_NET("TriggerSource", 7);
                // ch:触发命令 | en:Trigger command
                int nRet = camera.MV_CC_SetCommandValue_NET("TriggerSoftware");
                if (MyCamera.MV_OK != nRet)
                {
                    WriteInfoLog($"{this.Info.UserID} camera Software trigger failed");
                    return false;
                }
            }
            catch (Exception e)
            {
                WriteErrorLog($"{this.Info.UserID} camera {e.ToString()}");
                bConnectOk = false;
                return false;
            }
            return true;
        }
        public override bool SetExposureTime(string value)
        {

            try
            {
                //关闭自动曝光
                camera.MV_CC_SetEnumValue_NET("ExposureAuto", 0);

                if (Convert.ToInt64(value) > Info.MinExposureTime && Convert.ToInt64(value) < Info.MaxExposureTime)
                {
                    int nRet = camera.MV_CC_SetFloatValue_NET("ExposureTime", float.Parse(value));
                    if (nRet != MyCamera.MV_OK)
                    {
                        WriteInfoLog($"{this.Info.UserID} camera Set Exposure Time Fail!");
                    }
                }
            }
            catch (Exception e)
            {
                WriteErrorLog($"{this.Info.UserID} camera {e.ToString()}");
                bConnectOk = false;
                return false;
            }
            return base.SetExposureTime(value);
        }
        public override bool SetExternTrigger()
        {
            try
            {
                int nRet = camera.MV_CC_SetEnumValue_NET("TriggerMode", 1);
                if (MyCamera.MV_OK != nRet)
                {
                    WriteInfoLog("Set TriggerMode failed!");
                    return false;
                }
                nRet = camera.MV_CC_SetEnumValue_NET("TriggerSource", 1);
                if (MyCamera.MV_OK != nRet)
                {
                    WriteErrorLog("Set TriggerSource failed!");
                    return false;
                }
                nRet = camera.MV_CC_SetTriggerDelay_NET(10);
                if (nRet != MyCamera.MV_OK)
                {
                    WriteErrorLog("Set TriggerDelay failed!");
                }
            }
            catch (Exception e)
            {
                WriteErrorLog($"{this.Info.UserID} camera {e.ToString()}");
                bConnectOk = false;
                return false;
            }
            return base.SetExternTrigger();
        }

        public override bool SetFreerun()
        {
            return base.SetFreerun();
        }
        public override bool SetGain(string value)
        {
            try
            {
                if (Convert.ToInt64(value) > Info.MinGain && Convert.ToInt64(value) < Info.MaxGain)
                {
                    camera.MV_CC_SetEnumValue_NET("GainAuto", 0);
                    int nRet = camera.MV_CC_SetFloatValue_NET("Gain", float.Parse(value));
                    if (nRet != MyCamera.MV_OK)
                    {
                        WriteInfoLog($"{this.Info.UserID} camera Set Gain Fail!");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                WriteErrorLog($"{this.Info.UserID} camera {e.ToString()}");
                bConnectOk = false;
                return false;
            }
            return base.SetGain(value);
        }
        public override bool SetGrabOverTime(int value)
        {
            this.grabTime = value;
            return base.SetGrabOverTime(value);
        }
        public override bool SetHearBeatTime(string value)
        {
            try
            {
                uint heartbeatTime = Convert.ToUInt32(value) > 500 ? Convert.ToUInt32(value) : 500;
                int nRet = camera.MV_CC_SetIntValue_NET("GevHeartbeatTimeout", heartbeatTime);
                if (nRet != MyCamera.MV_OK)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                WriteErrorLog($"{this.Info.UserID} camera {e.ToString()}");
                bConnectOk = false;
                return false;
            }
            return base.SetHearBeatTime(value);
        }
        public override bool SetImageFormat(ImageFormat format)
        {


            try
            {
                if (format == ImageFormat.Mono8)
                {
                    MyCamera.MVCC_ENUMVALUE stEnumValue = new MyCamera.MVCC_ENUMVALUE();
                    int nRet = camera.MV_CC_GetPixelFormat_NET(ref stEnumValue);
                    if (stEnumValue.nCurValue != (uint)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
                    {
                        uint enValue = (uint)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8; nRet = camera.MV_CC_SetPixelFormat_NET(enValue);
                        if (MyCamera.MV_OK != nRet)
                        {
                            WriteInfoLog($"{this.Info.UserID} camera Set PixelFormat failed");
                        }

                    }
                }
                else
                {
                    MyCamera.MVCC_ENUMVALUE stEnumValue = new MyCamera.MVCC_ENUMVALUE();
                    int nRet = camera.MV_CC_GetPixelFormat_NET(ref stEnumValue);
                    if (stEnumValue.nCurValue != (uint)MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed)
                    {
                        uint enValue = (uint)MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
                        nRet = camera.MV_CC_SetPixelFormat_NET(enValue);
                        if (MyCamera.MV_OK != nRet)
                        {
                            WriteInfoLog($"{this.Info.UserID} camera Set PixelFormat failed");
                        }

                    }
                }
            }
            catch (Exception e)
            {
                WriteErrorLog($"{this.Info.UserID} camera {e.ToString()}");
                bConnectOk = false;
                return false;
            }
            return base.SetImageFormat(format);
        }
        public override bool SetSoftwareTrigger()
        {
            try
            {
                // ch:触发源设为软触发 | en:Set trigger source as Software
                camera.MV_CC_SetEnumValue_NET("TriggerSource", 7);
            }
            catch (Exception e)
            {
                WriteErrorLog($"{this.Info.UserID} camera {e.ToString()}");
                bConnectOk = false;
                return false;
            }
            return base.SetSoftwareTrigger();
        }
        public override string ToString()
        {
            return this.Info.UserID;
        }
        #endregion

        #region 返回图像数据
        public override HImage GetImage()
        {
            if (image != null)
            {
                image.Dispose();
                image = null;
            }
            if (!GrabOne())
            {
                WriteErrorLog($"{this.Info.UserID}触发拍照失败");
                return null;
            }
            timer.Start();
            while (image == null)
            {
                if (timer.TimeUp(0.5))
                {
                    WriteErrorLog($"{this.Info.UserID}等待图像超时");
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(10);
                    continue;
                }
            }
            return image;
        }
        #endregion

        #region 回调函数设置
        /// <summary>
        /// 掉线回调函数
        /// </summary>
        /// <param name="nMsgType"></param>
        /// <param name="pUser"></param>
        private void ReconnectCallbackFunc(uint nMsgType, IntPtr pUser)
        {
            WriteErrorLog($"{Info.UserID} camera has occured some mistakes");
            StopGrabbing();
            CloseCamera();
            OpenCamera();
        }

        /// <summary>
        /// 图像回调函数
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pFrameInfo"></param>
        /// <param name="pUser"></param>
        private void ImageCallbackFunc(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            //图像数据数组
            //UInt32 m_nBufSizeForDriver = 4096 * 3000;
            //byte[] m_pBufforDriver = new byte[4096 * 3000];
            //UInt32 m_nBufSizeForSaveImage = 4096 * 3000 * 3 + 3000;
            //byte[] m_pBufferForSaveImage = new byte[4096 * 3000 * 3 + 3000];
            try
            {
                grabTime = stopWatch.ElapsedMilliseconds;
                MyCamera.MvGvspPixelType enDstPixelType;
                if (IsMonoData(pFrameInfo.enPixelType))
                {
                    enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8;
                }
                else if (IsColorData(pFrameInfo.enPixelType))
                {
                    enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
                }
                else
                {
                    return;
                }
                //UInt32 nPayloadSize = 0;
                //MyCamera.MVCC_INTVALUE stParam = new MyCamera.MVCC_INTVALUE();
                //int nRet = camera.MV_CC_GetIntValue_NET("PayloadSize",ref stParam);
                //if (MyCamera.MV_OK!=nRet)
                //{
                //    return;
                //}
                //nPayloadSize = stParam.nCurValue;
                //if (nPayloadSize>m_nBufSizeForDriver)
                //{
                //    m_nBufSizeForDriver = nPayloadSize;
                //    m_pBufforDriver = new byte[m_nBufSizeForDriver];
                //    m_nBufSizeForSaveImage = m_nBufSizeForDriver * 3 + 2048;
                //    m_pBufferForSaveImage = new byte[m_nBufSizeForSaveImage];
                //}
                byte[] m_pBufForSaveImage = new byte[3 * (pFrameInfo.nWidth * pFrameInfo.nHeight) + 2048];
                ///数组字节转换为图像指针
                latestFrameAddress = Marshal.UnsafeAddrOfPinnedArrayElement(m_pBufForSaveImage, 0);
                ///转换图像格式并获取图像的指针地址
                MyCamera.MV_PIXEL_CONVERT_PARAM stConverPixelParam = new MyCamera.MV_PIXEL_CONVERT_PARAM();
                stConverPixelParam.nWidth = pFrameInfo.nWidth;
                stConverPixelParam.nHeight = pFrameInfo.nHeight;
                stConverPixelParam.pSrcData = pData;
                stConverPixelParam.nSrcDataLen = pFrameInfo.nFrameLen;
                stConverPixelParam.enSrcPixelType = pFrameInfo.enPixelType;
                stConverPixelParam.enDstPixelType = enDstPixelType;
                stConverPixelParam.pDstBuffer = latestFrameAddress;
                stConverPixelParam.nDstBufferSize = (uint)(3 * (pFrameInfo.nWidth * pFrameInfo.nHeight) + 2048);
                //赋值图像指针地址
                int nRet = camera.MV_CC_ConvertPixelType_NET(ref stConverPixelParam);
                if (MyCamera.MV_OK != nRet)
                {
                    return;
                }
                if (enDstPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
                {
                    //************************Mono8 转 Himage*******************************
                    // 转换为Halcon图像显示
                    Image.GenImage1("byte", pFrameInfo.nWidth, pFrameInfo.nHeight, latestFrameAddress);

                    GC.Collect();
                }
                else
                {
                    //*********************RGB8 转 HImage**************************
                    //for (int i = 0; i < pFrameInfo.nHeight; i++)
                    //{
                    //    for (int j = 0; j < pFrameInfo.nWidth; j++)
                    //    {
                    //        byte chRed = m_pBufForSaveImage[i * pFrameInfo.nWidth * 3 + j * 3];
                    //        m_pBufForSaveImage[i * pFrameInfo.nWidth * 3 + j * 3] = m_pBufForSaveImage[i * pFrameInfo.nWidth * 3 + j * 3 + 2];
                    //        m_pBufForSaveImage[i * pFrameInfo.nWidth * 3 + j * 3 + 2] = chRed;
                    //    }
                    //}
                    try
                    {
                        int imageWidth = pFrameInfo.nWidth - 1;
                        int imageHeight = pFrameInfo.nHeight - 1;
                        Image.GenImageInterleaved(latestFrameAddress, "bgr",
                                    imageWidth, imageHeight, -1, "byte", imageWidth, imageHeight, 0, 0, -1, 0);
                        GC.Collect();
                    }
                    catch
                    {

                    }

                }
                HImage newImage = Image.CopyImage();
                base.ProcessGrabTimeCallback(grabTime);
                // 抛出图像处理事件
                base.ProcessImageCallBack(newImage);
                Image.Dispose();
            }
            catch (Exception e)
            {
                WriteErrorLog($"{Info.UserID} camera {e.ToString()}");
            }
        }
        #endregion

        #region private methods
        private bool IsMonoData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;

                default:
                    return false;
            }
        }
        /************************************************************************
        *  @fn     IsColorData()
        *  @brief  判断是否是彩色数据
        *  @param  enGvspPixelType         [IN]           像素格式
        *  @return 成功，返回0；错误，返回-1 
        ************************************************************************/
        private bool IsColorData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YCBCR411_8_CBYYCRYY:
                    return true;

                default:
                    return false;
            }
        }
        #endregion
    }
}
