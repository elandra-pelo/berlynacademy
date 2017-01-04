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
    public partial class frmAssessment : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string asslog,thesnum,req,annualamountJ,annualamount,thefac,emptype,co,accesscode,activeSY,activeYr;
        public string FreeLastMonthTotalE,fiftyDiscTotalE,FreeLastMonthTotalJ,fiftyDiscTotalJ,signatoryRegistrar,totalAss_SOA;
        public double monthlyamt, comp1, annualamt_fiftydiscK, LessAmt_J, LessAmt_K,LessAmt_E,anuualamt_freelastmonthK, annualamt_fiftydiscE, anuualamt_freelastmonthE, annualamt_fiftydiscJ, anuualamt_freelastmonthJ, discountedAmtOtherDisc, discountedTotalOtherDisc;
        public string today, secpay, thipay, foupay, fifpay, sixpay, sevpay, eigpay, ninpay, tenpay;
        public string FreeLastMonthTotal_K, fiftyDiscTotal_K, fiftyDisc_K, monthlyamount_K, uponamount_K, annualamount_K,TFee_K,Reg_K,Mis_K;
        public string annualamount_J, uponamount_J, monthlyamount_J, fiftyDisc_J, FreeLastMonthTotal_J, fiftyDiscTotal_J,TFee_J, Reg_J, Mis_J;
        public string annualamount_E, uponamount_E, monthlyamount_E, fiftyDisc_E, FreeLastMonthTotal_E, fiftyDiscTotal_E,TFee_E, Reg_E, Mis_E;
        public int startsched;
        public string VISITED, discountedAmtDisp, discTotalAssDisp, notifstat, TheSiblingProvider;
        public bool isVisited, viewNotifDue, viewNotifDisc,viewNotifLate;
        public int lstEnye = 1, fnmEnye = 1, mnmEnye = 1,fatenye=1,motenye=1,guaenye=1;
        public DataView dv,dvSOA;

        public frmAssessment()
        {
            InitializeComponent();
        }

        private void frmaAssessment_Load(object sender, EventArgs e)
        {
            //this.BackColor = Color.FromArgb(49, 79, 142);
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            pnlTabLine1.BackColor = Color.FromArgb(244,194,13);
            pnlTabLine2.BackColor = Color.FromArgb(244, 194, 13);
            pnlTabLine3.BackColor = Color.FromArgb(244, 194, 13);
            pnlTabLine4.BackColor = Color.FromArgb(244, 194, 13);
            pnlTabLine5.BackColor = Color.FromArgb(244, 194, 13);
            pnlTabLine6.BackColor = Color.FromArgb(244, 194, 13);

            lblLogger.Text = asslog;
            lblLoggerPosition.Text = emptype;
            cmbFilter.Text = "Student number";
            cmbFilterSOA.Text = "Student number";
            //btnHomeReg.Text = "          " + asslog;

            
            setupStudents();

            lvwReqs.Columns.Add("Requirement",215,HorizontalAlignment.Left);
            lvwReqs.Columns.Add("Date submitted", 153, HorizontalAlignment.Left);

            lvwSG.Columns.Add("Subject", 230, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 1", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 2", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 3", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 4", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Average", 140, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Remarks", 200, HorizontalAlignment.Center);

            if (isVisited == false)
            {
                if (VISITED.Contains("Student records") == false)
                {
                    VISITED += "   Student records";
                    isVisited = true;
                }
            }

            setupMENU();
            setupRequirementList();
            setupRegisteredStudForSOA();
            GetActiveSchoolYear();
        }

        public void GetActiveSchoolYear()
        {
            con.Open();
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);
            con.Close();
            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString();
            activeYr = dtssy.Rows[0].ItemArray[0].ToString();
            }
        }

        public void setupRegisteredStudForSOA()
        {
            
            con.Open();
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_tbl", con);
            DataTable dts1 = new DataTable();
            da1.Fill(dts1);
            con.Close();

            con.Open();
            OdbcDataAdapter da2 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_old_tbl", con);
            DataTable dts2 = new DataTable();
            da2.Fill(dts2);
            con.Close();
            if (dts2.Rows.Count > 0)
            {

                for (int i = 0; i < dts2.Rows.Count; i++)
                {
                    dts1.Rows.Add(dts2.Rows[i].ItemArray[0].ToString(), dts2.Rows[i].ItemArray[1].ToString());

                }
            }

            dvSOA = new DataView(dts1);
            dgvSrc2.DataSource = dvSOA;
            dgvSrc2.Columns[0].Width = 90;
            dgvSrc2.Columns[1].Width = 300;

            
        }

        public void setupRegisteredStudForSOAAutoComplete()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            AutoCompleteStringCollection Data = new AutoCompleteStringCollection();
            if (dt.Rows.Count > 0)
            {
                string[] stud = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    stud[i] = dt.Rows[i].ItemArray[0].ToString() + "     " + dt.Rows[i].ItemArray[1].ToString();
                }
                Data.AddRange(stud);
                txtSOASrc.AutoCompleteCustomSource = Data;
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

            int getSRindex = 1;
            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                getSRindex++;
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                getSRindex++;
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
            dgvm.Rows[getSRindex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        public void setupRequirementList()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from requirement_tbl where type='" + "NTR" + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                for (int f = 0; f < dt.Rows.Count; f++)
                {
                    int num = f + 1;
                    lblntrReq.Text = lblntrReq.Text+ num +". "+dt.Rows[f].ItemArray[1].ToString()+"\n\n";
                }
            }


            con.Open();
            OdbcDataAdapter dao = new OdbcDataAdapter("Select*from requirement_tbl where type='" + "OLD" + "'", con);
            DataTable dto = new DataTable();
            dao.Fill(dto);
            con.Close();

            if (dto.Rows.Count > 0)
            {
                for (int f = 0; f < dto.Rows.Count; f++)
                {
                    int num = f + 1;
                    lbloReq.Text = lbloReq.Text + num + ". " + dto.Rows[f].ItemArray[1].ToString() + "\n\n";
                }
            }
        }


        public void setupStudents()
        {
            string activeSY = "";
            con.Open();
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);
           
            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString(); }

           
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from stud_tbl where syregistered='"+activeSY+"'and status='Active'", con);
            DataTable dts = new DataTable();
            da.Fill(dts);

            con.Close();
            dv = new DataView(dts);
            dgvSearch.DataSource = dv;
            dgvSearch.Columns[0].Width = 90;
            dgvSearch.Columns[1].Width = 162;
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

        private void frmaAssessment_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnHomeReg_Click(object sender, EventArgs e)
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
            ea.ablog = asslog;
            ea.emptype = "registrar";
            ea.Show();
        }

        private void btnSI_Click(object sender, EventArgs e)
        {
            frmStudInfo SIF = new frmStudInfo();
            this.Hide();
            SIF.studlog = asslog;
            SIF.emptype = "registrar";
            SIF.Show();
        }

        private void btnAssReg_Click(object sender, EventArgs e)
        {
            frmRegistrarMain regm = new frmRegistrarMain();
            this.Hide();
            regm.reglog = asslog;
            regm.Show();
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }

            string key = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            thesnum = key;
            string levstud = "";
           
            lvwAssessment.Clear();
            lvwAssessment.Columns.Add("Fee description", 450, HorizontalAlignment.Left);
            lvwAssessment.Columns.Add("Amount", 190, HorizontalAlignment.Right);
          
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='" + key + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                //int CURRENTYR = Convert.ToInt32(DateTime.Now.Year.ToString());
                int CURRENTYR = Convert.ToInt32(activeYr);
                int UPCOMING = CURRENTYR + 1;
                string SY = CURRENTYR + "-" + UPCOMING;
                txtSY.Text = activeSY;
                txtSnum.Text = key;
                txtLast.Text = dt.Rows[0].ItemArray[3].ToString();
                txtFirst.Text = dt.Rows[0].ItemArray[1].ToString();
                txtMid.Text = dt.Rows[0].ItemArray[2].ToString();
                txtGrd.Text = dt.Rows[0].ItemArray[4].ToString();
                txtSec.Text = dt.Rows[0].ItemArray[5].ToString();
                txtMOP.Text = dt.Rows[0].ItemArray[22].ToString();
              
                levstud = txtGrd.Text;
            }


            string levdep = "";
            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtGrd.Text + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdep = dtdep.Rows[0].ItemArray[0].ToString();
            }

            setupAssessmentDiscountType(key);
            setupAssessmentPerLevel(levdep);
            
            setupAssessmentLessAmt();
            setupGrades(key);
            setupRequirementSubmitted(key);
            setupPaymentSchedule(key);
            setupPaymentHistory();
            setupPaymentSummary();
            setupClassSched();
            setupRFContentAndAdviser();
            setupSOA();
            setupRegistrars();
            setupretrieveddata(txtSnum.Text);
           
        }

        public void setupAssessmentLessAmt()
        {
            string discounttype = lblAssesDiscount.Text;
            if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
            {
                double disc = 0;
                if (txtGrd.Text == "Kinder") { disc = Convert.ToDouble(monthlyamount_K); }
                if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6") { disc = Convert.ToDouble(monthlyamount_E); }
                if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10") { disc = Convert.ToDouble(monthlyamount_J); }
               
                if (disc >= 1000)
                {
                    lblAssesLessAmt.Text = "P " + String.Format(("{0:0,###.00#}"), Convert.ToDouble(disc));
                }
                if (disc < 1000)
                {
                    lblAssesLessAmt.Text = "P " + String.Format(("{0:0.00#}"), Convert.ToDouble(disc));
                }
            }
            if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
            {
                double disc = 0;
                if (txtGrd.Text == "Kinder") { disc = Convert.ToDouble(fiftyDisc_K); }
                if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6") { disc = Convert.ToDouble(fiftyDisc_E); }
                if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10") { disc = Convert.ToDouble(fiftyDisc_J); }
                if (disc >= 1000)
                {
                    lblAssesLessAmt.Text = "P " + String.Format(("{0:0,###.00#}"), Convert.ToDouble(disc));
                }
                if (disc < 1000)
                {
                    lblAssesLessAmt.Text = "P " + String.Format(("{0:0.00#}"), Convert.ToDouble(disc));
                }
               
            }
            if ((lblAssesDiscount.Text != "None") && ((discounttype.Contains("siblings") == false && discounttype.Contains("First") == false && discounttype.Contains("1st") == false && discounttype.Contains("Second") == false && discounttype.Contains("2nd") == false)))
            {
                con.Open();
                OdbcDataAdapter daa = new OdbcDataAdapter("Select*from discount_tbl where discname='" + lblAssesDiscount.Text + "'", con);
                DataTable dtt = new DataTable();
                daa.Fill(dtt);
                con.Close();
                if (dtt.Rows.Count > 0)
                {
                    string rate = dtt.Rows[0].ItemArray[3].ToString();
                    if (rate.Substring(0, 1).ToString().Contains(".") == false)
                    {
                        rate = "." + rate;
                    }

                    double anlamt = 0;
                    double TF_amt = 0;
                    double Reg_amt = 0;
                    double Mis_amt = 0;

                    if (txtGrd.Text == "Kinder") { anlamt = Convert.ToDouble(annualamount_K); TF_amt = Convert.ToDouble(TFee_K); Mis_amt = Convert.ToDouble(Mis_K); Reg_amt = Convert.ToDouble(Reg_K); }
                    if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6") { anlamt = Convert.ToDouble(annualamount_E); TF_amt = Convert.ToDouble(TFee_E); Mis_amt = Convert.ToDouble(Mis_E); Reg_amt = Convert.ToDouble(Reg_E); }
                    if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10") { anlamt = Convert.ToDouble(annualamount_J); TF_amt = Convert.ToDouble(TFee_J); Mis_amt = Convert.ToDouble(Mis_J); Reg_amt = Convert.ToDouble(Reg_J); }
                   
                    double discrate = Convert.ToDouble(rate);
                    double discAmt_Other = TF_amt * discrate;

                    double discamtother = Convert.ToDouble(discAmt_Other);
                    if (discamtother >= 1000)
                    {
                        lblAssesLessAmt.Text = "P " + String.Format(("{0:0,###.00#}"), Convert.ToDouble(discamtother));
                    }
                    if (discamtother < 1000)
                    {
                        lblAssesLessAmt.Text = "P " + String.Format(("{0:0.00#}"), Convert.ToDouble(discamtother));
                    }
                }
            }
        }
        public void setupAssessmentDiscountType(string key)
        {
            lblAssesDiscount.Visible = true;
            lblAssesLessAmt.Visible = true;
            lblAssessMode.Visible = true;
            
            //MODE OF PAYMENT
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='" + key + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblAssessMode.Text = dt.Rows[0].ItemArray[22].ToString();
            }

            //DISCOUNT TYPE
            con.Open();
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + key + "'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            con.Close();

            if (dt1.Rows.Count > 0)
            {
                lblAssesDiscount.Text = dt1.Rows[0].ItemArray[1].ToString();

                if (dt1.Rows[0].ItemArray[2].ToString() != "")
                {
                    con.Open();
                    OdbcDataAdapter da111 = new OdbcDataAdapter("Select*from stud_tbl where studno='" + dt1.Rows[0].ItemArray[2].ToString() + "'", con);
                    DataTable dt111 = new DataTable();
                    da111.Fill(dt111);
                    con.Close();
                    if (dt111.Rows.Count > 0)
                    {
                        TheSiblingProvider = dt111.Rows[0].ItemArray[3].ToString() + ", " + dt111.Rows[0].ItemArray[1].ToString() + " " + dt111.Rows[0].ItemArray[2].ToString();
                    }
                }
            }
            else
            {
                lblAssesDiscount.Text = "None";
                lblAssesLessAmt.Text = "P 0";
            }
            
        }

        public void retrievedAssessmentKinder()
        {
            annualamount_K = "";
            uponamount_K = "";
            monthlyamount_K = "";
            fiftyDisc_K = "";
            FreeLastMonthTotal_K = "";
            fiftyDiscTotal_K = "";
            TFee_K = "";
            Reg_K = "";
            Mis_K = "";

            string levdep = "";
            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtGrd.Text + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdep = dtdep.Rows[0].ItemArray[0].ToString();
            }

            con.Open();
            OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and fee='TUITION FEE'and SY='" + activeSY + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            con.Close();
            if (dt0.Rows.Count > 0)
            {
                TFee_K = dt0.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and fee='REGISTRATION'and SY='" + activeSY + "'", con);
            DataTable dt01 = new DataTable();
            da01.Fill(dt01);
            con.Close();
            if (dt01.Rows.Count > 0)
            {
                Reg_K = dt01.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da011 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and fee='MISCELLANEOUS'and SY='" + activeSY + "'", con);
            DataTable dt011 = new DataTable();
            da011.Fill(dt011);
            con.Close();
            if (dt011.Rows.Count > 0)
            {
                Mis_K = dt011.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter dakinder = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "' and SY='" + activeSY + "'", con);
            DataTable dtkinder = new DataTable();
            dakinder.Fill(dtkinder);
            con.Close();

            if (dtkinder.Rows.Count > 0)
            {
                for (int a = 0; a < dtkinder.Rows.Count; a++)
                {
                    if (dtkinder.Rows[a].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                    {
                        annualamount_K = dtkinder.Rows[a].ItemArray[2].ToString();
                    }
                    if (dtkinder.Rows[a].ItemArray[1].ToString() == "UPON ENROLLMENT")
                    {
                        double _uponK = Convert.ToDouble(dtkinder.Rows[a].ItemArray[2].ToString());

                        if (_uponK >= 1000)
                        {
                            uponamount_K = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_uponK));
                        }
                        if (_uponK < 1000)
                        {
                            uponamount_K = String.Format(("{0:0.00#}"), Convert.ToDouble(_uponK));
                        }

                    }
                    if (dtkinder.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                    {
                        double _montK = Convert.ToDouble(dtkinder.Rows[a].ItemArray[2].ToString());

                        if (_montK >= 1000)
                        {
                            monthlyamount_K = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_montK));
                        }
                        if (_montK < 1000)
                        {
                            monthlyamount_K = String.Format(("{0:0.00#}"), Convert.ToDouble(_montK));
                        }

                        double monthlyamt = Convert.ToDouble(monthlyamount_K);
                        LessAmt_K = monthlyamt * .5;
                        double discountedAmt = monthlyamt - LessAmt_K;
                        fiftyDisc_K = discountedAmt.ToString();
                        double annualamt = Convert.ToDouble(annualamount_K);
                        double DiscountedTotalFreeLastMonth = annualamt - monthlyamt;
                        FreeLastMonthTotal_K = DiscountedTotalFreeLastMonth.ToString();
                        double DiscountedTotalFiftyDisc = annualamt - discountedAmt;
                        fiftyDiscTotal_K = DiscountedTotalFiftyDisc.ToString();
                        annualamt_fiftydiscK = annualamt - LessAmt_K;
                        anuualamt_freelastmonthK = annualamt - monthlyamt;

                    }
                }
            }
        }

        public void retrievedAssessmentJunior()
        {
            annualamount_J = "";
            uponamount_J = "";
            monthlyamount_J = "";
            fiftyDisc_J = "";
            FreeLastMonthTotal_J = "";
            fiftyDiscTotal_J = "";
            TFee_J = "";
            Reg_J = "";
            Mis_J = "";

            string levdep = "";
            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtGrd.Text + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdep = dtdep.Rows[0].ItemArray[0].ToString();
            }

            con.Open();
            OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and fee='TUITION FEE'and SY='" + activeSY + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            con.Close();
            if (dt0.Rows.Count > 0)
            {
                TFee_J = dt0.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and fee='REGISTRATION'and SY='" + activeSY + "'", con);
            DataTable dt01 = new DataTable();
            da01.Fill(dt01);
            con.Close();
            if (dt01.Rows.Count > 0)
            {
                Reg_J = dt01.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da011 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and fee='MISCELLANEOUS'and SY='" + activeSY + "'", con);
            DataTable dt011 = new DataTable();
            da011.Fill(dt011);
            con.Close();
            if (dt011.Rows.Count > 0)
            {
                Mis_J = dt011.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter dajr = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
            DataTable dtjr = new DataTable();
            dajr.Fill(dtjr);
            con.Close();


            if (dtjr.Rows.Count > 0)
            {
                for (int a = 0; a < dtjr.Rows.Count; a++)
                {
                    if (dtjr.Rows[a].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                    {
                        annualamount_J = dtjr.Rows[a].ItemArray[2].ToString();
                    }
                    if (dtjr.Rows[a].ItemArray[1].ToString() == "UPON ENROLLMENT")
                    {
                        uponamount_J = dtjr.Rows[a].ItemArray[2].ToString();
                    }
                    if (dtjr.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                    {
                        monthlyamount_J = dtjr.Rows[a].ItemArray[2].ToString();
                        double monthlyamt = Convert.ToDouble(monthlyamount_J);
                        LessAmt_J = monthlyamt * .5;
                        double discountedAmt = monthlyamt - LessAmt_J;
                        fiftyDisc_J = discountedAmt.ToString();
                        double annualamt = Convert.ToDouble(annualamount_J);
                        double DiscountedTotalFreeLastMonth = annualamt - monthlyamt;
                        FreeLastMonthTotal_J = DiscountedTotalFreeLastMonth.ToString();
                        double DiscountedTotalFiftyDisc = annualamt - discountedAmt;
                        fiftyDiscTotal_J = DiscountedTotalFiftyDisc.ToString();
                        annualamt_fiftydiscJ = annualamt - LessAmt_J;
                        anuualamt_freelastmonthJ = annualamt - monthlyamt;

                    }
                }
            }
        }

        public void retrievedAssessmentElem()
        {
            annualamount_E = "";
            uponamount_E = "";
            monthlyamount_E = "";
            fiftyDisc_E = "";
            FreeLastMonthTotal_E = "";
            fiftyDiscTotal_E = "";
            TFee_E = "";
            Reg_E = "";
            Mis_E = "";

            string levdep = "";
            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtGrd.Text + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdep = dtdep.Rows[0].ItemArray[0].ToString();
            }

            con.Open();
            OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and fee='TUITION FEE'and SY='" + activeSY + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            con.Close();
            if (dt0.Rows.Count > 0)
            {
                TFee_E = dt0.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and fee='REGISTRATION'and SY='" + activeSY + "'", con);
            DataTable dt01 = new DataTable();
            da01.Fill(dt01);
            con.Close();
            if (dt01.Rows.Count > 0)
            {
                Reg_E = dt01.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da011 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and fee='MISCELLANEOUS'and SY='" + activeSY + "'", con);
            DataTable dt011 = new DataTable();
            da011.Fill(dt011);
            con.Close();
            if (dt011.Rows.Count > 0)
            {
                Mis_E = dt011.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter daelem = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
            DataTable dtelem = new DataTable();
            daelem.Fill(dtelem);
            con.Close();

            if (dtelem.Rows.Count > 0)
            {
                for (int a = 0; a < dtelem.Rows.Count; a++)
                {
                    if (dtelem.Rows[a].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                    {
                        annualamount_E = dtelem.Rows[a].ItemArray[2].ToString();
                    }
                    if (dtelem.Rows[a].ItemArray[1].ToString() == "UPON ENROLLMENT")
                    {
                        uponamount_E = dtelem.Rows[a].ItemArray[2].ToString();
                    }
                    if (dtelem.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                    {
                        monthlyamount_E = dtelem.Rows[a].ItemArray[2].ToString();
                        double monthlyamt = Convert.ToDouble(monthlyamount_E);
                        LessAmt_E = monthlyamt * .5;
                        double discountedAmt = monthlyamt - LessAmt_E;
                        fiftyDisc_E = discountedAmt.ToString();
                        double annualamt = Convert.ToDouble(annualamount_E);
                        double DiscountedTotalFreeLastMonth = annualamt - monthlyamt;
                        FreeLastMonthTotal_E = DiscountedTotalFreeLastMonth.ToString();
                        double DiscountedTotalFiftyDisc = annualamt - discountedAmt;
                        fiftyDiscTotal_E = DiscountedTotalFiftyDisc.ToString();
                        annualamt_fiftydiscE = annualamt - LessAmt_E;
                        anuualamt_freelastmonthE = annualamt - monthlyamt;
                    }
                }
            }
        }

        public void setupAssessmentPerLevel(string levelkey)
        {
            lvwAssessment.Clear();
            lvwAssessment.Columns.Add("Fee description", 450, HorizontalAlignment.Left);
            lvwAssessment.Columns.Add("Amount", 190, HorizontalAlignment.Left);
            pnlNotAss2.Visible = false;
            string totalAss = "";
            //TOTAL ASSESSMENT DISPLAY -------------------------------------------------------------------------------------------------------------
            con.Open();
            OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and fee<>'TUITION FEE'and fee<>'REGISTRATION'and fee<>'MISCELLANEOUS'and fee='ANNUAL PAYMENT' and SY='" + activeSY + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            con.Close();
            if (dt0.Rows.Count > 0)
            {
                for (int i = 0; i < dt0.Rows.Count; i++)
                {
                    totalAss = dt0.Rows[i].ItemArray[2].ToString();

                }
            }

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and type='fee' and SY='" + activeSY + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
               
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[1].ToString().Contains("TUITION FEE") == true)
                    {
                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = dt.Rows[i].ItemArray[1].ToString();
                        itmfee1.SubItems.Add("                         P " + dt.Rows[i].ItemArray[2].ToString());
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Bold);
                    }
                }

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j].ItemArray[1].ToString().Contains("REGISTRATION") == true)
                    {
                        ListViewItem itmfee2 = new ListViewItem();
                        itmfee2.Text = dt.Rows[j].ItemArray[1].ToString();
                        itmfee2.SubItems.Add("                         P " + dt.Rows[j].ItemArray[2].ToString());
                        lvwAssessment.Items.Add(itmfee2);
                        itmfee2.Font = new Font("Arial", 11, FontStyle.Bold);

                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from registrationfee_tbl where level='" + levelkey + "'and SY='" + activeSY + "' order by fee ASC", con);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        con.Close();

                        if (dt1.Rows.Count > 0)
                        {
                            for (int h = 0; h < dt1.Rows.Count; h++)
                            {
                                ListViewItem itmfeereg = new ListViewItem();
                                itmfeereg.Text = "     " + dt1.Rows[h].ItemArray[1].ToString();
                                itmfeereg.SubItems.Add("P " + dt1.Rows[h].ItemArray[2].ToString());
                                lvwAssessment.Items.Add(itmfeereg);
                            }

                        }

                    }
                }

                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    if (dt.Rows[k].ItemArray[1].ToString().Contains("MISCELLANEOUS") == true)
                    {
                        ListViewItem itmfee2 = new ListViewItem();
                        itmfee2.Text = dt.Rows[k].ItemArray[1].ToString();
                        itmfee2.SubItems.Add("                         P " + dt.Rows[k].ItemArray[2].ToString());
                        lvwAssessment.Items.Add(itmfee2);
                        itmfee2.Font = new Font("Arial", 11, FontStyle.Bold);

                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from miscellaneousfee_tbl where level='" + levelkey + "'and SY='" + activeSY + "' order by fee ASC", con);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        con.Close();

                        if (dt1.Rows.Count > 0)
                        {
                            for (int h = 0; h < dt1.Rows.Count; h++)
                            {
                                ListViewItem itmfeemis = new ListViewItem();
                                itmfeemis.Text = "     " + dt1.Rows[h].ItemArray[1].ToString();
                                itmfeemis.SubItems.Add("P " + dt1.Rows[h].ItemArray[2].ToString());
                                lvwAssessment.Items.Add(itmfeemis);
                            }

                        }

                    }
                }

                con.Open();
                OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and fee<>'TUITION FEE'and fee<>'REGISTRATION'and fee<>'MISCELLANEOUS'and type<>'payment'and SY='" + activeSY + "'", con);
                DataTable dt01 = new DataTable();
                da01.Fill(dt01);
                con.Close();
                if (dt01.Rows.Count > 0)
                {
                    for (int i = 0; i < dt01.Rows.Count; i++)
                    {
                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = dt01.Rows[i].ItemArray[1].ToString();
                        itmfee1.SubItems.Add("                         P " + dt01.Rows[i].ItemArray[2].ToString());
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Bold);
                    }
                }


                //SUBTOTAL
                ListViewItem itmst0 = new ListViewItem();
                itmst0.Text = "";
                itmst0.SubItems.Add("                      ___________");
                lvwAssessment.Items.Add(itmst0);

                ListViewItem itmst = new ListViewItem();
                itmst.Text = "Sub-Total";
                itmst.SubItems.Add("                         P " + totalAss);
                lvwAssessment.Items.Add(itmst);
                itmst.Font = new Font("Arial", 11, FontStyle.Bold);

                //--------------------------------FOR DISCOUNT
       
                if (txtGrd.Text == "Kinder")
                {
                    retrievedAssessmentKinder();
                    if (lblAssesDiscount.Text=="None")
                    {
                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less:";
                        itmfee1.SubItems.Add("                         P " + "0.00");
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }

                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == true) || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true))
                    {
                        
                        double monthlyAmtK = Convert.ToDouble(monthlyamount_K);
                        if (monthlyAmtK >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyAmtK));
                        }
                        if (monthlyAmtK < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyAmtK));
                        }

                        double freelastmonthK = Convert.ToDouble(FreeLastMonthTotal_K);
                        if (freelastmonthK >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(freelastmonthK));
                        }
                        if (freelastmonthK < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(freelastmonthK));
                        }

                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less:";
                        itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);


                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("Second") == true) || lblAssesDiscount.Text.Contains("2nd") == true))
                    {
                        double lessAmtK = Convert.ToDouble(LessAmt_K);
                        if (lessAmtK >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(lessAmtK));
                        }
                        if (lessAmtK < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(lessAmtK));
                        }

                        double fiftydisctotalk = Convert.ToDouble(fiftyDiscTotal_K);
                        if (fiftydisctotalk >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftydisctotalk));
                        }
                        if (fiftydisctotalk < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftydisctotalk));
                        }

                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less:";
                        itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                    {
                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + lblAssesDiscount.Text + "'", con);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        con.Close();
                        if (dt1.Rows.Count > 0)
                        {
                            string rate = dt1.Rows[0].ItemArray[3].ToString();
                            if (rate.Substring(0, 1).ToString().Contains(".") == false)
                            {
                                rate = "." + rate;
                            }

                            double TF_amt = Convert.ToDouble(TFee_K);
                            double Reg_amt = Convert.ToDouble(Reg_K);
                            double Mis_amt = Convert.ToDouble(Mis_K);
                            double anlamt = Convert.ToDouble(annualamount_K);
                            double discrate = Convert.ToDouble(rate);
                            discountedAmtOtherDisc = TF_amt * discrate;
                            TF_amt -= discountedAmtOtherDisc;
                            discountedTotalOtherDisc = TF_amt+Reg_amt+Mis_amt;

                            double discamtother = Convert.ToDouble(discountedAmtOtherDisc);
                            if (discamtother >= 1000)
                            {
                                discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discamtother));
                            }
                            if (discamtother < 1000)
                            {
                                discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discamtother));
                            }


                            double disctotalOther = Convert.ToDouble(discountedTotalOtherDisc);
                            if (disctotalOther >= 1000)
                            {
                                discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(disctotalOther));
                            }
                            if (disctotalOther < 1000)
                            {
                                discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(disctotalOther));
                            }

                            ListViewItem itmfee1 = new ListViewItem();
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                    }
                }
                else if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                {
                    retrievedAssessmentJunior();
                    if (lblAssesDiscount.Text == "None")
                    {
                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less:";
                        itmfee1.SubItems.Add("                         P " + "0.00");
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }

                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == true) || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true))
                    {
                        discountedAmtDisp = "";
                        double monthlyAmtJ = Convert.ToDouble(monthlyamount_J);
                        if (monthlyAmtJ >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyAmtJ));
                        }
                        if (monthlyAmtJ < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyAmtJ));
                        }

                        double flm_tot = Convert.ToDouble(FreeLastMonthTotal_J);
                        if (flm_tot >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(flm_tot));
                        }
                        if (flm_tot < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(flm_tot));
                        }

                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less:";
                        itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("Second") == true) || lblAssesDiscount.Text.Contains("2nd") == true))
                    {
                        discountedAmtDisp = "";
                        double lessAmtJ = Convert.ToDouble(LessAmt_J);
                        if (lessAmtJ >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(lessAmtJ));
                        }
                        if (lessAmtJ < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(lessAmtJ));
                        }

                        double fiftydisctotalj = Convert.ToDouble(fiftyDiscTotal_J);
                        if (fiftydisctotalj >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftydisctotalj));
                        }
                        if (fiftydisctotalj < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftydisctotalj));
                        }

                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less:";
                        itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                    {
                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + lblAssesDiscount.Text + "'", con);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        con.Close();
                        if (dt1.Rows.Count > 0)
                        {
                            string rate = dt1.Rows[0].ItemArray[3].ToString();
                            if (rate.Substring(0, 1).ToString().Contains(".") == false)
                            {
                                rate = "." + rate;
                            }

                            double TF_amt = Convert.ToDouble(TFee_J);
                            double Reg_amt = Convert.ToDouble(Reg_J);
                            double Mis_amt = Convert.ToDouble(Mis_J);
                            double anlamt = Convert.ToDouble(annualamount_J);
                            double discrate = Convert.ToDouble(rate);
                            discountedAmtOtherDisc = TF_amt * discrate;
                            TF_amt -= discountedAmtOtherDisc;
                            discountedTotalOtherDisc = TF_amt+Reg_amt+Mis_amt;

                            double discamtother = Convert.ToDouble(discountedAmtOtherDisc);
                            if (discamtother >= 1000)
                            {
                                discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discamtother));
                            }
                            if (discamtother < 1000)
                            {
                                discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discamtother));
                            }

                            double disctotalOther = Convert.ToDouble(discountedTotalOtherDisc);
                            if (disctotalOther >= 1000)
                            {
                                discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(disctotalOther));
                            }
                            if (disctotalOther < 1000)
                            {
                                discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(disctotalOther));
                            }


                            ListViewItem itmfee1 = new ListViewItem();
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                    }
                }
                else
                {
                    retrievedAssessmentElem();
                    if (lblAssesDiscount.Text == "None")
                    {
                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less:";
                        itmfee1.SubItems.Add("                         P " + "0.00");
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }

                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == true) || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true))
                    {
                        double monthlyAmtE = Convert.ToDouble(monthlyamount_E);
                        if (monthlyAmtE >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyAmtE));
                        }
                        if (monthlyAmtE < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyAmtE));
                        }

                        double flm_tot = Convert.ToDouble(FreeLastMonthTotal_E);
                        if (flm_tot >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(flm_tot));
                        }
                        if (flm_tot < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(flm_tot));
                        }

                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less:";
                        itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("Second") == true) || lblAssesDiscount.Text.Contains("2nd") == true))
                    {
                        double lessAmtE = Convert.ToDouble(LessAmt_E);
                        if (lessAmtE >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(lessAmtE));
                        }
                        if (lessAmtE < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(lessAmtE));
                        }

                        double fiftydisctotale = Convert.ToDouble(fiftyDiscTotal_E);
                        if (fiftydisctotale >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftydisctotale));
                        }
                        if (fiftydisctotale < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftydisctotale));
                        }

                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less:";
                        itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                    {
                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + lblAssesDiscount.Text + "'", con);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        con.Close();
                        if (dt1.Rows.Count > 0)
                        {
                            string rate = dt1.Rows[0].ItemArray[3].ToString();
                            if (rate.Substring(0, 1).ToString().Contains(".") == false)
                            {
                                rate = "." + rate;
                            }

                            double TF_amt = Convert.ToDouble(TFee_E);
                            double Reg_amt = Convert.ToDouble(Reg_E);
                            double Mis_amt = Convert.ToDouble(Mis_E);
                            double anlamt = Convert.ToDouble(annualamount_E);
                            double discrate = Convert.ToDouble(rate);
                            discountedAmtOtherDisc = TF_amt * discrate;
                            TF_amt -= discountedAmtOtherDisc;
                            discountedTotalOtherDisc = TF_amt+Reg_amt+Mis_amt;

                            string DiscamountDisp = "";
                            if (discountedAmtOtherDisc >= 1000)
                            {
                                DiscamountDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                            }
                            if (discountedAmtOtherDisc < 1000)
                            {
                                DiscamountDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                            }

                            double disctotalOther = Convert.ToDouble(discountedTotalOtherDisc);
                            if (disctotalOther >= 1000)
                            {
                                discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(disctotalOther));
                            }
                            if (disctotalOther < 1000)
                            {
                                discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(disctotalOther));
                            }

                            ListViewItem itmfee1 = new ListViewItem();
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + DiscamountDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                    }
                }

                if (lblAssesDiscount.Text!="None")
                {
                    ListViewItem itmfee0 = new ListViewItem();
                    itmfee0.Text = "";
                    itmfee0.SubItems.Add("                      ___________");
                    lvwAssessment.Items.Add(itmfee0);

                    ListViewItem itmfee = new ListViewItem();
                    itmfee.Text = "Total Assessment:";
                    itmfee.SubItems.Add("                         P " + discTotalAssDisp);
                    lvwAssessment.Items.Add(itmfee);
                    itmfee.Font = new Font("Arial", 11, FontStyle.Bold);
                }
                else
                {
                    ListViewItem itmfee0 = new ListViewItem();
                    itmfee0.Text = "";
                    itmfee0.SubItems.Add("                      ___________");
                    lvwAssessment.Items.Add(itmfee0);

                    ListViewItem itmfee = new ListViewItem();
                    itmfee.Text = "Total Assessment:";
                    itmfee.SubItems.Add("                         P " + totalAss);
                    lvwAssessment.Items.Add(itmfee);
                    itmfee.Font = new Font("Arial", 11, FontStyle.Bold);
                }

            }
            else
            {
                lvwAssessment.Visible = false;
            }
        }

        public void setupDateRegistered_Cash()
        {
            con.Open();
            OdbcDataAdapter dakc = new OdbcDataAdapter("Select dateregistered from paymentcash_tbl where studno='" + txtSnum.Text + "'", con);
            DataTable dtkc = new DataTable();
            dakc.Fill(dtkc);
            con.Close();
            if (dtkc.Rows.Count > 0)
            {
                DateTime dreg = Convert.ToDateTime(dtkc.Rows[0].ItemArray[0].ToString());
                today = dreg.ToShortDateString();
                secpay = dreg.AddMonths(1).ToShortDateString();
                thipay = dreg.AddMonths(2).ToShortDateString();
                foupay = dreg.AddMonths(3).ToShortDateString();
                fifpay = dreg.AddMonths(4).ToShortDateString();
                sixpay = dreg.AddMonths(5).ToShortDateString();
                sevpay = dreg.AddMonths(6).ToShortDateString();
                eigpay = dreg.AddMonths(7).ToShortDateString();
                ninpay = dreg.AddMonths(8).ToShortDateString();
                tenpay = dreg.AddMonths(9).ToShortDateString();
            }
        }

        public void setupDateRegistered_Installment()
        {
            con.Open();
            OdbcDataAdapter dakc = new OdbcDataAdapter("Select dateregistered from paymentmonthly_tbl where studno='" + txtSnum.Text + "'", con);
            DataTable dtkc = new DataTable();
            dakc.Fill(dtkc);
            con.Close();
            if (dtkc.Rows.Count > 0)
            {
                DateTime dreg = Convert.ToDateTime(dtkc.Rows[0].ItemArray[0].ToString());
                today = dreg.ToShortDateString();
                secpay = dreg.AddMonths(1).ToShortDateString();
                thipay = dreg.AddMonths(2).ToShortDateString();
                foupay = dreg.AddMonths(3).ToShortDateString();
                fifpay = dreg.AddMonths(4).ToShortDateString();
                sixpay = dreg.AddMonths(5).ToShortDateString();
                sevpay = dreg.AddMonths(6).ToShortDateString();
                eigpay = dreg.AddMonths(7).ToShortDateString();
                ninpay = dreg.AddMonths(8).ToShortDateString();
                tenpay = dreg.AddMonths(9).ToShortDateString();
            }
        }

       
        public void setupPaymentSchedule(string key)
        {
            lvwPaySched.Clear();
            lvwPaySched.Items.Clear();
            pnlpschednot.Visible = false;

            lvwPaySched.Columns.Add("Payments", 165, HorizontalAlignment.Left);
            lvwPaySched.Columns.Add("Date due", 90, HorizontalAlignment.Center);
            lvwPaySched.Columns.Add("Amount", 115, HorizontalAlignment.Right);

            string levdep = "";

            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtGrd.Text + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdep = dtdep.Rows[0].ItemArray[0].ToString();
            }

            /*today = DateTime.Now.ToShortDateString();
            secpay = DateTime.Now.AddMonths(1).ToShortDateString();
            thipay = DateTime.Now.AddMonths(2).ToShortDateString();
            foupay = DateTime.Now.AddMonths(3).ToShortDateString();
            fifpay = DateTime.Now.AddMonths(4).ToShortDateString();
            sixpay = DateTime.Now.AddMonths(5).ToShortDateString();
            sevpay = DateTime.Now.AddMonths(6).ToShortDateString();
            eigpay = DateTime.Now.AddMonths(7).ToShortDateString();
            ninpay = DateTime.Now.AddMonths(8).ToShortDateString();
            tenpay = DateTime.Now.AddMonths(9).ToShortDateString();*/

            if (txtGrd.Text == "Kinder")
            {
                lvwPaySched.Items.Clear();
                retrievedAssessmentKinder();

                if (lblAssessMode.Text== "Cash")
                {
                    setupDateRegistered_Cash();
                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(anuualamt_freelastmonthK);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }
                            ListViewItem itmkc = new ListViewItem();
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                            itmkc.Text = "ANNUAL PAYMENT";
                            itmkc.SubItems.Add(today);
                            itmkc.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkc);

                            ListViewItem itmkctotal = new ListViewItem();
                            itmkctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkctotal.Text = "Total:";
                            itmkctotal.SubItems.Add("");
                            itmkctotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkctotal);
                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(annualamt_fiftydiscK);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }
                            ListViewItem itmkc = new ListViewItem();
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                            itmkc.Text = "ANNUAL PAYMENT";
                            itmkc.SubItems.Add(today);
                            itmkc.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkc);

                            ListViewItem itmkctotal = new ListViewItem();
                            itmkctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkctotal.Text = "Total:";
                            itmkctotal.SubItems.Add("");
                            itmkctotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkctotal);
                        }
                        else
                        {
                            double _amt = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }
                            ListViewItem itmkc = new ListViewItem();
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                            itmkc.Text = "ANNUAL PAYMENT";
                            itmkc.SubItems.Add(today);
                            itmkc.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkc);

                            ListViewItem itmkctotal = new ListViewItem();
                            itmkctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkctotal.Text = "Total:";
                            itmkctotal.SubItems.Add("");
                            itmkctotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkctotal);
                        }
                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_K);
                        string amt_dis = "";
                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(annualamount_K));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(annualamount_K));
                        }
                        ListViewItem itmkc = new ListViewItem();
                        itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                        itmkc.Text = "ANNUAL PAYMENT";
                        itmkc.SubItems.Add(today);
                        itmkc.SubItems.Add("P " + amt_dis);
                        lvwPaySched.Items.Add(itmkc);

                        ListViewItem itmkctotal = new ListViewItem();
                        itmkctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                        itmkctotal.Text = "Total:";
                        itmkctotal.SubItems.Add("");
                        itmkctotal.SubItems.Add("P " + amt_dis);
                        lvwPaySched.Items.Add(itmkctotal);
                    }
                }
                if (lblAssessMode.Text == "Installment")
                {
                    setupDateRegistered_Installment();

                    ListViewItem itmki = new ListViewItem();
                    itmki.Text = "UPON ENROLLMENT";
                    itmki.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                    itmki.SubItems.Add(today);
                    itmki.SubItems.Add("P " + uponamount_K);
                    lvwPaySched.Items.Add(itmki);


                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(FreeLastMonthTotal_K);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            schedForInstallment_SecToNinPayment_k(monthlyamount_K);
                            ListViewItem itmki8 = new ListViewItem();
                            itmki8.Text = "10TH PAYMENT(FREE)";
                            itmki8.SubItems.Add("");
                            itmki8.SubItems.Add("0");
                            lvwPaySched.Items.Add(itmki8);

                            ListViewItem itmkitotal = new ListViewItem();
                            itmkitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkitotal.Text = "Total:";
                            itmkitotal.SubItems.Add("");
                            itmkitotal.SubItems.Add("P " + amt_dis);

                            lvwPaySched.Items.Add(itmkitotal);

                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(fiftyDiscTotal_K);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }
                            schedForInstallment_SecToNinPayment_k(monthlyamount_K);
                            ListViewItem itmki8 = new ListViewItem();
                            itmki8.Text = "10TH PAYMENT(DISC.)";
                            itmki8.SubItems.Add(tenpay);
                            itmki8.SubItems.Add("P " + fiftyDisc_K);
                            lvwPaySched.Items.Add(itmki8);

                            ListViewItem itmkitotal = new ListViewItem();
                            itmkitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkitotal.Text = "Total:";
                            itmkitotal.SubItems.Add("");
                            itmkitotal.SubItems.Add("P " + amt_dis);

                            lvwPaySched.Items.Add(itmkitotal);
                        }
                        else
                        {

                            double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_tot_dis = "";
                            string amt_monthlyIns_OtherDisc = "";
                            double uponamt = Convert.ToDouble(uponamount_K);
                            double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                            double monthlyInstallmentAmt_forOtherDisc = amt_deductUpon / 9;

                            if (_amt_tot >= 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt_tot));
                            }
                            if (_amt_tot < 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt_tot));
                            }

                            //-------------
                            if (monthlyInstallmentAmt_forOtherDisc >= 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }
                            if (monthlyInstallmentAmt_forOtherDisc < 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }


                            schedForInstallment_SecToNinPayment_k(amt_monthlyIns_OtherDisc);
                            ListViewItem itmki8 = new ListViewItem();
                            itmki8.Text = "10TH PAYMENT";
                            itmki8.SubItems.Add(tenpay);
                            itmki8.SubItems.Add("P " + amt_monthlyIns_OtherDisc);
                            lvwPaySched.Items.Add(itmki8);

                            ListViewItem itmkitotal = new ListViewItem();
                            itmkitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkitotal.Text = "Total:";
                            itmkitotal.SubItems.Add("");
                            itmkitotal.SubItems.Add("P " + amt_tot_dis);

                            lvwPaySched.Items.Add(itmkitotal);
                        }
                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_K);
                        string amt_dis = "";

                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }
                        schedForInstallment_SecToNinPayment_k(monthlyamount_K);
                        ListViewItem itmki8 = new ListViewItem();
                        itmki8.Text = "10TH PAYMENT";
                        itmki8.SubItems.Add(tenpay);
                        itmki8.SubItems.Add("P " + monthlyamount_K);
                        lvwPaySched.Items.Add(itmki8);

                        ListViewItem itmkitotal = new ListViewItem();
                        itmkitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                        itmkitotal.Text = "Total:";
                        itmkitotal.SubItems.Add("");
                        itmkitotal.SubItems.Add("P " + amt_dis);
                        lvwPaySched.Items.Add(itmkitotal);
                    }


                    //PAYMENT SCHEDULE MODIFY FOR DISCOUNT
                }
            }
            if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" ||
                txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
            {//here

                con.Open();
                OdbcDataAdapter daelem = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
                DataTable dtelem = new DataTable();
                daelem.Fill(dtelem);
                con.Close();

                if (dtelem.Rows.Count > 0)
                {
                    for (int a = 0; a < dtelem.Rows.Count; a++)
                    {
                        if (dtelem.Rows[a].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                        {
                            annualamount_E = dtelem.Rows[a].ItemArray[2].ToString();

                        }
                        if (dtelem.Rows[a].ItemArray[1].ToString() == "UPON ENROLLMENT")
                        {
                            double _uponE = Convert.ToDouble(dtelem.Rows[a].ItemArray[2].ToString());

                            if (_uponE >= 1000)
                            {
                                uponamount_E = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_uponE));
                            }
                            if (_uponE < 1000)
                            {
                                uponamount_E = String.Format(("{0:0.00#}"), Convert.ToDouble(_uponE));
                            }
                        }
                        if (dtelem.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                        {
                            double _montE = Convert.ToDouble(dtelem.Rows[a].ItemArray[2].ToString());

                            if (_montE >= 1000)
                            {
                                monthlyamount_E = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_montE));
                            }
                            if (_montE < 1000)
                            {
                                monthlyamount_E = String.Format(("{0:0.00#}"), Convert.ToDouble(_montE));
                            }
                        }
                    }
                }

                if (lblAssessMode.Text == "Cash")
                {
                    setupDateRegistered_Cash();
                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(anuualamt_freelastmonthE);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            ListViewItem itmec = new ListViewItem();
                            itmec.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                            itmec.Text = "ANNUAL PAYMENT";
                            itmec.SubItems.Add(today);
                            itmec.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmec);

                            ListViewItem itmectotal = new ListViewItem();
                            itmectotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmectotal.Text = "Total:";
                            itmectotal.SubItems.Add("");
                            itmectotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmectotal);
                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(annualamt_fiftydiscE);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            ListViewItem itmec = new ListViewItem();
                            itmec.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                            itmec.Text = "ANNUAL PAYMENT";
                            itmec.SubItems.Add(today);
                            itmec.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmec);

                            ListViewItem itmectotal = new ListViewItem();
                            itmectotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmectotal.Text = "Total:";
                            itmectotal.SubItems.Add("");
                            itmectotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmectotal);
                        }
                        else
                        {
                            double _amt = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }
                            ListViewItem itmkc = new ListViewItem();
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                            itmkc.Text = "ANNUAL PAYMENT";
                            itmkc.SubItems.Add(today);
                            itmkc.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkc);

                            ListViewItem itmkctotal = new ListViewItem();
                            itmkctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkctotal.Text = "Total:";
                            itmkctotal.SubItems.Add("");
                            itmkctotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkctotal);
                        }

                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_E);
                        string amt_dis = "";
                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }

                        ListViewItem itmec = new ListViewItem();
                        itmec.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                        itmec.Text = "ANNUAL PAYMENT";
                        itmec.SubItems.Add(today);
                        itmec.SubItems.Add("P " + amt_dis);
                        lvwPaySched.Items.Add(itmec);

                        ListViewItem itmectotal = new ListViewItem();
                        itmectotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                        itmectotal.Text = "Total:";
                        itmectotal.SubItems.Add("");
                        itmectotal.SubItems.Add("P " + amt_dis);
                        lvwPaySched.Items.Add(itmectotal);
                    }
                }
                if (lblAssessMode.Text == "Installment")
                {
                    setupDateRegistered_Installment();

                    ListViewItem itmei = new ListViewItem();
                    itmei.Text = "UPON ENROLLMENT";
                    itmei.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                    itmei.SubItems.Add(today);
                    itmei.SubItems.Add("P " + uponamount_E);
                    lvwPaySched.Items.Add(itmei);


                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(FreeLastMonthTotal_E);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            schedForInstallment_SecToNinPayment_e(monthlyamount_E);
                            ListViewItem itmei8 = new ListViewItem();
                            itmei8.Text = "10TH PAYMENT(FREE)";
                            itmei8.SubItems.Add("");
                            itmei8.SubItems.Add("0");
                            lvwPaySched.Items.Add(itmei8);

                            ListViewItem itmeitotal = new ListViewItem();
                            itmeitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmeitotal.Text = "Total:";
                            itmeitotal.SubItems.Add("");
                            itmeitotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmeitotal);
                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(fiftyDiscTotal_E);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }
                            schedForInstallment_SecToNinPayment_e(monthlyamount_E);
                            ListViewItem itmei8 = new ListViewItem();
                            itmei8.Text = "10TH PAYMENT(DISC.)";
                            itmei8.SubItems.Add(tenpay);
                            itmei8.SubItems.Add("P " + fiftyDisc_E);
                            lvwPaySched.Items.Add(itmei8);

                            ListViewItem itmeitotal = new ListViewItem();
                            itmeitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmeitotal.Text = "Total:";
                            itmeitotal.SubItems.Add("");
                            itmeitotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmeitotal);
                        }
                        else
                        {
                            double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_tot_dis = "";
                            string amt_monthlyIns_OtherDisc = "";
                            double uponamt = Convert.ToDouble(uponamount_E);
                            double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                            double monthlyInstallmentAmt_forOtherDisc = amt_deductUpon / 9;

                            if (_amt_tot >= 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt_tot));
                            }
                            if (_amt_tot < 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt_tot));
                            }

                            //-------------
                            if (monthlyInstallmentAmt_forOtherDisc >= 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }
                            if (monthlyInstallmentAmt_forOtherDisc < 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }


                            schedForInstallment_SecToNinPayment_e(amt_monthlyIns_OtherDisc);
                            ListViewItem itmki8 = new ListViewItem();
                            itmki8.Text = "10TH PAYMENT";
                            itmki8.SubItems.Add(tenpay);
                            itmki8.SubItems.Add("P " + amt_monthlyIns_OtherDisc);
                            lvwPaySched.Items.Add(itmki8);

                            ListViewItem itmkitotal = new ListViewItem();
                            itmkitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkitotal.Text = "Total:";
                            itmkitotal.SubItems.Add("");
                            itmkitotal.SubItems.Add("P " + amt_tot_dis);

                            lvwPaySched.Items.Add(itmkitotal);
                        }
                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_E);
                        string amt_dis = "";

                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }

                        schedForInstallment_SecToNinPayment_e(monthlyamount_E);
                        ListViewItem itmei8 = new ListViewItem();
                        itmei8.Text = "10TH PAYMENT";
                        itmei8.SubItems.Add(tenpay);
                        itmei8.SubItems.Add("P " + monthlyamount_E);
                        lvwPaySched.Items.Add(itmei8);

                        ListViewItem itmeitotal = new ListViewItem();
                        itmeitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                        itmeitotal.Text = "Total:";
                        itmeitotal.SubItems.Add("");
                        itmeitotal.SubItems.Add("P " + amt_dis);
                        lvwPaySched.Items.Add(itmeitotal);
                    }
                }
            }
            if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
            {
                con.Open();
                OdbcDataAdapter dajunior = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
                DataTable dtjunior = new DataTable();
                dajunior.Fill(dtjunior);
                con.Close();

                if (dtjunior.Rows.Count > 0)
                {
                    for (int a = 0; a < dtjunior.Rows.Count; a++)
                    {
                        if (dtjunior.Rows[a].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                        {
                            annualamount_J = dtjunior.Rows[a].ItemArray[2].ToString();
                        }
                        if (dtjunior.Rows[a].ItemArray[1].ToString() == "UPON ENROLLMENT")
                        {
                            double _uponJ = Convert.ToDouble(dtjunior.Rows[a].ItemArray[2].ToString());

                            if (_uponJ >= 1000)
                            {
                                uponamount_J = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_uponJ));
                            }
                            if (_uponJ < 1000)
                            {
                                uponamount_J = String.Format(("{0:0.00#}"), Convert.ToDouble(_uponJ));
                            }
                        }
                        if (dtjunior.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                        {
                            double _montJ = Convert.ToDouble(dtjunior.Rows[a].ItemArray[2].ToString());

                            if (_montJ >= 1000)
                            {
                                monthlyamount_J = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_montJ));
                            }
                            if (_montJ < 1000)
                            {
                                monthlyamount_J = String.Format(("{0:0.00#}"), Convert.ToDouble(_montJ));
                            }

                        }
                    }
                }


                if (lblAssessMode.Text == "Cash")
                {
                    setupDateRegistered_Cash();
                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {

                            double _amt = Convert.ToDouble(anuualamt_freelastmonthJ);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            ListViewItem itmjhc = new ListViewItem();
                            itmjhc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                            itmjhc.Text = "ANNUAL PAYMENT";
                            itmjhc.SubItems.Add(today);
                            itmjhc.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmjhc);

                            ListViewItem itmjhctotal = new ListViewItem();
                            itmjhctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmjhctotal.Text = "Total:";
                            itmjhctotal.SubItems.Add("");
                            itmjhctotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmjhctotal);
                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(annualamt_fiftydiscJ);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            ListViewItem itmjhc = new ListViewItem();
                            itmjhc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                            itmjhc.Text = "ANNUAL PAYMENT";
                            itmjhc.SubItems.Add(today);
                            itmjhc.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmjhc);

                            ListViewItem itmjhctotal = new ListViewItem();
                            itmjhctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmjhctotal.Text = "Total:";
                            itmjhctotal.SubItems.Add("");
                            itmjhctotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmjhctotal);
                        }
                        else
                        {
                            double _amt = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }
                            ListViewItem itmkc = new ListViewItem();
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                            itmkc.Text = "ANNUAL PAYMENT";
                            itmkc.SubItems.Add(today);
                            itmkc.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkc);

                            ListViewItem itmkctotal = new ListViewItem();
                            itmkctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkctotal.Text = "Total:";
                            itmkctotal.SubItems.Add("");
                            itmkctotal.SubItems.Add("P " + amt_dis);
                            lvwPaySched.Items.Add(itmkctotal);
                        }

                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_J);
                        string amt_dis = "";
                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }

                        ListViewItem itmjhc = new ListViewItem();
                        itmjhc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                        itmjhc.Text = "ANNUAL PAYMENT";
                        itmjhc.SubItems.Add(today);
                        itmjhc.SubItems.Add("P " + amt_dis);
                        lvwPaySched.Items.Add(itmjhc);

                        ListViewItem itmjhctotal = new ListViewItem();
                        itmjhctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                        itmjhctotal.Text = "Total:";
                        itmjhctotal.SubItems.Add("");
                        itmjhctotal.SubItems.Add("P " + amt_dis);
                        lvwPaySched.Items.Add(itmjhctotal);
                    }
                }
                if (lblAssessMode.Text== "Installment")
                {
                    setupDateRegistered_Installment();

                    ListViewItem itmjhi = new ListViewItem();
                    itmjhi.Text = "UPON ENROLLMENT";
                    itmjhi.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                    itmjhi.SubItems.Add(today);
                    itmjhi.SubItems.Add("P " + uponamount_J);
                    lvwPaySched.Items.Add(itmjhi);


                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(FreeLastMonthTotal_J);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            schedForInstallment_SecToNinPayment_j(monthlyamount_J);

                            ListViewItem itmki8 = new ListViewItem();
                            itmki8.Text = "10TH PAYMENT(FREE)";
                            itmki8.SubItems.Add("");
                            itmki8.SubItems.Add("0");
                            lvwPaySched.Items.Add(itmki8);

                            ListViewItem itmkitotal = new ListViewItem();
                            itmkitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkitotal.Text = "Total:";
                            itmkitotal.SubItems.Add("");
                            itmkitotal.SubItems.Add("P " + amt_dis);

                            lvwPaySched.Items.Add(itmkitotal);
                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(fiftyDiscTotal_J);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }
                            schedForInstallment_SecToNinPayment_j(monthlyamount_J);

                            ListViewItem itmki8 = new ListViewItem();
                            itmki8.Text = "10TH PAYMENT(DISC.)";
                            itmki8.SubItems.Add(tenpay);
                            itmki8.SubItems.Add("P " + fiftyDisc_J);
                            lvwPaySched.Items.Add(itmki8);

                            ListViewItem itmkitotal = new ListViewItem();
                            itmkitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkitotal.Text = "Total:";
                            itmkitotal.SubItems.Add("");
                            itmkitotal.SubItems.Add("P " + amt_dis);

                            lvwPaySched.Items.Add(itmkitotal);
                        }
                        else
                        {
                            double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_tot_dis = "";
                            string amt_monthlyIns_OtherDisc = "";
                            double uponamt = Convert.ToDouble(uponamount_J);
                            double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                            double monthlyInstallmentAmt_forOtherDisc = amt_deductUpon / 9;

                            if (_amt_tot >= 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt_tot));
                            }
                            if (_amt_tot < 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt_tot));
                            }

                            //-------------
                            if (monthlyInstallmentAmt_forOtherDisc >= 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }
                            if (monthlyInstallmentAmt_forOtherDisc < 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }


                            schedForInstallment_SecToNinPayment_j(amt_monthlyIns_OtherDisc);
                            ListViewItem itmki8 = new ListViewItem();
                            itmki8.Text = "10TH PAYMENT";
                            itmki8.SubItems.Add(tenpay);
                            itmki8.SubItems.Add("P " + amt_monthlyIns_OtherDisc);
                            lvwPaySched.Items.Add(itmki8);

                            ListViewItem itmkitotal = new ListViewItem();
                            itmkitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmkitotal.Text = "Total:";
                            itmkitotal.SubItems.Add("");
                            itmkitotal.SubItems.Add("P " + amt_tot_dis);

                            lvwPaySched.Items.Add(itmkitotal);
                        }
                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_J);
                        string amt_dis = "";

                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }

                        schedForInstallment_SecToNinPayment_j(monthlyamount_J);

                        ListViewItem itmjhi8 = new ListViewItem();
                        itmjhi8.Text = "10TH PAYMENT";
                        itmjhi8.SubItems.Add(tenpay);
                        itmjhi8.SubItems.Add("P " + monthlyamount_J);
                        lvwPaySched.Items.Add(itmjhi8);

                        ListViewItem itmjhitotal = new ListViewItem();
                        itmjhitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                        itmjhitotal.Text = "Total:";
                        itmjhitotal.SubItems.Add("");
                        itmjhitotal.SubItems.Add("P " + amt_dis);
                        lvwPaySched.Items.Add(itmjhitotal);

                    }
                }
            }
        }

        public void schedForInstallment_SecToNinPayment_e(string monthlyamount_E)
        {
            ListViewItem itmei0 = new ListViewItem();
            itmei0.Text = "2ND PAYMENT";
            itmei0.SubItems.Add(secpay);
            itmei0.SubItems.Add("P " + monthlyamount_E);
            lvwPaySched.Items.Add(itmei0);

            ListViewItem itmei1 = new ListViewItem();
            itmei1.Text = "3RD PAYMENT";
            itmei1.SubItems.Add(thipay);
            itmei1.SubItems.Add("P " + monthlyamount_E);
            lvwPaySched.Items.Add(itmei1);

            ListViewItem itmei2 = new ListViewItem();
            itmei2.Text = "4TH PAYMENT";
            itmei2.SubItems.Add(foupay);
            itmei2.SubItems.Add("P " + monthlyamount_E);
            lvwPaySched.Items.Add(itmei2);

            ListViewItem itmei3 = new ListViewItem();
            itmei3.Text = "5TH PAYMENT";
            itmei3.SubItems.Add(fifpay);
            itmei3.SubItems.Add("P " + monthlyamount_E);
            lvwPaySched.Items.Add(itmei3);

            ListViewItem itmei4 = new ListViewItem();
            itmei4.Text = "6TH PAYMENT";
            itmei4.SubItems.Add(sixpay);
            itmei4.SubItems.Add("P " + monthlyamount_E);
            lvwPaySched.Items.Add(itmei4);

            ListViewItem itmei5 = new ListViewItem();
            itmei5.Text = "7TH PAYMENT";
            itmei5.SubItems.Add(sevpay);
            itmei5.SubItems.Add("P " + monthlyamount_E);
            lvwPaySched.Items.Add(itmei5);

            ListViewItem itmei6 = new ListViewItem();
            itmei6.Text = "8TH PAYMENT";
            itmei6.SubItems.Add(eigpay);
            itmei6.SubItems.Add("P " + monthlyamount_E);
            lvwPaySched.Items.Add(itmei6);

            ListViewItem itmei7 = new ListViewItem();
            itmei7.Text = "9TH PAYMENT";
            itmei7.SubItems.Add(ninpay);
            itmei7.SubItems.Add("P " + monthlyamount_E);
            lvwPaySched.Items.Add(itmei7);
        }

        public void schedForInstallment_SecToNinPayment_k(string monthlyamount_K)
        {
            ListViewItem itmki0 = new ListViewItem();
            itmki0.Text = "2ND PAYMENT";
            itmki0.SubItems.Add(secpay);
            itmki0.SubItems.Add("P " + monthlyamount_K);
            lvwPaySched.Items.Add(itmki0);

            ListViewItem itmki1 = new ListViewItem();
            itmki1.Text = "3RD PAYMENT";
            itmki1.SubItems.Add(thipay);
            itmki1.SubItems.Add("P " + monthlyamount_K);
            lvwPaySched.Items.Add(itmki1);

            ListViewItem itmki2 = new ListViewItem();
            itmki2.Text = "4TH PAYMENT";
            itmki2.SubItems.Add(foupay);
            itmki2.SubItems.Add("P " + monthlyamount_K);
            lvwPaySched.Items.Add(itmki2);

            ListViewItem itmki3 = new ListViewItem();
            itmki3.Text = "5TH PAYMENT";
            itmki3.SubItems.Add(fifpay);
            itmki3.SubItems.Add("P " + monthlyamount_K);
            lvwPaySched.Items.Add(itmki3);

            ListViewItem itmki4 = new ListViewItem();
            itmki4.Text = "6TH PAYMENT";
            itmki4.SubItems.Add(sixpay);
            itmki4.SubItems.Add("P " + monthlyamount_K);
            lvwPaySched.Items.Add(itmki4);

            ListViewItem itmki5 = new ListViewItem();
            itmki5.Text = "7TH PAYMENT";
            itmki5.SubItems.Add(sevpay);
            itmki5.SubItems.Add("P " + monthlyamount_K);
            lvwPaySched.Items.Add(itmki5);

            ListViewItem itmki6 = new ListViewItem();
            itmki6.Text = "8TH PAYMENT";
            itmki6.SubItems.Add(eigpay);
            itmki6.SubItems.Add("P " + monthlyamount_K);
            lvwPaySched.Items.Add(itmki6);

            ListViewItem itmki7 = new ListViewItem();
            itmki7.Text = "9TH PAYMENT";
            itmki7.SubItems.Add(ninpay);
            itmki7.SubItems.Add("P " + monthlyamount_K);
            lvwPaySched.Items.Add(itmki7);
        }

       
        public void schedForInstallment_SecToNinPayment_j(string monthlyamount_J)
        {
            ListViewItem itmjhi0 = new ListViewItem();
            itmjhi0.Text = "2ND PAYMENT";
            itmjhi0.SubItems.Add(secpay);
            itmjhi0.SubItems.Add("P " + monthlyamount_J);
            lvwPaySched.Items.Add(itmjhi0);

            ListViewItem itmjhi1 = new ListViewItem();
            itmjhi1.Text = "3RD PAYMENT";
            itmjhi1.SubItems.Add(thipay);
            itmjhi1.SubItems.Add("P " + monthlyamount_J);
            lvwPaySched.Items.Add(itmjhi1);

            ListViewItem itmjhi2 = new ListViewItem();
            itmjhi2.Text = "4TH PAYMENT";
            itmjhi2.SubItems.Add(foupay);
            itmjhi2.SubItems.Add("P " + monthlyamount_J);
            lvwPaySched.Items.Add(itmjhi2);

            ListViewItem itmjhi3 = new ListViewItem();
            itmjhi3.Text = "5TH PAYMENT";
            itmjhi3.SubItems.Add(fifpay);
            itmjhi3.SubItems.Add("P " + monthlyamount_J);
            lvwPaySched.Items.Add(itmjhi3);

            ListViewItem itmjhi4 = new ListViewItem();
            itmjhi4.Text = "6TH PAYMENT";
            itmjhi4.SubItems.Add(sixpay);
            itmjhi4.SubItems.Add("P " + monthlyamount_J);
            lvwPaySched.Items.Add(itmjhi4);

            ListViewItem itmjhi5 = new ListViewItem();
            itmjhi5.Text = "7TH PAYMENT";
            itmjhi5.SubItems.Add(sevpay);
            itmjhi5.SubItems.Add("P " + monthlyamount_J);
            lvwPaySched.Items.Add(itmjhi5);

            ListViewItem itmjhi6 = new ListViewItem();
            itmjhi6.Text = "8TH PAYMENT";
            itmjhi6.SubItems.Add(eigpay);
            itmjhi6.SubItems.Add("P " + monthlyamount_J);
            lvwPaySched.Items.Add(itmjhi6);

            ListViewItem itmjhi7 = new ListViewItem();
            itmjhi7.Text = "9TH PAYMENT";
            itmjhi7.SubItems.Add(ninpay);
            itmjhi7.SubItems.Add("P " + monthlyamount_J);
            lvwPaySched.Items.Add(itmjhi7);
        }

        public void setupRequirementSubmitted(string snum)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from requirementpassed_tbl where studno='" + snum + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if(dt.Rows.Count>0)
            {
                lvwReqs.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem itm = new ListViewItem();
                    itm.Text = dt.Rows[i].ItemArray[2].ToString();
                    itm.SubItems.Add(dt.Rows[i].ItemArray[3].ToString());
                    lvwReqs.Items.Add(itm);
                }
            }
        }

        private void lvwReqs_Click(object sender, EventArgs e)
        {
            
            if (lvwReqs.Items.Count <= 0)
            {
                return;
            }

            if (lvwReqs.SelectedItems[0].SubItems[1].Text == "")
            {
                btnUpdate.Enabled = true;
                req = lvwReqs.SelectedItems[0].SubItems[0].Text;
            }
            else
            {
                btnUpdate.Enabled = false;
            }

           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string reqid = "";
            string date = DateTime.Now.ToShortDateString();

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from requirementpassed_tbl where studno='" + thesnum + "'and reqdesc='" + req + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                reqid = dt.Rows[0].ItemArray[4].ToString();

                con.Open();
                string update = "Update requirementpassed_tbl set datesubmitted='" + date + "'where id='" + reqid + "'";
                OdbcCommand cmdu = new OdbcCommand(update, con);
                cmdu.ExecuteNonQuery();
                con.Close();

                setupRequirementSubmitted(thesnum);
                btnUpdate.Enabled = false;
                MessageBox.Show("requirement successfully submitted.", "Student records", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

       public void setupPaymentHistory()
        {
            lvwPH.Clear();

            string levdep = "";
            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtGrd.Text+ "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdep = dtdep.Rows[0].ItemArray[0].ToString();
            }

            if (txtMOP.Text == "Cash")
            {
                lvwPH.Items.Clear();
                string datetoday = DateTime.Now.ToShortDateString();

                con.Open();
                OdbcDataAdapter daps = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + txtSnum.Text + "'and datepd!=''", con);
                DataTable dtps = new DataTable();
                daps.Fill(dtps);
                con.Close();

                if (dtps.Rows.Count > 0)
                {
                    string kinderannual = "";
                    string elemannual = "";
                    string jrannual="";

                    pnlNotPH.Visible = false;
                    lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                    lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                    lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                    lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                    lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                    double amt = Convert.ToDouble(dtps.Rows[0].ItemArray[2].ToString());
                    string displayAmt = "";
                    if (amt >= 1000)
                    {
                        displayAmt = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amt));
                    } if (amt < 1000)
                    {
                        displayAmt = String.Format(("{0:0.00#}"), Convert.ToDouble(amt));
                    }
                    //other disc.
                    double amtOD = Convert.ToDouble(discountedTotalOtherDisc);
                    string displayAmtOD = "";
                    if (amtOD >= 1000)
                    {
                        displayAmtOD = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtOD));
                    } if (amtOD < 1000)
                    {
                        displayAmtOD = String.Format(("{0:0.00#}"), Convert.ToDouble(amtOD));
                    }

                    
                    //----------------------------------------------------------------------------
                    if (txtGrd.Text == "Kinder")
                    {
                        //free last month
                        double amtFLK = Convert.ToDouble(anuualamt_freelastmonthK);
                        string displayAmtFLK = "";
                        if (amtFLK >= 1000)
                        {
                            displayAmtFLK = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtFLK));
                        } if (amtFLK < 1000)
                        {
                            displayAmtFLK = String.Format(("{0:0.00#}"), Convert.ToDouble(amtFLK));
                        }
                        //fifty disc.
                        double amtFPK = Convert.ToDouble(annualamt_fiftydiscK);
                        string displayAmtFPK = "";
                        if (amtFPK >= 1000)
                        {
                            displayAmtFPK = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtFPK));
                        } if (amtFPK < 1000)
                        {
                            displayAmtFPK = String.Format(("{0:0.00#}"), Convert.ToDouble(amtFPK));
                        }
                       
                        con.Open();
                        OdbcDataAdapter dak = new OdbcDataAdapter("Select amount from fee_tbl where fee='" + "ANNUAL PAYMENT" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk = new DataTable();
                        dak.Fill(dtk);
                        con.Close();

                        if (dtk.Rows.Count > 0)
                        {
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            con.Open();
                            OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='"+txtSnum.Text+"'", con);
                            DataTable dtk1 = new DataTable();
                            dak1.Fill(dtk1);
                            con.Close();
                            if (dtk1.Rows.Count > 0)
                            {
                                string discounttype = dtk1.Rows[0].ItemArray[1].ToString();
                                if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                                {
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + displayAmt);
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                    //itmpd.SubItems.Add(String.Format(("{0:#,###,###.##}"), Convert.ToDouble(anuualamt_freelastmonthK)));
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P "+displayAmtFLK);
                                    lvwPH.Items.Add(itps0);
                                }
                                else if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                                {
                                    kinderannual = dtk.Rows[0].ItemArray[0].ToString();
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + displayAmt);
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                    //itmpd.SubItems.Add(String.Format(("{0:#,###,###.##}"), Convert.ToDouble(annualamt_fiftydiscK)));
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + displayAmtFPK);
                                    lvwPH.Items.Add(itps0);
                                }
                                else
                                {
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + displayAmt);
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                    //itmpd.SubItems.Add(String.Format(("{0:#,###,###.##}"), Convert.ToDouble(annualamt_fiftydiscK)));
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + displayAmtOD);
                                    lvwPH.Items.Add(itps0);
                                }

                            }
                            else
                            {
                                kinderannual = dtk.Rows[0].ItemArray[0].ToString();
                                double amtK = Convert.ToDouble(kinderannual);
                                string displayAmtK = "";
                                if (amtK >= 1000)
                                {
                                    displayAmtK = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtK));
                                } if (amtK < 1000)
                                {
                                    displayAmtK = String.Format(("{0:0.00#}"), Convert.ToDouble(amtK));
                                }

                                ListViewItem itmpd = new ListViewItem();
                                itmpd.Text = "ANNUAL PAYMENT";
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                itmpd.SubItems.Add("P " + displayAmtK);
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                lvwPH.Items.Add(itmpd);

                                ListViewItem itps0 = new ListViewItem();
                                itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                itps0.Text = "Total:";
                                itps0.SubItems.Add("");
                                itps0.SubItems.Add("P " + displayAmtK);
                                lvwPH.Items.Add(itps0);
                            }
                        }
                    }
                    else if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                    {
                        //free last month
                        double amtFLJ = Convert.ToDouble(anuualamt_freelastmonthJ);
                        string displayAmtFLJ = "";
                        if (amtFLJ >= 1000)
                        {
                            displayAmtFLJ = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtFLJ));
                        } if (amtFLJ < 1000)
                        {
                            displayAmtFLJ = String.Format(("{0:0.00#}"), Convert.ToDouble(amtFLJ));
                        }
                        //fifty disc.
                        double amtFPJ = Convert.ToDouble(annualamt_fiftydiscJ);
                        string displayAmtFPJ = "";
                        if (amtFPJ >= 1000)
                        {
                            displayAmtFPJ = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtFPJ));
                        } if (amtFPJ < 1000)
                        {
                            displayAmtFPJ = String.Format(("{0:0.00#}"), Convert.ToDouble(amtFPJ));
                        }
                       
                         con.Open();
                         OdbcDataAdapter daj = new OdbcDataAdapter("Select amount from fee_tbl where fee='" + "ANNUAL PAYMENT" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtj = new DataTable();
                        daj.Fill(dtj);
                        con.Close();

                        if (dtj.Rows.Count > 0)
                        {
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            con.Open();
                            OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='"+txtSnum.Text+"'", con);
                            DataTable dtk1 = new DataTable();
                            dak1.Fill(dtk1);
                            con.Close();
                            if (dtk1.Rows.Count > 0)
                            {
                                string discounttype = dtk1.Rows[0].ItemArray[1].ToString();
                                if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                                {
                                    jrannual = dtj.Rows[0].ItemArray[0].ToString();
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + displayAmt);
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                    //itmpd.SubItems.Add(String.Format(("{0:#,###,###.##}"), Convert.ToDouble(anuualamt_freelastmonthJ)));
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + displayAmtFLJ);
                                    lvwPH.Items.Add(itps0);

                                }
                                else if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                                {
                                    jrannual = dtj.Rows[0].ItemArray[0].ToString();
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + displayAmt);
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                    //itmpd.SubItems.Add(String.Format(("{0:#,###,###.##}"), Convert.ToDouble(annualamt_fiftydiscJ)));
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + displayAmtFPJ);
                                    lvwPH.Items.Add(itps0);

                                }
                                else
                                {
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + displayAmt);
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                    //itmpd.SubItems.Add(String.Format(("{0:#,###,###.##}"), Convert.ToDouble(annualamt_fiftydiscJ)));
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + displayAmtOD);
                                    lvwPH.Items.Add(itps0);

                                }

                            }
                            else
                            {
                                jrannual = dtj.Rows[0].ItemArray[0].ToString();
                                double amtJ = Convert.ToDouble(jrannual);
                                string displayAmtJ = "";
                                if (amtJ >= 1000)
                                {
                                    displayAmtJ = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtJ));
                                } if (amtJ < 1000)
                                {
                                    displayAmtJ = String.Format(("{0:0.00#}"), Convert.ToDouble(amtJ));
                                }
                                ListViewItem itmpd = new ListViewItem();
                                itmpd.Text = "ANNUAL PAYMENT";
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                itmpd.SubItems.Add("P " + displayAmtJ);
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                lvwPH.Items.Add(itmpd);

                                ListViewItem itps0 = new ListViewItem();
                                itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                itps0.Text = "Total:";
                                itps0.SubItems.Add("");
                                itps0.SubItems.Add("P " + displayAmtJ);
                                lvwPH.Items.Add(itps0);
                            }
                        }
                    
                    }
                    else
                    {
                        //free last month
                        double amtFLE = Convert.ToDouble(anuualamt_freelastmonthE);
                        string displayAmtFLE = "";
                        if (amtFLE >= 1000)
                        {
                            displayAmtFLE = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtFLE));
                        } if (amtFLE < 1000)
                        {
                            displayAmtFLE = String.Format(("{0:0.00#}"), Convert.ToDouble(amtFLE));
                        }
                        //fifty disc.
                        double amtFPE = Convert.ToDouble(annualamt_fiftydiscE);
                        string displayAmtFPE = "";
                        if (amtFPE >= 1000)
                        {
                            displayAmtFPE = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtFPE));
                        } if (amtFPE < 1000)
                        {
                            displayAmtFPE = String.Format(("{0:0.00#}"), Convert.ToDouble(amtFPE));
                        }

                        con.Open();
                        OdbcDataAdapter dae = new OdbcDataAdapter("Select amount from fee_tbl where fee='" + "ANNUAL PAYMENT" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dte = new DataTable();
                        dae.Fill(dte);
                        con.Close();

                        if (dte.Rows.Count > 0)
                        {
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            con.Open();
                            OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='"+txtSnum.Text+"'", con);
                            DataTable dtk1 = new DataTable();
                            dak1.Fill(dtk1);
                            con.Close();
                            if (dtk1.Rows.Count > 0)
                            {
                                string discounttype = dtk1.Rows[0].ItemArray[1].ToString();
                                if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                                {
                                    elemannual = dte.Rows[0].ItemArray[0].ToString();
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + displayAmt);
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                    //itmpd.SubItems.Add(String.Format(("{0:#,###,###.##}"), Convert.ToDouble(anuualamt_freelastmonthE)));
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + displayAmtFLE);
                                    lvwPH.Items.Add(itps0);

                                }
                                else if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                                {
                                    elemannual = dte.Rows[0].ItemArray[0].ToString();
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + displayAmt);
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                    //itmpd.SubItems.Add(String.Format(("{0:#,###,###.##}"), Convert.ToDouble(annualamt_fiftydiscE)));
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + displayAmtFPE);
                                    lvwPH.Items.Add(itps0);

                                }
                                else
                                {
                                  
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + displayAmt);
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                    //itmpd.SubItems.Add(String.Format(("{0:#,###,###.##}"), Convert.ToDouble(annualamt_fiftydiscE)));
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + displayAmtOD);
                                    lvwPH.Items.Add(itps0);

                                }

                            }
                            else
                            {
                                elemannual = dte.Rows[0].ItemArray[0].ToString();
                                double amtE = Convert.ToDouble(elemannual);
                                string displayAmtE = "";
                                if (amtE >= 1000)
                                {
                                    displayAmtE = String.Format(("{0:0,###.00#}"), Convert.ToDouble(amtE));
                                } if (amtE < 1000)
                                {
                                    displayAmtE = String.Format(("{0:0.00#}"), Convert.ToDouble(amtE));
                                }

                                ListViewItem itmpd = new ListViewItem();
                                itmpd.Text = "ANNUAL PAYMENT";
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                itmpd.SubItems.Add("P " + displayAmtE);
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[5].ToString());
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[6].ToString());
                                lvwPH.Items.Add(itmpd);

                                ListViewItem itps0 = new ListViewItem();
                                itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                itps0.Text = "Total:";
                                itps0.SubItems.Add("");
                                itps0.SubItems.Add("P " + displayAmtE);
                                lvwPH.Items.Add(itps0);
                            }
                        }
                    }
                }
                else
                {
                    pnlNotPH.Visible = true;
                }
            }
            if (txtMOP.Text == "Installment")//INSTALLMENT HISTORY
            {
                lvwPH.Items.Clear();
                string datetoday = DateTime.Now.ToShortDateString();

                con.Open();
                OdbcDataAdapter dapski = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + txtSnum.Text + "'", con);
                DataTable dtpski = new DataTable();
                dapski.Fill(dtpski);
                con.Close();

                if (dtpski.Rows.Count > 0)
                {
                    double CurrBal = Convert.ToDouble(dtpski.Rows[0].ItemArray[4].ToString());
                    string annualAmt = dtpski.Rows[0].ItemArray[3].ToString();
                    string dateupon = dtpski.Rows[0].ItemArray[5].ToString();
                    string dpay2 = dtpski.Rows[0].ItemArray[6].ToString();
                    string dpay3 = dtpski.Rows[0].ItemArray[7].ToString();
                    string dpay4 = dtpski.Rows[0].ItemArray[8].ToString();
                    string dpay5 = dtpski.Rows[0].ItemArray[9].ToString();
                    string dpay6 = dtpski.Rows[0].ItemArray[10].ToString();
                    string dpay7 = dtpski.Rows[0].ItemArray[11].ToString();
                    string dpay8 = dtpski.Rows[0].ItemArray[12].ToString();
                    string dpay9 = dtpski.Rows[0].ItemArray[13].ToString();
                    string dpay10 = dtpski.Rows[0].ItemArray[14].ToString();

                    double TheAnnual = Convert.ToDouble(annualAmt);
                    if (TheAnnual >= 1000)
                    {
                        annualAmt = String.Format(("{0:0,###.00#}"), TheAnnual);
                    } if (TheAnnual < 1000)
                    {
                        annualAmt = String.Format(("{0:0.00#}"), TheAnnual);
                    }
                   

                    if (txtGrd.Text == "Kinder")
                    {
                        if (dateupon =="" && dpay2=="" && dpay3=="" && dpay4=="" && dpay5=="" && dpay6=="" && dpay7=="" && dpay8=="" && dpay9=="" && dpay10=="")
                        {
                            lvwPH.Clear();
                            lvwPH.Items.Clear();
                            pnlNotPH.Visible = true;
                        }
                        if (dateupon != "" && dpay2 == "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P "+dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[32].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[42].ToString());
                            lvwPH.Items.Add(itmdp8);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[32].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[42].ToString());
                            lvwPH.Items.Add(itmdp8);

                            ListViewItem itmdp9 = new ListViewItem();
                            itmdp9.Text = "NINETH PAYMENT";
                            itmdp9.SubItems.Add(dpay9);
                            itmdp9.SubItems.Add("P " + dtpski.Rows[0].ItemArray[23].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[33].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[43].ToString());
                            lvwPH.Items.Add(itmdp9);

                            con.Open();
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                                if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add("");
                                    itmdp10.SubItems.Add("P " + "0.00");
                                    lvwPH.Items.Add(itmdp10);

                                    double theAssessment = Convert.ToDouble(FreeLastMonthTotal_K);
                                    if (theAssessment >= 1000)
                                    {FreeLastMonthTotal_K = String.Format(("{0:0,###.00#}"), theAssessment);} else
                                    { FreeLastMonthTotal_K = String.Format(("{0:0.00#}"), theAssessment);}

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + FreeLastMonthTotal_K);
                                    lvwPH.Items.Add(itmdsum);
                                }

                            }
                            else
                            {
                                if (CurrBal <= 0)
                                {

                                    ListViewItem itmdpsumm = new ListViewItem();
                                    itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdpsumm.Text = "Total:";
                                    itmdpsumm.SubItems.Add("");
                                    itmdpsumm.SubItems.Add("P " + annualAmt);
                                    lvwPH.Items.Add(itmdpsumm);
                                }
                               
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 != "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[32].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[42].ToString());
                            lvwPH.Items.Add(itmdp8);

                            ListViewItem itmdp9 = new ListViewItem();
                            itmdp9.Text = "NINETH PAYMENT";
                            itmdp9.SubItems.Add(dpay9);
                            itmdp9.SubItems.Add("P " + dtpski.Rows[0].ItemArray[23].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[33].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[43].ToString());
                            lvwPH.Items.Add(itmdp9);

                            con.Open();
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                               
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add(dpay10);
                                    itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[34].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[44].ToString());
                                    lvwPH.Items.Add(itmdp10);

                                    double theAssessment = Convert.ToDouble(fiftyDiscTotal_K);
                                    if (theAssessment >= 1000)
                                    { fiftyDiscTotal_K = String.Format(("{0:0,###.00#}"), theAssessment); }
                                    else
                                    { fiftyDiscTotal_K= String.Format(("{0:0.00#}"), theAssessment); }

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + fiftyDiscTotal_K);
                                    lvwPH.Items.Add(itmdsum);
                                }
                                if (discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add(dpay10);
                                    itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[34].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[44].ToString());
                                    lvwPH.Items.Add(itmdp10);

                                    string theAssessment = "";
                                    if (discountedTotalOtherDisc >= 1000)
                                    { theAssessment = String.Format(("{0:0,###.00#}"),discountedTotalOtherDisc); }
                                    else
                                    { theAssessment = String.Format(("{0:0.00#}"), discountedTotalOtherDisc); }

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + theAssessment);
                                    lvwPH.Items.Add(itmdsum);
                                }
                            }
                            else
                            {
                                ListViewItem itmdp10 = new ListViewItem();
                                itmdp10.Text = "TENTH PAYMENT";
                                itmdp10.SubItems.Add(dpay10);
                                itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[34].ToString());
                                itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[44].ToString());
                                lvwPH.Items.Add(itmdp10);

                                ListViewItem itmdsum = new ListViewItem();
                                itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdsum.Text = "Total:";
                                itmdsum.SubItems.Add("");
                                itmdsum.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdsum);
                            }
                           
                        }
                        
                    }
                    else if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                    {
                       if (dateupon == "" && dpay2 == "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            lvwPH.Clear();
                            lvwPH.Items.Clear();
                            pnlNotPH.Visible = true;
                        }
                        if (dateupon != "" && dpay2 == "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[32].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[42].ToString());
                            lvwPH.Items.Add(itmdp8);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[32].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[42].ToString());
                            lvwPH.Items.Add(itmdp8);

                            ListViewItem itmdp9 = new ListViewItem();
                            itmdp9.Text = "NINETH PAYMENT";
                            itmdp9.SubItems.Add(dpay9);
                            itmdp9.SubItems.Add("P " + dtpski.Rows[0].ItemArray[23].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[33].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[43].ToString());
                            lvwPH.Items.Add(itmdp9);

                            con.Open();
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                                if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add("");
                                    itmdp10.SubItems.Add("P " + "0.00");
                                    lvwPH.Items.Add(itmdp10);

                                    double theAssessment = Convert.ToDouble(FreeLastMonthTotalJ);
                                    if (theAssessment >= 1000)
                                    { FreeLastMonthTotalJ = String.Format(("{0:0,###.00#}"), theAssessment); }
                                    else
                                    { FreeLastMonthTotalJ = String.Format(("{0:0.00#}"), theAssessment); }

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + FreeLastMonthTotalJ);
                                    lvwPH.Items.Add(itmdsum);
                                }
                               
                            }
                            else
                            {
                                if (CurrBal <= 0)
                                {
                                    ListViewItem itmdpsumm = new ListViewItem();
                                    itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdpsumm.Text = "Total:";
                                    itmdpsumm.SubItems.Add("");
                                    itmdpsumm.SubItems.Add("P " + annualAmt);
                                    lvwPH.Items.Add(itmdpsumm);
                                }
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 != "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[32].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[42].ToString());
                            lvwPH.Items.Add(itmdp8);

                            ListViewItem itmdp9 = new ListViewItem();
                            itmdp9.Text = "NINETH PAYMENT";
                            itmdp9.SubItems.Add(dpay9);
                            itmdp9.SubItems.Add("P " + dtpski.Rows[0].ItemArray[23].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[33].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[43].ToString());
                            lvwPH.Items.Add(itmdp9);


                            con.Open();
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();

                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add(dpay10);
                                    itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[34].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[44].ToString());
                                    lvwPH.Items.Add(itmdp10);

                                    double theAssessment = Convert.ToDouble(fiftyDiscTotalJ);
                                    if (theAssessment >= 1000)
                                    { fiftyDiscTotalJ = String.Format(("{0:0,###.00#}"), theAssessment); }
                                    else
                                    { fiftyDiscTotalJ = String.Format(("{0:0.00#}"), theAssessment); }

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + fiftyDiscTotalJ);
                                    lvwPH.Items.Add(itmdsum);
                                }
                                if (discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add(dpay10);
                                    itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[34].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[44].ToString());
                                    lvwPH.Items.Add(itmdp10);

                                    string theAssessment = "";
                                    if (discountedTotalOtherDisc >= 1000)
                                    { theAssessment = String.Format(("{0:0,###.00#}"), discountedTotalOtherDisc); }
                                    else
                                    { theAssessment = String.Format(("{0:0.00#}"), discountedTotalOtherDisc); }

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + theAssessment);
                                    lvwPH.Items.Add(itmdsum);
                                }
                            }
                            else
                            {
                                ListViewItem itmdp10 = new ListViewItem();
                                itmdp10.Text = "TENTH PAYMENT";
                                itmdp10.SubItems.Add(dpay10);
                                itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[34].ToString());
                                itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[44].ToString());
                                lvwPH.Items.Add(itmdp10);

                                ListViewItem itmdsum = new ListViewItem();
                                itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdsum.Text = "Total:";
                                itmdsum.SubItems.Add("");
                                itmdsum.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdsum);
                            }
                          
                        }
                        
                    }
                    else
                    {
                        if (dateupon == "" && dpay2 == "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            lvwPH.Clear();
                            lvwPH.Items.Clear();
                            pnlNotPH.Visible = true;
                        }
                        if (dateupon != "" && dpay2 == "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 == "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[32].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[42].ToString());
                            lvwPH.Items.Add(itmdp8);

                            if (CurrBal <= 0)
                            {
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 == "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[32].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[42].ToString());
                            lvwPH.Items.Add(itmdp8);

                            ListViewItem itmdp9 = new ListViewItem();
                            itmdp9.Text = "NINETH PAYMENT";
                            itmdp9.SubItems.Add(dpay9);
                            itmdp9.SubItems.Add("P " + dtpski.Rows[0].ItemArray[23].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[33].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[43].ToString());
                            lvwPH.Items.Add(itmdp9);

                            con.Open();
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                                if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add("");
                                    itmdp10.SubItems.Add("P " + "0.00");
                                    lvwPH.Items.Add(itmdp10);

                                    double theAssessment = Convert.ToDouble(FreeLastMonthTotalE);
                                    if (theAssessment >= 1000)
                                    { FreeLastMonthTotalE = String.Format(("{0:0,###.00#}"), theAssessment); }
                                    else
                                    { FreeLastMonthTotalE = String.Format(("{0:0.00#}"), theAssessment); }

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + FreeLastMonthTotalE);
                                    lvwPH.Items.Add(itmdsum);
                                }

                            }
                            else
                            {
                                if (CurrBal <= 0)
                                {
                                    ListViewItem itmdpsumm = new ListViewItem();
                                    itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdpsumm.Text = "Total:";
                                    itmdpsumm.SubItems.Add("");
                                    itmdpsumm.SubItems.Add("P " + annualAmt);
                                    lvwPH.Items.Add(itmdpsumm);
                                }
                            }
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 != "")
                        {
                            pnlNotPH.Visible = false;
                            lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                            lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                            lvwPH.Columns.Add("Time", 130, HorizontalAlignment.Center);
                            lvwPH.Columns.Add("Cashier", 256, HorizontalAlignment.Left);

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[25].ToString());
                            itmdp.SubItems.Add(dtpski.Rows[0].ItemArray[35].ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[26].ToString());
                            itmdp2.SubItems.Add(dtpski.Rows[0].ItemArray[36].ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[27].ToString());
                            itmdp3.SubItems.Add(dtpski.Rows[0].ItemArray[37].ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[28].ToString());
                            itmdp4.SubItems.Add(dtpski.Rows[0].ItemArray[38].ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[29].ToString());
                            itmdp5.SubItems.Add(dtpski.Rows[0].ItemArray[39].ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[30].ToString());
                            itmdp6.SubItems.Add(dtpski.Rows[0].ItemArray[40].ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[31].ToString());
                            itmdp7.SubItems.Add(dtpski.Rows[0].ItemArray[41].ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[32].ToString());
                            itmdp8.SubItems.Add(dtpski.Rows[0].ItemArray[42].ToString());
                            lvwPH.Items.Add(itmdp8);

                            ListViewItem itmdp9 = new ListViewItem();
                            itmdp9.Text = "NINETH PAYMENT";
                            itmdp9.SubItems.Add(dpay9);
                            itmdp9.SubItems.Add("P " + dtpski.Rows[0].ItemArray[23].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[33].ToString());
                            itmdp9.SubItems.Add(dtpski.Rows[0].ItemArray[43].ToString());
                            lvwPH.Items.Add(itmdp9);


                            con.Open();
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                               
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add(dpay10);
                                    itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[34].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[44].ToString());
                                    lvwPH.Items.Add(itmdp10);

                                    double theAssessment = Convert.ToDouble(fiftyDiscTotalE);
                                    if (theAssessment >= 1000)
                                    { fiftyDiscTotalE = String.Format(("{0:0,###.00#}"), theAssessment); }
                                    else
                                    { fiftyDiscTotalE= String.Format(("{0:0.00#}"), theAssessment); }

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + fiftyDiscTotalE);
                                    lvwPH.Items.Add(itmdsum);
                                }
                                if (discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add(dpay10);
                                    itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[34].ToString());
                                    itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[44].ToString());
                                    lvwPH.Items.Add(itmdp10);

                                    string theAssessment = "";
                                    if (discountedTotalOtherDisc >= 1000)
                                    { theAssessment = String.Format(("{0:0,###.00#}"), discountedTotalOtherDisc); }
                                    else
                                    { theAssessment = String.Format(("{0:0.00#}"), discountedTotalOtherDisc); }

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + theAssessment);
                                    lvwPH.Items.Add(itmdsum);
                                }
                            }
                            else
                            {
                                ListViewItem itmdp10 = new ListViewItem();
                                itmdp10.Text = "TENTH PAYMENT";
                                itmdp10.SubItems.Add(dpay10);
                                itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[34].ToString());
                                itmdp10.SubItems.Add(dtpski.Rows[0].ItemArray[44].ToString());
                                lvwPH.Items.Add(itmdp10);

                                ListViewItem itmdsum = new ListViewItem();
                                itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdsum.Text = "Total:";
                                itmdsum.SubItems.Add("");
                                itmdsum.SubItems.Add("P " + annualAmt);
                                lvwPH.Items.Add(itmdsum);
                            }
                          
                        }
                        
                    }
                }
                else
                {
                    pnlNotPH.Visible = true;
                }
            }
        }

       public void setupPaymentSummary()
       {
           lvwPS.Clear();
           string disctype = "";
           con.Open();
           OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
           DataTable dtDisc = new DataTable();
           daDisc.Fill(dtDisc);
           con.Close();

           double MonthlyAmt_Kin = Convert.ToDouble(monthlyamount_K);
           double MonthlyAmt_Elem = Convert.ToDouble(monthlyamount_E);
           double MonthlyAmt_Jr = Convert.ToDouble(monthlyamount_J);

           double fiftyDisc_Kin = Convert.ToDouble(fiftyDisc_K);
           double fiftyDisc_Elem = Convert.ToDouble(fiftyDisc_E);
           double fiftyDisc_Jr = Convert.ToDouble(fiftyDisc_J);

           if (dtDisc.Rows.Count > 0)
           {
               disctype = dtDisc.Rows[0].ItemArray[1].ToString();
           }
           else
           {
               disctype = "None";
           }

           if (txtMOP.Text == "Cash")
           {
             
              
               string bal = "";
               con.Open();
               OdbcDataAdapter daps = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + txtSnum.Text + "'", con);
               DataTable dtps = new DataTable();
               daps.Fill(dtps);
               con.Close();

               if (dtps.Rows.Count > 0)
               {
                   lvwPS.Columns.Add("Description", 100, HorizontalAlignment.Left);
                   lvwPS.Columns.Add("", 129, HorizontalAlignment.Right);

                   if (dtps.Rows[0].ItemArray[4].ToString() == "")
                   {
                       double b = Convert.ToDouble(dtps.Rows[0].ItemArray[2].ToString());
                       if (b >= 1000)
                       {
                           bal= String.Format(("{0:0,###.00#}"), b);
                       } if (b < 1000)
                       {
                           bal = String.Format(("{0:0.00#}"), b);
                       }
                     
                   }
                   else
                   {
                       bal = "0.00";
                   }

                   
                    ListViewItem itps0 = new ListViewItem();
                    itps0.Text = "Balance";
                    itps0.SubItems.Add("P " + bal);
                    lvwPS.Items.Add(itps0);
                  
                  
                   ListViewItem itps1 = new ListViewItem();
                   itps1.Text = "Discount";
                   itps1.SubItems.Add(disctype);
                   lvwPS.Items.Add(itps1);

                   string discounttype = disctype;
                   string lessAmtToDisplay = "";

                   if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                   {
                       if (txtGrd.Text == "Kinder")
                       {
                           if (MonthlyAmt_Kin >= 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyamount_K));
                           }
                           if (MonthlyAmt_Kin < 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyamount_K));
                           }
                       }
                       if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
                       {
                           if (MonthlyAmt_Elem >= 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyamount_E));
                           }
                           if (MonthlyAmt_Elem < 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyamount_E));
                           }
                       }
                       if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                       {
                           if (MonthlyAmt_Jr >= 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyamount_J));
                           }
                           if (MonthlyAmt_Jr < 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyamount_J));
                           }
                       }
                   }
                   else if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                   {
                       if (txtGrd.Text == "Kinder")
                       {
                           if (fiftyDisc_Kin >= 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftyDisc_K));
                           }
                           if (fiftyDisc_Kin < 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftyDisc_K));
                           }
                       }
                       if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
                       {
                           if (fiftyDisc_Elem >= 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftyDisc_E));
                           }
                           if (fiftyDisc_Elem < 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftyDisc_E));
                           }
                       }
                       if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                       {
                           if (fiftyDisc_Jr >= 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftyDisc_J));
                           }
                           if (fiftyDisc_Jr < 1000)
                           {
                               lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftyDisc_J));
                           }
                       }
                   }
                   else if ((discounttype!="None")&&(discounttype.Contains("siblings") == false || discounttype.Contains("First") == false || discounttype.Contains("1st") == false || discounttype.Contains("Second") == false || discounttype.Contains("2nd") == false))
                   {
                       if (discountedAmtOtherDisc >= 1000)
                       {
                           lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                       }
                       if (discountedAmtOtherDisc < 1000)
                       {
                           lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                       } 
                   }
                   else
                   {
                       lessAmtToDisplay = "0";
                   }

                   ListViewItem itps3 = new ListViewItem();
                   itps3.Text = "Less Amount";
                   itps3.SubItems.Add("P " + lessAmtToDisplay);
                   lvwPS.Items.Add(itps3);
               }
               else
               {
                   //lvwPS.Clear();
                   //lvwPS.Items.Clear();
               }
           }
           if (txtMOP.Text == "Installment")
           {
              
               string bal = "";

               con.Open();
               OdbcDataAdapter dapskmi = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + txtSnum.Text + "'", con);
               DataTable dtpskmi = new DataTable();
               dapskmi.Fill(dtpskmi);
               con.Close();

               if (dtpskmi.Rows.Count > 0)
               {
                    lvwPS.Columns.Add("Description", 100, HorizontalAlignment.Left);
                    lvwPS.Columns.Add("", 129, HorizontalAlignment.Right);

                    bal = dtpskmi.Rows[0].ItemArray[4].ToString();
                    double baldouble = Convert.ToDouble(bal);
                    if (baldouble>0)
                    {
                        ListViewItem itps1 = new ListViewItem();
                        itps1.Text = "Balance";
                        if (baldouble >= 1000)
                        {
                            bal = String.Format(("{0:0,###.00#}"), Convert.ToDouble(baldouble));
                        }
                        if (baldouble < 1000)
                        {
                            bal = String.Format(("{0:0.00#}"), Convert.ToDouble(baldouble));
                        }
                        itps1.SubItems.Add("P "+bal);
                        lvwPS.Items.Add(itps1);
                    }
                    else
                    {
                        ListViewItem itps1 = new ListViewItem();
                        itps1.Text = "Balance";
                        itps1.SubItems.Add("P " + "0.00");
                        lvwPS.Items.Add(itps1);
                    }

                    ListViewItem itps2 = new ListViewItem();
                    itps2.Text = "Discount";
                    itps2.SubItems.Add(disctype);
                    lvwPS.Items.Add(itps2);

                    string discounttype = disctype;
                    string lessAmtToDisplay = "";

                    if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                    {
                        if (txtGrd.Text == "Kinder") 
                        {
                            if (MonthlyAmt_Kin >= 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyamount_K));  
                            }
                            if (MonthlyAmt_Kin < 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyamount_K));
                            }
                        }
                        if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6") 
                        {
                            if (MonthlyAmt_Elem >= 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyamount_E));
                            }
                            if (MonthlyAmt_Elem < 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyamount_E));
                            }
                        }
                        if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10") 
                        {
                            if (MonthlyAmt_Jr >= 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyamount_J));
                            }
                            if (MonthlyAmt_Jr < 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyamount_J));
                            } 
                        }
                    }
                    else if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                    {
                        if (txtGrd.Text == "Kinder")
                        {
                            if (fiftyDisc_Kin >= 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftyDisc_K));
                            }
                            if (fiftyDisc_Kin < 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftyDisc_K));
                            }
                        }
                        if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
                        {
                            if (fiftyDisc_Elem >= 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftyDisc_E));
                            }
                            if (fiftyDisc_Elem < 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftyDisc_E));
                            }
                        }
                        if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                        {
                            if (fiftyDisc_Jr >= 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftyDisc_J));
                            }
                            if (fiftyDisc_Jr < 1000)
                            {
                                lessAmtToDisplay = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftyDisc_J));
                            }
                        }

                    }
                    else if ((discounttype != "None") && (discounttype.Contains("siblings") == false || discounttype.Contains("First") == false || discounttype.Contains("1st") == false || discounttype.Contains("Second") == false || discounttype.Contains("2nd") == false))
                    {
                        //here here
                       
                        if (discountedAmtOtherDisc >= 1000)
                        {
                            lessAmtToDisplay = String.Format(("{0:0,###.00#}"), discountedAmtOtherDisc);
                        }
                        if (discountedAmtOtherDisc < 1000)
                        {
                            lessAmtToDisplay = String.Format(("{0:0.00#}"),discountedAmtOtherDisc);
                        }
                        
                    }
                    else
                    {
                        lessAmtToDisplay = "0.00";
                    }

                    ListViewItem itps3 = new ListViewItem();
                    itps3.Text = "Less Amount";
                    itps3.SubItems.Add("P " + lessAmtToDisplay);
                    lvwPS.Items.Add(itps3);
                }
            }
            else
            {
                //lvwPS.Clear();
               // lvwPS.Items.Clear();
            }
       }

       public void setupClassSched()
       {
           if (dgvSearch.Rows.Count <= 0)
           {
               return;
           }

           string classgrade = txtGrd.Text;
           string classsec = txtSec.Text;
           setupviewbysection(classgrade,classsec);
           
       }

       public void setupviewbysection(string grd,string sec)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select subject as 'Subject',faculty as 'Faculty',room as 'Room',start as 'Time start',end as 'Time end',days as 'Days' from schedule_tbl where level='" + grd + "'and section='"+sec+"'", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           DataView dvTheSched = new DataView(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               pnldisnotify.Visible = false;
               dgvSched.DataSource = null;
               dgvSched.DataSource = dvTheSched;

               dgvSched.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               dgvSched.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               dgvSched.Columns[0].Width = 200;
               dgvSched.Columns[1].Width = 240;
               dgvSched.Columns[2].Width = 140;
               dgvSched.Columns[3].Width = 120;
               dgvSched.Columns[4].Width = 120;
               dgvSched.Columns[5].Width = 140;
           }
           else
           {
               dgvSched.DataSource = null;
               pnldisnotify.Visible = true;
               lbldismemo.Text = "no items found...";
           }
       }
       public void setupKinderGrades(string no)
       {
           con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from Kindergrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                ListViewItem itspace1 = new ListViewItem();
                itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                itspace1.BackColor = Color.WhiteSmoke;
                itspace1.Text = "Level: Kinder";
                lvwSG.Items.Add(itspace1);

                for (int b = 0; b < dt.Rows.Count; b++)
                {
                    ListViewItem itk = new ListViewItem();
                    itk.Text = dt.Rows[b].ItemArray[1].ToString();
                    itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                    itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                    itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                    itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                    itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                    itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                    lvwSG.Items.Add(itk);
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from Kindergrades_tbl where studno='" + no + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0;
                    if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                    {

                    }
                    else
                    {
                        genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                    }

                    string rem = "";
                    if (genave < 75)
                    {
                        if (genave == 0)
                        {
                            rem = "";
                        }
                        else
                        {
                            rem = "Failed";
                        }
                    }
                    else { rem = "Passed"; }

                    ListViewItem it = new ListViewItem();
                    it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                    it.BackColor = Color.FromArgb(216, 223, 234);
                    it.Text = "Average:";
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);

                    ListViewItem itspace = new ListViewItem();
                    itspace.Text = "";
                    lvwSG.Items.Add(itspace);
                } 
            }
       }

       public void setupGrade1Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradeonegrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 1";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeonegrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               } 
           }

           
       }

       public void setupGrade2Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradetwogrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 2";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradetwogrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               }
           }
       }

       public void setupGrade3Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradethreegrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 3";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradethreegrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               }
           }
       }

       public void setupGrade4Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradefourgrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 4";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradefourgrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               }
           }
       }

       public void setupGrade5Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradefivegrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 5";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradefivegrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               }
           }
       }

       public void setupGrade6Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradesixgrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 6";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradesixgrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               }
           }
       }

       public void setupGrade7Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradesevengrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 7";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradesevengrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               }
           }
       }

       public void setupGrade8Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradeeightgrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 8";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeeightgrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               }
           }
       }

       public void setupGrade9Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradeninegrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 9";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeninegrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               }
           }
       }

       public void setupGrade10Grades(string no)
       {
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from gradetengrades_tbl where studno='" + no + "'ORDER BY subdesc ASC", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               ListViewItem itspace1 = new ListViewItem();
               itspace1.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
               itspace1.BackColor = Color.WhiteSmoke;
               itspace1.Text = "Level: Grade 10";
               lvwSG.Items.Add(itspace1);

               for (int b = 0; b < dt.Rows.Count; b++)
               {
                   ListViewItem itk = new ListViewItem();
                   itk.Text = dt.Rows[b].ItemArray[1].ToString();
                   itk.SubItems.Add(dt.Rows[b].ItemArray[2].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[3].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[4].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[5].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[6].ToString());
                   itk.SubItems.Add(dt.Rows[b].ItemArray[7].ToString());
                   lvwSG.Items.Add(itk);
               }

               con.Open();
               OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradetengrades_tbl where studno='" + no + "'", con);
               DataTable dtk11 = new DataTable();
               dak11.Fill(dtk11);
               con.Close();
               if (dtk11.Rows.Count > 0)
               {
                   double genave = 0;
                   if (dtk11.Rows[0].ItemArray[4].ToString() == "")
                   {

                   }
                   else
                   {
                       genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
                   }

                   string rem = "";
                   if (genave < 75)
                   {
                       if (genave == 0)
                       {
                           rem = "";
                       }
                       else
                       {
                           rem = "Failed";
                       }
                   }
                   else { rem = "Passed"; }

                   ListViewItem it = new ListViewItem();
                   it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                   it.BackColor = Color.FromArgb(216, 223, 234);
                   it.Text = "Average:";
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                   it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                   it.SubItems.Add(rem);
                   lvwSG.Items.Add(it);

                   ListViewItem itspace = new ListViewItem();
                   itspace.Text = "";
                   lvwSG.Items.Add(itspace);
               }
           }
       }

       public void setupGrades(string no)
       {
           lvwSG.Items.Clear();
           string lev = txtGrd.Text;

           if (lev == "Kinder")
           {
               setupKinderGrades(no);
           }
           if (lev == "Grade 1")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
           }
           if (lev == "Grade 2")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
               setupGrade2Grades(no);
           }
           if (lev == "Grade 3")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
               setupGrade2Grades(no);
               setupGrade3Grades(no);
           }
           if (lev == "Grade 4")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
               setupGrade2Grades(no);
               setupGrade3Grades(no);
               setupGrade4Grades(no);
           }
           if (lev == "Grade 5")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
               setupGrade2Grades(no);
               setupGrade3Grades(no);
               setupGrade4Grades(no);
               setupGrade5Grades(no);
           }
           if (lev == "Grade 6")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
               setupGrade2Grades(no);
               setupGrade3Grades(no);
               setupGrade4Grades(no);
               setupGrade5Grades(no);
               setupGrade6Grades(no);
           }
           if (lev == "Grade 7")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
               setupGrade2Grades(no);
               setupGrade3Grades(no);
               setupGrade4Grades(no);
               setupGrade5Grades(no);
               setupGrade6Grades(no);
               setupGrade7Grades(no);
           }
           if (lev == "Grade 8")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
               setupGrade2Grades(no);
               setupGrade3Grades(no);
               setupGrade4Grades(no);
               setupGrade5Grades(no);
               setupGrade6Grades(no);
               setupGrade7Grades(no);
               setupGrade8Grades(no);
           }
           if (lev == "Grade 9")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
               setupGrade2Grades(no);
               setupGrade3Grades(no);
               setupGrade4Grades(no);
               setupGrade5Grades(no);
               setupGrade6Grades(no);
               setupGrade7Grades(no);
               setupGrade8Grades(no);
               setupGrade9Grades(no);
           }
           if (lev == "Grade 10")
           {
               setupKinderGrades(no);
               setupGrade1Grades(no);
               setupGrade2Grades(no);
               setupGrade3Grades(no);
               setupGrade4Grades(no);
               setupGrade5Grades(no);
               setupGrade6Grades(no);
               setupGrade7Grades(no);
               setupGrade8Grades(no);
               setupGrade9Grades(no);
               setupGrade10Grades(no);
           }


       }


       public void PrintSOA(object sender, PrintPageEventArgs e)
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
           Font df5 = new Font("Arial", 10, FontStyle.Bold);
           Font df6 = new Font("Arial", 10, FontStyle.Regular);
           Font df0 = new Font("Arial", 8, FontStyle.Regular);
           SolidBrush drawBrush = new SolidBrush(Color.Black);

           // Create a new pen.
           Pen pen1 = new Pen(Brushes.Black);
           pen1.Width = 1F;
           pen1.LineJoin = System.Drawing.Drawing2D.LineJoin.Miter;

           
           //REPORT'S HEADER

           e.Graphics.Clear(Color.White);
           e.Graphics.DrawRectangle(pen1, 50, 50, 671, 600);
           Rectangle r = new Rectangle(70, 70, 85, 80);
           Image newImage = Image.FromFile(@"C:\Users\valued client\Documents\Visual Studio 2010\Projects\1 - THESIS\berlyn.bmp");
           e.Graphics.DrawImage(newImage, r);

           e.Graphics.DrawString("Berlyn Academy", df4, Brushes.Black, 400, 75, sf);
           e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df0, Brushes.Black, 400, 95, sf);
           e.Graphics.DrawString("Recognition Nos. E-089 / E-110 / S-002", df0, Brushes.Black, 400, 110, sf);
           e.Graphics.DrawString("Email Address: berlynacademy@yahoo.com", df0, Brushes.Black, 400, 125, sf);
           e.Graphics.DrawString("STATEMENT OF ACCOUNT", df4, Brushes.Black, 300, 150, sf1);
           e.Graphics.DrawString("_________________________", df4, Brushes.Black, 301, 151);
           e.Graphics.DrawString("as of "+DateTime.Now.ToShortDateString(), df0, Brushes.Black, 365, 169);

           e.Graphics.DrawRectangle(pen1, 75, 190, 621, 30);
           e.Graphics.DrawRectangle(pen1, 75, 220, 621, 30);
           e.Graphics.DrawRectangle(pen1, 75, 250, 621, 45);//start locx end  locy
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(182, 220), new Point(182, 191));//vertical studno
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(510, 220), new Point(510, 191));//vertical before sy
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(152, 250), new Point(152, 220));//vertical lev
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(248, 250), new Point(248, 220));//vertical sec
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(327, 250), new Point(327, 220));//vertical sec
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(608, 250), new Point(608, 220));//vertical mop
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(400, 220), new Point(400, 191));//vertical sy
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(467, 250), new Point(467, 220));//vertical mop b4

           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(277, 294), new Point(277, 250));//vertical mop b4
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(487, 294), new Point(487, 250));//vertical mop b4

           e.Graphics.DrawString("Student no:", df5, Brushes.Black, 80, 197);
           e.Graphics.DrawString("School year:", df5, Brushes.Black, 403, 197);
           e.Graphics.DrawString("Level", df5, Brushes.Black, 80, 227);
           e.Graphics.DrawString("Section", df5, Brushes.Black, 250, 227);
           e.Graphics.DrawString("Lastname:", df5, Brushes.Black, 80, 257);
           e.Graphics.DrawString("Firstname:", df5, Brushes.Black, 280, 257);
           e.Graphics.DrawString("Middlename:", df5, Brushes.Black, 490, 257);
           e.Graphics.DrawString(txtGrd.Text, df6, Brushes.Black, 155, 227);
           e.Graphics.DrawString(txtSec.Text, df6, Brushes.Black, 330, 227);
           e.Graphics.DrawString(txtSnum.Text, df6, Brushes.Black, 185, 197);
           e.Graphics.DrawString(txtSY.Text, df6, Brushes.Black, 513, 197);
           e.Graphics.DrawString(txtLast.Text, df6, Brushes.Black, 80, 275);
           e.Graphics.DrawString(txtFirst.Text, df6, Brushes.Black, 280, 275);
           e.Graphics.DrawString(txtMid.Text, df6, Brushes.Black, 490, 275);
          
           e.Graphics.DrawString("Mode of Payment:", df5, Brushes.Black, 473, 227);
           e.Graphics.DrawString(txtMOP.Text, df6, Brushes.Black, 611, 227);

           e.Graphics.DrawRectangle(pen1, 75, 300, 300, 260);//assessment main
           e.Graphics.DrawRectangle(pen1, 75, 300, 300, 40);//assessment sub
           e.Graphics.FillRectangle(Brushes.WhiteSmoke, 76, 301, 299, 38);
           e.Graphics.DrawRectangle(pen1, 75, 300, 300, 20);//assessment title
           e.Graphics.DrawString("ASSESSMENT", df5, Brushes.Black, 182, 302);
           e.Graphics.DrawString("Payments", df5, Brushes.Black, 118, 322);
           e.Graphics.DrawString("Date due", df5, Brushes.Black, 228, 322);
           e.Graphics.DrawString("Amount", df5, Brushes.Black, 307, 322);
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(222, 320), new Point(222, 560));
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(294, 320), new Point(294, 560));


           e.Graphics.DrawRectangle(pen1, 375, 300, 321, 260);//payment history main
           e.Graphics.DrawRectangle(pen1, 375, 300, 321, 40);//payment history sub
           e.Graphics.FillRectangle(Brushes.WhiteSmoke, 376, 301, 319, 38);
           e.Graphics.DrawRectangle(pen1, 375, 300, 321, 20);//assessment title
           e.Graphics.DrawString("PAYMENT HISTORY", df5, Brushes.Black, 472, 302);
           e.Graphics.DrawString("OR#", df5, Brushes.Black, 397, 322);
           e.Graphics.DrawString("Date paid", df5, Brushes.Black, 452, 322);
           e.Graphics.DrawString("Amount", df5, Brushes.Black, 539, 322);
           e.Graphics.DrawString("Balance", df5, Brushes.Black, 623, 322);
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(448, 320), new Point(448, 560));
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(522, 320), new Point(522, 560));
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(611, 320), new Point(611, 560));

          //header soa

           int SOATop = 345;

           if (txtGrd.Text == "Kinder")
           {
               if (txtMOP.Text == "Cash")
               {
                   setupDateRegistered_Cash();
                   if (lblAssesDiscount.Text != "None")
                   {
                       if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                       {
                           double _amt = Convert.ToDouble(anuualamt_freelastmonthK);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }
                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                           e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                       {
                           double _amt = Convert.ToDouble(annualamt_fiftydiscK);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                           e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                       {
                           double _amt = Convert.ToDouble(discountedTotalOtherDisc);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                           e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                   }
                   else
                   {
                       double _amt = Convert.ToDouble(annualamount_K);
                       string amt_dis = "";
                       if (_amt >= 1000)
                       {
                           amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(annualamount_K));
                       }
                       if (_amt < 1000)
                       {
                           amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(annualamount_K));
                       }
                       totalAss_SOA = amt_dis;

                       e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                       e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                   }
               }
               if (txtMOP.Text == "Installment")
               {
                   setupDateRegistered_Installment();
                   //stopped
                   e.Graphics.DrawString("UPON ENROLLMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                   e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                   e.Graphics.DrawString("P " + uponamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                   if (lblAssesDiscount.Text != "None")
                   {
                       if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                       {
                           double _amt = Convert.ToDouble(FreeLastMonthTotal_K);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black,297, SOATop);

                           e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black,222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString("P 0.00", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                       {
                           double _amt = Convert.ToDouble(fiftyDiscTotal_K);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P "+fiftyDisc_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                       {

                           double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                           string amt_tot_dis = "";
                           string amt_monthlyIns_OtherDisc = "";
                           double uponamt = Convert.ToDouble(uponamount_K);
                           double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                           double monthlyInstallmentAmt_forOtherDisc = amt_deductUpon / 9;

                           if (_amt_tot >= 1000)
                           {
                               amt_tot_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt_tot));
                           }
                           if (_amt_tot < 1000)
                           {
                               amt_tot_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt_tot));
                           }

                           //-------------
                           if (monthlyInstallmentAmt_forOtherDisc >= 1000)
                           {
                               amt_monthlyIns_OtherDisc = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                           }
                           if (monthlyInstallmentAmt_forOtherDisc < 1000)
                           {
                               amt_monthlyIns_OtherDisc = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                           }

                           totalAss_SOA = amt_tot_dis;
                           e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                   }
                   else
                   {
                       double _amt = Convert.ToDouble(annualamount_K);
                       string amt_dis = "";

                       if (_amt >= 1000)
                       {
                           amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                       }
                       if (_amt < 1000)
                       {
                           amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                       }

                       totalAss_SOA = amt_dis;
                       e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);
                   }


                   //PAYMENT SCHEDULE MODIFY FOR DISCOUNT
               }//do current
           }
           if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" ||
                txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
           {
               if (txtMOP.Text == "Cash")
               {
                   setupDateRegistered_Cash();
                   if (lblAssesDiscount.Text != "None")
                   {
                       if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                       {
                           double _amt = Convert.ToDouble(anuualamt_freelastmonthE);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }
                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                           e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                       {
                           double _amt = Convert.ToDouble(annualamt_fiftydiscE);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                           e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                       {
                           double _amt = Convert.ToDouble(discountedTotalOtherDisc);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                           e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                   }
                   else
                   {
                       double _amt = Convert.ToDouble(annualamount_E);
                       string amt_dis = "";
                       if (_amt >= 1000)
                       {
                           amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(annualamount_E));
                       }
                       if (_amt < 1000)
                       {
                           amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(annualamount_E));
                       }
                       totalAss_SOA = amt_dis;

                       e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                       e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                   }
               }
               if (txtMOP.Text == "Installment")
               {
                   setupDateRegistered_Installment();
                   //stopped
                   e.Graphics.DrawString("UPON ENROLLMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                   e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                   e.Graphics.DrawString("P " + uponamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                   if (lblAssesDiscount.Text != "None")
                   {
                       if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                       {
                           double _amt = Convert.ToDouble(FreeLastMonthTotal_E);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString("P 0.00", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                       {
                           double _amt = Convert.ToDouble(fiftyDiscTotal_E);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + fiftyDisc_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                       {

                           double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                           string amt_tot_dis = "";
                           string amt_monthlyIns_OtherDisc = "";
                           double uponamt = Convert.ToDouble(uponamount_E);
                           double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                           double monthlyInstallmentAmt_forOtherDisc = amt_deductUpon / 9;

                           if (_amt_tot >= 1000)
                           {
                               amt_tot_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt_tot));
                           }
                           if (_amt_tot < 1000)
                           {
                               amt_tot_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt_tot));
                           }

                           //-------------
                           if (monthlyInstallmentAmt_forOtherDisc >= 1000)
                           {
                               amt_monthlyIns_OtherDisc = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                           }
                           if (monthlyInstallmentAmt_forOtherDisc < 1000)
                           {
                               amt_monthlyIns_OtherDisc = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                           }

                           totalAss_SOA = amt_tot_dis;
                           e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                   }
                   else
                   {
                       double _amt = Convert.ToDouble(annualamount_E);
                       string amt_dis = "";

                       if (_amt >= 1000)
                       {
                           amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                       }
                       if (_amt < 1000)
                       {
                           amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                       }

                       totalAss_SOA = amt_dis;
                       e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 225, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);
                   }


                   //PAYMENT SCHEDULE MODIFY FOR DISCOUNT
               }
           }
           if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
           {
               if (txtMOP.Text == "Cash")
               {
                   setupDateRegistered_Cash();
                   if (lblAssesDiscount.Text != "None")
                   {
                       if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                       {
                           double _amt = Convert.ToDouble(anuualamt_freelastmonthJ);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }
                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                           e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                       {
                           double _amt = Convert.ToDouble(annualamt_fiftydiscJ);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                           e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                       {
                           double _amt = Convert.ToDouble(discountedTotalOtherDisc);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                           e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                   }
                   else
                   {
                       double _amt = Convert.ToDouble(annualamount_J);
                       string amt_dis = "";
                       if (_amt >= 1000)
                       {
                           amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(annualamount_J));
                       }
                       if (_amt < 1000)
                       {
                           amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(annualamount_J));
                       }
                       totalAss_SOA = amt_dis;

                       e.Graphics.DrawString("ANNUAL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                       e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                   }
               }
               if (txtMOP.Text == "Installment")
               {
                   setupDateRegistered_Installment();
                   //stopped
                   e.Graphics.DrawString("UPON ENROLLMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop);
                   e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                   e.Graphics.DrawString("P " + uponamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                   if (lblAssesDiscount.Text != "None")
                   {
                       if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                       {
                           double _amt = Convert.ToDouble(FreeLastMonthTotal_J);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString("P 0.00", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                       {
                           double _amt = Convert.ToDouble(fiftyDiscTotal_J);
                           string amt_dis = "";
                           if (_amt >= 1000)
                           {
                               amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                           }
                           if (_amt < 1000)
                           {
                               amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                           }

                           totalAss_SOA = amt_dis;

                           e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + fiftyDisc_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                       if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                       {
                           double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                           string amt_tot_dis = "";
                           string amt_monthlyIns_OtherDisc = "";
                           double uponamt = Convert.ToDouble(uponamount_J);
                           double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                           double monthlyInstallmentAmt_forOtherDisc = amt_deductUpon / 9;

                           if (_amt_tot >= 1000)
                           {
                               amt_tot_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt_tot));
                           }
                           if (_amt_tot < 1000)
                           {
                               amt_tot_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt_tot));
                           }

                           //-------------
                           if (monthlyInstallmentAmt_forOtherDisc >= 1000)
                           {
                               amt_monthlyIns_OtherDisc = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                           }
                           if (monthlyInstallmentAmt_forOtherDisc < 1000)
                           {
                               amt_monthlyIns_OtherDisc = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                           }

                           totalAss_SOA = amt_tot_dis;
                           e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                           e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                           e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                           e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       }
                   }
                   else
                   {
                       double _amt = Convert.ToDouble(annualamount_J);
                       string amt_dis = "";

                       if (_amt >= 1000)
                       {
                           amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                       }
                       if (_amt < 1000)
                       {
                           amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                       }

                       totalAss_SOA = amt_dis;
                       e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);

                       e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, SOATop += 19);
                       e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 222, SOATop);
                       e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 297, SOATop);
                   }


                   //PAYMENT SCHEDULE MODIFY FOR DISCOUNT
               }
           }


           //PAYMENT HISTORY CONTENT
           int PHstart = 345;
           string TotalAmtPdPH_SOA="";
           string TotalBalPH_SOA="";
           string CurrentBal_SOA = "";

           if (txtMOP.Text == "")
           {
               //totalAss_SOA = "";
           }

           if (txtMOP.Text == "Cash")
           {
               con.Open();
               OdbcDataAdapter da = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + txtSnum.Text + "'", con);
               DataTable dt = new DataTable();
               da.Fill(dt);
               con.Close();

               if (dt.Rows.Count > 0)
               {
                   string ORNum = dt.Rows[0].ItemArray[8].ToString();
                   string AmtPaidPH_SOA = "";
                   string DatePdPH_SOA = dt.Rows[0].ItemArray[4].ToString();
                   string BalancePH_SOA = "";
                  
                   if (dt.Rows[0].ItemArray[4].ToString() != "")
                   {
                       double amtpd = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                       if (amtpd >= 1000)
                       {
                           AmtPaidPH_SOA = String.Format(("{0:0,###.00#}"), amtpd);
                       }
                       if (amtpd < 1000)
                       {
                           AmtPaidPH_SOA = String.Format(("{0:0.00#}"), amtpd);
                       }

                       TotalAmtPdPH_SOA = AmtPaidPH_SOA;
                       BalancePH_SOA = "0.00";
                       TotalBalPH_SOA = "0.00";
                       CurrentBal_SOA = "0.00";

                       e.Graphics.DrawString(ORNum, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                       e.Graphics.DrawString(DatePdPH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                       e.Graphics.DrawString("P " + AmtPaidPH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                       e.Graphics.DrawString("P " + BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                   }
                   else
                   {
                       double amt = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                       if (amt >= 1000)
                       {
                           BalancePH_SOA = String.Format(("{0:0,###.00#}"), amt);
                       }
                       if (amt < 1000)
                       {
                           BalancePH_SOA = String.Format(("{0:0.00#}"), amt);
                       }

                       AmtPaidPH_SOA = "";
                       CurrentBal_SOA = BalancePH_SOA;
                   }
               }
           }
           else
           {
               con.Open();
               OdbcDataAdapter da = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + txtSnum.Text + "'", con);
               DataTable dt = new DataTable();
               da.Fill(dt);
               con.Close();

               if (dt.Rows.Count > 0)
               {
                   //string AmtPaidPH_SOA = "";
                   string BalancePH_SOA = "";
                    
                   string ORNumUpon = dt.Rows[0].ItemArray[46].ToString();
                   string ORNumP2 = dt.Rows[0].ItemArray[47].ToString();
                   string ORNumP3 = dt.Rows[0].ItemArray[48].ToString();
                   string ORNumP4 = dt.Rows[0].ItemArray[49].ToString();
                   string ORNumP5 = dt.Rows[0].ItemArray[50].ToString();
                   string ORNumP6 = dt.Rows[0].ItemArray[51].ToString();
                   string ORNumP7 = dt.Rows[0].ItemArray[52].ToString();
                   string ORNumP8 = dt.Rows[0].ItemArray[53].ToString();
                   string ORNumP9 = dt.Rows[0].ItemArray[54].ToString();
                   string ORNumP10 = dt.Rows[0].ItemArray[55].ToString();

                    double annualAmountPH_SOA = Convert.ToDouble(dt.Rows[0].ItemArray[3].ToString());
                    string dateupon = dt.Rows[0].ItemArray[5].ToString();
                    string dpay2 = dt.Rows[0].ItemArray[6].ToString();
                    string dpay3 = dt.Rows[0].ItemArray[7].ToString();
                    string dpay4 = dt.Rows[0].ItemArray[8].ToString();
                    string dpay5 = dt.Rows[0].ItemArray[9].ToString();
                    string dpay6 = dt.Rows[0].ItemArray[10].ToString();
                    string dpay7 = dt.Rows[0].ItemArray[11].ToString();
                    string dpay8 = dt.Rows[0].ItemArray[12].ToString();
                    string dpay9 = dt.Rows[0].ItemArray[13].ToString();
                    string dpay10 = dt.Rows[0].ItemArray[14].ToString();

                    //item array 15 is amount paid
                        if (dateupon == "" && dpay2 == "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            double balPH = Convert.ToDouble(dt.Rows[0].ItemArray[4].ToString());
                          
                            if (balPH >= 1000)
                            {
                                BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                            }
                            if (balPH < 1000)
                            {
                                BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                            }
                            TotalBalPH_SOA = "";
                            CurrentBal_SOA = BalancePH_SOA;

                        }
                        if (dateupon != "" && dpay2 == "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            double paid_Amt = Convert.ToDouble(dt.Rows[0].ItemArray[15].ToString());
                            double balPH = annualAmountPH_SOA - paid_Amt;
                            if (balPH >= 1000)
                            {
                                BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                            }
                            if (balPH < 1000)
                            {
                                BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;
                            
                            e.Graphics.DrawString(ORNumUpon, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                            e.Graphics.DrawString(dateupon, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                            e.Graphics.DrawString("P "+dt.Rows[0].ItemArray[15].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                            e.Graphics.DrawString("P "+BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            double paid_Amt = 0; 
                          
                            string[]OR_Nums = new string[2]{ORNumUpon,ORNumP2};
                            string[]dates=new string[2]{dateupon,dpay2};
                            int RowNumInDB = 15;
                           
                            for(int i=0;i<2;i++)
                            {
                                paid_Amt += Convert.ToDouble(dt.Rows[0].ItemArray[RowNumInDB].ToString());
                                double balPH = annualAmountPH_SOA - paid_Amt;
                                    if (balPH >= 1000)
                                    {
                                        BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                                    }
                                    if (balPH < 1000)
                                    {
                                        BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                                    }

                                e.Graphics.DrawString(OR_Nums[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                                e.Graphics.DrawString(dates[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                                e.Graphics.DrawString("P "+dt.Rows[0].ItemArray[RowNumInDB].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                                e.Graphics.DrawString("P "+BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                                PHstart+=19;
                                RowNumInDB++;
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            double paid_Amt = 0;

                            string[] OR_Nums = new string[3] { ORNumUpon, ORNumP2,ORNumP3};
                            string[] dates = new string[3] { dateupon, dpay2,dpay3 };
                            int RowNumInDB = 15;

                            for (int i = 0; i < 3; i++)
                            {
                                paid_Amt += Convert.ToDouble(dt.Rows[0].ItemArray[RowNumInDB].ToString());
                                double balPH = annualAmountPH_SOA - paid_Amt;
                                if (balPH >= 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                                }
                                if (balPH < 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                                }

                                e.Graphics.DrawString(OR_Nums[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                                e.Graphics.DrawString(dates[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                                e.Graphics.DrawString("P " + dt.Rows[0].ItemArray[RowNumInDB].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                                e.Graphics.DrawString("P " + BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                                PHstart += 19;
                                RowNumInDB++;
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            double paid_Amt = 0;

                            string[] OR_Nums = new string[4] { ORNumUpon, ORNumP2, ORNumP3,ORNumP4};
                            string[] dates = new string[4] { dateupon, dpay2, dpay3,dpay4};
                            int RowNumInDB = 15;

                            for (int i = 0; i < 4; i++)
                            {
                                paid_Amt += Convert.ToDouble(dt.Rows[0].ItemArray[RowNumInDB].ToString());
                                double balPH = annualAmountPH_SOA - paid_Amt;
                                if (balPH >= 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                                }
                                if (balPH < 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                                }

                                e.Graphics.DrawString(OR_Nums[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                                e.Graphics.DrawString(dates[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                                e.Graphics.DrawString("P " + dt.Rows[0].ItemArray[RowNumInDB].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                                e.Graphics.DrawString("P " + BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                                PHstart += 19;
                                RowNumInDB++;
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            double paid_Amt = 0;

                            string[] OR_Nums = new string[5] { ORNumUpon, ORNumP2, ORNumP3, ORNumP4,ORNumP5};
                            string[] dates = new string[5] { dateupon, dpay2, dpay3, dpay4,dpay5};
                            int RowNumInDB = 15;

                            for (int i = 0; i < 5; i++)
                            {
                                paid_Amt += Convert.ToDouble(dt.Rows[0].ItemArray[RowNumInDB].ToString());
                                double balPH = annualAmountPH_SOA - paid_Amt;
                                if (balPH >= 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                                }
                                if (balPH < 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                                }

                                e.Graphics.DrawString(OR_Nums[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                                e.Graphics.DrawString(dates[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                                e.Graphics.DrawString("P " + dt.Rows[0].ItemArray[RowNumInDB].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                                e.Graphics.DrawString("P " + BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                                PHstart += 19;
                                RowNumInDB++;
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {

                            double paid_Amt = 0;

                            string[] OR_Nums = new string[6] { ORNumUpon, ORNumP2, ORNumP3, ORNumP4, ORNumP5,ORNumP6};
                            string[] dates = new string[6] { dateupon, dpay2, dpay3, dpay4, dpay5,dpay6};
                            int RowNumInDB = 15;

                            for (int i = 0; i < 6; i++)
                            {
                                paid_Amt += Convert.ToDouble(dt.Rows[0].ItemArray[RowNumInDB].ToString());
                                double balPH = annualAmountPH_SOA - paid_Amt;
                                if (balPH >= 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                                }
                                if (balPH < 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                                }

                                e.Graphics.DrawString(OR_Nums[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                                e.Graphics.DrawString(dates[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                                e.Graphics.DrawString("P " + dt.Rows[0].ItemArray[RowNumInDB].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                                e.Graphics.DrawString("P " + BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                                PHstart += 19;
                                RowNumInDB++;
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                        {
                            double paid_Amt = 0;

                            string[] OR_Nums = new string[7] { ORNumUpon, ORNumP2, ORNumP3, ORNumP4, ORNumP5, ORNumP6,ORNumP7};
                            string[] dates = new string[7] { dateupon, dpay2, dpay3, dpay4, dpay5, dpay6,dpay7};
                            int RowNumInDB = 15;

                            for (int i = 0; i < 7; i++)
                            {
                                paid_Amt += Convert.ToDouble(dt.Rows[0].ItemArray[RowNumInDB].ToString());
                                double balPH = annualAmountPH_SOA - paid_Amt;
                                if (balPH >= 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                                }
                                if (balPH < 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                                }

                                e.Graphics.DrawString(OR_Nums[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                                e.Graphics.DrawString(dates[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                                e.Graphics.DrawString("P " + dt.Rows[0].ItemArray[RowNumInDB].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                                e.Graphics.DrawString("P " + BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                                PHstart += 19;
                                RowNumInDB++;
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 == "" && dpay10 == "")
                        {
                            double paid_Amt = 0;

                            string[] OR_Nums = new string[8] { ORNumUpon, ORNumP2, ORNumP3, ORNumP4, ORNumP5, ORNumP6, ORNumP7,ORNumP8};
                            string[] dates = new string[8] { dateupon, dpay2, dpay3, dpay4, dpay5, dpay6, dpay7,dpay8};
                            int RowNumInDB = 15;

                            for (int i = 0; i < 8; i++)
                            {
                                paid_Amt += Convert.ToDouble(dt.Rows[0].ItemArray[RowNumInDB].ToString());
                                double balPH = annualAmountPH_SOA - paid_Amt;
                                if (balPH >= 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                                }
                                if (balPH < 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                                }

                                e.Graphics.DrawString(OR_Nums[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                                e.Graphics.DrawString(dates[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                                e.Graphics.DrawString("P " + dt.Rows[0].ItemArray[RowNumInDB].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                                e.Graphics.DrawString("P " + BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                                PHstart += 19;
                                RowNumInDB++;
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 == "")
                        {
                            double paid_Amt = 0;

                            string[] OR_Nums = new string[9] { ORNumUpon, ORNumP2, ORNumP3, ORNumP4, ORNumP5, ORNumP6, ORNumP7, ORNumP8,ORNumP9};
                            string[] dates = new string[9] { dateupon, dpay2, dpay3, dpay4, dpay5, dpay6, dpay7, dpay8,dpay9};
                            int RowNumInDB = 15;

                            for (int i = 0; i < 9; i++)
                            {
                                paid_Amt += Convert.ToDouble(dt.Rows[0].ItemArray[RowNumInDB].ToString());
                                double balPH = annualAmountPH_SOA - paid_Amt;
                                if (balPH >= 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                                }
                                if (balPH < 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                                }

                                e.Graphics.DrawString(OR_Nums[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                                e.Graphics.DrawString(dates[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                                e.Graphics.DrawString("P " + dt.Rows[0].ItemArray[RowNumInDB].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                                e.Graphics.DrawString("P " + BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                                PHstart += 19;
                                RowNumInDB++;
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;
                        }
                        if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 != "")
                        {
                            double paid_Amt = 0;

                            string[] OR_Nums = new string[10] { ORNumUpon, ORNumP2, ORNumP3, ORNumP4, ORNumP5, ORNumP6, ORNumP7, ORNumP8, ORNumP9,ORNumP10};
                            string[] dates = new string[10] { dateupon, dpay2, dpay3, dpay4, dpay5, dpay6, dpay7, dpay8, dpay9,dpay10};
                            int RowNumInDB = 15;

                            for (int i = 0; i < 10; i++)
                            {
                                paid_Amt += Convert.ToDouble(dt.Rows[0].ItemArray[RowNumInDB].ToString());
                                double balPH = annualAmountPH_SOA - paid_Amt;
                                if (balPH >= 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0,###.00#}"), balPH);
                                }
                                if (balPH < 1000)
                                {
                                    BalancePH_SOA = String.Format(("{0:0.00#}"), balPH);
                                }

                                e.Graphics.DrawString(OR_Nums[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, PHstart);
                                e.Graphics.DrawString(dates[i], new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 449, PHstart);
                                e.Graphics.DrawString("P " + dt.Rows[0].ItemArray[RowNumInDB].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 525, PHstart);
                                e.Graphics.DrawString("P " + BalancePH_SOA, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 614, PHstart);
                                PHstart += 19;
                                RowNumInDB++;
                            }

                            if (paid_Amt >= 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0,###.00#}"), paid_Amt);
                            }
                            if (paid_Amt < 1000)
                            {
                                TotalAmtPdPH_SOA = String.Format(("{0:0.00#}"), paid_Amt);
                            }

                            TotalBalPH_SOA = BalancePH_SOA;
                            CurrentBal_SOA = BalancePH_SOA;

                        }
               }
           }

           //FOOTER SOA
           e.Graphics.DrawRectangle(pen1, 75, 540, 621, 20);
           e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 80, 542);
           e.Graphics.DrawString("P " + totalAss_SOA, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 297, 542);

           e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, 542);
           e.Graphics.DrawString("P " +TotalAmtPdPH_SOA , new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 525, 542);
           e.Graphics.DrawString("P " + TotalBalPH_SOA, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 614, 542);

           e.Graphics.DrawLine(pen1, 75, 595, 695, 595);
           e.Graphics.DrawLine(pen1, 247, 595, 247, 624);//vertical v.a.s.b
           e.Graphics.DrawLine(pen1, 497, 595, 497, 624);//vertical b4 d.signed
           e.Graphics.DrawLine(pen1, 590, 595, 590, 624);//vertical after d.signed

           e.Graphics.DrawRectangle(pen1, 75, 565, 621, 60);//box 3
           e.Graphics.DrawString("Your current balance is P "+CurrentBal_SOA, df5, Brushes.Black, 80, 572);
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(398, 565), new Point(398, 595));
           e.Graphics.DrawString("Discount: ", df5, Brushes.Black, 400, 572);
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(487, 565), new Point(487, 595));
           e.Graphics.DrawString(lblAssesDiscount.Text + " (Less " + lblAssesLessAmt.Text + ")", df6, Brushes.Black, 495, 572);

           e.Graphics.DrawString("Validated and signed by: ", df6, Brushes.Black, 80, 602);
           e.Graphics.DrawString(co, df6, Brushes.Black, 250, 602);
           e.Graphics.DrawString("Date Signed: ", df6, Brushes.Black, 500, 602);
           //e.Graphics.DrawString("_________________________", df6, Brushes.Black, 236, 589);
           //e.Graphics.DrawString("____________________", df6, Brushes.Black, 158, 609);

         
          



            
    
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
           Font df5 = new Font("Arial", 10, FontStyle.Bold);
           Font df6 = new Font("Arial", 10, FontStyle.Regular);
           Font df0 = new Font("Arial", 8, FontStyle.Regular);
           SolidBrush drawBrush = new SolidBrush(Color.Black);

           // Create a new pen.
           Pen pen1 = new Pen(Brushes.Black);
           pen1.Width = 1F;
           pen1.LineJoin = System.Drawing.Drawing2D.LineJoin.Miter;

           
           
           

           //REPORT'S HEADER

           e.Graphics.Clear(Color.White);

           Rectangle r = new Rectangle(50, 40, 100, 95);
           Image newImage = Image.FromFile(@"C:\Users\valued client\Documents\Visual Studio 2010\Projects\1 - THESIS\berlyn.bmp");
           e.Graphics.DrawImage(newImage, r);

           e.Graphics.DrawString("Berlyn Academy", df1, Brushes.Black, 450, 50, sf);
           e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df0, Brushes.Black, 450, 70, sf);
           e.Graphics.DrawString("Recognition Nos. E-089 / E-110 / S-002", df0, Brushes.Black, 450, 85, sf);
           e.Graphics.DrawString("Email Address: berlynacademy@yahoo.com", df0, Brushes.Black, 450, 100, sf);
           e.Graphics.DrawString("REGISTRATION FORM", df4, Brushes.Black, 450, 118, sf);

           e.Graphics.DrawRectangle(pen1, 50, 140, 750, 390);
           e.Graphics.DrawString("Student no. ", df5, Brushes.Black, 60, 142, sf1);
           e.Graphics.DrawString(txtSnum.Text, df6, Brushes.Black, 175, 142, sf1);
           //horizontal line
           e.Graphics.DrawLine(pen1, 50, 161, 800, 161);
           e.Graphics.DrawLine(pen1, 50, 182, 800, 182);
           e.Graphics.DrawLine(pen1, 50, 203, 800, 203);
           //vertical line
           e.Graphics.DrawLine(pen1, 172, 140, 172, 161);//sno 
           e.Graphics.DrawLine(pen1, 272, 140, 272, 161);//sy title
           e.Graphics.DrawLine(pen1, 392, 140, 392, 161);//sy
           e.Graphics.DrawLine(pen1, 517, 140, 517, 161);//mop title
           e.Graphics.DrawLine(pen1, 673, 140, 673, 161);//mop

           e.Graphics.DrawLine(pen1, 132, 161, 132, 182);//lev
           e.Graphics.DrawLine(pen1, 247, 161, 247, 182);//sec title
           e.Graphics.DrawLine(pen1, 337, 161, 337, 182);//sec
           e.Graphics.DrawLine(pen1, 457, 161, 457, 182);//adv title
           e.Graphics.DrawLine(pen1, 547, 161, 547, 182);//adv

           e.Graphics.DrawLine(pen1, 272, 182, 272, 225);
           e.Graphics.DrawLine(pen1, 547, 182, 547, 225);//mn

          
           e.Graphics.DrawRectangle(pen1, 50, 225, 750, 25);
           e.Graphics.DrawString("ACADEMIC", df5, Brushes.Black, 400, 229);
         

           e.Graphics.DrawString("School year ", df5, Brushes.Black, 275, 142, sf1);
           e.Graphics.DrawString(txtSY.Text, df6, Brushes.Black, 395, 142, sf1);

           e.Graphics.DrawString("Lastname ", df5, Brushes.Black, 60, 184, sf1);
           e.Graphics.DrawString(txtLast.Text, df6, Brushes.Black, 60, 206, sf1);

           e.Graphics.DrawString("Firstname ", df5, Brushes.Black, 275, 184, sf1);
           e.Graphics.DrawString(txtFirst.Text, df6, Brushes.Black, 275, 206, sf1);

           e.Graphics.DrawString("Middlename ", df5, Brushes.Black, 550, 184, sf1);
           e.Graphics.DrawString(txtMid.Text, df6, Brushes.Black, 550, 206, sf1);

           e.Graphics.DrawString("Level ", df5, Brushes.Black, 60, 163, sf1);
           e.Graphics.DrawString(txtGrd.Text, df6, Brushes.Black, 135, 163, sf1);

           e.Graphics.DrawString("Section ", df5, Brushes.Black, 250, 163, sf1);
           e.Graphics.DrawString(txtSec.Text, df6, Brushes.Black, 340, 163, sf1);

           e.Graphics.DrawString("Adviser ", df5, Brushes.Black, 460, 163, sf1);
           e.Graphics.DrawString(txtAdviser.Text, df6, Brushes.Black, 550, 163, sf1);
         


           e.Graphics.DrawString("Mode of Payment ", df5, Brushes.Black, 520, 142, sf1);
           e.Graphics.DrawString(txtMOP.Text, df6, Brushes.Black, 675, 142, sf1);

           e.Graphics.DrawRectangle(Pens.Black, 50, 250, 750, 25);
           e.Graphics.FillRectangle(Brushes.White, 51, 251, 749, 25);//sched sub title
           e.Graphics.DrawString("          Subject                                Faculty                            Room                         Time                            Day", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 55, 254);
           e.Graphics.DrawRectangle(Pens.Black, 50, 275, 750, 255);
           e.Graphics.FillRectangle(Brushes.White, 51, 275, 749, 255);
          

           //e.Graphics.DrawLine(Pens.Black,160,250,520,0);       lochead ,start           loctail,height
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(200, 250), new Point(200, 506));
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(410, 250), new Point(410, 506));
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(510, 250), new Point(510, 506));
           e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(690, 250), new Point(690, 506));

           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from schedule_tbl where level='"+txtGrd.Text+"'and section='"+txtSec.Text+"'", con);
           DataTable dt = new DataTable();//where acadyr status is active
           da.Fill(dt);
           con.Close();
           DataView dvvla = new DataView(dt);

           int locy = 265;
           int locx = 60;

           if(dt.Rows.Count>0)
           {
               int fontsize = 0;
               int cntsub = Convert.ToInt32(dt.Rows.Count);
               if (cntsub >= 15)
               {
                   fontsize = 8;
               }
               else
               {
                   fontsize = 10;
               }
               for (int x = 0; x < dt.Rows.Count; x++)
               {
      
                   e.Graphics.DrawString(dt.Rows[x].ItemArray[3].ToString(), new Font("Arial", fontsize, FontStyle.Regular), Brushes.Black, locx, locy += 16);
                   //e.Graphics.DrawString(schedcode, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, locx += 247, locy);
                   e.Graphics.DrawString(dt.Rows[x].ItemArray[4].ToString(), new Font("Arial", fontsize, FontStyle.Regular), Brushes.Black, locx += 155, locy);
                   //e.Graphics.DrawString("  - " + dt.Rows[x].ItemArray[2].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, locx += 55, locy);
                   e.Graphics.DrawString(dt.Rows[x].ItemArray[5].ToString(), new Font("Arial", fontsize, FontStyle.Regular), Brushes.Black, locx += 210, locy);
                   e.Graphics.DrawString(dt.Rows[x].ItemArray[6].ToString() + " - " + dt.Rows[x].ItemArray[7].ToString(), new Font("Arial", fontsize, FontStyle.Regular), Brushes.Black, locx += 100, locy);
                   //e.Graphics.DrawString(dt.Rows[x].ItemArray[7].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, locx += 125, locy);
                   e.Graphics.DrawString(dt.Rows[x].ItemArray[8].ToString(), new Font("Arial", fontsize, FontStyle.Regular), Brushes.Black, locx += 180, locy);
                   locx = 60;

               }
           
           }

           

           e.Graphics.DrawRectangle(Pens.Black, 50, 531, 455, 420);
           e.Graphics.FillRectangle(Brushes.White, 51, 531, 453, 419);
           e.Graphics.DrawRectangle(pen1, 50, 506, 750, 25);
           e.Graphics.DrawString("FINANCIAL", df5, Brushes.Black, 400, 510);
           //title assessment
           e.Graphics.DrawRectangle(Pens.Black, 50, 531, 455, 25);
           e.Graphics.FillRectangle(Brushes.White, 51, 531, 454, 25);//assessment fill
           e.Graphics.DrawString("ASSESSMENT", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 60, 535);

           string levelkey="";
      
           con.Open();
           OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtGrd.Text + "'", con);
           DataTable dtdep = new DataTable();
           dadep.Fill(dtdep);
           con.Close();
           if (dtdep.Rows.Count > 0)
           {
               levelkey = dtdep.Rows[0].ItemArray[0].ToString();
           }

           int startAssessment=566;
            con.Open();
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and type='fee' and SY='" + activeSY + "'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            con.Close();

            if (dt1.Rows.Count > 0)
            {

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i].ItemArray[1].ToString().Contains("TUITION FEE") == true)
                    {
                        e.Graphics.DrawString(dt1.Rows[i].ItemArray[1].ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 60, startAssessment);
                        e.Graphics.DrawString("P " + dt1.Rows[i].ItemArray[2].ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment);
                    }
                }

                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (dt1.Rows[j].ItemArray[1].ToString().Contains("REGISTRATION") == true)
                    {
                        e.Graphics.DrawString(dt1.Rows[j].ItemArray[1].ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 60, startAssessment += 19);
                        e.Graphics.DrawString("P " + dt1.Rows[j].ItemArray[2].ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment);
                      
                      
                        con.Open();
                        OdbcDataAdapter da11 = new OdbcDataAdapter("Select*from registrationfee_tbl where level='" + levelkey + "'and SY='" + activeSY + "'order by fee ASC", con);
                        DataTable dt11 = new DataTable();
                        da11.Fill(dt11);
                        con.Close();

                        if (dt11.Rows.Count > 0)
                        {
                            int fontsize = 0;
                            if (dt11.Rows.Count >3)
                            {
                                fontsize = 6;
                                startAssessment += 2;
                            }
                            else
                            {
                                fontsize = 8;
                                startAssessment += 3;
                            }

                            for (int h = 0; h < dt11.Rows.Count; h++)
                            {
                                e.Graphics.DrawString(dt11.Rows[h].ItemArray[1].ToString(), new Font("Arial", fontsize, FontStyle.Regular), Brushes.Black, 100, startAssessment += 19);
                                e.Graphics.DrawString("P " + dt11.Rows[h].ItemArray[2].ToString(), new Font("Arial", fontsize, FontStyle.Regular), Brushes.Black, 310, startAssessment);
                            }

                        }

                    }
                }

                for (int k = 0; k < dt1.Rows.Count; k++)
                {
                    if (dt1.Rows[k].ItemArray[1].ToString().Contains("MISCELLANEOUS") == true)
                    {
                        e.Graphics.DrawString(dt1.Rows[k].ItemArray[1].ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 60, startAssessment += 19);
                        e.Graphics.DrawString("P " + dt1.Rows[k].ItemArray[2].ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment);

                        con.Open();
                        OdbcDataAdapter da3 = new OdbcDataAdapter("Select*from miscellaneousfee_tbl where level='" + levelkey + "'and SY='" + activeSY + "'order by fee ASC", con);
                        DataTable dt3 = new DataTable();
                        da3.Fill(dt3);
                        con.Close();

                        if (dt3.Rows.Count > 0)
                        {
                            int fontsize = 0;
                            if (dt3.Rows.Count >= 10)
                            {
                                fontsize = 6;
                                startAssessment += 2;
                            }
                            else
                            {
                                fontsize = 8;
                                startAssessment += 3;
                            }
                            for (int h = 0; h < dt3.Rows.Count; h++)
                            {
                                e.Graphics.DrawString(dt3.Rows[h].ItemArray[1].ToString(), new Font("Arial", fontsize, FontStyle.Regular), Brushes.Black, 100, startAssessment += 19);
                                e.Graphics.DrawString("P " + dt3.Rows[h].ItemArray[2].ToString(), new Font("Arial", fontsize, FontStyle.Regular), Brushes.Black, 310, startAssessment);
                            }

                        }

                    }
                }

                con.Open();
                OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and fee<>'TUITION FEE'and fee<>'REGISTRATION'and fee<>'MISCELLANEOUS'and type<>'payment'and SY='" + activeSY + "'", con);
                DataTable dt01 = new DataTable();
                da01.Fill(dt01);
                con.Close();
                if (dt01.Rows.Count > 0)
                {
                    for (int i = 0; i < dt01.Rows.Count; i++)
                    {
                        e.Graphics.DrawString(dt01.Rows[i].ItemArray[1].ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 60, startAssessment += 19);
                        e.Graphics.DrawString("P " + dt01.Rows[i].ItemArray[2].ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment);

                    }
                }

              


                string totalAss = "";
                //TOTAL ASSESSMENT DISPLAY -------------------------------------------------------------------------------------------------------------
                con.Open();
                OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and fee<>'TUITION FEE'and fee<>'REGISTRATION'and fee<>'MISCELLANEOUS'and fee='ANNUAL PAYMENT' and SY='" + activeSY + "'", con);
                DataTable dt0 = new DataTable();
                da0.Fill(dt0);
                con.Close();
                if (dt0.Rows.Count > 0)
                {
                    for (int i = 0; i < dt0.Rows.Count; i++)
                    {
                        totalAss = dt0.Rows[i].ItemArray[2].ToString();

                    }
                }
                //SUBTOTAL
                e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment += 17);
                e.Graphics.DrawString("Sub-Total", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 60, startAssessment += 18);
                e.Graphics.DrawString("P " + totalAss, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment);
              
                //FOR DISCOUNT DISPLAY
                //--------------------------------FOR DISCOUNT
                string discountedAmtDisp = "";
                string discTotalAssDisp = "";

                if (txtGrd.Text == "Kinder")
                {
                    if (lblAssesDiscount.Text=="None")
                    {
                        e.Graphics.DrawString("Less: ", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P 0.00", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);
                    }

                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == true) || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true))
                    {
                        double monthlyAmtK = Convert.ToDouble(monthlyamount_K);
                        if (monthlyAmtK >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyAmtK));
                        }
                        if (monthlyAmtK < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyAmtK));
                        }

                        double freelastmonthK = Convert.ToDouble(FreeLastMonthTotal_K);
                        if (freelastmonthK >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(freelastmonthK));
                        }
                        if (freelastmonthK < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(freelastmonthK));
                        }

                        string lessDisplay = "";
                        if (TheSiblingProvider != "")
                        {
                            lessDisplay = lblAssesDiscount.Text + " - " + TheSiblingProvider;
                        }
                        else
                        {
                            lessDisplay = lblAssesDiscount.Text;
                        }

                        e.Graphics.DrawString("Less: " + "(" + lessDisplay + ")", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + discountedAmtDisp, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);
                       
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("Second") == true) || lblAssesDiscount.Text.Contains("2nd") == true))
                    {
                        double lessAmtK = Convert.ToDouble(LessAmt_K);
                        if (lessAmtK >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(lessAmtK));
                        }
                        if (lessAmtK < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(lessAmtK));
                        }

                        double fiftydisctotalk = Convert.ToDouble(fiftyDiscTotal_K);
                        if (fiftydisctotalk >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftydisctotalk));
                        }
                        if (fiftydisctotalk < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftydisctotalk));
                        }

                        e.Graphics.DrawString("Less: " + "(" + lblAssesDiscount.Text + ")", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + discountedAmtDisp, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                    {
                        double discamtother = Convert.ToDouble(discountedAmtOtherDisc);
                        if (discamtother >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discamtother));
                        }
                        if (discamtother < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discamtother));
                        }

                        double disctotalOther = Convert.ToDouble(discountedTotalOtherDisc);
                        if (disctotalOther >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(disctotalOther));
                        }
                        if (disctotalOther < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(disctotalOther));
                        }

                        e.Graphics.DrawString("Less: " + "(" + lblAssesDiscount.Text + ")", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + discountedAmtDisp, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);

                    }
                }
                else if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                {
                    if (lblAssesDiscount.Text=="None")
                    {
                        e.Graphics.DrawString("Less: ", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + "0.00", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 410, startAssessment);
                    }

                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == true) || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true))
                    {
                        double monthlyAmtJ = Convert.ToDouble(monthlyamount_J);
                        if (monthlyAmtJ >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyAmtJ));
                        }
                        if (monthlyAmtJ < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyAmtJ));
                        }

                        double flm_tot = Convert.ToDouble(FreeLastMonthTotal_J);
                        if (flm_tot >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(flm_tot));
                        }
                        if (flm_tot < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(flm_tot));
                        }

                        string lessDisplay = "";
                        if (TheSiblingProvider != "")
                        {
                            lessDisplay = lblAssesDiscount.Text + " - " + TheSiblingProvider;
                        }
                        else
                        {
                            lessDisplay = lblAssesDiscount.Text;
                        }

                        e.Graphics.DrawString("Less: " + "(" + lessDisplay + ")", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + discountedAmtDisp, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("Second") == true) || lblAssesDiscount.Text.Contains("2nd") == true))
                    {
                        double lessAmtJ = Convert.ToDouble(LessAmt_J);
                        if (lessAmtJ >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(lessAmtJ));
                        }
                        if (lessAmtJ < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(lessAmtJ));
                        }

                        double fiftydisctotalj = Convert.ToDouble(fiftyDiscTotal_J);
                        if (fiftydisctotalj >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftydisctotalj));
                        }
                        if (fiftydisctotalj < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftydisctotalj));
                        }

                        e.Graphics.DrawString("Less: " + "(" + lblAssesDiscount.Text + ")", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + discountedAmtDisp, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);

                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                    {
                        double discamtother = Convert.ToDouble(discountedAmtOtherDisc);
                        if (discamtother >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discamtother));
                        }
                        if (discamtother < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discamtother));
                        }

                        double disctotalOther = Convert.ToDouble(discountedTotalOtherDisc);
                        if (disctotalOther >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(disctotalOther));
                        }
                        if (disctotalOther < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(disctotalOther));
                        }


                        e.Graphics.DrawString("Less: " + "(" + lblAssesDiscount.Text + ")", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + discountedAmtDisp, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);
                    }
                }
                else
                {
                    if (lblAssesDiscount.Text=="None")
                    {
                        e.Graphics.DrawString("Less: ", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + "0.00", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);
                       
                    }

                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == true) || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true))
                    {
                        double monthlyAmtE = Convert.ToDouble(monthlyamount_E);
                        if (monthlyAmtE >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyAmtE));
                        }
                        if (monthlyAmtE < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyAmtE));
                        }

                        double flm_tot = Convert.ToDouble(FreeLastMonthTotal_E);
                        if (flm_tot >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(flm_tot));
                        }
                        if (flm_tot < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(flm_tot));
                        }

                        string lessDisplay = "";
                        if (TheSiblingProvider != "")
                        {
                            lessDisplay = lblAssesDiscount.Text + " - " + TheSiblingProvider;
                        }
                        else
                        {
                            lessDisplay = lblAssesDiscount.Text;
                        }

                        e.Graphics.DrawString("Less: " + "(" + lessDisplay + ")", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + discountedAmtDisp, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("Second") == true) || lblAssesDiscount.Text.Contains("2nd") == true))
                    {
                        double lessAmtE = Convert.ToDouble(LessAmt_E);
                        if (lessAmtE >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(lessAmtE));
                        }
                        if (lessAmtE < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(lessAmtE));
                        }

                        double fiftydisctotale = Convert.ToDouble(fiftyDiscTotal_E);
                        if (fiftydisctotale >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftydisctotale));
                        }
                        if (fiftydisctotale < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftydisctotale));
                        }

                        e.Graphics.DrawString("Less: " + "(" + lblAssesDiscount.Text + ")", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + discountedAmtDisp, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);
                    }
                    if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
                    {
                        double discamtother = Convert.ToDouble(discountedAmtOtherDisc);
                        if (discamtother >= 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discamtother));
                        }
                        if (discamtother < 1000)
                        {
                            discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discamtother));
                        }

                        double disctotalOther = Convert.ToDouble(discountedTotalOtherDisc);
                        if (disctotalOther >= 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(disctotalOther));
                        }
                        if (disctotalOther < 1000)
                        {
                            discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(disctotalOther));
                        }

                        e.Graphics.DrawString("Less: "+"("+lblAssesDiscount.Text+")", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, startAssessment += 20);
                        e.Graphics.DrawString("P " + discountedAmtDisp, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment);
                    }
                }

                //DISPLAY TOTAL ASSESSMENT
                if (lblAssesDiscount.Text!="None")
                {
                    e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment += 7);
                    e.Graphics.DrawString("Total Assessment:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 60, startAssessment += 20);
                    e.Graphics.DrawString("P " + discTotalAssDisp, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment);
                    e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment += 4);
                    e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment += 3);
                }
                else
                {
                    e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 380, startAssessment += 7);
                    e.Graphics.DrawString("Total Assessment:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 60, startAssessment += 20);
                    e.Graphics.DrawString("P " + totalAss, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment);
                    e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment += 4);
                    e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 380, startAssessment += 3);
                }
              
            }//end dt.rows
        

           //payment sched rf
           //Payment Schedule Display----------------------------------------------------------------------------
            e.Graphics.DrawRectangle(Pens.Black, 465, 530, 335, 421);
            e.Graphics.FillRectangle(Brushes.White, 466, 531, 231, 418);
            e.Graphics.DrawRectangle(Pens.Black, 465, 530, 335, 25);
            e.Graphics.FillRectangle(Brushes.White, 466, 531, 334, 25);//payment sched fill
            e.Graphics.DrawString("PAYMENT SCHEDULE", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, 536);
            e.Graphics.DrawString("PAYMENTS", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, 566);
            e.Graphics.DrawString("DATE DUE", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 630, 566);
            e.Graphics.DrawString("AMOUNT", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, 566);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(50, 275), new Point(800, 275));
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(50, 556), new Point(800, 556));
            int startsched = 585;
            
            if (txtGrd.Text == "Kinder")
            {
                if (lblAssessMode.Text == "Cash")
                {
                    setupDateRegistered_Cash();
                    if (lblAssesDiscount.Text != "None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(anuualamt_freelastmonthK);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                            e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 7);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(annualamt_fiftydiscK);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                            e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 7);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }
                        else
                        {
                            double _amt = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                            e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 7);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }
                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_K);
                        string amt_dis = "";
                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(annualamount_K));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(annualamount_K));
                        }

                        e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                        e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 7);
                        e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                        e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                    }
                }
                if (lblAssessMode.Text == "Installment")
                {
                    setupDateRegistered_Installment();
                    //stopped
                    e.Graphics.DrawString("UPON ENROLLMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                    e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                    e.Graphics.DrawString("P " + uponamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                    if (lblAssesDiscount.Text != "None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(FreeLastMonthTotal_K);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString("P 0.00", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);

                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(fiftyDiscTotal_K);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P "+fiftyDisc_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                          
                        }
                        else
                        {

                            double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_tot_dis = "";
                            string amt_monthlyIns_OtherDisc = "";
                            double uponamt = Convert.ToDouble(uponamount_K);
                            double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                            double monthlyInstallmentAmt_forOtherDisc = amt_deductUpon / 9;

                            if (_amt_tot >= 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt_tot));
                            }
                            if (_amt_tot < 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt_tot));
                            }

                            //-------------
                            if (monthlyInstallmentAmt_forOtherDisc >= 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }
                            if (monthlyInstallmentAmt_forOtherDisc < 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }


                            e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_tot_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }
                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_K);
                        string amt_dis = "";

                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }


                        e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_K, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                        e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                        e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                    }


                    //PAYMENT SCHEDULE MODIFY FOR DISCOUNT
                }
            }
            if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" ||
                txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
            {//here
               
                con.Open();
                OdbcDataAdapter daelem = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and SY='" + activeSY + "'", con);
                DataTable dtelem = new DataTable();
                daelem.Fill(dtelem);
                con.Close();

                if (dtelem.Rows.Count > 0)
                {
                    for (int a = 0; a < dtelem.Rows.Count; a++)
                    {
                        if (dtelem.Rows[a].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                        {
                            annualamount_E = dtelem.Rows[a].ItemArray[2].ToString();
                            
                        }
                        if (dtelem.Rows[a].ItemArray[1].ToString() == "UPON ENROLLMENT")
                        {
                            double _uponE = Convert.ToDouble(dtelem.Rows[a].ItemArray[2].ToString());
                           
                            if (_uponE >= 1000)
                            {
                                uponamount_E = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_uponE));
                            }
                            if (_uponE < 1000)
                            {
                                uponamount_E = String.Format(("{0:0.00#}"), Convert.ToDouble(_uponE));
                            }    
                        }
                        if (dtelem.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                        {
                            double _montE = Convert.ToDouble(dtelem.Rows[a].ItemArray[2].ToString());

                            if (_montE >= 1000)
                            {
                                monthlyamount_E = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_montE));
                            }
                            if (_montE < 1000)
                            {
                                monthlyamount_E = String.Format(("{0:0.00#}"), Convert.ToDouble(_montE));
                            }   
                        }
                    }
                }

                if (lblAssessMode.Text== "Cash")
                {
                    setupDateRegistered_Cash();

                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(anuualamt_freelastmonthE);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }


                            e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                            e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 7);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);

                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(annualamt_fiftydiscE);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                            e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 7);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);

                        }
                        else
                        {
                            double _amt = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                            e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 7);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }

                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_E);
                        string amt_dis = "";
                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }

                        e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                        e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 7);
                        e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                        e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                    }
                }
                if (lblAssessMode.Text == "Installment")
                {
                    setupDateRegistered_Installment();

                    e.Graphics.DrawString("UPON ENROLLMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                    e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                    e.Graphics.DrawString("P " + uponamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(FreeLastMonthTotal_E);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString("P 0.00", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(fiftyDiscTotal_E);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P "+fiftyDisc_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);

                        }
                        else
                        {
                            double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_tot_dis = "";
                            string amt_monthlyIns_OtherDisc = "";
                            double uponamt = Convert.ToDouble(uponamount_E);
                            double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                            double monthlyInstallmentAmt_forOtherDisc = amt_deductUpon / 9;

                            if (_amt_tot >= 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt_tot));
                            }
                            if (_amt_tot < 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt_tot));
                            }

                            //-------------
                            if (monthlyInstallmentAmt_forOtherDisc >= 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }
                            if (monthlyInstallmentAmt_forOtherDisc < 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }

                            e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black,475, startsched += 19);
                            e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_tot_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }
                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_E);
                        string amt_dis = "";

                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }

                        e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_E, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                        e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                        e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                    }
                }
            }
            if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" ||txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
            {
                con.Open();
                OdbcDataAdapter dajunior = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and SY='" + activeSY + "'", con);
                DataTable dtjunior = new DataTable();
                dajunior.Fill(dtjunior);
                con.Close();

                if (dtjunior.Rows.Count > 0)
                {
                    for (int a = 0; a < dtjunior.Rows.Count; a++)
                    {
                        if (dtjunior.Rows[a].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                        {
                            annualamount_J = dtjunior.Rows[a].ItemArray[2].ToString();
                        }
                        if (dtjunior.Rows[a].ItemArray[1].ToString() == "UPON ENROLLMENT")
                        {
                            double _uponJ = Convert.ToDouble(dtjunior.Rows[a].ItemArray[2].ToString());

                            if (_uponJ >= 1000)
                            {
                                uponamount_J = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_uponJ));
                            }
                            if (_uponJ < 1000)
                            {
                                uponamount_J = String.Format(("{0:0.00#}"), Convert.ToDouble(_uponJ));
                            }
                        }
                        if (dtjunior.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                        {
                            double _montJ = Convert.ToDouble(dtjunior.Rows[a].ItemArray[2].ToString());

                            if (_montJ >= 1000)
                            {
                                monthlyamount_J = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_montJ));
                            }
                            if (_montJ < 1000)
                            {
                                monthlyamount_J = String.Format(("{0:0.00#}"), Convert.ToDouble(_montJ));
                            }

                        }
                    }
                }


                if (lblAssessMode.Text=="Cash")
                {
                    setupDateRegistered_Cash();
                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {

                            double _amt = Convert.ToDouble(anuualamt_freelastmonthJ);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                            e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched +=9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(annualamt_fiftydiscJ);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                            e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);

                        }
                        else
                        {
                            double _amt = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                            e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }

                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_J);
                        string amt_dis = "";
                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }

                        e.Graphics.DrawString("FULL PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                        e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                        e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                        e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                    }
                }
                if (lblAssessMode.Text == "Installment")
                {
                    setupDateRegistered_Installment();

                    e.Graphics.DrawString("UPON ENROLLMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched);
                    e.Graphics.DrawString(today, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                    e.Graphics.DrawString("P " + uponamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                    if (lblAssesDiscount.Text!="None")
                    {
                        if (lblAssesDiscount.Text.Contains("siblings") == true || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true)
                        {
                            double _amt = Convert.ToDouble(FreeLastMonthTotal_J);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString("P 0.00", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 630, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);

                        }
                        else if (lblAssesDiscount.Text.Contains("Second") == true || lblAssesDiscount.Text.Contains("2nd") == true)
                        {
                            double _amt = Convert.ToDouble(fiftyDiscTotal_J);
                            string amt_dis = "";
                            if (_amt >= 1000)
                            {
                                amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                            }
                            if (_amt < 1000)
                            {
                                amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                            }

                            e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P "+fiftyDisc_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);
                        }
                        else
                        {
                            double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                            string amt_tot_dis = "";
                            string amt_monthlyIns_OtherDisc = "";
                            double uponamt = Convert.ToDouble(uponamount_J);
                            double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                            double monthlyInstallmentAmt_forOtherDisc = amt_deductUpon / 9;

                            if (_amt_tot >= 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt_tot));
                            }
                            if (_amt_tot < 1000)
                            {
                                amt_tot_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt_tot));
                            }

                            //-------------
                            if (monthlyInstallmentAmt_forOtherDisc >= 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }
                            if (monthlyInstallmentAmt_forOtherDisc < 1000)
                            {
                                amt_monthlyIns_OtherDisc = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyInstallmentAmt_forOtherDisc));
                            }


                            e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                            e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                            e.Graphics.DrawString("P " + amt_monthlyIns_OtherDisc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                            e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                            e.Graphics.DrawString("P " + amt_tot_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                            e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);

                        }
                    }
                    else
                    {
                        double _amt = Convert.ToDouble(annualamount_J);
                        string amt_dis = "";

                        if (_amt >= 1000)
                        {
                            amt_dis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(_amt));
                        }
                        if (_amt < 1000)
                        {
                            amt_dis = String.Format(("{0:0.00#}"), Convert.ToDouble(_amt));
                        }

                        e.Graphics.DrawString("2ND PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(secpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("3RD PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(thipay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("4TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(foupay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("5TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(fifpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("6TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(sixpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("7TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(sevpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("8TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(eigpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("9TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(ninpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("10TH PAYMENT", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 475, startsched += 19);
                        e.Graphics.DrawString(tenpay, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 630, startsched);
                        e.Graphics.DrawString("P " + monthlyamount_J, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched);

                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 720, startsched += 9);
                        e.Graphics.DrawString("Total:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 475, startsched += 20);
                        e.Graphics.DrawString("P " + amt_dis, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 4);
                        e.Graphics.DrawString("__________", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 720, startsched += 3);

                    }
                }
            }

            e.Graphics.DrawString("Printed Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 611, 1017);
            //co may replace to signatoryregistrar
            e.Graphics.DrawRectangle(Pens.Black, 50, 951, 750, 25);
            e.Graphics.DrawRectangle(Pens.Black, 50, 976, 750, 24);
            e.Graphics.DrawLine(Pens.Black, 260, 951, 260, 1000);
            e.Graphics.DrawLine(Pens.Black, 530, 951, 530, 976);
            e.Graphics.DrawLine(Pens.Black, 660, 951, 660, 976);
            e.Graphics.DrawString(co, df6, Brushes.Black, 275, 955);
            e.Graphics.DrawString("Validated & Signed by:", df5, Brushes.Black, 60, 955, sf1);
            e.Graphics.DrawString("Date Signed:", df5, Brushes.Black, 540, 955, sf1);
            e.Graphics.DrawString("Parent/Guardian Signature:", df5, Brushes.Black, 60, 980, sf1);
       }

       public void setupRegistrars()
       {
           con.Open();
           OdbcDataAdapter das = new OdbcDataAdapter("Select (concat(firstname,' ',middlename,' ',lastname))from employees_tbl where position='registrar'", con);
           DataTable dts = new DataTable();
           das.Fill(dts);
           con.Close();
           if (dts.Rows.Count > 0)
           {
               cmbRegi.Items.Clear();

               for (int y = 0; y < dts.Rows.Count; y++)
               {
                   cmbRegi.Items.Add(dts.Rows[y].ItemArray[0].ToString());
               }
           }
       }

       public void setupSOA()
       {
            pdSOA.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintSOA);
            ppcSOA.Document = pdSOA;
            pprevSOA.Document = pdSOA;
            pprevSOA.Document = pdSOA;
 
       }

       public void setupRFContentAndAdviser()
       {
           con.Open();
           OdbcDataAdapter das = new OdbcDataAdapter("Select*from stud_tbl where studno='" + txtSnum.Text + "'", con);
           DataTable dts = new DataTable();
           das.Fill(dts);
           con.Close();

           if (dts.Rows.Count > 0)
           {
               pdRF.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintRF);
               ppcRF.Document = pdRF;
               pprevDlg.Document = pdRF;


               if (txtGrd.Text != "" && txtSec.Text != "")
               {
                   con.Open();
                   OdbcDataAdapter da = new OdbcDataAdapter("Select (concat(firstname,' ',middlename,' ',lastname))from employees_tbl where grade='" + txtGrd.Text + "'and advisory='" + txtSec.Text + "'", con);
                   DataTable dt = new DataTable();
                   da.Fill(dt);
                   con.Close();

                   if (dt.Rows.Count > 0)
                   {
                       txtAdviser.Text = dt.Rows[0].ItemArray[0].ToString();
                   }
                   else
                   {
                       txtAdviser.Text = "";
                   }
               }
               else
               {
                   txtAdviser.Text = "";
               }

           }
       }

       private void btnPrintRF_Click(object sender, EventArgs e)
       {
           pdlgPrint.ShowDialog();
       }

       private void btnAdmission_Click(object sender, EventArgs e)
       {
           frmAdmission admform = new frmAdmission();
           this.Hide();
           admform.admlog = asslog;
           admform.TheFaculty = asslog;
           admform.Show();
       }

       private void txtSearch_TextChanged(object sender, EventArgs e)
       {
           if (cmbFilter.Text=="Student number")
           {
               dv.RowFilter = string.Format("No LIKE '%{0}%'", txtSearch.Text);
               dgvSearch.DataSource = dv;
           }
           if (cmbFilter.Text == "Student's name")
           {
               dv.RowFilter = string.Format("Student LIKE '%{0}%'", txtSearch.Text);
               dgvSearch.DataSource = dv;
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

       private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
       {
           /*char ch = e.KeyChar;
           if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
           {
               e.Handled = true;
           }*/
       }

       private void lvwSG_SelectedIndexChanged(object sender, EventArgs e)
       {

       }

       private void dgvm_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
       {
           dgvm.Cursor = Cursors.Hand;
       }

       private void dgvm_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
       {
           dgvm.Cursor = Cursors.Default;
           if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Student records")
           {
               dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
           }
       }

       private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
       {
           if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Student records")
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

       private void cmbRegi_SelectedIndexChanged(object sender, EventArgs e)
       {
           setupRFContentAndAdviser();
           signatoryRegistrar = cmbRegi.Text;
       }

       private void btnPrev_Click(object sender, EventArgs e)
       {
           pprevDlg.FindForm().StartPosition = FormStartPosition.CenterScreen;
           pprevDlg.FindForm().Size = new System.Drawing.Size(1000, 640);
           pprevDlg.FindForm().Text = "Print preview - Registration form";
           pprevDlg.ShowDialog();
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

       private void label22_Click(object sender, EventArgs e)
       {

       }

       private void btnPrevSOA_Click(object sender, EventArgs e)
       {
           pprevSOA.FindForm().StartPosition = FormStartPosition.CenterScreen;
           pprevSOA.FindForm().Size = new System.Drawing.Size(1000, 640);
           pprevSOA.FindForm().Text = "Print preview - Statement of Account";
           pprevSOA.ShowDialog();
       }

       private void txtSOASrc_TextChanged(object sender, EventArgs e)
       {
           if (cmbFilterSOA.Text == "Student number")
           {
               dvSOA.RowFilter = string.Format("No LIKE '%{0}%'", txtSOASrc.Text);
               dgvSrc2.DataSource = dvSOA;
           }
           if (cmbFilterSOA.Text == "Student's name")
           {
               dvSOA.RowFilter = string.Format("Student LIKE '%{0}%'", txtSOASrc.Text);
               dgvSrc2.DataSource = dvSOA;
           }

           if (dgvSrc2.Rows.Count > 0)
           {
               pnlSrc2.Visible = false;
           }
           if (dgvSrc2.Rows.Count == 0 && txtSOASrc.Text != "")
           {
               pnlSrc2.Visible = true;
               lblnoteSOA.Text = "0 search result";
           }
           if (dgvSrc2.Rows.Count == 0 && txtSOASrc.Text == "")
           {
               pnlSrc2.Visible = true;
               lblnoteSOA.Text = "no items found!";
           }
       }

       private void cmbFilterSOA_SelectedIndexChanged(object sender, EventArgs e)
       {
           if (cmbFilterSOA.Text == "Student number")
           {
               toolTip1.SetToolTip(txtSOASrc, "student number");
           }
           if (cmbFilterSOA.Text == "Student's name")
           {
               toolTip1.SetToolTip(txtSOASrc, "student's name");
           }
           txtSOASrc.Clear();
           txtSOASrc.Focus();
       }

       private void dgvSrc2_Click(object sender, EventArgs e)
       {
           if (dgvSrc2.Rows.Count <= 0)
           {
               return;
           }

           lblAssesDiscount.Visible = false;
           lblAssesLessAmt.Visible = false;
           lblAssessMode.Visible = false;
           string key = dgvSrc2.SelectedRows[0].Cells[0].Value.ToString();
         
           string levstud = "";

           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from offprereg_tbl where studno='" + key + "'", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {
               int CURRENTYR = Convert.ToInt32(DateTime.Now.Year.ToString());
               int UPCOMING = CURRENTYR + 1;
               string SY = CURRENTYR + "-" + UPCOMING;
               txtSY.Text = activeSY;
               txtSnum.Text = key;
               txtLast.Text = dt.Rows[0].ItemArray[3].ToString();
               txtFirst.Text = dt.Rows[0].ItemArray[1].ToString();
               txtMid.Text = dt.Rows[0].ItemArray[2].ToString();
               txtGrd.Text = dt.Rows[0].ItemArray[4].ToString();
               txtSec.Text = dt.Rows[0].ItemArray[5].ToString();
               txtMOP.Text = dt.Rows[0].ItemArray[22].ToString();

               levstud = txtGrd.Text;
           }
           else
           {
               con.Open();
               OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from offprereg_old_tbl where studno='" + key + "'", con);
               DataTable dt1 = new DataTable();
               da1.Fill(dt1);
               con.Close();

               if (dt1.Rows.Count > 0)
               {
                   int CURRENTYR = Convert.ToInt32(DateTime.Now.Year.ToString());
                   int UPCOMING = CURRENTYR + 1;
                   string SY = CURRENTYR + "-" + UPCOMING;
                   txtSY.Text = activeSY;
                   txtSnum.Text = key;
                   txtLast.Text = dt1.Rows[0].ItemArray[3].ToString();
                   txtFirst.Text = dt1.Rows[0].ItemArray[1].ToString();
                   txtMid.Text = dt1.Rows[0].ItemArray[2].ToString();
                   txtGrd.Text = dt1.Rows[0].ItemArray[4].ToString();
                   txtSec.Text = dt1.Rows[0].ItemArray[5].ToString();
                   txtMOP.Text = dt1.Rows[0].ItemArray[22].ToString();

                   levstud = txtGrd.Text;
               }
           }

           if (txtGrd.Text == "Kinder")
           {
               retrievedAssessmentKinder();
           }
           if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
           {
               retrievedAssessmentElem();

           }
           if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
           {
               retrievedAssessmentJunior();
           }


           //CHECK IF THE STUDENT HAS A DISCOUNT
           con.Open();
           OdbcDataAdapter da11 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + key + "'", con);
           DataTable dt11 = new DataTable();
           da11.Fill(dt11);
           con.Close();

           if (dt11.Rows.Count > 0)
           {
               lblAssesDiscount.Text = dt11.Rows[0].ItemArray[1].ToString();
               if (dt11.Rows[0].ItemArray[2].ToString()!="")
               {
                   con.Open();
                   OdbcDataAdapter da111 = new OdbcDataAdapter("Select*from stud_tbl where studno='" + dt11.Rows[0].ItemArray[2].ToString() + "'", con);
                   DataTable dt111 = new DataTable();
                   da111.Fill(dt111);
                   con.Close();
                   if (dt111.Rows.Count > 0)
                   {
                       TheSiblingProvider = dt111.Rows[0].ItemArray[3].ToString() + ", " + dt111.Rows[0].ItemArray[1].ToString() + " " + dt111.Rows[0].ItemArray[2].ToString();
                   }
               }
           }
           else
           {
               lblAssesDiscount.Text = "None";
               lblAssesLessAmt.Text = "P 0";
           }

           setupAssessmentLessAmt();//get less amount of theres discount
           //---------------------------------------------------------------------------------------------------
           //GET THE DISCOUNT AMOUNT
           if (txtGrd.Text == "Kinder")
           {
               retrievedAssessmentKinder();
               if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == true) || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true))
               {

                   double monthlyAmtK = Convert.ToDouble(monthlyamount_K);
                   if (monthlyAmtK >= 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyAmtK));
                   }
                   if (monthlyAmtK < 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyAmtK));
                   }

                   double freelastmonthK = Convert.ToDouble(FreeLastMonthTotal_K);
                   if (freelastmonthK >= 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(freelastmonthK));
                   }
                   if (freelastmonthK < 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(freelastmonthK));
                   }
               }
               if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("Second") == true) || lblAssesDiscount.Text.Contains("2nd") == true))
               {
                   double lessAmtK = Convert.ToDouble(LessAmt_K);
                   if (lessAmtK >= 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(lessAmtK));
                   }
                   if (lessAmtK < 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(lessAmtK));
                   }

                   double fiftydisctotalk = Convert.ToDouble(fiftyDiscTotal_K);
                   if (fiftydisctotalk >= 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftydisctotalk));
                   }
                   if (fiftydisctotalk < 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftydisctotalk));
                   }
               }
               if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
               {
                   con.Open();
                   OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + lblAssesDiscount.Text + "'", con);
                   DataTable dt1 = new DataTable();
                   da1.Fill(dt1);
                   con.Close();
                   if (dt1.Rows.Count > 0)
                   {
                       string rate = dt1.Rows[0].ItemArray[3].ToString();
                       if (rate.Substring(0, 1).ToString().Contains(".") == false)
                       {
                           rate = "." + rate;
                       }

                       double TF_amt = Convert.ToDouble(TFee_K);
                       double Reg_amt = Convert.ToDouble(Reg_K);
                       double Mis_amt = Convert.ToDouble(Mis_K);
                       double anlamt = Convert.ToDouble(annualamount_K);
                       double discrate = Convert.ToDouble(rate);
                       discountedAmtOtherDisc = TF_amt * discrate;
                       TF_amt -= discountedAmtOtherDisc;
                       discountedTotalOtherDisc = TF_amt+Reg_amt+Mis_amt;

                       double discamtother = Convert.ToDouble(discountedAmtOtherDisc);
                       if (discamtother >= 1000)
                       {
                           discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discamtother));
                       }
                       if (discamtother < 1000)
                       {
                           discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discamtother));
                       }


                       double disctotalOther = Convert.ToDouble(discountedTotalOtherDisc);
                       if (disctotalOther >= 1000)
                       {
                           discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(disctotalOther));
                       }
                       if (disctotalOther < 1000)
                       {
                           discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(disctotalOther));
                       }
                   }
               }
           }
           else if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
           {
               retrievedAssessmentJunior();
              
               if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == true) || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true))
               {
                   discountedAmtDisp = "";
                   double monthlyAmtJ = Convert.ToDouble(monthlyamount_J);
                   if (monthlyAmtJ >= 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyAmtJ));
                   }
                   if (monthlyAmtJ < 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyAmtJ));
                   }

                   double flm_tot = Convert.ToDouble(FreeLastMonthTotal_J);
                   if (flm_tot >= 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(flm_tot));
                   }
                   if (flm_tot < 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(flm_tot));
                   }
               }
               if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("Second") == true) || lblAssesDiscount.Text.Contains("2nd") == true))
               {
                   discountedAmtDisp = "";
                   double lessAmtJ = Convert.ToDouble(LessAmt_J);
                   if (lessAmtJ >= 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(lessAmtJ));
                   }
                   if (lessAmtJ < 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(lessAmtJ));
                   }

                   double fiftydisctotalj = Convert.ToDouble(fiftyDiscTotal_J);
                   if (fiftydisctotalj >= 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftydisctotalj));
                   }
                   if (fiftydisctotalj < 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftydisctotalj));
                   }
               }
               if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
               {
                   con.Open();
                   OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + lblAssesDiscount.Text + "'", con);
                   DataTable dt1 = new DataTable();
                   da1.Fill(dt1);
                   con.Close();
                   if (dt1.Rows.Count > 0)
                   {
                       string rate = dt1.Rows[0].ItemArray[3].ToString();
                       if (rate.Substring(0, 1).ToString().Contains(".") == false)
                       {
                           rate = "." + rate;
                       }

                       double TF_amt = Convert.ToDouble(TFee_J);
                       double Reg_amt = Convert.ToDouble(Reg_J);
                       double Mis_amt = Convert.ToDouble(Mis_J);
                       double anlamt = Convert.ToDouble(annualamount_J);
                       double discrate = Convert.ToDouble(rate);
                       discountedAmtOtherDisc = TF_amt * discrate;
                       TF_amt -= discountedAmtOtherDisc;
                       discountedTotalOtherDisc = TF_amt+Reg_amt+Mis_amt;

                       double discamtother = Convert.ToDouble(discountedAmtOtherDisc);
                       if (discamtother >= 1000)
                       {
                           discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discamtother));
                       }
                       if (discamtother < 1000)
                       {
                           discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discamtother));
                       }

                       double disctotalOther = Convert.ToDouble(discountedTotalOtherDisc);
                       if (disctotalOther >= 1000)
                       {
                           discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(disctotalOther));
                       }
                       if (disctotalOther < 1000)
                       {
                           discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(disctotalOther));
                       }
                   }
               }
           }
           else
           {
               retrievedAssessmentElem();
               if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == true) || lblAssesDiscount.Text.Contains("First") == true || lblAssesDiscount.Text.Contains("1st") == true))
               {
                   double monthlyAmtE = Convert.ToDouble(monthlyamount_E);
                   if (monthlyAmtE >= 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(monthlyAmtE));
                   }
                   if (monthlyAmtE < 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(monthlyAmtE));
                   }

                   double flm_tot = Convert.ToDouble(FreeLastMonthTotal_E);
                   if (flm_tot >= 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(flm_tot));
                   }
                   if (flm_tot < 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(flm_tot));
                   }
               }
               if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("Second") == true) || lblAssesDiscount.Text.Contains("2nd") == true))
               {
                   double lessAmtE = Convert.ToDouble(LessAmt_E);
                   if (lessAmtE >= 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(lessAmtE));
                   }
                   if (lessAmtE < 1000)
                   {
                       discountedAmtDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(lessAmtE));
                   }

                   double fiftydisctotale = Convert.ToDouble(fiftyDiscTotal_E);
                   if (fiftydisctotale >= 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(fiftydisctotale));
                   }
                   if (fiftydisctotale < 1000)
                   {
                       discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(fiftydisctotale));
                   }
               }
               if ((lblAssesDiscount.Text != "None") && ((lblAssesDiscount.Text.Contains("siblings") == false && lblAssesDiscount.Text.Contains("First") == false && lblAssesDiscount.Text.Contains("1st") == false && lblAssesDiscount.Text.Contains("Second") == false && lblAssesDiscount.Text.Contains("2nd") == false)))
               {
                   con.Open();
                   OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + lblAssesDiscount.Text + "'", con);
                   DataTable dt1 = new DataTable();
                   da1.Fill(dt1);
                   con.Close();
                   if (dt1.Rows.Count > 0)
                   {
                       string rate = dt1.Rows[0].ItemArray[3].ToString();
                       if (rate.Substring(0, 1).ToString().Contains(".") == false)
                       {
                           rate = "." + rate;
                       }

                       double TF_amt = Convert.ToDouble(TFee_E);
                       double Reg_amt = Convert.ToDouble(Reg_E);
                       double Mis_amt = Convert.ToDouble(Mis_E);
                       double anlamt = Convert.ToDouble(annualamount_E);
                       double discrate = Convert.ToDouble(rate);
                       discountedAmtOtherDisc = TF_amt * discrate;
                       TF_amt -= discountedAmtOtherDisc;
                       discountedTotalOtherDisc = TF_amt+Reg_amt+Mis_amt;

                       string DiscamountDisp = "";
                       if (discountedAmtOtherDisc >= 1000)
                       {
                           DiscamountDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                       }
                       if (discountedAmtOtherDisc < 1000)
                       {
                           DiscamountDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                       }

                       double disctotalOther = Convert.ToDouble(discountedTotalOtherDisc);
                       if (disctotalOther >= 1000)
                       {
                           discTotalAssDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(disctotalOther));
                       }
                       if (disctotalOther < 1000)
                       {
                           discTotalAssDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(disctotalOther));
                       }
                   }
               }
           }


           setupSOA();
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
                   casmain.cashlog = asslog;
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
                   pmf.prinlog = asslog;
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
                   regmain.reglog = asslog;
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
                   empf.faclog = asslog;
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
               frmadm.admlog = asslog;
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
               formPay.paylog = asslog;
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
               dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
               return;
           }
           if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Student grades")
           {
               frmStdGrd formstdgrd = new frmStdGrd();
               this.Hide();
               formstdgrd.emptype = emptype;
               formstdgrd.CO = co;
               formstdgrd.grdlog = asslog;
               formstdgrd.accesscode = accesscode;
               formstdgrd.theFacultyName = thefac;
               formstdgrd.viewNotifLate = viewNotifLate;
               formstdgrd.VISITED = VISITED;
               formstdgrd.viewNotifDue = viewNotifDue;
               formstdgrd.viewNotifDisc = viewNotifDisc;
               formstdgrd.notifstat = notifstat;
               formstdgrd.Show();
           }
           if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Student information")
           {
               frmStudInfo stud = new frmStudInfo();
               this.Hide();
               stud.emptype = emptype;
               stud.CO = co;
               stud.studlog = asslog;
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
               facf.facinfolog = asslog;
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
               frmFacultyAdvisory frmFacAdv = new frmFacultyAdvisory();
               this.Hide();
               frmFacAdv.emptype = emptype;
               frmFacAdv.co = co;
               frmFacAdv.advlog = asslog;
               frmFacAdv.accesscode = accesscode;
               frmFacAdv.thefac = thefac;
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
               frmSec.seclog = asslog;
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
               rfac.replog = asslog;
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
               rsched.schedlog = asslog;
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
               about.ablog = asslog;
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

       private void btnEdit_Click(object sender, EventArgs e)
       {
           if (btnEdit.Text == "Edit")
           {
               pnlSI.Enabled = true;
               btnEdit.Text = "Save";
               btnCancelSI.Enabled = true;
           }
           else
           {
               if (txtFname.Text == "" || txtLastSI.Text == "" || txtAdd.Text == "" || cmbGen.Text == "" || cmbMonth.Text == "" || cmbDay.Text == "" || cmbYear.Text == "" ||
                   txtGrdName.Text == "" || txtGrdRelation.Text == "" || txtGrdCon.Text == "")
               {
                   inputcheck("valid", txtGrdCon);
                   MessageBox.Show("fill out required fields.", "Student records", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   return;
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
                   if ((txtCon.TextLength == 11) && (txtCon.Text.Substring(0, 2) != "09"))
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
               if (txtGrdCon.Text != "")
               {
                   if ((txtGrdCon.TextLength == 11) && (txtGrdCon.Text.Substring(0, 2) != "09"))
                   {
                       inputcheck("invalid", txtGrdCon);

                   }
                   else if ((txtGrdCon.TextLength != 11) && (txtGrdCon.TextLength != 7))
                   {

                       inputcheck("invalid", txtGrdCon);

                   }
                   else
                   {
                       inputcheck("valid", txtGrdCon);
                   }
               }
               if (txtMidl.Text == "")
               {
                   inputcheck("valid", txtMidl);
               }
               if (txtCon.Text == "")
               {
                   inputcheck("valid", txtCon);
               }


               if ((((txtGrdCon.TextLength == 11) && (txtGrdCon.Text.Substring(0, 2) == "09")) || (txtGrdCon.TextLength == 7)))
               {
                   if (txtCon.Text != "")
                   {
                       if (((txtCon.TextLength == 11) && (txtCon.Text.Substring(0, 2) != "09")) || ((txtCon.TextLength != 11) && (txtCon.TextLength != 7)))
                       {
                           return;
                       }
                   }
                   if (txtMidl.Text != "")
                   {
                       if (txtMidl.TextLength < 2)
                       {
                           return;
                       }
                   }

                   setupsaveoperation();
                  
               }
           }
       }

       public void setupsaveoperation()
       {
           string bday = cmbMonth.Text + " " + cmbDay.Text + " " + cmbYear.Text;
           int current = Convert.ToInt32(DateTime.Now.Year);
           int birthyear = Convert.ToInt32(cmbYear.Text);
           int age = current - birthyear;

           con.Open();
           string updateStudent = "Update stud_tbl set fname='" + txtFname.Text + "',mname='" + txtMidl.Text + "',lname='" + txtLast.Text + "',address='" + txtAdd.Text + "',birthdate='" + bday + "',age='" + age + "',gender='" + cmbGen.Text + "',studcon='" + txtCon.Text + "',school='" + txtSchool.Text + "',talentskill='" + txtTalSki.Text + "',award='" + txtAward.Text + "',fathername='" + txtFathName.Text + "',fatheroccup='" + txtFathOcc.Text + "',mothername='" + txtMothName.Text + "',motheroccup='" + txtMothOcc.Text + "',guardian='" + txtGrdName.Text + "',guardianoccup='" + txtGrdOcc.Text + "',pgcon='" + txtGrdCon.Text + "',guardianrelation='" + txtGrdRelation.Text + "' where studno='" + txtSnum.Text + "'";
           OdbcCommand cmdUpdateStudent = new OdbcCommand(updateStudent, con);
           cmdUpdateStudent.ExecuteNonQuery();
           con.Close();

           pnlSI.Enabled = false;
           btnEdit.Text = "Edit";
           btnCancelSI.Enabled = false;
           MessageBox.Show("student successfully updated", "Student records", MessageBoxButtons.OK, MessageBoxIcon.Information);
           txtMidl.BackColor = Color.White;
           txtSearch.Focus();
       }

       private void btnCancelSI_Click(object sender, EventArgs e)
       {
           setupretrieveddata(txtSnum.Text);
           pnlSI.Enabled = false;
           btnEdit.Text = "Edit";
           btnCancelSI.Enabled = false;
       }

       public void setupretrieveddata(string thekey)
       {
            
            int start = 1970;
            int current = Convert.ToInt32(DateTime.Now.Year);

            while (current >= start)
            {
                cmbYear.Items.Add(current);
                current--;
            }
        
           con.Open();
           OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='" + thekey + "'", con);
           DataTable dt = new DataTable();
           da.Fill(dt);
           con.Close();

           if (dt.Rows.Count > 0)
           {

               btnEdit.Enabled = true;
               txtFname.Text = dt.Rows[0].ItemArray[1].ToString();
               txtMidl.Text = dt.Rows[0].ItemArray[2].ToString();
               txtLastSI.Text = dt.Rows[0].ItemArray[3].ToString();
               txtSchool.Text = dt.Rows[0].ItemArray[6].ToString();
               txtAdd.Text = dt.Rows[0].ItemArray[7].ToString();
               if (dt.Rows[0].ItemArray[8].ToString() != "")
               {
                   cmbMonth.Text = dt.Rows[0].ItemArray[8].ToString().Substring(0, 3).ToString();//0 start of string 3 the length 
                   cmbDay.Text = dt.Rows[0].ItemArray[8].ToString().Substring(4, 2).ToString();
                   cmbYear.Text = dt.Rows[0].ItemArray[8].ToString().Substring(7, 4).ToString();
                   txtAge.Text = dt.Rows[0].ItemArray[9].ToString();
               }

               cmbGen.Text = dt.Rows[0].ItemArray[10].ToString();
               txtCon.Text = dt.Rows[0].ItemArray[11].ToString();
               txtFathName.Text = dt.Rows[0].ItemArray[12].ToString();
               txtFathOcc.Text = dt.Rows[0].ItemArray[13].ToString();
               txtMothName.Text = dt.Rows[0].ItemArray[14].ToString();
               txtMothOcc.Text = dt.Rows[0].ItemArray[15].ToString();
               txtGrdName.Text = dt.Rows[0].ItemArray[16].ToString();
               txtGrdOcc.Text = dt.Rows[0].ItemArray[17].ToString();
               txtGrdCon.Text = dt.Rows[0].ItemArray[18].ToString();
               txtTalSki.Text = dt.Rows[0].ItemArray[19].ToString();
               txtAward.Text = dt.Rows[0].ItemArray[20].ToString();
               txtGrdRelation.Text = dt.Rows[0].ItemArray[25].ToString();

           }
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
           if (txtLastSI.Text != "")
           {
               if (txtLastSI.TextLength == 1)
               {
                   string last = txtLastSI.Text.Substring(0, txtLastSI.TextLength);

                   if (last == "Ñ" || last == "ñ")
                   {
                       orgtext = txtLastSI.Text.Substring(0, txtLastSI.TextLength - 1);
                   }
                   else
                   {
                       orgtext = txtLastSI.Text;
                   }

               }
               else
               {
                   string last = txtLastSI.Text.Substring(txtLastSI.TextLength - 1, 1);

                   if (last == "Ñ" || last == "ñ")
                   {
                       orgtext = txtLastSI.Text.Substring(0, txtLastSI.TextLength - 1);
                   }
                   else
                   {
                       orgtext = txtLastSI.Text.Substring(0, txtLastSI.TextLength);
                   }
               }
           }

           if (lstEnye == 1)
           {
               if (txtLastSI.Text != "")
               {
                   txtLastSI.Text = orgtext + "Ñ";
                   lstEnye += 1;
               }
               else
               {
                   txtLastSI.Text = "Ñ";
                   lstEnye += 1;
               }
           }
           else
           {
               if (txtLastSI.Text != "")
               {
                   txtLastSI.Text = orgtext + "ñ";
                   lstEnye -= 1;
               }
               else
               {
                   txtLastSI.Text = "ñ";
                   lstEnye -= 1;
               }
           }

           txtLastSI.Focus();
           txtLastSI.SelectionStart = txtLastSI.Text.Length;
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
           if (txtGrdName.Text != "")
           {
               if (txtGrdName.TextLength == 1)
               {
                   string last = txtGrdName.Text.Substring(0, txtGrdName.TextLength);

                   if (last == "Ñ" || last == "ñ")
                   {
                       orgtext = txtGrdName.Text.Substring(0, txtGrdName.TextLength - 1);
                   }
                   else
                   {
                       orgtext = txtGrdName.Text;
                   }

               }
               else
               {
                   string last = txtGrdName.Text.Substring(txtGrdName.TextLength - 1, 1);

                   if (last == "Ñ" || last == "ñ")
                   {
                       orgtext = txtGrdName.Text.Substring(0, txtGrdName.TextLength - 1);
                   }
                   else
                   {
                       orgtext = txtGrdName.Text.Substring(0, txtGrdName.TextLength);
                   }
               }
           }

           if (guaenye == 1)
           {
               if (txtGrdName.Text != "")
               {
                   txtGrdName.Text = orgtext + "Ñ";
                   guaenye += 1;
               }
               else
               {
                   txtGrdName.Text = "Ñ";
                   guaenye += 1;
               }
           }
           else
           {
               if (txtGrdName.Text != "")
               {
                   txtGrdName.Text = orgtext + "ñ";
                   guaenye -= 1;
               }
               else
               {
                   txtGrdName.Text = "ñ";
                   guaenye -= 1;
               }
           }

           txtGrdName.Focus();
           txtGrdName.SelectionStart = txtGrdName.Text.Length;
       }
    }
}
