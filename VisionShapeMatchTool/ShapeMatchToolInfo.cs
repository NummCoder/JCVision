using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;
using VisionDisplayTool;
using VisionUtil;
using VisionUtil.EnumStrucks;
using HalconDotNet;
using System.ComponentModel;
using VisionUtil.GraphParameter;

namespace VisionShapeMatchTool
{
   
    public class ShapeMatchToolInfo : IToolInfo
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
        [Category("模板最小缩放系数")]
        public double ScaleMin { get; set; }
        [Category("模板最大缩放系数")]
        public double ScaleMax { get; set; }
        [Category("模板最小分数")]
        public double MinScore { get; set; }
        [Category("模板目标数量")]
        public int NumberMacths { get; set; }
        [Category("模板重叠度")]
        public double MaxOverlap { get; set; }
        [Category("缩放步距")]
        public string ScaleStep { get; set; }

        [Category("使用亚像素"), ReadOnly(true)]
        public string SubPixel { get; set; }
        [Category("贪婪度"),ReadOnly(true)]
        public double Greediness { get; set; }
        [Category("像素点优化"), ReadOnly(true)]
        public Optimization _Optimization { get; set; }
        [Category("极性"), ReadOnly(true)]
        public Metric _Metric { get; set; }
        [Category("最小尺寸"), ReadOnly(true)]
        public int MinSize { get; set; }
        [Category("对比度")]
        public double Contrast { get; set; }
        [Category("最小对比度")]
        public double MinContrast { get; set; }
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
            return new ShapeMatchToolInfo()
            {
                ToolName=this.ToolName,
                AngleStart = this.AngleStart,
                AngleExtent = this.AngleExtent,
                AngleStep = this.AngleStep,
                ScaleMax = this.ScaleMax,
                ScaleMin = this.ScaleMin,
                ScaleStep = this.ScaleStep,
                Contrast = this.Contrast,
                MinContrast = this.MinContrast,
                MinScore = this.MinScore,
                NumberMacths = this.NumberMacths,
                NumLevels = this.NumLevels,
                ModelROIParam=this.ModelROIParam
            };
        }
        public ShapeMatchToolInfo()
        {
            NumLevels = "4";
            AngleStart = -10;
            AngleExtent = 20;
            AngleStep = 0.0;
            ScaleMin = 0.8;
            ScaleMax = 1.1;
            ScaleStep = "auto";
            MinScore = 0.5;
            NumberMacths = 1;
            MaxOverlap = 0.5;
            SubPixel = "least_squares";
            Greediness = 0.5;
            _Optimization = Optimization.auto;
            _Metric = Metric.ignore_global_polarity;
            Contrast = 40;
            MinContrast = 10;
            MinSize = 20;
        }
        public ShapeMatchToolInfo(string toolName) 
        {
            ToolName = toolName;
            NumLevels = "4";
            AngleStart = -10;
            AngleExtent = 20;
            AngleStep = 0.0;
            ScaleMin = 0.8;
            ScaleMax = 1.1;
            ScaleStep = "auto";
            MinScore = 0.5;
            NumberMacths = 1;
            MaxOverlap = 0.5;
            SubPixel = "least_squares";
            Greediness = 0.5;
            _Optimization = Optimization.auto;
            _Metric = Metric.ignore_global_polarity;
            Contrast = 40;
            MinContrast = 10;
            MinSize = 20;
        }
        public override string ToString()
        {
            return "形状模板匹配工具";
        }

        public string GetToolType()
        {
            return "形状匹配工具";
        }

        public string GetToolNameSpace()
        {
            return "VisionShapeMatchTool";
        }

        public string GetToolClassName()
        {
            return "ShapeMatchTool";
        }
    }
}
