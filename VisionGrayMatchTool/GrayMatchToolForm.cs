using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionDisplayTool;
using VisionInterface;
using VisionUtil;
using VisionUtil.EnumStrucks;
using VisionUtil.GraphParameter;

namespace VisionGrayMatchTool
{
    public partial class GrayMatchToolForm : Form
    {
        #region Properties
        HImage currentImage { get; set; }
        public GrayMatchToolInfo Info { get; set; }

        public List<GrayMatchToolInfo> Infos { get; set; }

        Dictionary<string, GrayMatchTool> Tools;
        bool bModifyModelROI;

        bool bDrawFindROI;

        bool bModifyFindROI;

        string newModelName;

        private DisplayControl displayForm;
        #endregion

        #region Constructor
        public GrayMatchToolForm()
        {

            InitializeComponent();
            displayForm = new DisplayControl();
            displayForm.Size = panel1.Size;
            panel1.Controls.Add(displayForm);
            displayForm.Show();
            #region Initialize Comboxs
            foreach (var item in Enum.GetValues(typeof(ROI)))
            {
                ROIShapeComb.Items.Add(item);
                FindRegionShapeComb.Items.Add(item);
            }
            #endregion

            #region Initialize ShapeMatchTools
            Infos = new List<GrayMatchToolInfo>();
            Tools = new Dictionary<string, GrayMatchTool>();
            #endregion

            #region Register Events
            this.displayForm.CircleROIEvent += DisplayForm_CircleROIEvent;
            this.displayForm.Rectangle1ROIEvent += DisplayForm_Rectangle1ROIEvent;
            this.displayForm.Rectangle2ROIEvent += DisplayForm_Rectangle2ROIEvent;
            this.displayForm.EllipseROIEvent += DisplayForm_EllipseROIEvent;
            #endregion
        }
        public GrayMatchToolForm(List<IToolInfo> infoList)
        {
            InitializeComponent();

            #region Initialize ShapeMatchTools
            Infos = new List<GrayMatchToolInfo>();
            Tools = new Dictionary<string, GrayMatchTool>();
            foreach (var info in infoList)
            {
                if (info.GetType() == typeof(GrayMatchToolInfo))
                {
                    Infos.Add((GrayMatchToolInfo)info);
                    Tools.Add(info.ToolName, new GrayMatchTool(info, displayForm));
                    CurrentModelComb.Items.Add(info.ToolName);
                    currentFindRegionComb.Items.Add(info.ToolName);
                }
            }
            #endregion

            #region Register Events
            this.displayForm.CircleROIEvent += DisplayForm_CircleROIEvent;
            this.displayForm.Rectangle1ROIEvent += DisplayForm_Rectangle1ROIEvent;
            this.displayForm.Rectangle2ROIEvent += DisplayForm_Rectangle2ROIEvent;
            this.displayForm.EllipseROIEvent += DisplayForm_EllipseROIEvent;
            #endregion

        }
        #endregion

