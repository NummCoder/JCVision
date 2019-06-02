using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionDisplayTool.DisplayContents
{
   public class CrossContent:ContentBase
    {
        public double Col;
        public double Size;
        public double Row;

        public CrossContent()
        {

        }
        public CrossContent(string name, HWindow window):base(name,window)
        {

        }
        public CrossContent(string name, HWindow window, double row, double col, double size, string color = "green", int lineWidth = 1) :base(name,window)
        {
            this.Row = row;
            this.Col = col;
            this.Size = size;

        }

        public override void Display()
        {
            try
            {
                this.Window.DispCross(this.Row, this.Col, this.Size, 0.0);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString() + "\r\n");
            }
        }
    }
}
