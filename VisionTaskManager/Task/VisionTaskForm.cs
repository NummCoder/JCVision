using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionUtil;

namespace VisionTaskManager
{
    public partial class VisionTaskForm : Form
    {
        private TaskToolManager formSetting { get; set; }
        public VisionTaskForm()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty( TaskNameTxt.Text))
            {
                MessageHelper.ShowWarning("请输入任务名称！");
                return;
            }
            if (TaskRunFormCombox.SelectedItem==null)
            {
                MessageHelper.ShowWarning("请选择任务显示界面！");
                return;
            }
            VisionTaskInfo info = new VisionTaskInfo() { TaskName=TaskNameTxt.Text.Trim(),TaskRunFormName=TaskRunFormCombox.SelectedItem.ToString()};
            if (!string.IsNullOrEmpty(TaskDescriptionTxt.Text))
            {
                info.TaskDescription = TaskDescriptionTxt.Text.Trim();
            }
            if (!VisionTaskManger.AddTaskInfo(info))
            {
                MessageHelper.ShowWarning("已存在相同名称的任务！");
                return;
            }
            if (VisionTaskManger.GetTaskInfoInstance(TaskNameTxt.Text) != null)
            {
                DataGridViewRow newrow = new DataGridViewRow();
                DataGridViewTextBoxCell namecell = new DataGridViewTextBoxCell();
                namecell.Value = TaskNameTxt.Text.Trim();
                DataGridViewTextBoxCell runFormCell = new DataGridViewTextBoxCell();
                runFormCell.Value = TaskRunFormCombox.SelectedItem.ToString();
                DataGridViewTextBoxCell dsCell = new DataGridViewTextBoxCell();
                dsCell.Value = TaskDescriptionTxt.ToString();

                newrow.Cells.Add(namecell);
                newrow.Cells.Add(runFormCell);
                newrow.Cells.Add(dsCell);
                dataGridView1.Rows.Add(newrow);                
            }
            else
            {
                MessageHelper.ShowError("添加任务失败");
            }
            TaskNameTxt.Clear();
            TaskDescriptionTxt.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VisionTaskManger.SaveDoc();
            TaskNameTxt.Clear();
            TaskDescriptionTxt.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            if (iRow >= 0)
            {
                if (dataGridView1.Rows[iRow].Cells[0].Value != null && dataGridView1.Rows[iRow].Cells[1].Value != null)
                {
                    TaskNameTxt.Text = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                    TaskRunFormCombox.Text = dataGridView1.Rows[iRow].Cells[1].Value.ToString();
                    if (dataGridView1.Rows[iRow].Cells[2].Value!=null)
                    {
                        TaskDescriptionTxt.Text = dataGridView1.Rows[iRow].Cells[2].Value.ToString();
                    }
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            if (iRow >= 0)
            {
                if (dataGridView1.Rows[iRow].Cells[0].Value != null && dataGridView1.Rows[iRow].Cells[1].Value != null)
                {
                    string taskName = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                    formSetting = CreatingHelper<TaskToolManager>.GetSingleObject();
                    VisionTaskInfo info = VisionTaskManger.GetTaskInfoInstance(taskName);
                    if (info != null)
                    {
                        formSetting.Info = info;
                        formSetting.StartPosition = FormStartPosition.CenterScreen;
                        if (formSetting.IsShowing)
                        {
                            formSetting.IsShowing = false;
                            formSetting.Close();
                            System.Threading.Thread.Sleep(5);
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

        private void ModifyBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageHelper.ShowWarning("请选择任务列表中的任务再修改！");
                return;
            }
            int iRow = dataGridView1.SelectedRows[0].Index;
            if (iRow >= 0)
            {
                if (dataGridView1.Rows[iRow].Cells[0].Value==null|| dataGridView1.Rows[iRow].Cells[1].Value==null)
                {
                    return;
                }
                string taskName = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                string taskRunformName = dataGridView1.Rows[iRow].Cells[1].Value.ToString();
                
                string taskDescription = string.Empty;
                if (dataGridView1.Rows[iRow].Cells[2].Value!=null)
                {
                    taskDescription = dataGridView1.Rows[iRow].Cells[2].Value.ToString();                  
                }
                VisionTaskManger.RemoveTaskInfo(taskName);
                if (!string.IsNullOrEmpty(TaskNameTxt.Text) && TaskRunFormCombox.SelectedItem != null)
                {
                    VisionTaskInfo info = new VisionTaskInfo() { TaskName=TaskNameTxt.Text.Trim(),TaskRunFormName=TaskRunFormCombox.SelectedItem.ToString()};
                    if (!string.IsNullOrEmpty(TaskDescriptionTxt.Text))
                    {
                        info.TaskDescription = TaskDescriptionTxt.Text;
                    }
                    if (VisionTaskManger.AddTaskInfo(info))
                    {
                        dataGridView1.Rows[iRow].Cells[0].Value = TaskNameTxt.Text;
                        dataGridView1.Rows[iRow].Cells[1].Value = TaskRunFormCombox.Text;
                        dataGridView1.Rows[iRow].Cells[1].Value = TaskDescriptionTxt.Text;
                        TaskNameTxt.Clear();
                        TaskDescriptionTxt.Clear();
                        MessageHelper.ShowTips("修改任务成功！");
                        return;
                    }
                }
                VisionTaskManger.AddTaskInfo(new VisionTaskInfo() { TaskName=taskName,TaskRunFormName=taskRunformName,TaskDescription=taskDescription});

                MessageHelper.ShowWarning("修改任务失败！");
            }
            TaskNameTxt.Clear();
            TaskDescriptionTxt.Clear();
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageHelper.ShowWarning("请选择任务列表中的任一任务删除！");
                return;
            }
            int iRow = dataGridView1.SelectedRows[0].Index;
            if (iRow >= 0)
            {
                string taskName = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                string taskRunformName = dataGridView1.Rows[iRow].Cells[1].Value.ToString();
                if (VisionTaskManger.RemoveTaskInfo(taskName))
                    MessageHelper.ShowWarning("删除任务成功！");
                else
                {
                    MessageHelper.ShowWarning("删除任务失败！");
                }
            }
            TaskNameTxt.Clear();
            TaskDescriptionTxt.Clear();

        }

        private void VisionTaskForm_Load(object sender, EventArgs e)
        {
            //加载任务列表
        }
    }
}