        #region ROI Event Function
        private void DisplayForm_EllipseROIEvent(double t1, double t2, double t3, double t4, double t5)
        {
            if (!bModifyModelROI && !bModifyFindROI && !bDrawFindROI)
            {
                if (!string.IsNullOrEmpty(newModelName))
                {
                    //Create a new setting info
                    GrayMatchToolInfo info = new GrayMatchToolInfo();
                    info.ToolName = newModelName;
                    info.ModelROIParam = new EllipseParam()
                    {
                        EllipseCenterRow = t1,
                        EllipseCenterColumn = t2,
                        EllipseAngle = t3,
                        EllipseRadius1 = t4,
                        EllipseRadius2 = t5,
                        GraphName = newModelName
                    };
                    //Create a new match tool
                    GrayMatchTool tool = new GrayMatchTool(info, displayForm);
                    if (!tool.CreateMatchTool())
                    {
                        MessageHelper.ShowError("模板创建失败，请重新创建");
                        return;
                    }
                    Infos.Add(info);
                    Tools.Add(info.ToolName, tool);
                    CurrentModelComb.Items.Add(info.ToolName);
                    currentFindRegionComb.Items.Add(info.ToolName);
                }
            }
            else if (bModifyModelROI && !bModifyFindROI && !bDrawFindROI)
            {
                bModifyModelROI = false;
                if (Info != null)
                {
                    EllipseParam ellipse = Info.ModelROIParam as EllipseParam;
                    if (ellipse != null)
                    {
                        ellipse.EllipseCenterRow = t1;
                        ellipse.EllipseCenterColumn = t2;
                        ellipse.EllipseAngle = t3;
                        ellipse.EllipseRadius1 = t4;
                        ellipse.EllipseRadius2 = t5;
                    }
                }
            }
            else if (!bModifyModelROI && !bModifyFindROI && bDrawFindROI)
            {
                bDrawFindROI = false;
                GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
                if (info != null)
                {
                    info.FindModelROIParam = new EllipseParam()
                    {
                        GraphName = currentFindRegionComb.SelectedItem.ToString(),
                        EllipseCenterRow = t1,
                        EllipseCenterColumn = t2,
                        EllipseAngle = t3,
                        EllipseRadius1 = t4,
                        EllipseRadius2 = t5
                    };
                }
            }
            else if (!bModifyModelROI && bModifyFindROI && bDrawFindROI)
            {
                bDrawFindROI = false;
                bModifyFindROI = false;
                GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
                if (info != null)
                {
                    EllipseParam ellipse = info.FindModelROIParam as EllipseParam;
                    if (ellipse != null)
                    {
                        ellipse.EllipseCenterRow = t1;
                        ellipse.EllipseCenterColumn = t2;
                        ellipse.EllipseAngle = t3;
                        ellipse.EllipseRadius1 = t4;
                        ellipse.EllipseRadius2 = t5;
                    }
                }
            }
        }

        private void DisplayForm_Rectangle2ROIEvent(double t1, double t2, double t3, double t4, double t5)
        {
            if (!bModifyModelROI && !bModifyFindROI && !bDrawFindROI)
            {
                if (!string.IsNullOrEmpty(newModelName))
                {
                    //Create a new setting info
                    GrayMatchToolInfo info = new GrayMatchToolInfo();
                    info.ToolName = newModelName;
                    info.ModelROIParam = new Rectangle2Param()
                    {
                        Rectangle2CenterRow = t1,
                        Retangle2CenterColumn = t2,
                        Retangle2Angle = t3,
                        Rectangle2Length1 = t4,
                        Rectangle2Length2 = t5,
                        GraphName = newModelName
                    };
                    //Create a new match tool
                    GrayMatchTool tool = new GrayMatchTool(info, displayForm);
                    if (!tool.CreateMatchTool())
                    {
                        MessageHelper.ShowError("模板创建失败，请重新创建");
                        return;
                    }
                    Infos.Add(info);
                    Tools.Add(info.ToolName, tool);
                    CurrentModelComb.Items.Add(info.ToolName);
                    currentFindRegionComb.Items.Add(info.ToolName);
                }
            }
            else if (bModifyModelROI && !bModifyFindROI && !bDrawFindROI)
            {
                bModifyModelROI = false;
                if (Info != null)
                {
                    Rectangle2Param rectangle2 = Info.ModelROIParam as Rectangle2Param;
                    if (rectangle2 != null)
                    {
                        rectangle2.Rectangle2CenterRow = t1;
                        rectangle2.Retangle2CenterColumn = t2;
                        rectangle2.Retangle2Angle = t3;
                        rectangle2.Rectangle2Length1 = t4;
                        rectangle2.Rectangle2Length2 = t5;
                    }
                }
            }
            else if (!bModifyModelROI && !bModifyFindROI && bDrawFindROI)
            {
                bDrawFindROI = false;
                GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
                if (info != null)
                {
                    info.FindModelROIParam = new Rectangle2Param()
                    {
                        GraphName = currentFindRegionComb.SelectedItem.ToString(),
                        Rectangle2CenterRow = t1,
                        Retangle2CenterColumn = t2,
                        Retangle2Angle = t3,
                        Rectangle2Length1 = t4,
                        Rectangle2Length2 = t5
                    };
                }
            }
            else if (!bModifyModelROI && bModifyFindROI && bDrawFindROI)
            {
                bDrawFindROI = false;
                bModifyFindROI = false;
                GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
                if (info != null)
                {
                    Rectangle2Param rectangle2 = info.ModelROIParam as Rectangle2Param;
                    if (rectangle2 != null)
                    {
                        rectangle2.Rectangle2CenterRow = t1;
                        rectangle2.Retangle2CenterColumn = t2;
                        rectangle2.Retangle2Angle = t3;
                        rectangle2.Rectangle2Length1 = t4;
                        rectangle2.Rectangle2Length2 = t5;
                    }
                }
            }
        }

