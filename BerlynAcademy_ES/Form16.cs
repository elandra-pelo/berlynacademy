using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Globalization;

namespace BerlynAcademy_ES
{
    public partial class frmSched : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=sa;DB=berlyn");
        public string schedlog,classgrade,classsec,day,primarykey,tempsubj,tempdays,selectedfac,VISITED,day1code,day2code,emptype;
        public string CO, accesscode, TheFaculty, notifstat;
        public bool viewNotifDue, viewNotifLate, viewNotifDisc,isVisited;
        public int MINDAY, MAXDAY, MINDAYDB, MAXDAYDB;
        public DataView dvSec,dvFacs,dvFacSched;
        public frmSched()
        {
            InitializeComponent();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discf = new frmDiscount();
            this.Hide();
            discf.disclog = schedlog;
            discf.VISITED = VISITED;
            discf.Show();
        }

        private void frmSched_Load(object sender, EventArgs e)
        {
            //btnHome.Text = "          " + schedlog;
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //pnlhead.BackColor = Color.FromArgb(244, 194, 13);
            //this.BackColor = Color.FromArgb(49, 79, 142);
            //setupallfaculty();
            lblLogger.Text = schedlog;
            lblLoggerPos.Text =emptype;
            btnSched.BackColor = Color.LightGreen;
            setupview();
            //setuprooms();
            cmbLevel.Text = "Show all";
            cmbOperation.Text = "Class schedule";
            lbldismemo.Text = "no selected class.";
            lbldismemo.Location = new Point(318, 8);
            lblAcdNonCount.Location = new Point(3, 16);

            if (isVisited == false)
            {
                if (VISITED.Contains("Scheduling") == false)
                {
                    VISITED += "   Scheduling";
                    isVisited = true;
                }
            }
            setupMENU();
            lblUserNo.Focus();
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

            int getschedindex = 1;
            dtMenu.Rows.Add("  Activity");
            if (dt1.Rows.Count > 0)
            {
                getschedindex++;
                dtMenu.Rows.Add("  " + dt1.Rows[0].ItemArray[1].ToString());
            }
            if (dt2.Rows.Count > 0)
            {
                getschedindex++;
                dtMenu.Rows.Add("  " + dt2.Rows[0].ItemArray[1].ToString());
            }
            if (dt3.Rows.Count > 0)
            {
                getschedindex++;
                dtMenu.Rows.Add("  " + dt3.Rows[0].ItemArray[1].ToString());
            }
            if (dt4.Rows.Count > 0)
            {
                getschedindex++;
                dtMenu.Rows.Add("  " + dt4.Rows[0].ItemArray[1].ToString());
            }
            if (dt5.Rows.Count > 0)
            {
                getschedindex++;
                dtMenu.Rows.Add("  " + dt5.Rows[0].ItemArray[1].ToString());
            }
            if (dt6.Rows.Count > 0)
            {
                getschedindex++;
                dtMenu.Rows.Add("  " + dt6.Rows[0].ItemArray[1].ToString());
            }
            if (dt7.Rows.Count > 0)
            {
                getschedindex++;
                dtMenu.Rows.Add("  " + dt7.Rows[0].ItemArray[1].ToString());
            }
            if (dt8.Rows.Count > 0)
            {
                getschedindex++;
                dtMenu.Rows.Add("  " + dt8.Rows[0].ItemArray[1].ToString());
            }
            if (dt9.Rows.Count > 0)
            {
                getschedindex++;
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
            dgvm.Rows[getschedindex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        public void setupviewbysection(string id)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select subject as 'Subject',faculty as 'Faculty',room as 'Room',start as 'Time start',end as 'Time end',days as 'Days' from schedule_tbl where secid='"+id+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            string acadcnt = "";
            string nonacad = "";
            OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where secid='" + id + "'and type='Academic'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            OdbcDataAdapter da2 = new OdbcDataAdapter("Select count(subject) from schedule_tbl where secid='" + id + "'and type='Non-Academic'", con);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            if (dt1.Rows.Count > 0) { acadcnt = dt1.Rows[0].ItemArray[0].ToString(); }
            if (dt2.Rows.Count > 0) { nonacad = dt2.Rows[0].ItemArray[0].ToString(); }

            DataView dvTheSched = new DataView(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                pnldisnotify.Visible = false;
                pnlhead.DataSource = null;
                pnlhead.DataSource = dvTheSched;

                pnlhead.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                pnlhead.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                pnlhead.Columns[0].Width = 150;
                pnlhead.Columns[1].Width = 200;
                pnlhead.Columns[2].Width = 120;
                pnlhead.Columns[3].Width = 85;
                pnlhead.Columns[4].Width = 85;
                pnlhead.Columns[5].Width = 120;
                lblAcdNonCount.Text = acadcnt+ " Academic "+nonacad+" Non-Academic";
            }
            else
            {
                pnlhead.DataSource = null;
                pnldisnotify.Visible = true;
                lbldismemo.Location = new Point(312, 8);
                lbldismemo.Text = "no schedule found...";
                lblAcdNonCount.Text ="0 Academic 0 Non-Academic";
            }
        }

        public void setupallfaculty(string subject)
        {
            string selectedID = "";
            con.Open();
            OdbcDataAdapter da0 = new OdbcDataAdapter("Select id from facultyspecialization_tbl where subject='"+subject+"'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            con.Close();
            if (dt0.Rows.Count > 0)
            {
                selectedID = dt0.Rows[0].ItemArray[0].ToString();
            }

            string classDept = "";
            con.Open();
            OdbcDataAdapter da10 = new OdbcDataAdapter("Select department from level_tbl where level='" + classgrade + "'", con);
            DataTable dt10 = new DataTable();
            da10.Fill(dt10);
            con.Close();
            if (dt10.Rows.Count > 0)
            {
                classDept = dt10.Rows[0].ItemArray[0].ToString();
            }


          
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select (select concat(firstname,' ',middlename,' ',lastname))as 'Faculty' from employees_tbl where position='faculty' and subject LIKE '%"+selectedID+"%' and department='"+classDept+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dvFacs = new DataView(dt);
            con.Close();

            if (cmbOperation.Text == "Class schedule")
            {
                if (dt.Rows.Count > 0)
                {
                    cmbFacs.Items.Clear();
                    for (int x = 0; x < dt.Rows.Count; x++)
                    {
                        cmbFacs.Items.Add(dt.Rows[x].ItemArray[0].ToString());
                    }
                }
            }    
        }

        public void setupallfacultyNames()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select (select concat(firstname,' ',middlename,' ',lastname))as 'Faculty' from employees_tbl where position='faculty'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dvFacs = new DataView(dt);
            con.Close();

            if (cmbOperation.Text == "Faculty schedule")
            {
                if (dt.Rows.Count > 0)
                {
                    pnlfacnotif.Visible = false;
                    dgvSearch.DataSource = null;
                    dgvSearch.DataSource = dvFacs;

                    dgvSearch.Columns[0].Width = 260;
                }
                else
                {
                    dgvSearch.DataSource = null;
                    pnlfacnotif.Visible = true;
                    lblfacmemo.Location = new Point(312, 8);
                    lblfacmemo.Text = "no schedule found...";
                }

                lblResult.Text = "no. of faculty: " + dgvSearch.Rows.Count;
                lblfaccount.Text = "no. of class schedule: " + dgvFac.Rows.Count;
            }
        }

        public void setuprooms(string grade, string section)
        {
            con.Open();
            OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from roomallocation_tbl where grade='"+grade+"'and section='"+section+"'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);
            con.Close();

            if (dt0.Rows.Count > 0)
            {
                string id = dt0.Rows[0].ItemArray[0].ToString();
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select name from room_tbl where id='" + id + "'or type='Computer Laboratory'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    cmbRoom.Items.Clear();
                    for (int x = 0; x < dt.Rows.Count; x++)
                    {
                        cmbRoom.Items.Add(dt.Rows[x].ItemArray[0].ToString());
                    }
                }
                else
                {
                    cmbRoom.Items.Clear();
                    cmbRoom.Items.Add("no room found");
                }

            }
        }

         public void setupview()
         {
             con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id as 'ID',level as 'Level',section as 'Section' from section_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvSec = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlwith.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvSec;
                dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                dgvSearch.Columns[0].Width = -3;
                dgvSearch.Columns[1].Width = 150;
                dgvSearch.Columns[2].Width = 103;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlwith.Visible = true;
                lblmemowith.Location = new Point(312, 8);
                lblmemowith.Text = "no schedule found...";
            }

