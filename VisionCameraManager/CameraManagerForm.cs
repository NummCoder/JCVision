using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionInterface;
using VisionUtil;

namespace VisionCameraManager
{
    public partial class CameraManagerForm : Form
    {
        CameraSetting formSetting;
        public CameraManagerForm()
        {
            InitializeComponent();
        }

        private void AddCamBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CamNameTxt.Text))
            {
                MessageHelper.ShowWarning("pls input the valid name of Cam");
                return;
            }
            if (CamTypeComBox.SelectedItem == null)
            {
                MessageHelper.ShowWarning("pls select  a type of this Cam");
                return;
            }

            CameraManger.AddCameraInfo(CamNameTxt.Text.Trim(),CamTypeComBox.SelectedItem.ToString());
            if (CameraManger.GetCameraInfoInstance(CamNameTxt.Text)!=null)
            {
                DataGridViewRow newrow = new DataGridViewRow();
                DataGridViewTextBoxCell namecell = new DataGridViewTextBoxCell();
                namecell.Value = CamNameTxt.Text.Trim();
                DataGridViewTextBoxCell TypecbCell = new DataGridViewTextBoxCell();
                TypecbCell.Value = CamTypeComBox.SelectedItem.ToString();


                newrow.Cells.Add(namecell);
                newrow.Cells.Add(TypecbCell);

                dataGridView1.Rows.Add(newrow);
                CamNameTxt.Clear();
            }
            else
            {
                MessageHelper.ShowError("添加相机失败");
            }
           
        }

        private void ModifyCamBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageHelper.ShowWarning("请选择相机列表中的任一相机再修改！");
                return;
            }
            int iRow = dataGridView1.SelectedRows[0].Index;
            if (iRow >= 0)
            {
                string camName = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                string camType= dataGridView1.Rows[iRow].Cells[2].Value.ToString();
                CameraManger.RemoveCameraInfo(camName);
                if (!string.IsNullOrEmpty(CamNameTxt.Text)&&CamTypeComBox.SelectedItem!=null)
                {
                    if(CameraManger.AddCameraInfo(CamNameTxt.Text,CamTypeComBox.SelectedItem.ToString()))
                    {
                        dataGridView1.Rows[iRow].Cells[0].Value = CamNameTxt.Text;
                        dataGridView1.Rows[iRow].Cells[1].Value = CamTypeComBox.Text;
                        CamNameTxt.Clear();
                        MessageHelper.ShowTips("修改相机成功！");
                        return;
                    }
                }
                CameraManger.AddCameraInfo(camName,camType);
              
                MessageHelper.ShowWarning("修改相机失败！");
             }
            CamNameTxt.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            if (iRow >= 0)
            {
                if (dataGridView1.Rows[iRow].Cells[0].Value != null && dataGridView1.Rows[iRow].Cells[1].Value != null)
                {
                    CamNameTxt.Text = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                    CamTypeComBox.Text = dataGridView1.Rows[iRow].Cells[1].Value.ToString();
                    
                }
            }
        }

        private void DeleteCamBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageHelper.ShowWarning("请选择相机列表中的任一相机再修改类型或者名称！");
                return;
            }
            int iRow = dataGridView1.SelectedRows[0].Index;
            if (iRow >= 0)
            {
                string camName = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                string camType = dataGridView1.Rows[iRow].Cells[2].Value.ToString();
                if( CameraManger.RemoveCameraInfo(camName))               
                   MessageHelper.ShowWarning("删除相机成功！");
                else
                {
                    MessageHelper.ShowWarning("删除相机失败！");
                }
            }
            CamNameTxt.Clear();
        }

        private void CameraManagerForm_Load(object sender, EventArgs e)
        {
            foreach (IVisionCameraInfo info in CameraManger.GetCameraInfoList())
            {
                DataGridViewRow gridrow = new DataGridViewRow();
                DataGridViewTextBoxCell namecell = new DataGridViewTextBoxCell();
                namecell.Value = info.UserID;
                DataGridViewTextBoxCell typecell = new DataGridViewTextBoxCell();
                typecell.Value = info._CameraType.ToString();
               
                gridrow.Cells.Add(namecell);
                gridrow.Cells.Add(typecell);
                dataGridView1.Rows.Add(gridrow);
            }
            dataGridView1.Invalidate();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            if (iRow >= 0)
            {
                if (dataGridView1.Rows[iRow].Cells[0].Value != null && dataGridView1.Rows[iRow].Cells[1].Value != null)
                {
                    string camName = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                    formSetting = CreatingHelper<CameraSetting>.GetSingleObject();
                    IVisionCameraInfo info = CameraManger.GetCameraInfoInstance(camName);
                    if (info!=null)
                    {
                        formSetting.Info = info;
                        formSetting.StartPosition = FormStartPosition.CenterScreen;
                        if (formSetting.IsShowing)
                        {
                            formSetting.IsShowing = false;
                            formSetting.Close();
                            System.Threading.Thread.Sleep(10);
                            formSetting.Show();
                            formSetting.IsShowing = true;
                        }
                        else
                        {
                            formSetting.Show();
                            formSetting.IsShowing = true;
                        }
                    }
                }
            }
        }
    }
}
