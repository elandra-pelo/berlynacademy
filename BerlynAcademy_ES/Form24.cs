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
    public partial class frmPayment : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string paylog, CashierOperator, COwoex, TheFac, emptype, accesscode, VISITED, discountType, amt_monthlyIns_OtherDisc,activeSY,notifstat;
        public string FREELASTMONTHTOTAL_K_FORPAYMENTHISTORY,FIFTYDISCTOTAL_K_FORPAYMENTHISTORY,
                      FREELASTMONTHTOTAL_E_FORPAYMENTHISTORY,FIFTYDISCTOTAL_E_FORPAYMENTHISTORY,
                      FREELASTMONTHTOTAL_J_FORPAYMENTHISTORY,FIFTYDISCTOTAL_J_FORPAYMENTHISTORY;
        public string annualamount_K,uponamount_K = "",monthlyamount_K = "",fiftyDisc_K = "",FreeLastMonthTotal_K = "",fiftyDiscTotal_K = "",TFee_K,Reg_K,Mis_K;
        public string annualamount_E = "", uponamount_E = "", monthlyamount_E = "", fiftyDisc_E = "", FreeLastMonthTotal_E = "", fiftyDiscTotal_E = "",TFee_E, Reg_E, Mis_E;
        public string annualamount_J = "", uponamount_J = "", monthlyamount_J = "", fiftyDisc_J = "", FreeLastMonthTotal_J = "", fiftyDiscTotal_J = "",TFee_J, Reg_J, Mis_J;
        public string paydesc_rec, payamount_rec,paycash_rec,paychange_rec,payrecno_rec,addAmt_rec,balance_rec,assessment_rec,paymentNum_rec,transno_rec,regiAmt_rec,miscAmt_rec,tfAmt_rec,desc_upon;
        public string today, secpay, thipay, foupay, fifpay, sixpay, sevpay, eigpay, ninpay, tenpay;
        public string pue,p1, p2, p3, p4, p5, p6, p7, p8, p9, p10,datetoday,theAmountPaidToSet;
        public double newbal = 0, annualamt, monthlyamt, comp1, LessAmt_K, LessAmt_J, LessAmt_E, annualamt_fiftydiscK, anuualamt_freelastmonthK, annualamt_fiftydiscE, anuualamt_freelastmonthE, annualamt_fiftydiscJ, anuualamt_freelastmonthJ, discountedAmtOtherDisc, discountedTotalOtherDisc, InstallmentAmt_forOtherDisc;
        public bool isAdvance,isPrint,isPayChange,isthereCurrentTransaction;
        public DataView dvS;
        public bool isVisited,viewNotifDue,viewNotifDisc,viewNotifLate;
        public int nextTransNum,tickwait;

        public frmPayment()
        {
            InitializeComponent();
        }

        private void frmPayment_Load(object sender, EventArgs e)
        {
            //this.BackColor = Color.FromArgb(49, 79, 142);
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //pnlH1.BackColor = Color.FromArgb(0, 0, 25);
            //pnlH2.BackColor = Color.FromArgb(0, 0, 25);
            //pnlH3.BackColor = Color.FromArgb(0, 0, 25);
            //pnlH4.BackColor = Color.FromArgb(0, 0, 25);
           
            //lbl1.ForeColor = Color.FromArgb(39, 69, 132);
            //lbl2.ForeColor = Color.FromArgb(39, 69, 132);

            //btnHome.Text = "          " + paylog;
            lblLogger.Text = paylog;
            lblLoggerPosition.Text = emptype;
            txtCashier.Text = CashierOperator;
            COwoex = txtCashier.Text.Substring(4, txtCashier.Text.Length-4);
            tmrDateTime.Enabled = true;
            cmbFilter.Text = "Student number";

            if (isVisited == false)
            {
                if (VISITED.Contains("Payment") == false)
                {
                    VISITED += "   Payment";
                    isVisited = true;
                }
            }

            setupStudents();
            setupMENU();
            GetActiveSchoolYear();
           
            retrievedNotificationDisplay();
           
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

        public void setupTransactionNum()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select transno,daytransac from receiptno_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                DateTime dateStored = Convert.ToDateTime(dt.Rows[0].ItemArray[1].ToString());
                if (dateStored.ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    con.Open();
                    string upd = "Update receiptno_tbl set transno='" + "1" + "',daytransac='" + DateTime.Now.ToShortDateString()+"'";
                    OdbcCommand cmdupd = new OdbcCommand(upd, con);
                    cmdupd.ExecuteNonQuery();
                    con.Close();
                    transno_rec = "1";
                    nextTransNum = 1;
                }
                else
                {

                    nextTransNum = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString());
                    nextTransNum++;
                    transno_rec = nextTransNum.ToString();
                    
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

            int getpaymentindex = 1;
            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                getpaymentindex++;
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
            dgvm.Rows[getpaymentindex].DefaultCellStyle.BackColor = Color.LightGreen;
        }


        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin logf = new frmEmpLogin();
            this.Hide();
            logf.Show();
        }

        public void setupStudents()
        {
            con.Open();

            string activeSY = "";
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);

            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString(); }

            OdbcDataAdapter da = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from stud_tbl where syregistered='"+activeSY+"' and status='Active'", con);//status='"+"Active"+"'
            DataTable dts = new DataTable();
            da.Fill(dts);

            OdbcDataAdapter da1 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_tbl where syregistered='"+activeSY+"'", con);
            DataTable dts1 = new DataTable();
            da1.Fill(dts1);

            if (dts1.Rows.Count > 0)
            {
               
                for (int i = 0; i < dts1.Rows.Count; i++)
                {
                    dts.Rows.Add(dts1.Rows[i].ItemArray[0].ToString(), dts1.Rows[i].ItemArray[1].ToString());
                    
                }
            }

            OdbcDataAdapter da2 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_old_tbl where syregistered='"+activeSY+"'", con);
            DataTable dts2 = new DataTable();
            da2.Fill(dts2);

            if (dts2.Rows.Count > 0)
            {

                for (int i = 0; i < dts2.Rows.Count; i++)
                {
                    dts.Rows.Add(dts2.Rows[i].ItemArray[0].ToString(), dts2.Rows[i].ItemArray[1].ToString());

                }
            }

            con.Close();
            dvS = new DataView(dts);
            dgvSearch.DataSource = dvS;
            dgvSearch.Columns[0].Width = 90;
            dgvSearch.Columns[1].Width = 210;
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

        private void frmPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isthereCurrentTransaction == true && isPrint == false)
            {
                MessageBox.Show("Current transaction is not yet finish!", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {

                LOGOUT();
                frmEmpLogin logf = new frmEmpLogin();
                this.Hide();
                logf.Show();
            }
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmCashierMain casmain = new frmCashierMain();
            this.Hide();
            casmain.cashlog = paylog;
            casmain.CO = CashierOperator;
            casmain.Show();
        }

        private void btnStudI_Click(object sender, EventArgs e)
        {
            frmStudInfo cassi = new frmStudInfo();
            this.Hide();
            cassi.studlog = paylog;
            cassi.emptype = "cashier";
            cassi.CO = CashierOperator;
            cassi.Show();
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            frmEmpAbout eac = new frmEmpAbout();
            this.Hide();
            eac.ablog = paylog;
            eac.emptype = "cashier";
            eac.CO = CashierOperator;
            eac.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "Student number")
            {
                dvS.RowFilter = string.Format("No LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvS;
            }
            if (cmbFilter.Text == "Student's name")
            {
                dvS.RowFilter = string.Format("Student LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvS;
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

        public void setupPaymentSummary()
        {
            lvwPS.Clear();
            if(txtMOP.Text=="Cash")
            {
                lvwPS.Items.Clear();
                string bal = "";
                con.Open();
                OdbcDataAdapter daps = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + txtSnum.Text + "'",con);
                DataTable dtps = new DataTable();
                daps.Fill(dtps);
                con.Close();

                if (dtps.Rows.Count > 0)
                {
                    lvwPS.Columns.Add("Description", 280, HorizontalAlignment.Left);
                    lvwPS.Columns.Add("", 130, HorizontalAlignment.Right);

                    if (dtps.Rows[0].ItemArray[4].ToString() == "")
                    {
                        bal = dtps.Rows[0].ItemArray[2].ToString();
                        string bal_dis="";
                        double theBal = Convert.ToDouble(bal);
                        if(theBal>=1000)
                        {
                            bal_dis = String.Format(("{0:0,###.00#}"),theBal);
                        }
                        if(theBal<1000)
                        {
                            bal_dis = String.Format(("{0:0.00#}"),theBal);
                        }

                        ListViewItem itps00 = new ListViewItem();
                        itps00.Text = "Annual payment";
                        itps00.SubItems.Add("P "+bal_dis);
                        lvwPS.Items.Add(itps00);
                    }
                    else 
                    {
                        bal = "0.00";
                    }

                    if (bal== "0.00")
                    {
                        ListViewItem itps0 = new ListViewItem();
                        itps0.Text = "Balance";
                        itps0.SubItems.Add("P "+bal);
                        lvwPS.Items.Add(itps0);
                    }
                    else
                    {
                        string bal_dis = "";
                        double theBal = Convert.ToDouble(bal);
                        if (theBal >= 1000)
                        {
                            bal_dis = String.Format(("{0:0,###.00#}"), theBal);
                        }
                        if (theBal < 1000)
                        {
                            bal_dis = String.Format(("{0:0.00#}"), theBal);
                        }

                        ListViewItem itps0 = new ListViewItem();
                        itps0.Text = "Balance";
                        itps0.SubItems.Add("P "+bal_dis);
                        lvwPS.Items.Add(itps0);
                    }
                }
                else
                {
                    lvwPS.Clear();
                    lvwPS.Items.Clear();
                }
            }
            if(txtMOP.Text=="Installment")
            {
                lvwPS.Items.Clear();
                string bal = "";
                string upon = "";
                string monthlyamt = "";
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
                OdbcDataAdapter dapskmi = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + txtSnum.Text + "'", con);
                DataTable dtpskmi = new DataTable();
                dapskmi.Fill(dtpskmi);
                con.Close();

                if (dtpskmi.Rows.Count > 0)
                {
                    if(txtGrd.Text=="Kinder")
                    {
                        setupAssessmentKinder();
                        ////FOR THOSE STUDENT WHO HAVE DISCOUNT
                        con.Open();
                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        con.Close();
                        if (dtk1.Rows.Count > 0)
                        {
                            string discounttype = dtk1.Rows[0].ItemArray[1].ToString();
                            if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                            {
                                monthlyamt = monthlyamount_K;
                            }
                            if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                            {
                                monthlyamt = monthlyamount_K;
                            }
                            if (discounttype.Contains("siblings") == false && discounttype.Contains("First") == false && discounttype.Contains("1st") == false && discounttype.Contains("Second") == false && discounttype.Contains("2nd") == false)
                            {
                                retrieveMonthlyInstallmentAmt_OtherDisc(discounttype);
                                monthlyamt = InstallmentAmt_forOtherDisc.ToString();
                            }
                        }
                        else
                        {
                            //FOR THOSE STUDENT WHO DONT HAVE DISCOUNT
                            con.Open();
                            OdbcDataAdapter daa = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='"+activeSY+"'", con);
                            DataTable dtt = new DataTable();
                            daa.Fill(dtt);
                            con.Close();
                            if (dtt.Rows.Count > 0)
                            {
                                for (int a = 0; a < dtt.Rows.Count; a++)
                                {
                                    if (dtt.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                                    {
                                        monthlyamt = dtt.Rows[a].ItemArray[2].ToString();
                                    }
                                }
                            }
                        }    
                    }
                    if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
                    {
                        ////FOR THOSE STUDENT WHO HAVE DISCOUNT
                        setupAssessmentElem();
                        con.Open();
                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        con.Close();
                        if (dtk1.Rows.Count > 0)
                        {
                            string discounttype = dtk1.Rows[0].ItemArray[1].ToString();
                            if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                            {
                                monthlyamt = monthlyamount_E;
                            }
                            if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                            {
                                monthlyamt = monthlyamount_E;
                            }
                            if (discounttype.Contains("siblings") == false && discounttype.Contains("First") == false && discounttype.Contains("1st") == false && discounttype.Contains("Second") == false && discounttype.Contains("2nd") == false)
                            {
                                retrieveMonthlyInstallmentAmt_OtherDisc(discounttype);
                                monthlyamt = InstallmentAmt_forOtherDisc.ToString();
                            }
                        }
                        else
                        {
                            con.Open();
                            OdbcDataAdapter daa = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
                            DataTable dtt = new DataTable();
                            daa.Fill(dtt);
                            con.Close();
                            if (dtt.Rows.Count > 0)
                            {
                                for (int a = 0; a < dtt.Rows.Count; a++)
                                {
                                    if (dtt.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                                    {
                                        monthlyamt = dtt.Rows[a].ItemArray[2].ToString();
                                    }
                                }
                            }
                        }
                    }
                    if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                    {
                        ////FOR THOSE STUDENT WHO HAVE DISCOUNT
                        setupAssessmentJunior();
                        con.Open();
                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        con.Close();
                        if (dtk1.Rows.Count > 0)
                        {
                            string discounttype = dtk1.Rows[0].ItemArray[1].ToString();
                            if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                            {
                                monthlyamt = monthlyamount_J;
                            }
                            if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                            {
                                monthlyamt = monthlyamount_J;
                            }
                            if (discounttype.Contains("siblings") == false && discounttype.Contains("First") == false && discounttype.Contains("1st") == false && discounttype.Contains("Second") == false && discounttype.Contains("2nd") == false)
                            {//herehere
                                
                                retrieveMonthlyInstallmentAmt_OtherDisc(discounttype);
                                monthlyamt = InstallmentAmt_forOtherDisc.ToString();
                            }
                        }
                        else
                        {
                            con.Open();
                            OdbcDataAdapter daa = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
                            DataTable dtt = new DataTable();
                            daa.Fill(dtt);
                            con.Close();
                            if (dtt.Rows.Count > 0)
                            {
                                for (int a = 0; a < dtt.Rows.Count; a++)
                                {
                                    if (dtt.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                                    {
                                        monthlyamt = dtt.Rows[a].ItemArray[2].ToString();
                                    }
                                }
                            }
                        }
                    }

                    lvwPS.Columns.Add("Description", 280, HorizontalAlignment.Left);
                    lvwPS.Columns.Add("", 130, HorizontalAlignment.Right);

                    upon = dtpskmi.Rows[0].ItemArray[2].ToString();
                    bal = dtpskmi.Rows[0].ItemArray[4].ToString();
                    double thebal = Convert.ToDouble(bal);

                    if (dtpskmi.Rows[0].ItemArray[15].ToString() == "0")
                    {
                        ListViewItem itps0 = new ListViewItem();
                        itps0.Text = "Upon enrollment";
                        itps0.SubItems.Add("P " + upon);
                        lvwPS.Items.Add(itps0);

                        string bal_dis = "";
                        double theBal = Convert.ToDouble(bal);
                        if (theBal >= 1000)
                        {
                            bal_dis = String.Format(("{0:0,###.00#}"), theBal);
                        }
                        if (theBal < 1000)
                        {
                            bal_dis = String.Format(("{0:0.00#}"), theBal);
                        }

                        ListViewItem itps1 = new ListViewItem();
                        itps1.Text = "Balance";
                        itps1.SubItems.Add("P " + bal_dis);
                        lvwPS.Items.Add(itps1);
                    }
                    else
                    {
                        if (thebal > 0)
                        {
                            string bal_dis = "";
                            double theBal = Convert.ToDouble(bal);
                            if (theBal >= 1000)
                            {
                                bal_dis = String.Format(("{0:0,###.00#}"), theBal);
                            }
                            if (theBal < 1000)
                            {
                                bal_dis = String.Format(("{0:0.00#}"), theBal);
                            }

                            double theMonthlyToDisplay = Convert.ToDouble(monthlyamt);
                            if (theMonthlyToDisplay >= 1000)
                            {
                                monthlyamt = String.Format(("{0:0,###.00#}"), theMonthlyToDisplay);
                            }
                            else
                            {
                                monthlyamt = String.Format(("{0:0.00#}"), theMonthlyToDisplay);
                            }

                            if (thebal >= theMonthlyToDisplay)
                            {
                                ListViewItem itps3 = new ListViewItem();
                                itps3.Text = "Monthly installment";
                                itps3.SubItems.Add("P " + monthlyamt);
                                lvwPS.Items.Add(itps3);

                                ListViewItem itps1 = new ListViewItem();
                                itps1.Text = "Balance";
                                itps1.SubItems.Add("P " + bal_dis);
                                lvwPS.Items.Add(itps1);
                            }
                            else
                            {
                                ListViewItem itps1 = new ListViewItem();
                                itps1.Text = "Balance";
                                itps1.SubItems.Add("P " + bal_dis);
                                lvwPS.Items.Add(itps1);
                            }
                        }
                        else
                        {
                            ListViewItem itps1 = new ListViewItem();
                            itps1.Text = "Balance";
                            itps1.SubItems.Add("P 0.00");
                            lvwPS.Items.Add(itps1);
                        }
                    }
                }
                else
                {
                    lvwPS.Clear();
                    lvwPS.Items.Clear();
                }
            }
        }

        public void setupSectioning(string level,string studno)
        {
            int roomcapacity = 0;
            int minrank = 0;
            int maxrank = 0;
            //MIN-----------------
            con.Open();
            OdbcDataAdapter daa = new OdbcDataAdapter("Select min(rank) from section_tbl where level='" + level + "'and rank<>'0'", con);
            DataTable dtt = new DataTable();
            daa.Fill(dtt);
            con.Close();
            if (dtt.Rows.Count > 0)
            {
                minrank = Convert.ToInt32(dtt.Rows[0].ItemArray[0].ToString());
            }
            //MAX-----------------
            con.Open();
            OdbcDataAdapter daaa = new OdbcDataAdapter("Select max(rank) from section_tbl where level='" + level + "'", con);
            DataTable dttt = new DataTable();
            daaa.Fill(dttt);
            con.Close();
            if (dttt.Rows.Count > 0)
            {
                maxrank = Convert.ToInt32(dttt.Rows[0].ItemArray[0].ToString());
            }

            for (int l = 0; l < maxrank; l++)
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from section_tbl where level='" + level + "'and rank='" + minrank + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    for (int c = 0; c < dt.Rows.Count; c++)
                    {
                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from roomallocation_tbl where grade='" + level + "'and section='" + dt.Rows[c].ItemArray[1].ToString() + "'", con);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        con.Close();
                        if (dt1.Rows.Count > 0)
                        {
                            con.Open();
                            OdbcDataAdapter da2 = new OdbcDataAdapter("Select*from room_tbl where id='" + dt1.Rows[0].ItemArray[0].ToString() + "'", con);
                            DataTable dt2 = new DataTable();
                            da2.Fill(dt2);
                            con.Close();
                            if (dt2.Rows.Count > 0)
                            {
                                roomcapacity = Convert.ToInt32(dt2.Rows[0].ItemArray[3].ToString());
                                con.Open();
                                OdbcDataAdapter da3 = new OdbcDataAdapter("Select count(studno) from stud_tbl where level='" + level + "' and section='" + dt.Rows[c].ItemArray[1].ToString() + "'and status='Active'", con);
                                DataTable dt3 = new DataTable();
                                da3.Fill(dt3);
                                con.Close();
                                if (dt3.Rows.Count > 0)
                                {
                                    int totalinsection = Convert.ToInt32(dt3.Rows[0].ItemArray[0].ToString());
                                    if (totalinsection < roomcapacity)
                                    {
                                        con.Open();
                                        string update = "Update stud_tbl set section='" + dt.Rows[c].ItemArray[1].ToString() + "'where studno='" + studno + "'";
                                        OdbcCommand cmd = new OdbcCommand(update, con);
                                        cmd.ExecuteNonQuery();

                                        string update1 = "Update section_tbl set status='" + "inactive" + "'where level='" + level +"'";
                                        OdbcCommand cmd1 = new OdbcCommand(update1, con);
                                        cmd1.ExecuteNonQuery();

                                        string update2 = "Update section_tbl set status='" + "active" + "'where level='" + level + "'and section='" + dt.Rows[c].ItemArray[1].ToString() + "'";
                                        OdbcCommand cmd2 = new OdbcCommand(update2, con);
                                        cmd2.ExecuteNonQuery();

                                        con.Close();
                                        l =maxrank;
                                    }
                                    else
                                    {
                                        con.Open();
                                        string update2 = "Update section_tbl set status='" + "inactive" + "',isFull='"+"yes"+"'where level='" + level + "'and section='" + dt.Rows[c].ItemArray[1].ToString() + "'";
                                        OdbcCommand cmd2 = new OdbcCommand(update2, con);
                                        cmd2.ExecuteNonQuery();
                                        con.Close();

                                        minrank++;
                                        continue;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        public void setupPayment()
        {
            isPrint = false;
            isthereCurrentTransaction = true;
            setupReceiptno();
            lvwPH.Clear();
            newbal = 0;
            if (txtMOP.Text == "Cash")
            {
                lvwPH.Items.Clear();
                string datetoday = DateTime.Now.ToShortDateString();

                con.Open();
                OdbcDataAdapter daps = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + txtSnum.Text + "'", con);
                DataTable dtps = new DataTable();
                daps.Fill(dtps);
                con.Close();

                if (dtps.Rows.Count > 0)
                {
                    double tuitn = 0.00;
                    string tuitnstring = "";
                    double cash = Convert.ToDouble(txtCashAmt.Text);
                    double advanceAmt = 0.00;
                    if (isAdvance == true)
                    {
                        advanceAmt = Convert.ToDouble(txtATP.Text);
                    }
                    double change = 0.00;

                    string slev = dtps.Rows[0].ItemArray[1].ToString();
                    string FLM = "";
                    string FPD = "";
                    string levdep = "";
                    double deductionFIFDISC = 0;
                    double deductionFREELM = 0;
                    con.Open();
                    OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + slev + "'", con);
                    DataTable dtdep = new DataTable();
                    dadep.Fill(dtdep);
                    con.Close();
                    if (dtdep.Rows.Count > 0)
                    {
                        levdep = dtdep.Rows[0].ItemArray[0].ToString();
                    }


                    if (slev == "Kinder")
                    {
                        setupAssessmentKinder();
                        con.Open();
                        OdbcDataAdapter dak = new OdbcDataAdapter("Select amount from fee_tbl where fee='" + "ANNUAL PAYMENT" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk = new DataTable();
                        dak.Fill(dtk);

                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "REGISTRATION" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        OdbcDataAdapter dak2 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "MISCELLANEOUS" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk2 = new DataTable();
                        dak2.Fill(dtk2);
                        OdbcDataAdapter dak3 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "TUITION FEE" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk3 = new DataTable();
                        dak3.Fill(dtk3);

                        if (dtk1.Rows.Count > 0) { regiAmt_rec = dtk1.Rows[0].ItemArray[2].ToString(); }
                        if (dtk2.Rows.Count > 0) { miscAmt_rec = dtk2.Rows[0].ItemArray[2].ToString(); }
                        if (dtk3.Rows.Count > 0) { tfAmt_rec = dtk3.Rows[0].ItemArray[2].ToString(); }

                        con.Close();

                        if (dtk.Rows.Count > 0)
                        {
                            tuitn = Convert.ToDouble(dtk.Rows[0].ItemArray[0].ToString());
                            tuitnstring = dtk.Rows[0].ItemArray[0].ToString();
                        }
                       
                        

                        FLM = FreeLastMonthTotal_K;
                        FPD = fiftyDiscTotal_K;
                        deductionFIFDISC = LessAmt_K;
                        deductionFREELM = monthlyamt;
                    }
                    if (slev == "Grade 1" || slev == "Grade 2" || slev == "Grade 3" || slev == "Grade 4" || slev == "Grade 5" || slev == "Grade 6")
                    {
                        setupAssessmentElem();
                        con.Open();
                        OdbcDataAdapter dak = new OdbcDataAdapter("Select amount from fee_tbl where fee='" + "ANNUAL PAYMENT" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk = new DataTable();
                        dak.Fill(dtk);
                        con.Close();

                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "REGISTRATION" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        OdbcDataAdapter dak2 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "MISCELLANEOUS" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk2 = new DataTable();
                        dak2.Fill(dtk2);
                        OdbcDataAdapter dak3 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "TUITION FEE" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk3 = new DataTable();
                        dak3.Fill(dtk3);

                        if (dtk1.Rows.Count > 0) { regiAmt_rec = dtk1.Rows[0].ItemArray[2].ToString(); }//CASTRO
                        if (dtk2.Rows.Count > 0) { miscAmt_rec = dtk2.Rows[0].ItemArray[2].ToString(); }
                        if (dtk3.Rows.Count > 0) { tfAmt_rec = dtk3.Rows[0].ItemArray[2].ToString(); }


                        if (dtk.Rows.Count > 0)
                        {
                            tuitn = Convert.ToDouble(dtk.Rows[0].ItemArray[0].ToString());
                            tuitnstring = dtk.Rows[0].ItemArray[0].ToString();
                        }

                        FLM = FreeLastMonthTotal_E;
                        FPD = fiftyDiscTotal_E;
                        deductionFIFDISC = LessAmt_E;
                        deductionFREELM = monthlyamt;
                    }
                    if (slev == "Grade 7" || slev == "Grade 8" || slev == "Grade 9" || slev == "Grade 10")
                    {
                        setupAssessmentJunior();
                        con.Open();
                        OdbcDataAdapter dak = new OdbcDataAdapter("Select amount from fee_tbl where fee='" + "ANNUAL PAYMENT" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk = new DataTable();
                        dak.Fill(dtk);

                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "REGISTRATION" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        OdbcDataAdapter dak2 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "MISCELLANEOUS" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk2 = new DataTable();
                        dak2.Fill(dtk2);
                        OdbcDataAdapter dak3 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "TUITION FEE" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk3 = new DataTable();
                        dak3.Fill(dtk3);

                        if (dtk1.Rows.Count > 0) { regiAmt_rec = dtk1.Rows[0].ItemArray[2].ToString(); }//CASTRO
                        if (dtk2.Rows.Count > 0) { miscAmt_rec = dtk2.Rows[0].ItemArray[2].ToString(); }
                        if (dtk3.Rows.Count > 0) { tfAmt_rec = dtk3.Rows[0].ItemArray[2].ToString(); }
                        con.Close();

                        if (dtk.Rows.Count > 0)
                        {
                            tuitn = Convert.ToDouble(dtk.Rows[0].ItemArray[0].ToString());
                            tuitnstring = dtk.Rows[0].ItemArray[0].ToString();
                        }

                        FLM = FreeLastMonthTotal_J;
                        FPD = fiftyDiscTotal_J;
                        deductionFIFDISC = LessAmt_J;
                        deductionFREELM = monthlyamt;
                    }
                    //===========================//Check for the amount of Tuition fee if DISCOUNTED
                    con.Open();
                    OdbcDataAdapter dakDISC = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                    DataTable dtkDISC = new DataTable();
                    dakDISC.Fill(dtkDISC);
                    con.Close();
                    if (dtkDISC.Rows.Count > 0)
                    {
                        string discounttype = dtkDISC.Rows[0].ItemArray[1].ToString();
                        if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                        {
                            tuitn -=deductionFREELM;
                        }
                        if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                        {
                            tuitn -=deductionFIFDISC;
                        }
                        if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                        {
                            retrieveMonthlyInstallmentAmt_OtherDisc(discountType);
                            tuitn -=discountedAmtOtherDisc;
                        }
                    }

                    //==============================
                    if (cash < tuitn)
                    {
                        return;
                    }
                    else
                    {
                        pnlNotPH.Visible = false;
                        lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                        lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                        lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);


                        
                        con.Open();
                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        con.Close();
                        if (dtk1.Rows.Count > 0)
                        {
                            string discounttype = dtk1.Rows[0].ItemArray[1].ToString();
                            if (discounttype.Contains("siblings") == true || discounttype.Contains("First") == true || discounttype.Contains("1st") == true)
                            {
                                assessment_rec = FLM;
                                double anuualamt_freelastmonth = Convert.ToDouble(FLM);
                                change = cash - anuualamt_freelastmonth;
                                txtChange.Text = change.ToString();
                                
                                string amtToDisplay = "";
                                double amt = anuualamt_freelastmonth;
                                if (amt >= 1000)
                                {
                                    amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                }
                                if (amt < 1000)
                                {
                                    amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                }

                                double TFAmt = Convert.ToDouble(tfAmt_rec);
                                double newTFDiscounted = TFAmt - deductionFREELM;
                                if (newTFDiscounted >= 1000)
                                {
                                    tfAmt_rec = String.Format(("{0:0,###.00#}"), newTFDiscounted);
                                }
                                if (newTFDiscounted < 1000)
                                {
                                    tfAmt_rec = String.Format(("{0:0.00#}"), newTFDiscounted);
                                }
                                
                                payamount_rec = amtToDisplay;
                                theAmountPaidToSet = amtToDisplay;
                                paymentNum_rec = "UPON ENROLLMENT";
   
                                ListViewItem itmpd = new ListViewItem();
                                itmpd.Text = "ANNUAL PAYMENT";
                                itmpd.SubItems.Add(datetoday);
                                itmpd.SubItems.Add("P " + amtToDisplay);
                                lvwPH.Items.Add(itmpd);

                                ListViewItem itps0 = new ListViewItem();
                                itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                itps0.Text = "Total:";
                                itps0.SubItems.Add("");
                                itps0.SubItems.Add("P " + amtToDisplay);
                                lvwPH.Items.Add(itps0);
                            }
                            if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                            {
                                assessment_rec = FPD;
                                double annualamt_fiftydisc = Convert.ToDouble(FPD);
                                change = cash - annualamt_fiftydisc;
                                txtChange.Text = change.ToString();
                                
                                string amtToDisplay = "";
                                double amt = annualamt_fiftydisc;
                                if (amt >= 1000)
                                {
                                    amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                }
                                if (amt < 1000)
                                {
                                    amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                }

                                double TFAmt = Convert.ToDouble(tfAmt_rec);
                                double newTFDiscounted = TFAmt - deductionFIFDISC;
                                if (newTFDiscounted >= 1000)
                                {
                                    tfAmt_rec = String.Format(("{0:0,###.00#}"), newTFDiscounted);
                                }
                                if (newTFDiscounted < 1000)
                                {
                                    tfAmt_rec = String.Format(("{0:0.00#}"), newTFDiscounted);
                                }

                                payamount_rec = amtToDisplay;
                                theAmountPaidToSet = amtToDisplay;
                                paymentNum_rec = "UPON ENROLLMENT";

                                ListViewItem itmpd = new ListViewItem();
                                itmpd.Text = "ANNUAL PAYMENT";
                                itmpd.SubItems.Add(datetoday);
                                itmpd.SubItems.Add("P " + amtToDisplay);
                                lvwPH.Items.Add(itmpd);

                                ListViewItem itps0 = new ListViewItem();
                                itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                itps0.Text = "Total:";
                                itps0.SubItems.Add("");
                                itps0.SubItems.Add("P " + amtToDisplay);
                                lvwPH.Items.Add(itps0);
                            }
                            if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                            {
                                retrieveMonthlyInstallmentAmt_OtherDisc(discountType);
                                assessment_rec = discountedTotalOtherDisc.ToString();
                                change = cash - discountedTotalOtherDisc;
                                txtChange.Text = change.ToString();
                                
                                string amtToDisplay = "";
                                double amt = discountedTotalOtherDisc;
                                if(amt>=1000)
                                {
                                    amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                }
                                if (amt < 1000)
                                {
                                    amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                }
                                double TFAmt = Convert.ToDouble(tfAmt_rec);
                                double newTFDiscounted = TFAmt - discountedAmtOtherDisc;
                                if (newTFDiscounted >= 1000)
                                {
                                    tfAmt_rec = String.Format(("{0:0,###.00#}"), newTFDiscounted);
                                }
                                if (newTFDiscounted < 1000)
                                {
                                    tfAmt_rec = String.Format(("{0:0.00#}"), newTFDiscounted);
                                }
                               
                                payamount_rec = amtToDisplay;
                                theAmountPaidToSet = amtToDisplay;
                                paymentNum_rec = "UPON ENROLLMENT";

                                ListViewItem itmpd = new ListViewItem();
                                itmpd.Text = "ANNUAL PAYMENT";
                                itmpd.SubItems.Add(datetoday);
                                itmpd.SubItems.Add("P "+amtToDisplay);
                                lvwPH.Items.Add(itmpd);

                                ListViewItem itps0 = new ListViewItem();
                                itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                itps0.Text = "Total:";
                                itps0.SubItems.Add("");
                                itps0.SubItems.Add("P "+amtToDisplay);
                                lvwPH.Items.Add(itps0);
                            }
                                   

                        }
                        else
                        {
                            assessment_rec = tuitnstring;
                            change = cash - tuitn;
                            txtChange.Text = change.ToString();
                            
                            payamount_rec = tuitnstring;
                            theAmountPaidToSet = tuitnstring;
                            paymentNum_rec = "UPON ENROLLMENT";

                            ListViewItem itmpd = new ListViewItem();
                            itmpd.Text = "ANNUAL PAYMENT";
                            itmpd.SubItems.Add(datetoday);
                            itmpd.SubItems.Add("P "+tuitnstring);
                            lvwPH.Items.Add(itmpd);

                            ListViewItem itmpdsum = new ListViewItem();
                            itmpdsum.Font = new Font("Arial", 11, FontStyle.Bold);
                            itmpdsum.Text = "Total:";
                            itmpdsum.SubItems.Add("");
                            itmpdsum.SubItems.Add("P "+tuitnstring);
                            lvwPH.Items.Add(itmpdsum);
                        }

                        balance_rec = "0.00";
                        desc_upon = "TUITION FEE";
                        con.Open();
                        string setToPaid = "Update paymentcash_tbl set datepd='" + datetoday + "',timepd='" + txtTime.Text + "',cashier='" + COwoex + "',ORnum='"+payrecno_rec+"'where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                        cmdtopd.ExecuteNonQuery();
                        con.Close();
                        setupEnroleeENROLLED(slev);

                        //UPDATE THE DISPLAY
                        setupPaymentSummary();

                        }
                }
                else
                {
                    pnlNotPH.Visible = true;
                    lvwPH.Clear();
                    lvwPH.Items.Clear();
                }
            }
            if (txtMOP.Text == "Installment")//PAYMENT INSTALLMENT
            {
                lvwPH.Items.Clear();
               
                datetoday = DateTime.Now.ToShortDateString();

                pnlNotPH.Visible = false;
                lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);

                con.Open();
                OdbcDataAdapter daps = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + txtSnum.Text + "'", con);
                DataTable dtps = new DataTable();
                daps.Fill(dtps);
                con.Close();

                if (dtps.Rows.Count > 0)
                {
                    string uponestring = "";
                    string monthlystring = "";
                    string annualstring = "";
                    double monthlyi = 0.00;
                    double upone = 0.00;
                    double annual = 0.00;
                    double cash = 0;
                    double advamt = 0.00;

                    if (txtCashAmt.Text!="")
                    {
                        cash = Convert.ToDouble(txtCashAmt.Text);
                    }
                   
                    //these are the OR number in database
                    pue = dtps.Rows[0].ItemArray[46].ToString();
                    p2 = dtps.Rows[0].ItemArray[47].ToString();
                    p3 = dtps.Rows[0].ItemArray[48].ToString();
                    p4 = dtps.Rows[0].ItemArray[49].ToString();
                    p5 = dtps.Rows[0].ItemArray[50].ToString();
                    p6 = dtps.Rows[0].ItemArray[51].ToString();
                    p7 = dtps.Rows[0].ItemArray[52].ToString();
                    p8 = dtps.Rows[0].ItemArray[53].ToString();
                    p9 = dtps.Rows[0].ItemArray[54].ToString();
                    p10 = dtps.Rows[0].ItemArray[55].ToString();
                    //these are the amount were the student paid stored in database.
                    string paidUpon = dtps.Rows[0].ItemArray[15].ToString();
                    string paid2P = dtps.Rows[0].ItemArray[16].ToString();
                    string paid3P = dtps.Rows[0].ItemArray[17].ToString();
                    string paid4P = dtps.Rows[0].ItemArray[18].ToString();
                    string paid5P = dtps.Rows[0].ItemArray[19].ToString();
                    string paid6P = dtps.Rows[0].ItemArray[20].ToString();
                    string paid7P = dtps.Rows[0].ItemArray[21].ToString();
                    string paid8P = dtps.Rows[0].ItemArray[22].ToString();
                    string paid9P = dtps.Rows[0].ItemArray[23].ToString();
                    string paid10P = dtps.Rows[0].ItemArray[24].ToString();
                    //these are the dates when the student paid in database
                    string dateupon = dtps.Rows[0].ItemArray[5].ToString();
                    string dpay2 = dtps.Rows[0].ItemArray[6].ToString();
                    string dpay3 = dtps.Rows[0].ItemArray[7].ToString();
                    string dpay4 = dtps.Rows[0].ItemArray[8].ToString();
                    string dpay5 = dtps.Rows[0].ItemArray[9].ToString();
                    string dpay6 = dtps.Rows[0].ItemArray[10].ToString();
                    string dpay7 = dtps.Rows[0].ItemArray[11].ToString();
                    string dpay8 = dtps.Rows[0].ItemArray[12].ToString();
                    string dpay9 = dtps.Rows[0].ItemArray[13].ToString();
                    string dpay10 = dtps.Rows[0].ItemArray[14].ToString();

                    double change = 0.00;
                    double currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                    string slev = dtps.Rows[0].ItemArray[1].ToString();

                    if (isAdvance == true)
                    {
                        advamt = Convert.ToDouble(txtATP.Text);
                    }
                   
                    string fiftyDisc = "";
                    string FreeLastMonthTotal = "";
                    string fiftyDiscTotal = "";
                    double discountedAmtFiftyDisc = 0;
                    double DiscountedTotalFreeLastMonth = 0;
                    double DiscountedTotalFiftyDisc = 0;
                    double balanceCheck = currentbal;

                    string levdep = "";
                    con.Open();
                    OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + slev + "'", con);
                    DataTable dtdep = new DataTable();
                    dadep.Fill(dtdep);
                    con.Close();
                    if (dtdep.Rows.Count > 0)
                    {
                        levdep = dtdep.Rows[0].ItemArray[0].ToString();
                    }


                    if (slev == "Kinder")
                    {
                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "REGISTRATION" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        OdbcDataAdapter dak2 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "MISCELLANEOUS" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk2 = new DataTable();
                        dak2.Fill(dtk2);
                       
                        if (dtk1.Rows.Count > 0) { regiAmt_rec = dtk1.Rows[0].ItemArray[2].ToString(); }//CASTRO
                        if (dtk2.Rows.Count > 0) { miscAmt_rec = dtk2.Rows[0].ItemArray[2].ToString(); }
                       
                        con.Open();
                        OdbcDataAdapter dak = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk = new DataTable();
                        dak.Fill(dtk);
                        con.Close();

                        if (dtk.Rows.Count > 0)
                        {
                            for (int h = 0; h < dtk.Rows.Count; h++)
                            {
                                if (dtk.Rows[h].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                                {
                                    annual = Convert.ToDouble(dtk.Rows[h].ItemArray[2].ToString());
                                    annualstring = dtk.Rows[h].ItemArray[2].ToString();
                                }
                                if (dtk.Rows[h].ItemArray[1].ToString() == "UPON ENROLLMENT")
                                {
                                    upone = Convert.ToDouble(dtk.Rows[h].ItemArray[2].ToString());
                                    uponestring = dtk.Rows[h].ItemArray[2].ToString();
                                }
                                if (dtk.Rows[h].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                                {
                                    monthlyi = Convert.ToDouble(dtk.Rows[h].ItemArray[2].ToString());
                                    monthlystring = dtk.Rows[h].ItemArray[2].ToString();

                                    double comp1 = monthlyi * .5;
                                    discountedAmtFiftyDisc = monthlyi - comp1;
                                    fiftyDisc = discountedAmtFiftyDisc.ToString();
                                    double annualamt = Convert.ToDouble(annualstring);
                                    DiscountedTotalFreeLastMonth = annualamt - monthlyi;
                                    FreeLastMonthTotal = DiscountedTotalFreeLastMonth.ToString();
                                    DiscountedTotalFiftyDisc = annualamt - discountedAmtFiftyDisc;
                                    fiftyDiscTotal= DiscountedTotalFiftyDisc.ToString();
                                }
                            }
                        }
                    }
                    if (slev == "Grade 1" || slev == "Grade 2" || slev == "Grade 3" || slev == "Grade 4" || slev == "Grade 5" || slev == "Grade 6")
                    {
                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "REGISTRATION" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        OdbcDataAdapter dak2 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "MISCELLANEOUS" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk2 = new DataTable();
                        dak2.Fill(dtk2);
                        
                        if (dtk1.Rows.Count > 0) { regiAmt_rec = dtk1.Rows[0].ItemArray[2].ToString(); }//CASTRO
                        if (dtk2.Rows.Count > 0) { miscAmt_rec = dtk2.Rows[0].ItemArray[2].ToString(); }
                        
                        con.Open();
                        OdbcDataAdapter dak = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk = new DataTable();
                        dak.Fill(dtk);
                        con.Close();

                        if (dtk.Rows.Count > 0)
                        {
                            for (int h = 0; h < dtk.Rows.Count; h++)
                            {
                                if (dtk.Rows[h].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                                {
                                    annual = Convert.ToDouble(dtk.Rows[h].ItemArray[2].ToString());
                                    annualstring = dtk.Rows[h].ItemArray[2].ToString();
                                }
                                if (dtk.Rows[h].ItemArray[1].ToString() == "UPON ENROLLMENT")
                                {
                                    upone = Convert.ToDouble(dtk.Rows[h].ItemArray[2].ToString());
                                    uponestring = dtk.Rows[h].ItemArray[2].ToString();
                                }
                                if (dtk.Rows[h].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                                {
                                    monthlyi = Convert.ToDouble(dtk.Rows[h].ItemArray[2].ToString());
                                    monthlystring = dtk.Rows[h].ItemArray[2].ToString();

                                    double comp1 = monthlyi * .5;
                                    discountedAmtFiftyDisc = monthlyi - comp1;
                                    fiftyDisc = discountedAmtFiftyDisc.ToString();
                                    double annualamt = Convert.ToDouble(annualstring);
                                    DiscountedTotalFreeLastMonth = annualamt - monthlyi;
                                    FreeLastMonthTotal = DiscountedTotalFreeLastMonth.ToString();
                                    DiscountedTotalFiftyDisc = annualamt - discountedAmtFiftyDisc;
                                    fiftyDiscTotal = DiscountedTotalFiftyDisc.ToString();
                                }
                            }
                        }
                    }
                    if (slev == "Grade 7" || slev == "Grade 8" || slev == "Grade 9" || slev == "Grade 10")
                    {
                        OdbcDataAdapter dak1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "REGISTRATION" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk1 = new DataTable();
                        dak1.Fill(dtk1);
                        OdbcDataAdapter dak2 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE'" + "MISCELLANEOUS" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk2 = new DataTable();
                        dak2.Fill(dtk2);
                       
                        if (dtk1.Rows.Count > 0) { regiAmt_rec = dtk1.Rows[0].ItemArray[2].ToString(); }//CASTRO
                        if (dtk2.Rows.Count > 0) { miscAmt_rec = dtk2.Rows[0].ItemArray[2].ToString(); }
                        
                        con.Open();
                        OdbcDataAdapter dak = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk = new DataTable();
                        dak.Fill(dtk);
                        con.Close();

                        if (dtk.Rows.Count > 0)
                        {
                            for (int h = 0; h < dtk.Rows.Count; h++)
                            {
                                if (dtk.Rows[h].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                                {
                                    annual = Convert.ToDouble(dtk.Rows[h].ItemArray[2].ToString());
                                    annualstring = dtk.Rows[h].ItemArray[2].ToString();
                                }
                                if (dtk.Rows[h].ItemArray[1].ToString() == "UPON ENROLLMENT")
                                {
                                    upone = Convert.ToDouble(dtk.Rows[h].ItemArray[2].ToString());
                                    uponestring = dtk.Rows[h].ItemArray[2].ToString();
                                }
                                if (dtk.Rows[h].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                                {
                                    monthlyi = Convert.ToDouble(dtk.Rows[h].ItemArray[2].ToString());
                                    monthlystring = dtk.Rows[h].ItemArray[2].ToString();

                                    double comp1 = monthlyi * .5;
                                    discountedAmtFiftyDisc = monthlyi - comp1;
                                    fiftyDisc= discountedAmtFiftyDisc.ToString();
                                    double annualamt = Convert.ToDouble(annualstring);
                                    DiscountedTotalFreeLastMonth = annualamt - monthlyi;
                                    FreeLastMonthTotal= DiscountedTotalFreeLastMonth.ToString();
                                    DiscountedTotalFiftyDisc = annualamt - discountedAmtFiftyDisc;
                                    fiftyDiscTotal= DiscountedTotalFiftyDisc.ToString();
                                }
                            }
                        }
                    }

                    
                    if (isAdvance == true && cash <advamt)
                    {
                        return;
                    }
                    else
                    {
                        if (pue == "" && p2 == "" && p3 == "" && p4 == "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
                        {
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
                                    //error dito
                                    newbal = DiscountedTotalFreeLastMonth - upone;
                                    change = cash - upone;
                                    annualstring = FreeLastMonthTotal;
                                   
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    newbal = DiscountedTotalFiftyDisc - upone;
                                    change = cash - upone;
                                    annualstring = fiftyDiscTotal;
                                   
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);
                                    newbal = discountedTotalOtherDisc - upone;
                                    change = cash - upone;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                  
                                }
                            }
                            else
                            {
                                newbal = annual - upone;
                                change = cash - upone;
                               
                            }

                            assessment_rec = annualstring;
                           
                            if (balanceCheck< upone)
                            {
                                if (cash < balanceCheck)
                                {
                                    btnPTC.Enabled = false;
                                    newbal = balanceCheck;
                                    change = 0.00;
                                   
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "UPON ENROLLMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P "+AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    balanceCheck -= cash;
                                    newbal = balanceCheck;
                                }
                                else
                                {//first

                                    bool iscomputed = false;
                                    newbal = balanceCheck;
                                    if (newbal <= change && isPayChange == true && iscomputed == false)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        newbal += upone;
                                        payamount_rec = upone.ToString();
                                        theAmountPaidToSet = newbal.ToString();

                                        string AmtToDisplay = "";
                                        if (newbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), newbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), newbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "UPON ENROLLMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= newbal;
                                        if (balanceCheck <= 0)
                                        {
                                            newbal = 0.00;
                                        }

                                        txtChange.Text = change.ToString();
                                        iscomputed = true;
                                    }
                                    if (change <= currentbal && isPayChange == true && iscomputed == false)
                                    {

                                        double ATP = 0;
                                        ATP = upone + change;
                                        newbal -= change;

                                        addAmt_rec = change.ToString();
                                        payamount_rec = upone.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "UPON ENROLLMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);
                                        iscomputed = true;

                                    }
                                    if (currentbal <= cash && isPayChange == false && iscomputed == false)
                                    {
                                        change = cash - newbal;

                                        payamount_rec = newbal.ToString();
                                        theAmountPaidToSet = newbal.ToString();

                                        string AmtToDisplay = "";
                                        if (newbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), newbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), newbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "UPON ENROLLMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        newbal = 0.00;
                                        iscomputed = true;
                                    }
                                    
                                    

                                }
                                      
                                if (newbal <= 0)
                                {
                                    newbal = 0.00;
                                }
                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    newbal = balanceCheck;
                                    if (newbal <= change)
                                    {
                                        change -= newbal;
                                        addAmt_rec = newbal.ToString();
                                      
                                        newbal += upone;
                                        payamount_rec = upone.ToString();
                                        theAmountPaidToSet = newbal.ToString();

                                        string AmtToDisplay = "";
                                        if (newbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), newbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), newbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "UPON ENROLLMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= newbal;
                                        if (balanceCheck <= 0)
                                        {
                                            newbal= 0.00;
                                        }
                                    }
                                    else
                                    {
                                        newbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = upone + change;
                                        newbal -= change;
                                       
                                        addAmt_rec = change.ToString();
                                        payamount_rec = upone.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp2 = new ListViewItem();
                                        itmdp2.Text = "UPON ENROLLMENT";
                                        itmdp2.SubItems.Add(datetoday);
                                        itmdp2.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp2);

                                        if (newbal <= 0)
                                        {
                                            newbal = 0.00;
                                        }
                                    } 
                                }
                                else
                                {
                                 
                                    payamount_rec = uponestring;
                                    theAmountPaidToSet = uponestring;
                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "UPON ENROLLMENT";
                                    itmpd.SubItems.Add(datetoday);
                                    itmpd.SubItems.Add("P " + uponestring);
                                    lvwPH.Items.Add(itmpd);
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            desc_upon = "FIRST MONTH PAYMENT";
                            paymentNum_rec = "UPON ENROLLMENT";
                            tfAmt_rec = monthlyi.ToString();
                            balance_rec = newbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + newbal + "',amtupon='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            if (newbal <= 0)
                            {
                                string AmtToDisplaytot = "";
                                double anamt = Convert.ToDouble(annualstring);
                                if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                            txtChange.Text = change.ToString();    
                        }
                        if (pue != "" && p2 == "" && p3 == "" && p4 == "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
                        {      
                            double monthly = Convert.ToDouble(monthlystring);
                            double the_monthly =0;

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
                                    currentbal-= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = FreeLastMonthTotal;
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    currentbal-= monthly;
                                    change = cash -monthly;
                                    the_monthly = monthly;
                                    annualstring = fiftyDiscTotal;
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);
                                         
                                    currentbal -= InstallmentAmt_forOtherDisc;
                                    change = cash - InstallmentAmt_forOtherDisc;
                                    the_monthly = InstallmentAmt_forOtherDisc;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                }
                            }
                            else
                            {
                                currentbal-=monthlyi;
                                change = cash - monthlyi;
                                the_monthly = monthly;
                            }

                                  
                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P "+paidUpon.ToString());
                            lvwPH.Items.Add(itmdp);

                            assessment_rec = annualstring;
                            paymentNum_rec = "2ND MONTH PAYMENT";
                            if (balanceCheck <the_monthly)
                            {
                                if (cash < balanceCheck)
                                {
                                    btnPTC.Enabled = false;
                                    currentbal = balanceCheck;
                                    change = 0.00;
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "SECOND PAYMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    currentbal -= cash;
                                }
                                else
                                {
                                    bool iscomputed = false;
                                    currentbal = balanceCheck;
                                    if (currentbal <= change && isPayChange == true && iscomputed == false)
                                    {
                                       
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();//added
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SECOND PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                        txtChange.Text = change.ToString();
                                        iscomputed = true;
                                    }
                                    if (change <= currentbal && isPayChange == true && iscomputed == false)
                                    {
                                       
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SECOND PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);
                                        iscomputed = true;

                                    }
                                    if (currentbal <= cash && isPayChange == false && iscomputed == false)
                                    {
                                        change = cash - currentbal;
                                        paydesc_rec = "TUITION FEE";
                                        payamount_rec = currentbal.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SECOND PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        currentbal -= currentbal;
                                        iscomputed = true;

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }

                                }

                                //if (currentbal <= 0)
                                //{
                                //    currentbal = 0.00;
                                //}
                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    currentbal = balanceCheck;
                                    if (currentbal <= change)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SECOND PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                    else
                                    {
                                        currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp2 = new ListViewItem();
                                        itmdp2.Text = "SECOND PAYMENT";
                                        itmdp2.SubItems.Add(datetoday);
                                        itmdp2.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp2);

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }
                                else
                                {
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = the_monthly.ToString();
                                    theAmountPaidToSet = the_monthly.ToString();

                                    string AmtToDisplay = "";
                                    if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"), the_monthly); }
                                    ListViewItem itmdp2 = new ListViewItem();
                                    itmdp2.Text = "SECOND PAYMENT";
                                    itmdp2.SubItems.Add(datetoday);
                                    itmdp2.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp2);
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            balance_rec = currentbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + currentbal + "',amt2p='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            if (currentbal <= 0)
                            {
                                string AmtToDisplaytot = "";
                                double anamt = Convert.ToDouble(annualstring);
                                if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                            txtChange.Text = change.ToString();

                        }
                        if (pue != "" && p2 != "" && p3 == "" && p4 == "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
                        {
                            double monthly = Convert.ToDouble(monthlystring);
                            double the_monthly = 0;

                            con.Open();//current
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                                if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = FreeLastMonthTotal;
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = fiftyDiscTotal;
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);

                                    currentbal -= InstallmentAmt_forOtherDisc;
                                    change = cash - InstallmentAmt_forOtherDisc;
                                    the_monthly = InstallmentAmt_forOtherDisc;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                }
                            }
                            else
                            {
                                currentbal -= monthlyi;
                                change = cash - monthlyi;
                                the_monthly = monthly;
                            }

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P "+paidUpon.ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + paid2P.ToString());
                            lvwPH.Items.Add(itmdp2);

                            assessment_rec = annualstring;
                            paymentNum_rec = "3RD MONTH PAYMENT";
                            if (balanceCheck < the_monthly)
                            {
                                if (cash < balanceCheck)
                                {

                                    btnPTC.Enabled = false;
                                    currentbal = balanceCheck;
                                    change = 0.00;
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "THIRD PAYMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    currentbal -= cash;
                                }
                                else
                                {
                                    bool iscomputed = false;
                                    currentbal = balanceCheck;
                                    if (currentbal <= change && isPayChange == true && iscomputed == false)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();//added
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "THIRD PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                        iscomputed = true;
                                    }
                                    if (change <= currentbal && isPayChange == true && iscomputed == false)
                                    {
                                       
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "THIRD PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);
                                        iscomputed = true;
                                    }
                                    if (currentbal <= cash && isPayChange == false && iscomputed == false)
                                    {
                                        change = cash - currentbal;
                                        paydesc_rec = "TUITION FEE";
                                        payamount_rec = currentbal.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "THIRD PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        currentbal -= currentbal;
                                        iscomputed = true;

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }

                                }

                                //if (currentbal <= 0)
                                //{
                                //    currentbal = 0.00;
                                //}
                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    currentbal = balanceCheck;
                                    if (currentbal <= change)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "THIRD PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                    else
                                    {
                                        currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp3 = new ListViewItem();
                                        itmdp3.Text = "THIRD PAYMENT";
                                        itmdp3.SubItems.Add(datetoday);
                                        itmdp3.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp3);

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }
                                else
                                {
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = the_monthly.ToString();
                                    theAmountPaidToSet = the_monthly.ToString();

                                    string AmtToDisplay = "";
                                    if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"),the_monthly); }
                                    ListViewItem itmdp3 = new ListViewItem();
                                    itmdp3.Text = "THIRD PAYMENT";
                                    itmdp3.SubItems.Add(datetoday);
                                    itmdp3.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp3);
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            balance_rec = currentbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + currentbal + "',amt3p='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            if (currentbal <= 0)
                            {
                                string AmtToDisplaytot = "";
                                double anamt = Convert.ToDouble(annualstring);
                                if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                            txtChange.Text = change.ToString();
                        }
                        if (pue != "" && p2 != "" && p3 != "" && p4 == "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
                        {
                            double monthly = Convert.ToDouble(monthlystring);
                            double the_monthly = 0;

                            con.Open();//current
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                                if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = FreeLastMonthTotal;
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = fiftyDiscTotal;
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);

                                    currentbal -= InstallmentAmt_forOtherDisc;
                                    change = cash - InstallmentAmt_forOtherDisc;
                                    the_monthly = InstallmentAmt_forOtherDisc;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                }
                            }
                            else
                            {
                                currentbal -= monthlyi;
                                change = cash - monthlyi;
                                the_monthly = monthly;
                            }

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + paidUpon.ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + paid2P.ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + paid3P.ToString());
                            lvwPH.Items.Add(itmdp3);

                            assessment_rec = annualstring;
                            paymentNum_rec = "4TH MONTH PAYMENT";
                            if (balanceCheck < the_monthly)
                            {
                                if (cash < balanceCheck)
                                {
                                    btnPTC.Enabled = false;
                                    currentbal = balanceCheck;
                                    change = 0;
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "FOURTH PAYMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    currentbal -= cash;
                                }
                                else
                                {
                                    bool iscomputed = false;
                                    currentbal = balanceCheck;
                                    if (currentbal <= change && isPayChange == true && iscomputed == false)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();//added
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "FOURTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }

                                        txtChange.Text = change.ToString();
                                        iscomputed = true;
                                    }
                                    if (change <= currentbal && isPayChange == true && iscomputed == false)
                                    {
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "FOURTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);
                                        iscomputed = true;
                                    }
                                    if (currentbal <= cash && isPayChange == false && iscomputed == false)
                                    {
                                        change = cash - currentbal;
                                        paydesc_rec = "TUITION FEE";
                                        payamount_rec = currentbal.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "FOURTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        currentbal -= currentbal;
                                        iscomputed = true;

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }

                                   
                                }

                                //if (currentbal <= 0)
                                //{
                                //    currentbal = 0.00;
                                //}
                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    currentbal = balanceCheck;
                                    if (currentbal <= change)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "FOURTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                    else
                                    {
                                        currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp4 = new ListViewItem();
                                        itmdp4.Text = "FOURTH PAYMENT";
                                        itmdp4.SubItems.Add(datetoday);
                                        itmdp4.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp4);

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }
                                else
                                {
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = the_monthly.ToString();
                                    theAmountPaidToSet = the_monthly.ToString();

                                    string AmtToDisplay = "";
                                    if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"), the_monthly); }
                                    ListViewItem itmdp4 = new ListViewItem();
                                    itmdp4.Text = "FOURTH PAYMENT";
                                    itmdp4.SubItems.Add(datetoday);
                                    itmdp4.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp4);
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            balance_rec = currentbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + currentbal + "',amt4p='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            if (currentbal <= 0)
                            {
                                string AmtToDisplaytot = "";
                                double anamt = Convert.ToDouble(annualstring);
                                if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                            txtChange.Text = change.ToString();
                        }
                        if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
                        {
                            double monthly = Convert.ToDouble(monthlystring);
                            double the_monthly = 0;

                            con.Open();//current
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                                if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = FreeLastMonthTotal;
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = fiftyDiscTotal;
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);

                                    currentbal -= InstallmentAmt_forOtherDisc;
                                    change = cash - InstallmentAmt_forOtherDisc;
                                    the_monthly = InstallmentAmt_forOtherDisc;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                }
                            }
                            else
                            {
                                currentbal -= monthlyi;
                                change = cash - monthlyi;
                                the_monthly = monthly;
                            }


                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + paidUpon.ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + paid2P.ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + paid3P.ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + paid4P.ToString());
                            lvwPH.Items.Add(itmdp4);

                            assessment_rec = annualstring;
                            paymentNum_rec = "5TH MONTH PAYMENT";
                            if (balanceCheck < the_monthly)
                            {
                                if (cash < balanceCheck)
                                {
                                    btnPTC.Enabled = false;
                                    currentbal = balanceCheck;
                                    change = 0.00;
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "FIFTH PAYMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    currentbal -= cash;
                                }
                                else
                                {
                                    bool iscomputed = false;
                                    currentbal = balanceCheck;
                                    if (currentbal <= change && isPayChange == true && iscomputed == false)
                                    {                                      
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();//added
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "FIFTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                        txtChange.Text = change.ToString();
                                        iscomputed = true;
                                    }
                                    if (change <= currentbal && isPayChange == true && iscomputed == false)
                                    {
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "FIFTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);
                                        iscomputed = true;
                                    }
                                    if (currentbal <= cash && isPayChange == false && iscomputed == false)
                                    {
                                        change = cash - currentbal;
                                        paydesc_rec = "TUITION FEE";
                                        payamount_rec = currentbal.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "FIFTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        currentbal -= currentbal;
                                        iscomputed = true;

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }

                                //if (currentbal <= 0)
                                //{
                                //    currentbal = 0.00;
                                //}
                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    currentbal = balanceCheck;
                                    if (currentbal <= change)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "FIFTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                    else
                                    {
                                        currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp5 = new ListViewItem();
                                        itmdp5.Text = "FIFTH PAYMENT";
                                        itmdp5.SubItems.Add(datetoday);
                                        itmdp5.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp5);

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }
                                else
                                {
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = the_monthly.ToString();
                                    theAmountPaidToSet = the_monthly.ToString();

                                    string AmtToDisplay = "";
                                    if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"), the_monthly); }
                                    ListViewItem itmdp5 = new ListViewItem();
                                    itmdp5.Text = "FIFTH PAYMENT";
                                    itmdp5.SubItems.Add(datetoday);
                                    itmdp5.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp5);
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            balance_rec = currentbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + currentbal + "',amt5p='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            if (currentbal <= 0)
                            {
                                string AmtToDisplaytot = "";
                                double anamt = Convert.ToDouble(annualstring);
                                if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                            txtChange.Text = change.ToString();//p5 na
                        }
                        if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
                        {
                            double monthly = Convert.ToDouble(monthlystring);
                            double the_monthly = 0;

                            con.Open();//current
                            OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDisc = new DataTable();
                            daDisc.Fill(dtDisc);
                            con.Close();
                            if (dtDisc.Rows.Count > 0)
                            {
                                string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                                if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = FreeLastMonthTotal;
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = fiftyDiscTotal;
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);

                                    currentbal -= InstallmentAmt_forOtherDisc;
                                    change = cash - InstallmentAmt_forOtherDisc;
                                    the_monthly = InstallmentAmt_forOtherDisc;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                }
                            }
                            else
                            {
                                currentbal -= monthlyi;
                                change = cash - monthlyi;
                                the_monthly = monthly;
                            }

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + paidUpon.ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + paid2P.ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + paid3P.ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + paid4P.ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + paid5P.ToString());
                            lvwPH.Items.Add(itmdp5);

                            assessment_rec = annualstring;
                            paymentNum_rec = "6TH MONTH PAYMENT";
                            if (balanceCheck < the_monthly)
                            {
                                if (cash < balanceCheck)
                                {
                                    btnPTC.Enabled = false;
                                    currentbal = balanceCheck;
                                    change = 0.00;
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "SIXTH PAYMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    currentbal -= cash;
                                }
                                else
                                {
                                    bool iscomputed = false;
                                    currentbal = balanceCheck;
                                    if (currentbal <= change && isPayChange == true && iscomputed == false)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();//added
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();//currentbal
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SIXTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                        iscomputed = true;
                                    }
                                    if (change <= currentbal && isPayChange == true && iscomputed == false)
                                    {
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SIXTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);
                                        iscomputed = true;

                                    }
                                    if (currentbal <= cash && isPayChange == false && iscomputed == false)
                                    {
                                        change = cash - currentbal;
                                        paydesc_rec = "TUITION FEE";
                                        payamount_rec = currentbal.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SIXTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        currentbal -= currentbal;
                                        iscomputed = true;

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }

                                }

                                //if (currentbal <= 0)
                                //{
                                //    currentbal = 0.00;
                                //}

                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    currentbal = balanceCheck;
                                    if (currentbal <= change)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SIXTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                    else
                                    {
                                        
                                        currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp6 = new ListViewItem();
                                        itmdp6.Text = "SIXTH PAYMENT";
                                        itmdp6.SubItems.Add(datetoday);
                                        itmdp6.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp6);

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }
                                else
                                {
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = the_monthly.ToString();
                                    theAmountPaidToSet = the_monthly.ToString();

                                    string AmtToDisplay = "";
                                    if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"), the_monthly); }
                                    ListViewItem itmdp6 = new ListViewItem();
                                    itmdp6.Text = "SIXTH PAYMENT";
                                    itmdp6.SubItems.Add(datetoday);
                                    itmdp6.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp6);
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            balance_rec = currentbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + currentbal + "',amt6p='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            if (currentbal <= 0)
                            {
                                string AmtToDisplaytot = "";
                                double anamt = Convert.ToDouble(annualstring);
                                if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                            txtChange.Text = change.ToString();

                        }
                        if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 != "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
                        {
                            double monthly = Convert.ToDouble(monthlystring);
                            double the_monthly = 0;

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
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = FreeLastMonthTotal;
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = fiftyDiscTotal;
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);

                                    currentbal -= InstallmentAmt_forOtherDisc;
                                    change = cash - InstallmentAmt_forOtherDisc;
                                    the_monthly = InstallmentAmt_forOtherDisc;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                }
                            }
                            else
                            {
                                currentbal -= monthlyi;
                                change = cash - monthlyi;
                                the_monthly = monthly;
                            }

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + paidUpon.ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + paid2P.ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + paid3P.ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + paid4P.ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + paid5P.ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + paid6P.ToString());
                            lvwPH.Items.Add(itmdp6);

                            assessment_rec = annualstring;
                            paymentNum_rec = "7TH MONTH PAYMENT";
                            if (balanceCheck < the_monthly)
                            {
                                if (cash < balanceCheck)
                                {
                                    btnPTC.Enabled = false;
                                    currentbal = balanceCheck;
                                 
                                    change = 0.00;
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "SEVENTH PAYMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    currentbal -= cash;
                                }
                                else
                                {
                                   bool iscomputed = false;
                                   currentbal = balanceCheck;
                                   if (currentbal <= change && isPayChange == true && iscomputed == false)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();//added
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SEVENTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                        iscomputed = true;
                                    }

                                   if (change <= currentbal && isPayChange == true && iscomputed == false)
                                    {
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SEVENTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);
                                        iscomputed = true;

                                    }
                                   if (currentbal <= cash && isPayChange == false && iscomputed == false)
                                    {
                                        change = cash - currentbal;
                                        paydesc_rec = "TUITION FEE";
                                        payamount_rec = currentbal.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SEVENTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        currentbal -= currentbal;
                                        iscomputed = true;

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }

                                }

                                //if (currentbal <= 0)
                                //{
                                //    currentbal = 0.00;
                                //}
                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    currentbal = balanceCheck;
                                    if (currentbal <= change)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "SEVENTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                    else
                                    {
                                        currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp7 = new ListViewItem();
                                        itmdp7.Text = "SEVENTH PAYMENT";
                                        itmdp7.SubItems.Add(datetoday);
                                        itmdp7.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp7);

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }
                                else
                                {
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = the_monthly.ToString();
                                    theAmountPaidToSet = the_monthly.ToString();

                                    string AmtToDisplay = "";
                                    if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"), the_monthly); }
                                    ListViewItem itmdp7 = new ListViewItem();
                                    itmdp7.Text = "SEVENTH PAYMENT";
                                    itmdp7.SubItems.Add(datetoday);
                                    itmdp7.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp7);
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            balance_rec = currentbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + currentbal + "',amt7p='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            if (currentbal <= 0)
                            {
                                string AmtToDisplaytot = "";
                                double anamt = Convert.ToDouble(annualstring);
                                if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                            txtChange.Text = change.ToString();

                        }
                        if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 != "" && p7 != "" && p8 == "" && p9 == "" && p10 == "")
                        {//peig
                            double monthly = Convert.ToDouble(monthlystring);
                            double the_monthly = 0;

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
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = FreeLastMonthTotal;
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = fiftyDiscTotal;
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);

                                    currentbal -= InstallmentAmt_forOtherDisc;
                                    change = cash - InstallmentAmt_forOtherDisc;
                                    the_monthly = InstallmentAmt_forOtherDisc;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                }
                            }
                            else
                            {
                                currentbal -= monthlyi;
                                change = cash - monthlyi;
                                the_monthly = monthly;
                            }

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + paidUpon.ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + paid2P.ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + paid3P.ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + paid4P.ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + paid5P.ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + paid6P.ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + paid7P.ToString());
                            lvwPH.Items.Add(itmdp7);

                            assessment_rec = annualstring;
                            paymentNum_rec = "8TH  MONTH PAYMENT";
                            if (balanceCheck < the_monthly)
                            {
                                if (cash < balanceCheck)
                                {
                                    btnPTC.Enabled = false;
                                    currentbal = balanceCheck;
                                    change = 0.00;
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "EIGHT PAYMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    currentbal -= cash;
                                }
                                else
                                {
                                    bool iscomputed = false;
                                   currentbal = balanceCheck;
                                   if (currentbal <= change && isPayChange == true && iscomputed == false)
                                    {
                                      
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();//added
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "EIGHT PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                        iscomputed = true;
                                    }
                                   if (change <= currentbal && isPayChange == true && iscomputed == false)
                                    {
                                      
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "EIGHT PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);
                                        iscomputed = true;
                                    }
                                   if (currentbal <= cash && isPayChange == false && iscomputed == false)
                                    {
                                        change = cash - currentbal;
                                        paydesc_rec = "TUITION FEE";
                                        payamount_rec = currentbal.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "EIGHT PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        currentbal -= currentbal;
                                        iscomputed = true;

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }

                                        
                                }

                               // if (currentbal <= 0)
                               // {
                               //     currentbal = 0.00;
                                //}
                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    currentbal = balanceCheck;
                                    if (currentbal <= change)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "EIGHTTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                    else
                                    {
                                        currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp8 = new ListViewItem();
                                        itmdp8.Text = "EIGHTTH PAYMENT";
                                        itmdp8.SubItems.Add(datetoday);
                                        itmdp8.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp8);

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }
                                else
                                {
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = the_monthly.ToString();
                                    theAmountPaidToSet = the_monthly.ToString();

                                    string AmtToDisplay = "";
                                    if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"), the_monthly); }
                                    ListViewItem itmdp8 = new ListViewItem();
                                    itmdp8.Text = "EIGHTTH PAYMENT";
                                    itmdp8.SubItems.Add(datetoday);
                                    itmdp8.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp8);
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            balance_rec = currentbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + currentbal + "',amt8p='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            if (currentbal <= 0)
                            {
                                string AmtToDisplaytot = "";
                                double anamt = Convert.ToDouble(annualstring);
                                if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                                lvwPH.Items.Add(itmdpsumm);
                            }
                            txtChange.Text = change.ToString();
                        }
                        if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 != "" && p7 != "" && p8 != "" && p9 == "" && p10 == "")
                        {
                            double monthly = Convert.ToDouble(monthlystring);
                            double the_monthly = 0;

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
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = FreeLastMonthTotal;
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = fiftyDiscTotal;
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);

                                    currentbal -= InstallmentAmt_forOtherDisc;
                                    change = cash - InstallmentAmt_forOtherDisc;
                                    the_monthly = InstallmentAmt_forOtherDisc;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                }
                            }
                            else
                            {
                                currentbal -= monthlyi;
                                change = cash - monthlyi;
                                the_monthly = monthly;
                            }

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + paidUpon.ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + paid2P.ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + paid3P.ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + paid4P.ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + paid5P.ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + paid6P.ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + paid7P.ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + paid8P.ToString());
                            lvwPH.Items.Add(itmdp8);

                            assessment_rec = annualstring;
                            paymentNum_rec = "9TH MONTH PAYMENT";
                            if (balanceCheck < the_monthly)
                            {
                                if (cash < balanceCheck)
                                {
                                    btnPTC.Enabled = false;
                                    currentbal = balanceCheck;
                                    change = 0.00;
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "NINETH PAYMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    currentbal -= cash;
                                }
                                else
                                {
                                    bool iscomputed = false;
                                    currentbal = balanceCheck;
                                    if (currentbal <= change && isPayChange == true && iscomputed == false)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();//added
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec =the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "NINETH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                        iscomputed = true;
                                    }
                                    if (change <= currentbal && isPayChange == true && iscomputed == false)
                                    {
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "NINETH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);
                                        iscomputed = true;
                                    }

                                    if (currentbal <= cash && isPayChange == false && iscomputed == false)
                                    {
                                        change = cash - currentbal;
                                        paydesc_rec = "TUITION FEE";
                                        payamount_rec = currentbal.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "NINETH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        currentbal -= currentbal;
                                        iscomputed = true;

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }

                                }

                                //if (currentbal <= 0)
                                //{
                                //    currentbal = 0.00;
                                //}
                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    currentbal = balanceCheck;
                                    if (currentbal <= change)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "NINETH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                    else
                                    {
                                        currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp9 = new ListViewItem();
                                        itmdp9.Text = "NINETH PAYMENT";
                                        itmdp9.SubItems.Add(datetoday);
                                        itmdp9.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp9); ;

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }
                                else
                                {
                                    
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = the_monthly.ToString();
                                    theAmountPaidToSet = the_monthly.ToString();

                                    string AmtToDisplay = "";
                                    if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"), the_monthly); }
                                    ListViewItem itmdp9 = new ListViewItem();
                                    itmdp9.Text = "NINETH PAYMENT";
                                    itmdp9.SubItems.Add(datetoday);
                                    itmdp9.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp9);
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            balance_rec = currentbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + currentbal + "',amt9p='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            txtChange.Text = change.ToString();


                            //this will check if the student is free for the last month
                            con.Open();
                            OdbcDataAdapter daDiscFLM = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDiscFLM = new DataTable();
                            daDiscFLM.Fill(dtDiscFLM);
                            con.Close();
                            if (dtDiscFLM.Rows.Count > 0)
                            {
                                string discountType = dtDiscFLM.Rows[0].ItemArray[1].ToString();
                                if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
                                {

                                    if (currentbal <= 0)
                                    {
                                        ListViewItem itmdp10 = new ListViewItem();
                                        itmdp10.Text = "TENTH PAYMENT";
                                        itmdp10.SubItems.Add("");
                                        itmdp10.SubItems.Add("P " + "0.00");
                                        lvwPH.Items.Add(itmdp10);

                                        double amt = Convert.ToDouble(FreeLastMonthTotal);
                                        if (amt >= 1000) { FreeLastMonthTotal = String.Format(("{0:0,###.00#}"), amt); } else { FreeLastMonthTotal = String.Format(("{0:0.00#}"), amt); }
                                        ListViewItem itmdsum = new ListViewItem();
                                        itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                        itmdsum.Text = "Total:";
                                        itmdsum.SubItems.Add("");//free error
                                        itmdsum.SubItems.Add("P " + FreeLastMonthTotal);
                                        lvwPH.Items.Add(itmdsum);
                                    }
                                }
                            }
                            else
                            {
                                /*ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add(annualstring);
                                lvwPH.Items.Add(itmdpsumm);*/
                                if (currentbal <= 0)
                                {
                                    string AmtToDisplaytot = "";
                                    double anamt = Convert.ToDouble(annualstring);
                                    if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }
                                    ListViewItem itmdpsumm = new ListViewItem();
                                    itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdpsumm.Text = "Total:";
                                    itmdpsumm.SubItems.Add("");
                                    itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                                    lvwPH.Items.Add(itmdpsumm);
                                }
                            }
                        }
                        if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 != "" && p7 != "" && p8 != "" && p9 != "" && p10 == "")
                        {//TEN NA
                            double monthly = Convert.ToDouble(monthlystring);
                            double the_monthly = 0;

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
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = FreeLastMonthTotal;
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    currentbal -= monthly;
                                    change = cash - monthly;
                                    the_monthly = monthly;
                                    annualstring = fiftyDiscTotal;
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    retrieveMonthlyInstallmentAmt_OtherDisc(discountType);

                                    currentbal -= InstallmentAmt_forOtherDisc;
                                    change = cash - InstallmentAmt_forOtherDisc;
                                    the_monthly = InstallmentAmt_forOtherDisc;
                                    annualstring = discountedTotalOtherDisc.ToString();
                                }
                            }
                            else
                            {
                                currentbal -= monthlyi;
                                change = cash - monthlyi;
                                the_monthly = monthly;
                            }

                            ListViewItem itmdp = new ListViewItem();
                            itmdp.Text = "UPON ENROLLMENT";
                            itmdp.SubItems.Add(dateupon);
                            itmdp.SubItems.Add("P " + paidUpon.ToString());
                            lvwPH.Items.Add(itmdp);

                            ListViewItem itmdp2 = new ListViewItem();
                            itmdp2.Text = "SECOND PAYMENT";
                            itmdp2.SubItems.Add(dpay2);
                            itmdp2.SubItems.Add("P " + paid2P.ToString());
                            lvwPH.Items.Add(itmdp2);

                            ListViewItem itmdp3 = new ListViewItem();
                            itmdp3.Text = "THIRD PAYMENT";
                            itmdp3.SubItems.Add(dpay3);
                            itmdp3.SubItems.Add("P " + paid3P.ToString());
                            lvwPH.Items.Add(itmdp3);

                            ListViewItem itmdp4 = new ListViewItem();
                            itmdp4.Text = "FOURTH PAYMENT";
                            itmdp4.SubItems.Add(dpay4);
                            itmdp4.SubItems.Add("P " + paid4P.ToString());
                            lvwPH.Items.Add(itmdp4);

                            ListViewItem itmdp5 = new ListViewItem();
                            itmdp5.Text = "FIFTH PAYMENT";
                            itmdp5.SubItems.Add(dpay5);
                            itmdp5.SubItems.Add("P " + paid5P.ToString());
                            lvwPH.Items.Add(itmdp5);

                            ListViewItem itmdp6 = new ListViewItem();
                            itmdp6.Text = "SIXTH PAYMENT";
                            itmdp6.SubItems.Add(dpay6);
                            itmdp6.SubItems.Add("P " + paid6P.ToString());
                            lvwPH.Items.Add(itmdp6);

                            ListViewItem itmdp7 = new ListViewItem();
                            itmdp7.Text = "SEVENTH PAYMENT";
                            itmdp7.SubItems.Add(dpay7);
                            itmdp7.SubItems.Add("P " + paid7P.ToString());
                            lvwPH.Items.Add(itmdp7);

                            ListViewItem itmdp8 = new ListViewItem();
                            itmdp8.Text = "EIGHTTH PAYMENT";
                            itmdp8.SubItems.Add(dpay8);
                            itmdp8.SubItems.Add("P " + paid8P.ToString());
                            lvwPH.Items.Add(itmdp8);

                            ListViewItem itmdp9 = new ListViewItem();
                            itmdp9.Text = "NINETH PAYMENT";
                            itmdp9.SubItems.Add(dpay9);
                            itmdp9.SubItems.Add("P " + paid9P.ToString());
                            lvwPH.Items.Add(itmdp9);

                            assessment_rec = annualstring;
                            paymentNum_rec = "10TH MONTH PAYMENT";
                            if (balanceCheck < the_monthly)
                            {
                                if (cash < balanceCheck)
                                {
                                    btnPTC.Enabled = false;
                                    currentbal = balanceCheck;
                                    change =0;
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = cash.ToString();
                                    theAmountPaidToSet = cash.ToString();

                                    string AmtToDisplay = "";
                                    if (cash >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), cash); } else { AmtToDisplay = String.Format(("{0:0.00#}"), cash); }
                                    ListViewItem itmdp61 = new ListViewItem();
                                    itmdp61.Text = "TENTH PAYMENT";
                                    itmdp61.SubItems.Add(datetoday);
                                    itmdp61.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp61);

                                    currentbal -= cash;
                                }
                                else
                                {
                                    if (isPayChange == true)
                                    {
                                        bool iscomputed = false;
                                        currentbal = balanceCheck;
                                        if (currentbal <= change && iscomputed == false)
                                        {
                                            change -= currentbal;
                                            addAmt_rec = currentbal.ToString();//added
                                            paydesc_rec = "TUITION FEE";
                                            currentbal += the_monthly;
                                            payamount_rec = the_monthly.ToString();
                                            theAmountPaidToSet = currentbal.ToString();

                                            string AmtToDisplay = "";
                                            if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                            ListViewItem itmdp61 = new ListViewItem();
                                            itmdp61.Text = "TENTH PAYMENT";
                                            itmdp61.SubItems.Add(datetoday);
                                            itmdp61.SubItems.Add("P " + AmtToDisplay);
                                            lvwPH.Items.Add(itmdp61);

                                            balanceCheck -= currentbal;
                                            if (balanceCheck <= 0)
                                            {
                                                currentbal = 0.00;
                                            }
                                            iscomputed = true;
                                        }
                                        if (change <= currentbal && iscomputed == false)
                                        {
                                            double ATP = 0;
                                            ATP = the_monthly + change;
                                            currentbal -= change;
                                            paydesc_rec = "TUITION FEE";
                                            addAmt_rec = change.ToString();
                                            payamount_rec = the_monthly.ToString();
                                            theAmountPaidToSet = ATP.ToString();
                                            change = 0.00;
                                            btnPTC.Enabled = false;

                                            string AmtToDisplay = "";
                                            if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                            ListViewItem itmdp61 = new ListViewItem();
                                            itmdp61.Text = "TENTH PAYMENT";
                                            itmdp61.SubItems.Add(datetoday);
                                            itmdp61.SubItems.Add("P " + AmtToDisplay);
                                            lvwPH.Items.Add(itmdp61);
                                            iscomputed = true;
                                        }
                                    }
                                    else
                                    {
                                        if (currentbal <= cash)
                                        {
                                            currentbal = balanceCheck;
                                            change = cash - currentbal;
                                            paydesc_rec = "TUITION FEE";
                                            payamount_rec = currentbal.ToString();
                                            theAmountPaidToSet = currentbal.ToString();

                                            string AmtToDisplay = "";
                                            if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                            ListViewItem itmdp61 = new ListViewItem();
                                            itmdp61.Text = "TENTH PAYMENT";
                                            itmdp61.SubItems.Add(datetoday);
                                            itmdp61.SubItems.Add("P " + AmtToDisplay);
                                            lvwPH.Items.Add(itmdp61);

                                            currentbal -= currentbal;

                                            if (currentbal <= 0)
                                            {
                                                currentbal = 0.00;
                                            }
                                          
                                        }
                                    }
                                }

                                //if (currentbal <= 0)
                                //{
                                //    currentbal = 0.00;
                               // }
                                        
                            }
                            else
                            {
                                if (isPayChange == true)
                                {
                                    currentbal = balanceCheck;
                                    if (currentbal <= change)
                                    {
                                        change -= currentbal;
                                        addAmt_rec = currentbal.ToString();
                                        paydesc_rec = "TUITION FEE";
                                        currentbal += the_monthly;
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = currentbal.ToString();

                                        string AmtToDisplay = "";
                                        if (currentbal >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), currentbal); } else { AmtToDisplay = String.Format(("{0:0.00#}"), currentbal); }
                                        ListViewItem itmdp61 = new ListViewItem();
                                        itmdp61.Text = "TENTH PAYMENT";
                                        itmdp61.SubItems.Add(datetoday);
                                        itmdp61.SubItems.Add("P " + AmtToDisplay);
                                        lvwPH.Items.Add(itmdp61);

                                        balanceCheck -= currentbal;
                                        if (balanceCheck <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                    else
                                    {
                                        currentbal = Convert.ToDouble(dtps.Rows[0].ItemArray[4].ToString());
                                        double ATP = 0;
                                        ATP = the_monthly + change;
                                        currentbal -= change;
                                        paydesc_rec = "TUITION FEE";
                                        addAmt_rec = change.ToString();
                                        payamount_rec = the_monthly.ToString();
                                        theAmountPaidToSet = ATP.ToString();
                                        change = 0.00;
                                        btnPTC.Enabled = false;

                                        string AmtToDisplay = "";
                                        if (ATP >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), ATP); } else { AmtToDisplay = String.Format(("{0:0.00#}"), ATP); }
                                        ListViewItem itmdp10 = new ListViewItem();
                                        itmdp10.Text = "TENTH PAYMENT";
                                        itmdp10.SubItems.Add(datetoday);
                                        lvwPH.Items.Add(itmdp10);
                                        itmdp10.SubItems.Add("P " + AmtToDisplay);

                                        if (currentbal <= 0)
                                        {
                                            currentbal = 0.00;
                                        }
                                    }
                                }
                                else
                                {
                                }
                            }

                            double formatTAPTS = Convert.ToDouble(theAmountPaidToSet);
                            if (formatTAPTS >= 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0,###.00#}"), formatTAPTS);
                            }
                            if (formatTAPTS < 1000)
                            {
                                theAmountPaidToSet = string.Format(("{0:0.00#}"), formatTAPTS);
                            }

                            balance_rec = currentbal.ToString();
                            con.Open();
                            string setToPaid = "Update paymentmonthly_tbl set balance='" + currentbal + "',amt10p='" + theAmountPaidToSet + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                            cmdtopd.ExecuteNonQuery();
                            con.Close();

                            txtChange.Text = change.ToString();

                            con.Open();
                            OdbcDataAdapter daDiscFD = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                            DataTable dtDiscFD = new DataTable();
                            daDiscFD.Fill(dtDiscFD);
                            con.Close();
                            if (dtDiscFD.Rows.Count > 0)
                            {
                                string discountType = dtDiscFD.Rows[0].ItemArray[1].ToString();
                                   //total leeber 
                                if (discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)
                                {
                                    string AmtToDisplay = "";
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = the_monthly.ToString();
                                    theAmountPaidToSet = the_monthly.ToString();
                               
                                    if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"), the_monthly); }
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add(datetoday);
                                    itmdp10.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdp10);

                                    double anamt = Convert.ToDouble(annualstring);
                                    if (anamt >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplay = String.Format(("{0:0.00#}"), anamt); }
                                    ListViewItem itmdpsumm = new ListViewItem();
                                    itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdpsumm.Text = "Total:";
                                    itmdpsumm.SubItems.Add("");
                                    itmdpsumm.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdpsumm);
                                }
                                if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                                {
                                    paydesc_rec = "TUITION FEE";
                                    payamount_rec = fiftyDisc.ToString();
                                   
                                    /*ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT(DISC.)";
                                    itmdp10.SubItems.Add(datetoday);
                                    itmdp10.SubItems.Add("P " + fiftyDisc);
                                    lvwPH.Items.Add(itmdp10);*/
                                    //kindererrorr
                                    string AmtToDisplay = "";
                                    double anamt = Convert.ToDouble(annualstring);
                                    if (anamt >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplay = String.Format(("{0:0.00#}"), anamt); }
                                    ListViewItem itmdpsumm = new ListViewItem();
                                    itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdpsumm.Text = "Total:";
                                    itmdpsumm.SubItems.Add("");
                                    itmdpsumm.SubItems.Add("P " + AmtToDisplay);
                                    lvwPH.Items.Add(itmdpsumm);
                                }
                            }
                            else
                            {
                                paydesc_rec = "TUITION FEE";
                                payamount_rec = the_monthly.ToString();
                                theAmountPaidToSet = the_monthly.ToString();

                                string AmtToDisplay = "";
                                if (the_monthly >= 1000) { AmtToDisplay = String.Format(("{0:0,###.00#}"), the_monthly); } else { AmtToDisplay = String.Format(("{0:0.00#}"), the_monthly); }
                                ListViewItem itmdp10 = new ListViewItem();
                                itmdp10.Text = "TENTH PAYMENT";
                                itmdp10.SubItems.Add(datetoday);
                                itmdp10.SubItems.Add("P " + AmtToDisplay);
                                lvwPH.Items.Add(itmdp10);

                                /*ListViewItem itmdp10 = new ListViewItem();
                                itmdp10.Text = "TENTH PAYMENT";
                                itmdp10.SubItems.Add(datetoday);
                                itmdp10.SubItems.Add(currentbal.ToString());
                                lvwPH.Items.Add(itmdp10);*/

                               
                               string AmtToDisplaytot = "";
                               double anamt = Convert.ToDouble(annualstring);
                               if (anamt >= 1000) { AmtToDisplaytot = String.Format(("{0:0,###.00#}"), anamt); } else { AmtToDisplaytot = String.Format(("{0:0.00#}"), anamt); }       
                               ListViewItem itmdpsumm = new ListViewItem();
                               itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                               itmdpsumm.Text = "Total:";
                               itmdpsumm.SubItems.Add("");
                               itmdpsumm.SubItems.Add("P " + AmtToDisplaytot);
                               lvwPH.Items.Add(itmdpsumm);
                            }      
                        }

                        //UPDATE THE DISPLAY
                        setupPaymentSummary();
                    }//end of else cash>monthly installment

                    if (currentbal <= 0) { btnPTC.Enabled = false; } else { btnPTC.Enabled = true; }
                    if (change <= 0) { txtChange.Text = "0"; btnPTC.Enabled = false; }
                    if (change >0 && currentbal<=0) { txtChange.Text = "0"; btnPTC.Enabled = false; }

                    txtChange.Text = change.ToString();
                    setupEnroleeENROLLED(slev);

                }//end dt.rows.count for monthly ins.

               
            }//end else of installment

            
            double changedisplay = Convert.ToDouble(txtChange.Text);
            double cashamtdisplay = Convert.ToDouble(txtCashAmt.Text);
            if (changedisplay >= 1000)
            { txtChange.Text = String.Format(("{0:#,###.00#}"), Convert.ToDouble(changedisplay)); }
            if (changedisplay < 1000 && changedisplay > 0) { txtChange.Text = String.Format(("{0:#.00#}"), Convert.ToDouble(changedisplay)); }
            if (changedisplay <= 0) { txtChange.Text = "0.00"; }
            txtSearch.Focus();

            if (isPayChange == false)
            {
                setupTransactionNum();
            }
            paychange_rec = txtChange.Text;
            paycash_rec = String.Format(("{0:#,###,###.##}"), Convert.ToDouble(cashamtdisplay));

            btnPrintReceipt.Enabled = true;
            btnPPrev.Enabled = true;
            
          

        }

        public void setupEnroleeENROLLED(string slev)
        {
            
            if (slev == "Kinder")
            {
                con.Open();
                OdbcDataAdapter daOfficialStud = new OdbcDataAdapter("Select*from offprereg_tbl where studno='" + txtSnum.Text + "'", con);
                DataTable dtOS = new DataTable();
                daOfficialStud.Fill(dtOS);
                con.Close();

                if (dtOS.Rows.Count > 0)
                {
                    string fn = dtOS.Rows[0].ItemArray[1].ToString();
                    string mn = dtOS.Rows[0].ItemArray[2].ToString();
                    string ln = dtOS.Rows[0].ItemArray[3].ToString();
                    string le = dtOS.Rows[0].ItemArray[4].ToString();
                    string se = dtOS.Rows[0].ItemArray[5].ToString();
                    string sc = dtOS.Rows[0].ItemArray[6].ToString();
                    string ad = dtOS.Rows[0].ItemArray[7].ToString();
                    string bd = dtOS.Rows[0].ItemArray[8].ToString();
                    string ag = dtOS.Rows[0].ItemArray[9].ToString();
                    string ge = dtOS.Rows[0].ItemArray[10].ToString();
                    string co = dtOS.Rows[0].ItemArray[11].ToString();
                    string fa = dtOS.Rows[0].ItemArray[12].ToString();
                    string fo = dtOS.Rows[0].ItemArray[13].ToString();
                    string mt = dtOS.Rows[0].ItemArray[14].ToString();
                    string mo = dtOS.Rows[0].ItemArray[15].ToString();
                    string gu = dtOS.Rows[0].ItemArray[16].ToString();
                    string go = dtOS.Rows[0].ItemArray[17].ToString();
                    string pg = dtOS.Rows[0].ItemArray[18].ToString();
                    string ta = dtOS.Rows[0].ItemArray[19].ToString();
                    string aw = dtOS.Rows[0].ItemArray[20].ToString();
                    string sr = dtOS.Rows[0].ItemArray[21].ToString();
                    string mp = dtOS.Rows[0].ItemArray[22].ToString();
                    string re = dtOS.Rows[0].ItemArray[23].ToString();
                    string syre = dtOS.Rows[0].ItemArray[24].ToString();
                    string sibGrantee = dtOS.Rows[0].ItemArray[25].ToString();
                    string sibDescrip = dtOS.Rows[0].ItemArray[26].ToString();
                    string sibProvider = dtOS.Rows[0].ItemArray[27].ToString();

                    con.Open();
                    string AddAsOfficialStud = "Insert Into stud_tbl(studno,fname,mname,lname,level,section,school,address,birthdate,age,gender,studcon,fathername,fatheroccup,mothername,motheroccup,guardian,guardianoccup,pgcon,talentskill,award,subreq,mop,syenrolled,status,guardianrelation,syregistered)values('" +
                    txtSnum.Text + "','" + fn + "','" + mn + "','" + ln + "','" + le + "','" + se + "','" + sc + "','" + ad + "','" + bd + "','" + ag + "','" + ge + "','" + co + "','" + fa + "','" + fo + "','" + mt + "','" + mo + "','" + gu + "','" + go + "','" + pg + "','" + ta + "','" + aw + "','" + sr + "','" + mp + "','" + txtSY.Text + "','" + "Active" + "','" + re + "','"+syre+"')";
                    OdbcCommand cmdOS = new OdbcCommand(AddAsOfficialStud, con);
                    cmdOS.ExecuteNonQuery();
                    con.Close();

                    if (sibGrantee != "" && sibDescrip != "" && sibProvider != "")
                    {
                        con.Open();
                        string AddSibDiscOldYoung = "Insert Into studdiscounted_tbl(studno,disctype,provider)values('" +
                        sibGrantee + "','" + sibDescrip + "','" + sibProvider + "')";
                        OdbcCommand cmdSDOY = new OdbcCommand(AddSibDiscOldYoung, con);
                        cmdSDOY.ExecuteNonQuery();
                        con.Close();

                        setUpLessSiblingDiscountOldYoung(sibGrantee);
                    }

                    con.Open();
                    string deleteAsRegister = "Delete from offprereg_tbl where studno='" + txtSnum.Text + "'";
                    OdbcCommand cmddel = new OdbcCommand(deleteAsRegister, con);
                    cmddel.ExecuteNonQuery();
                    con.Close();


                    con.Open();
                    OdbcDataAdapter dasa = new OdbcDataAdapter("Select*from studacct_tbl where studno='" + txtSnum.Text + "'", con);
                    DataTable dtsa = new DataTable();
                    dasa.Fill(dtsa);
                    con.Close();
                    if (dtsa.Rows.Count > 0)
                    {
                        con.Open();
                        string del = "Delete from studacct_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelacct = new OdbcCommand(del, con);
                        cmddelacct.ExecuteNonQuery();
                        con.Close();
                    }
                    con.Open();
                    string AddstudAcct = "Insert Into studacct_tbl(studno,username,password)values('" +
                    txtSnum.Text + "','" + "" + "','" + "" + "')";
                    OdbcCommand cmdsa = new OdbcCommand(AddstudAcct, con);
                    cmdsa.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    string updatestat = "Update stud_tbl set status='" + "Active" + "'where studno='" + txtSnum.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(updatestat, con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //this will assign section to student
                    setupSectioning(le, txtSnum.Text);
                    viewthesection();

                    con.Open();
                    OdbcDataAdapter dasub = new OdbcDataAdapter("Select*from subject_tbl where level='" + "Kinder" + "'", con);
                    DataTable dtsub = new DataTable();
                    dasub.Fill(dtsub);
                    con.Close();

                    if (dtsub.Rows.Count > 0)
                    {
                        for (int s = 0; s < dtsub.Rows.Count; s++)
                        {
                            con.Open();
                            string add = "Insert Into Kindergrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                            dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                            OdbcCommand cmdsub = new OdbcCommand(add, con);
                            cmdsub.ExecuteNonQuery();
                            con.Close();
                        }

                        con.Open();
                        string update = "Update Kindergrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(update, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }//end of level kinder
            if (slev == "Grade 1" || slev == "Grade 2" || slev == "Grade 3" || slev == "Grade 4" || slev == "Grade 5" || slev == "Grade 6")
            {
                con.Open();
                OdbcDataAdapter daOfficialStud = new OdbcDataAdapter("Select*from offprereg_tbl where studno='" + txtSnum.Text + "'", con);
                DataTable dtOS = new DataTable();
                daOfficialStud.Fill(dtOS);
                con.Close();

                if (dtOS.Rows.Count > 0)
                {
                    string fn = dtOS.Rows[0].ItemArray[1].ToString();
                    string mn = dtOS.Rows[0].ItemArray[2].ToString();
                    string ln = dtOS.Rows[0].ItemArray[3].ToString();
                    string le = dtOS.Rows[0].ItemArray[4].ToString();
                    string se = dtOS.Rows[0].ItemArray[5].ToString();
                    string sc = dtOS.Rows[0].ItemArray[6].ToString();
                    string ad = dtOS.Rows[0].ItemArray[7].ToString();
                    string bd = dtOS.Rows[0].ItemArray[8].ToString();
                    string ag = dtOS.Rows[0].ItemArray[9].ToString();
                    string ge = dtOS.Rows[0].ItemArray[10].ToString();
                    string co = dtOS.Rows[0].ItemArray[11].ToString();
                    string fa = dtOS.Rows[0].ItemArray[12].ToString();
                    string fo = dtOS.Rows[0].ItemArray[13].ToString();
                    string mt = dtOS.Rows[0].ItemArray[14].ToString();
                    string mo = dtOS.Rows[0].ItemArray[15].ToString();
                    string gu = dtOS.Rows[0].ItemArray[16].ToString();
                    string go = dtOS.Rows[0].ItemArray[17].ToString();
                    string pg = dtOS.Rows[0].ItemArray[18].ToString();
                    string ta = dtOS.Rows[0].ItemArray[19].ToString();
                    string aw = dtOS.Rows[0].ItemArray[20].ToString();
                    string sr = dtOS.Rows[0].ItemArray[21].ToString();
                    string mp = dtOS.Rows[0].ItemArray[22].ToString();
                    string re = dtOS.Rows[0].ItemArray[23].ToString();
                    string syre = dtOS.Rows[0].ItemArray[24].ToString();
                    string sibGrantee = dtOS.Rows[0].ItemArray[25].ToString();
                    string sibDescrip = dtOS.Rows[0].ItemArray[26].ToString();
                    string sibProvider = dtOS.Rows[0].ItemArray[27].ToString();

                    con.Open();
                    string AddAsOfficialStud = "Insert Into stud_tbl(studno,fname,mname,lname,level,section,school,address,birthdate,age,gender,studcon,fathername,fatheroccup,mothername,motheroccup,guardian,guardianoccup,pgcon,talentskill,award,subreq,mop,syenrolled,status,guardianrelation,syregistered)values('" +
                    txtSnum.Text + "','" + fn + "','" + mn + "','" + ln + "','" + le + "','" + se + "','" + sc + "','" + ad + "','" + bd + "','" + ag + "','" + ge + "','" + co + "','" + fa + "','" + fo + "','" + mt + "','" + mo + "','" + gu + "','" + go + "','" + pg + "','" + ta + "','" + aw + "','" + sr + "','" + mp + "','" + txtSY.Text + "','" + "Active" + "','" + re + "','"+syre+"')";
                    OdbcCommand cmdOS = new OdbcCommand(AddAsOfficialStud, con);
                    cmdOS.ExecuteNonQuery();
                    con.Close();

                    if (sibGrantee != "" && sibDescrip != "" && sibProvider != "")
                    {
                        con.Open();
                        string AddSibDiscOldYoung = "Insert Into studdiscounted_tbl(studno,disctype,provider)values('" +
                        sibGrantee + "','" + sibDescrip + "','" + sibProvider + "')";
                        OdbcCommand cmdSDOY = new OdbcCommand(AddSibDiscOldYoung, con);
                        cmdSDOY.ExecuteNonQuery();
                        con.Close();

                        setUpLessSiblingDiscountOldYoung(sibGrantee);
                    }

                    con.Open();
                    string deleteAsRegister = "Delete from offprereg_tbl where studno='" + txtSnum.Text + "'";
                    OdbcCommand cmddel = new OdbcCommand(deleteAsRegister, con);
                    cmddel.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    OdbcDataAdapter dasa = new OdbcDataAdapter("Select*from studacct_tbl where studno='" + txtSnum.Text + "'", con);
                    DataTable dtsa = new DataTable();
                    dasa.Fill(dtsa);
                    con.Close();
                    if (dtsa.Rows.Count > 0)
                    {
                        con.Open();
                        string del = "Delete from studacct_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelacct = new OdbcCommand(del, con);
                        cmddelacct.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string AddstudAcct = "Insert Into studacct_tbl(studno,username,password)values('" +
                    txtSnum.Text + "','" + "" + "','" + "" + "')";
                    OdbcCommand cmdsa = new OdbcCommand(AddstudAcct, con);
                    cmdsa.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    string updatestat = "Update stud_tbl set status='" + "Active" + "'where studno='" + txtSnum.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(updatestat, con);
                    cmd.ExecuteNonQuery();
                    con.Close();


                    //this will assign section to student
                    setupSectioning(le, txtSnum.Text);
                    viewthesection();

                    con.Open();
                    OdbcDataAdapter dasub = new OdbcDataAdapter("Select*from subject_tbl where level='" + txtGrd.Text + "'", con);
                    DataTable dtsub = new DataTable();
                    dasub.Fill(dtsub);
                    con.Close();

                    if (dtsub.Rows.Count > 0)
                    {
                        if (txtGrd.Text == "Grade 1")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradeonegrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradeonegrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();

                        }
                        if (txtGrd.Text == "Grade 2")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradetwogrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradetwogrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();

                        }
                        if (txtGrd.Text == "Grade 3")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradethreegrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradethreegrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();

                        }
                        if (txtGrd.Text == "Grade 4")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradefourgrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradefourgrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();

                        }
                        if (txtGrd.Text == "Grade 5")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradefivegrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradefivegrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();

                        }
                        if (txtGrd.Text == "Grade 6")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradesixgrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradesixgrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                else//for those old students
                {
                    con.Open();
                    OdbcDataAdapter daOfficialStud2 = new OdbcDataAdapter("Select*from offprereg_old_tbl where studno='" + txtSnum.Text + "'", con);
                    DataTable dtOS2 = new DataTable();
                    daOfficialStud2.Fill(dtOS2);
                    con.Close();

                    if (dtOS2.Rows.Count > 0)
                    {
                        string fn = dtOS2.Rows[0].ItemArray[1].ToString();
                        string mn = dtOS2.Rows[0].ItemArray[2].ToString();
                        string ln = dtOS2.Rows[0].ItemArray[3].ToString();
                        string le = dtOS2.Rows[0].ItemArray[4].ToString();
                        string se = dtOS2.Rows[0].ItemArray[5].ToString();
                        string sc = dtOS2.Rows[0].ItemArray[6].ToString();
                        string ad = dtOS2.Rows[0].ItemArray[7].ToString();
                        string bd = dtOS2.Rows[0].ItemArray[8].ToString();
                        string ag = dtOS2.Rows[0].ItemArray[9].ToString();
                        string ge = dtOS2.Rows[0].ItemArray[10].ToString();
                        string co = dtOS2.Rows[0].ItemArray[11].ToString();
                        string fa = dtOS2.Rows[0].ItemArray[12].ToString();
                        string fo = dtOS2.Rows[0].ItemArray[13].ToString();
                        string mt = dtOS2.Rows[0].ItemArray[14].ToString();
                        string mo = dtOS2.Rows[0].ItemArray[15].ToString();
                        string gu = dtOS2.Rows[0].ItemArray[16].ToString();
                        string go = dtOS2.Rows[0].ItemArray[17].ToString();
                        string pg = dtOS2.Rows[0].ItemArray[18].ToString();
                        string ta = dtOS2.Rows[0].ItemArray[19].ToString();
                        string aw = dtOS2.Rows[0].ItemArray[20].ToString();
                        string sr = dtOS2.Rows[0].ItemArray[21].ToString();
                        string mp = dtOS2.Rows[0].ItemArray[22].ToString();
                        string sy = dtOS2.Rows[0].ItemArray[23].ToString();
                        string syre = dtOS2.Rows[0].ItemArray[24].ToString();
                        string pgrel = dtOS2.Rows[0].ItemArray[25].ToString();
                        string sibGrantee = dtOS2.Rows[0].ItemArray[26].ToString();
                        string sibDescrip = dtOS2.Rows[0].ItemArray[27].ToString();
                        string sibProvider = dtOS2.Rows[0].ItemArray[28].ToString();

                        con.Open();
                        string AddAsOfficialStud = "Insert Into stud_tbl(studno,fname,mname,lname,level,section,school,address,birthdate,age,gender,studcon,fathername,fatheroccup,mothername,motheroccup,guardian,guardianoccup,pgcon,talentskill,award,subreq,mop,syenrolled,status,syregistered,guardianrelation)values('" +
                        txtSnum.Text + "','" + fn + "','" + mn + "','" + ln + "','" + le + "','" + se + "','" + sc + "','" + ad + "','" + bd + "','" + ag + "','" + ge + "','" + co + "','" + fa + "','" + fo + "','" + mt + "','" + mo + "','" + gu + "','" + go + "','" + pg + "','" + ta + "','" + aw + "','" + sr + "','" + mp + "','" + sy + "','" + "Active" + "','"+syre+"','"+pgrel+"')";
                        OdbcCommand cmdOS = new OdbcCommand(AddAsOfficialStud, con);
                        cmdOS.ExecuteNonQuery();
                        con.Close();

                        if (sibGrantee != "" && sibDescrip != "" && sibProvider != "")
                        {
                            con.Open();
                            string AddSibDiscOldYoung = "Insert Into studdiscounted_tbl(studno,disctype,provider)values('" +
                            sibGrantee + "','" + sibDescrip + "','" + sibProvider + "')";
                            OdbcCommand cmdSDOY = new OdbcCommand(AddSibDiscOldYoung, con);
                            cmdSDOY.ExecuteNonQuery();
                            con.Close();

                            setUpLessSiblingDiscountOldYoung(sibGrantee);
                        }

                        con.Open();
                        string deleteAsRegister = "Delete from offprereg_old_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddel = new OdbcCommand(deleteAsRegister, con);
                        cmddel.ExecuteNonQuery();
                        con.Close();


                        con.Open();
                        OdbcDataAdapter dasa = new OdbcDataAdapter("Select*from studacct_tbl where studno='" + txtSnum.Text + "'", con);
                        DataTable dtsa = new DataTable();
                        dasa.Fill(dtsa);
                        con.Close();
                        if (dtsa.Rows.Count > 0)
                        {
                            con.Open();
                            string del = "Delete from studacct_tbl where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmddelacct = new OdbcCommand(del, con);
                            cmddelacct.ExecuteNonQuery();
                            con.Close();
                        }
                        con.Open();
                        string AddstudAcct = "Insert Into studacct_tbl(studno,username,password)values('" +
                        txtSnum.Text + "','" + "" + "','" + "" + "')";
                        OdbcCommand cmdsa = new OdbcCommand(AddstudAcct, con);
                        cmdsa.ExecuteNonQuery();
                        con.Close();

                        con.Open();
                        string updatestat = "Update stud_tbl set status='" + "Active" + "'where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmd = new OdbcCommand(updatestat, con);
                        cmd.ExecuteNonQuery();
                        con.Close();


                        //this will assign section to student
                        setupSectioning(le, txtSnum.Text);
                        viewthesection();

                        con.Open();
                        OdbcDataAdapter dasub = new OdbcDataAdapter("Select*from subject_tbl where level='" + txtGrd.Text + "'", con);
                        DataTable dtsub = new DataTable();
                        dasub.Fill(dtsub);
                        con.Close();

                        if (dtsub.Rows.Count > 0)
                        {
                            if (txtGrd.Text == "Grade 1")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradeonegrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradeonegrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();
                            }
                            if (txtGrd.Text == "Grade 2")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradetwogrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradetwogrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();

                            }
                            if (txtGrd.Text == "Grade 3")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradethreegrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradethreegrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();

                            }
                            if (txtGrd.Text == "Grade 4")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradefourgrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradefourgrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();

                            }
                            if (txtGrd.Text == "Grade 5")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradefivegrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradefivegrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();

                            }
                            if (txtGrd.Text == "Grade 6")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradesixgrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradesixgrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                }
            }//end of level elem
            if (slev == "Grade 7" || slev == "Grade 8" || slev == "Grade 9" || slev == "Grade 10")
            {
                con.Open();
                OdbcDataAdapter daOfficialStud = new OdbcDataAdapter("Select*from offprereg_tbl where studno='" + txtSnum.Text + "'", con);
                DataTable dtOS = new DataTable();
                daOfficialStud.Fill(dtOS);
                con.Close();

                if (dtOS.Rows.Count > 0)
                {
                    string fn = dtOS.Rows[0].ItemArray[1].ToString();
                    string mn = dtOS.Rows[0].ItemArray[2].ToString();
                    string ln = dtOS.Rows[0].ItemArray[3].ToString();
                    string le = dtOS.Rows[0].ItemArray[4].ToString();
                    string se = dtOS.Rows[0].ItemArray[5].ToString();
                    string sc = dtOS.Rows[0].ItemArray[6].ToString();
                    string ad = dtOS.Rows[0].ItemArray[7].ToString();
                    string bd = dtOS.Rows[0].ItemArray[8].ToString();
                    string ag = dtOS.Rows[0].ItemArray[9].ToString();
                    string ge = dtOS.Rows[0].ItemArray[10].ToString();
                    string co = dtOS.Rows[0].ItemArray[11].ToString();
                    string fa = dtOS.Rows[0].ItemArray[12].ToString();
                    string fo = dtOS.Rows[0].ItemArray[13].ToString();
                    string mt = dtOS.Rows[0].ItemArray[14].ToString();
                    string mo = dtOS.Rows[0].ItemArray[15].ToString();
                    string gu = dtOS.Rows[0].ItemArray[16].ToString();
                    string go = dtOS.Rows[0].ItemArray[17].ToString();
                    string pg = dtOS.Rows[0].ItemArray[18].ToString();
                    string ta = dtOS.Rows[0].ItemArray[19].ToString();
                    string aw = dtOS.Rows[0].ItemArray[20].ToString();
                    string sr = dtOS.Rows[0].ItemArray[21].ToString();
                    string mp = dtOS.Rows[0].ItemArray[22].ToString();
                    string re = dtOS.Rows[0].ItemArray[23].ToString();
                    string syre = dtOS.Rows[0].ItemArray[24].ToString();
                    string sibGrantee = dtOS.Rows[0].ItemArray[25].ToString();
                    string sibDescrip = dtOS.Rows[0].ItemArray[26].ToString();
                    string sibProvider = dtOS.Rows[0].ItemArray[27].ToString();

                    con.Open();
                    string AddAsOfficialStud = "Insert Into stud_tbl(studno,fname,mname,lname,level,section,school,address,birthdate,age,gender,studcon,fathername,fatheroccup,mothername,motheroccup,guardian,guardianoccup,pgcon,talentskill,award,subreq,mop,syenrolled,status,guardianrelation,syregistered)values('" +
                    txtSnum.Text + "','" + fn + "','" + mn + "','" + ln + "','" + le + "','" + se + "','" + sc + "','" + ad + "','" + bd + "','" + ag + "','" + ge + "','" + co + "','" + fa + "','" + fo + "','" + mt + "','" + mo + "','" + gu + "','" + go + "','" + pg + "','" + ta + "','" + aw + "','" + sr + "','" + mp + "','" + txtSY.Text + "','" + "Active" + "','" + re + "','"+syre+"')";
                    OdbcCommand cmdOS = new OdbcCommand(AddAsOfficialStud, con);
                    cmdOS.ExecuteNonQuery();
                    con.Close();

                    if (sibGrantee != "" && sibDescrip != "" && sibProvider != "")
                    {
                        con.Open();
                        string AddSibDiscOldYoung = "Insert Into studdiscounted_tbl(studno,disctype,provider)values('" +
                        sibGrantee + "','" + sibDescrip + "','" + sibProvider + "')";
                        OdbcCommand cmdSDOY = new OdbcCommand(AddSibDiscOldYoung, con);
                        cmdSDOY.ExecuteNonQuery();
                        con.Close();

                        setUpLessSiblingDiscountOldYoung(sibGrantee);
                    }

                    con.Open();
                    string deleteAsRegister = "Delete from offprereg_tbl where studno='" + txtSnum.Text + "'";
                    OdbcCommand cmddel = new OdbcCommand(deleteAsRegister, con);
                    cmddel.ExecuteNonQuery();
                    con.Close();


                    con.Open();
                    OdbcDataAdapter dasa = new OdbcDataAdapter("Select*from studacct_tbl where studno='" + txtSnum.Text + "'", con);
                    DataTable dtsa = new DataTable();
                    dasa.Fill(dtsa);
                    con.Close();
                    if (dtsa.Rows.Count > 0)
                    {
                        con.Open();
                        string del = "Delete from studacct_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelacct = new OdbcCommand(del, con);
                        cmddelacct.ExecuteNonQuery();
                        con.Close();
                    }
                    con.Open();
                    string AddstudAcct = "Insert Into studacct_tbl(studno,username,password)values('" +
                    txtSnum.Text + "','" + "" + "','" + "" + "')";
                    OdbcCommand cmdsa = new OdbcCommand(AddstudAcct, con);
                    cmdsa.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    string updatestat = "Update stud_tbl set status='" + "Active" + "'where studno='" + txtSnum.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(updatestat, con);
                    cmd.ExecuteNonQuery();
                    con.Close();


                    //this will assign section to student
                    setupSectioning(le, txtSnum.Text);
                    viewthesection();


                    con.Open();
                    OdbcDataAdapter dasub = new OdbcDataAdapter("Select*from subject_tbl where level='" + txtGrd.Text + "'", con);
                    DataTable dtsub = new DataTable();
                    dasub.Fill(dtsub);
                    con.Close();

                    if (dtsub.Rows.Count > 0)
                    {
                        if (txtGrd.Text == "Grade 7")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradesevengrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradesevengrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();

                        }
                        if (txtGrd.Text == "Grade 8")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradeeightgrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradeeightgrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();

                        }
                        if (txtGrd.Text == "Grade 9")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradeninegrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradeninegrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();

                        }
                        if (txtGrd.Text == "Grade 10")
                        {
                            for (int s = 0; s < dtsub.Rows.Count; s++)
                            {
                                con.Open();
                                string add = "Insert Into gradetengrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                OdbcCommand cmdsub = new OdbcCommand(add, con);
                                cmdsub.ExecuteNonQuery();
                                con.Close();
                            }

                            con.Open();
                            string update = "Update gradetengrades_tbl set syenrolled='" + txtSY.Text + "'where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmd1 = new OdbcCommand(update, con);
                            cmd1.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                else//for those old students
                {
                    con.Open();
                    OdbcDataAdapter daOfficialStud2 = new OdbcDataAdapter("Select*from offprereg_old_tbl where studno='" + txtSnum.Text + "'", con);
                    DataTable dtOS2 = new DataTable();
                    daOfficialStud2.Fill(dtOS2);
                    con.Close();

                    if (dtOS2.Rows.Count > 0)
                    {
                        string fn = dtOS2.Rows[0].ItemArray[1].ToString();
                        string mn = dtOS2.Rows[0].ItemArray[2].ToString();
                        string ln = dtOS2.Rows[0].ItemArray[3].ToString();
                        string le = dtOS2.Rows[0].ItemArray[4].ToString();
                        string se = dtOS2.Rows[0].ItemArray[5].ToString();
                        string sc = dtOS2.Rows[0].ItemArray[6].ToString();
                        string ad = dtOS2.Rows[0].ItemArray[7].ToString();
                        string bd = dtOS2.Rows[0].ItemArray[8].ToString();
                        string ag = dtOS2.Rows[0].ItemArray[9].ToString();
                        string ge = dtOS2.Rows[0].ItemArray[10].ToString();
                        string co = dtOS2.Rows[0].ItemArray[11].ToString();
                        string fa = dtOS2.Rows[0].ItemArray[12].ToString();
                        string fo = dtOS2.Rows[0].ItemArray[13].ToString();
                        string mt = dtOS2.Rows[0].ItemArray[14].ToString();
                        string mo = dtOS2.Rows[0].ItemArray[15].ToString();
                        string gu = dtOS2.Rows[0].ItemArray[16].ToString();
                        string go = dtOS2.Rows[0].ItemArray[17].ToString();
                        string pg = dtOS2.Rows[0].ItemArray[18].ToString();
                        string ta = dtOS2.Rows[0].ItemArray[19].ToString();
                        string aw = dtOS2.Rows[0].ItemArray[20].ToString();
                        string sr = dtOS2.Rows[0].ItemArray[21].ToString();
                        string mp = dtOS2.Rows[0].ItemArray[22].ToString();
                        string sy = dtOS2.Rows[0].ItemArray[23].ToString();
                        string syre = dtOS2.Rows[0].ItemArray[24].ToString();
                        string pgrel = dtOS2.Rows[0].ItemArray[25].ToString();
                        string sibGrantee = dtOS2.Rows[0].ItemArray[26].ToString();
                        string sibDescrip = dtOS2.Rows[0].ItemArray[27].ToString();
                        string sibProvider = dtOS2.Rows[0].ItemArray[28].ToString();

                        con.Open();
                        string AddAsOfficialStud = "Insert Into stud_tbl(studno,fname,mname,lname,level,section,school,address,birthdate,age,gender,studcon,fathername,fatheroccup,mothername,motheroccup,guardian,guardianoccup,pgcon,talentskill,award,subreq,mop,syenrolled,status,syregistered,guardianrelation)values('" +
                        txtSnum.Text + "','" + fn + "','" + mn + "','" + ln + "','" + le + "','" + se + "','" + sc + "','" + ad + "','" + bd + "','" + ag + "','" + ge + "','" + co + "','" + fa + "','" + fo + "','" + mt + "','" + mo + "','" + gu + "','" + go + "','" + pg + "','" + ta + "','" + aw + "','" + sr + "','" + mp + "','" + sy + "','" + "Active" + "','"+syre+"','"+pgrel+"')";
                        OdbcCommand cmdOS = new OdbcCommand(AddAsOfficialStud, con);
                        cmdOS.ExecuteNonQuery();
                        con.Close();

                        if (sibGrantee != "" && sibDescrip != "" && sibProvider != "")
                        {
                            con.Open();
                            string AddSibDiscOldYoung = "Insert Into studdiscounted_tbl(studno,disctype,provider)values('" +
                            sibGrantee + "','" + sibDescrip + "','" + sibProvider + "')";
                            OdbcCommand cmdSDOY = new OdbcCommand(AddSibDiscOldYoung, con);
                            cmdSDOY.ExecuteNonQuery();
                            con.Close();

                            setUpLessSiblingDiscountOldYoung(sibGrantee);
                        }

                        con.Open();
                        string deleteAsRegister = "Delete from offprereg_old_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddel = new OdbcCommand(deleteAsRegister, con);
                        cmddel.ExecuteNonQuery();
                        con.Close();

                        con.Open();
                        OdbcDataAdapter dasa = new OdbcDataAdapter("Select*from studacct_tbl where studno='" + txtSnum.Text + "'", con);
                        DataTable dtsa = new DataTable();
                        dasa.Fill(dtsa);
                        con.Close();

                        if (dtsa.Rows.Count > 0)
                        {
                            con.Open();
                            string del = "Delete from studacct_tbl where studno='" + txtSnum.Text + "'";
                            OdbcCommand cmddelacct = new OdbcCommand(del, con);
                            cmddelacct.ExecuteNonQuery();
                            con.Close();
                        }
                        con.Open();
                        string AddstudAcct = "Insert Into studacct_tbl(studno,username,password)values('" +
                        txtSnum.Text + "','" + "" + "','" + "" + "')";
                        OdbcCommand cmdsa = new OdbcCommand(AddstudAcct, con);
                        cmdsa.ExecuteNonQuery();
                        con.Close();

                        con.Open();
                        string updatestat = "Update stud_tbl set status='" + "Active" + "'where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmd = new OdbcCommand(updatestat, con);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        //this will assign section to student
                        setupSectioning(le, txtSnum.Text);
                        viewthesection();

                        con.Open();
                        OdbcDataAdapter dasub = new OdbcDataAdapter("Select*from subject_tbl where level='" + txtGrd.Text + "'", con);
                        DataTable dtsub = new DataTable();
                        dasub.Fill(dtsub);
                        con.Close();

                        if (dtsub.Rows.Count > 0)
                        {
                            if (txtGrd.Text == "Grade 7")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradesevengrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradesevengrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();
                            }
                            if (txtGrd.Text == "Grade 8")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradeeightgrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradeeightgrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();

                            }
                            if (txtGrd.Text == "Grade 9")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradeninegrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradeninegrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();
                            }
                            if (txtGrd.Text == "Grade 10")
                            {
                                for (int s = 0; s < dtsub.Rows.Count; s++)
                                {
                                    con.Open();
                                    string add = "Insert Into gradetengrades_tbl(studno,subdesc,q1,q2,q3,q4,ave)values('" + txtSnum.Text + "','" +
                                    dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();
                                }

                                con.Open();
                                string update = "Update gradetengrades_tbl set syenrolled='" + sy + "'where studno='" + txtSnum.Text + "'";
                                OdbcCommand cmd1 = new OdbcCommand(update, con);
                                cmd1.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                }
            }
        }

        public void setUpLessSiblingDiscountOldYoung(string sibGrantee)
        {
            string department = "";
            double monthlyAmt = 0;
            double AnnualAmt = 0;
            string MOP = "";
            string granteeLev = "";

            con.Open();
            OdbcDataAdapter daRec = new OdbcDataAdapter("Select*from stud_tbl where studno='" + sibGrantee + "'", con);
            DataTable dtRec = new DataTable();
            daRec.Fill(dtRec);
            con.Close();
            if (dtRec.Rows.Count > 0)
            {
                MOP = dtRec.Rows[0].ItemArray[22].ToString();
                granteeLev = dtRec.Rows[0].ItemArray[4].ToString();
            }

            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + granteeLev + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                department = dtdep.Rows[0].ItemArray[0].ToString();
            }

            con.Open();
            OdbcDataAdapter daMI = new OdbcDataAdapter("Select*from fee_tbl where level='" + department + "'and SY='" + activeSY + "'", con);
            DataTable dtMI = new DataTable();
            daMI.Fill(dtMI);
            con.Close();
            if (dtMI.Rows.Count > 0)
            {
                if (dtMI.Rows[0].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                {
                    monthlyAmt = Convert.ToDouble(dtMI.Rows[0].ItemArray[2].ToString());
                }
                if (dtMI.Rows[0].ItemArray[1].ToString() == "ANNUAL PAYMENT")
                {
                    AnnualAmt = Convert.ToDouble(dtMI.Rows[0].ItemArray[2].ToString());
                }
            }


            if (MOP == "Cash")
            {
                con.Open();
                OdbcDataAdapter daC = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + sibGrantee + "'", con);
                DataTable dtC = new DataTable();
                daC.Fill(dtC);
                con.Close();
                if (dtC.Rows.Count > 0)
                {
                    double netAmount = Convert.ToDouble(dtC.Rows[0].ItemArray[2].ToString());
                    double discAmt = netAmount - monthlyAmt;

                    con.Open();
                    string update = "Update paymentcash_tbl set amount='" + discAmt + "'where studno='" + sibGrantee + "'";
                    OdbcCommand cmd = new OdbcCommand(update,con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            else
            {
                con.Open();
                OdbcDataAdapter daI = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + sibGrantee + "'", con);
                DataTable dtI = new DataTable();
                daI.Fill(dtI);
                con.Close();
                if (dtI.Rows.Count > 0)
                {
                    double Balance = Convert.ToDouble(dtI.Rows[0].ItemArray[4].ToString());
                    double discAmt = Balance - monthlyAmt;
                    double newAnnual = AnnualAmt - monthlyAmt;

                    con.Open();
                    string update = "Update paymentmonthly_tbl set annual='"+newAnnual+"',balance='" + discAmt + "'where studno='" + sibGrantee + "'";
                    OdbcCommand cmd = new OdbcCommand(update,con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void viewthesection()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='" + txtSnum.Text + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                txtSec.Text = dt.Rows[0].ItemArray[5].ToString();
            }
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }

            string key = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            string levstud = "";
            string levoff = "";
            txtChange.Clear();
            txtCashAmt.Clear();
            txtATP.Clear();
            isAdvance = false;
            //isPrint = false;
            btnPrintReceipt.Enabled = false;
            btnPPrev.Enabled = false;

            btnEnter.Enabled = true;

            lvwAssessment.Clear();
            lvwAssessment.Columns.Add("Fee description", 220, HorizontalAlignment.Left);
            lvwAssessment.Columns.Add("Amount", 130, HorizontalAlignment.Right);

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='" + key + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from offprereg_tbl where studno='" + key + "'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            OdbcDataAdapter da2 = new OdbcDataAdapter("Select*from offprereg_old_tbl where studno='" + key + "'", con);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            con.Close();

            //int CURRENTYR = Convert.ToInt32(DateTime.Now.Year.ToString());
            //int UPCOMING = CURRENTYR + 1;
            //string SY = "SY:" + CURRENTYR + "-" + UPCOMING;
            if (dt.Rows.Count > 0)
            {
               
               
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
            if (dt1.Rows.Count > 0)
            {
               
               
                txtSY.Text = activeSY;
                txtSnum.Text = key;
                txtLast.Text = dt1.Rows[0].ItemArray[3].ToString();
                txtFirst.Text = dt1.Rows[0].ItemArray[1].ToString();
                txtMid.Text = dt1.Rows[0].ItemArray[2].ToString();
                txtGrd.Text = dt1.Rows[0].ItemArray[4].ToString();
                txtSec.Text = "none";
                txtMOP.Text = dt1.Rows[0].ItemArray[22].ToString();

                levoff = txtGrd.Text;
            }

            if (dt2.Rows.Count > 0)
            {
              
               
                txtSY.Text = activeSY;
                txtSnum.Text = key;
                txtLast.Text = dt2.Rows[0].ItemArray[3].ToString();
                txtFirst.Text = dt2.Rows[0].ItemArray[1].ToString();
                txtMid.Text = dt2.Rows[0].ItemArray[2].ToString();
                txtGrd.Text = dt2.Rows[0].ItemArray[4].ToString();
                txtSec.Text = "none";
                txtMOP.Text = dt2.Rows[0].ItemArray[22].ToString();

                levoff = txtGrd.Text;
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

            if (levstud == "Kinder" || levoff=="Kinder")
            {
                setupAssessmentDiscountType(txtSnum.Text);
                setupAssessmentPerLevel(levdep);
                setupPaymentHistory();
                setupPaymentSchedule(key);
                setupPaymentSummary();
            }
            else if (levstud == "Grade 7" || levstud == "Grade 8" || levstud == "Grade 9" || levstud == "Grade 10" ||
                        levoff=="Grade 7" || levoff=="Grade 8" || levoff=="Grade 9" || levoff=="Grade 10")
            {
                setupAssessmentDiscountType(txtSnum.Text);
                setupAssessmentPerLevel(levdep);
               setupPaymentHistory();
               setupPaymentSchedule(key);
                setupPaymentSummary();
            }
            else
            {
                setupAssessmentDiscountType(txtSnum.Text);
                setupAssessmentPerLevel(levdep);
                setupPaymentHistory();
                setupPaymentSchedule(key);
                setupPaymentSummary();
            }

            txtCashAmt.Focus();
        }

        public void setupAssessmentJunior()
        {
            string levdep = "";
            TFee_J = "";
            Reg_J = "";
            Mis_J = "";

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

                        monthlyamount_J = dtjunior.Rows[a].ItemArray[2].ToString();
                        monthlyamt = Convert.ToDouble(monthlyamount_J);
                        LessAmt_J = monthlyamt * .5;
                        double discountedAmt = monthlyamt - LessAmt_J;
                        fiftyDisc_J = discountedAmt.ToString();
                        annualamt = Convert.ToDouble(annualamount_J);
                        double DiscountedTotalFreeLastMonth = annualamt - monthlyamt;
                        FreeLastMonthTotal_J = DiscountedTotalFreeLastMonth.ToString();
                        double DiscountedTotalFiftyDisc = annualamt - discountedAmt;
                        fiftyDiscTotal_J = DiscountedTotalFiftyDisc.ToString();

                        FREELASTMONTHTOTAL_J_FORPAYMENTHISTORY = FreeLastMonthTotal_J;
                        FIFTYDISCTOTAL_J_FORPAYMENTHISTORY = fiftyDiscTotal_J;
                    }
                    annualamt = Convert.ToDouble(annualamount_J);
                    annualamt_fiftydiscJ = annualamt - LessAmt_J;
                    anuualamt_freelastmonthJ = annualamt - monthlyamt;
                }   
            }
        }

        public void setupAssessmentElem()
        {
            string levdep = "";
            TFee_E = "";
            Reg_E = "";
            Mis_E = "";

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

                        monthlyamount_E = dtelem.Rows[a].ItemArray[2].ToString();
                        monthlyamt = Convert.ToDouble(monthlyamount_E);
                        LessAmt_E = monthlyamt * .5;
                        double discountedAmt = monthlyamt - LessAmt_E;
                        fiftyDisc_E = discountedAmt.ToString();
                        annualamt = Convert.ToDouble(annualamount_E);
                        double DiscountedTotalFreeLastMonth = annualamt - monthlyamt;
                        FreeLastMonthTotal_E = DiscountedTotalFreeLastMonth.ToString();
                        double DiscountedTotalFiftyDisc = annualamt - discountedAmt;
                        fiftyDiscTotal_E = DiscountedTotalFiftyDisc.ToString();

                        FREELASTMONTHTOTAL_E_FORPAYMENTHISTORY = FreeLastMonthTotal_E;
                        FIFTYDISCTOTAL_E_FORPAYMENTHISTORY = fiftyDiscTotal_E;
                       
                    }
                    annualamt = Convert.ToDouble(annualamount_E);
                    annualamt_fiftydiscE = annualamt - LessAmt_E;
                    anuualamt_freelastmonthE = annualamt - monthlyamt;

                   
                }
            }
        }

        public void setupAssessmentKinder()
        {
            string levdep = "";
            TFee_K = "";
            Reg_K = "";
            Mis_K = "";

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
            OdbcDataAdapter dakinder = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdep + "'and SY='" + activeSY + "'", con);
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
                        uponamount_K = dtkinder.Rows[a].ItemArray[2].ToString();
                    }
                    if (dtkinder.Rows[a].ItemArray[1].ToString() == "MONTHLY INSTALLMENT")
                    {
                        monthlyamount_K = dtkinder.Rows[a].ItemArray[2].ToString();
                        monthlyamt = Convert.ToDouble(monthlyamount_K);
                        LessAmt_K = monthlyamt * .5;
                        double discountedAmt = monthlyamt - LessAmt_K;
                        fiftyDisc_K = discountedAmt.ToString();
                        annualamt = Convert.ToDouble(annualamount_K);
                        double DiscountedTotalFreeLastMonth = annualamt - monthlyamt;
                        FreeLastMonthTotal_K = DiscountedTotalFreeLastMonth.ToString();
                        double DiscountedTotalFiftyDisc = annualamt - discountedAmt;
                        fiftyDiscTotal_K = DiscountedTotalFiftyDisc.ToString();

                        FREELASTMONTHTOTAL_K_FORPAYMENTHISTORY = FreeLastMonthTotal_K;
                        FIFTYDISCTOTAL_K_FORPAYMENTHISTORY = fiftyDiscTotal_K;
                       
                    }
                    annualamt = Convert.ToDouble(annualamount_K);
                    annualamt_fiftydiscK = annualamt - LessAmt_K;
                    anuualamt_freelastmonthK = annualamt - monthlyamt;
                }
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

        public void setupAssessmentDiscountType(string key)
        {
            //DISCOUNT TYPE
            con.Open();
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + key + "'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            con.Close();

            if (dt1.Rows.Count > 0)
            {
                discountType = dt1.Rows[0].ItemArray[1].ToString(); 
            }
            else
            {
                 discountType = "None";
            }
            
        }

        public void retrieveMonthlyInstallmentAmt_OtherDisc(string discountType)
        {
            con.Open();
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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

                string slev = txtGrd.Text;
                if (slev == "Kinder")
                {
                    setupAssessmentKinder();
                    double TF_amt = Convert.ToDouble(TFee_K);
                    double Reg_amt = Convert.ToDouble(Reg_K);
                    double Mis_amt = Convert.ToDouble(Mis_K);
                    double anlamt = Convert.ToDouble(annualamount_K);
                    double discrate = Convert.ToDouble(rate);
                    discountedAmtOtherDisc = TF_amt * discrate;
                    TF_amt -= discountedAmtOtherDisc;
                    discountedTotalOtherDisc = TF_amt+Reg_amt+Mis_amt;

                    double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                    amt_monthlyIns_OtherDisc = "";
                    double uponamt = Convert.ToDouble(uponamount_K);
                    double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                    InstallmentAmt_forOtherDisc = amt_deductUpon / 9;
                }
                if (slev == "Grade 1" || slev == "Grade 2" || slev == "Grade 3" || slev == "Grade 4" || slev == "Grade 5" || slev == "Grade 6")
                {
                    setupAssessmentElem();
                    double TF_amt = Convert.ToDouble(TFee_E);
                    double Reg_amt = Convert.ToDouble(Reg_E);
                    double Mis_amt = Convert.ToDouble(Mis_E);
                    double anlamt = Convert.ToDouble(annualamount_E);
                    double discrate = Convert.ToDouble(rate);
                    discountedAmtOtherDisc = TF_amt * discrate;
                    TF_amt -= discountedAmtOtherDisc;
                    discountedTotalOtherDisc = TF_amt+Reg_amt+Mis_amt;

                    double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                    amt_monthlyIns_OtherDisc = "";
                    double uponamt = Convert.ToDouble(uponamount_E);
                    double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                    InstallmentAmt_forOtherDisc = amt_deductUpon / 9;
                }
                if (slev == "Grade 7" || slev == "Grade 8" || slev == "Grade 9" || slev == "Grade 10")
                {
                    setupAssessmentJunior();
                    double TF_amt = Convert.ToDouble(TFee_J);
                    double Reg_amt = Convert.ToDouble(Reg_J);
                    double Mis_amt = Convert.ToDouble(Mis_J);
                    double anlamt = Convert.ToDouble(annualamount_J);
                    double discrate = Convert.ToDouble(rate);
                    discountedAmtOtherDisc = TF_amt * discrate;
                    TF_amt -= discountedAmtOtherDisc;
                    discountedTotalOtherDisc =TF_amt+Reg_amt+Mis_amt;

                    double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                    amt_monthlyIns_OtherDisc = "";
                    double uponamt = Convert.ToDouble(uponamount_J);
                    double amt_deductUpon = discountedTotalOtherDisc - uponamt;
                    InstallmentAmt_forOtherDisc = amt_deductUpon / 9;
                }
                
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

            setupAssessmentDiscountType(txtSnum.Text);//check if theres discount the stud.

            if (txtGrd.Text == "Kinder")
            {
                lvwPaySched.Items.Clear();
                setupAssessmentKinder();

                if (txtMOP.Text == "Cash")
                {
                    setupDateRegistered_Cash();
                    if (discountType != "None")
                    {
                        if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
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
                        if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
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
                        if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                        {
                            con.Open();
                            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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
                if (txtMOP.Text == "Installment")
                {
                    setupDateRegistered_Installment();

                    ListViewItem itmki = new ListViewItem();
                    itmki.Text = "UPON ENROLLMENT";
                    itmki.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                    itmki.SubItems.Add(today);
                    itmki.SubItems.Add("P " + uponamount_K);
                    lvwPaySched.Items.Add(itmki);


                    if (discountType != "None")
                    {
                        if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
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
                        if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
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
                        if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                        {
                            con.Open();
                            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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

                                double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                                string amt_tot_dis = "";
                                amt_monthlyIns_OtherDisc = "";
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
                setupAssessmentElem();

                if (txtMOP.Text == "Cash")
                {
                    setupDateRegistered_Cash();

                    if (discountType != "None")
                    {
                        if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
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
                        if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
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
                        if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                        {
                            con.Open();
                            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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
                if (txtMOP.Text == "Installment")
                {
                    setupDateRegistered_Installment();

                    ListViewItem itmei = new ListViewItem();
                    itmei.Text = "UPON ENROLLMENT";
                    itmei.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                    itmei.SubItems.Add(today);
                    itmei.SubItems.Add("P " + uponamount_E);
                    lvwPaySched.Items.Add(itmei);


                    if (discountType != "None")
                    {
                        if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
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
                        if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
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
                        if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                        {
                            con.Open();
                            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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

                                double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                                string amt_tot_dis = "";
                                amt_monthlyIns_OtherDisc = "";
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
                setupAssessmentJunior();

                if (txtMOP.Text == "Cash")
                {
                    setupDateRegistered_Cash();

                    if (discountType != "None")
                    {
                        if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
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
                        if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
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
                        if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                        {
                            con.Open();
                            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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
                if (txtMOP.Text == "Installment")
                {
                    setupDateRegistered_Installment();

                    ListViewItem itmjhi = new ListViewItem();
                    itmjhi.Text = "UPON ENROLLMENT";
                    itmjhi.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Regular));
                    itmjhi.SubItems.Add(today);
                    itmjhi.SubItems.Add("P " + uponamount_J);
                    lvwPaySched.Items.Add(itmjhi);


                    if (discountType != "None")
                    {
                        if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
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
                        if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
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
                        if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                        {
                            con.Open();
                            OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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

                                double _amt_tot = Convert.ToDouble(discountedTotalOtherDisc);
                                string amt_tot_dis = "";
                                amt_monthlyIns_OtherDisc = "";
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
       

        public void setupPaymentHistory()
        {
            lvwPH.Clear();
            setupAssessmentKinder();
            setupAssessmentElem();
            setupAssessmentJunior();

            if (txtMOP.Text == "Cash")
            {
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

                    if (txtGrd.Text == "Kinder")
                    {
                        
                        con.Open();
                        OdbcDataAdapter dak = new OdbcDataAdapter("Select amount from fee_tbl where fee='" + "ANNUAL PAYMENT" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtk = new DataTable();
                        dak.Fill(dtk);
                        con.Close();

                        if (dtk.Rows.Count > 0)
                        {//here stop
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
                                    kinderannual = dtk.Rows[0].ItemArray[0].ToString();
                                    string amtToDisplay = "";
                                    double amt = anuualamt_freelastmonthK;
                                    if (amt >= 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                    }
                                    if (amt < 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                    }

                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itps0);
                                }
                                if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                                {
                                    kinderannual = dtk.Rows[0].ItemArray[0].ToString();
                                    string amtToDisplay = "";
                                    double amt = annualamt_fiftydiscK;
                                    if (amt >= 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                    }
                                    if (amt < 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                    }

                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itps0);
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    kinderannual = dtk.Rows[0].ItemArray[0].ToString();
                                    string amtToDisplay = "";
                                    double amt = discountedTotalOtherDisc;
                                    if (amt >= 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                    }
                                    if (amt < 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                    }

                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itps0);
                                }
                            }
                            else
                            {
                                kinderannual = dtk.Rows[0].ItemArray[0].ToString();
                                ListViewItem itmpd = new ListViewItem();
                                itmpd.Text = "ANNUAL PAYMENT";
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                itmpd.SubItems.Add("P " + kinderannual);
                                lvwPH.Items.Add(itmpd);

                                ListViewItem itps0 = new ListViewItem();
                                itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                itps0.Text = "Total:";
                                itps0.SubItems.Add("");
                                itps0.SubItems.Add("P " + kinderannual);
                                lvwPH.Items.Add(itps0);
                            }
                        }
                    }
                    else if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                    {
                         con.Open();
                         OdbcDataAdapter daj = new OdbcDataAdapter("Select amount from fee_tbl where fee='" + "ANNUAL PAYMENT" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dtj = new DataTable();
                        daj.Fill(dtj);
                        con.Close();

                        if (dtj.Rows.Count > 0)
                        {
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
                                    string amtToDisplay = "";
                                    double amt = anuualamt_freelastmonthJ;
                                    if (amt >= 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                    }
                                    if (amt < 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                    }

                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itps0);
                                }
                                if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                                {
                                    jrannual = dtj.Rows[0].ItemArray[0].ToString();
                                    string amtToDisplay = "";
                                    double amt = annualamt_fiftydiscJ;
                                    if (amt >= 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                    }
                                    if (amt < 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                    }

                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itps0);
                                }
                                if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                                {
                                    jrannual = dtj.Rows[0].ItemArray[0].ToString();
                                    string amtToDisplay = "";
                                    double amt = discountedTotalOtherDisc;
                                    if (amt >= 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                    }
                                    if (amt < 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                    }

                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itps0);
                                }

                            }
                            else
                            {
                                jrannual = dtj.Rows[0].ItemArray[0].ToString();
                                ListViewItem itmpd = new ListViewItem();
                                itmpd.Text = "ANNUAL PAYMENT";
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                itmpd.SubItems.Add("P " + jrannual);
                                lvwPH.Items.Add(itmpd);

                                ListViewItem itps0 = new ListViewItem();
                                itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                itps0.Text = "Total:";
                                itps0.SubItems.Add("");
                                itps0.SubItems.Add("P " + jrannual);
                                lvwPH.Items.Add(itps0);
                            }
                        }
                    
                    }
                    else
                    {
                        con.Open();
                        OdbcDataAdapter dae = new OdbcDataAdapter("Select amount from fee_tbl where fee='" + "ANNUAL PAYMENT" + "'AND level='" + levdep + "'and SY='" + activeSY + "'", con);
                        DataTable dte = new DataTable();
                        dae.Fill(dte);
                        con.Close();

                        if (dte.Rows.Count > 0)
                        {
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
                                    string amtToDisplay = "";
                                    double amt = anuualamt_freelastmonthE;
                                    if (amt >= 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                    }
                                    if (amt < 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                    }

                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itps0);
                                }
                                if (discounttype.Contains("Second") == true || discounttype.Contains("2nd") == true)
                                {
                                    elemannual = dte.Rows[0].ItemArray[0].ToString();
                                    string amtToDisplay = "";
                                    double amt = annualamt_fiftydiscE;
                                    if (amt >= 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                    }
                                    if (amt < 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                    }

                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itps0);
                                }
                                if (discounttype.Contains("siblings") == false && discounttype.Contains("First") == false && discounttype.Contains("1st") == false && discounttype.Contains("Second") == false && discounttype.Contains("2nd") == false)
                                {
                                    elemannual = dte.Rows[0].ItemArray[0].ToString();
                                    string amtToDisplay = "";
                                    double amt = discountedTotalOtherDisc;
                                    if (amt >= 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0,###.00#}"), amt);
                                    }
                                    if (amt < 1000)
                                    {
                                        amtToDisplay = String.Format(("{0:0.00#}"), amt);
                                    }

                                    ListViewItem itmpd = new ListViewItem();
                                    itmpd.Text = "ANNUAL PAYMENT";
                                    itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                    itmpd.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itmpd);

                                    ListViewItem itps0 = new ListViewItem();
                                    itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                    itps0.Text = "Total:";
                                    itps0.SubItems.Add("");
                                    itps0.SubItems.Add("P " + amtToDisplay);
                                    lvwPH.Items.Add(itps0);
                                }

                            }
                            else
                            {
                                elemannual = dte.Rows[0].ItemArray[0].ToString();
                                ListViewItem itmpd = new ListViewItem();
                                itmpd.Text = "ANNUAL PAYMENT";
                                itmpd.SubItems.Add(dtps.Rows[0].ItemArray[4].ToString());
                                itmpd.SubItems.Add("P " + elemannual);
                                lvwPH.Items.Add(itmpd);

                                ListViewItem itps0 = new ListViewItem();
                                itps0.Font = new Font("Arial", 11, FontStyle.Bold);
                                itps0.Text = "Total:";
                                itps0.SubItems.Add("");
                                itps0.SubItems.Add("P "+elemannual);
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
            if (txtMOP.Text == "Installment")//installment history
            {
                
                string datetoday = DateTime.Now.ToShortDateString();

                con.Open();
                OdbcDataAdapter dapski = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + txtSnum.Text + "'", con);
                DataTable dtpski = new DataTable();
                dapski.Fill(dtpski);
                con.Close();

                if (dtpski.Rows.Count > 0)
                {
                    //these are the OR number in database
                    string annualAmt = dtpski.Rows[0].ItemArray[3].ToString();
                    double CurrBal = Convert.ToDouble(dtpski.Rows[0].ItemArray[4].ToString());
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

                    if (dateupon != "")
                    {
                        pnlNotPH.Visible = false;
                        lvwPH.Columns.Add("Payment", 194, HorizontalAlignment.Left);
                        lvwPH.Columns.Add("Date paid", 110, HorizontalAlignment.Center);
                        lvwPH.Columns.Add("Amount", 110, HorizontalAlignment.Right);
                    }

                    string FLM = "";
                    string FPD = "";
                    string ANL = "";
                    //start
                    con.Open();
                    OdbcDataAdapter daDisc0 = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                    DataTable dtDisc0 = new DataTable();
                    daDisc0.Fill(dtDisc0);
                    con.Close();
                    if (dtDisc0.Rows.Count > 0)
                    {
                        string discountType = dtDisc0.Rows[0].ItemArray[1].ToString();
                        if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st") == true)
                        {
                            if (txtGrd.Text == "Kinder")
                            {
                                setupAssessmentKinder();
                                ANL = FreeLastMonthTotal_K;
                            }
                            if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
                            {
                                setupAssessmentElem();
                                ANL = FreeLastMonthTotal_E;
                            }
                            if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                            {
                                setupAssessmentJunior();
                                ANL = FreeLastMonthTotal_J;
                            }
                        }
                        if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                        {
                            if (txtGrd.Text == "Kinder")
                            {
                                setupAssessmentKinder();
                                ANL = fiftyDiscTotal_K;
                            }
                            if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
                            {
                                setupAssessmentElem();
                                ANL = fiftyDiscTotal_E;
                            }
                            if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                            {
                                setupAssessmentJunior();
                                ANL = fiftyDiscTotal_J;
                            }
                        }
                        if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                        {
                            if (txtGrd.Text == "Kinder")
                            {
                                setupAssessmentKinder();
                                ANL = discountedTotalOtherDisc.ToString();
                            }
                            if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
                            {
                                setupAssessmentElem();
                                ANL = discountedTotalOtherDisc.ToString();
                            }
                            if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                            {
                                setupAssessmentJunior();
                                ANL = discountedTotalOtherDisc.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (txtGrd.Text == "Kinder")
                        {
                            setupAssessmentKinder();
                            ANL = annualamount_K;
                        }
                        if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
                        {
                            setupAssessmentElem();
                            ANL = annualamount_E;
                        }
                        if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                        {
                            setupAssessmentJunior();
                            ANL = annualamount_J;
                        }
                    }

                    if (txtGrd.Text == "Kinder")
                    {
                        setupAssessmentKinder();
                        FLM = FreeLastMonthTotal_K;
                        FPD = fiftyDiscTotal_K;
                    }
                    if (txtGrd.Text == "Grade 1" || txtGrd.Text == "Grade 2" || txtGrd.Text == "Grade 3" || txtGrd.Text == "Grade 4" || txtGrd.Text == "Grade 5" || txtGrd.Text == "Grade 6")
                    {
                        setupAssessmentElem();
                        FLM = FreeLastMonthTotal_E;
                        FPD = fiftyDiscTotal_E;
                    }
                    if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                    {
                        setupAssessmentJunior();
                        FLM = FreeLastMonthTotal_J;
                        FPD = fiftyDiscTotal_J;
                    }

                    double theAnnual = Convert.ToDouble(ANL);
                    if (theAnnual >= 1000) { ANL = String.Format(("{0:0,###.00#}"), theAnnual); } else { ANL = String.Format(("{0:0.00#}"), theAnnual); }
                    double theFLM = Convert.ToDouble(FLM);
                    if (theFLM>= 1000) { FLM = String.Format(("{0:0,###.00#}"), theFLM); } else { FLM = String.Format(("{0:0.00#}"), theFLM); }
                    double theFPD = Convert.ToDouble(FPD);
                    if (theFPD >= 1000) { FPD = String.Format(("{0:0,###.00#}"), theFPD); } else { FPD = String.Format(("{0:0.00#}"), theFPD); }

                    if (dateupon =="" && dpay2=="" && dpay3=="" && dpay4=="" && dpay5=="" && dpay6=="" && dpay7=="" && dpay8=="" && dpay9=="" && dpay10=="")
                    {
                        lvwPH.Items.Clear();
                        pnlNotPH.Visible = true;
                    }
                    if (dateupon != "" && dpay2 == "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                    {
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P "+dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        if (CurrBal <= 0)
                        {
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);
                        }
                    }
                    if (dateupon != "" && dpay2 != "" && dpay3 == "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                    {
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        ListViewItem itmdp2 = new ListViewItem();
                        itmdp2.Text = "SECOND PAYMENT";
                        itmdp2.SubItems.Add(dpay2);
                        itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                        lvwPH.Items.Add(itmdp2);

                        if (CurrBal <= 0)
                        {
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);
                        }
                    }
                    if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 == "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                    {
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        ListViewItem itmdp2 = new ListViewItem();
                        itmdp2.Text = "SECOND PAYMENT";
                        itmdp2.SubItems.Add(dpay2);
                        itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                        lvwPH.Items.Add(itmdp2);

                        ListViewItem itmdp3 = new ListViewItem();
                        itmdp3.Text = "THIRD PAYMENT";
                        itmdp3.SubItems.Add(dpay3);
                        itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                        lvwPH.Items.Add(itmdp3);

                        if (CurrBal <= 0)
                        {
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);
                        }
                    }
                    if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 == "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                    {
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        ListViewItem itmdp2 = new ListViewItem();
                        itmdp2.Text = "SECOND PAYMENT";
                        itmdp2.SubItems.Add(dpay2);
                        itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                        lvwPH.Items.Add(itmdp2);

                        ListViewItem itmdp3 = new ListViewItem();
                        itmdp3.Text = "THIRD PAYMENT";
                        itmdp3.SubItems.Add(dpay3);
                        itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                        lvwPH.Items.Add(itmdp3);

                        ListViewItem itmdp4 = new ListViewItem();
                        itmdp4.Text = "FOURTH PAYMENT";
                        itmdp4.SubItems.Add(dpay4);
                        itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                        lvwPH.Items.Add(itmdp4);

                        if (CurrBal <= 0)
                        {
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);
                        }
                    }
                    if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 == "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                    {
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        ListViewItem itmdp2 = new ListViewItem();
                        itmdp2.Text = "SECOND PAYMENT";
                        itmdp2.SubItems.Add(dpay2);
                        itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                        lvwPH.Items.Add(itmdp2);

                        ListViewItem itmdp3 = new ListViewItem();
                        itmdp3.Text = "THIRD PAYMENT";
                        itmdp3.SubItems.Add(dpay3);
                        itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                        lvwPH.Items.Add(itmdp3);

                        ListViewItem itmdp4 = new ListViewItem();
                        itmdp4.Text = "FOURTH PAYMENT";
                        itmdp4.SubItems.Add(dpay4);
                        itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                        lvwPH.Items.Add(itmdp4);

                        ListViewItem itmdp5 = new ListViewItem();
                        itmdp5.Text = "FIFTH PAYMENT";
                        itmdp5.SubItems.Add(dpay5);
                        itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                        lvwPH.Items.Add(itmdp5);

                        if (CurrBal <= 0)
                        {
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);
                        }
                    }
                    if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 == "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                    {
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        ListViewItem itmdp2 = new ListViewItem();
                        itmdp2.Text = "SECOND PAYMENT";
                        itmdp2.SubItems.Add(dpay2);
                        itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                        lvwPH.Items.Add(itmdp2);

                        ListViewItem itmdp3 = new ListViewItem();
                        itmdp3.Text = "THIRD PAYMENT";
                        itmdp3.SubItems.Add(dpay3);
                        itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                        lvwPH.Items.Add(itmdp3);

                        ListViewItem itmdp4 = new ListViewItem();
                        itmdp4.Text = "FOURTH PAYMENT";
                        itmdp4.SubItems.Add(dpay4);
                        itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                        lvwPH.Items.Add(itmdp4);

                        ListViewItem itmdp5 = new ListViewItem();
                        itmdp5.Text = "FIFTH PAYMENT";
                        itmdp5.SubItems.Add(dpay5);
                        itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                        lvwPH.Items.Add(itmdp5);

                        ListViewItem itmdp6 = new ListViewItem();
                        itmdp6.Text = "SIXTH PAYMENT";
                        itmdp6.SubItems.Add(dpay6);
                        itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                        lvwPH.Items.Add(itmdp6);

                        if (CurrBal <= 0)
                        {
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);
                        }
                    }
                    if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 == "" && dpay9 == "" && dpay10 == "")
                    {
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        ListViewItem itmdp2 = new ListViewItem();
                        itmdp2.Text = "SECOND PAYMENT";
                        itmdp2.SubItems.Add(dpay2);
                        itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                        lvwPH.Items.Add(itmdp2);

                        ListViewItem itmdp3 = new ListViewItem();
                        itmdp3.Text = "THIRD PAYMENT";
                        itmdp3.SubItems.Add(dpay3);
                        itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                        lvwPH.Items.Add(itmdp3);

                        ListViewItem itmdp4 = new ListViewItem();
                        itmdp4.Text = "FOURTH PAYMENT";
                        itmdp4.SubItems.Add(dpay4);
                        itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                        lvwPH.Items.Add(itmdp4);

                        ListViewItem itmdp5 = new ListViewItem();
                        itmdp5.Text = "FIFTH PAYMENT";
                        itmdp5.SubItems.Add(dpay5);
                        itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                        lvwPH.Items.Add(itmdp5);

                        ListViewItem itmdp6 = new ListViewItem();
                        itmdp6.Text = "SIXTH PAYMENT";
                        itmdp6.SubItems.Add(dpay6);
                        itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                        lvwPH.Items.Add(itmdp6);

                        ListViewItem itmdp7 = new ListViewItem();
                        itmdp7.Text = "SEVENTH PAYMENT";
                        itmdp7.SubItems.Add(dpay7);
                        itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                        lvwPH.Items.Add(itmdp7);

                        if (CurrBal <= 0)
                        {
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);
                        }
                    }
                    if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 == "" && dpay10 == "")
                    {
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        ListViewItem itmdp2 = new ListViewItem();
                        itmdp2.Text = "SECOND PAYMENT";
                        itmdp2.SubItems.Add(dpay2);
                        itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                        lvwPH.Items.Add(itmdp2);

                        ListViewItem itmdp3 = new ListViewItem();
                        itmdp3.Text = "THIRD PAYMENT";
                        itmdp3.SubItems.Add(dpay3);
                        itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                        lvwPH.Items.Add(itmdp3);

                        ListViewItem itmdp4 = new ListViewItem();
                        itmdp4.Text = "FOURTH PAYMENT";
                        itmdp4.SubItems.Add(dpay4);
                        itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                        lvwPH.Items.Add(itmdp4);

                        ListViewItem itmdp5 = new ListViewItem();
                        itmdp5.Text = "FIFTH PAYMENT";
                        itmdp5.SubItems.Add(dpay5);
                        itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                        lvwPH.Items.Add(itmdp5);

                        ListViewItem itmdp6 = new ListViewItem();
                        itmdp6.Text = "SIXTH PAYMENT";
                        itmdp6.SubItems.Add(dpay6);
                        itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                        lvwPH.Items.Add(itmdp6);

                        ListViewItem itmdp7 = new ListViewItem();
                        itmdp7.Text = "SEVENTH PAYMENT";
                        itmdp7.SubItems.Add(dpay7);
                        itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                        lvwPH.Items.Add(itmdp7);

                        ListViewItem itmdp8 = new ListViewItem();
                        itmdp8.Text = "EIGHTTH PAYMENT";
                        itmdp8.SubItems.Add(dpay8);
                        itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                        lvwPH.Items.Add(itmdp8);

                        if (CurrBal <= 0)
                        {
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);
                        }
                    }
                    if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 == "")
                    {
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        ListViewItem itmdp2 = new ListViewItem();
                        itmdp2.Text = "SECOND PAYMENT";
                        itmdp2.SubItems.Add(dpay2);
                        itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                        lvwPH.Items.Add(itmdp2);

                        ListViewItem itmdp3 = new ListViewItem();
                        itmdp3.Text = "THIRD PAYMENT";
                        itmdp3.SubItems.Add(dpay3);
                        itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                        lvwPH.Items.Add(itmdp3);

                        ListViewItem itmdp4 = new ListViewItem();
                        itmdp4.Text = "FOURTH PAYMENT";
                        itmdp4.SubItems.Add(dpay4);
                        itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                        lvwPH.Items.Add(itmdp4);

                        ListViewItem itmdp5 = new ListViewItem();
                        itmdp5.Text = "FIFTH PAYMENT";
                        itmdp5.SubItems.Add(dpay5);
                        itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                        lvwPH.Items.Add(itmdp5);

                        ListViewItem itmdp6 = new ListViewItem();
                        itmdp6.Text = "SIXTH PAYMENT";
                        itmdp6.SubItems.Add(dpay6);
                        itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                        lvwPH.Items.Add(itmdp6);

                        ListViewItem itmdp7 = new ListViewItem();
                        itmdp7.Text = "SEVENTH PAYMENT";
                        itmdp7.SubItems.Add(dpay7);
                        itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                        lvwPH.Items.Add(itmdp7);

                        ListViewItem itmdp8 = new ListViewItem();
                        itmdp8.Text = "EIGHTTH PAYMENT";
                        itmdp8.SubItems.Add(dpay8);
                        itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                        lvwPH.Items.Add(itmdp8);

                        ListViewItem itmdp9 = new ListViewItem();
                        itmdp9.Text = "NINETH PAYMENT";
                        itmdp9.SubItems.Add(dpay9);
                        itmdp9.SubItems.Add("P " + dtpski.Rows[0].ItemArray[23].ToString());
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
                                if (CurrBal <= 0)
                                {
                                    ListViewItem itmdp10 = new ListViewItem();
                                    itmdp10.Text = "TENTH PAYMENT";
                                    itmdp10.SubItems.Add("");
                                    itmdp10.SubItems.Add("P " + "0.00");
                                    lvwPH.Items.Add(itmdp10);

                                    ListViewItem itmdsum = new ListViewItem();
                                    itmdsum.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                    itmdsum.Text = "Total:";
                                    itmdsum.SubItems.Add("");
                                    itmdsum.SubItems.Add("P " + FLM);
                                    lvwPH.Items.Add(itmdsum);
                                }
                            }
                            //dito po
                        }
                        else
                        {
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);   
                        }
                    }
                    if (dateupon != "" && dpay2 != "" && dpay3 != "" && dpay4 != "" && dpay5 != "" && dpay6 != "" && dpay7 != "" && dpay8 != "" && dpay9 != "" && dpay10 != "")
                    {
                           
                        ListViewItem itmdp = new ListViewItem();
                        itmdp.Text = "UPON ENROLLMENT";
                        itmdp.SubItems.Add(dateupon);
                        itmdp.SubItems.Add("P " + dtpski.Rows[0].ItemArray[15].ToString());
                        lvwPH.Items.Add(itmdp);

                        ListViewItem itmdp2 = new ListViewItem();
                        itmdp2.Text = "SECOND PAYMENT";
                        itmdp2.SubItems.Add(dpay2);
                        itmdp2.SubItems.Add("P " + dtpski.Rows[0].ItemArray[16].ToString());
                        lvwPH.Items.Add(itmdp2);

                        ListViewItem itmdp3 = new ListViewItem();
                        itmdp3.Text = "THIRD PAYMENT";
                        itmdp3.SubItems.Add(dpay3);
                        itmdp3.SubItems.Add("P " + dtpski.Rows[0].ItemArray[17].ToString());
                        lvwPH.Items.Add(itmdp3);

                        ListViewItem itmdp4 = new ListViewItem();
                        itmdp4.Text = "FOURTH PAYMENT";
                        itmdp4.SubItems.Add(dpay4);
                        itmdp4.SubItems.Add("P " + dtpski.Rows[0].ItemArray[18].ToString());
                        lvwPH.Items.Add(itmdp4);

                        ListViewItem itmdp5 = new ListViewItem();
                        itmdp5.Text = "FIFTH PAYMENT";
                        itmdp5.SubItems.Add(dpay5);
                        itmdp5.SubItems.Add("P " + dtpski.Rows[0].ItemArray[19].ToString());
                        lvwPH.Items.Add(itmdp5);

                        ListViewItem itmdp6 = new ListViewItem();
                        itmdp6.Text = "SIXTH PAYMENT";
                        itmdp6.SubItems.Add(dpay6);
                        itmdp6.SubItems.Add("P " + dtpski.Rows[0].ItemArray[20].ToString());
                        lvwPH.Items.Add(itmdp6);

                        ListViewItem itmdp7 = new ListViewItem();
                        itmdp7.Text = "SEVENTH PAYMENT";
                        itmdp7.SubItems.Add(dpay7);
                        itmdp7.SubItems.Add("P " + dtpski.Rows[0].ItemArray[21].ToString());
                        lvwPH.Items.Add(itmdp7);

                        ListViewItem itmdp8 = new ListViewItem();
                        itmdp8.Text = "EIGHTTH PAYMENT";
                        itmdp8.SubItems.Add(dpay8);
                        itmdp8.SubItems.Add("P " + dtpski.Rows[0].ItemArray[22].ToString());
                        lvwPH.Items.Add(itmdp8);

                        ListViewItem itmdp9 = new ListViewItem();
                        itmdp9.Text = "NINETH PAYMENT";
                        itmdp9.SubItems.Add(dpay9);
                        itmdp9.SubItems.Add("P " + dtpski.Rows[0].ItemArray[23].ToString());
                        lvwPH.Items.Add(itmdp9);


                        con.Open();
                        OdbcDataAdapter daDisc = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'", con);
                        DataTable dtDisc = new DataTable();
                        daDisc.Fill(dtDisc);
                        con.Close();
                        if (dtDisc.Rows.Count > 0)
                        {
                            string discountType = dtDisc.Rows[0].ItemArray[1].ToString();
                            //tenth error
                            if (discountType.Contains("siblings") == true || discountType.Contains("First") == true || discountType.Contains("1st"))
                            {
                                ListViewItem itmdp10 = new ListViewItem();
                                itmdp10.Text = "TENTH PAYMENT";
                                itmdp10.SubItems.Add(dpay10);
                                itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                lvwPH.Items.Add(itmdp10);

                              
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + ANL);
                                lvwPH.Items.Add(itmdpsumm);

                                //dito po2
                            }
                            if (discountType.Contains("Second") == true || discountType.Contains("2nd") == true)
                            {
                                ListViewItem itmdp10 = new ListViewItem();
                                itmdp10.Text = "TENTH PAYMENT";
                                itmdp10.SubItems.Add(dpay10);
                                itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                lvwPH.Items.Add(itmdp10);

                              
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + ANL);
                                lvwPH.Items.Add(itmdpsumm);
                                
                                //dito po2
                            }
                            if (discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)
                            {
                                ListViewItem itmdp10 = new ListViewItem();
                                itmdp10.Text = "TENTH PAYMENT";
                                itmdp10.SubItems.Add(dpay10);
                                itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                                lvwPH.Items.Add(itmdp10);

                               
                                ListViewItem itmdpsumm = new ListViewItem();
                                itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                                itmdpsumm.Text = "Total:";
                                itmdpsumm.SubItems.Add("");
                                itmdpsumm.SubItems.Add("P " + ANL);
                                lvwPH.Items.Add(itmdpsumm);
                                
                            }
                        }
                        else
                        {
                            ListViewItem itmdp10 = new ListViewItem();
                            itmdp10.Text = "TENTH PAYMENT";
                            itmdp10.SubItems.Add(dpay10);
                            itmdp10.SubItems.Add("P " + dtpski.Rows[0].ItemArray[24].ToString());
                            lvwPH.Items.Add(itmdp10);

                          
                            ListViewItem itmdpsumm = new ListViewItem();
                            itmdpsumm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
                            itmdpsumm.Text = "Total:";
                            itmdpsumm.SubItems.Add("");
                            itmdpsumm.SubItems.Add("P " + ANL);
                            lvwPH.Items.Add(itmdpsumm);
                            
                        }
                    }   
                }
                else
                {
                    pnlNotPH.Visible = true;
                }
            }
        }

        public void setupAssessmentPerLevel(string levelkey)
        {
            lvwAssessment.Items.Clear();
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
                pnlNotAss.Visible = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[1].ToString().Contains("TUITION FEE") == true)
                    {
                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = dt.Rows[i].ItemArray[1].ToString();
                        itmfee1.SubItems.Add("P " + dt.Rows[i].ItemArray[2].ToString());
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                }

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j].ItemArray[1].ToString().Contains("REGISTRATION") == true)
                    {
                        ListViewItem itmfee2 = new ListViewItem();
                        itmfee2.Text = dt.Rows[j].ItemArray[1].ToString();
                        itmfee2.SubItems.Add("P " + dt.Rows[j].ItemArray[2].ToString());
                        lvwAssessment.Items.Add(itmfee2);
                        itmfee2.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                }

                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    if (dt.Rows[k].ItemArray[1].ToString().Contains("MISCELLANEOUS") == true)
                    {
                        ListViewItem itmfee2 = new ListViewItem();
                        itmfee2.Text = dt.Rows[k].ItemArray[1].ToString();
                        itmfee2.SubItems.Add("P " + dt.Rows[k].ItemArray[2].ToString());
                        lvwAssessment.Items.Add(itmfee2);
                        itmfee2.Font = new Font("Arial", 11, FontStyle.Regular);
                       
                    }
                }

                con.Open();
                OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and fee<>'TUITION FEE'and fee<>'REGISTRATION'and fee<>'MISCELLANEOUS'and type<>'payment' and SY='" + activeSY + "'", con);
                DataTable dt01 = new DataTable();
                da01.Fill(dt01);
                con.Close();
                if (dt01.Rows.Count > 0)
                {
                    for (int i = 0; i < dt01.Rows.Count; i++)
                    {
                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = dt01.Rows[i].ItemArray[1].ToString();
                        itmfee1.SubItems.Add("P " + dt01.Rows[i].ItemArray[2].ToString());
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                }

                //SUBTOTAL
                if (discountType != "None")
                {
                    ListViewItem itmst0 = new ListViewItem();
                    itmst0.Text = "";
                    itmst0.SubItems.Add("___________");
                    lvwAssessment.Items.Add(itmst0);

                    double amt = Convert.ToDouble(totalAss);
                    if (amt >= 1000)
                    {
                        totalAss = String.Format(("{0:0,###.00#}"), amt);
                    } if (amt < 1000)
                    {
                        totalAss = String.Format(("{0:0.00#}"), amt);
                    }

                    ListViewItem itmst = new ListViewItem();
                    itmst.Text = "Total:";
                    itmst.SubItems.Add("P " + totalAss);
                    lvwAssessment.Items.Add(itmst);
                    itmst.Font = new Font("Arial", 11, FontStyle.Bold);
                }

                //--------------------------------FOR DISCOUNT
                string discountedAmtDisp = "";
                string discTotalAssDisp = "";

                if (txtGrd.Text == "Kinder")
                {
                    setupAssessmentKinder();
                    if (discountType=="None")
                    {
                        
                    }

                    if ((discountType != "None") && ((discountType.Contains("siblings") == true) || discountType.Contains("First") == true || discountType.Contains("1st") == true))
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
                        itmfee1.Text = "Less: ("+discountType+")";
                        itmfee1.SubItems.Add("P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);


                    }
                    if ((discountType != "None") && ((discountType.Contains("Second") == true) || discountType.Contains("2nd") == true))
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
                        itmfee1.Text = "Less: (" + discountType + ")";
                        itmfee1.SubItems.Add("P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                    {
                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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
                            discountedTotalOtherDisc = TF_amt + Reg_amt + Mis_amt;

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
                            itmfee1.Text = "Less: (" + discountType + ")";
                            itmfee1.SubItems.Add("P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                    }
                }
                else if (txtGrd.Text == "Grade 7" || txtGrd.Text == "Grade 8" || txtGrd.Text == "Grade 9" || txtGrd.Text == "Grade 10")
                {
                    setupAssessmentJunior();
                    if (discountType=="None")
                    {
                        
                    }

                    if ((discountType != "None") && ((discountType.Contains("siblings") == true) || discountType.Contains("First") == true || discountType.Contains("1st") == true))
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

                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less: (" + discountType + ")";
                        itmfee1.SubItems.Add("P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((discountType != "None") && ((discountType.Contains("Second") == true) || discountType.Contains("2nd") == true))
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

                        ListViewItem itmfee1 = new ListViewItem();
                        itmfee1.Text = "Less: (" + discountType + ")";
                        itmfee1.SubItems.Add("P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                    {
                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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
                            itmfee1.Text = "Less: (" + discountType + ")";
                            itmfee1.SubItems.Add("P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                    }
                }
                else
                {
                    setupAssessmentElem();
                    if (discountType=="None")
                    {
                       
                    }

                    if ((discountType != "None") && ((discountType.Contains("siblings") == true) || discountType.Contains("First") == true || discountType.Contains("1st") == true))
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
                        itmfee1.Text = "Less: (" + discountType + ")";
                        itmfee1.SubItems.Add("P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((discountType != "None") && ((discountType.Contains("Second") == true) || discountType.Contains("2nd") == true))
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
                        itmfee1.Text = "Less: (" + discountType + ")";
                        itmfee1.SubItems.Add("P " + discountedAmtDisp);
                        lvwAssessment.Items.Add(itmfee1);
                        itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                    }
                    if ((discountType != "None") && ((discountType.Contains("siblings") == false && discountType.Contains("First") == false && discountType.Contains("1st") == false && discountType.Contains("Second") == false && discountType.Contains("2nd") == false)))
                    {
                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from discount_tbl where discname='" + discountType + "'", con);
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
                            itmfee1.Text = "Less: (" + discountType + ")";
                            itmfee1.SubItems.Add("P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                    }
                }



                if (discountType!="None")
                {
                    double amt = Convert.ToDouble(discTotalAssDisp);
                    if (amt >= 1000)
                    {
                        discTotalAssDisp = String.Format(("{0:0,###.00#}"), amt);
                    } if (amt < 1000)
                    {
                        discTotalAssDisp = String.Format(("{0:0.00#}"), amt);
                    }

                    ListViewItem itmst0 = new ListViewItem();
                    itmst0.Text = "";
                    itmst0.SubItems.Add("___________");
                    lvwAssessment.Items.Add(itmst0);

                    ListViewItem itmfee = new ListViewItem();
                    itmfee.Text = "Total Assessment:";
                    itmfee.SubItems.Add("P " + discTotalAssDisp);
                    lvwAssessment.Items.Add(itmfee);
                    itmfee.Font = new Font("Arial", 11, FontStyle.Bold);
                }
                else
                {
                    double amt = Convert.ToDouble(totalAss);
                    if (amt >= 1000)
                    {
                        totalAss = String.Format(("{0:0,###.00#}"), amt);
                    } if (amt < 1000)
                    {
                        totalAss = String.Format(("{0:0.00#}"), amt);
                    }

                    ListViewItem itmst0 = new ListViewItem();
                    itmst0.Text = "";
                    itmst0.SubItems.Add("___________");
                    lvwAssessment.Items.Add(itmst0);

                    ListViewItem itmfee = new ListViewItem();
                    itmfee.Text = "Total Assessment:";
                    itmfee.SubItems.Add("P " + totalAss);
                    lvwAssessment.Items.Add(itmfee);
                    itmfee.Font = new Font("Arial", 11, FontStyle.Bold);
                }

            }
            else
            {
                pnlNotAss.Visible = true;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            txtSearch.Focus();
            
            if (lvwPS.Items.Count != 0)
            {
                if (txtMOP.Text == "Cash")
                {
                    if (txtSnum.Text == "" || txtCashAmt.Text == "") { return; }
                   
                    double camt = Convert.ToDouble(txtCashAmt.Text);
                    string balanceCheck = lvwPS.Items[0].SubItems[1].Text.Substring(2, lvwPS.Items[0].SubItems[1].Text.Length - 2);
                    double bal = Convert.ToDouble(balanceCheck);
                    if (balanceCheck == "0.00" || balanceCheck == "0")
                    {
                        return;
                    }

                    if (camt<bal)
                    {
                        MessageBox.Show("Cash insuficient.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        btnEnter.Enabled = false;
                        btnClear.Enabled = false;
                        dgvSearch.Enabled = false;
                        setupPayment();
                    }
                }
                if (txtMOP.Text == "Installment")
                {
                    if (txtCashAmt.Text == "")
                    {
                        return;
                    }
                    string upon = lvwPS.Items[0].SubItems[1].Text.Substring(2, lvwPS.Items[0].SubItems[1].Text.Length-2);
                    double camt = Convert.ToDouble(txtCashAmt.Text);
                    double adv = 0.00;
                    if (isAdvance == true)
                    {
                        adv = Convert.ToDouble(txtATP.Text);
                    }
                    else
                    {
                        adv = 0.00;
                    }

                    double uamt = Convert.ToDouble(upon);

                    if(lvwPS.Items[0].Text=="Upon enrollment")
                    {
                        if (txtSnum.Text == "" || txtCashAmt.Text == "") { return; }
                        double balance = Convert.ToDouble(lvwPS.Items[1].SubItems[1].Text.Substring(2, lvwPS.Items[1].SubItems[1].Text.Length - 2));
                        if (balance <= 0) { return; }

                        if (camt < uamt ||((camt < uamt) && (balance > uamt)) || camt <= 0 || ((adv < uamt) && (isAdvance == true)) || ((camt < adv) && (isAdvance == true)))
                        {
                            MessageBox.Show("Cash insuficient.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);//camt < balance
                            return;
                        }
                        else
                        {
                            btnEnter.Enabled = false;
                            btnClear.Enabled = false;
                            dgvSearch.Enabled = false;
                            setupPayment();
                        }
                    }
                    else if (lvwPS.Items[0].Text == "Monthly installment")
                    {
                        double balance = 0.00;
                        if(txtSnum.Text == "" || txtCashAmt.Text == ""){return;}
                        if (lvwPS.Items[1].SubItems[1].Text!="")
                        {
                            balance = Convert.ToDouble(lvwPS.Items[1].SubItems[1].Text.Substring(2, lvwPS.Items[1].SubItems[1].Text.Length - 2));
                        }
                        string monthlyamt = lvwPS.Items[0].SubItems[1].Text.Substring(2, lvwPS.Items[0].SubItems[1].Text.Length - 2);
                        double mamt = Convert.ToDouble(monthlyamt);

                        if (camt<balance) { btnPTC.Enabled = false; }
                        if (balance <= mamt) { btnPTC.Enabled = false; }
                        if (balance <= 0) { return; }

                        if (camt < mamt || ((camt < mamt) && (balance > mamt)) || camt <= 0 || ((adv < mamt) && (isAdvance == true) && (balance > mamt)) || ((camt < adv) && (isAdvance == true)))
                        {
                            MessageBox.Show("Cash insuficient.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);//((adv < balance) && (isAdvance == true))
                            return;
                        }
                        else
                        {
                            btnEnter.Enabled = false;
                            btnClear.Enabled = false;
                            dgvSearch.Enabled = false;
                            setupPayment();
                        }
                    }
                    else
                    {
                        if (txtSnum.Text == "" || txtCashAmt.Text == "") { return; }
                        string balance = "";
                        double bamt = 0.00;

                        if (lvwPS.Items[0].Text != "Balance")
                        {
                            balance = lvwPS.Items[1].SubItems[1].Text.Substring(2, lvwPS.Items[1].SubItems[1].Text.Length - 2);
                            bamt = Convert.ToDouble(balance);
                        }
                        else
                        {
                            balance = lvwPS.Items[0].SubItems[1].Text.Substring(2, lvwPS.Items[0].SubItems[1].Text.Length - 2);
                            bamt = Convert.ToDouble(balance);
                        }

                        if (balance == "0.00" || camt <= 0 || balance == "0" || bamt <= 0 || ((camt < adv) && (isAdvance == true)))
                        {
                            return;
                        }
                        else
                        {
                            btnEnter.Enabled = false;
                            btnClear.Enabled = false;
                            dgvSearch.Enabled = false;
                            setupPayment();
                        }
                    }
                }
            }
            else { return; }

    
        }

        private void txtCashAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46) 
            {
                e.Handled = true;

                
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtChange.Clear();
            txtCashAmt.Clear();
            txtATP.Clear();
            chkAdvPay.Checked = false;
            txtCashAmt.Focus();
            isAdvance = false;
            btnPPrev.Enabled = false;
            btnPrintReceipt.Enabled = false;
            btnPTC.Enabled = false;
        }

        private void tmrDateTime_Tick(object sender, EventArgs e)
        {
            txtDate.Text = DateTime.Now.ToLongDateString();
            txtTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void chkAdvPay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdvPay.Checked == true)
            {
                isAdvance = true;
                txtATP.Enabled = true;
                txtATP.Focus();
            }
            else
            {
                isAdvance = false;
                txtATP.Clear();
                txtATP.Enabled = false;
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            //char ch = e.KeyChar;
            //if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            //{
            //    e.Handled = true;
           // }
        }

        private void dgvm_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Hand;
        }

        private void dgvm_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Default;
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Payment")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Payment")
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

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            /*pdReceipt.PrintPage += new PrintPageEventHandler(pcReceipt);
            pprevDlg.Document = pdReceipt;
            pprevDlg.FindForm().StartPosition = FormStartPosition.CenterScreen;
            pprevDlg.FindForm().Size = new System.Drawing.Size(1000, 640);
            pprevDlg.FindForm().Text = "Print preview - Receipt";
            pprevDlg.ShowDialog();*/

            if (pue == "" && p2 == "" && p3 == "" && p4 == "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set dateupon='" + datetoday + "',timeupon='" + txtTime.Text + "',cashierupon='" + COwoex + "',ORNumUpon='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }
            if (pue != "" && p2 == "" && p3 == "" && p4 == "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set date2p='" + datetoday + "',time2p='" + txtTime.Text + "',cashier2p='" + COwoex + "',ORNumP2='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }
            if (pue != "" && p2 != "" && p3 == "" && p4 == "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set date3p='" + datetoday + "',time3p='" + txtTime.Text + "',cashier3p='" + COwoex + "',ORNumP3='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }
            if (pue != "" && p2 != "" && p3 != "" && p4 == "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set date4p='" + datetoday + "',time4p='" + txtTime.Text + "',cashier4p='" + COwoex + "',ORNumP4='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }
            if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 == "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set date5p='" + datetoday + "',time5p='" + txtTime.Text + "',cashier5p='" + COwoex + "',ORNumP5='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }
            if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 == "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set date6p='" + datetoday + "',time6p='" + txtTime.Text + "',cashier6p='" + COwoex + "',ORNumP6='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }
            if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 != "" && p7 == "" && p8 == "" && p9 == "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set date7p='" + datetoday + "',time7p='" + txtTime.Text + "',cashier7p='" + COwoex + "',ORNumP7='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }
            if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 != "" && p7 != "" && p8 == "" && p9 == "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set date8p='" + datetoday + "',time8p='" + txtTime.Text + "',cashier8p='" + COwoex + "',ORNumP8='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }
            if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 != "" && p7 != "" && p8 != "" && p9 == "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set date9p='" + datetoday + "',time9p='" + txtTime.Text + "',cashier9p='" + COwoex + "',ORNumP9='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }
            if (pue != "" && p2 != "" && p3 != "" && p4 != "" && p5 != "" && p6 != "" && p7 != "" && p8 != "" && p9 != "" && p10 == "")
            {
                con.Open();
                string setToPaid = "Update paymentmonthly_tbl set date10p='" + datetoday + "',time10p='" + txtTime.Text + "',cashier10p='" + COwoex + "',ORNumP10='" + payrecno_rec + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdtopd = new OdbcCommand(setToPaid, con);
                cmdtopd.ExecuteNonQuery();
                con.Close();
            }


            pdReceipt.PrintPage += new PrintPageEventHandler(pcReceipt);
            pdlgReceipt.Document = pdReceipt;
            pdlgReceipt.ShowDialog();
            isPrint = true;
            isPayChange = false;
            btnPrintReceipt.Enabled = false;
            btnPPrev.Enabled = false;
            btnPTC.Enabled = false;
           
            if (isPrint==true)
            {
                con.Open();
                string update = "Update receiptno_tbl set receiptno='" + payrecno_rec + "'";
                OdbcCommand cmd = new OdbcCommand(update, con);
                cmd.ExecuteNonQuery();
                con.Close();

                con.Open();
                string upd = "Update receiptno_tbl set transno='" + nextTransNum + "'";
                OdbcCommand cmdupd = new OdbcCommand(upd, con);
                cmdupd.ExecuteNonQuery();
                con.Close();
                isPrint = false;

                if (btnShowNotif.Text == "Hide notifications")
                {
                    btnShowNotif.Text = "Reload notifications";
                }
                
            }

            txtSearch.Focus();
            pnlNextTrans.Visible = true;
           
        }

        public void pcReceipt(object sender, PrintPageEventArgs e)
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
            Font df9 = new Font("Arial", 16, FontStyle.Regular);
            Font df11 = new Font("Arial", 16, FontStyle.Bold);
            Font df3 = new Font("Arial", 11, FontStyle.Regular);
            Font df5 = new Font("Arial", 10, FontStyle.Bold);
            Font df6 = new Font("Arial", 10, FontStyle.Regular);
            Font df7 = new Font("Arial", 8, FontStyle.Regular);
            Font df8 = new Font("Arial", 8, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Create a new pen.
            Pen pen1 = new Pen(Brushes.Black);
            pen1.Width = 1F;
            pen1.LineJoin = System.Drawing.Drawing2D.LineJoin.Miter;


            setupReceiptno();
            //REPORT'S HEADER

            e.Graphics.Clear(Color.White);

            Rectangle r = new Rectangle(90, 40, 100, 95);
            Image newImage = Image.FromFile(@"C:\Users\valued client\Documents\Visual Studio 2010\Projects\1 - THESIS\berlyn.bmp");
            e.Graphics.DrawImage(newImage, r);

            e.Graphics.DrawString("Berlyn Academy", df4, Brushes.Black, 200, 50);
            e.Graphics.DrawString("Lot 77 Phase A, Francisco Homes, CSJDM, Bulacan", df7, Brushes.Black, 200, 70);
            e.Graphics.DrawString("Recognition Nos. E-089 / E-110 / S-002", df7, Brushes.Black, 200, 85);
            e.Graphics.DrawString("Email Address: berlynacademy@yahoo.com", df7, Brushes.Black, 200, 100);
            e.Graphics.DrawString("No.", df11, Brushes.Black, 620, 85);
            e.Graphics.DrawString(payrecno_rec, df9, Brushes.Black, 660, 85);
            e.Graphics.DrawString("Transaction no. " + transno_rec, df6, Brushes.Black, 620, 108);

            Rectangle recbody = new Rectangle(100, 140, 660, 90);
            e.Graphics.DrawRectangle(pen1, recbody);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(600, 211), new Point(600, 500));

            e.Graphics.DrawString("Student no.:", df4, Brushes.Black, 105, 146);
            e.Graphics.DrawString(txtSnum.Text, df3, Brushes.Black, 200, 146);
            e.Graphics.DrawString("School year:", df4, Brushes.Black, 500, 146);
            e.Graphics.DrawString(txtSY.Text, df3, Brushes.Black, 597, 146);

            e.Graphics.DrawString("Student name:", df4, Brushes.Black, 105, 166);
            e.Graphics.DrawString(txtFirst.Text + " " + txtMid.Text + " " + txtLast.Text, df3, Brushes.Black, 215, 166);
            e.Graphics.DrawString("Mode of Payment:", df4, Brushes.Black, 500, 166);
            e.Graphics.DrawString(txtMOP.Text, df3, Brushes.Black, 637, 166);

            e.Graphics.DrawString("Level/Section:", df4, Brushes.Black, 105, 186);
            e.Graphics.DrawString(txtGrd.Text + "-" + txtSec.Text, df3, Brushes.Black, 214, 186);
           
          
            e.Graphics.DrawString("Particulars", df4, Brushes.Black, 300, 211);
            e.Graphics.DrawString("Amount", df4, Brushes.Black, 650, 211);
            Rectangle recmain = new Rectangle(100, 210, 660, 290);
            e.Graphics.DrawRectangle(pen1,recmain);

            double theass_rec = Convert.ToDouble(assessment_rec);
            if (theass_rec >= 1000){
                assessment_rec = String.Format(("{0:0,###.00#}"), theass_rec);}else {
                assessment_rec = String.Format(("{0:0.00#}"), theass_rec);}

            double thebal_rec = Convert.ToDouble(balance_rec);
            if (thebal_rec >= 1000){
                balance_rec = String.Format(("{0:0,###.00#}"), thebal_rec);}else{
                balance_rec = String.Format(("{0:0.00#}"), thebal_rec);}

            double thePayAmtToDisplay_rec = Convert.ToDouble(payamount_rec);
            if (thePayAmtToDisplay_rec >= 1000){
                payamount_rec = String.Format(("{0:0,###.00#}"), thePayAmtToDisplay_rec);}else{
                payamount_rec = String.Format(("{0:0.00#}"), thePayAmtToDisplay_rec);}

            double tfdisplay = Convert.ToDouble(tfAmt_rec);
            if (tfdisplay >= 1000){
                tfAmt_rec = String.Format(("{0:0,###.00#}"), tfdisplay);} else{
                tfAmt_rec = String.Format(("{0:0.00#}"), tfdisplay); }

            double regdisplay = Convert.ToDouble(regiAmt_rec);
            if (regdisplay >= 1000) {
                regiAmt_rec = String.Format(("{0:0,###.00#}"), regdisplay);
            } else{
                regiAmt_rec = String.Format(("{0:0.00#}"), regdisplay);}

            double miscdisplay = Convert.ToDouble(miscAmt_rec);
            if (miscdisplay >= 1000){
                miscAmt_rec = String.Format(("{0:0,###.00#}"), miscdisplay); } else {
                miscAmt_rec = String.Format(("{0:0.00#}"), miscdisplay);  }


            if (paymentNum_rec == "UPON ENROLLMENT")
            {
                e.Graphics.DrawString(desc_upon, df3, Brushes.Black, 120, 240);
                e.Graphics.DrawString("P " + tfAmt_rec, df3, Brushes.Black, 650, 240);
                e.Graphics.DrawString("REGISTRATION FEE", df3, Brushes.Black, 120, 260);
                e.Graphics.DrawString("P " + regiAmt_rec, df3, Brushes.Black, 650, 260);
                e.Graphics.DrawString("MISCELLANEOUS FEE", df3, Brushes.Black, 120, 280);
                e.Graphics.DrawString("P " + miscAmt_rec, df3, Brushes.Black, 650, 280);
            }
            else
            {
                e.Graphics.DrawString(paydesc_rec, df3, Brushes.Black, 120, 240);
                e.Graphics.DrawString("P " + payamount_rec, df3, Brushes.Black, 650, 240);
            }

            if (isPayChange == true)
            {
                double theAmtAddToDisplay_rec = Convert.ToDouble(addAmt_rec);
                if (theAmtAddToDisplay_rec >= 1000){
                    addAmt_rec = String.Format(("{0:0,###.00#}"), theAmtAddToDisplay_rec);}else{
                    addAmt_rec = String.Format(("{0:0.00#}"), theAmtAddToDisplay_rec);}

                if (paymentNum_rec == "UPON ENROLLMENT")
                {
                    e.Graphics.DrawString("Advance:", df3, Brushes.Black, 120, 300);
                    e.Graphics.DrawString("P " + addAmt_rec, df3, Brushes.Black, 650, 300);
                }
                else
                {
                    e.Graphics.DrawString("Advance:", df3, Brushes.Black, 120, 260);
                    e.Graphics.DrawString("P " + addAmt_rec, df3, Brushes.Black, 650, 260);
                }
            }

            e.Graphics.DrawString("Assessment:   P " + assessment_rec, df5, Brushes.Black, 120, 380);
            e.Graphics.DrawString("Balance:         P " + balance_rec, df5, Brushes.Black, 120, 400);
            e.Graphics.DrawString("Cash amount: P "+paycash_rec, df6, Brushes.Black, 120, 423);
            e.Graphics.DrawString("Change:         P "+paychange_rec, df6, Brushes.Black, 120, 440);

            e.Graphics.DrawString(paymentNum_rec, df4, Brushes.Black, 120, 469);
           
            double theAmtToDisplay_rec = Convert.ToDouble(theAmountPaidToSet);
            if (theAmtToDisplay_rec >= 1000){
                theAmountPaidToSet = String.Format(("{0:0,###.00#}"), theAmtToDisplay_rec);}else{
                theAmountPaidToSet = String.Format(("{0:0.00#}"), theAmtToDisplay_rec);}

            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(600, 455), new Point(759, 455));
            e.Graphics.DrawString("P "+theAmountPaidToSet, df4, Brushes.Black, 650, 469);


            //----------------------------------------------------------------
            e.Graphics.DrawString("Note: ", df8, Brushes.Black, 100, 504);
            e.Graphics.DrawString("          No Refund of school fees as mandated in the educational law ( Sec 104 ) after two (2) weeks of regular classes unless there" +
            "\nis a justifiable cause whereby a written notice of withdrawal is to be issued. Ten percent (10%) of the total amount of fees shall be" +
            "\ncollected if the withdrawal is done within two (2) weeks after the start of regular classes and twenty (20%) if the withdrawal is done" +
            "\nafter two (2) weeks but not later than one (1) month.", df7, Brushes.Black, 100, 505);
            e.Graphics.DrawString("Issue Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), df3, Brushes.Black, 500, 186);
            

            e.Graphics.DrawString(txtCashier.Text, df4, Brushes.Black, 100, 590);
            e.Graphics.DrawString("________________________", df3, Brushes.Black, 100, 590);
            e.Graphics.DrawString("Cashier", df6, Brushes.Black, 100, 610);  
        }

        public void setupReceiptno()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select * from receiptno_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                double current = Convert.ToDouble(dt.Rows[0].ItemArray[0].ToString());
                double newrecno = current;
                string receiptformat = "";

                if (newrecno > 0 && newrecno <= 9)
                {
                    receiptformat = "0000000"+newrecno;
                }
                if (newrecno > 10 && newrecno <= 99)
                {
                    receiptformat = "000000" + newrecno;
                }
                if (newrecno > 100 && newrecno <= 999)
                {
                    receiptformat = "00000" + newrecno;
                }
                if (newrecno > 1000 && newrecno <= 9999)
                {
                    receiptformat = "0000" + newrecno;
                }
                if (newrecno > 10000 && newrecno <= 99999)
                {
                    receiptformat = "000" + newrecno;
                }
                if (newrecno > 100000 && newrecno <= 999999)
                {
                    receiptformat = "00" + newrecno;
                }
                if (newrecno > 1000000 && newrecno <= 9999999)
                {
                    receiptformat = "0" + newrecno;
                }
                if (newrecno > 10000000 && newrecno <= 99999999)
                {
                    receiptformat = newrecno.ToString();
                }
                payrecno_rec = receiptformat;
            }
        }

        private void btnPPrev_Click(object sender, EventArgs e)
        {
            btnPrintReceipt.Enabled = true;
            pdReceipt.PrintPage += new PrintPageEventHandler(pcReceipt);
            pprevDlg.Document = pdReceipt;
            pprevDlg.FindForm().StartPosition = FormStartPosition.CenterScreen;
            pprevDlg.FindForm().Size = new System.Drawing.Size(1000, 640);
            pprevDlg.FindForm().Text = "Print preview - Receipt";
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

        private void txtCashAmt_TextChanged(object sender, EventArgs e)
        {
           /* if (txtCashAmt.Text != "")
            {
                txtCashAmt.Focus();
                txtCashAmt.SelectionStart = txtCashAmt.Text.Length;
                double Money = Convert.ToDouble(txtCashAmt.Text);
                if (Money >= 1000)
                {
                    txtCashAmt.Text = String.Format(("{0:0,###.00}"), Money);
                }
                if (Money < 1000)
                {
                    txtCashAmt.Text = String.Format(("{0:0}"), Money);
                }
                
            }*/
        }

        private void btnPTC_Click(object sender, EventArgs e)
        {
            isPayChange = true;
            setupPayment();
            btnPTC.Enabled = false;
        }

        private void lvwPaySched_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnNextTrans_Click(object sender, EventArgs e)
        {
            pnlNextTrans.Visible = false;
            btnEnter.Enabled = true;//ready to accept new transaction
            btnClear.Enabled = true;
            isthereCurrentTransaction = false;
            dgvSearch.Enabled = true;
            txtChange.Clear();
            txtCashAmt.Clear();
            txtCashAmt.Focus();
        }

        private void pnlPdue_MouseEnter(object sender, EventArgs e)
        {
            //pnlPdue.BackColor = Color.WhiteSmoke;
        }

        private void pnlPdisc_MouseEnter(object sender, EventArgs e)
        {
            //pnlPdisc.BackColor = Color.WhiteSmoke;
        }

        private void pnlPdue_MouseLeave(object sender, EventArgs e)
        {
            //pnlPdue.BackColor = Color.White;
        }

        private void pnlPdisc_MouseLeave(object sender, EventArgs e)
        {
            //pnlPdisc.BackColor = Color.White;
        }

        private void lblCountDue_MouseEnter(object sender, EventArgs e)
        {
            //pnlPdue.BackColor = Color.WhiteSmoke;
        }

        private void lblCountDisc_MouseEnter(object sender, EventArgs e)
        {
            //pnlPdisc.BackColor = Color.WhiteSmoke;
        }

        private void lblCountDue_MouseLeave(object sender, EventArgs e)
        {
            //pnlPdue.BackColor = Color.White;
        }

        private void lblCountDisc_MouseLeave(object sender, EventArgs e)
        {
            //pnlPdisc.BackColor = Color.White;
        }

        private void pbPdue_MouseEnter(object sender, EventArgs e)
        {
            //pnlPdue.BackColor = Color.WhiteSmoke;
        }

        private void pbPdisc_MouseEnter(object sender, EventArgs e)
        {
            //pnlPdisc.BackColor = Color.WhiteSmoke;
        }

        private void pbPdue_MouseLeave(object sender, EventArgs e)
        {
            //pnlPdue.BackColor = Color.White;
        }

        private void pbPdisc_MouseLeave(object sender, EventArgs e)
        {
            //pnlPdisc.BackColor = Color.White;
        }

        private void lvwPS_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pnlPdue_Click(object sender, EventArgs e)
        {
            if (pnlTheDiscounted.Visible == false && pnlTheLatePay.Visible == false)
            {
                pnlTheDueDate.Visible = true;
                pnlTheDueDate.Location = new Point(282, 155);
                viewNotifDue = true;
            }
        }

        private void btnCloseDuedate_Click(object sender, EventArgs e)
        {
            lblCountDue.Text = "";
            pnlTheDueDate.Visible = false;
        }

        private void pbPdue_Click(object sender, EventArgs e)
        {
            if (pnlTheDiscounted.Visible == false && pnlTheLatePay.Visible == false)
            {
                pnlTheDueDate.Visible = true;
                pnlTheDueDate.Location = new Point(282, 155);
                viewNotifDue = true;
            }
           
        }

        private void lblCountDue_Click(object sender, EventArgs e)
        {

            if (pnlTheDiscounted.Visible == false && pnlTheLatePay.Visible == false)
            {
                pnlTheDueDate.Visible = true;
                pnlTheDueDate.Location = new Point(282, 155);
                viewNotifDue = true;
            }
           
        }

        public void setupDiscountedStudent()
        {
           
            lvwDiscountedStud.Clear();
            lvwDiscountedStud.Columns.Add("Stud.no.", 90, HorizontalAlignment.Center);
            lvwDiscountedStud.Columns.Add("Student name", 200, HorizontalAlignment.Left);
            lvwDiscountedStud.Columns.Add("Discount", 130, HorizontalAlignment.Left);
            lvwDiscountedStud.Columns[0].ListView.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            lvwDiscountedStud.Columns[1].ListView.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            lvwDiscountedStud.Columns[2].ListView.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from studdiscounted_tbl", con);
            DataTable dts = new DataTable();
            da.Fill(dts);
            con.Close();

            if (dts.Rows.Count > 0)
            {
                for (int s = 0; s < dts.Rows.Count; s++)
                {
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_tbl where studno='" + dts.Rows[s].ItemArray[0].ToString() + "'and syregistered='" + activeSY + "'", con);
                    DataTable dts1 = new DataTable();
                    da1.Fill(dts1);
                    con.Close();

                    if (dts1.Rows.Count > 0)
                    {
                        ListViewItem itm = new ListViewItem();
                        itm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                        itm.Text = dts1.Rows[0].ItemArray[0].ToString();
                        itm.SubItems.Add(dts1.Rows[0].ItemArray[1].ToString());
                        itm.SubItems.Add(dts.Rows[s].ItemArray[1].ToString());
                        lvwDiscountedStud.Items.Add(itm);
                       
                    }

                    con.Open();
                    OdbcDataAdapter da11 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_old_tbl where studno='" + dts.Rows[s].ItemArray[0].ToString() + "'", con);
                    DataTable dts11 = new DataTable();
                    da11.Fill(dts11);
                    con.Close();

                    if (dts11.Rows.Count > 0)
                    {
                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                        itm1.Text = dts11.Rows[0].ItemArray[0].ToString();
                        itm1.SubItems.Add(dts11.Rows[0].ItemArray[1].ToString());
                        itm1.SubItems.Add(dts.Rows[s].ItemArray[1].ToString());
                        lvwDiscountedStud.Items.Add(itm1);
                       
                    }

                    con.Open();
                    OdbcDataAdapter da111 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from stud_tbl where studno='" + dts.Rows[s].ItemArray[0].ToString() + "' and status='Active'", con);
                    DataTable dts111 = new DataTable();
                    da111.Fill(dts111);
                    con.Close();

                    if (dts111.Rows.Count > 0)
                    {
                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                        itm1.Text = dts111.Rows[0].ItemArray[0].ToString();
                        itm1.SubItems.Add(dts111.Rows[0].ItemArray[1].ToString());
                        itm1.SubItems.Add(dts.Rows[s].ItemArray[1].ToString());
                        lvwDiscountedStud.Items.Add(itm1);
                       
                    }
                }
            }

            //-----------------------------------------
            if (lvwDiscountedStud.Items.Count > 0)
            {
                if (viewNotifDisc == false)
                {
                    lblCountDisc.Text = lvwDiscountedStud.Items.Count.ToString();
                }
                else
                {
                    lblCountDisc.Text = "";
                }
            }
            else
            {
                lblCountDisc.Text = "";
            }
        }

        public void setupDueDateStudent()
        {
            lvwDuedate.Clear();
            lvwDuedate.Columns.Add("Stud.no.", 90, HorizontalAlignment.Center);
            lvwDuedate.Columns.Add("Student name", 330, HorizontalAlignment.Left);
            lvwDuedate.Columns[0].ListView.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            lvwDuedate.Columns[1].ListView.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno from paymentcash_tbl where dateregistered='" + DateTime.Now.ToShortDateString() + "'", con);
            DataTable dts = new DataTable();
            da.Fill(dts);
            con.Close();

            if (dts.Rows.Count > 0)
            {
                for (int s = 0; s < dts.Rows.Count; s++)
                {
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_tbl where studno='" + dts.Rows[s].ItemArray[0].ToString() + "'", con);
                    DataTable dts1 = new DataTable();
                    da1.Fill(dts1);
                    con.Close();

                    if (dts1.Rows.Count > 0)
                    {
                        ListViewItem itm = new ListViewItem();
                        itm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                        itm.Text = dts1.Rows[0].ItemArray[0].ToString();
                        itm.SubItems.Add(dts1.Rows[0].ItemArray[1].ToString());
                        lvwDuedate.Items.Add(itm);
                    }

                    con.Open();
                    OdbcDataAdapter da11 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_old_tbl where studno='" + dts.Rows[s].ItemArray[0].ToString() + "'", con);
                    DataTable dts11 = new DataTable();
                    da11.Fill(dts11);
                    con.Close();

                    if (dts11.Rows.Count > 0)
                    {
                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                        itm1.Text = dts11.Rows[0].ItemArray[0].ToString();
                        itm1.SubItems.Add(dts11.Rows[0].ItemArray[1].ToString());
                        lvwDuedate.Items.Add(itm1);
                    }
                }
            }


            con.Open();
            OdbcDataAdapter dai = new OdbcDataAdapter("Select studno from paymentmonthly_tbl where dateregistered='" + DateTime.Now.ToShortDateString() + "'", con);
            DataTable dti = new DataTable();
            dai.Fill(dti);
            con.Close();

            if (dti.Rows.Count > 0)
            {
                //for those enrolee
                for (int s = 0; s < dti.Rows.Count; s++)
                {
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_tbl where studno='" + dti.Rows[s].ItemArray[0].ToString() + "'", con);
                    DataTable dts1 = new DataTable();
                    da1.Fill(dts1);
                    con.Close();

                    if (dts1.Rows.Count > 0)
                    {
                        ListViewItem itm = new ListViewItem();
                        itm.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                        itm.Text = dts1.Rows[0].ItemArray[0].ToString();
                        itm.SubItems.Add(dts1.Rows[0].ItemArray[1].ToString());
                        lvwDuedate.Items.Add(itm);
                    }

                    con.Open();
                    OdbcDataAdapter da11 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_old_tbl where studno='" + dti.Rows[s].ItemArray[0].ToString() + "'", con);
                    DataTable dts11 = new DataTable();
                    da11.Fill(dts11);
                    con.Close();

                    if (dts11.Rows.Count > 0)
                    {
                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                        itm1.Text = dts11.Rows[0].ItemArray[0].ToString();
                        itm1.SubItems.Add(dts11.Rows[0].ItemArray[1].ToString());
                        lvwDuedate.Items.Add(itm1);
                    }
                }
                //end for those enrolee
            }

            //for those enrolled and monthly installment 
            con.Open();
            OdbcDataAdapter dae = new OdbcDataAdapter("Select studno from stud_tbl where status='" + "Active" + "'", con);
            DataTable dte = new DataTable();
            dae.Fill(dte);
            con.Close();

            if (dte.Rows.Count > 0)
            {
                for (int s = 0; s < dte.Rows.Count; s++)
                {
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + dte.Rows[s].ItemArray[0].ToString() + "'", con);
                    DataTable dts1 = new DataTable();
                    da1.Fill(dts1);
                    con.Close();

                    if (dts1.Rows.Count > 0)
                    {
                        string p1 = dts1.Rows[0].ItemArray[5].ToString();
                        string p2 = dts1.Rows[0].ItemArray[6].ToString();
                        string p3 = dts1.Rows[0].ItemArray[7].ToString();
                        string p4 = dts1.Rows[0].ItemArray[8].ToString();
                        string p5 = dts1.Rows[0].ItemArray[9].ToString();
                        string p6 = dts1.Rows[0].ItemArray[10].ToString();
                        string p7 = dts1.Rows[0].ItemArray[11].ToString();
                        string p8 = dts1.Rows[0].ItemArray[12].ToString();
                        string p9 = dts1.Rows[0].ItemArray[13].ToString();
                        string p10 = dts1.Rows[0].ItemArray[14].ToString();
                        string dr = dts1.Rows[0].ItemArray[45].ToString();

                        DateTime comp2 = Convert.ToDateTime(dr).AddMonths(1);
                        string constring2 = comp2.ToShortDateString();
                        DateTime comp3 = Convert.ToDateTime(dr).AddMonths(2);
                        string constring3 = comp3.ToShortDateString();
                        DateTime comp4 = Convert.ToDateTime(dr).AddMonths(3);
                        string constring4 = comp4.ToShortDateString();
                        DateTime comp5 = Convert.ToDateTime(dr).AddMonths(4);
                        string constring5 = comp5.ToShortDateString();
                        DateTime comp6 = Convert.ToDateTime(dr).AddMonths(5);
                        string constring6 = comp6.ToShortDateString();
                        DateTime comp7 = Convert.ToDateTime(dr).AddMonths(6);
                        string constring7 = comp7.ToShortDateString();
                        DateTime comp8 = Convert.ToDateTime(dr).AddMonths(7);
                        string constring8 = comp8.ToShortDateString();
                        DateTime comp9 = Convert.ToDateTime(dr).AddMonths(8);
                        string constring9 = comp9.ToShortDateString();
                        DateTime comp10 = Convert.ToDateTime(dr).AddMonths(9);
                        string constring10 = comp10.ToShortDateString();

                        con.Open();
                        OdbcDataAdapter da2 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from stud_tbl where studno='" + dts1.Rows[0].ItemArray[0].ToString() + "'", con);
                        DataTable dt2 = new DataTable();
                        da2.Fill(dt2);
                        con.Close();

                        if (constring2 == DateTime.Now.ToShortDateString() && p2 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwDuedate.Items.Add(itm1);
                            }
                        }
                        if (constring3 == DateTime.Now.ToShortDateString() && p3 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwDuedate.Items.Add(itm1);
                            }
                        }
                        if (constring4 == DateTime.Now.ToShortDateString() && p4 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwDuedate.Items.Add(itm1);
                            }
                        }
                        if (constring5 == DateTime.Now.ToShortDateString() && p5 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwDuedate.Items.Add(itm1);
                            }
                        }
                        if (constring6 == DateTime.Now.ToShortDateString() && p6 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwDuedate.Items.Add(itm1);
                            }
                        }
                        if (constring7 == DateTime.Now.ToShortDateString() && p7 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwDuedate.Items.Add(itm1);
                            }
                        }
                        if (constring8 == DateTime.Now.ToShortDateString() && p8 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwDuedate.Items.Add(itm1);
                            }
                        }
                        if (constring9 == DateTime.Now.ToShortDateString() && p9 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwDuedate.Items.Add(itm1);
                            }
                        }
                        if (constring10 == DateTime.Now.ToShortDateString() && p10 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwDuedate.Items.Add(itm1);
                            }
                        }
                    }
                }
            }

            //-----------------------------------------
            if (lvwDuedate.Items.Count > 0)
            {
                if (viewNotifDue == false)
                {
                    lblCountDue.Text = lvwDuedate.Items.Count.ToString();
                }
                else
                {
                    lblCountDue.Text = "";
                }
            }
            else
            {
                lblCountDue.Text = "";
            }
          
        }

        public void setupLatePayer()
        {
            lvwLate.Clear();
            lvwLate.Columns.Add("Stud.no.", 90, HorizontalAlignment.Center);
            lvwLate.Columns.Add("Student name", 330, HorizontalAlignment.Left);
            lvwLate.Columns[0].ListView.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            lvwLate.Columns[1].ListView.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);

            con.Open();
            OdbcDataAdapter dae = new OdbcDataAdapter("Select studno from stud_tbl where status='" + "Active" + "'", con);
            DataTable dte = new DataTable();
            dae.Fill(dte);
            con.Close();

            if (dte.Rows.Count > 0)
            {
                for (int s = 0; s < dte.Rows.Count; s++)
                {
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + dte.Rows[s].ItemArray[0].ToString() + "'", con);
                    DataTable dts1 = new DataTable();
                    da1.Fill(dts1);
                    con.Close();

                    if (dts1.Rows.Count > 0)
                    {
                        string p1 = dts1.Rows[0].ItemArray[5].ToString();
                        string p2 = dts1.Rows[0].ItemArray[6].ToString();
                        string p3 = dts1.Rows[0].ItemArray[7].ToString();
                        string p4 = dts1.Rows[0].ItemArray[8].ToString();
                        string p5 = dts1.Rows[0].ItemArray[9].ToString();
                        string p6 = dts1.Rows[0].ItemArray[10].ToString();
                        string p7 = dts1.Rows[0].ItemArray[11].ToString();
                        string p8 = dts1.Rows[0].ItemArray[12].ToString();
                        string p9 = dts1.Rows[0].ItemArray[13].ToString();
                        string p10 = dts1.Rows[0].ItemArray[14].ToString();
                        string dr = dts1.Rows[0].ItemArray[45].ToString();

                        DateTime upn = Convert.ToDateTime(dr);
                        DateTime comp2 = Convert.ToDateTime(dr).AddMonths(1);
                        string constring2 = comp2.ToShortDateString();
                        DateTime comp3 = Convert.ToDateTime(dr).AddMonths(2);
                        string constring3 = comp3.ToShortDateString();
                        DateTime comp4 = Convert.ToDateTime(dr).AddMonths(3);
                        string constring4 = comp4.ToShortDateString();
                        DateTime comp5 = Convert.ToDateTime(dr).AddMonths(4);
                        string constring5 = comp5.ToShortDateString();
                        DateTime comp6 = Convert.ToDateTime(dr).AddMonths(5);
                        string constring6 = comp6.ToShortDateString();
                        DateTime comp7 = Convert.ToDateTime(dr).AddMonths(6);
                        string constring7 = comp7.ToShortDateString();
                        DateTime comp8 = Convert.ToDateTime(dr).AddMonths(7);
                        string constring8 = comp8.ToShortDateString();
                        DateTime comp9 = Convert.ToDateTime(dr).AddMonths(8);
                        string constring9 = comp9.ToShortDateString();
                        DateTime comp10 = Convert.ToDateTime(dr).AddMonths(9);
                        string constring10 = comp10.ToShortDateString();

                        con.Open();
                        OdbcDataAdapter da2 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from stud_tbl where studno='" + dts1.Rows[0].ItemArray[0].ToString() + "'", con);
                        DataTable dt2 = new DataTable();
                        da2.Fill(dt2);
                        con.Close();

                        if (comp2 < DateTime.Now && p2 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }

                        if (comp3 < DateTime.Now && p3 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                        if (comp4 < DateTime.Now && p4 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                        if (comp5 < DateTime.Now && p5 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                        if (comp6 < DateTime.Now && p6 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                        if (comp7 < DateTime.Now && p7 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                        if (comp8 < DateTime.Now && p8 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                        if (comp9 <= DateTime.Now && p9 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                        if (comp10 < DateTime.Now && p10 == "")
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt2.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt2.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                    }
                }
            }

            con.Open();
            OdbcDataAdapter daeoff = new OdbcDataAdapter("Select*from offprereg_tbl where syregistered='" + activeSY + "'", con);
            DataTable dteoff = new DataTable();
            daeoff.Fill(dteoff);
            con.Close();

            if (dteoff.Rows.Count > 0)
            {
                for (int s = 0; s < dteoff.Rows.Count; s++)
                {
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + dteoff.Rows[s].ItemArray[0].ToString() + "'", con);
                    DataTable dts1 = new DataTable();
                    da1.Fill(dts1);
                    con.Close();

                    if (dts1.Rows.Count > 0)
                    {
                        string p1 = dts1.Rows[0].ItemArray[5].ToString();
                        string p2 = dts1.Rows[0].ItemArray[6].ToString();
                        string p3 = dts1.Rows[0].ItemArray[7].ToString();
                        string p4 = dts1.Rows[0].ItemArray[8].ToString();
                        string p5 = dts1.Rows[0].ItemArray[9].ToString();
                        string p6 = dts1.Rows[0].ItemArray[10].ToString();
                        string p7 = dts1.Rows[0].ItemArray[11].ToString();
                        string p8 = dts1.Rows[0].ItemArray[12].ToString();
                        string p9 = dts1.Rows[0].ItemArray[13].ToString();
                        string p10 = dts1.Rows[0].ItemArray[14].ToString();
                        string dr = dts1.Rows[0].ItemArray[45].ToString();

                        DateTime upn = Convert.ToDateTime(dr);
                        DateTime comp2 = Convert.ToDateTime(dr).AddMonths(1);
                        string constring2 = comp2.ToShortDateString();
                        DateTime comp3 = Convert.ToDateTime(dr).AddMonths(2);
                        string constring3 = comp3.ToShortDateString();
                        DateTime comp4 = Convert.ToDateTime(dr).AddMonths(3);
                        string constring4 = comp4.ToShortDateString();
                        DateTime comp5 = Convert.ToDateTime(dr).AddMonths(4);
                        string constring5 = comp5.ToShortDateString();
                        DateTime comp6 = Convert.ToDateTime(dr).AddMonths(5);
                        string constring6 = comp6.ToShortDateString();
                        DateTime comp7 = Convert.ToDateTime(dr).AddMonths(6);
                        string constring7 = comp7.ToShortDateString();
                        DateTime comp8 = Convert.ToDateTime(dr).AddMonths(7);
                        string constring8 = comp8.ToShortDateString();
                        DateTime comp9 = Convert.ToDateTime(dr).AddMonths(8);
                        string constring9 = comp9.ToShortDateString();
                        DateTime comp10 = Convert.ToDateTime(dr).AddMonths(9);
                        string constring10 = comp10.ToShortDateString();

                        con.Open();
                        OdbcDataAdapter da3 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_tbl where studno='" + dts1.Rows[0].ItemArray[0].ToString() + "'", con);
                        DataTable dt3 = new DataTable();
                        da3.Fill(dt3);
                        con.Close();

                        if (upn < DateTime.Now && p1 == "")
                        {
                            if (dt3.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt3.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt3.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                    }
                    else//CASH
                    {
                        con.Open();
                        OdbcDataAdapter da11 = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + dteoff.Rows[s].ItemArray[0].ToString() + "'", con);
                        DataTable dts11 = new DataTable();
                        da11.Fill(dts11);
                        con.Close();

                        if (dts11.Rows.Count > 0)
                        {
                            string p1 = dts11.Rows[0].ItemArray[4].ToString();
                            DateTime upn = Convert.ToDateTime(dts11.Rows[0].ItemArray[3].ToString());

                            if (upn < DateTime.Now && p1 == "")
                            {
                                    ListViewItem itm1 = new ListViewItem();
                                    itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                    itm1.Text = dteoff.Rows[s].ItemArray[0].ToString();
                                    itm1.SubItems.Add(dteoff.Rows[s].ItemArray[3].ToString() + " " + dteoff.Rows[s].ItemArray[1].ToString() + " " + dteoff.Rows[s].ItemArray[2].ToString());
                                    lvwLate.Items.Add(itm1);
                            }
                        }
                    }
                }
            }



            con.Open();
            OdbcDataAdapter daeoffold = new OdbcDataAdapter("Select*from offprereg_old_tbl where syregistered='" + activeSY + "'", con);
            DataTable dteoffold = new DataTable();
            daeoffold.Fill(dteoffold);
            con.Close();

            if (dteoffold.Rows.Count > 0)
            {
                for (int s = 0; s < dteoffold.Rows.Count; s++)
                {
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + dteoffold.Rows[s].ItemArray[0].ToString() + "'", con);
                    DataTable dts1 = new DataTable();
                    da1.Fill(dts1);
                    con.Close();

                    if (dts1.Rows.Count > 0)
                    {
                        string p1 = dts1.Rows[0].ItemArray[5].ToString();
                        string p2 = dts1.Rows[0].ItemArray[6].ToString();
                        string p3 = dts1.Rows[0].ItemArray[7].ToString();
                        string p4 = dts1.Rows[0].ItemArray[8].ToString();
                        string p5 = dts1.Rows[0].ItemArray[9].ToString();
                        string p6 = dts1.Rows[0].ItemArray[10].ToString();
                        string p7 = dts1.Rows[0].ItemArray[11].ToString();
                        string p8 = dts1.Rows[0].ItemArray[12].ToString();
                        string p9 = dts1.Rows[0].ItemArray[13].ToString();
                        string p10 = dts1.Rows[0].ItemArray[14].ToString();
                        string dr = dts1.Rows[0].ItemArray[45].ToString();

                        DateTime upn = Convert.ToDateTime(dr);
                        DateTime comp2 = Convert.ToDateTime(dr).AddMonths(1);
                        string constring2 = comp2.ToShortDateString();
                        DateTime comp3 = Convert.ToDateTime(dr).AddMonths(2);
                        string constring3 = comp3.ToShortDateString();
                        DateTime comp4 = Convert.ToDateTime(dr).AddMonths(3);
                        string constring4 = comp4.ToShortDateString();
                        DateTime comp5 = Convert.ToDateTime(dr).AddMonths(4);
                        string constring5 = comp5.ToShortDateString();
                        DateTime comp6 = Convert.ToDateTime(dr).AddMonths(5);
                        string constring6 = comp6.ToShortDateString();
                        DateTime comp7 = Convert.ToDateTime(dr).AddMonths(6);
                        string constring7 = comp7.ToShortDateString();
                        DateTime comp8 = Convert.ToDateTime(dr).AddMonths(7);
                        string constring8 = comp8.ToShortDateString();
                        DateTime comp9 = Convert.ToDateTime(dr).AddMonths(8);
                        string constring9 = comp9.ToShortDateString();
                        DateTime comp10 = Convert.ToDateTime(dr).AddMonths(9);
                        string constring10 = comp10.ToShortDateString();

                        con.Open();
                        OdbcDataAdapter da4 = new OdbcDataAdapter("Select studno as'No',(select concat(lname,' ',fname,' ',mname)) as 'Student' from offprereg_old_tbl where studno='" + dts1.Rows[0].ItemArray[0].ToString() + "'", con);
                        DataTable dt4 = new DataTable();
                        da4.Fill(dt4);
                        con.Close();

                        if (upn < DateTime.Now && p1 == "")
                        {
                            if (dt4.Rows.Count > 0)
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dt4.Rows[0].ItemArray[0].ToString();
                                itm1.SubItems.Add(dt4.Rows[0].ItemArray[1].ToString());
                                lvwLate.Items.Add(itm1);
                            }
                        }
                    }
                    else//CASH
                    {
                        con.Open();
                        OdbcDataAdapter da11 = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + dteoffold.Rows[s].ItemArray[0].ToString() + "'", con);
                        DataTable dts11 = new DataTable();
                        da11.Fill(dts11);
                        con.Close();

                        if (dts11.Rows.Count > 0)
                        {
                            string p1 = dts11.Rows[0].ItemArray[4].ToString();
                            DateTime upn = Convert.ToDateTime(dts11.Rows[0].ItemArray[3].ToString());

                            if (upn < DateTime.Now && p1 == "")
                            {
                                ListViewItem itm1 = new ListViewItem();
                                itm1.Font = new System.Drawing.Font("Arial", 11, FontStyle.Regular);
                                itm1.Text = dteoffold.Rows[s].ItemArray[0].ToString();
                                itm1.SubItems.Add(dteoffold.Rows[s].ItemArray[3].ToString() + " " + dteoffold.Rows[s].ItemArray[1].ToString() +" "+ dteoffold.Rows[s].ItemArray[2].ToString());
                                lvwLate.Items.Add(itm1);

                            }
                        }
                    }
                }
            }

            //-----------------------------------------
            if (lvwLate.Items.Count > 0)
            {
                if (viewNotifLate == false)
                {
                    lblCountLate.Text = lvwLate.Items.Count.ToString();
                }
                else
                {
                    lblCountLate.Text = "";
                }
            }
            else
            {
                lblCountLate.Text = "";
            }
        }

        private void btnCloseDuedate_MouseEnter(object sender, EventArgs e)
        {

        }

        private void btnCloseDuedate_MouseLeave(object sender, EventArgs e)
        {

        }

        private void pnlPdisc_Click(object sender, EventArgs e)
        {
            if (pnlTheDueDate.Visible == false && pnlTheLatePay.Visible == false)
            {
                pnlTheDiscounted.Visible = true;
                pnlTheDiscounted.Location = new Point(239, 148);
                viewNotifDisc = true;
            }
        }

        private void lblCountDue_MouseEnter_1(object sender, EventArgs e)
        {
            //pnlPdue.BackColor = Color.WhiteSmoke;
        }

        private void lblCountDue_Click_1(object sender, EventArgs e)
        {
            pnlTheDueDate.Visible = true;
            pnlTheDueDate.Location = new Point(239, 148);
        }

        private void lblCountDue_MouseLeave_1(object sender, EventArgs e)
        {
            //pnlPdue.BackColor = Color.White;
        }

        private void pbPdisc_Click(object sender, EventArgs e)
        {
            if (pnlTheDueDate.Visible == false && pnlTheLatePay.Visible == false)
            {
                pnlTheDiscounted.Visible = true;
                pnlTheDiscounted.Location = new Point(239, 148);
                viewNotifDisc = true;
            }
        }

        private void lblCountDisc_Click(object sender, EventArgs e)
        {
            if (pnlTheDueDate.Visible == false && pnlTheLatePay.Visible == false)
            {
                pnlTheDiscounted.Visible = true;
                pnlTheDiscounted.Location = new Point(239, 148);
                viewNotifDisc = true;
            }
        }

        private void btnCloseDiscounted_Click(object sender, EventArgs e)
        {
            lblCountDisc.Text = "";
            pnlTheDiscounted.Visible = false;
        }

        public void retrievedNotificationDisplay()
        {
            if (notifstat == "Hide notifications")
            {
                //pnlNotifHome.BackColor = Color.FromArgb(244, 180, 0);
                tmrwait.Enabled = true;
                pnlNotifHome.Visible = true;
                lblwait.Text = "CHECKING FOR UPDATES...";
                lblwait.Location = new Point(56, 390);
                btnShowNotif.Text = "Hide notifications";
            }
        }
        private void btnShowNotif_Click(object sender, EventArgs e)
        {
            if (btnShowNotif.Text == "Show notifications"|| btnShowNotif.Text=="Reload notifications")
            {
                //pnlNotifHome.BackColor = Color.FromArgb(244,180,0);
                tmrwait.Enabled = true;

                lblwait.Text = "PLEASE WAIT...";
                lblwait.Location = new Point(105, 390);
                pnlNotifHome.Visible = true;
                btnShowNotif.Text = "Hide notifications";
                notifstat = "Hide notifications";
            }
            else
            {
                pnlNotifHome.Visible = false;
                notifstat = "Show notifications";
                btnShowNotif.Text = "Show notifications";
            }
            
        }

        private void btnCloseNotifHome_Click(object sender, EventArgs e)
        {
            pnlNotifHome.Visible = false;
        }

        private void tmrwait_Tick(object sender, EventArgs e)
        {
            if (tickwait++ <= 12)
            {
                lblwait.Visible = true;
            }
            else
            {
                setupDueDateStudent();
                setupDiscountedStudent();
                setupLatePayer();
                lblwait.Visible = false;
                tmrwait.Enabled = false;
                tickwait = 0;
            }
        }

        private void lblCountLate_Click(object sender, EventArgs e)
        {
            if (pnlTheDueDate.Visible == false && pnlTheDiscounted.Visible == false)
            {
                pnlTheLatePay.Visible = true;
                pnlTheLatePay.Location = new Point(239, 148);
                viewNotifLate = true;
            }
        }

        private void pbLate_Click(object sender, EventArgs e)
        {
            if (pnlTheDueDate.Visible == false && pnlTheDiscounted.Visible == false)
            {
                pnlTheLatePay.Visible = true;
                pnlTheLatePay.Location = new Point(239, 148);
                viewNotifLate = true;
            }
        }

        private void pnlLate_Click(object sender, EventArgs e)
        {
            if (pnlTheDueDate.Visible == false && pnlTheDiscounted.Visible == false)
            {
                pnlTheLatePay.Visible = true;
                pnlTheLatePay.Location = new Point(239, 148);
                viewNotifLate = true;
            }
        }

        private void btnCloseLate_Click(object sender, EventArgs e)
        {
            lblCountLate.Text = "";
            pnlTheLatePay.Visible = false;
        }

        private void lvwLate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvm_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (isthereCurrentTransaction == true && isPrint == false)
            {
                MessageBox.Show("Current transaction is not yet finish!", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Activity")
            {
                if (emptype == "Cashier")
                {
                    frmCashierMain casmain = new frmCashierMain();
                    this.Hide();
                    casmain.emptype = emptype;
                    casmain.cashlog = paylog;
                    casmain.accesscode = accesscode;
                    casmain.CO = CashierOperator;
                    casmain.thefac = TheFac;
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
                    pmf.co = CashierOperator;
                    pmf.thefac = TheFac;
                    pmf.prinlog = paylog;
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
                    regmain.co = CashierOperator;
                    regmain.reglog = paylog;
                    regmain.accesscode = accesscode;
                    regmain.thefac = TheFac;
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
                    empf.CO = CashierOperator;
                    empf.faclog = paylog;
                    empf.accesscode = accesscode;
                    empf.TheFacultyName = TheFac;
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
                frmadm.admlog = paylog;
                frmadm.CO = CashierOperator;
                frmadm.accesscode = accesscode;
                frmadm.TheFaculty = TheFac;
                frmadm.VISITED = VISITED;
                frmadm.viewNotifDue = viewNotifDue;
                frmadm.viewNotifDisc = viewNotifDisc;
                frmadm.viewNotifLate = viewNotifLate;
                frmadm.notifstat = notifstat;
                frmadm.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Payment")
            {
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;

            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Student records")
            {
                frmAssessment formStudRec = new frmAssessment();
                this.Hide();
                formStudRec.emptype = emptype;
                formStudRec.co = CashierOperator;
                formStudRec.asslog = paylog;
                formStudRec.accesscode = accesscode;
                formStudRec.thefac = TheFac;
                formStudRec.VISITED = VISITED;
                formStudRec.viewNotifDue = viewNotifDue;
                formStudRec.viewNotifDisc = viewNotifDisc;
                formStudRec.notifstat = notifstat;
                formStudRec.viewNotifLate = viewNotifLate;
                formStudRec.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Student grades")
            {
                frmStdGrd formstdgrd = new frmStdGrd();
                this.Hide();
                formstdgrd.emptype = emptype;
                formstdgrd.CO = CashierOperator;
                formstdgrd.grdlog = paylog;
                formstdgrd.accesscode = accesscode;
                formstdgrd.theFacultyName = TheFac;
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
                stud.CO = CashierOperator;
                stud.studlog = paylog;
                stud.accesscode = accesscode;
                stud.TheFaculty = TheFac;
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
                facf.CO = CashierOperator;
                facf.facinfolog = paylog;
                facf.accesscode = accesscode;
                facf.TheFaculty = TheFac;
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
                frmFacAdv.co = CashierOperator;
                frmFacAdv.advlog = paylog;
                frmFacAdv.accesscode = accesscode;
                frmFacAdv.thefac = TheFac;
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
                frmSec.co = CashierOperator;
                frmSec.seclog = paylog;
                frmSec.accesscode = accesscode;
                frmSec.TheFaculty = TheFac;
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
                rfac.co = CashierOperator;
                rfac.replog = paylog;
                rfac.emptype = emptype;
                rfac.accesscode = accesscode;
                rfac.theFaculty = TheFac;
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
                rsched.CO = CashierOperator;
                rsched.schedlog = paylog;
                rsched.emptype = emptype;
                rsched.accesscode = accesscode;
                rsched.TheFaculty = TheFac;
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
                about.ablog = paylog;
                about.emptype = emptype;
                about.CO = CashierOperator;
                about.accesscode = accesscode;
                about.theFaculty = TheFac;
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
