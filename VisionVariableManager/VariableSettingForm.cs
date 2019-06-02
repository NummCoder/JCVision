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
using VisionTaskManager;
using VisionUtil;
using VisionUtil.EnumStrucks;

namespace VisionVariableManager
{
    public partial class VariableSettingForm : Form
    {
        public VariableSettingForm()
        {
            InitializeComponent();
        }
        private void VariableSettingForm_Load(object sender, EventArgs e)
        {
            #region Init Control Value
            foreach (var type in Enum.GetValues(typeof(VariableType)))
            {
                TypeComb.Items.Add(type);
            }
            foreach (var range in Enum.GetValues(typeof(VariableRange)))
            {
                UseRangeComb.Items.Add(range);
            }
            foreach (var task in VisionTaskManger.GetTaskInfoList())
            {
                TaskComb.Items.Add(task.TaskName);
            }
            #endregion

            #region 加载变量

            #endregion
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (!JudgeControlValue())
            {
                return;
            }
            string variableName = NameTxt.Text.Trim();
            VariableType variableType = (VariableType)TypeComb.SelectedIndex;
            VariableRange range = (VariableRange)UseRangeComb.SelectedItem;
            if (range == VariableRange.任务变量)
            {
                if (TaskComb.SelectedItem == null)
                {
                    MessageHelper.ShowWarning("设置为任务变量时，请选择绑定的任务！");
                    return;
                }
            }
            string taskName = TaskComb.SelectedItem != null ? TaskComb.SelectedItem.ToString() : string.Empty;
            string annotation = string.IsNullOrEmpty(AnnotationTxt.Text) ? string.Empty : AnnotationTxt.Text.Trim();
            VariableInfoBase variable = null;
            switch (variableType)
            {
                case VariableType.Double:
                    variable = new VariableInfo<double>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                    break;
                case VariableType.String:
                    variable = new VariableInfo<string>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                    break;
                case VariableType.Image:
                    variable = new VariableInfo<HImage>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                    break;
                case VariableType.Region:
                    variable = new VariableInfo<HRegion>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                    break;
                case VariableType.XLD:
                    variable = new VariableInfo<HXLDCont>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                    break;
                default:
                    break;
            }
            if (variable != null)
            {
                if (variable.Range == VariableRange.任务变量)
                {
                    if (VariableManager.CheckTaskVarUnique(variable.TaskName, variable.VariableName))
                    {
                        VariableManager.AddTaskVarInfo(variable);
                    }
                    else
                    {
                        MessageHelper.ShowTips("添加任务变量失败，变量名称重复！");
                        return;
                    }
                }
                else if (variable.Range == VariableRange.全局变量)
                {
                    if (VariableManager.CheckGlobalVarUnique(variable.VariableName))
                    {
                        VariableManager.AddGlobalVarInfo(variable);
                    }
                    else
                    {
                        MessageHelper.ShowTips("添加全局变量失败，变量名称重复！");
                        return;
                    }
                }
                else
                {
                    if (VariableManager.CheckSystemVarUnique(variable.VariableName))
                    {
                        VariableManager.AddSystemVarInfo(variable);
                    }
                    else
                    {
                        MessageHelper.ShowTips("添加系统变量失败，变量名称重复！");
                        return;
                    }
                }
                //更新列表
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                nameCell.Value = variable.VariableName;
                DataGridViewTextBoxCell typeCell = new DataGridViewTextBoxCell();
                typeCell.Value = variable.variableType.ToString();
                DataGridViewTextBoxCell rangeCell = new DataGridViewTextBoxCell();
                rangeCell.Value = variable.Range.ToString();
                DataGridViewTextBoxCell taskNameCell = new DataGridViewTextBoxCell();
                taskNameCell.Value = variable.TaskName;
                DataGridViewTextBoxCell annotationCell = new DataGridViewTextBoxCell();
                annotationCell.Value = variable.VariableAnnotation;
                row.Cells.AddRange(new DataGridViewCell[] { nameCell, typeCell, rangeCell, taskNameCell, annotationCell });
                dataGridView1.Rows.Add(row);

            }

        }
        private bool JudgeControlValue()
        {
            if (string.IsNullOrEmpty(NameTxt.Text.Trim()))
            {
                MessageHelper.ShowWarning("变量名称不能为空!");
                return false;
            }
            if (TypeComb.SelectedItem == null)
            {
                MessageHelper.ShowWarning("变量类型不能为空!");
                return false;
            }
            if (UseRangeComb.SelectedItem == null)
            {
                MessageHelper.ShowWarning("变量范围不能为空!");
                return false;
            }
            return true;
        }

