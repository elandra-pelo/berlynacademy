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
    public partial class frmFaculty : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public DataView dvFaculty;
        public string facmlog,primarykey,specializationcode,VISITED;
        public int lstEnye = 1, fnmEnye = 1, mnmEnye = 1, fatenye = 1, motenye = 1, emerenye = 1,spoenye=1;
        public frmFaculty()
        {
            InitializeComponent();
        }

        private void frmFaculty_Load(object sender, EventArgs e)
        {
            lblLogger.Text = facmlog;
            lblLoggerPosition.Text = "Admin";

            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            lvwSpec.Columns.Add("",354,HorizontalAlignment.Left);
            btnFaculty.BackColor = Color.LightGreen;
            setupdays_forFaculty();
            setupyears_forFaculty();
            setupSubjects();
            setupview_forFaculty();
            setupdisableinput_forFaculty();
            setupDept();

            if (VISITED.Contains("Faculty") == false)
            {
                VISITED += "   Faculty";
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
                cmbAssDept.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbAssDept.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        public void setupSubjects()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select distinct subject from facultyspecialization_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                cmbSubSpec.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSubSpec.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        public void setupview_forFaculty()
        {
            dgvSearch.DataSource = null;

            con.Open();
            OdbcDataAdapter daFaculty = new OdbcDataAdapter("Select empno,lastname as 'Lastname',firstname as 'Firstname',middlename as 'Middlename'from employees_tbl where position='" + "faculty" + "' order by lastname ASC", con);
            DataTable dtfaculty = new DataTable();
            daFaculty.Fill(dtfaculty);
            con.Close();
            dvFaculty = new DataView(dtfaculty);
            dgvSearch.DataSource = dvFaculty;

            dgvSearch.Columns[0].Width = 0;
            dgvSearch.Columns[1].Width = 115;
            dgvSearch.Columns[2].Width = 115;
            dgvSearch.Columns[3].Width = 115;
            dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
           
            lblResult.Text = "number of faculty: " + dgvSearch.Rows.Count.ToString();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance user = new frmMaintenance();
            this.Hide();
            user.adminlog = facmlog;
            user.VISITED = VISITED;
            user.Show();
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = facmlog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subjmaintenance = new frmSubject();
            subjmaintenance.wholog = facmlog;
            subjmaintenance.VISITED = VISITED;
            subjmaintenance.Show();
            this.Hide();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Hide();
            levmain.levlog = facmlog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            frmSection section = new frmSection();
            section.secwholog = facmlog;
            section.VISITED = VISITED;
            section.Show();
            this.Hide();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roommaintenance = new frmRoom();
            roommaintenance.logger = facmlog;
            roommaintenance.VISITED = VISITED;
            roommaintenance.Show();
            this.Hide();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            return;
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

        private void frmFaculty_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = facmlog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched schedf = new frmSched();
            this.Hide();
            schedf.schedlog = facmlog;
            schedf.VISITED = VISITED;
            schedf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqf = new frmRequirement();
            this.Hide();
            reqf.reqlog = facmlog;
            reqf.VISITED = VISITED;
            reqf.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feef = new frmFee();
            this.Hide();
            feef.feelog = facmlog;
            feef.VISITED = VISITED;
            feef.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Hide();
            discform.disclog = facmlog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void txtFacCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 36)
            {
                e.Handled = true;
            }
        }

        private void txtEmergencyCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 36)
            {
                e.Handled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvFaculty.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvFaculty;

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

        public void computeAge()
        {
            if (cmbFacMonth.Text != "" && cmbFacDay.Text != "" && cmbFacYear.Text != "")
            {
                int current = Convert.ToInt32(DateTime.Now.Year);
                int birth = Convert.ToInt32(cmbFacYear.Text);
                int age = current - birth;
                txtFacAge.Text = age.ToString();
            }
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }


            uncheckSubject();
            setupdisableinput_forFaculty();
            btnFacUpdate.Text = "Update";
            btnFacClear.Text = "Clear";
            btnFacUpdate.Enabled = true;
           
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
            {
                primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }
            
            setupretrieveddata_forFaculty(primarykey);
        }

        public void setupdisableinput_forFaculty()
        {
            btnN1.Enabled = false;
            btnN2.Enabled = false;
            btnN3.Enabled = false;
            btnNFat.Enabled = false;
            btnNMot.Enabled = false;
            btnNGua.Enabled = false;
            btnNSpouse.Enabled = false;
            txtFacFname.Enabled = false;
            txtFacMidl.Enabled = false;
            txtFacLast.Enabled = false;
            txtFacAdd.Enabled = false;
            txtFacCon.Enabled = false;
            txtFacEmail.Enabled = false;
            cmbFacMonth.Enabled = false;
            cmbFacDay.Enabled = false;
            cmbFacYear.Enabled = false;
            cmbFacGen.Enabled = false;
            txtFathName.Enabled = false;
            txtMothName.Enabled = false;
            txtSpouse.Enabled = false;
            txtSpouseOcc.Enabled = false;
            txtEmergency.Enabled = false;
            txtEmerRelation.Enabled = false;
            txtEmergencyCon.Enabled = false;
            txtFacGrad.Enabled = false;
            txtProgram.Enabled = false;
            cmbDegree.Enabled = false;
            cmbFacCivil.Enabled = false;
            pnlSubs.Enabled = false;
            lvwSpec.Enabled = false;
            cmbSubSpec.Enabled = false;
            cmbAssDept.Enabled = false;
        }

        public void setupenableinput_forFaculty()
        {
            btnN1.Enabled = true;
            btnN2.Enabled = true;
            btnN3.Enabled = true;
            btnNFat.Enabled = true;
            btnNMot.Enabled = true;
            btnNGua.Enabled = true;
            btnNSpouse.Enabled = true;
            txtFacFname.Enabled = true;
            txtFacMidl.Enabled = true;
            txtFacLast.Enabled = true;
            txtFacAdd.Enabled = true;
            txtFacCon.Enabled = true;
            txtFacEmail.Enabled = true;
            cmbFacMonth.Enabled = true;
            cmbFacDay.Enabled = true;
            cmbFacYear.Enabled = true;
            cmbFacGen.Enabled = true;
            txtFathName.Enabled = true;
            txtMothName.Enabled = true;
            txtSpouse.Enabled = true;
            txtSpouseOcc.Enabled = true;
            txtEmergency.Enabled = true;
            txtEmerRelation.Enabled = true;
            txtEmergencyCon.Enabled = true;
            txtFacGrad.Enabled = true;
            txtProgram.Enabled = true;
            cmbDegree.Enabled = true;
            cmbFacCivil.Enabled = true;
            pnlSubs.Enabled = true;
            lvwSpec.Enabled = true;
            cmbSubSpec.Enabled = true;
            cmbAssDept.Enabled = true;
        }

        public void setupretrieveddata_forFaculty(string thekey)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno='" + thekey + "' and position='"+"faculty"+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                setupSubjects();
                lvwSpec.Items.Clear();
                txtFacFname.Text = dt.Rows[0].ItemArray[1].ToString();
                txtFacMidl.Text = dt.Rows[0].ItemArray[2].ToString();
                txtFacLast.Text = dt.Rows[0].ItemArray[3].ToString();
                txtFacAdd.Text = dt.Rows[0].ItemArray[4].ToString();
                cmbFacCivil.Text = dt.Rows[0].ItemArray[8].ToString();
                txtFacCon.Text = dt.Rows[0].ItemArray[9].ToString();
                txtFacEmail.Text = dt.Rows[0].ItemArray[10].ToString();
                if (dt.Rows[0].ItemArray[5].ToString() != "")
                {
                    cmbFacMonth.Text = dt.Rows[0].ItemArray[5].ToString().Substring(0, 3).ToString();//0 start of string 3 the length 
                    cmbFacDay.Text = dt.Rows[0].ItemArray[5].ToString().Substring(4, 2).ToString();
                    cmbFacYear.Text = dt.Rows[0].ItemArray[5].ToString().Substring(7, 4).ToString();
                    txtFacAge.Text = dt.Rows[0].ItemArray[6].ToString();
                }
                cmbFacGen.Text = dt.Rows[0].ItemArray[7].ToString();
                txtFacGrad.Text = dt.Rows[0].ItemArray[11].ToString();
                cmbDegree.Text = dt.Rows[0].ItemArray[18].ToString();
                txtFathName.Text = dt.Rows[0].ItemArray[19].ToString();
                txtMothName.Text = dt.Rows[0].ItemArray[20].ToString();
                txtSpouse.Text = dt.Rows[0].ItemArray[21].ToString();
                txtSpouseOcc.Text = dt.Rows[0].ItemArray[22].ToString();
                txtEmergency.Text = dt.Rows[0].ItemArray[23].ToString();
                txtEmerRelation.Text = dt.Rows[0].ItemArray[24].ToString();
                txtEmergencyCon.Text = dt.Rows[0].ItemArray[25].ToString();
                txtProgram.Text = dt.Rows[0].ItemArray[26].ToString();
                cmbAssDept.Text = dt.Rows[0].ItemArray[27].ToString();
                string speccode = dt.Rows[0].ItemArray[13].ToString();


                for (int i = 0; i < speccode.Length; i++)
                {
                    string id = speccode.Substring(i, 1);
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from facultyspecialization_tbl where id='" + id + "'", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {
                        ListViewItem itmspec = new ListViewItem();
                        itmspec.Text = dt1.Rows[0].ItemArray[1].ToString();
                        lvwSpec.Items.Add(itmspec);
                    }
                }
                /*if (speccode.Contains("1") == true)
                {
                    chkEng.Checked = true;
                }
                if (speccode.Contains("2") == true)
                {
                    chkMat.Checked = true;
                }
                if (speccode.Contains("3") == true)
                {
                    chkSci.Checked = true;
                }
                if (speccode.Contains("4") == true)
                {
                    chkFil.Checked = true;
                }
                if (speccode.Contains("5") == true)
                {
                    chkAP.Checked = true;
                }
                if (speccode.Contains("6") == true)
                {
                    chkESP.Checked = true;
                }
                if (speccode.Contains("7") == true)
                {
                    chkMAP.Checked = true;
                }
                if (speccode.Contains("8") == true)
                {
                    chkMot.Checked = true;
                }
                if (speccode.Contains("9") == true)
                {
                    chkTLE.Checked = true;
                }
                if (speccode.Contains("0") == true)
                {
                    chkHEL.Checked = true;
                }*/

               
            }
        }

        public void setupyears_forFaculty()
        {
            int start = 1970;
            int current = Convert.ToInt32(DateTime.Now.Year);

            while (current >= start)
            {
                cmbFacYear.Items.Add(current);
                current--;
            }
        }

        public void setupdays_forFaculty()
        {
            cmbFacDay.Items.Clear();

            int start = 1;
            while (start <= 31)
            {
                if (start < 10)
                {
                    cmbFacDay.Items.Add("0" + start);
                }
                else
                {
                    cmbFacDay.Items.Add(start);
                }
                start++;
            }
        }

        private void btnFacClear_Click(object sender, EventArgs e)
        {
            if (btnFacClear.Text == "Clear")
            {
                setupclear_forFaculty();
                setupdisableinput_forFaculty();
                btnFacUpdate.Enabled = false;
                btnFacUpdate.Text = "Update";
            }
            else
            {
                btnFacClear.Text = "Clear";
                btnFacUpdate.Text = "Update";

                setupretrieveddata_forFaculty(primarykey);
                setupdisableinput_forFaculty();
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
  
        }

        public void setupclear_forFaculty()
        {
            txtFacFname.Clear();
            txtFacMidl.Clear();
            txtFacLast.Clear();
            txtFacAdd.Clear();
            txtFacAge.Clear();
            txtFacCon.Clear();
            txtFacEmail.Clear();
            txtFathName.Clear();
            txtMothName.Clear();
            txtSpouse.Clear();
            txtSpouseOcc.Clear();
            txtEmergency.Clear();
            txtEmergencyCon.Clear();
            txtEmerRelation.Clear();
            txtFacGrad.Clear();
            txtProgram.Clear();

            cmbFacMonth.SelectedIndex = -1;
            cmbFacDay.SelectedIndex = -1;
            cmbFacYear.SelectedIndex = -1;
            cmbFacGen.SelectedIndex = -1;
            cmbFacCivil.SelectedIndex = -1;
            cmbDegree.SelectedIndex = -1;
            lvwSpec.Items.Clear();


            uncheckSubject();
            setupSubjects();
            setupDept();
            setupenableinput_forFaculty();
        }

        public void uncheckSubject()
        {
            chkAP.Checked = false;
            chkEng.Checked = false;
            chkESP.Checked = false;
            chkFil.Checked = false;
            chkHEL.Checked = false;
            chkMAP.Checked = false;
            chkMat.Checked = false;
            chkMot.Checked = false;
            chkSci.Checked = false;
            chkTLE.Checked = false;
        }

        private void btnFacUpdate_Click(object sender, EventArgs e)
        {
            if (btnFacUpdate.Text == "Update")
            {
                setupenableinput_forFaculty();
                btnFacUpdate.Text = "Save";
                btnFacClear.Text = "Cancel";
            }
            else
            {
                if (txtFacFname.Text == "" || txtFacLast.Text == "" || cmbFacMonth.Text == "" || cmbFacDay.Text == "" ||
               cmbFacYear.Text == "" || cmbFacGen.Text == "" || cmbFacCivil.Text=="" || txtEmergency.Text=="" || txtEmerRelation.Text=="" || 
                    txtEmergencyCon.Text=="" || cmbDegree.Text=="" || txtFacGrad.Text=="" || txtProgram.Text=="" || cmbAssDept.Text=="")
                {
                    inputcheck("valid", txtEmergencyCon);
                    MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtFacAge.Text != "")
                {

                    int age = Convert.ToInt32(txtFacAge.Text);
                    if (age < 18)
                    {
                        MessageBox.Show("faculty too young", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
                if (cmbFacDay.Text != "")
                {
                    if (cmbFacMonth.Text == "Feb" && Convert.ToInt32(cmbFacDay.Text) > 28)
                    {
                        MessageBox.Show("day of birth is out of range.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (txtFacMidl.Text != "")
                {
                    if (txtFacMidl.TextLength < 2)
                    {
                        inputcheck("invalid", txtFacMidl);
                    }
                    else
                    {
                        inputcheck("valid", txtFacMidl);
                    }
                }
                if (txtFacCon.Text != "")
                {
                    if ((txtFacCon.TextLength ==11) && (txtFacCon.Text.Substring(0, 2) != "09"))
                    {
                        inputcheck("invalid", txtFacCon);
                    }
                    else if ((txtFacCon.TextLength != 11) && (txtFacCon.TextLength != 7))
                    {
                        inputcheck("invalid", txtFacCon);
                    }
                    else
                    {
                        inputcheck("valid", txtFacCon);
                    }
                }
                if (txtEmergencyCon.Text != "")
                {
                    if ((txtEmergencyCon.TextLength ==11) && (txtEmergencyCon.Text.Substring(0, 2) != "09"))
                    {
                        inputcheck("invalid", txtEmergencyCon);
                    }
                    else if ((txtEmergencyCon.TextLength != 11) && (txtEmergencyCon.TextLength != 7))
                    {
                        inputcheck("invalid", txtEmergencyCon);
                    }
                    else
                    {
                        inputcheck("valid", txtEmergencyCon);
                    }
                }
                if (txtFacEmail.Text != "")
                {
                    if ((txtFacEmail.Text.Contains("@") == false) || (txtFacEmail.Text.Contains(".") == false))
                    {
                        inputcheck("invalid", txtFacEmail);
                    }
                    else
                    {
                        inputcheck("valid", txtFacEmail);
                    }
                }
                if (txtFacEmail.Text == "")
                {
                    inputcheck("valid", txtFacEmail);
                }
                if (txtFacMidl.Text == "")
                {
                    inputcheck("valid", txtFacMidl);
                }
                if (txtFacCon.Text == "")
                {
                    inputcheck("valid", txtFacCon);
                }

                if ((((txtEmergencyCon.TextLength == 11) && (txtEmergencyCon.Text.Substring(0, 2) == "09")) || (txtEmergencyCon.TextLength == 7)))
                {
                    if (txtFacMidl.Text != "")
                    {
                        if (txtFacMidl.TextLength < 2)
                        {
                            return;
                        }
                    }
                    if (txtFacCon.Text != "")
                    {
                        if (((txtFacCon.TextLength == 11) && (txtFacCon.Text.Substring(0, 2) != "09")) || ((txtFacCon.TextLength != 11) && (txtFacCon.TextLength != 7)))
                        {
                            return;
                        }
                    }
                    if (txtFacEmail.Text != "")
                    {
                        if ((txtFacEmail.Text.Contains("@") == false) || (txtFacEmail.Text.Contains(".") == false))
                        {
                            return;
                        }
                    }

                    setupsaveoperation_forFaculty();
                    btnFacUpdate.Text = "Update";
                    btnFacUpdate.Enabled = false;

                    if (dgvSearch.Rows.Count >= 1)
                    {
                        dgvSearch.Rows[0].Selected = true;
                    }
                }
            }
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

        public void setupSpecializationCode()
        {
            specializationcode = "";
            for (int i = 0; i < lvwSpec.Items.Count; i++)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select id from facultyspecialization_tbl where subject='" + lvwSpec.Items[i].Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    specializationcode += dt.Rows[0].ItemArray[0].ToString();
                }
            }
            /*if (chkEng.Checked == true)
            {
                specializationcode += "1";
            }
            if (chkMat.Checked == true)
            {
                specializationcode+= "2";
            }
            if (chkSci.Checked == true)
            {
                specializationcode += "3";
            }
            if (chkFil.Checked == true)
            {
                specializationcode += "4";
            }
            if (chkAP.Checked == true)
            {
                specializationcode += "5";
            }
            if (chkESP.Checked == true)
            {
                specializationcode += "6";
            }
            if (chkMAP.Checked == true)
            {
                specializationcode += "7";
            }
            if (chkMot.Checked == true)
            {
                specializationcode += "8";
            }
            if (chkTLE.Checked == true)
            {
                specializationcode += "9";
            }
            if (chkHEL.Checked == true)
            {
                specializationcode += "0";
            }*/
        }

        public void setupsaveoperation_forFaculty()
        {
           
            setupSpecializationCode();
           
            string bday = cmbFacMonth.Text + " " + cmbFacDay.Text + " " + cmbFacYear.Text;
            int current = Convert.ToInt32(DateTime.Now.Year);
            int birthyear = Convert.ToInt32(cmbFacYear.Text);
            int age = current - birthyear;

            con.Open();
            string updateFaculty2 = "Update employees_tbl set firstname='" + txtFacFname.Text + "',middlename='" + txtFacMidl.Text + "',lastname='" +
                txtFacLast.Text + "',birthdate='" + bday + "',address='"+txtFacAdd.Text+"',age='" + age + "',gender='" +
                cmbFacGen.Text + "',civilstatus='"+cmbFacCivil.Text+"',contactnum='"+txtFacCon.Text+"',email='"+txtFacEmail.Text+"',schoolgrad='"+
                txtFacGrad.Text + "',position='" + "faculty" + "',subject='"+specializationcode+"',educattainment='" + cmbDegree.Text + "',fathername='" + txtFathName.Text + "',mothername='" +
                txtMothName.Text+"',spousename='"+txtSpouse.Text+"',spouseocc='"+txtSpouseOcc.Text+"',emergencyperson='"+
                txtEmergency.Text+"',emergencyrelation='"+txtEmerRelation.Text+"',emergencycon='"+txtEmergencyCon.Text+"',program='"+
                txtProgram.Text+"',department='"+cmbAssDept.Text+"'where empno='" + primarykey + "'";
            OdbcCommand cmdUpdateFaculty2 = new OdbcCommand(updateFaculty2, con);
            cmdUpdateFaculty2.ExecuteNonQuery();
            con.Close();

           
            setupview_forFaculty();
            btnFacClear.Text = "Clear";
            setupdisableinput_forFaculty();
            MessageBox.Show("record successfully updated", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtSearch.Focus();
        }

        private void cmbFacYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void cmbFacDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void cmbFacMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = facmlog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = facmlog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void cmbSubSpec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwSpec.Items.Count == 0)
            {
                ListViewItem itmspec = new ListViewItem();
                itmspec.Text = cmbSubSpec.Text;
                lvwSpec.Items.Add(cmbSubSpec.Text);
            }
            else
            {
                for (int g = 0; g < lvwSpec.Items.Count; g++)
                {
                    if (cmbSubSpec.Text == lvwSpec.Items[g].Text)
                    {
                        return;
                    }
                    else
                    {
                        if (g == lvwSpec.Items.Count - 1)
                        {
                            ListViewItem itmspec = new ListViewItem();
                            itmspec.Text = cmbSubSpec.Text;
                            lvwSpec.Items.Add(cmbSubSpec.Text);
                        }
                    }
                }
            }
        }

        private void lvwSpec_Click(object sender, EventArgs e)
        {
            lvwSpec.SelectedItems[0].Remove();
        }

        private void btnN1_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtFacFname.Text != "")
            {
                if (txtFacFname.TextLength == 1)
                {
                    string last = txtFacFname.Text.Substring(0, txtFacFname.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFacFname.Text.Substring(0, txtFacFname.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFacFname.Text;
                    }

                }
                else
                {
                    string last = txtFacFname.Text.Substring(txtFacFname.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFacFname.Text.Substring(0, txtFacFname.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFacFname.Text.Substring(0, txtFacFname.TextLength);
                    }
                }
            }

            if (fnmEnye == 1)
            {
                if (txtFacFname.Text != "")
                {
                    txtFacFname.Text = orgtext + "Ñ";
                    fnmEnye += 1;
                }
                else
                {
                    txtFacFname.Text = "Ñ";
                    fnmEnye += 1;
                }
            }
            else
            {
                if (txtFacFname.Text != "")
                {
                    txtFacFname.Text = orgtext + "ñ";
                    fnmEnye -= 1;
                }
                else
                {
                    txtFacFname.Text = "ñ";
                    fnmEnye -= 1;
                }
            }

            txtFacFname.Focus();
            txtFacFname.SelectionStart = txtFacFname.Text.Length;
        }

        private void btnN2_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtFacMidl.Text != "")
            {
                if (txtFacMidl.TextLength == 1)
                {
                    string last = txtFacMidl.Text.Substring(0, txtFacMidl.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFacMidl.Text.Substring(0, txtFacMidl.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFacMidl.Text;
                    }

                }
                else
                {
                    string last = txtFacMidl.Text.Substring(txtFacMidl.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFacMidl.Text.Substring(0, txtFacMidl.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFacMidl.Text.Substring(0, txtFacMidl.TextLength);
                    }
                }
            }

            if (mnmEnye == 1)
            {
                if (txtFacMidl.Text != "")
                {
                    txtFacMidl.Text = orgtext + "Ñ";
                    mnmEnye += 1;
                }
                else
                {
                    txtFacMidl.Text = "Ñ";
                    mnmEnye += 1;
                }
            }
            else
            {
                if (txtFacMidl.Text != "")
                {
                    txtFacMidl.Text = orgtext + "ñ";
                    mnmEnye -= 1;
                }
                else
                {
                    txtFacMidl.Text = "ñ";
                    mnmEnye -= 1;
                }
            }

            txtFacMidl.Focus();
            txtFacMidl.SelectionStart = txtFacMidl.Text.Length;
        }

        private void btnN3_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtFacLast.Text != "")
            {
                if (txtFacLast.TextLength == 1)
                {
                    string last = txtFacLast.Text.Substring(0, txtFacLast.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFacLast.Text.Substring(0, txtFacLast.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFacLast.Text;
                    }

                }
                else
                {
                    string last = txtFacLast.Text.Substring(txtFacLast.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFacLast.Text.Substring(0, txtFacLast.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFacLast.Text.Substring(0, txtFacLast.TextLength);
                    }
                }
            }

            if (lstEnye == 1)
            {
                if (txtFacLast.Text != "")
                {
                    txtFacLast.Text = orgtext + "Ñ";
                    lstEnye += 1;
                }
                else
                {
                    txtFacLast.Text = "Ñ";
                    lstEnye += 1;
                }
            }
            else
            {
                if (txtFacLast.Text != "")
                {
                    txtFacLast.Text = orgtext + "ñ";
                    lstEnye -= 1;
                }
                else
                {
                    txtFacLast.Text = "ñ";
                    lstEnye -= 1;
                }
            }

            txtFacLast.Focus();
            txtFacLast.SelectionStart = txtFacLast.Text.Length;
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = facmlog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void cmbFacCivil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFacCivil.Text == "Single")
            {
                txtSpouse.Enabled = false;
                txtSpouseOcc.Enabled = false;
            }
            else
            {
                txtSpouse.Enabled = true;
                txtSpouseOcc.Enabled = true;
            }
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
            if (txtEmergency.Text != "")
            {
                if (txtEmergency.TextLength == 1)
                {
                    string last = txtEmergency.Text.Substring(0, txtEmergency.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtEmergency.Text.Substring(0, txtEmergency.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtEmergency.Text;
                    }

                }
                else
                {
                    string last = txtEmergency.Text.Substring(txtEmergency.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtEmergency.Text.Substring(0, txtEmergency.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtEmergency.Text.Substring(0, txtEmergency.TextLength);
                    }
                }
            }

            if (emerenye == 1)
            {
                if (txtEmergency.Text != "")
                {
                    txtEmergency.Text = orgtext + "Ñ";
                    emerenye += 1;
                }
                else
                {
                    txtEmergency.Text = "Ñ";
                    emerenye += 1;
                }
            }
            else
            {
                if (txtEmergency.Text != "")
                {
                    txtEmergency.Text = orgtext + "ñ";
                    emerenye -= 1;
                }
                else
                {
                    txtEmergency.Text = "ñ";
                    emerenye -= 1;
                }
            }

            txtEmergency.Focus();
            txtEmergency.SelectionStart = txtEmergency.Text.Length;
        }

        private void btnNSpouse_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtSpouse.Text != "")
            {
                if (txtSpouse.TextLength == 1)
                {
                    string last = txtSpouse.Text.Substring(0, txtSpouse.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtSpouse.Text.Substring(0, txtSpouse.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtSpouse.Text;
                    }

                }
                else
                {
                    string last = txtSpouse.Text.Substring(txtSpouse.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtSpouse.Text.Substring(0, txtSpouse.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtSpouse.Text.Substring(0, txtSpouse.TextLength);
                    }
                }
            }

            if (spoenye == 1)
            {
                if (txtSpouse.Text != "")
                {
                    txtSpouse.Text = orgtext + "Ñ";
                    spoenye += 1;
                }
                else
                {
                    txtSpouse.Text = "Ñ";
                    spoenye += 1;
                }
            }
            else
            {
                if (txtSpouse.Text != "")
                {
                    txtSpouse.Text = orgtext + "ñ";
                    spoenye -= 1;
                }
                else
                {
                    txtSpouse.Text = "ñ";
                    spoenye -= 1;
                }
            }

            txtSpouse.Focus();
            txtSpouse.SelectionStart = txtSpouse.Text.Length;
        }
    }
}
