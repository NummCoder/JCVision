using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using HalconDotNet;
using System.Reflection;
using VisionInterface;
using VisionDisplayTool;

namespace VisionLineTool
{
    public partial class LineToolForm : DevExpress.XtraEditors.XtraForm
    {
        #region Properties
        public LineToolInfo Info { get; set; }
        public LineTool Tool { get; set; }
        private HImage currentImage { get; set; }

        bool bDrawROI;
        private DisplayControl displayForm;
        #endregion
        public LineToolForm()
        {
            InitializeComponent();
            displayForm = new DisplayControl();
            displayForm.Size = panel2.Size;
            panel1.Controls.Add(displayForm);
            displayForm.Show();
            displayForm.Rectangle2ROIEvent += DisplayForm_Rectangle2ROIEvent;
        }      
        public LineToolForm(IToolInfo info)
        {
            
            InitializeComponent();

            #region Init Control and Tool
            displayForm = new DisplayControl();
            displayForm.Size = panel2.Size;
            panel1.Controls.Add(displayForm);
            displayForm.Show();
            displayForm.Rectangle2ROIEvent += DisplayForm_Rectangle2ROIEvent;
            this.parametersPropertyGrid.SelectedObject = info;
            if (info.GetType() == typeof(LineToolInfo))
            {
                this.Info = (LineToolInfo)info;
            }
            Tool = new LineTool(this.Info, this.displayForm); 
            #endregion
        }
        private void DisplayForm_Rectangle2ROIEvent(double t1, double t2, double t3, double t4, double t5)
        {
            if (Info != null)
            {
                Info.Rectangle2Row = t1;
                Info.Rectangle2Col = t2;
                Info.Rectangle2Angle = t3;
                Info.Rectangle2Length1 = t4;
                Info.Rectangle2Length2 = t5;
            }
        }
        private void GetLineResultBtn_Click(object sender, EventArgs e)
        {
            ToolResult result = Tool.GetResult();
            //显示结果
            if (Info.IsShowResult)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (camerasComb.SelectedItem == null)
            {
                return;
            }
            if (camerasComb.SelectedItem.ToString() == string.Empty)
            {
                MessageBox.Show("pls select a camera then grab image");
                return;
            }
            //CamerasManager.GrabImage(camerasComb.SelectedItem.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openImageFileDialog.InitialDirectory = Assembly.GetExecutingAssembly().Location ;
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

        private void ModifyBtn_Click(object sender, EventArgs e)
        {
            bDrawROI = true;
            if (currentImage==null)
            {
                MessageBox.Show("请获取图像后再修改搜索框！");
            }
            if (Info==null)
            {
                return;
            }
            if (Info.Rectangle2Row>0)
            {
                displayForm.DrawRectangle2ROI(Info.Rectangle2Row,Info.Rectangle2Col,Info.Rectangle2Angle,Info.Rectangle2Length1,Info.Rectangle2Length2);
            }
            else
            {
                displayForm.DrawRectangle2ROI();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Info == null|| bDrawROI||currentImage==null)
            {
                return;
            }
            displayForm.DisplayRectangle2(Info.ToolName,Info.Rectangle2Row,Info.Rectangle2Col,Info.Rectangle2Angle,Info.Rectangle2Length1,Info.Rectangle2Length2,true);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {

        }
    }
}