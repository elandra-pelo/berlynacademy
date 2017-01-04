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
    public partial class frmStdGrd : Form
    {
       
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string grdlog, theFacultyName, selectedstud, snum, emptype, CO, accesscode, VISITED, notifstat;
        public DataView dvs;
        public bool isVisited, viewNotifDue, viewNotifDisc, viewNotifLate;
        public frmStdGrd()
        {
            InitializeComponent();
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

       
        private void frmStdGrd_Load(object sender, EventArgs e)
        {

            //this.BackColor = Color.FromArgb(0, 0, 25);
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            lvwDisplay.BackColor = Color.FromArgb(216, 223, 234);
            lblLogger.Text = grdlog;
            lblLoggerPosition.Text = emptype;
            //pnlsub.Visible = false;
            //btnHome.Text = "          " + grdlog;
            //MessageBox.Show(theFacultyName);
            lvwDisplay.Columns.Add("Quarter 1", 120, HorizontalAlignment.Center);
            lvwDisplay.Columns.Add("Quarter 2", 120, HorizontalAlignment.Center);
            lvwDisplay.Columns.Add("Quarter 3", 120, HorizontalAlignment.Center);
            lvwDisplay.Columns.Add("Quarter 4", 120, HorizontalAlignment.Center);
            lvwDisplay.Columns.Add("Average", 120, HorizontalAlignment.Center);
            lvwDisplay.Columns.Add("Remarks", 165, HorizontalAlignment.Center);

            lvwSG.Columns.Add("Subject", 120, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 1", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 2", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 3", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Quarter 4", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Average", 110, HorizontalAlignment.Center);
            lvwSG.Columns.Add("Remarks", 110, HorizontalAlignment.Center);

            if (isVisited == false)
            {
                if (VISITED.Contains("Student grades") == false)
                {
                    VISITED += "   Student grades";
                    isVisited = true;
                }
            }

            setupgrade();
            setupsec();
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

            int getSGindex = 1;
            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                getSGindex++;
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                getSGindex++;
                dtMenu.Rows.Add("  " + dt2.Rows[0].ItemArray[1].ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                getSGindex++;
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
            dgvm.Rows[getSGindex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        public void setupsec()
        {
            cmbSec.Items.Clear();

            if (cmbGrade.Text == "Kinder")
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Kinder" + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Kinder" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }


            if (cmbGrade.Text == "Grade 1")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 1" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select distinct section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 1" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Clear();
                            for (int g = 0; g < dtt.Rows.Count; g++)
                            {
                                cmbSec.Items.Add(dtt.Rows[g].ItemArray[0].ToString());
                            }
                        }
                    }
                }
            }


            if (cmbGrade.Text == "Grade 2")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 2" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 2" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }



            if (cmbGrade.Text == "Grade 3")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 3" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 3" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }




            if (cmbGrade.Text == "Grade 4")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 4" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 4" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }



            if (cmbGrade.Text == "Grade 5")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 5" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 5" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }




            if (cmbGrade.Text == "Grade 6")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 6" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 6" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }


            if (cmbGrade.Text == "Grade 7")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 7" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 7" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }


            if (cmbGrade.Text == "Grade 8")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 8" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 8" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }




            if (cmbGrade.Text == "Grade 9")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 9" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 9" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }


            if (cmbGrade.Text == "Grade 10")
            {
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(section) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 10" + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                    if (val > 0)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select section from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 10" + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            cmbSec.Items.Add(dtt.Rows[0].ItemArray[0].ToString());
                        }
                    }
                }
            }
        }

        public void setupgrade()
        {
            cmbGrade.Items.Clear();

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='"+"Kinder"+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 1" + "'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            OdbcDataAdapter da2 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 2" + "'", con);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            OdbcDataAdapter da3 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 3" + "'", con);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);

            OdbcDataAdapter da4 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 4" + "'", con);
            DataTable dt4 = new DataTable();
            da4.Fill(dt4);

            OdbcDataAdapter da5 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 5" + "'", con);
            DataTable dt5 = new DataTable();
            da5.Fill(dt5);

            OdbcDataAdapter da6 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 6" + "'", con);
            DataTable dt6 = new DataTable();
            da6.Fill(dt6);

            OdbcDataAdapter da7 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 7" + "'", con);
            DataTable dt7 = new DataTable();
            da7.Fill(dt7);

            OdbcDataAdapter da8 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 8" + "'", con);
            DataTable dt8 = new DataTable();
            da8.Fill(dt8);

            OdbcDataAdapter da9 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 9" + "'", con);
            DataTable dt9 = new DataTable();
            da9.Fill(dt9);

            OdbcDataAdapter da0 = new OdbcDataAdapter("Select count(level) from facultysched_tbl where faculty='" + theFacultyName + "'and level='" + "Grade 10" + "'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);

            con.Close();

            if (dt.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Kinder"); }
                
            }
            if (dt1.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 1"); }

            }
            if (dt2.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt2.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 2"); }

            }
            if (dt3.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt3.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 3"); }

            }
            if (dt4.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt4.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 4"); }

            }
            if (dt5.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt5.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 5"); }

            }
            if (dt6.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt6.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 6"); }

            }
            if (dt7.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt7.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 7"); }

            }
            if (dt8.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt8.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 8"); }

            }
            if (dt9.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt9.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 9"); }

            }
            if (dt0.Rows.Count > 0)
            {
                int val = Convert.ToInt32(dt0.Rows[0].ItemArray[0].ToString());
                if (val > 0)
                { cmbGrade.Items.Add("Grade 10"); }

            }
                    
        }

        public void setupfacultylog_Subjects()
        {
    
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select subject from facultysched_tbl where level='" + cmbGrade.Text + "'and section='" + cmbSec.Text + "'and type='"+"Academic"+"'and faculty='" + theFacultyName + "'order by subject ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

           
            if (dt.Rows.Count > 0)
            {
                cmbSubject.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSubject.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }

            }
            else
            {
                cmbSubject.Items.Clear();
            }
        }

        public void setupStudentsInSec()
        {
            string activeSY = "";
            con.Open();
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);

            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString(); }

            OdbcDataAdapter da = new OdbcDataAdapter("Select (select concat(lname,' ',fname,' ',mname))as 'Student' from stud_tbl where level='" + cmbGrade.Text + "'and section='" + cmbSec.Text + "'and status='"+"Active"+"'order by Student ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dvs = new DataView(dt);
            con.Close();
           
            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                lblnote.Location = new Point(18, 6);
                dgvDisplay.DataSource = dvs;
                
                dgvDisplay.Columns[0].Width = 260;

            }
            else
            {
                pnlnotify.Visible = true;
                lblnote.Location = new Point(78, 6);
                dgvDisplay.DataSource = null;
                lblnote.Text = "no items found...";
                lblSubject.Text = "Subject";
                lblStudent.Text = "Student";
                lvwDisplay.Items.Clear();
                lvwSG.Items.Clear();
                txtQ1.Clear();
                txtQ2.Clear();
                txtQ3.Clear();
                txtQ4.Clear();

            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmEmpAbout eabout = new frmEmpAbout();
            this.Hide();
            eabout.ablog = grdlog;
            eabout.emptype = "faculty";
            eabout.theFaculty = theFacultyName;
            eabout.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void frmStdGrd_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmEmpMain emain = new frmEmpMain();
            this.Hide();
            emain.faclog = grdlog;
            emain.TheFacultyName = theFacultyName;
            emain.Show();

        }

        private void btnAdm_Click(object sender, EventArgs e)
        {
            frmAdmission formadm = new frmAdmission();
            this.Hide();
            formadm.admlog = grdlog;
            formadm.TheFaculty = theFacultyName;
            formadm.Show();
        }

        private void btnSI_Click(object sender, EventArgs e)
        {
            frmStudInfo FSI = new frmStudInfo();
            this.Hide();
            FSI.studlog = grdlog;
            FSI.emptype = "faculty";
            FSI.TheFaculty = theFacultyName;
            FSI.Show();
        }

        private void btnFI_Click(object sender, EventArgs e)
        {
            frmFacInfo FFI = new frmFacInfo();
            this.Hide();
            FFI.facinfolog = grdlog;
            FFI.emptype = "faculty";
            FFI.TheFaculty = theFacultyName;
            FFI.Show();
        }

        private void btnrepFac_Click(object sender, EventArgs e)
        {
            frmReport repf = new frmReport();
            this.Hide();
            repf.replog = grdlog;
            repf.emptype = "faculty";
            repf.theFaculty = theFacultyName;
            repf.Show();
        }

        private void txtQ1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtQ2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtQ3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtQ4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void cmbSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            selectedstud = "";
            if (cmbGrade.Text != "" && cmbSec.Text != "")
            {
                //pnlsub.Visible = false;
                //lblSubject.Text = "";
                lblSubject.Text = "Subject";
                lvwDisplay.Cursor = Cursors.Default;
                setupfacultylog_Subjects();
                setupStudentsInSec();
            }
        }

        private void cmbGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGrade.Text != "")
            {
                
                //pnlsub.Visible = false;
                //lblSubject.Text = "";
                selectedstud = "";
                lvwDisplay.Cursor = Cursors.Default;
                setupfacultylog_Subjects();
                //setupStudentsInSec();
                dgvDisplay.DataSource = null;
                setupsec();
                pnlnotify.Visible = false;
            }
        }

        public void getsnum()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("select studno from stud_tbl where (select concat(lname,' ',fname,' ',mname))='" + lblStudent.Text + "'and level='" + cmbGrade.Text + "'and section='" + cmbSec.Text + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                snum = dt.Rows[0].ItemArray[0].ToString();
            }
        }

        public void refreshDisplay()
        {
            lvwDisplay.Items.Clear();
            lvwSG.Items.Clear();

            string studentnumber = "";
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("select studno from stud_tbl where (select concat(lname,' ',fname,' ',mname))='" + lblStudent.Text + "'and level='" + cmbGrade.Text + "'and section='" + cmbSec.Text + "'and status='"+"Active"+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                studentnumber = dt.Rows[0].ItemArray[0].ToString();
            }

            if (cmbGrade.Text == "Kinder")
            {
                con.Open();
                OdbcDataAdapter dak = new OdbcDataAdapter("select*from kindergrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dtk = new DataTable();
                dak.Fill(dtk);
                con.Close();
                if (dtk.Rows.Count > 0)
                {
                    ListViewItem itm = new ListViewItem();
                    itm.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm.BackColor =Color.FromArgb(216,223,234);
                    itm.Text = dtk.Rows[0].ItemArray[2].ToString();
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[3].ToString());
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[4].ToString());
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[5].ToString());
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[6].ToString());
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm);
                }

                con.Open();
                OdbcDataAdapter dak1 = new OdbcDataAdapter("select*from kindergrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from kindergrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                    
                }
                
            }
            if (cmbGrade.Text == "Grade 1")
            {
                
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("select*from gradeonegrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {
                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }

                    con.Open();
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("select*from gradeonegrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeonegrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {

                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
            if (cmbGrade.Text == "Grade 2")
            {
                con.Open();
                OdbcDataAdapter da2 = new OdbcDataAdapter("select*from gradetwogrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                con.Close();
                if (dt2.Rows.Count > 0)
                {
                    ListViewItem itm2 = new ListViewItem();
                    itm2.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm2.BackColor = Color.FromArgb(216, 223, 234);
                    itm2.Text = dt2.Rows[0].ItemArray[2].ToString();
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[3].ToString());
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[4].ToString());
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[5].ToString());
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[6].ToString());
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm2);
                }

                con.Open();
                OdbcDataAdapter dak2 = new OdbcDataAdapter("select*from gradetwogrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradetwogrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
                
            }
            if (cmbGrade.Text == "Grade 3")
            {
                con.Open();
                OdbcDataAdapter da3 = new OdbcDataAdapter("select*from gradethreegrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);
                con.Close();
                if (dt3.Rows.Count > 0)
                {
                    ListViewItem itm3 = new ListViewItem();
                    itm3.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm3.BackColor = Color.FromArgb(216, 223, 234);
                    itm3.Text = dt3.Rows[0].ItemArray[2].ToString();
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[3].ToString());
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[4].ToString());
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[5].ToString());
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[6].ToString());
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm3);
                }

                con.Open();
                OdbcDataAdapter dak1 = new OdbcDataAdapter("select*from gradethreegrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradethreegrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
                
            }
            if (cmbGrade.Text == "Grade 4")
            {
                con.Open();
                OdbcDataAdapter da4 = new OdbcDataAdapter("select*from gradefourgrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                DataTable dt4 = new DataTable();
                da4.Fill(dt4);
                con.Close();
                if (dt4.Rows.Count > 0)
                {
                    ListViewItem itm4 = new ListViewItem();
                    itm4.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm4.BackColor = Color.FromArgb(216, 223, 234);
                    itm4.Text = dt4.Rows[0].ItemArray[2].ToString();
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[3].ToString());
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[4].ToString());
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[5].ToString());
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[6].ToString());
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm4);
                }

                con.Open();
                OdbcDataAdapter dak4 = new OdbcDataAdapter("select*from gradefourgrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradefourgrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
                
            }
            if (cmbGrade.Text == "Grade 5")
            {
                con.Open();
                OdbcDataAdapter da5 = new OdbcDataAdapter("select*from gradefivegrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                DataTable dt5 = new DataTable();
                da5.Fill(dt5);
                con.Close();
                if (dt5.Rows.Count > 0)
                {
                    ListViewItem itm5 = new ListViewItem();
                    itm5.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm5.BackColor = Color.FromArgb(216, 223, 234);
                    itm5.Text = dt5.Rows[0].ItemArray[2].ToString();
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[3].ToString());
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[4].ToString());
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[5].ToString());
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[6].ToString());
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm5);
                }

                con.Open();
                OdbcDataAdapter dak4 = new OdbcDataAdapter("select*from gradefivegrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradefivegrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
                
            }
            if (cmbGrade.Text == "Grade 6")
            {
                con.Open();
                OdbcDataAdapter da6 = new OdbcDataAdapter("select*from gradesixgrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                DataTable dt6 = new DataTable();
                da6.Fill(dt6);
                con.Close();
                if (dt6.Rows.Count > 0)
                {
                    ListViewItem itm6 = new ListViewItem();
                    itm6.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm6.BackColor = Color.FromArgb(216, 223, 234);
                    itm6.Text = dt6.Rows[0].ItemArray[2].ToString();
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[3].ToString());
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[4].ToString());
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[5].ToString());
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[6].ToString());
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm6);
                }

                con.Open();
                OdbcDataAdapter dak5 = new OdbcDataAdapter("select*from gradesixgrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradesixgrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
                
            }
            if (cmbGrade.Text == "Grade 7")
            {
                con.Open();
                OdbcDataAdapter da7 = new OdbcDataAdapter("select*from gradesevengrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                DataTable dt7 = new DataTable();
                da7.Fill(dt7);
                con.Close();
                if (dt7.Rows.Count > 0)
                {
                    ListViewItem itm7 = new ListViewItem();
                    itm7.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm7.BackColor = Color.FromArgb(216, 223, 234);
                    itm7.Text = dt7.Rows[0].ItemArray[2].ToString();
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[3].ToString());
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[4].ToString());
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[5].ToString());
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[6].ToString());
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm7);
                }

                con.Open();
                OdbcDataAdapter dak7 = new OdbcDataAdapter("select*from gradesevengrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradesevengrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
                
            }
            if (cmbGrade.Text == "Grade 8")
            {
                con.Open();
                OdbcDataAdapter da8 = new OdbcDataAdapter("select*from gradeeightgrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                DataTable dt8 = new DataTable();
                da8.Fill(dt8);
                con.Close();
                if (dt8.Rows.Count > 0)
                {
                    ListViewItem itm8 = new ListViewItem();
                    itm8.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm8.BackColor = Color.FromArgb(216, 223, 234);
                    itm8.Text = dt8.Rows[0].ItemArray[2].ToString();
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[3].ToString());
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[4].ToString());
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[5].ToString());
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[6].ToString());
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm8);
                }

                con.Open();
                OdbcDataAdapter dak8 = new OdbcDataAdapter("select*from gradeeightgrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeeightgrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
                
            }
            if (cmbGrade.Text == "Grade 9")
            {
                con.Open();
                OdbcDataAdapter da9 = new OdbcDataAdapter("select*from gradeninegrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                DataTable dt9 = new DataTable();
                da9.Fill(dt9);
                con.Close();
                if (dt9.Rows.Count > 0)
                {
                    ListViewItem itm9 = new ListViewItem();
                    itm9.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm9.BackColor = Color.FromArgb(216, 223, 234);
                    itm9.Text = dt9.Rows[0].ItemArray[2].ToString();
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[3].ToString());
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[4].ToString());
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[5].ToString());
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[6].ToString());
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm9);
                }

                con.Open();
                OdbcDataAdapter dak9 = new OdbcDataAdapter("select*from gradeninegrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeninegrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
                
            }
            if (cmbGrade.Text == "Grade 10")
            {
                con.Open();
                OdbcDataAdapter da10 = new OdbcDataAdapter("select*from gradetengrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                DataTable dt10 = new DataTable();
                da10.Fill(dt10);
                con.Close();
                if (dt10.Rows.Count > 0)
                {
                    ListViewItem itm10 = new ListViewItem();
                    itm10.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm10.BackColor = Color.FromArgb(216, 223, 234);
                    itm10.Text = dt10.Rows[0].ItemArray[2].ToString();
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[3].ToString());
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[4].ToString());
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[5].ToString());
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[6].ToString());
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm10);
                }

                con.Open();
                OdbcDataAdapter dak9 = new OdbcDataAdapter("select*from gradetengrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradetengrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
                
            }

            MessageBox.Show("Grades successfully saved.", "Student grades", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void dgvDisplay_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.Rows.Count <= 0)
            {
                return;
            }

            txtQ1.Clear(); txtQ2.Clear(); txtQ3.Clear(); txtQ4.Clear();
            btnUpdate.Text = "Update";
            btnUpdate.Enabled = false;
            lvwDisplay.Cursor = Cursors.Hand;
            string studentnumber="";
            selectedstud = dgvDisplay.SelectedRows[0].Cells[0].Value.ToString();
            lblStudent.Text = selectedstud;
            lvwDisplay.Items.Clear();
            lvwSG.Items.Clear();

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("select studno,gender from stud_tbl where (select concat(lname,' ',fname,' ',mname))='"+selectedstud+"'and level='"+cmbGrade.Text+"'and section='"+cmbSec.Text+"'",con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if(dt.Rows.Count>0)
            {
                studentnumber = dt.Rows[0].ItemArray[0].ToString();
                if (dt.Rows[0].ItemArray[1].ToString() == "Male")
                {
                    pnlStudIcon.BackgroundImage = Properties.Resources.male;
                }
                else
                {
                    pnlStudIcon.BackgroundImage = Properties.Resources.female3;
                }
            }

            if (cmbGrade.Text == "Kinder")
            {
                con.Open();
                OdbcDataAdapter dak = new OdbcDataAdapter("select*from kindergrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dtk = new DataTable();
                dak.Fill(dtk);
                con.Close();
                if (dtk.Rows.Count > 0)
                {
                    ListViewItem itm = new ListViewItem();
                    itm.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm.BackColor = Color.FromArgb(216, 223, 234);
                    itm.Text = dtk.Rows[0].ItemArray[2].ToString();
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[3].ToString());
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[4].ToString());
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[5].ToString());
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[6].ToString());
                    itm.SubItems.Add(dtk.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm);
                }

                con.Open();
                OdbcDataAdapter dak1 = new OdbcDataAdapter("select*from kindergrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from kindergrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                    it.SubItems.Add(rem);
                    lvwSG.Items.Add(it);
                }
            }
            if (cmbGrade.Text == "Grade 1")
            {
               
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("select*from gradeonegrades_tbl where studno='" + studentnumber + "'", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {

                        con.Open();
                        OdbcDataAdapter daa1 = new OdbcDataAdapter("select*from gradeonegrades_tbl where studno='" + studentnumber + "'and subdesc='"+cmbSubject.Text+"'", con);
                        DataTable dtt1 = new DataTable();
                        daa1.Fill(dtt1);
                        con.Close();

                        if (dtt1.Rows.Count > 0)
                        {
                            ListViewItem itm1 = new ListViewItem();
                            itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                            itm1.BackColor = Color.FromArgb(216, 223, 234);
                            itm1.Text = dtt1.Rows[0].ItemArray[2].ToString();
                            itm1.SubItems.Add(dtt1.Rows[0].ItemArray[3].ToString());
                            itm1.SubItems.Add(dtt1.Rows[0].ItemArray[4].ToString());
                            itm1.SubItems.Add(dtt1.Rows[0].ItemArray[5].ToString());
                            itm1.SubItems.Add(dtt1.Rows[0].ItemArray[6].ToString());
                            itm1.SubItems.Add(dtt1.Rows[0].ItemArray[7].ToString());
                            lvwDisplay.Items.Add(itm1);
                        }

                        con.Open();
                        OdbcDataAdapter dak1 = new OdbcDataAdapter("select*from gradeonegrades_tbl where studno='" + studentnumber + "'order by subdesc ASC", con);
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
                        OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeonegrades_tbl where studno='" + studentnumber + "'", con);
                        DataTable dtk11 = new DataTable();
                        dak11.Fill(dtk11);
                        con.Close();
                        if (dtk11.Rows.Count > 0)
                        {
                            double genave = 0.00;
                            if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                        } 
                    }      
            }
            if (cmbGrade.Text == "Grade 2")
            {
                con.Open();
                OdbcDataAdapter da2 = new OdbcDataAdapter("select*from gradetwogrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                con.Close();
                if (dt2.Rows.Count > 0)
                {
                    ListViewItem itm2 = new ListViewItem();
                    itm2.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm2.BackColor = Color.FromArgb(216, 223, 234);
                    itm2.Text = dt2.Rows[0].ItemArray[2].ToString();
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[3].ToString());
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[4].ToString());
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[5].ToString());
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[6].ToString());
                    itm2.SubItems.Add(dt2.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm2);
                }

                con.Open();
                OdbcDataAdapter dak2 = new OdbcDataAdapter("select*from gradetwogrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradetwogrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
            if (cmbGrade.Text == "Grade 3")
            {
                con.Open();
                OdbcDataAdapter da3 = new OdbcDataAdapter("select*from gradethreegrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);
                con.Close();
                if (dt3.Rows.Count > 0)
                {
                    ListViewItem itm3 = new ListViewItem();
                    itm3.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm3.BackColor = Color.FromArgb(216, 223, 234);
                    itm3.Text = dt3.Rows[0].ItemArray[2].ToString();
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[3].ToString());
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[4].ToString());
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[5].ToString());
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[6].ToString());
                    itm3.SubItems.Add(dt3.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm3);
                }

                con.Open();
                OdbcDataAdapter dak1 = new OdbcDataAdapter("select*from gradethreegrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradethreegrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
            if (cmbGrade.Text == "Grade 4")
            {
                con.Open();
                OdbcDataAdapter da4 = new OdbcDataAdapter("select*from gradefourgrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dt4 = new DataTable();
                da4.Fill(dt4);
                con.Close();
                if (dt4.Rows.Count > 0)
                {
                    ListViewItem itm4 = new ListViewItem();
                    itm4.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm4.BackColor = Color.FromArgb(216, 223, 234);
                    itm4.Text = dt4.Rows[0].ItemArray[2].ToString();
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[3].ToString());
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[4].ToString());
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[5].ToString());
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[6].ToString());
                    itm4.SubItems.Add(dt4.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm4);
                }

                con.Open();
                OdbcDataAdapter dak4 = new OdbcDataAdapter("select*from gradefourgrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradefourgrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
            if (cmbGrade.Text == "Grade 5")
            {
                con.Open();
                OdbcDataAdapter da5 = new OdbcDataAdapter("select*from gradefivegrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dt5 = new DataTable();
                da5.Fill(dt5);
                con.Close();
                if (dt5.Rows.Count > 0)
                {
                    ListViewItem itm5 = new ListViewItem(); 
                    itm5.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm5.BackColor = Color.FromArgb(216, 223, 234);
                    itm5.Text = dt5.Rows[0].ItemArray[2].ToString();
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[3].ToString());
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[4].ToString());
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[5].ToString());
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[6].ToString());
                    itm5.SubItems.Add(dt5.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm5);
                }

                con.Open();
                OdbcDataAdapter dak4 = new OdbcDataAdapter("select*from gradefivegrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradefivegrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
            if (cmbGrade.Text == "Grade 6")
            {
                con.Open();
                OdbcDataAdapter da6 = new OdbcDataAdapter("select*from gradesixgrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dt6 = new DataTable();
                da6.Fill(dt6);
                con.Close();
                if (dt6.Rows.Count > 0)
                {
                    ListViewItem itm6 = new ListViewItem();
                    itm6.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm6.BackColor = Color.FromArgb(216, 223, 234);
                    itm6.Text = dt6.Rows[0].ItemArray[2].ToString();
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[3].ToString());
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[4].ToString());
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[5].ToString());
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[6].ToString());
                    itm6.SubItems.Add(dt6.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm6);
                }

                con.Open();
                OdbcDataAdapter dak5 = new OdbcDataAdapter("select*from gradesixgrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradesixgrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
            if (cmbGrade.Text == "Grade 7")
            {
                con.Open();
                OdbcDataAdapter da7 = new OdbcDataAdapter("select*from gradesevengrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dt7 = new DataTable();
                da7.Fill(dt7);
                con.Close();
                if (dt7.Rows.Count > 0)
                {
                    ListViewItem itm7 = new ListViewItem();
                    itm7.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm7.BackColor = Color.FromArgb(216, 223, 234);
                    itm7.Text = dt7.Rows[0].ItemArray[2].ToString();
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[3].ToString());
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[4].ToString());
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[5].ToString());
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[6].ToString());
                    itm7.SubItems.Add(dt7.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm7);
                }

                con.Open();
                OdbcDataAdapter dak7 = new OdbcDataAdapter("select*from gradesevengrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradesevengrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
            if (cmbGrade.Text == "Grade 8")
            {
                con.Open();
                OdbcDataAdapter da8 = new OdbcDataAdapter("select*from gradeeightgrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dt8 = new DataTable();
                da8.Fill(dt8);
                con.Close();
                if (dt8.Rows.Count > 0)
                {
                    ListViewItem itm8 = new ListViewItem();
                    itm8.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm8.BackColor =Color.FromArgb(216, 223, 234);
                    itm8.Text = dt8.Rows[0].ItemArray[2].ToString();
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[3].ToString());
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[4].ToString());
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[5].ToString());
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[6].ToString());
                    itm8.SubItems.Add(dt8.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm8);
                }

                con.Open();
                OdbcDataAdapter dak8 = new OdbcDataAdapter("select*from gradeeightgrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeeightgrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
            if (cmbGrade.Text == "Grade 9")
            {
                con.Open();
                OdbcDataAdapter da9 = new OdbcDataAdapter("select*from gradeninegrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dt9 = new DataTable();
                da9.Fill(dt9);
                con.Close();
                if (dt9.Rows.Count > 0)
                {
                    ListViewItem itm9 = new ListViewItem();
                    itm9.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm9.BackColor = Color.FromArgb(216, 223, 234);
                    itm9.Text = dt9.Rows[0].ItemArray[2].ToString();
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[3].ToString());
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[4].ToString());
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[5].ToString());
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[6].ToString());
                    itm9.SubItems.Add(dt9.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm9);
                }

                con.Open();
                OdbcDataAdapter dak9 = new OdbcDataAdapter("select*from gradeninegrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradeninegrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
            if (cmbGrade.Text == "Grade 10")
            {
                con.Open();
                OdbcDataAdapter da10 = new OdbcDataAdapter("select*from gradetengrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                DataTable dt10 = new DataTable();
                da10.Fill(dt10);
                con.Close();
                if (dt10.Rows.Count > 0)
                {
                    ListViewItem itm10 = new ListViewItem();
                    itm10.Font = new Font("Arial", 20, FontStyle.Regular);
                    itm10.BackColor =Color.FromArgb(216, 223, 234);
                    itm10.Text = dt10.Rows[0].ItemArray[2].ToString();
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[3].ToString());
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[4].ToString());
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[5].ToString());
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[6].ToString());
                    itm10.SubItems.Add(dt10.Rows[0].ItemArray[7].ToString());
                    lvwDisplay.Items.Add(itm10);
                }

                con.Open();
                OdbcDataAdapter dak9 = new OdbcDataAdapter("select*from gradetengrades_tbl where studno='" + studentnumber + "'ORDER BY subdesc ASC", con);
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
                OdbcDataAdapter dak11 = new OdbcDataAdapter("select avg(q1),avg(q2),avg(q3),avg(q4),avg(ave)from gradetengrades_tbl where studno='" + studentnumber + "'", con);
                DataTable dtk11 = new DataTable();
                dak11.Fill(dtk11);
                con.Close();
                if (dtk11.Rows.Count > 0)
                {
                    double genave = 0.00;
                    if (dtk11.Rows[0].ItemArray[4].ToString() != "")
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
                }
            }
        }

        private void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlsub.Visible = true;
            lblSubject.Text = cmbSubject.Text;
            lvwDisplay.Items.Clear();
            txtQ1.Clear(); txtQ2.Clear(); txtQ3.Clear(); txtQ4.Clear();
            btnUpdate.Text = "Update";
            btnUpdate.Enabled = false;
            string studentnumber = "";

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("select studno from stud_tbl where (select concat(lname,' ',fname,' ',mname))='" + selectedstud + "'and level='" + cmbGrade.Text + "'and section='" + cmbSec.Text + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                studentnumber = dt.Rows[0].ItemArray[0].ToString();

                if (cmbGrade.Text == "Kinder")
                {
                    con.Open();
                    OdbcDataAdapter dak = new OdbcDataAdapter("select*from kindergrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dtk = new DataTable();
                    dak.Fill(dtk);
                    con.Close();
                    if (dtk.Rows.Count > 0)
                    {
                        ListViewItem itm = new ListViewItem();
                        itm.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm.BackColor = Color.FromArgb(216, 223, 234);
                        itm.Text = dtk.Rows[0].ItemArray[2].ToString();
                        itm.SubItems.Add(dtk.Rows[0].ItemArray[3].ToString());
                        itm.SubItems.Add(dtk.Rows[0].ItemArray[4].ToString());
                        itm.SubItems.Add(dtk.Rows[0].ItemArray[5].ToString());
                        itm.SubItems.Add(dtk.Rows[0].ItemArray[6].ToString());
                        itm.SubItems.Add(dtk.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm);
                    }
                }
                if (cmbGrade.Text == "Grade 1")
                {
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("select*from gradeonegrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {
                       
                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }
                if (cmbGrade.Text == "Grade 2")
                {
                    con.Open();
                    OdbcDataAdapter da2 = new OdbcDataAdapter("select*from gradetwogrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    con.Close();

                    if (dt2.Rows.Count > 0)
                    {

                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt2.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt2.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt2.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt2.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt2.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt2.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }
                if (cmbGrade.Text == "Grade 3")
                {
                    con.Open();
                    OdbcDataAdapter da3 = new OdbcDataAdapter("select*from gradethreegrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da3.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {

                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }
                if (cmbGrade.Text == "Grade 4")
                {
                    con.Open();
                    OdbcDataAdapter da4 = new OdbcDataAdapter("select*from gradefourgrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da4.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {

                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }
                if (cmbGrade.Text == "Grade 5")
                {
                    con.Open();
                    OdbcDataAdapter da5 = new OdbcDataAdapter("select*from gradefivegrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da5.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {

                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }
                if (cmbGrade.Text == "Grade 6")
                {
                    con.Open();
                    OdbcDataAdapter da6 = new OdbcDataAdapter("select*from gradesixgrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da6.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {

                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }
                if (cmbGrade.Text == "Grade 7")
                {
                    con.Open();
                    OdbcDataAdapter da7 = new OdbcDataAdapter("select*from gradesevengrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da7.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {

                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }
                if (cmbGrade.Text == "Grade 8")
                {
                    con.Open();
                    OdbcDataAdapter da8 = new OdbcDataAdapter("select*from gradeeightgrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da8.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {

                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }
                if (cmbGrade.Text == "Grade 9")
                {
                    con.Open();
                    OdbcDataAdapter da9 = new OdbcDataAdapter("select*from gradeninegrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da9.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {

                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }
                if (cmbGrade.Text == "Grade 10")
                {
                    con.Open();
                    OdbcDataAdapter da10 = new OdbcDataAdapter("select*from gradetengrades_tbl where studno='" + studentnumber + "'and subdesc='" + cmbSubject.Text + "'", con);
                    DataTable dt1 = new DataTable();
                    da10.Fill(dt1);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {

                        ListViewItem itm1 = new ListViewItem();
                        itm1.Font = new Font("Arial", 20, FontStyle.Regular);
                        itm1.BackColor = Color.FromArgb(216, 223, 234);
                        itm1.Text = dt1.Rows[0].ItemArray[2].ToString();
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[3].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[4].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[5].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[6].ToString());
                        itm1.SubItems.Add(dt1.Rows[0].ItemArray[7].ToString());
                        lvwDisplay.Items.Add(itm1);
                    }
                }


                txtQ1.Clear();
                txtQ2.Clear();
                txtQ3.Clear();
                txtQ4.Clear();
            }
        }

        private void btnclr_Click(object sender, EventArgs e)
        {
            txtQ1.Clear();
            txtQ2.Clear();
            txtQ3.Clear();
            txtQ4.Clear();
            btnUpdate.Enabled = false;
        }

        private void lvwDisplay_Click(object sender, EventArgs e)
        {
            if (lvwDisplay.Items.Count == 0) 
            { 
                return;
            }
            else 
            {
          
                btnUpdate.Enabled = true;

                if (lvwDisplay.Items[0].Text == "0")
                {
                    txtQ1.Text = "0";
                    txtQ1.Focus();
                }
                else
                {
                    txtQ1.Text = lvwDisplay.Items[0].Text;
                }



                if (lvwDisplay.Items[0].SubItems[1].Text == "0")
                {
                    txtQ2.Text = "0";
                }
                else
                {
                    txtQ2.Text = lvwDisplay.Items[0].SubItems[1].Text;
                }



                if (lvwDisplay.Items[0].SubItems[2].Text == "0")
                {
                    txtQ3.Text = "0";
                }
                else
                {
                    txtQ3.Text = lvwDisplay.Items[0].SubItems[2].Text;
                }



                if (lvwDisplay.Items[0].SubItems[3].Text == "0")
                {
                    txtQ4.Text = "0";
                }
                else
                {
                    txtQ4.Text = lvwDisplay.Items[0].SubItems[3].Text;
                }
                lblStudent.Focus();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(btnUpdate.Text=="Update")
            {
                txtQ1.Enabled = true;
                txtQ2.Enabled = true;
                txtQ3.Enabled = true;
                txtQ4.Enabled = true;
                btnUpdate.Text = "Save";
            }
            else
            {
            double ave = 0.00;
            double sum = 0.00;
            string remarks = "";

            getsnum();

            if (txtQ1.Text != "" && txtQ2.Text != "" && txtQ3.Text != "" && txtQ4.Text != "" && txtQ1.Text != "0" && txtQ2.Text != "0" && txtQ3.Text != "0" && txtQ4.Text != "0")
            {
                double q1 = Convert.ToDouble(txtQ1.Text);
                double q2 = Convert.ToDouble(txtQ2.Text);
                double q3 = Convert.ToDouble(txtQ3.Text);
                double q4 = Convert.ToDouble(txtQ4.Text);
                sum = q1 + q2 + q3 + q4;
                ave = sum / 4;

                if (ave < 75 )
                {
                    if (ave == 0)
                    {
                        remarks = "";
                    }
                    else
                    {
                        remarks = "Failed";
                    }
                }
                else 
                {
                    remarks = "Passed";
                }

                if(cmbGrade.Text=="Kinder")
                {
                    con.Open();
                    string upd = "Update kindergrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    
                }
                if (cmbGrade.Text == "Grade 1")
                {
                    con.Open();
                    string upd = "Update gradeonegrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    
                }
                if (cmbGrade.Text == "Grade 2")
                {
                    con.Open();
                    string upd = "Update gradetwogrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 3")
                {
                    con.Open();
                    string upd = "Update gradethreegrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close(); 
                   
                }
                if (cmbGrade.Text == "Grade 4")
                {
                    con.Open();
                    string upd = "Update gradefourgrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 5")
                {
                    con.Open();
                    string upd = "Update gradefivegrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 6")
                {
                    con.Open();
                    string upd = "Update gradesixgrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 7")
                {
                    con.Open();
                    string upd = "Update gradesevengrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                  
                }
                if (cmbGrade.Text == "Grade 8")
                {
                    con.Open();
                    string upd = "Update gradeeightgrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 9")
                {
                    con.Open();
                    string upd = "Update gradeninegrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    
                }
                if (cmbGrade.Text == "Grade 10")
                {
                    con.Open();
                    string upd = "Update gradetengrades_tbl set ave='" + ave + "',remarks='" + remarks + "',q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
            }
            else
            {
                if (cmbGrade.Text == "Kinder")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update kindergrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update kindergrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='"+txtQ3.Text+"',q4='"+txtQ4.Text+"',ave='"+""+"'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 1")
                {
                    
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradeonegrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradeonegrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 2")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradetwogrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradetwogrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    
                }
                if (cmbGrade.Text == "Grade 3")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradethreegrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradethreegrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 4")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradefourgrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradefourgrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 5")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradefivegrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradefivegrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                  
                }
                if (cmbGrade.Text == "Grade 6")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradesixgrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradesixgrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                  
                }
                if (cmbGrade.Text == "Grade 7")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradesevengrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradesevengrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                  
                }
                if (cmbGrade.Text == "Grade 8")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradeeightgrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradeeightgrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                  
                }
                if (cmbGrade.Text == "Grade 9")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradeninegrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradeninegrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                }
                if (cmbGrade.Text == "Grade 10")
                {
                    if (txtQ1.Text == "" || txtQ2.Text == "" || txtQ3.Text == "" || txtQ4.Text == "" || txtQ1.Text == "0" || txtQ2.Text == "0" || txtQ3.Text == "0" || txtQ4.Text == "0")
                    {
                        con.Open();
                        string upd1 = "Update gradetengrades_tbl set ave='" + "0.00" + "',remarks='" + "" + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                        OdbcCommand cmd1 = new OdbcCommand(upd1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string upd = "Update gradetengrades_tbl set q1='" + txtQ1.Text + "',q2='" + txtQ2.Text + "',q3='" + txtQ3.Text + "',q4='" + txtQ4.Text + "'where studno='" + snum + "'and subdesc='" + cmbSubject.Text + "'";
                    OdbcCommand cmd = new OdbcCommand(upd, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    
                }
            }

            refreshDisplay();
            //txtQ1.Clear();
            //txtQ2.Clear();
            //txtQ3.Clear();
            //txtQ4.Clear();
            txtQ1.Enabled = false; txtQ2.Enabled = false;
            txtQ3.Enabled = false; txtQ4.Enabled = false;
            btnUpdate.Enabled = true; btnUpdate.Text = "Update";
        }
        }

        private void lvwDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSectioning_Click(object sender, EventArgs e)
        {
            frmSectioning sectioningfrm = new frmSectioning();
            this.Hide();
            sectioningfrm.seclog = grdlog;
            sectioningfrm.TheFaculty = theFacultyName;
            sectioningfrm.Show();
        }

        private void dgvm_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Hand;
        }

        private void dgvm_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Default;
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Student grades")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Student grades")
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
                    casmain.cashlog = grdlog;
                    casmain.accesscode = accesscode;
                    casmain.CO = CO;
                    casmain.thefac = theFacultyName;
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
                    pmf.thefac = theFacultyName;
                    pmf.prinlog = grdlog;
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
                    regmain.reglog = grdlog;
                    regmain.accesscode = accesscode;
                    regmain.thefac = theFacultyName;
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
                    empf.faclog = grdlog;
                    empf.accesscode = accesscode;
                    empf.TheFacultyName = theFacultyName;
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
                frmadm.admlog = grdlog;
                frmadm.CO = CO;
                frmadm.accesscode = accesscode;
                frmadm.TheFaculty = theFacultyName;
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
                formPay.paylog = grdlog;
                formPay.CashierOperator = CO;
                formPay.accesscode = accesscode;
                formPay.TheFac = theFacultyName;
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
                formStudRec.asslog = grdlog;
                formStudRec.accesscode = accesscode;
                formStudRec.thefac = theFacultyName;
                formStudRec.VISITED = VISITED;
                formStudRec.viewNotifDue = viewNotifDue;
                formStudRec.viewNotifDisc = viewNotifDisc;
                formStudRec.viewNotifLate = viewNotifLate;
                formStudRec.notifstat = notifstat;
                formStudRec.Show();
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Student grades")
            {
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  Student information")
            {
                frmStudInfo stud = new frmStudInfo();
                this.Hide();
                stud.emptype = emptype;
                stud.CO = CO;
                stud.studlog = grdlog;
                stud.accesscode = accesscode;
                stud.TheFaculty = theFacultyName;
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
                facf.facinfolog = grdlog;
                facf.accesscode = accesscode;
                facf.TheFaculty = theFacultyName;
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
                frmFacAdv.advlog = grdlog;
                frmFacAdv.accesscode = accesscode;
                frmFacAdv.thefac = theFacultyName;
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
                frmSec.seclog = grdlog;
                frmSec.accesscode = accesscode;
                frmSec.TheFaculty = theFacultyName;
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
                rfac.replog = grdlog;
                rfac.emptype = emptype;
                rfac.accesscode = accesscode;
                rfac.theFaculty = theFacultyName;
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
                rsched.schedlog = grdlog;
                rsched.emptype = emptype;
                rsched.accesscode = accesscode;
                rsched.TheFaculty = theFacultyName;
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
                about.ablog = grdlog;
                about.emptype = emptype;
                about.CO = CO;
                about.accesscode = accesscode;
                about.theFaculty = theFacultyName;
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
