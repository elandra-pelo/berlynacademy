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
    public partial class frmStudentStats : Form
    {
        public string statlog,VISITED,activeSY,snum;
        public DataView dvv;
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public frmStudentStats()
        {
            InitializeComponent();
        }

        private void frmStudentStats_Load(object sender, EventArgs e)
        {
            lblLogger.Text = statlog;
            lblLoggerPosition.Text = "Admin";
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            btnStudStat.BackColor = Color.LightGreen;
            cmbFilter.Text = "Student number";
            if (VISITED.Contains("Student status") == false)
            {
                VISITED += "   Student status";
              
            }
            setupSYList();
            setupLevelList();
            setupAllStudents(); 
        }

        public void setupSYList()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from schoolyear_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                cmbSY.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[2].ToString()=="Active")
                    {
                        activeSY = dt.Rows[i].ItemArray[1].ToString();
                        lblActiveSY.Text = activeSY;
                    }

                    cmbSY.Items.Add(dt.Rows[i].ItemArray[1].ToString());
                }
            }
            cmbSY.Text = activeSY;
        }

        public void setupLevelList()
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select level from level_tbl", con);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbLev.Items.Clear();
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbLev.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                }
            }
        }
        

        public void setupAllStudents()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'Student_no',lname as 'Lastname',fname as 'Firstname',mname as 'Middlename',status as 'Status'from stud_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dvv = new DataView(dt);
            dgvStud.DataSource = dvv;
            con.Close();

            dgvStud.Columns[0].Width = 170;
            dgvStud.Columns[1].Width = 185;
            dgvStud.Columns[2].Width = 185;
            dgvStud.Columns[3].Width = 185;
            dgvStud.Columns[4].Width = 170;
           
            dgvStud.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
           
        }

        private void btnStudStat_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnEdays_Click(object sender, EventArgs e)
        {
            frmEnrollmentDays eform = new frmEnrollmentDays();
            this.Hide();
            eform.edlog = statlog;
            eform.VISITED = VISITED;
            eform.Show();
        }

        private void btncoa_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Hide();
            actform.actlog = statlog;
            actform.VISITED = VISITED;
            actform.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abm = new frmAboutMaintenance();
            this.Hide();
            abm.amlog = statlog;
            abm.VISITED = VISITED;
            abm.Show();
        }

        private void btnHomeMainte_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = statlog;
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

        private void frmStudentStats_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            con.Open();
            string update = "Update stud_tbl set status='" + "Active" + "'where studno='" + snum + "'";
            OdbcCommand cmd = new OdbcCommand(update, con);
            cmd.ExecuteNonQuery();
            con.Close();
            setupAllStudents();
            MessageBox.Show("Student successfully activated.", "Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            con.Open();
            string update = "Update stud_tbl set status='" + "Inactive" + "'where studno='" + snum + "'";
            OdbcCommand cmd = new OdbcCommand(update, con);
            cmd.ExecuteNonQuery();
            con.Close();
            setupAllStudents();
            MessageBox.Show("Student successfully Deactivated.", "Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            
            btnActive.Enabled = false; btnInactive.Enabled = false;

            if (cmbFilter.Text == "Student number")
            {
                toolTip1.SetToolTip(txtSearch,"search student number");
                dvv.RowFilter = string.Format("Student_no LIKE '%{0}%'", txtSearch.Text);
                dgvStud.DataSource = dvv;
            }
            if (cmbFilter.Text == "Student's lastname")
            {
                toolTip1.SetToolTip(txtSearch, "search student lastname");
                dvv.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
                dgvStud.DataSource = dvv;
            }


            if (dgvStud.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
            }
            if (dgvStud.Rows.Count == 0 && txtSearch.Text != "")
            {
                pnlnotify.Visible = true;
                lblnote.Text = "0 search result";
            }
            if (dgvStud.Rows.Count == 0 && txtSearch.Text == "")
            {
                pnlnotify.Visible = true;
                lblnote.Text = "no items found!";
            }
           
        }

        private void cmbSY_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupLevelList();
            cmbSec.Items.Clear();
            cmbLev.SelectedIndex = -1;
            cmbGen.SelectedIndex = -1;
            cmbStat.SelectedIndex = -1;

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'Student_no',lname as 'Lastname',fname as 'Firstname',mname as 'Middlename',status as 'Status'from stud_tbl where syenrolled='" + cmbSY.Text + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvv = new DataView(dt);
            dgvStud.DataSource = dvv;

            
            btnActive.Enabled = false; btnInactive.Enabled = false;
            //dv.RowFilter = string.Format("SY_Enrolled LIKE '%{0}%'",cmbSY.Text);
        }

        private void cmbLev_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGen.Text != "" && cmbStat.Text != "")
            {
                setupSectionOfLevel(cmbLev.Text);
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'Student_no',lname as 'Lastname',fname as 'Firstname',mname as 'Middlename',status as 'Status'from stud_tbl where level='" + cmbLev.Text + "'and syenrolled='" + cmbSY.Text + "'and gender='"+cmbGen.Text+"'and status='"+cmbStat.Text+"'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                dvv = new DataView(dt);
                dgvStud.DataSource = dvv;
            }
            else
            {
                setupSectionOfLevel(cmbLev.Text);
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'Student_no',lname as 'Lastname',fname as 'Firstname',mname as 'Middlename',status as 'Status'from stud_tbl where level='" + cmbLev.Text + "'and syenrolled='" + cmbSY.Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                dvv = new DataView(dt);
                dgvStud.DataSource = dvv;
            }

            
            btnActive.Enabled = false; btnInactive.Enabled = false;
        }

        public void setupSectionOfLevel(string lev)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select section from section_tbl where level='" + lev + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                cmbSec.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSec.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }


        private void cmbSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGen.Text != "" && cmbStat.Text != "")
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'Student_no',lname as 'Lastname',fname as 'Firstname',mname as 'Middlename',status as 'Status'from stud_tbl where syenrolled='" + cmbSY.Text + "'and level='" + cmbLev.Text + "'and section='" + cmbSec.Text + "'and gender='"+cmbGen.Text+"'and status='"+cmbStat.Text+"'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                dvv = new DataView(dt);
                dgvStud.DataSource = dvv;
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'Student_no',lname as 'Lastname',fname as 'Firstname',mname as 'Middlename',status as 'Status'from stud_tbl where syenrolled='" + cmbSY.Text + "'and level='" + cmbLev.Text + "'and section='" + cmbSec.Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                dvv = new DataView(dt);
                dgvStud.DataSource = dvv;
            }

            
            btnActive.Enabled = false; btnInactive.Enabled = false;
        }

        private void dgvStud_Click(object sender, EventArgs e)
        {
            string selectedstatus = "";
            if (dgvStud.Rows.Count>0)
            {
                snum = dgvStud.SelectedRows[0].Cells[0].Value.ToString();
                selectedstatus = dgvStud.SelectedRows[0].Cells[4].Value.ToString();
            }
            

            if (selectedstatus == "Active")
            {
                btnInactive.Enabled = true;
                btnActive.Enabled = false;
            }
            else
            {
                btnActive.Enabled = true;
                btnInactive.Enabled = false;
            }
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvStud_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dgvStud.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
            }
            else
            {
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }

            lblResult.Text = "result: " + dgvStud.Rows.Count;
        }

        private void btnPriority_Click(object sender, EventArgs e)
        {
            frmPrioritySec priorsec = new frmPrioritySec();
            this.Dispose();
            priorsec.priorlog = statlog;
            priorsec.VISITED = VISITED;
            priorsec.Show();
        }

        private void btnAssRoom_Click(object sender, EventArgs e)
        {
            frmAssignRoom asrom = new frmAssignRoom();
            this.Dispose();
            asrom.asromlog = statlog;
            asrom.VISITED = VISITED;
            asrom.Show();
        }

        private void cmbGen_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'Student_no',lname as 'Lastname',fname as 'Firstname',mname as 'Middlename',status as 'Status'from stud_tbl where syenrolled='" + cmbSY.Text + "'and level='" + cmbLev.Text + "'and section='" + cmbSec.Text + "'and gender='"+cmbGen.Text+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvv = new DataView(dt);
            dgvStud.DataSource = dvv;
            btnActive.Enabled = false; btnInactive.Enabled = false;
        }

        private void cmbStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'Student_no',lname as 'Lastname',fname as 'Firstname',mname as 'Middlename',status as 'Status'from stud_tbl where syenrolled='" + cmbSY.Text + "'and level='" + cmbLev.Text + "'and section='" + cmbSec.Text + "'and gender='" + cmbGen.Text + "'and status='"+cmbStat.Text+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvv = new DataView(dt);
            dgvStud.DataSource = dvv;
            btnActive.Enabled = false; btnInactive.Enabled = false;
        }
    }
}
