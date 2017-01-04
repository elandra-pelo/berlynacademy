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
    public partial class frmAdmission : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string admlog, accesscode,emptype,CO,TheFaculty, newStudentNumber, firstdigit, seconddigit, thirddigit, fourthdigit,co,oldstudGenAve,oldstudlevel,reqcode,current,newsnum,thefeelev,enr_LN,enr_MN,activeSY,activeYr,selecteddisc;
        public bool isinserted, ispassones, ispasstenths, ispasshunths, ispassmax, isnospace,isdone,oldstudenrolee,isthereenrolee,isviewAssessment_OtherDisc;
        public static int tick=0;
        public int currnum,MyLevel,SiblingLevel;
        public int lstEnye=1, fnmEnye=1, mnmEnye=1,fatenye=1,motenye=1,guaenye=1;

        public string annualamount_K, uponamount_K, monthlyamount_K, fiftyDisc_K, FreeLastMonthTotal_K, fiftyDiscTotal_K,TFee_K,Reg_K,Mis_K;
        public string annualamount_E, uponamount_E, monthlyamount_E, fiftyDisc_E, FreeLastMonthTotal_E, fiftyDiscTotal_E,TFee_E,Reg_E,Mis_E;
        public string annualamount_J, uponamount_J, monthlyamount_J, fiftyDisc_J, FreeLastMonthTotal_J, fiftyDiscTotal_J,TFee_J,Reg_J,Mis_J;
        public string today, secpay, thipay, foupay, fifpay, sixpay, sevpay, eigpay, ninpay, tenpay;
        public double LessAmt_K, LessAmt_E, LessAmt_J, annualamt_fiftydiscK, anuualamt_freelastmonthK, annualamt_fiftydiscE, anuualamt_freelastmonthE, annualamt_fiftydiscJ, anuualamt_freelastmonthJ,discountedAmtOtherDisc,discountedTotalOtherDisc;
        public string VISITED, amt_monthlyIns_OtherDisc, MONTHLY, notifstat, sibDiscname, siblingGrantee,siblingProvider;
        public bool cancelAdmThruMenuFrm, isVisited, viewNotifDue, viewNotifDisc, viewNotifLate, siblingOldYoung, isApproved;
        public DataView dvoldstud,dvsdv;

        public frmAdmission()
        {
            InitializeComponent();
        }

        private void frmAdmission_Load(object sender, EventArgs e)
        {
            
            //this.BackColor = Color.FromArgb(0, 0, 25);
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //MessageBox.Show(TheFaculty);
            //btnHome.Text = "          " + admlog;
            //pnlend 263, 65

            //IT WILL CHECKED IF ALL SCHEDULE WAS CREATED.
            //---------------------------------------------
            //checkIfOKSched();
            //---------------------------------------------
            GetActiveSchoolYear();
            checkIfAllowedEnrollment();
            
           
            pnlEnd.BackColor = Color.FromArgb(0,0,0,10);

            pnlOldForm.Visible = false;
            pnlRegistered.Visible = false;
            pnlnotify.Visible = true;
            lblHeader.Text = "Registration";
            pnlRegistered.Location = new Point(297, 65);
            lblLogger.Text = admlog;
            lblLoggerFacPos.Text = emptype;
           // pnlnotif.BackColor = Color.FromArgb(60, 186, 84);
         
            
            setupyears();
           
            lvwReq.Columns.Add("", 260, HorizontalAlignment.Left);
          
            btnRemove.Enabled = false;
            lbldismemo.Text = "please select mode of payment.";
     

            lvwSG.Columns.Add("Subject", 120, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 1", 105, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 2", 105, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 3", 105, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 4", 105, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Average", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Remarks", 110, HorizontalAlignment.Center);

            if (isVisited == false)
            {
                if (VISITED.Contains("Admission") == false)
                {
                    VISITED += "   Admission";
                    isVisited = true;
                }
            }

            setupLevelList();
            setupMENU();
            setupStudnum();
            setupDisable();
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

        public void setupLevelList()
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select level from level_tbl", con);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbLev.Items.Clear();
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbLev.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                }
            }
        }

        public void checkIfOKSched()
        {
           
            bool NotOkSched = false;
            int kinderTotal = 0;
            int g1Total = 0;
            int g2Total = 0;
            int g3Total = 0;
            int g4Total = 0;
            int g5Total = 0;
            int g6Total = 0;
            int g7Total = 0;
            int g8Total = 0;
            int g9Total = 0;
            int g10Total = 0;

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select (select count(subject) from subject_tbl where level='Kinder'),(select count(subject) from subject_tbl where level='Grade 1'),(select count(subject) from subject_tbl where level='Grade 2'),(select count(subject) from subject_tbl where level='Grade 3'),(select count(subject) from subject_tbl where level='Grade 4'),(select count(subject) from subject_tbl where level='Grade 5'),(select count(subject) from subject_tbl where level='Grade 6'),(select count(subject) from subject_tbl where level='Grade 7'),(select count(subject) from subject_tbl where level='Grade 8'),(select count(subject) from subject_tbl where level='Grade 9'),(select count(subject) from subject_tbl where level='Grade 10')", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if(dt.Rows.Count>0)
            {
                //plus 4 referring to recess, lunch , homeroom, class preparation
                kinderTotal = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString())+4;
                g1Total = Convert.ToInt32(dt.Rows[0].ItemArray[1].ToString())+4;
                g2Total = Convert.ToInt32(dt.Rows[0].ItemArray[2].ToString()) + 4;
                g3Total = Convert.ToInt32(dt.Rows[0].ItemArray[3].ToString()) + 4;
                g4Total = Convert.ToInt32(dt.Rows[0].ItemArray[4].ToString()) + 4;
                g5Total = Convert.ToInt32(dt.Rows[0].ItemArray[5].ToString()) + 4;
                g6Total = Convert.ToInt32(dt.Rows[0].ItemArray[6].ToString()) + 4;
                g7Total = Convert.ToInt32(dt.Rows[0].ItemArray[7].ToString()) + 4;
                g8Total = Convert.ToInt32(dt.Rows[0].ItemArray[8].ToString()) + 4;
                g9Total = Convert.ToInt32(dt.Rows[0].ItemArray[9].ToString()) + 4;
                g10Total = Convert.ToInt32(dt.Rows[0].ItemArray[10].ToString()) + 4;
            }

            //checks the schedule of every sections of kinder if its completed.
            con.Open();
            OdbcDataAdapter dak = new OdbcDataAdapter("Select section from section_tbl where level='Kinder'", con);
            DataTable dtk = new DataTable();
            dak.Fill(dtk);
            con.Close();

            if (dtk.Rows.Count > 0)
            {
                for (int k = 0; k < dtk.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='"+dtk.Rows[k].ItemArray[0].ToString()+"'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != kinderTotal)
                        {
                            NotOkSched = true;
                            
                        }
                    }
                }
            }


            //checks the schedule of every sections of grade 1 if its completed.
            con.Open();
            OdbcDataAdapter dag1 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 1'", con);
            DataTable dtg1 = new DataTable();
            dag1.Fill(dtg1);
            con.Close();

            if (dtg1.Rows.Count > 0)
            {
                for (int k = 0; k < dtg1.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg1.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g1Total)
                        {
                            NotOkSched = true;

                        }
                    }
                }
            }



            //checks the schedule of every sections of grade 2 if its completed.
            con.Open();
            OdbcDataAdapter dag2 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 2'", con);
            DataTable dtg2 = new DataTable();
            dag2.Fill(dtg2);
            con.Close();

            if (dtg2.Rows.Count > 0)
            {
                for (int k = 0; k < dtg2.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg2.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g2Total)
                        {
                            NotOkSched = true;

                        }
                    }
                }
            }


            //checks the schedule of every sections of grade 3 if its completed.
            con.Open();
            OdbcDataAdapter dag3 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 3'", con);
            DataTable dtg3 = new DataTable();
            dag3.Fill(dtg3);
            con.Close();

            if (dtg3.Rows.Count > 0)
            {
                for (int k = 0; k < dtg3.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg3.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g3Total)
                        {
                            NotOkSched = true;

                        }
                    }
                }
            }


            //checks the schedule of every sections of grade 4 if its completed.
            con.Open();
            OdbcDataAdapter dag4 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 4'", con);
            DataTable dtg4 = new DataTable();
            dag4.Fill(dtg4);
            con.Close();

            if (dtg4.Rows.Count > 0)
            {
                for (int k = 0; k < dtg4.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg4.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g4Total)
                        {
                            NotOkSched = true;

                        }
                    }
                }
            }


            //checks the schedule of every sections of grade 5 if its completed.
            con.Open();
            OdbcDataAdapter dag5 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 5'", con);
            DataTable dtg5 = new DataTable();
            dag5.Fill(dtg5);
            con.Close();

            if (dtg5.Rows.Count > 0)
            {
                for (int k = 0; k < dtg5.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg5.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g5Total)
                        {
                            NotOkSched = true;

                        }
                    }
                }
            }


            //checks the schedule of every sections of grade 6 if its completed.
            con.Open();
            OdbcDataAdapter dag6 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 6'", con);
            DataTable dtg6 = new DataTable();
            dag6.Fill(dtg6);
            con.Close();

            if (dtg6.Rows.Count > 0)
            {
                for (int k = 0; k < dtg6.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg6.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g6Total)
                        {
                            NotOkSched = true;

                        }
                    }
                }
            }


            //checks the schedule of every sections of grade 7 if its completed.
            con.Open();
            OdbcDataAdapter dag7 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 7'", con);
            DataTable dtg7 = new DataTable();
            dag7.Fill(dtg7);
            con.Close();

            if (dtg7.Rows.Count > 0)
            {
                for (int k = 0; k < dtg7.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg7.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g7Total)
                        {
                            NotOkSched = true;

                        }
                    }
                }
            }



            //checks the schedule of every sections of grade 8 if its completed.
            con.Open();
            OdbcDataAdapter dag8 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 8'", con);
            DataTable dtg8 = new DataTable();
            dag8.Fill(dtg8);
            con.Close();

            if (dtg8.Rows.Count > 0)
            {
                for (int k = 0; k < dtg8.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg8.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g8Total)
                        {
                            NotOkSched = true;

                        }
                    }
                }
            }



            //checks the schedule of every sections of grade 9 if its completed.
            con.Open();
            OdbcDataAdapter dag9 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 9'", con);
            DataTable dtg9 = new DataTable();
            dag9.Fill(dtg9);
            con.Close();

            if (dtg9.Rows.Count > 0)
            {
                for (int k = 0; k < dtg9.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg9.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g9Total)
                        {
                            NotOkSched = true;

                        }
                    }
                }
            }



            //checks the schedule of every sections of grade 10 if its completed.
            con.Open();
            OdbcDataAdapter dag10 = new OdbcDataAdapter("Select section from section_tbl where level='Grade 10'", con);
            DataTable dtg10 = new DataTable();
            dag10.Fill(dtg10);
            con.Close();

            if (dtg10.Rows.Count > 0)
            {
                for (int k = 0; k < dtg10.Rows.Count; k++)
                {
                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtg10.Rows[k].ItemArray[0].ToString() + "'", con);
                    DataTable dtk1 = new DataTable();
                    dak1.Fill(dtk1);
                    con.Close();

                    if (dtk1.Rows.Count > 0)
                    {
                        int secsubjcount = Convert.ToInt32(dtk1.Rows[0].ItemArray[0].ToString());
                        if (secsubjcount != g10Total)
                        {
                            NotOkSched = true;
                        }
                    }
                }
            }



            if (NotOkSched == true)
            {
                pnlEnd.Location = new Point(297, 65);
                pnlEnd.Visible = true;
                lblWarningSub.Location = new Point(421, 301);
                lblWarningSub.Text = "System is under maintenance.";
                
                return;
            }


           
        }

        public void checkIfAllowedEnrollment()
        {
            //check if all fees is ok
            con.Open();
            DataTable dt8 = new DataTable();
            OdbcDataAdapter da8 = new OdbcDataAdapter("Select*from fee_tbl where amount ='0.00' and type='fee' and SY='" + activeSY + "'", con);
            da8.Fill(dt8);
            con.Close();
            if (dt8.Rows.Count > 0)
            {
                pnlEnd.Location = new Point(297, 65);
                pnlEnd.Visible = true;

                lblWarningSub.Location = new Point(421, 301);
                lblWarningSub.Text = "System is under maintenance.";
                return;
            }

            //check if all payment is ok
            con.Open();
            DataTable dt9 = new DataTable();
            OdbcDataAdapter da9 = new OdbcDataAdapter("Select*from fee_tbl where amount ='0.00' and type='payment' and SY='" + activeSY + "'", con);
            da9.Fill(dt9);
            con.Close();
            if (dt9.Rows.Count > 0)
            {
                pnlEnd.Location = new Point(297, 65);
                pnlEnd.Visible = true;

                lblWarningSub.Location = new Point(421, 301);
                lblWarningSub.Text = "System is under maintenance.";
                return;
            }

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from enrollmentdays_tbl where SY='"+activeSY+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                DateTime tod;
                string syfirstterm = activeSY.Substring(3, 4).ToString();
                string sysecondterm = activeSY.Substring(8, 4).ToString();
                if (DateTime.Now.Year.ToString() == syfirstterm || DateTime.Now.Year.ToString() == sysecondterm)
                {
                    tod = DateTime.Now;
                }
                else
                {
                    tod = Convert.ToDateTime(dt.Rows[0].ItemArray[0].ToString());
                }
                DateTime end = Convert.ToDateTime(dt.Rows[0].ItemArray[1].ToString());
                TimeSpan remaining = end.Subtract(tod);
                DateTime now = Convert.ToDateTime(tod.ToLongDateString());
                DateTime endlong = Convert.ToDateTime(end.ToLongDateString());

                if (now > endlong)
                {
                    pnlEnd.Location = new Point(297, 65);
                    pnlEnd.Visible = true;

                    lblWarningSub.Location = new Point(434, 301);
                    lblWarningSub.Text = "Enrollment days was end.";
                    return;
                }
               
                if (tod > DateTime.Now)
                {
                    pnlEnd.Location = new Point(297, 65);
                    pnlEnd.Visible = true;

                    lblWarningSub.Location = new Point(434, 301);
                    lblWarningSub.Text = "Enrollment not yet started.";
                    return;
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
            dgvm.Rows[1].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        public void setupDiscountItems(string lev)
        {
            string level = "";

            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + lev+ "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                level = dtdep.Rows[0].ItemArray[0].ToString();
            }

            if (lev == "All levels")
            {
                level = "All";
            }
           
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from discount_tbl where level='"+level+"'or level='All'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbDiscount.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbDiscount.Items.Add(dt.Rows[i].ItemArray[1].ToString());
                }
            }
        }


        public void setupStudnum()
        {
            current = "";
            string yr = "";
            string zerocon = "";

            //yr = DateTime.Now.Year.ToString();
            yr = activeYr;

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from studno_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                currnum = Convert.ToInt32(dt.Rows[0].ItemArray[2].ToString());
                currnum += 1;

                if (currnum > 0 && currnum <= 9)
                {
                    zerocon="000";
                    newsnum = zerocon + currnum.ToString();
                    newStudentNumber = yr + "-" + newsnum;
                }
                if (currnum >=10 && currnum <= 99)
                {
                    zerocon = "00";
                    newsnum = zerocon + currnum.ToString();
                    newStudentNumber = yr + "-" + newsnum;

                }
                if (currnum >= 100 && currnum <= 999)
                {
                    zerocon = "0";
                    newsnum = zerocon + currnum.ToString();
                    newStudentNumber = yr + "-" + newsnum;

                }
                if (currnum >= 1000 && currnum <= 9999)
                {
                    zerocon = "";
                    newsnum = zerocon + currnum.ToString();
                    newStudentNumber = yr + "-" + newsnum;

                }
            }


           
        }

        public void AddOKUpdateStudnoTbl()
        {
            con.Open();
            string updatecurrent = "Update studno_tbl set current='"+newsnum+"',number='" + currnum  + "'";
            OdbcCommand cmd = new OdbcCommand(updatecurrent, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void setupyears()
        {
            int end = 1970;
            int current = Convert.ToInt32(DateTime.Now.Year);

            while (current!=end-1)
            {
                cmbYears.Items.Add(current);
                current--;
            }
        }

        public void setupallrequirements(string type)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from requirement_tbl where type='"+type+"'",con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
               
                cmbReq.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbReq.Items.Add(dt.Rows[i].ItemArray[1].ToString());
                }
            }
            else
            {
               
            }
        }

        private void cmbReq_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwReq.Items.Count == 0)
            {
                if (cmbReq.Text.Contains("138")==true || cmbReq.Text.Contains("Report card")==true)
                {
                    setupEnable();
                    pnlOldForm.Enabled = true;
                    txtLast.Focus();
                    txtSrc.Focus();
                }

                lvwReq.Items.Add(cmbReq.Text);
                pnlnotify.Visible = false;
                pbpass.Visible = true;
                lblnotepass.Visible = true;
                lblnotepass.Text = lvwReq.Items.Count + " requirement was submitted.";
            }
            for (int g = 0; g < lvwReq.Items.Count; g++)
            {
                if (cmbReq.Text == lvwReq.Items[g].Text)
                {
                    return;
                }
                else
                {
                    if (g == lvwReq.Items.Count - 1)
                    {
                        if (cmbReq.Text.Contains("138") == true || cmbReq.Text.Contains("Report card") == true)
                        {
                            setupEnable();
                            pnlOldForm.Enabled = true;
                            txtLast.Focus();
                            txtSrc.Focus();
                        }

                        lvwReq.Items.Add(cmbReq.Text);
                        pbpass.Visible = true;
                        lblnotepass.Visible = true;

                        if (cmbReq.Items.Count == lvwReq.Items.Count)
                        {
                            lblnotepass.Text ="All requirements was submitted.";
                        }
                        else
                        {
                            lblnotepass.Text = lvwReq.Items.Count + " requirements was submitted.";
                        }
                    }
                }
            }

            
            pnlnotify.Visible = false;
            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvwReq.Items.Count<1)
            {
                pnlnotify.Visible = true;
                pbpass.Visible = false;
                lblnotepass.Visible = false;
                return;
               
            }
            else
            {
                if (lvwReq.SelectedItems[0].Text.Contains("138") == true || lvwReq.SelectedItems[0].Text.Contains("Report card") == true)
                {
                    setupDisable();
                    pnlOldForm.Enabled = false;
                }

                lvwReq.SelectedItems[0].Remove();
                btnRemove.Enabled = false;

                if (lvwReq.Items.Count < 1)
                {
                    pnlnotify.Visible = true;
                    pbpass.Visible = false;
                    lblnotepass.Visible = false;
                    if (cmbType.Text == "New/Transferee")
                    {
                        setupallrequirements("NTR");
                    }
                    else
                    {
                        setupallrequirements("OLD");
                    }
                }

                if (lvwReq.Items.Count == 1 && lvwReq.Items.Count!=0)
                {
                    
                    lblnotepass.Text = lvwReq.Items.Count + " requirement was submitted.";
                }
                else
                {
                    
                    lblnotepass.Text = lvwReq.Items.Count + " requirements was submitted.";
                }
               
            }
        }

        private void txtScon_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtParGuaCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
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

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmRegistrarMain regmain = new frmRegistrarMain();
            this.Hide();
            regmain.reglog = admlog;
            regmain.Show();
        }

        private void btnSI_Click(object sender, EventArgs e)
        {
            frmStudInfo si = new frmStudInfo();
            this.Hide();
            si.studlog = admlog;
            si.emptype = "registrar";
            si.TheFaculty = TheFaculty;
            si.Show();
        }

        private void btnFI_Click(object sender, EventArgs e)
        {
            frmFacInfo fi = new frmFacInfo();
            this.Hide();
            fi.facinfolog = admlog;
            fi.emptype = "faculty";
            fi.TheFaculty = TheFaculty;
            fi.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmEmpAbout ea = new frmEmpAbout();
            this.Hide();
            ea.ablog = admlog;
            ea.emptype = "registrar";
            ea.theFaculty = TheFaculty;
            ea.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isthereenrolee = true;

            if (txtFirst.Text == "" || txtLast.Text == "" || cmbLev.Text == "" || txtFat.Text=="" || txtMot.Text=="" ||
              txtAddress.Text == "" || (txtSchool.Text=="" && cmbLev.Text.Contains("Kinder")==false) || cmbMonth.Text == "" || cmbDay.Text == "" ||
             cmbYears.Text == "" || cmbGen.Text == "" || (txtGua.Text=="" || txtParGuaCon.Text=="" || txtRelation.Text=="") ||(txtGua.Text == "" && txtFat.Text == "" && txtMot.Text == ""))
            {
                MessageBox.Show("fill out required fields.", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (lvwReq.Items.Count < 1)
            {
                MessageBox.Show("please submit requirements.", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtEAge.Text != "")
            {
                int age = Convert.ToInt32(txtEAge.Text);
                if (age<=3)
                {
                    MessageBox.Show("Too young to enroll.", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            } 
            if (cmbDay.Text != "")
            {
                if (cmbMonth.Text == "Feb" && Convert.ToInt32(cmbDay.Text) > 28)
                {
                    MessageBox.Show("day of birth is out of range.", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (txtMid.Text != "")
            {
                if (txtMid.TextLength == 1)
                {
                   inputcheck("invalid", txtMid);
                }
                else
                {
                    inputcheck("valid", txtMid);
                }
            }
            if (txtScon.Text != "")
            {
                if ((txtScon.TextLength > 7) && (txtScon.Text.Substring(0, 2)!="09"))
                {
                    inputcheck("invalid", txtScon);
                }
                else if ((txtScon.TextLength != 11) && (txtScon.TextLength != 7))
                {
                    inputcheck("invalid", txtScon);
                }
                else
                {
                    inputcheck("valid", txtScon);
                }
            }
            if (txtParGuaCon.Text != "")
            {
                if ((txtParGuaCon.TextLength > 7) && (txtParGuaCon.Text.Substring(0, 2) != "09"))
                {
                    inputcheck("invalid", txtParGuaCon);
                }
                else if ((txtParGuaCon.TextLength != 11) && (txtParGuaCon.TextLength != 7))
                {
                    inputcheck("invalid", txtParGuaCon);
                }
                else
                {
                    inputcheck("valid", txtParGuaCon);
                }
            }
            if (txtMid.Text == "")
            {
                inputcheck("valid", txtMid);
            }
            if (txtScon.Text == "")
            {
                inputcheck("valid", txtScon);
            }
            if (txtParGuaCon.Text == "")
            {
                inputcheck("valid", txtParGuaCon);
            }
            if ((((txtScon.Text != "") && (((txtScon.TextLength == 11) && (txtScon.Text.Substring(0, 2) == "09")) || (txtScon.TextLength == 7))) || (txtScon.Text == "")) &&
                    (((txtParGuaCon.Text != "") && (((txtParGuaCon.TextLength == 11)&&(txtParGuaCon.Text.Substring(0, 2) == "09")) || (txtParGuaCon.TextLength == 7))) || (txtParGuaCon.Text == "")) &&((txtMid.Text!="" && txtMid.TextLength>1) || txtMid.Text==""))
            {
                inputcheck("valid", txtMid);
                inputcheck("valid", txtScon);
                inputcheck("valid", txtParGuaCon);
                addtoOffPREREG();
                enr_LN = txtLast.Text;
                enr_MN = txtMid.Text;
            }
        }

        public void setupTheReqcode(string lev)
        {
            reqcode = "";
            string THETYPE = "";

            if (cmbType.Text == "New/Transferee")
            {
                THETYPE = "NTR";
            }
            else
            {
                THETYPE = "OLD";
            }

            /*if (isnospace == false)
            {
                setupStudnum();
            }
            else
            {
                firstdigit = "9"; seconddigit = "9"; thirddigit = "9"; fourthdigit = "9";
            }*/

            if (lvwReq.Items.Count > 0)
            {
                for (int i = 0; i < lvwReq.Items.Count; i++)
                {
                    con.Open();
                    OdbcDataAdapter dafind = new OdbcDataAdapter("Select id from requirement_tbl where name='" + lvwReq.Items[i].Text + "'", con);
                    DataTable dtf = new DataTable();
                    dafind.Fill(dtf);
                    con.Close();
                    if (dtf.Rows.Count > 0)
                    {
                        reqcode = reqcode + dtf.Rows[0].ItemArray[0].ToString();
                    }
                }

                con.Open();
                OdbcDataAdapter dasub = new OdbcDataAdapter("Select*from requirement_tbl where type='" + THETYPE + "'", con);
                DataTable dtsub = new DataTable();
                dasub.Fill(dtsub);
                con.Close();

                if (dtsub.Rows.Count > 0)
                {
                    
                    if (THETYPE == "NTR")
                    {
                        for (int s = 0; s < dtsub.Rows.Count; s++)
                        {
                            con.Open();
                            string add = "Insert Into requirementpassed_tbl(studno,level,reqdesc,datesubmitted)values('" + newStudentNumber + "','" + cmbLev.Text + "','" + dtsub.Rows[s].ItemArray[1].ToString() + "','" + "" + "')";
                            OdbcCommand cmdsub = new OdbcCommand(add, con);
                            cmdsub.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    else
                    {
                        for (int s = 0; s < dtsub.Rows.Count; s++)
                        {
                            con.Open();
                           // string delt = "Delete from requirementpassed_tbl where studno='" + txtTheSnum.Text + "'and reqdesc='" + "Form 138 (Report card)" + "'";
                            //OdbcCommand cmddelt = new OdbcCommand(delt, con);
                            //cmddelt.ExecuteNonQuery();

                            string updt = "Update requirementpassed_tbl set level='" + lev + "'where studno='" + txtTheSnum.Text + "'";
                            OdbcCommand cmdupdt = new OdbcCommand(updt,con);
                            cmdupdt.ExecuteNonQuery();
                            con.Close();
                            
                        }
                        for (int i = 0; i < lvwReq.Items.Count; i++)
                        {
                            con.Open();
                            OdbcDataAdapter dafind = new OdbcDataAdapter("Select*from requirement_tbl where name='" + lvwReq.Items[i].Text + "'", con);
                            DataTable dtf = new DataTable();
                            dafind.Fill(dtf);
                            con.Close();
                            if (dtf.Rows.Count > 0)
                            {
                                con.Open();
                                OdbcDataAdapter dachk = new OdbcDataAdapter("Select*from requirementpassed_tbl where studno='" + txtTheSnum.Text + "'and reqdesc='" + dtf.Rows[0].ItemArray[1].ToString() + "'", con);
                                DataTable dtchk = new DataTable();
                                dachk.Fill(dtchk);
                                con.Close();
                                if (dtchk.Rows.Count ==0)
                                {
                                    con.Open();
                                    string add = "Insert Into requirementpassed_tbl(studno,level,reqdesc,datesubmitted)values('" + txtTheSnum.Text + "','" + lev + "','" + dtf.Rows[0].ItemArray[1].ToString() + "','" + "" + "')";
                                    OdbcCommand cmdsub = new OdbcCommand(add, con);
                                    cmdsub.ExecuteNonQuery();
                                    con.Close();


                                }
                            }
                        }
                    }
                }

                if (THETYPE == "NTR")
                {
                    
                    con.Open();
                    OdbcDataAdapter dachk = new OdbcDataAdapter("Select count(name)from requirement_tbl where type='" + THETYPE + "'", con);
                    DataTable dtchk = new DataTable();
                    dachk.Fill(dtchk);
                    con.Close();
                    if (dtchk.Rows.Count > 0)
                    {
                        int rs = Convert.ToInt32(dtchk.Rows[0].ItemArray[0].ToString());
                        for (int g = 0; g < rs; g++)
                        {
                            for (int o = 0; o < lvwReq.Items.Count; o++)
                            {
                                con.Open();
                                OdbcDataAdapter da = new OdbcDataAdapter("Select reqdesc from requirementpassed_tbl where reqdesc LIKE'" + lvwReq.Items[o].Text + "'and studno='" + newStudentNumber + "'", con);
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                con.Close();
                                if (dt.Rows.Count > 0)
                                {
                                    con.Open();
                                    string u = "Update requirementpassed_tbl set datesubmitted='" + DateTime.Now.ToShortDateString() + "'where studno='" + newStudentNumber + "'and reqdesc='" + lvwReq.Items[o].Text + "'";
                                    OdbcCommand cmd = new OdbcCommand(u, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
                else//for old students
                {
                    
                    con.Open();
                    OdbcDataAdapter dachk = new OdbcDataAdapter("Select count(name)from requirement_tbl where type='" + THETYPE + "'", con);
                    DataTable dtchk = new DataTable();
                    dachk.Fill(dtchk);
                    con.Close();
                    if (dtchk.Rows.Count > 0)
                    {
                        int rs = Convert.ToInt32(dtchk.Rows[0].ItemArray[0].ToString());
                        for (int g = 0; g < rs; g++)
                        {
                            for (int o = 0; o < lvwReq.Items.Count; o++)
                            {
                                con.Open();
                                OdbcDataAdapter da = new OdbcDataAdapter("Select reqdesc from requirementpassed_tbl where reqdesc LIKE'" + lvwReq.Items[o].Text + "'and studno='" + txtTheSnum.Text + "'", con);
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                con.Close();
                                if (dt.Rows.Count > 0)
                                {
                                    con.Open();
                                    string u = "Update requirementpassed_tbl set datesubmitted='" + DateTime.Now.ToShortDateString() + "'where studno='" + txtTheSnum.Text + "'and reqdesc='" + lvwReq.Items[o].Text + "'";
                                    OdbcCommand cmd = new OdbcCommand(u, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
            else
            {
                reqcode = "none";
            }
        }
        public void addtoOffPREREG()
        {
            string concatBday = cmbMonth.Text + " " + cmbDay.Text + " " + cmbYears.Text;
            int current = Convert.ToInt32(DateTime.Now.Year);
            int birthyear = Convert.ToInt32(cmbYears.Text);
            int age = current - birthyear;
            

            string yr = activeYr;
            string zerocon = "";

            //ADDED
             con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from studno_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                currnum = Convert.ToInt32(dt.Rows[0].ItemArray[2].ToString());
                currnum += 1;

                if (currnum > 0 && currnum <= 9)
                {
                    zerocon = "000";
                    newsnum = zerocon + currnum.ToString();
                    newStudentNumber = yr + "-" + newsnum;
                }
                if (currnum >= 10 && currnum <= 99)
                {
                    zerocon = "00";
                    newsnum = zerocon + currnum.ToString();
                    newStudentNumber = yr + "-" + newsnum;

                }
                if (currnum >= 100 && currnum <= 999)
                {
                    zerocon = "0";
                    newsnum = zerocon + currnum.ToString();
                    newStudentNumber = yr + "-" + newsnum;

                }
                if (currnum >= 1000 && currnum <= 9999)
                {
                    zerocon = "";
                    newsnum = zerocon + currnum.ToString();
                    newStudentNumber = yr + "-" + newsnum;

                }
            }

            con.Open();
            txtSnum.Text = newStudentNumber;
            string admit = "Insert Into offprereg_tbl(studno,fname,mname,lname,lev,sec,sch,addr,bd,age,gen,scon,fat,fatoc,mot,motoc,gua,guaoc,pgcon,tal,awa,subreq,relationtostud,syregistered)values('"+newStudentNumber+"','" + txtFirst.Text + "','" + txtMid.Text + "','" + txtLast.Text + "','" + cmbLev.Text + "','" + txtSec.Text + "','" + txtSchool.Text + "','" + txtAddress.Text + "','" + concatBday + "','"+age+"','" + cmbGen.Text + "','" + txtScon.Text + "','" + txtFat.Text + "','" + txtFatocc.Text + "','" + txtMot.Text + "','" + txtMotocc.Text + "','" + txtGua.Text + "','" + txtGuaocc.Text + "','" + txtParGuaCon.Text + "','" + txtTalSki.Text + "','" + txtAward.Text + "','"+reqcode+"','"+txtRelation.Text+"','"+activeSY+"')";
            OdbcCommand cmdAdmission = new OdbcCommand(admit, con);
            cmdAdmission.ExecuteNonQuery();
            con.Close();

            btnAdd.Enabled = false;
            isinserted = true;
            setupTheReqcode("");

            //setupStudnum();
            AddOKUpdateStudnoTbl();

            txtParGuaCon.BackColor = Color.White;
            txtScon.BackColor = Color.White;
            pnlAdmit.Visible = false;
            pnlAdmReq.Visible = false;
            lblHeader.Text = "Assessment";
            pnlRegistered.Location = new Point(297, 65);
            pnlRegistered.Visible = true;

            //------ set up success registration
            SucessRegistration();
            
        }

        public void SucessRegistration()
        {
            string thefeelevel = "";
            txtenrolee.Text = txtFirst.Text + " " + txtMid.Text + " " + txtLast.Text;
            txtenroleegrd.Text = cmbLev.Text;
            if (txtenroleegrd.Text == "Kinder") { MyLevel = 0; }
            if (txtenroleegrd.Text == "Grade 1") { MyLevel = 1; }
            if (txtenroleegrd.Text == "Grade 2") { MyLevel = 2; }
            if (txtenroleegrd.Text == "Grade 3") { MyLevel = 3; }
            if (txtenroleegrd.Text == "Grade 4") { MyLevel = 4; }
            if (txtenroleegrd.Text == "Grade 5") { MyLevel = 5; }
            if (txtenroleegrd.Text == "Grade 6") { MyLevel = 6; }
            if (txtenroleegrd.Text == "Grade 7") { MyLevel=7;}
            if (txtenroleegrd.Text == "Grade 8") { MyLevel = 8; }
            if (txtenroleegrd.Text == "Grade 9") { MyLevel = 9; }
            if (txtenroleegrd.Text == "Grade 10") { MyLevel = 10; }

            
            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + cmbLev.Text + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                thefeelevel = dtdep.Rows[0].ItemArray[0].ToString();
            }

           
            thefeelev = thefeelevel;
            lblstatusicon.Text = "a"; lblstatusicon.ForeColor = Color.White;
            lblstatus.Text = "Registration successful!"; lblstatus.ForeColor = Color.White;
            pnlnotif.BackColor = Color.ForestGreen;
            
            setupAssessmentPerLevel(thefeelevel);
            lvwPaySched.Clear(); pnldisnotify.Visible = true;
            lbldismemo.Text = "please select mode of payment.";
            lbldismemo.Location = new Point(88, 8);

            btnCanReg.Text = "Cancel admission";
            btnCanReg.Size = new Size(136, 33);
            btnCanReg.Location = new Point(160, 475);
            btnOK.Visible = true;
            btnOK.Location = new Point(303, 475);

            pnlMop.Visible = true;
            pnlMop.Location = new Point(12, 176);
        }

        public void setupAssessmentPerLevel(string levelkey) 
        {
           
            lvwAssessment.Clear();
            lvwAssessment.Columns.Add("Fee description", 393, HorizontalAlignment.Left);
            lvwAssessment.Columns.Add("Amount", 190, HorizontalAlignment.Left);
         
            string totalAss = "";
            //TOTAL ASSESSMENT DISPLAY -------------------------------------------------------------------------------------------------------------
            con.Open();
            OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and fee<>'TUITION FEE'and fee<>'REGISTRATION'and fee<>'MISCELLANEOUS'and fee='ANNUAL PAYMENT'and SY='" + activeSY + "'", con);
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
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where level='" + levelkey + "'and type='fee'and SY='"+activeSY+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblnoass.Visible = false;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[1].ToString().Contains("TUITION FEE")==true)
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
                    if (dt.Rows[j].ItemArray[1].ToString().Contains("REGISTRATION")==true)
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
                double amtAssessment = Convert.ToDouble(totalAss);
                if (amtAssessment >= 1000) {
                    totalAss = String.Format(("{0:0,###.00#}"), amtAssessment); } 
                if (amtAssessment < 1000){
                    totalAss = String.Format(("{0:0.00#}"), amtAssessment); }

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
                if (chkWithDisc.Checked==true)
                {
                    string discountedAmtDisp = "";
                    string discTotalAssDisp = "";

              
                    if (txtenroleegrd.Text == "Kinder")
                    {
                        if (chkWithDisc.Checked == false)
                        {
                            ListViewItem itmfee1 = new ListViewItem();
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + "0.00");
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }

                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true) || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true))
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
                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("Second") == true) || cmbDiscount.Text.Contains("2nd") == true))
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
                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == false && cmbDiscount.Text.Contains("First") == false && cmbDiscount.Text.Contains("1st") == false && cmbDiscount.Text.Contains("Second") == false && cmbDiscount.Text.Contains("2nd") == false)))
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

                            ListViewItem itmfee1 = new ListViewItem();
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                    }
                    else if (txtenroleegrd.Text == "Grade 7" || txtenroleegrd.Text == "Grade 8" || txtenroleegrd.Text == "Grade 9" || txtenroleegrd.Text == "Grade 10")
                    {
                        if (chkWithDisc.Checked == false)
                        {
                            ListViewItem itmfee1 = new ListViewItem();
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + "0.00");
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
    
                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true) || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true))
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
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("Second") == true) || cmbDiscount.Text.Contains("2nd") == true))
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
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == false && cmbDiscount.Text.Contains("First") == false && cmbDiscount.Text.Contains("1st") == false && cmbDiscount.Text.Contains("Second") == false && cmbDiscount.Text.Contains("2nd") == false)))
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

                      
                            ListViewItem itmfee1 = new ListViewItem();
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                    }
                    else
                    {
                        if (chkWithDisc.Checked == false)
                        {
                            ListViewItem itmfee1 = new ListViewItem();
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + "0.00");
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }

                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true) || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true))
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
                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("Second") == true) || cmbDiscount.Text.Contains("2nd") == true))
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
                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == false && cmbDiscount.Text.Contains("First") == false && cmbDiscount.Text.Contains("1st") == false && cmbDiscount.Text.Contains("Second") == false && cmbDiscount.Text.Contains("2nd") == false)))
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

                            ListViewItem itmfee1 = new ListViewItem();
                            itmfee1.Text = "Less:";
                            itmfee1.SubItems.Add("                         P " + discountedAmtDisp);
                            lvwAssessment.Items.Add(itmfee1);
                            itmfee1.Font = new Font("Arial", 11, FontStyle.Regular);
                        }
                    }

                    double amt = Convert.ToDouble(discTotalAssDisp);
                    if (amt >= 1000)
                    {
                        discTotalAssDisp = String.Format(("{0:0,###.00#}"), amt);
                    } if (amt < 1000)
                    {
                        discTotalAssDisp = String.Format(("{0:0.00#}"), amt);
                    }

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
                double amt = Convert.ToDouble(totalAss);
                if (amt >= 1000)
                {
                    totalAss = String.Format(("{0:0,###.00#}"), amt);
                } if (amt < 1000)
                {
                    totalAss = String.Format(("{0:0.00#}"), amt);
                }

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
                lblnoass.Visible = true;
                //lvwAssessment.Visible = false;
            }
        }

        public void inputcheck(string result, TextBox txt)
        {
            if (result == "valid")
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
                    txt.BackColor = Color.Silver;
                   
                }
                else
                {
                }
            }
        }

        private void btnGrd_Click(object sender, EventArgs e)
        {
            frmStdGrd formgrade = new frmStdGrd();
            this.Hide();
            formgrade.grdlog = admlog;
            formgrade.theFacultyName = TheFaculty;
            formgrade.Show();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbMop.Text == "")
            {
                return;
            }

            
            if (isdone == false)
            {
                con.Open();
                string updateoffprereg = "Update offprereg_tbl set mop='" + cmbMop.Text + "',sibgrantee='"+siblingGrantee+"',sibdesc='"+sibDiscname+"',sibprovider='"+siblingProvider+"'where studno='" + txtSnum.Text + "'and syregistered='" + activeSY + "'";
                OdbcCommand cmdu = new OdbcCommand(updateoffprereg, con);
                cmdu.ExecuteNonQuery();
                con.Close();

                if (cmbMop.Text == "Cash")
                {
                    string datetoday = DateTime.Now.ToShortDateString();

                    if (txtenroleegrd.Text == "Kinder")
                    {
                        retrievedAssessmentKinder();//theres a connection inside
                        con.Open();

                        double annualamttostore = 0;
                        if (chkWithDisc.Checked == true)
                        {
                            if (cmbDiscount.Text.Contains("siblings") == true || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true)
                            {
                                annualamttostore = anuualamt_freelastmonthK;
                            }
                            else if (cmbDiscount.Text.Contains("Second") == true || cmbDiscount.Text.Contains("2nd") == true)
                            {
                                annualamttostore = annualamt_fiftydiscK;
                            }
                            else
                            {
                                annualamttostore = discountedTotalOtherDisc;
                            }
                        }
                        else
                        {
                            annualamttostore = Convert.ToDouble(annualamount_K);
                        }

                        string delduplicate = "Delete from paymentcash_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelduplicate = new OdbcCommand(delduplicate, con);
                        cmddelduplicate.ExecuteNonQuery();

                        string addcash = "Insert Into paymentcash_tbl(studno,level,amount,datedue,datepd,timepd,cashier,dateregistered)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" + annualamttostore + "','" + datetoday + "','" + "" + "','" + "" + "','" + "" + "','" + today + "')";
                        OdbcCommand cmdaddcash = new OdbcCommand(addcash, con);
                        cmdaddcash.ExecuteNonQuery();

                        string delmon = "Delete from paymentmonthly_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelmon = new OdbcCommand(delmon, con);
                        cmddelmon.ExecuteNonQuery();

                        con.Close();
                    }
                    else if (txtenroleegrd.Text == "Grade 1" || txtenroleegrd.Text == "Grade 2" || txtenroleegrd.Text == "Grade 3" ||
                        txtenroleegrd.Text == "Grade 4" || txtenroleegrd.Text == "Grade 5" || txtenroleegrd.Text == "Grade 6")
                    {
                        retrievedAssessmentElem();//theres a connection inside
                        con.Open();

                        double annualamttostore = 0;
                        if (chkWithDisc.Checked == true)
                        {
                            if (cmbDiscount.Text.Contains("siblings") == true || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true)
                            {
                                annualamttostore = anuualamt_freelastmonthE;
                            }
                            else if (cmbDiscount.Text.Contains("Second") == true || cmbDiscount.Text.Contains("2nd") == true)
                            {
                                annualamttostore = annualamt_fiftydiscE;
                            }
                            else
                            {
                                annualamttostore = discountedTotalOtherDisc;
                            }
                        }
                        else
                        {
                            annualamttostore = Convert.ToDouble(annualamount_E);
                        }

                        string delduplicate = "Delete from paymentcash_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelduplicate = new OdbcCommand(delduplicate, con);
                        cmddelduplicate.ExecuteNonQuery();

                        string addcash = "Insert Into paymentcash_tbl(studno,level,amount,datedue,datepd,timepd,cashier,dateregistered)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" + annualamttostore + "','" + datetoday + "','" + "" + "','" + "" + "','" + "" + "','" + today + "')";
                        OdbcCommand cmdaddcash = new OdbcCommand(addcash, con);
                        cmdaddcash.ExecuteNonQuery();

                        string delmon = "Delete from paymentmonthly_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelmon = new OdbcCommand(delmon, con);
                        cmddelmon.ExecuteNonQuery();

                        con.Close();
                    }
                    else
                    {
                        retrievedAssessmentJunior();//theres a connection inside
                        con.Open();

                        double annualamttostore = 0;
                        if (chkWithDisc.Checked == true)
                        {
                            if (cmbDiscount.Text.Contains("siblings") == true || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true)
                            {
                                annualamttostore = anuualamt_freelastmonthJ;
                            }
                            else if (cmbDiscount.Text.Contains("Second") == true || cmbDiscount.Text.Contains("2nd") == true)
                            {
                                annualamttostore = annualamt_fiftydiscJ;
                            }
                            else
                            {
                                annualamttostore = discountedTotalOtherDisc;
                            }
                        }
                        else
                        {
                            annualamttostore = Convert.ToDouble(annualamount_J);
                        }

                        string delduplicate = "Delete from paymentcash_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelduplicate = new OdbcCommand(delduplicate, con);
                        cmddelduplicate.ExecuteNonQuery();

                        string addcash = "Insert Into paymentcash_tbl(studno,level,amount,datedue,datepd,timepd,cashier,dateregistered)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" + annualamttostore + "','" + datetoday + "','" + "" + "','" + "" + "','" + "" + "','" + today + "')";
                        OdbcCommand cmdaddcash = new OdbcCommand(addcash, con);
                        cmdaddcash.ExecuteNonQuery();

                        string delmon = "Delete from paymentmonthly_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelmon = new OdbcCommand(delmon, con);
                        cmddelmon.ExecuteNonQuery();

                        con.Close();
                    }
                }
                if (cmbMop.Text == "Installment")
                {
                    if (txtenroleegrd.Text == "Kinder")
                    {
                        retrievedAssessmentKinder();//theres a connection inside
                        con.Open();

                        string delduplicate = "Delete from paymentmonthly_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelduplicate = new OdbcCommand(delduplicate, con);
                        cmddelduplicate.ExecuteNonQuery();

                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true) || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true))
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_K + "','" + anuualamt_freelastmonthK + "','" + FreeLastMonthTotal_K
                            + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + monthlyamount_K + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }
                        else if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("Second") == true) || cmbDiscount.Text.Contains("2nd") == true))
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_K + "','" + annualamt_fiftydiscK + "','" + fiftyDiscTotal_K
                            + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + monthlyamount_K + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }
                        else if ((chkWithDisc.Checked == true) && (cmbDiscount.Text.Contains("siblings") == false && cmbDiscount.Text.Contains("First") == false && cmbDiscount.Text.Contains("1st") == false && cmbDiscount.Text.Contains("Second") == false && cmbDiscount.Text.Contains("2nd") == false))
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_K + "','" + discountedTotalOtherDisc + "','" + discountedTotalOtherDisc
                            + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + amt_monthlyIns_OtherDisc + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }
                        else
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_K + "','" + annualamount_K + "','" + annualamount_K + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + monthlyamount_K + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }

                        string delcash = "Delete from paymentcash_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelcash = new OdbcCommand(delcash, con);
                        cmddelcash.ExecuteNonQuery();

                        con.Close();
                    }
                    else if (txtenroleegrd.Text == "Grade 1" || txtenroleegrd.Text == "Grade 2" || txtenroleegrd.Text == "Grade 3" ||
                        txtenroleegrd.Text == "Grade 4" || txtenroleegrd.Text == "Grade 5" || txtenroleegrd.Text == "Grade 6")
                    {
                        retrievedAssessmentElem();//theres a connection inside
                        con.Open();

                        string delduplicate = "Delete from paymentmonthly_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelduplicate = new OdbcCommand(delduplicate, con);
                        cmddelduplicate.ExecuteNonQuery();

                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true) || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true))
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_E + "','" + anuualamt_freelastmonthE + "','" + FreeLastMonthTotal_E
                            + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + monthlyamount_E + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }
                        else if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("Second") == true) || cmbDiscount.Text.Contains("2nd") == true))
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_E + "','" + annualamt_fiftydiscE + "','" + fiftyDiscTotal_E
                            + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + monthlyamount_E + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }
                        else if ((chkWithDisc.Checked == true) && (cmbDiscount.Text.Contains("siblings") == false && cmbDiscount.Text.Contains("First") == false && cmbDiscount.Text.Contains("1st") == false && cmbDiscount.Text.Contains("Second") == false && cmbDiscount.Text.Contains("2nd") == false))
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_E + "','" + discountedTotalOtherDisc + "','" + discountedTotalOtherDisc
                            + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + amt_monthlyIns_OtherDisc + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }
                        else
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_E + "','" + annualamount_E + "','" + annualamount_E + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + monthlyamount_E + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }

                        string delcash = "Delete from paymentcash_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelcash = new OdbcCommand(delcash, con);
                        cmddelcash.ExecuteNonQuery();

                        con.Close();
                    }
                    else
                    {
                        retrievedAssessmentJunior();//theres a connection inside
                        con.Open();

                        string delduplicate = "Delete from paymentmonthly_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelduplicate = new OdbcCommand(delduplicate, con);
                        cmddelduplicate.ExecuteNonQuery();

                        if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true) || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true))
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_J + "','" + anuualamt_freelastmonthJ + "','" + FreeLastMonthTotal_J
                            + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + monthlyamount_J + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }
                        else if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("Second") == true) || cmbDiscount.Text.Contains("2nd") == true))
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_J + "','" + annualamt_fiftydiscJ + "','" + fiftyDiscTotal_J
                            + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + monthlyamount_J + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }
                        else if ((chkWithDisc.Checked == true) && (cmbDiscount.Text.Contains("siblings") == false && cmbDiscount.Text.Contains("First") == false && cmbDiscount.Text.Contains("1st") == false && cmbDiscount.Text.Contains("Second") == false && cmbDiscount.Text.Contains("2nd") == false))
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                            uponamount_J + "','" + discountedTotalOtherDisc + "','" + discountedTotalOtherDisc
                            + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + amt_monthlyIns_OtherDisc + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }
                        else
                        {
                            string addmon = "Insert Into paymentmonthly_tbl(studno,level,uponenrollment,annual,balance,dateupon,date2p,date3p,date4p,date5p,date6p,date7p,date8p,date9p,date10p,amtupon,amt2p,amt3p,amt4p,amt5p,amt6p,amt7p,amt8p,amt9p,amt10p,timeupon,time2p,time3p,time4p,time5p,time6p,time7p,time8p,time9p,time10p,cashierupon,cashier2p,cashier3p,cashier4p,cashier5p,cashier6p,cashier7p,cashier8p,cashier9p,cashier10p,dateregistered,MONTHLY)values('" + txtSnum.Text + "','" + txtenroleegrd.Text + "','" +
                              uponamount_J + "','" + annualamount_J + "','" + annualamount_J + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "0" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + today + "','" + monthlyamount_J + "')";
                            OdbcCommand cmdaddmon = new OdbcCommand(addmon, con);
                            cmdaddmon.ExecuteNonQuery();
                        }

                        string delcash = "Delete from paymentcash_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmddelcash = new OdbcCommand(delcash, con);
                        cmddelcash.ExecuteNonQuery();

                        con.Close();
                    }
                }

                if (oldstudenrolee == true)
                {
                    con.Open();
                    OdbcDataAdapter daSelectInfofromstudtbl = new OdbcDataAdapter("Select*from stud_tbl where studno='" + txtSnum.Text + "'", con);
                    DataTable dtSIFS = new DataTable();
                    daSelectInfofromstudtbl.Fill(dtSIFS);
                    con.Close();

                    if (dtSIFS.Rows.Count > 0)
                    {
                        con.Open();
                        string delete = "Delete from offprereg_old_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(delete, con);
                        cmd1.ExecuteNonQuery();

                        string add = "Insert Into offprereg_old_tbl (studno,fname,mname,lname,lev,sec,sch,addr,bd,age,gen,scon,fat,fatoc,mot,motoc,gua,guaoc,pgcon,tal,awa,subreq,mop,syenrolled,syregistered,pgrel,sibgrantee,sibdesc,sibprovider)values('" +
                        dtSIFS.Rows[0].ItemArray[0].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[1].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[2].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[3].ToString() + "','" +
                        txtenroleegrd.Text + "','" +
                        "" + "','" +
                        dtSIFS.Rows[0].ItemArray[6].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[7].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[8].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[9].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[10].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[11].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[12].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[13].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[14].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[15].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[16].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[17].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[18].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[19].ToString() + "','" +
                        dtSIFS.Rows[0].ItemArray[20].ToString() + "','" +
                        reqcode + "','" +
                       cmbMop.Text + "','" +
                        dtSIFS.Rows[0].ItemArray[23].ToString() + "','"+activeSY+"','"+dtSIFS.Rows[0].ItemArray[25].ToString()+"','"+
                        siblingGrantee+"','"+sibDiscname+"','"+siblingProvider+"')";

                        OdbcCommand cmd = new OdbcCommand(add, con);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        /*con.Open();
                        string delete = "Delete from stud_tbl where studno='" + txtSnum.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(delete, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();*/

                        //oldstudenrolee = false;
                    }
                }
            }
            //original code of this event
            pnlRegistered.Visible = false;
            pnlAdmit.Visible = true;
            pnlAdmReq.Visible = true;
            pnlnotify.Visible = true;
            btnAdd.Enabled = true;

            lblnotedone.Visible = false;
            pnlOldForm.Location = new Point(0, 70);
            pnlOldForm.Visible = false;
            pnlform.Visible = true;
            pnlform.Location = new Point(9, 57);

            if (chkWithDisc.Checked == true)
            {
                con.Open();
                string del = "Delete from studdiscounted_tbl where studno='" + txtSnum.Text + "'";
                OdbcCommand cmddel = new OdbcCommand(del, con);
                cmddel.ExecuteNonQuery();

                string addToStudentWithDiscount = "Insert Into studdiscounted_tbl(studno,disctype,provider)values('" + txtSnum.Text + "','" + cmbDiscount.Text + "','"+siblingProvider+"')";
                OdbcCommand cmddisc = new OdbcCommand(addToStudentWithDiscount, con);
                cmddisc.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from studdiscounted_tbl where studno='" + txtSnum.Text + "'",con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    con.Open();
                    string del = "Delete from studdiscounted_tbl where studno='" + txtSnum.Text + "'";
                    OdbcCommand cmddel = new OdbcCommand(del, con);
                    cmddel.ExecuteNonQuery();
                    con.Close();
                }
            }

            isdone = true;
            isthereenrolee = false;
           

            con.Open();
            string deleteStd = "Delete from stud_tbl where studno='" + txtSnum.Text + "'";
            OdbcCommand cmdStd = new OdbcCommand(deleteStd, con);
            cmdStd.ExecuteNonQuery();
            con.Close();
            //student will set to active after undergoing with regis.

            chkHasClearance.Checked = false;
            chkClrd.Checked = false;
            cmbMop.SelectedIndex = -1;
            lvwPaySched.Clear();
            setupClear();
            setupEnroleeTypeItems();
            setupDisable();
            btnAdd.Enabled = false;
            chkWithDisc.Checked = false;
            lblHeader.Text = "Registration";

            btnApprove.Text = "Approve";
            chkOthers.Checked = false;
            pnlGuardian.Visible = true;
            pnlGuardian.Location = new Point(1, 4);
            txtGua.Location = new Point(-300, 3);
            btnCancelOthers.Visible = false;
            btnNGua.Visible = false;

            isApproved = false;
            lblApproved.Text = "";
            ClearSibDiscVerif();
            setupStudnum();
           
        }

        public void ClearSibDiscVerif()
        {
           // lblApproved.Visible = false;
            //lblApproved.Text = "";
            txtlst.Text = "";
            txtmnm.Text = "";
            txtfnm.Text = "";
            txtgdr.Text = "";
            txtagesi.Text = "";
            txtbdt.Text = "";
            txtfatnm.Text = "";
            txtmotnm.Text = "";
            txtgdnm.Text = "";
            txtsno.Text = "";
            txtSILevel.Text = "";
            btnApprove.Text = "Approve";
        }

        public void setupEnroleeTypeItems()
        {
            cmbType.Items.Clear();
            cmbType.Items.Add("New/Transferee");
            cmbType.Items.Add("Old");
        }

        public void setupClear()
        {
            txtLast.Clear();
            txtMid.Clear();
            txtFirst.Clear();
            txtSchool.Clear();
            txtRelation.Clear();
            cmbLev.SelectedIndex = -1;
            txtAddress.Clear();
            txtEAge.Clear();
            cmbMonth.SelectedIndex = -1;
            cmbDay.SelectedIndex = -1;
            cmbYears.SelectedIndex = -1;
            cmbGen.SelectedIndex = -1;
            txtScon.Clear();
            txtFat.Clear(); txtFatocc.Clear();
            txtMot.Clear(); txtMotocc.Clear();
            txtGua.Clear(); txtGuaocc.Clear();
            txtParGuaCon.Clear();
            txtTalSki.Clear();
            txtAward.Clear();
            chkFatG.Checked = false;
            chkMotG.Checked = false;
            chkOthers.Checked = false;
            
            cmbReq.SelectedIndex = -1;
            lvwReq.Items.Clear();
            lvwPaySched.Clear();
            pbpass.Visible = false;
            lblnotepass.Visible = false;
            isdone = false;
            
        }

        private void cmbMop_SelectedIndexChanged(object sender, EventArgs e)
        {
            today = DateTime.Now.ToShortDateString();
           
            if (txtenroleegrd.Text == "Kinder")
            {
                retrievedAssessmentKinder();
        
                if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true) || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true))
                {
                    txtLessAmt.Text = "P " + monthlyamount_K;
                }
                if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("Second") == true) || cmbDiscount.Text.Contains("2nd") == true))
                {
                    txtLessAmt.Text = "P " + LessAmt_K.ToString();
                }
                if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == false && cmbDiscount.Text.Contains("First") == false && cmbDiscount.Text.Contains("1st") == false && cmbDiscount.Text.Contains("Second") == false && cmbDiscount.Text.Contains("2nd") == false)))
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from discount_tbl where discname='"+cmbDiscount.Text+"'",con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        string rate = dt.Rows[0].ItemArray[3].ToString();
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
                        discountedTotalOtherDisc = TF_amt+Mis_amt+Reg_amt;
                        
                        string DiscamountDisp = "";
                        if (discountedAmtOtherDisc >= 1000)
                        {
                            DiscamountDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                        }
                        if (discountedAmtOtherDisc < 1000)
                        {
                            DiscamountDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                        }
                        txtLessAmt.Text = "P "+DiscamountDisp.ToString();
                        txtRate.Text = rate;
                    }
                }
                
            }
            else if (txtenroleegrd.Text == "Grade 7" || txtenroleegrd.Text == "Grade 8" || txtenroleegrd.Text == "Grade 9" || txtenroleegrd.Text == "Grade 10")
            {
                retrievedAssessmentJunior();
               
                if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true) || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true))
                {
                    txtLessAmt.Text = "P " + monthlyamount_J;
                }
                if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("Second") == true) || cmbDiscount.Text.Contains("2nd") == true))
                {
                    txtLessAmt.Text = "P " + LessAmt_J.ToString();
                }
                if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == false && cmbDiscount.Text.Contains("First") == false && cmbDiscount.Text.Contains("1st") == false && cmbDiscount.Text.Contains("Second") == false && cmbDiscount.Text.Contains("2nd") == false)))
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from discount_tbl where discname='" + cmbDiscount.Text + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        string rate = dt.Rows[0].ItemArray[3].ToString();
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

                        string DiscamountDisp = "";
                        if (discountedAmtOtherDisc >= 1000)
                        {
                            DiscamountDisp = String.Format(("{0:0,###.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                        }
                        if (discountedAmtOtherDisc < 1000)
                        {
                            DiscamountDisp = String.Format(("{0:0.00#}"), Convert.ToDouble(discountedAmtOtherDisc));
                        }
                        txtLessAmt.Text = "P "+DiscamountDisp.ToString();
                        txtRate.Text = rate;
                    }
                    //setupAssessmentPerLevel(thefeelevel);
                }
                
            }
            else
            {
                retrievedAssessmentElem();
                if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true) || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true))
                {
                    txtLessAmt.Text = "P " + monthlyamount_E;
                }
                if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("Second") == true) || cmbDiscount.Text.Contains("2nd") == true))
                {
                    txtLessAmt.Text = "P " + LessAmt_E.ToString();
                }
                if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == false && cmbDiscount.Text.Contains("First") == false && cmbDiscount.Text.Contains("1st") == false && cmbDiscount.Text.Contains("Second") == false && cmbDiscount.Text.Contains("2nd") == false)))
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from discount_tbl where discname='" + cmbDiscount.Text + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        string rate = dt.Rows[0].ItemArray[3].ToString();
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
                        txtLessAmt.Text = "P "+DiscamountDisp.ToString();
                        txtRate.Text = rate;
                    }
                    //setupAssessmentPerLevel(thefeelevel);
                }
               
            }

           
            lblnotedone.Visible = true;
            tmrAssess.Enabled = true;
            lvwPaySched.Clear();
        }

       
        public void retrievedAssessmentKinder()
        {
             annualamount_K="";
             uponamount_K = "";
             monthlyamount_K = "";
             fiftyDisc_K = "";
             FreeLastMonthTotal_K = "";
             fiftyDiscTotal_K = "";
             TFee_K = "";
             Reg_K = "";
             Mis_K = "";

             string levdept = "";
             con.Open();
             OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtenroleegrd.Text + "'", con);
             DataTable dtdep = new DataTable();
             dadep.Fill(dtdep);
             con.Close();
             if (dtdep.Rows.Count > 0)
             {
                 levdept = dtdep.Rows[0].ItemArray[0].ToString();
             }

             con.Open();
             OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and fee='TUITION FEE'and SY='" + activeSY + "'", con);
             DataTable dt0 = new DataTable();
             da0.Fill(dt0);
             con.Close();
             if (dt0.Rows.Count > 0)
             {
                 TFee_K = dt0.Rows[0].ItemArray[2].ToString();

             }

             con.Open();
             OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and fee='REGISTRATION'and SY='" + activeSY + "'", con);
             DataTable dt01 = new DataTable();
             da01.Fill(dt01);
             con.Close();
             if (dt01.Rows.Count > 0)
             {
                 Reg_K = dt01.Rows[0].ItemArray[2].ToString();

             }

             con.Open();
             OdbcDataAdapter da011 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and fee='MISCELLANEOUS'and SY='" + activeSY + "'", con);
             DataTable dt011 = new DataTable();
             da011.Fill(dt011);
             con.Close();
             if (dt011.Rows.Count > 0)
             {
                 Mis_K = dt011.Rows[0].ItemArray[2].ToString();

             }


            con.Open();
            OdbcDataAdapter dakinder = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and SY='" + activeSY + "'", con);
            DataTable dtkinder = new DataTable();
            dakinder.Fill(dtkinder);
            con.Close();

            if(dtkinder.Rows.Count>0)
            {
                for (int a = 0; a < dtkinder.Rows.Count;a++ )
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

            string levdept = "";
            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtenroleegrd.Text + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdept = dtdep.Rows[0].ItemArray[0].ToString();
            }

            con.Open();
            OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and fee='TUITION FEE'and SY='" + activeSY + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            con.Close();
            if (dt0.Rows.Count > 0)
            {
                TFee_E = dt0.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and fee='REGISTRATION'and SY='" + activeSY + "'", con);
            DataTable dt01 = new DataTable();
            da01.Fill(dt01);
            con.Close();
            if (dt01.Rows.Count > 0)
            {
                Reg_E = dt01.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da011 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and fee='MISCELLANEOUS'and SY='" + activeSY + "'", con);
            DataTable dt011 = new DataTable();
            da011.Fill(dt011);
            con.Close();
            if (dt011.Rows.Count > 0)
            {
                Mis_E = dt011.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter daelem = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and SY='" + activeSY + "'", con);
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

            string levdept = "";
            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtenroleegrd.Text + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdept = dtdep.Rows[0].ItemArray[0].ToString();
            }

            con.Open();
            OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and fee='TUITION FEE'and SY='" + activeSY + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            con.Close();
            if (dt0.Rows.Count > 0)
            {
                TFee_J = dt0.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and fee='REGISTRATION'and SY='" + activeSY + "'", con);
            DataTable dt01 = new DataTable();
            da01.Fill(dt01);
            con.Close();
            if (dt01.Rows.Count > 0)
            {
                Reg_J = dt01.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter da011 = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and fee='MISCELLANEOUS'and SY='" + activeSY + "'", con);
            DataTable dt011 = new DataTable();
            da011.Fill(dt011);
            con.Close();
            if (dt011.Rows.Count > 0)
            {
                Mis_J = dt011.Rows[0].ItemArray[2].ToString();

            }

            con.Open();
            OdbcDataAdapter dajr = new OdbcDataAdapter("Select*from fee_tbl where level='" + levdept + "'and SY='" + activeSY + "'", con);
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
         public void schedForInstallment_SecToNinPayment_j(string monthlyamount_J)
         {
             ListViewItem itmjhi0 = new ListViewItem();
             itmjhi0.Text = "2ND PAYMENT";
             itmjhi0.SubItems.Add(secpay);
             itmjhi0.SubItems.Add("P "+monthlyamount_J);
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

        public void setupPaymentSchedule()
        {
            lvwPaySched.Columns.Add("Payments",220,HorizontalAlignment.Left);
            lvwPaySched.Columns.Add("Date due", 220, HorizontalAlignment.Center);
            lvwPaySched.Columns.Add("Amount", 150, HorizontalAlignment.Right);

            today = DateTime.Now.ToShortDateString();
            secpay = DateTime.Now.AddMonths(1).ToShortDateString();
            thipay = DateTime.Now.AddMonths(2).ToShortDateString();
            foupay = DateTime.Now.AddMonths(3).ToShortDateString();
            fifpay = DateTime.Now.AddMonths(4).ToShortDateString();
            sixpay = DateTime.Now.AddMonths(5).ToShortDateString();
            sevpay = DateTime.Now.AddMonths(6).ToShortDateString();
            eigpay = DateTime.Now.AddMonths(7).ToShortDateString();
            ninpay = DateTime.Now.AddMonths(8).ToShortDateString();
            tenpay = DateTime.Now.AddMonths(9).ToShortDateString();

            string levdep = "";

            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtenroleegrd.Text + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdep = dtdep.Rows[0].ItemArray[0].ToString();
            }

            if (txtenroleegrd.Text == "Kinder")
            {
                lvwPaySched.Items.Clear();
                retrievedAssessmentKinder();
               
                if (cmbMop.Text == "Cash")
                {
                    if (chkWithDisc.Checked == true)
                    {
                        if (cmbDiscount.Text.Contains("siblings") == true || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true)
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
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                        else if (cmbDiscount.Text.Contains("Second") == true || cmbDiscount.Text.Contains("2nd") == true)
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
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                        itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                if (cmbMop.Text == "Installment")
                {
                    ListViewItem itmki = new ListViewItem();
                    itmki.Text = "UPON ENROLLMENT";
                    itmki.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                    itmki.SubItems.Add(today);
                    itmki.SubItems.Add("P " + uponamount_K);
                    lvwPaySched.Items.Add(itmki);

                   
                    if (chkWithDisc.Checked == true)
                    {
                        if (cmbDiscount.Text.Contains("siblings") == true || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true)
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
                        else if (cmbDiscount.Text.Contains("Second") == true || cmbDiscount.Text.Contains("2nd") == true)
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
            if (txtenroleegrd.Text == "Grade 1" || txtenroleegrd.Text == "Grade 2" || txtenroleegrd.Text == "Grade 3" ||
                txtenroleegrd.Text == "Grade 4" || txtenroleegrd.Text == "Grade 5" || txtenroleegrd.Text == "Grade 6")
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

                if (cmbMop.Text == "Cash")
                {
                    if (chkWithDisc.Checked == true)
                    {
                        if (cmbDiscount.Text.Contains("siblings") == true || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true)
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
                            itmec.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                        else if (cmbDiscount.Text.Contains("Second") == true || cmbDiscount.Text.Contains("2nd") == true)
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
                            itmec.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                        itmec.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                if (cmbMop.Text == "Installment")
                {
                    ListViewItem itmei = new ListViewItem();
                    itmei.Text = "UPON ENROLLMENT";
                    itmei.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                    itmei.SubItems.Add(today);
                    itmei.SubItems.Add("P " + uponamount_E);
                    lvwPaySched.Items.Add(itmei);

                    
                    if (chkWithDisc.Checked == true)
                    {
                        if (cmbDiscount.Text.Contains("siblings") == true || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true)
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
                        else if (cmbDiscount.Text.Contains("Second") == true || cmbDiscount.Text.Contains("2nd") == true)
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
                        itmeitotal.SubItems.Add("P "+amt_dis);
                        lvwPaySched.Items.Add(itmeitotal);
                    }
                }
            }
            if (txtenroleegrd.Text == "Grade 7" || txtenroleegrd.Text == "Grade 8" || txtenroleegrd.Text == "Grade 9" ||
               txtenroleegrd.Text == "Grade 10")
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


                if (cmbMop.Text == "Cash")
                {
                    if (chkWithDisc.Checked == true)
                    {
                        if (cmbDiscount.Text.Contains("siblings") == true || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true)
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
                            itmjhc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmjhc.Text = "ANNUAL PAYMENT";
                            itmjhc.SubItems.Add(today);
                            itmjhc.SubItems.Add("P "+amt_dis);
                            lvwPaySched.Items.Add(itmjhc);

                            ListViewItem itmjhctotal = new ListViewItem();
                            itmjhctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                            itmjhctotal.Text = "Total:";
                            itmjhctotal.SubItems.Add("");
                            itmjhctotal.SubItems.Add("P "+amt_dis);
                            lvwPaySched.Items.Add(itmjhctotal);
                        }
                        else if (cmbDiscount.Text.Contains("Second") == true || cmbDiscount.Text.Contains("2nd") == true)
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
                            itmjhc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                            itmkc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
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
                        itmjhc.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                        itmjhc.Text = "ANNUAL PAYMENT";
                        itmjhc.SubItems.Add(today);
                        itmjhc.SubItems.Add("P "+amt_dis);
                        lvwPaySched.Items.Add(itmjhc);

                        ListViewItem itmjhctotal = new ListViewItem();
                        itmjhctotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                        itmjhctotal.Text = "Total:";
                        itmjhctotal.SubItems.Add("");
                        itmjhctotal.SubItems.Add("P "+amt_dis);
                        lvwPaySched.Items.Add(itmjhctotal);
                    }
                }
                if (cmbMop.Text == "Installment")
                {
                    ListViewItem itmjhi = new ListViewItem();
                    itmjhi.Text = "UPON ENROLLMENT";
                    itmjhi.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                    itmjhi.SubItems.Add(today);
                    itmjhi.SubItems.Add("P "+uponamount_J);
                    lvwPaySched.Items.Add(itmjhi);


                    if (chkWithDisc.Checked == true)
                    {
                        if (cmbDiscount.Text.Contains("siblings") == true || cmbDiscount.Text.Contains("First") == true || cmbDiscount.Text.Contains("1st") == true)
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
                            itmkitotal.SubItems.Add("P "+amt_dis);

                            lvwPaySched.Items.Add(itmkitotal);
                        }
                        else if (cmbDiscount.Text.Contains("Second") == true || cmbDiscount.Text.Contains("2nd") == true)
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
                            itmki8.SubItems.Add("P "+fiftyDisc_J);
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
                        itmjhi8.SubItems.Add("P "+monthlyamount_J);
                        lvwPaySched.Items.Add(itmjhi8);

                        ListViewItem itmjhitotal = new ListViewItem();
                        itmjhitotal.Font = new System.Drawing.Font("Arial", 11, (FontStyle.Bold));
                        itmjhitotal.Text = "Total:";
                        itmjhitotal.SubItems.Add("");
                        itmjhitotal.SubItems.Add("P "+amt_dis);
                        lvwPaySched.Items.Add(itmjhitotal);

                    }
                }
            }
        }

        private void lvwReq_Click(object sender, EventArgs e)
        {
            btnRemove.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtLast.Text = "wowjj";
            txtMid.Text = "jkjlk";
            txtFirst.Text = "hahahha";
            txtAddress.Text = "gkgkgkg";
            txtSchool.Text = "sti";
            cmbGen.Text = "Male";
            cmbMonth.Text = "Jan";
            cmbDay.Text = "12";
            cmbYears.Text = "2012";
            cmbLev.Text = "Kinder";
            txtGua.Text = "ewkjkhan";
            txtGuaocc.Text = "ajkwan";
            txtParGuaCon.Text = "09202721856";
        }

        public void CancelRegistration()
        {
            string backtoOldLevel = "";
            string enroleeUpcomingLevel = txtenroleegrd.Text;

            if (enroleeUpcomingLevel == "Grade 1")
            {
                backtoOldLevel = "Kinder";
            }
            if (enroleeUpcomingLevel == "Grade 2")
            {
                backtoOldLevel = "Grade 1";
            }
            if (enroleeUpcomingLevel == "Grade 3")
            {
                backtoOldLevel = "Grade 2";
            }
            if (enroleeUpcomingLevel == "Grade 4")
            {
                backtoOldLevel = "Grade 3";
            }
            if (enroleeUpcomingLevel == "Grade 5")
            {
                backtoOldLevel = "Grade 4";
            }
            if (enroleeUpcomingLevel == "Grade 6")
            {
                backtoOldLevel = "Grade 5";
            }
            if (enroleeUpcomingLevel == "Grade 7")
            {
                backtoOldLevel = "Grade 6";
            }
            if (enroleeUpcomingLevel == "Grade 8")
            {
                backtoOldLevel = "Grade 7";
            }
            if (enroleeUpcomingLevel == "Grade 9")
            {
                backtoOldLevel = "Grade 8";
            }
            if (enroleeUpcomingLevel == "Grade 10")
            {
                backtoOldLevel = "Grade 9";
            }

            /*string delete = "Delete from offprereg_tbl where studno='" + txtSnum.Text + "'";
            OdbcCommand cmddel = new OdbcCommand(delete, con);
            cmddel.ExecuteNonQuery();

            string delete1 = "Delete from offprereg_old_tbl where studno='" + txtSnum.Text + "'";
            OdbcCommand cmddel1 = new OdbcCommand(delete1, con);
            cmddel1.ExecuteNonQuery();

            //NEW COMMENTED B4 UPDATED
            /*string delete2 = "Delete from paymentcash_tbl where studno='" + txtSnum.Text + "'";
            OdbcCommand cmddel2 = new OdbcCommand(delete2, con);
            cmddel2.ExecuteNonQuery();

            string delete3 = "Delete from paymentmonthly_tbl where studno='" + txtSnum.Text + "'";
            OdbcCommand cmddel3 = new OdbcCommand(delete3, con);
            cmddel3.ExecuteNonQuery();
                     
                string delete4 = "Delete from stud_tbl where studno='" + txtSnum.Text + "'";
            OdbcCommand cmd4 = new OdbcCommand(delete4, con);
            cmd4.ExecuteNonQuery();
                * 
                *  string upd1 = "Update offprereg_tbl set lev='"+backtoOldLevel+"'where studno='" + txtSnum.Text + "'";
            OdbcCommand cmdu1 = new OdbcCommand(upd1, con);
            cmdu1.ExecuteNonQuery();
                * 
                *  string upd2 = "Update offprereg_old_tbl set lev='"+backtoOldLevel+"'where studno='" + txtSnum.Text + "'";
            OdbcCommand cmdu2 = new OdbcCommand(upd2, con);
            cmdu2.ExecuteNonQuery();
                */

            if (oldstudenrolee == true)
            {
                con.Open();

                string updateReqsTableToOldLevel = "Update requirementpassed_tbl set level='" + backtoOldLevel + "'where studno='" + txtTheSnum.Text + "'";
                OdbcCommand cmdupdtbl = new OdbcCommand(updateReqsTableToOldLevel, con);
                cmdupdtbl.ExecuteNonQuery();

                string delete = "Delete from offprereg_tbl where studno='" + txtSnum.Text + "'and syregistered='" + activeSY + "'";
                OdbcCommand cmddel = new OdbcCommand(delete, con);
                cmddel.ExecuteNonQuery();

                string delete1 = "Delete from offprereg_old_tbl where studno='" + txtSnum.Text + "'";
                OdbcCommand cmddel1 = new OdbcCommand(delete1, con);
                cmddel1.ExecuteNonQuery();

                string upd3 = "Update stud_tbl set level='" + backtoOldLevel + "'where studno='" + txtSnum.Text + "'";
                OdbcCommand cmdu3 = new OdbcCommand(upd3, con);
                cmdu3.ExecuteNonQuery();

                /*string delete2 = "Delete from paymentcash_tbl where studno='" + txtSnum.Text + "'";
                OdbcCommand cmddel2 = new OdbcCommand(delete2, con);
                cmddel2.ExecuteNonQuery();

                string delete3 = "Delete from paymentmonthly_tbl where studno='" + txtSnum.Text + "'";
                OdbcCommand cmddel3 = new OdbcCommand(delete3, con);
                cmddel3.ExecuteNonQuery();*/

                con.Close();
            }
            else
            {
                int current = Convert.ToInt32(txtSnum.Text.Substring(5, 4));
                int newcurrent = current;
                //string toreplace = "";

                string yr = "";
                string zerocon = "";

                //yr = DateTime.Now.Year.ToString();
                yr = activeYr;

                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from studno_tbl", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    currnum = Convert.ToInt32(dt.Rows[0].ItemArray[2].ToString());
                    currnum -= 1;

                    if (currnum >=0 && currnum <= 9)
                    {
                        zerocon = "000";
                        newsnum = zerocon + currnum.ToString();
                        newStudentNumber = yr + "-" + newsnum;
                    }
                    if (currnum >= 10 && currnum <= 99)
                    {
                        zerocon = "00";
                        newsnum = zerocon + currnum.ToString();
                        newStudentNumber = yr + "-" + newsnum;

                    }
                    if (currnum >= 100 && currnum <= 999)
                    {
                        zerocon = "0";
                        newsnum = zerocon + currnum.ToString();
                        newStudentNumber = yr + "-" + newsnum;

                    }
                    if (currnum >= 1000 && currnum <= 9999)
                    {
                        zerocon = "";
                        newsnum = zerocon + currnum.ToString();
                        newStudentNumber = yr + "-" + newsnum;

                    }

                    con.Open();
                    string update = "Update studno_tbl set current='" + newsnum + "',number='" + currnum + "'";
                    OdbcCommand cmdcan = new OdbcCommand(update, con);
                    cmdcan.ExecuteNonQuery();
                }

                
                   
                //MessageBox.Show("new");
                string delete = "Delete from requirementpassed_tbl where studno='" + txtSnum.Text + "'";
                OdbcCommand cmddel = new OdbcCommand(delete, con);
                cmddel.ExecuteNonQuery();

                string delete4 = "Delete from offprereg_tbl where studno='" + txtSnum.Text + "'and syregistered='" + activeSY + "'";
                OdbcCommand cmddel4 = new OdbcCommand(delete4, con);
                cmddel4.ExecuteNonQuery();

                string delete2 = "Delete from paymentcash_tbl where studno='" + txtSnum.Text + "'";
                OdbcCommand cmddel2 = new OdbcCommand(delete2, con);
                cmddel2.ExecuteNonQuery();

                string delete3 = "Delete from paymentmonthly_tbl where studno='" + txtSnum.Text + "'";
                OdbcCommand cmddel3 = new OdbcCommand(delete3, con);
                cmddel3.ExecuteNonQuery();

                con.Close();
            }


            if (cancelAdmThruMenuFrm == false)
            {
                lblstatus.Text = "Admission cancelled."; lblstatus.ForeColor = Color.White;
                lblstatusicon.Text = "r"; lblstatusicon.ForeColor = Color.White;
                btnCanReg.Text = "Close";
                btnCanReg.Size = new Size(83, 33);
                btnCanReg.Location = new Point(303, 192);
                pnlnotif.BackColor = Color.Firebrick;
                btnOK.Visible = false;

                
                txtSnum.Clear(); txtenrolee.Clear(); txtenroleegrd.Clear();
                cmbMop.Items.Clear();
                cmbMop.Items.Add("Cash");
                cmbMop.Items.Add("Installment");

                chkHasClearance.Checked = false;
                chkClrd.Checked = false;
                chkWithDisc.CheckState = CheckState.Unchecked;
                pnlMop.Visible = false;
                setupEnroleeTypeItems();
                setupDisable();
                btnAdd.Enabled = false;
                isdone = true;
                isthereenrolee = false;
                lvwAssessment.Clear();
                lvwPaySched.Clear(); pnldisnotify.Visible = false;
            }
        }

        private void btnCanReg_Click(object sender, EventArgs e)
        {
            if (btnCanReg.Text == "Cancel admission")
            {
                if (MessageBox.Show("Do you really want to cancel admission?", "Admission", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CancelRegistration();
                }
                else
                {
                    return;
                }
            }
            else
            {
                pnlRegistered.Visible = false;
                pnlAdmit.Visible = true;
                pnlAdmReq.Visible = true;
                pnlnotify.Visible = true;
                pnlOldForm.Location = new Point(0, 70);
                pnlOldForm.Visible = false;
                pnlform.Visible = true;
                pnlform.Location = new Point(9, 57);
               
                isdone = true;
                oldstudenrolee = false;
                isthereenrolee = false;

                chkHasClearance.Checked = false;
                chkClrd.Checked = false;
                cmbMop.SelectedIndex = -1;
                lvwPaySched.Clear();
                setupClear();
                lblHeader.Text = "Registration";
            }

            setupStudnum();
        }

        private void btnfacrep_Click(object sender, EventArgs e)
        {
            frmReport rff = new frmReport();
            this.Hide();
            rff.replog = admlog;
            rff.emptype = "faculty";
            rff.theFaculty = TheFaculty;
            rff.Show();
        }

        public void setupClearOldStudDisplay()
        {
            txtTheSnum.Clear();
            txtSY.Clear();
            txtLN.Clear();
            txtFN.Clear();
            txtMN.Clear();
            txtGen.Clear();
            txtBD.Clear();
            txtAge.Clear();
            txtAdd.Clear();
            txtGn.Clear();
            txtCon.Clear();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupClear();
            setupDisable();
            txtSrc.Clear();
            chkHasClearance.Checked = false;
            chkClrd.Checked = false;
            lvwSG.Items.Clear();
            setupClearOldStudDisplay();
            //isdone = true;
            isthereenrolee = false;

            if (cmbType.Text == "")
            {
                
            }
            else
            {
                lvwReq.Items.Clear();
                pnlnotify.Visible = true;
                string type = "";
                if (cmbType.Text == "New/Transferee")
                {
                    pnlEtype.BackColor = Color.WhiteSmoke;
                    type = "NTR";
                    pnlOldForm.Visible = false;
                    pnlform.Visible = true;
                    lblHeaderAdmission.Text = "Personal Information";
                    oldstudenrolee = false;
                }
                else
                {
                    pnlEtype.BackColor = Color.White;
                    type = "OLD";
                    pnlform.Visible = false;
                    pnlOldForm.Visible = true;
                    pnlOldForm.Location = new Point(9, 57);
                    lblHeaderAdmission.Text="Checking for Promotion";
                    setupOldStudentList();
                    pnlOldForm.Enabled = false;
                    cmbFilter.Text = "Student's name";
                    oldstudenrolee = true;
                }

                setupallrequirements(type);
                //setupEnable();
                btnRemove.Enabled = false;
                txtLast.Focus();
                

            }
        }

        public void setupEnable()
        {
            btnN1.Enabled = true;
            btnN2.Enabled = true;
            btnN3.Enabled = true;
            btnNFat.Enabled = true;
            btnNMot.Enabled = true;
            btnNGua.Enabled = true;
            txtLast.Enabled = true;
            txtFirst.Enabled = true;
            txtMid.Enabled = true;
            txtSchool.Enabled = true;
            txtAddress.Enabled = true;
            txtSec.Enabled = true;
            cmbGen.Enabled = true;
            cmbLev.Enabled = true;
            cmbDay.Enabled = true;
            cmbMonth.Enabled = true;
            cmbYears.Enabled = true;
            txtScon.Enabled = true;
            txtFat.Enabled = true;
            txtFatocc.Enabled = true;
            txtMot.Enabled = true;
            txtMotocc.Enabled = true;
            //txtGua.Enabled = true;
            txtGuaocc.Enabled = true;
            txtParGuaCon.Enabled = true;
            txtTalSki.Enabled = true;
            txtAward.Enabled = true;
            txtRelation.Enabled = true;
            btnAdd.Enabled = true;
            btnRemove.Enabled = true;
            pnlGuardian.Enabled = true;
        }

        public void setupDisable()
        {
            btnN1.Enabled = false;
            btnN2.Enabled = false;
            btnN3.Enabled = false;
            btnNFat.Enabled = false;
            btnNMot.Enabled = false;
            btnNGua.Enabled = false;
            txtLast.Enabled = false;
            txtFirst.Enabled = false;
            txtMid.Enabled = false;
            txtSchool.Enabled = false;
            txtAddress.Enabled = false;
            txtSec.Enabled = false;
            cmbGen.Enabled = false;
            cmbLev.Enabled = false;
            cmbDay.Enabled = false;
            cmbMonth.Enabled = false;
            cmbYears.Enabled = false;
            txtScon.Enabled = false;
            txtFat.Enabled = false;
            txtFatocc.Enabled = false;
            txtMot.Enabled = false;
            txtMotocc.Enabled = false;
            //txtGua.Enabled = false;
            txtGuaocc.Enabled = false;
            txtParGuaCon.Enabled = false;
            txtTalSki.Enabled = false;
            txtAward.Enabled = false;
            btnAdd.Enabled = false;
           // cmbReq.Enabled = false;
            btnRemove.Enabled = false;
            pnlGuardian.Enabled = false;
        }

        private void pnlOldForm_Paint(object sender, PaintEventArgs e)
        {

        }

        public void setupOldStudentList()
        {
            int oldyear = Convert.ToInt32(activeYr)-1;
            int currentyear = Convert.ToInt32(activeYr);
            string theoldSY = "SY:"+oldyear + "-" + currentyear;

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select studno,(concat(lname,' ',fname,' ',mname)) as 'Student' from stud_tbl where syregistered='" + theoldSY+ "'", con);//syregistered!='" + activeSY
            DataTable dt = new DataTable();
            da.Fill(dt);
            dvoldstud = new DataView(dt);
            dgvOS.DataSource = dvoldstud;
            con.Close();

            dgvOS.Columns[0].Width = 135;
            dgvOS.Columns[1].Width = 340;
        }

        private void txtSrc_TextChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "Student's name")
            {
                dvoldstud.RowFilter = string.Format("Student LIKE '%{0}%'", txtSrc.Text);
                dgvOS.DataSource = dvoldstud;
                toolTip2.SetToolTip(txtSrc,"search student's name");
            }
            else
            {
                dvoldstud.RowFilter = string.Format("studno LIKE '%{0}%'", txtSrc.Text);
                dgvOS.DataSource = dvoldstud;
                toolTip2.SetToolTip(txtSrc, "search student number");
            }

            if (dgvOS.Rows.Count > 0)
            {
                pnlNot.Visible = false;
            }
            if (dgvOS.Rows.Count == 0 && txtSrc.Text != "")
            {
                pnlNot.Visible = true;
                lblnoteos.Text = "0 search result";
            }
            if (dgvOS.Rows.Count == 0 && txtSrc.Text == "")
            {
                pnlNot.Visible = true;
                lblnoteos.Text = "no items found!";
            }
        }

        private void dgvOS_Click(object sender, EventArgs e)
        {
            if (dgvOS.Rows.Count <= 0)
            {
                return;
            }

            string studentno=dgvOS.SelectedRows[0].Cells[0].Value.ToString();
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='" + studentno + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();


            if (dt.Rows.Count > 0)
            {
                //int current = Convert.ToInt32(DateTime.Now.Year);
                int current = Convert.ToInt32(activeYr);
                int upcoming = Convert.ToInt32(activeYr)+1;
                string thesy = current + "-" + upcoming;

                btnProceed.Enabled = true;
                oldstudlevel = dt.Rows[0].ItemArray[4].ToString();
                txtTheSnum.Text = dt.Rows[0].ItemArray[0].ToString();
                txtSY.Text = thesy.ToString();
                txtLN.Text = dt.Rows[0].ItemArray[3].ToString();
                txtMN.Text = dt.Rows[0].ItemArray[2].ToString();
                txtFN.Text = dt.Rows[0].ItemArray[1].ToString();
                txtGen.Text = dt.Rows[0].ItemArray[10].ToString();
                txtAge.Text = dt.Rows[0].ItemArray[9].ToString();
                txtAdd.Text = dt.Rows[0].ItemArray[7].ToString();
                txtBD.Text = dt.Rows[0].ItemArray[8].ToString();
                txtGn.Text = dt.Rows[0].ItemArray[16].ToString();
                txtCon.Text = dt.Rows[0].ItemArray[18].ToString();
                lvwSG.Items.Clear();
                setupTheGrades(studentno);

            }
        }

        public void setupTheGrades(string selectedstud)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("select*from stud_tbl where studno='" + selectedstud + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            string oldstudgrade = "";


            if(dt.Rows.Count>0)
            {
                oldstudgrade = dt.Rows[0].ItemArray[4].ToString();
            }
            
            if (oldstudgrade == "Kinder")
            {

                con.Open();
                OdbcDataAdapter dak1 = new OdbcDataAdapter("select*from kindergrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk1 = new DataTable();
                dak1.Fill(dtk1);
                con.Close();
                if (dtk1.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk1.Rows.Count; b++)
                    {
                        ListViewItem it = new ListViewItem();
                        it.Text = dtk1.Rows[b].ItemArray[1].ToString();
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[2].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[3].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[4].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[5].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[6].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from kindergrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString()=="")
                    {
                        genave = 0.00;
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
                    it.BackColor = Color.FromArgb(216, 223, 234);
                    it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                    it.Text = "Average:";
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 1")
            {
                
                con.Open();
                OdbcDataAdapter dak1 = new OdbcDataAdapter("select*from gradeonegrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk1 = new DataTable();
                dak1.Fill(dtk1);
                con.Close();
                if (dtk1.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk1.Rows.Count; b++)
                    {
                        ListViewItem it = new ListViewItem();
                        it.Text = dtk1.Rows[b].ItemArray[1].ToString();
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[2].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[3].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[4].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[5].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[6].ToString());
                        it.SubItems.Add(dtk1.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeonegrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 2")
            {
                con.Open();
                OdbcDataAdapter dak2 = new OdbcDataAdapter("select*from gradetwogrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk2 = new DataTable();
                dak2.Fill(dtk2);
                con.Close();
                if (dtk2.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk2.Rows.Count; b++)
                    {
                        ListViewItem it = new ListViewItem();
                        it.Text = dtk2.Rows[b].ItemArray[1].ToString();
                        it.SubItems.Add(dtk2.Rows[b].ItemArray[2].ToString());
                        it.SubItems.Add(dtk2.Rows[b].ItemArray[3].ToString());
                        it.SubItems.Add(dtk2.Rows[b].ItemArray[4].ToString());
                        it.SubItems.Add(dtk2.Rows[b].ItemArray[5].ToString());
                        it.SubItems.Add(dtk2.Rows[b].ItemArray[6].ToString());
                        it.SubItems.Add(dtk2.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradetwogrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 3")
            {
                con.Open();
                OdbcDataAdapter dak1 = new OdbcDataAdapter("select*from gradethreegrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk1 = new DataTable();
                dak1.Fill(dtk1);
                con.Close();
                if (dtk1.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk1.Rows.Count; b++)
                    {
                        ListViewItem it3 = new ListViewItem();
                        it3.Text = dtk1.Rows[b].ItemArray[1].ToString();
                        it3.SubItems.Add(dtk1.Rows[b].ItemArray[2].ToString());
                        it3.SubItems.Add(dtk1.Rows[b].ItemArray[3].ToString());
                        it3.SubItems.Add(dtk1.Rows[b].ItemArray[4].ToString());
                        it3.SubItems.Add(dtk1.Rows[b].ItemArray[5].ToString());
                        it3.SubItems.Add(dtk1.Rows[b].ItemArray[6].ToString());
                        it3.SubItems.Add(dtk1.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it3);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradethreegrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 4")
            {
                con.Open();
                OdbcDataAdapter dak4 = new OdbcDataAdapter("select*from gradefourgrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk4 = new DataTable();
                dak4.Fill(dtk4);
                con.Close();
                if (dtk4.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk4.Rows.Count; b++)
                    {
                        ListViewItem it3 = new ListViewItem();
                        it3.Text = dtk4.Rows[b].ItemArray[1].ToString();
                        it3.SubItems.Add(dtk4.Rows[b].ItemArray[2].ToString());
                        it3.SubItems.Add(dtk4.Rows[b].ItemArray[3].ToString());
                        it3.SubItems.Add(dtk4.Rows[b].ItemArray[4].ToString());
                        it3.SubItems.Add(dtk4.Rows[b].ItemArray[5].ToString());
                        it3.SubItems.Add(dtk4.Rows[b].ItemArray[6].ToString());
                        it3.SubItems.Add(dtk4.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it3);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradefourgrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 5")
            {
               
                con.Open();
                OdbcDataAdapter dak4 = new OdbcDataAdapter("select*from gradefivegrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk4 = new DataTable();
                dak4.Fill(dtk4);
                con.Close();
                if (dtk4.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk4.Rows.Count; b++)
                    {
                        ListViewItem it5 = new ListViewItem();
                        it5.Text = dtk4.Rows[b].ItemArray[1].ToString();
                        it5.SubItems.Add(dtk4.Rows[b].ItemArray[2].ToString());
                        it5.SubItems.Add(dtk4.Rows[b].ItemArray[3].ToString());
                        it5.SubItems.Add(dtk4.Rows[b].ItemArray[4].ToString());
                        it5.SubItems.Add(dtk4.Rows[b].ItemArray[5].ToString());
                        it5.SubItems.Add(dtk4.Rows[b].ItemArray[6].ToString());
                        it5.SubItems.Add(dtk4.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it5);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradefivegrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 6")
            {
                con.Open();
                OdbcDataAdapter dak5 = new OdbcDataAdapter("select*from gradesixgrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk5 = new DataTable();
                dak5.Fill(dtk5);
                con.Close();
                if (dtk5.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk5.Rows.Count; b++)
                    {
                        ListViewItem it5 = new ListViewItem();
                        it5.Text = dtk5.Rows[b].ItemArray[1].ToString();
                        it5.SubItems.Add(dtk5.Rows[b].ItemArray[2].ToString());
                        it5.SubItems.Add(dtk5.Rows[b].ItemArray[3].ToString());
                        it5.SubItems.Add(dtk5.Rows[b].ItemArray[4].ToString());
                        it5.SubItems.Add(dtk5.Rows[b].ItemArray[5].ToString());
                        it5.SubItems.Add(dtk5.Rows[b].ItemArray[6].ToString());
                        it5.SubItems.Add(dtk5.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it5);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradesixgrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 7")
            {
                con.Open();
                OdbcDataAdapter dak7 = new OdbcDataAdapter("select*from gradesevengrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk7 = new DataTable();
                dak7.Fill(dtk7);
                con.Close();
                if (dtk7.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk7.Rows.Count; b++)
                    {
                        ListViewItem it5 = new ListViewItem();
                        it5.Text = dtk7.Rows[b].ItemArray[1].ToString();
                        it5.SubItems.Add(dtk7.Rows[b].ItemArray[2].ToString());
                        it5.SubItems.Add(dtk7.Rows[b].ItemArray[3].ToString());
                        it5.SubItems.Add(dtk7.Rows[b].ItemArray[4].ToString());
                        it5.SubItems.Add(dtk7.Rows[b].ItemArray[5].ToString());
                        it5.SubItems.Add(dtk7.Rows[b].ItemArray[6].ToString());
                        it5.SubItems.Add(dtk7.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it5);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradesevengrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 8")
            {
                con.Open();
                OdbcDataAdapter dak8 = new OdbcDataAdapter("select*from gradeeightgrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk8 = new DataTable();
                dak8.Fill(dtk8);
                con.Close();
                if (dtk8.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk8.Rows.Count; b++)
                    {
                        ListViewItem it5 = new ListViewItem();
                        it5.Text = dtk8.Rows[b].ItemArray[1].ToString();
                        it5.SubItems.Add(dtk8.Rows[b].ItemArray[2].ToString());
                        it5.SubItems.Add(dtk8.Rows[b].ItemArray[3].ToString());
                        it5.SubItems.Add(dtk8.Rows[b].ItemArray[4].ToString());
                        it5.SubItems.Add(dtk8.Rows[b].ItemArray[5].ToString());
                        it5.SubItems.Add(dtk8.Rows[b].ItemArray[6].ToString());
                        it5.SubItems.Add(dtk8.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it5);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeeightgrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 9")
            {
               
                con.Open();
                OdbcDataAdapter dak9 = new OdbcDataAdapter("select*from gradeninegrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk9 = new DataTable();
                dak9.Fill(dtk9);
                con.Close();
                if (dtk9.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk9.Rows.Count; b++)
                    {
                        ListViewItem it5 = new ListViewItem();
                        it5.Text = dtk9.Rows[b].ItemArray[1].ToString();
                        it5.SubItems.Add(dtk9.Rows[b].ItemArray[2].ToString());
                        it5.SubItems.Add(dtk9.Rows[b].ItemArray[3].ToString());
                        it5.SubItems.Add(dtk9.Rows[b].ItemArray[4].ToString());
                        it5.SubItems.Add(dtk9.Rows[b].ItemArray[5].ToString());
                        it5.SubItems.Add(dtk9.Rows[b].ItemArray[6].ToString());
                        it5.SubItems.Add(dtk9.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it5);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeninegrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    it.BackColor = Color.FromArgb(216, 223, 234);
                    it.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                    it.Text = "Average:";
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[0].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[1].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[2].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[3].ToString());
                    it.SubItems.Add(dtk11.Rows[0].ItemArray[4].ToString());
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (oldstudgrade == "Grade 10")
            {
                con.Open();
                OdbcDataAdapter dak9 = new OdbcDataAdapter("select*from gradetengrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk9 = new DataTable();
                dak9.Fill(dtk9);
                con.Close();
                if (dtk9.Rows.Count > 0)
                {
                    for (int b = 0; b < dtk9.Rows.Count; b++)
                    {
                        ListViewItem it10 = new ListViewItem();
                        it10.Text = dtk9.Rows[b].ItemArray[1].ToString();
                        it10.SubItems.Add(dtk9.Rows[b].ItemArray[2].ToString());
                        it10.SubItems.Add(dtk9.Rows[b].ItemArray[3].ToString());
                        it10.SubItems.Add(dtk9.Rows[b].ItemArray[4].ToString());
                        it10.SubItems.Add(dtk9.Rows[b].ItemArray[5].ToString());
                        it10.SubItems.Add(dtk9.Rows[b].ItemArray[6].ToString());
                        it10.SubItems.Add(dtk9.Rows[b].ItemArray[7].ToString());
                        lvwSG.Items.Add(it10);
                    }
                }

                con.Open();
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradetengrades_tbl where studno='" + selectedstud + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = Convert.ToDouble(dtk11.Rows[0].ItemArray[4].ToString());
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
                    oldstudGenAve = dtk11.Rows[0].ItemArray[4].ToString();
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            isthereenrolee = true;
            double genAve = 0;
           
            if (oldstudGenAve != "")
            {
                genAve = Convert.ToDouble(oldstudGenAve);
            }
            if (oldstudlevel == "Grade 10")
            {
                MessageBox.Show("Operation not allowed!"+"\nLast Grade level was reached.", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            con.Open();
            OdbcDataAdapter dac = new OdbcDataAdapter("select*from paymentcash_tbl where studno='" + txtTheSnum.Text+ "'", con);
            DataTable dtc = new DataTable();
            dac.Fill(dtc);
            con.Close();
            if (dtc.Rows.Count > 0)
            {
                if (dtc.Rows[0].ItemArray[4].ToString()=="")
                {
                    MessageBox.Show("You have remaining balance" + "\non the previous grade level!", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            con.Open();
            OdbcDataAdapter dai = new OdbcDataAdapter("select*from paymentmonthly_tbl where studno='" + txtTheSnum.Text + "'", con);
            DataTable dti = new DataTable();
            dai.Fill(dti);
            con.Close();
            if (dti.Rows.Count > 0)
            {
                double bal = Convert.ToDouble(dti.Rows[0].ItemArray[4].ToString());
                if (bal>0)
                {
                    MessageBox.Show("You have remaining balance" + "\non the previous grade level!", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }


            if (genAve < 75)
            {
                MessageBox.Show("Student not allowed to enroll." + "\nStudent is failed on the previous grade level.", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (chkHasClearance.Checked == true && chkClrd.Checked == false)
            {
                MessageBox.Show("Clearance not cleared!", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (lvwReq.Items.Count < 1)
            {
                MessageBox.Show("please submit requirements.", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
      
            /*if (lvwReq.Items.Count != 2)
            {
                MessageBox.Show("all requirements are required to pass.", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }*/
            
            /*if (lvwReq.Items.Count > 0 && cmbType.Text == "Old")
            {
                int itr = 1;
                for (int i = 0; i < lvwReq.Items.Count; i++)
                {
                    if ((lvwReq.Items[i].Text != "Clearance" && lvwReq.Items[i].Text != "clearance") && (itr == lvwReq.Items.Count))
                    {
                        MessageBox.Show("Clearance is required to pass.", "Admission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    itr++;
                }
            }*/

            pnlAdmit.Visible = false;
            lblHeader.Text = "Assessment";
            pnlRegistered.Location = new Point(297, 65);
            pnlOldForm.Visible = false;
            pnlRegistered.Visible = true;
            pnlAdmReq.Visible = false;
            
            lblstatusicon.Text = "a"; lblstatusicon.ForeColor = Color.White;
            lblstatus.Text = "Registration successful!"; lblstatus.ForeColor = Color.White;
            pnlnotif.BackColor = Color.ForestGreen;

            
            lvwPaySched.Clear(); pnldisnotify.Visible = true;
            lbldismemo.Text = "please select mode of payment.";
            lbldismemo.Location = new Point(88, 8);

            btnCanReg.Text = "Cancel admission";
            btnCanReg.Size = new Size(136, 33);
            btnCanReg.Location = new Point(160, 475);
            btnOK.Visible = true;
            btnOK.Location = new Point(303, 475);

            pnlMop.Visible = true;
            pnlMop.Location = new Point(12, 176);
            string newlevel = "";
            



            if (oldstudlevel == "Kinder")
            {
                newlevel = "Grade 1";
            }
            if (oldstudlevel == "Grade 1")
            {
                newlevel = "Grade 2";
            }
            if (oldstudlevel == "Grade 2")
            {
                newlevel = "Grade 3";
            }
            if (oldstudlevel == "Grade 3")
            {
                newlevel = "Grade 4";
            }
            if (oldstudlevel == "Grade 4")
            {
                newlevel = "Grade 5";
            }
            if (oldstudlevel == "Grade 5")
            {
                newlevel = "Grade 6";
            }
            if (oldstudlevel == "Grade 6")
            {
                newlevel = "Grade 7";
            }
            if (oldstudlevel == "Grade 7")
            {
                newlevel = "Grade 8";
            }
            if (oldstudlevel == "Grade 8")
            {
                newlevel = "Grade 9";
            }
            if (oldstudlevel == "Grade 9")
            {
                newlevel = "Grade 10";
            }
          

            setupTheReqcode(newlevel);
            string thefeelevel = "";

            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + newlevel + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                thefeelevel = dtdep.Rows[0].ItemArray[0].ToString();
            }

          
            setupAssessmentPerLevel(thefeelevel);
            txtSnum.Text = txtTheSnum.Text;
            txtenrolee.Text = txtLN.Text + ", " + txtFN.Text + " " + txtMN.Text;
            setupDiscountItems(newlevel);
            txtenroleegrd.Text = newlevel;
            oldstudenrolee = true;
        }

        private void txtAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            isviewAssessment_OtherDisc = false;
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from discount_tbl where discname='"+cmbDiscount.Text+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                txtLessAmt.Text = "P 0.00";
                txtDiscDesc.Text = dt.Rows[0].ItemArray[2].ToString();
                selecteddisc = cmbDiscount.Text;
            }

            cmbMop.Items.Clear();
            cmbMop.Items.Add("Cash");
            cmbMop.Items.Add("Installment");
         
            lblnotedone.Visible = false;
            lvwPaySched.Clear();
            lvwAssessment.Clear();
            txtRate.Clear();
            lblassnotif.Text = "please select discount and mode of payment.";
            lblassnotif.Location = new Point(44, 8);
            lbldismemo.Text = "please select mode of payment.";
            pnlassnotif.Visible = true;
            pnldisnotify.Visible = true;

            if ((chkWithDisc.Checked == true) && ((cmbDiscount.Text.Contains("siblings") == true)))
            {
                cmbSDVSrcType.Text = "Relative";
                cmbFilt.Text = "Student number";
                setupSiblingDiscVerification();
                if (isApproved == false)
                {
                    cmbMop.Enabled = false;
                }
                pnlSDVMain.Location = new Point(79, 109);
                pnlSDVMain.Visible = true;
            }
            else
            {
                cmbMop.Enabled = true;
            }

          

        }

        private void btnAss_Click(object sender, EventArgs e)
        {
            frmAssessment ass = new frmAssessment();
            this.Hide();
            ass.asslog = admlog;
            ass.Show();
        }

        private void chkWithDisc_CheckedChanged(object sender, EventArgs e)
        {
            cmbMop.Items.Clear();
            cmbMop.Items.Add("Cash");
            cmbMop.Items.Add("Installment");

            lblnotedone.Visible = false;
            lvwAssessment.Clear();
            lblassnotif.Location = new Point(44, 8);
            lblassnotif.Text = "please select discount and mode of payment.";
            pnlassnotif.Visible = true;
            lvwPaySched.Clear();
            lbldismemo.Location = new Point(88, 8);
            lbldismemo.Text = "please select mode of payment.";
            pnldisnotify.Visible = true;

            if (chkWithDisc.Checked == true)
            {
                cmbDiscount.Enabled = true;
             
                txtDiscDesc.Text = "...";
                txtRate.Text = "...";
                cmbMop.Items.Clear();
                cmbMop.Items.Add("Cash");
                cmbMop.Items.Add("Installment");
                cmbMop.Enabled = false;
                setupDiscountItems(txtenroleegrd.Text);
            }
            else
            {
                if (pnlSDVMain.Visible == false)
                {
                    pnlSame.Visible = false;
                    chkSameEnrolee.Checked = false;
                    chkSameStud.Checked = false;
                   
                    ClearSibDiscVerif();
                    lblApproved.Text = "";
                }

                cmbMop.Enabled = true;
                cmbDiscount.Enabled = false;
                txtLessAmt.Text = "P 0.00";
                txtRelation.Text = "";
                txtDiscDesc.Text = "...";
                txtRate.Text = "...";
                pnlassnotif.Visible = false;
                string thefeelevel = "";
               
                con.Open();
                OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtenroleegrd.Text + "'", con);
                DataTable dtdep = new DataTable();
                dadep.Fill(dtdep);
                con.Close();
                if (dtdep.Rows.Count > 0)
                {
                    thefeelevel = dtdep.Rows[0].ItemArray[0].ToString();
                }

               
                setupAssessmentPerLevel(thefeelevel);
                setupPaymentSchedule();
                

                setupDiscountItems(txtenroleegrd.Text);
            }
        }

        
        private void dgvm_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Hand;
        }

        private void dgvm_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Default;
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Admission")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Admission")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gainsboro;
            }
        }

        private void cmbYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        public void computeAge()
        {
            if (cmbMonth.Text != "" && cmbDay.Text != "" && cmbYears.Text != "")
            {
                int current = Convert.ToInt32(DateTime.Now.Year);
                int birth = Convert.ToInt32(cmbYears.Text);
                int age = current - birth;
                txtEAge.Text = age.ToString();
            }
        }

        private void cmbDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAge();
        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void txtParGuaCon_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvOS_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dgvOS.Rows.Count != 0)
            {
                pnlNot.Visible = false;
            }
            else
            {
                pnlNot.Visible = true;
            }
        }

        private void chkHasClearance_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHasClearance.Checked == true)
            {
                pnlClrd.Enabled = true;
            }
            else
            {
                pnlClrd.Enabled = false;
                chkClrd.Checked = false;
            }

        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSrc.Text = "";
            txtSrc.Focus();
        }

        private void frmAdmission_Shown(object sender, EventArgs e)
        {
           
        }

        private void btnCloseSDV_Click(object sender, EventArgs e)
        {
            pnlSDVMain.Hide();
            lblResult.Text = "result: 0";
            if (lblApproved.Text == "Discount cancelled.")
            {
                lblApproved.Text = "";
            }
            /*lblApproved.Visible = false;
            txtlst.Text = "";
            txtmnm.Text = "";
            txtfnm.Text = "";
            txtgdr.Text = "";
            txtagesi.Text = "";
            txtbdt.Text = "";
            txtfatnm.Text = "";
            txtmotnm.Text = "";
            txtgdnm.Text = "";
            txtsno.Text = "";
            txtSILevel.Text = "";
            btnApprove.Text = "Approve";*/
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (txtSILevel.Text == "Kinder") { SiblingLevel = 0; }
            if (txtSILevel.Text == "Grade 1") {SiblingLevel = 1; }
            if (txtSILevel.Text == "Grade 2") { SiblingLevel = 2; }
            if (txtSILevel.Text == "Grade 3") { SiblingLevel = 3; }
            if (txtSILevel.Text == "Grade 4") { SiblingLevel = 4; }
            if (txtSILevel.Text == "Grade 5") { SiblingLevel = 5; }
            if (txtSILevel.Text == "Grade 6") { SiblingLevel = 6; }
            if (txtSILevel.Text == "Grade 7") { SiblingLevel=7;}
            if (txtSILevel.Text == "Grade 8") { SiblingLevel = 8; }
            if (txtSILevel.Text == "Grade 9") { SiblingLevel = 9; }
            if (txtSILevel.Text == "Grade 10") { SiblingLevel = 10; }

            if (btnApprove.Text == "Approve")
            {
               
                if (MyLevel < SiblingLevel)
                {
                    isApproved = true;
                    lblApproved.Visible = true;
                    pnlSame.Visible = false;
                    lblApproved.ForeColor = Color.ForestGreen;
                    lblApproved.Text = "Enrolee is approved for sibling discount.";
                    cmbMop.Enabled = true;
                    btnApprove.Text = "Cancel";
                    siblingProvider = txtsno.Text;
                    siblingOldYoung = false;
                }
                else if (MyLevel == SiblingLevel)
                {
                    pnlSame.Visible = true;
                }
                else//the student enrolled will be given the discount
                {
                    isApproved = true;
                    sibDiscname = cmbDiscount.Text;
                    lblApproved.Visible = true;
                    lblApproved.ForeColor = Color.ForestGreen;
                    lblApproved.Text = txtlst.Text+", "+txtfnm.Text+" "+txtmnm.Text+" will grant"+"\nsibling discount after the settlement of full downpayment.";
                    cmbMop.Enabled = true;
                    btnApprove.Text = "Cancel";
                    siblingGrantee = txtsno.Text;
                    siblingProvider = txtSnum.Text;
                    chkWithDisc.Checked = false;
                    siblingOldYoung = true;
                    
                    

                    
                }
               // pnlSDVMain.Visible = false;
            }
            else if (btnApprove.Text == "Confirm")
            {
                if ((chkSameEnrolee.Checked == true && chkSameStud.Checked==true)||(chkSameEnrolee.Checked==false && chkSameStud.Checked==false))
                {
                    return;
                }


                if (chkSameEnrolee.Checked == true)
                {
                    chkSameStud.Checked = false;
                    isApproved = true;
                    lblApproved.Visible = true;
                    lblApproved.ForeColor = Color.ForestGreen;
                    lblApproved.Text = "Enrolee is approved for sibling discount.";
                    cmbMop.Enabled = true;
                    siblingProvider = txtsno.Text;
                    btnApprove.Text = "Cancel";
                    chkWithDisc.Checked = true;
                    siblingOldYoung = false;
                }
                if (chkSameStud.Checked == true)
                {
                    chkSameEnrolee.Checked = false;
                    isApproved = true;
                    sibDiscname = cmbDiscount.Text;
                    lblApproved.Visible = true;
                    lblApproved.ForeColor = Color.ForestGreen;
                    lblApproved.Text = txtlst.Text + ", " + txtfnm.Text + " " + txtmnm.Text + " will grant" + "\nsibling discount after the settlement of full downpayment.";
                    cmbMop.Enabled = true;
                    btnApprove.Text = "Cancel";
                    siblingGrantee = txtsno.Text;
                    siblingProvider = txtSnum.Text;
                    chkWithDisc.Checked = false;
                    siblingOldYoung = true;
                }
            }
            else
            {
                btnApprove.Enabled = false;
                isApproved = false;
                pnlSame.Visible = false;
                chkSameEnrolee.Checked = false;
                chkSameStud.Checked = false;
                if (MyLevel < SiblingLevel)
                {
                    //lblApproved.Visible = false;
                    lblApproved.ForeColor = Color.Firebrick;
                    lblApproved.Text = "Discount cancelled.";
                    cmbMop.Enabled = false;
                    siblingOldYoung = false;
                    sibDiscname = "";
                    siblingProvider = "";
                    siblingGrantee = "";
                    btnApprove.Text = "Approve";
                    ClearSibDiscVerif();
                }
                else if (MyLevel == SiblingLevel)
                {
                    //lblApproved.Visible = false;
                    lblApproved.ForeColor = Color.Firebrick;
                    lblApproved.Text = "Discount cancelled.";
                    cmbMop.Enabled = false;
                    siblingOldYoung = false;
                    sibDiscname = "";
                    siblingProvider = "";
                    siblingGrantee = "";
                    chkWithDisc.Checked = false;
                    btnApprove.Text = "Approve";
                    ClearSibDiscVerif();
                }
                else
                {
                    //lblApproved.Visible = false;
                    lblApproved.ForeColor = Color.Firebrick;
                    lblApproved.Text = "Discount cancelled.";
                    cmbMop.Enabled = false;
                    siblingOldYoung = false;
                    sibDiscname = "";
                    siblingProvider = "";
                    siblingGrantee = "";
                    btnApprove.Text = "Approve";
                    ClearSibDiscVerif();


                    /*
                    string department = "";
                    double monthlyAmt = 0;
                    string MOP = "";

                    con.Open();
                    OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtSILevel.Text + "'", con);
                    DataTable dtdep = new DataTable();
                    dadep.Fill(dtdep);
                    con.Close();
                    if (dtdep.Rows.Count > 0)
                    {
                        department = dtdep.Rows[0].ItemArray[0].ToString();
                    }

                    con.Open();
                    OdbcDataAdapter daMI = new OdbcDataAdapter("Select amount from fee_tbl where level='" + department + "' and fee='MONTHLY INSTALLMENT' and SY='" + activeSY + "'", con);
                    DataTable dtMI = new DataTable();
                    daMI.Fill(dtMI);
                    con.Close();
                    if (dtMI.Rows.Count > 0)
                    {
                        monthlyAmt = Convert.ToDouble(dtMI.Rows[0].ItemArray[0].ToString());
                    }

                    con.Open();
                    OdbcDataAdapter daRec = new OdbcDataAdapter("Select*from stud_tbl where studno='" + txtsno.Text + "'", con);
                    DataTable dtRec = new DataTable();
                    daRec.Fill(dtRec);
                    con.Close();
                    if (dtRec.Rows.Count > 0)
                    {
                        MOP = dtRec.Rows[0].ItemArray[22].ToString();
                    }

                    if (MOP == "Cash")
                    {
                        con.Open();
                        OdbcDataAdapter daC = new OdbcDataAdapter("Select*from paymentcash_tbl where studno='" + txtsno.Text + "'", con);
                        DataTable dtC = new DataTable();
                        daC.Fill(dtC);
                        con.Close();
                        if (dtC.Rows.Count > 0)
                        {
                            double netAmount = Convert.ToDouble(dtC.Rows[0].ItemArray[2].ToString());
                            double discAmt = netAmount + monthlyAmt;

                            con.Open();
                            string update = "Update paymentcash_tbl set amount='" + discAmt + "'where studno='" + txtsno.Text + "'";
                            OdbcCommand cmd = new OdbcCommand(update);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    else
                    {
                        con.Open();
                        OdbcDataAdapter daI = new OdbcDataAdapter("Select*from paymentmonthly_tbl where studno='" + txtsno.Text + "'", con);
                        DataTable dtI = new DataTable();
                        daI.Fill(dtI);
                        con.Close();
                        if (dtI.Rows.Count > 0)
                        {
                            double Balance = Convert.ToDouble(dtI.Rows[0].ItemArray[4].ToString());
                            double discAmt = Balance + monthlyAmt;

                            con.Open();
                            string update = "Update paymentmonthly_tbl set balance='" + discAmt + "'where studno='" + txtsno.Text + "'";
                            OdbcCommand cmd = new OdbcCommand(update);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }*/
                }
                txtSearch.Focus();
            }

        }

        private void pnlsdvHeader_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        public void setupSiblingDiscVerification()
        {
          
            if (cmbSDVSrcType.Text == "Relative")
            {
                lblRelSrc.Text = "Relative search for: " + txtenrolee.Text;
                con.Open();
                OdbcDataAdapter dar = new OdbcDataAdapter("Select studno as 'No',(select concat(fname,' ',mname,' ',lname))as 'Name' from stud_tbl where lname LIKE'" + enr_LN + "'OR mname LIKE'"+enr_MN+"'OR lname LIKE'"+enr_MN+"'OR mname LIKE'"+enr_LN+"' and syregistered='"+activeSY+"'", con);
                DataTable dts = new DataTable();
                dar.Fill(dts);
                con.Close();

                DataTable dtfilt = new DataTable();
                DataRow dr;
                dtfilt.Columns.Add("No");
                dtfilt.Columns.Add("Name");

                if (dts.Rows.Count > 0)
                {
                    con.Open();
                    for (int x = 0; x < dts.Rows.Count; x++)
                    {
                        
                        OdbcDataAdapter da5 = new OdbcDataAdapter("Select studno from studdiscounted_tbl where studno='" + dts.Rows[x].ItemArray[0].ToString() + "'", con);
                        DataTable dts5 = new DataTable();
                        da5.Fill(dts5);
                       
                        if (dts5.Rows.Count <= 0)
                        {
                            dr = dtfilt.NewRow();
                            dr[0] = dts.Rows[x].ItemArray[0].ToString();
                            dr[1] = dts.Rows[x].ItemArray[1].ToString();
                            dtfilt.Rows.Add(dr);
                        }
                    }
                    con.Close();

                    dvsdv = new DataView(dtfilt);
                    pnlnotesdv.Visible = false;
                    dgvSearch.DataSource = null;
                    dgvSearch.DataSource = dvsdv;
                    dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvSearch.Columns[0].Width = 120;
                    dgvSearch.Columns[1].Width = 245;
                }
                else
                {
                    dgvSearch.DataSource = null;
                    pnlnotesdv.Visible = true;
                    lblnotesdv.Text = "no items found...";

                }
            }
            else
            {
                lblRelSrc.Text = "Searching for relative of: " + txtenrolee.Text;
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select studno as 'No',(select concat(fname,' ',mname,' ',lname))as 'Name' from stud_tbl where syregistered='"+activeSY+"'", con);
                DataTable dts = new DataTable();
                da.Fill(dts);
                con.Close();
                
                DataTable dtfilt = new DataTable();
                DataRow dr;
                dtfilt.Columns.Add("No");
                dtfilt.Columns.Add("Name");

                if (dts.Rows.Count > 0)
                {
                    con.Open();
                    for (int x = 0; x < dts.Rows.Count; x++)
                    {
                        
                        OdbcDataAdapter da5 = new OdbcDataAdapter("Select studno from studdiscounted_tbl where studno='" + dts.Rows[x].ItemArray[0].ToString() + "'", con);
                        DataTable dts5 = new DataTable();
                        da5.Fill(dts5);
                        
                        if (dts5.Rows.Count <= 0)
                        {
                            dr = dtfilt.NewRow();
                            dr[0] = dts.Rows[x].ItemArray[0].ToString();
                            dr[1] = dts.Rows[x].ItemArray[1].ToString();
                            dtfilt.Rows.Add(dr);
                        }
                    }
                    con.Close();

                    dvsdv = new DataView(dtfilt);
                    pnlnotesdv.Visible = false;
                    dgvSearch.DataSource = null;
                    dgvSearch.DataSource = dvsdv;
                    dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvSearch.Columns[0].Width = 120;
                    dgvSearch.Columns[1].Width = 245;
                }
                else
                {
                    dgvSearch.DataSource = null;
                    pnlnotesdv.Visible = true;
                    lblnotesdv.Text = "no items found...";

                }
            } 
        }

        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dgvSearch.Rows.Count <=0)
            {
                lblResult.Text = dgvSearch.Rows.Count.ToString() + " result";
            }
            else
            {
                lblResult.Text = dgvSearch.Rows.Count.ToString() + " results";
            }
           
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }
            string sno = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            lblApproved.Text = "";


            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select lname,fname,mname,gender,birthdate,age,fathername,mothername,guardian,level from stud_tbl where studno='" + sno + "'", con);
            DataTable dts = new DataTable();
            da.Fill(dts);
          
            con.Close();

            if (dts.Rows.Count > 0)
            {
                txtsno.Text = sno;
                txtlst.Text = dts.Rows[0].ItemArray[0].ToString();
                txtfnm.Text = dts.Rows[0].ItemArray[1].ToString();
                txtmnm.Text = dts.Rows[0].ItemArray[2].ToString();
                txtgdr.Text = dts.Rows[0].ItemArray[3].ToString();
                txtbdt.Text = dts.Rows[0].ItemArray[4].ToString();
                txtagesi.Text = dts.Rows[0].ItemArray[5].ToString();
                txtfatnm.Text = dts.Rows[0].ItemArray[6].ToString();
                txtmotnm.Text = dts.Rows[0].ItemArray[7].ToString();
                txtgdnm.Text = dts.Rows[0].ItemArray[8].ToString();
                txtSILevel.Text = dts.Rows[0].ItemArray[9].ToString();
                btnApprove.Enabled = true;
            }
        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text!="")
            {
                lblnotesrc.Visible = false;
            }
            else
            {
                lblnotesrc.Visible = true;
            }


            if (cmbFilt.Text == "Student number")
            {
                dvsdv.RowFilter = string.Format("No LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvsdv;
            }
            if (cmbFilt.Text == "Student's name")
            {
                dvsdv.RowFilter = string.Format("Name LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvsdv;
            }


            if (dgvSearch.Rows.Count > 0)
            {
                pnlnotesdv.Visible = false;
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text != "")
            {
                pnlnotesdv.Visible = true;
                lblnotesdv.Text = "0 search result";
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text == "")
            {
                pnlnotesdv.Visible = true;
                lblnotesdv.Text = "no items found!";
            }
        }

        private void cmbFilt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilt.Text == "Student number")
            {
                toolTip2.SetToolTip(txtSearch, "student number");
            }
            if (cmbFilt.Text == "Student's name")
            {
                toolTip2.SetToolTip(txtSearch, "student's name");
            }

            txtSearch.Clear();
            txtSearch.Focus();
        }

        private void btnN1_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtLast.Text != "")
            {
                if (txtLast.TextLength == 1)
                {
                    string last = txtLast.Text.Substring(0, txtLast.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtLast.Text.Substring(0, txtLast.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtLast.Text;
                    }
                    
                }
                else
                {
                    string last = txtLast.Text.Substring(txtLast.TextLength-1,1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtLast.Text.Substring(0, txtLast.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtLast.Text.Substring(0, txtLast.TextLength);
                    }
                }
            }

            if (lstEnye == 1)
            {
                if (txtLast.Text != "")
                {
                    txtLast.Text = orgtext + "Ñ";
                    lstEnye += 1;
                }
                else
                {
                    txtLast.Text = "Ñ";
                    lstEnye += 1;
                }
            }
            else
            {
                if (txtLast.Text != "")
                {
                    txtLast.Text = orgtext + "ñ";
                    lstEnye -= 1;
                }
                else
                {
                    txtLast.Text = "ñ";
                    lstEnye -= 1;
                }
            }

            txtLast.Focus();
            txtLast.SelectionStart = txtLast.Text.Length;
        }

        private void btnN2_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtFirst.Text != "")
            {
                if (txtFirst.TextLength == 1)
                {
                    string last = txtFirst.Text.Substring(0, txtFirst.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFirst.Text.Substring(0, txtFirst.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFirst.Text;
                    }

                }
                else
                {
                    string last = txtFirst.Text.Substring(txtFirst.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFirst.Text.Substring(0, txtFirst.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFirst.Text.Substring(0, txtFirst.TextLength);
                    }
                }
            }

            if (fnmEnye == 1)
            {
                if (txtFirst.Text != "")
                {
                    txtFirst.Text = orgtext + "Ñ";
                    fnmEnye += 1;
                }
                else
                {
                    txtFirst.Text = "Ñ";
                    fnmEnye += 1;
                }
            }
            else
            {
                if (txtFirst.Text != "")
                {
                    txtFirst.Text = orgtext + "ñ";
                    fnmEnye -= 1;
                }
                else
                {
                    txtFirst.Text ="ñ";
                    fnmEnye -= 1;
                }
            }
            txtFirst.Focus();
            txtFirst.SelectionStart = txtFirst.Text.Length;
        }

        private void btnN3_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtMid.Text != "")
            {
                if (txtMid.TextLength == 1)
                {
                    string last = txtMid.Text.Substring(0, txtMid.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtMid.Text.Substring(0, txtMid.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtMid.Text;
                    }

                }
                else
                {
                    string last = txtMid.Text.Substring(txtMid.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtMid.Text.Substring(0, txtMid.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtMid.Text.Substring(0, txtMid.TextLength);
                    }
                }
            }

            if (mnmEnye == 1)
            {
                if (txtMid.Text != "")
                {
                    txtMid.Text = orgtext + "Ñ";
                    mnmEnye += 1;
                }
                else
                {
                    txtMid.Text = "Ñ";
                    mnmEnye += 1;
                }
            }
            else
            {
                if (txtMid.Text != "")
                {
                    txtMid.Text = orgtext + "ñ";
                    mnmEnye -= 1;
                }
                else
                {
                    txtMid.Text = "ñ";
                    mnmEnye -= 1;
                }
            }

            txtMid.Focus();
            txtMid.SelectionStart = txtMid.Text.Length;
        }

        private void cmbSDVSrcType_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            setupSiblingDiscVerification();
           
        }

       
        private void dgvm_Click(object sender, EventArgs e)
        {
            if (dgvm.Rows.Count < 0)
            {
                 return;
            }
        }

        private void frmAdmission_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isdone == false && isthereenrolee == true)
            {
                if (MessageBox.Show("Do you really want to cancel admission?", "Admission", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {

                    return;
                }
                else
                {
                    cancelAdmThruMenuFrm = true;
                    CancelRegistration();
                }
            }
            else
            {

                LOGOUT();
                frmEmpLogin home = new frmEmpLogin();
                this.Hide();
                home.Show();
            }
        }

        private void panel24_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvm_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //this means that enrolling transaction not been done.
            if (isdone == false && isthereenrolee == true)
            {
                if (MessageBox.Show("Do you really want to cancel admission?", "Admission", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {

                    return;
                }
                else
                {

                    cancelAdmThruMenuFrm = true;
                    CancelRegistration();
                }
            }

            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Activity")
            {
                if (emptype == "Cashier")
                {
                    frmCashierMain casmain = new frmCashierMain();
                    this.Hide();
                    casmain.emptype = emptype;
                    casmain.cashlog = admlog;
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
                if (emptype == "Registrar")
                {
                    frmRegistrarMain regmain = new frmRegistrarMain();
                    this.Hide();
                    regmain.emptype = emptype;
                    regmain.co = CO;
                    regmain.reglog = admlog;
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
                    pmf.co = CO;
                    pmf.thefac = TheFaculty;
                    pmf.prinlog = admlog;
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
                    empf.CO = CO;
                    empf.faclog = admlog;
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
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Payment")
            {
                frmPayment formPay = new frmPayment();
                this.Hide();
                formPay.emptype = emptype;
                formPay.paylog = admlog;
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
                formStudRec.asslog = admlog;
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
                formstdgrd.grdlog = admlog;
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
                stud.studlog = admlog;
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
                facf.CO = CO;
                facf.facinfolog = admlog;
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
                frmFacAdv.advlog = admlog;
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
                frmSec.seclog = admlog;
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
                rfac.replog = admlog;
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
                rsched.schedlog = admlog;
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
                about.ablog = admlog;
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

        private void tmrAssess_Tick(object sender, EventArgs e)
        {
            tick++;
            if (tick == 10)
            {

                pnldisnotify.Visible = false;
                tmrAssess.Enabled = false;
                lbldismemo.Location = new Point(88, 8);
                tick = 0;
                setupPaymentSchedule();
                if (chkWithDisc.Checked == true)
                {
                    pnlassnotif.Visible = false;
                    string thefeelevel = "";

                    con.Open();
                    OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + txtenroleegrd.Text + "'", con);
                    DataTable dtdep = new DataTable();
                    dadep.Fill(dtdep);
                    con.Close();
                    if (dtdep.Rows.Count > 0)
                    {
                        thefeelevel = dtdep.Rows[0].ItemArray[0].ToString();
                    }

                    if (isviewAssessment_OtherDisc==false)
                    {
                        setupAssessmentPerLevel(thefeelevel);
                        isviewAssessment_OtherDisc = true;
                    }
                }
            }
            else
            {

                pnldisnotify.Visible = true;
                lbldismemo.Text = "please wait...";
                lbldismemo.Location = new Point(149, 8);
                if (chkWithDisc.Checked == true)
                {

                    pnlassnotif.Visible = true;
                    lblassnotif.Text = "please wait...";
                    lblassnotif.Location = new Point(149, 8);
                    if (isviewAssessment_OtherDisc == false)
                    {
                        lvwAssessment.Clear();
                    }
                }
            }
        }

        private void cmbLev_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLev.Text.Contains("Kinder") == true)
            {
                lblSchool.Text = "Last school attended";
            }
            else
            {
                lblSchool.Text = "*Last school attended";
            }
        }

        private void btnCancelOthers_Click(object sender, EventArgs e)
        {
            chkOthers.Checked = false;
            pnlGuardian.Visible = true;
            pnlGuardian.Location = new Point(1,4);
            txtGua.Location = new Point(-300, 3);
            btnCancelOthers.Visible = false;
            btnNGua.Visible = false;
        }

        private void chkOthers_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOthers.Checked == true)
            {
                chkMotG.Checked = false; chkFatG.Checked = false;
                txtGua.Text= "";
                pnlGuardian.Visible = false;
                btnNGua.Visible = true;
                txtGua.Location = new Point(0, 3);
                pnlGuardian.Location = new Point(-300, 4);
                btnCancelOthers.Visible = true ;
                btnCancelOthers.Enabled = true;
                txtGua.Enabled = true;
                txtGua.Focus();
            }
        }

        private void chkMotG_CheckedChanged(object sender, EventArgs e)
        {
            chkFatG.Checked = false;
            chkOthers.Checked = false;
            txtGua.Text = txtMot.Text;
        }

        private void chkFatG_CheckedChanged(object sender, EventArgs e)
        {
            chkMotG.Checked = false;
            chkOthers.Checked = false;
            txtGua.Text = txtFat.Text;

        }

        private void btnNFat_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtFat.Text != "")
            {
                if (txtFat.TextLength == 1)
                {
                    string last = txtFat.Text.Substring(0, txtFat.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFat.Text.Substring(0, txtFat.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFat.Text;
                    }

                }
                else
                {
                    string last = txtFat.Text.Substring(txtFat.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtFat.Text.Substring(0, txtFat.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtFat.Text.Substring(0, txtFat.TextLength);
                    }
                }
            }

            if (fatenye == 1)
            {
                if (txtFat.Text != "")
                {
                    txtFat.Text = orgtext + "Ñ";
                    fatenye += 1;
                }
                else
                {
                    txtFat.Text = "Ñ";
                    fatenye += 1;
                }
            }
            else
            {
                if (txtFat.Text != "")
                {
                    txtFat.Text = orgtext + "ñ";
                    fatenye -= 1;
                }
                else
                {
                    txtFat.Text = "ñ";
                    fatenye -= 1;
                }
            }

            txtFat.Focus();
            txtFat.SelectionStart = txtFat.Text.Length;
        }

        private void btnNMot_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtMot.Text != "")
            {
                if (txtMot.TextLength == 1)
                {
                    string last = txtMot.Text.Substring(0, txtMot.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtMot.Text.Substring(0, txtMot.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtMot.Text;
                    }

                }
                else
                {
                    string last = txtMot.Text.Substring(txtMot.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtMot.Text.Substring(0, txtMot.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtMot.Text.Substring(0, txtMot.TextLength);
                    }
                }
            }

            if (motenye == 1)
            {
                if (txtMot.Text != "")
                {
                    txtMot.Text = orgtext + "Ñ";
                    motenye += 1;
                }
                else
                {
                    txtMot.Text = "Ñ";
                    motenye += 1;
                }
            }
            else
            {
                if (txtMot.Text != "")
                {
                    txtMot.Text = orgtext + "ñ";
                    motenye -= 1;
                }
                else
                {
                    txtMot.Text = "ñ";
                    motenye -= 1;
                }
            }

            txtMot.Focus();
            txtMot.SelectionStart = txtMot.Text.Length;
        }

        private void btnNGua_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtGua.Text != "")
            {
                if (txtGua.TextLength == 1)
                {
                    string last = txtGua.Text.Substring(0, txtGua.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtGua.Text.Substring(0, txtGua.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtGua.Text;
                    }

                }
                else
                {
                    string last = txtGua.Text.Substring(txtGua.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtGua.Text.Substring(0, txtGua.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtGua.Text.Substring(0, txtGua.TextLength);
                    }
                }
            }

            if (guaenye == 1)
            {
                if (txtGua.Text != "")
                {
                    txtGua.Text = orgtext + "Ñ";
                    guaenye += 1;
                }
                else
                {
                    txtGua.Text = "Ñ";
                    guaenye += 1;
                }
            }
            else
            {
                if (txtGua.Text != "")
                {
                    txtGua.Text = orgtext + "ñ";
                    guaenye -= 1;
                }
                else
                {
                    txtGua.Text = "ñ";
                    guaenye -= 1;
                }
            }

            txtGua.Focus();
            txtGua.SelectionStart = txtGua.Text.Length;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSchool.Text = "First City Providential College";
            txtAddress.Text = "Francisco homes lot 34 San Jose Del Monte Bulacan";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtSchool.Text = "St. Dominic Savio College";
            txtAddress.Text = "Brgy. Tungkong mangga San Jose Del monte Bulacan";
        }

        private void panel15_Click(object sender, EventArgs e)
        {

        }

        private void chkSameEnrolee_CheckedChanged(object sender, EventArgs e)
        {
            btnApprove.Text = "Confirm";
        }

        private void chkSameStud_CheckedChanged(object sender, EventArgs e)
        {
            btnApprove.Text = "Confirm";
        }

       
       }
}
