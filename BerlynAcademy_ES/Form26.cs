using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Drawing.Printing;

namespace BerlynAcademy_ES
{
    public partial class frmReport : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string replog, emptype,theFaculty,co,activeSY,accesscode,preparedby,IDofFacultyWhoLog,TheAdvisoryClass,nc,ncpr,selectedLev;
        DataView dvs;
        public string syr, snu, lna, fna, mna, lev, sec, adv, mop, VISITED, notifstat;
        public bool isVisited, viewNotifDue, viewNotifDisc, viewNotifLate;
        //MasterList Report
        public int cntrowsMale_ml = 0, cntrowsFem_ml = 0;
        int totalnumberMale_ml = 0;
        int itemperpageMale_ml = 0;
        int totalnumberFem_ml = 0;
        int itemperpageFem_ml = 0;
        int pagecount = 1;
        int ofpagecount = 1;
        //-----------------------------------------------
      
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            GetActiveSY();

            //this.BackColor = Color.FromArgb(0, 0, 25);
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //pnlHead.BackColor = Color.FromArgb(244,194,13);

             //btnRegForm.BackColor = Color.FromArgb(49, 79, 142);
             //btnStudClass.BackColor = Color.FromArgb(49, 79, 142);
             //btnWosec.BackColor = Color.FromArgb(49, 79, 142);
             //btnMasterList.BackColor = Color.FromArgb(49, 79, 142);
             //btnESTY.BackColor = Color.FromArgb(49, 79, 142);

            //MessageBox.Show(theFaculty);
           

            if (emptype == "Faculty")
            {
                pnlMenuPrin.Visible = false;
                pnlMenuFac.Visible = true;
                pnlMenuFac.Size = new System.Drawing.Size(263, 757);
                pnlMenuFac.Location = new Point(0, 0);
                lblLoggerFac.Text = replog;
                
               // btnHomeFac.Text = "          " + replog;
               
            }
            if (emptype == "principal")
            {
                pnlMenuFac.Visible = false;
                pnlMenuPrin.Visible = true;
                pnlMenuPrin.Size = new System.Drawing.Size(263, 757);
                pnlMenuPrin.Location = new Point(0, 0);
                lblLogPrin.Text = replog;
              
                //btnHomePrin.Text = "          " + replog;
            }

            if (isVisited == false)
            {
                if (VISITED.Contains("Report") == false)
                {
                    VISITED += "   Report";
                    isVisited = true;
                }
            }

