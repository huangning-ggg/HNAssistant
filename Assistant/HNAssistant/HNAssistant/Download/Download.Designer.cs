namespace HNAssistant
{
    partial class Download
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Download));
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pictureBoxLog = new System.Windows.Forms.PictureBox();
            this.lbProject = new System.Windows.Forms.Label();
            this.lbPercentBig = new System.Windows.Forms.Label();
            this.lbPercent = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLog)).BeginInit();
            this.SuspendLayout();
            // 
            // pbClose
            // 
            this.pbClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbClose.BackgroundImage")));
            this.pbClose.Location = new System.Drawing.Point(373, 12);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(16, 16);
            this.pbClose.TabIndex = 7;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.Close_Click);
            // 
            // pictureBoxLog
            // 
            this.pictureBoxLog.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxLog.BackgroundImage")));
            this.pictureBoxLog.Location = new System.Drawing.Point(16, 12);
            this.pictureBoxLog.Name = "pictureBoxLog";
            this.pictureBoxLog.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxLog.TabIndex = 8;
            this.pictureBoxLog.TabStop = false;
            // 
            // lbProject
            // 
            this.lbProject.AutoSize = true;
            this.lbProject.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbProject.Location = new System.Drawing.Point(38, 138);
            this.lbProject.Name = "lbProject";
            this.lbProject.Size = new System.Drawing.Size(44, 17);
            this.lbProject.TabIndex = 4;
            this.lbProject.Text = "名称：";
            // 
            // lbPercentBig
            // 
            this.lbPercentBig.AutoSize = true;
            this.lbPercentBig.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPercentBig.Location = new System.Drawing.Point(38, 56);
            this.lbPercentBig.Name = "lbPercentBig";
            this.lbPercentBig.Size = new System.Drawing.Size(93, 21);
            this.lbPercentBig.TabIndex = 5;
            this.lbPercentBig.Text = "已完成 xx%";
            // 
            // lbPercent
            // 
            this.lbPercent.AutoSize = true;
            this.lbPercent.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPercent.Location = new System.Drawing.Point(38, 11);
            this.lbPercent.Name = "lbPercent";
            this.lbPercent.Size = new System.Drawing.Size(71, 17);
            this.lbPercent.TabIndex = 6;
            this.lbPercent.Text = "已完成 xx%";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(40, 87);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(330, 32);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Download
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 180);
            this.Controls.Add(this.lbPercentBig);
            this.Controls.Add(this.pbClose);
            this.Controls.Add(this.pictureBoxLog);
            this.Controls.Add(this.lbProject);
            this.Controls.Add(this.lbPercent);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(400, 180);
            this.MinimumSize = new System.Drawing.Size(400, 180);
            this.Name = "Download";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Download";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this._MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this._MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pictureBoxLog;
        private System.Windows.Forms.Label lbProject;
        private System.Windows.Forms.Label lbPercent;
        private System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Label lbPercentBig;
        private System.Windows.Forms.Timer timer1;
    }
}