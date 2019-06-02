using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionUtil.EnumStrucks;

namespace VisionUtil.GraphParameter
{
    /// <summary>
    /// 图形参数基类
    /// </summary>
   public class GraphParamBase
    {
        public string GraphName { get; set; }
        public ROI type { get; set; }
    }
    public class LineParam:GraphParamBase
    {
        public LineParam()
        {
            type = ROI.Line;
        }
        public double LineStartRow { get; set; }
        public double LineStartCol { get; set; }
        public double LineEndRow { get; set; }
        public double LineEndCol { get; set; }
    }
    public class CircleParam : GraphParamBase
    {
        public CircleParam()
        {
            type = ROI.Circle;
        }
        public double Radius { get; set; }
 
        public double CircleRow { get; set; }

        public double CircleColumn { get; set; }
    }
    public class Rectangle1Param:GraphParamBase
    {

        #region Create Rectangle1 Region
        public Rectangle1Param()
        {
            type = ROI.Rectangle1;
        }

        public double RectangleStartRow { get; set; }

        public double RectangleStartColumn { get; set; }

        public double RectangleEndRow { get; set; }

        public double RectangleEndColumn { get; set; }

        public double RectanglePhi { get; set; }
        #endregion

    }
    public class Rectangle2Param:GraphParamBase
    {
        #region Create Rectangle2 Region
        public Rectangle2Param()
        {
            type = ROI.Rectangle2;
        }
        public double Rectangle2Length1 { get; set; }

        public double Rectangle2Length2 { get; set; }

        public double Rectangle2CenterRow { get; set; }

        public double Retangle2CenterColumn { get; set; }
        public double Retangle2Angle { get; set; }

        #endregion
    }
    public class EllipseParam:GraphParamBase
    {
        public EllipseParam()
        {
            type = ROI.Ellipse;
        }
        public double EllipseRadius1 { get; set; }

        public double EllipseRadius2 { get; set; }

        public double EllipseCenterRow { get; set; }

        public double EllipseCenterColumn { get; set; }
        public double EllipseAngle { get; set; }
    }
}
