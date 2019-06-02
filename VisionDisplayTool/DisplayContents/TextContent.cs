using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionDisplayTool.DisplayContents
{
   public class TextContent:ContentBase
    {
        #region Propeyties
        public int DisplayRow { get; set; } = 0;
        public int DisplayCol { get; set; } = 0;

        public string FontSize { get; set; }
        /// <summary>
        /// Decription message
        /// </summary>
        public string DisplayMessage { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string DisplayHeadMessage { get; set; } = string.Empty;

        public string DisplayEndMessage { get; set; } = string.Empty;
        #endregion

        public TextContent()
        {

        }
        public TextContent(string name, HWindow hwindow) : base(name,hwindow)
        {

        }
        public TextContent(string name, HWindow hwindow, int row, int col) : base(name, hwindow)
        {

            this.DisplayRow = row;
            this.DisplayCol = col;
        }

        public TextContent(string name, HWindow hwindow, int row, int col, string displayMessage, string color = "green", string font = "15") : base(name, hwindow)
        {

            this.DisplayRow = row;
            this.DisplayCol = col;
            this.DisplayColor = color;
            this.FontSize = font;
            this.DisplayMessage = displayMessage;
            this.DisplayColor = color;
            this.FontSize = font;
        }
        public override void Display()
        {
            if (!string.IsNullOrEmpty(DisplayHeadMessage))
            {
                DisplayMessage = DisplayHeadMessage + DisplayMessage;
            }
            if (!string.IsNullOrEmpty(DisplayEndMessage))
            {
                DisplayMessage = DisplayMessage + DisplayEndMessage;
            }
            try
            {
                this.Window.SetColor(this.DisplayColor);
                this.Window.SetTposition(this.DisplayRow, this.DisplayCol);
                this.Window.SetFont("-Courier New -" + this.FontSize + " - *-*-*-*-1 -");
                this.Window.WriteString(this.DisplayMessage);
                this.Window.NewLine();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString() + "\r\n");
            }
        }
    }
}
