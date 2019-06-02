using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionDisplayTool.DisplayContents
{
   public class CrossLineContent:ContentBase
    {
        public double VerticalLineStartRow { get; set; } = 0;
        public double VerticalLineStartCol { get; set; } = 0;
        public double VerticalLineEndRow { get; set; } = 0;
        public double VerticalLineEndCol { get; set; } = 0;

        public double HorizontalLineStartRow { get; set; } = 0;
        public double HorizontalLineStartCol { get; set; } = 0;
        public double HorizontalLineEndRow { get; set; } = 0;
        public double HorizontalLineEndCol { get; set; } = 0;


        public CrossLineContent()
        {

        }
        public CrossLineContent(string name, HWindow window):base(name,window)
        {

        }
        public CrossLineContent(string name, HWindow window, double imageWidth, double imageHeight, string color = "green", int lineWidth = 1) :base(name,window)
        {
            VerticalLineStartRow = 0;
            VerticalLineStartCol = imageWidth / 2;
            VerticalLineEndRow = imageHeight;
            VerticalLineEndCol = imageWidth / 2;

            HorizontalLineStartRow = imageHeight / 2;
            HorizontalLineStartCol = 0;
            HorizontalLineEndRow = imageHeight / 2;
            HorizontalLineEndCol = imageWidth;
        }
        /// <summary>
        /// 指定八个参数创建十字线
        /// </summary>
        /// <param name="name"></param>
        /// <param name="window"></param>
        /// <param name="row1">第一条直线起始点</param>
        /// <param name="col1">第一条直线起始点</param>
        /// <param name="row2">第一条直线结束点</param>
        /// <param name="col2">第一条直线结束点</param>
        /// <param name="row3">第二条直线起始点</param>
        /// <param name="col3">第二条直线结束点</param>
        /// <param name="row4">第二条直线结束点</param>
        /// <param name="col4">第二条直线起始点</param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        public CrossLineContent(string name, HWindow window, double row1, double col1, double row2, double col2, double row3, double col3, double row4, double col4, string color = "green", int lineWidth = 1) : base(name, window)
        {
            VerticalLineStartRow = row1;
            VerticalLineStartCol = col1;
            VerticalLineEndRow = row2;
            VerticalLineEndCol = col2;

            HorizontalLineStartRow = row3;
            HorizontalLineStartCol = col3;
            HorizontalLineEndRow = row4;
            HorizontalLineEndCol = col4;
        }

        public override void Display()
        {
            try
            {
                base.SetDefaultSetting();
                this.Window.DispLine(VerticalLineStartRow, VerticalLineStartCol, VerticalLineEndRow, VerticalLineEndCol);
                this.Window.DispLine(HorizontalLineStartRow, HorizontalLineStartCol, HorizontalLineEndRow, HorizontalLineEndCol);
                //Display the cross on the Intersection of two line 
                HTuple Row, Col, IsOverlapping;
                HOperatorSet.IntersectionLines(new HTuple(VerticalLineStartRow), new HTuple(VerticalLineStartCol), new HTuple(VerticalLineEndRow), new HTuple(VerticalLineEndCol),
                    new HTuple(HorizontalLineStartRow), new HTuple(HorizontalLineStartCol), new HTuple(HorizontalLineEndRow), new HTuple(HorizontalLineEndCol), out Row, out Col, out IsOverlapping);
                if (Row.Length < 1 && Col.Length < 1)
                {
                    return;
                }
                this.Window.DispCross(Row.D, Col.D, 40, 0.0);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString() + "\r\n");
            }
        }
    }
}
