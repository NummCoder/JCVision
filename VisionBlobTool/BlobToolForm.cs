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

namespace VisionBlobTool
{
    public partial class BlobToolForm : Form
    {
        #region Properties
        public BlobToolInfo Info { get; set; }
        public BlobTool Tool { get; set; }
        private HImage currentImage { get; set; }

        bool bDrawROI;

        private DisplayControl displayForm;
        #endregion
        public BlobToolForm()
        {
            InitializeComponent();

            #region Init DisplayControl
            displayForm = new DisplayControl();
            displayForm.Size = panel2.Size;
            panel1.Controls.Add(displayForm);
            displayForm.Show();
            displayForm.Rectangle2ROIEvent += DisplayForm_Rectangle2ROIEvent;
            #endregion
        }
        public BlobToolForm(IToolInfo info)
        {
    
            InitializeComponent();

            #region Init Control and Tool
            displayForm = new DisplayControl();
            displayForm.Size = panel2.Size;
            panel1.Controls.Add(displayForm);
            displayForm.Show();
            displayForm.Rectangle2ROIEvent += DisplayForm_Rectangle2ROIEvent;
            this.parametersPropertyGrid.SelectedObject = info;
            if (info.GetType() == typeof(BlobToolInfo))
            {
                this.Info = (BlobToolInfo)info;
            }
            Tool = new BlobTool(this.Info, this.displayForm); 
            #endregion
        }
        #region Draw Event Functions     
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

        #endregion

        private void button4_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void ModifyBtn_Click(object sender, EventArgs e)
        {
            bDrawROI = true;
            if (currentImage == null)
            {
                MessageBox.Show("请获取图像后再修改搜索框！");
            }
            if (Info == null)
            {
                return;
            }
            if (Info.Rectangle2Row > 0)
            {
                displayForm.DrawRectangle2ROI(Info.Rectangle2Row, Info.Rectangle2Col, Info.Rectangle2Angle, Info.Rectangle2Length1, Info.Rectangle2Length2);
            }
            else
            {
                displayForm.DrawRectangle2ROI();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Info == null || bDrawROI || currentImage == null)
            {
                return;
            }
            displayForm.DisplayRectangle2(Info.ToolName, Info.Rectangle2Row, Info.Rectangle2Col, Info.Rectangle2Angle, Info.Rectangle2Length1, Info.Rectangle2Length2, true);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            ///工具参数保存管理
        }

        private void ExecuteBtn_Click(object sender, EventArgs e)
        {
            if (currentImage!=null)
            {
                this.Tool.SetImage(currentImage);
            }
            ToolResult result = Tool.GetResult();
            //显示结果
            
        }
    }
}
