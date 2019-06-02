using System.ComponentModel;
using VisionInterface;
using VisionUtil.GraphParameter;

namespace VisionBlobTool
{
    public class BlobToolInfo : IToolInfo
    {
        public string ToolName { get; set; }
        [Category("使用面积特征")]
        public bool IsUseArea { get; set; }
        [Category("最小面积特征")]
        public double MinArea { get; set; }
        [Category("最大面积特征")]
        public double MaxArea { get; set; }
        [Category("使用宽度特征")]
        public bool IsUseWidth { get; set; }
        [Category("最小宽度特征")]
        public double MinWidth { get; set; }
        [Category("最大宽度特征")]
        public double MaxWidth { get; set; }
        [Category("使用高度特征")]
        public bool IsUseHeight { get; set; }
        [Category("最小高度特征")]
        public double MinHeight { get; set; }
        [Category("最大高度特征")]
        public double MaxHeight { get; set; }
        [Category("使用圆度特征")]
        public bool IsUseCircularity { get; set; }
        [Category("最小圆度特征")]
        public double MinCircularity { get; set; }
        [Category("最大圆度特征")]
        public double MaxCircularity { get; set; }
        [Category("使用矩形度特征")]
        public bool IsUseRetanglarity { get; set; }
        [Category("最小矩形度特征")]
        public double MinRetanglarity { get; set; }
        [Category("最大矩形度特征")]
        public double MaxRetanglarity { get; set; }


        [Category("矩形中心行坐标"), ReadOnly(true)]
        public double Rectangle2Row { get; set; }
        [Category("矩形中心列坐标"), ReadOnly(true)]
        public double Rectangle2Col { get; set; }
        [Category("矩形中心半宽"), ReadOnly(true)]
        public double Rectangle2Length1 { get; set; }
        [Category("矩形中心角度"), ReadOnly(true)]
        public double Rectangle2Angle { get; set; }
        [Category("矩形中心半高"), ReadOnly(true)]
        public double Rectangle2Length2 { get; set; }
        [Category("跟随二值化工具名称"), ReadOnly(true)]
        public string TraceTwoValueToolName { get; set; }

        public string TaskName { get; set; }
        public IToolInfo CopyInfo()
        {
            return new BlobToolInfo()
            {
                ToolName = this.ToolName
                ,
                MinArea = this.MinArea,
                MaxArea = this.MaxArea,
                MinWidth = this.MinWidth,
                MaxWidth = this.MaxWidth,
                MinHeight = this.MinHeight,
                MaxHeight = this.MaxHeight,
                MinCircularity = this.MinCircularity,
                MaxCircularity = this.MaxCircularity,
                MinRetanglarity = this.MinRetanglarity,
                MaxRetanglarity = this.MaxRetanglarity,
                IsUseArea = this.IsUseArea,
                IsUseWidth = this.IsUseWidth,
                IsUseHeight = this.IsUseHeight,
                IsUseCircularity = this.IsUseCircularity,
                IsUseRetanglarity = this.IsUseRetanglarity
                ,
                Rectangle2Row = this.Rectangle2Row,
                Rectangle2Col = this.Rectangle2Col,
                Rectangle2Angle = this.Rectangle2Angle,
                Rectangle2Length1 = this.Rectangle2Length1,
                Rectangle2Length2 = this.Rectangle2Length2,
                TraceTwoValueToolName = this.TraceTwoValueToolName
            };
        }

        public string GetToolType()
        {
            return "斑点工具";
        }

        public string GetToolNameSpace()
        {
            return "VisionBlobTool";
        }

        public string GetToolClassName()
        {
            return "BlobTool";
        }
    }
}
