namespace VisionGrayMatchTool
{
    partial class GrayMatchToolForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.operatePanel = new System.Windows.Forms.Panel();
            this.ExecuteToolBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.currentFindRegionComb = new System.Windows.Forms.ComboBox();
            this.FindRegionShapeComb = new System.Windows.Forms.ComboBox();
            this.deleteCurrentRegionBtn = new System.Windows.Forms.Button();
            this.deleteAllRegionBtn = new System.Windows.Forms.Button();
            this.modifyFindRegionBtn = new System.Windows.Forms.Button();
            this.createFindRegionBtn = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.DrawROIGroup = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ROINameTxb = new System.Windows.Forms.TextBox();
            this.CurrentModelComb = new System.Windows.Forms.ComboBox();
            this.ROIShapeComb = new System.Windows.Forms.ComboBox();
            this.DeleteCurBtn = new System.Windows.Forms.Button();
            this.DeleteAllBtn = new System.Windows.Forms.Button();
            this.ModifyBtn = new System.Windows.Forms.Button();
            this.CreateNewBtn = new System.Windows.Forms.Button();
            this.ImageGroup = new System.Windows.Forms.GroupBox();
            this.LoadTaskImageBtn = new System.Windows.Forms.Button();
            this.camerasComb = new System.Windows.Forms.ComboBox();
            this.openFileBtn = new System.Windows.Forms.Button();
            this.grabBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.operatePanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.DrawROIGroup.SuspendLayout();
            this.ImageGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // operatePanel
            // 
            this.operatePanel.Controls.Add(this.button1);
            this.operatePanel.Controls.Add(this.button2);
            this.operatePanel.Controls.Add(this.ExecuteToolBtn);
            this.operatePanel.Controls.Add(this.SaveBtn);
            this.operatePanel.Controls.Add(this.groupBox1);
            this.operatePanel.Controls.Add(this.propertyGrid1);
            this.operatePanel.Controls.Add(this.DrawROIGroup);
            this.operatePanel.Controls.Add(this.ImageGroup);
            this.operatePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.operatePanel.Location = new System.Drawing.Point(0, 0);
            this.operatePanel.Name = "operatePanel";
            this.operatePanel.Size = new System.Drawing.Size(351, 748);
            this.operatePanel.TabIndex = 2;
            // 
            // ExecuteToolBtn
            // 
            this.ExecuteToolBtn.Location = new System.Drawing.Point(12, 758);
            this.ExecuteToolBtn.Name = "ExecuteToolBtn";
            this.ExecuteToolBtn.Size = new System.Drawing.Size(80, 26);
            this.ExecuteToolBtn.TabIndex = 11;
            this.ExecuteToolBtn.Text = "执行工具";
            this.ExecuteToolBtn.UseVisualStyleBackColor = true;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(208, 758);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 26);
            this.SaveBtn.TabIndex = 10;
            this.SaveBtn.Text = "保存模板";
            this.SaveBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.currentFindRegionComb);
            this.groupBox1.Controls.Add(this.FindRegionShapeComb);
            this.groupBox1.Controls.Add(this.deleteCurrentRegionBtn);
            this.groupBox1.Controls.Add(this.deleteAllRegionBtn);
            this.groupBox1.Controls.Add(this.modifyFindRegionBtn);
            this.groupBox1.Controls.Add(this.createFindRegionBtn);
            this.groupBox1.Location = new System.Drawing.Point(3, 288);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 121);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "搜索框操作";
            // 
            // currentFindRegionComb
            // 
            this.currentFindRegionComb.FormattingEnabled = true;
            this.currentFindRegionComb.Location = new System.Drawing.Point(205, 92);
            this.currentFindRegionComb.Name = "currentFindRegionComb";
            this.currentFindRegionComb.Size = new System.Drawing.Size(94, 20);
            this.currentFindRegionComb.TabIndex = 9;
            // 
            // FindRegionShapeComb
            // 
            this.FindRegionShapeComb.FormattingEnabled = true;
            this.FindRegionShapeComb.Location = new System.Drawing.Point(205, 41);
            this.FindRegionShapeComb.Name = "FindRegionShapeComb";
            this.FindRegionShapeComb.Size = new System.Drawing.Size(94, 20);
            this.FindRegionShapeComb.TabIndex = 8;
            // 
            // deleteCurrentRegionBtn
            // 
            this.deleteCurrentRegionBtn.Location = new System.Drawing.Point(104, 92);
            this.deleteCurrentRegionBtn.Name = "deleteCurrentRegionBtn";
            this.deleteCurrentRegionBtn.Size = new System.Drawing.Size(80, 23);
            this.deleteCurrentRegionBtn.TabIndex = 4;
            this.deleteCurrentRegionBtn.Text = "删除当前";
            this.deleteCurrentRegionBtn.UseVisualStyleBackColor = true;
            this.deleteCurrentRegionBtn.Click += new System.EventHandler(this.deleteCurrentRegionBtn_Click);
            // 
            // deleteAllRegionBtn
            // 
            this.deleteAllRegionBtn.Location = new System.Drawing.Point(104, 38);
            this.deleteAllRegionBtn.Name = "deleteAllRegionBtn";
            this.deleteAllRegionBtn.Size = new System.Drawing.Size(80, 23);
            this.deleteAllRegionBtn.TabIndex = 3;
            this.deleteAllRegionBtn.Text = "删除所有";
            this.deleteAllRegionBtn.UseVisualStyleBackColor = true;
            this.deleteAllRegionBtn.Click += new System.EventHandler(this.deleteAllRegionBtn_Click);
            // 
            // modifyFindRegionBtn
            // 
            this.modifyFindRegionBtn.Location = new System.Drawing.Point(9, 92);
            this.modifyFindRegionBtn.Name = "modifyFindRegionBtn";
            this.modifyFindRegionBtn.Size = new System.Drawing.Size(80, 23);
            this.modifyFindRegionBtn.TabIndex = 2;
            this.modifyFindRegionBtn.Text = "修改";
            this.modifyFindRegionBtn.UseVisualStyleBackColor = true;
            this.modifyFindRegionBtn.Click += new System.EventHandler(this.modifyFindRegionBtn_Click);
            // 
            // createFindRegionBtn
            // 
            this.createFindRegionBtn.Location = new System.Drawing.Point(9, 38);
            this.createFindRegionBtn.Name = "createFindRegionBtn";
            this.createFindRegionBtn.Size = new System.Drawing.Size(80, 23);
            this.createFindRegionBtn.TabIndex = 1;
            this.createFindRegionBtn.Text = "创建";
            this.createFindRegionBtn.UseVisualStyleBackColor = true;
            this.createFindRegionBtn.Click += new System.EventHandler(this.createFindRegionBtn_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 415);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(325, 289);
            this.propertyGrid1.TabIndex = 6;
            // 
            // DrawROIGroup
            // 
            this.DrawROIGroup.Controls.Add(this.label1);
            this.DrawROIGroup.Controls.Add(this.ROINameTxb);
            this.DrawROIGroup.Controls.Add(this.CurrentModelComb);
            this.DrawROIGroup.Controls.Add(this.ROIShapeComb);
            this.DrawROIGroup.Controls.Add(this.DeleteCurBtn);
            this.DrawROIGroup.Controls.Add(this.DeleteAllBtn);
            this.DrawROIGroup.Controls.Add(this.ModifyBtn);
            this.DrawROIGroup.Controls.Add(this.CreateNewBtn);
            this.DrawROIGroup.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DrawROIGroup.Location = new System.Drawing.Point(3, 131);
            this.DrawROIGroup.Name = "DrawROIGroup";
            this.DrawROIGroup.Size = new System.Drawing.Size(325, 151);
            this.DrawROIGroup.TabIndex = 5;
            this.DrawROIGroup.TabStop = false;
            this.DrawROIGroup.Text = "模板框操作";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(27, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ROINameTxb
            // 
            this.ROINameTxb.Location = new System.Drawing.Point(104, 119);
            this.ROINameTxb.Name = "ROINameTxb";
            this.ROINameTxb.Size = new System.Drawing.Size(80, 21);
            this.ROINameTxb.TabIndex = 6;
            // 
            // CurrentModelComb
            // 
            this.CurrentModelComb.FormattingEnabled = true;
            this.CurrentModelComb.Location = new System.Drawing.Point(205, 81);
            this.CurrentModelComb.Name = "CurrentModelComb";
            this.CurrentModelComb.Size = new System.Drawing.Size(94, 20);
            this.CurrentModelComb.TabIndex = 5;
            this.CurrentModelComb.SelectedIndexChanged += new System.EventHandler(this.CurrentModelComb_SelectedIndexChanged);
            // 
            // ROIShapeComb
            // 
            this.ROIShapeComb.FormattingEnabled = true;
            this.ROIShapeComb.Location = new System.Drawing.Point(205, 34);
            this.ROIShapeComb.Name = "ROIShapeComb";
            this.ROIShapeComb.Size = new System.Drawing.Size(94, 20);
            this.ROIShapeComb.TabIndex = 4;
            // 
            // DeleteCurBtn
            // 
            this.DeleteCurBtn.Location = new System.Drawing.Point(104, 81);
            this.DeleteCurBtn.Name = "DeleteCurBtn";
            this.DeleteCurBtn.Size = new System.Drawing.Size(80, 23);
            this.DeleteCurBtn.TabIndex = 3;
            this.DeleteCurBtn.Text = "删除当前";
            this.DeleteCurBtn.UseVisualStyleBackColor = true;
            this.DeleteCurBtn.Click += new System.EventHandler(this.DeleteCurBtn_Click);
            // 
            // DeleteAllBtn
            // 
            this.DeleteAllBtn.Location = new System.Drawing.Point(104, 32);
            this.DeleteAllBtn.Name = "DeleteAllBtn";
            this.DeleteAllBtn.Size = new System.Drawing.Size(80, 23);
            this.DeleteAllBtn.TabIndex = 2;
            this.DeleteAllBtn.Text = "删除所有";
            this.DeleteAllBtn.UseVisualStyleBackColor = true;
            this.DeleteAllBtn.Click += new System.EventHandler(this.DeleteAllBtn_Click);
            // 
            // ModifyBtn
            // 
            this.ModifyBtn.Location = new System.Drawing.Point(9, 81);
            this.ModifyBtn.Name = "ModifyBtn";
            this.ModifyBtn.Size = new System.Drawing.Size(80, 23);
            this.ModifyBtn.TabIndex = 1;
            this.ModifyBtn.Text = "修改";
            this.ModifyBtn.UseVisualStyleBackColor = true;
            this.ModifyBtn.Click += new System.EventHandler(this.ModifyBtn_Click);
            // 
            // CreateNewBtn
            // 
            this.CreateNewBtn.Location = new System.Drawing.Point(9, 32);
            this.CreateNewBtn.Name = "CreateNewBtn";
            this.CreateNewBtn.Size = new System.Drawing.Size(80, 23);
            this.CreateNewBtn.TabIndex = 0;
            this.CreateNewBtn.Text = "创建";
            this.CreateNewBtn.UseVisualStyleBackColor = true;
            this.CreateNewBtn.Click += new System.EventHandler(this.CreateNewBtn_Click);
            // 
            // ImageGroup
            // 
            this.ImageGroup.Controls.Add(this.LoadTaskImageBtn);
            this.ImageGroup.Controls.Add(this.camerasComb);
            this.ImageGroup.Controls.Add(this.openFileBtn);
            this.ImageGroup.Controls.Add(this.grabBtn);
            this.ImageGroup.Location = new System.Drawing.Point(3, 3);
            this.ImageGroup.Name = "ImageGroup";
            this.ImageGroup.Size = new System.Drawing.Size(325, 122);
            this.ImageGroup.TabIndex = 4;
            this.ImageGroup.TabStop = false;
            this.ImageGroup.Text = "图像操作";
            // 
            // LoadTaskImageBtn
            // 
            this.LoadTaskImageBtn.Location = new System.Drawing.Point(178, 79);
            this.LoadTaskImageBtn.Name = "LoadTaskImageBtn";
            this.LoadTaskImageBtn.Size = new System.Drawing.Size(121, 26);
            this.LoadTaskImageBtn.TabIndex = 11;
            this.LoadTaskImageBtn.Text = "加载任务图像";
            this.LoadTaskImageBtn.UseVisualStyleBackColor = true;
            this.LoadTaskImageBtn.Click += new System.EventHandler(this.LoadTaskImageBtn_Click);
            // 
            // camerasComb
            // 
            this.camerasComb.FormattingEnabled = true;
            this.camerasComb.Location = new System.Drawing.Point(178, 32);
            this.camerasComb.Name = "camerasComb";
            this.camerasComb.Size = new System.Drawing.Size(121, 20);
            this.camerasComb.TabIndex = 3;
            // 
            // openFileBtn
            // 
            this.openFileBtn.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.openFileBtn.Location = new System.Drawing.Point(6, 82);
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.Size = new System.Drawing.Size(127, 23);
            this.openFileBtn.TabIndex = 2;
            this.openFileBtn.Text = "打开图像文件";
            this.openFileBtn.UseVisualStyleBackColor = true;
            this.openFileBtn.Click += new System.EventHandler(this.openFileBtn_Click);
            // 
            // grabBtn
            // 
            this.grabBtn.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grabBtn.Location = new System.Drawing.Point(6, 32);
            this.grabBtn.Name = "grabBtn";
            this.grabBtn.Size = new System.Drawing.Size(127, 23);
            this.grabBtn.TabIndex = 1;
            this.grabBtn.Text = "相机拍照";
            this.grabBtn.UseVisualStyleBackColor = true;
            this.grabBtn.Click += new System.EventHandler(this.grabBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(351, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(860, 748);
            this.panel1.TabIndex = 3;
            // 
            // openImageFileDialog
            // 
            this.openImageFileDialog.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 710);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 26);
            this.button1.TabIndex = 13;
            this.button1.Text = "执行工具";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(208, 710);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 26);
            this.button2.TabIndex = 12;
            this.button2.Text = "保存模板";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // GrayMatchToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 748);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.operatePanel);
            this.Name = "GrayMatchToolForm";
            this.Text = "GrayMatchToolForm";
            this.operatePanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.DrawROIGroup.ResumeLayout(false);
            this.DrawROIGroup.PerformLayout();
            this.ImageGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel operatePanel;
        private System.Windows.Forms.Button ExecuteToolBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox currentFindRegionComb;
        private System.Windows.Forms.ComboBox FindRegionShapeComb;
        private System.Windows.Forms.Button deleteCurrentRegionBtn;
        private System.Windows.Forms.Button deleteAllRegionBtn;
        private System.Windows.Forms.Button modifyFindRegionBtn;
        private System.Windows.Forms.Button createFindRegionBtn;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.GroupBox DrawROIGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ROINameTxb;
        private System.Windows.Forms.ComboBox CurrentModelComb;
        private System.Windows.Forms.ComboBox ROIShapeComb;
        private System.Windows.Forms.Button DeleteCurBtn;
        private System.Windows.Forms.Button DeleteAllBtn;
        private System.Windows.Forms.Button ModifyBtn;
        private System.Windows.Forms.Button CreateNewBtn;
        private System.Windows.Forms.GroupBox ImageGroup;
        private System.Windows.Forms.Button LoadTaskImageBtn;
        private System.Windows.Forms.ComboBox camerasComb;
        private System.Windows.Forms.Button openFileBtn;
        private System.Windows.Forms.Button grabBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.OpenFileDialog openImageFileDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}