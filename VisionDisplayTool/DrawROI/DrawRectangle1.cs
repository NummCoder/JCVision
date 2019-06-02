using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionDisplayTool.DisplayContents;

namespace VisionDisplayTool.DrawROI
{
   public class DrawRectangle1 : DrawBase<double, double, double, double, object>
    {
        public double Row1 { get { return Content1; }  set { Content1 = value; } }
        public double Col1 { get { return Content2; } set { Content2 = value; } }
        public double Row2 { get { return Content3; } set { Content3 = value; } }
        public double Col2 { get { return Content4; } set { Content4 = value; } }

        public DrawRectangle1()
        {

        }

        public DrawRectangle1(HWindow window, HImage image):base(window,image)
        {
            if (image != null)
            {
                int width, height;
                image.GetImageSize(out  width, out  height);
                Row1 = height / 2;
                Col1 = width / 2;
                Row2 = height / 2 - 50;
                Col2 = width / 2 - 50;
            }
        }

        public DrawRectangle1(HWindow window, HImage image, double row1, double col1, double row2, double col2):base(window,image)
        {
            this.Row1 = row1;
            this.Col1 = col1;
            this.Row2 = row2;
            this.Col2 = col2;
        }

        public override void CreateROI()
        {
            lock (lockObj)
            {
                if (Image != null)
                {
                    Window.ClearWindow();
                    HTuple[] values = new HTuple[] { Row1, Col1, Row2, Col2 };
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
                    drawingObj = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, values);
                    Window.AttachDrawingObjectToWindow(drawingObj);
                    Window.AttachBackgroundToWindow(Image);
                    base.SetDefaultSetting();
                }
            }
        }

        public override void DrawROIComplete()
        {
            string drawingObjectParams = drawingObj.GetDrawingObjectParams("type");
            if (drawingObjectParams == "rectangle1")
            {
                try
                {
                    this.Content1 = drawingObj.GetDrawingObjectParams("row1").D;
                    this.Content2 = drawingObj.GetDrawingObjectParams("column1").D;
                    this.Content3 = drawingObj.GetDrawingObjectParams("row2").D;
                    this.Content4 = drawingObj.GetDrawingObjectParams("column2").D;
                    base.RaiseProcessROIParameter(this.Content1, this.Content2, this.Content3, this.Content4, null);
                    base.DrawROIComplete();
                    Window.DetachDrawingObjectFromWindow(this.drawingObj);
                    Rectangle1Content rectangle1 = new Rectangle1Content("", Window, Row1, Col1, Row2, Col2);
                    rectangle1.Display();
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 获取矩形参数
        /// </summary>      
        public void GetRectangle1Parameters(out double row1, out double col1, out double row2, out double col2)
        {
            row1 = this.Row1;
            row2 = this.Row2;
            col1 = this.Col1;
            col2 = this.Col2;
        }
    }
}
