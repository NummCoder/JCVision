using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionDisplayTool;
using VisionInterface;
using VisionUtil;
using VisionUtil.NLog;




namespace VisionLineTool
{
    public class LineTool : LogBaseClass,ITool
    {   
        public HImage Image { get; set; }
        public DisplayControl Window { get; set; }

        private LineToolInfo info;
        public IToolInfo Info
        {
            get { return info; }
            set
            {
                if (value.GetType()==typeof(LineToolInfo))
                {
                    info = (LineToolInfo)value;
                }
            }
        }

        public string ToolName { get; set; }
        public string TaskName { get; set; }
        public LineTool()
        {

        }
        public LineTool(IToolInfo info, DisplayControl window)
        {
            this.Info = info;
            this.Window = window;

        }
        public LineTool(IToolInfo info,HImage image,DisplayControl window)
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
                HTuple startRow, startCol, endRow, endCol;
                HHomMat2D matd = new HHomMat2D();
                HHomMat2D matd3 = matd.HomMat2dRotate(info.Rectangle2Angle, 0.0, 0.0);
                HHomMat2D matd4 = matd3.HomMat2dTranslate(info.Rectangle2Row, info.Rectangle2Col);
                HOperatorSet.AffineTransPoint2d(matd4, new HTuple(0.0), new HTuple(-info.Rectangle2Length1), out startRow, out startCol);
                HOperatorSet.AffineTransPoint2d(matd4, new HTuple(0.0), new HTuple(info.Rectangle2Length1), out endRow, out endCol);
                //创建找线工具
                Image.GetImageSize(out imageWidt, out imageHeight);
                int[] value = new int[] { Convert.ToInt32(startRow.D), Convert.ToInt32(startCol.D), Convert.ToInt32(endRow.D), Convert.ToInt32(endCol.D) };
                HTuple values = new HTuple(value);
                metrologyModel.CreateMetrologyModel();
                metrologyModel.SetMetrologyModelImageSize(imageWidt, imageHeight);
                int index = metrologyModel.AddMetrologyObjectGeneric("line", values, 20, 5, 1, 30, "min_score", 0.6);
                #region 设置参数
                metrologyModel.SetMetrologyObjectParam(new HTuple(index), new HTuple("measure_length1"), new HTuple(Convert.ToInt32(info.measure_length1)));
                metrologyModel.SetMetrologyObjectParam(new HTuple(index), new HTuple("measure_length2"), new HTuple(Convert.ToInt32(info.measure_length2)));
                metrologyModel.SetMetrologyObjectParam(new HTuple(index), new HTuple("measure_distance"), new HTuple(info.measure_distance));
                metrologyModel.SetMetrologyObjectParam(new HTuple(index), new HTuple("num_measures"), new HTuple(info.num_measures));
                metrologyModel.SetMetrologyObjectParam(new HTuple(index), new HTuple("num_instances"), new HTuple(info.num_instances));
                metrologyModel.SetMetrologyObjectParam(new HTuple(index), new HTuple("measure_sigma"), new HTuple(info.measure_sigma));
                metrologyModel.SetMetrologyObjectParam(new HTuple(index), new HTuple("measure_threshold"), new HTuple(info.measure_threshold));
                metrologyModel.SetMetrologyObjectParam(new HTuple(index), new HTuple("measure_transition"), new HTuple(info.measure_transition.ToString()));
                #endregion

                HTuple Row;
                HTuple Col;
                HXLDCont modelXLD = metrologyModel.GetMetrologyObjectMeasures(index, "all", out Row, out Col);
                if (info.IsShowLineMeasureDetail)
                {
                    Window.SetColor("red");
                    modelXLD.DispXld(Window.Window);
                }
                
                return ApplyMetrology(metrologyModel,index,info.ToolName);
            }
            catch (Exception e)
            {

                return new ToolResult() { ResultName=info.ToolName,Errormessage=e.ToString(),GetResultTime=DateTime.Now,IsSuccess=false};
            }
        }
        private ToolResult ApplyMetrology(HMetrologyModel hMetrology, int index, string measureName)
        {
            HiPerfTimer timer = new HiPerfTimer();
            timer.Start();
            try
            {
                //window.DisplayImage(image);
                hMetrology.ApplyMetrologyModel(Image);
                ///获取直线的中心坐标
                ///lineresult 为获取到的直线起点与终点坐标
                double lineCenterRow, lineCenterCol, angle;
                HTuple lineResult = hMetrology.GetMetrologyObjectResult(new HTuple("all"), new HTuple("all"), new HTuple("result_type"), new HTuple("all_param"));

                lineCenterRow = (lineResult[0].D + lineResult[2]).D / 2;
                lineCenterCol = (lineResult[1].D + lineResult[3]).D / 2;
                //获取直线测量矩形
                angle = HMisc.LineOrientation(lineResult[0].D, lineResult[1].D, lineResult[2].D, lineResult[3].D);
                angle = TransAngle.HuToAngle(angle);
                hMetrology.ClearMetrologyObject(new HTuple(index));

                Window.DisplaySingleLine(info.ToolName, lineResult[0].D, lineResult[1].D, lineResult[2].D, lineResult[3].D, true, "blue");
                if (info.IsShowFindRegion)
                {
                    Window.DisplayRectangle2(info.ToolName, info.Rectangle2Row, info.Rectangle2Col, info.Rectangle2Angle, info.Rectangle2Length1, info.Rectangle2Length2, true);
                }
                return new ToolResult() { ResultName = measureName, ImageX = lineCenterCol, ImageY = lineCenterRow, ImageAngle = angle, GetResultTime = DateTime.Now, ElapsedTime = timer.Duration, IsSuccess = true };
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
