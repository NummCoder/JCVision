using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;
using VisionDisplayTool;
using VisionUtil;
using HalconDotNet;
using System.IO;
using VisionUtil.GraphParameter;
using VisionUtil.CommonHelpers;

namespace VisionShapeMatchTool
{
    public class ShapeMatchTool : LogBaseClass,ITool,IAffine,IMatch
    {

        public HImage Image { get; set; }

        private ShapeMatchToolInfo info;
        public IToolInfo Info
        {
            get { return info; }
            set
            {
                if (value.GetType() == typeof(ShapeMatchToolInfo))
                {
                    info = (ShapeMatchToolInfo)value;
                }
            }
        }
        public DisplayControl Window { get; set; }
        public string ToolName { get; set; }
        public string TaskName { get; set; }
        private HShapeModel ShapeModel;

        private HXLDCont ModelXLD;

        private bool IsSuccess;
        public ShapeMatchTool()
        {

        }
        public ShapeMatchTool(IToolInfo info, HImage image, DisplayControl window)
        {
            this.Info = info;
            this.Window = window;
            this.Image = image;
            this.ToolName = info.ToolName;
            try
            {
                ShapeModel = new HShapeModel();
                ShapeModel = ReadModelFromFile(info.ToolName);
                ModelXLD = ShapeModel.GetShapeModelContours(1);
            }
            catch (Exception e)
            {
                ShapeModel = null;
                WriteErrorLog("VisionTool",e.ToString());
            }
        }

