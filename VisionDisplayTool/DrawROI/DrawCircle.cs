using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionDisplayTool.DisplayContents;

namespace VisionDisplayTool.DrawROI
{
   public class DrawCircle: DrawBase<int, int, Double, object, object>
    {

        public int Col
        {
            get
            {
                return Content2;
            }
            set
            {
                Content2 = value;
            }
        }
        public double Radius
        {
            get
            {
                return Content3;
            }
            set
            {
                Content3 = value;
            }
        }
        public int Row
        {
            get
            {
                return Content1;
            }
            set
            {
                Content1 = value;
            }
        }
        public DrawCircle()
        {

        }
        public DrawCircle(HWindow window, HImage image) :base(window,image)
        {
            if (image != null)
            {
                int width, height;
                image.GetImageSize(out  width, out height);
                Row = height / 2;
                Col = width / 2;
                Radius = 10.0;
            }
        }

   

        public DrawCircle(HWindow window, HImage image, int circleRow, int circleCol, double radius):base(window,image)
        {
            this.Row = circleRow;
            this.Col = circleCol;
            this.Radius = radius;
        }

        public override void CreateROI()
        {
            lock (lockObj)
            {
                if (Image != null)
                {
                    Window.ClearWindow();
                    HTuple[] values = new HTuple[] { Row, Col, Radius };
                    if (drawingObj.ID>-1)
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
                    drawingObj = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.CIRCLE, values);
                    Window.AttachDrawingObjectToWindow(drawingObj);
                    Window.AttachBackgroundToWindow(Image);
                    base.SetDefaultSetting();
                }
            }
        }
        public override void DrawROIComplete()
        {
            string drawingObjectParams = drawingObj.GetDrawingObjectParams("type");
            if (drawingObjectParams == "circle")
            {
                try
                {
                    this.Content1 = drawingObj.GetDrawingObjectParams("row").I;
                    this.Content2 = drawingObj.GetDrawingObjectParams("column").I;
                    this.Content3 = drawingObj.GetDrawingObjectParams("radius").D;
                    base.RaiseProcessROIParameter(this.Content1, this.Content2, this.Content3, null, null);
                    base.DrawROIComplete();
                    Window.DetachDrawingObjectFromWindow(this.drawingObj);
                    CircleContent circle = new CircleContent("", Window, Convert.ToDouble(Row), Convert.ToDouble(Col), Radius);
                    circle.Display();
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex.ToString());
                }
            }
        }
        /// <summary>
        /// 获取圆参数
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="radius"></param>
        public void GetCircleParameters(out int row, out int col, out double radius)
        {
            row = Row;
            col = Col;
            radius = Radius;
        }
    }
}
