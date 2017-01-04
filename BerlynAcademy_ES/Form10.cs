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
    public partial class frmAboutMaintenance : Form
    {
        public string amlog,VISITED;
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public frmAboutMaintenance()
        {
            InitializeComponent();
        }

        private void frmAboutMaintenance_Load_1(object sender, EventArgs e)
        {
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);

            //this.BackColor = Color.FromArgb(49, 79, 142);
            lblLogger.Text = amlog;
            //btnHome.Text = "          " + amlog;
            btnAbout.BackColor = Color.LightGreen;
            rtbAbout.Enabled = false;
            viewcontent_about();
            if (VISITED.Contains("About") == false)
            {
                VISITED += "   About";

            }
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

        private void btnAddAbout_Click(object sender, EventArgs e)
        {
            if (btnAddAbout.Text == "")
            {
                return;
            }
            else if (btnAddAbout.Text == "Update")
            {
                rtbAbout.Enabled = true;
                btnAddAbout.Text = "Set";
                rtbAbout.Focus();
                btnClearAbout.Text = "Cancel";
            }
            else
            {
               
                con.Open();
                string delabout = "Delete from about_tbl";
                OdbcCommand cmdabt = new OdbcCommand(delabout, con);
                cmdabt.ExecuteNonQuery();
                con.Close();

                con.Open();
                for (int x = 0; x < rtbAbout.Lines.Count(); x++)
                {
                    string line = rtbAbout.Lines[x].ToString();
                    string add = "Insert Into about_tbl (linescontent)values('" + line + "')";
                    OdbcCommand cmdabout = new OdbcCommand(add, con);
                    cmdabout.ExecuteNonQuery();
                }
                con.Close();

                btnAddAbout.Text = "Update";
                rtbAbout.Enabled = false;
                btnReset.Enabled = true;
                btnClearAbout.Enabled = true;
                MessageBox.Show("content successfully saved.", "About maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnClearAbout.Text = "Clear";
            }
        }

        private void btnClearAbout_Click(object sender, EventArgs e)
        {
            if (btnClearAbout.Text == "Clear")
            {
                rtbAbout.Clear();
                btnReset.Enabled = false;
                btnClearAbout.Text = "Cancel";
                lblprev.Text = "...";

            }
            else
            {
                viewcontent_about();
                btnClearAbout.Text = "Clear";
                btnAddAbout.Text = "Update";
                rtbAbout.Enabled = false;
                lblprev.Text = "...";
            }
           
           
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to reset data?", "About maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                con.Open();
                string delabout = "Delete from about_tbl";
                OdbcCommand cmdabt = new OdbcCommand(delabout, con);
                cmdabt.ExecuteNonQuery();
                con.Close();

                viewcontent_about();
                btnReset.Enabled = false;
                MessageBox.Show("content successfully reset.", "About maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnClearAbout.Text = "Clear";
            }
        }

        public void viewcontent_about()
        {
            con.Open();

            OdbcDataAdapter da = new OdbcDataAdapter("Select*from about_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            if (dt.Rows.Count > 0)
            {
                rtbAbout.Text = "";
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    string a = dt.Rows[h].ItemArray[0].ToString();

                    rtbAbout.Text = rtbAbout.Text + a + "\n";
                }
            }
        }

        private void lnkprev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (rtbAbout.Text == "")
            {
                return;
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from about_tbl", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    lblprev.Text = "";
                    for (int h = 0; h < dt.Rows.Count; h++)
                    {
                        string a = dt.Rows[h].ItemArray[0].ToString();

                        lblprev.Text = lblprev.Text + a + "\n";
                    }
                }
            }
        
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maine = new frmMaintenance();
            this.Dispose();
            maine.adminlog = amlog;
            maine.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subjform = new frmSubject();
            this.Dispose();
            subjform.wholog = amlog;
            subjform.Show();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection secform = new frmSection();
            this.Dispose();
            secform.secwholog = amlog;
            secform.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roomform = new frmRoom();
            this.Dispose();
            roomform.logger = amlog;
            roomform.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin homeform = new frmEmpLogin();
            this.Dispose();
            homeform.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqform = new frmRequirement();
            this.Dispose();
            reqform.reqlog = amlog;
            reqform.Show();
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            frmAudit audform = new frmAudit();
            this.Dispose();
            audform.auditlogger = amlog;
            audform.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched schedf = new frmSched();
            this.Dispose();
            schedf.schedlog = amlog;
            schedf.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feeform = new frmFee();
            this.Dispose();
            feeform.feelog = amlog;
            feeform.Show();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Dispose();
            actform.actlog = amlog;
            actform.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Dispose();
            discform.disclog = amlog;
            discform.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Dispose();
            buf.backlog = amlog;
            buf.Show();
        }

        private void frmAboutMaintenance_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Dispose();
            hf.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            
            
        }

        private void btnEdays_Click(object sender, EventArgs e)
        {
            frmEnrollmentDays eform = new frmEnrollmentDays();
            this.Dispose();
            eform.edlog = amlog;
            eform.VISITED = VISITED;
            eform.Show();
        }

        private void btnCoa_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Dispose();
            actform.actlog = amlog;
            actform.VISITED = VISITED;
            actform.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnHomeMainte_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = amlog;
            hm.VISITED = VISITED;
            this.Dispose();
            hm.Show();
        }

        private void btnStudStat_Click(object sender, EventArgs e)
        {
            frmStudentStats stform = new frmStudentStats();
            this.Dispose();
            stform.statlog = amlog;
            stform.VISITED = VISITED;
            stform.Show();
        }

        private void btnPriority_Click(object sender, EventArgs e)
        {
            frmPrioritySec priorsec = new frmPrioritySec();
            this.Dispose();
            priorsec.priorlog = amlog;
            priorsec.VISITED = VISITED;
            priorsec.Show();
        }

        private void btnAssRoom_Click(object sender, EventArgs e)
        {
            frmAssignRoom asrom = new frmAssignRoom();
            this.Dispose();
            asrom.asromlog = amlog;
            asrom.VISITED = VISITED;
            asrom.Show();
        }  
    }
}