        private void DisplayForm_Rectangle1ROIEvent(double t1, double t2, double t3, double t4, object t5)
        {
            if (!bModifyModelROI && !bModifyFindROI && !bDrawFindROI)
            {
                if (!string.IsNullOrEmpty(newModelName))
                {
                    //Create a new setting info
                    GrayMatchToolInfo info = new GrayMatchToolInfo();
                    info.ToolName = newModelName;
                    info.ModelROIParam = new Rectangle1Param()
                    {
                        RectangleStartRow = t1,
                        RectangleStartColumn = t2,
                        RectangleEndRow = t3,
                        RectangleEndColumn = t4,
                        GraphName = newModelName
                    };
                    //Create a new match tool
                    GrayMatchTool tool = new GrayMatchTool(info, displayForm);
                    if (!tool.CreateMatchTool())
                    {
                        MessageHelper.ShowError("模板创建失败，请重新创建");
                        return;
                    }
                    Infos.Add(info);
                    Tools.Add(info.ToolName, tool);
                    CurrentModelComb.Items.Add(info.ToolName);
                    currentFindRegionComb.Items.Add(info.ToolName);
                }
            }
            else if (bModifyModelROI && !bModifyFindROI && !bDrawFindROI)
            {
                bModifyModelROI = false;
                if (Info != null)
                {
                    Rectangle1Param rectangle1 = Info.ModelROIParam as Rectangle1Param;
                    if (rectangle1 != null)
                    {
                        rectangle1.RectangleStartRow = t1;
                        rectangle1.RectangleStartColumn = t2;
                        rectangle1.RectangleEndRow = t3;
                        rectangle1.RectangleEndColumn = t4;
                    }
                }
            }
            else if (!bModifyModelROI && !bModifyFindROI && bDrawFindROI)
            {
                bDrawFindROI = false;
                GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
                if (info != null)
                {
                    info.FindModelROIParam = new Rectangle1Param()
                    {
                        GraphName = currentFindRegionComb.SelectedItem.ToString(),
                        RectangleStartRow = t1,
                        RectangleStartColumn = t2,
                        RectangleEndRow = t3,
                        RectangleEndColumn = t4,
                    };
                }
            }
            else if (!bModifyModelROI && bModifyFindROI && bDrawFindROI)
            {
                bDrawFindROI = false;
                bModifyFindROI = false;
                GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
                if (info != null)
                {
                    Rectangle1Param rectangle1 = info.FindModelROIParam as Rectangle1Param;
                    if (rectangle1 != null)
                    {
                        rectangle1.RectangleStartRow = t1;
                        rectangle1.RectangleStartColumn = t2;
                        rectangle1.RectangleEndRow = t3;
                        rectangle1.RectangleEndColumn = t4;
                    }
                }
            }
        }