        public ShapeMatchTool(IToolInfo info, DisplayControl window)
        {
            this.Info = info;
            this.Window = window;
            this.ToolName = info.ToolName;
            try
            {
                ShapeModel = new HShapeModel();
                ShapeModel = ReadModelFromFile(info.ToolName);
                ModelXLD = ShapeModel.GetShapeModelContours(1);
            }
            catch (Exception e)
            {
                ShapeModel = null;
                WriteErrorLog("VisionTool", e.ToString());
            }
        }
        private void SaveModelFile(string fileName)
        {
            try
            {
                if (!Directory.Exists(@".//ModelMatchFile/"))
                {
                    Directory.CreateDirectory(@".//ModelMatchFile/");
                }
                if (!File.Exists(@".//ModelMatchFile/" + fileName + ".shm"))
                {
                    this.ShapeModel.WriteShapeModel(@".//ModelMatchFile/" + fileName + ".shm");
                }
                else
                {
                    File.Delete(@".//ModelMatchFile/" + fileName + ".shm");
                    this.ShapeModel.WriteShapeModel(@".//ModelMatchFile/" + fileName + ".shm");
                }
            }
            catch
            {
                WriteErrorLog("VisionTool", fileName + "模板保存失败");

            }
        }
        private HShapeModel ReadModelFromFile(string toolName)
        {
            HShapeModel model = new HShapeModel(); ;
            try
            {
                if (File.Exists(@".//ModelMatchFile/" + toolName + ".shm"))
                {
                    model.ReadShapeModel(@".//ModelMatchFile/" + info.ToolName + ".shm");
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                WriteErrorLog("VisionTool", "模板读取失败");
            }
            return model;
        }
        public ToolResult GetResult()
        {
            string str;
            int width, height;
            HRegion findRegion=null;
            IsSuccess = false;
            HiPerfTimer timer = new HiPerfTimer();
            timer.Start();
            if (Image == null )
            {
                return new ToolResult() { ResultName=info.ToolName,Errormessage="图像采集失败", IsSuccess = false ,GetResultTime=DateTime.Now};
            }
            else if (ShapeModel == null)
            {
                return new ToolResult() { ResultName = info.ToolName, Errormessage = "模板文件没有找到", IsSuccess = false, GetResultTime = DateTime.Now };
            }
            Image.GetImagePointer1(out str, out  width, out  height);
            HImage image2 = new HImage(str, width, height);
            image2 = Image.CopyImage();
            try
            {
                if (info.FindModelROIParam!=null)
                {
                    if (info.FindModelROIParam.GetType() == typeof(CircleParam))
                    {
                        CircleParam circle = info.ModelROIParam as CircleParam;
                        if (circle != null)
                        {
                             findRegion = GenRegionHelper.GenCircleRegion(circle.CircleRow, circle.CircleColumn, circle.Radius);
                        }
                    }
                    else if (info.FindModelROIParam.GetType() == typeof(Rectangle1Param))
                    {
                        Rectangle1Param rectangle1 = info.ModelROIParam as Rectangle1Param;
                        if (rectangle1 != null)
                        {
                            findRegion = GenRegionHelper.GenRectangle1Region(rectangle1.RectangleStartRow, rectangle1.RectangleStartColumn, rectangle1.RectangleEndRow,rectangle1.RectangleEndColumn);
                        }
                    }
                    else if (info.FindModelROIParam.GetType() == typeof(Rectangle2Param))
                    {
                        Rectangle2Param rectangle2 = info.ModelROIParam as Rectangle2Param;
                        if (rectangle2 != null)
                        {
                            findRegion = GenRegionHelper.GenRectangle2Region(rectangle2.Rectangle2CenterRow, rectangle2.Retangle2CenterColumn, rectangle2.Retangle2Angle,rectangle2.Rectangle2Length1,rectangle2.Rectangle2Length2);
                        }
                    }
                    else if (info.FindModelROIParam.GetType() == typeof(EllipseParam))
                    {
                        EllipseParam ellipse = info.ModelROIParam as EllipseParam;
                        if (ellipse != null)
                        {
                            findRegion = GenRegionHelper.GenEllipseRegion(ellipse.EllipseCenterRow, ellipse.EllipseCenterColumn, ellipse.EllipseAngle,ellipse.EllipseRadius1,ellipse.EllipseRadius2);
                        }
                    }
                }
                if (findRegion != null)
                {
                    image2 = image2.ReduceDomain(findRegion);
                }
                HTuple modelRow, modelCol, modelAngle, modelScale, modelScore;
                if (this.ShapeModel!=null)
                {
                    this.ShapeModel.FindScaledShapeModel(image2, TransAngle.AngleToHu(info.AngleStart), TransAngle.AngleToHu(info.AngleExtent)
                                , info.ScaleMin, info.ScaleMax, info.MinScore, info.NumberMacths, info.MaxOverlap, info.SubPixel, info.NumberMacths, info.Greediness, out modelRow, out modelCol, out modelAngle, out modelScale, out modelScore
                                );
                    if (modelScore.Length > info.NumberMacths)
                    {
                        IsSuccess = true;
                        ToolResult result = new ToolResult();
                        result.ImageModelX = modelCol.D;
                        result.ImageModelY = modelRow.D;
                        result.ImageModelAngle = modelAngle.D;
                        result.ResultScore = modelScore.D;
                        result.IsSuccess = true;
                        result.GetResultTime = DateTime.Now;
                        this.info.ResultX = result.ImageModelX;
                        this.info.ResultY = result.ImageModelY;
                        this.info.ResultAngle = result.ImageAngle;
                        result.ElapsedTime = timer.Duration;             
                        return result;
                    }
                    else
                    {
                        return new ToolResult() { Errormessage = "模板查找失败", IsSuccess = false, GetResultTime = DateTime.Now };
                    } 
                }
                else
                {
                    return new ToolResult() { Errormessage="模板尚未创建", IsSuccess = false, GetResultTime = DateTime.Now };
                }
            }
            catch (Exception e)
            {
                WriteErrorLog("VisionTool",e.ToString());
                return new ToolResult() { Errormessage = "模板查找出现错误" };
            }
        }

        public HXLDCont TransContXLD(HXLDCont xldContour)
        {
            try
            {
                HHomMat2D matd = new HHomMat2D();
                matd.VectorAngleToRigid(this.info.ModelRegionRow, this.info.ModelRegionCol, this.info.ModelRegionAngle, this.info.ResultY, this.info.ResultX, this.info.ResultAngle);
                if (IsSuccess)
                {
                    xldContour = matd.AffineTransContourXld(xldContour);
                    return xldContour;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("VisionTool",ToolName + "模板" + ex.ToString());
            }
            return null;
        }

        public HImage TransImage(HImage image)
        {
            try
            {
                HHomMat2D matd = new HHomMat2D();
                matd.VectorAngleToRigid(this.info.ModelRegionRow, this.info.ModelRegionCol, this.info.ModelRegionAngle, this.info.ResultY, this.info.ResultX, this.info.ResultAngle);
                if (IsSuccess)
                {
                    image = matd.AffineTransImage(image, "constant", "false");
                    return image;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("VisionTool", ToolName + "模板" + ex.ToString());
            }
            return null;
        }

        public bool TransPoint(double row, double col, out double outRow, out double outCol)
        {
            try
            {
                HHomMat2D matd = new HHomMat2D();
                matd.VectorAngleToRigid(this.info.ModelRegionRow, this.info.ModelRegionCol, this.info.ModelRegionAngle, this.info.ResultY, this.info.ResultX, this.info.ResultAngle);
                if (IsSuccess)
                {
                    matd.AffineTransPixel(row, col, out outRow, out outCol);
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("VisionTool", ToolName + "模板" + ex.ToString());
            }
            outRow = 0;
            outCol = 0;
            return false;
        }

        public HXLDPoly TransPolygonXLD(HXLDPoly xldPoly)
        {
            try
            {
                HHomMat2D matd = new HHomMat2D();
                matd.VectorAngleToRigid(this.info.ModelRegionRow, this.info.ModelRegionCol, this.info.ModelRegionAngle, this.info.ResultY, this.info.ResultX, this.info.ResultAngle);
                if (IsSuccess)
                {
                    xldPoly = matd.AffineTransPolygonXld(xldPoly);
                    return xldPoly;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("VisionTool", ToolName + "模板" + ex.ToString());
            }
            return null;
        }

        public HRegion TransRegion(HRegion region)
        {
            try
            {
                HHomMat2D matd = new HHomMat2D();
                matd.VectorAngleToRigid(this.info.ModelRegionRow, this.info.ModelRegionCol, this.info.ModelRegionAngle, this.info.ResultY, this.info.ResultX, this.info.ResultAngle);
                if (IsSuccess)
                {
                    region = matd.AffineTransRegion(region, "nearest_neighbor");
                    return region;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("VisionTool", ToolName + "模板" + ex.ToString());
            }
            return null;
        }

        public double GetRefAngle()
        {
            if (this.info!=null)
            {
                return this.info.ModelRegionAngle;
            }
            return 0;
        }

        public double GetRefRowCoordinate()
        {
            if (this.info != null)
            {
                return this.info.ModelRegionRow;
            }
            return 0;
        }

        public double GetRefColCoodinate()
        {
            if (this.info != null)
            {
                return this.info.ModelRegionCol;
            }
            return 0;
        }

        public bool CreateMatchTool()
        {
            HImage modelImage = null;
            HRegion modelRegion=null;
            if (info.ModelROIParam.GetType()==typeof(CircleParam))
            {
                CircleParam circle = info.ModelROIParam as CircleParam;
                if (circle!=null)
                {
                    modelImage = GetModelImageByCircle(Image.CopyImage(),circle.CircleRow,circle.CircleColumn,circle.Radius,out modelRegion);
                }
            }
            else if (info.ModelROIParam.GetType() == typeof(Rectangle1Param))
            {
                Rectangle1Param rectangle1 = info.ModelROIParam as Rectangle1Param;
                if (rectangle1 != null)
                {
                    modelImage = GetModelImageByRectangle1(Image.CopyImage(), rectangle1.RectangleStartRow, rectangle1.RectangleStartColumn, rectangle1.RectangleEndRow,rectangle1.RectangleEndColumn, out modelRegion);
                }
            }
            else if (info.ModelROIParam.GetType() == typeof(Rectangle2Param))
            {
                Rectangle2Param rectangle2 = info.ModelROIParam as Rectangle2Param;
                if (rectangle2 != null)
                {
                    modelImage = GetModelImageByRectangle2(Image.CopyImage(), rectangle2.Rectangle2CenterRow, rectangle2.Retangle2CenterColumn, rectangle2.Retangle2Angle, rectangle2.Rectangle2Length1,rectangle2.Rectangle2Length2, out modelRegion);
                }
            }
            else if (info.ModelROIParam.GetType() == typeof(EllipseParam))
            {
                EllipseParam ellipse = info.ModelROIParam as EllipseParam;
                if (ellipse != null)
                {
                    modelImage = GetModelImageByEllipse(Image.CopyImage(), ellipse.EllipseCenterRow, ellipse.EllipseCenterColumn, ellipse.EllipseAngle, ellipse.EllipseRadius1, ellipse.EllipseRadius2, out modelRegion);
                }
            }
            else
            {
                WriteErrorLog("VisionTool", info.ToolName + "模板创建失败");
                return false;
            }
            HShapeModel shapeScaleModel = new HShapeModel();
            double modelRow, modelCol;
            if (modelImage!=null)
            {
                try
                {
                    shapeScaleModel.CreateScaledShapeModel(modelImage, new HTuple(info.NumLevels),TransAngle.AngleToHu(info.AngleStart), TransAngle.AngleToHu(info.AngleExtent), new HTuple(info.AngleStep), info.ScaleMin, info.ScaleMax, new HTuple(info.ScaleStep), new HTuple(info._Optimization.ToString()), info._Metric.ToString(), new HTuple(info.Contrast), new HTuple(info.MinContrast));
                    AffineTransModelContour(shapeScaleModel, modelRegion);
                    //record the model coordinate to the setting info.
                    modelRegion.AreaCenter(out modelRow,out modelCol);
                    info.ModelRegionRow = modelRow;
                    info.ModelRegionCol = modelCol;
                    info.ModelRegionAngle = 0.0;

                    this.ShapeModel = shapeScaleModel;
                    this.ModelXLD = shapeScaleModel.GetShapeModelContours(1);
                    SaveModelFile(info.ToolName);
                }
                catch
                {
                    WriteErrorLog("VisionTool", info.ToolName + "模板创建失败");
                    return false;
                }
            }
            else
            {
                WriteErrorLog("VisionTool", info.ToolName + "模板创建失败");
                return false;
            }
            return true;
        }
        private void AffineTransModelContour(HShapeModel model, HRegion modelRegion)
        {
            if (model != null && modelRegion != null)
            {
                try
                {
                    //获取金字塔第一层轮廓
                    HXLDCont modelContour = model.GetShapeModelContours(1);
                    double num3, num4;
                    modelRegion.AreaCenter(out num3, out num4);
                    HHomMat2D matd = new HHomMat2D();
                    matd.VectorAngleToRigid(0.0, 0.0, 0.0, num3, num4, 0.0);
                    HXLD affterAffineModelXLD = matd.AffineTransContourXld(modelContour);
                    if (info.IsShowModelXLD)
                    {
                        Window.SetColor("green");
                        affterAffineModelXLD.DispXld(Window.Window);
                    }
                }
                catch (Exception e)
                {
                    WriteErrorLog("VisionTool", e.ToString());
                }
            }

        }

        private HImage GetModelImageByCircle(HImage inputImage, double circleCenterRow, double circleCenterCol, double circleRadius, out HRegion modelregion)
        {
            HImage rtnImage = null;
            try
            {
                HRegion circleRegion = GenRegionHelper.GenCircleRegion(circleCenterRow, circleCenterCol, circleRadius);
                rtnImage = inputImage.ReduceDomain(circleRegion);
                modelregion = circleRegion;
                return rtnImage;
            }
            catch (Exception)
            {
                modelregion = null;
                return rtnImage;
            }
        }
        
        private HImage GetModelImageByRectangle1(HImage inputImage, double row1, double col1, double row2, double col2, out HRegion modelregion)
        {
            HImage rtnImage = null;
            try
            {
                HRegion rectangleRegion = GenRegionHelper.GenRectangle1Region(row1,col1,row2,col2);
                rtnImage = inputImage.ReduceDomain(rectangleRegion);
                modelregion = rectangleRegion;
                return rtnImage;
            }
            catch (Exception)
            {
                modelregion = null;
                return rtnImage;
            }
        }
        private HImage GetModelImageByRectangle2(HImage inputImage, double centerRow, double centerCol, double angle, double length1, double length2, out HRegion modelregion)
        {
            HImage rtnImage = null;
            try
            {
                HRegion rectangleRegion = GenRegionHelper.GenRectangle2Region(centerRow, centerCol, angle, length1, length2);
                rtnImage = inputImage.ReduceDomain(rectangleRegion);
                modelregion = rectangleRegion;
                return rtnImage;
            }
            catch (Exception)
            {
                modelregion = null;
                return rtnImage;
            }
        }
        private HImage GetModelImageByEllipse(HImage inputImage, double centerRow, double centerCol, double angle, double radius1, double radius2, out HRegion modelregion)
        {
            HImage rtnImage = null;
            try
            {
                HRegion rectangleRegion = GenRegionHelper.GenEllipseRegion(centerRow, centerCol, angle, radius1, radius2);
                rtnImage = inputImage.ReduceDomain(rectangleRegion);
                modelregion = rectangleRegion;
                return rtnImage;
            }
            catch (Exception)
            {
                modelregion = null;
                return rtnImage;
            }
        }

        public void SetImage(HImage image)
        {
           this.Image=image;
        }

        public void SetWindow(DisplayControl window)
        {
            this.Window=window;
        }
    }
}