            setupLevelList();
            pnlMenuFac.Visible = true;
            lblLoggerFac.Text = replog;
            lblLoggerFacPosition.Text = emptype;
            setupMENU();
            getFacultyIdWhoLog();

        }

        public void getFacultyIdWhoLog()
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where (select concat(firstname,' ',middlename,' ',lastname))='"+theFaculty+"'", con);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                IDofFacultyWhoLog = dt.Rows[0].ItemArray[0].ToString();
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
                cmbLevML.Items.Clear();
                cmbLevSWS.Items.Clear();
                cmbLevSIAC.Items.Clear();
                cmbLevel.Items.Clear();
                
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbLevML.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                    cmbLevSWS.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                    cmbLevSIAC.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                    cmbLevel.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                }
                
            }
        }

        public void setupLevelPassedAndFailed()
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select level from level_tbl", con);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbPFS.Items.Clear();
                if (emptype != "Faculty")
                {
                    for (int u = 0; u < dt.Rows.Count; u++)
                    {
                        cmbPFS.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                    }
                }
                else
                {
                    setupgrade();
                }
            }
        }

        public void setupgrade()
        {
            cmbPFS.Items.Clear();

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Kinder" + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 1" + "'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            OdbcDataAdapter da2 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 2" + "'", con);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            OdbcDataAdapter da3 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 3" + "'", con);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);

            OdbcDataAdapter da4 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 4" + "'", con);
            DataTable dt4 = new DataTable();
            da4.Fill(dt4);

            OdbcDataAdapter da5 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 5" + "'", con);
            DataTable dt5 = new DataTable();
            da5.Fill(dt5);

            OdbcDataAdapter da6 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 6" + "'", con);
            DataTable dt6 = new DataTable();
            da6.Fill(dt6);

            OdbcDataAdapter da7 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 7" + "'", con);
            DataTable dt7 = new DataTable();
            da7.Fill(dt7);

            OdbcDataAdapter da8 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 8" + "'", con);
            DataTable dt8 = new DataTable();
            da8.Fill(dt8);

            OdbcDataAdapter da9 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 9" + "'", con);
            DataTable dt9 = new DataTable();
            da9.Fill(dt9);

            OdbcDataAdapter da0 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFaculty + "'and level='" + "Grade 10" + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);

            con.Close();

            if (dt.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Kinder"); }

            }
            if (dt1.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 1"); }

            }
            if (dt2.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt2.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 2"); }

            }
            if (dt3.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt3.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 3"); }

            }
            if (dt4.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt4.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 4"); }

            }
            if (dt5.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt5.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 5"); }

            }
            if (dt6.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt6.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 6"); }

            }
            if (dt7.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt7.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 7"); }

            }
            if (dt8.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt8.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 8"); }

            }
            if (dt9.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt9.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 9"); }

            }
            if (dt0.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt0.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbPFS.Items.Add("Grade 10"); }

            }

        }

        public void setupLevelBasedonAdvClass()
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno='"+IDofFacultyWhoLog+"'", con);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbLevSIAC.Items.Clear(); cmbSecSIAC.Items.Clear();
                TheAdvisoryClass = dt.Rows[0].ItemArray[15].ToString();
                cmbLevSIAC.Items.Add(dt.Rows[0].ItemArray[14].ToString());
                cmbSecSIAC.Items.Add(TheAdvisoryClass);
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

            int getReportIndex = 1;
            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                getReportIndex++;
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                getReportIndex++;
                dtMenu.Rows.Add("  " + dt2.Rows[0].ItemArray[1].ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                getReportIndex++;
                dtMenu.Rows.Add("  " + dt3.Rows[0].ItemArray[1].ToString());
            }
            if (dt4.Rows.Count > 0)
            {
                getReportIndex++;
                dtMenu.Rows.Add("  " + dt4.Rows[0].ItemArray[1].ToString());
            }
            if (dt5.Rows.Count > 0)
            {
                getReportIndex++;
                dtMenu.Rows.Add("  " + dt5.Rows[0].ItemArray[1].ToString());
            }
            if (dt6.Rows.Count > 0)
            {
                getReportIndex++;
                dtMenu.Rows.Add("  " + dt6.Rows[0].ItemArray[1].ToString());
            }
            if (dt7.Rows.Count > 0)
            {
                getReportIndex++;
                dtMenu.Rows.Add("  " + dt7.Rows[0].ItemArray[1].ToString());
            }
            if (dt8.Rows.Count > 0)
            {
                getReportIndex++;
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
            dgvm.Rows[getReportIndex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        public void GetActiveSY()
        {
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);

            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString(); }
        }

        private void btnActPrin_Click(object sender, EventArgs e)
        {
            frmPrincipalMain pmf = new frmPrincipalMain();
            this.Hide();
            pmf.prinlog = replog;
            pmf.Show();
        }

        private void btnStudIPrin_Click(object sender, EventArgs e)
        {
            frmStudInfo sif = new frmStudInfo();
            this.Hide();
            sif.studlog = replog;
            sif.emptype = "principal";
            sif.Show();
        }

        private void btnFIPrin_Click(object sender, EventArgs e)
        {
            frmFacInfo fif = new frmFacInfo();
            this.Hide();
            fif.facinfolog = replog;
            fif.emptype = "principal";
            fif.Show();
        }

        private void btnAbtPrin_Click(object sender, EventArgs e)
        {
            frmEmpAbout eaf = new frmEmpAbout();
            this.Hide();
            eaf.ablog = replog;
            eaf.emptype = "principal";
            eaf.Show();
        }

        private void btnHomePrin_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin logf = new frmEmpLogin();
            this.Hide();
            logf.Show();
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

        private void frmReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin logf = new frmEmpLogin();
            this.Hide();
            logf.Show();
        }

        private void btnActFac_Click(object sender, EventArgs e)
        {
            frmEmpMain mf = new frmEmpMain();
            this.Hide();
            mf.faclog = replog;
            mf.TheFacultyName = theFaculty;
            mf.Show();
        }

        private void btnSIFac_Click(object sender, EventArgs e)
        {
            frmStudInfo sif = new frmStudInfo();
            this.Hide();
            sif.studlog = replog;
            sif.emptype = "faculty";
            sif.TheFaculty = theFaculty;
            sif.CO = co;
            sif.Show();
        }

        private void btnFIFac_Click(object sender, EventArgs e)
        {
            frmFacInfo fif = new frmFacInfo();
            this.Hide();
            fif.facinfolog = replog;
            fif.emptype = "faculty";
            fif.TheFaculty = theFaculty;
            fif.CO = co;
            fif.Show();
        }

        private void btnAbtFac_Click(object sender, EventArgs e)
        {
            frmEmpAbout eaf = new frmEmpAbout();
            this.Hide();
            eaf.ablog = replog;
            eaf.emptype = "faculty";
            eaf.theFaculty = theFaculty;
            eaf.Show();
        }

        private void btnHomeFac_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin logf = new frmEmpLogin();
            this.Hide();
            logf.Show();
        }

        private void btnAdmFac_Click(object sender, EventArgs e)
        {
            frmAdmission formAdm = new frmAdmission();
            this.Hide();
            formAdm.admlog = replog;
            formAdm.TheFaculty = theFaculty;
            formAdm.Show();
        }

        private void btnGradeFac_Click(object sender, EventArgs e)
        {
            frmStdGrd grdf = new frmStdGrd();
            this.Hide();
            grdf.grdlog = replog;
            grdf.theFacultyName = theFaculty;
            grdf.Show();
        }

        private void btnRegForm_Click(object sender, EventArgs e)
        {
            pnlHome.Visible = false;
            pnlTheESTY.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheRF.Visible = true;
          
            pnlTheRF.Location = new Point(297, 65);
            setupStudents();

            if (dgvSearch.Rows.Count > 0)
            {
                setupRFContent(dgvSearch.Rows[0].Cells[0].Value.ToString());
            }
        }

        public void PrintRF(object sender, PrintPageEventArgs e)
        {
            // String format
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Near;

            // Create font and brush.
            Font df1 = new Font("Arial", 12, FontStyle.Bold);
            Font df4 = new Font("Arial", 11, (FontStyle.Bold));
            Font df2 = new Font("Arial", 12, FontStyle.Regular);
            Font df3 = new Font("Arial", 11, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

             // Create a new pen.
            Pen pen1 = new Pen(Brushes.Black);
            pen1.Width = 1F;
            pen1.LineJoin = System.Drawing.Drawing2D.LineJoin.Miter;

            //REPORT'S HEADER
            e.Graphics.Clear(Color.White);
            e.Graphics.DrawString("Berlyn Academy", df1, Brushes.Black, 400, 50, sf);
            e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df3, Brushes.Black, 400, 75, sf);
            e.Graphics.DrawString("REGISTRATION FORM", df4, Brushes.Black, 400, 100, sf);

            e.Graphics.DrawRectangle(pen1,50,140,750,930);
            e.Graphics.DrawString("Student no: ", df4, Brushes.Black, 60, 160, sf1);
            e.Graphics.DrawString(snu, df3, Brushes.Black, 160, 160, sf1);

            e.Graphics.DrawString("School year: ", df4, Brushes.Black, 550, 160, sf1);
            e.Graphics.DrawString(syr, df3, Brushes.Black, 660, 160, sf1);

            e.Graphics.DrawString("Lastname: ", df4, Brushes.Black, 60, 220, sf1);
            e.Graphics.DrawString(lna, df3, Brushes.Black, 150, 220, sf1);

            e.Graphics.DrawString("Firstname: ", df4, Brushes.Black, 275, 220, sf1);
            e.Graphics.DrawString(fna, df3, Brushes.Black, 365, 220, sf1);

            e.Graphics.DrawString("Middlename: ", df4, Brushes.Black, 565, 220, sf1);
            e.Graphics.DrawString(mna, df3, Brushes.Black, 670, 220, sf1);

            e.Graphics.DrawString("Level: ", df4, Brushes.Black, 60, 250, sf1);
            e.Graphics.DrawString(lev, df3, Brushes.Black, 120, 250, sf1);

            e.Graphics.DrawString("Section: ", df4, Brushes.Black, 200, 250, sf1);
            e.Graphics.DrawString(sec, df3, Brushes.Black, 275, 250, sf1);

            e.Graphics.DrawString("Adviser: ", df4, Brushes.Black, 450, 250, sf1);
            e.Graphics.DrawString(adv, df3, Brushes.Black, 545, 250, sf1);


            e.Graphics.DrawString("Mode of Payment: ", df4, Brushes.Black, 60, 280, sf1);
            e.Graphics.DrawString(mop, df3, Brushes.Black, 205, 280, sf1);
        }

        public void setupStudents()
        {
            int year = Convert.ToInt32(DateTime.Now.Year);
            int upc = year+1;
            string sy = year+"-"+upc;

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select (select concat(lname,' ',fname,' ',mname)) as 'Student',studno from stud_tbl where syenrolled='" + sy + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dvs = new DataView(dt);
            dgvSearch.DataSource = dvs;
            con.Close();
            dgvSearch.Columns[0].Width = 196;
           
        }
        private void btnESTY_Click(object sender, EventArgs e)
        {
            setupLevelList();
            pnlHome.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlTheESTY.Visible = true;
           
          
            pnlTheESTY.Location = new Point(297, 65);
            //pdESTY.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PageContent);
           // ppcESTY.Document = pdESTY;
        }

        public void PageContent(Object sender, PrintPageEventArgs e)
        {
           
        }
    
        private void btnStudClass_Click(object sender, EventArgs e)
        {
            if (emptype != "Faculty")
            {
                setupLevelList();
            }
            else
            {
                setupLevelBasedonAdvClass();
            }
            pnlHome.Visible = false;
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheML.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlTheSIAC.Visible = true;
            pnlTheSIAC.Location = new Point(297, 65);

            //pdSIAC.PrintPage+=new PrintPageEventHandler(pgSIAC);
            //ppcSIAC.Document = pdSIAC;
        }

        public void pgSIAC(object sender, PrintPageEventArgs e)
        {
           
        }

        private void btnMasterList_Click(object sender, EventArgs e)
        {
            setupPreparedPersons();
            setupLevelList();
            pnlHome.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheESTY.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlTheML.Visible = true;
            pnlTheML.Location = new Point(297, 65);

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlHome.Visible = true;
            
        }

        private void btnWosec_Click(object sender, EventArgs e)
        {
            setupPreparedPersons();
            setupLevelList();
            pnlHome.Visible = false;
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheML.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlTheSWS.Visible = true;
            pnlTheSWS.Location = new Point(297, 65);
            
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;
            //selectedLev = cmbLevML.Text;
            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            //pdESTY.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PageContent);
            ppcESTY.Document = pdESTY;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvs.RowFilter = string.Format("studno LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvs;

            if (dgvSearch.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text != "")
            {
                pnlnotify.Visible = true;
                lblnote.Text = "no student found.";
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text == "")
            {
                pnlnotify.Visible = true;
                lblnote.Text = "no student found!";
            }
        }

        private void btnBackRF_Click(object sender, EventArgs e)
        {
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlHome.Visible = true;
            
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() == "")
            { 
                return;
            }

            string name = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            setupRFContent(name);

           

        }

        public void setupRFContent(string studname)
        {
            con.Open();
            OdbcDataAdapter das = new OdbcDataAdapter("Select*from stud_tbl where (select concat(lname,' ',fname,' ',mname))='" + studname + "'", con);
            DataTable dts = new DataTable();
            das.Fill(dts);
            con.Close();

            if (dts.Rows.Count > 0)
            {
                snu = dts.Rows[0].ItemArray[0].ToString();
                syr = dts.Rows[0].ItemArray[23].ToString();
                lna = dts.Rows[0].ItemArray[3].ToString();
                fna = dts.Rows[0].ItemArray[1].ToString();
                mna = dts.Rows[0].ItemArray[2].ToString();
                lev = dts.Rows[0].ItemArray[4].ToString();
                sec = dts.Rows[0].ItemArray[5].ToString();
                mop = dts.Rows[0].ItemArray[22].ToString();

                pdRF.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintRF);
                ppcRF.Document = pdRF;

                if (lev != "" && sec != "")
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select (concat(firstname,' ',middlename,' ',lastname))from employees_tbl where grade='" + lev + "'and advisory='" + sec + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        adv = dt.Rows[0].ItemArray[0].ToString();
                    }
                    else
                    {
                        adv = "";
                    }
                }
                else
                {
                    adv = "";
                }

            }
        }

        private void btnWosec_MouseHover(object sender, EventArgs e)
        {
            btnWosec.ForeColor = Color.LightGray;
        }

        private void btnWosec_MouseLeave(object sender, EventArgs e)
        {
            btnWosec.ForeColor = Color.White;
        }

        private void btnStudClass_MouseHover(object sender, EventArgs e)
        {
            btnStudClass.ForeColor = Color.LightGray;
        }

        private void btnStudClass_MouseLeave(object sender, EventArgs e)
        {
            btnStudClass.ForeColor = Color.White;
        }

        private void btnESTY_MouseHover(object sender, EventArgs e)
        {
            btnESTY.ForeColor = Color.LightGray;
        }

        private void btnESTY_MouseLeave(object sender, EventArgs e)
        {
            btnESTY.ForeColor = Color.White;
        }

        private void btnMasterList_MouseHover(object sender, EventArgs e)
        {
            btnMasterList.ForeColor = Color.LightGray;
        }

        private void btnMasterList_MouseLeave(object sender, EventArgs e)
        {
            btnMasterList.ForeColor = Color.White;
        }

        private void dgvSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbLevSIAC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (emptype != "Faculty") { setupSectionSIAC(cmbLevSIAC.Text); };
        }

        public void setupSectionSIAC(string keystring)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select section from section_tbl where level='" + keystring + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbSecSIAC.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSecSIAC.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        private void btnBackSIAC_Click(object sender, EventArgs e)
        {
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlHome.Visible = true;
            
        }

        private void cmbSecSIAC_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;

            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            //pdSIAC.PrintPage+=new PrintPageEventHandler(pgSIAC);
            ppcSIAC.Document = pdSIAC;
        }

        private void btnBackSWS_Click(object sender, EventArgs e)
        {
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlHome.Visible = true;
          
        }

        private void cmbLevSWS_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;
           
            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            //pdSWS.PrintPage+=new PrintPageEventHandler(pgSWS);
            ppcSWS.Document = pdSWS;
        }

        public void pgSWS(object sender, PrintPageEventArgs e)
        {
            
        }

        private void cmbLevML_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;
            selectedLev = cmbLevML.Text;
            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            //pdML.PrintPage+=new PrintPageEventHandler(pgML);
            ppcML.Document = pdML;
        }

        public void pgML(object sender, PrintPageEventArgs e)
        {
            
        }

        private void btnBackML_Click(object sender, EventArgs e)
        {
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlHome.Visible = true;
            
        }

        private void btnPrintSIAC_Click(object sender, EventArgs e)
        {
            pdlgPrint.ShowDialog();
        }

        private void btnPrintML_Click(object sender, EventArgs e)
        {
            pdlgPrint.ShowDialog();
        }

        private void btnPrintSWS_Click(object sender, EventArgs e)
        {
            pdlgPrint.ShowDialog();
        }

        private void btnPrintESTY_Click(object sender, EventArgs e)
        {
            pdlgPrint.ShowDialog();
        }

        private void btnPrintRF_Click(object sender, EventArgs e)
        {
            pdlgPrint.ShowDialog();
        }

        private void btnSectioning_Click(object sender, EventArgs e)
        {
            frmSectioning sectioningfrm = new frmSectioning();
          
            sectioningfrm.seclog = replog;
            sectioningfrm.TheFaculty = theFaculty;
            sectioningfrm.Show();
            this.Hide();
        }

        private void btnFacAdv_Click(object sender, EventArgs e)
        {
            frmFacultyAdvisory faf = new frmFacultyAdvisory();
       
            faf.advlog = replog;
            faf.Show();
            this.Hide();
        }

        private void dgvm_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Hand;
        }

        private void dgvm_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Default;
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Report")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Report")
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

        private void cmbPrepared_SelectedIndexChanged(object sender, EventArgs e)
        {
            pdML.PrintPage += new PrintPageEventHandler(pgML);
            ppcML.Document = pdML;
        }

        public void setupPreparedPersons()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select (concat(firstname,' ',middlename,' ',lastname))from employees_tbl where position='" +"faculty" + "'or position='" + "principal" + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbPrepared.Items.Clear();
                cmbPreparedSWS.Items.Clear();
                cmbPrepESTY.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbPrepared.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                    cmbPreparedSWS.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                    cmbPrepESTY.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        private void cmbPreparedSWS_SelectedIndexChanged(object sender, EventArgs e)
        {
            pdSWS.PrintPage += new PrintPageEventHandler(pgSWS);
            ppcSWS.Document = pdSWS;
        }

        private void cmbPrepESTY_SelectedIndexChanged(object sender, EventArgs e)
        {
            pdESTY.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PageContent);
            ppcESTY.Document = pdESTY;
        }

        private void btnPrevESTY_Click(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;

            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            pprevDlg.Document = pdESTY;
            pprevDlg.FindForm().StartPosition = FormStartPosition.CenterScreen;
            pprevDlg.FindForm().Size = new System.Drawing.Size(1000, 640);
            pprevDlg.FindForm().Text = "Print preview - Registered enrolees this School year";
            pprevDlg.ShowDialog();
        }

        private void btnPrevML_Click(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;
           
            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            pprevDlg.Document = pdML;
            pprevDlg.FindForm().StartPosition = FormStartPosition.CenterScreen;
            pprevDlg.FindForm().Size = new System.Drawing.Size(1000, 640);
            pprevDlg.FindForm().Text = "Print preview - Students Master list";
            pprevDlg.ShowDialog();
        }

        private void btnPrevSIAC_Click(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;

            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            pprevDlg.Document = pdSIAC;
            pprevDlg.FindForm().StartPosition = FormStartPosition.CenterScreen;
            pprevDlg.FindForm().Size = new System.Drawing.Size(1000, 640);
            pprevDlg.FindForm().Text = "Print preview - Students in a Class";
            pprevDlg.ShowDialog();
        }

        private void btnPrevSWS_Click(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;

            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            pprevDlg.Document = pdSWS;
            pprevDlg.FindForm().StartPosition = FormStartPosition.CenterScreen;
            pprevDlg.FindForm().Size = new System.Drawing.Size(1000, 640);
            pprevDlg.FindForm().Text = "Print preview - Students without Section";
            pprevDlg.ShowDialog();
        }

        private void btnPFS_MouseHover(object sender, EventArgs e)
        {
            btnPFS.ForeColor = Color.LightGray;
        }

        private void btnPFS_MouseLeave(object sender, EventArgs e)
        {
            btnPFS.ForeColor = Color.White;
        }

        private void btnGS_MouseHover(object sender, EventArgs e)
        {
            btnGS.ForeColor = Color.LightGray;
        }

        private void btnGS_MouseLeave(object sender, EventArgs e)
        {
            btnGS.ForeColor = Color.White;
        }

        private void btnPFS_Click(object sender, EventArgs e)
        {
            setupLevelPassedAndFailed();
            pnlHome.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheGS.Visible = false;
            pnlTheESTY.Visible = false;
            pnlThePFS.Visible = true;

            pnlThePFS.Location = new Point(297, 65);
            
            //pdPFS.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pcpfs);
            //ppcPFS.Document = pdPFS;
        }

        public void pcpfs(object sender, PrintPageEventArgs e)
        {
            
        }

        private void btnGS_Click(object sender, EventArgs e)
        {
            pnlHome.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheESTY.Visible = false;
            pnlThePFS.Visible = false;
            pnlTheGS.Visible = true;

            pnlTheGS.Location = new Point(297, 65);

            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;

            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            //pdGS.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pcgs);
            ppcGS.Document = pdGS;
        }

        public void pcgs(object sender, PrintPageEventArgs e)
        {
            
        }

       
        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void cmbPFS_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;

            setupSectionPFS(cmbPFS.Text);
            cmbRemPFS.Items.Clear();
            cmbRemPFS.Items.Add("Passed");
            cmbRemPFS.Items.Add("Failed");
            cmbGrading.SelectedIndex = -1;
            cmbAve.SelectedIndex = -1;
            setup_VIEW(cmbPFS.Text);
            cmbSubs.Enabled = true;
            cmbGrading.Enabled = true;
            cmbAve.Enabled = true;

        }

        public void setup_VIEW(string level)
        {
            cmbGrading.SelectedIndex = -1;
            cmbAve.SelectedIndex = -1;
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select subject as 'Subject' from subject_tbl where level='" + level + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbSubs.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSubs.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        public void setupSectionPFS(string keystring)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select section from section_tbl where level='" + keystring + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbSecPFS.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSecPFS.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        private void btnBackPFS_Click(object sender, EventArgs e)
        {
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlHome.Visible = true;
            
        }

        private void btnBackGS_Click(object sender, EventArgs e)
        {
            pnlTheESTY.Visible = false;
            pnlTheRF.Visible = false;
            pnlTheSIAC.Visible = false;
            pnlTheSWS.Visible = false;
            pnlTheML.Visible = false;
            pnlTheGS.Visible = false;
            pnlThePFS.Visible = false;
            pnlHome.Visible = true;
            
        }

        private void btnPrintPFS_Click(object sender, EventArgs e)
        {
            pdlgPrint.ShowDialog();
        }

        private void btnPrintGS_Click(object sender, EventArgs e)
        {
            pdlgPrint.ShowDialog();
        }

        private void btnPrevPFS_Click(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;

            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            pprevDlg.Document = pdPFS;
            pprevDlg.FindForm().StartPosition = FormStartPosition.CenterScreen;
            pprevDlg.FindForm().Size = new System.Drawing.Size(1000, 640);
            pprevDlg.FindForm().Text = "Print preview - Passed and Failed Students";
            pprevDlg.ShowDialog();
        }

        private void btnPrevGS_Click(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;

            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            pprevDlg.Document = pdGS;
            pprevDlg.FindForm().StartPosition = FormStartPosition.CenterScreen;
            pprevDlg.FindForm().Size = new System.Drawing.Size(1000, 640);
            pprevDlg.FindForm().Text = "Print preview - List of Graduating Students";
            pprevDlg.ShowDialog();
        }

        private void cmbSecPFS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRemPFS.Text != "")
            {
                itemperpageMale_ml = totalnumberMale_ml = 0;
                itemperpageFem_ml = totalnumberFem_ml = 0;
                cntrowsFem_ml = 0; cntrowsMale_ml = 0;
                pagecount = 1; ofpagecount = 1;

                ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
                = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

                ppcPFS.Document = pdPFS;
            }
        }

        private void cmbRemPFS_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemperpageMale_ml = totalnumberMale_ml = 0;
            itemperpageFem_ml = totalnumberFem_ml = 0;
            cntrowsFem_ml = 0; cntrowsMale_ml = 0;
            pagecount = 1; ofpagecount = 1;

            ((ToolStripButton)((ToolStrip)pprevDlg.Controls[1]).Items[0]).Enabled
            = false;//disable the direct print from printpreview.as when we click that Print button PrintPage event fires again.

            //pdPFS.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pcpfs);
            ppcPFS.Document = pdPFS;
            //cmbSubs.Enabled = true;
            //cmbGrading.Enabled = true;
            //cmbAve.Enabled = true;
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
                    casmain.cashlog = replog;
                    casmain.accesscode = accesscode;
                    casmain.CO = co;
                    casmain.thefac = theFaculty;
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
                    regmain.reglog = replog;
                    regmain.accesscode = accesscode;
                    regmain.thefac = theFaculty;
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
                    pmf.thefac = theFaculty;
                    pmf.prinlog = replog;
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
                    empf.faclog = replog;
                    empf.accesscode = accesscode;
                    empf.TheFacultyName = theFaculty;
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
                frmadm.admlog = replog;
                frmadm.CO = co;
                frmadm.accesscode = accesscode;
                frmadm.TheFaculty = theFaculty;
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
                formPay.paylog = replog;
                formPay.CashierOperator = co;
                formPay.accesscode = accesscode;
                formPay.TheFac = theFaculty;
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
                formStudRec.asslog = replog;
                formStudRec.accesscode = accesscode;
                formStudRec.thefac = theFaculty;
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
                formstdgrd.grdlog = replog;
                formstdgrd.accesscode = accesscode;
                formstdgrd.theFacultyName = theFaculty;
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
                stud.studlog = replog;
                stud.accesscode = accesscode;
                stud.TheFaculty = theFaculty;
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
                facf.facinfolog = replog;
                facf.accesscode = accesscode;
                facf.TheFaculty = theFaculty;
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
                frmFacAdv.advlog = replog;
                frmFacAdv.accesscode = accesscode;
                frmFacAdv.thefac = theFaculty;
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
                frmSec.co = co;
                frmSec.seclog = replog;
                frmSec.accesscode = accesscode;
                frmSec.TheFaculty = theFaculty;
                frmSec.VISITED = VISITED;
                frmSec.viewNotifDue = viewNotifDue;
                frmSec.viewNotifDisc = viewNotifDisc;
                frmSec.viewNotifLate = viewNotifLate;
                frmSec.notifstat = notifstat;
                frmSec.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Report")
            {
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Scheduling")
            {
                frmSched rsched = new frmSched();
                this.Hide();
                rsched.CO =co;
                rsched.schedlog = replog;
                rsched.emptype = emptype;
                rsched.accesscode = accesscode;
                rsched.TheFaculty = theFaculty;
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
                about.ablog = replog;
                about.emptype = emptype;
                about.CO = co;
                about.accesscode = accesscode;
                about.theFaculty = theFaculty;
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

        private void pnlType_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pdML_PrintPage(object sender, PrintPageEventArgs e)
        {
            int current = Convert.ToInt32(DateTime.Now.Year);
            int upcoming = current + 1;
            string sy = activeSY;
            int ymstart = 0;
            int yfstart = 0;

            // String format
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Near;

            // Create font and brush.
            Font df1 = new Font("Arial", 12, FontStyle.Bold);
            Font df4 = new Font("Arial", 11, (FontStyle.Bold));
            Font df2 = new Font("Arial", 12, FontStyle.Regular);
            Font df3 = new Font("Arial", 11, FontStyle.Regular);
            Font df5 = new Font("Arial", 8, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            //REPORT'S HEADER
            e.Graphics.Clear(Color.White);

            Rectangle r = new Rectangle(100, 55, 100,95);
            Image newImage = Image.FromFile(@"C:\Users\valued client\Documents\Visual Studio 2010\Projects\1 - THESIS\berlyn.bmp");
            e.Graphics.DrawImage(newImage, r);

            e.Graphics.DrawString("Berlyn Academy", df1, Brushes.Black, 420, 65, sf);
            e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df3, Brushes.Black, 420, 85, sf);
            e.Graphics.DrawString("STUDENTS MASTER LIST", df4, Brushes.Black, 420, 115, sf);
            e.Graphics.DrawString("" + sy, df3, Brushes.Black, 420, 135, sf);
            e.Graphics.DrawString("Level: " + cmbLevML.Text, df4, Brushes.Black, 361, 155, sf1);

            e.Graphics.DrawString("Printed Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), df5, Brushes.Black, 575, 1005, sf1);
            //==========================
            //RETRIEVE DATA FOR THOSE MALE STUDENTS WHO ARE ENROLLED
            string[] males=new string[5];

            con.Open();
            OdbcDataAdapter dam = new OdbcDataAdapter("Select*from stud_tbl where status='" + "Active" + "' and level='" + cmbLevML.Text + "'and gender='" + "Male" + "'Order by lname ASC", con);
            DataTable dtm = new DataTable();//                                  may change to sy, may syenrolled=activeSY
            dam.Fill(dtm);
            con.Close();

            if (dtm.Rows.Count > 0)
            {
                males = new string[dtm.Rows.Count];
                e.Graphics.DrawString("Male", df3, Brushes.Black, 100, 220, sf1);
                ymstart = 230;

                for (int a = 0; a < dtm.Rows.Count; a++)
                {
                    string list=a + 1 + ". " + dtm.Rows[a].ItemArray[3].ToString() + ", " + dtm.Rows[a].ItemArray[1].ToString() + " " + dtm.Rows[a].ItemArray[2].ToString();
                    males[a] = list;
                }
            }

            //RETRIEVE DATA FOR THOSE FEMALE STUDENTS WHO ARE ENROLLED
            string[] femls = new string[0];

            con.Open();
            OdbcDataAdapter daf = new OdbcDataAdapter("Select*from stud_tbl where status='" + "Active" + "' and level='" + cmbLevML.Text + "'and gender='" + "Female" + "' ORDER BY lname ASC", con);
            DataTable dtf = new DataTable();//                        may change to sy
            daf.Fill(dtf);
            con.Close();

            if (dtf.Rows.Count > 0)
            {
                femls = new string[dtf.Rows.Count];
                e.Graphics.DrawString("Female", df3, Brushes.Black, 500, 220, sf1);
                yfstart = 230;

                for (int a = 0; a < dtf.Rows.Count; a++)
                {
                    string list = a + 1 + ". " + dtf.Rows[a].ItemArray[3].ToString() + ", " + dtf.Rows[a].ItemArray[1].ToString() + " " + dtf.Rows[a].ItemArray[2].ToString();
                    femls[a] = list;
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////
            //===============P R I N T I N G  O F  C O N T E N T ===============================
            if (males.Length > femls.Length || males.Length == femls.Length)
            {
                int getMaxPages = males.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <=30)
                    {
                        ofpagecount = 1;
                        lblnotepage.Visible = true;
                        lblNudPage.Visible = false; nudPage.Visible = false;
                        
                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepage.Visible = true;
                        lblNudPage.Visible = true; nudPage.Visible = true;
                        
                    }
                }
                else
                {
                    if (males.Length== 30)
                    {
                        ofpagecount = 1;
                        lblnotepage.Visible = true;
                        lblNudPage.Visible = false; nudPage.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepage.Visible = true;
                        lblNudPage.Visible = true; nudPage.Visible = true;
                    }
                   
                }
                lblnotepage.Text = "Page " + nudPage.Text + " of " + ofpagecount;
                nudPage.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudPage.Items.Add(p);
                    nudPage.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberMale_ml <males.Length)
                {
                    e.Graphics.DrawString(males[cntrowsMale_ml] , df3, Brushes.Black, 100, ymstart += 20, sf1);
                    if (cntrowsFem_ml<femls.Length)
                    {
                        e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);
                    }

                    totalnumberMale_ml += 1;
                    if (itemperpageMale_ml < 29)
                    {
                        itemperpageMale_ml += 1;
                        e.HasMorePages = false;
                        cntrowsMale_ml++;
                        cntrowsFem_ml++;
                    }

                    else
                    {
                        itemperpageMale_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;
                        if (cntrowsMale_ml != males.Length && males.Length>cntrowsMale_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }
                    }

                    if (cntrowsMale_ml == males.Length)
                    {
                        cntrowsMale_ml = 0;
                        totalnumberMale_ml = males.Length + 1;//to stop iteration
                    }
            //--------------------------
                }
            }
            if (femls.Length > males.Length)
            {
                int getMaxPages = femls.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepage.Visible = true;
                        lblNudPage.Visible = false; nudPage.Visible = false;
                       
                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepage.Visible = true;
                        lblNudPage.Visible = true; nudPage.Visible = true;
                    }
                }
                else
                {
                    if (femls.Length== 30)
                    {
                        ofpagecount = 1;
                        lblnotepage.Visible = true;
                        lblNudPage.Visible = false; nudPage.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepage.Visible = true;
                        lblNudPage.Visible = true; nudPage.Visible = true;
                    }
                }
                lblnotepage.Text = "Page " + nudPage.Text + " of " + ofpagecount;
                nudPage.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudPage.Items.Add(p);
                    nudPage.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberFem_ml < femls.Length)
                {
                    if (cntrowsMale_ml < males.Length)
                    {
                        e.Graphics.DrawString(males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    }
                    e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);

                    totalnumberFem_ml += 1;
                    if (itemperpageFem_ml < 29)
                    {
                        cntrowsFem_ml++;
                        cntrowsMale_ml++;
                        itemperpageFem_ml += 1;
                        e.HasMorePages = false;

                    }

                    else
                    {
                        itemperpageFem_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;
                        if (cntrowsFem_ml != femls.Length && femls.Length>cntrowsFem_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }
                        
                    }

                    if (cntrowsFem_ml == femls.Length)
                    {
                        cntrowsFem_ml = 0;
                        totalnumberFem_ml = femls.Length + 1;//to stop iteration
                    }
                }
            }
            //--footer
            //----------------------------------------------------------

            int totalstud = dtm.Rows.Count + dtf.Rows.Count;
            preparedby = cmbPrepared.Text;
            if ((ymstart >= yfstart) && (dtf.Rows.Count != 0 || dtm.Rows.Count != 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, ymstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, ymstart + 90, sf1);
            }
            if ((yfstart > ymstart) && (dtf.Rows.Count != 0 || dtm.Rows.Count != 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, yfstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, yfstart + 90, sf1);
            }

            if (dtf.Rows.Count == 0 && dtm.Rows.Count == 0)
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, 200, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, 290, sf1);
            }
            //========
        }

        private void nudPage_SelectedItemChanged(object sender, EventArgs e)
        {
            int curpage = Convert.ToInt32(nudPage.Text) - 1;
            ppcML.StartPage = curpage;
            lblnotepage.Text = "Page " + nudPage.Text + " of " + ofpagecount;
        }

        private void pdESTY_PrintPage(object sender, PrintPageEventArgs e)
        {
            int current = Convert.ToInt32(DateTime.Now.Year);
            int upcoming = current + 1;
            string sy = activeSY;
            int ymstart = 0;
            int yfstart = 0;

            // String format
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Near;

            // Create font and brush.
            Font df1 = new Font("Arial", 12, FontStyle.Bold);
            Font df4 = new Font("Arial", 11, (FontStyle.Bold));
            Font df2 = new Font("Arial", 12, FontStyle.Regular);
            Font df3 = new Font("Arial", 11, FontStyle.Regular);
            Font df5 = new Font("Arial", 8, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            //REPORT'S HEADER
            e.Graphics.Clear(Color.White);

            Rectangle r = new Rectangle(100, 55, 100, 95);
            Image newImage = Image.FromFile(@"C:\Users\valued client\Documents\Visual Studio 2010\Projects\1 - THESIS\berlyn.bmp");
            e.Graphics.DrawImage(newImage, r);

            e.Graphics.DrawString("Berlyn Academy", df1, Brushes.Black, 420, 65, sf);
            e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df3, Brushes.Black, 420, 85, sf);
            e.Graphics.DrawString("REGISTERED ENROLEES", df4, Brushes.Black, 420, 115, sf);
            e.Graphics.DrawString("" + sy, df4, Brushes.Black, 420, 135, sf);
            e.Graphics.DrawString("Level: " + cmbLevel.Text, df4, Brushes.Black, 361, 155, sf1);

            e.Graphics.DrawString("Printed Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), df5, Brushes.Black, 575, 1005, sf1);
            //RETRIEVE DATA FOR THOSE MALE STUDENTS WHO ARE registered
            string[] males = new string[0];
            int oldstudentreg = 0;
            int newstudentreg = 0;

            con.Open();
            OdbcDataAdapter dam = new OdbcDataAdapter("Select*from offprereg_tbl where syregistered='" + activeSY + "' and lev='" + cmbLevel.Text + "'and gen='" + "Male" + "'Order by lname ASC", con);
            DataTable dtm = new DataTable();//                                  may change to sy, may syenrolled=activeSY
            dam.Fill(dtm);
            con.Close();
            con.Open();
            OdbcDataAdapter damOld = new OdbcDataAdapter("Select*from offprereg_old_tbl where syregistered='" + activeSY + "' and lev='" + cmbLevel.Text + "'and gen='" + "Male" + "'Order by lname ASC", con);
            DataTable dtmOld = new DataTable();//                                  may change to sy, may syenrolled=activeSY
            damOld.Fill(dtmOld);
            con.Close();
            if (dtmOld.Rows.Count > 0)
            {
                oldstudentreg = Convert.ToInt32(dtmOld.Rows.Count);
            }
            if (dtm.Rows.Count > 0)
            {
                newstudentreg = Convert.ToInt32(dtm.Rows.Count);
            }

            if (dtm.Rows.Count <=0 && dtmOld.Rows.Count<=0)
            {
                totalnumberMale_ml = 30;//this will set to max no. to not iterate
            }
          
            males = new string[oldstudentreg+newstudentreg];
            if (dtmOld.Rows.Count > 0 || dtm.Rows.Count > 0)
            {
                e.Graphics.DrawString("Male", df3, Brushes.Black, 100, 220, sf1);
            }
            ymstart = 230;
            int newIndexinArray = 0;
            for (int a = 0; a < dtm.Rows.Count; a++)
            {
                string list = dtm.Rows[a].ItemArray[3].ToString() + ", " + dtm.Rows[a].ItemArray[1].ToString() + " " + dtm.Rows[a].ItemArray[2].ToString();
                males[newIndexinArray] = list;
                newIndexinArray++;
            }
            for (int a = 0; a < dtmOld.Rows.Count; a++)
            {
                string list = dtmOld.Rows[a].ItemArray[3].ToString() + ", " + dtmOld.Rows[a].ItemArray[1].ToString() + " " + dtmOld.Rows[a].ItemArray[2].ToString();
                males[newIndexinArray] = list;
                newIndexinArray++;
            }

            //RETRIEVE DATA FOR THOSE FEMALE STUDENTS WHO ARE registered
            string[] femls = new string[0];
            int oldstudentreg_F = 0;
            int newstudentreg_F = 0;
            con.Open();
            OdbcDataAdapter damF = new OdbcDataAdapter("Select*from offprereg_tbl where syregistered='" + activeSY + "' and lev='" + cmbLevel.Text + "'and gen='" + "Female" + "'Order by lname ASC", con);
            DataTable dtmF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
            damF.Fill(dtmF);
            con.Close();
            con.Open();
            OdbcDataAdapter damOldF = new OdbcDataAdapter("Select*from offprereg_old_tbl where syregistered='" + activeSY + "' and lev='" + cmbLevel.Text + "'and gen='" + "Female" + "'Order by lname ASC", con);
            DataTable dtmOldF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
            damOldF.Fill(dtmOldF);
            con.Close();
            if (dtmOldF.Rows.Count > 0)
            {
                oldstudentreg_F = Convert.ToInt32(dtmOldF.Rows.Count);
            }
            if (dtmF.Rows.Count > 0)
            {
                newstudentreg_F = Convert.ToInt32(dtmF.Rows.Count);
            }
            if (dtmF.Rows.Count <= 0 && dtmOldF.Rows.Count <= 0)
            {
                totalnumberFem_ml = 30;//set to max to not iterate
            }

            femls = new string[oldstudentreg_F + newstudentreg_F];
            if (dtmOldF.Rows.Count > 0 || dtmF.Rows.Count > 0)
            {
                e.Graphics.DrawString("Female", df3, Brushes.Black, 500, 220, sf1);
            }
            yfstart = 230;
            int newIndexinArrayF = 0;
            for (int a = 0; a < dtmF.Rows.Count; a++)
            {
                string list = dtmF.Rows[a].ItemArray[3].ToString() + ", " + dtmF.Rows[a].ItemArray[1].ToString() + " " + dtmF.Rows[a].ItemArray[2].ToString();
                femls[newIndexinArrayF] = list;
                newIndexinArrayF++;
            }
            for (int a = 0; a < dtmOldF.Rows.Count; a++)
            {
                string list = dtmOldF.Rows[a].ItemArray[3].ToString() + ", " + dtmOldF.Rows[a].ItemArray[1].ToString() + " " + dtmOldF.Rows[a].ItemArray[2].ToString();
                femls[newIndexinArrayF] = list;
                newIndexinArrayF++;
            }


            //PRINTING OF CONTENT//================================================================================
            //=====================================================================================================
           
            if (males.Length > femls.Length || males.Length == femls.Length)
            {
                int getMaxPages = males.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <=30)
                    {
                        ofpagecount = 1;
                        lblnotepageEsty.Visible = true;
                        lblPageEsty.Visible = false; nudPageEsty.Visible = false;
                        
                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepageEsty.Visible = true;
                        lblPageEsty.Visible = true; nudPageEsty.Visible = true;
                        
                    }
                }
                else
                {
                    if (males.Length == 30)
                    {
                        ofpagecount = 1;
                        lblnotepageEsty.Visible = true;
                        lblPageEsty.Visible = false; nudPageEsty.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepageEsty.Visible = true;
                        lblPageEsty.Visible = true; nudPageEsty.Visible = true;
                    }
                   
                }
                lblnotepageEsty.Text = "Page " + nudPageEsty.Text + " of " + ofpagecount;
                nudPageEsty.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudPageEsty.Items.Add(p);
                    nudPageEsty.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberMale_ml <males.Length)
                {
                    Array.Sort(males);
                    Array.Sort(femls);
                    //MessageBox.Show(males[cntrowsMale_ml]+" "+ totalnumberMale_ml);
                    e.Graphics.DrawString(cntrowsMale_ml+1+". "+males[cntrowsMale_ml] , df3, Brushes.Black, 100, ymstart += 20, sf1);
                    if (cntrowsFem_ml<femls.Length)
                    {
                        e.Graphics.DrawString(cntrowsFem_ml+1+". "+femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);
                    }

                    totalnumberMale_ml += 1;
                    if (itemperpageMale_ml < 29)
                    {
                        itemperpageMale_ml += 1;
                        e.HasMorePages = false;
                        cntrowsMale_ml++;
                        cntrowsFem_ml++;
                    }

                    else
                    {
                        itemperpageMale_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;

                        //IF EXACT THE LENGTH TO 3O
                      
                        if (cntrowsMale_ml!= males.Length && males.Length>cntrowsMale_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }
                    }

                    if (cntrowsMale_ml == males.Length)
                    {
                        cntrowsMale_ml = 0;
                        totalnumberMale_ml = males.Length + 1;//to stop iteration
                    }
                }
            }
            if (femls.Length > males.Length)
            {
                int getMaxPages = femls.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepageEsty.Visible = true;
                        lblPageEsty.Visible = false; nudPageEsty.Visible = false;

                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepageEsty.Visible = true;
                        lblPageEsty.Visible = true; nudPageEsty.Visible = true;
                    }
                }
                else
                {
                    if (femls.Length== 30)
                    {
                        ofpagecount = 1;
                        lblnotepageEsty.Visible = true;
                        lblPageEsty.Visible = false; nudPageEsty.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepageEsty.Visible = true;
                        lblPageEsty.Visible = true; nudPageEsty.Visible = true;
                    }
                }

                lblnotepageEsty.Text = "Page " + nudPageEsty.Text + " of " + ofpagecount;
                nudPageEsty.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudPageEsty.Items.Add(p);
                    nudPageEsty.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberFem_ml <femls.Length)
                {
                    Array.Sort(femls);
                    Array.Sort(males);
                    if (cntrowsMale_ml < males.Length)
                    {
                        e.Graphics.DrawString(cntrowsMale_ml+1+". "+males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    }
                    e.Graphics.DrawString(cntrowsFem_ml+1+". "+femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);

                    totalnumberFem_ml += 1;
                    if (itemperpageFem_ml < 29)
                    {
                        cntrowsFem_ml++;
                        cntrowsMale_ml++;
                        itemperpageFem_ml += 1;
                        e.HasMorePages = false;

                    }

                    else
                    {
                        itemperpageFem_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;

                        if (cntrowsFem_ml != femls.Length && femls.Length>cntrowsFem_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }
                    }

                    if (cntrowsFem_ml == femls.Length)
                    {
                        cntrowsFem_ml = 0;
                        totalnumberFem_ml = femls.Length + 1;//to stop iteration
                    }
                }
            }
            //                                    F  O  O  T  E  R
            //=====================================================================================================
            //=====================================================================================================

            int totalstud = dtm.Rows.Count + dtmOld.Rows.Count+dtmF.Rows.Count+dtmOldF.Rows.Count;
            int allMales = dtm.Rows.Count + dtmOld.Rows.Count;
            int allFemls = dtmF.Rows.Count + dtmOldF.Rows.Count;

            string prepEsty = cmbPrepESTY.Text;

            if ((ymstart >= yfstart) && (allFemls != 0 || allMales != 0))
            {
                e.Graphics.DrawString("Total enrolees: " + totalstud, df3, Brushes.Black, 100, ymstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, ymstart + 90, sf1);
            }
            if ((yfstart > ymstart) && (allFemls != 0 || allMales != 0))
            {
                e.Graphics.DrawString("Total enrolees: " + totalstud, df3, Brushes.Black, 100, yfstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, yfstart + 90, sf1);
            }

            if (allFemls == 0 && allMales == 0)
            {
                e.Graphics.DrawString("Total enrolees: " + totalstud, df3, Brushes.Black, 100, 200, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, 290, sf1);
            }
        }

        private void nudPageEsty_SelectedItemChanged(object sender, EventArgs e)
        {
            int curpage = Convert.ToInt32(nudPageEsty.Text) - 1;
            ppcESTY.StartPage = curpage;
            lblnotepageEsty.Text = "Page " + nudPageEsty.Text + " of " + ofpagecount;
        }

        private void pagenudSWS_SelectedItemChanged(object sender, EventArgs e)
        {
            int curpage = Convert.ToInt32(pagenudSWS.Text) - 1;
            ppcSWS.StartPage = curpage;
            lblnotepageSWS.Text = "Page " + pagenudSWS.Text + " of " + ofpagecount;
        }

        private void pdSWS_PrintPage(object sender, PrintPageEventArgs e)
        {
            int current = Convert.ToInt32(DateTime.Now.Year);
            int upcoming = current + 1;
            string sy = activeSY;
            int ymstart = 0;
            int yfstart = 0;

            // String format
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Near;

            // Create font and brush.
            Font df1 = new Font("Arial", 12, FontStyle.Bold);
            Font df4 = new Font("Arial", 11, (FontStyle.Bold));
            Font df2 = new Font("Arial", 12, FontStyle.Regular);
            Font df3 = new Font("Arial", 11, FontStyle.Regular);
            Font df5 = new Font("Arial", 8, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            //REPORT'S HEADER
            e.Graphics.Clear(Color.White);

            Rectangle r = new Rectangle(100, 55, 100, 95);
            Image newImage = Image.FromFile(@"C:\Users\valued client\Documents\Visual Studio 2010\Projects\1 - THESIS\berlyn.bmp");
            e.Graphics.DrawImage(newImage, r);

            e.Graphics.DrawString("Berlyn Academy", df1, Brushes.Black, 420, 65, sf);
            e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df3, Brushes.Black, 420, 85, sf);
            e.Graphics.DrawString("STUDENTS WITHOUT SECTION", df4, Brushes.Black, 420, 115, sf);
            e.Graphics.DrawString("" + sy, df3, Brushes.Black, 420, 135, sf);
            e.Graphics.DrawString("Level: " + cmbLevSWS.Text, df4, Brushes.Black, 361, 155, sf1);

            e.Graphics.DrawString("Printed Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), df5, Brushes.Black, 575, 1005, sf1);

            //RETRIEVE DATA FOR THOSE MALE STUDENTS WHO ARE ENROLLED
            string[] males = new string[0];

            con.Open();
            OdbcDataAdapter dam = new OdbcDataAdapter("Select*from stud_tbl where status='" + "Active" + "' and level='" + cmbLevSWS.Text + "'and section='' and gender='" + "Male" + "'Order by lname ASC", con);
            DataTable dtm = new DataTable();//                               may change to sy, may syenrolled=activeSY
            dam.Fill(dtm);
            con.Close();

            if (dtm.Rows.Count > 0)
            {
                males = new string[dtm.Rows.Count];
                e.Graphics.DrawString("Male", df3, Brushes.Black, 100, 220, sf1);
                ymstart = 230;

                for (int a = 0; a < dtm.Rows.Count; a++)
                {
                    string list = a + 1 + ". " + dtm.Rows[a].ItemArray[3].ToString() + ", " + dtm.Rows[a].ItemArray[1].ToString() + " " + dtm.Rows[a].ItemArray[2].ToString();
                    males[a] = list;
                }
            }

            //RETRIEVE DATA FOR THOSE FEMALE STUDENTS WHO ARE ENROLLED
            string[] femls = new string[0];

            con.Open();
            OdbcDataAdapter daf = new OdbcDataAdapter("Select*from stud_tbl where status='" + "Active" + "' and level='" + cmbLevSWS.Text + "'and section='' and gender='" + "Female" + "' ORDER BY lname ASC", con);
            DataTable dtf = new DataTable();//                        may change to sy
            daf.Fill(dtf);
            con.Close();

            if (dtf.Rows.Count > 0)
            {
                femls = new string[dtf.Rows.Count];
                e.Graphics.DrawString("Female", df3, Brushes.Black, 500, 220, sf1);
                yfstart = 230;

                for (int a = 0; a < dtf.Rows.Count; a++)
                {
                    string list = a + 1 + ". " + dtf.Rows[a].ItemArray[3].ToString() + ", " + dtf.Rows[a].ItemArray[1].ToString() + " " + dtf.Rows[a].ItemArray[2].ToString();
                    femls[a] = list;
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////
            //===============P R I N T I N G  O F  C O N T E N T ===============================
            if (males.Length > femls.Length || males.Length == femls.Length)
            {
                int getMaxPages = males.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepageSWS.Visible = true;
                        lblnotenudSWS.Visible = false; pagenudSWS.Visible = false;

                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepageSWS.Visible = true;
                        lblnotenudSWS.Visible = true; pagenudSWS.Visible = true;

                    }
                }
                else
                {
                    if (males.Length == 30)
                    {
                        ofpagecount = 1;
                        lblnotepageSWS.Visible = true;
                        lblnotenudSWS.Visible = false; pagenudSWS.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepageSWS.Visible = true;
                        lblnotenudSWS.Visible = true; pagenudSWS.Visible = true;
                    }

                }
                lblnotepageSWS.Text = "Page " + pagenudSWS.Text + " of " + ofpagecount;
                pagenudSWS.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    pagenudSWS.Items.Add(p);
                    pagenudSWS.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberMale_ml < males.Length)
                {
                    e.Graphics.DrawString(males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    if (cntrowsFem_ml < femls.Length)
                    {
                        e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);
                    }

                    totalnumberMale_ml += 1;
                    if (itemperpageMale_ml < 29)
                    {
                        itemperpageMale_ml += 1;
                        e.HasMorePages = false;
                        cntrowsMale_ml++;
                        cntrowsFem_ml++;
                    }

                    else
                    {
                        itemperpageMale_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;
                        if (cntrowsMale_ml != males.Length && males.Length > cntrowsMale_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }
                    }

                    if (cntrowsMale_ml == males.Length)
                    {
                        cntrowsMale_ml = 0;
                        totalnumberMale_ml = males.Length + 1;//to stop iteration
                    }
                }
            }
            if (femls.Length > males.Length)
            {
                int getMaxPages = femls.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepageSWS.Visible = true;
                        lblnotenudSWS.Visible = false; pagenudSWS.Visible = false;

                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepageSWS.Visible = true;
                        lblnotenudSWS.Visible = true; pagenudSWS.Visible = true;
                    }
                }
                else
                {
                    if (femls.Length == 30)
                    {
                        ofpagecount = 1;
                        lblnotepageSWS.Visible = true;
                        lblnotenudSWS.Visible = false; pagenudSWS.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepageSWS.Visible = true;
                        lblnotenudSWS.Visible = true; pagenudSWS.Visible = true;
                    }
                }
                lblnotepageSWS.Text = "Page " + pagenudSWS.Text + " of " + ofpagecount;
                pagenudSWS.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    pagenudSWS.Items.Add(p);
                    pagenudSWS.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberFem_ml < femls.Length)
                {
                    if (cntrowsMale_ml < males.Length)
                    {
                        e.Graphics.DrawString(males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    }
                    e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);

                    totalnumberFem_ml += 1;
                    if (itemperpageFem_ml < 29)
                    {
                        cntrowsFem_ml++;
                        cntrowsMale_ml++;
                        itemperpageFem_ml += 1;
                        e.HasMorePages = false;

                    }

                    else
                    {
                        itemperpageFem_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;
                        if (cntrowsFem_ml != femls.Length && femls.Length > cntrowsFem_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }

                    }

                    if (cntrowsFem_ml == femls.Length)
                    {
                        cntrowsFem_ml = 0;
                        totalnumberFem_ml = femls.Length + 1;//to stop iteration
                    }
                }
            }
            //--footer
            //----------------------------------------------------------

            //=======================================

            int totalstud = dtm.Rows.Count + dtf.Rows.Count;
            string prep = cmbPreparedSWS.Text;
            if ((ymstart >= yfstart) && (dtf.Rows.Count != 0 || dtm.Rows.Count != 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, ymstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, ymstart + 90, sf1);
            }
            if ((yfstart > ymstart) && (dtf.Rows.Count != 0 || dtm.Rows.Count != 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, yfstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, yfstart + 90, sf1);
            }

            if (dtf.Rows.Count == 0 && dtm.Rows.Count == 0)
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, 200, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, 290, sf1);
            } 
        }

        private void nudSIAC_SelectedItemChanged(object sender, EventArgs e)
        {
            int curpage = Convert.ToInt32(nudSIAC.Text) - 1;
            ppcSIAC.StartPage = curpage;
            lblnotepageSIAC.Text = "Page " + nudSIAC.Text + " of " + ofpagecount;
        }

        private void pdSIAC_PrintPage(object sender, PrintPageEventArgs e)
        {
            int current = Convert.ToInt32(DateTime.Now.Year);
            int upcoming = current + 1;
            string sy = activeSY;
            int ymstart = 0;
            int yfstart = 0;

            // String format
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Near;

            // Create font and brush.
            Font df1 = new Font("Arial", 12, FontStyle.Bold);
            Font df4 = new Font("Arial", 11, (FontStyle.Bold));
            Font df2 = new Font("Arial", 12, FontStyle.Regular);
            Font df3 = new Font("Arial", 11, FontStyle.Regular);
            Font df5 = new Font("Arial", 8, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            //REPORT'S HEADER
            e.Graphics.Clear(Color.White);

            Rectangle r = new Rectangle(100, 55, 100, 95);
            Image newImage = Image.FromFile(@"C:\Users\valued client\Documents\Visual Studio 2010\Projects\1 - THESIS\berlyn.bmp");
            e.Graphics.DrawImage(newImage, r);

            e.Graphics.DrawString("Berlyn Academy", df1, Brushes.Black, 420, 65, sf);
            e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df3, Brushes.Black, 420, 85, sf);
            e.Graphics.DrawString("STUDENTS LIST", df4, Brushes.Black, 420, 115, sf);
            e.Graphics.DrawString("" + sy, df3, Brushes.Black, 420, 135, sf);
            e.Graphics.DrawString(cmbLevSIAC.Text+" - " + cmbSecSIAC.Text, df4, Brushes.Black, 420, 155, sf);
           
            e.Graphics.DrawString("Printed Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), df5, Brushes.Black, 575, 1005, sf1);
            string signatoryAdv = "";
            string signatoryPrin = "";

            con.Open();
            OdbcDataAdapter dapr = new OdbcDataAdapter("Select (concat(firstname,' ',middlename,' ',lastname)),gender from employees_tbl where position='" + "principal" + "'and principalstatus='present'", con);
            DataTable dtpr = new DataTable();
            dapr.Fill(dtpr);
            con.Close();
            if (dtpr.Rows.Count > 0)
            {
                if (dtpr.Rows[0].ItemArray[1].ToString() == "Male")
                {
                    ncpr = "Mr. ";
                }
                else
                {
                    ncpr = "Ms. ";
                }
                signatoryPrin = dtpr.Rows[0].ItemArray[0].ToString();
            }


            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select (concat(firstname,' ',middlename,' ',lastname)),gender from employees_tbl where grade='" + cmbLevSIAC.Text + "'and advisory='" + cmbSecSIAC.Text + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0].ItemArray[1].ToString() == "Male")
                {
                    nc = "Mr. ";
                }
                else
                {
                    nc = "Ms. ";
                }

                signatoryAdv = dt.Rows[0].ItemArray[0].ToString();
                e.Graphics.DrawString("Adviser: " + nc + dt.Rows[0].ItemArray[0].ToString(), df4, Brushes.Black, 265, 175, sf1);
            }
            else
            {
                signatoryAdv = "Faculty ABC";
                e.Graphics.DrawString("Adviser: Faculty ABC", df4, Brushes.Black, 315, 175, sf1);
            }

            //==========================
            //RETRIEVE DATA FOR THOSE MALE STUDENTS WHO ARE ENROLLED
            string[] males = new string[0];

            con.Open();
            OdbcDataAdapter dam = new OdbcDataAdapter("Select*from stud_tbl where status='" + "Active" + "' and level='" + cmbLevSIAC.Text + "'and section='" + cmbSecSIAC.Text + "'and gender='" + "Male" + "'Order by lname ASC", con);
            DataTable dtm = new DataTable();//                                      may also change to sy variable , maysyenrolled=activeSY
            dam.Fill(dtm);
            con.Close();

            if (dtm.Rows.Count > 0)
            {
                males = new string[dtm.Rows.Count];
                e.Graphics.DrawString("Male", df3, Brushes.Black, 100, 220, sf1);
                ymstart = 230;

                for (int a = 0; a < dtm.Rows.Count; a++)
                {
                    string list = a + 1 + ". " + dtm.Rows[a].ItemArray[3].ToString() + ", " + dtm.Rows[a].ItemArray[1].ToString() + " " + dtm.Rows[a].ItemArray[2].ToString();
                    males[a] = list;
                }
            }

            //RETRIEVE DATA FOR THOSE FEMALE STUDENTS WHO ARE ENROLLED
            string[] femls = new string[0];

            con.Open();
            OdbcDataAdapter daf = new OdbcDataAdapter("Select*from stud_tbl where status='" + "Active" + "' and level='" + cmbLevSIAC.Text + "'and section='" + cmbSecSIAC.Text + "'and gender='" + "Female" + "' ORDER BY lname ASC", con);
            DataTable dtf = new DataTable();
            daf.Fill(dtf);
            con.Close();

            if (dtf.Rows.Count > 0)
            {
                femls = new string[dtf.Rows.Count];
                e.Graphics.DrawString("Female", df3, Brushes.Black, 500, 220, sf1);
                yfstart = 230;

                for (int a = 0; a < dtf.Rows.Count; a++)
                {
                    string list = a + 1 + ". " + dtf.Rows[a].ItemArray[3].ToString() + ", " + dtf.Rows[a].ItemArray[1].ToString() + " " + dtf.Rows[a].ItemArray[2].ToString();
                    femls[a] = list;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////
            //===============P R I N T I N G  O F  C O N T E N T ===============================
            if (males.Length > femls.Length || males.Length == femls.Length)
            {
                int getMaxPages = males.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepageSIAC.Visible = true;
                        lblnudSIAC.Visible = false; nudSIAC.Visible = false;

                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepageSIAC.Visible = true;
                        lblnudSIAC.Visible = true; nudSIAC.Visible = true;

                    }
                }
                else
                {
                    if (males.Length == 30)
                    {
                        ofpagecount = 1;
                        lblnotepageSIAC.Visible = true;
                        lblnudSIAC.Visible = false; nudSIAC.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepageSIAC.Visible = true;
                        lblnudSIAC.Visible = true; nudSIAC.Visible = true;
                    }

                }
                lblnotepageSIAC.Text = "Page " + nudSIAC.Text + " of " + ofpagecount;
                nudSIAC.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudSIAC.Items.Add(p);
                    nudSIAC.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberMale_ml < males.Length)
                {
                    e.Graphics.DrawString(males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    if (cntrowsFem_ml < femls.Length)
                    {
                        e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);
                    }

                    totalnumberMale_ml += 1;
                    if (itemperpageMale_ml < 29)
                    {
                        itemperpageMale_ml += 1;
                        e.HasMorePages = false;
                        cntrowsMale_ml++;
                        cntrowsFem_ml++;
                    }

                    else
                    {
                        itemperpageMale_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;
                        if (cntrowsMale_ml != males.Length && males.Length > cntrowsMale_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }
                    }

                    if (cntrowsMale_ml == males.Length)
                    {
                        cntrowsMale_ml = 0;
                        totalnumberMale_ml = males.Length + 1;//to stop iteration
                    }
                }
            }
            if (femls.Length > males.Length)
            {
                int getMaxPages = femls.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepageSIAC.Visible = true;
                        lblnudSIAC.Visible = false; nudSIAC.Visible = false;

                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepageSIAC.Visible = true;
                        lblnudSIAC.Visible = true; nudSIAC.Visible = true;
                    }
                }
                else
                {
                    if (femls.Length == 30)
                    {
                        ofpagecount = 1;
                        lblnotepageSIAC.Visible = true;
                        lblnudSIAC.Visible = false; nudSIAC.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepageSIAC.Visible = true;
                        lblnudSIAC.Visible = true; nudSIAC.Visible = true;
                    }
                }
                lblnotepageSIAC.Text = "Page " + nudSIAC.Text + " of " + ofpagecount;
                nudSIAC.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudSIAC.Items.Add(p);
                    nudSIAC.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberFem_ml < femls.Length)
                {
                    if (cntrowsMale_ml < males.Length)
                    {
                        e.Graphics.DrawString(males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    }
                    e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);

                    totalnumberFem_ml += 1;
                    if (itemperpageFem_ml < 29)
                    {
                        cntrowsFem_ml++;
                        cntrowsMale_ml++;
                        itemperpageFem_ml += 1;
                        e.HasMorePages = false;

                    }

                    else
                    {
                        itemperpageFem_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;
                        if (cntrowsFem_ml != femls.Length && femls.Length > cntrowsFem_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }

                    }

                    if (cntrowsFem_ml == femls.Length)
                    {
                        cntrowsFem_ml = 0;
                        totalnumberFem_ml = femls.Length + 1;//to stop iteration
                    }
                }
            }
            //--footer
            //----------------------------------------------------------
            int totalstud = dtm.Rows.Count + dtf.Rows.Count;
            if ((ymstart >= yfstart) && (dtf.Rows.Count != 0 || dtm.Rows.Count != 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, ymstart + 50, sf1);
                e.Graphics.DrawString(nc + signatoryAdv, df4, Brushes.Black, 330, ymstart + 90, sf1);
                e.Graphics.DrawString("_____________________________", df3, Brushes.Black, 305, ymstart + 90, sf1);
                e.Graphics.DrawString("Adviser", df3, Brushes.Black, 405, ymstart + 110, sf1);

                e.Graphics.DrawString(ncpr + signatoryPrin, df4, Brushes.Black, 330, ymstart + 170, sf1);
                e.Graphics.DrawString("_____________________________", df3, Brushes.Black, 305, ymstart + 170, sf1);
                e.Graphics.DrawString("Principal", df3, Brushes.Black, 405, ymstart + 190, sf1);
            }
            if ((yfstart > ymstart) && (dtf.Rows.Count != 0 || dtm.Rows.Count != 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, yfstart + 50, sf1);
                e.Graphics.DrawString(nc + signatoryAdv, df4, Brushes.Black, 330, yfstart + 90, sf1);
                e.Graphics.DrawString("_____________________________", df3, Brushes.Black, 305, yfstart + 90, sf1);
                e.Graphics.DrawString("Adviser", df3, Brushes.Black, 405, yfstart + 110, sf1);

                e.Graphics.DrawString(ncpr + signatoryPrin, df4, Brushes.Black, 330, yfstart + 170, sf1);
                e.Graphics.DrawString("_____________________________", df3, Brushes.Black, 305, yfstart + 170, sf1);
                e.Graphics.DrawString("Principal", df3, Brushes.Black, 405, yfstart + 190, sf1);
            }

            if (dtf.Rows.Count == 0 && dtm.Rows.Count == 0)
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, 200, sf1);
                e.Graphics.DrawString(nc + signatoryAdv, df4, Brushes.Black, 330, 240, sf1);
                e.Graphics.DrawString("_____________________________", df3, Brushes.Black, 315, 240, sf1);
                e.Graphics.DrawString("Adviser", df3, Brushes.Black, 405, 260, sf1);

                e.Graphics.DrawString(ncpr + signatoryPrin, df4, Brushes.Black, 330, 310, sf1);
                e.Graphics.DrawString("_____________________________", df3, Brushes.Black, 315, 310, sf1);
                e.Graphics.DrawString("Principal", df3, Brushes.Black, 405, 330, sf1);
            }
        }

        private void nudPFS_SelectedItemChanged(object sender, EventArgs e)
        {
            int curpage = Convert.ToInt32(nudPFS.Text) - 1;
            ppcPFS.StartPage = curpage;
            lblnotepagePFS.Text = "Page " + nudPFS.Text + " of " + ofpagecount;
        }

        private void pdPFS_PrintPage(object sender, PrintPageEventArgs e)
        {
            int current = Convert.ToInt32(DateTime.Now.Year);
            int upcoming = current + 1;
            string sy = activeSY;
            int ymstart = 0;
            int yfstart = 0;

            // String format
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Near;

            // Create font and brush.
            Font df1 = new Font("Arial", 12, FontStyle.Bold);
            Font df4 = new Font("Arial", 11, (FontStyle.Bold));
            Font df2 = new Font("Arial", 12, FontStyle.Regular);
            Font df3 = new Font("Arial", 11, FontStyle.Regular);
            Font df5 = new Font("Arial", 8, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            //REPORT'S HEADER
            e.Graphics.Clear(Color.White);
            
            Rectangle r = new Rectangle(100, 55, 100, 95);
            Image newImage = Image.FromFile(@"C:\Users\valued client\Documents\Visual Studio 2010\Projects\1 - THESIS\berlyn.bmp");
            e.Graphics.DrawImage(newImage, r);

            e.Graphics.DrawString("Berlyn Academy", df1, Brushes.Black, 420, 65, sf);
            e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df3, Brushes.Black, 420, 85, sf);
            e.Graphics.DrawString(cmbRemPFS.Text + " Students", df4, Brushes.Black, 420, 115, sf);
            e.Graphics.DrawString("" + sy, df3, Brushes.Black, 420, 135, sf);
            e.Graphics.DrawString("Level/Section: " + cmbPFS.Text + " - " + cmbSecPFS.Text, df4, Brushes.Black, 420, 155, sf);

            if (cmbAve.Enabled == false)
            {
                e.Graphics.DrawString("Subject/Grading: " + cmbSubs.Text + " - " + cmbGrading.Text, df4, Brushes.Black, 420, 175, sf);
            }
            else
            {
                e.Graphics.DrawString("GENERAL AVERAGE", df4, Brushes.Black, 420, 175, sf);
            }

            e.Graphics.DrawString("Printed Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), df5, Brushes.Black, 575, 1005, sf1);
            //==========================

            int CountMale = 0;
            int CountFeml = 0;
            int maleindex = 0;
            int femlindex = 0;
            string[] males = new string[0];
            //RETRIEVE DATA FOR THOSE MALE STUDENTS WHO ARE ENROLLED
            con.Open();
            OdbcDataAdapter dam = new OdbcDataAdapter("Select*from stud_tbl where status='" + "Active" + "' and level='" + cmbPFS.Text + "'and section='" + cmbSecPFS.Text + "'and gender='" + "Male" + "'and syregistered='" + activeSY + "'Order by lname ASC", con);
            DataTable dtm = new DataTable();//                                  may change to sy, may syenrolled=activeSY
            dam.Fill(dtm);
            con.Close();
            //pfs code
            if (dtm.Rows.Count > 0)
            {
                DataTable dtAve = new DataTable();
                males = new string[dtm.Rows.Count];
                e.Graphics.DrawString("Male", df3, Brushes.Black, 100, 220, sf1);
                ymstart = 230;

                for (int a = 0; a < dtm.Rows.Count; a++)
                {
                    string gradelevel = dtm.Rows[a].ItemArray[4].ToString();
                    if ((cmbAve.Text == "General Average" || cmbAve.Text == "1st Quarter" || cmbAve.Text == "2nd Quarter" || cmbAve.Text == "3rd Quarter" || cmbAve.Text == "4th Quarter"))
                    {
                        if (gradelevel == "Kinder")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4) from kindergrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 1")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradeonegrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 2")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradetwogrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 3")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradethreegrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 4")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradefourgrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 5")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradefivegrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 6")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradesixgrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 7")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradesevengrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 8")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradeeightgrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 9")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradeninegrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 10")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradetengrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }

                        //CHECK THE AVE. =================================================================

                        if (dtAve.Rows.Count > 0)
                        {
                            double generalAve = 0;
                            if (cmbAve.Text == "1st Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAve.Rows[0].ItemArray[1].ToString());
                            }
                            if (cmbAve.Text == "2nd Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAve.Rows[0].ItemArray[2].ToString());
                            }
                            if (cmbAve.Text == "3rd Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAve.Rows[0].ItemArray[3].ToString());
                            }
                            if (cmbAve.Text == "4th Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAve.Rows[0].ItemArray[4].ToString());
                            }
                            if (cmbAve.Text == "General Average")
                            {
                                generalAve = Convert.ToDouble(dtAve.Rows[0].ItemArray[0].ToString());
                            }

                            
                            if (generalAve < 75 && cmbRemPFS.Text == "Failed")
                            {
                                CountMale++;
                                string list = maleindex + 1 + ". " + dtm.Rows[a].ItemArray[3].ToString() + ", " + dtm.Rows[a].ItemArray[1].ToString() + " " + dtm.Rows[a].ItemArray[2].ToString();
                                males[maleindex] = list;
                                maleindex++;
                            }
                            if (generalAve >= 75 && cmbRemPFS.Text == "Passed")
                            {
                                CountMale++;
                                string list = maleindex + 1 + ". " + dtm.Rows[a].ItemArray[3].ToString() + ", " + dtm.Rows[a].ItemArray[1].ToString() + " " + dtm.Rows[a].ItemArray[2].ToString();
                                males[maleindex] = list;
                                maleindex++;
                            }
                        }
                    }
                    if ((cmbAve.Enabled==false && cmbSubs.Text!="" && cmbGrading.Text!=""))
                    {
                        if (gradelevel == "Kinder")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from kindergrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='"+cmbSubs.Text+"'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 1")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradeonegrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 2")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradetwogrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 3")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradethreegrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 4")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradefourgrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 5")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradefivegrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 6")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradesixgrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 7")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradesevengrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 8")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradeeightgrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 9")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradeninegrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }
                        if (gradelevel == "Grade 10")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradetengrades_tbl where studno='" + dtm.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAve = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAve);
                            con.Close();
                        }

                        //CHECK THE AVE. =================================================================

                        if (dtAve.Rows.Count > 0)
                        {
                            double Grade = 0;
                            if (cmbGrading.Text == "1st Quarter")
                            {
                                Grade = Convert.ToDouble(dtAve.Rows[0].ItemArray[0].ToString());
                            }
                            if (cmbGrading.Text == "2nd Quarter")
                            {
                                Grade = Convert.ToDouble(dtAve.Rows[0].ItemArray[1].ToString());
                            }
                            if (cmbGrading.Text == "3rd Quarter")
                            {
                                Grade = Convert.ToDouble(dtAve.Rows[0].ItemArray[2].ToString());
                            }
                            if (cmbGrading.Text == "4th Quarter")
                            {
                                Grade = Convert.ToDouble(dtAve.Rows[0].ItemArray[3].ToString());
                            }
                           
                            if (Grade < 75 && cmbRemPFS.Text == "Failed")
                            {
                                CountMale++;
                                string list = maleindex + 1 + ". " + dtm.Rows[a].ItemArray[3].ToString() + ", " + dtm.Rows[a].ItemArray[1].ToString() + " " + dtm.Rows[a].ItemArray[2].ToString();
                                males[maleindex] = list;
                                maleindex++;
                            }
                            if (Grade>= 75 && cmbRemPFS.Text == "Passed")
                            {
                                CountMale++;
                                string list = maleindex + 1 + ". " + dtm.Rows[a].ItemArray[3].ToString() + ", " + dtm.Rows[a].ItemArray[1].ToString() + " " + dtm.Rows[a].ItemArray[2].ToString();
                                males[maleindex] = list;
                                maleindex++;
                            }
                        }
                    }
                }
            }


            string[] femls = new string[0];

            //RETRIEVE DATA FOR THOSE FEMALE STUDENTS WHO ARE ENROLLED
            con.Open();
            OdbcDataAdapter daf = new OdbcDataAdapter("Select*from stud_tbl where status='" + "Active" + "' and level='" + cmbPFS.Text + "'and section='" + cmbSecPFS.Text + "'and gender='" + "Female" + "'and syregistered='" + activeSY + "' ORDER BY lname ASC", con);
            DataTable dtf = new DataTable();//                        may change to sy
            daf.Fill(dtf);
            con.Close();

            if (dtf.Rows.Count > 0)
            {
                femls = new string[dtf.Rows.Count];
                e.Graphics.DrawString("Female", df3, Brushes.Black, 500, 220, sf1);
                DataTable dtAveF = new DataTable();
                yfstart = 230;
                for (int a = 0; a < dtf.Rows.Count; a++)
                {
                    string gradelevel = dtf.Rows[a].ItemArray[4].ToString();
                    if (cmbAve.Text == "General Average" || cmbAve.Text == "1st Quarter" || cmbAve.Text == "2nd Quarter" || cmbAve.Text == "3rd Quarter" || cmbAve.Text == "4th Quarter")
                    {
                        if (gradelevel == "Kinder")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from kindergrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 1")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradeonegrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 2")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradetwogrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 3")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradethreegrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 4")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradefourgrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 5")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradefivegrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 6")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradesixgrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 7")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradesevengrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 8")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradeeightgrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 9")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradeninegrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 10")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select avg(ave),avg(q1),avg(q2),avg(q3),avg(q4)from gradetengrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }

                        //CHECK THE AVE. =================================================================

                        if (dtAveF.Rows.Count > 0)
                        {
                            double generalAve = 0;
                            if (cmbAve.Text == "1st Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAveF.Rows[0].ItemArray[1].ToString());
                            }
                            if (cmbAve.Text == "2nd Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAveF.Rows[0].ItemArray[2].ToString());
                            }
                            if (cmbAve.Text == "3rd Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAveF.Rows[0].ItemArray[3].ToString());
                            }
                            if (cmbAve.Text == "4th Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAveF.Rows[0].ItemArray[4].ToString());
                            }
                            if (cmbAve.Text == "General Average")
                            {
                                generalAve = Convert.ToDouble(dtAveF.Rows[0].ItemArray[0].ToString());
                            }

                            if (generalAve < 75 && cmbRemPFS.Text == "Failed")
                            {
                                CountFeml++;
                                string list = femlindex + 1 + ". " + dtf.Rows[a].ItemArray[3].ToString() + ", " + dtf.Rows[a].ItemArray[1].ToString() + " " + dtf.Rows[a].ItemArray[2].ToString();
                                femls[femlindex] = list;
                                femlindex++;
                            }
                            if (generalAve >= 75 && cmbRemPFS.Text == "Passed")
                            {
                                CountFeml++;
                                string list = femlindex + 1 + ". " + dtf.Rows[a].ItemArray[3].ToString() + ", " + dtf.Rows[a].ItemArray[1].ToString() + " " + dtf.Rows[a].ItemArray[2].ToString();
                                femls[femlindex] = list;
                                femlindex++;
                            }
                        }
                    }
                    if ((cmbAve.Enabled == false && cmbSubs.Text != "" && cmbGrading.Text != ""))
                    {
                        if (gradelevel == "Kinder")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from kindergrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 1")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradeonegrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 2")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradetwogrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 3")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradethreegrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 4")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradefourgrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 5")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradefivegrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 6")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradesixgrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 7")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradesevengrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 8")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradeeightgrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 9")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradeninegrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='" + cmbSubs.Text + "'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }
                        if (gradelevel == "Grade 10")
                        {
                            con.Open();
                            OdbcDataAdapter dak = new OdbcDataAdapter("Select q1,q2,q3,q4 from gradetengrades_tbl where studno='" + dtf.Rows[a].ItemArray[0].ToString() + "'and subdesc='"+cmbSubs.Text+"'", con);
                            dtAveF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            dak.Fill(dtAveF);
                            con.Close();
                        }

                        //CHECK THE AVE. =================================================================

                        if (dtAveF.Rows.Count > 0)
                        {
                            double generalAve = 0;
                            if (cmbGrading.Text == "1st Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAveF.Rows[0].ItemArray[1].ToString());
                            }
                            if (cmbGrading.Text == "2nd Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAveF.Rows[0].ItemArray[2].ToString());
                            }
                            if (cmbGrading.Text == "3rd Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAveF.Rows[0].ItemArray[3].ToString());
                            }
                            if (cmbGrading.Text == "4th Quarter")
                            {
                                generalAve = Convert.ToDouble(dtAveF.Rows[0].ItemArray[4].ToString());
                            }
                            
                            if (generalAve < 75 && cmbRemPFS.Text == "Failed")
                            {
                                CountFeml++;
                                string list = femlindex + 1 + ". " + dtf.Rows[a].ItemArray[3].ToString() + ", " + dtf.Rows[a].ItemArray[1].ToString() + " " + dtf.Rows[a].ItemArray[2].ToString();
                                femls[femlindex] = list;
                                femlindex++;
                            }
                            if (generalAve >= 75 && cmbRemPFS.Text == "Passed")
                            {
                                CountFeml++;
                                string list = femlindex + 1 + ". " + dtf.Rows[a].ItemArray[3].ToString() + ", " + dtf.Rows[a].ItemArray[1].ToString() + " " + dtf.Rows[a].ItemArray[2].ToString();
                                femls[femlindex] = list;
                                femlindex++;
                            }
                        }
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////
            //===============P R I N T I N G  O F  C O N T E N T ===============================
            if (males.Length > femls.Length || males.Length == femls.Length)
            {
                int getMaxPages = males.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepagePFS.Visible = true;
                        lblnudPFS.Visible = false; nudPFS.Visible = false;

                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepagePFS.Visible = true;
                        lblnudPFS.Visible = true; nudPFS.Visible = true;

                    }
                }
                else
                {
                    if (males.Length == 30)
                    {
                        ofpagecount = 1;
                        lblnotepagePFS.Visible = true;
                        lblnudPFS.Visible = false; nudPFS.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepagePFS.Visible = true;
                        lblnudPFS.Visible = true; nudPFS.Visible = true;
                    }

                }
                lblnotepagePFS.Text = "Page " + nudPFS.Text + " of " + ofpagecount;
                nudPFS.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudPFS.Items.Add(p);
                    nudPFS.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberMale_ml < males.Length)
                {
                    e.Graphics.DrawString(males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    if (cntrowsFem_ml < femls.Length)
                    {
                        e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);
                    }

                    totalnumberMale_ml += 1;
                    if (itemperpageMale_ml < 29)
                    {
                        itemperpageMale_ml += 1;
                        e.HasMorePages = false;
                        cntrowsMale_ml++;
                        cntrowsFem_ml++;
                    }

                    else
                    {
                        itemperpageMale_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;
                        if (cntrowsMale_ml != males.Length && males.Length > cntrowsMale_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }
                    }

                    if (cntrowsMale_ml == males.Length)
                    {
                        cntrowsMale_ml = 0;
                        totalnumberMale_ml = males.Length + 1;//to stop iteration
                    }
                    //--------------------------
                }
            }
            if (femls.Length > males.Length)
            {
                int getMaxPages = femls.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepagePFS.Visible = true;
                        lblnudPFS.Visible = false; nudPFS.Visible = false;

                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepagePFS.Visible = true;
                        lblnudPFS.Visible = true; nudPFS.Visible = true;
                    }
                }
                else
                {
                    if (femls.Length == 30)
                    {
                        ofpagecount = 1;
                        lblnotepagePFS.Visible = true;
                        lblnudPFS.Visible = false; nudPFS.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepagePFS.Visible = true;
                        lblnudPFS.Visible = true; nudPFS.Visible = true;
                    }
                }
                lblnotepagePFS.Text = "Page " + nudPFS.Text + " of " + ofpagecount;
                nudPFS.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudPFS.Items.Add(p);
                    nudPFS.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberFem_ml < femls.Length)
                {
                    if (cntrowsMale_ml < males.Length)
                    {
                        e.Graphics.DrawString(males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    }
                    e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);

                    totalnumberFem_ml += 1;
                    if (itemperpageFem_ml < 29)
                    {
                        cntrowsFem_ml++;
                        cntrowsMale_ml++;
                        itemperpageFem_ml += 1;
                        e.HasMorePages = false;

                    }

                    else
                    {
                        itemperpageFem_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;
                        if (cntrowsFem_ml != femls.Length && femls.Length > cntrowsFem_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }

                    }

                    if (cntrowsFem_ml == femls.Length)
                    {
                        cntrowsFem_ml = 0;
                        totalnumberFem_ml = femls.Length + 1;//to stop iteration
                    }
                }
            }
            //--footer
            //----------------------------------------------------------
            int totalstud = CountMale + CountFeml;
            preparedby = cmbPrepared.Text;
            if ((ymstart >= yfstart) && (CountFeml != 0 || CountMale!= 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, ymstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, ymstart + 90, sf1);
            }
            if ((yfstart > ymstart) && (CountFeml != 0 || CountMale != 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, yfstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, yfstart + 90, sf1);
            }

            if (CountFeml == 0 && CountMale == 0)
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, 260, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, 290, sf1);
            }
            //========

           
        }

        private void nudGS_SelectedItemChanged(object sender, EventArgs e)
        {
            int curpage = Convert.ToInt32(nudGS.Text) - 1;
            ppcGS.StartPage = curpage;
            lblnotepageGS.Text = "Page " + nudGS.Text + " of " + ofpagecount;
        }

        private void pdGS_PrintPage(object sender, PrintPageEventArgs e)
        {
            int current = Convert.ToInt32(DateTime.Now.Year);
            int upcoming = current + 1;
            string sy = activeSY;
            int ymstart = 0;
            int yfstart = 0;

            // String format
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Near;

            // Create font and brush.
            Font df1 = new Font("Arial", 12, FontStyle.Bold);
            Font df4 = new Font("Arial", 11, (FontStyle.Bold));
            Font df2 = new Font("Arial", 12, FontStyle.Regular);
            Font df3 = new Font("Arial", 11, FontStyle.Regular);
            Font df5 = new Font("Arial", 8, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            //REPORT'S HEADER
            e.Graphics.Clear(Color.White);

            Rectangle r = new Rectangle(100, 55, 100, 95);
            Image newImage = Image.FromFile(@"C:\Users\valued client\Documents\Visual Studio 2010\Projects\1 - THESIS\berlyn.bmp");
            e.Graphics.DrawImage(newImage, r);

            e.Graphics.DrawString("Berlyn Academy", df1, Brushes.Black, 420, 65, sf);
            e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df3, Brushes.Black, 420, 85, sf);
            e.Graphics.DrawString("LIST OF GRADUATING STUDENTS", df4, Brushes.Black, 420, 115, sf);
            e.Graphics.DrawString("" + sy, df3, Brushes.Black, 420, 135, sf);
            e.Graphics.DrawString("Grade level: " + "Grade 10", df4, Brushes.Black, 420, 155, sf);

            e.Graphics.DrawString("Printed Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), df5, Brushes.Black, 575, 1005, sf1);

            double GenAve = 0;
            //RETRIEVE DATA FOR THOSE MALE STUDENTS WHO ARE ENROLLED
            string[] males = new string[0];
            int malecash = 0;
            int maleinst = 0;
            int newIndexinArrayMales = 0;
            con.Open();
            OdbcDataAdapter dam = new OdbcDataAdapter("Select*from stud_tbl where syregistered='" + activeSY + "' and level='" + "Grade 10" + "'and gender='" + "Male" + "'Order by lname ASC", con);
            DataTable dtm = new DataTable();//                                  may change to sy, may syenrolled=activeSY
            dam.Fill(dtm);
            con.Close();

            if (dtm.Rows.Count > 0)
            {
                males = new string[dtm.Rows.Count];
                ymstart = 230;
                for (int i = 0; i < dtm.Rows.Count; i++)
                {
                    string modeofPay = dtm.Rows[i].ItemArray[22].ToString();
                    if (modeofPay == "Cash")
                    {
                        con.Open();
                        OdbcDataAdapter daminfo = new OdbcDataAdapter("Select*from stud_tbl where studno='" + dtm.Rows[i].ItemArray[0].ToString() + "'", con);
                        DataTable dtminfo = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                        daminfo.Fill(dtminfo);
                        con.Close();
                        if(dtminfo.Rows.Count>0)
                        {
                            con.Open();
                            OdbcDataAdapter da = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + dtm.Rows[i].ItemArray[0].ToString() + "'", con);
                            DataTable dt = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            da.Fill(dt);
                            con.Close();
                            if (dt.Rows.Count > 0)
                            {
                                malecash = Convert.ToInt32(dt.Rows.Count);
                           
                                if (dt.Rows[0].ItemArray[4].ToString() != "")
                                {
                                    con.Open();
                                    OdbcDataAdapter dam1 = new OdbcDataAdapter("Select*from gradetengrades_tbl where studno='" + dtm.Rows[i].ItemArray[0].ToString() + "'", con);
                                    DataTable dtm1 = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                                    dam1.Fill(dtm1);
                                    con.Close();

                                    if (dtm1.Rows.Count > 0)
                                    {
                                        con.Open();
                                        OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(ave)from gradetengrades_tbl where studno='" + dtm1.Rows[0].ItemArray[0].ToString() + "'", con);
                                        DataTable dtk11 = new DataTable();
                                        dak11.Fill(dtk11);
                                        con.Close();
                                        if (dtk11.Rows.Count > 0)
                                        {
                                            if (dtk11.Rows[0].ItemArray[0].ToString() != "")
                                            {
                                                GenAve = Convert.ToDouble(dtk11.Rows[0].ItemArray[0].ToString());
                                            }
                                            // MessageBox.Show(GenAve.ToString() + "     studno" + dtm1.Rows[0].ItemArray[0].ToString());
                                            if (GenAve >= 75)
                                            {
                                           
                                                for (int a = 0; a < dtminfo.Rows.Count; a++)
                                                {
                                                    string list = newIndexinArrayMales + 1 + ". " + dtminfo.Rows[a].ItemArray[3].ToString() + ", " + dtminfo.Rows[a].ItemArray[1].ToString() + " " + dtminfo.Rows[a].ItemArray[2].ToString();
                                                    males[newIndexinArrayMales] = list;
                                                    newIndexinArrayMales++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (modeofPay == "Installment")
                    {
                        con.Open();
                        OdbcDataAdapter daminfo = new OdbcDataAdapter("Select*from stud_tbl where studno='" + dtm.Rows[i].ItemArray[0].ToString() + "'", con);
                        DataTable dtminfo = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                        daminfo.Fill(dtminfo);
                        con.Close();
                        if (dtminfo.Rows.Count > 0)
                        {
                            con.Open();
                            OdbcDataAdapter da = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + dtm.Rows[i].ItemArray[0].ToString() + "'", con);
                            DataTable dt = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            da.Fill(dt);
                            con.Close();
                            if (dt.Rows.Count > 0)
                            {
                                maleinst = Convert.ToInt32(dt.Rows.Count);
                           
                                double balance = Convert.ToDouble(dt.Rows[0].ItemArray[4].ToString());
                                if (balance <= 0)
                                {
                                    con.Open();
                                    OdbcDataAdapter dam1 = new OdbcDataAdapter("Select*from gradetengrades_tbl where studno='" + dtm.Rows[i].ItemArray[0].ToString() + "'", con);
                                    DataTable dtm1 = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                                    dam1.Fill(dtm1);
                                    con.Close();

                                    if (dtm1.Rows.Count > 0)
                                    {
                                        con.Open();
                                        OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(ave)from gradetengrades_tbl where studno='" + dtm1.Rows[0].ItemArray[0].ToString() + "'", con);
                                        DataTable dtk11 = new DataTable();
                                        dak11.Fill(dtk11);
                                        con.Close();
                                        if (dtk11.Rows.Count > 0)
                                        {
                                            if (dtk11.Rows[0].ItemArray[0].ToString() != "")
                                            {
                                                GenAve = Convert.ToDouble(dtk11.Rows[0].ItemArray[0].ToString());
                                            }
                                            // MessageBox.Show(GenAve.ToString() + "     studno" + dtm1.Rows[0].ItemArray[0].ToString());
                                            if (GenAve >= 75)
                                            {
                                                for (int a = 0; a < dtminfo.Rows.Count; a++)
                                                {
                                                    string list = newIndexinArrayMales + 1 + ". " + dtminfo.Rows[a].ItemArray[3].ToString() + ", " + dtminfo.Rows[a].ItemArray[1].ToString() + " " + dtminfo.Rows[a].ItemArray[2].ToString();
                                                    males[newIndexinArrayMales] = list;
                                                    newIndexinArrayMales++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

           
            if (malecash<= 0 && maleinst <= 0)
            {
                totalnumberMale_ml = 30;//this will set to max no. to not iterate
            }

            if (malecash > 0 || maleinst > 0)
            {
                e.Graphics.DrawString("Male", df3, Brushes.Black, 100, 220, sf1);
            }
           
            //RETRIEVE DATA FOR THOSE FEMALE STUDENTS WHO ARE ENROLLED
            string[] femls = new string[0];
            int cashfeml = 0;
            int instfeml = 0;
            int newIndexinArrayFemls = 0;
            con.Open();
            OdbcDataAdapter daF = new OdbcDataAdapter("Select*from stud_tbl where syregistered='" + activeSY + "' and level='" + "Grade 10" + "'and gender='" + "Female" + "'Order by lname ASC", con);
            DataTable dtF = new DataTable();//                                  may change to sy, may syenrolled=activeSY
            daF.Fill(dtF);
            con.Close();

            if (dtF.Rows.Count > 0)
            {
                femls = new string[dtF.Rows.Count];
                yfstart = 230;

                for (int i = 0; i < dtF.Rows.Count; i++)
                {
                    string modeofPay = dtF.Rows[i].ItemArray[22].ToString();
                    if (modeofPay == "Cash")
                    {
                        con.Open();
                        OdbcDataAdapter dafinfo = new OdbcDataAdapter("Select*from stud_tbl where studno='" + dtF.Rows[i].ItemArray[0].ToString() + "'", con);
                        DataTable dtfinfo = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                        dafinfo.Fill(dtfinfo);
                        con.Close();
                        if (dtfinfo.Rows.Count > 0)
                        {
                            con.Open();
                            OdbcDataAdapter da = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + dtF.Rows[i].ItemArray[0].ToString() + "'", con);
                            DataTable dt = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            da.Fill(dt);
                            con.Close();
                            if (dt.Rows.Count > 0)
                            {
                                cashfeml = Convert.ToInt32(dt.Rows.Count);
                            
                                if (dt.Rows[0].ItemArray[4].ToString() != "")
                                {
                                    con.Open();
                                    OdbcDataAdapter daf1 = new OdbcDataAdapter("Select*from gradetengrades_tbl where studno='" + dtF.Rows[i].ItemArray[0].ToString() + "'", con);
                                    DataTable dtf1 = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                                    daf1.Fill(dtf1);
                                    con.Close();

                                    if (dtf1.Rows.Count > 0)
                                    {
                                        con.Open();
                                        OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(ave)from gradetengrades_tbl where studno='" + dtf1.Rows[0].ItemArray[0].ToString() + "'", con);
                                        DataTable dtk11 = new DataTable();
                                        dak11.Fill(dtk11);
                                        con.Close();
                                        if (dtk11.Rows.Count > 0)
                                        {
                                            if (dtk11.Rows[0].ItemArray[0].ToString() != "")
                                            {
                                                GenAve = Convert.ToDouble(dtk11.Rows[0].ItemArray[0].ToString());
                                            }
                                            // MessageBox.Show(GenAve.ToString() + "     studno" + dtm1.Rows[0].ItemArray[0].ToString());
                                            if (GenAve >= 75)
                                            {
                                                for (int a = 0; a < dtk11.Rows.Count; a++)
                                                {
                                                    string list = newIndexinArrayFemls + 1 + ". " + dtfinfo.Rows[a].ItemArray[3].ToString() + ", " + dtfinfo.Rows[a].ItemArray[1].ToString() + " " + dtfinfo.Rows[a].ItemArray[2].ToString();
                                                    femls[newIndexinArrayFemls] = list;
                                                    newIndexinArrayFemls++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (modeofPay == "Installment")
                    {
                        con.Open();
                        OdbcDataAdapter dafinfo = new OdbcDataAdapter("Select*from stud_tbl where studno='" + dtF.Rows[i].ItemArray[0].ToString() + "'", con);
                        DataTable dtfinfo = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                        dafinfo.Fill(dtfinfo);
                        con.Close();
                        if (dtfinfo.Rows.Count > 0)
                        {
                            con.Open();
                            OdbcDataAdapter da = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + dtF.Rows[i].ItemArray[0].ToString() + "'", con);
                            DataTable dt = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                            da.Fill(dt);
                            con.Close();
                            if (dt.Rows.Count > 0)
                            {
                                instfeml = Convert.ToInt32(dt.Rows.Count);
                            
                                double balance = Convert.ToDouble(dt.Rows[0].ItemArray[4].ToString());
                                if (balance <= 0)
                                {
                                    con.Open();
                                    OdbcDataAdapter daf1 = new OdbcDataAdapter("Select*from gradetengrades_tbl where studno='" + dtF.Rows[i].ItemArray[0].ToString() + "'", con);
                                    DataTable dtf1 = new DataTable();//                                  may change to sy, may syenrolled=activeSY
                                    daf1.Fill(dtf1);
                                    con.Close();

                                    if (dtf1.Rows.Count > 0)
                                    {
                                        con.Open();
                                        OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(ave)from gradetengrades_tbl where studno='" + dtf1.Rows[0].ItemArray[0].ToString() + "'", con);
                                        DataTable dtk11 = new DataTable();
                                        dak11.Fill(dtk11);
                                        con.Close();
                                        if (dtk11.Rows.Count > 0)
                                        {
                                            if (dtk11.Rows[0].ItemArray[0].ToString() != "")
                                            {
                                                GenAve = Convert.ToDouble(dtk11.Rows[0].ItemArray[0].ToString());
                                            }
                                            // MessageBox.Show(GenAve.ToString() + "     studno" + dtm1.Rows[0].ItemArray[0].ToString());
                                            if (GenAve >= 75)
                                            {
                                                for (int a = 0; a < dtF.Rows.Count; a++)
                                                {
                                                    string list = newIndexinArrayFemls + 1 + ". " + dtfinfo.Rows[a].ItemArray[3].ToString() + ", " + dtfinfo.Rows[a].ItemArray[1].ToString() + " " + dtfinfo.Rows[a].ItemArray[2].ToString();
                                                    femls[newIndexinArrayFemls] = list;
                                                    newIndexinArrayFemls++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (cashfeml <= 0 && instfeml <= 0)
            {
                totalnumberFem_ml = 30;//this will set to max no. to not iterate
            }

            if (cashfeml > 0 || instfeml > 0)
            {
                e.Graphics.DrawString("Female", df3, Brushes.Black, 500, 220, sf1);
            }
            //PRINTING OF CONTENT//================================================================================
            //=====================================================================================================

            if (males.Length > femls.Length || males.Length == femls.Length)
            {
                int getMaxPages = males.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepageGS.Visible = true;
                        lblnudGS.Visible = false; nudGS.Visible = false;

                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepageGS.Visible = true;
                        lblnudGS.Visible = true; nudGS.Visible = true;

                    }
                }
                else
                {
                    if (males.Length == 30)
                    {
                        ofpagecount = 1;
                        lblnotepageGS.Visible = true;
                        lblnudGS.Visible = false; nudGS.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepageGS.Visible = true;
                        lblnudGS.Visible = true; nudGS.Visible = true;
                    }

                }
                lblnotepageGS.Text = "Page " + nudGS.Text + " of " + ofpagecount;
                nudGS.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudGS.Items.Add(p);
                    nudGS.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberMale_ml < males.Length)
                {
                    //MessageBox.Show(males[cntrowsMale_ml]+" "+ totalnumberMale_ml);
                    e.Graphics.DrawString(males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    if (cntrowsFem_ml < femls.Length)
                    {
                        e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);
                    }

                    totalnumberMale_ml += 1;
                    if (itemperpageMale_ml < 29)
                    {
                        itemperpageMale_ml += 1;
                        e.HasMorePages = false;
                        cntrowsMale_ml++;
                        cntrowsFem_ml++;
                    }

                    else
                    {
                        itemperpageMale_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;

                        //IF EXACT THE LENGTH TO 3O

                        if (cntrowsMale_ml != males.Length && males.Length > cntrowsMale_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }
                    }

                    if (cntrowsMale_ml == males.Length)
                    {
                        cntrowsMale_ml = 0;
                        totalnumberMale_ml = males.Length + 1;//to stop iteration
                    }
                }
            }
            if (femls.Length > males.Length)
            {
                int getMaxPages = femls.Length / 30;
                if (getMaxPages % 2 == 0)
                {
                    if (getMaxPages <= 30)
                    {
                        ofpagecount = 1;
                        lblnotepageGS.Visible = true;
                        lblnudGS.Visible = false; nudGS.Visible = false;

                    }
                    else
                    {
                        ofpagecount = getMaxPages;
                        lblnotepageGS.Visible = true;
                        lblnudGS.Visible = true; nudGS.Visible = true;
                    }
                }
                else
                {
                    if (femls.Length == 30)
                    {
                        ofpagecount = 1;
                        lblnotepageGS.Visible = true;
                        lblnudGS.Visible = false; nudGS.Visible = false;
                    }
                    else
                    {
                        ofpagecount = getMaxPages + 1;
                        lblnotepageGS.Visible = true;
                        lblnudGS.Visible = true; nudGS.Visible = true;
                    }
                }

                lblnotepageGS.Text = "Page " + nudGS.Text + " of " + ofpagecount;
                nudGS.Items.Clear();
                for (int p = 1; p <= ofpagecount; p++)
                {
                    nudGS.Items.Add(p);
                    nudGS.Text = "1";
                }

                e.Graphics.DrawString("Page " + pagecount + " of " + ofpagecount, df3, Brushes.Black, 420, 1000, sf);

                while (totalnumberFem_ml < femls.Length)
                {
                    if (cntrowsMale_ml < males.Length)
                    {
                        e.Graphics.DrawString(males[cntrowsMale_ml], df3, Brushes.Black, 100, ymstart += 20, sf1);
                    }
                    e.Graphics.DrawString(femls[cntrowsFem_ml], df3, Brushes.Black, 500, yfstart += 20, sf1);

                    totalnumberFem_ml += 1;
                    if (itemperpageFem_ml < 29)
                    {
                        cntrowsFem_ml++;
                        cntrowsMale_ml++;
                        itemperpageFem_ml += 1;
                        e.HasMorePages = false;

                    }

                    else
                    {
                        itemperpageFem_ml = 0;
                        cntrowsMale_ml += 1;
                        cntrowsFem_ml += 1;

                        if (cntrowsFem_ml != femls.Length && femls.Length > cntrowsFem_ml)
                        {
                            e.HasMorePages = true;
                            pagecount++;
                            return;
                        }
                    }

                    if (cntrowsFem_ml == femls.Length)
                    {
                        cntrowsFem_ml = 0;
                        totalnumberFem_ml = femls.Length + 1;//to stop iteration
                    }
                }
            }
            //                                    F  O  O  T  E  R
            //=====================================================================================================
            //=====================================================================================================

            int totalstud = newIndexinArrayMales + newIndexinArrayFemls;
            //int allMales = malecash + maleinst;
            //int allFemls = cashfeml + instfeml;

            string prepEsty = cmbPrepESTY.Text;

            if ((ymstart >= yfstart) && (newIndexinArrayFemls != 0 || newIndexinArrayMales != 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, ymstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, ymstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, ymstart + 90, sf1);
            }
            if ((yfstart > ymstart) && (newIndexinArrayFemls != 0 || newIndexinArrayMales != 0))
            {
                e.Graphics.DrawString("Total students: " + totalstud, df3, Brushes.Black, 100, yfstart + 50, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, yfstart + 90, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, yfstart + 90, sf1);
            }

            if (newIndexinArrayFemls == 0 && newIndexinArrayMales == 0)
            {
                e.Graphics.DrawString("Total students: " + 0, df3, Brushes.Black, 100, 255, sf1);
                e.Graphics.DrawString(co, df4, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("__________________________", df3, Brushes.Black, 205, 290, sf1);
                e.Graphics.DrawString("Prepared by:", df4, Brushes.Black, 103, 290, sf1);
            }
           
        }

        private void cmbAve_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSubs.SelectedIndex = -1;
            cmbGrading.SelectedIndex = -1;
            cmbSubs.Enabled = false;
            cmbGrading.Enabled = false;
        }

        private void cmbGrading_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubs.Text != "" && cmbGrading.Text != "")
            {
                cmbAve.Enabled = false;
            }
        }

        private void cmbSubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubs.Text != "" && cmbGrading.Text != "")
            {
                cmbAve.Enabled = false;
            }
        }
    }
}
