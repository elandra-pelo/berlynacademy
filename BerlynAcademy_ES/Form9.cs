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
    public partial class frmAudit : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string auditlogger,VISITED;
        public DataView dvAud;
        public frmAudit()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);

           // this.BackColor = Color.FromArgb(49, 79, 142);
            lblLogger.Text = auditlogger;
            lblLoggerPosition.Text = "Admin";
            btnAudit.BackColor = Color.LightGreen;
            //btnHome.Text = "          " + auditlogger;
            pnlnotify.Visible = false;
            if (VISITED.Contains("Audit trail") == false)
            {
                VISITED += "   Audit trail";
            }
            setup_VIEW();
        }

        public void setup_VIEW()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select name as 'Name',position as 'Position',date as 'Date',login as 'Login', logout as 'Logout',visited as 'Visited module' from audittrail_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvAud = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvAud;

                dgvSearch.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[0].Width = 200;
                dgvSearch.Columns[1].Width = 140;
                dgvSearch.Columns[2].Width = 230;
                dgvSearch.Columns[3].Width = 120;
                dgvSearch.Columns[4].Width = 120;
                dgvSearch.Columns[5].Width = 850;
                
                lblAsof.Text = "as of " + dgvSearch.Rows[0].Cells[2].Value.ToString();
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
                lblAsof.Text = "";
            }

            lblResult.Text = "number of login: " + dgvSearch.Rows.Count.ToString();
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin homeform = new frmEmpLogin();
            this.Hide();
            homeform.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maine = new frmMaintenance();
            this.Hide();
            maine.adminlog = auditlogger;
            maine.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subform = new frmSubject();
            this.Hide();
            subform.wholog = auditlogger;
            subform.Show();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection secform = new frmSection();
            this.Hide();
            secform.secwholog = auditlogger;
            secform.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roomform = new frmRoom();
            this.Hide();
            roomform.logger = auditlogger;
            roomform.Show();
        }

        private void btnSch_Click(object sender, EventArgs e)
        {
            frmSched sf = new frmSched();
            this.Hide();
            sf.schedlog = auditlogger;
            sf.Show();
        }

        private void btnReqs_Click(object sender, EventArgs e)
        {
            frmRequirement reqform = new frmRequirement();
            this.Hide();
            reqform.reqlog = auditlogger;
            reqform.Show();
        }

        private void btnFees_Click(object sender, EventArgs e)
        {
            frmFee feeform = new frmFee();
            this.Hide();
            feeform.feelog = auditlogger;
            feeform.Show();
        }

        private void btnActs_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Hide();
            actform.actlog = auditlogger;
            actform.Show();
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abtmain = new frmAboutMaintenance();
            this.Hide();
            abtmain.amlog = auditlogger;
            abtmain.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Hide();
            buf.backlog = auditlogger;
            buf.VISITED = VISITED;
            buf.Show();
        }

        private void frmAudit_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            setup_VIEW();
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dvAud.RowFilter = string.Format("Date LIKE '%{0}%'", DateTime.Now.ToLongDateString());
            dgvSearch.DataSource = dvAud;
            lblAsof.Text = "as of today";
            lblResult.Text = "number of login: " + dgvSearch.Rows.Count.ToString();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            frmDiscount df = new frmDiscount();
            this.Hide(); df.disclog = auditlogger;
            df.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            
        }

        private void btnHomeMainte_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = auditlogger;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnUserAcc_Click(object sender, EventArgs e)
        {
            frmUserAccessLevel ualform = new frmUserAccessLevel();
            this.Hide();
            ualform.acclog = auditlogger;
            ualform.VISITED = VISITED;
            ualform.Show();
        }

        private void dtpgo_ValueChanged(object sender, EventArgs e)
        {
            dvAud.RowFilter = string.Format("Date LIKE '%{0}%'",dtpgo.Text);
            dgvSearch.DataSource = dvAud;

            if (dtpgo.Text == DateTime.Now.ToLongDateString())
            {
                lblAsof.Text = "as of today";
                lblResult.Text = "number of login: " + dgvSearch.Rows.Count.ToString();
            }
            else
            {
                lblAsof.Text = "as of " + dtpgo.Text;
                lblResult.Text = "number of login: " + dgvSearch.Rows.Count.ToString();
            }
        }

        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dgvSearch.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
            }
            else
            {
                pnlnotify.Visible = true;
            }
        }

      
    }
}
