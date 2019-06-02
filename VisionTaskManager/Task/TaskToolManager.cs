using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using VisionUtil;
using VisionInterface;
using System.Reflection;
using HalconDotNet;
using VisionDisplayTool;

namespace VisionTaskManager
{
    public partial class TaskToolManager : Form
    {
        public bool IsShowing { get; set; }

        private VisionTaskInfo info;

        private VisionTask task;
        public VisionTaskInfo Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                task = VisionTaskManger.GetVisionTask(info.TaskName);
                RefreshControlValue();
                #region 加载任务工具列表

                #endregion
                //绑定任务事件
                if (task != null)
                {
                    task.RegisteRunTaskEvent(UpdateTaskResult);
                }
            }
        }

        public HImage currentImage { get; private set; }
        public DisplayControl displayForm { get; private set; }
        /// <summary>
        /// 记录当前需要插入位置
        /// </summary>
        private int TaskToolDatagridIndex;
        private void RefreshControlValue()
        {
            if (info==null)
            {
                return;
            }
            this.TaskName.Text = info.TaskName;
            this.SaveOKCBox.Checked = info.IsSaveOKImage;
            this.SaveNGCBox.Checked = info.IsSaveNGImage;

        }

        public TaskToolManager()
        {
            InitializeComponent();
            displayForm = new DisplayControl(ImagePanel);

        }
        public TaskToolManager(VisionTaskInfo info)
        {
            InitializeComponent();
            this.Info = info;
            displayForm = new DisplayControl(ImagePanel);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.info.IsSaveOKImage = this.SaveOKCBox.Checked;
        }

        private void TaskToolManager_Load(object sender, EventArgs e)
        {
            #region 加载工具列表
            List<FileInfo> fileInfos = new List<FileInfo>();
            //获取所有工具
            if (!Directory.Exists(@".\\VisionTools\"))
            {
                if (MessageHelper.ShowWarning("没有找到任何图像工具文件，是否停用该软件？") == DialogResult.OK)
                {
                    //关闭软件

                }
            }
            string[] filePath = Directory.GetFiles(@".\\VisionTools\");
            if (filePath.Length > 0)
            {
                for (int i = 0; i < filePath.Length; i++)
                {
                    FileInfo info = new FileInfo(filePath[i]);
                    if (info.Exists && info.Extension == ".dll")
                    {
                        fileInfos.Add(info);
                    }
                }
            }
            foreach (var item in fileInfos)
            {
                string toolType = string.Empty;
                string toolDescription = item.FullName;
                //通过反射获取工具名称并添加到列表中
                var value = CreatingHelper<IToolInfo>.CreateInstance(item.FullName, item.Name.Substring(0, item.Name.Length - 4), item.Name.Substring(6, item.Name.Length - 10));
                if (value != null)
                {
                    DataGridViewRow newrow = new DataGridViewRow();
                    DataGridViewTextBoxCell namecell = new DataGridViewTextBoxCell();
                    namecell.Value = value.GetToolType();
                    DataGridViewTextBoxCell desCell = new DataGridViewTextBoxCell();
                    desCell.Value = toolDescription;
                    newrow.Cells.Add(namecell);
                    newrow.Cells.Add(desCell);
                    ToolDataGridView.Rows.Add(newrow);
                }
            }
            #endregion
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VisionTaskManger.StartTask(this.info.TaskName);
            UpdateTaskResult();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text=="连续执行")
            {
                button2.Text = "停止执行";
                VisionTaskManger.StartLoopTask(this.info.TaskName);
            }
            else if (button2.Text=="停止执行")
            {
                button2.Text = "连续执行";
                VisionTaskManger.StopTask(this.info.TaskName);

            }           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VisionTaskManger.StopTask(this.info.TaskName);
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

        private void grabBtn_Click(object sender, EventArgs e)
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
        /// <summary>
        /// 把当前图像保存到固定文件夹中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.currentImage != null)
            {
                try
                {
                    HImage image = currentImage.CopyImage();
                    if (!Directory.Exists($@".//{this.info.TaskName}/"))
                    {
                        Directory.CreateDirectory($@".//{this.info.TaskName}/");
                    }
                    string fileName = $@".//{this.info.TaskName}/" + this.info.TaskName + ".png";
                    HOperatorSet.WriteImage(image, "png", 0, fileName);
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowError(ex.ToString());
                    return;
                }
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(toolNameTxt.Text))
            {
                MessageHelper.ShowWarning("请先输入工具名称");
                return;
            }
            if (ToolDataGridView.SelectedRows.Count==1)
            {
                int iRow = ToolDataGridView.SelectedRows[0].Index;
                if (iRow >= 0)
                {
                    if (ToolDataGridView.Rows[iRow].Cells[0].Value == null || ToolDataGridView.Rows[iRow].Cells[1].Value == null)
                    {                        
                        return;
                    }
                    string toolName = toolNameTxt.Text.Trim();
                    string toolType = ToolDataGridView.Rows[iRow].Cells[0].Value.ToString();
                    string toolDllPath = ToolDataGridView.Rows[iRow].Cells[1].Value.ToString();
                    FileInfo fileInfo = new FileInfo(toolDllPath);
                    if (fileInfo.Exists)
                    {
                        try
                        {
                            object[] infoParam = new object[] { toolName };
                            var toolInfo = CreatingHelper<IToolInfo>.CreateInstance(fileInfo.FullName, fileInfo.Name.Substring(0, fileInfo.Name.Length - 4), fileInfo.Name.Substring(6, fileInfo.Name.Length - 10) + "Info", infoParam);
                            if (toolInfo!=null)
                            {                              
                                object[] toolParam = new object[] { toolInfo};
                                //添加工具到任务中
                                var tool= CreatingHelper<ITool>.CreateInstance(fileInfo.FullName, fileInfo.Name.Substring(0, fileInfo.Name.Length - 4), fileInfo.Name.Substring(6, fileInfo.Name.Length - 10), toolParam);
                                if (tool !=null)
                                {
                                    if (this.info!=null&&this.task!=null)
                                    {
                                        ///刷新工具界面，可以通过datagridview的插入来更新
                                        if (this.info.AddToolInfo(toolInfo)&& this.task.AddTool(tool))
                                        {
                                            UpdateToolGridView();
                                        }
                                    }
                                    else
                                    {
                                        MessageHelper.ShowError("添加工具失败，不存在该任务！");
                                    }
                                }
                                else
                                {
                                    MessageHelper.ShowError("添加工具失败，生成工具出错！");

                                }
                            }
                            else
                            {
                                MessageHelper.ShowError("添加工具失败，生成工具信息出错！");
                            }

                        }
                        catch (Exception ex)
                        {

                            MessageHelper.ShowError($"{ex.ToString()}");
                        }
                    }
                    else
                    {
                        MessageHelper.ShowError("工具插件不存在，请检查该工具的DLL文件！");
                    }
                }
            }
        }

        private void InsertBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(toolNameTxt.Text))
            {
                MessageHelper.ShowWarning("请先输入工具名称");
                return;
            }
            if (ToolDataGridView.SelectedRows.Count == 1)
            {
                int iRow = ToolDataGridView.SelectedRows[0].Index;
                if (iRow >= 0)
                {
                    if (ToolDataGridView.Rows[iRow].Cells[0].Value == null || ToolDataGridView.Rows[iRow].Cells[1].Value == null)
                    {
                        return;
                    }
                    string toolName = toolNameTxt.Text.Trim();
                    string toolType = ToolDataGridView.Rows[iRow].Cells[0].Value.ToString();
                    string toolDllPath = ToolDataGridView.Rows[iRow].Cells[1].Value.ToString();
                    FileInfo fileInfo = new FileInfo(toolDllPath);
                    if (fileInfo.Exists)
                    {
                        try
                        {
                            object[] infoParam = new object[] { toolName };
                            var toolInfo = CreatingHelper<IToolInfo>.CreateInstance(fileInfo.FullName, fileInfo.Name.Substring(0, fileInfo.Name.Length - 4), fileInfo.Name.Substring(6, fileInfo.Name.Length - 10) + "Info", infoParam);
                            if (toolInfo != null)
                            {
                                object[] toolParam = new object[] { toolInfo };
                                //添加工具到任务中
                                var tool = CreatingHelper<ITool>.CreateInstance(fileInfo.FullName, fileInfo.Name.Substring(0, fileInfo.Name.Length - 4), fileInfo.Name.Substring(6, fileInfo.Name.Length - 10), toolParam);
                                if (tool != null)
                                {
                                    if (this.info != null && this.task != null)
                                    {
                                        if (this.task.InsertTool(TaskToolDatagridIndex, tool)&& this.info.InsertToolInfo(TaskToolDatagridIndex, toolInfo))
                                        {
                                            //刷新工具界面
                                            UpdateToolGridView();
                                        }
                                        else
                                        {
                                            MessageHelper.ShowError("插入工具失败！");
                                        }
                                    }
                                    else
                                    {
                                        MessageHelper.ShowError("插入工具失败，不存在该任务！");
                                    }
                                }
                                else
                                {
                                    MessageHelper.ShowError("插入工具失败，生成工具出错！");
                                }
                            }
                            else
                            {
                                MessageHelper.ShowError("插入工具失败，生成工具信息出错！");
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageHelper.ShowError($"{ex.ToString()}");
                        }
                    }
                    else
                    {
                        MessageHelper.ShowError("工具插件不存在，请检查该工具的DLL文件！");
                    }
                }
            }
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            if (ToolDataGridView.SelectedRows.Count == 1)
            {
                int iRow = ToolDataGridView.SelectedRows[0].Index;
                if (iRow >= 0)
                {
                    if (ToolDataGridView.Rows[iRow].Cells[0].Value!=null)
                    {
                        string toolName = ToolDataGridView.Rows[iRow].Cells[0].Value.ToString();
                        if (this.info.RemoveToolInfo(toolName)&& this.task.RemoverTool(toolName))
                        {
                            ToolDataGridView.Rows.Remove(ToolDataGridView.SelectedRows[0]);
                        }
                        else
                        {
                            MessageHelper.ShowWarning("移除工具失败！");
                        }                       
                    }                   
                }
            }
        }
       

        private void MoveUpBtn_Click(object sender, EventArgs e)
        {
            if (ToolDataGridView.SelectedRows.Count == 1)
            {
                int iRow = ToolDataGridView.SelectedRows[0].Index;
                if (iRow >= 0)
                {
                    if (ToolDataGridView.Rows[iRow].Cells[0].Value != null)
                    {
                        string toolName = ToolDataGridView.Rows[iRow].Cells[0].Value.ToString();
                        int currentIndex = ToolDataGridView.CurrentRow.Index;
                        var temp = this.info.GetToolInfo(currentIndex);
                        if (temp!=null)
                        {
                            this.info.TaskToolsInfo[currentIndex] = this.info.TaskToolsInfo[currentIndex-1];
                            this.info.TaskToolsInfo[currentIndex - 1] = temp;
                            //更新列表
                            UpdateToolGridView();
                        }
                    }
                }
            }
        }
        private void MoveDownBtn_Click(object sender, EventArgs e)
        {
            if (ToolDataGridView.SelectedRows.Count == 1)
            {
                int iRow = ToolDataGridView.SelectedRows[0].Index;
                if (iRow >= 0)
                {
                    if (ToolDataGridView.Rows[iRow].Cells[0].Value != null)
                    {
                        string toolName = ToolDataGridView.Rows[iRow].Cells[0].Value.ToString();
                        int currentIndex = ToolDataGridView.CurrentRow.Index;
                        var temp = this.info.GetToolInfo(currentIndex);
                        if (temp != null)
                        {
                            this.info.TaskToolsInfo[currentIndex] = this.info.TaskToolsInfo[currentIndex + 1];
                            this.info.TaskToolsInfo[currentIndex + 1] = temp;
                            //更新列表
                            UpdateToolGridView();
                        }
                    }
                }
            }
        }
        private void UpdateToolGridView()
        {
            if (ToolDataGridView.Rows.Count>0)
            {
                ToolDataGridView.Rows.Clear();
            }
            foreach (var item in this.info.TaskToolsInfo)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell toolNameCell = new DataGridViewTextBoxCell();
                toolNameCell.Value = item.ToolName;
                DataGridViewTextBoxCell toolTypeCell = new DataGridViewTextBoxCell();
                toolTypeCell.Value = item.GetToolType();
                DataGridViewTextBoxCell toolResult = new DataGridViewTextBoxCell();
                toolResult.Value = "false";
                DataGridViewTextBoxCell toolExecuteTime = new DataGridViewTextBoxCell();
                toolResult.Value = "0.0";
                DataGridViewTextBoxCell toolResultX = new DataGridViewTextBoxCell();
                toolResultX.Value = "0.0";
                DataGridViewTextBoxCell toolResultY = new DataGridViewTextBoxCell();
                toolResultY.Value = "0.0";
                DataGridViewTextBoxCell toolResultAngle = new DataGridViewTextBoxCell();
                toolResultAngle.Value = "0.0";
                DataGridViewTextBoxCell toolErrorMessage = new DataGridViewTextBoxCell();
                toolErrorMessage.Value = string.Empty;
                row.Cells.AddRange(new DataGridViewCell[] { toolNameCell, toolTypeCell , toolResult , toolExecuteTime , toolResultX , toolResultY , toolResultAngle , toolErrorMessage });
                ToolDataGridView.Rows.Add(row);
            }
        }
        /// <summary>
        /// 更新任务执行的结果
        /// </summary>
        private void UpdateTaskResult()
        {
            if (ToolDataGridView.InvokeRequired)
            {
                Action action = () => {
                    UpdateTaskResult();
                };
                this.Invoke(action);
            }
            if (ToolDataGridView.Rows.Count > 0)
            {
                for (int i = 0; i < ToolDataGridView.Rows.Count; i++)
                {
                    foreach (var item in this.task.ToolResultDic.Values)
                    {
                        if (ToolDataGridView.Rows[i].Cells[0].Value!=null&&ToolDataGridView.Rows[i].Cells[0].Value.ToString()==item.ResultName)
                        {
                            try
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                DataGridViewTextBoxCell toolNameCell = new DataGridViewTextBoxCell();
                                toolNameCell.Value = item.ResultName;
                                DataGridViewTextBoxCell toolTypeCell = new DataGridViewTextBoxCell();
                                toolTypeCell.Value = this.info.GetToolInfo(item.ResultName).GetToolType();
                                DataGridViewTextBoxCell toolResult = new DataGridViewTextBoxCell();
                                toolResult.Value = item.IsSuccess.ToString();
                                DataGridViewTextBoxCell toolExecuteTime = new DataGridViewTextBoxCell();
                                toolExecuteTime.Value =item.ElapsedTime.ToString();
                                DataGridViewTextBoxCell toolResultX = new DataGridViewTextBoxCell();
                                toolResultX.Value = item.ImageX;
                                DataGridViewTextBoxCell toolResultY = new DataGridViewTextBoxCell();
                                toolResultY.Value =item.ImageY;
                                DataGridViewTextBoxCell toolResultAngle = new DataGridViewTextBoxCell();
                                toolResultAngle.Value = item.ImageAngle;
                                DataGridViewTextBoxCell toolErrorMessage = new DataGridViewTextBoxCell();
                                toolErrorMessage.Value = item.Errormessage;
                                row.Cells.AddRange(new DataGridViewCell[] { toolNameCell, toolTypeCell, toolResult, toolExecuteTime, toolResultX, toolResultY, toolResultAngle, toolErrorMessage });
                                ToolDataGridView.Rows.Add(row);
                            }
                            catch (Exception ex)
                            {
                                MessageHelper.ShowError(ex.ToString());
                                continue;
                            }
                        }
                    }
                }
            }
           
        }

        private void SaveNGCBox_CheckedChanged(object sender, EventArgs e)
        {
            this.info.IsSaveNGImage = SaveOKCBox.Checked;
        }
    }
}
