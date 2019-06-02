using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionDisplayTool.DisplayContents
{
   public class EllipseContent:ContentBase
    {
        public int Row { get; set; } = 0;
        public int Col { get; set; } = 0;
        public double Angle { get; set; } = 0.0;
        public double Radius1 { get; set; } = 5;
        public double Radius2 { get; set; } = 5;

        public EllipseContent()
        {

        }
        public EllipseContent(string name, HWindow window) : base(name,window)
        {

        }

        public EllipseContent(string name, HWindow window, int row, int col, double angle, double radius1, double radius2, string color = "green", int lineWidth = 1) : base(name, window)
        {
            this.Row = row;
            this.Col = col;
            this.Angle = angle;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
        }

        public override void Display()
        {
            try
            {
                base.SetDefaultSetting();
                this.Window.DispEllipse(Row, Col, Angle, Radius1, Radius2);
            }
            catch (Exception ex)
            {

                WriteErrorLog(ex.ToString() + "\r\n");
            }
        }
    }
}