        private void UseRangeComb_SelectedIndexChanged(object sender, EventArgs e)
        {
            label5.Visible = true;
            TaskComb.Visible = true;
        }



        private void SaveBtn_Click(object sender, EventArgs e)
        {
            VariableManager.SaveDoc();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageHelper.ShowWarning("请选择变量列表中的任一变量删除！");
                return;
            }
            int iRow = dataGridView1.SelectedRows[0].Index;
            if (iRow >= 0)
            {
                #region 删除当前变量
                string varName = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                string varRange = dataGridView1.Rows[iRow].Cells[2].Value.ToString();
                string varTaskName = dataGridView1.Rows[iRow].Cells[3].Value == null ? string.Empty : dataGridView1.Rows[iRow].Cells[3].Value.ToString();
                if (varRange == VariableRange.任务变量.ToString() && !string.IsNullOrEmpty(varTaskName))
                {
                    if (VariableManager.CheckTaskVarUnique(varTaskName, varName))
                    {
                        if (VariableManager.RemoveTaskVarInfo(varTaskName, varName))
                        {
                            MessageHelper.ShowTips("删除任务变量成功");
                        }
                        else
                        {
                            MessageHelper.ShowTips("删除任务变量失败");
                        }
                    }
                }
                else if (varRange == VariableRange.全局变量.ToString())
                {
                    if (VariableManager.CheckGlobalVarUnique(varName))
                    {
                        if (VariableManager.RemoveGlobalVarInfo(varName))
                        {
                            MessageHelper.ShowTips("删除全局变量成功");
                        }
                        else
                        {
                            MessageHelper.ShowTips("删除全局变量失败");
                        }
                    }
                }
                else
                {
                    if (VariableManager.CheckSystemVarUnique(varName))
                    {
                        if (VariableManager.RemoveSystemVarInfo(varName))
                        {
                            MessageHelper.ShowTips("删除系统变量成功");
                        }
                        else
                        {
                            MessageHelper.ShowTips("删除系统变量失败");
                        }
                    }
                }
                #endregion

                #region 增加修改后的变量
                string variableName = NameTxt.Text.Trim();
                VariableType variableType = (VariableType)TypeComb.SelectedIndex;
                VariableRange range = (VariableRange)UseRangeComb.SelectedItem;
                if (range == VariableRange.任务变量)
                {
                    if (TaskComb.SelectedItem == null)
                    {
                        MessageHelper.ShowWarning("设置为任务变量时，请选择绑定的任务！");
                        return;
                    }
                }
                string taskName = TaskComb.SelectedItem.ToString();
                string annotation = string.IsNullOrEmpty(AnnotationTxt.Text) ? string.Empty : AnnotationTxt.Text.Trim();
                VariableInfoBase variable = null;
                switch (variableType)
                {
                    case VariableType.Double:
                        variable = new VariableInfo<double>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                        break;
                    case VariableType.String:
                        variable = new VariableInfo<string>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                        break;
                    case VariableType.Image:
                        variable = new VariableInfo<HImage>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                        break;
                    case VariableType.Region:
                        variable = new VariableInfo<HRegion>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                        break;
                    case VariableType.XLD:
                        variable = new VariableInfo<HXLDCont>() { VariableName = variableName, variableType = variableType, Range = range, TaskName = taskName, VariableAnnotation = annotation };
                        break;
                    default:
                        break;
                }
                if (variable != null)
                {
                    if (variable.Range == VariableRange.任务变量)
                    {
                        if (VariableManager.CheckTaskVarUnique(variable.TaskName, variable.VariableName))
                        {
                            VariableManager.AddTaskVarInfo(variable);
                        }
                        else
                        {
                            MessageHelper.ShowTips("修改变量失败！");
                            return;
                        }
                    }
                    else if (variable.Range == VariableRange.全局变量)
                    {
                        if (VariableManager.CheckGlobalVarUnique(variable.VariableName))
                        {
                            VariableManager.AddGlobalVarInfo(variable);
                        }
                        else
                        {
                            MessageHelper.ShowTips("修改变量失败！");
                            return;
                        }
                    }
                    else
                    {
                        if (VariableManager.CheckSystemVarUnique(variable.VariableName))
                        {
                            VariableManager.AddSystemVarInfo(variable);
                        }
                        else
                        {
                            MessageHelper.ShowTips("修改变量失败！");
                            return;
                        }
                    }
                    //更新变量列表值
                    dataGridView1.Rows[iRow].Cells[0].Value = variable.VariableName;
                    dataGridView1.Rows[iRow].Cells[1].Value = variable.variableType.ToString();
                    dataGridView1.Rows[iRow].Cells[2].Value = variable.Range.ToString();
                    dataGridView1.Rows[iRow].Cells[3].Value = variable.TaskName;
                    dataGridView1.Rows[iRow].Cells[4].Value = variable.VariableAnnotation;
                    MessageHelper.ShowTips("修改变量成功！");
                    return;
                }
                MessageHelper.ShowTips("修改变量失败！");
                #endregion
            }

        }

