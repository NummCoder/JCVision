using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionDisplayTool;
using VisionInterface;
using VisionUtil;

namespace VisionGrabImageTool
{
    public class GrabImageTool : ITool
    {
        CameraBase camera;
        public HImage Image { get; set; }
        public DisplayControl Window { get; set; }

        private GrabImageToolInfo info;
        public string TaskName { get; set; }

        public IToolInfo Info
        {
            get { return info; }
            set
            {
                if (value.GetType() == typeof(GrabImageToolInfo))
                {
                    info = (GrabImageToolInfo)value;
                }
            }
        }

        public string ToolName { get; set; }

        public GrabImageTool()
        {

        }
        public GrabImageTool(IToolInfo info, DisplayControl window)
        {
            this.Info = info;
            this.Window = window;

        }
        public GrabImageTool(IToolInfo info, HImage image, DisplayControl window)
        {
            this.Info = info;
            this.Window = window;
            this.Image = image;
        }

        public void SetImage(HImage image)
        {
            this.Image = image;
        }

        public ToolResult GetResult()
        {
            if (info!=null)
            {
                if (!string.IsNullOrEmpty(info.CameraName))
                {
                    //通过相机管理工具获取结果
                }
            }
            throw new NotImplementedException();
        }
        public void SetWindow(DisplayControl window)
        {
            this.Window = window;
        }
    }
}
