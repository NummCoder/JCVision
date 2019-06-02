using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionDisplayTool.DisplayContents
{
   public class Rectangle2Content:ContentBase
    {
        public double CenterRow { get; set; } = 0;
        public double CenterCol { get; set; } = 0;
        public double Angle { get; set; } = 0;
        public double Length1 { get; set; } = 10;
        public double Length2 { get; set; } = 10;

        public Rectangle2Content()
        {

        }
        public Rectangle2Content(string name, HWindow window, double centerRow, double centerCol, double phi, double length1, double length2, string color = "green", int lineWidth = 1) :base(name,window)
        {
            this.CenterRow = centerRow;
            this.CenterCol = centerCol;
            this.Angle = phi;
            this.Length1 = length1;
            this.Length2 = length2;
        }
        public override void Display()
        {
            try
            {
                base.SetDefaultSetting();
                Window.DispRectangle2(CenterRow, CenterCol, Angle, Length1, Length2);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
    }
}
