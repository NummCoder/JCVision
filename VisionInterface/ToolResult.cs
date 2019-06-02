using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionInterface
{
    public class ToolResult : IResult
    {
        #region Interface Properties
        public string ErrorCode
        {
            get;
            set;
        }

        public string Errormessage
        {
            get;
            set;

        }

        public bool IsSuccess
        {
            get;
            set;

        }

        public string ResultName
        {
            get;
            set;

        }


        #endregion

        #region Coordinate Properties
       
        /// <summary>
        /// 图像坐标系下的X坐标
        /// </summary>
        public double ImageX { get; set; }
        public double ImageY { get; set; }
        public double ImageAngle { get; set; }

        public double ImageRadius { get; set; }
        /// <summary>
        /// 图像坐标系下的模板X坐标
        /// </summary>
        public double ImageModelX { get; set; }
        public double ImageModelY { get; set; }
        public double ImageModelAngle { get; set; }
        /// <summary>
        /// 图像坐标系下的角度偏差，一般情况下，图像坐标系的角度与世界坐标系的角度相同。
        /// </summary>
        public double ImageAngleOffset { get; set; }
        public double WorldX { get; set; }
        public double WorldY { get; set; }
        public double WorldAngle { get; set; }
        public double WorldModelX { get; set; }
        public double WorldModelY { get; set; }
        public double WorldModelAngle { get; set; }
        public double WorldAngleOffset { get; set; }
        public DateTime GetResultTime { get; set; }
        public double ResultScore { get; set; }
        public double ElapsedTime { get; set; }

        public bool IsUseCalibrate { get; set; }
        #endregion

        #region Image and Region Properties
        /// <summary>
        /// Blob工具或二值化处理后得到的区域
        /// </summary>      
        public HRegion Region { get; set; }
        /// <summary>
        /// 经过滤波或者其他图像预处理工具后得到的图像
        /// </summary>
        public HImage Image { get; set; }
        /// <summary>
        /// 经过轮廓工具处理后得到的亚像素轮廓
        /// </summary>
        public HXLDCont XLD { get; set; }
        #endregion
        public IResult CopyResult()
        {
            return new ToolResult() { ErrorCode = this.ErrorCode,Errormessage=this.Errormessage,IsSuccess=this.IsSuccess,ResultName=this.ResultName };
        }
    }
}
