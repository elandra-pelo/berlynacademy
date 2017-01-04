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
    public partial class frmDiscount : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public DataView dvDisc;
        public string disclog,primarykey,VISITED,selectedDisc;
        public frmDiscount()
        {
            InitializeComponent();
        }

        private void frmDiscount_Load(object sender, EventArgs e)
        {
            lblLogger.Text = disclog;
            lblLoggerPosition.Text = "Admin";
            //btnHome.Text = "          " + disclog;
           // pnlType.BackColor = Color.FromArgb(0, 0, 25);
           // this.BackColor = Color.FromArgb(49, 79, 142);
            btnDiscount.BackColor = Color.LightGreen;
            pnlnotify.Visible = false;
            lblKey.Text = "";
            //setupdiscitem();
            setupDept();
            cmbLevel.Text = "Kinder";
           
            if (VISITED.Contains("Discount") == false)
            {
                VISITED += "   Discount";
            }

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
                cmbDept.Items.Add("All Department");
            }
           
        }

        public void setupLevelList(string dep)
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select level from level_tbl where department='"+dep+"'", con);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbLevel.Items.Clear();
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbLevel.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                }
                cmbLevel.Items.Add("All levels");
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

        public void setupdiscitem()
        {
            cmbDisc.Items.Clear();

            for (int x = 1; x < 101; x++)
            {
                cmbDisc.Items.Add(x);
            }
        }

        private void btnASClear_Click(object sender, EventArgs e)
        {
            if (btnASClear.Text == "Clear")
            {
                setup_CLEAR();
                btnASUpdate.Enabled = false;
                btnASDelete.Enabled = false;
                btnASAdd.Enabled = true;
                btnASUpdate.Text = "Update";
            }
            else
            {
                btnASDelete.Enabled = true;
                btnASClear.Text = "Clear";
                btnASUpdate.Text = "Update";

                //primarykey = lblKey.Text;
                setupretrieveddata(primarykey);
                setup_DISABLEINPUT();
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
        }

        public void setupretrieveddata(string thekey)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from discount_tbl where id='" + thekey + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                txtDisc.Text = dt.Rows[0].ItemArray[1].ToString();
                txtDesc.Text = dt.Rows[0].ItemArray[2].ToString();
                txtRate.Text=dt.Rows[0].ItemArray[3].ToString();
                btnASAdd.Enabled = false;

                if (txtRate.Text == "")
                {
                    btnASUpdate.Enabled = false;
                }
                else
                {
                    btnASUpdate.Enabled = true;
                }
            }
        }

        public void setup_CLEAR()
        {
            txtDisc.Clear();
            txtDesc.Clear();
            txtRate.Clear();
            cmbDisc.SelectedIndex = -1;
           
            lblKey.Text = "";
            setup_ENABLEINPUT();
            
        }

        public void setup_ENABLEINPUT()
        {
            txtDisc.Enabled = true;
            cmbDisc.Enabled = true;
            txtDesc.Enabled = true;
            txtRate.Enabled = true;
        }

        public void setup_DISABLEINPUT()
        {
            txtDisc.Enabled = false;
            cmbDisc.Enabled = false;
            txtDesc.Enabled = false;
            txtRate.Enabled = false;
        }

        private void btnASDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete?", "Discount maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtDesc.Text == "" || txtDisc.Text == ""|| txtRate.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    setupdeleteoperation();
                    btnASUpdate.Enabled = false;
                    btnASDelete.Enabled = false;
                    setup_CLEAR();
                    setup_ENABLEINPUT();
                    btnASAdd.Enabled = true;
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


        public void setupdeleteoperation()
        {
            con.Open();
            string deleteSec = "Delete from discount_tbl where id='" + primarykey + "'";
            OdbcCommand cmdDeleteSec = new OdbcCommand(deleteSec, con);
            cmdDeleteSec.ExecuteNonQuery();
            con.Close();

            btnASAdd.Enabled = false;
            setupview();
            MessageBox.Show("discount successfully deleted", "Discount maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        public void setupview()
        {
            //string lev = cmbLevel.Text;
           
            if (cmbDept.Text == "All Department")
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select discname as 'Discount',description as 'Description',rate as 'Rate' from discount_tbl where level='All'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                dvDisc = new DataView(dt);

                if (dt.Rows.Count > 0)
                {
                    pnlnotify.Visible = false;
                    dgvSearch.DataSource = null;
                    dgvSearch.DataSource = dvDisc;

                    dgvSearch.Columns[0].Width = 150;
                    dgvSearch.Columns[1].Width = 225;
                    dgvSearch.Columns[2].Width = 100;
                }
                else
                {
                    dgvSearch.DataSource = null;
                    pnlnotify.Visible = true;
                    lblmemowith.Text = "no items found...";
                }
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select discname as 'Discount',description as 'Description',rate as 'Rate' from discount_tbl where level='"+cmbDept.Text+"'or level='All'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                dvDisc = new DataView(dt);

                if (dt.Rows.Count > 0)
                {
                    pnlnotify.Visible = false;
                    dgvSearch.DataSource = null;
                    dgvSearch.DataSource = dvDisc;

                    dgvSearch.Columns[0].Width = 150;
                    dgvSearch.Columns[1].Width = 225;
                    dgvSearch.Columns[2].Width = 100;
                }
                else
                {
                    dgvSearch.DataSource = null;
                    pnlnotify.Visible = true;
                    lblmemowith.Text = "no items found...";
                }
            }

            lblResult.Text = "number of discount: " + dgvSearch.Rows.Count.ToString();
        }

        private void btnASUpdate_Click(object sender, EventArgs e)
        {
            if (btnASUpdate.Text == "Update")
            {
                setup_ENABLEINPUT();
                btnASUpdate.Text = "Save";
                btnASDelete.Enabled = false;
                btnASClear.Text = "Cancel";

                if (txtDisc.Text.Contains("First honor") == true || txtDisc.Text.Contains("Second honor") == true || txtDisc.Text.Contains("siblings") == true)
                {
                    txtRate.Enabled = false;
                }
                else
                {
                    txtRate.Enabled = true;
                }
            }
            else
            {
                if (txtDesc.Text == "" || txtDisc.Text == ""|| txtRate.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from discount_tbl where discname='" + txtDisc.Text + "'and discname<>'"+selectedDisc+"'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("discount already exists.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    setupsaveoperation();
                    btnASUpdate.Text = "Update";
                    btnASClear.Text = "Clear";
                    btnASUpdate.Enabled = false;
                    btnASDelete.Enabled = false;
                    if (dgvSearch.Rows.Count >= 1)
                    {
                        dgvSearch.Rows[0].Selected = true;
                    }
                }
            }
        }

        public void setupsaveoperation()
        {
            con.Open();
            string updateSec = "Update discount_tbl set discname='" + txtDisc.Text + "',description='" + txtDesc.Text + "',rate='"+txtRate.Text+"'where id='" + primarykey + "'";
            OdbcCommand cmdUpdateSec = new OdbcCommand(updateSec, con);
            cmdUpdateSec.ExecuteNonQuery();
            con.Close();

            btnASAdd.Enabled = false;
            setupview();
            btnASClear.Text = "Clear";
            MessageBox.Show("discount successfully updated", "Discount maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnASAdd_Click(object sender, EventArgs e)
        {
            if (txtDesc.Text == "" || txtDisc.Text == ""|| txtRate.Text=="")
            {
                MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from discount_tbl where discname='" + txtDisc.Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("discount already added.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                setup_Add();
            }
        }

        public void setup_Add()
        {
            string lev = "";
            if (cmbDept.Text == "All Department")
            {
                lev = "All";
            }
            else
            {
                lev = cmbDept.Text;
            }
           
            con.Open();
            string addDis = "Insert Into discount_tbl(discname,description,rate,level)values('" + txtDisc.Text + "','" + txtDesc.Text + "','"+txtRate.Text+"','"+lev+"')";

            OdbcCommand cmdAdd = new OdbcCommand(addDis, con);
            cmdAdd.ExecuteNonQuery();
            con.Close();

            btnASAdd.Enabled = false;
            setupview();
            MessageBox.Show("discount successfully added", "Discount maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvDisc.RowFilter = string.Format("Discount LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvDisc;
            if (dgvSearch.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text != "")
            {
                pnlnotify.Visible = true;
                lblmemowith.Text = "0 search result";
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text == "")
            {
                pnlnotify.Visible = true;
                lblmemowith.Text = "no items found!";
            }
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count > 0)
            {
                //primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }

            string dname = "";
            string ddesc = "";
            string drate = "";
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString()!="")
            {
                dname = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                selectedDisc = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }
            if (dgvSearch.SelectedRows[0].Cells[1].Value.ToString() != "")
            {
                ddesc = dgvSearch.SelectedRows[0].Cells[1].Value.ToString();
                
            }
            if (dgvSearch.SelectedRows[0].Cells[2].Value.ToString() != "")
            {
                drate = dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
            }

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id from discount_tbl where discname='" + dname + "'and description='" + ddesc + "'and rate='" + drate + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0) { primarykey = dt.Rows[0].ItemArray[0].ToString(); }

            setup_DISABLEINPUT();
            btnASUpdate.Enabled = true;
            btnASDelete.Enabled = true;

            setupretrieveddata(primarykey);
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maine = new frmMaintenance();
            this.Hide();
            maine.adminlog = disclog;
            maine.VISITED = VISITED;
            maine.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subf = new frmSubject();
            this.Hide();
            subf.wholog = disclog;
            subf.VISITED = VISITED;
            subf.Show();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection secf = new frmSection();
            this.Hide();
            secf.secwholog = disclog;
            secf.VISITED = VISITED;
            secf.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roomf = new frmRoom();
            this.Hide();
            roomf.logger = disclog;
            roomf.VISITED = VISITED;
            roomf.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched schedf = new frmSched();
            this.Hide();
            schedf.schedlog = disclog;
            schedf.VISITED = VISITED;
            schedf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqf = new frmRequirement();
            this.Hide();
            reqf.reqlog = disclog;
            reqf.VISITED = VISITED;
            reqf.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feef = new frmFee();
            this.Hide();
            feef.feelog = disclog;
            feef.VISITED = VISITED;
            feef.Show();
        }

        private void btnActivity_Click(object sender, EventArgs e)
        {
            frmActivity actf = new frmActivity();
            this.Hide();
            actf.actlog = disclog;
            actf.Show();
        }

        private void btnAudittrail_Click(object sender, EventArgs e)
        {
            frmAudit audf = new frmAudit();
            this.Hide();
            audf.auditlogger = disclog;
            audf.Show();
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abtf = new frmAboutMaintenance();
            this.Hide();
            abtf.amlog = disclog;
            abtf.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Hide();
            buf.backlog = disclog;
            buf.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin homef = new frmEmpLogin();
            this.Hide();
            homef.Show();
        }

        private void frmDiscount_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
          
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = disclog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Hide();
            levmain.levlog = disclog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = disclog;
            facmain.VISITED = VISITED;
            facmain.Show();
            this.Hide();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = disclog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 36)
            {
                e.Handled = true;
            }
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = disclog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog =disclog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = disclog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            setup_CLEAR();
            btnASUpdate.Enabled = false;
            btnASDelete.Enabled = false;
            btnASAdd.Enabled = true;
            btnASUpdate.Text = "Update";
            setup_ENABLEINPUT();
            setupview();
        }
    }
}