            lblResult.Text = "number of section: " + dgvSearch.Rows.Count.ToString();
        }

        public void setupfilter(string level,string sec)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id as 'ID',level as 'Level',section as 'Section' from section_tbl where level='" + level + "' and section='" + sec + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvSec = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlwith.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvSec;

                dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSearch.Columns[0].Width = 50;
                dgvSearch.Columns[1].Width = 100;
                dgvSearch.Columns[2].Width = 100;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlwith.Visible = true;
                lblmemowith.Location = new Point(312, 8);
                lblmemowith.Text = "no schedule found...";
            }

            lblResult.Text = "number of section: " + dgvSearch.Rows.Count.ToString();
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

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maine = new frmMaintenance();
            this.Hide();
            maine.adminlog = schedlog;
            maine.VISITED = VISITED;
            maine.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subf = new frmSubject();
            this.Hide();
            subf.wholog = schedlog;
            subf.VISITED = VISITED;
            subf.Show();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection secf = new frmSection();
            this.Hide();
            secf.secwholog = schedlog;
            secf.VISITED = VISITED;
            secf.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roomf = new frmRoom();
            this.Hide();
            roomf.logger = schedlog;
            roomf.VISITED = VISITED;
            roomf.Show();
        }

        private void btnReqs_Click(object sender, EventArgs e)
        {
            frmRequirement reqf = new frmRequirement();
            this.Hide();
            reqf.reqlog = schedlog;
            reqf.VISITED = VISITED;
            reqf.Show();
        }

        private void btnFees_Click(object sender, EventArgs e)
        {
            frmFee feef = new frmFee();
            this.Hide();
            feef.feelog =schedlog;
            feef.VISITED = VISITED;
            feef.Show();
        }

        private void btnActs_Click(object sender, EventArgs e)
        {
            frmActivity actf = new frmActivity();
            this.Hide();
            actf.actlog = schedlog;
            actf.Show();
        }

        private void btnAudits_Click(object sender, EventArgs e)
        {
            frmAudit audf = new frmAudit();
            this.Hide();
            audf.auditlogger =schedlog;
            audf.Show();
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abtf = new frmAboutMaintenance();
            this.Hide();
            abtf.amlog =schedlog;
            abtf.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Hide();
            buf.backlog = schedlog;
            buf.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin homef = new frmEmpLogin();
            this.Hide();
            homef.Show();
       }

        private void frmSched_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide(); 
            hf.Show();
        }

        private void cmbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbOperation.Text == "Class schedule")
            {
                pnlclassMain.Visible = true;
                pnlFacMain.Visible = false;
                cmbLevel.Visible = true;
                cmbSec.Visible = true;
                txtSearch.Text = "";
                txtSearch.Visible = false;
                pnlwith.Visible = false;
                panelsrcicon.Visible = false;
                lblSrcStat.Text = "filter by:";
                lblSrcStat.Location = new Point(6, 11);
                cmbLevel.Location = new Point(69, 7);
                cmbSec.Location = new Point(168, 7);
                pnlclassMain.Location = new Point(6, 41);
                setupview();
               // setupallfaculty();
               
            }
            if (cmbOperation.Text == "Faculty schedule")
            {
                pnlclassMain.Visible = false;
                pnlFacMain.Visible = true;
                cmbLevel.Visible = false;
                cmbSec.Visible = false;
                pnlwith.Visible = false;
                lblUserNo.Visible = false;
                lblKey.Visible = false;
                txtSearch.Visible = true;
                panelsrcicon.Visible = true;
                lblSrcStat.Text = "";
                txtSearch.Location = new Point(13, 7);
                panelsrcicon.Location = new Point(201, 5);
                pnlFacMain.Location = new Point(13, 41);
                setupallfacultyNames();

                if(dgvSearch.Rows.Count>0)
                {
                    lblTheFac.Text = cmbFacs.Text;
                    setupSchedOfFaculty(lblTheFac.Text);
                }
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
                cmbSec.Items.Clear();
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbSec.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                }
            }
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
          if(cmbLevel.Text=="Grade")
          {
              cmbSec.Enabled = true;
              setupLevelList();
          }
          if (cmbLevel.Text == "Section")
          {
            cmbSec.Enabled = true;
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select distinct section from section_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbSec.Items.Clear();
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    cmbSec.Items.Add(dt.Rows[x].ItemArray[0].ToString());
                }
            }
          }
          if (cmbLevel.Text == "Show all")
          {
              cmbSec.Items.Clear();
              cmbSec.Enabled = false;
              setupview();
          }
           
        }

        private void cmbSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLevel.Text == "Grade")
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select id,level as 'Level',section as 'Section' from section_tbl where level='" + cmbSec.Text + "'", con);
                DataTable dt = new DataTable();
                DataView dvtemp = new DataView(dt);
                
                da.Fill(dt);
                con.Close();

                //dvSec.RowFilter = string.Format("Level = '%{0}%'", cmbSec.Text);
                dgvSearch.DataSource = dvtemp;
                setuppersubject(cmbSec.Text);
                //btnAdd.Enabled = false;
                btnAdd.Text = "Add";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;


                if (dt.Rows.Count > 0)
                {
                    pnlwith.Visible = false;
                    dgvSearch.DataSource = null;
                    dgvSearch.DataSource = dvtemp;
                    dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                    dgvSearch.Columns[0].Width = -3;
                    dgvSearch.Columns[1].Width = 150;
                    dgvSearch.Columns[2].Width = 103;
                }
                else
                {
                    dgvSearch.DataSource = null;
                    pnlwith.Visible = true;
                    lblmemowith.Location = new Point(312, 8);
                    lblmemowith.Text = "no schedule found...";
                }

                lblResult.Text = "number of section: " + dgvSearch.Rows.Count.ToString();
            }
            if (cmbLevel.Text == "Section")
            {
                dvSec.RowFilter = string.Format("Section LIKE '%{0}%'", cmbSec.Text);
                dgvSearch.DataSource = dvSec;
                setuppersubject(cmbSec.Text);
                //btnAdd.Enabled = false;
                btnAdd.Text = "Add";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                if (dgvSearch.Rows.Count > 0)
                {
                    pnlwith.Visible = false;
                    dgvSearch.DataSource = null;
                    dgvSearch.DataSource = dvSec;

                    
                    dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                    dgvSearch.Columns[0].Width = -3;
                    dgvSearch.Columns[1].Width = 150;
                    dgvSearch.Columns[2].Width = 103;
                }
                else
                {
                    dgvSearch.DataSource = null;
                    pnlwith.Visible = true;
                    lblmemowith.Location = new Point(312, 8);
                    lblmemowith.Text = "no schedule found...";
                }

                lblResult.Text = "number of section: " + dgvSearch.Rows.Count.ToString();
            }
        }

        public void setuppersubject(string levs)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select subject from subject_tbl where level='" + levs + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbsub.Items.Clear();
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    cmbsub.Items.Add(dt.Rows[x].ItemArray[0].ToString());
                }
                cmbsub.Items.Add("Recess");
                cmbsub.Items.Add("Lunch break");
                cmbsub.Items.Add("Flag cer./Class prep.");
                cmbsub.Items.Add("Club");
                cmbsub.Items.Add("Homeroom");
            }
            else 
            {
                cmbsub.Items.Clear();
                cmbsub.Items.Add("no subject found");
            }

        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (cmbOperation.Text == "Class schedule")
            {
                setupclear();
                setupenableinput();
                if (dgvSearch.Rows.Count <= 0)
                {
                    return;
                }
                string secid = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                lblKey.Text = secid;
                classgrade = dgvSearch.SelectedRows[0].Cells[1].Value.ToString();
                classsec = dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
                lblSelected.Text = dgvSearch.SelectedRows[0].Cells[1].Value.ToString() + " - " + dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
                setupviewbysection(secid);
                setuprooms(classgrade,classsec);
               
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                if (cmbLevel.Text == "Grade")
                {
                    setuppersubject(cmbSec.Text);
                }
                else
                {
                    setuppersubject(classgrade);
                }
               
            }
            else
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    return;
                }
                lblTheFac.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                setupSchedOfFaculty(lblTheFac.Text);
            }
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
                pnlfacnotif.Visible = false;
                dgvFac.DataSource = null;
                dgvFac.DataSource = dvFacSched;
                dgvFac.Columns[0].Width = 150;
                dgvFac.Columns[1].Width = 85;
                dgvFac.Columns[2].Width = 85;
                dgvFac.Columns[3].Width = 88;
                dgvFac.Columns[4].Width = 88;
                dgvFac.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFac.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFac.Columns[5].Width = 150;
                dgvFac.Columns[6].Width = 100;
               
            }
            else
            {
                dgvFac.DataSource = null;
                pnlfacnotif.Visible = true;
                lblfacmemo.Location = new Point(312, 8);
                lblfacmemo.Text = "no schedule found...";
            }

            lblfaccount.Text = "no. of class schedule: " + dgvFac.Rows.Count.ToString();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Clear")
            {
                setupclear();
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnAdd.Text = "Add";
                btnUpdate.Text = "Update";
             
            }
            else
            {
                //btnDelete.Enabled = true;
                btnAdd.Text = "Add";
                btnClear.Text = "Clear";
                btnUpdate.Text = "Update";
                setupclear();
                primarykey = lblKey.Text;
                setupretrieveddata(primarykey);

                setupdisableinput();
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }
            if (pnlhead.Rows.Count >= 1)
            {
                pnlhead.Rows[0].Selected = true;
            }
        }

        public void setupretrieveddata(string key)
        {
            string selectedsub = "";
            string selectfac = "";
            string selectroom = "";
            con.Open();
            OdbcDataAdapter daa = new OdbcDataAdapter("Select*from schedule_tbl where secid='" + key + "'and subject='" + tempsubj + "'", con);
            DataTable dtt = new DataTable();
            daa.Fill(dtt);
            con.Close();

            if (dtt.Rows.Count > 0)
            {
                
                selectedsub = dtt.Rows[0].ItemArray[3].ToString();
                selectfac = dtt.Rows[0].ItemArray[4].ToString();
                selectroom = dtt.Rows[0].ItemArray[5].ToString();
                dudHrStart.Text = dtt.Rows[0].ItemArray[6].ToString().Substring(0, 2);
                dudMinStart.Text = dtt.Rows[0].ItemArray[6].ToString().Substring(3, 2);
                cmbDayStart.Text = dtt.Rows[0].ItemArray[6].ToString().Substring(6, 2);
                dudHourEnd.Text = dtt.Rows[0].ItemArray[7].ToString().Substring(0, 2);
                dudMinEnd.Text = dtt.Rows[0].ItemArray[7].ToString().Substring(3, 2);
                cmbDayEnd.Text = dtt.Rows[0].ItemArray[7].ToString().Substring(6, 2);
               

                setupdisplaydayofsubj();
            }

            if (cmbsub.Text == "Music" || cmbsub.Text == "Arts" || cmbsub.Text == "P.E." || cmbsub.Text == "Health")
            {
                setupallfaculty("M.A.P.E.H.");
                cmbFacs.Enabled = false;
            }
            else if (cmbsub.Text == "Reading" || cmbsub.Text == "Writing" || cmbsub.Text == "Language")
            {
                setupallfaculty("English");
                cmbFacs.Enabled = false;
            }
            else
            {
                setupallfaculty(cmbsub.Text);
                cmbFacs.Enabled = false;
            }

            cmbsub.Text = selectedsub;
            cmbFacs.Text = selectfac;
            cmbRoom.Text = selectroom;
            setupdisableinput();
        }

        public void setupclear()
        {
            cmbsub.SelectedIndex = -1;
            cmbFacs.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
            cmbday1.SelectedIndex = -1;
            cmbday2.SelectedIndex = -1;
            cmbDayStart.SelectedIndex = -1;
            cmbDayEnd.SelectedIndex = -1;
            dudHrStart.Text = "";
            dudMinStart.Text = "";
            dudHourEnd.Text = "";
            dudMinEnd.Text = "";
           


            setupenableinput();
        }

        public void setupdisableinput()
        {
            cmbsub.Enabled = false;
            cmbRoom.Enabled = false;
            cmbday1.Enabled = false;
            cmbday2.Enabled = false;
            cmbDayStart.Enabled = false;
            cmbDayEnd.Enabled = false;
            dudHrStart.Enabled = false;
            dudMinStart.Enabled = false;
            dudHourEnd.Enabled = false;
            dudMinEnd.Enabled = false;
            cmbFacs.Enabled = false;
        }

        public void setupenableinput()
        {
            cmbsub.Enabled = true;
            cmbFacs.Enabled = true;
            cmbRoom.Enabled = true;
            cmbday1.Enabled = true;
            cmbday2.Enabled = true;
            cmbDayStart.Enabled = true;
            cmbDayEnd.Enabled = true;
            dudHrStart.Enabled = true;
            dudMinStart.Enabled = true;
            dudHourEnd.Enabled = true;
            dudMinEnd.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dudHrStart.Text == "" || dudMinStart.Text == "" || dudHourEnd.Text == "" || dudMinEnd.Text == "" || cmbsub.Text == "" ||
                cmbDayStart.Text == "" || cmbDayEnd.Text == "" || ((cmbday1.Text == "" || cmbday2.Text == "") && (cmbsub.Text != "Recess" && cmbsub.Text != "Lunch break")))
            {
                MessageBox.Show("some field missing", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string start = dudHrStart.Text + ":" + dudMinStart.Text + " " + cmbDayStart.Text;
            string end = dudHourEnd.Text + ":" + dudMinEnd.Text + " " + cmbDayEnd.Text;
            float fstart = Convert.ToInt32(dudHrStart.Text + "" + dudMinStart.Text);
            float fend = Convert.ToInt32(dudHourEnd.Text + "" + dudMinEnd.Text);
            DateTime startTime = Convert.ToDateTime(start);
            DateTime endTime = Convert.ToDateTime(end);
                       

            if (lblKey.Text == "")
            {
                MessageBox.Show("no class selected!", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbsub.Text == "" || cmbFacs.Text == "" || cmbRoom.Text == "" || cmbday1.Text == "" || cmbday2.Text == "" ||
                cmbDayStart.Text == "" || cmbDayEnd.Text == "" || dudHrStart.Text == "" || dudMinStart.Text == "" || dudHourEnd.Text == "" || dudMinEnd.Text == "")
            {
                if (cmbsub.Text == "Recess" || cmbsub.Text == "Lunch break" || cmbsub.Text == "Flag cer./Class prep." || cmbsub.Text == "Club" || cmbsub.Text == "Homeroom")
                {
                    if (cmbDayStart.Text == "" || cmbDayEnd.Text == "" || dudHrStart.Text == "" || dudMinStart.Text == "" || dudHourEnd.Text == "" || dudMinEnd.Text == "")
                    {
                        MessageBox.Show("Time not set.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        //condition for those subject that dont need to fill out all fields.

                        setupdayofsubj();
                        TimeSpan span = endTime.Subtract(startTime);

                        //1-----------------------------------
                        if (((endTime.Hour == startTime.Hour) && (endTime.Minute == 0 && startTime.Minute == 0)) || endTime.Hour < startTime.Hour)
                        {
                            MessageBox.Show("Invalid time schedule", "Scheduling", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //2-----------------------------------span of subjs/acts
                        if (((Convert.ToDouble(span.Hours.ToString()) > 1) || ((Convert.ToDouble(span.Hours.ToString()) >= 1) && (Convert.ToDouble(span.Minutes.ToString()) >= 1))) && (cmbsub.Text != "Recess" || cmbsub.Text != "Lunch break" || cmbsub.Text != "Flag cer./Class prep." || cmbsub.Text != "Homeroom"))
                        {
                            MessageBox.Show("Subject should not exceed 1 hour", "Scheduling", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //3-----------------------------------checks for duplicate entry
                        con.Open();
                        OdbcDataAdapter dac3 = new OdbcDataAdapter("Select*from schedule_tbl where subject='" + cmbsub.Text + "'and secid='" + lblKey.Text + "'", con);
                        DataTable dtc3 = new DataTable();
                        dac3.Fill(dtc3);
                        con.Close();
                        if (dtc3.Rows.Count > 0)
                        {
                            MessageBox.Show("Subject/Activity already added", "Scheduling", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //4-----------------------------------checks if theres already acts in the class, same hour and same day
                        con.Open();
                        OdbcDataAdapter dac11 = new OdbcDataAdapter("Select*from schedule_tbl where start='" + start + "'and section='" + classsec + "' and level='" + classgrade + "'", con);
                        DataTable dtc11 = new DataTable();
                        dac11.Fill(dtc11);
                        con.Close();
                        if (dtc11.Rows.Count > 0)
                        {
                            string dbday = dtc11.Rows[0].ItemArray[8].ToString();
                            string dbstarttime = dtc11.Rows[0].ItemArray[6].ToString();
                           
                            if ((day.Contains("Mon") == true && dbday.Contains("Mon") == true && start == dbstarttime) || (day.Contains("Tue") == true && dbday.Contains("Tue") == true && start == dbstarttime) || (day.Contains("Wed") == true && dbday.Contains("Wed") == true && start == dbstarttime) || (day.Contains("Thu") == true && dbday.Contains("Thu") == true && start == dbstarttime) || (day.Contains("Fri") == true && dbday.Contains("Fri") == true && start == dbstarttime) || (day.Contains("Sat") == true && dbday.Contains("Sat") == true && start == dbstarttime))
                            {
                                MessageBox.Show("Conflict schedule in section " + classsec, "Scheduling13", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        //5-----------------------------------checks all the sched of the class if theres overlapp 
                        con.Open();
                        OdbcDataAdapter dac = new OdbcDataAdapter("Select*from schedule_tbl where secid='" + lblKey.Text + "'", con);
                        DataTable dtc = new DataTable();
                        dac.Fill(dtc);
                        con.Close();
                        if (dtc.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtc.Rows.Count; i++)
                            {
                                float dbstart = Convert.ToSingle(dtc.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                                float dbend = Convert.ToSingle(dtc.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                                string unitdaystart = dtc.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                                string unitdayend = dtc.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                                string daysofsched = dtc.Rows[i].ItemArray[8].ToString();
                                TimeSpan spn = endTime.Subtract(startTime);
                                if ((fstart >= dbstart) && (fend <= dbend) && (spn.Hours <= 0) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && ((daysofsched.Contains(day1code) == true) || (daysofsched.Contains(day2code) == true)))
                                {
                                    MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling41", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        //6--------------------------------Checks for all sched
                        con.Open();
                        OdbcDataAdapter dac1 = new OdbcDataAdapter("Select*from schedule_tbl where level='" + classgrade + "'and section='" + classsec + "'and days LIKE'" + cmbDayStart + "'", con);
                        DataTable dtc1 = new DataTable();
                        dac1.Fill(dtc);
                        con.Close();
                        if (dtc1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtc1.Rows.Count; i++)
                            {
                                DateTime dateTime = DateTime.ParseExact(dtc.Rows[i].ItemArray[6].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                                float dbstart = Convert.ToSingle(dtc.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                                float dbend = Convert.ToSingle(dtc.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                                string dbday = dtc1.Rows[i].ItemArray[8].ToString();
                                if ((dbend == fend && dbstart == fstart) || (dbstart == fstart && fend >= dbend) || (dbend == fend && fstart <= dbstart) || ((dbday.Contains(day1code) == true)|| (dbday.Contains(day2code) == true)))
                                {
                                    MessageBox.Show("Conflict schedule in section " + classsec, "Scheduling42", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        //7------------------------------------span time error
                        con.Open();
                        OdbcDataAdapter daxx1 = new OdbcDataAdapter("Select*from schedule_tbl where section='" + classsec + "'and level='" + classgrade + "'", con);
                        DataTable dtxx1 = new DataTable();
                        daxx1.Fill(dtxx1);
                        con.Close();
                        if (dtxx1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtxx1.Rows.Count; i++)
                            {
                                string dbhourstart = dtxx1.Rows[i].ItemArray[6].ToString().Substring(0, 2);
                                string dbhourend = dtxx1.Rows[i].ItemArray[7].ToString().Substring(0, 2);
                                int dudminstart = Convert.ToInt32(dudMinStart.Text);
                                float dbstart = Convert.ToSingle(dtxx1.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtxx1.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                                float dbend = Convert.ToSingle(dtxx1.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtxx1.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                                int dbendmin = Convert.ToInt32(dtxx1.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                                string dbday = dtxx1.Rows[i].ItemArray[8].ToString();
                                string unitdaystart = dtxx1.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                                string unitdayend = dtxx1.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                                if (dbhourstart == dudHrStart.Text)
                                {
                                    if ((dbendmin > dudminstart) && (dbend < fend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                                    {
                                        if ((dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                                        {
                                            MessageBox.Show("Conflict schedule in " + classsec, "Scheduling51", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Conflict schedule in " + classsec, "Scheduling52", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }
                                    }
                                    if ((dudminstart > dbendmin) && (fend < dbend) && (fstart < dbstart) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                                    {
                                        if ((dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                                        {
                                            MessageBox.Show("Conflict schedule in " + classsec, "Scheduling54", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Conflict schedule in " + classsec, "Scheduling55", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }
                                    }

                                }
                            }
                        }
                        //---------------------------------------
                        con.Open();
                        OdbcDataAdapter dac33 = new OdbcDataAdapter("Select*from schedule_tbl where secid='" + lblKey.Text + "'", con);
                        DataTable dtc33 = new DataTable();
                        dac33.Fill(dtc33);
                        con.Close();
                        if (dtc33.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtc33.Rows.Count; i++)
                            {
                                float dbstart = Convert.ToSingle(dtc33.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc33.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                                float dbend = Convert.ToSingle(dtc33.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc33.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                                string dbday = dtc33.Rows[i].ItemArray[8].ToString();
                                string unitdaystart = dtc33.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                                string unitdayend = dtc33.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                                DateTime dbstartTime = Convert.ToDateTime(dtc33.Rows[i].ItemArray[6].ToString());
                                DateTime dbendTime = Convert.ToDateTime(dtc33.Rows[i].ItemArray[7].ToString());
                                string daysofsched = dtc33.Rows[i].ItemArray[8].ToString();
                                TimeSpan spanClassStart = dbstartTime.Subtract(startTime);
                                TimeSpan spanClassEnd = dbendTime.Subtract(endTime);
                                if ((spanClassStart.Hours == 0) && (spanClassEnd.Hours == 0) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (daysofsched == day))
                                {
                                    MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling11", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        //8--------------------------------------span of the middle of days
                        con.Open();
                        OdbcDataAdapter dacs = new OdbcDataAdapter("Select*from schedule_tbl where secid='" + lblKey.Text + "'", con);
                        DataTable dtcs = new DataTable();
                        dacs.Fill(dtcs);
                        con.Close();
                        if (dtcs.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtcs.Rows.Count; i++)
                            {
                                float dbstart = Convert.ToSingle(dtcs.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtcs.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                                float dbend = Convert.ToSingle(dtcs.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtcs.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                                string unitdaystart = dtcs.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                                string unitdayend = dtcs.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                                string daysofsched = dtcs.Rows[i].ItemArray[8].ToString();
                                TimeSpan spn = endTime.Subtract(startTime);

                                string day1codedb = "";
                                string day2codedb = "";
                                int MINDAY_DB = 0;
                                int MAXDAY_DB = 0;

                                if (daysofsched.Length > 3)
                                {
                                    day1codedb = dtcs.Rows[i].ItemArray[8].ToString().Substring(0, 3);
                                    day2codedb = dtcs.Rows[i].ItemArray[8].ToString().Substring(4, 3);
                                }
                               
                                if (day1codedb.Contains("Mon"))
                                {
                                    MINDAY_DB = 1;
                                }
                                if (day1codedb.Contains("Tue"))
                                {
                                    MINDAY_DB = 2;
                                }
                                if (day1codedb.Contains("Wed"))
                                {
                                    MINDAY_DB = 3;
                                }
                                if (day1codedb.Contains("Thu"))
                                {
                                    MINDAY_DB = 4;
                                }
                                if (day1codedb.Contains("Fri"))
                                {
                                    MINDAY_DB = 5;
                                }
                                if (day1codedb.Contains("Sat"))
                                {
                                    MINDAY_DB = 6;
                                }
                               
                                //----
                                if (day2codedb.Contains("Mon"))
                                {
                                    MAXDAY_DB = 1;
                                }
                                if (day2codedb.Contains("Tue"))
                                {
                                    MAXDAY_DB = 2;
                                }
                                if (day2codedb.Contains("Wed"))
                                {
                                    MAXDAY_DB = 3;
                                }
                                if (day2codedb.Contains("Thu"))
                                {
                                    MAXDAY_DB = 4;
                                }
                                if (day2codedb.Contains("Fri"))
                                {
                                    MAXDAY_DB = 5;
                                }
                                if (day2codedb.Contains("Sat"))
                                {
                                    MAXDAY_DB = 6;
                                }

                                if ((fstart >= dbstart) && (fend < dbend) && (spn.Hours <= 0) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB<MINDAY&&MAXDAY<MAXDAY_DB))
                                {
                                    MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling31", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                  
                                    return;
                                }
                                if ((fstart >= dbstart) && (fend > dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB < MINDAY && MAXDAY < MAXDAY_DB))
                                {
                                    MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling35", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                if ((fstart >= dbstart) && (fend <= dbend) && (spn.Hours <= 0) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB > MINDAY && MAXDAY > MAXDAY_DB))
                                {
                                    MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling31.1", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return;
                                }
                                if ((fstart >= dbstart) && (fend > dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB > MINDAY && MAXDAY > MAXDAY_DB))
                                {
                                    MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling35.1", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                //if (MINDAY_DB < MINDAY && MAXDAY < MAXDAY_DB)
                                //{
                                //    MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    return;
                                //}
                            }
                        }
                        //---------------------------------------


                        setupAddForClass();
                    }
                }
                else
                {
                    MessageBox.Show("fill out all fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                
                setupdayofsubj();
                TimeSpan span = endTime.Subtract ( startTime );

                //CONDITION 1---------------------------------
                if (((endTime.Hour == startTime.Hour) && (endTime.Minute ==0 && startTime.Minute ==0)) || endTime.Hour < startTime.Hour)
                {
                    MessageBox.Show("Invalid time schedule", "Scheduling", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //CONDITION 2---------------------------------required time span of subjs.
                if ((span.Hours == 0) || (endTime.Hour == 0) || (endTime.Hour == startTime.Hour && (endTime.Minute > 0 || startTime.Minute > 0)))
                {
                    MessageBox.Show("Subject should not less 1 hour", "Scheduling", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //CONDITION 3---------------------------------required time span of subjs.
                if (((Convert.ToDouble(span.Hours.ToString()) > 1) || ((Convert.ToDouble(span.Hours.ToString()) >= 1) && (Convert.ToDouble(span.Minutes.ToString()) >= 1))) && (cmbsub.Text != "Recess" || cmbsub.Text != "Lunch break" || cmbsub.Text != "Flag cer./Class prep." || cmbsub.Text != "Homeroom"))
                {
                    MessageBox.Show("Subject should not exceed 1 hour", "Scheduling", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //CONDITION 4---------------------------------checks duplicate entry
                con.Open();
                OdbcDataAdapter dac3 = new OdbcDataAdapter("Select*from schedule_tbl where subject='" + cmbsub.Text + "'and secid='" + lblKey.Text + "'", con);
                DataTable dtc3 = new DataTable();
                dac3.Fill(dtc3);
                con.Close();
                if (dtc3.Rows.Count > 0)
                {
                    MessageBox.Show("Subject/Activity already added", "Scheduling", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //-----------------------------------
                //this will check the start hour
                con.Open();
                //OdbcDataAdapter daxxx1 = new OdbcDataAdapter("Select*from schedule_tbl where section='" + classsec + "'and level='" + classgrade + "'", con);
                string query = @"
                   SELECT *
                   FROM schedule_tbl AS tb
                   WHERE tb.level = '{0}'
                   AND tb.section = '{1}'
                   AND (
                      STR_TO_DATE(tb.start, '%h:%i %p') >= STR_TO_DATE('{2}', '%h:%i %p')
                      OR
                      STR_TO_DATE(tb.end, '%h:%i %p') <= STR_TO_DATE('{3}', '%h:%i %p')
                   )
                   AND (tb.days LIKE '{4}-%' OR tb.days LIKE '%-{5}');
                ";
                string formattedQuery = string.Format(query, classgrade, classsec, start, end, day1code, day2code);
                OdbcDataAdapter daxxx1 = new OdbcDataAdapter(formattedQuery, con);
                DataTable dtxxx1 = new DataTable();
                daxxx1.Fill(dtxxx1);
                con.Close();
                if (dtxxx1.Rows.Count > 0)
                {
                    MessageBox.Show("Conflict schedule in " + classsec, "Schedule maintenance103", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                    //for (int i = 0; i < dtxxx1.Rows.Count; i++)
                    //{
                    //    string dbhourstart = dtxxx1.Rows[i].ItemArray[6].ToString().Substring(0, 2);//2 DIGIT HR.
                    //    string dbminstart = dtxxx1.Rows[i].ItemArray[6].ToString().Substring(3, 2);//2 DIGIT min.
                    //    string unitdaystart = dtxxx1.Rows[i].ItemArray[6].ToString().Substring(6, 2);//AM OR PM
                    //    string dbday = dtxxx1.Rows[i].ItemArray[8].ToString();
                    //    if ((dbhourstart == dudHrStart.Text) && (dbminstart == dudMinStart.Text) && (unitdaystart == cmbDayStart.Text)&&(dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                    //    {
                    //        MessageBox.Show("Conflict schedule in " + classsec, "Schedule maintenance103", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //        return;
                    //    }
                    //}
                }
                //this wil check the class end hour
                con.Open();
                OdbcDataAdapter daend = new OdbcDataAdapter("Select*from schedule_tbl where section='" + classsec + "'and level='" + classgrade + "'", con);
                DataTable dtend = new DataTable();
                daend.Fill(dtend);
                con.Close();
                if (dtend.Rows.Count > 0)
                {
                    for (int i = 0; i < dtend.Rows.Count; i++)
                    {
                        string dbhourend = dtend.Rows[i].ItemArray[7].ToString().Substring(0, 2);//2 DIGIT HR.
                        int dbhrend = Convert.ToInt32(dbhourend);
                        int hrstart = Convert.ToInt32(dudHrStart.Text);
                        int hrends = Convert.ToInt32(dudHourEnd.Text);
                        string dbminend = dtend.Rows[i].ItemArray[7].ToString().Substring(3, 2);//2 DIGIT min.
                        string unitdayend = dtend.Rows[i].ItemArray[7].ToString().Substring(6, 2);//AM OR PM
                        string dbday = dtend.Rows[i].ItemArray[8].ToString();
                        DateTime dbtimeend = Convert.ToDateTime(dtend.Rows[i].ItemArray[7].ToString());
                        TimeSpan spans = endTime.Subtract(dbtimeend);
                        string daysofsched = dtend.Rows[i].ItemArray[8].ToString();
                        string day1codedb = "";
                        string day2codedb = "";
                        int MINDAY_DB = 0;
                        int MAXDAY_DB = 0;

                        if (daysofsched.Length > 3)
                        {
                            day1codedb = dtend.Rows[i].ItemArray[8].ToString().Substring(0, 3);
                            day2codedb = dtend.Rows[i].ItemArray[8].ToString().Substring(4, 3);
                        }
                        else
                        {
                            day1codedb = dtend.Rows[i].ItemArray[8].ToString().Substring(0, 3);
                            day2codedb = day1codedb;
                        }

                        if (day1codedb.Contains("Mon"))
                        {
                            MINDAY_DB = 1;
                        }
                        if (day1codedb.Contains("Tue"))
                        {
                            MINDAY_DB = 2;
                        }
                        if (day1codedb.Contains("Wed"))
                        {
                            MINDAY_DB = 3;
                        }
                        if (day1codedb.Contains("Thu"))
                        {
                            MINDAY_DB = 4;
                        }
                        if (day1codedb.Contains("Fri"))
                        {
                            MINDAY_DB = 5;
                        }
                        if (day1codedb.Contains("Sat"))
                        {
                            MINDAY_DB = 6;
                        }

                        //----
                        if (day2codedb.Contains("Mon"))
                        {
                            MAXDAY_DB = 1;
                        }
                        if (day2codedb.Contains("Tue"))
                        {
                            MAXDAY_DB = 2;
                        }
                        if (day2codedb.Contains("Wed"))
                        {
                            MAXDAY_DB = 3;
                        }
                        if (day2codedb.Contains("Thu"))
                        {
                            MAXDAY_DB = 4;
                        }
                        if (day2codedb.Contains("Fri"))
                        {
                            MAXDAY_DB = 5;
                        }
                        if (day2codedb.Contains("Sat"))
                        {
                            MAXDAY_DB = 6;
                        }

                        if ((hrstart <dbhrend) && (spans.Hours == 0) && (unitdayend == cmbDayStart.Text) && (MINDAY_DB > MINDAY && MAXDAY <= MAXDAY_DB &&  daysofsched.Length <=3))
                        {
                            MessageBox.Show("Conflict schedule in " + classsec, "Schedule maintenance1031", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if ((hrstart <dbhrend) && (spans.Hours == 0) && (unitdayend == cmbDayStart.Text) && (MINDAY_DB < MINDAY && MAXDAY < MAXDAY_DB))
                        {
                            MessageBox.Show("Conflict schedule in " + classsec, "Schedule maintenance1032", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                //-----------------------------------
                //LAST ERROR
                con.Open();
                OdbcDataAdapter daxx1 = new OdbcDataAdapter("Select*from schedule_tbl where section='" + classsec + "'and level='" + classgrade + "'", con);
                DataTable dtxx1 = new DataTable();
                daxx1.Fill(dtxx1);
                con.Close();
                if (dtxx1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtxx1.Rows.Count; i++)
                    {
                        string dbhourstart = dtxx1.Rows[i].ItemArray[6].ToString().Substring(0, 2);
                        string dbhourend = dtxx1.Rows[i].ItemArray[7].ToString().Substring(0, 2);
                        int dudminstart = Convert.ToInt32(dudMinStart.Text);
                        float dbstart = Convert.ToSingle(dtxx1.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtxx1.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtxx1.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtxx1.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        int dbendmin = Convert.ToInt32(dtxx1.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtxx1.Rows[i].ItemArray[8].ToString();
                        string unitdaystart = dtxx1.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                        string unitdayend = dtxx1.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                        if (dbhourstart == dudHrStart.Text)
                        {
                            if ((dbendmin > dudminstart) && (dbend < fend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                            {
                                if ((dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                                {
                                    MessageBox.Show("Conflict schedule in " + classsec, "Schedule maintenance103", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Conflict schedule in " + classsec, "Schedule maintenance103", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            if ((dudminstart > dbendmin) && (fend < dbend) && (fstart < dbstart) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                            {
                                if ((dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                                {
                                    MessageBox.Show("Conflict schedule in " + classsec, "Schedule maintenance103", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Conflict schedule in " + classsec, "Schedule maintenance103", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }

                        }
                    }
                }
                //========================

                //this will check the end hour
                con.Open();
                OdbcDataAdapter daxx2 = new OdbcDataAdapter("Select*from schedule_tbl where section='" + classsec + "'and level='" + classgrade + "'", con);
                DataTable dtxx2 = new DataTable();
                daxx2.Fill(dtxx2);
                con.Close();
                if (dtxx2.Rows.Count > 0)
                {
                    for (int i = 0; i < dtxx2.Rows.Count; i++)
                    {
                        string dbhourend = dtxx2.Rows[i].ItemArray[7].ToString().Substring(0, 2);//2 DIGIT HR.
                        string dbminend = dtxx2.Rows[i].ItemArray[7].ToString().Substring(3, 2);//2 DIGIT HR.
                        string unitdayend = dtxx2.Rows[i].ItemArray[7].ToString().Substring(6, 2);//AM OR PM
                        string dbday = dtxx2.Rows[i].ItemArray[8].ToString();
                        if ((dbhourend == dudHourEnd.Text) && (dbminend == dudMinEnd.Text) && (unitdayend == cmbDayEnd.Text) && (dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                        {
                            MessageBox.Show("Conflict schedule in " + classsec, "Schedule maintenance104", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                con.Open();
                OdbcDataAdapter dac = new OdbcDataAdapter("Select*from facultySched_tbl where faculty='" + cmbFacs.Text + "'and section='"+classsec+"'and level='"+classgrade+"'and days='"+cmbDayStart+"'", con);
                DataTable dtc = new DataTable();
                dac.Fill(dtc);
                con.Close();
                if (dtc.Rows.Count > 0)
                {
                   
                    //string to float
                    for (int i = 0; i < dtc.Rows.Count; i++)
                    {
                        float dbstart = Convert.ToSingle(dtc.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtc.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtc.Rows[i].ItemArray[8].ToString();
                        if ((dbend == fend && dbstart == fstart) || (dbstart == fstart && fend >= dbend) || (dbend == fend && fstart <= dbstart) || (dbday.Contains(day) == true))
                        {
                            MessageBox.Show("Conflict schedule for " + cmbFacs.Text+" in section "+classsec, "Scheduling4", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                // 08:00 AM - 09:00 AM THEN 08:30 AM - 09:00 AM INSTANCE
                con.Open();
                OdbcDataAdapter dac34 = new OdbcDataAdapter("Select*from schedule_tbl where secid='" + lblKey.Text + "'", con);
                DataTable dtc34 = new DataTable();
                dac34.Fill(dtc34);
                con.Close();
                if (dtc34.Rows.Count > 0)
                {
                    for (int i = 0; i < dtc34.Rows.Count; i++)
                    {
                        float dbstart = Convert.ToSingle(dtc34.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc34.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtc34.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc34.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtc34.Rows[i].ItemArray[8].ToString();
                        string unitdaystart = dtc34.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                        string unitdayend = dtc34.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                        string daysofsched = dtc34.Rows[i].ItemArray[8].ToString();
                        TimeSpan spn = endTime.Subtract(startTime);
                        
                        if ((fstart >= dbstart) && (fend <= dbend) && (spn.Hours <= 0) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (daysofsched == day))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling5", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                con.Open();
                OdbcDataAdapter dac33 = new OdbcDataAdapter("Select*from schedule_tbl where secid='" + lblKey.Text + "'", con);
                DataTable dtc33 = new DataTable();
                dac33.Fill(dtc33);
                con.Close();
                if (dtc33.Rows.Count > 0)
                {
                    for (int i = 0; i < dtc33.Rows.Count; i++)
                    {
                        float dbstart = Convert.ToSingle(dtc33.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc33.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtc33.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc33.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtc33.Rows[i].ItemArray[8].ToString();
                        string unitdaystart = dtc33.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                        string unitdayend = dtc33.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                        DateTime dbstartTime = Convert.ToDateTime(dtc33.Rows[i].ItemArray[6].ToString());
                        DateTime dbendTime = Convert.ToDateTime(dtc33.Rows[i].ItemArray[7].ToString());
                        string daysofsched = dtc33.Rows[i].ItemArray[8].ToString();
                        TimeSpan spanClassStart = dbstartTime.Subtract(startTime);
                        TimeSpan spanClassEnd = dbendTime.Subtract(endTime);
                        if ((spanClassStart.Hours == 0) && (spanClassEnd.Hours == 0) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (daysofsched == day))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling45", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                //LAST ERROR
                con.Open();
                OdbcDataAdapter daxx1x = new OdbcDataAdapter("Select*from schedule_tbl where secid='" + lblKey.Text + "'", con);
                DataTable dtxx1x = new DataTable();
                daxx1x.Fill(dtxx1x);
                con.Close();
                if (dtxx1x.Rows.Count > 0)
                {
                    for (int i = 0; i < dtxx1x.Rows.Count; i++)
                    {
                        string dbhourstart = dtxx1x.Rows[i].ItemArray[6].ToString().Substring(0, 2);
                        string dbhourend = dtxx1x.Rows[i].ItemArray[7].ToString().Substring(0, 2);
                        int dudminstart = Convert.ToInt32(dudMinStart.Text);
                        float dbstart = Convert.ToSingle(dtxx1x.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtxx1x.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtxx1x.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtxx1x.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        int dbendmin = Convert.ToInt32(dtxx1x.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtxx1x.Rows[i].ItemArray[8].ToString();
                        string unitdaystart = dtxx1x.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                        string unitdayend = dtxx1x.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                        if (dbhourend == dudHrStart.Text)
                        {
                            string daysofsched = dtxx1x.Rows[i].ItemArray[8].ToString();
                            string day1codedb = "";
                            string day2codedb = "";
                            int MINDAY_DB = 0;
                            int MAXDAY_DB = 0;

                            if (daysofsched.Length > 3)
                            {
                                day1codedb = dtxx1x.Rows[i].ItemArray[8].ToString().Substring(0, 3);
                                day2codedb = dtxx1x.Rows[i].ItemArray[8].ToString().Substring(4, 3);
                            }
                            else
                            {
                                day1codedb = dtxx1x.Rows[i].ItemArray[8].ToString().Substring(0, 3);
                                day2codedb = day1codedb;
                            }

                            if (day1codedb.Contains("Mon"))
                            {
                                MINDAY_DB = 1;
                            }
                            if (day1codedb.Contains("Tue"))
                            {
                                MINDAY_DB = 2;
                            }
                            if (day1codedb.Contains("Wed"))
                            {
                                MINDAY_DB = 3;
                            }
                            if (day1codedb.Contains("Thu"))
                            {
                                MINDAY_DB = 4;
                            }
                            if (day1codedb.Contains("Fri"))
                            {
                                MINDAY_DB = 5;
                            }
                            if (day1codedb.Contains("Sat"))
                            {
                                MINDAY_DB = 6;
                            }

                            //----
                            if (day2codedb.Contains("Mon"))
                            {
                                MAXDAY_DB = 1;
                            }
                            if (day2codedb.Contains("Tue"))
                            {
                                MAXDAY_DB = 2;
                            }
                            if (day2codedb.Contains("Wed"))
                            {
                                MAXDAY_DB = 3;
                            }
                            if (day2codedb.Contains("Thu"))
                            {
                                MAXDAY_DB = 4;
                            }
                            if (day2codedb.Contains("Fri"))
                            {
                                MAXDAY_DB = 5;
                            }
                            if (day2codedb.Contains("Sat"))
                            {
                                MAXDAY_DB = 6;
                            }

                            if ((dbendmin > dudminstart) && (dbend < fend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB > MINDAY && MAXDAY <= MAXDAY_DB && daysofsched.Length <= 3))// || ((fstart < dbstart) && (fend <= dbend))
                            {
                                if ((dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                                {
                                    MessageBox.Show("Conflict schedule in " + classsec, "Scheduling12.2", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                //else
                                //{
                                //    MessageBox.Show("Conflict schedule in " + classsec, "Scheduling11", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    return;
                                //}
                            }
                            if ((dbendmin > dudminstart) && (dbend < fend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB > MINDAY && MAXDAY <= MAXDAY_DB))// || ((fstart < dbstart) && (fend <= dbend))
                            {
                                if ((dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                                {
                                    MessageBox.Show("Conflict schedule in " + classsec, "Scheduling12", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                //else
                                //{
                                //    MessageBox.Show("Conflict schedule in " + classsec, "Scheduling11", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    return;
                                //}
                            }
                            if ((dudminstart >dbendmin) && (fend < dbend) && (fstart < dbstart) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                            {
                                if ((dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                                {
                                    MessageBox.Show("Conflict schedule in " + classsec, "Scheduling32", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Conflict schedule in " + classsec, "Scheduling23", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                           
                        }
                    }
                }
                //4-----------------------------------checks if theres already acts in the class, same hour and same day
                con.Open();
                OdbcDataAdapter dac11a = new OdbcDataAdapter("Select*from schedule_tbl where start='" + start + "'and section='" + classsec + "' and level='" + classgrade + "'", con);
                DataTable dtc11a = new DataTable();
                dac11a.Fill(dtc11a);
                con.Close();
                if (dtc11a.Rows.Count > 0)
                {
                    string dbday = dtc11a.Rows[0].ItemArray[8].ToString();
                    string dbstarttime = dtc11a.Rows[0].ItemArray[6].ToString();

                    if ((day.Contains("Mon") == true && dbday.Contains("Mon") == true && start == dbstarttime) || (day.Contains("Tue") == true && dbday.Contains("Tue") == true && start == dbstarttime) || (day.Contains("Wed") == true && dbday.Contains("Wed") == true && start == dbstarttime) || (day.Contains("Thu") == true && dbday.Contains("Thu") == true && start == dbstarttime) || (day.Contains("Fri") == true && dbday.Contains("Fri") == true && start == dbstarttime) || (day.Contains("Sat") == true && dbday.Contains("Sat") == true && start == dbstarttime))
                    {
                        MessageBox.Show("Conflict schedule in section " + classsec, "Scheduling45", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                //========================
                con.Open();
                OdbcDataAdapter dacs = new OdbcDataAdapter("Select*from schedule_tbl where secid='" + lblKey.Text + "'", con);
                DataTable dtcs = new DataTable();
                dacs.Fill(dtcs);
                con.Close();
                if (dtcs.Rows.Count > 0)
                {
                    for (int i = 0; i < dtcs.Rows.Count; i++)
                    {
                        float dbstart = Convert.ToSingle(dtcs.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtcs.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtcs.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtcs.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string unitdaystart = dtcs.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                        string unitdayend = dtcs.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                        string daysofsched = dtcs.Rows[i].ItemArray[8].ToString();
                        TimeSpan spn = endTime.Subtract(startTime);

                        string day1codedb = "";
                        string day2codedb = "";
                        int MINDAY_DB = 0;
                        int MAXDAY_DB = 0;

                        if (daysofsched.Length > 3)
                        {
                            day1codedb = dtcs.Rows[i].ItemArray[8].ToString().Substring(0, 3);
                            day2codedb = dtcs.Rows[i].ItemArray[8].ToString().Substring(4, 3);
                        }
                        else
                        {
                            day1codedb = dtcs.Rows[i].ItemArray[8].ToString().Substring(0, 3);
                            day2codedb = day1codedb;
                        }

                        if (day1codedb.Contains("Mon"))
                        {
                            MINDAY_DB = 1;
                        }
                        if (day1codedb.Contains("Tue"))
                        {
                            MINDAY_DB = 2;
                        }
                        if (day1codedb.Contains("Wed"))
                        {
                            MINDAY_DB = 3;
                        }
                        if (day1codedb.Contains("Thu"))
                        {
                            MINDAY_DB = 4;
                        }
                        if (day1codedb.Contains("Fri"))
                        {
                            MINDAY_DB = 5;
                        }
                        if (day1codedb.Contains("Sat"))
                        {
                            MINDAY_DB = 6;
                        }

                        //----
                        if (day2codedb.Contains("Mon"))
                        {
                            MAXDAY_DB = 1;
                        }
                        if (day2codedb.Contains("Tue"))
                        {
                            MAXDAY_DB = 2;
                        }
                        if (day2codedb.Contains("Wed"))
                        {
                            MAXDAY_DB = 3;
                        }
                        if (day2codedb.Contains("Thu"))
                        {
                            MAXDAY_DB = 4;
                        }
                        if (day2codedb.Contains("Fri"))
                        {
                            MAXDAY_DB = 5;
                        }
                        if (day2codedb.Contains("Sat"))
                        {
                            MAXDAY_DB = 6;
                        }

                        if ((fstart >= dbstart) && (fend < dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB < MINDAY && MAXDAY < MAXDAY_DB))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling55", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if ((fstart >= dbstart) && (fend > dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB < MINDAY && MAXDAY < MAXDAY_DB))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling54", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if ((fstart >= dbstart) && (fend <=dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB > MINDAY && MAXDAY > MAXDAY_DB))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling31.1", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            return;
                        }
                        if ((fstart >= dbstart) && (fend > dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB > MINDAY && MAXDAY > MAXDAY_DB))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling35.1", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //if (MINDAY_DB < MINDAY && MAXDAY < MAXDAY_DB)
                        //{
                        //    MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}
                    }
                }
                //=======================
                //facsched
               
                //4-----------------------------------checks if theres already acts in facs, same hour and same day
                con.Open();
                OdbcDataAdapter da55 = new OdbcDataAdapter("Select*from facultysched_tbl where section<>'" + classsec + "'and faculty='"+cmbFacs.Text+"'", con);
                DataTable dt55 = new DataTable();
                da55.Fill(dt55);
                con.Close();
                if (dt55.Rows.Count > 0)
                {
                    for (int i = 0; i < dt55.Rows.Count; i++)
                    {
                        string dbday = dt55.Rows[i].ItemArray[8].ToString();
                        string dbstarttime = dt55.Rows[i].ItemArray[6].ToString();
                        int dbstarttimeint = Convert.ToInt32(dtcs.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtcs.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        int dbendtimeint = Convert.ToInt32(dtcs.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtcs.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        
                        if ((day.Contains("Mon") == true && dbday.Contains("Mon") == true && fstart >= dbstarttimeint) || (day.Contains("Tue") == true && dbday.Contains("Tue") == true && fstart >= dbstarttimeint) || (day.Contains("Wed") == true && dbday.Contains("Wed") == true && fstart >= dbstarttimeint) || (day.Contains("Thu") == true && dbday.Contains("Thu") == true && fstart >= dbstarttimeint) || (day.Contains("Fri") == true && dbday.Contains("Fri") == true && fstart >= dbstarttimeint) || (day.Contains("Sat") == true && dbday.Contains("Sat") == true && fstart >= dbstarttimeint))
                        {
                            MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Scheduling55", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                //========================FACULTY BETWEEN THE DAYS
                con.Open();
                OdbcDataAdapter dacs1 = new OdbcDataAdapter("Select*from facultysched_tbl where section!='" + classsec + "'and level!='" + classgrade + "'and faculty='" + cmbFacs.Text + "'", con);
                DataTable dtcs1 = new DataTable();
                dacs1.Fill(dtcs1);
                con.Close();
                if (dtcs1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtcs1.Rows.Count; i++)
                    {
                        float dbstart = Convert.ToSingle(dtcs1.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtcs1.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtcs1.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtcs1.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string unitdaystart = dtcs1.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                        string unitdayend = dtcs1.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                        string daysofsched = dtcs1.Rows[i].ItemArray[8].ToString();
                        TimeSpan spn = endTime.Subtract(startTime);

                        string day1codedb = "";
                        string day2codedb = "";
                        int MINDAY_DB = 0;
                        int MAXDAY_DB = 0;

                        if (daysofsched.Length > 3)
                        {
                            day1codedb = dtcs1.Rows[i].ItemArray[8].ToString().Substring(0, 3);
                            day2codedb = dtcs1.Rows[i].ItemArray[8].ToString().Substring(4, 3);
                        }
                        else
                        {
                            day1codedb = dtcs1.Rows[i].ItemArray[8].ToString().Substring(0, 3);
                            day2codedb = day1codedb;
                        }

                        if (day1codedb.Contains("Mon"))
                        {
                            MINDAY_DB = 1;
                        }
                        if (day1codedb.Contains("Tue"))
                        {
                            MINDAY_DB = 2;
                        }
                        if (day1codedb.Contains("Wed"))
                        {
                            MINDAY_DB = 3;
                        }
                        if (day1codedb.Contains("Thu"))
                        {
                            MINDAY_DB = 4;
                        }
                        if (day1codedb.Contains("Fri"))
                        {
                            MINDAY_DB = 5;
                        }
                        if (day1codedb.Contains("Sat"))
                        {
                            MINDAY_DB = 6;
                        }

                        //----
                        if (day2codedb.Contains("Mon"))
                        {
                            MAXDAY_DB = 1;
                        }
                        if (day2codedb.Contains("Tue"))
                        {
                            MAXDAY_DB = 2;
                        }
                        if (day2codedb.Contains("Wed"))
                        {
                            MAXDAY_DB = 3;
                        }
                        if (day2codedb.Contains("Thu"))
                        {
                            MAXDAY_DB = 4;
                        }
                        if (day2codedb.Contains("Fri"))
                        {
                            MAXDAY_DB = 5;
                        }
                        if (day2codedb.Contains("Sat"))
                        {
                            MAXDAY_DB = 6;
                        }

                        if ((fstart >= dbstart) && (fend < dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB < MINDAY && MAXDAY < MAXDAY_DB))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling44", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if ((fstart >= dbstart) && (fend > dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB < MINDAY && MAXDAY < MAXDAY_DB))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling43", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if ((fstart >= dbstart) && (fend < dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB > MINDAY && MAXDAY > MAXDAY_DB))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling44.1", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            return;
                        }
                        if ((fstart >= dbstart) && (fend > dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text) && (MINDAY_DB > MINDAY && MAXDAY > MAXDAY_DB))
                        {
                            MessageBox.Show("Conflict schedule for section " + classsec, "Scheduling43.1", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                //=======================

                con.Open();
                OdbcDataAdapter daccc = new OdbcDataAdapter("Select*from facultysched_tbl where section!='" + classsec + "'and level!='" + classgrade + "'and faculty='"+cmbFacs.Text+"'", con);
                DataTable dtccc = new DataTable();
                daccc.Fill(dtccc);
                con.Close();
                if (dtccc.Rows.Count > 0)
                {
                    
                    for (int i = 0; i < dtccc.Rows.Count; i++)
                    {
                        string dbhourstart = dtccc.Rows[i].ItemArray[6].ToString().Substring(0, 2);
                        int dudminstart = Convert.ToInt32(dudMinStart.Text);
                        float dbstart = Convert.ToSingle(dtccc.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtccc.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtccc.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtccc.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        int dbstartmin = Convert.ToInt32(dtccc.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        string dbday = dtccc.Rows[i].ItemArray[8].ToString();
                        string unitdaystart = dtccc.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                        string unitdayend = dtccc.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                        if (dbhourstart == dudHrStart.Text)
                        {
                            //if ((dbstartmin >dudminstart) && (dbend >fend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                            //{
                             //   MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance011", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                             //   return;
                            //}
                            if ((dudminstart >dbstartmin) && (fend >= dbend) && (fstart < dbstart) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                            {
                                MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance012", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if ((dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                            {
                                MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance013", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

                
                con.Open();
                OdbcDataAdapter dax1 = new OdbcDataAdapter("Select*from facultysched_tbl where section!='" + classsec + "'and level!='" + classgrade + "'and faculty='" + cmbFacs.Text + "'", con);
                DataTable dtx1 = new DataTable();
                dax1.Fill(dtx1);
                con.Close();
                if (dtx1.Rows.Count > 0)
                {

                    for (int i = 0; i < dtx1.Rows.Count; i++)
                    {
                        string dbhourstart = dtx1.Rows[i].ItemArray[6].ToString().Substring(0, 2);
                        string dbhourend = dtx1.Rows[i].ItemArray[7].ToString().Substring(0, 2);
                        int dudminstart = Convert.ToInt32(dudMinStart.Text);
                        float dbstart = Convert.ToSingle(dtx1.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtx1.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtx1.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtx1.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        int dbendmin = Convert.ToInt32(dtx1.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtx1.Rows[i].ItemArray[8].ToString();
                        string unitdaystart = dtx1.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                        string unitdayend = dtx1.Rows[i].ItemArray[7].ToString().Substring(6, 2);
                        if (dbhourend == dudHrStart.Text)
                        {
                            if ((dbendmin >dudminstart) && (dbend <fend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                            {
                                MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance02", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if ((dudminstart >dbendmin) && (fend <dbend) && (fstart < dbstart) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                            {
                                MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance02", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            //if ((dbday.Contains(day1code) == true || dbday.Contains(day2code) == true))
                            //{
                             //   MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance02", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                             //   return;
                            //}
                        }
                    }
                }

                con.Open();
                OdbcDataAdapter dac4 = new OdbcDataAdapter("Select*from facultySched_tbl where section='" + classsec + "'and level='" + classgrade + "'and days='" + cmbDayStart + "'", con);
                DataTable dtc4 = new DataTable();
                dac4.Fill(dtc4);
                con.Close();
                if (dtc4.Rows.Count > 0)
                {
                    //string to float
                    for (int i = 0; i < dtc4.Rows.Count; i++)
                    {
                        float dbstart = Convert.ToSingle(dtc4.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc4.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtc4.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc4.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtc4.Rows[i].ItemArray[8].ToString();
                        if ((dbend == fend && dbstart == fstart) || (dbstart == fstart && fend >= dbend) || (dbend == fend && fstart <= dbstart) || (dbday.Contains(day) == true))
                        {
                            MessageBox.Show("Conflict schedule for section "+classsec, "Schedule maintenance22", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                con.Open();
                OdbcDataAdapter dac111 = new OdbcDataAdapter("Select*from facultySched_tbl where faculty='" + cmbFacs.Text + "'", con);
                DataTable dtc111 = new DataTable();
                dac111.Fill(dtc111);
                con.Close();
                if (dtc111.Rows.Count > 0)
                {
                    string dbday = dtc111.Rows[0].ItemArray[8].ToString();
                    string dbstarttime = dtc111.Rows[0].ItemArray[6].ToString();
                    setupdayofsubj();

                    if ((day.Contains("Mon") == true && dbday.Contains("Mon") == true && dbstarttime == start) || (day.Contains("Tue") == true && dbday.Contains("Tue") == true && dbstarttime == start) || (day.Contains("Wed") == true && dbday.Contains("Wed") == true && dbstarttime == start) || (day.Contains("Thu") == true && dbday.Contains("Thu") == true && dbstarttime == start) || (day.Contains("Fri") == true && dbday.Contains("Fri") == true && dbstarttime == start) || (day.Contains("Sat") == true && dbday.Contains("Sat") == true && dbstarttime == start) || (day.Contains("Sun") == true && dbday.Contains("Sun") == true && dbstarttime == start))
                    {
                        MessageBox.Show("Conflict schedule for " + dtc111.Rows[0].ItemArray[4].ToString(), "Schedule maintenance11", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                con.Open();
                OdbcDataAdapter dac11 = new OdbcDataAdapter("Select*from facultySched_tbl where faculty='" + cmbFacs.Text + "'and start='" + start + "'and level='" + classgrade + "'and section='" + classsec + "'", con);
                DataTable dtc11 = new DataTable();
                dac11.Fill(dtc11);
                con.Close();
                if (dtc11.Rows.Count > 0)
                {
                    string dbday = dtc11.Rows[0].ItemArray[8].ToString();
                    string dbstarttime = dtc11.Rows[0].ItemArray[6].ToString();
                    setupdayofsubj();

                    if ((day.Contains("Mon") == true && dbday.Contains("Mon") == true && start == dbstarttime) || (day.Contains("Tue") == true && dbday.Contains("Tue") == true && start == dbstarttime) || (day.Contains("Wed") == true && dbday.Contains("Wed") == true && start == dbstarttime) || (day.Contains("Thu") == true && dbday.Contains("Thu") == true && start == dbstarttime) || (day.Contains("Fri") == true && dbday.Contains("Fri") == true && start == dbstarttime) || (day.Contains("Sat") == true && dbday.Contains("Sat") == true && start == dbstarttime) || (day.Contains("Sun") == true && dbday.Contains("Sun") == true && start == dbstarttime))
                    {
                        MessageBox.Show("Conflict schedule for " + dtc11.Rows[0].ItemArray[4].ToString() + " in section " + classsec, "Schedule maintenance32", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                //not yet added to update event method
                con.Open();
                OdbcDataAdapter dac1 = new OdbcDataAdapter("Select*from facultySched_tbl where faculty='" + cmbFacs.Text + "'", con);
                DataTable dtc1 = new DataTable();
                dac1.Fill(dtc1);
                con.Close();
                if (dtccc.Rows.Count > 0)
                {
                    for (int i = 0; i < dtc1.Rows.Count; i++)
                    {
                        float dbstart = Convert.ToSingle(dtc1.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc1.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtc1.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc1.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtc1.Rows[i].ItemArray[8].ToString();
                        string unitdaystart = dtc1.Rows[i].ItemArray[6].ToString().Substring(6, 2);
                        string unitdayend = dtc1.Rows[i].ItemArray[7].ToString().Substring(6, 2);

                        if ((fstart > dbstart) && (fend < dbend) && (unitdaystart == cmbDayStart.Text) && (unitdayend == cmbDayEnd.Text))// || ((fstart < dbstart) && (fend <= dbend))
                        {
                            if (fstart > dbstart)
                            {
                                float spantime = fstart - dbstart;
                                if (spantime == 0)
                                {
                                    MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance1", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance1", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

                con.Open();
                OdbcDataAdapter dacc2 = new OdbcDataAdapter("Select*from facultySched_tbl where faculty='" + cmbFacs.Text + "'", con);
                DataTable dtcc2 = new DataTable();
                dacc2.Fill(dtcc2);
                con.Close();
                if (dtcc2.Rows.Count > 0)
                {
                    for (int i = 0; i < dtcc2.Rows.Count; i++)
                    {
                        float dbstart = Convert.ToSingle(dtcc2.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtcc2.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtcc2.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtcc2.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtcc2.Rows[i].ItemArray[8].ToString();
                        if ((fstart > dbend) && (fend <= dbend))//((fstart < dbstart) && )
                        {
                            if (fstart > dbend)
                            {
                                float spantime = fstart - dbend;
                                if (spantime == 0)
                                {
                                    MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Conflict schedule for " + cmbFacs.Text, "Schedule maintenance3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

               
                //===========================
                con.Open();
                OdbcDataAdapter dac2 = new OdbcDataAdapter("Select*from facultySched_tbl where start='" + start + "'and days='" + day + "'and section='" + classsec + "'and level='"+classgrade+"'", con);
                DataTable dtc2 = new DataTable();
                dac2.Fill(dtc2);
                con.Close();
                if (dtc2.Rows.Count > 0)
                {
                    MessageBox.Show("Conflict schedule for section "+classsec, "Schedule maintenance54", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                 con.Open();
                OdbcDataAdapter dacc = new OdbcDataAdapter("Select*from facultySched_tbl where faculty='" + cmbFacs.Text + "'and section!='"+classsec+"'and days='"+day+"'", con);
                DataTable dtcc = new DataTable();
                dacc.Fill(dtcc);
                con.Close();
                if (dtcc.Rows.Count > 0)
                {
                    for (int i = 0; i < dtcc.Rows.Count; i++)
                    {
                        float dbstart = Convert.ToSingle(dtcc.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtcc.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                        float dbend = Convert.ToSingle(dtcc.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtcc.Rows[i].ItemArray[7].ToString().Substring(3, 2));
                        string dbday = dtcc.Rows[i].ItemArray[8].ToString();
                        if ((((dbend == fend && dbstart == fstart) || (dbstart == fstart && fend >= dbend) || (dbend == fend && fstart <= dbstart))&& (dbday.Contains(day) == true)))
                        {
                            MessageBox.Show("Conflict schedule for "+cmbFacs.Text, "Schedule maintenance34", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                setupAddForClass();
            }
        }

       
        public void setupAddForClass()
        {
            string start = dudHrStart.Text + ":" + dudMinStart.Text + " " + cmbDayStart.Text;
            string end = dudHourEnd.Text + ":" + dudMinEnd.Text + " " + cmbDayEnd.Text;
            setupdayofsubj();
            string type="";
            if (cmbsub.Text != "Recess" && cmbsub.Text != "Lunch break" && cmbsub.Text != "Flag cer./Class prep." && cmbsub.Text != "Club" && cmbsub.Text != "Homeroom")
            {
                type = "Academic";
            }
            else
            {
                type = "Non-Academic";
            }
           
            con.Open();
            string addSch = "Insert Into schedule_tbl(secid,level,section,subject,faculty,room,start,end,days,type)values('" + lblKey.Text + "','" +
            classgrade + "','" + classsec + "','" + cmbsub.Text + "','" + cmbFacs.Text + "','" + cmbRoom.Text + "','" + start + "','" + end + "','" + day + "','"+type+"')";
            OdbcCommand cmdAdd = new OdbcCommand(addSch, con);
            cmdAdd.ExecuteNonQuery();
            con.Close();

            con.Open();
            OdbcDataAdapter dafindEmpno = new OdbcDataAdapter("Select empno from employees_tbl where (select concat(firstname,' ',middlename,' ',lastname))='" + cmbFacs.Text + "' and position='faculty'", con);
            DataTable dtfindEmpno = new DataTable();
            dafindEmpno.Fill(dtfindEmpno);
            con.Close();

            if (dtfindEmpno.Rows.Count > 0)
            {
                con.Open();
                string addtoFacshed = "Insert Into facultySched_tbl(empno,level,section,subject,faculty,room,start,end,days,type)values('" + dtfindEmpno.Rows[0].ItemArray[0].ToString() + "','" +
                    classgrade + "','" + classsec + "','" + cmbsub.Text + "','" + cmbFacs.Text + "','" + cmbRoom.Text + "','" + start + "','" + end + "','" + day + "','"+type+"')";
                OdbcCommand cmdfind = new OdbcCommand(addtoFacshed, con);
                cmdfind.ExecuteNonQuery();
                con.Close();
            }

            btnAdd.Enabled = false;
            btnAdd.Text = "Add";
            btnClear.Text = "Clear";
            setupviewbysection(lblKey.Text);
            cmbFacs.Items.Clear();
            MessageBox.Show("schedule successfully added", "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void setupdayofsubj()
        {
            string f = cmbday1.Text;
            string s = cmbday2.Text;

            if (f == "Monday")
            {
                day1code = "Mon";
                MINDAY = 1;
            }
            if (f == "Tuesday")
            {
                day1code = "Tue";
                MINDAY = 2;
            }
            if (f == "Wednesday")
            {
                day1code = "Wed";
                MINDAY = 3;
            }
            if (f == "Thursday")
            {
                day1code = "Thu";
                MINDAY = 4;
            }
            if (f == "Friday")
            {
                day1code = "Fri";
                MINDAY = 5;
            }
            if (f == "Saturday")
            {
                day1code = "Sat";
                MINDAY = 6;
            }
            if (f == "Sunday")
            {
                day1code = "Sun";
            }
            //----
            if (s == "Monday")
            {
                day2code = "Mon";
                MAXDAY = 1;
            }
            if (s == "Tuesday")
            {
                day2code = "Tue";
                MAXDAY = 2;
            }
            if (s == "Wednesday")
            {
                day2code = "Wed";
                MAXDAY = 3;
            }
            if (s == "Thursday")
            {
                day2code = "Thu";
                MAXDAY = 4;
            }
            if (s == "Friday")
            {
                day2code = "Fri";
                MAXDAY = 5;
            }
            if (s == "Saturday")
            {
                day2code = "Sat";
                MAXDAY = 6;
            }
            if (s == "Sunday")
            {
                day2code = "Sun";
            }




            if (f == "Monday" && s == "Monday")
            {
                day = "Mon";
            }
            if (f == "Monday" && s == "Tuesday")
            {
                day = "Mon-Tue";
            }
            if (f == "Monday" && s == "Wednesday")
            {
                day = "Mon-Wed";
            }
            if (f == "Monday" && s == "Thursday")
            {
                day = "Mon-Thu";
            }
            if (f == "Monday" && s == "Friday")
            {
                day = "Mon-Fri";
            }
            if (f == "Monday" && s == "Saturday")
            {
                day = "Mon-Sat";
            }
            if (f == "Monday" && s == "Sunday")
            {
                day = "Mon-Sun";
            }



            if (f == "Tuesday" && s == "Tuesday")
            {
                day = "Tue";
            }
            if (f == "Tuesday" && s == "Monday")
            {
                day = "Tue-Mon";
            }
            if (f == "Tuesday" && s == "Wednesday")
            {
                day = "Tue-Wed";
            }
            if (f == "Tuesday" && s == "Thursday")
            {
                day = "Tue-Thu";
            }
            if (f == "Tuesday" && s == "Friday")
            {
                day = "Tue-Fri";
            }
            if (f == "Tuesday" && s == "Saturday")
            {
                day = "Tue-Sat";
            }
            if (f == "Tuesday" && s == "Sunday")
            {
                day = "Tue-Sun";
            }



            if (f == "Wednesday" && s == "Wednesday")
            {
                day = "Wed";
            }
            if (f == "Wednesday" && s == "Monday")
            {
                day = "Wed-Mon";
            }
            if (f == "Wednesday" && s == "Tuesday")
            {
                day = "Wed-Tue";
            }
            if (f == "Wednesday" && s == "Thursday")
            {
                day = "Wed-Thu";
            }
            if (f == "Wednesday" && s == "Friday")
            {
                day = "Wed-Fri";
            }
            if (f == "Wednesday" && s == "Saturday")
            {
                day = "Wed-Sat";
            }
            if (f == "Wednesday" && s == "Sunday")
            {
                day = "Wed-Sun";
            }



            if (f == "Thursday" && s == "Thursday")
            {
                day = "Thu";
            }
            if (f == "Thursday" && s == "Tuesday")
            {
                day = "Thu-Tue";
            }
            if (f == "Thursday" && s == "Wednesday")
            {
                day = "Thu-Wed";
            }
            if (f == "Thursday" && s == "Monday")
            {
                day = "Thu-Mon";
            }
            if (f == "Thursday" && s == "Friday")
            {
                day = "Thu-Fri";
            }
            if (f == "Thursday" && s == "Saturday")
            {
                day = "Thu-Sat";
            }
            if (f == "Thursday" && s == "Sunday")
            {
                day = "Thu-Sun";
            }





            if (f == "Friday" && s == "Friday")
            {
                day = "Fri";
            }
            if (f == "Friday" && s == "Tuesday")
            {
                day = "Fri-Tue";
            }
            if (f == "Friday" && s == "Wednesday")
            {
                day = "Fri-Wed";
            }
            if (f == "Friday" && s == "Monday")
            {
                day = "Thu-Mon";
            }
            if (f == "Friday" && s == "Thursday")
            {
                day = "Fri-Thu";
            }
            if (f == "Friday" && s == "Saturday")
            {
                day = "Fri-Sat";
            }
            if (f == "Friday" && s == "Sunday")
            {
                day = "Fri-Sun";
            }



            if (f == "Saturday" && s == "Saturday")
            {
                day = "Sat";
            }
            if (f == "Saturday" && s == "Tuesday")
            {
                day = "Sat-Tue";
            }
            if (f == "Saturday" && s == "Wednesday")
            {
                day = "Sat-Wed";
            }
            if (f == "Saturday" && s == "Friday")
            {
                day = "Sat-Fri";
            }
            if (f == "Saturday" && s == "Thursday")
            {
                day = "Sat-Thu";
            }
            if (f == "Saturday" && s == "Monday")
            {
                day = "Sat-Mon";
            }
            if (f == "Saturday" && s == "Sunday")
            {
                day = "Sat-Sun";
            }


            if (f == "Sunday" && s == "Sunday")
            {
                day = "Sun";
            }
            if (f == "Sunday" && s == "Tuesday")
            {
                day = "Sun-Tue";
            }
            if (f == "Sunday" && s == "Wednesday")
            {
                day = "Sun-Wed";
            }
            if (f == "Sunday" && s == "Monday")
            {
                day = "Sun-Mon";
            }
            if (f == "Sunday" && s == "Thursday")
            {
                day = "Sun-Thu";
            }
            if (f == "Sunday" && s == "Saturday")
            {
                day = "Sun-Sat";
            }
            if (f == "Sunday" && s == "Friday")
            {
                day = "Sun-Fri";
            }
        }

        public void setupdisplaydayofsubj()
        {
            if (tempdays=="Mon")
            {
                cmbday1.Text = "Monday";
                cmbday2.Text = "Monday";
            }
            if (tempdays=="Mon-Tue")
            {
                cmbday1.Text = "Monday";
                cmbday2.Text = "Tuesday";
            }
            if (tempdays=="Mon-Wed")
            {
                cmbday1.Text = "Monday";
                cmbday2.Text = "Wednesday";
            }
            if (tempdays== "Mon-Thu")
            {
                cmbday1.Text = "Monday";
                cmbday2.Text = "Thursday";
            }
            if (tempdays== "Mon-Fri")
            {
                cmbday1.Text = "Monday";
                cmbday2.Text = "Friday";
            }
            if (tempdays== "Mon-Sat")
            {
                cmbday1.Text = "Monday";
                cmbday2.Text = "Saturday";
            }
            if (tempdays == "Mon-Sun")
            {
                cmbday1.Text = "Monday";
                cmbday2.Text = "Sunday";
            }



            if (tempdays== "Tue")
            {
                cmbday1.Text = "Tuesday";
                cmbday2.Text = "Tuesday";
            }
            if (tempdays== "Tue-Mon")
            {
                cmbday1.Text = "Tuesday";
                cmbday2.Text = "Monday";
            }
            if (tempdays== "Tue-Wed")
            {
                cmbday1.Text = "Tuesday";
                cmbday2.Text = "Wednesday";
            }
            if (tempdays== "Tue-Thu")
            {
                cmbday1.Text = "Tuesday";
                cmbday2.Text = "Thursday";
            }
            if (tempdays == "Tue-Fri")
            {
                cmbday1.Text = "Tuesday";
                cmbday2.Text = "Friday";
            }
            if (tempdays == "Tue-Sat")
            {
                cmbday1.Text = "Tuesday";
                cmbday2.Text = "Saturday";
            }
            if (tempdays== "Tue-Sun")
            {
                cmbday1.Text = "Tuesday";
                cmbday2.Text = "Sunday";
            }



            if (tempdays== "Wed")
            {
                cmbday1.Text = "Wednesday";
                cmbday2.Text = "Wednesday";
            }
            if (tempdays == "Wed-Mon")
            {
                cmbday1.Text = "Wednesday";
                cmbday2.Text = "Monday";
            }
            if (tempdays == "Wed-Tue")
            {
                cmbday1.Text = "Wednesday";
                cmbday2.Text = "Tuesday";
            }
            if (tempdays == "Wed-Thu")
            {
                cmbday1.Text = "Wednesday";
                cmbday2.Text = "Thursday";
            }
            if (tempdays== "Wed-Fri")
            {
                cmbday1.Text = "Wednesday";
                cmbday2.Text = "Friday";
            }
            if (tempdays== "Wed-Sat")
            {
                cmbday1.Text = "Wednesday";
                cmbday2.Text = "Saturday";
            }
            if (tempdays== "Wed-Sun")
            {
                cmbday1.Text = "Wednesday";
                cmbday2.Text = "Sunday";
            }



            if (tempdays== "Thu")
            {
                cmbday1.Text = "Thursday";
                cmbday2.Text = "Thursday";
            }
            if (tempdays== "Thu-Tue")
            {
                cmbday1.Text = "Thursday";
                cmbday2.Text = "Tuesday";
            }
            if (tempdays == "Thu-Wed")
            {
                cmbday1.Text = "Thursday";
                cmbday2.Text = "Wednesday";
            }
            if (tempdays == "Thu-Mon")
            {
                cmbday1.Text = "Thursday";
                cmbday2.Text = "Monday";
            }
            if (tempdays == "Thu-Fri")
            {
                cmbday1.Text = "Thursday";
                cmbday2.Text = "Friday";
            }
            if (tempdays== "Thu-Sat")
            {
                cmbday1.Text = "Thursday";
                cmbday2.Text = "Saturday";
            }
            if (tempdays== "Thu-Sun")
            {
                cmbday1.Text = "Thursday";
                cmbday2.Text = "Sunday";
            }





            if (tempdays == "Fri")
            {
                cmbday1.Text = "Friday";
                cmbday2.Text = "Friday";
            }
            if (tempdays == "Fri-Tue")
            {
                cmbday1.Text = "Friday";
                cmbday2.Text = "Tuesday";
            }
            if (tempdays == "Fri-Wed")
            {
                cmbday1.Text = "Friday";
                cmbday2.Text = "Wednesday";
            }
            if (tempdays == "Thu-Mon")
            {
                cmbday1.Text = "Thursday";
                cmbday2.Text = "Monday";
            }
            if (tempdays == "Fri-Thu")
            {
                cmbday1.Text = "Friday";
                cmbday2.Text = "Thursday";
            }
            if (tempdays == "Fri-Sat")
            {
                cmbday1.Text = "Friday";
                cmbday2.Text = "Saturday";
            }
            if (tempdays == "Fri-Sun")
            {
                cmbday1.Text = "Friday";
                cmbday2.Text = "Sunday";
            }



            if (tempdays == "Sat")
            {
                cmbday1.Text = "Saturday";
                cmbday2.Text = "Saturday";
            }
            if (tempdays == "Sat-Tue")
            {
                cmbday1.Text = "Saturday";
                cmbday2.Text = "Tuesday";
            }
            if (tempdays == "Sat-Wed")
            {
                cmbday1.Text = "Saturday";
                cmbday2.Text = "Wednesday";
            }
            if (tempdays == "Sat-Fri")
            {
                cmbday1.Text = "Saturday";
                cmbday2.Text = "Friday";
            }
            if (tempdays == "Sat-Thu")
            {
                cmbday1.Text = "Saturday";
                cmbday2.Text = "Thursday";
            }
            if (tempdays == "Sat-Mon")
            {
                cmbday1.Text = "Saturday";
                cmbday2.Text = "Monday";
            }
            if (tempdays == "Sat-Sun")
            {
                cmbday1.Text = "Saturday";
                cmbday2.Text = "Sunday";
            }


            if (tempdays== "Sun")
            {
                cmbday1.Text = "Sunday";
                cmbday2.Text = "Sunday";
            }
            if (tempdays == "Sun-Tue")
            {
                cmbday1.Text = "Sunday";
                cmbday2.Text = "Tuesday";
            }
            if (tempdays == "Sun-Wed")
            {
                cmbday1.Text = "Sunday";
                cmbday2.Text = "Wednesday";
            }
            if (tempdays == "Sun-Mon")
            {
                cmbday1.Text = "Sunday";
                cmbday2.Text = "Monday";
            }
            if (tempdays == "Sun-Thu")
            {
                cmbday1.Text = "Sunday";
                cmbday2.Text = "Thursday";
            }
            if (tempdays== "Sun-Sat")
            {
                cmbday1.Text = "Sunday";
                cmbday2.Text = "Saturday";
            }
            if (tempdays == "Sun-Fri")
            {
                cmbday1.Text = "Sunday";
                cmbday2.Text = "Friday";
            }
        }

        private void dgvDisplay_Click(object sender, EventArgs e)
        {
            setupdisableinput();
            lblcount.Location = new Point(3, 6);
            lblAcdNonCount.Location = new Point(3, 25);
            btnUpdate.Text = "Update";
            btnClear.Text = "Clear";
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            if (pnlhead.Rows.Count > 0)
            {
                selectedfac = pnlhead.SelectedRows[0].Cells[1].Value.ToString();
                string selectedroom = pnlhead.SelectedRows[0].Cells[2].Value.ToString();
                tempsubj = pnlhead.SelectedRows[0].Cells[0].Value.ToString();
                tempdays = pnlhead.SelectedRows[0].Cells[5].Value.ToString();



                if (selectedfac == "")
                {
                    //setupallfaculty();
                }
                if (selectedroom == "")
                {
                    setuprooms(classgrade,classsec);
                }

                setupretrieveddata(lblKey.Text);
            }
            else
            {
                return;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
           
            if (btnUpdate.Text == "Update")
            {
                setupenableinput();
                btnUpdate.Text = "Save";
                btnDelete.Enabled = false;
                btnClear.Text = "Cancel";

                if (tempsubj == "Recess" || tempsubj == "Lunch break" || tempsubj == "Flag cer./Class prep.")
                {
                    //setupallfaculty();
                    cmbFacs.Enabled = false;
                    if (tempsubj == "Recess" || tempsubj == "Lunch break")
                    {
                        cmbday1.Enabled=false;
                        cmbday2.Enabled = false;
                    }
                }
                else
                {
                    cmbFacs.Enabled = true;
                    cmbday1.Enabled = true;
                    cmbday2.Enabled = true;
                }

                if (tempsubj == "Flag cer./Class prep.")
                {
                    setuprooms(classgrade,classsec);
                    cmbRoom.Enabled = false;
                }
                else
                {
                    cmbRoom.Enabled = true;
                }
            }
            else
            {
                if (cmbsub.Text == "" || cmbFacs.Text == "" || cmbRoom.Text == "" || cmbday1.Text == "" || cmbday2.Text == "" ||
               cmbDayStart.Text == "" || cmbDayEnd.Text == "" || dudHrStart.Text == "" || dudMinStart.Text == "" || dudHourEnd.Text == "" || dudMinEnd.Text == "")
                {
                    if (cmbsub.Text == "Recess" || cmbsub.Text == "Lunch break" || cmbsub.Text == "Flag cer./Class prep." || cmbsub.Text == "Club" || cmbsub.Text == "Homeroom")
                    {
                        if (cmbDayStart.Text == "" || cmbDayEnd.Text == "" || dudHrStart.Text == "" || dudMinStart.Text == "" || dudHourEnd.Text == "" || dudMinEnd.Text == "")
                        {
                            MessageBox.Show("time not set.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("fill out all fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    string start = dudHrStart.Text + ":" + dudMinStart.Text + " " + cmbDayStart.Text;
                    string end = dudHourEnd.Text + ":" + dudMinEnd.Text + " " + cmbDayEnd.Text;
                    
                    float fstart = Convert.ToInt32(dudHrStart.Text + "" + dudMinStart.Text);
                    float fend = Convert.ToInt32(dudHourEnd.Text + "" + dudMinEnd.Text);

                    DateTime startTime = Convert.ToDateTime(start);
                    DateTime endTime = Convert.ToDateTime(end);

                    setupdayofsubj();

                    if ((endTime.Hour < startTime.Hour) || (endTime.Hour == startTime.Hour))
                    {
                        MessageBox.Show("Invalid time schedule", "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    con.Open();
                    OdbcDataAdapter dac11 = new OdbcDataAdapter("Select*from schedule_tbl where start='" + start + "'and section='" + classsec + "'", con);
                    DataTable dtc11 = new DataTable();
                    dac11.Fill(dtc11);
                    con.Close();
                    if (dtc11.Rows.Count > 0)
                    {
                        string secid = dtc11.Rows[0].ItemArray[0].ToString();
                        string dbday = dtc11.Rows[0].ItemArray[8].ToString();
                        string dbstarttime = dtc11.Rows[0].ItemArray[6].ToString();
                        setupdayofsubj();

                        if (((day.Contains("Mon") == true && dbday.Contains("Mon") == true && start == dbstarttime) || (day.Contains("Tue") == true && dbday.Contains("Tue") == true && start == dbstarttime) || (day.Contains("Wed") == true && dbday.Contains("Wed") == true && start == dbstarttime) || (day.Contains("Thu") == true && dbday.Contains("Thu") == true && start == dbstarttime) || (day.Contains("Fri") == true && dbday.Contains("Fri") == true && start == dbstarttime) || (day.Contains("Sat") == true && dbday.Contains("Sat") == true && start == dbstarttime) || (day.Contains("Sun") == true && dbday.Contains("Sun") == true && start == dbstarttime))&& secid!=lblKey.Text)
                        {
                            MessageBox.Show("theres a subject/activity scheduled on the set time." + "\n\nSubject: " + dtc11.Rows[0].ItemArray[3].ToString() + "\nFaculty: " + dtc11.Rows[0].ItemArray[4].ToString() + "\nDays: " + dbday, "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    con.Open();
                    OdbcDataAdapter dac = new OdbcDataAdapter("Select*from facultySched_tbl where faculty='" + cmbFacs.Text + "'and days='" + cmbDayStart + "'", con);
                    DataTable dtc = new DataTable();
                    dac.Fill(dtc);
                    con.Close();
                    if (dtc.Rows.Count > 0)
                    {
                        //string to float
                        for (int i = 0; i < dtc.Rows.Count; i++)
                        {
                            string lev = dtc.Rows[i].ItemArray[1].ToString();
                            string sec = dtc.Rows[i].ItemArray[2].ToString();
                            float dbstart = Convert.ToSingle(dtc.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                            float dbend = Convert.ToSingle(dtc.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc.Rows[i].ItemArray[7].ToString().Substring(3, 2));

                            if (((dbend == fend && dbstart == fstart) || (dbstart == fstart && fend >= dbend) || (dbend == fend && fstart <= dbstart))&& (classgrade!=lev && classsec!=sec))
                            {
                                MessageBox.Show("faculty already have a schedule on the set time." + "\n\nClass name: " + dtc.Rows[i].ItemArray[1].ToString() + " - " + dtc.Rows[i].ItemArray[2].ToString() + "\nSubject: " + dtc.Rows[i].ItemArray[3].ToString(), "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    con.Open();
                    OdbcDataAdapter dac4 = new OdbcDataAdapter("Select*from facultySched_tbl where section='" + classsec + "'and level='" + classgrade + "'and days='" + cmbDayStart + "'", con);
                    DataTable dtc4 = new DataTable();
                    dac4.Fill(dtc4);
                    con.Close();
                    if (dtc4.Rows.Count > 0)
                    {
                        //string to float
                        for (int i = 0; i < dtc4.Rows.Count; i++)
                        {
                            float dbstart = Convert.ToSingle(dtc4.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtc4.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                            float dbend = Convert.ToSingle(dtc4.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtc4.Rows[i].ItemArray[7].ToString().Substring(3, 2));

                            if ((dbend == fend && dbstart == fstart) || (dbstart == fstart && fend >= dbend) || (dbend == fend && fstart <= dbstart))
                            {
                                MessageBox.Show("class already have a schedule on the set time." + "\n\nClass name: " + dtc4.Rows[i].ItemArray[1].ToString() + " - " + dtc4.Rows[i].ItemArray[2].ToString() + "\nSubject: " + dtc4.Rows[i].ItemArray[3].ToString(), "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                   
                    con.Open();
                    OdbcDataAdapter dac2 = new OdbcDataAdapter("Select*from facultySched_tbl where start='" + start + "'and days='" + day + "'and section='" + classsec + "'and faculty!='"+selectedfac+"'", con);
                    DataTable dtc2 = new DataTable();
                    dac2.Fill(dtc2);
                    con.Close();
                    if (dtc2.Rows.Count > 0)
                    {
                        MessageBox.Show("theres a faculty scheduled on the set time." + "\n\nClass name: " + dtc2.Rows[0].ItemArray[1].ToString() + " - " + dtc2.Rows[0].ItemArray[2].ToString() + "\nSubject: " + dtc2.Rows[0].ItemArray[3].ToString() + "\nFaculty: " + dtc2.Rows[0].ItemArray[4].ToString(), "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    con.Open();
                    OdbcDataAdapter dacc = new OdbcDataAdapter("Select*from facultySched_tbl where faculty='" + cmbFacs.Text + "'and section!='" + classsec + "'and days='" + day + "'", con);
                    DataTable dtcc = new DataTable();
                    dacc.Fill(dtcc);
                    con.Close();
                    if (dtcc.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtcc.Rows.Count; i++)
                        {
                            float dbstart = Convert.ToSingle(dtcc.Rows[i].ItemArray[6].ToString().Substring(0, 2) + dtcc.Rows[i].ItemArray[6].ToString().Substring(3, 2));
                            float dbend = Convert.ToSingle(dtcc.Rows[i].ItemArray[7].ToString().Substring(0, 2) + dtcc.Rows[i].ItemArray[7].ToString().Substring(3, 2));

                            if ((dbend == fend && dbstart == fstart) || (dbstart == fstart && fend >= dbend) || (dbend == fend && fstart <= dbstart))
                            {
                                MessageBox.Show("faculty already have a schedule on the set time." + "\n\nClass name: " + dtcc.Rows[i].ItemArray[1].ToString() + " - " + dtcc.Rows[i].ItemArray[2].ToString() + "\nSubject: " + dtcc.Rows[i].ItemArray[3].ToString(), "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }
                


                setupsaveoperation();
                btnUpdate.Text = "Update";
                btnClear.Text = "Clear";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                if (dgvSearch.Rows.Count >= 1)
                {
                    dgvSearch.Rows[0].Selected = true;
                }
                if (pnlhead.Rows.Count >= 1)
                {
                    pnlhead.Rows[0].Selected = true;
                }
            }
        }

        public void setupsaveoperation()
        {
            string soc = dudHrStart.Text + ":" + dudMinStart.Text + " " + cmbDayStart.Text;
            string eoc = dudHourEnd.Text + ":" + dudMinEnd.Text + " " + cmbDayEnd.Text;

            setupdayofsubj();
            string type = "";
            if (cmbsub.Text != "Recess" && cmbsub.Text != "Lunch break" && cmbsub.Text != "Flag cer./Class prep." && cmbsub.Text != "Club" && cmbsub.Text != "Homeroom")
            {
                type = "Academic";
            }
            else
            {
                type = " ";
            }

            con.Open();
            string updsch = "Update schedule_tbl set subject='" + cmbsub.Text + "',faculty='" + cmbFacs.Text
                + "',room='" + cmbRoom.Text + "',start='" + soc + "',end='" + eoc + "',days='" + day + "' where secid='" + lblKey.Text + "' and subject='" + tempsubj + "'";
            OdbcCommand cmdUpd = new OdbcCommand(updsch, con);
            cmdUpd.ExecuteNonQuery();
            con.Close();
            setupviewbysection(lblKey.Text);

             con.Open();
             OdbcDataAdapter dafindEmpno = new OdbcDataAdapter("Select empno from employees_tbl where (select concat(firstname,' ',middlename,' ',lastname))='" + cmbFacs.Text + "' and position='faculty'", con);
             DataTable dtfindEmpno = new DataTable();
             dafindEmpno.Fill(dtfindEmpno);
             con.Close();

             if (dtfindEmpno.Rows.Count > 0)
             {
                 con.Open();
                 OdbcDataAdapter dafac = new OdbcDataAdapter("Select*from facultySched_tbl where empno='" + dtfindEmpno.Rows[0].ItemArray[0].ToString() + "'", con);
                 DataTable dtdafac = new DataTable();
                 dafac.Fill(dtdafac);
                 con.Close();

                 if (dtdafac.Rows.Count > 0)
                 {
                     con.Open();
                     string del = "Delete from facultySched_tbl where subject='" + cmbsub.Text + "'and faculty='" + selectedfac + "'";
                     OdbcCommand cmdel = new OdbcCommand(del, con);
                     cmdel.ExecuteNonQuery();
                     con.Close();

                     con.Open();
                     string addtoFacshed = "Insert Into facultySched_tbl(empno,level,section,subject,faculty,room,start,end,days,type)values('" + dtfindEmpno.Rows[0].ItemArray[0].ToString() + "','" +
                         classgrade + "','" + classsec + "','" + cmbsub.Text + "','" + cmbFacs.Text + "','" + cmbRoom.Text + "','" + soc + "','" + eoc + "','" + day + "','"+type+"')";
                     OdbcCommand cmdfind = new OdbcCommand(addtoFacshed, con);
                     cmdfind.ExecuteNonQuery();
                     con.Close();

                     con.Open();
                     string updtoFacshed = "Update facultySched_tbl set subject='" + cmbsub.Text + "',faculty='" + cmbFacs.Text
                     + "',room='" + cmbRoom.Text + "',start='" + soc + "',end='" + eoc + "',days='" + day + "' where empno='" + dtfindEmpno.Rows[0].ItemArray[0].ToString() + "' and subject='" + tempsubj + "'and section='" + classsec + "'";
                     OdbcCommand cmdfind1 = new OdbcCommand(updtoFacshed, con);
                     cmdfind1.ExecuteNonQuery();
                     con.Close();

                     btnAdd.Enabled = false;
                     btnClear.Text = "Clear";
                     MessageBox.Show("schedule successfully updated", "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }
                 else
                 {
                    
                     con.Open();
                     string del = "Delete from facultySched_tbl where subject='"+cmbsub.Text+"'and faculty='"+selectedfac+"'";
                     OdbcCommand cmdel= new OdbcCommand(del, con);
                     cmdel.ExecuteNonQuery();
                     con.Close();

                     con.Open();
                     string addtoFacshed = "Insert Into facultySched_tbl(empno,level,section,subject,faculty,room,start,end,days,type)values('" + dtfindEmpno.Rows[0].ItemArray[0].ToString() + "','" +
                         classgrade + "','" + classsec + "','" + cmbsub.Text + "','" + cmbFacs.Text + "','" + cmbRoom.Text + "','" + soc + "','" + eoc + "','" + day + "','"+type+"')";
                     OdbcCommand cmdfind = new OdbcCommand(addtoFacshed, con);
                     cmdfind.ExecuteNonQuery();
                     con.Close();
                     MessageBox.Show("schedule was swapped to " + cmbFacs.Text, "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }
             }
             else
             {

                 MessageBox.Show("schedule successfully updated", "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
             cmbFacs.Items.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete?", "User maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (lblKey.Text == "")
                {
                    MessageBox.Show("no class selected!", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cmbsub.Text == "" || cmbFacs.Text == "" || cmbRoom.Text == "" || cmbday1.Text == "" || cmbday2.Text == "" ||
                    cmbDayStart.Text == "" || cmbDayEnd.Text == "" || dudHrStart.Text == "" || dudMinStart.Text == "" || dudHourEnd.Text == "" || dudMinEnd.Text == "")
                {
                    if (cmbsub.Text == "Recess" || cmbsub.Text == "Lunch break" || cmbsub.Text == "Flag cer./Class prep." || cmbsub.Text == "Club" || cmbsub.Text == "Homeroom")
                    {
                        setupDeleteForClass();
                    }
                    else
                    {
                        MessageBox.Show("fill out all fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; 
                    }
                   
                }
                else
                {
                    setupDeleteForClass();
                }
            }
            else
            {
                return;
            }
        }

        public void setupDeleteForClass()
        {
            con.Open();
            string delSch = "Delete from schedule_tbl where secid='"+lblKey.Text+"'and subject='"+tempsubj+"'";
            OdbcCommand cmdDel = new OdbcCommand(delSch, con);
            cmdDel.ExecuteNonQuery();

            string delSch1 = "Delete from facultysched_tbl where level='" + classgrade + "'and subject='" + tempsubj + "'and section='"+classsec+"'and faculty='"+cmbFacs.Text+"'";
            OdbcCommand cmdDel1 = new OdbcCommand(delSch1, con);
            cmdDel1.ExecuteNonQuery();

            con.Close();

            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnUpdate.Text = "Update";
            btnClear.Text = "Clear";
            setupclear();
            setupviewbysection(lblKey.Text);
            cmbFacs.Items.Clear();
            MessageBox.Show("schedule successfully deleted", "Schedule maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvFacs.RowFilter = string.Format("Faculty LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvFacs;

            if (dgvSearch.Rows.Count > 0 && txtSearch.Text == "")
            {
                pnlwith.Visible = false;
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text != "")
            {
                pnlwith.Visible = true;
                lblmemowith.Text = "0 search result";
            }
            if (dgvSearch.Rows.Count == 0 && txtSearch.Text == "")
            {
                pnlwith.Visible = true;
                lblmemowith.Location = new Point(312, 8);
                lblmemowith.Text = "no schedule found!";
            }
        }

        private void cmbsub_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(btnUpdate.Text=="Update")
            {
                if ((cmbsub.Text != "Recess" || cmbsub.Text != "Lunch break" || cmbsub.Text != "Flag cer./Class prep." || cmbsub.Text != "Club" || cmbsub.Text != "Homeroom") && (btnUpdate.Text != "Update"))
                {
                    if (cmbsub.Text != "Recess" || cmbsub.Text != "Lunch break" )
                    {
                        day1code = "Mon";
                        day2code = "Fri";
                        day = "Mon-Fri";
                        MINDAY = 1;
                        MAXDAY = 5;
                    }

                    cmbFacs.Enabled = true;
                    cmbday1.Enabled = true;
                    cmbday2.Enabled = true;
                }
                else
                {
                    if (cmbsub.Text == "Music" || cmbsub.Text == "Arts" || cmbsub.Text == "P.E." || cmbsub.Text == "Health")
                    {
                        setupallfaculty("M.A.P.E.H.");
                        //cmbFacs.Enabled = false;
                    }
                    else if (cmbsub.Text == "Reading" || cmbsub.Text == "Writing" || cmbsub.Text=="Language")
                    {
                        setupallfaculty("English");
                        //cmbFacs.Enabled = false;
                    }
                    else
                    {
                        setupallfaculty(cmbsub.Text);
                        cmbFacs.Enabled = false;
                    }

                    if (cmbsub.Text == "Recess" || cmbsub.Text == "Lunch break")
                    {
                        day1code = "Mon";
                        day2code = "Fri";
                        day = "Mon-Fri";
                        cmbday1.Enabled = false;
                        cmbday2.Enabled = false;
                    }
                    else
                    {
                        cmbday1.Enabled = true;
                        cmbday2.Enabled = true;
                    }
                }

                if ((cmbsub.Text != "Flag cer./Class prep.") && (btnUpdate.Text != "Update"))
                {
                    cmbRoom.Enabled = true;
                }
                else
                {
                    setuprooms(classgrade,classsec);
                    cmbRoom.Enabled = false;
                }
            }

            if (btnAdd.Enabled == true)
            {
                if ((cmbsub.Text == "Recess" || cmbsub.Text == "Lunch break" || cmbsub.Text == "Flag cer./Class prep." || cmbsub.Text == "Club" || cmbsub.Text == "Homeroom"))
                {
                    //setupallfaculty();
                    cmbFacs.Enabled = false;
                }
                else
                {
                    cmbFacs.Enabled = true;
                }


                if ((cmbsub.Text == "Flag cer./Class prep."))
                {
                    setuprooms(classgrade,classsec);
                    cmbRoom.Enabled = false;
                }
                else
                {

                    cmbRoom.Enabled = true ;
                }
            }

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
           
        }

        private void dgvFac_Click(object sender, EventArgs e)
        {
           
        }

        private void pnlhead_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbsub_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = schedlog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Hide();
            levmain.levlog = schedlog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = schedlog;
            facmain.VISITED = VISITED;
            facmain.Show();
            this.Hide();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = schedlog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void pnlhead_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            lblcount.Location = new Point(3, 6);
            lblAcdNonCount.Location = new Point(3, 25);
            lblcount.Text = pnlhead.Rows.Count + " subjects/activities in this class";
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = schedlog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = schedlog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = schedlog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void cmbDayStart_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbday1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                    casmain.cashlog = schedlog;
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
                    regmain.reglog = schedlog;
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
                    pmf.prinlog = schedlog;
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
                    empf.faclog = schedlog;
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
                frmadm.admlog = schedlog;
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
                formPay.paylog = schedlog;
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
                formStudRec.asslog = schedlog;
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
                formstdgrd.grdlog = schedlog;
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
                stud.studlog = schedlog;
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
                facf.facinfolog = schedlog;
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
                frmFacAdv.advlog = schedlog;
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
                frmSec.seclog = schedlog;
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
                rfac.replog = schedlog;
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
                dgvm.SelectedRows[0].Cells[0].Style.SelectionBackColor = Color.LightGreen;
                return;
            }
            if (dgvm.SelectedRows[0].Cells[0].Value.ToString() == "  About us")
            {
                frmEmpAbout about = new frmEmpAbout();
                this.Hide();
                about.ablog = schedlog;
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

        private void dgvm_Click(object sender, EventArgs e)
        {
            if (dgvm.Rows.Count < 0)
            {
                return;
            }
        }

        private void dgvm_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Hand;
        }

        private void dgvm_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvm.Cursor = Cursors.Default;
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Scheduling")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvm_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "  Scheduling")
            {
                dgvm.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gainsboro;
            }
        }
    }
}
