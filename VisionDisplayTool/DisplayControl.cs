using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionUtil.NLog;
using HalconDotNet;
using VisionDisplayTool.DrawROI;
using VisionDisplayTool.DisplayContents;
using System.IO;
using VisionUtil;

namespace VisionDisplayTool
{
    public enum WindowOperate
    {
        Zoom,
        ZoomOut,
        Translate,
        Select,
        Clear,
        Fit
    }
    public enum ROIShape
    {
        Circle,
        Retangle1,
        Retangle2,
        Ellipse,
        Line,
        Empty
    }

    public enum DisplayType
    {
        Image,
        Region,
        XLD
    }
    public partial class DisplayControl : UserControl
    {
        #region Public Properties
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private HWindow window;
        public HWindow Window
        {
            get { return window; }
            private set { window = value; }
        }
            /// <summary>
            /// 当前图像
            /// </summary>
            public HImage Image { get; set; }

            public HImage RImage;
            public HImage GImage;
            public HImage BImage;
            /// <summary>
            /// 图像宽
            /// </summary>
            private int imageWidth;
        public int ImageWidth
        {
            get { return imageWidth; }
            private set { imageWidth = value; }
        }
        /// <summary>
        /// 图像高
        /// </summary>
        private int imageHeight;
        public int ImageHeight
        {
            get { return imageHeight; }
            private set { imageHeight = value; }
        }
        /// <summary>
        /// 当前显示控件大小
        /// </summary>
        Rectangle partRectangle { get; set; }
        /// <summary>
        /// 初始显示控件大小
        /// </summary>
        Rectangle initRectangle { get; set; }
        /// <summary>
        ///记录当前的窗口操作
        /// </summary>
        WindowOperate _WindowOpreate { get; set; }
        /// <summary>
        /// 记录窗口显示的图像类型
        /// </summary>
        DisplayType _DisplayType { get; set; } = DisplayType.Image;
        /// <summary>
        /// 画ROI标志
        /// </summary>
        public bool bDrawing = false;
        /// <summary>
        /// 记录当前点
        /// </summary>
        Point CurrentPoint { get; set; } = new Point();
        /// <summary>
        /// 鼠标当前点
        /// </summary>

        //记录鼠标的X坐标
        int CurrentMousePosX { get; set; }
        /// <summary>
        ///记录鼠标的Y坐标
        /// </summary>
        int CurrentMousePosY { get; set; }

        Point MouseDown_Point { get; set; } = new Point();
        /// <summary>
        /// 当前感兴趣区域
        /// </summary>
        public HRegion _Region { get; set; }
        /// <summary>
        /// 当前亚像素轮廓
        /// </summary>
        public HXLD hXLD { get; set; }
        /// <summary>
        /// 用于保存所有的显示项
        /// </summary>
        public Dictionary<string, ContentBase> displayItemsDic { get; set; }
        /// <summary>
        /// 用于显示当前位置的RGB值
        /// </summary>
        public int MouseRedValue { get; set; }
        public int MouseBlueValue { get; set; }
        public int MouseGreenValue { get; set; }

        /// <summary>
        /// ROI操作
        /// </summary>   
        private DrawCircle circle;
        private DrawRectangle1 rectangle1;
        private DrawRectangle2 rectangle2;
        private DrawEllipse ellipse;
        private DrawLine line;
        //当前ROI形状
        public ROIShape DrawingShape { get; set; } = ROIShape.Empty;
        /// <summary>
        /// ROI事件,绑定接收参数
        /// </summary>
        public event DrawBase<int, int, double, object, object>.ParametersHandler CircleROIEvent;
        public event DrawBase<double, double, double, double, object>.ParametersHandler Rectangle1ROIEvent;
        public event DrawBase<double, double, double, double, double>.ParametersHandler Rectangle2ROIEvent;
        public event DrawBase<double, double, double, double, double>.ParametersHandler EllipseROIEvent;
        public event DrawBase<double, double, double, double, object>.ParametersHandler LineROIEvent;
        #endregion

