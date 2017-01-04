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
    public partial class frmDepartment : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public DataView dv;
        public string primarykey,activeSY,activeYr,deplog, VISITED,orgDep;
        public frmDepartment()
        {
            InitializeComponent();
        }

        private void frmDepartment_Load(object sender, EventArgs e)
        {
            lblLogger.Text = deplog;
            lblLoggerPosition.Text = "Admin";
            btnDept.BackColor = Color.LightGreen;
            setupDepartment();
            GetActiveSchoolYear();
            lblcount.Text = "no. of department: " + (dgvSearch.Rows.Count).ToString();

            if (VISITED.Contains("Department") == false)
            {
                VISITED += "   Department";
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

        public void setupDepartment()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select deptname as 'Department' from department_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dv = new DataView(dt);
            dgvSearch.DataSource = dv;
            con.Close();

            dgvSearch.Columns[0].Width = 407;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtDept.Text == "")
            {
                MessageBox.Show("fill out field.", "Department maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from department_tbl where deptname='" +txtDept.Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("department already added.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                setupAdd();
            }
        }

        public void setupAdd()
        {
            con.Open();
            string add = "Insert Into department_tbl(deptname)values('" + txtDept.Text + "')";
            OdbcCommand cmd = new OdbcCommand(add, con);
            cmd.ExecuteNonQuery();
            con.Close();

            setupDepartment();
            MessageBox.Show("department successfully added.", "Department maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnAdd.Enabled = false;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Update")
            {
                txtDept.Enabled = true;
                btnUpdate.Text = "Save";
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from department_tbl where deptname='" + txtDept.Text + "' and deptname<>'"+orgDep+"'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("department already exist.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                con.Open();
                string upd = "Update department_tbl set deptname='" + txtDept.Text + "'where id='" + primarykey + "'";
                OdbcCommand cmd = new OdbcCommand(upd, con);
                cmd.ExecuteNonQuery();

                string upd1 = "Update fee_tbl set level='" + txtDept.Text + "'where level='" + orgDep + "'and SY='"+activeSY+"'";
                OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                cmd1.ExecuteNonQuery();

                string upd2 = "Update miscellaneousfee_tbl set level='" + txtDept.Text + "'where level='" + orgDep + "'and SY='" + activeSY + "'";
                OdbcCommand cmd2 = new OdbcCommand(upd2, con);
                cmd2.ExecuteNonQuery();

                string upd3 = "Update registrationfee_tbl set level='" + txtDept.Text + "'where level='" + orgDep + "'and SY='" + activeSY + "'";
                OdbcCommand cmd3 = new OdbcCommand(upd3, con);
                cmd3.ExecuteNonQuery();

                string upd4 = "Update level_tbl set department='" + txtDept.Text + "'where department='" + orgDep + "'";
                OdbcCommand cmd4 = new OdbcCommand(upd4, con);
                cmd4.ExecuteNonQuery();

                string upd5 = "Update discount_tbl set level='" + txtDept.Text + "'where level='" + orgDep + "'";
                OdbcCommand cmd5 = new OdbcCommand(upd5, con);
                cmd5.ExecuteNonQuery();
                
                con.Close();
                setupDepartment();
                btnUpdate.Text = "Update";
                MessageBox.Show("department successfully updated.", "Department maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnUpdate.Enabled = false;
            }
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            txtDept.Text = "";
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            txtDept.Enabled = true;
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            txtDept.Enabled = false;
            txtDept.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;

            string dep = "";
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
            {
                dep = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                orgDep = dep;
            }

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id from department_tbl where deptname='" + dep + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                primarykey = dt.Rows[0].ItemArray[0].ToString();
            }
        }

        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            lblcount.Text = "no. of department: " + (dgvSearch.Rows.Count).ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = string.Format("Department LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dv;
            toolTip1.SetToolTip(txtSearch, "search department");

            if (dgvSearch.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text != "")
            {
                pnlnotify.Visible = true;
                lblnote.Text = "0 search result";
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text == "")
            {
                pnlnotify.Visible = true;
                lblnote.Text = "no items found!";
            }
        }

        private void frmDepartment_FormClosing(object sender, FormClosingEventArgs e)
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
            string setOut = "Update audittrail_tbl set logout='" + time + "',visited='" + VISITED + "'Where logout='" + def + "'";
            OdbcCommand cmd = new OdbcCommand(setOut, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = deplog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Hide();
            discform.disclog = deplog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feef = new frmFee();
            this.Hide();
            feef.feelog = deplog;
            feef.VISITED = VISITED;
            feef.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqf = new frmRequirement();
            this.Hide();
            reqf.reqlog = deplog;
            reqf.VISITED = VISITED;
            reqf.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched schedf = new frmSched();
            this.Hide();
            schedf.schedlog = deplog;
            schedf.VISITED = VISITED;
            schedf.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = deplog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = deplog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = deplog;
            facmain.VISITED = VISITED;
            facmain.Show();
            this.Hide();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roommaintenance = new frmRoom();
            roommaintenance.logger = deplog;
            roommaintenance.VISITED = VISITED;
            roommaintenance.Show();
            this.Hide();
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            frmSection section = new frmSection();
            section.secwholog = deplog;
            section.VISITED = VISITED;
            section.Show();
            this.Hide();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Dispose();
            levmain.levlog = deplog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subjmaintenance = new frmSubject();
            subjmaintenance.wholog = deplog;
            subjmaintenance.Show();
            subjmaintenance.VISITED = VISITED;
            this.Hide();
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = deplog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance user = new frmMaintenance();
            user.adminlog = deplog;
            user.VISITED = VISITED;
            user.Show();
            this.Hide();
        }
    }
}
