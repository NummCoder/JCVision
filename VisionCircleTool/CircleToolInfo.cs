using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;
using VisionUtil.EnumStrucks;

namespace VisionCircleTool
{
   public class CircleToolInfo : IToolInfo
    {
        #region properties
        [Category("Measure parameters")]
        public int measure_length1 { get; set; }
        [Category("Measure parameters")]
        public int measure_length2 { get; set; }
        [Category("Measure parameters")]
        public int measure_distance { get; set; }
        [Category("Measure parameters")]
        public int num_measures { get; set; }
        [Category("Measure parameters")]
        public int measure_sigma { get; set; }
        [Category("Measure parameters")]
        public int measure_threshold { get; set; }
        [Category("Measure parameters")]
        public string measure_select { get; set; }
        [Category("Measure parameters")]
        public Translation Measure_transition { get; set; }
        [Category("Measure parameters")]
        public string measure_interpolation { get; set; }
        [Category("Measure parameters")]
        public double min_score { get; set; }
        [Category("Measure parameters")]
        public int num_instances { get; set; }
        [Category("Measure parameters")]
        public int distance_threshold { get; set; }
        [Category("Measure parameters")]
        public string max_num_iterations { get; set; }
        [Category("Measure parameters")]
        public string rand_seed { get; set; }
        [Category("Measure parameters")]
        public string instances_outside_measure_regions { get; set; }

        [Category("显示搜索框")]
        public bool IsShowFindRegion { get; set; }
        [Category("显示测量圆细节")]
        public bool IsShowCircleMeasureDetail { get; set; }
        [Category("显示结果")]
        public bool IsShowResult { get; set; }

        [Category("测量圆形中心行坐标"), ReadOnly(true)]
        public double CircleRow { get; set; }
        [Category("测量圆形中心列坐标"), ReadOnly(true)]
        public double CircleCol { get; set; }
        [Category("测量圆形半径"), ReadOnly(true)]
        public double CircleRadius { get; set; }

        [Category("工具名称")]
        public string ToolName { get; set ; }
        [Category("追随工具名称")]
        public string TracToolName { get; set; }

        public string TaskName { get; set; }
        #endregion
        public CircleToolInfo(string toolInfoName)
        {
            measure_length1 = 20;
            measure_length2 = 5;
            measure_distance = 10;
            Measure_transition = Translation.all;
            measure_threshold = 30;
            min_score = 0.7;
            measure_sigma = 1;
            num_instances = 40;
            num_measures = 50;
            distance_threshold = 3;
            max_num_iterations = "1";
            rand_seed = "42";
            instances_outside_measure_regions = "false";
            ToolName = toolInfoName;
            measure_select = "all";
            measure_interpolation = "bicubic";
        }
        public CircleToolInfo()
        {
            measure_length1 = 20;
            measure_length2 = 5;
            measure_distance = 10;
            Measure_transition = Translation.all;
            measure_threshold = 30;
            measure_sigma = 1;
            min_score = 0.7;
            num_instances = 40;
            num_measures = 50;
            distance_threshold = 3;
            max_num_iterations = "1";
            rand_seed = "42";
            instances_outside_measure_regions = "false";
            measure_interpolation = "bicubic";
        }

        public IToolInfo CopyInfo()
        {
            return new CircleToolInfo()
            {
                measure_length1 = this.measure_length1,
                measure_length2 = this.measure_length2,
                measure_distance = this.measure_distance,
                Measure_transition = this.Measure_transition,
                measure_threshold = this.measure_threshold,
                measure_sigma = this.measure_sigma,
                num_instances = this.num_instances,
                num_measures = this.num_measures,
                ToolName = this.ToolName,
                TracToolName = this.TracToolName

            };
        }
        public override string ToString()
        {
            return "圆查找工具";
        }

        public string GetToolType()
        {
            return "找圆工具";
        }

        public string GetToolNameSpace()
        {
            return "VisionCircleTool";
        }

        public string GetToolClassName()
        {
            return "CircleTool";
        }
    }
}
