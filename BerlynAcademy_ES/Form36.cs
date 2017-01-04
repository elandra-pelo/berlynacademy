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
    public partial class frmEnrollmentDays : Form
    {
        public string edlog,VISITED,activeSY,activeYr;
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public frmEnrollmentDays()
        {
            InitializeComponent();
        }

        private void frmEnrollmentDays_Load(object sender, EventArgs e)
        {
            lblLogger.Text = edlog;
            lblLoggerPosition.Text = "Admin";
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            btnEdays.BackColor = Color.LightGreen;
            GetActiveSchoolYear();
            setupLoadStoredDays();
            if (VISITED.Contains("Enrollment days") == false)
            {
                VISITED += "   Enrollment days";

            }
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
                
            }

        }

        public void setupLoadStoredDays()
        {
           
             con.Open();
            DataTable dted = new DataTable();
            OdbcDataAdapter daed = new OdbcDataAdapter("Select * from enrollmentdays_tbl where SY='"+activeSY+"'", con);
            daed.Fill(dted);
            con.Close();
            if (dted.Rows.Count > 0)
            {
                if (dted.Rows[0].ItemArray[0].ToString() != "" && dted.Rows[0].ItemArray[1].ToString() != "")
                {
                    DateTime start = Convert.ToDateTime(dted.Rows[0].ItemArray[0].ToString());
                    DateTime end = Convert.ToDateTime(dted.Rows[0].ItemArray[1].ToString());
                    dtpStart.Value = start;
                    dtpEnd.Value = end;
                }
                else
                {
                    int yr = Convert.ToInt32(activeYr);
                    dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    dtpEnd.Value = new DateTime(yr, DateTime.Now.Month, DateTime.Now.Day);
                }
            }
        }

        private void btnEdays_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnCoa_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Hide();
            actform.actlog = edlog;
            actform.VISITED = VISITED;
            actform.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abm = new frmAboutMaintenance();
            this.Hide();
            abm.amlog = edlog;
            abm.VISITED = VISITED;
            abm.Show();
        }

        private void btnHomeMainte_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = edlog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void frmEnrollmentDays_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
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

        private void btnStdStat_Click(object sender, EventArgs e)
        {
            frmStudentStats stform = new frmStudentStats();
            this.Hide();
            stform.statlog = edlog;
            stform.VISITED = VISITED;
            stform.Show();
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan span = dtpEnd.Value.Subtract(dtpStart.Value);
            txtSpan.Text = span.Days.ToString();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (dtpStart.Text=="" || dtpStart.Text=="" || txtSpan.Text=="")
            {
                MessageBox.Show("please select enrollment days", "Setting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtpStart.Value == dtpEnd.Value)
            {
                MessageBox.Show("Invalid selection", "Setting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {


                con.Open();
                string update = "Update enrollmentdays_tbl set start='" + dtpStart.Value.ToLongDateString() + "',end='" + dtpEnd.Value.ToLongDateString() + "'where SY='"+activeSY+"'";
                OdbcCommand cmd = new OdbcCommand(update, con);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Enrollment days successfully set", "Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPriority_Click(object sender, EventArgs e)
        {
            frmPrioritySec priorsec = new frmPrioritySec();
            this.Dispose();
            priorsec.priorlog = edlog;
            priorsec.VISITED = VISITED;
            priorsec.Show();
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan span = dtpEnd.Value.Subtract(dtpStart.Value);
            txtSpan.Text = span.Days.ToString();
        }

        private void btnAssRoom_Click(object sender, EventArgs e)
        {
            frmAssignRoom asrom = new frmAssignRoom();
            this.Dispose();
            asrom.asromlog = edlog;
            asrom.VISITED = VISITED;
            asrom.Show();
        }
    }
}
