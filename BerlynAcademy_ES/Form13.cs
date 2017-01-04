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
    public partial class frmActivity : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string actlog,temp,VISITED,activeSY,activeYr;
        public DataView dvact;
        public frmActivity()
        {
            InitializeComponent();
        }

        private void frmActivity_Load(object sender, EventArgs e)
        {
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //pnlhead.BackColor = Color.FromArgb(244, 194, 13);
            //this.BackColor = Color.FromArgb(49, 79, 142);
            lblLogger.Text = actlog;
            lblLoggerPosition.Text = "Admin";
            btncoa.BackColor = Color.LightGreen;
            //btnHome.Text = "          " + actlog;

            dtpDue.Format = DateTimePickerFormat.Custom;
            // Display the date as "Mon 26 Feb 2001".
            dtpDue.CustomFormat = "ddd, dd MMM yyyy";
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            if (VISITED.Contains("Calendar of activities") == false)
            {
                VISITED += "   Calendar of activities";

            }
            setupdays();
            setupyears();
            GetActiveSchoolYear();
            setupview();
            
        }

        public void GetActiveSchoolYear()
        {
            con.Open();
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);
            con.Close();
            if (dtssy.Rows.Count > 0)
            {

                activeSY = dtssy.Rows[0].ItemArray[1].ToString();
                activeYr = dtssy.Rows[0].ItemArray[0].ToString();
                int yr = Convert.ToInt32(activeYr);
                lblSY.Text = activeSY;
                dtpDue.Value = new DateTime(yr, DateTime.Now.Month, DateTime.Now.Day);
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

        public void setupyears()
        {
            int current = Convert.ToInt32(DateTime.Now.Year);
            cmbYears.Items.Add(current);
        }

        public void setupdays()
        {
            cmbDay.Items.Clear();

            int start = 1;
            while (start <= 31)
            {
                if (start < 10)
                {
                    cmbDay.Items.Add("0" + start);
                }
                else
                {
                    cmbDay.Items.Add(start);
                }
                start++;
            }
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance main = new frmMaintenance();
            this.Hide();
            main.adminlog = actlog;
            main.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subform = new frmSubject();
            this.Hide();
            subform.wholog = actlog;
            subform.Show();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection secform = new frmSection();
            this.Hide();
            secform.secwholog = actlog;
            secform.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roomform = new frmRoom();
            this.Hide();
            roomform.logger = actlog;
            roomform.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched sf = new frmSched();
            this.Hide();
            sf.schedlog = actlog;
            sf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqform = new frmRequirement();
            this.Hide();
            reqform.reqlog = actlog;
            reqform.Show();
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            frmAudit audform = new frmAudit();
            this.Hide();
            audform.auditlogger = actlog;
            audform.Show();
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abtform = new frmAboutMaintenance();
            this.Hide();
            abtform.amlog = actlog;
            abtform.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Clear")
            {
                txtAct.Clear();

                cmbMonth.SelectedIndex = -1;
                cmbDay.SelectedIndex = -1;
                cmbYears.SelectedIndex = -1;
                txtAct.Enabled = true;
                cmbMonth.Enabled = true;
                cmbDay.Enabled = true;
                cmbYears.Enabled = true;
                dtpDue.Enabled = true;
                

                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnAdd.Enabled = true;
                btnUpdate.Text = "Update";
            }
            else
            {
                btnDelete.Enabled = true;
                btnClear.Text = "Clear";
                btnUpdate.Text = "Update";
                txtAct.Enabled = false;
                cmbMonth.Enabled = false;
                cmbDay.Enabled = false;
                cmbYears.Enabled = false;
                setup_retrieve(temp);
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

           
        }

        public void setup_retrieve(string thekey)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from activity_tbl where activity='" + thekey + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                txtAct.Text = dt.Rows[0].ItemArray[0].ToString();
                cmbMonth.Text = dt.Rows[0].ItemArray[1].ToString().Substring(3, 3);
                cmbDay.Text = dt.Rows[0].ItemArray[1].ToString().Substring(0, 2);
                cmbYears.Text = dt.Rows[0].ItemArray[1].ToString().Substring(7, 4);

                btnAdd.Enabled = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete?", "Activity maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtAct.Text == "" || dtpDue.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Activity maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    setup_delete();
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;

                    txtAct.Clear();
                    cmbMonth.SelectedIndex = -1;
                    cmbDay.SelectedIndex = -1;
                    cmbYears.SelectedIndex = -1;

                    txtAct.Enabled = true;
                    cmbMonth.Enabled = true;
                    cmbDay.Enabled = true;
                    cmbYears.Enabled = true;

                    btnAdd.Enabled = true;
                    if (dgvSearch.Rows.Count >= 1)
                    {
                        dgvSearch.Rows[0].Selected = true;
                    }
                }
            }
            else
            {
                return;
            }
        }

        public void setup_delete()
        {
            con.Open();
            string delete = "Delete from activity_tbl where activity='" + temp + "'and SY='"+activeSY+"'";
            OdbcCommand cmdDelete = new OdbcCommand(delete, con);
            cmdDelete.ExecuteNonQuery();
            btnAdd.Enabled = false;
            con.Close();
           
            setupview();
            MessageBox.Show("activity successfully deleted", "Activity maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
        }

        public void setupview()
        {
            dgvSearch.DataSource = null;

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select date as 'Date',activity as'Activity' from activity_tbl where SY='"+activeSY+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvact = new DataView(dt);
            dgvSearch.DataSource = dvact;

           
            dgvSearch.Columns[0].Width = 150;
            dgvSearch.Columns[1].Width = 430;

         
            lblResult.Text = "number of activities: " + dgvSearch.Rows.Count.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Update")
            {
                txtAct.Enabled = true;
                cmbMonth.Enabled = true;
                cmbDay.Enabled = true;
                cmbYears.Enabled = true;
                dtpDue.Enabled = true;
                btnUpdate.Text = "Save";
                btnDelete.Enabled = false;
                btnClear.Text = "Cancel";
            }
            else
            {
                if (txtAct.Text == "" || dtpDue.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Activity maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    setup_save();
                    btnUpdate.Text = "Update";
                    btnClear.Text = "Clear";
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                    if (dgvSearch.Rows.Count >= 1)
                    {
                        dgvSearch.Rows[0].Selected = true;
                    }
                }
            }
        }


        public void setup_save()
        {
            con.Open();
            string day = cmbDay.Text + " " + cmbMonth.Text + " " + cmbYears.Text;
            string updateact = "Update activity_tbl set activity='" + txtAct.Text + "',date='"+dtpDue.Text+"'where activity='"+temp+"'and SY='"+activeSY+"'";
            OdbcCommand cmdUpdateRoom = new OdbcCommand(updateact, con);
            cmdUpdateRoom.ExecuteNonQuery();
            con.Close();

            btnAdd.Enabled = false;
            setupview();
            btnClear.Text = "Clear";
            MessageBox.Show("activity successfully updated", "Activity maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtAct.Text == "" || dtpDue.Text=="")
            {
                MessageBox.Show("fill out required fields.", "Activity maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                setupAdd();
            }
        }

        public void setupAdd()
        {
            con.Open();
            string day = cmbDay.Text + " " + cmbMonth.Text + " " + cmbYears.Text;
            string add = "Insert Into activity_tbl(activity,date,SY)values('" + txtAct.Text + "','"+dtpDue.Text+"','"+activeSY+"')";

            OdbcCommand cmdAdd = new OdbcCommand(add, con);
            cmdAdd.ExecuteNonQuery();
            con.Close();

            btnAdd.Enabled = false;
            setupview();
            MessageBox.Show("activity successfully added", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            txtAct.Enabled = false;
            cmbDay.Enabled = false;
            cmbMonth.Enabled = false;
            cmbYears.Enabled = false;
            dtpDue.Enabled = false;
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            if (dgvSearch.Rows.Count <=0)
            {
                return;
            }
            temp = dgvSearch.SelectedRows[0].Cells[1].Value.ToString();
            txtAct.Text = dgvSearch.SelectedRows[0].Cells[1].Value.ToString();
            dtpDue.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
           
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feeform = new frmFee();
            this.Hide();
            feeform.feelog = actlog;
            feeform.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Hide();
            discform.disclog = actlog;
            discform.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Hide();
            buf.backlog = actlog;
            buf.Show();
        }

        private void frmActivity_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            
        }

        private void pnlMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnEdays_Click(object sender, EventArgs e)
        {
            frmEnrollmentDays eform = new frmEnrollmentDays();
            this.Dispose();
            eform.edlog = actlog;
            eform.VISITED = VISITED;
            eform.Show();
        }

        private void btncoa_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abm = new frmAboutMaintenance();
            this.Dispose();
            abm.amlog = actlog;
            abm.VISITED = VISITED;
            abm.Show();
        }

        private void btnHomeMainte_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = actlog;
            hm.VISITED = VISITED;
            this.Dispose();
            hm.Show();
        }

        private void btnStudStat_Click(object sender, EventArgs e)
        {
            frmStudentStats stform = new frmStudentStats();
            this.Dispose();
            stform.statlog = actlog;
            stform.VISITED = VISITED;
            stform.Show();
        }

        private void btnPriority_Click(object sender, EventArgs e)
        {
            frmPrioritySec priorsec = new frmPrioritySec();
            this.Dispose();
            priorsec.priorlog = actlog;
            priorsec.VISITED = VISITED;
            priorsec.Show();
        }

        private void lblSY_Click(object sender, EventArgs e)
        {

        }

        private void btnAssRoom_Click(object sender, EventArgs e)
        {
            frmAssignRoom asrom = new frmAssignRoom();
            this.Dispose();
            asrom.asromlog = actlog;
            asrom.VISITED = VISITED;
            asrom.Show();
        }
    }
}
