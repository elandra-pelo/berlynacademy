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
    public partial class frmRequirement : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string reqlog,primarykey,VISITED,selectedReq;
        public DataView dvReq;
        public frmRequirement()
        {
            InitializeComponent();
        }

        private void frmRequirement_Load(object sender, EventArgs e)
        {
            //pnlType.BackColor = Color.FromArgb(0, 0, 25); ;

            //this.BackColor = Color.FromArgb(49, 79, 142);
            lblLogger.Text = reqlog;
            btnReq.BackColor = Color.LightGreen;
            //btnHome.Text = "          " + reqlog;
            pnlnotify.Visible = false;
           
            if (VISITED.Contains("Requirement") == false)
            {
                VISITED += "   Requirement";
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin homeform = new frmEmpLogin();
            this.Hide();
            homeform.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance def = new frmMaintenance();
            this.Hide();
            def.adminlog = reqlog;
            def.VISITED = VISITED;
            def.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subform = new frmSubject();
            this.Hide();
            subform.wholog = reqlog;
            subform.VISITED = VISITED;
            subform.Show();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection secform = new frmSection();
            this.Hide();
            secform.secwholog = reqlog;
            secform.VISITED = VISITED;
            secform.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roomform = new frmRoom();
            this.Hide();
            roomform.logger = reqlog;
            roomform.VISITED = VISITED;
            roomform.Show();
        }

        private void btnAudittrail_Click(object sender, EventArgs e)
        {
            frmAudit auditform = new frmAudit();
            this.Hide();
            auditform.auditlogger = reqlog;
            auditform.Show();
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abtmain = new frmAboutMaintenance();
            this.Hide();
            abtmain.amlog = reqlog;
            abtmain.Show();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Clear")
            {
                setup_CLEAR();
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

                //primarykey = lblKey.Text;
                setup_RETRIEVEDATA(primarykey);
                setup_DISABLEINPUT();
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
        }

        public void setup_DISABLEINPUT()
        {
            txtReq.Enabled = false;
           
        }

        public void setup_RETRIEVEDATA(string thekey)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from requirement_tbl where id='" + thekey + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                txtReq.Text = dt.Rows[0].ItemArray[1].ToString();
                btnAdd.Enabled = false;
            }
        }

        public void setup_CLEAR()
        {
            txtReq.Clear();
            lblKey.Text = "";
            txtSearch.Focus();
            setup_ENABLEINPUT();
        }

        public void setup_ENABLEINPUT()
        {
            txtReq.Enabled = true;
        
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete?", "Requirement maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtReq.Text == "")
                {
                    MessageBox.Show("fill out field.", "Requirement Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    setup_DELETE();
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                    setup_CLEAR();
                    setup_ENABLEINPUT();
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

        public void setup_DELETE()
        {
            con.Open();
            string deleteReq = "Delete from requirement_tbl where id='" + primarykey + "'";
            OdbcCommand cmdDeleteReq = new OdbcCommand(deleteReq, con);
            cmdDeleteReq.ExecuteNonQuery();
            con.Close();

            btnAdd.Enabled = false;
            setup_VIEW();
            MessageBox.Show("requirement successfully deleted", "Requirement maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();

        }

        public void setup_VIEW()
        {
            string type = "";
            if (cmbType.Text == "New/Transferee")
            {
                type = "NTR";
            }
            else
            {
                type = "OLD";
            }

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select name as 'Requirement'from requirement_tbl where type='"+type+"' order by name ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvReq = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvReq;
                dgvSearch.Columns[0].Width = 475;
               
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }

            lblResult.Text = "number of subject: " + dgvSearch.Rows.Count.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Update")
            {
                setup_ENABLEINPUT();
                btnUpdate.Text = "Save";
                btnDelete.Enabled = false;
                btnClear.Text = "Cancel";
            }
            else
            {
                if (txtReq.Text == "")
                {
                    MessageBox.Show("fill out field.", "Requirement Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    string type = "";
                    if (cmbType.Text == "New/Transferee")
                    {
                        type = "NTR";
                    }
                    else
                    {
                        type = "OLD";
                    }
               
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from requirement_tbl where name LIKE'" + txtReq.Text + "'and type='" + type + "'and name<>'"+selectedReq+"'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Requirement already exists.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    setup_SAVE();
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

        public void setup_SAVE()
        {
            con.Open();
            string updateReq = "Update requirement_tbl set name='" + txtReq.Text + "'where id='" + primarykey + "'";
            OdbcCommand cmdUpdateReq = new OdbcCommand(updateReq, con);
            cmdUpdateReq.ExecuteNonQuery();
            con.Close();

            btnAdd.Enabled = false;
            setup_VIEW();
            btnClear.Text = "Clear";
            MessageBox.Show("requirement successfully updated", "Requirement maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtReq.Text == "")
            {
                MessageBox.Show("fill out field.", "Requirement Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
               
                string type = "";
                if (cmbType.Text == "New/Transferee")
                {
                    type = "NTR";
                }
                else
                {
                    type = "OLD";
                }
               
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from requirement_tbl where name LIKE'" + txtReq.Text + "'and type='"+type+"'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Requirement already added.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                setup_ADD();
            }
        }

        public void setup_ADD()
        {
            string type="";
            if (cmbType.Text == "New/Transferee")
            {
                type = "NTR";
            }
            else
            {
                type = "OLD";
            }

            con.Open();
            string addReq = "Insert Into requirement_tbl(name,type)values('" + txtReq.Text + "','"+type+"')";
            OdbcCommand cmdAddReq = new OdbcCommand(addReq, con);
            cmdAddReq.ExecuteNonQuery();
            con.Close();

            btnAdd.Enabled = false;
            setup_VIEW();
            MessageBox.Show("requirement successfully added", "Requirement maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvReq.RowFilter = string.Format("Requirement LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvReq;
            toolTip1.SetToolTip(txtSearch, "search requirement");

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

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            //primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();

            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }

            string reqname = "";
            string type = "";

            if (cmbType.Text == "New/Transferee")
            {
                type = "NTR";
            }
            else
            {
                type = "OLD";
            }
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString()!="")
            {
                reqname = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from requirement_tbl where name='" + reqname + "'and type='" + type+ "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0) { primarykey = dt.Rows[0].ItemArray[0].ToString();
            selectedReq = dt.Rows[0].ItemArray[1].ToString();
            }

            setup_DISABLEINPUT();
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            setup_RETRIEVEDATA(primarykey);  
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched sf = new frmSched();
            this.Hide();
            sf.VISITED = VISITED;
            sf.schedlog = reqlog;
            sf.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feeform = new frmFee();
            this.Hide();
            feeform.feelog = reqlog;
            feeform.VISITED = VISITED;
            feeform.Show();
        }

        private void btnActivity_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Hide();
            actform.actlog = reqlog;
            actform.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Hide();
            discform.disclog = reqlog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Hide();
            buf.backlog = reqlog;
            buf.Show();
        }

        private void frmRequirement_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtReq.Text = "";
            btnUpdate.Text = "Update"; btnClear.Text = "Clear";
            btnAdd.Enabled = true; btnUpdate.Enabled = false; btnDelete.Enabled = false;
            setup_VIEW();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = reqlog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Hide();
            levmain.levlog = reqlog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = reqlog;
            facmain.VISITED = VISITED;
            facmain.Show();
            this.Hide();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = reqlog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = reqlog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = reqlog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = reqlog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }
    }
}
