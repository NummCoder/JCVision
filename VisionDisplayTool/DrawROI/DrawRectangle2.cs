using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionDisplayTool.DisplayContents;

namespace VisionDisplayTool.DrawROI
{
   public class DrawRectangle2 : DrawBase<double, double, double, double, double>
    {
        public double CenterRow { get { return Content1; } set { Content1 = value; } }
        public double CenterCol { get { return Content2; } set { Content2 = value; } }
        public double Angle { get { return Content3; } set { Content3 = value; } }
        public double Length1 { get { return Content4; } set { Content4 = value; } }
        public double Length2 { get { return Content5; } set { Content5 = value; } }

        public DrawRectangle2()
        {

        }

        public DrawRectangle2(HWindow window, HImage image) : base(window,image)
        {
            if (image != null)
            {
                int width, height;
                image.GetImageSize(out width, out height);
                CenterRow = height / 2;
                CenterCol = width / 2;
                Angle = 0;
                Length1 = 5;
                Length2 = 10;
            }
        }

        public DrawRectangle2(HWindow window, HImage image, double centerRow, double centerCol, double angle, double length1, double length2):base(window,image)
        {
            CenterRow = centerRow;
            CenterCol = centerCol;
            Angle = angle;
            Length1 = length1;
            Length2 = length2;
        }

        public override void CreateROI()
        {
            lock (lockObj)
            {
                if (Image != null)
                {
                    Window.ClearWindow();
                    HTuple[] values = new HTuple[] { CenterRow, CenterCol, Angle, Length1, Length2 };
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
                    drawingObj = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE2, values);
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
                    this.Content1 = drawingObj.GetDrawingObjectParams("row").D;
                    this.Content2 = drawingObj.GetDrawingObjectParams("column").D;
                    this.Content3 = drawingObj.GetDrawingObjectParams("angle").D;
                    this.Content4 = drawingObj.GetDrawingObjectParams("length1").D;
                    this.Content5 = drawingObj.GetDrawingObjectParams("length2").D;
                    base.RaiseProcessROIParameter(Content1, Content2, Content3, Content4, Content5);
                    base.DrawROIComplete();
                    Window.DetachDrawingObjectFromWindow(this.drawingObj);
                    Rectangle2Content rectangle2 = new Rectangle2Content("", Window, CenterRow, CenterCol, Angle, Length1, Length2);
                    rectangle2.Display();
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
        public void GetRectangle1Parameters(out double row, out double col, out double angle, out double length1, out double length2)
        {
            row = CenterRow;
            col = CenterCol;
            angle = Angle;
            length1 = Length1;
            length2 = Length2;
        }
    }
}
