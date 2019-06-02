namespace VisionDisplayTool
{
    partial class DisplayControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayControl));
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Clear = new System.Windows.Forms.ToolStripButton();
            this.Selected = new System.Windows.Forms.ToolStripButton();
            this.Translate = new System.Windows.Forms.ToolStripButton();
            this.Zoom = new System.Windows.Forms.ToolStripButton();
            this.ZoomOut = new System.Windows.Forms.ToolStripButton();
            this.Fit = new System.Windows.Forms.ToolStripButton();
            this.imageSave = new System.Windows.Forms.ToolStripButton();
            this.Cross = new System.Windows.Forms.ToolStripButton();
            this.zoomNumber = new System.Windows.Forms.ToolStripComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.RValueLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.GValueLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.BValueLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.XValueLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.YValueLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(400, 484);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(400, 484);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Clear,
            this.Selected,
            this.Translate,
            this.Zoom,
            this.ZoomOut,
            this.Fit,
            this.imageSave,
            this.Cross,
            this.zoomNumber});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(400, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "ClearWindow";
            // 
            // Clear
            // 
            this.Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Clear.Image = global::VisionDisplayTool.Resources.清空;
            this.Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Clear.Margin = new System.Windows.Forms.Padding(5, 1, 2, 2);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(23, 22);
            this.Clear.Text = "Clear Window";
            this.Clear.Click += new System.EventHandler(this.EventHandlers);
            // 
            // Selected
            // 
            this.Selected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Selected.Image = global::VisionDisplayTool.Resources.鼠标;
            this.Selected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Selected.Margin = new System.Windows.Forms.Padding(0, 1, 2, 2);
            this.Selected.Name = "Selected";
            this.Selected.Size = new System.Drawing.Size(23, 22);
            this.Selected.Text = "Select Window";
            this.Selected.Click += new System.EventHandler(this.EventHandlers);
            // 
            // Translate
            // 
            this.Translate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Translate.Image = ((System.Drawing.Image)(resources.GetObject("Translate.Image")));
            this.Translate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Translate.Name = "Translate";
            this.Translate.Size = new System.Drawing.Size(23, 22);
            this.Translate.Text = "Translate Image";
            this.Translate.Click += new System.EventHandler(this.EventHandlers);
            // 
            // Zoom
            // 
            this.Zoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Zoom.Image = ((System.Drawing.Image)(resources.GetObject("Zoom.Image")));
            this.Zoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Zoom.Name = "Zoom";
            this.Zoom.Size = new System.Drawing.Size(23, 22);
            this.Zoom.Text = "Zoom Image";
            this.Zoom.Click += new System.EventHandler(this.EventHandlers);
            // 
            // ZoomOut
            // 
            this.ZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("ZoomOut.Image")));
            this.ZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomOut.Name = "ZoomOut";
            this.ZoomOut.Size = new System.Drawing.Size(23, 22);
            this.ZoomOut.Text = "Zoom Out Image";
            this.ZoomOut.Click += new System.EventHandler(this.EventHandlers);
            // 
            // Fit
            // 
            this.Fit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Fit.Image = global::VisionDisplayTool.Resources.自适应图标;
            this.Fit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Fit.Name = "Fit";
            this.Fit.Size = new System.Drawing.Size(23, 22);
            this.Fit.Text = "Fit image of  window";
            this.Fit.Click += new System.EventHandler(this.EventHandlers);
            // 
            // imageSave
            // 
            this.imageSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.imageSave.Image = global::VisionDisplayTool.Resources.保存;
            this.imageSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imageSave.Name = "imageSave";
            this.imageSave.Size = new System.Drawing.Size(23, 22);
            this.imageSave.Text = "Save";
            this.imageSave.Click += new System.EventHandler(this.EventHandlers);
            // 
            // Cross
            // 
            this.Cross.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Cross.Image = global::VisionDisplayTool.Resources.十字线;
            this.Cross.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Cross.Name = "Cross";
            this.Cross.Size = new System.Drawing.Size(23, 22);
            this.Cross.Text = "Cross Line";
            this.Cross.Click += new System.EventHandler(this.EventHandlers);
            // 
            // zoomNumber
            // 
            this.zoomNumber.Items.AddRange(new object[] {
            "10%",
            "25%",
            "50%",
            "100%",
            "200%",
            "400%"});
            this.zoomNumber.Name = "zoomNumber";
            this.zoomNumber.Size = new System.Drawing.Size(121, 25);
            this.zoomNumber.Text = "100%";
            this.zoomNumber.SelectedIndexChanged += new System.EventHandler(this.zoomNumber_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RValueLabel,
            this.GValueLabel,
            this.BValueLabel,
            this.XValueLabel,
            this.YValueLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 462);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(400, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // RValueLabel
            // 
            this.RValueLabel.AutoSize = false;
            this.RValueLabel.Name = "RValueLabel";
            this.RValueLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RValueLabel.Size = new System.Drawing.Size(50, 17);
            this.RValueLabel.Text = "R:";
            // 
            // GValueLabel
            // 
            this.GValueLabel.AutoSize = false;
            this.GValueLabel.Name = "GValueLabel";
            this.GValueLabel.Size = new System.Drawing.Size(50, 17);
            this.GValueLabel.Text = "G:";
            // 
            // BValueLabel
            // 
            this.BValueLabel.AutoSize = false;
            this.BValueLabel.Name = "BValueLabel";
            this.BValueLabel.Size = new System.Drawing.Size(50, 17);
            this.BValueLabel.Text = "B:";
            // 
            // XValueLabel
            // 
            this.XValueLabel.AutoSize = false;
            this.XValueLabel.Name = "XValueLabel";
            this.XValueLabel.Size = new System.Drawing.Size(50, 17);
            this.XValueLabel.Text = "X:";
            // 
            // YValueLabel
            // 
            this.YValueLabel.AutoSize = false;
            this.YValueLabel.Name = "YValueLabel";
            this.YValueLabel.Size = new System.Drawing.Size(50, 17);
            this.YValueLabel.Text = "Y:";
            // 
            // DisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.hWindowControl1);
            this.Name = "DisplayControl";
            this.Size = new System.Drawing.Size(400, 484);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton Clear;
        private System.Windows.Forms.ToolStripButton Selected;
        private System.Windows.Forms.ToolStripButton Translate;
        private System.Windows.Forms.ToolStripButton Zoom;
        private System.Windows.Forms.ToolStripButton ZoomOut;
        private System.Windows.Forms.ToolStripButton Fit;
        private System.Windows.Forms.ToolStripButton imageSave;
        private System.Windows.Forms.ToolStripButton Cross;
        private System.Windows.Forms.ToolStripComboBox zoomNumber;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RValueLabel;
        private System.Windows.Forms.ToolStripStatusLabel GValueLabel;
        private System.Windows.Forms.ToolStripStatusLabel BValueLabel;
        private System.Windows.Forms.ToolStripStatusLabel XValueLabel;
        private System.Windows.Forms.ToolStripStatusLabel YValueLabel;
    }
}
