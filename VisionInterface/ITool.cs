using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionDisplayTool;

namespace VisionInterface
{
   public interface ITool
    {
        string ToolName { get; set; }
        IToolInfo Info { get; set; }
        HImage Image { get; set; }
        DisplayControl Window { get; set; }
        string TaskName { get; set; }
        ToolResult GetResult();
        void SetImage(HImage image);

        void SetWindow(DisplayControl window);
    }
}
