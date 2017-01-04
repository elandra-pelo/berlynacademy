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
    public partial class frmSection : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public DataView dvAS,dvNoSec,dvPerLevel,dvAdv,dvAdvNoAdv;
        public string primarykey,secwholog,selectedsec,VISITED;
        public frmSection()
        {
            InitializeComponent();
        }

        private void frmSection_Load(object sender, EventArgs e)
        {
            lblLogger.Text = secwholog;
            lblLoggerPosition.Text = "Admin";
            //btnHome.Text = "          " + secwholog;
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //this.BackColor = Color.FromArgb(49, 79, 142);
            cmbOperation.Text = "Add section";
            setupDept();
            btnSection.BackColor = Color.LightGreen;
            pnlwith.Visible = false;
            pnlwithout.Visible = false;
           
            if (VISITED.Contains("Section") == false)
            {
                VISITED += "   Section";
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
                cmbLevelSetAdv.Items.Clear();
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbLevel.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                    cmbLevelSetAdv.Items.Add(dt.Rows[u].ItemArray[0].ToString());
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

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maintenance = new frmMaintenance();
            maintenance.adminlog = secwholog;
            maintenance.VISITED = VISITED;
            maintenance.Show();
            this.Hide();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subject = new frmSubject();
            subject.wholog = secwholog;
            subject.VISITED = VISITED;
            subject.Show();
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void cmbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            txtSearchWithoutAdv.Clear();
            txtSearchNoSec.Clear();

            if (cmbOperation.Text == "Add section")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    pnlwith.Visible = true;
                }

                pnlAddSection.Visible = true;
                pnlSetAdvisory.Visible = false;
                pnlSectioning.Visible = false;
                pnlAddSection.Location = new Point(6, 30);
                btnASUpdate.Enabled = false;
                btnASDelete.Enabled = false;
                btnASAdd.Enabled = true;
                setup_ENABLEINPUTforAS();
                setup_CLEARforAS();

                lblgenres.Visible = false;
                lblUserNo.Text = "Section ID:";
                lblKey.Text = "";
                toolTip1.SetToolTip(txtSearch, "search section");
            
                setupview_forAddSection(cmbLevel.Text);
            }
            if (cmbOperation.Text == "Student sectioning")
            {
                string sec = "";

                if (dgvSearch.Rows.Count <= 0)
                {
                    pnlwith.Visible = true;
                }

                if (cmbSecNoSec.Text == "")
                {
                    sec = "?";
                }
                else
                {
                    sec = cmbSecNoSec.Text;
                }


                pnlAddSection.Visible = false;
                pnlSetAdvisory.Visible = false;
                pnlSectioning.Visible = true;
                pnlSectioningFooter.Visible = true;
                pnlSectioningSearch.Visible = true;
                btnMovetowith.Visible = true;
                btnMovetowithout.Visible = true;
                btnCancel.Visible = true;
                pnlSectioning.Location = new Point(6, 35);

                setuplist_NoSectionforSEC("Kinder");
                setupview_perLevelforSEC("Kinder", "1");
                btnMovetowith.Enabled = false;
                btnMovetowithout.Enabled = false;
                btnCancel.Enabled = false;

                lblUserNo.Text = "Student no:";
           
                lblgenres.Visible = true;
                cmbLevelNoSec.Text = "Kinder";
                cmbSecNoSec.Text = "1";

                lblResult.Text = "no. of student in " + cmbLevelNoSec.Text + " section " + sec + ": " + dgvSearch.Rows.Count;

                lblUserNo.Text = "Student no:";
                lblKey.Text = "";
                toolTip1.SetToolTip(txtSearch, "search student");
            }
            if (cmbOperation.Text == "Set advisory class")
            {

                if (dgvSearch.Rows.Count<=0)
                {
                    pnlwith.Visible = true;
                }

                pnlAddSection.Visible = false;
                pnlSectioning.Visible = false;
                pnlSectioningFooter.Visible = false;
                pnlSectioningSearch.Visible = false;
                btnMovetowith.Visible = false;
                btnMovetowithout.Visible = false;
                btnCancel.Visible = false;
                pnlSetAdvisory.Visible = true;
                pnlSetAdvisory.Location = new Point(6, 35);

                btnmovetoWithAdv.Visible = true;
                btnMovetoWithoutAdv.Visible = true;
                btnsetAdvCancel.Visible = true;
                btnmovetoWithAdv.Enabled = false;
                btnMovetoWithoutAdv.Enabled = false;
                btnsetAdvCancel.Enabled = false;

                lblgenres.Visible = false;
                lblUserNo.Text = "Faculty no:";
                lblKey.Text = "";
                toolTip1.SetToolTip(txtSearch, "search faculty");

                cmbLevelSetAdv.Text = "Kinder";
                cmbSecSetAdv.Text = "1";
                setupview_forSetAdvisory(cmbLevelSetAdv.Text);
                setupview_forSetAdvisoryWithoutAdv();
            }
        }

      

        public void setuplist_NoSectionforSEC(string lev)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'No.',(select concat(fname,' ',mname,' ',lname)) as 'Name',level as 'Level' from stud_tbl where section='' and level='"+lev+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            OdbcDataAdapter da0 = new OdbcDataAdapter("Select count(gender) from stud_tbl where gender='Male' and section='' and level='" + lev + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(gender) from stud_tbl where gender='Female' and section='' and level='" + lev + "'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            con.Close();
            dvNoSec = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlWOSec.Visible = false;
                dgvNoSec.DataSource = dvNoSec;

                dgvNoSec.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvNoSec.Columns[0].Width = 100;
                dgvNoSec.Columns[1].Width = 255;
                dgvNoSec.Columns[2].Width = 80;
            }
            else
            {
                dgvNoSec.DataSource = null;
                pnlWOSec.Visible = true;
                lblWOSec.Text = "no items found...";
            }

            lblresultnosec.Text = "no. of student without section in " + cmbLevelNoSec.Text + ": " + dgvNoSec.Rows.Count.ToString();


           
            if (dt0.Rows.Count > 0 && dt1.Rows.Count>0)
            {
                lblGenResult.Text = "Male: " + dt0.Rows[0].ItemArray[0].ToString() + "  Female: " + dt1.Rows[0].ItemArray[0].ToString();
            }
            else if (dt0.Rows.Count > 0 && dt1.Rows.Count < 0)
            {
                lblGenResult.Text = "Male: " + dt0.Rows[0].ItemArray[0].ToString() + "  Female: 0";
            }
            else if (dt0.Rows.Count < 0 && dt1.Rows.Count > 0)
            {
                lblGenResult.Text = "Male: 0" + "  Female: " + dt1.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                lblGenResult.Text = "Male: 0" + "  Female: 0";
            }
        }

        public void setupview_perLevelforSEC(string level, string section)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'No.',(select concat(fname,' ',mname,' ',lname)) as 'Name',level as 'Level' from stud_tbl where level='"+level+"' and section='"+section+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            OdbcDataAdapter da0 = new OdbcDataAdapter("Select count(gender) from stud_tbl where gender='Male' and level='" + level + "' and section='" + section + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(gender) from stud_tbl where gender='Female' and level='" + level + "' and section='" + section + "'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            con.Close();
            dvPerLevel = new DataView(dt);


            if (dt.Rows.Count > 0)
            {
                pnlwithout.Visible = false;
                dgvSearch.DataSource = dvPerLevel;

                dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[0].Width = 100;
                dgvSearch.Columns[1].Width = 290;
                dgvSearch.Columns[2].Width = 80;

            }
            else
            {
                dgvSearch.DataSource = null;
                pnlwith.Visible = true;
                lblmemowith.Text = "no items found...";
            }

            lblResult.Text = "no. of student: " + dgvSearch.Rows.Count.ToString();

            if (dt0.Rows.Count > 0 && dt1.Rows.Count > 0)
            {
                lblgenres.Text = "Male: " + dt0.Rows[0].ItemArray[0].ToString() + "  Female: " + dt1.Rows[0].ItemArray[0].ToString();
            }
            else if (dt0.Rows.Count > 0 && dt1.Rows.Count < 0)
            {
                lblgenres.Text = "Male: " + dt0.Rows[0].ItemArray[0].ToString() + "  Female: 0";
            }
            else if (dt0.Rows.Count < 0 && dt1.Rows.Count > 0)
            {
                lblgenres.Text = "Male: 0" + "  Female: " + dt1.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                lblgenres.Text = "Male: 0" + "  Female: 0";
            }
        }

        public void setup_ENABLEINPUTforAS()
        {
            txtSectionName.Enabled = true;
        }

        public void setup_DISABLEINPUTforAS()
        {
            txtSectionName.Enabled = false;
        }

        public void setup_CLEARforAS()
        {
            txtSectionName.Clear();
         
            lblKey.Text = "";
        }

        public void setupview_forAddSection(string level)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select section as 'Section'from section_tbl where level='" + level + "' order by section ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvAS = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlwith.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvAS;
                dgvSearch.Columns[0].Width = 407;
                
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlwith.Visible = true;
                lblmemowith.Text = "no items found...";
            }

            lblResult.Text = "number of section: " + dgvSearch.Rows.Count.ToString();
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (btnASAdd.Enabled == false && btnASUpdate.Text=="Update")
            {
                txtSectionName.Clear(); btnASAdd.Enabled = true; txtSectionName.Enabled = true;
                btnASUpdate.Enabled = false; btnASDelete.Enabled = false;
            }
            setupview_forAddSection(cmbLevel.Text);
        }

        private void btnASAdd_Click(object sender, EventArgs e)
        {
            if (cmbLevel.Text == "" || txtSectionName.Text == "")
            {
                MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                setup_AddforAS(selectedsec);
            }
        }

        public void setup_AddforAS(string selected)
        {
            con.Open();
            OdbcDataAdapter dac = new OdbcDataAdapter("Select*from section_tbl where level='" + cmbLevel.Text + "'and isFull='" + "no" + "'", con);
            DataTable dtc = new DataTable();
            dac.Fill(dtc);
            con.Close();
            if (dtc.Rows.Count > 0)
            {
                MessageBox.Show("Adding not allowed, Section "+dtc.Rows[0].ItemArray[1].ToString()+" is not full.", "Section maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from section_tbl where section='" + txtSectionName.Text + "'and level='" + cmbLevel.Text + "'and section<>'" + selected + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("section already added.", "Section maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            con.Open();
            string addSec = "Insert Into section_tbl(section,level)values('" + txtSectionName.Text + "','" +cmbLevel.Text + "')";

            OdbcCommand cmdAddSec = new OdbcCommand(addSec, con);
            cmdAddSec.ExecuteNonQuery();
            con.Close();

            btnASAdd.Enabled = false;
            setupview_forAddSection(cmbLevel.Text);
            MessageBox.Show("section successfully added", "Section maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnASClear_Click(object sender, EventArgs e)
        {
            if (btnASClear.Text == "Clear")
            {
                setup_CLEARforAS();

                btnASUpdate.Enabled = false;
                btnASDelete.Enabled = false;
                btnASAdd.Enabled = true;
                btnASUpdate.Text = "Update";
                setup_ENABLEINPUTforAS();
            }
            else
            {
                btnASDelete.Enabled = true;
                btnASClear.Text = "Clear";
                btnASUpdate.Text = "Update";

                primarykey = lblKey.Text;
                setupretrieveddata_forAS(primarykey);
                setup_DISABLEINPUTforAS();
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
        }

        public void setupretrieveddata_forAS(string thekey)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from section_tbl where id='" + thekey + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                txtSectionName.Text = dt.Rows[0].ItemArray[1].ToString();
                cmbLevel.Text = dt.Rows[0].ItemArray[2].ToString();
               
                selectedsec = txtSectionName.Text;
              
                btnASAdd.Enabled = false;
            }
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count > 0)
            {
               
            }
            if (cmbOperation.Text == "Add section")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    return;
                }
                setup_DISABLEINPUTforAS();
                btnASUpdate.Enabled = true;
                btnASDelete.Enabled = true;
                selectedsec = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();

                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select id from section_tbl where section='" + selectedsec + "'and level='" + cmbLevel.Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    primarykey = dt.Rows[0].ItemArray[0].ToString();
                }
                setupretrieveddata_forAS(primarykey);
            }
            if (cmbOperation.Text == "Student sectioning")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    return;
                }
                lblKey.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                btnMovetowith.Enabled = false;
                btnMovetowithout.Enabled = true;
                btnCancel.Enabled = true;
            }
            if (cmbOperation.Text == "Set advisory class")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    return;
                }
                lblKey.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                btnmovetoWithAdv.Enabled = false;
                btnMovetoWithoutAdv.Enabled = true;
                btnsetAdvCancel.Enabled = true;
            }
        }

        private void btnASUpdate_Click(object sender, EventArgs e)
        {
            if (btnASUpdate.Text == "Update")
            {
                setup_ENABLEINPUTforAS();
                btnASUpdate.Text = "Save";
                
                btnASDelete.Enabled = false;
                btnASClear.Text = "Cancel";
            }
            else
            {
                if (cmbLevel.Text == "" || txtSectionName.Text == "")
                {
                    MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    setupsaveoperation_forAS(selectedsec);
                   
                    if (dgvSearch.Rows.Count >= 1)
                    {
                        dgvSearch.Rows[0].Selected = true;
                    }
                }
            }
        }
        public void setupsaveoperation_forAS(string selected)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from section_tbl where section='" + txtSectionName.Text + "'and section<>'" + selected + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("section already exists.", "Section maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {

                con.Open();
                string updateSec = "Update section_tbl set section='" + txtSectionName.Text + "',level='" + cmbLevel.Text + "'where id='" + lblKey.Text + "'";
                OdbcCommand cmdUpdateSec = new OdbcCommand(updateSec, con);
                cmdUpdateSec.ExecuteNonQuery();

                string updateSec2 = "Update roomallocation_tbl set section='" + txtSectionName.Text + "' where grade='" + cmbLevel.Text + "'and section='"+selectedsec+"'";
                OdbcCommand cmdUpdateSec2 = new OdbcCommand(updateSec2, con);
                cmdUpdateSec2.ExecuteNonQuery();

                string updateSec3 = "Update facultysched_tbl set section='" + txtSectionName.Text + "' where level='" + cmbLevel.Text + "'and section='" + selectedsec + "'";
                OdbcCommand cmdUpdateSec3 = new OdbcCommand(updateSec3, con);
                cmdUpdateSec3.ExecuteNonQuery();

                string updateSec4 = "Update schedule_tbl set section='" + txtSectionName.Text + "' where level='" + cmbLevel.Text + "'and section='" + selectedsec + "'";
                OdbcCommand cmdUpdateSec4 = new OdbcCommand(updateSec4, con);
                cmdUpdateSec4.ExecuteNonQuery();

                string updateSec5 = "Update stud_tbl set section='" + txtSectionName.Text + "' where level='" + cmbLevel.Text + "'and section='" + selectedsec + "'";
                OdbcCommand cmdUpdateSec5 = new OdbcCommand(updateSec5, con);
                cmdUpdateSec5.ExecuteNonQuery();

                con.Close();

                btnASAdd.Enabled = false;
                setupview_forAddSection(cmbLevel.Text);
                btnASClear.Text = "Clear";
                btnASUpdate.Text = "Update";
                btnASClear.Text = "Clear";
                btnASUpdate.Enabled = false;
                btnASDelete.Enabled = false;
                MessageBox.Show("section successfully updated", "Section maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtSearch.Focus();
            }
        }

        private void btnASDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete?", "Section maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (cmbLevel.Text == "" || txtSectionName.Text == "")
                {
                    MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    setupdeleteoperation_forAS();
                    btnASUpdate.Enabled = false;
                    btnASDelete.Enabled = false;
                    setup_CLEARforAS();
                    setup_ENABLEINPUTforAS();
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

        public void setupdeleteoperation_forAS()
        {
             con.Open();
             string deleteSec = "Delete from section_tbl where id='" + lblKey.Text + "'";
             OdbcCommand cmdDeleteSec = new OdbcCommand(deleteSec, con);
             cmdDeleteSec.ExecuteNonQuery();
             con.Close();

             btnASAdd.Enabled = false;
             setupview_forAddSection(cmbLevel.Text);
             MessageBox.Show("section successfully deleted", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
             txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (cmbOperation.Text == "Add section")
            {
                dvAS.RowFilter = string.Format("Section LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvAS;
                if (dgvSearch.Rows.Count > 0)
                {
                    pnlwith.Visible = false;
                }
                if (dgvSearch.Rows.Count == 0 && txtSearch.Text != "")
                {
                    pnlwith.Visible = true;
                    lblmemowith.Text = "0 search result";
                }
                if (dgvSearch.Rows.Count == 0 && txtSearch.Text == "")
                {
                    pnlwith.Visible = true;
                    lblmemowith.Text = "no items found!";
                }
            }
            if (cmbOperation.Text == "Student sectioning")
            {
                dvPerLevel.RowFilter = string.Format("Name LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvPerLevel ;
                if (dgvSearch.Rows.Count > 0)
                {
                    pnlwith.Visible = false;
                }
                if (dgvSearch.Rows.Count == 0 && txtSearch.Text != "")
                {
                    pnlwith.Visible = true;
                    lblmemowith.Text = "0 search result";
                }
                if (dgvSearch.Rows.Count == 0 && txtSearch.Text == "")
                {
                    pnlwith.Visible = true;
                    lblmemowith.Text = "no items found!";
                }
            }
            if (cmbOperation.Text == "Set advisory class")// && txtSearch.Text != ""
            {
                dvAdv.RowFilter = string.Format("Faculty LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvAdv;
                if (dgvSearch.Rows.Count > 0)
                {
                    pnlwith.Visible = false;
                }
                if (dgvSearch.Rows.Count == 0 && txtSearch.Text != "")
                {
                    pnlwith.Visible = true;
                    lblmemowith.Text = "0 search result";
                }
                if (dgvSearch.Rows.Count == 0 && txtSearch.Text == "")
                {
                    pnlwith.Visible = true;
                    lblmemowith.Text = "no items found!";
                }
            }
        }

        private void txtSearchNoSec_TextChanged(object sender, EventArgs e)
        {
            if (dgvNoSec.Rows.Count > 0)
            {
                pnlWOSec.Visible = false;
            }
            if (dgvNoSec.Rows.Count == 0 && txtSearchNoSec.Text != "")
            {
                pnlWOSec.Visible = true;
                lblWOSec.Text = "0 search result";
            }
            if (dgvNoSec.Rows.Count == 0 && txtSearchNoSec.Text == "")
            {
                pnlWOSec.Visible = true;
                lblWOSec.Text = "no items found!";
            }

            dvNoSec.RowFilter = string.Format("Name LIKE '%{0}%'", txtSearchNoSec.Text);
            dgvNoSec.DataSource = dvNoSec;
        }

        private void cmbLevelNoSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSecNoSec.Text = "1";
            setupview_perLevelforSEC(cmbLevelNoSec.Text,"1");
            setuplist_NoSectionforSEC(cmbLevelNoSec.Text);
            setupSectionSectioning(cmbLevelNoSec.Text);
            lblResult.Text = "no. of student in " + cmbLevelNoSec.Text + " section " + cmbSecNoSec.Text + ": " + dgvSearch.Rows.Count;
        }

        public void setupSectionSectioning(string keystring)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select section from section_tbl where level='" + keystring + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbSecNoSec.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSecNoSec.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        private void cmbSecNoSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupview_perLevelforSEC(cmbLevelNoSec.Text, cmbSecNoSec.Text);
            setuplist_NoSectionforSEC(cmbLevelNoSec.Text);
            lblResult.Text = "no. of student in " + cmbLevelNoSec.Text + " section " + cmbSecNoSec.Text + ": " + dgvSearch.Rows.Count;
        }

        private void dgvNoSec_Click(object sender, EventArgs e)
        {
            if (dgvNoSec.Rows.Count <= 0)
            {
                return;
            }

            lblKey.Text = dgvNoSec.SelectedRows[0].Cells[0].Value.ToString();

            if (cmbSecNoSec.Text != "")
            {
                btnMovetowith.Enabled = true;
                btnCancel.Enabled = true;
            }
            btnMovetowithout.Enabled = false;
            
        }

        private void btnMovetowith_Click(object sender, EventArgs e)
        {
            string level = cmbLevelNoSec.Text;

            con.Open();
            string updateSec1 = "Update stud_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
            OdbcCommand cmdUpdateSec1 = new OdbcCommand(updateSec1, con);
            cmdUpdateSec1.ExecuteNonQuery();
            con.Close();

            btnMovetowith.Enabled = false;
            btnCancel.Enabled = false;
            setupview_perLevelforSEC(cmbLevelNoSec.Text, cmbSecNoSec.Text);
            setuplist_NoSectionforSEC(cmbLevelNoSec.Text);
            lblResult.Text = "no. of student in " + cmbLevelNoSec.Text + " section " + cmbSecNoSec.Text + ": " + dgvSearch.Rows.Count;

            if(level=="Kinder")
            {
                con.Open();
                string update = "Update Kindergrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();
            }
            if (level == "Grade 1")
            {
                con.Open();
                string update = "Update gradeonegrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 2")
            {
                con.Open();
                string update = "Update gradetwogrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 3")
            {
                con.Open();
                string update = "Update gradethreegrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 4")
            {
                con.Open();
                string update = "Update gradefourgrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 5")
            {
                con.Open();
                string update = "Update gradefivegrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 6")
            {
                con.Open();
                string update = "Update gradesixgrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 7")
            {
                con.Open();
                string update = "Update gradesevengrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 8")
            {
                con.Open();
                string update = "Update gradeeightgrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 9")
            {
                con.Open();
                string update = "Update gradeninegrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 10")
            {
                con.Open();
                string update = "Update gradetengrades_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
        }

        private void btnMovetowithout_Click(object sender, EventArgs e)
        {
            string level = cmbLevelNoSec.Text;

            con.Open();
            string updateSec2 = "Update stud_tbl set section='' where studno='" + lblKey.Text + "'";
            OdbcCommand cmdUpdateSec2 = new OdbcCommand(updateSec2, con);
            cmdUpdateSec2.ExecuteNonQuery();
            con.Close();

            if (level == "Kinder")
            {
                con.Open();
                string update = "Update Kindergrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();
            }
            if (level == "Grade 1")
            {
                con.Open();
                string update = "Update gradeonegrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 2")
            {
                con.Open();
                string update = "Update gradetwogrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 3")
            {
                con.Open();
                string update = "Update gradethreegrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 4")
            {
                con.Open();
                string update = "Update gradefourgrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 5")
            {
                con.Open();
                string update = "Update gradefivegrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 6")
            {
                con.Open();
                string update = "Update gradesixgrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 7")
            {
                con.Open();
                string update = "Update gradesevengrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 8")
            {
                con.Open();
                string update = "Update gradeeightgrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 9")
            {
                con.Open();
                string update = "Update gradeninegrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 10")
            {
                con.Open();
                string update = "Update gradetengrades_tbl set section='" + "" + "'where studno='" + lblKey.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }

            btnMovetowithout.Enabled = false;
            btnCancel.Enabled = false;
            setupview_perLevelforSEC(cmbLevelNoSec.Text, cmbSecNoSec.Text);
            setuplist_NoSectionforSEC(cmbLevelNoSec.Text);
            lblResult.Text = "no. of student in " + cmbLevelNoSec.Text + " section " + cmbSecNoSec.Text + ": " + dgvSearch.Rows.Count;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnMovetowith.Enabled = false;
            btnMovetowithout.Enabled = false;
            btnCancel.Enabled = false;
            lblKey.Text = "";

            if (dgvNoSec.Rows.Count > 0)
            {
                dgvNoSec.Rows[0].Selected = true;
            }
            if (dgvSearch.Rows.Count > 0)
            {
                dgvSearch.Rows[0].Selected = true;
            }
        }

        private void cmbLevelSetAdv_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupview_forSetAdvisory(cmbLevelSetAdv.Text);
            setupSection(cmbLevelSetAdv.Text);
        }

        public void setupSection(string keystring)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select section from section_tbl where level='"+keystring+"'",con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbSecSetAdv.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSecSetAdv.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        public void setupSetanAdviserToGradesTable(string level,string facID)
        {
            string theAdv = "";
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno='"+facID+"'",con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                theAdv = dt.Rows[0].ItemArray[1].ToString() +" "+ dt.Rows[0].ItemArray[2].ToString() +" "+ dt.Rows[0].ItemArray[3].ToString();
            }

            if (level == "Kinder")
            {
                con.Open();
                string update = "Update Kindergrades_tbl set adviser='" +theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();
            }
            if (level == "Grade 1")
            {
                con.Open();
                string update = "Update gradeonegrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 2")
            {
                con.Open();
                string update = "Update gradetwogrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 3")
            {
                con.Open();
                string update = "Update gradethreegrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 4")
            {
                con.Open();
                string update = "Update gradefourgrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 5")
            {
                con.Open();
                string update = "Update gradefivegrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 6")
            {
                con.Open();
                string update = "Update gradesixgrades_tbl set adviser='" + theAdv+ "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 7")
            {
                con.Open();
                string update = "Update gradesevengrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 8")
            {
                con.Open();
                string update = "Update gradeeightgrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 9")
            {
                con.Open();
                string update = "Update gradeninegrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 10")
            {
                con.Open();
                string update = "Update gradetengrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
        }

        private void btnmovetoWithAdv_Click(object sender, EventArgs e)
        {
            if (cmbSecSetAdv.Text == "")
            {
                MessageBox.Show("no selected section.", "Section maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnmovetoWithAdv.Enabled = false;
                btnsetAdvCancel.Enabled = false;
                return;
            }
            else
            {
                string level = cmbLevelSetAdv.Text;
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where position='faculty' and advisory='" + cmbSecSetAdv.Text + "' and grade='" + cmbLevelSetAdv.Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    DialogResult res = MessageBox.Show(cmbLevelSetAdv.Text + " section " + cmbSecSetAdv.Text + " already have adviser." + "\nWould you like to replace it?", "Section Maintenance", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res== DialogResult.Yes)
                    {
                        string originsID = dt.Rows[0].ItemArray[0].ToString();

                        con.Open();

                        string updateReplace = "Update employees_tbl set advisory='" + cmbSecSetAdv.Text + "',grade='" + cmbLevelSetAdv.Text + "' where empno='" + lblKey.Text + "'";
                        OdbcCommand cmdUpdateReplace = new OdbcCommand(updateReplace, con);
                        cmdUpdateReplace.ExecuteNonQuery();

                        string updateRemove = "Update employees_tbl set advisory='',grade='" + cmbLevelSetAdv.Text + "' where empno='" + originsID + "'";
                        OdbcCommand cmdUpdateRemove = new OdbcCommand(updateRemove, con);
                        cmdUpdateRemove.ExecuteNonQuery();

                        con.Close();

                        setupSetanAdviserToGradesTable(level,lblKey.Text);
                        btnmovetoWithAdv.Enabled = false;
                        btnsetAdvCancel.Enabled = false;

                        setupview_forSetAdvisory(cmbLevelSetAdv.Text);
                        setupview_forSetAdvisoryWithoutAdv();
                    }
                    else if (res==DialogResult.Cancel)
                    {
                        lblKey.Text = "";
                        btnmovetoWithAdv.Enabled = false;
                        btnMovetoWithoutAdv.Enabled = false;
                        btnsetAdvCancel.Enabled = false;
                        dgvSetAdvNoAdv.Rows[0].Selected = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    con.Open();
                    string updateAdv1 = "Update employees_tbl set advisory='" + cmbSecSetAdv.Text + "',grade='" + cmbLevelSetAdv.Text + "' where empno='" + lblKey.Text + "'";
                    OdbcCommand cmdUpdateAdv1 = new OdbcCommand(updateAdv1, con);
                    cmdUpdateAdv1.ExecuteNonQuery();
                    con.Close();

                    setupSetanAdviserToGradesTable(level,lblKey.Text);
                    btnmovetoWithAdv.Enabled = false;
                    btnsetAdvCancel.Enabled = false;

                    setupview_forSetAdvisory(cmbLevelSetAdv.Text);
                    setupview_forSetAdvisoryWithoutAdv();
                }
            }

        }

        private void btnMovetoWithoutAdv_Click(object sender, EventArgs e)
        {
            string level = cmbLevelSetAdv.Text;
            string sectionwhowasremovedanadviser = dgvSearch.SelectedRows[0].Cells[3].Value.ToString();
            con.Open();
            string updateAdv2 = "Update employees_tbl set advisory='' where empno='" + lblKey.Text + "'";
            OdbcCommand cmdUpdateAdv2 = new OdbcCommand(updateAdv2, con);
            cmdUpdateAdv2.ExecuteNonQuery();
            con.Close();

            if (level == "Kinder")
            {
                con.Open();
                string update = "Update Kindergrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();
            }
            if (level == "Grade 1")
            {
                con.Open();
                string update = "Update gradeonegrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 2")
            {
                con.Open();
                string update = "Update gradetwogrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 3")
            {
                con.Open();
                string update = "Update gradethreegrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 4")
            {
                con.Open();
                string update = "Update gradefourgrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 5")
            {
                con.Open();
                string update = "Update gradefivegrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 6")
            {
                con.Open();
                string update = "Update gradesixgrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 7")
            {
                con.Open();
                string update = "Update gradesevengrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 8")
            {
                con.Open();
                string update = "Update gradeeightgrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 9")
            {
                con.Open();
                string update = "Update gradeninegrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }
            if (level == "Grade 10")
            {
                con.Open();
                string update = "Update gradetengrades_tbl set adviser='" + "" + "'where section='" + sectionwhowasremovedanadviser + "'";
                OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                cmdUpdate.ExecuteNonQuery();
                con.Close();

            }

            btnMovetoWithoutAdv.Enabled = false;
            btnsetAdvCancel.Enabled = false;

            setupview_forSetAdvisory(cmbLevelSetAdv.Text);
            setupview_forSetAdvisoryWithoutAdv();
        }

        public void setupview_forSetAdvisory(string leveladv)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select empno as 'No.',(select concat(firstname,' ',middlename,' ',lastname))as'Faculty',subject as 'Subject',advisory as 'Advisory' from employees_tbl where grade='" + leveladv + "' and advisory<>'' and position='faculty'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvAdv = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlwith.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvAdv;
                dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[0].Width = 50;
                dgvSearch.Columns[1].Width = 230;
                dgvSearch.Columns[2].Width = 120;
                dgvSearch.Columns[3].Width = 70;

            }
            else
            {
                dgvSearch.DataSource = null;
                pnlwith.Visible = true;
                lblmemowith.Text = "no items found...";
            }

            lblResult.Text = "number of faculty: " + dgvSearch.Rows.Count.ToString();
            lblResultNoAdv.Text = "no. of faculty without advisory class: " + dgvSetAdvNoAdv.Rows.Count.ToString();
        }

        public void setupview_forSetAdvisoryWithoutAdv()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select empno as 'No.',(select concat(firstname,' ',middlename,' ',lastname))as'Faculty',subject as 'Subject' from employees_tbl where position='faculty' and advisory=''", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvAdvNoAdv = new DataView(dt);


            if (dt.Rows.Count > 0)
            {
                pnlwithout.Visible = false;
                dgvSetAdvNoAdv.DataSource = dvAdvNoAdv;
                dgvSetAdvNoAdv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSetAdvNoAdv.Columns[0].Width = 50;
                dgvSetAdvNoAdv.Columns[1].Width = 250;
                dgvSetAdvNoAdv.Columns[2].Width = 130;

            }
            else
            {
                dgvSetAdvNoAdv.DataSource = null;
                pnlwithout.Visible = true;
                lblmemowithout.Text = "no items found...";
            }

            lblResult.Text = "number of faculty: " + dgvSearch.Rows.Count.ToString();
            lblResultNoAdv.Text = "no. of faculty without advisory class: " + dgvSetAdvNoAdv.Rows.Count.ToString();
        }

        private void dgvSetAdvNoAdv_Click(object sender, EventArgs e)
        {
            if (dgvSetAdvNoAdv.Rows.Count <= 0)
            {
                return;
            }
            lblKey.Text = dgvSetAdvNoAdv.SelectedRows[0].Cells[0].Value.ToString();

            if (cmbSecSetAdv.Text != "")
            {
                btnsetAdvCancel.Enabled = true;
                btnmovetoWithAdv.Enabled = true;
            }
        }

        private void btnsetAdvCancel_Click(object sender, EventArgs e)
        {
            btnMovetoWithoutAdv.Enabled = false;
            btnsetAdvCancel.Enabled = false;
            btnmovetoWithAdv.Enabled = false;
            lblKey.Text = "";

            if (dgvSetAdvNoAdv.Rows.Count > 0)
            {
                dgvSetAdvNoAdv.Rows[0].Selected = true;
            }
            if (dgvSearch.Rows.Count > 0)
            {
                dgvSearch.Rows[0].Selected = true;
            }
        }

        private void txtSearchWithoutAdv_TextChanged(object sender, EventArgs e)
        {
            if (dgvSetAdvNoAdv.Rows.Count > 0)
            {
                pnlwithout.Visible = false;
            }
            if (dgvSetAdvNoAdv.Rows.Count == 0 && txtSearchWithoutAdv.Text != "")
            {
                pnlwithout.Visible = true;
                lblmemowithout.Text = "0 search result";
            }
            if (dgvSetAdvNoAdv.Rows.Count == 0 && txtSearchWithoutAdv.Text == "")
            {
                pnlwithout.Visible = true;
                lblmemowithout.Text = "no items found!";
            }
            if (dgvSetAdvNoAdv.Rows.Count >= 1)
            {
                dgvSetAdvNoAdv.Rows[0].Selected = true;
            }
                dvAdvNoAdv.RowFilter = string.Format("Faculty LIKE '%{0}%'", txtSearchWithoutAdv.Text);
                dgvSetAdvNoAdv.DataSource = dvAdvNoAdv;
                toolTip1.SetToolTip(txtSearch, "search faculty");
       
        }

        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (txtSearch.Text=="")
            {
                pnlwith.Visible = false;
            }
        }

        private void dgvSetAdvNoAdv_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (txtSearchWithoutAdv.Text == "")
            {
                pnlwithout.Visible = false;
            }
        }

        private void dgvNoSec_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (txtSearchNoSec.Text == "")
            {
                pnlWOSec.Visible = false;
            }
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roommaintenance = new frmRoom();
            roommaintenance.logger = secwholog;
            roommaintenance.VISITED = VISITED;
            roommaintenance.Show();
            this.Hide();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance am = new frmAboutMaintenance();
            am.amlog = secwholog;
            am.Show();
            this.Hide();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched sf = new frmSched();
            this.Hide();
            sf.schedlog = secwholog;
            sf.VISITED = VISITED;
            sf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqform = new frmRequirement();
            this.Hide();
            reqform.reqlog = secwholog;
            reqform.VISITED = VISITED;
            reqform.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feeform = new frmFee();
            this.Hide();
            feeform.feelog = secwholog;
            feeform.VISITED = VISITED;
            feeform.Show();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Hide();
            actform.actlog = secwholog;
            actform.Show();
        }

        private void btnAud_Click(object sender, EventArgs e)
        {
            frmAudit audform = new frmAudit();
            this.Hide();
            audform.auditlogger = secwholog;
            audform.Show();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Hide();
            discform.disclog = secwholog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Hide();
            buf.backlog = secwholog;
            buf.Show();
        }

        private void frmSection_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void pnlUser_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
        }

        private void cmbSecSetAdv_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = secwholog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Hide();
            levmain.levlog = secwholog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = secwholog;
            facmain.VISITED = VISITED;
            facmain.Show();
            this.Hide();
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = secwholog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = secwholog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = secwholog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = secwholog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSectionName.Text = "";
            setupLevelList(cmbDept.Text);
            setupview_forAddSection("");
            pnlwith.Visible = false;
           
           
        }

       
    }
}