        private void DisplayForm_CircleROIEvent(int t1, int t2, double t3, object t4, object t5)
        {
            if (!bModifyModelROI && !bModifyFindROI && !bDrawFindROI)
            {
                if (!string.IsNullOrEmpty(newModelName))
                {
                    //Create a new setting info
                    GrayMatchToolInfo info = new GrayMatchToolInfo();
                    info.ToolName = newModelName;
                    info.ModelROIParam = new CircleParam() { CircleRow = t1, CircleColumn = t2, Radius = t3, GraphName = newModelName };
                    //Create a new match tool
                    GrayMatchTool tool = new GrayMatchTool(info, displayForm);
                    if (!tool.CreateMatchTool())
                    {
                        MessageHelper.ShowError("模板创建失败，请重新创建");
                        return;
                    }
                    Infos.Add(info);
                    Tools.Add(info.ToolName, tool);
                    CurrentModelComb.Items.Add(info.ToolName);
                    currentFindRegionComb.Items.Add(info.ToolName);
                }
            }
            else if (bModifyModelROI && !bModifyFindROI && !bDrawFindROI)
            {
                bModifyModelROI = false;
                if (Info != null)
                {
                    CircleParam circle = Info.ModelROIParam as CircleParam;
                    if (circle != null)
                    {
                        circle.CircleRow = t1;
                        circle.CircleColumn = t2;
                        circle.Radius = t3;
                    }
                }
            }
            else if (!bModifyModelROI && !bModifyFindROI && bDrawFindROI)
            {
                bDrawFindROI = false;
                GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
                if (info != null)
                {
                    info.FindModelROIParam = new CircleParam()
                    {
                        GraphName = currentFindRegionComb.SelectedItem.ToString(),
                        CircleRow = t1,
                        CircleColumn = t2,
                        Radius = t3
                    };
                }
            }
            else if (!bModifyModelROI && bModifyFindROI && bDrawFindROI)
            {
                bDrawFindROI = false;
                bModifyFindROI = false;
                GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
                if (info != null)
                {
                    CircleParam circle = info.FindModelROIParam as CircleParam;
                    if (circle != null)
                    {
                        circle.CircleRow = t1;
                        circle.CircleColumn = t2;
                        circle.Radius = t3;
                    }
                }
            }
        }
        #endregion

        #region Update the tool when tools has been modified
        private void UpdateToolsToTask()
        {

        }

        #endregion

        private void grabBtn_Click(object sender, EventArgs e)
        {
            if (camerasComb.SelectedItem == null)
            {
                return;
            }
            if (camerasComb.SelectedItem.ToString() == string.Empty)
            {
                MessageHelper.ShowWarning("请选择一个相机后再触发拍照");
                return;
            }
            //CamerasManager.GrabImage(camerasComb.SelectedItem.ToString());
        }

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            openImageFileDialog.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            openImageFileDialog.Filter = "JPEG文件|*.jpg*|所有文件|*|BMP文件|*.bmp*|TIFF文件|*.tiff*";
            openImageFileDialog.RestoreDirectory = true;
            openImageFileDialog.FilterIndex = 2;
            if (openImageFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openImageFileDialog.FileName;
                currentImage = new HImage(path);
            }
            displayForm.DisplayImage(currentImage);
        }

        private void LoadTaskImageBtn_Click(object sender, EventArgs e)
        {

        }

        private void CreateNewBtn_Click(object sender, EventArgs e)
        {
            bModifyModelROI = false;
            if (string.IsNullOrEmpty(ROINameTxb.Text.Trim()))
            {
                MessageHelper.ShowWarning("请输入模板名称后再创建模板");
                return;
            }
            if (currentImage == null)
            {
                MessageHelper.ShowWarning("请获取图像后再创建模板");
                return;
            }
            if (ROIShapeComb.SelectedItem == null)
            {
                MessageHelper.ShowWarning("请选择ROI类型再创建模板");
                return;
            }
            //check  the matchtool  exists of this task's tools 
            if (Tools.ContainsKey(ROINameTxb.Text) && Infos.Find(p => p.ToolName == ROINameTxb.Text.Trim()) != null)
            {
                MessageHelper.ShowWarning("已经存在相同名称的模板，请修改名称后再创建");
                return;
            }
            switch (ROIShapeComb.SelectedItem.ToString())
            {
                case "Circle":
                    displayForm.DrawCircleROI();
                    break;
                case "Rectangl1":
                    displayForm.DrawRectangle1ROI();
                    break;
                case "Rectangl2":
                    displayForm.DrawRectangle2ROI();
                    break;
                case "Ellipse":
                    displayForm.DrawEllipseROI();
                    break;
                default:
                    break;
            }
            newModelName = ROINameTxb.Text.Trim();
            ROINameTxb.Clear();
        }

