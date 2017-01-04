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
    public partial class frmEmpAbout : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string ablog, emptype, theFaculty, CO, accesscode, VISITED, notifstat;
        public bool isVisited, viewNotifDue, viewNotifDisc, viewNotifLate;

        public frmEmpAbout()
        {
            InitializeComponent();
        }

        private void frmEmpAbout_Load(object sender, EventArgs e)
        {
            //this.BackColor = Color.FromArgb(0, 0, 25);
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //lblabt.BackColor = Color.FromArgb(49, 79, 142);
            viewcontent_about();
            //MessageBox.Show(CO);
          
            //if (emptype == "Faculty")
            //{
                pnlMenuFac.Visible = true;
                pnlMenuFac.Size = new System.Drawing.Size(263, 757);
                pnlMenuCas.Visible = false;
                pnlMenuReg.Visible = false;
                pnlMenuPrin.Visible = false;
                pnlMenuFac.Location = new Point(0, 0);
                lblLogger.Text = ablog;
                lblLoggerPosition.Text = emptype;
                //btnHome.Text = "          " + ablog;
            //}
            /*if (emptype == "Registrar")
            {
                pnlMenuReg.Visible = true;
                pnlMenuReg.Size = new System.Drawing.Size(263, 757);
                pnlMenuCas.Visible = false;
                pnlMenuFac.Visible = false;
                pnlMenuPrin.Visible = false;
                pnlMenuReg.Location = new Point(0, 0);
                lblLoggerReg.Text = ablog;
                
                //btnHomeReg.Text = "          " + ablog;
            }
            if (emptype == "principal")
            {
                pnlMenuPrin.Visible = true;
                pnlMenuPrin.Size = new System.Drawing.Size(263, 757);
                pnlMenuCas.Visible = false;
                pnlMenuFac.Visible = false;
                pnlMenuReg.Visible = false;
                pnlMenuPrin.Location = new Point(0, 0);
                lblLoggerPrin.Text = ablog;
                lblLoggerPrinPos.Text = "Principal";
                //btnHmPrin.Text = "          " + ablog;
            }
            if (emptype == "cashier")
            {
                pnlMenuCas.Visible = true;
                pnlMenuCas.Size = new System.Drawing.Size(263, 757);
                pnlMenuFac.Visible = false;
                pnlMenuReg.Visible = false;
                pnlMenuPrin.Visible = false;
                pnlMenuCas.Location = new Point(0, 0);
                lblLoggerCas.Text = ablog;
                lblLoggerCasPos.Text = "Cashier";
                //btnHomeCas.Text = "          " + ablog;
            }*/

            if (isVisited == false)
            {
                if (VISITED.Contains("About us")==false)
                {
                    VISITED += "   About us";
                    isVisited = true;
                }
            }

            lblLoggerRegPos.Text = emptype;
            setupMENU();
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

            int getaboutindex = 1;
            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt2.Rows[0].ItemArray[1].ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt3.Rows[0].ItemArray[1].ToString());
            }
            if (dt4.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt4.Rows[0].ItemArray[1].ToString());
            }
            if (dt5.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt5.Rows[0].ItemArray[1].ToString());
            }
            if (dt6.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt6.Rows[0].ItemArray[1].ToString());
            }
            if (dt7.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt7.Rows[0].ItemArray[1].ToString());
            }
            if (dt8.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt8.Rows[0].ItemArray[1].ToString());
            }
            if (dt9.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt9.Rows[0].ItemArray[1].ToString());
            }
            if (dt0.Rows.Count > 0)
            {
                getaboutindex++;
                dtMenu.Rows.Add("  " + dt0.Rows[0].ItemArray[1].ToString());
            }

            dtMenu.Rows.Add("  About us");

            dtMenu.Rows.Add("  Logout");

            DataView dvMenu = new DataView(dtMenu);
            dgvm.DataSource = dvMenu;
            dgvm.Rows[0].Selected = false;
            dgvm.Columns[0].Width = 263;
            dgvm.Rows[getaboutindex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        public void viewcontent_about()
        {
            con.Open();

            OdbcDataAdapter da = new OdbcDataAdapter("Select*from about_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblabt.Text = "";
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    string a = dt.Rows[h].ItemArray[0].ToString();
              
                    lblabt.Text = lblabt.Text + a+"\n";
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            this.Hide();
            frmEmpLogin home = new frmEmpLogin();
            home.Show();
        }

        private void frmEmpAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            this.Hide();
            frmEmpLogin home = new frmEmpLogin();
            home.Show();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmEmpMain empf = new frmEmpMain();
            this.Hide();
            empf.faclog = ablog;
            empf.TheFacultyName = theFaculty;
            empf.Show();
        }

        private void btnFac_Click(object sender, EventArgs e)
        {
            frmFacInfo facf = new frmFacInfo();
            this.Hide();
            facf.emptype = "faculty";
            facf.facinfolog = ablog;
            facf.TheFaculty = theFaculty;
            facf.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudInfo stud = new frmStudInfo();
            this.Hide();
            stud.emptype = "faculty";
            stud.studlog = ablog;
            stud.TheFaculty = theFaculty;
            stud.Show();
        }

        private void btnAdm_Click(object sender, EventArgs e)
        {
            frmAdmission fadm = new frmAdmission();
            this.Hide();
            fadm.admlog = ablog;
            fadm.TheFaculty = theFaculty;
            fadm.Show();
        }

        private void btnGrd_Click(object sender, EventArgs e)
        {
            frmStdGrd fgrd = new frmStdGrd();
            this.Hide();
            fgrd.grdlog = ablog;
            fgrd.theFacultyName = theFaculty;
            fgrd.Show();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            frmPayment payf = new frmPayment();
            this.Hide();
            payf.paylog = ablog;
            payf.CashierOperator = CO;
            payf.Show();
        }

        private void btnActCas_Click(object sender, EventArgs e)
        {
            frmCashierMain cmain = new frmCashierMain();
            this.Hide();
            cmain.cashlog = ablog;
            cmain.CO = CO;
            cmain.Show();
        }

        private void btnHomeCas_Click(object sender, EventArgs e)
        {
            LOGOUT();
            this.Hide();
            frmEmpLogin home = new frmEmpLogin();
            home.Show();
        }

        private void btnHomeReg_Click(object sender, EventArgs e)
        {
            LOGOUT();
            this.Hide();
            frmEmpLogin home = new frmEmpLogin();
            home.Show();
        }

        private void btnSIReg_Click(object sender, EventArgs e)
        {
            frmStudInfo si = new frmStudInfo();
            this.Hide();
            si.emptype = "registrar";
            si.studlog = ablog;
            si.Show();
        }

        private void btnAssReg_Click(object sender, EventArgs e)
        {
            frmAssessment ass = new frmAssessment();
            this.Hide();
            ass.asslog = ablog;
            ass.Show();
        }

        private void btnActReg_Click(object sender, EventArgs e)
        {
            frmRegistrarMain regm = new frmRegistrarMain();
            this.Hide();
            regm.reglog = ablog;
            regm.Show();
        }

        private void btnActPrin_Click(object sender, EventArgs e)
        {
            frmPrincipalMain pmf = new frmPrincipalMain();
            this.Hide();
            pmf.prinlog = ablog;
            pmf.Show();
        }

        private void btnStudIPrin_Click(object sender, EventArgs e)
        {
            frmStudInfo sif = new frmStudInfo();
            this.Hide();
            sif.studlog = ablog;
            sif.emptype = "principal";
            sif.Show();
        }

        private void btnFIPrin_Click(object sender, EventArgs e)
        {
            frmFacInfo fif = new frmFacInfo();
            this.Hide();
            fif.facinfolog = ablog;
            fif.emptype = "principal";
            fif.Show();
        }

        private void btnHmPrin_Click(object sender, EventArgs e)
        {
            LOGOUT();
            this.Hide();
            frmEmpLogin home = new frmEmpLogin();
            home.Show();
        }

        private void btnReprin_Click(object sender, EventArgs e)
        {
            frmReport rf = new frmReport();
            this.Hide();
            rf.replog = ablog;
            rf.emptype = "principal";
            rf.theFaculty = theFaculty;
            rf.Show();
        }

        private void btnRepFac_Click(object sender, EventArgs e)
        {
            frmReport rf = new frmReport();
            this.Hide();
            rf.replog = ablog;
            rf.emptype = "faculty";
            rf.theFaculty = theFaculty;
            rf.Show();
        }

        private void btnSICas_Click(object sender, EventArgs e)
        {
            frmStudInfo sicas = new frmStudInfo();
            this.Hide();
            sicas.studlog = ablog;
            sicas.emptype = "cashier";
            sicas.TheFaculty = theFaculty;
            sicas.CO = CO;
            sicas.Show();
        }

        private void pnlMenuFac_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAdmission_Click(object sender, EventArgs e)
        {
            frmAdmission admform = new frmAdmission();
            this.Hide();
            admform.admlog = ablog;
            admform.TheFaculty = ablog;
            admform.Show();
        }

        private void btnSectioning_Click(object sender, EventArgs e)
        {
            frmSectioning sectioningfrm = new frmSectioning();
         
            sectioningfrm.seclog =  ablog;
            sectioningfrm.TheFaculty = ablog;
            sectioningfrm.Show();
            this.Hide();
        }

        private void btnFacAdv_Click(object sender, EventArgs e)
        {
            frmFacultyAdvisory faf = new frmFacultyAdvisory();
          
            faf.advlog = ablog;
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
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  About us")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  About us")
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
                    casmain.cashlog = ablog;
                    casmain.accesscode = accesscode;
                    casmain.CO = CO;
                    casmain.thefac = theFaculty;
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
                    pmf.co = CO;
                    pmf.thefac = theFaculty;
                    pmf.prinlog = ablog;
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
                    regmain.co = CO;
                    regmain.reglog = ablog;
                    regmain.accesscode = accesscode;
                    regmain.thefac = theFaculty;
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
                    empf.CO = CO;
                    empf.faclog = ablog;
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
                frmadm.admlog = ablog;
                frmadm.CO = CO;
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
                formPay.paylog = ablog;
                formPay.CashierOperator = CO;
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
                formStudRec.co = CO;
                formStudRec.asslog = ablog;
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
                formstdgrd.CO = CO;
                formstdgrd.grdlog = ablog;
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
                stud.CO = CO;
                stud.studlog = ablog;
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
                facf.CO = CO;
                facf.facinfolog = ablog;
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
                frmFacAdv.co = CO;
                frmFacAdv.advlog = ablog;
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
                frmSec.co = CO;
                frmSec.seclog = ablog;
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
                frmReport rfac = new frmReport();
                this.Hide();
                rfac.co = CO;
                rfac.replog = ablog;
                rfac.emptype = emptype;
                rfac.accesscode = accesscode;
                rfac.theFaculty = theFaculty;
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
                rsched.schedlog = ablog;
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
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
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
