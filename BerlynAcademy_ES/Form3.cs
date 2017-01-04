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
    public partial class frmEmpMain : Form
    {
         OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");

         public string faclog, TheFacultyName, CO, accesscode, emptype, VISITED, notifstat,activeSY,activeYr;
         public bool isVisited, viewNotifDue, viewNotifDisc, viewNotifLate;

        public frmEmpMain()
        {
            InitializeComponent();
        }

        private void frmEmpMain_Load(object sender, EventArgs e)
        {
            //this.BackColor = Color.FromArgb(0, 0, 25);
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            labelmain.BackColor = Color.Transparent;
            lblLogger.Text = faclog;
            lblLoggerPosition.Text = "Faculty";
            //btnHome.Text = "          " + faclog;
            //MessageBox.Show(TheFacultyName);

            if (VISITED == null)
            {
                VISITED += "   Activity";
            }

            if (isVisited == false)
            {
                if (VISITED.Contains("Activity") == false)
                {
                    VISITED += "   Activity";
                    isVisited = true;
                }

            }
            GetActiveSchoolYear();
            setupactivities();
            setupMENU();
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
                lblsy.Text = activeSY;

                int yr = 0;
                string syfirstterm = activeSY.Substring(3, 4).ToString();
                string sysecondterm = activeSY.Substring(8, 4).ToString();
                if (DateTime.Now.Year.ToString() == syfirstterm || DateTime.Now.Year.ToString() == sysecondterm)
                {
                    yr = Convert.ToInt32(DateTime.Now.Year);
                }
                else
                {
                    yr = Convert.ToInt32(activeYr);
                }

                
                mcd.TodayDate = new DateTime(yr, DateTime.Now.Month, DateTime.Now.Day);
                mcd.SelectionStart = new System.DateTime(yr, DateTime.Now.Month, DateTime.Now.Day);
                mcd.SelectionEnd = new System.DateTime(yr, DateTime.Now.Month, DateTime.Now.Day);
               
            }
        }

        public void setupMENU()
        {
            string sortedAccessCode = "";
            string[]ac = new string[555];
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

            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                dtMenu.Rows.Add("  " + dt2.Rows[0].ItemArray[1].ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                dtMenu.Rows.Add("  " + dt3.Rows[0].ItemArray[1].ToString());
            }
            if (dt4.Rows.Count > 0)
            {
                dtMenu.Rows.Add("  " + dt4.Rows[0].ItemArray[1].ToString());
            }
            if (dt5.Rows.Count > 0)
            {
                dtMenu.Rows.Add("  " + dt5.Rows[0].ItemArray[1].ToString());
            }
            if (dt6.Rows.Count > 0)
            {
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
            dgvm.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        public void setupactivities()
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter daact = new OdbcDataAdapter("Select * from activity_tbl where SY='"+activeSY+"'", con);
            daact.Fill(dt);
            con.Close();
            //string tmp = "";
            if (dt.Rows.Count > 0)
            {
                lvwAct.Columns.Add("", 150, HorizontalAlignment.Left);
                lvwAct.Columns.Add("", 50, HorizontalAlignment.Center);
                lvwAct.Columns.Add("", 550, HorizontalAlignment.Left);

                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    //tmp = dt.Rows[a].ItemArray[1].ToString() + "     ›      " + dt.Rows[a].ItemArray[0].ToString();
                    //lblActs.Text = lblActs.Text + tmp + "\n";
                    DateTime assdate = Convert.ToDateTime(dt.Rows[a].ItemArray[1].ToString());
                    DateTime dateToday = Convert.ToDateTime(mcd.TodayDate);
                    ListViewItem itm = new ListViewItem();
                    if (assdate.ToLongDateString() == dateToday.ToLongDateString())
                    {
                        itm.Font = new Font("Arial",12,FontStyle.Bold);
                    }
                    itm.Text = dt.Rows[a].ItemArray[1].ToString();
                    itm.SubItems.Add("-");
                    itm.SubItems.Add(dt.Rows[a].ItemArray[0].ToString());
                    lvwAct.Items.Add(itm);
                }
            }
            else
            {
                lvwAct.Columns.Add("", 550, HorizontalAlignment.Center);
                ListViewItem itm = new ListViewItem();
                itm.Text = "No Upcoming Activities...";
                lvwAct.Items.Add(itm);
                //lblActs.Text = "no upcoming activities...";
            }

            lblActs.Text += "\n" + "Today is: " + DateTime.Now.ToLongDateString();
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
            frmEmpAbout about = new frmEmpAbout();
            this.Hide();
            about.ablog = faclog;
            about.emptype = "faculty";
            about.theFaculty = TheFacultyName;
            about.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void frmEmpMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnStudI_Click(object sender, EventArgs e)
        {
            frmStudInfo stud = new frmStudInfo();
            this.Hide();
            stud.emptype = "faculty";
            stud.studlog = faclog;
            stud.TheFaculty = TheFacultyName;
            stud.Show();
        }

        private void btnFac_Click(object sender, EventArgs e)
        {
            frmFacInfo facf = new frmFacInfo();
            this.Hide();
            facf.emptype = "faculty";
            facf.facinfolog = faclog;
            facf.TheFaculty = TheFacultyName;
            facf.Show();
        }

        private void btnAdm_Click(object sender, EventArgs e)
        {
            frmAdmission formAdm = new frmAdmission();
            this.Hide();
            formAdm.admlog = faclog;
            formAdm.TheFaculty = TheFacultyName;
            formAdm.Show();
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            frmStdGrd formstd = new frmStdGrd();
            this.Hide();
            formstd.grdlog = faclog;
            formstd.theFacultyName = TheFacultyName;
            formstd.Show();
        }

        private void btnFacRep_Click(object sender, EventArgs e)
        {
            frmReport rfac = new frmReport();
            this.Hide();
            rfac.replog = faclog;
            rfac.emptype = "faculty";
            rfac.theFaculty = TheFacultyName;
            rfac.Show();
        }

        private void btnSectioning_Click(object sender, EventArgs e)
        {
            frmSectioning sectioningfrm = new frmSectioning();
            this.Hide();
            sectioningfrm.seclog = faclog;
            sectioningfrm.TheFaculty = TheFacultyName;
            sectioningfrm.Show();
        }

        private void dgvm_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Hand;
            
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Activity")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gainsboro;
            }
        }

        private void dgvm_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Default;
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Activity")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_Click(object sender, EventArgs e)
        {
            if (dgvm.Rows.Count < 0)
            {
                 return;
            }
        }

        private void btnFacPayment_Click(object sender, EventArgs e)
        {

        }

        private void btnFacFacAdvisory_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void dgvm_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Activity")
            {
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Admission")
            {
                frmAdmission formAdm = new frmAdmission();
                this.Hide();
                formAdm.emptype = emptype;
                formAdm.CO = CO;
                formAdm.admlog = faclog;
                formAdm.accesscode = accesscode;
                formAdm.TheFaculty = TheFacultyName;
                formAdm.VISITED = VISITED;
                formAdm.viewNotifDue = viewNotifDue;
                formAdm.viewNotifDisc = viewNotifDisc;
                formAdm.viewNotifLate = viewNotifLate;
                formAdm.notifstat = notifstat;
                formAdm.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Payment")
            {
                frmPayment formPay = new frmPayment();
                this.Hide();
                formPay.emptype = emptype;
                formPay.CashierOperator = CO;
                formPay.paylog = faclog;
                formPay.accesscode = accesscode;
                formPay.TheFac = TheFacultyName;
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
                formStudRec.co = CO;
                formStudRec.asslog = faclog;
                formStudRec.accesscode = accesscode;
                formStudRec.thefac = TheFacultyName;
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
                formstdgrd.CO = CO;
                formstdgrd.grdlog = faclog;
                formstdgrd.accesscode = accesscode;
                formstdgrd.theFacultyName = TheFacultyName;
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
                stud.CO = CO;
                stud.studlog = faclog;
                stud.accesscode = accesscode;
                stud.TheFaculty = TheFacultyName;
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
                facf.CO = CO;
                facf.facinfolog = faclog;
                facf.accesscode = accesscode;
                facf.TheFaculty = TheFacultyName;
                facf.viewNotifDue = viewNotifDue;
                facf.VISITED = VISITED;
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
                frmFacAdv.co = CO;
                frmFacAdv.advlog = faclog;
                frmFacAdv.accesscode = accesscode;
                frmFacAdv.thefac = TheFacultyName;
                frmFacAdv.VISITED = VISITED;
                frmFacAdv.viewNotifDue = viewNotifDue;
                frmFacAdv.viewNotifDisc = viewNotifDisc;
                frmFacAdv.viewNotifLate = viewNotifLate;
                frmFacAdv.notifstat = notifstat;
                frmFacAdv.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Sectioning")
            {
                frmSectioning frmSec = new frmSectioning();
                this.Hide();
                frmSec.emptype = emptype;
                frmSec.co = CO;
                frmSec.seclog = faclog;
                frmSec.accesscode = accesscode;
                frmSec.TheFaculty = TheFacultyName;
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
                rfac.co = CO;
                rfac.replog = faclog;
                rfac.emptype = emptype;
                rfac.accesscode = accesscode;
                rfac.theFaculty = TheFacultyName;
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
                rsched.CO = CO;
                rsched.schedlog = faclog;
                rsched.emptype = emptype;
                rsched.accesscode = accesscode;
                rsched.TheFaculty = TheFacultyName;
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
                about.ablog = faclog;
                about.emptype = emptype;
                about.CO = CO;
                about.accesscode = accesscode;
                about.theFaculty = TheFacultyName;
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
    }
}
