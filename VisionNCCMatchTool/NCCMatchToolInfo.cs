using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionDisplayTool;
using VisionInterface;
using VisionUtil.EnumStrucks;
using VisionUtil.GraphParameter;

namespace VisionNCCMatchTool
{
    public class NCCMatchToolInfo : IToolInfo
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
        [Category("模板重叠度")]
        public double MaxOverlap { get; set; }
        [Category("模板最小分数")]
        public double MinScore { get; set; }

        [Category("使用亚像素"), ReadOnly(true)]
        public string SubPixel { get; set; }
        [Category("极性"), ReadOnly(true)]
        public Metric _Metric { get; set; }
        [Category("金字塔层数"), ReadOnly(true)]
        public string NumLevels { get; set; } = "4";
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
            return new NCCMatchToolInfo()
            {
                ToolName = this.ToolName,
                AngleStart = this.AngleStart,
                AngleExtent = this.AngleExtent,
                AngleStep = this.AngleStep,
                NumberMacths = this.NumberMacths,
                NumLevels = this.NumLevels,
                ModelROIParam = this.ModelROIParam
            };
        }
        public NCCMatchToolInfo()
        {
            NumLevels = "4";
            AngleStart = -10;
            AngleExtent = 20;
            AngleStep = 0.0;
            NumberMacths = 1;
            MaxOverlap = 0.5;
            SubPixel = "least_squares";
            _Metric = Metric.ignore_global_polarity;
        }
        public NCCMatchToolInfo(string toolName)
        {
            ToolName = toolName;
            NumLevels = "4";
            AngleStart = -10;
            AngleExtent = 20;
            AngleStep = 0.0;
            NumberMacths = 1;
            MaxOverlap = 0.5;
            SubPixel = "least_squares";
            _Metric = Metric.ignore_global_polarity;
        }
        public override string ToString()
        {
            return "相关性模板匹配工具";
        }

        public string GetToolType()
        {
            return "相关性匹配工具";
        }

        public string GetToolNameSpace()
        {
            return "VisionNCCMatchTool";
        }

        public string GetToolClassName()
        {
            return "NCCMatchTool";
        }
    }
}
