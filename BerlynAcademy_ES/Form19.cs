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
    public partial class frmFacInfo : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string facinfolog, emptype, TheFaculty, CO, accesscode, VISITED, notifstat;
        public bool isVisited, viewNotifDue, viewNotifDisc, viewNotifLate;
        public DataView dvFI,dvFacSched;

        public frmFacInfo()
        {
            InitializeComponent();
        }

        private void frmFacInfo_Load(object sender, EventArgs e)
        {
            //this.BackColor = Color.FromArgb(49, 79, 142);
           // this.BackColor = Color.FromArgb(0, 0, 25);
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //pnlhead.BackColor = Color.FromArgb(244, 194, 13);
            //pnlcon.Size = new System.Drawing.Size(769, 657);
            //pnlcon.Location = new Point(13, 0);
            pnlfound.Visible = false;
            pnlnotify.Visible = false;
            //MessageBox.Show(TheFaculty);
            lblLogFacPos.Text = emptype;
            
            if (emptype == "Faculty")
            {
                pnlMenuPrin.Visible = false;
                
                pnlMenuFac.Location = new Point(0, 0);
                pnlMenuFac.Size = new System.Drawing.Size(263, 757);
                
               
               // btnHome.Text = "          " + facinfolog;
            }
            if (emptype == "principal")
            {
                pnlMenuFac.Visible = false;
                pnlMenuPrin.Visible = true;
                pnlMenuPrin.Location = new Point(0, 0);
                pnlMenuPrin.Size = new System.Drawing.Size(263, 757);
                lblLogPrin.Text = facinfolog;
               
               // btnHomePrin.Text = "          " + facinfolog;
            }
            pnlMenuFac.Visible = true;
            lblLogFac.Text = facinfolog;
            setupview();
            if (dgvSearch.Rows.Count > 0)
            {
                setupinfo(dgvSearch.Rows[0].Cells[0].Value.ToString());
                setupSchedOfFaculty(dgvSearch.Rows[0].Cells[1].Value.ToString());
            }
            else
            {
                clear();
            }

            if (isVisited == false)
            {
                if (VISITED.Contains("Faculty information") == false)
                {
                    VISITED += "   Faculty information";
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

            dtMenu.Rows.Add("  Activity");
            int getFIindex = 1;
            if (dt1.Rows.Count > 0)
            {
                getFIindex++;
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                getFIindex++;
                dtMenu.Rows.Add("  " + dt2.Rows[0].ItemArray[1].ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                getFIindex++;
                dtMenu.Rows.Add("  " + dt3.Rows[0].ItemArray[1].ToString());
            }
            if (dt4.Rows.Count > 0)
            {
                getFIindex++;
                dtMenu.Rows.Add("  " + dt4.Rows[0].ItemArray[1].ToString());
            }
            if (dt5.Rows.Count > 0)
            {
                getFIindex++;
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
            dgvm.Rows[getFIindex].DefaultCellStyle.BackColor = Color.LightGreen;
        }


        public void clear()
        {
            lblTheName.Text = "";
            lblTeach.Text = "";
            lblbd.Text = "";
            lblgen.Text = "";
            lbladd.Text = "";
            lblfcon.Text = "";
            lblEmail.Text = "";
            lblCivil.Text = "";
            lblGrad.Text = "";
        }

        public void setupSchedOfFaculty(string me)
        {
            con.Open();
            OdbcDataAdapter daff = new OdbcDataAdapter("Select subject as 'Subject',level as 'Grade',section as 'Section',start as 'Time start',end as 'Time end',room as 'Room',days as 'Day' from facultysched_tbl where faculty='" + me + "'", con);
            DataTable dtff = new DataTable();
            daff.Fill(dtff);
            dvFacSched = new DataView(dtff);
            con.Close();

            if (dtff.Rows.Count > 0)
            {
                pnlfound.Visible = false;
                dgvFac.DataSource = null;
                dgvFac.DataSource = dvFacSched;
                dgvFac.Columns[0].Width = 160;
                dgvFac.Columns[1].Width = 85;
                dgvFac.Columns[2].Width = 85;
                dgvFac.Columns[3].Width = 88;
                dgvFac.Columns[4].Width = 88;
                dgvFac.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFac.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFac.Columns[5].Width = 130;
                dgvFac.Columns[6].Width = 100;

            }
            else
            {
                dgvFac.DataSource = null;
                pnlfound.Visible = true;
                lblfound.Text = "no schedule found...";
            }

          

        }

        public void setupview()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select empno as 'No.',(select concat(firstname,' ',middlename,' ',lastname))as 'Faculty' from employees_tbl where position='faculty' order by lastname ASC", con);
            DataTable dts = new DataTable();
            da.Fill(dts);
            con.Close();
            dvFI = new DataView(dts);
            if (dts.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvFI;
                dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[0].Width = 0;
                dgvSearch.Columns[1].Width = 240;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }
            lblResult.Text = "number of faculty: " + dgvSearch.Rows.Count.ToString();
        }

        public void setupinfo(string key)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno='" + key + "'and position='faculty'", con);
            DataTable dti = new DataTable();
            da.Fill(dti);
            con.Close();
            if (dti.Rows.Count > 0)
            {
                lblsubteach.Text = "";
                string whole = dti.Rows[0].ItemArray[3].ToString() + ", " + dti.Rows[0].ItemArray[1].ToString() + " " + dti.Rows[0].ItemArray[2].ToString();
                string sub = dti.Rows[0].ItemArray[13].ToString();
                lblTheName.Text = whole;

                string speccode = sub;
               
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
                        lblsubteach.Text = lblsubteach.Text + dt1.Rows[0].ItemArray[1].ToString()+", ";
                    }
                }

                lblTeach.Text = dti.Rows[0].ItemArray[27].ToString();

                lblbd.Text = dti.Rows[0].ItemArray[5].ToString();
                lblgen.Text = dti.Rows[0].ItemArray[7].ToString();
                lbladd.Text = dti.Rows[0].ItemArray[4].ToString();
               

                if (dti.Rows[0].ItemArray[14].ToString() == "" || dti.Rows[0].ItemArray[15].ToString() == "")
                {//display for those faculty who dont have grade or section hold
                    lblTheadvisory.Text = "no advisory class";
                }
                else
                {
                    con.Open();
                    OdbcDataAdapter daadv = new OdbcDataAdapter("Select*from employees_tbl where grade='" + dti.Rows[0].ItemArray[14].ToString() + "'and advisory='" + dti.Rows[0].ItemArray[15].ToString() + "'", con);
                    DataTable dtlookadv = new DataTable();
                    daadv.Fill(dtlookadv);
                    con.Close();
                    if (dtlookadv.Rows.Count > 0)
                    {
                        string advisory = dtlookadv.Rows[0].ItemArray[14].ToString() + " - " + dtlookadv.Rows[0].ItemArray[15].ToString();
                        lblTheadvisory.Text = advisory;
                        lblfacadvisory.Text = advisory;
                    }
                    else//display for those faculty who has a grade/section but their adviser didnt set.
                    {
                        lblTheadvisory.Text = "no advisory class";
                        lblfacadvisory.Text = "none";
                    }
                }


                if (dti.Rows[0].ItemArray[9].ToString() == "")
                {
                    lblfcon.Text = "none";
                }
                else
                {
                    lblfcon.Text = dti.Rows[0].ItemArray[9].ToString();
                }

                if (dti.Rows[0].ItemArray[10].ToString() == "")
                {
                    lblEmail.Text = "none";
                }
                else
                {
                    lblEmail.Text = dti.Rows[0].ItemArray[10].ToString();
                }

                if (dti.Rows[0].ItemArray[8].ToString() == "")
                {
                    lblCivil.Text = "none";
                }
                else
                {
                    lblCivil.Text = dti.Rows[0].ItemArray[8].ToString();
                }

                if (dti.Rows[0].ItemArray[11].ToString() == "")
                {
                    lblGrad.Text = "none";
                }
                else
                {
                    lblGrad.Text = dti.Rows[0].ItemArray[11].ToString();
                }

               
            }
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }
            setupinfo(dgvSearch.SelectedRows[0].Cells[0].Value.ToString());
            setupSchedOfFaculty(dgvSearch.SelectedRows[0].Cells[1].Value.ToString());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvFI.RowFilter = string.Format("Faculty LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvFI;

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

        private void pnlcon_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudInfo stud = new frmStudInfo();
            this.Hide();
            stud.emptype = "faculty";
            stud.studlog = facinfolog;
            stud.TheFaculty = TheFaculty;
            stud.Show();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmEmpMain emf = new frmEmpMain();
            this.Hide();
            emf.faclog = facinfolog;
            emf.TheFacultyName = TheFaculty;
            emf.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmEmpAbout abtf = new frmEmpAbout(); 
            this.Hide();
            abtf.ablog = facinfolog;
            abtf.emptype = "faculty";
            abtf.theFaculty = TheFaculty;
            abtf.Show();
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

        private void frmFacInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin logf = new frmEmpLogin();
            this.Hide(); 
            logf.Show();
        }

        private void btnAdm_Click(object sender, EventArgs e)
        {
            frmAdmission admform = new frmAdmission();
            this.Hide();
            admform.admlog = facinfolog;
            admform.TheFaculty = TheFaculty;
            admform.Show();
        }

        private void btnGrd_Click(object sender, EventArgs e)
        {
            frmStdGrd fgrd = new frmStdGrd(); 
            this.Hide();
            fgrd.grdlog = facinfolog;
            fgrd.theFacultyName = TheFaculty;
            fgrd.Show();
        }

        private void btnActPrin_Click(object sender, EventArgs e)
        {
            frmPrincipalMain pmf = new frmPrincipalMain();
            this.Hide();
            pmf.prinlog = facinfolog;
            pmf.Show();
        }

        private void btnStudIPrin_Click(object sender, EventArgs e)
        {
            frmStudInfo sif = new frmStudInfo();
            this.Hide();
            sif.studlog = facinfolog;
            sif.emptype = "principal";
            sif.Show();
        }

        private void btnRepPrin_Click(object sender, EventArgs e)
        {
            frmReport rf = new frmReport();
            this.Hide();
            rf.replog = facinfolog;
            rf.emptype = "principal";
            rf.theFaculty = TheFaculty;
            rf.Show();
        }

        private void btnAbtPrin_Click(object sender, EventArgs e)
        {
            frmEmpAbout eaf = new frmEmpAbout();
            this.Hide();
            eaf.ablog = facinfolog;
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

        private void btnRepFac_Click(object sender, EventArgs e)
        {
            frmReport rff = new frmReport();
            this.Hide();
            rff.replog = facinfolog;
            rff.emptype = "faculty";
            rff.theFaculty = TheFaculty;
            rff.Show();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void btnSectioning_Click(object sender, EventArgs e)
        {
            frmSectioning sectioningfrm = new frmSectioning();
         
            sectioningfrm.seclog = facinfolog;
            sectioningfrm.TheFaculty = TheFaculty;
            sectioningfrm.Show();
            this.Hide();
        }

        private void btnFacAdv_Click(object sender, EventArgs e)
        {
            frmFacultyAdvisory faf = new frmFacultyAdvisory();
      
            faf.advlog = facinfolog;
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
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Faculty information")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Faculty information")
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
                    casmain.cashlog = facinfolog;
                    casmain.accesscode = accesscode;
                    casmain.CO = CO;
                    casmain.thefac = TheFaculty;
                    casmain.VISITED = VISITED;
                    casmain.viewNotifDue = viewNotifDue;
                    casmain.viewNotifDisc = viewNotifDisc;
                    casmain.notifstat = notifstat;
                    casmain.viewNotifLate = viewNotifLate;
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
                    pmf.prinlog = facinfolog;
                    pmf.VISITED = VISITED;
                    pmf.viewNotifDue = viewNotifDue;
                    pmf.viewNotifDisc = viewNotifDisc;
                    pmf.notifstat = notifstat;
                    pmf.viewNotifLate = viewNotifLate;
                    pmf.Show();
                }
                if (emptype == "Registrar")
                {
                    frmRegistrarMain regmain = new frmRegistrarMain();
                    this.Hide();
                    regmain.emptype = emptype;
                    regmain.co = CO;
                    regmain.reglog = facinfolog;
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
                    empf.faclog = facinfolog;
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
                frmadm.admlog = facinfolog;
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
                formPay.paylog = facinfolog;
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
                formStudRec.asslog = facinfolog;
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
                formstdgrd.grdlog = facinfolog;
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
                stud.CO = CO;
                stud.studlog = facinfolog;
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
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Faculty advisory")
            {
                frmFacultyAdvisory frmFacAdv = new frmFacultyAdvisory();
                this.Hide();
                frmFacAdv.emptype = emptype;
                frmFacAdv.co = CO;
                frmFacAdv.advlog = facinfolog;
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
                frmSec.seclog = facinfolog;
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
                rfac.replog = facinfolog;
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
                rsched.schedlog = facinfolog;
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
                about.ablog = facinfolog;
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
