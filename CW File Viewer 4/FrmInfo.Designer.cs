namespace CW_File_Viewer_4
{
    partial class FrmInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInfo));
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.filename = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.filepic = new System.Windows.Forms.PictureBox();
            this.filetype = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.filesize = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.filepath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.filecreatdate = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.filechangedate = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.close = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.filepic)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Location = new System.Drawing.Point(12, 117);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(90, 23);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX2.TabIndex = 4;
            this.labelX2.Text = "文件大小：";
            this.labelX2.Click += new System.EventHandler(this.labelX2_Click);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Location = new System.Drawing.Point(12, 161);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(90, 23);
            this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX3.TabIndex = 5;
            this.labelX3.Text = "文件路径：";
            this.labelX3.Click += new System.EventHandler(this.labelX3_Click);
            // 
            // filename
            // 
            this.filename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.filename.Border.Class = "TextBoxBorder";
            this.filename.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.filename.ForeColor = System.Drawing.Color.Black;
            this.filename.Location = new System.Drawing.Point(108, 15);
            this.filename.Name = "filename";
            this.filename.ReadOnly = true;
            this.filename.Size = new System.Drawing.Size(405, 27);
            this.filename.TabIndex = 6;
            this.filename.TextChanged += new System.EventHandler(this.filename_TextChanged);
            // 
            // filepic
            // 
            this.filepic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.filepic.ForeColor = System.Drawing.Color.Black;
            this.filepic.Location = new System.Drawing.Point(25, 7);
            this.filepic.Name = "filepic";
            this.filepic.Size = new System.Drawing.Size(45, 45);
            this.filepic.TabIndex = 7;
            this.filepic.TabStop = false;
            this.filepic.Click += new System.EventHandler(this.filepic_Click);
            // 
            // filetype
            // 
            this.filetype.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filetype.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.filetype.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.filetype.ForeColor = System.Drawing.Color.Black;
            this.filetype.Location = new System.Drawing.Point(108, 71);
            this.filetype.Name = "filetype";
            this.filetype.ReadOnly = true;
            this.filetype.Size = new System.Drawing.Size(405, 21);
            this.filetype.TabIndex = 8;
            this.filetype.TextChanged += new System.EventHandler(this.filetype_TextChanged);
            // 
            // filesize
            // 
            this.filesize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.filesize.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.filesize.ForeColor = System.Drawing.Color.Black;
            this.filesize.Location = new System.Drawing.Point(108, 115);
            this.filesize.Name = "filesize";
            this.filesize.ReadOnly = true;
            this.filesize.Size = new System.Drawing.Size(405, 21);
            this.filesize.TabIndex = 9;
            this.filesize.TextChanged += new System.EventHandler(this.filesize_TextChanged);
            // 
            // filepath
            // 
            this.filepath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filepath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.filepath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.filepath.ForeColor = System.Drawing.Color.Black;
            this.filepath.Location = new System.Drawing.Point(108, 159);
            this.filepath.Name = "filepath";
            this.filepath.ReadOnly = true;
            this.filepath.Size = new System.Drawing.Size(405, 21);
            this.filepath.TabIndex = 10;
            this.filepath.TextChanged += new System.EventHandler(this.filepath_TextChanged);
            // 
            // filecreatdate
            // 
            this.filecreatdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filecreatdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.filecreatdate.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.filecreatdate.ForeColor = System.Drawing.Color.Black;
            this.filecreatdate.Location = new System.Drawing.Point(108, 203);
            this.filecreatdate.Name = "filecreatdate";
            this.filecreatdate.ReadOnly = true;
            this.filecreatdate.Size = new System.Drawing.Size(405, 21);
            this.filecreatdate.TabIndex = 11;
            this.filecreatdate.TextChanged += new System.EventHandler(this.filecreatdate_TextChanged);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(12, 205);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(90, 23);
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX1.TabIndex = 12;
            this.labelX1.Text = "创建日期：";
            this.labelX1.Click += new System.EventHandler(this.labelX1_Click);
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.ForeColor = System.Drawing.Color.Black;
            this.labelX4.Location = new System.Drawing.Point(12, 73);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(90, 23);
            this.labelX4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX4.TabIndex = 13;
            this.labelX4.Text = "文件类型：";
            this.labelX4.Click += new System.EventHandler(this.labelX4_Click);
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.ForeColor = System.Drawing.Color.Black;
            this.labelX5.Location = new System.Drawing.Point(12, 249);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(90, 23);
            this.labelX5.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX5.TabIndex = 14;
            this.labelX5.Text = "修改日期：";
            this.labelX5.Click += new System.EventHandler(this.labelX5_Click);
            // 
            // filechangedate
            // 
            this.filechangedate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filechangedate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.filechangedate.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.filechangedate.ForeColor = System.Drawing.Color.Black;
            this.filechangedate.Location = new System.Drawing.Point(108, 249);
            this.filechangedate.Name = "filechangedate";
            this.filechangedate.ReadOnly = true;
            this.filechangedate.Size = new System.Drawing.Size(405, 21);
            this.filechangedate.TabIndex = 15;
            this.filechangedate.TextChanged += new System.EventHandler(this.filechangedate_TextChanged);
            // 
            // close
            // 
            this.close.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.close.FocusCuesEnabled = false;
            this.close.Location = new System.Drawing.Point(376, 296);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(137, 45);
            this.close.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.close.TabIndex = 16;
            this.close.Text = "关闭";
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // FrmInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 353);
            this.Controls.Add(this.close);
            this.Controls.Add(this.filechangedate);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.filecreatdate);
            this.Controls.Add(this.filepath);
            this.Controls.Add(this.filesize);
            this.Controls.Add(this.filetype);
            this.Controls.Add(this.filepic);
            this.Controls.Add(this.filename);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(543, 400);
            this.MinimumSize = new System.Drawing.Size(543, 400);
            this.Name = "FrmInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CW File Viewer - 文件属性";
            this.Load += new System.EventHandler(this.FrmInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.filepic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX filename;
        private System.Windows.Forms.PictureBox filepic;
        private DevComponents.DotNetBar.Controls.TextBoxX filetype;
        private DevComponents.DotNetBar.Controls.TextBoxX filesize;
        private DevComponents.DotNetBar.Controls.TextBoxX filepath;
        private DevComponents.DotNetBar.Controls.TextBoxX filecreatdate;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX filechangedate;
        private DevComponents.DotNetBar.ButtonX close;

    }
}

