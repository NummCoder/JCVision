using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionUtil.NLog;

namespace VisionDisplayTool.DisplayContents
{
   public class ContentBase
    {
        public string ContentName { get; set; }

        public HWindow Window { get; set; }

        public string DisplayColor { get; set; } = "Green";

        public int DisplayLineWidth { get; set; } = 1;

        public string DisplayFillMode { get; set; } = "margin";

        public bool bDisplay = true;

        public ContentBase()
        {

        }
        public ContentBase(string name, HWindow window)
        {
            this.ContentName = name;
            this.Window = window;
        }

        public virtual void Display()
        {
            return;
        }
        public virtual void SetDefaultSetting()
        {
            try
            {
                this.Window.SetColor(DisplayColor);
                this.Window.SetLineWidth(DisplayLineWidth);
                this.Window.SetDraw(DisplayFillMode);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString() + "\r\n");
            }
        }
        protected virtual void WriteInfoLog(string infoMessage)
        {
            LogFileManager.Info("Display", infoMessage);
        }

        protected virtual void WriteErrorLog(string errorMessage)
        {
            LogFileManager.Error("Display", errorMessage);
        }

        protected virtual void WriteFatalLog(string fatalMessage)
        {
            LogFileManager.Fatal("Display", fatalMessage);
        }
        protected virtual void WriteDebugLog(string debugMessage)
        {
            LogFileManager.Debug("Display", debugMessage);
        }
    }
}
