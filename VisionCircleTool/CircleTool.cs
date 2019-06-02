using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionDisplayTool;
using VisionInterface;
using VisionUtil;
using VisionUtil.NLog;

namespace VisionCircleTool
{
    public class CircleTool : LogBaseClass, ITool
    {
        public string ToolName { get ; set ; }

        private CircleToolInfo info;
        public IToolInfo Info
        {
            get { return info; }
            set
            {
                if (value.GetType() == typeof(CircleToolInfo))
                {
                    info = (CircleToolInfo)value;
                }
            }
        }
        public HImage Image { get ; set ; }
        public DisplayControl Window { get; set; }

        public string TaskName { get; set; }


        public CircleTool()
        {

        }
        public CircleTool(IToolInfo info, DisplayControl window)
        {
            this.Info = info;
            this.Window = window;

        }
        public CircleTool(IToolInfo info, HImage image, DisplayControl window)
        {
            this.Info = info;
            this.Window = window;
            this.Image = image;
        }
        public ToolResult GetResult()
        {
            try
            {
                int imageWidt, imageHeight;
                HMetrologyModel metrologyModel = new HMetrologyModel();
                int circleRow = Convert.ToInt32(info.CircleRow), circleCol = Convert.ToInt32(info.CircleCol), circleRadius = Convert.ToInt32(info.CircleRadius);
                //创建找圆工具
                Image.GetImageSize(out imageWidt, out imageHeight);
                metrologyModel.CreateMetrologyModel();
                metrologyModel.SetMetrologyModelImageSize(imageWidt, imageHeight);
                HTuple index = metrologyModel.AddMetrologyObjectCircleMeasure(new HTuple(circleRow), new HTuple(circleCol), new HTuple(circleRadius), new HTuple(info.measure_length1), new HTuple(info.measure_length2), new HTuple(info.measure_sigma), new HTuple(info.measure_threshold), new HTuple(), new HTuple());
                #region 设置参数
                metrologyModel.SetMetrologyObjectParam(index, new HTuple("measure_transition"), new HTuple(info.Measure_transition.ToString()));
                metrologyModel.SetMetrologyObjectParam(index, new HTuple("min_score"), new HTuple(info.min_score));
                metrologyModel.SetMetrologyObjectParam(index, new HTuple("measure_interpolation"), new HTuple(info.measure_interpolation));
                metrologyModel.SetMetrologyObjectParam(index, new HTuple("measure_distance"), new HTuple(info.measure_distance));
                #endregion

                #region 获取轮廓
                HTuple Row;
                HTuple Col;
                HXLDCont modelXLD = metrologyModel.GetMetrologyObjectMeasures(index, "all", out Row, out Col);
                if (info.IsShowCircleMeasureDetail)
                {
                    Window.SetColor("red");
                    modelXLD.DispXld(Window.Window);
                } 
                #endregion
                return ApplyMetrology(metrologyModel, index, info.ToolName);
            }
            catch (Exception e)
            {

                return new ToolResult() { ResultName = info.ToolName, Errormessage = e.ToString(), GetResultTime = DateTime.Now, IsSuccess = false };
            }
        }
        private ToolResult ApplyMetrology(HMetrologyModel hMetrology, int index, string measureName)
        {
            HiPerfTimer timer = new HiPerfTimer();
            timer.Start();
            try
            {
                hMetrology.ApplyMetrologyModel(Image);
                double circleCenterRow, circleCenterCol, radius;
                HTuple circleResult = hMetrology.GetMetrologyObjectResult(new HTuple( index),new HTuple("all"), new HTuple("result_type"), new HTuple("all_param"));
                HXLDCont circleXLD = hMetrology.GetMetrologyObjectResultContour(new HTuple("all"), new HTuple("all"), new HTuple(1.5));
                HXLDCont modelXLD = hMetrology.GetMetrologyObjectMeasures(index, "all", out HTuple Row, out HTuple Col);

                if (circleResult.Length < 3)
                {
                    return new ToolResult() { ResultName = measureName, Errormessage = "圆形测量失败", GetResultTime = DateTime.Now, IsSuccess = false };
                }
                circleCenterRow = circleResult[0].D;
                circleCenterCol = circleResult[1].D;
                radius = circleResult[2].D;               
                if (info.IsShowResult)
                {
                    Window.SetColor("blue");
                    circleXLD.DispXld(Window.Window);
                }
                if (info.IsShowFindRegion)
                {
                    Window.SetColor("green");
                    Window.DisplayCircle(info.ToolName, info.CircleRow, info.CircleCol, info.CircleRadius, true);
                }
                hMetrology.ClearMetrologyObject(new HTuple(index));
                return new ToolResult() { ResultName = measureName, ImageX = circleCenterCol, ImageY = circleCenterRow, ImageRadius = radius, GetResultTime = DateTime.Now, ElapsedTime = timer.Duration, IsSuccess = true };
            }
            catch (Exception e)
            {
                WriteErrorLog("VisionTool", e.ToString());
                return new ToolResult() { ResultName = measureName, Errormessage = e.ToString(), GetResultTime = DateTime.Now, IsSuccess = false };
            }
        }
        public void SetImage(HImage image)
        {
            this.Image = image;
        }

        public void SetWindow(DisplayControl window)
        {
            this.Window=window;
        }
    }
}
