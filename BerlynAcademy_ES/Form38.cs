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
    public partial class frmStudent : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public DataView dvstf;
        public string stdlog, primarykey,VISITED,activeYr,activeSY;
        public int lstEnye = 1, fnmEnye = 1, mnmEnye = 1,fatenye=1,motenye=1,guaenye=1;

        public frmStudent()
        {
            InitializeComponent();
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            lblLogger.Text = stdlog;
            lblLoggerPosition.Text = "Admin";
            btnStud.BackColor = Color.LightGreen;
            cmbFilter.Text = "All students";
            setupview_forStudent();
            setupyears();
            GetActiveSchoolYear();
            setupdisableinput();

            if (VISITED.Contains("Student") == false)
            {
                VISITED += "   Student";
            }
        }

        public void setupyears()
        {
            int start = 1970;
            int current = Convert.ToInt32(DateTime.Now.Year);

            while (current >= start)
            {
                cmbYear.Items.Add(current);
                current--;
            }
        }

        public void setupview_forStudent()
        {
            dgvSearch.DataSource = null;

            con.Open();
            OdbcDataAdapter daStudent = new OdbcDataAdapter("Select studno,lname as 'Lastname',fname as 'Firstname',mname as 'Middlename' from stud_tbl order by lname ASC", con);
            DataTable dtStudent = new DataTable();
            daStudent.Fill(dtStudent);
            con.Close();
            dvstf = new DataView(dtStudent);
            dgvSearch.DataSource = dvstf;

            dgvSearch.Columns[0].Width = 0;
            dgvSearch.Columns[1].Width = 115;
            dgvSearch.Columns[2].Width = 115;
            dgvSearch.Columns[3].Width = 115;
            dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
            lblResult.Text = "number of students: " + dgvSearch.Rows.Count.ToString();
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }

            setupdisableinput();
            btnUpdate.Enabled = true;
            btnUpdate.Text = "Update";
            btnClear.Text = "Clear";

            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
            {
                primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }

            setupretrieveddata(primarykey);
        }

        public void setupretrieveddata(string thekey)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='" + thekey + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                txtFname.Text = dt.Rows[0].ItemArray[1].ToString();
                txtMidl.Text = dt.Rows[0].ItemArray[2].ToString();
                txtLast.Text = dt.Rows[0].ItemArray[3].ToString();
                txtSchool.Text = dt.Rows[0].ItemArray[6].ToString();
                txtAdd.Text = dt.Rows[0].ItemArray[7].ToString();
                if (dt.Rows[0].ItemArray[8].ToString() != "")
                {
                    cmbMonth.Text = dt.Rows[0].ItemArray[8].ToString().Substring(0, 3).ToString();//0 start of string 3 the length 
                    cmbDay.Text = dt.Rows[0].ItemArray[8].ToString().Substring(4, 2).ToString();
                    cmbYear.Text = dt.Rows[0].ItemArray[8].ToString().Substring(7, 4).ToString();
                    txtAge.Text = dt.Rows[0].ItemArray[9].ToString();
                }
              
                cmbGen.Text = dt.Rows[0].ItemArray[10].ToString();
                txtCon.Text = dt.Rows[0].ItemArray[11].ToString();
                txtFathName.Text = dt.Rows[0].ItemArray[12].ToString();
                txtFathOcc.Text = dt.Rows[0].ItemArray[13].ToString();
                txtMothName.Text = dt.Rows[0].ItemArray[14].ToString();
                txtMothOcc.Text = dt.Rows[0].ItemArray[15].ToString();
                txtGrdName.Text = dt.Rows[0].ItemArray[16].ToString();
                txtGrdOcc.Text = dt.Rows[0].ItemArray[17].ToString();
                txtGrdCon.Text = dt.Rows[0].ItemArray[18].ToString();
                txtTalSki.Text = dt.Rows[0].ItemArray[19].ToString();
                txtAward.Text = dt.Rows[0].ItemArray[20].ToString();
                txtGrdRelation.Text = dt.Rows[0].ItemArray[25].ToString();
              
            }
        }

        public void computeAge()
        {
            if (cmbMonth.Text != "" && cmbDay.Text != "" && cmbYear.Text != "")
            {
                int current = Convert.ToInt32(DateTime.Now.Year);
                int birth = Convert.ToInt32(cmbYear.Text);
                int age = current - birth;
                txtAge.Text = age.ToString();
            }
        }

        public void setupdisableinput()
        {
            btnN1.Enabled = false;
            btnN2.Enabled = false;
            btnN3.Enabled = false;
            btnNFat.Enabled = false;
            btnNMot.Enabled = false;
            btnNGua.Enabled = false;
            txtLast.Enabled = false;
            txtFname.Enabled = false;
            txtMidl.Enabled = false;
            txtAdd.Enabled = false;
            cmbMonth.Enabled = false;
            cmbDay.Enabled = false;
            cmbYear.Enabled = false;
            cmbGen.Enabled = false;
            txtCon.Enabled = false;
            txtSchool.Enabled = false;
            txtAward.Enabled = false;
            txtTalSki.Enabled = false;
            txtFathName.Enabled = false;
            txtFathOcc.Enabled = false;
            txtMothName.Enabled = false;
            txtMothOcc.Enabled = false;
            txtGrdName.Enabled = false;
            txtGrdOcc.Enabled = false;
            txtGrdRelation.Enabled = false;
            txtGrdCon.Enabled = false;
        }

        public void setupclear()
        {
            txtLast.Clear();
            txtFname.Clear();
            txtMidl.Clear();
            txtAdd.Clear();
            cmbMonth.SelectedIndex = -1;
            cmbDay.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            cmbGen.SelectedIndex = -1;
            txtCon.Clear();
            txtAge.Clear();
            txtSchool.Clear();
            txtAward.Clear();
            txtTalSki.Clear();
            txtFathName.Clear();
            txtFathOcc.Clear();
            txtMothName.Clear();
            txtMothOcc.Clear();
            txtGrdName.Clear();
            txtGrdOcc.Clear();
            txtGrdRelation.Clear();
            txtGrdCon.Clear();
        }

        public void setupenableinput()
        {
            btnN1.Enabled = true;
            btnN2.Enabled = true;
            btnN3.Enabled = true;
            btnNFat.Enabled = true;
            btnNMot.Enabled = true;
            btnNGua.Enabled = true;
           
            txtLast.Enabled = true;
            txtFname.Enabled = true;
            txtMidl.Enabled = true;
            txtAdd.Enabled = true;
            cmbMonth.Enabled = true;
            cmbDay.Enabled = true;
            cmbYear.Enabled = true;
            cmbGen.Enabled = true;
            txtCon.Enabled = true;
            txtSchool.Enabled = true;
            txtAward.Enabled = true;
            txtTalSki.Enabled = true;
            txtFathName.Enabled = true;
            txtFathOcc.Enabled = true;
            txtMothName.Enabled = true;
            txtMothOcc.Enabled = true;
            txtGrdName.Enabled = true;
            txtGrdOcc.Enabled = true;
            txtGrdRelation.Enabled = true;
            txtGrdCon.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Update")
            {
                setupenableinput();
                btnUpdate.Text = "Save";
                btnClear.Text = "Cancel";
            }
            else
            {
                if (txtFname.Text == "" || txtLast.Text == "" || txtAdd.Text=="" || cmbGen.Text=="" || cmbMonth.Text=="" || cmbDay.Text=="" || cmbYear.Text=="" ||
                    txtGrdName.Text=="" || txtGrdRelation.Text=="" || txtGrdCon.Text=="")
                {
                    inputcheck("valid", txtGrdCon);
                    MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtMidl.Text != "")
                {
                    if (txtMidl.TextLength < 2)
                    {
                        inputcheck("invalid", txtMidl);
                       
                    }
                    else
                    {
                        inputcheck("valid", txtMidl);
                    }
                }
                if (txtCon.Text != "")
                {
                    if ((txtCon.TextLength == 11) && (txtCon.Text.Substring(0, 2) != "09"))
                    {
                        inputcheck("invalid", txtCon);
                       
                    }
                    else if ((txtCon.TextLength != 11) && (txtCon.TextLength != 7))
                    {
                        inputcheck("invalid", txtCon);
                       
                    }
                    else
                    {
                        inputcheck("valid", txtCon);
                    }
                }
                if (txtGrdCon.Text != "")
                {
                    if ((txtGrdCon.TextLength == 11) && (txtGrdCon.Text.Substring(0, 2) != "09"))
                    {
                        inputcheck("invalid", txtGrdCon);
                       
                    }
                    else if ((txtGrdCon.TextLength != 11) && (txtGrdCon.TextLength != 7))
                    {

                        inputcheck("invalid", txtGrdCon);
                       
                    }
                    else
                    {
                        inputcheck("valid", txtGrdCon);
                    }
                }
                if (txtMidl.Text == "")
                {
                    inputcheck("valid", txtMidl);
                }
                if (txtCon.Text == "")
                {
                    inputcheck("valid", txtCon);
                }


                if ((((txtGrdCon.TextLength == 11) && (txtGrdCon.Text.Substring(0, 2) == "09")) || (txtGrdCon.TextLength == 7)))
                {
                    if (txtCon.Text != "")
                    {
                        if (((txtCon.TextLength == 11) && (txtCon.Text.Substring(0, 2) != "09")) || ((txtCon.TextLength != 11) && (txtCon.TextLength != 7)))
                        {
                            return;
                        }
                    }
                    if (txtMidl.Text != "")
                    {
                        if (txtMidl.TextLength<2)
                        {
                            return;
                        }
                    }
                 
                    setupsaveoperation();
                    btnUpdate.Text = "Update";
                    btnUpdate.Enabled = false;
                    if (dgvSearch.Rows.Count >= 1)
                    {
                        dgvSearch.Rows[0].Selected = true;
                    }
                }
            }
        }

        public void setupsaveoperation()
        {
            string bday = cmbMonth.Text + " " + cmbDay.Text + " " + cmbYear.Text;
            int current = Convert.ToInt32(DateTime.Now.Year);
            int birthyear = Convert.ToInt32(cmbYear.Text);
            int age = current - birthyear;

            con.Open();
            string updateStudent = "Update stud_tbl set fname='" + txtFname.Text + "',mname='" + txtMidl.Text + "',lname='" + txtLast.Text + "',address='"+txtAdd.Text+"',birthdate='"+bday+"',age='"+age+"',gender='"+cmbGen.Text+"',studcon='"+txtCon.Text+"',school='"+txtSchool.Text+"',talentskill='"+txtTalSki.Text+"',award='"+txtAward.Text+"',fathername='"+txtFathName.Text+"',fatheroccup='"+txtFathOcc.Text+"',mothername='"+txtMothName.Text+"',motheroccup='"+txtMothOcc.Text+"',guardian='"+txtGrdName.Text+"',guardianoccup='"+txtGrdOcc.Text+"',pgcon='"+txtGrdCon.Text+"',guardianrelation='"+txtGrdRelation.Text+"' where studno='" + primarykey + "'";
            OdbcCommand cmdUpdateStudent = new OdbcCommand(updateStudent, con);
            cmdUpdateStudent.ExecuteNonQuery();
            con.Close();

            setupview_forStudent();
            btnClear.Text = "Clear";
            setupdisableinput();
            MessageBox.Show("student successfully updated", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtMidl.BackColor = Color.White;
            txtSearch.Focus();
        }

        public void inputcheck(string validation, TextBox txt)
        {
            if (validation == "valid")
            {
                if (txt != null)
                {
                    txt.BackColor = Color.White;
                }
                else
                {
                }
            }
            else
            {
                if (txt != null)
                {
                    txt.BackColor = Color.Salmon;
                }
                else
                {
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Clear")
            {
                setupclear();
                btnUpdate.Enabled = false;
                btnUpdate.Text = "Update";
            }
            else
            {
               
                btnClear.Text = "Clear";
                btnUpdate.Text = "Update";

                setupretrieveddata(primarykey);
                setupdisableinput();
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            dvstf.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvstf;


            if (dgvSearch.Rows.Count > 0 && txtSearch.Text != "")
            {
                pnlnotify.Visible = false;
            }
            if (dgvSearch.Rows.Count > 0 && txtSearch.Text == "")
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

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "Level")
            {
                cmbfilteritems.Enabled = true;
                setupLevelList();
            }
            if (cmbFilter.Text == "Section")
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select distinct section from section_tbl", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    cmbfilteritems.Enabled = true;
                    cmbfilteritems.Items.Clear();
                    for (int x = 0; x < dt.Rows.Count; x++)
                    {
                        cmbfilteritems.Items.Add(dt.Rows[x].ItemArray[0].ToString());
                    }
                }
            }
            if (cmbFilter.Text == "Gender")
            {
                cmbfilteritems.Enabled = true;
                cmbfilteritems.Items.Clear();
                cmbfilteritems.Items.Add("Male");
                cmbfilteritems.Items.Add("Female");
            }
            if (cmbFilter.Text == "All students")
            {
                cmbfilteritems.Enabled = false;
                cmbfilteritems.Items.Clear();
                setupview_forStudent();
            }
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
                cmbfilteritems.Items.Clear();
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbfilteritems.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                }
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

        private void cmbfilteritems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "Level")
            {
                con.Open();
                OdbcDataAdapter daStudent = new OdbcDataAdapter("Select studno,lname as 'Lastname',fname as 'Firstname',mname as 'Middlename' from stud_tbl where level='" + cmbfilteritems.Text + "'and status='Active'", con);
                DataTable dtStudent = new DataTable();
                daStudent.Fill(dtStudent);
                con.Close();
                dvstf = new DataView(dtStudent);
                dgvSearch.DataSource = dvstf;
                lblResult.Text = "no of student from " + cmbfilteritems.Text + " : " + dgvSearch.Rows.Count;
            }
            if (cmbFilter.Text == "Section")
            {
                con.Open();
                OdbcDataAdapter daStudent = new OdbcDataAdapter("Select studno,lname as 'Lastname',fname as 'Firstname',mname as 'Middlename' from stud_tbl where section='" + cmbfilteritems.Text + "'and status='Active'", con);
                DataTable dtStudent = new DataTable();
                daStudent.Fill(dtStudent);
                con.Close();
                dvstf = new DataView(dtStudent);
                dgvSearch.DataSource = dvstf;
                lblResult.Text = "no of student from section " + cmbfilteritems.Text + " : " + dgvSearch.Rows.Count;
            }
            if (cmbFilter.Text == "Gender")
            {
                con.Open();
                OdbcDataAdapter daStudent = new OdbcDataAdapter("Select studno,lname as 'Lastname',fname as 'Firstname',mname as 'Middlename' from stud_tbl where gender='" + cmbfilteritems.Text + "'and status='Active'", con);
                DataTable dtStudent = new DataTable();
                daStudent.Fill(dtStudent);
                con.Close();
                dvstf = new DataView(dtStudent);
                dgvSearch.DataSource = dvstf;
                lblResult.Text = "no of "+cmbfilteritems.Text+" students: " + dgvSearch.Rows.Count;
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

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = stdlog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maine = new frmMaintenance();
            this.Hide();
            maine.adminlog = stdlog;
            maine.VISITED = VISITED;
            maine.Show();
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            this.Dispose();
            symaintenance.sylog = stdlog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subjmaintenance = new frmSubject();
            this.Dispose();
            subjmaintenance.wholog = stdlog;
            subjmaintenance.VISITED = VISITED;
            subjmaintenance.Show();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Dispose();
            levmain.levlog = stdlog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            frmSection section = new frmSection();
            this.Dispose();
            section.secwholog = stdlog;
            section.VISITED = VISITED;
            section.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roommaintenance = new frmRoom();
            this.Dispose();
            roommaintenance.logger = stdlog;
            roommaintenance.VISITED = VISITED;
            roommaintenance.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            this.Dispose();
            facmain.facmlog = stdlog;
            facmain.VISITED = VISITED;
            facmain.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched sf = new frmSched();
            this.Dispose();
            sf.schedlog = stdlog;
            sf.VISITED = VISITED;
            sf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqform = new frmRequirement();
            this.Dispose();
            reqform.reqlog = stdlog;
            reqform.VISITED = VISITED;
            reqform.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feeform = new frmFee();
            this.Dispose();
            feeform.feelog = stdlog;
            feeform.VISITED = VISITED;
            feeform.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Dispose();
            discform.disclog = stdlog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            this.Dispose();
            hm.adminlog =stdlog;
            hm.VISITED = VISITED;
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

        private void frmStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Dispose();
            home.Show();
        }

        private void btnN1_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtFname.Text != "")
            {
                if (txtFname.TextLength == 1)
                {
                    string last = txtFname.Text.Substring(0, txtFname.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFname.Text.Substring(0, txtFname.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFname.Text;
                    }

                }
                else
                {
                    string last = txtFname.Text.Substring(txtFname.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFname.Text.Substring(0, txtFname.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFname.Text.Substring(0, txtFname.TextLength);
                    }
                }
            }

            if (fnmEnye == 1)
            {
                if (txtFname.Text != "")
                {
                    txtFname.Text = orgtext + "Ñ";
                    fnmEnye += 1;
                }
                else
                {
                    txtFname.Text = "Ñ";
                    fnmEnye += 1;
                }
            }
            else
            {
                if (txtFname.Text != "")
                {
                    txtFname.Text = orgtext + "ñ";
                    fnmEnye -= 1;
                }
                else
                {
                    txtFname.Text = "ñ";
                    fnmEnye -= 1;
                }
            }

            txtFname.Focus();
            txtFname.SelectionStart = txtFname.Text.Length;
        }

        private void btnN2_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtMidl.Text != "")
            {
                if (txtMidl.TextLength == 1)
                {
                    string last = txtMidl.Text.Substring(0, txtMidl.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtMidl.Text.Substring(0, txtMidl.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtMidl.Text;
                    }

                }
                else
                {
                    string last = txtMidl.Text.Substring(txtMidl.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtMidl.Text.Substring(0, txtMidl.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtMidl.Text.Substring(0, txtMidl.TextLength);
                    }
                }
            }

            if (mnmEnye == 1)
            {
                if (txtMidl.Text != "")
                {
                    txtMidl.Text = orgtext + "Ñ";
                    mnmEnye += 1;
                }
                else
                {
                    txtMidl.Text = "Ñ";
                    mnmEnye += 1;
                }
            }
            else
            {
                if (txtMidl.Text != "")
                {
                    txtMidl.Text = orgtext + "ñ";
                    mnmEnye -= 1;
                }
                else
                {
                    txtMidl.Text = "ñ";
                    mnmEnye -= 1;
                }
            }

            txtMidl.Focus();
            txtMidl.SelectionStart = txtMidl.Text.Length;
        }

        private void btnN3_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtLast.Text != "")
            {
                if (txtLast.TextLength == 1)
                {
                    string last = txtLast.Text.Substring(0, txtLast.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtLast.Text.Substring(0, txtLast.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtLast.Text;
                    }

                }
                else
                {
                    string last = txtLast.Text.Substring(txtLast.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtLast.Text.Substring(0, txtLast.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtLast.Text.Substring(0, txtLast.TextLength);
                    }
                }
            }

            if (lstEnye == 1)
            {
                if (txtLast.Text != "")
                {
                    txtLast.Text = orgtext + "Ñ";
                    lstEnye += 1;
                }
                else
                {
                    txtLast.Text = "Ñ";
                    lstEnye += 1;
                }
            }
            else
            {
                if (txtLast.Text != "")
                {
                    txtLast.Text = orgtext + "ñ";
                    lstEnye -= 1;
                }
                else
                {
                    txtLast.Text = "ñ";
                    lstEnye -= 1;
                }
            }

            txtLast.Focus();
            txtLast.SelectionStart = txtLast.Text.Length;
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = stdlog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();

        }

        private void cmbDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void btnNFat_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtFathName.Text != "")
            {
                if (txtFathName.TextLength == 1)
                {
                    string last = txtFathName.Text.Substring(0, txtFathName.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFathName.Text.Substring(0, txtFathName.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFathName.Text;
                    }

                }
                else
                {
                    string last = txtFathName.Text.Substring(txtFathName.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFathName.Text.Substring(0, txtFathName.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFathName.Text.Substring(0, txtFathName.TextLength);
                    }
                }
            }

            if (fatenye == 1)
            {
                if (txtFathName.Text != "")
                {
                    txtFathName.Text = orgtext + "Ñ";
                    fatenye += 1;
                }
                else
                {
                    txtFathName.Text = "Ñ";
                    fatenye += 1;
                }
            }
            else
            {
                if (txtFathName.Text != "")
                {
                    txtFathName.Text = orgtext + "ñ";
                    fatenye -= 1;
                }
                else
                {
                    txtFathName.Text = "ñ";
                    fatenye -= 1;
                }
            }

            txtFathName.Focus();
            txtFathName.SelectionStart = txtFathName.Text.Length;
        }

        private void btnNMot_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtMothName.Text != "")
            {
                if (txtMothName.TextLength == 1)
                {
                    string last = txtMothName.Text.Substring(0, txtMothName.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtMothName.Text.Substring(0, txtMothName.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtMothName.Text;
                    }

                }
                else
                {
                    string last = txtMothName.Text.Substring(txtMothName.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtMothName.Text.Substring(0, txtMothName.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtMothName.Text.Substring(0, txtMothName.TextLength);
                    }
                }
            }

            if (motenye == 1)
            {
                if (txtMothName.Text != "")
                {
                    txtMothName.Text = orgtext + "Ñ";
                    motenye += 1;
                }
                else
                {
                    txtMothName.Text = "Ñ";
                    motenye += 1;
                }
            }
            else
            {
                if (txtMothName.Text != "")
                {
                    txtMothName.Text = orgtext + "ñ";
                    motenye -= 1;
                }
                else
                {
                    txtMothName.Text = "ñ";
                    motenye -= 1;
                }
            }

            txtMothName.Focus();
            txtMothName.SelectionStart = txtMothName.Text.Length;
        }

        private void btnNGua_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtGrdName.Text != "")
            {
                if (txtGrdName.TextLength == 1)
                {
                    string last = txtGrdName.Text.Substring(0, txtGrdName.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtGrdName.Text.Substring(0, txtGrdName.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtGrdName.Text;
                    }

                }
                else
                {
                    string last = txtGrdName.Text.Substring(txtGrdName.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtGrdName.Text.Substring(0, txtGrdName.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtGrdName.Text.Substring(0, txtGrdName.TextLength);
                    }
                }
            }

            if (guaenye == 1)
            {
                if (txtGrdName.Text != "")
                {
                    txtGrdName.Text = orgtext + "Ñ";
                    guaenye += 1;
                }
                else
                {
                    txtGrdName.Text = "Ñ";
                    guaenye += 1;
                }
            }
            else
            {
                if (txtGrdName.Text != "")
                {
                    txtGrdName.Text = orgtext + "ñ";
                    guaenye -= 1;
                }
                else
                {
                    txtGrdName.Text = "ñ";
                    guaenye -= 1;
                }
            }

            txtGrdName.Focus();
            txtGrdName.SelectionStart = txtGrdName.Text.Length;
        }
    }
}
