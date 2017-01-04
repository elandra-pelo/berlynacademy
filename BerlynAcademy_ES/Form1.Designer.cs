namespace BerlynAcademy_ES
{
    partial class frmHome
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHome));
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lnkClose = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEmp = new System.Windows.Forms.Button();
            this.btnStd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ttp1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlFooter.Controls.Add(this.lnkClose);
            this.pnlFooter.Controls.Add(this.label3);
            this.pnlFooter.Controls.Add(this.label2);
            this.pnlFooter.Controls.Add(this.label1);
            this.pnlFooter.Location = new System.Drawing.Point(0, 646);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1369, 108);
            this.pnlFooter.TabIndex = 0;
            // 
            // lnkClose
            // 
            this.lnkClose.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lnkClose.AutoSize = true;
            this.lnkClose.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkClose.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkClose.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lnkClose.Location = new System.Drawing.Point(1290, 26);
            this.lnkClose.Name = "lnkClose";
            this.lnkClose.Size = new System.Drawing.Size(39, 16);
            this.lnkClose.TabIndex = 4;
            this.lnkClose.TabStop = true;
            this.lnkClose.Text = "close";
            this.lnkClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClose_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(587, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "berlynacademy@yahoo.com";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(510, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(314, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(628, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Berlyn Academy";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnEmp
            // 
            this.btnEmp.BackColor = System.Drawing.Color.Transparent;
            this.btnEmp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEmp.BackgroundImage")));
            this.btnEmp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEmp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEmp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEmp.FlatAppearance.BorderSize = 0;
            this.btnEmp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEmp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEmp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmp.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmp.ForeColor = System.Drawing.Color.White;
            this.btnEmp.Location = new System.Drawing.Point(550, 302);
            this.btnEmp.Name = "btnEmp";
            this.btnEmp.Size = new System.Drawing.Size(106, 92);
            this.btnEmp.TabIndex = 1;
            this.btnEmp.Text = "Employee";
            this.btnEmp.UseVisualStyleBackColor = false;
            this.btnEmp.Click += new System.EventHandler(this.btnEmp_Click);
            this.btnEmp.MouseEnter += new System.EventHandler(this.btnEmp_MouseEnter);
            this.btnEmp.MouseLeave += new System.EventHandler(this.btnEmp_MouseLeave);
            // 
            // btnStd
            // 
            this.btnStd.BackColor = System.Drawing.Color.Transparent;
            this.btnStd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStd.BackgroundImage")));
            this.btnStd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStd.FlatAppearance.BorderSize = 0;
            this.btnStd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStd.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStd.ForeColor = System.Drawing.Color.White;
            this.btnStd.Location = new System.Drawing.Point(714, 302);
            this.btnStd.Name = "btnStd";
            this.btnStd.Size = new System.Drawing.Size(106, 92);
            this.btnStd.TabIndex = 2;
            this.btnStd.Text = "Student";
            this.btnStd.UseVisualStyleBackColor = false;
            this.btnStd.Click += new System.EventHandler(this.btnStd_Click);
            this.btnStd.MouseEnter += new System.EventHandler(this.btnStd_MouseEnter);
            this.btnStd.MouseLeave += new System.EventHandler(this.btnStd_MouseLeave);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(578, 91);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(215, 183);
            this.panel2.TabIndex = 3;
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnStd);
            this.Controls.Add(this.btnEmp);
            this.Controls.Add(this.pnlFooter);
            this.Name = "frmHome";
            this.Text = "Computerized Enrollment System with Online Pre-registration for Berlyn academy";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnEmp;
        private System.Windows.Forms.Button btnStd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel lnkClose;
        private System.Windows.Forms.ToolTip ttp1;
    }
}

