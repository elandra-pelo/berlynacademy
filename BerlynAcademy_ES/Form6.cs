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
    public partial class frmSubject : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string wholog,primarykey,sub,VISITED;
        public DataView dvSubject;
        public frmSubject()
        {
            InitializeComponent();
        }

        private void frmSubject_Load(object sender, EventArgs e)
        {
            lblLogger.Text = wholog;
            lblLoggerPosition.Text = "Admin";
            //btnHome.Text = "          "+wholog;
            btnSubject.BackColor = Color.LightGreen;
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //this.BackColor = Color.FromArgb(49, 79, 142);
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            pnlnotify.Visible = false;
            setupDept();
           
            if (VISITED.Contains("Subject") == false)
            {
                VISITED += "   Subject";
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

       
        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                pnlnotify.Visible = true;
            }
            txtSub.Clear();
            txtDesc.Clear();
            txtUnit.Clear();
            setup_VIEW(cmbLevel.Text);
          
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

                primarykey = lblKey.Text;
                setup_RETRIEVEDATA(primarykey);
                setup_DISABLEINPUT();
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
        }

        public void setup_CLEAR()
        {
            txtSub.Clear();
            txtUnit.Clear();
            txtDesc.Clear();
            lblKey.Text = "";
            txtSearch.Focus();
            setup_ENABLEINPUT();
        }

        public void setup_ENABLEINPUT()
        {
            txtSub.Enabled = true;
            txtDesc.Enabled = true;
        }

        public void setup_DISABLEINPUT()
        {
            txtSub.Enabled = false;
            txtDesc.Enabled = false;
        }

        public void setup_RETRIEVEDATA(string thekey)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from subject_tbl where id='" + thekey + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                txtSub.Text = dt.Rows[0].ItemArray[1].ToString();
                txtUnit.Text = dt.Rows[0].ItemArray[2].ToString();
                
                btnAdd.Enabled = false;

                con.Open();
                OdbcDataAdapter daa = new OdbcDataAdapter("Select*from facultyspecialization_tbl where subject='" + txtSub.Text + "'", con);
                DataTable dtt = new DataTable();
                daa.Fill(dtt);
                con.Close();

                if (dtt.Rows.Count > 0)
                {
                    txtDesc.Text = dtt.Rows[0].ItemArray[2].ToString();
                }
                else
                {
                    txtDesc.Text = "";
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete?", "User maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtSub.Text == "" || cmbLevel.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
             string deleteSub = "Delete from subject_tbl where id='" + primarykey + "'";
             OdbcCommand cmdDeleteSub = new OdbcCommand(deleteSub, con);
             cmdDeleteSub.ExecuteNonQuery();
             con.Close();

             btnAdd.Enabled = false;
             setup_VIEW(cmbLevel.Text);
             MessageBox.Show("subject successfully deleted", "Subject maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
             txtSearch.Focus();
         
        }

        public void setup_VIEW(string level)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select subject as 'Subject' from subject_tbl where level='" + level + "' order by subject ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvSubject = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvSubject;

               
                //dgvSearch.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[0].Width = 407;
           
                //dgvSearch.Columns[2].Width = 80;
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
                if (txtSub.Text == "" || cmbLevel.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
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
            string updateSub = "Update subject_tbl set subject='" + txtSub.Text + "',unit='" + txtUnit.Text + "',level='"+cmbLevel.Text+"'where id='" + primarykey+ "'";
            OdbcCommand cmdUpdateSub = new OdbcCommand(updateSub, con);
            cmdUpdateSub.ExecuteNonQuery();

            string updateSub2 = "Update facultyspecialization_tbl set description='" + txtDesc.Text + "'where subject='" + sub + "'";
            OdbcCommand cmdUpdateSub2 = new OdbcCommand(updateSub2, con);
            cmdUpdateSub2.ExecuteNonQuery();
            con.Close();

            btnAdd.Enabled = false;
            setup_VIEW(cmbLevel.Text);
            btnClear.Text = "Clear";
            MessageBox.Show("subject successfully updated", "Subject maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtSub.Text == "" || cmbLevel.Text=="")
            {
                MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                setup_ADD();
            }
        }

        public void setup_ADD()
        {
            con.Open();
            string addSub = "Insert Into subject_tbl(subject,unit,level)values('" + txtSub.Text + "','" +txtUnit.Text + "','" + cmbLevel.Text + "')";
            OdbcCommand cmdAddSub = new OdbcCommand(addSub, con);
            cmdAddSub.ExecuteNonQuery();
            con.Close();
            //-----
            char newID = 'a';
            con.Open();
            OdbcDataAdapter daa = new OdbcDataAdapter("Select max(id) from facultyspecialization_tbl", con);
            DataTable dtt = new DataTable();
            daa.Fill(dtt);
            con.Close();
            if (dtt.Rows.Count > 0)
            {
                newID = Convert.ToChar(dtt.Rows[0].ItemArray[0].ToString());
                newID++;
            }
            //-----
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select subject from facultyspecialization_tbl where subject LIKE'"+txtSub.Text+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
            }
            else
            {
                con.Open();
                string addS = "Insert Into facultyspecialization_tbl(id,subject,description)values('"+newID+"','" + txtSub.Text + "','" + txtDesc.Text + "')";
                OdbcCommand cmdAddS = new OdbcCommand(addS, con);
                cmdAddS.ExecuteNonQuery();
                con.Close();
            }

            btnAdd.Enabled = false;
            setup_VIEW(cmbLevel.Text);
            MessageBox.Show("subject successfully added", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
  
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            //primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();

            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }
            setup_DISABLEINPUT();
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            sub = "";
            string lev = cmbLevel.Text;
           
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
            {
                sub = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id from subject_tbl where subject='" + sub + "'and level='" + lev + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                primarykey = dt.Rows[0].ItemArray[0].ToString();
            }

            setup_RETRIEVEDATA(primarykey);  
        }

        private void txtUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch!=8 && ch!=46)
            {
                e.Handled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            /*if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }*/
         
            dvSubject.RowFilter = string.Format("Subject LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvSubject;
            toolTip1.SetToolTip(txtSearch, "search subject");

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

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maintenance = new frmMaintenance();
            this.Dispose();
            maintenance.adminlog = wholog;
            maintenance.VISITED = VISITED;
            maintenance.Show();
           
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection section = new frmSection();
            this.Dispose();
            section.secwholog = wholog;
            section.VISITED = VISITED;
            section.Show();
 
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Dispose();
            home.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roommaintenance = new frmRoom();
            this.Dispose();
            roommaintenance.logger = wholog;
            roommaintenance.VISITED = VISITED;
            roommaintenance.Show();
            
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance am = new frmAboutMaintenance();
            this.Dispose();
            am.amlog = wholog;
            am.Show();
         
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched sf = new frmSched();
            this.Dispose();
            sf.schedlog = wholog;
            sf.VISITED = VISITED;
            sf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqform = new frmRequirement();
            this.Dispose();
            reqform.reqlog = wholog;
            reqform.VISITED = VISITED;
            reqform.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feeform = new frmFee();
            this.Dispose();
            feeform.feelog = wholog;
            feeform.VISITED = VISITED;
            feeform.Show();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Dispose();
            actform.actlog = wholog;
            actform.Show();
        }

        private void btnAud_Click(object sender, EventArgs e)
        {
            frmAudit audform = new frmAudit();
            this.Dispose();
            audform.auditlogger = wholog;
            audform.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Dispose();
            discform.disclog = wholog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Dispose();
            buf.backlog = wholog;
            buf.Show();
        }

        private void frmSubject_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf= new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
          
        }

        private void btnSubject_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            this.Dispose();
            symaintenance.sylog = wholog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Dispose();
            levmain.levlog = wholog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            this.Dispose();
            facmain.facmlog = wholog;
            facmain.VISITED = VISITED;
            facmain.Show();
           
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            this.Dispose();
            hm.adminlog = wholog;
            hm.VISITED = VISITED;
            hm.Show();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = wholog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = wholog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = wholog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSub.Text = "";
            txtDesc.Text = "";
            setupLevelList(cmbDept.Text);
            setup_VIEW("");
            pnlnotify.Visible = false;
          
        }

       
    }
}
