namespace BerlynAcademy_ES
{
    partial class frmPrincipalMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipalMain));
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.dgvm = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblLoggerPosition = new System.Windows.Forms.Label();
            this.lblLogger = new System.Windows.Forms.Label();
            this.btnFacAdv = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnAbtPrin = new System.Windows.Forms.Button();
            this.btnReprin = new System.Windows.Forms.Button();
            this.btnFIPrin = new System.Windows.Forms.Button();
            this.btnSIPrin = new System.Windows.Forms.Button();
            this.lblActs = new System.Windows.Forms.Label();
            this.pnlType = new System.Windows.Forms.Panel();
            this.labelmain = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mcd = new System.Windows.Forms.MonthCalendar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblsy = new System.Windows.Forms.Label();
            this.lvwAct = new System.Windows.Forms.ListView();
            this.pnlMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlType.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.White;
            this.pnlMenu.Controls.Add(this.dgvm);
            this.pnlMenu.Controls.Add(this.label1);
            this.pnlMenu.Controls.Add(this.label2);
            this.pnlMenu.Controls.Add(this.pictureBox1);
            this.pnlMenu.Controls.Add(this.lblLoggerPosition);
            this.pnlMenu.Controls.Add(this.lblLogger);
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(263, 757);
            this.pnlMenu.TabIndex = 7;
            // 
            // dgvm
            // 
            this.dgvm.AllowUserToAddRows = false;
            this.dgvm.AllowUserToResizeColumns = false;
            this.dgvm.AllowUserToResizeRows = false;
            this.dgvm.BackgroundColor = System.Drawing.Color.White;
            this.dgvm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvm.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvm.ColumnHeadersVisible = false;
            this.dgvm.Cursor = System.Windows.Forms.Cursors.Default;
            this.dgvm.GridColor = System.Drawing.Color.Crimson;
            this.dgvm.Location = new System.Drawing.Point(45, 132);
            this.dgvm.MultiSelect = false;
            this.dgvm.Name = "dgvm";
            this.dgvm.ReadOnly = true;
            this.dgvm.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvm.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvm.RowTemplate.Height = 41;
            this.dgvm.RowTemplate.ReadOnly = true;
            this.dgvm.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvm.Size = new System.Drawing.Size(218, 606);
            this.dgvm.TabIndex = 94;
            this.dgvm.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvm_CellContentClick);
            this.dgvm.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvm_CellMouseEnter);
            this.dgvm.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvm_CellMouseLeave);
            this.dgvm.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvm_CellMouseMove);
            this.dgvm.Click += new System.EventHandler(this.dgvm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Turquoise;
            this.label1.Location = new System.Drawing.Point(42, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 17);
            this.label1.TabIndex = 85;
            this.label1.Text = "_____________________________________";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Turquoise;
            this.label2.Font = new System.Drawing.Font("Segoe UI Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(45, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 32);
            this.label2.TabIndex = 84;
            this.label2.Text = "Menu";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(45, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(45, 45);
            this.pictureBox1.TabIndex = 31;
            this.pictureBox1.TabStop = false;
            // 
            // lblLoggerPosition
            // 
            this.lblLoggerPosition.AutoSize = true;
            this.lblLoggerPosition.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggerPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblLoggerPosition.Location = new System.Drawing.Point(96, 57);
            this.lblLoggerPosition.Name = "lblLoggerPosition";
            this.lblLoggerPosition.Size = new System.Drawing.Size(66, 21);
            this.lblLoggerPosition.TabIndex = 30;
            this.lblLoggerPosition.Text = "Principal";
            // 
            // lblLogger
            // 
            this.lblLogger.AutoSize = true;
            this.lblLogger.Font = new System.Drawing.Font("Segoe UI Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogger.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblLogger.Location = new System.Drawing.Point(94, 26);
            this.lblLogger.Name = "lblLogger";
            this.lblLogger.Size = new System.Drawing.Size(76, 32);
            this.lblLogger.TabIndex = 29;
            this.lblLogger.Text = "Name";
            // 
            // btnFacAdv
            // 
            this.btnFacAdv.BackColor = System.Drawing.Color.White;
            this.btnFacAdv.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFacAdv.FlatAppearance.BorderSize = 0;
            this.btnFacAdv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFacAdv.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFacAdv.Location = new System.Drawing.Point(269, 150);
            this.btnFacAdv.Name = "btnFacAdv";
            this.btnFacAdv.Size = new System.Drawing.Size(69, 41);
            this.btnFacAdv.TabIndex = 86;
            this.btnFacAdv.Text = "          Faculty advisory";
            this.btnFacAdv.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFacAdv.UseVisualStyleBackColor = false;
            this.btnFacAdv.Visible = false;
            this.btnFacAdv.Click += new System.EventHandler(this.btnFacAdv_Click);
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.White;
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(269, 360);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(69, 41);
            this.btnHome.TabIndex = 25;
            this.btnHome.Text = "          Logout";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Visible = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnAbtPrin
            // 
            this.btnAbtPrin.BackColor = System.Drawing.Color.White;
            this.btnAbtPrin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbtPrin.FlatAppearance.BorderSize = 0;
            this.btnAbtPrin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbtPrin.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbtPrin.Location = new System.Drawing.Point(269, 318);
            this.btnAbtPrin.Name = "btnAbtPrin";
            this.btnAbtPrin.Size = new System.Drawing.Size(69, 41);
            this.btnAbtPrin.TabIndex = 24;
            this.btnAbtPrin.Text = "          About us";
            this.btnAbtPrin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbtPrin.UseVisualStyleBackColor = false;
            this.btnAbtPrin.Visible = false;
            this.btnAbtPrin.Click += new System.EventHandler(this.btnAbtPrin_Click);
            // 
            // btnReprin
            // 
            this.btnReprin.BackColor = System.Drawing.Color.White;
            this.btnReprin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReprin.FlatAppearance.BorderSize = 0;
            this.btnReprin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReprin.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReprin.Location = new System.Drawing.Point(269, 276);
            this.btnReprin.Name = "btnReprin";
            this.btnReprin.Size = new System.Drawing.Size(69, 41);
            this.btnReprin.TabIndex = 16;
            this.btnReprin.Text = "          Report";
            this.btnReprin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReprin.UseVisualStyleBackColor = false;
            this.btnReprin.Visible = false;
            this.btnReprin.Click += new System.EventHandler(this.btnReprin_Click);
            // 
            // btnFIPrin
            // 
            this.btnFIPrin.BackColor = System.Drawing.Color.White;
            this.btnFIPrin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFIPrin.FlatAppearance.BorderSize = 0;
            this.btnFIPrin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFIPrin.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFIPrin.Location = new System.Drawing.Point(269, 234);
            this.btnFIPrin.Name = "btnFIPrin";
            this.btnFIPrin.Size = new System.Drawing.Size(69, 41);
            this.btnFIPrin.TabIndex = 15;
            this.btnFIPrin.Text = "          Faculty information";
            this.btnFIPrin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFIPrin.UseVisualStyleBackColor = false;
            this.btnFIPrin.Visible = false;
            this.btnFIPrin.Click += new System.EventHandler(this.btnFIPrin_Click);
            // 
            // btnSIPrin
            // 
            this.btnSIPrin.BackColor = System.Drawing.Color.White;
            this.btnSIPrin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSIPrin.FlatAppearance.BorderSize = 0;
            this.btnSIPrin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSIPrin.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSIPrin.Location = new System.Drawing.Point(269, 192);
            this.btnSIPrin.Name = "btnSIPrin";
            this.btnSIPrin.Size = new System.Drawing.Size(69, 41);
            this.btnSIPrin.TabIndex = 14;
            this.btnSIPrin.Text = "          Student information";
            this.btnSIPrin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSIPrin.UseVisualStyleBackColor = false;
            this.btnSIPrin.Visible = false;
            this.btnSIPrin.Click += new System.EventHandler(this.btnSIPrin_Click);
            // 
            // lblActs
            // 
            this.lblActs.AutoSize = true;
            this.lblActs.BackColor = System.Drawing.Color.Transparent;
            this.lblActs.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActs.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblActs.Location = new System.Drawing.Point(306, 91);
            this.lblActs.Name = "lblActs";
            this.lblActs.Size = new System.Drawing.Size(0, 18);
            this.lblActs.TabIndex = 44;
            // 
            // pnlType
            // 
            this.pnlType.BackColor = System.Drawing.Color.Crimson;
            this.pnlType.Controls.Add(this.labelmain);
            this.pnlType.Location = new System.Drawing.Point(263, 0);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(1111, 79);
            this.pnlType.TabIndex = 45;
            // 
            // labelmain
            // 
            this.labelmain.AutoSize = true;
            this.labelmain.BackColor = System.Drawing.Color.Transparent;
            this.labelmain.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelmain.ForeColor = System.Drawing.Color.White;
            this.labelmain.Location = new System.Drawing.Point(40, 24);
            this.labelmain.Name = "labelmain";
            this.labelmain.Size = new System.Drawing.Size(196, 30);
            this.labelmain.TabIndex = 4;
            this.labelmain.Text = "Calendar of Activities";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.mcd);
            this.panel1.Location = new System.Drawing.Point(1118, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 195);
            this.panel1.TabIndex = 97;
            // 
            // mcd
            // 
            this.mcd.BackColor = System.Drawing.Color.White;
            this.mcd.Location = new System.Drawing.Point(5, 28);
            this.mcd.Name = "mcd";
            this.mcd.TabIndex = 95;
            this.mcd.TitleBackColor = System.Drawing.Color.Crimson;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.lblsy);
            this.panel2.Location = new System.Drawing.Point(308, 78);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(657, 34);
            this.panel2.TabIndex = 101;
            // 
            // lblsy
            // 
            this.lblsy.AutoSize = true;
            this.lblsy.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsy.ForeColor = System.Drawing.Color.Black;
            this.lblsy.Location = new System.Drawing.Point(4, 2);
            this.lblsy.Name = "lblsy";
            this.lblsy.Size = new System.Drawing.Size(135, 30);
            this.lblsy.TabIndex = 32;
            this.lblsy.Text = "SY:2016-2017";
            // 
            // lvwAct
            // 
            this.lvwAct.BackColor = System.Drawing.Color.White;
            this.lvwAct.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwAct.Enabled = false;
            this.lvwAct.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwAct.ForeColor = System.Drawing.Color.Black;
            this.lvwAct.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwAct.Location = new System.Drawing.Point(308, 111);
            this.lvwAct.Name = "lvwAct";
            this.lvwAct.Scrollable = false;
            this.lvwAct.Size = new System.Drawing.Size(657, 635);
            this.lvwAct.TabIndex = 100;
            this.lvwAct.UseCompatibleStateImageBehavior = false;
            this.lvwAct.View = System.Windows.Forms.View.Details;
            // 
            // frmPrincipalMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1362, 742);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lvwAct);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnFacAdv);
            this.Controls.Add(this.pnlType);
            this.Controls.Add(this.lblActs);
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.btnSIPrin);
            this.Controls.Add(this.btnFIPrin);
            this.Controls.Add(this.btnReprin);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.btnAbtPrin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPrincipalMain";
            this.Text = "Welcome Principal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipalMain_FormClosing);
            this.Load += new System.EventHandler(this.frmPrincipalMain_Load);
            this.pnlMenu.ResumeLayout(false);
            this.pnlMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlType.ResumeLayout(false);
            this.pnlType.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnAbtPrin;
        private System.Windows.Forms.Button btnReprin;
        private System.Windows.Forms.Button btnFIPrin;
        private System.Windows.Forms.Button btnSIPrin;
        private System.Windows.Forms.Label lblActs;
        private System.Windows.Forms.Panel pnlType;
        private System.Windows.Forms.Label labelmain;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblLoggerPosition;
        private System.Windows.Forms.Label lblLogger;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFacAdv;
        private System.Windows.Forms.DataGridView dgvm;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MonthCalendar mcd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView lvwAct;
        private System.Windows.Forms.Label lblsy;
    }
}