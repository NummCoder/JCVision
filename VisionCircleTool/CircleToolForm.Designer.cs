namespace VisionCircleTool
{
    partial class CircleToolForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.GetLineResultBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.parametersPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.camerasComb = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.DrawROIGroup = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.ModifyBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.DrawROIGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(272, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(627, 687);
            this.panel2.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.GetLineResultBtn);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.SaveBtn);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.DrawROIGroup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(272, 687);
            this.panel1.TabIndex = 12;
            // 
            // GetLineResultBtn
            // 
            this.GetLineResultBtn.Location = new System.Drawing.Point(12, 645);
            this.GetLineResultBtn.Name = "GetLineResultBtn";
            this.GetLineResultBtn.Size = new System.Drawing.Size(97, 23);
            this.GetLineResultBtn.TabIndex = 2;
            this.GetLineResultBtn.Text = "执行工具";
            this.GetLineResultBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.parametersPropertyGrid);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(3, 214);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 411);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "参数设置";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "追随工具";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(134, 26);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(109, 20);
            this.comboBox1.TabIndex = 5;
            // 
            // parametersPropertyGrid
            // 
            this.parametersPropertyGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.parametersPropertyGrid.HelpVisible = false;
            this.parametersPropertyGrid.Location = new System.Drawing.Point(3, 66);
            this.parametersPropertyGrid.Name = "parametersPropertyGrid";
            this.parametersPropertyGrid.Size = new System.Drawing.Size(251, 342);
            this.parametersPropertyGrid.TabIndex = 0;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(149, 645);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(97, 23);
            this.SaveBtn.TabIndex = 4;
            this.SaveBtn.Text = "保存参数";
            this.SaveBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.camerasComb);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(3, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(257, 106);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "图像操作";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(146, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "打开图像文件";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // camerasComb
            // 
            this.camerasComb.FormattingEnabled = true;
            this.camerasComb.Location = new System.Drawing.Point(149, 28);
            this.camerasComb.Name = "camerasComb";
            this.camerasComb.Size = new System.Drawing.Size(94, 20);
            this.camerasComb.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "相机取图";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(9, 65);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(97, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = "打开图像文件";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // DrawROIGroup
            // 
            this.DrawROIGroup.Controls.Add(this.button2);
            this.DrawROIGroup.Controls.Add(this.ModifyBtn);
            this.DrawROIGroup.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DrawROIGroup.Location = new System.Drawing.Point(3, 135);
            this.DrawROIGroup.Name = "DrawROIGroup";
            this.DrawROIGroup.Size = new System.Drawing.Size(257, 73);
            this.DrawROIGroup.TabIndex = 8;
            this.DrawROIGroup.TabStop = false;
            this.DrawROIGroup.Text = "ROI操作";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(146, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "显示搜索框";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // ModifyBtn
            // 
            this.ModifyBtn.Location = new System.Drawing.Point(9, 32);
            this.ModifyBtn.Name = "ModifyBtn";
            this.ModifyBtn.Size = new System.Drawing.Size(97, 23);
            this.ModifyBtn.TabIndex = 1;
            this.ModifyBtn.Text = "画搜索框";
            this.ModifyBtn.UseVisualStyleBackColor = true;
            // 
            // CircleToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 687);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CircleToolForm";
            this.Text = "找圆工具";
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.DrawROIGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button GetLineResultBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PropertyGrid parametersPropertyGrid;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox camerasComb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox DrawROIGroup;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button ModifyBtn;
    }
}