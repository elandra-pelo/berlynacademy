namespace BerlynAcademy_ES
{
    partial class frmCashierMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCashierMain));
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.dgvm = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblLoggerCasPosition = new System.Windows.Forms.Label();
            this.lblLoggerCas = new System.Windows.Forms.Label();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnAbt = new System.Windows.Forms.Button();
            this.btnStudI = new System.Windows.Forms.Button();
            this.btnPay = new System.Windows.Forms.Button();
            this.pnlType = new System.Windows.Forms.Panel();
            this.labelmain = new System.Windows.Forms.Label();
            this.lblActs = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mcd = new System.Windows.Forms.MonthCalendar();
            this.lvwAct = new System.Windows.Forms.ListView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblsy = new System.Windows.Forms.Label();
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
            this.pnlMenu.Controls.Add(this.lblLoggerCasPosition);
            this.pnlMenu.Controls.Add(this.lblLoggerCas);
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
            this.dgvm.TabIndex = 92;
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
            // lblLoggerCasPosition
            // 
            this.lblLoggerCasPosition.AutoSize = true;
            this.lblLoggerCasPosition.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggerCasPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblLoggerCasPosition.Location = new System.Drawing.Point(96, 57);
            this.lblLoggerCasPosition.Name = "lblLoggerCasPosition";
            this.lblLoggerCasPosition.Size = new System.Drawing.Size(59, 21);
            this.lblLoggerCasPosition.TabIndex = 30;
            this.lblLoggerCasPosition.Text = "Cashier";
            // 
            // lblLoggerCas
            // 
            this.lblLoggerCas.AutoSize = true;
            this.lblLoggerCas.Font = new System.Drawing.Font("Segoe UI Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggerCas.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblLoggerCas.Location = new System.Drawing.Point(94, 26);
            this.lblLoggerCas.Name = "lblLoggerCas";
            this.lblLoggerCas.Size = new System.Drawing.Size(76, 32);
            this.lblLoggerCas.TabIndex = 29;
            this.lblLoggerCas.Text = "Name";
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.White;
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(269, 675);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(118, 41);
            this.btnHome.TabIndex = 17;
            this.btnHome.Text = "          Logout";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Visible = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnAbt
            // 
            this.btnAbt.BackColor = System.Drawing.Color.White;
            this.btnAbt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbt.FlatAppearance.BorderSize = 0;
            this.btnAbt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbt.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbt.Location = new System.Drawing.Point(269, 633);
            this.btnAbt.Name = "btnAbt";
            this.btnAbt.Size = new System.Drawing.Size(118, 41);
            this.btnAbt.TabIndex = 16;
            this.btnAbt.Text = "          About us";
            this.btnAbt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbt.UseVisualStyleBackColor = false;
            this.btnAbt.Visible = false;
            this.btnAbt.Click += new System.EventHandler(this.btnAbt_Click);
            // 
            // btnStudI
            // 
            this.btnStudI.BackColor = System.Drawing.Color.White;
            this.btnStudI.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStudI.FlatAppearance.BorderSize = 0;
            this.btnStudI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudI.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStudI.Location = new System.Drawing.Point(269, 591);
            this.btnStudI.Name = "btnStudI";
            this.btnStudI.Size = new System.Drawing.Size(118, 41);
            this.btnStudI.TabIndex = 15;
            this.btnStudI.Text = "          Student information";
            this.btnStudI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStudI.UseVisualStyleBackColor = false;
            this.btnStudI.Visible = false;
            this.btnStudI.Click += new System.EventHandler(this.btnStudI_Click);
            // 
            // btnPay
            // 
            this.btnPay.BackColor = System.Drawing.Color.White;
            this.btnPay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPay.FlatAppearance.BorderSize = 0;
            this.btnPay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPay.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPay.Location = new System.Drawing.Point(269, 549);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(118, 41);
            this.btnPay.TabIndex = 14;
            this.btnPay.Text = "          Payment";
            this.btnPay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPay.UseVisualStyleBackColor = false;
            this.btnPay.Visible = false;
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // pnlType
            // 
            this.pnlType.BackColor = System.Drawing.Color.Crimson;
            this.pnlType.Controls.Add(this.labelmain);
            this.pnlType.Location = new System.Drawing.Point(263, 0);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(1111, 79);
            this.pnlType.TabIndex = 46;
            // 
            // labelmain
            // 
            this.labelmain.AutoSize = true;
            this.labelmain.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelmain.ForeColor = System.Drawing.Color.White;
            this.labelmain.Location = new System.Drawing.Point(41, 24);
            this.labelmain.Name = "labelmain";
            this.labelmain.Size = new System.Drawing.Size(196, 30);
            this.labelmain.TabIndex = 4;
            this.labelmain.Text = "Calendar of Activities";
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
            this.lblActs.TabIndex = 47;
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
            // lvwAct
            // 
            this.lvwAct.BackColor = System.Drawing.Color.White;
            this.lvwAct.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwAct.Enabled = false;
            this.lvwAct.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwAct.ForeColor = System.Drawing.Color.Black;
            this.lvwAct.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwAct.Location = new System.Drawing.Point(309, 111);
            this.lvwAct.Name = "lvwAct";
            this.lvwAct.Scrollable = false;
            this.lvwAct.Size = new System.Drawing.Size(657, 635);
            this.lvwAct.TabIndex = 98;
            this.lvwAct.UseCompatibleStateImageBehavior = false;
            this.lvwAct.View = System.Windows.Forms.View.Details;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.lblsy);
            this.panel2.Location = new System.Drawing.Point(309, 78);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(657, 34);
            this.panel2.TabIndex = 99;
            // 
            // lblsy
            // 
            this.lblsy.AutoSize = true;
            this.lblsy.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsy.ForeColor = System.Drawing.Color.Black;
            this.lblsy.Location = new System.Drawing.Point(4, 2);
            this.lblsy.Name = "lblsy";
            this.lblsy.Size = new System.Drawing.Size(135, 30);
            this.lblsy.TabIndex = 33;
            this.lblsy.Text = "SY:2016-2017";
            // 
            // frmCashierMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1362, 742);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lvwAct);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblActs);
            this.Controls.Add(this.pnlType);
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.btnStudI);
            this.Controls.Add(this.btnPay);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.btnAbt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCashierMain";
            this.Text = "Welcome Cashier";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCashierMain_FormClosing);
            this.Load += new System.EventHandler(this.frmCashierMain_Load);
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
        private System.Windows.Forms.Button btnAbt;
        private System.Windows.Forms.Button btnStudI;
        private System.Windows.Forms.Button btnPay;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Panel pnlType;
        private System.Windows.Forms.Label labelmain;
        private System.Windows.Forms.Label lblActs;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblLoggerCasPosition;
        private System.Windows.Forms.Label lblLoggerCas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvm;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MonthCalendar mcd;
        private System.Windows.Forms.ListView lvwAct;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblsy;
    }
}