using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionDisplayTool;

namespace VisionCircleTool
{
    public partial class CircleToolForm : Form
    {
        #region Properties
        public CircleToolInfo Info { get; set; }
        public CircleTool Tool { get; set; }
        private HImage currentImage { get; set; }

        bool bDrawROI;
        private DisplayControl displayForm;
        #endregion

        public CircleToolForm()
        {
            InitializeComponent();
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
    }
}
