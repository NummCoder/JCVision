using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;

namespace VisionGrabImageTool
{
    public class GrabImageToolInfo : IToolInfo
    {
        public string ToolName { get ; set ; }

        public string TaskName { get; set; }
        public string CameraName { get; set; }
        public IToolInfo CopyInfo()
        {
            return new GrabImageToolInfo() { ToolName=this.ToolName,CameraName=this.CameraName};
        }

        public string GetToolType()
        {
            return "取图工具";
        }

        public string GetToolNameSpace()
        {
            return "VisionGrabImageTool";
        }

        public string GetToolClassName()
        {
            return "GrabImageTool";
        }

        public GrabImageToolInfo()
        {

        }
        public GrabImageToolInfo(string toolName,string cameraName)
        {
            ToolName = toolName;
            CameraName = cameraName;
        }
    }
}
