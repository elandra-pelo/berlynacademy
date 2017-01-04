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
    public partial class frmSchoolYear : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public bool isupdate;
        public string sylog,VISITED,activeSY,selectedyr;
        public DataView dv;
        public frmSchoolYear()
        {
            InitializeComponent();
        }

        private void frmSchoolYear_Load(object sender, EventArgs e)
        {
            lblLogger.Text = sylog;
            lblLoggerPosition.Text = "Admin";

            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            btnSY.BackColor = Color.LightGreen;
            if (VISITED.Contains("School year") == false)
            {
                VISITED += "   School year";
            }
            setUpAY();
            GetActiveSchoolYear();
            lblcount.Text = "no. of school years: " + (dgvSearch.Rows.Count).ToString();
            
        }

         public void GetActiveSchoolYear()
        {
            con.Open();
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);
            con.Close();
            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString(); }
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

        private void frmSchoolYear_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subjmaintenance = new frmSubject();
            subjmaintenance.wholog = sylog;
            subjmaintenance.VISITED = VISITED;
            subjmaintenance.Show();
            this.Hide();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance user = new frmMaintenance();
            this.Hide();
            user.adminlog = sylog;
            user.VISITED = VISITED;
            user.Show();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Hide();
            levmain.levlog = sylog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            frmSection section = new frmSection();
            section.secwholog = sylog;
            section.VISITED = VISITED;
            section.Show();
            this.Hide();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roommaintenance = new frmRoom();
            roommaintenance.logger = sylog;
            roommaintenance.VISITED = VISITED;
            roommaintenance.Show();
           
            this.Hide();
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = sylog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = sylog;
            facmain.VISITED = VISITED;
            facmain.Show();
            
            this.Hide();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched schedf = new frmSched();
            this.Hide();
            schedf.schedlog = sylog;
            schedf.VISITED = VISITED;
            schedf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqf = new frmRequirement();
            this.Hide();
            reqf.reqlog = sylog;
            reqf.VISITED = VISITED;
            reqf.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feef = new frmFee();
            this.Hide();
            feef.feelog = sylog;
            feef.VISITED = VISITED;
            feef.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Hide();
            discform.disclog = sylog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if ((txtBegYear.Text == "") || (rdbActive.Checked == false && rdbInactive.Checked == false))
            {
                MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                string stat = "";
                if (rdbActive.Checked == true)
                {
                    stat = "Active";
                }
                else
                {
                    stat = "Inactive";
                }

                if (txtBegYear.TextLength != 4)
                {
                    MessageBox.Show("school year not valid!", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBegYear.Focus();
                    return;
                }

                con.Open();
                OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from schoolyear_tbl where year='" + txtBegYear.Text + "'", con);
                DataTable dt0 = new DataTable();
                da0.Fill(dt0);
                con.Close();

                if (dt0.Rows.Count > 0)
                {
                    MessageBox.Show("shool year already added.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBegYear.Clear(); txtBegYear.Focus();
                    return;
                }
                else
                {
                    con.Open();
                    OdbcDataAdapter daa = new OdbcDataAdapter("select*from schoolyear_tbl where status='" + "Active" + "'", con);
                    DataTable dtt = new DataTable();
                    daa.Fill(dtt);
                    con.Close();

                    if ((dtt.Rows.Count > 0) && (stat == "Active"))
                    {
                        MessageBox.Show(dtt.Rows[0].ItemArray[0].ToString() + " is already set to active", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        int year = Convert.ToInt32(txtBegYear.Text);
                        string format = "SY:" + year.ToString() + "-" + (year + 1).ToString();
                        ListViewItem itmyr = new ListViewItem();
                        itmyr.Text = (dgvSearch.Rows.Count + 1).ToString();

                        con.Open();
                        string add = "Insert Into schoolyear_tbl(year,syformat,status)values('" + year.ToString() + "','" + format + "','" + stat + "')";
                        OdbcCommand cmd = new OdbcCommand(add, con);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        con.Open();
                        string updatestudno = "Update studno_tbl set current='" + "0000" + "',number='" + "0" + "'";
                        OdbcCommand cmdus = new OdbcCommand(updatestudno, con);
                        cmdus.ExecuteNonQuery();
                        con.Close();


                        setUpAY();
                        isupdate = false;
                        MessageBox.Show("school year " + txtBegYear.Text + " successfully added!", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rdbActive.Checked = false; rdbInactive.Checked = false; txtBegYear.Clear(); txtBegYear.Focus();
                      
                        //ADD ENROLLMENT DAYS FOR SY
                        con.Open();
                        string addED = "Insert Into enrollmentdays_tbl(start,end,SY)values('" + "" + "','" + "" + "','" +format+ "')";
                        OdbcCommand cmdED = new OdbcCommand(addED, con);
                        cmdED.ExecuteNonQuery();
                        con.Close();

                        //
                        con.Open();
                        OdbcDataAdapter da = new OdbcDataAdapter("Select deptname from department_tbl", con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        con.Close();
                       
                        string[] fees = new string[6]{"ANNUAL PAYMENT","TUITION FEE","UPON ENROLLMENT","REGISTRATION","MISCELLANEOUS","MONTHLY INSTALLMENT"};
                        string[] ftype = new string[6] { "payment", "fee", "payment", "fee", "fee", "payment" };
                        string[] levs = new string[dt.Rows.Count];

                        if (dt.Rows.Count > 0)
                        {
                            for (int g = 0; g < dt.Rows.Count; g++)
                            {
                                levs[g] = dt.Rows[g].ItemArray[0].ToString();
                            }
                        }


                        for (int s = 0; s < levs.Count(); s++)
                        {
                            for (int t = 0; t < fees.Count(); t++)
                            {
                                con.Open();
                                string addfee = "Insert Into fee_tbl(fee,amount,level,type,SY)values('" + fees[t] + "','" + "0.00" + "','" + levs[s] + "','"+ftype[t]+"','"+format+"')";
                                OdbcCommand cmdf = new OdbcCommand(addfee, con);
                                cmdf.ExecuteNonQuery();
                                con.Close();
                            }
                        }

                    }
                }
            }
        }

        public void setUpAY()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select year as 'School_year',syformat as 'Format',status as 'Status' from schoolyear_tbl order by year ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dv = new DataView(dt);
            dgvSearch.DataSource = dv;
            con.Close();

            dgvSearch.Columns[0].Width = 135;
            dgvSearch.Columns[1].Width = 200;
            dgvSearch.Columns[2].Width = 139;

            dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSearch.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSearch.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if ((txtBegYear.Text == "") || (rdbActive.Checked == false && rdbInactive.Checked == false))
            {
                MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                string stat = "";
                string curr = "";
                string snum = "";

                if (btnUpdate.Text == "Update")
                {
                    btnUpdate.Text = "Save";
                    txtBegYear.Enabled = true;
                    rdbActive.Enabled = true;
                    rdbInactive.Enabled = true;
                    //txtBegYear.ReadOnly = false;
                }
                else
                {
                    if (rdbActive.Checked == true)
                    {
                        stat = "Active";
                    }
                    else
                    {
                        stat = "Inactive";
                    }


                    con.Open();
                    OdbcDataAdapter daa1 = new OdbcDataAdapter("select*from schoolyear_tbl where status='" + "Active" + "'", con);
                    DataTable dtt1 = new DataTable();
                    daa1.Fill(dtt1);
                    con.Close();
                    if (dtt1.Rows.Count > 0)
                    {
                        con.Open();
                        OdbcDataAdapter da = new OdbcDataAdapter("select*from studno_tbl", con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        con.Close();
                        if (dt.Rows.Count > 0)
                        {
                            curr = dt.Rows[0].ItemArray[1].ToString();
                            snum = dt.Rows[0].ItemArray[2].ToString();
                            con.Open();
                            string updateAct = "Update schoolyear_tbl set format_snum='" + curr + "',last_snum='" + snum + "'where year='" + dtt1.Rows[0].ItemArray[0].ToString() + "'";
                            OdbcCommand cmdAct = new OdbcCommand(updateAct, con);
                            cmdAct.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                    con.Open();
                    OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from schoolyear_tbl where year='" + txtBegYear.Text + "'", con);
                    DataTable dt0 = new DataTable();
                    da0.Fill(dt0);
                    con.Close();

                    if (dt0.Rows.Count > 0)
                    {
                        if (txtBegYear.Text!=selectedyr)
                        {
                            MessageBox.Show("shool year already exists", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //MessageBox.Show("academic year already added.", "Academic year", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //txtBYear.Clear(); txtBYear.Focus();
                        //return;
                        //}
                        //else
                        //{
                        
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("select*from schoolyear_tbl where status='" + "Active" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();

                        if ((dtt.Rows.Count > 0) && (stat == "Active"))
                        {
                            con.Open();
                            OdbcDataAdapter da = new OdbcDataAdapter("select*from studno_tbl", con);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            con.Close();
                            if(dt.Rows.Count > 0)
                            {
                                curr = dt.Rows[0].ItemArray[1].ToString();
                                snum = dt.Rows[0].ItemArray[2].ToString();
                                con.Open();
                                string update = "Update schoolyear_tbl set format_snum='" + curr + "',last_snum='" + snum + "'where year='" + dtt.Rows[0].ItemArray[0].ToString() + "'";
                                OdbcCommand cmd = new OdbcCommand(update, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }

                            MessageBox.Show("School year "+dtt.Rows[0].ItemArray[0].ToString() + " is active", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                           

                            con.Open();
                            string update = "Update schoolyear_tbl set status='" + stat + "'where year='" + txtBegYear.Text + "'";
                            OdbcCommand cmd = new OdbcCommand(update, con);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            con.Open();
                            string update1 = "Update stud_tbl set status='" + "Inactive" + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update1, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();

                            con.Open();
                            string update2 = "Update stud_tbl set status='" + stat + "'where syregistered='" + dt0.Rows[0].ItemArray[1].ToString() + "'";
                            OdbcCommand cmd2 = new OdbcCommand(update2, con);
                            cmd2.ExecuteNonQuery();
                            con.Close();

                            con.Open();
                            OdbcDataAdapter daaa = new OdbcDataAdapter("select*from schoolyear_tbl where year='" + txtBegYear.Text + "'", con);
                            DataTable dttt = new DataTable();
                            daaa.Fill(dttt);
                            con.Close();
                            if (dttt.Rows.Count > 0)
                            {
                                if (stat == "Active")
                                {
                                    string current = dttt.Rows[0].ItemArray[3].ToString();
                                    string number = dttt.Rows[0].ItemArray[4].ToString();
                                    if (dttt.Rows[0].ItemArray[3].ToString()=="")
                                    {
                                        current = "0000";
                                    }
                                    if (dttt.Rows[0].ItemArray[4].ToString() == "")
                                    {
                                        number = "0";
                                    }

                                    con.Open();
                                    string update3 = "Update studno_tbl set current='" + current + "',number='" + number + "'";
                                    OdbcCommand cmd3 = new OdbcCommand(update3, con);
                                    cmd3.ExecuteNonQuery();
                                    con.Close();
                                }
                          
                            }

                         
                            setUpAY();
                            btnUpdate.Text = "Update";
                            MessageBox.Show("school year successfully set to " + stat + ".", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtBegYear.Clear(); rdbActive.Checked = false; rdbInactive.Checked = false; txtBegYear.Focus(); btnUpdate.Enabled = false; btnAdd.Enabled = true; txtBegYear.Enabled = true; txtBegYear.ReadOnly = false;
                            isupdate = false;

                        }

                    }
                }
            }
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            txtBegYear.Clear();
            rdbActive.Checked = false;
            rdbInactive.Checked = false;
            btnUpdate.Enabled = false;
            isupdate = false;
            txtBegYear.ReadOnly = false;
            rdbActive.Enabled = true;
            rdbInactive.Enabled = true;
            btnAdd.Enabled = true;
            //btnUpdate.Enabled = true;
            btnUpdate.Text = "Update";
            txtBegYear.Enabled = true;
            txtBegYear.Focus();
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            isupdate = true;
            btnUpdate.Text = "Update";
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;


            txtBegYear.ReadOnly = true; rdbActive.Enabled = false; rdbInactive.Enabled = false;
            txtBegYear.Enabled = false;

            txtBegYear.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            selectedyr = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();

            string stats = dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
            if (stats == "Active")
            {
                rdbActive.Checked = true;
            }
            if (stats == "Inactive")
            {
                rdbInactive.Checked = true;
            }
        }

        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            lblcount.Text = "no. of school years: " + (dgvSearch.Rows.Count).ToString();
        }

        private void rdbActive_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = string.Format("Convert(School_year, 'System.String') LIKE '%{0}%'",txtSearch.Text);
            dgvSearch.DataSource = dv;
            toolTip1.SetToolTip(txtSearch, "search school year");

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

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = sylog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = sylog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void pnlUser_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = sylog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }
    }
}
