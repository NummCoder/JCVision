using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionDisplayTool.DisplayContents
{
   public class CircleContent:ContentBase
    {
        public double Row { get; set; } = 0.0;
        public double Col { get; set; } = 0.0;

        public double Radius { get; set; } = 5;

        public CircleContent()
        {

        }
        public CircleContent(string name, HWindow window):base(name,window)
        {

        }
        public CircleContent(string name, HWindow window, double row, double col, double radius, string color = "green", int lineWidth = 1) :base(name,window)
        {
            this.Row = row;
            this.Col = col;
            this.Radius = radius;
        }

        public override void Display()
        {
            try
            {
                base.SetDefaultSetting();
                this.Window.DispCircle(Row, Col, Radius);
            }
            catch (Exception ex)
            {

                WriteErrorLog(ex.ToString() + "\r\n");
            }
        }
    }
}
