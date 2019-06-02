namespace VisionCameraManager
{
    partial class CameraManagerForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddCamBtn = new System.Windows.Forms.Button();
            this.DeleteCamBtn = new System.Windows.Forms.Button();
            this.CamTypeComBox = new System.Windows.Forms.ComboBox();
            this.ModifyCamBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CamNameTxt = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(283, 496);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(283, 496);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "相机名称";
            this.Column1.Name = "Column1";
            this.Column1.Width = 120;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "相机类型";
            this.Column2.Name = "Column2";
            this.Column2.Width = 120;
            // 
            // AddCamBtn
            // 
            this.AddCamBtn.Location = new System.Drawing.Point(12, 542);
            this.AddCamBtn.Name = "AddCamBtn";
            this.AddCamBtn.Size = new System.Drawing.Size(75, 23);
            this.AddCamBtn.TabIndex = 2;
            this.AddCamBtn.Text = "添加相机";
            this.AddCamBtn.UseVisualStyleBackColor = true;
            this.AddCamBtn.Click += new System.EventHandler(this.AddCamBtn_Click);
            // 
            // DeleteCamBtn
            // 
            this.DeleteCamBtn.Location = new System.Drawing.Point(117, 582);
            this.DeleteCamBtn.Name = "DeleteCamBtn";
            this.DeleteCamBtn.Size = new System.Drawing.Size(100, 23);
            this.DeleteCamBtn.TabIndex = 3;
            this.DeleteCamBtn.Text = "删除相机";
            this.DeleteCamBtn.UseVisualStyleBackColor = true;
            this.DeleteCamBtn.Click += new System.EventHandler(this.DeleteCamBtn_Click);
            // 
            // CamTypeComBox
            // 
            this.CamTypeComBox.FormattingEnabled = true;
            this.CamTypeComBox.Items.AddRange(new object[] {
            "Basler",
            "Hik"});
            this.CamTypeComBox.Location = new System.Drawing.Point(117, 542);
            this.CamTypeComBox.Name = "CamTypeComBox";
            this.CamTypeComBox.Size = new System.Drawing.Size(100, 20);
            this.CamTypeComBox.TabIndex = 4;
            // 
            // ModifyCamBtn
            // 
            this.ModifyCamBtn.Location = new System.Drawing.Point(12, 582);
            this.ModifyCamBtn.Name = "ModifyCamBtn";
            this.ModifyCamBtn.Size = new System.Drawing.Size(75, 23);
            this.ModifyCamBtn.TabIndex = 5;
            this.ModifyCamBtn.Text = "修改相机";
            this.ModifyCamBtn.UseVisualStyleBackColor = true;
            this.ModifyCamBtn.Click += new System.EventHandler(this.ModifyCamBtn_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 502);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "相机名称";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CamNameTxt
            // 
            this.CamNameTxt.Location = new System.Drawing.Point(117, 502);
            this.CamNameTxt.Name = "CamNameTxt";
            this.CamNameTxt.Size = new System.Drawing.Size(100, 21);
            this.CamNameTxt.TabIndex = 7;
            // 
            // CameraManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 606);
            this.Controls.Add(this.CamNameTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ModifyCamBtn);
            this.Controls.Add(this.CamTypeComBox);
            this.Controls.Add(this.DeleteCamBtn);
            this.Controls.Add(this.AddCamBtn);
            this.Controls.Add(this.panel2);
            this.Name = "CameraManagerForm";
            this.Text = "CameraManagerForm";
            this.Load += new System.EventHandler(this.CameraManagerForm_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button AddCamBtn;
        private System.Windows.Forms.Button DeleteCamBtn;
        private System.Windows.Forms.ComboBox CamTypeComBox;
        private System.Windows.Forms.Button ModifyCamBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CamNameTxt;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}