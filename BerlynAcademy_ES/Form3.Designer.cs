namespace BerlynAcademy_ES
{
    partial class frmEmpMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmpMain));
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.dgvm = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblLoggerPosition = new System.Windows.Forms.Label();
            this.lblLogger = new System.Windows.Forms.Label();
            this.btnFacFacAdvisory = new System.Windows.Forms.Button();
            this.btnFacStudRec = new System.Windows.Forms.Button();
            this.btnFacPayment = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnSectioning = new System.Windows.Forms.Button();
            this.btnFacRep = new System.Windows.Forms.Button();
            this.btnGrade = new System.Windows.Forms.Button();
            this.btnFac = new System.Windows.Forms.Button();
            this.btnStudI = new System.Windows.Forms.Button();
            this.btnAdm = new System.Windows.Forms.Button();
            this.pnlType = new System.Windows.Forms.Panel();
            this.labelmain = new System.Windows.Forms.Label();
            this.lblActs = new System.Windows.Forms.Label();
            this.mcd = new System.Windows.Forms.MonthCalendar();
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.pnlMenu.Controls.Add(this.label3);
            this.pnlMenu.Controls.Add(this.pictureBox1);
            this.pnlMenu.Controls.Add(this.lblLoggerPosition);
            this.pnlMenu.Controls.Add(this.lblLogger);
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(263, 757);
            this.pnlMenu.TabIndex = 6;
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
            this.dgvm.TabIndex = 91;
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
            this.label1.TabIndex = 89;
            this.label1.Text = "_____________________________________";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Turquoise;
            this.label3.Font = new System.Drawing.Font("Segoe UI Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(45, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 32);
            this.label3.TabIndex = 88;
            this.label3.Text = "Menu";
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
            this.lblLoggerPosition.Size = new System.Drawing.Size(56, 21);
            this.lblLoggerPosition.TabIndex = 30;
            this.lblLoggerPosition.Text = "Faculty";
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
            // btnFacFacAdvisory
            // 
            this.btnFacFacAdvisory.BackColor = System.Drawing.Color.White;
            this.btnFacFacAdvisory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFacFacAdvisory.FlatAppearance.BorderSize = 0;
            this.btnFacFacAdvisory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFacFacAdvisory.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFacFacAdvisory.Location = new System.Drawing.Point(269, 492);
            this.btnFacFacAdvisory.Name = "btnFacFacAdvisory";
            this.btnFacFacAdvisory.Size = new System.Drawing.Size(79, 41);
            this.btnFacFacAdvisory.TabIndex = 93;
            this.btnFacFacAdvisory.Text = "          Faculty advisory";
            this.btnFacFacAdvisory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFacFacAdvisory.UseVisualStyleBackColor = false;
            this.btnFacFacAdvisory.Visible = false;
            this.btnFacFacAdvisory.Click += new System.EventHandler(this.btnFacFacAdvisory_Click);
            // 
            // btnFacStudRec
            // 
            this.btnFacStudRec.BackColor = System.Drawing.Color.White;
            this.btnFacStudRec.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFacStudRec.FlatAppearance.BorderSize = 0;
            this.btnFacStudRec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFacStudRec.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFacStudRec.Location = new System.Drawing.Point(269, 448);
            this.btnFacStudRec.Name = "btnFacStudRec";
            this.btnFacStudRec.Size = new System.Drawing.Size(79, 41);
            this.btnFacStudRec.TabIndex = 92;
            this.btnFacStudRec.Text = "          Student records";
            this.btnFacStudRec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFacStudRec.UseVisualStyleBackColor = false;
            this.btnFacStudRec.Visible = false;
            // 
            // btnFacPayment
            // 
            this.btnFacPayment.BackColor = System.Drawing.Color.White;
            this.btnFacPayment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFacPayment.FlatAppearance.BorderSize = 0;
            this.btnFacPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFacPayment.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFacPayment.Location = new System.Drawing.Point(269, 401);
            this.btnFacPayment.Name = "btnFacPayment";
            this.btnFacPayment.Size = new System.Drawing.Size(79, 41);
            this.btnFacPayment.TabIndex = 91;
            this.btnFacPayment.Text = "          Payment";
            this.btnFacPayment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFacPayment.UseVisualStyleBackColor = false;
            this.btnFacPayment.Visible = false;
            this.btnFacPayment.Click += new System.EventHandler(this.btnFacPayment_Click);
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.White;
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(269, 586);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(79, 41);
            this.btnHome.TabIndex = 23;
            this.btnHome.Text = "          Logout";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Visible = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.White;
            this.btnAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbout.FlatAppearance.BorderSize = 0;
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.Location = new System.Drawing.Point(269, 539);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(79, 41);
            this.btnAbout.TabIndex = 17;
            this.btnAbout.Text = "          About us";
            this.btnAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Visible = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnSectioning
            // 
            this.btnSectioning.BackColor = System.Drawing.Color.White;
            this.btnSectioning.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSectioning.FlatAppearance.BorderSize = 0;
            this.btnSectioning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSectioning.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSectioning.Location = new System.Drawing.Point(269, 140);
            this.btnSectioning.Name = "btnSectioning";
            this.btnSectioning.Size = new System.Drawing.Size(79, 41);
            this.btnSectioning.TabIndex = 90;
            this.btnSectioning.Text = "          Sectioning";
            this.btnSectioning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSectioning.UseVisualStyleBackColor = false;
            this.btnSectioning.Visible = false;
            this.btnSectioning.Click += new System.EventHandler(this.btnSectioning_Click);
            // 
            // btnFacRep
            // 
            this.btnFacRep.BackColor = System.Drawing.Color.White;
            this.btnFacRep.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFacRep.FlatAppearance.BorderSize = 0;
            this.btnFacRep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFacRep.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFacRep.Location = new System.Drawing.Point(269, 308);
            this.btnFacRep.Name = "btnFacRep";
            this.btnFacRep.Size = new System.Drawing.Size(79, 41);
            this.btnFacRep.TabIndex = 25;
            this.btnFacRep.Text = "          Report";
            this.btnFacRep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFacRep.UseVisualStyleBackColor = false;
            this.btnFacRep.Visible = false;
            this.btnFacRep.Click += new System.EventHandler(this.btnFacRep_Click);
            // 
            // btnGrade
            // 
            this.btnGrade.BackColor = System.Drawing.Color.White;
            this.btnGrade.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGrade.FlatAppearance.BorderSize = 0;
            this.btnGrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrade.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGrade.Location = new System.Drawing.Point(269, 266);
            this.btnGrade.Name = "btnGrade";
            this.btnGrade.Size = new System.Drawing.Size(79, 41);
            this.btnGrade.TabIndex = 24;
            this.btnGrade.Text = "          Student grades";
            this.btnGrade.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGrade.UseVisualStyleBackColor = false;
            this.btnGrade.Visible = false;
            this.btnGrade.Click += new System.EventHandler(this.btnGrade_Click);
            // 
            // btnFac
            // 
            this.btnFac.BackColor = System.Drawing.Color.White;
            this.btnFac.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFac.FlatAppearance.BorderSize = 0;
            this.btnFac.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFac.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFac.Location = new System.Drawing.Point(269, 224);
            this.btnFac.Name = "btnFac";
            this.btnFac.Size = new System.Drawing.Size(79, 41);
            this.btnFac.TabIndex = 16;
            this.btnFac.Text = "          Faculty information";
            this.btnFac.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFac.UseVisualStyleBackColor = false;
            this.btnFac.Visible = false;
            this.btnFac.Click += new System.EventHandler(this.btnFac_Click);
            // 
            // btnStudI
            // 
            this.btnStudI.BackColor = System.Drawing.Color.White;
            this.btnStudI.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStudI.FlatAppearance.BorderSize = 0;
            this.btnStudI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudI.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStudI.Location = new System.Drawing.Point(269, 182);
            this.btnStudI.Name = "btnStudI";
            this.btnStudI.Size = new System.Drawing.Size(79, 41);
            this.btnStudI.TabIndex = 15;
            this.btnStudI.Text = "          Student information";
            this.btnStudI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStudI.UseVisualStyleBackColor = false;
            this.btnStudI.Visible = false;
            this.btnStudI.Click += new System.EventHandler(this.btnStudI_Click);
            // 
            // btnAdm
            // 
            this.btnAdm.BackColor = System.Drawing.Color.White;
            this.btnAdm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdm.FlatAppearance.BorderSize = 0;
            this.btnAdm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdm.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdm.Location = new System.Drawing.Point(269, 354);
            this.btnAdm.Name = "btnAdm";
            this.btnAdm.Size = new System.Drawing.Size(79, 41);
            this.btnAdm.TabIndex = 14;
            this.btnAdm.Text = "          Admission";
            this.btnAdm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdm.UseVisualStyleBackColor = false;
            this.btnAdm.Visible = false;
            this.btnAdm.Click += new System.EventHandler(this.btnAdm_Click);
            // 
            // pnlType
            // 
            this.pnlType.BackColor = System.Drawing.Color.Crimson;
            this.pnlType.Controls.Add(this.labelmain);
            this.pnlType.Location = new System.Drawing.Point(263, 0);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(1111, 79);
            this.pnlType.TabIndex = 42;
            // 
            // labelmain
            // 
            this.labelmain.AutoSize = true;
            this.labelmain.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelmain.ForeColor = System.Drawing.Color.White;
            this.labelmain.Location = new System.Drawing.Point(40, 24);
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
            this.lblActs.TabIndex = 43;
            // 
            // mcd
            // 
            this.mcd.BackColor = System.Drawing.Color.White;
            this.mcd.Location = new System.Drawing.Point(5, 28);
            this.mcd.Name = "mcd";
            this.mcd.TabIndex = 95;
            this.mcd.TitleBackColor = System.Drawing.Color.Crimson;
            this.mcd.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.mcd);
            this.panel1.Location = new System.Drawing.Point(1118, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 195);
            this.panel1.TabIndex = 96;
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
            this.lblsy.TabIndex = 31;
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
            // frmEmpMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1362, 742);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lvwAct);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.btnFacFacAdvisory);
            this.Controls.Add(this.lblActs);
            this.Controls.Add(this.btnFacStudRec);
            this.Controls.Add(this.pnlType);
            this.Controls.Add(this.btnFacPayment);
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.btnGrade);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnAdm);
            this.Controls.Add(this.btnSectioning);
            this.Controls.Add(this.btnStudI);
            this.Controls.Add(this.btnFac);
            this.Controls.Add(this.btnFacRep);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEmpMain";
            this.Text = "Welcome Faculty";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEmpMain_FormClosing);
            this.Load += new System.EventHandler(this.frmEmpMain_Load);
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
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnFac;
        private System.Windows.Forms.Button btnStudI;
        private System.Windows.Forms.Button btnAdm;
        private System.Windows.Forms.Button btnFacRep;
        private System.Windows.Forms.Button btnGrade;
        private System.Windows.Forms.Panel pnlType;
        private System.Windows.Forms.Label labelmain;
        private System.Windows.Forms.Label lblActs;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblLoggerPosition;
        private System.Windows.Forms.Label lblLogger;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSectioning;
        private System.Windows.Forms.Button btnFacFacAdvisory;
        private System.Windows.Forms.Button btnFacStudRec;
        private System.Windows.Forms.Button btnFacPayment;
        private System.Windows.Forms.DataGridView dgvm;
        private System.Windows.Forms.MonthCalendar mcd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView lvwAct;
        private System.Windows.Forms.Label lblsy;
    }
}