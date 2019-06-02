using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;
using VisionUtil.EnumStrucks;

namespace VisionHikCamera
{
    public class HikCameraInfo : IVisionCameraInfo
    {
        public long ImageWidth { get; set; }
        public long ImageHeight { get; set; }
        public long MinExposureTime { get; set; }
        public long MaxExposureTime { get; set; }
        public string CurExposureTime { get; set; }
        public long MinGain { get; set; }
        public long MaxGain { get; set; }
        public string CurGain { get; set; }
        public string CameraIP { get; set; }
        public string CameraMac { get; set; }
        public CameraType _CameraType { get; set; }
        public string UserID { get; set; }
        public string HeartbeatTime { get; set; }  //图像的心跳时间
        public ImageFormat Format { get; set; }    //图像的格式
        public string TriggerDelayTime { get; set; }    //触发拍照延时    
        public string LineDebouncerTime { get; set; }   //防抖延时
        public string OutLineTime { get; set; }        //输出延时
        public HikCameraInfo()
        {
            _CameraType = CameraType.HiK;
        }
        public HikCameraInfo(string camName)
        {
            UserID = camName;
            _CameraType = CameraType.HiK;
        }
    }
}
