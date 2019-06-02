using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionDisplayTool.DisplayContents
{
   public class SingleLineContent:ContentBase
    {
        public double Row1 { get; set; } = 0;
        public double Col1 { get; set; } = 0;
        public double Row2 { get; set; } = 0;
        public double Col2 { get; set; } = 0;

        public SingleLineContent()
        {

        }

        public SingleLineContent(string name, HWindow window, double row1, double col1, double row2, double col2, string color = "green", int lineWidth = 1) :base(name,window)
        {
            this.Row1 = row1;
            this.Col1 = col1;
            this.Row2 = row2;
            this.Col2 = col2;
            this.DisplayColor = color;
            this.DisplayLineWidth = lineWidth;
        }
        public override void Display()
        {
            try
            {
                base.SetDefaultSetting();
                Window.DispLine(Row1, Col1, Row2, Col2);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
    }
}
