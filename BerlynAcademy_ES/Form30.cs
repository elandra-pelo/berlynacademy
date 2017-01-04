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
    public partial class frmHomeMaintenance : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=sa;DB=berlyn");
        public string adminlog,VISITED;
        public frmHomeMaintenance()
        {
            InitializeComponent();
        }

        private void frmHomeMaintenance_Load(object sender, EventArgs e)
        {
            lblLogger.Text = adminlog;
            lblLoggerPosition.Text = "Admin";
            //this.BackColor = Color.FromArgb(0, 0, 25);
            //pbline.BackColor = Color.FromArgb(15, 15, 15);
            lblActs.Text = "Today is \n" + DateTime.Now.ToLongDateString();
            lvwnotif.Columns.Add("", 15, HorizontalAlignment.Center);
            lvwnotif.Columns.Add("", 700, HorizontalAlignment.Left);
            //checkIfOKSched();
            //setupNotifications();

            if (VISITED == null)
            {
                VISITED += "   Home";
            }
            else
            {
                if (VISITED.Contains("Home") == false)
                {
                    VISITED += "   Home";
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

            if (dt.Rows.Count > 0)
            {
                //plus 4 referring to recess, lunch , homeroom, class preparation
                kinderTotal = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString()) + 4;
                g1Total = Convert.ToInt32(dt.Rows[0].ItemArray[1].ToString()) + 4;
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
                    OdbcDataAdapter dak1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where section='" + dtk.Rows[k].ItemArray[0].ToString() + "'", con);
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
                ListViewItem itmsched = new ListViewItem();
                itmsched.Text = "!";
                itmsched.SubItems.Add("scheduling is not finish.");
                lvwnotif.Items.Add(itmsched);
            }



        }

        public void setupNotifications()
        {
            string activeSY = "";
            string activeYr = "";
            //--------------------------
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select * from schoolyear_tbl where status='Active'", con);
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count < 1)
            {
                ListViewItem itmsy = new ListViewItem();
                itmsy.Text = "!";
                itmsy.SubItems.Add("there is no active school year.");
                lvwnotif.Items.Add(itmsy);
            }
            else
            {
                activeSY = dt.Rows[0].ItemArray[1].ToString();
                activeYr = dt.Rows[0].ItemArray[0].ToString();
            }
            //------------------------------
            bool isthereNoAvailable = false;
            string levellistfull = "";
            con.Open();
            DataTable dtlev = new DataTable();
            OdbcDataAdapter dalev = new OdbcDataAdapter("Select distinct level from level_tbl", con);
            dalev.Fill(dtlev);
            con.Close();
            if (dtlev.Rows.Count > 0)
            {
                int theNumFull = 0;
                int theNumSec = 0;
                for (int i = 0; i < dtlev.Rows.Count; i++)
                {
                   
                    con.Open();
                    DataTable dtra = new DataTable();
                    OdbcDataAdapter dara = new OdbcDataAdapter("Select section from section_tbl where level='" + dtlev.Rows[i].ItemArray[0].ToString() + "'", con);
                    dara.Fill(dtra);
                    con.Close();
                    if (dtra.Rows.Count > 0)
                    {
                        theNumSec = dtra.Rows.Count;
                      
                        for (int s = 0; s < dtra.Rows.Count; s++)
                        {
                            theNumFull = 0;
                            con.Open();
                            DataTable dtrm = new DataTable();
                            OdbcDataAdapter darm = new OdbcDataAdapter("Select id from roomallocation_tbl where grade='" + dtlev.Rows[i].ItemArray[0].ToString() + "'and section='" + dtra.Rows[s].ItemArray[0].ToString() + "'", con);
                            darm.Fill(dtrm);
                            con.Close();
                            if (dtrm.Rows.Count > 0)
                            {
                               
                                con.Open();
                                DataTable dtcap = new DataTable();
                                OdbcDataAdapter dacap = new OdbcDataAdapter("Select capacity from room_tbl where id='" + dtrm.Rows[0].ItemArray[0].ToString() + "'", con);
                                dacap.Fill(dtcap);
                                con.Close();
                                if (dtcap.Rows.Count > 0)
                                {
                                  
                                    int cap = Convert.ToInt32(dtcap.Rows[0].ItemArray[0].ToString());
                                    int numstud = 0;
                                    con.Open();
                                   
                                    OdbcDataAdapter dast = new OdbcDataAdapter("Select Count(studno)from stud_tbl where section='" + dtra.Rows[s].ItemArray[0].ToString() + "'and level='" + dtlev.Rows[i].ItemArray[0].ToString() + "'and status='Active'", con);
                                    DataTable dtst = new DataTable();
                                    dast.Fill(dtst);
                                    con.Close();
                                    if (dtst.Rows.Count > 0)
                                    {
                                        numstud = Convert.ToInt32(dtst.Rows[0].ItemArray[0].ToString()); 
                                    }

                                 
                                    if (cap == numstud)
                                    {
                                        theNumFull++;
                                        isthereNoAvailable = true;
                                    }

                                   
                                    if (theNumSec == theNumFull)
                                    {
                                        levellistfull += dtlev.Rows[i].ItemArray[0].ToString()+" ";
                                    }

                                }
                            }
                        }
                    }
                }
            }

            if (isthereNoAvailable==true && levellistfull!="")
            {
                ListViewItem itmfl = new ListViewItem();
                itmfl.Text = "!";
                itmfl.SubItems.Add("no available section in " + levellistfull + " for auto-sectioning.");
                lvwnotif.Items.Add(itmfl);
            }
           

           //----------------------------
                con.Open();
                OdbcDataAdapter dafind= new OdbcDataAdapter("select level from level_tbl", con);
                DataTable dtfind = new DataTable();
                dafind.Fill(dtfind);
                con.Close();
                if (dtfind.Rows.Count > 0)
                {
                    for (int x = 0; x < dtfind.Rows.Count; x++)
                    {
                        con.Open();
                        OdbcDataAdapter daax = new OdbcDataAdapter("select level from section_tbl where status='" + "active" + "'and level='" + dtfind.Rows[x].ItemArray[0].ToString() + "'", con);
                        DataTable dttx = new DataTable();
                        daax.Fill(dttx);
                        con.Close();
                        if (dttx.Rows.Count <= 0)
                        {
                            ListViewItem itmrk = new ListViewItem();
                            itmrk.Text = "!";
                            itmrk.SubItems.Add("there is no active section for auto-sectioning in " + dtfind.Rows[x].ItemArray[0].ToString());
                            lvwnotif.Items.Add(itmrk);
                        }
                    }

                }


            //--------------------
            con.Open();
            DataTable dtta = new DataTable();
            OdbcDataAdapter daat = new OdbcDataAdapter("Select*from activity_tbl where SY='"+activeSY+"'", con);
            daat.Fill(dtta);
            con.Close();
            if (dtta.Rows.Count <=0)
            {
                ListViewItem itmrk = new ListViewItem();
                itmrk.Text = "!";
                itmrk.SubItems.Add("active school year has no calendar of activities.");
                lvwnotif.Items.Add(itmrk);
            }

            //-------------------
            con.Open();
            DataTable dtt = new DataTable();
            OdbcDataAdapter daa = new OdbcDataAdapter("Select * from section_tbl where rank='' or rank='0'", con);
            daa.Fill(dtt);
            con.Close();
            if (dtt.Rows.Count >0)
            {
                ListViewItem itmrk = new ListViewItem();
                itmrk.Text = "!";
                itmrk.SubItems.Add("there is a section has no rank.");
                lvwnotif.Items.Add(itmrk);
            }

            //check if all sections are allocated.
            bool allocated = true;
            con.Open();
            DataTable dt1 = new DataTable();
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select section from section_tbl", con);
            da1.Fill(dt1);
            con.Close();
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    con.Open();
                    DataTable dt2 = new DataTable();
                    OdbcDataAdapter da2 = new OdbcDataAdapter("Select section from roomallocation_tbl where section='"+dt1.Rows[i].ItemArray[0].ToString()+"'", con);
                    da2.Fill(dt2);
                    con.Close();
                    if (dt2.Rows.Count < 1)
                    {
                        allocated = false;
                        i = dt1.Rows.Count;
                    }
                }
            }

            if (allocated == false)
            {
                ListViewItem itmalloc = new ListViewItem();
                itmalloc.Text = "!";
                itmalloc.SubItems.Add("there is a section not yet assigned to room.");
                lvwnotif.Items.Add(itmalloc);
            }


            //check if theres a grade level in a db
            con.Open();
            DataTable dt3 = new DataTable();
            OdbcDataAdapter da3 = new OdbcDataAdapter("Select level from level_tbl", con);
            da3.Fill(dt3);
            con.Close();
            if (dt3.Rows.Count <1)
            {
                ListViewItem itmlev = new ListViewItem();
                itmlev.Text = "!";
                itmlev.SubItems.Add("there is no grade level.");
                lvwnotif.Items.Add(itmlev);
            }


            //check if theres a subject in db
            bool theressubject = true;
            con.Open();
            DataTable dt4 = new DataTable();
            OdbcDataAdapter da4 = new OdbcDataAdapter("Select level from level_tbl", con);
            da4.Fill(dt4);
            con.Close();
            if (dt4.Rows.Count>0)
            {
                for (int h = 0; h < dt4.Rows.Count; h++)
                {
                    con.Open();
                    DataTable dt5 = new DataTable();
                    OdbcDataAdapter da5 = new OdbcDataAdapter("Select*from subject_tbl where level='"+dt4.Rows[h].ItemArray[0].ToString()+"'", con);
                    da5.Fill(dt5);
                    con.Close();
                    if (dt5.Rows.Count <1)
                    {
                        theressubject = false;
                        h = dt4.Rows.Count;
                    }
                }
            }

            if (theressubject == false)
            {
                ListViewItem itmsub = new ListViewItem();
                itmsub.Text = "!";
                itmsub.SubItems.Add("there is a level that has no subject.");
                lvwnotif.Items.Add(itmsub);
            }

            //check if theres requirements for NTR in db
            con.Open();
            DataTable dt6 = new DataTable();
            OdbcDataAdapter da6 = new OdbcDataAdapter("Select name from requirement_tbl where type='NTR'", con);
            da6.Fill(dt6);
            con.Close();
            if (dt6.Rows.Count < 0)
            {
                ListViewItem itmntr = new ListViewItem();
                itmntr.Text = "!";
                itmntr.SubItems.Add("there is no requirements for new/transferee enrolee.");
                lvwnotif.Items.Add(itmntr);
            }

            //check if theres requirements for old in db
            con.Open();
            DataTable dt7 = new DataTable();
            OdbcDataAdapter da7 = new OdbcDataAdapter("Select name from requirement_tbl where type='OLD'", con);
            da7.Fill(dt7);
            con.Close();
            if (dt7.Rows.Count < 0)
            {
                ListViewItem itmold = new ListViewItem();
                itmold.Text = "!";
                itmold.SubItems.Add("there is no requirements for old students.");
                lvwnotif.Items.Add(itmold);
            }

            //check if all fees is ok
            con.Open();
            DataTable dt8 = new DataTable();
            OdbcDataAdapter da8 = new OdbcDataAdapter("Select*from fee_tbl where amount LIKE'0.00' and type='fee' and SY='"+activeSY+"'", con);
            da8.Fill(dt8);
            con.Close();
            if (dt8.Rows.Count > 0)
            {
                ListViewItem itmfee = new ListViewItem();
                itmfee.Text = "!";
                itmfee.SubItems.Add("school fees is not yet finish.");
                lvwnotif.Items.Add(itmfee);
            }

            //check if all payment is ok
            con.Open();
            DataTable dt9 = new DataTable();
            OdbcDataAdapter da9 = new OdbcDataAdapter("Select*from fee_tbl where amount LIKE'0.00' and type='payment' and SY='" + activeSY + "'", con);
            da9.Fill(dt9);
            con.Close();
            if (dt9.Rows.Count > 0)
            {
                ListViewItem itmpay = new ListViewItem();
                itmpay.Text = "!";
                itmpay.SubItems.Add("school payment is not yet finish.");
                lvwnotif.Items.Add(itmpay);
            }


            //----------------------------
            con.Open();
            DataTable dted = new DataTable();
            OdbcDataAdapter daed = new OdbcDataAdapter("Select * from enrollmentdays_tbl where SY='" + activeSY + "'", con);
            daed.Fill(dted);
            con.Close();
            if (dted.Rows.Count > 0)
            {
                if (dted.Rows[0].ItemArray[0].ToString() == "" || dted.Rows[0].ItemArray[1].ToString() == "")
                {
                    ListViewItem itmed = new ListViewItem();
                    itmed.Text = "!";
                    itmed.SubItems.Add("enrollment schedule is not set.");
                    lvwnotif.Items.Add(itmed);
                    return;
                }

                DateTime tod;
                string syfirstterm = activeSY.Substring(3, 4).ToString();
                string sysecondterm = activeSY.Substring(8, 4).ToString();
                if (DateTime.Now.Year.ToString() == syfirstterm || DateTime.Now.Year.ToString()==sysecondterm)
                {
                    tod = DateTime.Now;
                }
                else
                {
                    tod = Convert.ToDateTime(dted.Rows[0].ItemArray[0].ToString());
                }
                DateTime end = Convert.ToDateTime(dted.Rows[0].ItemArray[1].ToString());
                TimeSpan remaining = end.Subtract(tod);

                //DateTime now = Convert.ToDateTime(DateTime.Now.ToLongDateString());orig
                DateTime now = Convert.ToDateTime(tod.ToLongDateString());
                DateTime endlong = Convert.ToDateTime(end.ToLongDateString());

                if (now > endlong)
                {
                    ListViewItem itmed = new ListViewItem();
                    itmed.Text = "!";
                    itmed.SubItems.Add("enrollment days was end.");
                    lvwnotif.Items.Add(itmed);
                }
                else
                {
                    if ((tod.Day == end.Day) && (tod.Month == end.Month) && (tod.Year == end.Year))
                    {
                        ListViewItem itmed = new ListViewItem();
                        itmed.ForeColor = Color.Green;
                        itmed.Text = "i";
                        itmed.SubItems.Add("enrollment is until today.");
                        lvwnotif.Items.Add(itmed);
                    }
                    else
                    {
                        ListViewItem itmed = new ListViewItem();
                        itmed.ForeColor = Color.Green;
                        itmed.Text = "i";
                        if (remaining.Days == 0)
                        {
                            itmed.SubItems.Add(remaining.Days + 1 + " day before enrollment will end.");
                        }
                        else
                        {
                            string dis = "";
                            double dates = Convert.ToDouble(remaining.Days+1);
                            if (dates >= 1000)
                            {
                                dis = String.Format(("{0:0,###}"), dates);
                            }
                            if (dates < 1000)
                            {
                                dis = String.Format(("{0:0}"), dates);
                            }

                            itmed.SubItems.Add(dis+ " days before enrollment will end.");
                        }
                        lvwnotif.Items.Add(itmed);
                    }
                }
            }
            else
            {
                ListViewItem itmed = new ListViewItem();
                itmed.Text = "!";
                itmed.SubItems.Add("enrollment schedule is not set.");
                lvwnotif.Items.Add(itmed);
            }

            //-------------------
            
        }

        private void pbuser_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(256, 398);
            lbltype.Text = "User";
            lbltype.Location = new Point(620, 201);
            lblmaintenance.Visible = true;
        }

        private void pbsubj_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(319, 398);
            lbltype.Text = "Subject";
            lbltype.Location = new Point(589, 201);
            lblmaintenance.Visible = true;
        }

        private void pbsec_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(380, 398);
            lbltype.Text = "Section";
            lbltype.Location = new Point(589, 201);
            lblmaintenance.Visible = true;
        }

        private void pbroom_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(441, 398);
            lbltype.Text = "Room";
            lbltype.Location = new Point(604, 201);
            lblmaintenance.Visible = true;
        }

        private void pbsched_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(502, 398);
            lbltype.Text = "Schedule";
            lbltype.Location = new Point(571, 201);
            lblmaintenance.Visible = true;
        }

        private void pbreqs_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(563, 398);
            lbltype.Text = "Requirement";
            lbltype.Location = new Point(531, 201);
            lblmaintenance.Visible = true;
        }

        private void pbfee_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(624, 398);
            lbltype.Text = "Student fee";
            lbltype.Location = new Point(546, 201);
            lblmaintenance.Visible = true;
        }

        private void pbdisc_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(685, 398);
            lbltype.Text = "Discount";
            lbltype.Location = new Point(574, 201);
            lblmaintenance.Visible = true;
        }

        private void pbact_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(746, 398);
            lbltype.Text = "Activity";
            lbltype.Location = new Point(590, 201);
            lblmaintenance.Visible = true;
        }

        private void pbabout_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(868, 398);
            lbltype.Text = "About";
            lbltype.Location = new Point(602, 201);
            lblmaintenance.Visible = true;
        }

        private void pbbackup_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(929, 398);
            lbltype.Text = "Back-up";
            lbltype.Location = new Point(581, 201);
            lblmaintenance.Visible=true;
        }

        private void pbsetting_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(990, 398);
            lbltype.Text = "Settings";
            lbltype.Location = new Point(582, 201);
            lblmaintenance.Visible = false;
        }

        private void pblogout_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(1051, 398);
            lbltype.Text = "Logout";
            lbltype.Location = new Point(592, 201);
            lblmaintenance.Visible=false;
        }

        private void pbuser_Click(object sender, EventArgs e)
        {
           
            frmMaintenance user = new frmMaintenance();
            this.Dispose();
            user.adminlog = adminlog;
            user.VISITED = VISITED;
            user.Show();
        }

        private void pbsubj_Click(object sender, EventArgs e)
        {
            
            frmSubject subj = new frmSubject();
            this.Dispose();
            subj.wholog = adminlog;
            subj.VISITED = VISITED;
            subj.Show();
        }

        private void pbsec_Click(object sender, EventArgs e)
        {
            frmSection secform = new frmSection();
            this.Dispose();
            secform.secwholog = adminlog;
            secform.VISITED = VISITED;
            secform.Show();
        }

        private void pbroom_Click(object sender, EventArgs e)
        {
            frmRoom roomform = new frmRoom();
            this.Dispose();
            roomform.logger = adminlog;
            roomform.VISITED = VISITED;
            roomform.Show();
        }

        private void pbsched_Click(object sender, EventArgs e)
        {
            frmSched sf = new frmSched();
            this.Dispose();
            sf.schedlog = adminlog;
            sf.VISITED = VISITED;
            sf.Show();
        }

        private void pbreqs_Click(object sender, EventArgs e)
        {
            frmRequirement reqform = new frmRequirement();
            this.Dispose();
            reqform.reqlog = adminlog;
            reqform.VISITED = VISITED;
            reqform.Show();
        }

        private void pbfee_Click(object sender, EventArgs e)
        {
            frmFee feeform = new frmFee();
            this.Dispose();
            feeform.feelog = adminlog;
            feeform.VISITED = VISITED;
            feeform.Show();
        }

        private void pbdisc_Click(object sender, EventArgs e)
        {
            frmDiscount df = new frmDiscount();
            this.Dispose();
            df.disclog = adminlog;
            df.VISITED = VISITED;
            df.Show();
        }

        private void pbact_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Dispose();
            actform.actlog = adminlog;
            actform.VISITED = VISITED;
            actform.Show();
        }

        private void pbabout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abtmain = new frmAboutMaintenance();
            this.Dispose();
            abtmain.amlog = adminlog;
            abtmain.VISITED = VISITED;
            abtmain.Show();
        }

        private void pbbackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Dispose();
            buf.backlog = adminlog;
            buf.VISITED = VISITED;
            buf.Show();
        }

        private void pbsetting_Click(object sender, EventArgs e)
        {
            
        }

        private void pblogout_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin homeform = new frmEmpLogin();
            this.Dispose();
            homeform.Show();
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

        private void pbAudit_Click(object sender, EventArgs e)
        {
            frmAudit auditform = new frmAudit();
            this.Dispose();
            auditform.auditlogger = adminlog;
            auditform.VISITED = VISITED;
            auditform.Show();
        }

        private void pbAudit_MouseEnter(object sender, EventArgs e)
        {
            pborange.Visible = true;
            pborange.Location = new Point(807, 398);
            lbltype.Text = "Audit trail";
            lbltype.Location = new Point(563, 201);
            lblmaintenance.Visible = false;
        }

        private void lnkOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LOGOUT();
            frmEmpLogin homeform = new frmEmpLogin();
            this.Dispose();
            homeform.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance user = new frmMaintenance();
            this.Dispose();
            user.adminlog = adminlog;
            user.VISITED = VISITED;
            user.Show();
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            this.Dispose();
            symaintenance.sylog = adminlog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subjmaintenance = new frmSubject();
            this.Dispose();
            subjmaintenance.wholog = adminlog;
            subjmaintenance.VISITED = VISITED;
            subjmaintenance.Show();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Dispose();
            levmain.levlog = adminlog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection section = new frmSection();
            this.Dispose();
            section.secwholog = adminlog;
            section.VISITED = VISITED;
            section.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roommaintenance = new frmRoom();
            this.Dispose();
            roommaintenance.logger = adminlog;
            roommaintenance.VISITED = VISITED;
            roommaintenance.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            this.Dispose();
            facmain.facmlog = adminlog;
            facmain.VISITED = VISITED;
            facmain.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched schedf = new frmSched();
            this.Dispose();
            schedf.schedlog = adminlog;
            schedf.VISITED = VISITED;
            schedf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqf = new frmRequirement();
            this.Dispose();
            reqf.reqlog = adminlog;
            reqf.VISITED = VISITED;
            reqf.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feef = new frmFee();
            this.Dispose();
            feef.feelog = adminlog;
            feef.VISITED = VISITED;
            feef.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Dispose();
            discform.disclog = adminlog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            frmAudit auditform = new frmAudit();
            this.Dispose();
            auditform.auditlogger = adminlog;
            auditform.VISITED = VISITED;
            auditform.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Dispose();
            buf.backlog = adminlog;
            buf.VISITED = VISITED;
            buf.Show();
        }

        private void btnAccess_Click(object sender, EventArgs e)
        {
            frmUserAccessLevel ualform = new frmUserAccessLevel();
            this.Dispose();
            ualform.acclog = adminlog;
            ualform.VISITED = VISITED;
            ualform.Show();
        }

        private void pbaccess_Click(object sender, EventArgs e)
        {
            frmUserAccessLevel ualform = new frmUserAccessLevel();
            this.Dispose();
            ualform.acclog = adminlog;
            ualform.VISITED = VISITED;
            ualform.Show();
        }

        private void pbsy_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            this.Dispose();
            symaintenance.sylog = adminlog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
        }

        private void pblevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Dispose();
            levmain.levlog = adminlog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void pbFac_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            this.Dispose();
            facmain.facmlog = adminlog;
            facmain.VISITED = VISITED;
            facmain.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abm = new frmAboutMaintenance();
            this.Dispose();
            abm.amlog = adminlog;
            abm.VISITED = VISITED;
            abm.Show();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Dispose();
            actform.actlog =adminlog;
            actform.VISITED = VISITED;
            actform.Show();
        }

        private void btnEDays_Click(object sender, EventArgs e)
        {
            frmEnrollmentDays eform = new frmEnrollmentDays();
            this.Dispose();
            eform.edlog = adminlog;
            eform.VISITED = VISITED;
            eform.Show();
        }

        private void pbedays_Click(object sender, EventArgs e)
        {
            frmEnrollmentDays eform = new frmEnrollmentDays();
            this.Dispose();
            eform.edlog = adminlog;
            eform.VISITED = VISITED;
            eform.Show();
        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            frmStudentStats stform = new frmStudentStats();
            this.Dispose();
            stform.statlog = adminlog;
            stform.VISITED = VISITED;
            stform.Show();
        }

        private void pbstat_Click(object sender, EventArgs e)
        {
            frmStudentStats stform = new frmStudentStats();
            this.Dispose();
            stform.statlog = adminlog;
            stform.VISITED = VISITED;
            stform.Show();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = adminlog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStudent_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = adminlog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void pbStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = adminlog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void pbStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = adminlog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

       
        private void btnSecPri_Click(object sender, EventArgs e)
        {
            frmPrioritySec priorsec = new frmPrioritySec();
            this.Dispose();
            priorsec.priorlog = adminlog;
            priorsec.VISITED = VISITED;
            priorsec.Show();
        }

        private void pbSecPri_Click(object sender, EventArgs e)
        {
            frmPrioritySec priorsec = new frmPrioritySec();
            this.Dispose();
            priorsec.priorlog = adminlog;
            priorsec.VISITED = VISITED;
            priorsec.Show();
        }

        private void lnkOut_MouseEnter(object sender, EventArgs e)
        {
            lnkOut.BackColor = Color.WhiteSmoke;
        }

        private void lnkOut_MouseLeave(object sender, EventArgs e)
        {
            lnkOut.BackColor = Color.White;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog= adminlog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void pbDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = adminlog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void btnAssRoom_Click(object sender, EventArgs e)
        {
            frmAssignRoom asrom = new frmAssignRoom();
            this.Dispose();
            asrom.asromlog = adminlog;
            asrom.VISITED = VISITED;
            asrom.Show();
        }

        private void pbAssRoom_Click(object sender, EventArgs e)
        {
            frmAssignRoom asrom = new frmAssignRoom();
            this.Dispose();
            asrom.asromlog = adminlog;
            asrom.VISITED = VISITED;
            asrom.Show();
        }

      
    }
}
