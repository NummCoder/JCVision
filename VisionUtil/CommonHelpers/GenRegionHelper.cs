using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionUtil.CommonHelpers
{
   public static class GenRegionHelper
    {
        public static HRegion GenCircleRegion(double row,double col,double radius)
        {
            HRegion circleRegion = new HRegion();
            circleRegion.GenEmptyRegion();
            circleRegion.GenCircle(row, col, radius);
            return circleRegion;

        }
        public static HRegion GenRectangle1Region(double row1,double col1,double row2,double col2)
        {
            HRegion Rectangle1 = new HRegion();
            Rectangle1.GenEmptyRegion();
            Rectangle1.GenRectangle1(row1, col1, row2,col2);
            return Rectangle1;
        }
        public static HRegion GenRectangle2Region(double row, double col, double angle, double length1,double length2)
        {
            HRegion Rectangle2 = new HRegion();
            Rectangle2.GenEmptyRegion();
            Rectangle2.GenRectangle2(row, col, angle, length1,length2);
            return Rectangle2;
        }
        public static HRegion GenEllipseRegion(double row, double col, double angle, double radius1, double radius2)
        {
            HRegion ellipse = new HRegion();
            ellipse.GenEmptyRegion();
            ellipse.GenEllipse(row, col, angle, radius1, radius2);
            return ellipse;
        }
        public static HRegion GenLineRegion(double startRow, double startCol, double endRow, double endCol)
        {
            HRegion line = new HRegion();
            line.GenEmptyRegion();
            line.GenRegionLine(startRow, startCol, endRow, endCol);
            return line;
        }
    }
}
