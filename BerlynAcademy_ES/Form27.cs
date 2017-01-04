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
    public partial class frmSectioning : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public DataView dvPerLevel,dvNoSec;
        public string seclog, TheFaculty, primarykey, emptype, co, accesscode, capacity, VISITED, notifstat;
        public bool isclickAss;
        public bool isVisited, viewNotifDue, viewNotifDisc, viewNotifLate;

        public frmSectioning()
        {
            InitializeComponent();
        }

        private void frmSectioning_Load(object sender, EventArgs e)
        {
            lblLogger.Text = seclog;
            lblLoggerPosition.Text = emptype;
            if (isVisited == false)
            {
                if (VISITED.Contains("Sectioning") == false)
                {
                    VISITED += "   Sectioning";
                    isVisited = true;
                }
            }
          
            setupLevelList();
            setupMENU();
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
                cmbLevelNoSec.Items.Clear();
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbLevelNoSec.Items.Add(dt.Rows[u].ItemArray[0].ToString());
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
           // MessageBox.Show(sortedAccessCode);
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

            int getSectioningIndex = 1;
            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                getSectioningIndex++;
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                getSectioningIndex++;
                dtMenu.Rows.Add("  " + dt2.Rows[0].ItemArray[1].ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                getSectioningIndex++;
                dtMenu.Rows.Add("  " + dt3.Rows[0].ItemArray[1].ToString());
            }
            if (dt4.Rows.Count > 0)
            {
                getSectioningIndex++;
                dtMenu.Rows.Add("  " + dt4.Rows[0].ItemArray[1].ToString());
            }
            if (dt5.Rows.Count > 0)
            {
                getSectioningIndex++;
                dtMenu.Rows.Add("  " + dt5.Rows[0].ItemArray[1].ToString());
            }
            if (dt6.Rows.Count > 0)
            {
                getSectioningIndex++;
                dtMenu.Rows.Add("  " + dt6.Rows[0].ItemArray[1].ToString());
            }
            if (dt7.Rows.Count > 0)
            {
                getSectioningIndex++;
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
            dgvm.Rows[getSectioningIndex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        public void setupview_perLevelforSEC(string level, string section)
        {
            con.Open();
            string activeSY = "";
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);

            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString(); }

            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'No',(select concat(lname,', ',fname,' ',mname)) as 'Name'from stud_tbl where level='" + level + "' and section='" + section + "'and status='"+"Active"+"' order by lname ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            OdbcDataAdapter da0 = new OdbcDataAdapter("Select count(gender) from stud_tbl where gender='Male' and level='" + level + "' and section='" + section + "'and status='"+"Active"+"'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(gender) from stud_tbl where gender='Female' and level='" + level + "' and section='" + section + "'and status='"+"Active"+"'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            con.Close();
            dvPerLevel = new DataView(dt);


            if (dt.Rows.Count > 0)
            {
                pnlwith.Visible = false;
                dgvSearch.DataSource = dvPerLevel;

                dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[0].Width = 100;
                dgvSearch.Columns[1].Width = 370;
               

            }
            else
            {
                dgvSearch.DataSource = null;
                pnlwith.Visible = true;
                lblmemowith.Text = "no items found...";
            }

            lblResult.Text = "no. of students: " + dgvSearch.Rows.Count.ToString();

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

        public void setuplist_NoSectionforSEC(string lev)
        {
            con.Open();
            string activeSY = "";
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);

            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString(); }

            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'No',(select concat(lname,', ',fname,' ',mname)) as 'Name' from stud_tbl where section='' and level='" + lev + "'and status='"+"Active"+"'order by lname ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            OdbcDataAdapter da0 = new OdbcDataAdapter("Select count(gender) from stud_tbl where gender='Male' and section='' and level='" + lev + "'and status='" + "Active" + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(gender) from stud_tbl where gender='Female' and section='' and level='" + lev + "'and status='" + "Active" + "'", con);
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
                dgvNoSec.Columns[1].Width = 335;
               
            }
            else
            {
                dgvNoSec.DataSource = null;
                pnlWOSec.Visible = true;
                lblWOSec.Text = "no items found...";
            }

            lblresultnosec.Text = "no. of students without section in " + cmbLevelNoSec.Text + ": " + dgvNoSec.Rows.Count.ToString();



            if (dt0.Rows.Count > 0 && dt1.Rows.Count > 0)
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

        private void btnActivity_Click(object sender, EventArgs e)
        {
            frmEmpMain empf = new frmEmpMain();
            this.Hide();
            empf.faclog = seclog;
            empf.TheFacultyName = TheFaculty;
            empf.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnStudI_Click(object sender, EventArgs e)
        {
            frmStudInfo stud = new frmStudInfo();
            this.Hide();
            stud.emptype = "faculty";
            stud.studlog = seclog;
            stud.TheFaculty = TheFaculty;
            stud.Show();
        }

        private void btnFac_Click(object sender, EventArgs e)
        {
            frmFacInfo facf = new frmFacInfo();
            this.Hide();
            facf.emptype = "faculty";
            facf.facinfolog = seclog;
            facf.TheFaculty = TheFaculty;
            facf.Show();
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            frmStdGrd fgrd = new frmStdGrd();
            this.Hide();
            fgrd.grdlog = seclog;
            fgrd.theFacultyName = TheFaculty;
            fgrd.Show();
        }

        private void btnFacRep_Click(object sender, EventArgs e)
        {
            frmReport rf = new frmReport();
            this.Hide();
            rf.replog = seclog;
            rf.emptype = "faculty";
            rf.theFaculty = TheFaculty;
            rf.Show();
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

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmEmpAbout eabout = new frmEmpAbout();
            this.Hide();
            eabout.ablog = seclog;
            eabout.emptype = "faculty";
            eabout.theFaculty = TheFaculty;
            eabout.Show();
        }

        private void cmbLevelNoSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbFilter.Text = "Student's name";
            cmbFilterNoSec.Text = "Student's name";
            setupview_perLevelforSEC("", "");
            setuplist_NoSectionforSEC(cmbLevelNoSec.Text);
            setupSectionSectioning(cmbLevelNoSec.Text);
            cmbSecNoSec.Enabled = true;
            lblResult.Text = "no. of students in " + cmbLevelNoSec.Text + " section " + cmbSecNoSec.Text + ": " + dgvSearch.Rows.Count;
            lblAssigned.Text = "room assigned: ";
            lblCapacity.Text = "room capacity: ";
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
            setupCapacityInformation(cmbLevelNoSec.Text, cmbSecNoSec.Text);
            setupview_perLevelforSEC(cmbLevelNoSec.Text, cmbSecNoSec.Text);
            setuplist_NoSectionforSEC(cmbLevelNoSec.Text);
            lblResult.Text = "no. of students in " + cmbLevelNoSec.Text + " section " + cmbSecNoSec.Text + ": " + dgvSearch.Rows.Count;
        }

        public void setupCapacityInformation(string lev, string sec)
        {
            capacity = "";
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select name,grade,section,id from roomallocation_tbl where grade='" + lev + "'and section='"+sec+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select capacity from room_tbl where id='" + dt.Rows[0].ItemArray[3].ToString() + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();
                if (dt1.Rows.Count > 0)
                {
                    capacity = dt1.Rows[0].ItemArray[0].ToString();
                }

                lblAssigned.Text = "room assigned: " + dt.Rows[0].ItemArray[0].ToString();
                lblCapacity.Text = "room capacity: " + capacity;
            }
            else
            {
                MessageBox.Show("Section "+cmbSecNoSec.Text+" has no room assign"+"\nOperation could not perform.", "Sectioning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnMovetowith_Click(object sender, EventArgs e)
        {
            string level = cmbLevelNoSec.Text;
            int countrows = dgvSearch.Rows.Count;
            int capact = 0;
            if (capacity != "")
            {
                capact = Convert.ToInt32(capacity);
            }
            if (countrows >=capact && capact!=0)
            {
                MessageBox.Show("maximum no. of student in section was reached.\n"+"Operation could not perform.","Sectioning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            con.Open();
            string updateSec1 = "Update stud_tbl set section='" + cmbSecNoSec.Text + "'where studno='" + lblKey.Text + "'";
            OdbcCommand cmdUpdateSec1 = new OdbcCommand(updateSec1, con);
            cmdUpdateSec1.ExecuteNonQuery();
            con.Close();

            btnMovetowith.Enabled = false;
            btnCancel.Enabled = false;
            setupview_perLevelforSEC(cmbLevelNoSec.Text, cmbSecNoSec.Text);
            setuplist_NoSectionforSEC(cmbLevelNoSec.Text);
            lblResult.Text = "no. of students in " + cmbLevelNoSec.Text + " section " + cmbSecNoSec.Text + ": " + dgvSearch.Rows.Count;

            

            if (level == "Kinder")
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
            dgvNoSec.DefaultCellStyle.SelectionBackColor = Color.White;
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
            lblResult.Text = "no. of students in " + cmbLevelNoSec.Text + " section " + cmbSecNoSec.Text + ": " + dgvSearch.Rows.Count;
            dgvSearch.DefaultCellStyle.SelectionBackColor = Color.White;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dgvNoSec.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvSearch.DefaultCellStyle.SelectionBackColor = Color.White;
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
            txtSearchNoSec.Focus();
        }

        private void dgvNoSec_Click(object sender, EventArgs e)
        {
            if (dgvNoSec.Rows.Count <= 0)
            {
                return;
            }
            else
            {
                string PK = dgvNoSec.SelectedRows[0].Cells[0].Value.ToString();
                con.Open();
                DataTable dt = new DataTable();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='" + PK + "'", con);
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    string gender = dt.Rows[0].ItemArray[10].ToString();
                    if (gender == "Male")
                    {
                        dgvNoSec.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
                    }
                    else
                    {
                        dgvNoSec.DefaultCellStyle.SelectionBackColor = Color.Pink;
                    }

                }
            }

            lblKey.Text = dgvNoSec.SelectedRows[0].Cells[0].Value.ToString();

            if (cmbSecNoSec.Text != "")
            {
                btnMovetowith.Enabled = true;
                btnCancel.Enabled = true;
            }
            btnMovetowithout.Enabled = false;
        }

        private void txtSearchNoSec_TextChanged(object sender, EventArgs e)
        {
            dgvNoSec.DefaultCellStyle.SelectionBackColor = Color.White;
          
            if (cmbFilter.Text == "Student's name")
            {
                toolTip1.SetToolTip(txtSearchNoSec, "student's name");
                dvNoSec.RowFilter = string.Format("Name LIKE '%{0}%'", txtSearchNoSec.Text);
            }
            else
            {
                toolTip1.SetToolTip(txtSearchNoSec, "student number");
                dvNoSec.RowFilter = string.Format("No LIKE '%{0}%'", txtSearchNoSec.Text);
            }

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

            
            dgvNoSec.DataSource = dvNoSec;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvSearch.DefaultCellStyle.SelectionBackColor = Color.White;
            if (cmbFilter.Text == "Student's name")
            {
                toolTip1.SetToolTip(txtSearch, "student's name");
                dvPerLevel.RowFilter = string.Format("Name LIKE '%{0}%'", txtSearch.Text);
                
            }
            else
            {
                toolTip1.SetToolTip(txtSearch, "student number");
                dvPerLevel.RowFilter = string.Format("No LIKE '%{0}%'", txtSearch.Text);
               
            }

            dgvSearch.DataSource = dvPerLevel;
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

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }

            if (dgvSearch.Rows.Count > 0)
            {
                primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                con.Open();
                DataTable dt = new DataTable();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='"+primarykey+"'", con);
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    string gender = dt.Rows[0].ItemArray[10].ToString();
                    if (gender == "Male")
                    {
                        dgvSearch.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
                    }
                    else
                    {
                        dgvSearch.DefaultCellStyle.SelectionBackColor = Color.Pink;
                    }

                }
                
            }

           
            lblKey.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            btnMovetowith.Enabled = false;
            btnMovetowithout.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void dgvNoSec_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (txtSearchNoSec.Text == "")
            {
                pnlWOSec.Visible = false;
            }
        }

        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (txtSearch.Text == "")
            {
                pnlwith.Visible = false;
            }
        }

        private void frmSectioning_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void dgvm_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Hand;
        }

        private void dgvm_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Default;
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Sectioning")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Sectioning")
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

        private void pnlASS_MouseEnter(object sender, EventArgs e)
        {
            pnlASS.BackColor = Color.CadetBlue;
        }

        private void pnlASS_MouseLeave(object sender, EventArgs e)
        {
            pnlASS.BackColor = Color.DarkSeaGreen;
        }

        private void pbAss_MouseEnter(object sender, EventArgs e)
        {
            pnlASS.BackColor = Color.CadetBlue;
        }

        private void pbAss_MouseLeave(object sender, EventArgs e)
        {
            pnlASS.BackColor = Color.DarkSeaGreen;
        }

        private void lblAss_MouseEnter(object sender, EventArgs e)
        {
            pnlASS.BackColor = Color.CadetBlue;
        }

        private void lblAss_MouseLeave(object sender, EventArgs e)
        {
          
            pnlASS.BackColor = Color.DarkSeaGreen;
            
        }

        private void pnlASS_Click(object sender, EventArgs e)
        {
           
            frmASS assform = new frmASS();
            assform.ShowDialog();
           
        }

        private void pbAss_Click(object sender, EventArgs e)
        {
           
            frmASS assform = new frmASS();
            assform.ShowDialog();
           
        }

        private void lblAss_Click(object sender, EventArgs e)
        {
           
            frmASS assform = new frmASS();
            assform.ShowDialog();
            
           
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
                    casmain.cashlog = seclog;
                    casmain.accesscode = accesscode;
                    casmain.CO = co;
                    casmain.thefac = TheFaculty;
                    casmain.VISITED = VISITED;
                    casmain.viewNotifDue = viewNotifDue;
                    casmain.viewNotifDisc = viewNotifDisc;
                    casmain.viewNotifLate = viewNotifLate;
                    casmain.notifstat = notifstat;
                    casmain.Show();
                }
                if (emptype == "Registrar")
                {
                    frmRegistrarMain regmain = new frmRegistrarMain();
                    this.Hide();
                    regmain.emptype = emptype;
                    regmain.co = co;
                    regmain.reglog = seclog;
                    regmain.accesscode = accesscode;
                    regmain.thefac = TheFaculty;
                    regmain.VISITED = VISITED;
                    regmain.viewNotifDue = viewNotifDue;
                    regmain.viewNotifDisc = viewNotifDisc;
                    regmain.viewNotifLate = viewNotifLate;
                    regmain.notifstat = notifstat;
                    regmain.Show();
                }
                if (emptype == "Principal")
                {
                    frmPrincipalMain pmf = new frmPrincipalMain();
                    this.Hide();
                    pmf.emptype = emptype;
                    pmf.accesscode = accesscode;
                    pmf.co = co;
                    pmf.thefac = TheFaculty;
                    pmf.prinlog = seclog;
                    pmf.VISITED = VISITED;
                    pmf.viewNotifDue = viewNotifDue;
                    pmf.viewNotifDisc = viewNotifDisc;
                    pmf.viewNotifLate = viewNotifLate;
                    pmf.notifstat = notifstat;
                    pmf.Show();
                }
                if (emptype == "Faculty")
                {
                    frmEmpMain empf = new frmEmpMain();
                    this.Hide();
                    empf.emptype = emptype;
                    empf.CO = co;
                    empf.faclog = seclog;
                    empf.accesscode = accesscode;
                    empf.TheFacultyName = TheFaculty;
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
                frmadm.admlog = seclog;
                frmadm.CO = co;
                frmadm.accesscode = accesscode;
                frmadm.TheFaculty = TheFaculty;
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
                formPay.paylog = seclog;
                formPay.CashierOperator = co;
                formPay.accesscode = accesscode;
                formPay.TheFac = TheFaculty;
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
                formStudRec.asslog = seclog;
                formStudRec.accesscode = accesscode;
                formStudRec.thefac = TheFaculty;
                formStudRec.VISITED = VISITED;
                formStudRec.viewNotifDue = viewNotifDue;
                formStudRec.viewNotifDisc = viewNotifDisc;
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
                formstdgrd.grdlog = seclog;
                formstdgrd.accesscode = accesscode;
                formstdgrd.theFacultyName = TheFaculty;
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
                stud.studlog = seclog;
                stud.accesscode = accesscode;
                stud.TheFaculty = TheFaculty;
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
                facf.facinfolog = seclog;
                facf.accesscode = accesscode;
                facf.TheFaculty = TheFaculty;
                facf.VISITED = VISITED;
                facf.viewNotifDue = viewNotifDue;
                facf.viewNotifDisc = viewNotifDisc;
                facf.viewNotifLate = viewNotifLate;
                facf.notifstat = notifstat;
                facf.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Faculty advisory")
            {
                frmFacultyAdvisory frmFacAdv = new frmFacultyAdvisory();
                this.Hide();
                frmFacAdv.emptype = emptype;
                frmFacAdv.co = co;
                frmFacAdv.advlog = seclog;
                frmFacAdv.accesscode = accesscode;
                frmFacAdv.thefac = TheFaculty;
                frmFacAdv.VISITED = VISITED;
                frmFacAdv.viewNotifDue = viewNotifDue;
                frmFacAdv.viewNotifDisc = viewNotifDisc;
                frmFacAdv.viewNotifLate = viewNotifLate;
                frmFacAdv.notifstat = notifstat;
                frmFacAdv.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Sectioning")
            {
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Report")
            {
                frmReport rfac = new frmReport();
                this.Hide();
                rfac.co = co;
                rfac.replog = seclog;
                rfac.emptype = emptype;
                rfac.accesscode = accesscode;
                rfac.theFaculty = TheFaculty;
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
                rsched.schedlog = seclog;
                rsched.emptype = emptype;
                rsched.accesscode = accesscode;
                rsched.TheFaculty = TheFaculty;
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
                about.ablog = seclog;
                about.emptype = emptype;
                about.CO = co;
                about.accesscode = accesscode;
                about.theFaculty = TheFaculty;
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

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cmbFilterNoSec_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
