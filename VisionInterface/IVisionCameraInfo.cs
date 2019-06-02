using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionUtil.EnumStrucks;

namespace VisionInterface
{
   public interface IVisionCameraInfo
    {
         CameraType _CameraType { get; set; }
         string UserID { get; set; }
         long ImageWidth { get; set; }         // 图像宽
         long ImageHeight { get; set; }        // 图像高
         long MinExposureTime { get; set; }     // 最小曝光时间
         long MaxExposureTime { get; set; }   // 最大曝光时间
         string CurExposureTime { get; set; } 
         long MinGain { get; set; }            // 最小增益
         long MaxGain { get; set; }            // 最大增益
         string CurGain { get; set; } 
         string CameraIP { get; set; } 
         string CameraMac { get; set; } 
         string HeartbeatTime { get; set; }  //图像的心跳时间
         ImageFormat Format { get; set; }    //图像的格式
         string TriggerDelayTime { get; set; }    //触发拍照延时    
         string LineDebouncerTime { get; set; }   //防抖延时
         string OutLineTime { get; set; }        //输出延时

    }
}
