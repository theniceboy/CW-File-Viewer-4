namespace CW_File_Viewer_4
{
    partial class FrmPtxt
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
            this.tcanuse = new System.Windows.Forms.Timer(this.components);
            this.textprev = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tmv = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tcanuse
            // 
            this.tcanuse.Enabled = true;
            this.tcanuse.Interval = 500;
            this.tcanuse.Tick += new System.EventHandler(this.tcanuse_Tick);
            // 
            // textprev
            // 
            this.textprev.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textprev.BackColor = System.Drawing.Color.Ivory;
            // 
            // 
            // 
            this.textprev.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textprev.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textprev.Font = new System.Drawing.Font("宋体", 11F);
            this.textprev.ForeColor = System.Drawing.Color.Black;
            this.textprev.Location = new System.Drawing.Point(7, 7);
            this.textprev.Multiline = true;
            this.textprev.Name = "textprev";
            this.textprev.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textprev.Size = new System.Drawing.Size(386, 386);
            this.textprev.TabIndex = 1;
            this.textprev.TextChanged += new System.EventHandler(this.textprev_TextChanged);
            // 
            // tmv
            // 
            this.tmv.Enabled = true;
            this.tmv.Tick += new System.EventHandler(this.tmv_Tick);
            // 
            // FrmPtxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.textprev);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmPtxt";
            this.Text = "FrmPtxt";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tcanuse;
        private DevComponents.DotNetBar.Controls.TextBoxX textprev;
        private System.Windows.Forms.Timer tmv;
    }
}