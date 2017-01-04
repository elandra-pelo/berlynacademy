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
    public partial class frmFacultyAdvisory : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string advlog, primarykey, thefac, emptype, co, accesscode, VISITED, notifstat,classDept;
        public DataView dvAdv, dvAdvNoAdv;
        public bool isVisited, viewNotifDue, viewNotifDisc, viewNotifLate;
        public frmFacultyAdvisory()
        {
            InitializeComponent();
        }

        private void frmFacultyAdvisory_Load(object sender, EventArgs e)
        {
            lblLogger.Text = advlog;
            lblLoggerPosition.Text = emptype;
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

            if (isVisited == false)
            {
                if (VISITED.Contains("Faculty advisory") == false)
                {
                    VISITED += "   Faculty advisory";
                    isVisited = true;
                }
            }

            setupDept();
            setupMENU();
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

        public void setupLevelList(string dept)
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select level from level_tbl where department='"+dept+"'", con);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbLevelSetAdv.Items.Clear();
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbLevelSetAdv.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                }
            }
        }

        public void setupMENU()
        {
            string sortedAccessCode = "";
            string[] ac = new string[555];
            for (int a = 0; a < accesscode.Length; a++)
            {
                ac[a] = accesscode.Substring(a, 1);
            }
            Array.Sort(ac);
            foreach (string s in ac)
            {
                sortedAccessCode += s;
            }
            //MessageBox.Show(sortedAccessCode);
            DataTable dtMenu = new DataTable();
            dtMenu.Columns.Add("accmod");
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt5 = new DataTable();
            DataTable dt6 = new DataTable();
            DataTable dt7 = new DataTable();
            DataTable dt8 = new DataTable();
            DataTable dt9 = new DataTable();
            DataTable dt0 = new DataTable();

            if (sortedAccessCode.Contains("1") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='1'", con);
                da.Fill(dt1);
                con.Close();
            }
            if (sortedAccessCode.Contains("2") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='2'", con);
                da.Fill(dt2);
                con.Close();
            }
            if (sortedAccessCode.Contains("3") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='3'", con);
                da.Fill(dt3);
                con.Close();
            }
            if (sortedAccessCode.Contains("4") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='4'", con);
                da.Fill(dt4);
                con.Close();
            }
            if (sortedAccessCode.Contains("5") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='5'", con);
                da.Fill(dt5);
                con.Close();
            }
            if (sortedAccessCode.Contains("6") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='6'", con);
                da.Fill(dt6);
                con.Close();
            }
            if (sortedAccessCode.Contains("7") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='7'", con);
                da.Fill(dt7);
                con.Close();
            }
            if (sortedAccessCode.Contains("8") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='8'", con);
                da.Fill(dt8);
                con.Close();
            }
            if (sortedAccessCode.Contains("9") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='9'", con);
                da.Fill(dt9);
                con.Close();
            }
            if (sortedAccessCode.Contains("0") == true)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select * from module_tbl where id='0'", con);
                da.Fill(dt0);
                con.Close();
            }

            int getFAdindex = 1;
            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                getFAdindex++;
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                getFAdindex++;
                dtMenu.Rows.Add("  " + dt2.Rows[0].ItemArray[1].ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                getFAdindex++;
                dtMenu.Rows.Add("  " + dt3.Rows[0].ItemArray[1].ToString());
            }
            if (dt4.Rows.Count > 0)
            {
                getFAdindex++;
                dtMenu.Rows.Add("  " + dt4.Rows[0].ItemArray[1].ToString());
            }
            if (dt5.Rows.Count > 0)
            {
                getFAdindex++;
                dtMenu.Rows.Add("  " + dt5.Rows[0].ItemArray[1].ToString());
            }
            if (dt6.Rows.Count > 0)
            {
                getFAdindex++;
                dtMenu.Rows.Add("  " + dt6.Rows[0].ItemArray[1].ToString());
            }
            if (dt7.Rows.Count > 0)
            {
                dtMenu.Rows.Add("  " + dt7.Rows[0].ItemArray[1].ToString());
            }
            if (dt8.Rows.Count > 0)
            {
                dtMenu.Rows.Add("  " + dt8.Rows[0].ItemArray[1].ToString());
            }
            if (dt9.Rows.Count > 0)
            {
                dtMenu.Rows.Add("  " + dt9.Rows[0].ItemArray[1].ToString());
            }
            if (dt0.Rows.Count > 0)
            {
                dtMenu.Rows.Add("  " + dt0.Rows[0].ItemArray[1].ToString());
            }

            dtMenu.Rows.Add("  About us");
            dtMenu.Rows.Add("  Logout");

            DataView dvMenu = new DataView(dtMenu);
            dgvm.DataSource = dvMenu;
            dgvm.Rows[0].Selected = false;
            dgvm.Columns[0].Width = 263;
            dgvm.Rows[getFAdindex].DefaultCellStyle.BackColor = Color.LightGreen;
        }


        public void setupview_forSetAdvisoryWithoutAdv(string dept)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select empno as 'No.',(select concat(lastname,', ',firstname,' ',middlename))as'Faculty' from employees_tbl where position='faculty' and advisory='' and department='"+dept+"' order by lastname ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvAdvNoAdv = new DataView(dt);


            if (dt.Rows.Count > 0)
            {
                pnlwithout.Visible = false;
                dgvSetAdvNoAdv.DataSource = dvAdvNoAdv;
                dgvSetAdvNoAdv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSetAdvNoAdv.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                dgvSetAdvNoAdv.Columns[0].Width = 0;
                dgvSetAdvNoAdv.Columns[1].Width = 450;
               
            }
            else
            {
                dgvSetAdvNoAdv.DataSource = null;
                pnlwithout.Visible = true;
                lblmemowithout.Text = "no items found...";
            }

            lblResult.Text = "no. of faculty: " + dgvSearch.Rows.Count.ToString();
            lblResultNoAdv.Text = "no. of faculty without advisory class: " + dgvSetAdvNoAdv.Rows.Count.ToString();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            this.Hide();
            frmEmpLogin home = new frmEmpLogin();
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

        private void frmFacultyAdvisory_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void cmbLevelSetAdv_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupview_forSetAdvisory(cmbLevelSetAdv.Text);
            setupSection(cmbLevelSetAdv.Text);
        }

        public void setupview_forSetAdvisory(string leveladv)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select empno as 'No.',(select concat(lastname,', ',firstname,' ',middlename))as'Faculty',advisory as 'Advisory' from employees_tbl where grade='" + leveladv + "' and advisory<>'' and position='faculty' order by lastname ASC", con);
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
                dgvSearch.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                dgvSearch.Columns[0].Width = 0;
                dgvSearch.Columns[1].Width = 370;
                dgvSearch.Columns[2].Width = 100;

            }
            else
            {
                dgvSearch.DataSource = null;
                pnlwith.Visible = true;
                lblmemowith.Text = "no items found...";
            }

            lblResult.Text = "no. of faculty: " + dgvSearch.Rows.Count.ToString();
            lblResultNoAdv.Text = "no. of faculty without advisory class: " + dgvSetAdvNoAdv.Rows.Count.ToString();
        }

        public void setupSection(string keystring)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select section from section_tbl where level='" + keystring + "'", con);
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
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

        private void dgvSetAdvNoAdv_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (txtSearchWithoutAdv.Text == "")
            {
                pnlwithout.Visible = false;
            }
        }

        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dgvSearch.Rows.Count > 0)
            {
                pnlwith.Visible = false;
            }
            else
            {
                pnlwith.Visible = true;
            }
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

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count > 0)
            {
                primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }

            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }
            lblKey.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            btnmovetoWithAdv.Enabled = false;
            btnMovetoWithoutAdv.Enabled = true;
            btnsetAdvCancel.Enabled = true;
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
                    if (res == DialogResult.Yes)
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

                        setupSetanAdviserToGradesTable(level, lblKey.Text);
                        btnmovetoWithAdv.Enabled = false;
                        btnsetAdvCancel.Enabled = false;

                        setupview_forSetAdvisory(cmbLevelSetAdv.Text);
                        setupview_forSetAdvisoryWithoutAdv(cmbDept.Text);
                        lblKey.Text = "";
                    }
                    else if (res == DialogResult.Cancel)
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

                    setupSetanAdviserToGradesTable(level, lblKey.Text);
                    btnmovetoWithAdv.Enabled = false;
                    btnsetAdvCancel.Enabled = false;

                    setupview_forSetAdvisory(cmbLevelSetAdv.Text);
                    setupview_forSetAdvisoryWithoutAdv(cmbDept.Text);
                    lblKey.Text = "";
                }
            }
        }

        public void setupSetanAdviserToGradesTable(string level, string facID)
        {
            string theAdv = "";
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno='" + facID + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                theAdv = dt.Rows[0].ItemArray[1].ToString() + " " + dt.Rows[0].ItemArray[2].ToString() + " " + dt.Rows[0].ItemArray[3].ToString();
            }

            if (level == "Kinder")
            {
                con.Open();
                string update = "Update Kindergrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
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
                string update = "Update gradesixgrades_tbl set adviser='" + theAdv + "'where section='" + cmbSecSetAdv.Text + "'";
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

        private void btnMovetoWithoutAdv_Click(object sender, EventArgs e)
        {
            string level = cmbLevelSetAdv.Text;
            string sectionwhowasremovedanadviser = dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
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
            setupview_forSetAdvisoryWithoutAdv(cmbDept.Text);
            lblKey.Text = "";
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

        private void btnSIPrin_Click(object sender, EventArgs e)
        {
            frmStudInfo sif = new frmStudInfo();
            this.Hide();
            sif.studlog = advlog;
            sif.emptype = "principal";
            sif.Show();
        }

        private void btnFIPrin_Click(object sender, EventArgs e)
        {
            frmFacInfo fif = new frmFacInfo();
            this.Hide();
            fif.facinfolog = advlog;
            fif.emptype = "principal";
            fif.Show();
        }

        private void btnReprin_Click(object sender, EventArgs e)
        {
            frmReport rf = new frmReport();
            this.Hide();
            rf.replog = advlog;
            rf.emptype = "principal";
            rf.theFaculty = advlog;
            rf.Show();
        }

        private void btnAbtPrin_Click(object sender, EventArgs e)
        {
            frmEmpAbout eaf = new frmEmpAbout();
            this.Hide();
            eaf.ablog = advlog;
            eaf.emptype = "principal";
            eaf.Show();
        }

        private void btnFacAdv_Click(object sender, EventArgs e)
        {
            frmPrincipalMain pmf = new frmPrincipalMain();
           
            pmf.prinlog = advlog;
            pmf.Show();
            this.Hide();
        }

        private void dgvm_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Hand;
        }

        private void dgvm_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Default;
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Faculty advisory")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Faculty advisory")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gainsboro;
            }
        }

        private void dgvm_Click(object sender, EventArgs e)
        {
            if (dgvm.Rows.Count < 0)
            {
                return;
            }
        }

        private void dgvm_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Activity")
            {
                if (emptype == "Cashier")
                {
                    frmCashierMain casmain = new frmCashierMain();
                    this.Hide();
                    casmain.emptype = emptype;
                    casmain.cashlog = advlog;
                    casmain.accesscode = accesscode;
                    casmain.CO = co;
                    casmain.thefac = thefac;
                    casmain.VISITED = VISITED;
                    casmain.viewNotifDue = viewNotifDue;
                    casmain.viewNotifDisc = viewNotifDisc;
                    casmain.viewNotifLate = viewNotifLate;
                    casmain.notifstat = notifstat;
                    casmain.Show();
                }
                if (emptype == "Principal")
                {
                    frmPrincipalMain pmf = new frmPrincipalMain();
                    this.Hide();
                    pmf.emptype = emptype;
                    pmf.accesscode = accesscode;
                    pmf.co = co;
                    pmf.thefac = thefac;
                    pmf.prinlog = advlog;
                    pmf.VISITED = VISITED;
                    pmf.viewNotifDue = viewNotifDue;
                    pmf.viewNotifDisc = viewNotifDisc;
                    pmf.viewNotifLate = viewNotifLate;
                    pmf.notifstat = notifstat;
                    pmf.Show();
                }
                if (emptype == "Registrar")
                {
                    frmRegistrarMain regmain = new frmRegistrarMain();
                    this.Hide();
                    regmain.emptype = emptype;
                    regmain.co = co;
                    regmain.reglog = advlog;
                    regmain.accesscode = accesscode;
                    regmain.thefac = thefac;
                    regmain.VISITED = VISITED;
                    regmain.viewNotifDue = viewNotifDue;
                    regmain.viewNotifDisc = viewNotifDisc;
                    regmain.viewNotifLate = viewNotifLate;
                    regmain.notifstat = notifstat;
                    regmain.Show();
                }
                if (emptype == "Faculty")
                {
                    frmEmpMain empf = new frmEmpMain();
                    this.Hide();
                    empf.emptype = emptype;
                    empf.CO = co;
                    empf.faclog = advlog;
                    empf.accesscode = accesscode;
                    empf.TheFacultyName = thefac;
                    empf.VISITED = VISITED;
                    empf.viewNotifDue = viewNotifDue;
                    empf.viewNotifDisc = viewNotifDisc;
                    empf.viewNotifLate = viewNotifLate;
                    empf.notifstat = notifstat;
                    empf.Show();
                }
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Admission")
            {
                frmAdmission frmadm = new frmAdmission();
                this.Hide();
                frmadm.emptype = emptype;
                frmadm.admlog = advlog;
                frmadm.CO = co;
                frmadm.accesscode = accesscode;
                frmadm.TheFaculty = thefac;
                frmadm.VISITED = VISITED;
                frmadm.viewNotifDue = viewNotifDue;
                frmadm.viewNotifDisc = viewNotifDisc;
                frmadm.viewNotifLate = viewNotifLate;
                frmadm.notifstat = notifstat;
                frmadm.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Payment")
            {
                frmPayment formPay = new frmPayment();
                this.Hide();
                formPay.emptype = emptype;
                formPay.paylog = advlog;
                formPay.CashierOperator = co;
                formPay.accesscode = accesscode;
                formPay.TheFac = thefac;
                formPay.VISITED = VISITED;
                formPay.viewNotifDue = viewNotifDue;
                formPay.viewNotifDisc = viewNotifDisc;
                formPay.viewNotifLate = viewNotifLate;
                formPay.notifstat = notifstat;
                formPay.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Student records")
            {
                frmAssessment formStudRec = new frmAssessment();
                this.Hide();
                formStudRec.emptype = emptype;
                formStudRec.co = co;
                formStudRec.asslog = advlog;
                formStudRec.accesscode = accesscode;
                formStudRec.thefac = thefac;
                formStudRec.VISITED = VISITED;
                formStudRec.viewNotifDisc = viewNotifDisc;
                formStudRec.viewNotifDue = viewNotifDue;
                formStudRec.viewNotifLate = viewNotifLate;
                formStudRec.notifstat = notifstat;
                formStudRec.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Student grades")
            {
                frmStdGrd formstdgrd = new frmStdGrd();
                this.Hide();
                formstdgrd.emptype = emptype;
                formstdgrd.CO = co;
                formstdgrd.grdlog = advlog;
                formstdgrd.accesscode = accesscode;
                formstdgrd.theFacultyName = thefac;
                formstdgrd.VISITED = VISITED;
                formstdgrd.viewNotifDue = viewNotifDue;
                formstdgrd.viewNotifDisc = viewNotifDisc;
                formstdgrd.viewNotifLate = viewNotifLate;
                formstdgrd.notifstat = notifstat;
                formstdgrd.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Student information")
            {
                frmStudInfo stud = new frmStudInfo();
                this.Hide();
                stud.emptype = emptype;
                stud.CO = co;
                stud.studlog = advlog;
                stud.accesscode = accesscode;
                stud.TheFaculty = thefac;
                stud.VISITED = VISITED;
                stud.viewNotifDue = viewNotifDue;
                stud.viewNotifDisc = viewNotifDisc;
                stud.viewNotifLate = viewNotifLate;
                stud.notifstat = notifstat;
                stud.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Faculty information")
            {
                frmFacInfo facf = new frmFacInfo();
                this.Hide();
                facf.emptype = emptype;
                facf.CO = co;
                facf.facinfolog = advlog;
                facf.accesscode = accesscode;
                facf.TheFaculty = thefac;
                facf.VISITED = VISITED;
                facf.viewNotifDue = viewNotifDue;
                facf.viewNotifDisc = viewNotifDisc;
                facf.viewNotifLate = viewNotifLate;
                facf.notifstat = notifstat;
                facf.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Faculty advisory")
            {
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Sectioning")
            {
                frmSectioning frmSec = new frmSectioning();
                this.Hide();
                frmSec.emptype = emptype;
                frmSec.co = co;
                frmSec.seclog = advlog;
                frmSec.accesscode = accesscode;
                frmSec.TheFaculty = thefac;
                frmSec.VISITED = VISITED;
                frmSec.viewNotifDue = viewNotifDue;
                frmSec.viewNotifDisc = viewNotifDisc;
                frmSec.viewNotifLate = viewNotifLate;
                frmSec.notifstat = notifstat;
                frmSec.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Report")
            {
                frmReport rfac = new frmReport();
                this.Hide();
                rfac.co = co;
                rfac.replog = advlog;
                rfac.emptype = emptype;
                rfac.accesscode = accesscode;
                rfac.theFaculty = thefac;
                rfac.VISITED = VISITED;
                rfac.viewNotifDue = viewNotifDue;
                rfac.viewNotifDisc = viewNotifDisc;
                rfac.viewNotifLate = viewNotifLate;
                rfac.notifstat = notifstat;
                rfac.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Scheduling")
            {
                frmSched rsched = new frmSched();
                this.Hide();
                rsched.CO = co;
                rsched.schedlog = advlog;
                rsched.emptype = emptype;
                rsched.accesscode = accesscode;
                rsched.TheFaculty = thefac;
                rsched.VISITED = VISITED;
                rsched.viewNotifDue = viewNotifDue;
                rsched.viewNotifDisc = viewNotifDisc;
                rsched.viewNotifLate = viewNotifLate;
                rsched.notifstat = notifstat;
                rsched.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  About us")
            {
                frmEmpAbout about = new frmEmpAbout();
                this.Hide();
                about.ablog = advlog;
                about.emptype = emptype;
                about.CO = co;
                about.accesscode = accesscode;
                about.theFaculty = thefac;
                about.VISITED = VISITED;
                about.viewNotifDue = viewNotifDue;
                about.viewNotifDisc = viewNotifDisc;
                about.viewNotifLate = viewNotifLate;
                about.notifstat = notifstat;
                about.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Logout")
            {
                LOGOUT();
                frmEmpLogin home = new frmEmpLogin();
                this.Hide();
                home.Show();
            }
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupLevelList(cmbDept.Text);
            setupview_forSetAdvisoryWithoutAdv(cmbDept.Text);
            setupview_forSetAdvisory("");
            cmbSecSetAdv.Items.Clear();

        }
    }
}
