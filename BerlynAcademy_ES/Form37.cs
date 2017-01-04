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
    public partial class frmStaff : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public DataView dvstf;
        public string stflog,primarykey,VISITED;
        public int lstEnye = 1, fnmEnye = 1, mnmEnye = 1, fatenye = 1, motenye = 1, emerenye = 1, spoenye = 1;
        public frmStaff()
        {
            InitializeComponent();
        }

        private void frmStaff_Load(object sender, EventArgs e)
        {
            lblLogger.Text = stflog;
            lblLoggerPosition.Text = "Admin";

            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //pnlvert.BackColor = Color.FromArgb(0, 0, 25);
            btnStaff.BackColor = Color.LightGreen;
            cmbFilter.Text = "All staff";

            setupdays_forstf();
            setupyears_forstf();
            setupview_forStaff();
            setupdisableinput_forStaff();

            if (VISITED.Contains("Staff") == false)
            {
                VISITED += "   Staff";
            }
        }

        public void setupyears_forstf()
        {
            int start = 1970;
            int current = Convert.ToInt32(DateTime.Now.Year);

            while (current >= start)
            {
                cmbYear.Items.Add(current);
                current--;
            }
        }

        public void setupdays_forstf()
        {
            cmbDay.Items.Clear();

            int start = 1;
            while (start <= 31)
            {
                if (start < 10)
                {
                    cmbDay.Items.Add("0" + start);
                }
                else
                {
                    cmbDay.Items.Add(start);
                }
                start++;
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

        public void setupview_forStaff()
        {
            dgvSearch.DataSource = null;

            con.Open();
            OdbcDataAdapter dastf = new OdbcDataAdapter("Select empno,lastname as 'Lastname',firstname as 'Firstname',middlename as 'Middlename' from employees_tbl where position<>'" + "faculty" + "' order by lastname ASC", con);
            DataTable dtstf = new DataTable();
            dastf.Fill(dtstf);
            con.Close();
            dvstf = new DataView(dtstf);
            dgvSearch.DataSource = dvstf;

            dgvSearch.Columns[0].Width = 0;
            dgvSearch.Columns[1].Width =115;
            dgvSearch.Columns[2].Width = 115;
            dgvSearch.Columns[3].Width = 115;
            dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
            lblResult.Text = "number of staff: " + dgvSearch.Rows.Count.ToString();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            return;
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }

           
            setupdisableinput_forStaff();
            btnFacUpdate.Enabled = true;
            btnFacUpdate.Text = "Update";
            btnClear.Text = "Clear";
           
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
            {
                primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }
           
            setupretrieveddata_forStaff(primarykey);
        }

        public void setupretrieveddata_forStaff(string thekey)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno='" + thekey + "' and position<>'" + "faculty" + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {

                txtFname.Text = dt.Rows[0].ItemArray[1].ToString();
                txtMidl.Text = dt.Rows[0].ItemArray[2].ToString();
                txtLast.Text = dt.Rows[0].ItemArray[3].ToString();
                txtAdd.Text = dt.Rows[0].ItemArray[4].ToString();
                cmbCivil.Text = dt.Rows[0].ItemArray[8].ToString();
                txtCon.Text = dt.Rows[0].ItemArray[9].ToString();
                txtEmail.Text = dt.Rows[0].ItemArray[10].ToString();
                if (dt.Rows[0].ItemArray[5].ToString() != "")
                {
                    cmbMonth.Text = dt.Rows[0].ItemArray[5].ToString().Substring(0, 3).ToString();//0 start of string 3 the length 
                    cmbDay.Text = dt.Rows[0].ItemArray[5].ToString().Substring(4, 2).ToString();
                    cmbYear.Text = dt.Rows[0].ItemArray[5].ToString().Substring(7, 4).ToString();
                    txtAge.Text = dt.Rows[0].ItemArray[6].ToString();
                }
                cmbGen.Text = dt.Rows[0].ItemArray[7].ToString();
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
                if (dt.Rows[0].ItemArray[28].ToString() == "present")
                {
                    chkSetPrincipal.Checked = true;
                }
                else
                {
                    chkSetPrincipal.Checked = false;
                }

                if (cmbFilter.Text == "All staff")
                {
                    lblThePosi.Visible = true; lblPosi.Visible = true;
                    lblThePosi.Text = dt.Rows[0].ItemArray[12].ToString();
                }
                else
                {
                    lblPosi.Visible = false; lblThePosi.Visible = false;
                }

                if (lblThePosi.Text == "principal")
                {
                    pnlPrin.Visible = true;
                }
                else
                {
                    pnlPrin.Visible = false;
                }
     
            }
        }

        public void setupdisableinput_forStaff()
        {
            btnN1.Enabled = false;
            btnN2.Enabled = false;
            btnN3.Enabled = false;
            btnNFat.Enabled = false;
            btnNMot.Enabled = false;
            btnNGua.Enabled = false;
            btnNSpouse.Enabled = false;
            txtFname.Enabled = false;
            txtMidl.Enabled = false;
            txtLast.Enabled = false;
            txtAdd.Enabled = false;
            txtCon.Enabled = false;
            txtEmail.Enabled = false;
            cmbMonth.Enabled = false;
            cmbDay.Enabled = false;
            cmbYear.Enabled = false;
            cmbGen.Enabled = false;
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
            cmbCivil.Enabled = false;
            pnlPrin.Enabled = false;
        }

        public void setupenableinput_forStaff()
        {
            btnN1.Enabled = true;
            btnN2.Enabled = true;
            btnN3.Enabled = true;
            btnNFat.Enabled = true;
            btnNMot.Enabled = true;
            btnNGua.Enabled = true;
            btnNSpouse.Enabled = true;
            txtFname.Enabled = true;
            txtMidl.Enabled = true;
            txtLast.Enabled = true;
            txtAdd.Enabled = true;
            txtCon.Enabled = true;
            txtEmail.Enabled = true;
            cmbMonth.Enabled = true;
            cmbDay.Enabled = true;
            cmbYear.Enabled = true;
            cmbGen.Enabled = true;
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
            cmbCivil.Enabled = true;
            pnlPrin.Enabled = true;
            
        }

        public void setupclear()
        {
            txtFname.Clear();
            txtMidl.Clear();
            txtLast.Clear();
            txtAdd.Clear();
            txtCon.Clear();
            txtEmail.Clear();
            txtAge.Clear();
            cmbMonth.SelectedIndex = -1;
            cmbDay.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            cmbGen.SelectedIndex = -1;
            txtFathName.Clear();
            txtMothName.Clear();
            txtSpouse.Clear();
            txtSpouseOcc.Clear();
            txtEmergency.Clear();
            txtEmerRelation.Clear();
            txtEmergencyCon.Clear();
            txtFacGrad.Clear();
            txtProgram.Clear();
            cmbDegree.SelectedIndex = -1;
            cmbCivil.SelectedIndex = -1;
            chkSetPrincipal.Checked = false;
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

        private void btnFacUpdate_Click(object sender, EventArgs e)
        {
            if (btnFacUpdate.Text == "Update")
            {
                setupenableinput_forStaff();
                btnFacUpdate.Text = "Save";
                btnClear.Text = "Cancel";
            }
            else
            {
                if (txtFname.Text == "" || txtLast.Text == "" || cmbMonth.Text == "" || cmbDay.Text == "" ||
               cmbYear.Text == "" || cmbGen.Text == "" || cmbCivil.Text == "" || txtEmergency.Text == "" || txtEmerRelation.Text == "" ||
                    txtEmergencyCon.Text == "" || cmbDegree.Text == "" || txtFacGrad.Text == "" || txtProgram.Text == "")
                {
                    inputcheck("valid", txtEmergencyCon);
                    MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtAge.Text != "")
                {
                    int age = Convert.ToInt32(txtAge.Text);
                    if (age < 18)
                    {
                        MessageBox.Show("staff too young.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
                 if (cmbDay.Text != "")
                {
                    if (cmbMonth.Text == "Feb" && Convert.ToInt32(cmbDay.Text) > 28)
                    {
                        MessageBox.Show("day of birth is out of range.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
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
                    if ((txtCon.TextLength ==11) && (txtCon.Text.Substring(0, 2) != "09"))
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
                if (txtEmail.Text != "")
                {
                    if ((txtEmail.Text.Contains("@") == false) || (txtEmail.Text.Contains(".") == false))
                    {
                        inputcheck("invalid", txtEmail);
                        
                    }
                    else
                    {
                        inputcheck("valid", txtEmail);
                    }
                }
                if (txtEmail.Text == "")
                {
                    inputcheck("valid", txtEmail);
                }
                if (txtMidl.Text == "")
                {
                    inputcheck("valid", txtMidl);
                }
                if (txtCon.Text == "")
                {
                    inputcheck("valid", txtCon);
                }

                if ((((txtEmergencyCon.TextLength == 11) && (txtEmergencyCon.Text.Substring(0, 2) == "09")) || (txtEmergencyCon.TextLength ==7 )))
                {
                    if (txtMidl.Text != "")
                    {
                        if (txtMidl.TextLength < 2)
                        {
                            return;
                        }
                    }
                    if (txtCon.Text != "")
                    {
                        if (((txtCon.TextLength == 11) && (txtCon.Text.Substring(0, 2) != "09")) || ((txtCon.TextLength != 11) && (txtCon.TextLength != 7)))
                        {
                            return;
                        }
                    }
                    if (txtEmail.Text != "")
                    {
                        if ((txtEmail.Text.Contains("@") == false) || (txtEmail.Text.Contains(".") == false))
                        {
                            return;
                        }
                    }

                    setupsaveoperation_forStaff();
                   
                }

            }
        }

        public void setupsaveoperation_forStaff()
        {
            if (lblThePosi.Text == "principal" || cmbFilter.Text == "Principal")
            {
                if (chkSetPrincipal.Checked == true)
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where principalstatus='" + "present" + "'and empno<>'" + primarykey + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("operation could not perform." + "\nthere is other principal is set to current" + "\nremove its status to update.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        con.Open();
                        string update = "Update employees_tbl set principalstatus='present'where empno='" + primarykey + "'";
                        OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                        cmdUpdate.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else
                {
                    con.Open();
                    string update = "Update employees_tbl set principalstatus=''where empno='" + primarykey + "'";
                    OdbcCommand cmdUpdate = new OdbcCommand(update, con);
                    cmdUpdate.ExecuteNonQuery();
                    con.Close();
                }
            }

            string bday = cmbMonth.Text + " " + cmbDay.Text + " " + cmbYear.Text;
            int current = Convert.ToInt32(DateTime.Now.Year);
            int birthyear = Convert.ToInt32(cmbYear.Text);
            int age = current - birthyear;

            con.Open();
            string updatestf = "Update employees_tbl set firstname='" + txtFname.Text + "',middlename='" + txtMidl.Text + "',lastname='" +
                txtLast.Text + "',birthdate='" + bday + "',address='" + txtAdd.Text + "',age='" + age + "',gender='" +
                cmbGen.Text + "',civilstatus='" + cmbCivil.Text + "',contactnum='" + txtCon.Text + "',email='" + txtEmail.Text + "',schoolgrad='" +
                txtFacGrad.Text + "',educattainment='" + cmbDegree.Text + "',fathername='" + txtFathName.Text + "',mothername='" +
                txtMothName.Text + "',spousename='" + txtSpouse.Text + "',spouseocc='" + txtSpouseOcc.Text + "',emergencyperson='" +
                txtEmergency.Text + "',emergencyrelation='" + txtEmerRelation.Text + "',emergencycon='" + txtEmergencyCon.Text + "',program='" +
                txtProgram.Text + "'where empno='" + primarykey + "'";
            OdbcCommand cmdUpdatestf = new OdbcCommand(updatestf, con);
            cmdUpdatestf.ExecuteNonQuery();
            con.Close();


            setupview_forStaff();
            btnClear.Text = "Clear";
            setupdisableinput_forStaff();
            MessageBox.Show("record successfully updated", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnFacUpdate.Text = "Update";
            btnFacUpdate.Enabled = false;

            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }
            txtSearch.Focus();
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void cmbDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupclear();
            btnFacUpdate.Enabled = false;
            btnFacUpdate.Text = "Update";
            string posi = "";
            if (cmbFilter.Text == "Cashier")
            {
                posi = "cashier";
            }
            if (cmbFilter.Text == "Registrar")
            {
                posi = "registrar";
            }
            if (cmbFilter.Text == "Principal")
            {
                posi = "principal";
            }

            if (posi == "cashier" || posi == "registrar" || posi == "principal")
            {
                lblPosi.Visible = false; lblThePosi.Visible = false;
                con.Open();
                OdbcDataAdapter dastf = new OdbcDataAdapter("Select empno,lastname as 'Lastname',firstname as 'Firstname',middlename as 'Middlename' from employees_tbl where position='" + posi + "'", con);
                DataTable dtstf = new DataTable();
                dastf.Fill(dtstf);
                con.Close();
                dvstf = new DataView(dtstf);
                dgvSearch.DataSource = dvstf;
            }
            else
            {
               
                setupview_forStaff();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvstf.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvstf;
            toolTip1.SetToolTip(txtSearch, "search admin lastname");

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

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Clear")
            {
                setupclear();
                btnFacUpdate.Enabled = false;
                btnFacUpdate.Text = "Update";
            }
            else
            {

                btnClear.Text = "Clear";
                btnFacUpdate.Text = "Update";

                setupretrieveddata_forStaff(primarykey);
                setupdisableinput_forStaff();
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = stflog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maine = new frmMaintenance();
            this.Hide();
            maine.adminlog = stflog;
            maine.VISITED = VISITED;
            maine.Show();

        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = stflog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subform = new frmSubject();
            this.Hide();
            subform.wholog = stflog;
            subform.VISITED = VISITED;
            subform.Show();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Hide();
            levmain.levlog = stflog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            frmSection secform = new frmSection();
            this.Hide();
            secform.secwholog = stflog;
            secform.VISITED = VISITED;
            secform.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roomform = new frmRoom();
            this.Hide();
            roomform.logger = stflog;
            roomform.VISITED = VISITED;
            roomform.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = stflog;
            facmain.VISITED = VISITED;
            facmain.Show();
            this.Hide();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched schedf = new frmSched();
            this.Hide();
            schedf.schedlog = stflog;
            schedf.VISITED = VISITED;
            schedf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqf = new frmRequirement();
            this.Hide();
            reqf.reqlog = stflog;
            reqf.VISITED = VISITED;
            reqf.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feef = new frmFee();
            this.Hide();
            feef.feelog = stflog;
            feef.VISITED = VISITED;
            feef.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Dispose();
            discform.disclog = stflog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            this.Dispose();
            hm.adminlog = stflog;
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

        private void frmStaff_FormClosing(object sender, FormClosingEventArgs e)
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
            deptmainte.deplog = stflog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void cmbCivil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCivil.Text == "Single")
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
       
    }
}
