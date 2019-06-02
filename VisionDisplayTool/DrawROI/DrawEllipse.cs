using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionDisplayTool.DisplayContents;

namespace VisionDisplayTool.DrawROI
{
   public class DrawEllipse : DrawBase<double, double, double, double, double>
    {
        public double Angle { get { return Content3; } set { Content3 = value; } }
        public double Col { get { return Content2; } set { Content2 = value; } }
        public double Radius1 { get { return Content4; } set { Content4 = value; } }
        public double Radius2 { get { return Content5; } set { Content5 = value; } }
        public double Row { get { return Content1; } set { Content1 = value; } }

        public DrawEllipse()
        {

        }

        public DrawEllipse(HWindow window, HImage image) :base(window,image)
        {
            if (image != null)
            {
                int width, height;
                image.GetImageSize(out  width, out  height);
                Row = height / 2;
                Col = width / 2;
                Angle = 0;
                Radius1 = 10.0;
                Radius2 = 10.0;
            }
        }
        public DrawEllipse(HWindow window, HImage image, double row, double col, double angle, double radius1, double radius2):base(window,image)
        {
            this.Row = row;
            this.Col = col;
            this.Angle = angle;
            this.Radius1 = radius1;
            this.Radius2 = radius2;
        }

       

        public override void CreateROI()
        {
            lock (lockObj)
            {
                if (Image != null)
                {
                    Window.ClearWindow();
                    HTuple[] values = new HTuple[] { Row, Col, Angle, Radius1, Radius2 };
                    if (drawingObj.ID > -1)
                    {
                        try
                        {
                            Window.DetachBackgroundFromWindow();
                            Window.DetachDrawingObjectFromWindow(drawingObj);
                            drawingObj.Dispose();
                        }
                        catch (Exception ex)
                        {
                            WriteErrorLog(ex.ToString());
                        }
                    }
                    drawingObj = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.ELLIPSE, values);
                    Window.AttachDrawingObjectToWindow(drawingObj);
                    Window.AttachBackgroundToWindow(Image);
                    base.SetDefaultSetting();
                }
            }
        }

        public override void DrawROIComplete()
        {
            string drawingObjectParams = drawingObj.GetDrawingObjectParams("type");
            if (drawingObjectParams == "ellipse")
            {
                try
                {
                    this.Content1 = drawingObj.GetDrawingObjectParams("row").D;
                    this.Content2 = drawingObj.GetDrawingObjectParams("column").D;
                    this.Content3 = drawingObj.GetDrawingObjectParams("angle").D;
                    this.Content4 = drawingObj.GetDrawingObjectParams("radius1").D;
                    this.Content5 = drawingObj.GetDrawingObjectParams("radius2").D;
                    base.RaiseProcessROIParameter(this.Content1, this.Content2, this.Content3, this.Content4, this.Content5);
                    base.DrawROIComplete();
                    Window.DetachDrawingObjectFromWindow(this.drawingObj);
                    EllipseContent ellipse = new EllipseContent("", Window, Convert.ToInt16(Row), Convert.ToInt16(Col), Angle, Radius1, Radius2);
                    ellipse.Display();
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex.ToString());
                }
            }
        }
        /// <summary>
        /// 获取椭圆参数
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="angle"></param>
        /// <param name="radius1"></param>
        /// <param name="radius2"></param>
        public void GetEllipseParameters(out double row, out double col, out double angle, out double radius1, out double radius2)
        {
            row = this.Row;
            col = this.Col;
            angle = this.Angle;
            radius1 = this.Radius1;
            radius2 = this.Radius2;
        }
    }
}
