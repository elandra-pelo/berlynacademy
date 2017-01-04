using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;

namespace BerlynAcademy_ES
{
    public partial class frmUserAccessLevel : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string acclog,VISITED;
        public static bool isupdate, selecting;
        public frmUserAccessLevel()
        {
            InitializeComponent();
        }

        private void frmUserAccessLevel_Load(object sender, EventArgs e)
        {
            lblLogger.Text = acclog;
            lblLoggerPosition.Text = "Admin";
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);

            btnUpdateAccess.Enabled = false; btnCancel.Visible = false;
            btnUpdateAccess.Location = new Point(197, 597);
            lblDesc.Text = "Admission module will allow the user to admit enrollees during" + "\nthe enrollment. It has Registration where the user will" + "\nencode enrollees information, Submission of requirements" + "\nand Assessment.";
            btnUserAcc.BackColor = Color.LightGreen;

            if (VISITED.Contains("User access level") == false)
            {
                VISITED += "   User access level";
            }
        }

        private void btnAud_Click(object sender, EventArgs e)
        {
            frmAudit auditform = new frmAudit();
            this.Hide();
            auditform.auditlogger = acclog;
            auditform.VISITED = VISITED;
            auditform.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Hide();
            buf.backlog = acclog;
            buf.VISITED = VISITED;
            buf.Show();
        }

        private void btnHomeMainte_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = acclog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void btnUserAcc_Click(object sender, EventArgs e)
        {
            return;
        }

        private void cmbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            selecting = true;
            string accesscode = "";
            setupToUncheckCheckbox();
            btnUpdateAccess.Enabled = true;

            con.Open();
            OdbcDataAdapter daa = new OdbcDataAdapter("Select*from useraccesslevel_tbl", con);
            DataTable dtt = new DataTable();
            daa.Fill(dtt);
            con.Close();

            if (dtt.Rows.Count > 0)
            {
                string Facposition = dtt.Rows[0].ItemArray[1].ToString();
                string Facacccode = dtt.Rows[0].ItemArray[2].ToString();

                string Regposition = dtt.Rows[3].ItemArray[1].ToString();
                string Regacccode = dtt.Rows[3].ItemArray[2].ToString();

                string Priposition = dtt.Rows[1].ItemArray[1].ToString();
                string Priacccode = dtt.Rows[1].ItemArray[2].ToString();

                string Casposition = dtt.Rows[2].ItemArray[1].ToString();
                string Casacccode = dtt.Rows[2].ItemArray[2].ToString();

                if (((Facposition == "Faculty") && (Facacccode == "4589")) && ((Regposition == "Registrar") && (Regacccode == "135890")) && ((Priposition == "Principal") && (Priacccode == "5679")) && ((Casposition == "Cashier") && (Casacccode == "25")))
                {
                    chkDef.Checked = true;
                }
                else
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from useraccesslevel_tbl where position='" + cmbUserType.Text + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        accesscode = dt.Rows[0].ItemArray[2].ToString();
                    }

                    if (accesscode.Contains("1") == true)
                    {
                        chkAdm.Checked = true;
                    }
                    if (accesscode.Contains("2") == true)
                    {
                        chkPay.Checked = true;
                    }
                    if (accesscode.Contains("3") == true)
                    {
                        chkStudRec.Checked = true;
                    }
                    if (accesscode.Contains("4") == true)
                    {
                        chkStudGrd.Checked = true;
                    }
                    if (accesscode.Contains("5") == true)
                    {
                        chkStudInfo.Checked = true;
                    }
                    if (accesscode.Contains("6") == true)
                    {
                        chkFacInfo.Checked = true;
                    }
                    if (accesscode.Contains("7") == true)
                    {
                        chkFacAdv.Checked = true;
                    }
                    if (accesscode.Contains("8") == true)
                    {
                        chkSec.Checked = true;
                    }
                    if (accesscode.Contains("9") == true)
                    {
                        chkRep.Checked = true;
                    }
                    if (accesscode.Contains("0") == true)
                    {
                        chkSched.Checked = true;
                    }
                }
            }


            selecting = false;
        }

        private void btnUpdateAccess_Click(object sender, EventArgs e)
        {
            if (btnUpdateAccess.Text == "Update")
            {
                btnUpdateAccess.Text = "Save";
                btnCancel.Visible = true;
                btnCancel.Location = new Point(197, 597);
                btnUpdateAccess.Location = new Point(79, 597);
                pnlcheck.Enabled = true;
                selecting = false;
            }
            else
            {
                string accesscode = "";
                if (chkAdm.Checked == true)
                {
                    accesscode += "1";
                }
                if (chkPay.Checked == true)
                {
                    accesscode += "2";
                }
                if (chkStudRec.Checked == true)
                {
                    accesscode += "3";
                }
                if (chkStudGrd.Checked == true)
                {
                    accesscode += "4";
                }
                if (chkStudInfo.Checked == true)
                {
                    accesscode += "5";
                }
                if (chkFacInfo.Checked == true)
                {
                    accesscode += "6";
                }
                if (chkFacAdv.Checked == true)
                {
                    accesscode += "7";
                }
                if (chkSec.Checked == true)
                {
                    accesscode += "8";
                }
                if (chkRep.Checked == true)
                {
                    accesscode += "9";
                }
                if (chkSched.Checked == true)
                {
                    accesscode += "0";
                }


                if (chkDef.Checked == true)
                {
                    
                    if (cmbUserType.Text == "Faculty")
                    {
                        con.Open();
                        string update = "Update useraccesslevel_tbl set accesscode='" + "4589" + "'where position='" + "Faculty" + "'";
                        OdbcCommand cmd = new OdbcCommand(update, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    if (cmbUserType.Text == "Registrar")
                    {
                        con.Open();
                        string update2 = "Update useraccesslevel_tbl set accesscode='" + "135890" + "'where position='" + "Registrar" + "'";
                        OdbcCommand cmd2 = new OdbcCommand(update2, con);
                        cmd2.ExecuteNonQuery();
                        con.Close();
                    }
                    if (cmbUserType.Text == "Principal")
                    {
                        con.Open();
                        string update3 = "Update useraccesslevel_tbl set accesscode='" + "5679" + "'where position='" + "Principal" + "'";
                        OdbcCommand cmd3 = new OdbcCommand(update3, con);
                        cmd3.ExecuteNonQuery();
                        con.Close();
                    }
                    if (cmbUserType.Text == "Cashier")
                    {
                        con.Open();
                        string update4 = "Update useraccesslevel_tbl set accesscode='" + "25" + "'where position='" + "Cashier" + "'";
                        OdbcCommand cmd4 = new OdbcCommand(update4, con);
                        cmd4.ExecuteNonQuery();
                        con.Close();
                    }

                   
                }
                else
                {
                    con.Open();
                    string update = "Update useraccesslevel_tbl set accesscode='" + accesscode + "'where position='" + cmbUserType.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(update, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }


                pnlcheck.Enabled = false;
                btnUpdateAccess.Enabled = false;

                //setupToUncheckCheckbox();
                setupPositions();
                pnlModule.BackgroundImage = Properties.Resources.Adm;
                lblDesc.Text = "Admission module will allow the user to admit enrollees during" + "\nthe enrollment. It has Registration where the user will" + "\nencode enrollees information, Submission of requirements" + "\nand Assessment.";

                MessageBox.Show("changes successfully saved.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnCancel.Visible = false;
                btnUpdateAccess.Text = "Update";
                btnUpdateAccess.Location = new Point(197, 597);


            }
        }

        public void setupPositions()
        {
            cmbUserType.Items.Clear();
            cmbUserType.Items.Add("Cashier");
            cmbUserType.Items.Add("Faculty");
            cmbUserType.Items.Add("Principal");
            cmbUserType.Items.Add("Registrar");

        }

        public void setupToUncheckCheckbox()
        {
            chkAdm.Checked = false;
            chkPay.Checked = false;
            chkStudGrd.Checked = false;
            chkStudInfo.Checked = false;
            chkStudRec.Checked = false;
            chkFacAdv.Checked = false;
            chkFacInfo.Checked = false;
            chkRep.Checked = false;
            chkAdm.Checked = false;
            chkDef.Checked = false;
            chkSec.Checked = false;
            chkSched.Checked = false;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            setupPositions();
            setupToUncheckCheckbox();

            selecting = false;
            pnlcheck.Enabled = false;
            btnCancel.Visible = false;
            btnUpdateAccess.Text = "Update";
            btnUpdateAccess.Location = new Point(197, 597);
            btnUpdateAccess.Enabled = false;
            pnlModule.BackgroundImage = Properties.Resources.Adm;
            lblDesc.Text = "Admission module will allow the user to admit enrollees during" + "\nthe enrollment. It has Registration where the user will" + "\nencode enrollees information, Submission of requirements" + "\nand Assessment.";
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin homef = new frmEmpLogin();
            this.Hide();
            homef.Show();
        }

        public void LOGOUT()
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter daout = new OdbcDataAdapter("Select * from audittrail_tbl", con);
            daout.Fill(dt);
            con.Close();

            string time = DateTime.Now.ToString("hh:mm tt");
            string def = "...";
            con.Open();
            string setOut = "Update audittrail_tbl set logout='" + time + "',visited='"+VISITED+"'Where logout='" + def + "'";
            OdbcCommand cmd = new OdbcCommand(setOut, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void frmUserAccessLevel_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin homef = new frmEmpLogin();
            this.Hide();
            homef.Show();
        }

        private void chkDef_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDef.Checked == true)
            {
                chkAdm.Checked = false;
                chkPay.Checked = false;
                chkStudRec.Checked = false;
                chkStudGrd.Checked = false;
                chkStudInfo.Checked = false;
                chkFacInfo.Checked = false;
                chkFacAdv.Checked = false;
                chkSec.Checked = false;
                chkRep.Checked = false;
                chkSched.Checked = false;
            }
        }

        private void chkFacInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Fai;
                lblDesc.Text = "Faculty information module will allow the user to view faculty's" + "\nbasic information as well as their Advisory class and Class schedules.";
            }
        }

        private void chkAdm_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Adm;
                lblDesc.Text = "Admission module will allow the user to admit enrollees during" + "\nthe enrollment. It has Registration where the user will" + "\nencode enrollees information, Submission of requirements" + "\nand Assessment.";
            }
        }

        private void chkPay_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Pay;
                lblDesc.Text = "Payment module will allow the user to handle payment of tuition" + "\nfees.";
            }
        }

        private void chkStudRec_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Str;
                lblDesc.Text = "Student records module will allow the user to update requirements" + "\nsubmitted by enrollee or student. It will also view Student's Assessment," + "\nPayment history, Grades and current Class schedule; This module" + "\ngenerating Registration form for students.";
            }
        }

        private void chkStudGrd_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Stg;
                lblDesc.Text = "Student grades module will allow the user to encode student's grades" + "\nper subject every quarter.";
            }
        }

        private void chkStudInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Sti;
                lblDesc.Text = "Student information module will allow the user to view student's" + "\nbasic information as well as their Grade level, Section and Class" + "\nadviser.";
            }
        }

        private void chkFacAdv_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Faa;
                lblDesc.Text = "Faculty advisory module will allow the user to assign advisory class" + "\nfor every faculty.";
            }
        }

        private void chkSec_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Sec;
                lblDesc.Text = "Sectioning module will allow the user to assign section for every" + "\nenrollee or student who don't have section.";
            }
        }

        private void chkRep_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Rep;
                lblDesc.Text = "Report module will allow the user to view reports such as:" + "\n- Students Master list                                 - Passed and Failed Students" + "\n- Students without Section                         - List of Graduating Students" + "\n- Students in a Class" + "\n- Registered enrolees this School year";
            }
        }

        private void chkSched_CheckedChanged(object sender, EventArgs e)
        {
            if (selecting == false)
            {
                chkDef.Checked = false;
                pnlModule.BackgroundImage = Properties.Resources.Sch;
                lblDesc.Text = "Scheduling module will allow the user to set Class schedule and view Faculty schedule.";
            }
        }
    }
}
