using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionInterface;
using VisionUtil;
using Basler.Pylon;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using VisionUtil.EnumStrucks;
using VisionDisplayTool;

namespace VisionBaslerCamera
{
   public class BaslerCamera:CameraBase
    {
        private Camera camera = null;
        private PixelDataConverter converter = new PixelDataConverter();
        /// <summary>
        /// if >= Sfnc2_0_0,说明是ＵＳＢ３的相机
        /// </summary>
        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        private HImage image;
        public BaslerCamera()
        {
            try
            {
                Image = new HImage();
                camera = new Camera();
                timer = new HiPerfTimer();
                this.UserID = camera.CameraInfo[CameraInfoKey.UserDefinedName];
            }
            catch (Exception e)
            {
                WriteErrorLog(e.ToString());
            }
        }
        public BaslerCamera(IVisionCameraInfo info):base(info) 
        {
            this.UserID = Info.UserID;
            Image = new HImage();
            timer = new HiPerfTimer();
            try
            {
                Image = new HImage();
                // 枚举相机列表，获取固定相机名称
                List<ICameraInfo> allCameraInfos = CameraFinder.Enumerate();
                foreach (ICameraInfo cameraInfo in allCameraInfos)
                {
                    if (this.UserID == cameraInfo[CameraInfoKey.UserDefinedName])
                    {
                        camera = new Camera(cameraInfo);
                        Info.CameraIP = cameraInfo[CameraInfoKey.DeviceIpAddress].ToString();
                        info.CameraMac = cameraInfo[CameraInfoKey.DeviceMacAddress].ToString();                        
                    }
                }
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

        #region 相机操作

        public override bool OpenCamera()
        {
            try
            {
                camera.Open();
                //camera.Parameters[PLCamera.AcquisitionFrameRateEnable].SetValue(true);  // 限制相机帧率
                //camera.Parameters[PLCamera.AcquisitionFrameRateAbs].SetValue(90);
                //camera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(10);          // 设置内存中接收图像缓冲区大小

                Info.ImageWidth = camera.Parameters[PLCamera.Width].GetValue();               // 获取图像宽 
                Info.ImageHeight = camera.Parameters[PLCamera.Height].GetValue();              // 获取图像高
                GetMinExposureTime();
                GetMaxExposureTime();
                GetMaxGain();
                GetMinGain();
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;                      // 注册采集回调函数
                camera.ConnectionLost += OnConnectionLost;
                bConnectOk = true;
                WriteInfoLog(this.UserID + "相机打开成功");
                return true;
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机----" + e.ToString());
            }
            return false;
        }

        public override bool CloseCamera()
        {
            try
            {
                camera.Close();
                camera.Dispose();
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
                WriteErrorLog(UserID + "相机----" + e.ToString());
                return false;
            }
            WriteInfoLog(this.UserID + " 相机关闭成功!");
            return true;
        }

        public override bool GrabOne()
        {
            try
            {
                if (camera.StreamGrabber.IsGrabbing)
                {

                    MessageBox.Show("相机当前正处于采集状态！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    Image.Dispose();
                    Image = null;
                    camera.Parameters[PLCamera.AcquisitionMode].SetValue("SingleFrame");
                    camera.StreamGrabber.Start(1, GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);
                    stopWatch.Restart();    // ****  重启采集时间计时器   ****
                    WriteInfoLog(this.UserID + " Camera Grab Image Once");
                    return true;
                }
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机----" + e.ToString());
                bConnectOk = false;
                return false;
            }
        }

        public override bool StartGrabbing()
        {
            try
            {
                if (camera.StreamGrabber.IsGrabbing)
                {
                    MessageBox.Show("相机当前正处于采集状态！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                    camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);
                    stopWatch.Restart();    // ****  重启采集时间计时器   ****
                    WriteInfoLog(UserID + " 相机开始实时采集!");
                    return true;
                }
            }
            catch
            {
                WriteErrorLog(UserID + "相机打开实时失败 ");
                bConnectOk = false;
                return false;
            }
        }

        public override bool StopGrabbing()
        {
            try
            {
                if (camera.StreamGrabber.IsGrabbing)
                {
                    camera.StreamGrabber.Stop();
                }
            }
            catch
            {
                bConnectOk = false;
                WriteErrorLog(UserID + "相机关闭实时采集失败");
                return false;
            }
            return true;
        }
        private void OnConnectionLost(object sender, EventArgs e)
        {
            try
            {
                camera.Close();

                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        camera.Open();
                        if (camera.IsOpen)
                        {
                            MessageBox.Show("已重新连接上UserID为“" + UserID + "”的相机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        Thread.Sleep(500);
                    }
                    catch
                    {
                        MessageBox.Show("请重新连接UserID为“" + UserID + "”的相机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (camera == null)
                {
                    MessageBox.Show("重连超时20s:未识别到UserID为“" + UserID + "”的相机！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                SetHearBeatTime("5000");
                //camera.Parameters[PLCamera.AcquisitionFrameRateEnable].SetValue(true);  // 限制相机帧率
                //camera.Parameters[PLCamera.AcquisitionFrameRateAbs].SetValue(90);
                //camera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(10);          // 设置内存中接收图像缓冲区大小

                Info.ImageWidth = camera.Parameters[PLCamera.Width].GetValue();               // 获取图像宽 
                Info.ImageHeight = camera.Parameters[PLCamera.Height].GetValue();              // 获取图像高
                GetMinExposureTime();
                GetMaxExposureTime();
                GetMinGain();
                GetMaxGain();
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;                      // 注册采集回调函数
                camera.ConnectionLost += OnConnectionLost;
                bConnectOk = true;

            }
            catch (Exception exception)
            {
                bConnectOk = false;
                WriteErrorLog(exception.ToString());
            }
        }

        private void OnImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.GrabSucceeded)
                {
                    grabTime = stopWatch.ElapsedMilliseconds;
                    base.ProcessGrabTimeCallback(grabTime);

                    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                    // ****  降低显示帧率，减少CPU占用率  **** //
                    //if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)

                    {
                        //stopWatch.Restart();
                        // 判断是否是黑白图片格式
                        if (grabResult.PixelTypeValue == PixelType.Mono8)
                        {
                            //allocate the m_stream_size amount of bytes in non-managed environment 
                            if (latestFrameAddress == IntPtr.Zero)
                            {
                                latestFrameAddress = Marshal.AllocHGlobal((Int32)grabResult.PayloadSize);
                            }
                            converter.OutputPixelFormat = PixelType.Mono8;
                            converter.Convert(latestFrameAddress, grabResult.PayloadSize, grabResult);

                            // 转换为Halcon图像显示
                            Image.GenImage1("byte", grabResult.Width, grabResult.Height, latestFrameAddress);

                        }
                        else if (grabResult.PixelTypeValue == PixelType.BayerBG8 || grabResult.PixelTypeValue == PixelType.BayerGB8
                                    || grabResult.PixelTypeValue == PixelType.BayerRG8 || grabResult.PixelTypeValue == PixelType.BayerGR8)
                        {
                            int imageWidth = grabResult.Width - 1;
                            int imageHeight = grabResult.Height - 1;
                            int payloadSize = imageWidth * imageHeight;

                            //allocate the m_stream_size amount of bytes in non-managed environment 
                            if (latestFrameAddress == IntPtr.Zero)
                            {
                                latestFrameAddress = Marshal.AllocHGlobal((Int32)(3 * payloadSize));
                            }
                            converter.OutputPixelFormat = PixelType.BGR8packed;     // 根据bayer格式不同切换以下代码
                            //converter.OutputPixelFormat = PixelType.RGB8packed;
                            converter.Parameters[PLPixelDataConverter.InconvertibleEdgeHandling].SetValue("Clip");
                            converter.Convert(latestFrameAddress, 3 * payloadSize, grabResult);

                            Image.GenImageInterleaved(latestFrameAddress, "bgr",
                                     imageWidth, imageHeight, -1, "byte", imageWidth, imageHeight, 0, 0, -1, 0);

                        }
                        HImage newImage = Image.CopyImage();
                        image = newImage;
                        // 抛出图像处理事件
                        base.ProcessImageCallBack(newImage);

                        Image.Dispose();
                    }
                }
                else
                {

                    WriteErrorLog("Grab faild!\n" + grabResult.ErrorDescription + " of " + UserID);
                }
            }
            catch
            {

                bConnectOk = false;
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
        }
       
        #endregion

        #region 相机参数设置
        public override bool SetHearBeatTime(string value)
        {
            try
            {
                // 判断是否是网口相机
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    camera.Parameters[PLGigECamera.GevHeartbeatTimeout].SetValue(Convert.ToInt64(value));
                    WriteInfoLog(UserID + "相机设置心跳时间成功");
                    return true;
                }
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机----" + e.ToString());
            }
            return false;
        }
        public override string GetMinExposureTime()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    Info.MinExposureTime = camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
                }
                else
                {
                    Info.MinExposureTime = (long)camera.Parameters[PLUsbCamera.ExposureTime].GetMinimum();
                }
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return Info.MinExposureTime.ToString();
        }
        public override string GetMaxExposureTime()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    Info.MaxExposureTime = camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
                }
                else
                {
                    Info.MaxExposureTime = (long)camera.Parameters[PLUsbCamera.ExposureTime].GetMaximum();
                }
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return Info.MaxExposureTime.ToString();
        }
        public override string GetMinGain()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    Info.MinGain = camera.Parameters[PLCamera.GainRaw].GetMinimum();
                }
                else
                {
                    Info.MinGain = (long)camera.Parameters[PLUsbCamera.Gain].GetMinimum();
                }
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return Info.MinGain.ToString();
        }
        public override string GetMaxGain()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    Info.MaxGain = camera.Parameters[PLCamera.GainRaw].GetMaximum();
                }
                else
                {
                    Info.MaxGain = (long)camera.Parameters[PLUsbCamera.Gain].GetMaximum();
                }
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return Info.MaxGain.ToString();
        }
        public override bool SetExposureTime(string value)
        {
            try
            {
                // Some camera models may have auto functions enabled. To set the ExposureTime value to a specific value,
                // the ExposureAuto function must be disabled first (if ExposureAuto is available).
                camera.Parameters[PLCamera.ExposureAuto].TrySetValue(PLCamera.ExposureAuto.Off); // Set ExposureAuto to Off if it is writable.
                camera.Parameters[PLCamera.ExposureMode].TrySetValue(PLCamera.ExposureMode.Timed); // Set ExposureMode to Timed if it is writable.

                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    // In previous SFNC versions, ExposureTimeRaw is an integer parameter,单位us
                    // integer parameter的数据，设置之前，需要进行有效值整合，否则可能会报错
                    long min = camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
                    long max = camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
                    long incr = camera.Parameters[PLCamera.ExposureTimeRaw].GetIncrement();
                    if (Convert.ToInt64(value) < min)
                    {
                        value = min.ToString();
                    }
                    else if (Convert.ToInt64(value) > max)
                    {
                        value = max.ToString();
                    }
                    else
                    {
                        value = (min + (((Convert.ToInt64(value) - min) / incr) * incr)).ToString();
                    }
                    camera.Parameters[PLCamera.ExposureTimeRaw].SetValue(Convert.ToInt64(value));

                    // Or,here, we let pylon correct the value if needed.
                    //camera.Parameters[PLCamera.ExposureTimeRaw].SetValue(value, IntegerValueCorrection.Nearest);
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // In SFNC 2.0, ExposureTimeRaw is renamed as ExposureTime,is a float parameter, 单位us.
                    camera.Parameters[PLUsbCamera.ExposureTime].SetValue(Convert.ToDouble(value));
                }
                return true;
            }
            catch (Exception e)
            {
                bConnectOk = false;
                WriteErrorLog(UserID + "相机-----" + e.ToString());

            }
            return false;
        }
        public override bool SetGain(string value)
        {
            try
            {
                // Some camera models may have auto functions enabled. To set the gain value to a specific value,
                // the Gain Auto function must be disabled first (if gain auto is available).
                camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off); // Set GainAuto to Off if it is writable.

                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    // Some parameters have restrictions. You can use GetIncrement/GetMinimum/GetMaximum to make sure you set a valid value.                              
                    // In previous SFNC versions, GainRaw is an integer parameter.
                    // integer parameter的数据，设置之前，需要进行有效值整合，否则可能会报错
                    long min = camera.Parameters[PLCamera.GainRaw].GetMinimum();
                    long max = camera.Parameters[PLCamera.GainRaw].GetMaximum();
                    long incr = camera.Parameters[PLCamera.GainRaw].GetIncrement();
                    if (Convert.ToInt64(value) < min)
                    {
                        value = min.ToString();
                    }
                    else if (Convert.ToInt64(value) > max)
                    {
                        value = max.ToString();
                    }
                    else
                    {
                        value = (min + (((Convert.ToInt64(value) - min) / incr) * incr)).ToString();
                    }
                    camera.Parameters[PLCamera.GainRaw].SetValue(Convert.ToInt64(value));

                    //// Or,here, we let pylon correct the value if needed.
                    //camera.Parameters[PLCamera.GainRaw].SetValue(value, IntegerValueCorrection.Nearest);
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // In SFNC 2.0, Gain is a float parameter.
                    camera.Parameters[PLUsbCamera.Gain].SetValue(Convert.ToInt64(value));
                }
                return true;
            }
            catch (Exception e)
            {
                bConnectOk = false;
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return false;
        }
        public override bool SetImageFormat(ImageFormat format)
        {

            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    string oldPixelFormat = camera.Parameters[PLCamera.PixelFormat].GetValue();
                    if (oldPixelFormat != format.ToString())
                    {
                        if (camera.Parameters[PLCamera.PixelFormat].TrySetValue(format.ToString()))
                        {
                            return true;
                        }
                    }
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // In SFNC 2.0, Gain is a float parameter.
                    if (camera.Parameters[PLCamera.PixelFormat].TrySetValue(format.ToString()))
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                bConnectOk = false;
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return false;
        }

        public override bool SetFreerun()
        {
            try
            {
                // Set an enum parameter.
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                    }
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                    }
                }
                stopWatch.Restart();    // ****  重启采集时间计时器   ****
                return true;
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return false;
        }

        public override bool SetSoftwareTrigger()
        {
            try
            {
                // Set an enum parameter.
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                    }
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                    }
                }
                stopWatch.Reset();    // ****  重置采集时间计时器   ****
                return true;
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return false;
        }

        public override bool SendSoftwareExecute()
        {
            try
            {
                if (camera.WaitForFrameTriggerReady(1000, TimeoutHandling.ThrowException))
                {
                    camera.ExecuteSoftwareTrigger();
                    stopWatch.Restart();    // ****  重启采集时间计时器   ****
                    return true;
                }
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return false;
        }

        public override bool SetExternTrigger()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                    }

                    //Sets the trigger delay time in microseconds.
                    camera.Parameters[PLCamera.TriggerDelayAbs].SetValue(5);        // 设置触发延时

                    //Sets the absolute value of the selected line debouncer time in microseconds
                    camera.Parameters[PLCamera.LineSelector].TrySetValue(PLCamera.LineSelector.Line1);
                    camera.Parameters[PLCamera.LineMode].TrySetValue(PLCamera.LineMode.Input);
                    camera.Parameters[PLCamera.LineDebouncerTimeAbs].SetValue(5);       // 设置去抖延时，过滤触发信号干扰

                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                    }

                    //Sets the trigger delay time in microseconds.//float
                    camera.Parameters[PLCamera.TriggerDelay].SetValue(5);       // 设置触发延时

                    //Sets the absolute value of the selected line debouncer time in microseconds
                    camera.Parameters[PLCamera.LineSelector].TrySetValue(PLCamera.LineSelector.Line1);
                    camera.Parameters[PLCamera.LineMode].TrySetValue(PLCamera.LineMode.Input);
                    camera.Parameters[PLCamera.LineDebouncerTime].SetValue(5);       // 设置去抖延时，过滤触发信号干扰

                }
                stopWatch.Reset();    // ****  重置采集时间计时器   ****
                return true;
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return false;
        }

        public override bool SetGrabOverTime(int value)
        {
            if (value > 0)
            {
                grabTime = value;
                return true;
            }
            return false;
        }

        public override string GetCameraIPAddress()
        {
            try
            {
                List<ICameraInfo> allCameras = CameraFinder.Enumerate();
                foreach (ICameraInfo camerainfo in allCameras)
                {
                    if (this.UserID == camerainfo[CameraInfoKey.UserDefinedName])
                    {
                        Info.CameraIP = camerainfo[CameraInfoKey.DeviceIpAddress].ToString();
                        break;
                    }

                }
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return Info.CameraIP;
        }

        public override string GetCameraMacAddress()
        {
            try
            {
                List<ICameraInfo> allCameras = CameraFinder.Enumerate();

                foreach (ICameraInfo camerainfo in allCameras)
                {
                    if (this.UserID == camerainfo[CameraInfoKey.UserDefinedName])
                    {
                        Info.CameraMac = camerainfo[CameraInfoKey.DeviceMacAddress].ToString();
                        break;
                    }

                }
            }
            catch (Exception e)
            {
                WriteErrorLog(UserID + "相机-----" + e.ToString());
            }
            return Info.CameraMac;
        }

        #endregion

        #region 返回图像数据
        public override HImage GetImage()
        {
            if (image!=null)
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
            while (image==null)
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
    }
}