        private void ModifyBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageHelper.ShowWarning("请选择变量列表中的变量再修改！");
                return;
            }
            int iRow = dataGridView1.SelectedRows[0].Index;
            if (iRow >= 0)
            {
                if (dataGridView1.Rows[iRow].Cells[0].Value == null || dataGridView1.Rows[iRow].Cells[1].Value == null)
                {
                    return;
                }
                string varName = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                string varType = dataGridView1.Rows[iRow].Cells[1].Value.ToString();
                string varRange = dataGridView1.Rows[iRow].Cells[2].Value.ToString();
                string varTaskName = dataGridView1.Rows[iRow].Cells[3].Value != null ? dataGridView1.Rows[iRow].Cells[3].Value.ToString() : string.Empty;
                string varAnnotation = dataGridView1.Rows[iRow].Cells[4].Value != null ? dataGridView1.Rows[iRow].Cells[4].Value.ToString() : string.Empty;
                if (!JudgeControlValue())
                {
                    return;
                }
                if (varRange == VariableRange.任务变量.ToString() && !string.IsNullOrEmpty(varTaskName))
                {
                    if (VariableManager.CheckTaskVarUnique(varTaskName, varName))
                    {
                        if (!VariableManager.RemoveTaskVarInfo(varTaskName, varName))
                        {
                            MessageHelper.ShowTips("修改任务变量失败");
                            return;
                        }
                    }
                    else
                    {
                        MessageHelper.ShowTips("不存在该变量");
                        dataGridView1.Rows.RemoveAt(iRow);
                        return;
                    }
                }
                else if (varRange == VariableRange.全局变量.ToString())
                {
                    if (VariableManager.CheckGlobalVarUnique(varName))
                    {
                        if (!VariableManager.RemoveGlobalVarInfo(varName))
                        {
                            MessageHelper.ShowTips("修改全局变量失败");
                            return;
                        }
                    }
                    else
                    {
                        MessageHelper.ShowTips("不存在该变量");
                        dataGridView1.Rows.RemoveAt(iRow);
                        return;
                    }
                }
                else
                {
                    if (VariableManager.CheckSystemVarUnique(varName))
                    {
                        if (!VariableManager.RemoveSystemVarInfo(varName))
                        {
                            MessageHelper.ShowTips("修改系统变量失败");
                            return;
                        }
                    }
                    else
                    {
                        MessageHelper.ShowTips("不存在该变量");
                        dataGridView1.Rows.RemoveAt(iRow);
                        return;
                    }
                }
                
            }
        }
            private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                int iRow = e.RowIndex;
                if (iRow >= 0)
                {
                    if (dataGridView1.Rows[iRow].Cells[0].Value != null && dataGridView1.Rows[iRow].Cells[1].Value != null)
                    {
                        NameTxt.Text = dataGridView1.Rows[iRow].Cells[0].Value.ToString();
                        TypeComb.Text = dataGridView1.Rows[iRow].Cells[1].Value.ToString();
                        UseRangeComb.Text = dataGridView1.Rows[iRow].Cells[2].Value.ToString();
                        TaskComb.Text = dataGridView1.Rows[iRow].Cells[3].Value != null ? dataGridView1.Rows[iRow].Cells[3].Value.ToString() : string.Empty;
                        AnnotationTxt.Text = dataGridView1.Rows[iRow].Cells[4].Value != null ? dataGridView1.Rows[iRow].Cells[4].Value.ToString() : string.Empty;
                    }
                }
            }
        }   
}
