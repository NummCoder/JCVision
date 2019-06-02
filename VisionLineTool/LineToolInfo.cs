using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;
using VisionUtil.EnumStrucks;

namespace VisionLineTool
{
    public class LineToolInfo: IToolInfo
    {
        #region properties
        [Category("Measure parameters")]
        public string measure_length1 { get; set; }
        [Category("Measure parameters")]
        public string measure_length2 { get; set; }
        [Category("Measure parameters")]
        public int measure_distance { get; set; }
        [Category("Measure parameters")]
        public int num_measures { get; set; }
        [Category("Measure parameters")]
        public int measure_sigma { get; set; }
        [Category("Measure parameters")]
        public int measure_threshold { get; set; }

        [Category("Measure parameters")]
        public Translation measure_transition { get; set; }
        [Category("Measure parameters")]
        public int num_instances { get; set; }

        [Category("显示搜索框")]
        public bool IsShowFindRegion { get; set; }
        [Category("显示直线细节")]
        public bool IsShowLineMeasureDetail { get; set; }
        [Category("显示结果")]
        public bool IsShowResult { get; set; }


        [Category("测量矩形中心行坐标"),ReadOnly(true)]
        public double Rectangle2Row { get; set; }
        [Category("测量矩形中心列坐标"), ReadOnly(true)]
        public double Rectangle2Col { get; set; }
        [Category("测量矩形中心半宽"), ReadOnly(true)]
        public double Rectangle2Length1{ get; set; }
        [Category("测量矩形中心角度"), ReadOnly(true)]
        public double Rectangle2Angle { get; set; }
        [Category("测量矩形中心半高"), ReadOnly(true)]
        public double Rectangle2Length2 { get; set; }
        [ReadOnly(true),Browsable(false)]
        public string CameraName { get; set; }
        [Category("追随工具名称"), ReadOnly(true)]
        public string TracToolName { get; set; }
        public string ToolName
        {
            get;
            set;
        }
        public string TaskName { get; set; }
        #endregion
        public LineToolInfo(string toolInfoName)
        {
            measure_length1 = "20";
            measure_length2 = "5";
            measure_distance = 10;
            measure_transition = Translation.all;
            measure_threshold = 30;
            measure_sigma = 1;
            num_instances = 40;
            num_measures = 50;
            ToolName = toolInfoName;
            
        }
        public LineToolInfo()
        {
            measure_length1 = "20";
            measure_length2 = "5";
            measure_distance = 10;
            measure_transition = Translation.all;
            measure_threshold = 30;
            measure_sigma = 1;
            num_instances = 40;
            num_measures = 50;
        }

        public IToolInfo CopyInfo()
        {
            return new LineToolInfo()
            {
                measure_length1 = this.measure_length1,
                measure_length2 = this.measure_length2,
                measure_distance = this.measure_distance,
                measure_transition = this.measure_transition,
                measure_threshold = this.measure_threshold,
                measure_sigma = this.measure_sigma,
                num_instances = this.num_instances,
                num_measures = this.num_measures,
                ToolName = this.ToolName,
                Rectangle2Row=this.Rectangle2Row,
                Rectangle2Col=this.Rectangle2Col,
                Rectangle2Angle=this.Rectangle2Angle,
                Rectangle2Length1=this.Rectangle2Length1,
                Rectangle2Length2=this.Rectangle2Length2,
                CameraName=this.CameraName,
                TracToolName=this.TracToolName
                
            };
        }
        public override string ToString()
        {
            return "直线查找工具";
        }

        public string GetToolType()
        {
            return "找线工具";
        }

        public string GetToolNameSpace()
        {
            return "VisionLineTool";
        }

        public string GetToolClassName()
        {
            return "LineTool";
        }
    }
   
}
