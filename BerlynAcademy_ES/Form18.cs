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
    public partial class frmStudInfo : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string studlog,emptype,TheFaculty,CO,accesscode,VISITED,notifstat;
        public bool isVisited, viewNotifDue, viewNotifDisc, viewNotifLate;
        public DataView dvStud;

        public frmStudInfo()
        {
            InitializeComponent();
        }

        private void frmStudInfo_Load(object sender, EventArgs e)
        {
            //this.BackColor = Color.FromArgb(0, 0, 25);
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //pnlhead.BackColor = Color.FromArgb(244, 194, 13);
            //MessageBox.Show(CO);
           
            pnlnotify.Visible = false;
            lblLogFacPos.Text = emptype;
            cmbFilter.Text = "Student number";

            if (emptype == "Faculty")
            {
                pnlMenuFac.Visible = true;
                pnlMenuCas.Visible = false;
                pnlMenuReg.Visible = false;
                pnlMenuPrin.Visible = false;
                pnlMenuFac.Location = new Point(0, 0);
                pnlMenuFac.Size = new System.Drawing.Size(263, 757);
                lblLogFac.Text = studlog;
                
                //btnHome.Text = "          " + studlog;

            }
            if (emptype == "cashier")
            {
                pnlMenuCas.Visible = true;
                pnlMenuFac.Visible = false;
                pnlMenuReg.Visible = false;
                pnlMenuPrin.Visible = false;
                pnlMenuCas.Location = new Point(0, 0);
                pnlMenuCas.Size = new System.Drawing.Size(263, 757);
                lblLogCas.Text = studlog;
                //btnHomeCas.Text = "          " + studlog;
            }
            if (emptype == "registrar")
            {
                pnlMenuCas.Visible = false;
                pnlMenuFac.Visible = false;
                pnlMenuReg.Visible = true;
                pnlMenuPrin.Visible = false;
                pnlMenuReg.Location = new Point(0, 0);
                pnlMenuReg.Size = new System.Drawing.Size(263, 757);
                lblLogReg.Text = studlog;
               // btnHomeReg.Text = "          " + studlog;
            }
            if (emptype == "principal")
            {
                pnlMenuCas.Visible = false;
                pnlMenuFac.Visible = false;
                pnlMenuReg.Visible = false;
                pnlMenuPrin.Visible = true;
                pnlMenuPrin.Location = new Point(0, 0);
                pnlMenuPrin.Size = new System.Drawing.Size(263, 757);
                lblLoggerPrin.Text = studlog;
                //btnHomePrin.Text = "          " + studlog;
            }
            pnlMenuFac.Visible = true;
            lblLogFac.Text = studlog;
            setupview();
            if (dgvSearch.Rows.Count > 0)
            {
                setupinfo(dgvSearch.Rows[0].Cells[0].Value.ToString());
            }
            else
            {
                clear();
            }

            if (isVisited == false)
            {
                if (VISITED.Contains("Student information") == false)
                {
                    VISITED += "   Student information";
                    isVisited = true;
                }
            }

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

            int getSIindex = 1;
            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                getSIindex++;
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                getSIindex++;
                dtMenu.Rows.Add("  " + dt2.Rows[0].ItemArray[1].ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                getSIindex++;
                dtMenu.Rows.Add("  " + dt3.Rows[0].ItemArray[1].ToString());
            }
            if (dt4.Rows.Count > 0)
            {
                getSIindex++;
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
            dgvm.Rows[getSIindex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        public void clear()
        {
            lblTheName.Text = "";
            lblTheLevsec.Text = "";
            lblbd.Text = "";
            lblgen.Text ="";
            lbladd.Text ="";
            lblscon.Text = "";
            lblTal.Text ="";
            lblAwa.Text = "";
            lblFat.Text ="";
            lblFatOcc.Text ="";
            lblMot.Text = "";
            lblMotOcc.Text ="";
            lblGar.Text ="";
            lblGarOcc.Text = "";
            lblParGarcon.Text = "";
            lblPGRelation.Text = "";
        }

        public void setupview()
        {
            string activeSY="";
            con.Open();
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='"+"Active"+"'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);
            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString(); }

            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'No',(select concat(lname,', ',fname,' ',mname))as 'Name' from stud_tbl where status='"+"Active"+"' order by lname ASC", con);
            DataTable dts = new DataTable();
            da.Fill(dts);
            con.Close();
            dvStud = new DataView(dts);
            if (dts.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvStud;
                dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[0].Width = 95;
                dgvSearch.Columns[1].Width = 165;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
                lblTheAdviser.Text = "";
            }
            lblResult.Text = "number of students: " + dgvSearch.Rows.Count.ToString();
        }

        public void setupinfo(string key)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='"+key+"'", con);
            DataTable dti = new DataTable();
            da.Fill(dti);
            con.Close();
            if (dti.Rows.Count > 0)
            {
                string whole = dti.Rows[0].ItemArray[3].ToString() + ", " + dti.Rows[0].ItemArray[1].ToString() + " " + dti.Rows[0].ItemArray[2].ToString();
                string levsec = "";
                lblTheName.Text = whole;

                if (dti.Rows[0].ItemArray[5].ToString() == "")
                {
                    levsec = dti.Rows[0].ItemArray[4].ToString();
                }
                else
                {
                    levsec = dti.Rows[0].ItemArray[4].ToString() + " - " + dti.Rows[0].ItemArray[5].ToString();
                }
                lblTheLevsec.Text = levsec;

                lblbd.Text = dti.Rows[0].ItemArray[8].ToString();
                lblgen.Text = dti.Rows[0].ItemArray[10].ToString();
                lbladd.Text = dti.Rows[0].ItemArray[7].ToString();
                lbllevel.Text = dti.Rows[0].ItemArray[4].ToString();
                lblsection.Text = dti.Rows[0].ItemArray[5].ToString();


                if (dti.Rows[0].ItemArray[5].ToString() == "" || dti.Rows[0].ItemArray[4].ToString() == "")
                {//display for those student who dont have grade or section
                    lblTheAdviser.Text = "Faculty ABC";
                    lbladviser.Text = "none";
                }
                else
                {
                    con.Open();
                    OdbcDataAdapter daadv = new OdbcDataAdapter("Select*from employees_tbl where grade='" + dti.Rows[0].ItemArray[4].ToString() + "'and advisory='"+dti.Rows[0].ItemArray[5].ToString()+"'", con);
                    DataTable dtlookadv = new DataTable();
                    daadv.Fill(dtlookadv);
                    con.Close();
                    if (dtlookadv.Rows.Count > 0)
                    {
                        string facwhole = dtlookadv.Rows[0].ItemArray[1].ToString() + ", " + dtlookadv.Rows[0].ItemArray[2].ToString() + " " + dtlookadv.Rows[0].ItemArray[3].ToString();
                        lblTheAdviser.Text = facwhole;
                        lbladviser.Text = facwhole;
                    }
                    else//display for those student who has a grade/section but their adviser didnt set.
                    {
                        lblTheAdviser.Text = "Faculty ABC";
                        lbladviser.Text = "none";
                    }
                }

                if (dti.Rows[0].ItemArray[9].ToString() == "")
                {
                    lblscon.Text = "none";
                }
                else
                {
                    lblStdAge.Text = dti.Rows[0].ItemArray[9].ToString();
                }
                if (dti.Rows[0].ItemArray[11].ToString() == "")
                {
                    lblscon.Text = "none";
                }
                else
                {
                    lblscon.Text = dti.Rows[0].ItemArray[11].ToString();
                }

                if (dti.Rows[0].ItemArray[19].ToString() == "")
                {
                    lblTal.Text = "none";
                }
                else
                {
                    lblTal.Text = dti.Rows[0].ItemArray[19].ToString();
                }

                if (dti.Rows[0].ItemArray[20].ToString() == "")
                {
                    lblAwa.Text = "none";
                }
                else
                {
                    lblAwa.Text = dti.Rows[0].ItemArray[20].ToString();
                }

                if (dti.Rows[0].ItemArray[12].ToString() == "")
                {
                    lblFat.Text = "none";
                }
                else
                {
                    lblFat.Text = dti.Rows[0].ItemArray[12].ToString();
                }

                if (dti.Rows[0].ItemArray[13].ToString() == "")
                {
                    lblFatOcc.Text = "none";
                }
                else
                {
                    lblFatOcc.Text = dti.Rows[0].ItemArray[13].ToString();
                }

                if (dti.Rows[0].ItemArray[14].ToString() == "")
                {
                    lblMot.Text = "none";
                }
                else
                {
                    lblMot.Text = dti.Rows[0].ItemArray[14].ToString();
                }

                if (dti.Rows[0].ItemArray[15].ToString() == "")
                {
                    lblMotOcc.Text = "none";
                }
                else
                {
                    lblMotOcc.Text = dti.Rows[0].ItemArray[15].ToString();
                }

                if (dti.Rows[0].ItemArray[16].ToString() == "")
                {
                    lblGar.Text = "none";
                }
                else
                {
                    lblGar.Text = dti.Rows[0].ItemArray[16].ToString();
                }

                if (dti.Rows[0].ItemArray[17].ToString() == "")
                {
                    lblGarOcc.Text = "none";
                }
                else
                {
                    lblGarOcc.Text = dti.Rows[0].ItemArray[17].ToString();
                }

                if (dti.Rows[0].ItemArray[18].ToString() == "")
                {
                    lblParGarcon.Text = "none";
                }
                else
                {
                    lblParGarcon.Text = dti.Rows[0].ItemArray[18].ToString();
                }

                if (dti.Rows[0].ItemArray[25].ToString() == "")
                {
                    lblPGRelation.Text = "none";
                }
                else
                {
                    lblPGRelation.Text = dti.Rows[0].ItemArray[25].ToString();
                }

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text =="Student number")
            {
                dvStud.RowFilter = string.Format("No LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvStud;
            }
            if (cmbFilter.Text == "Student's name")
            {
                dvStud.RowFilter = string.Format("Name LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvStud;
            }
           

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

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }
            setupinfo(dgvSearch.SelectedRows[0].Cells[0].Value.ToString());
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmEmpMain empf = new frmEmpMain();
            this.Hide();
            empf.faclog = studlog;
            empf.TheFacultyName = TheFaculty;
            empf.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmEmpAbout abf = new frmEmpAbout();
            this.Hide();
            abf.ablog = studlog;
            abf.emptype = "faculty";
            abf.theFaculty = TheFaculty;
            abf.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
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

        private void frmStudInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin logf = new frmEmpLogin();
            this.Hide();
            logf.Show();
        }

        private void btnFac_Click(object sender, EventArgs e)
        {
            frmFacInfo facf = new frmFacInfo();
            this.Hide();
            facf.emptype = "faculty";
            facf.facinfolog = studlog;
            facf.TheFaculty = TheFaculty;
            facf.Show();
        }

        private void btnAdm_Click(object sender, EventArgs e)
        {
            frmAdmission formadm = new frmAdmission();
            this.Hide();
            formadm.admlog = studlog;
            formadm.TheFaculty = TheFaculty;
            formadm.Show();
        }

        private void btnGrd_Click(object sender, EventArgs e)
        {
            frmStdGrd fgrd = new frmStdGrd();
            this.Hide();
            fgrd.grdlog = studlog;
            fgrd.theFacultyName = TheFaculty;
            fgrd.Show();
        }

        private void btnActCas_Click(object sender, EventArgs e)
        {
            frmCashierMain cmain = new frmCashierMain();
            this.Hide();
            cmain.cashlog = studlog;
            cmain.CO = CO;
            cmain.Show();
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            frmEmpAbout abtcas = new frmEmpAbout();
            this.Hide();
            abtcas.emptype = "cashier";
            abtcas.ablog = studlog;
            abtcas.CO = CO;
            abtcas.Show();
        }

        private void btnHomeCas_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnAbtReg_Click(object sender, EventArgs e)
        {
            frmEmpAbout ea = new frmEmpAbout();
            this.Hide();
            ea.ablog = studlog;
            ea.emptype = "registrar";
            ea.Show();
        }

        private void btnActReg_Click(object sender, EventArgs e)
        {
            frmRegistrarMain regmain = new frmRegistrarMain();
            this.Hide();
            regmain.reglog = studlog;
            regmain.Show();
        }

        private void btnAssReg_Click(object sender, EventArgs e)
        {
            frmAssessment ass = new frmAssessment();
            this.Hide();
            ass.asslog = studlog;
            ass.Show();
        }

        private void btnHomeReg_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnActPrin_Click(object sender, EventArgs e)
        {
            frmPrincipalMain pmf = new frmPrincipalMain();
            this.Hide();
            pmf.prinlog = studlog;
            pmf.Show();
        }

        private void btnFacPrin_Click(object sender, EventArgs e)
        {
            frmFacInfo fif = new frmFacInfo();
            this.Hide();
            fif.facinfolog = studlog;
            fif.emptype = "principal";
            fif.Show();
        }

        private void btnAbtPrin_Click(object sender, EventArgs e)
        {
            frmEmpAbout eaf = new frmEmpAbout();
            this.Hide();
            eaf.ablog = studlog;
            eaf.emptype = "principal";
            eaf.Show();
        }

        private void btnHomePrin_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnReprin_Click(object sender, EventArgs e)
        {
            frmReport rf = new frmReport();
            this.Hide();
            rf.replog = studlog;
            rf.emptype = "principal";
            rf.theFaculty = TheFaculty;
            rf.Show();
        }

        private void btnRepFac_Click(object sender, EventArgs e)
        {
            frmReport rff = new frmReport();
            this.Hide();
            rff.replog = studlog;
            rff.emptype = "faculty";
            rff.theFaculty = TheFaculty;
            rff.Show();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            frmPayment payf = new frmPayment();
            this.Hide();
            payf.paylog = studlog;
            payf.CashierOperator = CO;
            payf.Show();
        }

        private void btnAdmission_Click(object sender, EventArgs e)
        {
            frmAdmission admform = new frmAdmission();
            this.Hide();
            admform.admlog = studlog;
            admform.TheFaculty = studlog;
            admform.Show();
        }

        private void btnSectioning_Click(object sender, EventArgs e)
        {
            frmSectioning sectioningfrm = new frmSectioning();
         
            sectioningfrm.seclog = studlog;
            sectioningfrm.TheFaculty = TheFaculty;
            sectioningfrm.Show();
            this.Hide();
        }

        private void btnFacAdv_Click(object sender, EventArgs e)
        {
            frmFacultyAdvisory faf = new frmFacultyAdvisory();
        
            faf.advlog = studlog;
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
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Student information")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Student information")
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

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "Student number")
            {
                toolTip1.SetToolTip(txtSearch, "student number");
            }
            if (cmbFilter.Text == "Student's name")
            {
                toolTip1.SetToolTip(txtSearch, "student's name");
            }

            txtSearch.Clear();
            txtSearch.Focus();
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
                    casmain.cashlog = studlog;
                    casmain.accesscode = accesscode;
                    casmain.CO = CO;
                    casmain.thefac = TheFaculty;
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
                    pmf.thefac = TheFaculty;
                    pmf.prinlog = studlog;
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
                    regmain.reglog = studlog;
                    regmain.accesscode = accesscode;
                    regmain.thefac = TheFaculty;
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
                    empf.faclog = studlog;
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
                frmadm.admlog = studlog;
                frmadm.CO = CO;
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
                formPay.paylog = studlog;
                formPay.CashierOperator = CO;
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
                formStudRec.co = CO;
                formStudRec.asslog = studlog;
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
                formstdgrd.CO = CO;
                formstdgrd.grdlog = studlog;
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
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Faculty information")
            {
                frmFacInfo facf = new frmFacInfo();
                this.Hide();
                facf.emptype = emptype;
                facf.CO = CO;
                facf.facinfolog = studlog;
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
                frmFacAdv.co = CO;
                frmFacAdv.advlog = studlog;
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
                frmSectioning frmSec = new frmSectioning();
                this.Hide();
                frmSec.emptype = emptype;
                frmSec.co = CO;
                frmSec.seclog = studlog;
                frmSec.accesscode = accesscode;
                frmSec.TheFaculty = TheFaculty;
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
                rfac.replog = studlog;
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
                rsched.CO = CO;
                rsched.schedlog = studlog;
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
                about.ablog = studlog;
                about.emptype = emptype;
                about.CO = CO;
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

        
    }
}
