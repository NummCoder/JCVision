using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;
using VisionUtil.EnumStrucks;
using VisionUtil.GraphParameter;


namespace VisionGrayMatchTool
{
    public class GrayMatchToolInfo : IToolInfo
    {
        public string ToolName
        {
            get;
            set;
        }
        public string TaskName { get; set; }
        [Category("寻找模板开始角度")]
        public double AngleStart { get; set; }
        [Category("寻找模板最大角度")]
        public double AngleExtent { get; set; }
        [Category("寻找模板角度步长")]
        public double AngleStep { get; set; }

        [Category("模板目标数量")]
        public int NumberMacths { get; set; }

        [Category("像素点优化"), ReadOnly(true)]
        public Optimization _Optimization { get; set; } = Optimization.none;

        public string SubPixel { get; set; } = "least_squares";

        [Category("金字塔层数"), ReadOnly(true)]
        public string NumLevels { get; set; } = "4";
        [Category("模板灰度值")]
        public GrayValues grayValues { get; set; } = GrayValues.original;
        [Category("模板最大误差")]
        public double MaxError { get; set; } = 20.0;
       
        [Category("模板初始中心坐标"), ReadOnly(true)]
        public double ModelRegionRow { get; set; }
        [Category("模板初始中心坐标"), ReadOnly(true)]
        public double ModelRegionCol { get; set; }
        [Category("模板初始角度"), ReadOnly(true)]
        public double ModelRegionAngle { get; set; } = 0;
        [Category("显示搜索框"), ReadOnly(true)]
        public bool IsShowFindRegion { get; set; }
        [Category("显示模板框"), ReadOnly(true)]
        public bool IsShowModelRegion { get; set; }
        [Category("显示模板轮廓"), ReadOnly(true)]
        public bool IsShowModelXLD { get; set; }
        [Category("模板结果X坐标"), ReadOnly(true)]
        public double ResultX { get; set; }
        [Category("模板结果Y坐标"), ReadOnly(true)]
        public double ResultY { get; set; }
        [Category("模板结果角度"), ReadOnly(true)]
        public double ResultAngle { get; set; }

        public GraphParamBase ModelROIParam;

        public GraphParamBase FindModelROIParam;
        public IToolInfo CopyInfo()
        {
            return new GrayMatchToolInfo()
            {
                ToolName = this.ToolName,
                AngleStart = this.AngleStart,
                AngleExtent = this.AngleExtent,
                NumberMacths = this.NumberMacths,
                NumLevels = this.NumLevels,
                ModelROIParam = this.ModelROIParam
            };
        }
        public GrayMatchToolInfo()
        {
            NumLevels = "4";
            AngleStart = -10;
            AngleExtent = 20;
            NumberMacths = 1;
            AngleStep = 0.5;
        }
        public GrayMatchToolInfo(string toolName)
        {
            ToolName = toolName;
            NumLevels = "4";
            AngleStart = -10;
            AngleExtent = 20;
            NumberMacths = 1;
            AngleStep = 0.5;

        }

        public override string ToString()
        {
            return "灰度模板匹配工具";
        }

        public string GetToolType()
        {
            return "灰度匹配工具";
        }

        public string GetToolNameSpace()
        {
            return "VisionGrayMatchTool";
        }

        public string GetToolClassName()
        {
            return "GrayMatchTool";
        }
    }
}
