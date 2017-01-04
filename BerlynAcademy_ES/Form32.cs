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
    public partial class frmLevel : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public DataView dv;
        public string levlog,primarykey,orglevel,VISITED;
        public frmLevel()
        {
            InitializeComponent();
        }

        private void frmLevel_Load(object sender, EventArgs e)
        {
            lblLogger.Text = levlog;
            lblLoggerPosition.Text = "Admin";

            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            btnLevel.BackColor = Color.LightGreen;
            setupDept();
            lblcount.Text = "no. of grade level: " + (dgvSearch.Rows.Count).ToString();

            if (VISITED.Contains("Grade level") == false)
            {
                VISITED += "   Grade level";
            }
        }

        public void setupLevels()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select level as 'Grade_level' from level_tbl where department='"+cmbDept.Text+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dv = new DataView(dt);
            dgvSearch.DataSource = dv;
            con.Close();

            dgvSearch.Columns[0].Width = 407;
          
        }

        public void setupDept()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select deptname from department_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbDept.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbDept.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subjmaintenance = new frmSubject();
            subjmaintenance.wholog = levlog;
            subjmaintenance.VISITED = VISITED;
            subjmaintenance.Show();
            
            this.Hide();
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = levlog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance user = new frmMaintenance();
            user.adminlog = levlog;
            user.VISITED = VISITED;
            user.Show();
            this.Hide();
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

        private void frmLevel_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roommaintenance = new frmRoom();
            roommaintenance.logger = levlog;
            roommaintenance.VISITED = VISITED;
            roommaintenance.Show();
            this.Hide();
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            frmSection section = new frmSection();
            section.secwholog = levlog;
            section.VISITED = VISITED;
            section.Show();
            this.Hide();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = levlog;
            facmain.VISITED = VISITED;
            facmain.Show();
            this.Hide();
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = levlog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched schedf = new frmSched();
            this.Hide();
            schedf.schedlog = levlog;
            schedf.VISITED = VISITED;
            schedf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqf = new frmRequirement();
            this.Hide();
            reqf.reqlog = levlog;
            reqf.VISITED = VISITED;
            reqf.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feef = new frmFee();
            this.Hide();
            feef.feelog = levlog;
            feef.VISITED = VISITED;
            feef.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Hide();
            discform.disclog = levlog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = string.Format("Grade_level LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dv;
            toolTip1.SetToolTip(txtSearch, "search grade level");

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtLev.Text == "" || cmbDept.Text == "")
            {
                MessageBox.Show("fill out field.", "Level maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from level_tbl where level='" + txtLev.Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("grade level already added.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                setupAdd();
            }
        }

        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            lblcount.Text = "no. of grade level: " + (dgvSearch.Rows.Count).ToString();
        }

        public void setupAdd()
        {
            con.Open();
            string add = "Insert Into level_tbl(level,department)values('" + txtLev.Text + "','"+cmbDept.Text+"')";
            OdbcCommand cmd = new OdbcCommand(add, con);
            cmd.ExecuteNonQuery();
            con.Close();

            string[] deffee = new string[3] { "TUITION FEE", "REGISTRATION", "MISCELLANEOUS" };
            string[] defamt = new string[3] { "0.00", "1,000.00", "2,280.00" };

            for (int x = 0; x < deffee.Length; x++)
            {
                con.Open();
                string addfee = "Insert Into fee_tbl(fee,amount,level,type)values('" + deffee[x].ToString() + "','" + defamt[x] + "','" + txtLev.Text + "','" + "fee" + "')";
                OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                cmdAddfee.ExecuteNonQuery();
                con.Close();
            }

            string[] defpayment = new string[3] { "ANNUAL PAYMENT", "UPON ENROLLMENT", "MONTHLY INSTALLMENT" };
            string[] defamtpayment = new string[3] { "3,280.00", "0.00", "0.00" };

            for (int x = 0; x < defpayment.Length; x++)
            {
                con.Open();
                string addfee = "Insert Into fee_tbl(fee,amount,level,type)values('" + defpayment[x].ToString() + "','" + defamtpayment[x] + "','" + txtLev.Text + "','" + "payment" + "')";
                OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                cmdAddfee.ExecuteNonQuery();
                con.Close();
            }

            string[] defregfee = new string[3] { "ENROLLMENT FORM", "OFFICE SUPLIES", "PERIODICALS / SUBSCRIPTIONS" };
            string[] defregamt = new string[3] { "450.00", "250.00", "300.00" };

            for (int x = 0; x < defregfee.Length; x++)
            {
                con.Open();
                string addfee = "Insert Into registrationfee_tbl(fee,amount,level)values('" + defregfee[x].ToString() + "','" + defregamt[x] + "','" + txtLev.Text + "')";
                OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                cmdAddfee.ExecuteNonQuery();
                con.Close();
            }


            string[] defmisfee = new string[9] {"MEDICAL & DENTAL", "GUIDANCE AND COUNSELING", "LABORATORY", "LIBRARY", "ATHLETIC", "COMPUTER", "TESTING MATERIALS", "MAINTENANCE", "ID / HANDBOOK" };
            string[] defmisamt = new string[9] { "400.00", "200.00", "300.00", "200.00", "100.00", "330.00", "200.00", "400.00", "150.00" };

            for (int x = 0; x < defmisfee.Length; x++)
            {
                con.Open();
                string addfee = "Insert Into miscellaneousfee_tbl(fee,amount,level)values('" + defmisfee[x].ToString() + "','" + defmisamt[x] + "','" + txtLev.Text + "')";
                OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                cmdAddfee.ExecuteNonQuery();
                con.Close();
            }

             
            setupLevels();
            MessageBox.Show("level successfully added.", "Level maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnAdd.Enabled = false;
            
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            txtLev.Text = "";
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            txtLev.Enabled = true;
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            txtLev.Enabled = false;
            txtLev.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            orglevel = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;

            string lev = "";
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
            {
                lev = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();

            }

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id from level_tbl where level='"+lev+"'",con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                primarykey = dt.Rows[0].ItemArray[0].ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Update")
            {
                txtLev.Enabled = true;
                btnUpdate.Text = "Save";
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from level_tbl where level='" + txtLev.Text + "' and level<>'"+orglevel+"'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("grade level already exists.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                con.Open();
                string upd = "Update level_tbl set level='" + txtLev.Text + "'where id='" + primarykey + "'";
                OdbcCommand cmd = new OdbcCommand(upd, con);
                cmd.ExecuteNonQuery();

                string upd1 = "Update fee_tbl set level='" + txtLev.Text + "'where level LIKE'" +orglevel + "'";
                OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                cmd1.ExecuteNonQuery();

                string upd2 = "Update registrationfee_tbl set level='" + txtLev.Text + "'where level LIKE'" + orglevel + "'";
                OdbcCommand cmd2 = new OdbcCommand(upd2, con);
                cmd2.ExecuteNonQuery();

                string upd3 = "Update miscellaneousfee_tbl set level='" + txtLev.Text + "'where level LIKE'" + orglevel + "'";
                OdbcCommand cmd3 = new OdbcCommand(upd3, con);
                cmd3.ExecuteNonQuery();


                con.Close();
                setupLevels();
                btnUpdate.Text = "Update";
                MessageBox.Show("level successfully updated.", "Level maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnUpdate.Enabled = false;
            }
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = levlog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = levlog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = levlog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void pnlAddSection_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtLev.Text = "";
            setupLevels();
        }
    }
}