        #region Constructor
        public DisplayControl()
        {
            InitializeComponent();
            this.window = hWindowControl1.HalconWindow;
            displayItemsDic = new Dictionary<string, ContentBase>();


        }
        public DisplayControl(TabControl tabControl)
        {
            InitializeComponent();
            this.Size = tabControl.Size;
            tabControl.Controls.Add(this);
            this.Show();
            this.window = hWindowControl1.HalconWindow;
            displayItemsDic = new Dictionary<string, ContentBase>();
        }
        public DisplayControl(Panel panel)
        {
            InitializeComponent();
            this.Size = panel.Size;
            panel.Controls.Add(this);
            this.Show();
            this.window = hWindowControl1.HalconWindow;
            displayItemsDic = new Dictionary<string, ContentBase>();
        }
        #endregion

        #region Display Methods
        /// <summary>
        /// 显示图片并自适应控件大小
        /// </summary>
        /// <param name="image"></param>
        public void DisplayImage(HImage image)
        {
            if (image != null)
            {
                Image = image;
                image.GetImageSize(out imageWidth, out imageHeight);
                HOperatorSet.SetSystem("flush_graphic", "false");
                Window.ClearWindow();
                HOperatorSet.SetSystem("flush_graphic", "true");
                Window.DispImage(image);
            }
        }
        public void DisplayRegion(HRegion region)
        {
            if (region != null)
            {
                _Region = region;
                HOperatorSet.SetSystem("flush_graphic", "false");
                Window.ClearWindow();
                HOperatorSet.SetSystem("flush_graphic", "true");
                Window.DispRegion(_Region);
            }
        }
        public void DisplayXLD(HXLD XLD)
        {
            if (XLD != null)
            {
                hXLD = XLD;
                HOperatorSet.SetSystem("flush_graphic", "false");
                Window.ClearWindow();
                HOperatorSet.SetSystem("flush_graphic", "true");
                Window.DispXld(XLD);
            }
        }
        public void DisplayObj(DisplayType type)
        {
            if (type == DisplayType.Region)
            {
                DisplayRegion(_Region);
            }
            if (type == DisplayType.Image)
            {
                DisplayImage(Image);
            }
            if (type == DisplayType.XLD)
            {
                DisplayXLD(hXLD);
            }
        }
        public bool SaveImage(HImage image, bool NG)
        {
            string fileName = string.Empty;
            try
            {
                if (!NG)
                {
                    if (!Directory.Exists(@"D://OkImageFile"))
                    {
                        Directory.CreateDirectory(@"D://OkImageFile");
                    }
                    fileName = @"D://OkImageFile/" + DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_fff") + "OK" + ".png";
                }
                else
                {
                    if (!Directory.Exists(@"D:\NGImageFile"))
                    {
                        Directory.CreateDirectory(@"D:\NGImageFile");
                    }
                    fileName = @"D://OkImageFile/" + DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_fff") + "NG" + ".png";
                }
                HOperatorSet.WriteImage(image, "png", 0, fileName);
                return true;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
                return false;
            }
        }
        public void DisplayWord(string name, int row, int col, string displayMessage, bool bAddList = false, string color = "green", string font = "15")
        {
            try
            {
                ContentBase word = new TextContent(name, Window, row, col, displayMessage, color, font);
                if (bAddList)
                {
                    displayItemsDic.Add(word.ContentName, word);
                }
                word.Display();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
        public void DisplayWord(string name, string displayMessage, bool bAddList = false, string color = "green", string font = "15")
        {
            try
            {
                ContentBase word = new TextContent(name, Window, 0, 0, displayMessage, color, font);
                if (bAddList)
                {
                    displayItemsDic.Add(word.ContentName, word);
                }
                word.Display();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
        public void DisplayCircle(string name, double row, double col, double radius, bool bAddList = false, string color = "green", int lineWidth = 1)
        {
            try
            {
                ContentBase circle = new CircleContent(name, Window, row, col, radius, color, lineWidth);
                if (bAddList)
                {
                    displayItemsDic.Add(circle.ContentName, circle);
                }
                circle.Display();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
        public void DisplayCross(string name, double row, double col, double size, bool bAddList = false, string color = "green", int lineWidth = 1)
        {
            try
            {
                ContentBase cross = new CrossContent(name, Window, row, col, size, color, lineWidth);
                if (bAddList)
                {
                    displayItemsDic.Add(cross.ContentName, cross);
                }
                cross.Display();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
        public void DisplayEllipse(string name, int row, int col, double phi, double radius1, double radius2, bool bAddList = false, string color = "green", int lineWidth = 1)
        {
            try
            {
                ContentBase ellipse = new EllipseContent(name, Window, row, col, phi, radius1, radius2, color, lineWidth);
                if (bAddList)
                {
                    displayItemsDic.Add(ellipse.ContentName, ellipse);
                }
                ellipse.Display();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
        public void DisplayAll(bool bOn)
        {
            if (bOn)
            {
                foreach (var item in displayItemsDic.Values)
                {
                    if (item.bDisplay)
                    {
                        item.Display();
                    }
                }
            }
        }
        public void DisplayPolygon()
        {
            return;
        }
        public void DisplayRectangle1(string name, double row1, double col1, double row2, double col2, bool bAddList = false, string color = "green", int lineWidth = 1)
        {
            try
            {
                ContentBase rectangle1 = new Rectangle1Content(name, Window, row1, col1, row2, col2, color, lineWidth);
                if (bAddList)
                {
                    displayItemsDic.Add(rectangle1.ContentName, rectangle1);
                }
                rectangle1.Display();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
        public void DisplayRectangle2(string name, double row, double col, double phi, double length1, double length2, bool bAddList = false, string color = "green", int lineWidth = 1)
        {
            try
            {
                ContentBase retangle2 = new Rectangle2Content(name, Window, row, col, phi, length1, length2, color, lineWidth);
                if (bAddList)
                {
                    if (!displayItemsDic.ContainsKey(name))
                    {
                        displayItemsDic.Add(retangle2.ContentName, retangle2);
                    }                   
                }
                retangle2.Display();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
        public void DisplaySingleLine(string lineName, double row1, double col1, double row2, double col2, bool bAddList = false, string color = "green", int lineWidth = 1)
        {
            try
            {
                ContentBase line = new SingleLineContent(lineName, Window, row1, col1, row2, col2);
                if (bAddList)
                {
                    displayItemsDic.Add(line.ContentName, line);
                }
                line.Display();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
        /// <summary>
        /// 指定八个参数创建十字线
        /// </summary>
        /// <param name="name"></param>
        /// <param name="window"></param>
        /// <param name="row1">第一条直线起始点</param>
        /// <param name="col1">第一条直线起始点</param>
        /// <param name="row2">第一条直线结束点</param>
        /// <param name="col2">第一条直线结束点</param>
        /// <param name="row3">第二条直线起始点</param>
        /// <param name="col3">第二条直线结束点</param>
        /// <param name="row4">第二条直线结束点</param>
        /// <param name="col4">第二条直线起始点</param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        public void DisplayCrossLine(string name, double row1, double col1, double row2, double col2, double row3, double col3, double row4, double col4, bool bAddList = false, string color = "green", int lineWidth = 1)
        {
            try
            {
                ContentBase crossLine = new CrossLineContent(name, Window, row1, col1, row2, col2, row3, col3, row4, col4, color, lineWidth);
                if (bAddList)
                {
                    displayItemsDic.Add(crossLine.ContentName, crossLine);
                }
                crossLine.Display();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
        /// <summary>
        /// 屏蔽某一子项
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="bDisplay"></param>
        public void SetDisplayItemValue(string itemName, bool bDisplay)
        {
            if (displayItemsDic.ContainsKey(itemName))
            {
                displayItemsDic[itemName].bDisplay = bDisplay;
            }
        }
        /// <summary>
        /// 清除所有显示子项
        /// </summary>
        public void ClearAllDisplayItem()
        {
            if (displayItemsDic.Count>0)
            {
                displayItemsDic.Clear();
            }
        }
        private void DisplayCrossLine()
        {
            if (!displayItemsDic.ContainsKey("crossline") && Image != null)
            {
                ContentBase crossLine = new CrossLineContent("crossline", Window, ImageWidth, ImageHeight);
                displayItemsDic.Add(crossLine.ContentName, crossLine);
            }
            if (displayItemsDic.ContainsKey("crossline"))
            {
                displayItemsDic["crossline"].Display();
            }
        }
        private void SaveImage()
        {
            if (this.Image != null)
            {
                try
                {
                    HImage image = Image.CopyImage();
                    if (!Directory.Exists(@".//DisplayImages/"))
                    {
                        Directory.CreateDirectory(@".//DisplayImages/");
                    }
                    string fileName = @".//DisplayImages/" + DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_fff") + ".png";
                    HOperatorSet.WriteImage(image, "png", 0, fileName);
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex.ToString());
                    return;
                }
            }
        }
        private void FitImage()
        {
            _WindowOpreate = WindowOperate.Fit;
            if (Image == null)
            {
                return;
            }
            Image.GetImageSize(out imageWidth, out imageHeight);
            partRectangle = new Rectangle(0, 0, imageWidth, imageHeight);
            Window.ClearWindow();
            hWindowControl1.ImagePart = partRectangle;
            DisplayAll(true);
        }
        private void TranslateImage()
        {
            _WindowOpreate = WindowOperate.Translate;
        }
        private void MouseSelect()
        {
            _WindowOpreate = WindowOperate.Select;
        }
        private void ClearWindow()
        {
            _WindowOpreate = WindowOperate.Clear;
            Image.Dispose();
            Image = null;
            Window.ClearWindow();
            displayItemsDic.Clear();
        }
        
        private void ZoomImage(double scaleWidth, double scaleHeight)
        {
            if (Image != null)
            {
                try
                {
                    //Rectangle rec = new Rectangle(0, 0, ImageWidth, ImageHeight);
                    //rec.Width = (int)(rec.Width * scaleWidth);
                    //rec.Height = (int)(rec.Height * scaleHeight);
                    if (hWindowControl1.InvokeRequired)
                    {
                        Action action = () => {
                            hWindowControl1.Width = ImageWidth * Convert.ToInt32(scaleWidth);
                            hWindowControl1.Height = ImageHeight * Convert.ToInt32(scaleHeight);
                        };
                        this.BeginInvoke(action);
                    }
                    else
                    {
                        hWindowControl1.Width = ImageWidth * Convert.ToInt32(scaleWidth);
                        hWindowControl1.Height = ImageHeight * Convert.ToInt32(scaleHeight);
                    }
                    HImage newImage = Image.CopyImage();
                    DisplayImage(newImage);
                    DisplayAll(true);
                }
                catch (Exception ex)
                {
                    WriteErrorLog(ex.ToString());
                }
            }
        }
        private void ZoomImage(int posX, int posY)
        {

            if (Image != null)
            {
                Rectangle rec = new Rectangle();
                rec = hWindowControl1.ImagePart;
                rec.Width = (int)(rec.Width * 0.8);
                rec.Height = (int)(rec.Height * 0.8);
                int org_x = (int)((double)rec.X + (posX - (double)rec.X) * 0.2);
                int org_y = (int)((double)rec.Y + (posY - (double)rec.Y) * 0.2);
                rec.X = org_x;
                rec.Y = org_y;
                hWindowControl1.ImagePart = rec;
                DisplayAll(true);
            }
        }
        private void ZoomOutImage(int posX, int posY)
        {

            if (Image != null)
            {
                Rectangle rec = new Rectangle();
                rec = hWindowControl1.ImagePart;
                rec.Width = (int)(rec.Width * 1.2);
                rec.Height = (int)(rec.Height * 1.2);
                int org_x = (int)((double)rec.X - (posX - (double)rec.X) * 0.2);
                int org_y = (int)((double)rec.Y - (posY - (double)rec.Y) * 0.2);
                rec.X = org_x;
                rec.Y = org_y;
                hWindowControl1.ImagePart = rec;
                DisplayAll(true);
            }
        }
        #endregion

        #region Control Events
        private void EventHandlers(object sender, EventArgs e)
        {
            ToolStripButton stripButton = (ToolStripButton)sender;
            switch (stripButton.Name)
            {
                case "Clear":
                    ClearWindow();
                    break;
                case "Selected":
                    MouseSelect();
                    break;
                case "Translate":
                    TranslateImage();
                    break;
                case "Zoom":
                    _WindowOpreate = WindowOperate.Zoom;
                    break;
                case "ZoomOut":
                    _WindowOpreate = WindowOperate.ZoomOut;
                    break;
                case "Fit":
                    FitImage();
                    break;
                case "imageSave":
                    SaveImage();
                    break;
                case "Cross":
                    DisplayCrossLine();
                    break;
                default:
                    break;
            }
        }
        private void zoomNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = zoomNumber.SelectedItem.ToString();
            double number = 0.0;
            if (str.Length == 3)
            {
                number = Convert.ToDouble(str.Substring(0, 2));
                if (number == 25)
                {
                    number = 4;
                }

                if (number == 50)
                {
                    number = 2;
                }

            }
            else if (str.Length == 4)
            {
                number = Convert.ToDouble(str.Substring(0, 3));
                if (number == 100)
                {
                    number = 1;
                }
                if (number == 200)
                {
                    number = 0.5;
                }
                if (number == 400)
                {
                    number = 0.25;
                }
            }
            ZoomImage(number, number);
        }
        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                initRectangle = hWindowControl1.ImagePart;
                int x = (int)e.X;
                int y = (int)e.Y;
                MouseDown_Point = new Point(x, y);
            }
            if (_WindowOpreate == WindowOperate.Zoom)
            {
                ZoomImage((int)e.X, (int)e.Y);
            }
            if (_WindowOpreate == WindowOperate.ZoomOut)
            {
                ZoomOutImage((int)e.X, (int)e.Y);
            }
        }

        private void hWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            GetStatus(e);
            if (Image == null)
                return;
            if (MouseButtons == MouseButtons.Left && _WindowOpreate == WindowOperate.Translate)
            {
                int x = (int)e.X;
                int y = (int)e.Y;
                CurrentPoint = new Point(x, y);

                int dx = CurrentPoint.X - MouseDown_Point.X;
                int dy = CurrentPoint.Y - MouseDown_Point.Y;
                if (partRectangle != null)
                {
                    int row1 = initRectangle.X - dx;
                    int col1 = initRectangle.Y - dy;
                    Size size = initRectangle.Size;
                    partRectangle = new Rectangle(new Point(row1, col1), size);
                    hWindowControl1.ImagePart = partRectangle;
                    DisplayAll(true);
                }
            }
        }
        private void GetStatus(HMouseEventArgs eventArgs)
        {
            int num;
            int num2;
            try
            {
                int num3;
                this.Window.GetMposition(out num, out num2, out num3);
            }
            catch
            {
                return;
            }
            try
            {
                if (Image != null)
                {
                    if (Image.GetImageType() == "byte")
                    {
                        string str;
                        int num4;
                        int num5;
                        this.Image.GetImagePointer1(out str, out num4, out num5);
                        if ((((num < num5) && (num > 0)) && (num2 < num4)) && (num2 > 0))
                        {
                            if (this.Image.CountChannels().I == 1)
                            {
                                this.XValueLabel.Text = "X:" + num2.ToString();
                                this.YValueLabel.Text = "Y:" + num.ToString();
                                this.RValueLabel.Text = "R:" + this.Image.GetGrayval(num, num2).ToString();
                                this.GValueLabel.Text = "G:" + this.RValueLabel.Text;
                                this.BValueLabel.Text = "B:" + this.RValueLabel.Text;
                            }
                            if (this.Image.CountChannels().I == 3)
                            {
                                this.XValueLabel.Text = "X:" + num2.ToString();
                                this.YValueLabel.Text = "Y:" + num.ToString();
                                this.RImage = this.Image.Decompose3(out this.GImage, out this.BImage);
                                this.RValueLabel.Text = "R:" + this.RImage.GetGrayval(num, num2).ToString();
                                this.GValueLabel.Text = "G:" + this.GImage.GetGrayval(num, num2).ToString();
                                this.BValueLabel.Text = "B:" + this.BImage.GetGrayval(num, num2).ToString();
                            }
                        }
                    }
                    else if (Image.GetImageType() == "uint2")
                    {
                        string str2;
                        int num7;
                        int num8;
                        this.Image.GetImagePointer1(out str2, out num7, out num8);
                        if ((((num < num8) && (num > 0)) && (num2 < num7)) && (num2 > 0))
                        {
                            this.XValueLabel.Text = "X:" + num2.ToString();
                            this.YValueLabel.Text = "Y:" + num.ToString();
                            this.RValueLabel.Text = "R:" + this.Image.GetGrayval(num, num2).ToString();
                            this.GValueLabel.Text = "G:" + this.RValueLabel.Text;
                            this.BValueLabel.Text = "B:" + this.RValueLabel.Text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }

        }
        private void hWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            if (Image == null)
            {
                return;
            }
            Rectangle rec = new Rectangle();
            rec = hWindowControl1.ImagePart;
            if (e.Delta > 0)
            {
                rec.Width = (int)(rec.Width * 0.8);
                rec.Height = (int)(rec.Height * 0.8);
                int org_x = (int)((double)rec.X + (e.X - (double)rec.X) * 0.2);
                int org_y = (int)((double)rec.Y + (e.Y - (double)rec.Y) * 0.2);
                rec.X = org_x;
                rec.Y = org_y;
                hWindowControl1.ImagePart = rec;

            }
            else if (e.Delta < 0)
            {
                rec.Width = (int)(rec.Width * 1.2);
                rec.Height = (int)(rec.Height * 1.2);
                int org_x = (int)((double)rec.X - (e.X - (double)rec.X) * 0.2);
                int org_y = (int)((double)rec.Y - (e.Y - (double)rec.Y) * 0.2);
                rec.X = org_x;
                rec.Y = org_y;
                hWindowControl1.ImagePart = rec;
            }
            DisplayObj(_DisplayType);
            DisplayAll(true);
        }
        private void hWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            if (Image != null)
            {
                if (bDrawing && e.Button == MouseButtons.Right)
                {
                    //触发画图完成事件
                    switch (DrawingShape)
                    {
                        case ROIShape.Circle:
                            circle.DrawROIComplete();
                            //移除绑定事件
                            BindingCircleROIEvent(false);
                            break;
                        case ROIShape.Retangle1:
                            rectangle1.DrawROIComplete();
                            BindingRectangle1ROIEvent(false);
                            break;
                        case ROIShape.Retangle2:
                            rectangle2.DrawROIComplete();
                            BindingRectangle2ROIEvent(false);
                            break;
                        case ROIShape.Ellipse:
                            ellipse.DrawROIComplete();
                            BindingEllipseROIEvent(false);
                            break;
                        case ROIShape.Line:
                            line.DrawROIComplete();
                            BindingLineROIEvent(false);
                            break;
                        case ROIShape.Empty:
                            break;
                        default:
                            break;
                    }
                    DrawingShape = ROIShape.Empty;
                    bDrawing = false;
                }
            }
        }
        #endregion

        #region DrawROI Methods
        public void DrawCircleROI(int row = 100, int col = 100, double radius = 10)
        {
            if (bDrawing)
            {
                MessageHelper.ShowWarning("显示界面正在进行ROI操作！");
                return;
            }
            if (Image == null)
            {
                return;
            }
            if (circle == null)
            {
                circle = new DrawCircle(Window, Image, row, col, radius);
            }
            else
            {
                circle.Row = row;
                circle.Col = col;
                circle.Radius = radius;
            }
            bDrawing = true;
            DrawingShape = ROIShape.Circle;
            //添加参数事件
            BindingCircleROIEvent();
            circle.CreateROI();

        }
        public void DrawRectangle1ROI(double row1 = 100, double col1 = 100, double row2 = 100, double col2 = 200)
        {
            if (bDrawing)
            {
                MessageHelper.ShowWarning("显示界面正在进行ROI操作！");
                return;
            }
            if (Image == null)
            {
                return;
            }
            if (rectangle1 == null)
            {
                rectangle1 = new DrawRectangle1(Window, Image, row1, col1, row2, col2);
            }
            else
            {
                rectangle1.Row1 = row1;
                rectangle1.Col1 = col1;
                rectangle1.Row2 = row2;
                rectangle1.Col2 = col2;
            }
            bDrawing = true;
            DrawingShape = ROIShape.Retangle1;
            BindingRectangle1ROIEvent();
            rectangle1.CreateROI();
        }
        public void DrawRectangle2ROI(double row = 100, double col = 100, double phi = 0, double length1 = 10, double length2 = 10)
        {
            if (bDrawing)
            {
                MessageHelper.ShowWarning("显示界面正在进行ROI操作！");
                return;
            }
            if (Image == null)
            {
                return;
            }
            if (rectangle2 == null)
            {
                rectangle2 = new DrawRectangle2(Window, Image, row, col, phi, length1, length2);
            }
            else
            {
                rectangle2.CenterRow = row;
                rectangle2.CenterCol = col;
                rectangle2.Angle = phi;
                rectangle2.Length1 = length1;
                rectangle2.Length2 = length2;
            }
            bDrawing = true;
            DrawingShape = ROIShape.Retangle2;
            BindingRectangle2ROIEvent();
            rectangle2.CreateROI();
        }
        public void DrawEllipseROI(double row = 100, double col = 100, double phi = 10, double radius1 = 100, double radius2 = 100)
        {
            if (bDrawing)
            {
                MessageHelper.ShowWarning("显示界面正在进行ROI操作！");
                return;
            }
            if (Image == null)
            {
                return;
            }
            if (ellipse == null)
            {
                ellipse = new DrawEllipse(Window, Image, row, col, phi, radius1, radius2);
            }
            else
            {
                ellipse.Row = row;
                ellipse.Col = col;
                ellipse.Angle = phi;
                ellipse.Radius1 = radius1;
                ellipse.Radius2 = radius2;
            }
            bDrawing = true;
            DrawingShape = ROIShape.Ellipse;
            BindingEllipseROIEvent();
            ellipse.CreateROI();
        }
        public void DrawLineROI(double row1 = 100, double col1 = 100, double row2 = 100, double col2 = 200)
        {
            if (bDrawing)
            {
                MessageHelper.ShowWarning("显示界面正在进行ROI操作！");
                return;
            }
            if (Image == null)
            {
                return;
            }
            if (line == null)
            {
                line = new DrawLine(Window, Image, row1, col1, row2, col2);
            }
            else
            {
                line.Row1 = row1;
                line.Col1 = col1;
                line.Row2 = row2;
                line.Col2 = col2;
            }
            bDrawing = true;
            DrawingShape = ROIShape.Line;
            BindingLineROIEvent();
            line.CreateROI();
        }

        //外部事件绑定
        /// <summary>
        /// 绑定ROI画圆参数传递事件，需要传入五个参数类型分别为int,int,double,object,object的函数
        /// </summary>
        /// <param name="Func"></param>
        public void BindingCircleROIEvent(DrawBase<int, int, double, object, object>.ParametersHandler Func, bool bAdd = true)
        {
            if (circle != null)
            {
                if (bAdd)
                {
                    circle.ProcessParameters += Func;
                }
                else
                {
                    circle.ProcessParameters -= Func;
                }
            }
        }
        /// <summary>
        /// 绑定ROI画矩形参数传递事件，需要传入五个参数类型分别为double,double,double,double,double的函数
        /// </summary>
        /// <param name="Func"></param>
        public void BindingRectangle1ROIEvent(DrawBase<double, double, double, double, object>.ParametersHandler Func, bool bAdd = true)
        {
            if (rectangle1 != null)
            {
                if (bAdd)
                {
                    rectangle1.ProcessParameters += Func;
                }
                else
                {
                    rectangle1.ProcessParameters -= Func;
                }
            }
        }
        /// <summary>
        /// 绑定ROI画矩形参数传递事件，需要传入五个参数类型分别为double,double,double,double,double的函数
        /// </summary>
        /// <param name="Func"></param>
        public void BindingRectangle2ROIEvent(DrawBase<double, double, double, double, double>.ParametersHandler Func, bool bAdd = true)
        {
            if (rectangle2 != null)
            {
                if (bAdd)
                {
                    rectangle2.ProcessParameters += Func;
                }
                else
                {
                    rectangle2.ProcessParameters -= Func;
                }
            }
        }
        /// <summary>
        /// 绑定ROI画椭圆参数传递事件，需要传入五个参数类型分别为double,double,double,double,double的函数
        /// </summary>
        /// <param name="Func"></param>
        public void BindingEllipseROIEvent(DrawBase<double, double, double, double, double>.ParametersHandler Func, bool bAdd = true)
        {
            if (ellipse != null)
            {
                if (bAdd)
                {
                    ellipse.ProcessParameters += Func;
                }
                else
                {
                    ellipse.ProcessParameters -= Func;
                }
            }
        }
        /// <summary>
        /// 绑定ROI画直线参数传递事件，需要传入五个参数类型分别为double,double,double,double,object的函数
        /// </summary>
        /// <param name="Func"></param>
        public void BindingLineROIEvent(DrawBase<double, double, double, double, object>.ParametersHandler Func, bool bAdd = true)
        {
            if (line != null)
            {
                if (bAdd)
                {
                    line.ProcessParameters += Func;
                }
                else
                {
                    line.ProcessParameters -= Func;
                }
            }
        }

        //私有方法
        private void BindingCircleROIEvent(bool bAdd = true)
        {
            if (circle != null && CircleROIEvent != null)
            {
                if (bAdd)
                {
                    circle.ProcessParameters += CircleROIEvent;
                }
                else
                {
                    circle.ProcessParameters -= CircleROIEvent;
                }
            }
        }
        private void BindingRectangle1ROIEvent(bool bAdd = true)
        {
            if (rectangle1 != null && Rectangle1ROIEvent != null)
            {
                if (bAdd)
                {
                    rectangle1.ProcessParameters += Rectangle1ROIEvent;
                }
                else
                {
                    rectangle1.ProcessParameters -= Rectangle1ROIEvent;
                }
            }
        }
        private void BindingRectangle2ROIEvent(bool bAdd = true)
        {
            if (rectangle2 != null && Rectangle2ROIEvent != null)
            {
                if (bAdd)
                {
                    rectangle2.ProcessParameters += Rectangle2ROIEvent;
                }
                else
                {
                    rectangle2.ProcessParameters -= Rectangle2ROIEvent;
                }
            }
        }
        private void BindingEllipseROIEvent(bool bAdd = true)
        {
            if (ellipse != null && EllipseROIEvent != null)
            {
                if (bAdd)
                {
                    ellipse.ProcessParameters += EllipseROIEvent;
                }
                else
                {
                    ellipse.ProcessParameters -= EllipseROIEvent;
                }
            }
        }
        private void BindingLineROIEvent(bool bAdd = true)
        {
            if (line != null && LineROIEvent != null)
            {
                if (bAdd)
                {
                    line.ProcessParameters += LineROIEvent;
                }
                else
                {
                    line.ProcessParameters -= LineROIEvent;
                }
            }
        }
        #endregion

        #region Change Display Type
        public void SetColor(string color)
        {
            try
            {
                window.SetColor(color);
            }
            catch (Exception e)
            {
                WriteErrorLog(e.ToString());               
            }
        }
        #endregion

        #region private Methods
        protected virtual void WriteInfoLog(string infoMessage)
        {
            LogFileManager.Info("DisplayControl", infoMessage);
        }

        protected virtual void WriteErrorLog(string errorMessage)
        {
            LogFileManager.Error("DisplayControl", errorMessage);
        }

        protected virtual void WriteFatalLog(string fatalMessage)
        {
            LogFileManager.Fatal("DisplayControl", fatalMessage);
        }
        protected virtual void WriteDebugLog(string debugMessage)
        {
            LogFileManager.Debug("DisplayControl", debugMessage);
        }



        #endregion
    }
}
