using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;


namespace BerlynAcademy_ES
{
    public partial class frmBackup : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=sa;DB=berlyn");
        public string backlog,VISITED;
        public frmBackup()
        {
            InitializeComponent();
        }

        private void frmBackup_Load(object sender, EventArgs e)
        {
            lblLogger.Text = backlog;
            lblLoggerPosition.Text = "Admin";
            //btnHome.Text = "          " + backlog;
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            //this.BackColor = Color.FromArgb(49, 79, 142);
            btnBackup.BackColor = Color.LightGreen;

            if (VISITED.Contains("Back-up and Recovery") == false)
            {
                VISITED += "   Back-up and Recovery";
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
            frmEmpLogin homef = new frmEmpLogin();
            this.Hide();
            homef.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maine = new frmMaintenance();
            this.Hide();
            maine.adminlog = backlog;
            maine.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subf = new frmSubject();
            this.Hide();
            subf.wholog = backlog;
            subf.Show();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection secf = new frmSection();
            this.Hide();
            secf.secwholog = backlog;
            secf.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roomf = new frmRoom();
            this.Hide();
            roomf.logger = backlog;
            roomf.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched schedf = new frmSched();
            this.Hide();
            schedf.schedlog = backlog;
            schedf.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqf = new frmRequirement();
            this.Hide();
            reqf.reqlog = backlog;
            reqf.Show();

        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feef = new frmFee();
            this.Hide();
            feef.feelog = backlog;
            feef.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discf = new frmDiscount();
            this.Hide();
            discf.disclog = backlog;
            discf.Show();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmActivity actf = new frmActivity();
            this.Hide();
            actf.actlog = backlog;
            actf.Show();
        }

        private void btnAud_Click(object sender, EventArgs e)
        {
            frmAudit audf = new frmAudit();
            this.Hide();
            audf.auditlogger = backlog;
            audf.VISITED = VISITED;
            audf.Show();
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abtf = new frmAboutMaintenance();
            this.Hide();
            abtf.amlog = backlog;
            abtf.Show();
        }

        private void frmBackup_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin homef = new frmEmpLogin();
            this.Hide();
            homef.Show();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (fbdBrowser.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fbdBrowser.SelectedPath;
                btnBU.Enabled = true;
            }

        }

        private void btnBU_Click(object sender, EventArgs e)
        {
  
            saveFileDialog1.Filter = "Text files (*.sql)|*.sql|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Process sd = null;
                    ProcessStartInfo r1 = new ProcessStartInfo(@"C:\Program Files\MySQL\MySQL Server 5.5\bin\mysqldump.exe", "--databases berlyn --compress --routines --triggers --add-drop-database --add-drop-table --add-locks --extended-insert --password=sa --user=root --disable-keys --quick --comments --complete-insert --result-file=" + saveFileDialog1.FileName);

                    r1.CreateNoWindow = true;
                    r1.WorkingDirectory = @"C:\Program Files\MySQL\MySQL Server 5.5\bin";
                    r1.UseShellExecute = false;
                    r1.WindowStyle = ProcessWindowStyle.Minimized;
                    r1.RedirectStandardInput = false;

                    sd = Process.Start(r1);
                    sd.WaitForExit();

                    if (!sd.HasExited) {
                        sd.Close();
                    }
                    sd.Dispose();
                    r1 = null;
                    sd = null;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured During DB backup process." + ex.ToString());

                }
                  

                /*string constri = "Data source=.\\SQLEXPRESS;User Id=LEEVC;Password=leebert";
                try
                {
                    SqlConnection cont = new SqlConnection(constri);
                    cont.Open();
                    string bu = "Backup database berlyn to disc='" + saveFileDialog1.FileName + "'";
                    SqlCommand cmd = new SqlCommand(bu, cont);
                    //OdbcCommand cmd = new OdbcCommand(bu, cont);
                    cmd.ExecuteNonQuery();
                    cont.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured During DB backup process." + ex.ToString());
                  
                }*/

                /*MessageBox.Show(saveFileDialog1.FileName);
                string path = @"C:\Program Files\MySQL\MySQL Server 5.0\bin\mysqldump.exe -u " + "root" + " -p " + "leebert"+ "> "+saveFileDialog1.FileName+ "";
                Process p = new Process();
                p.StartInfo.FileName = "@"+path;

                p.Start();*/

                /*try
                {
                    string connectionString1 = ("SERVER=.\\SQLEXPRESS;Database=berlyn;Integrated Security=True; User=root;Password=leebert;Connection Timeout=15;Trusted_Connection=true;");
                    SqlConnection cn = new SqlConnection(connectionString1);
                    cn.Open();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;
                    cmd.CommandText = "BACKUP DATABASE berlyn TO DISK = '" + saveFileDialog1.FileName + "'";

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cn;
                    reader = cmd.ExecuteReader();
                    cn.Close();
                    MessageBox.Show("Database Backup Successfull.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured During DB backup process.\n" + ex.ToString());

                }*/
                
              
                //con.Open();
                //string backup = "Backup database berlyn to disk='" +saveFileDialog1.FileName + "'";
               // string backup = "BACKUP DATABASE berlyn TO disk = 'C:\\Castro\\BackupFileName.bak'";
                //string backup = "BACKUP DATABASE berlyn TO DISK='" + txtPath.Text + "\\" + "berlyn-" + DateTime.Now.Ticks.ToString()+".bak'";
               // OdbcCommand cmd = new OdbcCommand(backup, con);
               // cmd.ExecuteNonQuery();
               // con.Close();
                //MessageBox.Show("Database successfuly backup","Utilities",MessageBoxButtons.OK,MessageBoxIcon.Information);
                
            }
        }
       
        private void btnBackup_Click(object sender, EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            
        }

        private void btnBackup_Click_1(object sender, EventArgs e)
        {
            return;
        }

        private void btnHomeMainte_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = backlog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void btnUserAcc_Click(object sender, EventArgs e)
        {
            frmUserAccessLevel ualform = new frmUserAccessLevel();
            this.Hide();
            ualform.acclog = backlog;
            ualform.VISITED = VISITED;
            ualform.Show();
        }

       
    }
}