        private void DeleteAllBtn_Click(object sender, EventArgs e)
        {
            Infos.Clear();
            Tools.Clear();
            CurrentModelComb.Items.Clear();
            CurrentModelComb.Text = "";
            currentFindRegionComb.Items.Clear();
            currentFindRegionComb.Text = "";
        }

        private void ModifyBtn_Click(object sender, EventArgs e)
        {
            if (CurrentModelComb.SelectedItem != null)
            {
                if (Info != null&&Info.ToolName==CurrentModelComb.SelectedItem.ToString())
                {
                    switch (Info.ModelROIParam.type)
                    {
                        case ROI.Line:
                            break;
                        case ROI.Circle:
                            CircleParam circle = Info.ModelROIParam as CircleParam;
                            if (circle != null)
                            {
                                bModifyModelROI = true;
                                displayForm.DrawCircleROI(Convert.ToInt32(circle.CircleRow), Convert.ToInt32(circle.CircleColumn), circle.Radius);
                            }
                            else
                            {
                                MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                            }
                            break;
                        case ROI.Ellipse:
                            EllipseParam ellipse = Info.ModelROIParam as EllipseParam;
                            if (ellipse != null)
                            {
                                bModifyModelROI = true;
                                displayForm.DrawEllipseROI(ellipse.EllipseCenterRow, ellipse.EllipseCenterColumn, ellipse.EllipseAngle, ellipse.EllipseRadius1, ellipse.EllipseRadius2);
                            }
                            else
                            {
                                MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                            }
                            break;
                        case ROI.Rectangle1:
                            Rectangle1Param rectangle1 = Info.ModelROIParam as Rectangle1Param;
                            if (rectangle1 != null)
                            {
                                bModifyModelROI = true;
                                displayForm.DrawRectangle1ROI(rectangle1.RectangleStartRow, rectangle1.RectangleStartColumn, rectangle1.RectangleEndRow, rectangle1.RectangleEndColumn);
                            }
                            else
                            {
                                MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                            }
                            break;
                        case ROI.Rectangle2:
                            Rectangle2Param rectangle2 = Info.ModelROIParam as Rectangle2Param;
                            if (rectangle2 != null)
                            {
                                bModifyModelROI = true;
                                displayForm.DrawRectangle2ROI(rectangle2.Rectangle2CenterRow, rectangle2.Retangle2CenterColumn, rectangle2.Retangle2Angle, rectangle2.Rectangle2Length1, rectangle2.Rectangle2Length2);
                            }
                            else
                            {
                                MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void DeleteCurBtn_Click(object sender, EventArgs e)
        {
            if (CurrentModelComb.SelectedItem != null)
            {
                string modelName = CurrentModelComb.SelectedItem.ToString();
                if (Infos.FindAll(p => p.ToolName == modelName).Count > 0)
                {
                    if (Info != null)
                    {
                        Infos.Remove(Info);
                        Info = null;
                    }
                }
                if (Tools.ContainsKey(modelName))
                {
                    Tools.Remove(modelName);
                }
                CurrentModelComb.Text = string.Empty;
                currentFindRegionComb.Text = string.Empty;
            }
            else
            {
                MessageHelper.ShowWarning("请选择一个模板删除！");
            }
        }

        private void CurrentModelComb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentModelComb.SelectedItem != null)
            {
                Info = Infos.Find(p => p.ToolName == CurrentModelComb.SelectedItem.ToString());
                this.propertyGrid1.SelectedObject = Info;
            }
        }

        private void createFindRegionBtn_Click(object sender, EventArgs e)
        {
            if (currentImage == null)
            {
                MessageHelper.ShowWarning("请获取图像后再创建搜索框");
                return;
            }
            if (FindRegionShapeComb.SelectedItem == null)
            {
                MessageHelper.ShowWarning("请选择ROI类型再创建搜索框");
                return;
            }
            if (currentFindRegionComb.SelectedItem == null)
            {
                MessageHelper.ShowWarning("请选择一个模板来创建搜索框");
                return;
            }
            GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
            if (info == null)
            {
                MessageHelper.ShowWarning("无法创建该模板搜索框，请删除该模板后重新创建！");
                return;
            }
            if (info.FindModelROIParam != null)
            {
                switch (info.FindModelROIParam.type)
                {
                    case ROI.Line:
                        break;
                    case ROI.Circle:
                        CircleParam circle = info.FindModelROIParam as CircleParam;
                        if (circle != null)
                        {
                            bDrawFindROI = false;
                            bModifyFindROI = true;
                            displayForm.DrawCircleROI(Convert.ToInt32(circle.CircleRow), Convert.ToInt32(circle.CircleColumn), circle.Radius);
                        }
                        else
                        {
                            MessageHelper.ShowError("创建搜索框失败，请删除后再重新创建！");
                        }
                        break;
                    case ROI.Ellipse:
                        EllipseParam ellipse = info.FindModelROIParam as EllipseParam;
                        if (ellipse != null)
                        {
                            bDrawFindROI = false;
                            bModifyFindROI = true;
                            displayForm.DrawEllipseROI(ellipse.EllipseCenterRow, ellipse.EllipseCenterColumn, ellipse.EllipseAngle, ellipse.EllipseRadius1, ellipse.EllipseRadius2);
                        }
                        else
                        {
                            MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                        }
                        break;
                    case ROI.Rectangle1:
                        Rectangle1Param rectangle1 = info.FindModelROIParam as Rectangle1Param;
                        if (rectangle1 != null)
                        {
                            bDrawFindROI = false;
                            bModifyFindROI = true;
                            displayForm.DrawRectangle1ROI(rectangle1.RectangleStartRow, rectangle1.RectangleStartColumn, rectangle1.RectangleEndRow, rectangle1.RectangleEndColumn);
                        }
                        else
                        {
                            MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                        }
                        break;
                    case ROI.Rectangle2:
                        Rectangle2Param rectangle2 = info.FindModelROIParam as Rectangle2Param;
                        if (rectangle2 != null)
                        {
                            bDrawFindROI = false;
                            bModifyFindROI = true;
                            displayForm.DrawRectangle2ROI(rectangle2.Rectangle2CenterRow, rectangle2.Retangle2CenterColumn, rectangle2.Retangle2Angle, rectangle2.Rectangle2Length1, rectangle2.Rectangle2Length2);
                        }
                        else
                        {
                            MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (FindRegionShapeComb.SelectedItem.ToString())
                {
                    case "Circle":
                        bDrawFindROI = true;
                        bModifyFindROI = false;
                        displayForm.DrawCircleROI();
                        break;
                    case "Rectangl1":
                        bDrawFindROI = true;
                        bModifyFindROI = false;
                        displayForm.DrawRectangle1ROI();
                        break;
                    case "Rectangl2":
                        bDrawFindROI = true;
                        bModifyFindROI = false;
                        displayForm.DrawRectangle2ROI();
                        break;
                    case "Ellipse":
                        bDrawFindROI = true;
                        bModifyFindROI = false;
                        displayForm.DrawEllipseROI();
                        break;
                    default:
                        break;
                }
            }
        }

        private void deleteAllRegionBtn_Click(object sender, EventArgs e)
        {
            foreach (var item in Infos)
            {
                item.FindModelROIParam = null;
            }
        }

        private void modifyFindRegionBtn_Click(object sender, EventArgs e)
        {
            if (currentImage == null)
            {
                MessageHelper.ShowWarning("请获取图像后再修改搜索框");
                return;
            }
            if (currentFindRegionComb.SelectedItem == null)
            {
                MessageHelper.ShowWarning("请选择一个模板进行修改搜索框");
                return;
            }
            GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
            if (info == null)
            {
                MessageHelper.ShowWarning("无法修改模板搜索框，请删除该模板后重新创建！");
                return;
            }
            if (info.FindModelROIParam != null)
            {
                switch (info.FindModelROIParam.type)
                {
                    case ROI.Line:
                        break;
                    case ROI.Circle:
                        CircleParam circle = info.FindModelROIParam as CircleParam;
                        if (circle != null)
                        {
                            bDrawFindROI = false;
                            bModifyFindROI = true;
                            displayForm.DrawCircleROI(Convert.ToInt32(circle.CircleRow), Convert.ToInt32(circle.CircleColumn), circle.Radius);
                        }
                        else
                        {
                            MessageHelper.ShowError("创建搜索框失败，请删除后再重新创建！");
                        }
                        break;
                    case ROI.Ellipse:
                        EllipseParam ellipse = info.FindModelROIParam as EllipseParam;
                        if (ellipse != null)
                        {
                            bDrawFindROI = false;
                            bModifyFindROI = true;
                            displayForm.DrawEllipseROI(ellipse.EllipseCenterRow, ellipse.EllipseCenterColumn, ellipse.EllipseAngle, ellipse.EllipseRadius1, ellipse.EllipseRadius2);
                        }
                        else
                        {
                            MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                        }
                        break;
                    case ROI.Rectangle1:
                        Rectangle1Param rectangle1 = info.FindModelROIParam as Rectangle1Param;
                        if (rectangle1 != null)
                        {
                            bDrawFindROI = false;
                            bModifyFindROI = true;
                            displayForm.DrawRectangle1ROI(rectangle1.RectangleStartRow, rectangle1.RectangleStartColumn, rectangle1.RectangleEndRow, rectangle1.RectangleEndColumn);
                        }
                        else
                        {
                            MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                        }
                        break;
                    case ROI.Rectangle2:
                        Rectangle2Param rectangle2 = info.FindModelROIParam as Rectangle2Param;
                        if (rectangle2 != null)
                        {
                            bDrawFindROI = false;
                            bModifyFindROI = true;
                            displayForm.DrawRectangle2ROI(rectangle2.Rectangle2CenterRow, rectangle2.Retangle2CenterColumn, rectangle2.Retangle2Angle, rectangle2.Rectangle2Length1, rectangle2.Rectangle2Length2);
                        }
                        else
                        {
                            MessageHelper.ShowError("修改模板出现错误，请删除当前模板后重修创建");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void deleteCurrentRegionBtn_Click(object sender, EventArgs e)
        {
            if (currentFindRegionComb.SelectedItem == null)
            {
                MessageHelper.ShowWarning("请选择一个模板搜索框进行删除");
                return;
            }
            GrayMatchToolInfo info = Infos.Find(p => p.ToolName == currentFindRegionComb.SelectedItem.ToString());
            if (info == null)
            {
                return;
            }
            else
            {
                info.FindModelROIParam = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool bOk = true;
            foreach (var info in Infos)
            {
                if (Tools.ContainsKey(info.ToolName))
                {
                    GrayMatchTool tool = Tools[info.ToolName];
                    tool.Image = currentImage;
                    tool.Window = displayForm;
                    bOk &= tool.CreateMatchTool();
                }
            }
            if (bOk)
            {
                MessageHelper.ShowTips("模板保存成功！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ToolResult> resultList = new List<ToolResult>();
            foreach (var item in Tools.Values)
            {
                if (currentImage != null)
                {
                    item.SetImage(currentImage.CopyImage());
                    resultList.Add(item.GetResult());
                }
                else
                {
                    MessageHelper.ShowError("请打开一张图片或者使用相机捉取一张图片！");
                    return;
                }
            }
            ///显示结果
            if (resultList.Count > 0)
            {
                for (int i = 0; i < resultList.Count; i++)
                {
                    if (resultList[i].IsSuccess)
                    {
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-Result  : OK", true, "green");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-ImageX  : " + resultList[i].ImageX, true, "green");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-ImageY  : " + resultList[i].ImageY, true, "green");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-Angle   : " + resultList[i].ImageAngle, true, "green");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-Score   : " + resultList[i].ResultScore, true, "green");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-Time    : " + resultList[i].ElapsedTime, true, "green");
                    }
                    else
                    {
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-Result  : NG", true, "red");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-ImageX  : " + 0.0, true, "red");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-ImageY  : " + 0.0, true, "red");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-Angle   : " + 0.0, true, "red");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-Score   : " + 0.0, true, "red");
                        displayForm.DisplayWord(resultList[i].ResultName, resultList[i].ResultName + "-Time    : " + 0.0, true, "red");
                    }
                }
            }
        }
    }
}
