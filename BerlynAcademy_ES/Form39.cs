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
    public partial class frmPrioritySec : Form
    {
        public string priorlog,id,orgrank,VISITED;
        public DataView dvsec;
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public frmPrioritySec()
        {
            InitializeComponent();
        }

        private void frmPrioritySec_Load(object sender, EventArgs e)
        {
            lblLogger.Text = priorlog;
            lblLoggerPosition.Text = "Admin";
            btnPriority.BackColor = Color.LightGreen;
            setupLevelList();
          
            if (VISITED.Contains("Priority section") == false)
            {
                VISITED += "   Priority section";

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
               
                cmbLevel.Items.Clear();
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbLevel.Items.Add(dt.Rows[u].ItemArray[0].ToString());
                   
                }
            }
        }

        private void btnPriority_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnStudStat_Click(object sender, EventArgs e)
        {
            frmStudentStats stform = new frmStudentStats();
            this.Dispose();
            stform.statlog = priorlog;
            stform.VISITED = VISITED;
            stform.Show();
        }

        private void btnEdays_Click(object sender, EventArgs e)
        {
            frmEnrollmentDays eform = new frmEnrollmentDays();
            this.Dispose();
            eform.edlog = priorlog;
            eform.VISITED = VISITED;
            eform.Show();
        }

        private void btncoa_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Dispose();
            actform.actlog = priorlog;
            actform.VISITED = VISITED;
            actform.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abm = new frmAboutMaintenance();
            this.Dispose();
            abm.amlog = priorlog;
            abm.VISITED = VISITED;
            abm.Show();
        }

        private void btnHomeMainte_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            this.Dispose();
            hm.adminlog = priorlog;
            hm.VISITED = VISITED;
            hm.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
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

        private void frmPrioritySec_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Dispose();
            hf.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvsec.RowFilter = string.Format("Section LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvsec;
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

        public void setupView()
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id,section as 'Section',rank as 'Priority no.' from section_tbl where level='" + cmbLevel.Text + "'", con);
            da.Fill(dt);
            con.Close();
            dvsec = new DataView(dt);
            dgvSearch.DataSource = dvsec;
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.Open();
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id,section as 'Section',rank as 'Priority no.' from section_tbl where level='"+cmbLevel.Text+"'", con);
            da.Fill(dt);
            con.Close();
            dvsec = new DataView(dt);
            dgvSearch.DataSource = dvsec;

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvsec;
                dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                dgvSearch.Columns[0].Width = 0;
                dgvSearch.Columns[1].Width = 330;
                dgvSearch.Columns[2].Width = 150;


                cmbRank.Items.Clear();
                int num = 1;
                for (int u = 0; u < dt.Rows.Count; u++)
                {
                    cmbRank.Items.Add(num);
                    num++;

                }
               
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }
        }

        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            lblResult.Text = "number of section: " + dgvSearch.Rows.Count.ToString();
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <=0)
            {
                return;
            }

            btnUpd.Text = "Update";
            btnClr.Text = "Clear";
            btnUpd.Enabled = true;
            cmbRank.Enabled = false;
            cmbRank.Text = dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
            orgrank = cmbRank.Text;
            if (dgvSearch.SelectedRows[0].Cells[2].Value.ToString() != "" && dgvSearch.SelectedRows[0].Cells[2].Value.ToString() != "0")
            {
                btnRem.Enabled = true;
            }
            id = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void btnUpd_Click(object sender, EventArgs e)
        {
            if (btnUpd.Text == "Update")
            {
                btnUpd.Text = "Save";
                btnClr.Text = "Cancel";
                cmbRank.Enabled = true;
                btnRem.Enabled = false;
            }
            else
            {
                if (cmbRank.Text == "" || cmbLevel.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    con.Open();
                    DataTable dt = new DataTable();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from section_tbl where level='" + cmbLevel.Text + "' and id<>'"+id+"'and rank='"+cmbRank.Text+"'", con);
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("remove rank of "+dt.Rows[0].ItemArray[1].ToString()+" to change.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    setup_save();
                    btnUpd.Text = "Update";
                    btnClr.Text = "Clear";
                    btnUpd.Enabled = false;
                  
                    if (dgvSearch.Rows.Count >= 1)
                    {
                        dgvSearch.Rows[0].Selected = true;
                    }
                }
            }
        }

        public void setup_save()
        {
            con.Open();
            string update = "Update section_tbl set rank='" + cmbRank.Text + "'where id='" + id + "'";
            OdbcCommand cmdUpdate= new OdbcCommand(update, con);
            cmdUpdate.ExecuteNonQuery();
            con.Close();
            setupView();

            int minrank = 0;
            int maxrank = 0;
            //MIN-----------------
            con.Open();
            OdbcDataAdapter daa = new OdbcDataAdapter("Select min(rank) from section_tbl where level='" + cmbLevel.Text + "'and rank<>'0'", con);
            DataTable dtt = new DataTable();
            daa.Fill(dtt);
            con.Close();
            if (dtt.Rows.Count > 0)
            {
                minrank = Convert.ToInt32(dtt.Rows[0].ItemArray[0].ToString());
            }
            //MAX-----------------
            con.Open();
            OdbcDataAdapter daaa = new OdbcDataAdapter("Select max(rank) from section_tbl where level='" + cmbLevel.Text + "'", con);
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
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from section_tbl where level='" + cmbLevel.Text+ "'and rank='" + minrank + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0].ItemArray[5].ToString() == "no")
                    {
                        con.Open();
                        string update2 = "Update section_tbl set status='" + "active" + "'where id='" + dt.Rows[0].ItemArray[0].ToString() + "'";
                        OdbcCommand cmd2 = new OdbcCommand(update2, con);
                        cmd2.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        con.Open();
                        string update1 = "Update section_tbl set status='" + "inactive" + "'where level='" + cmbLevel.Text + "'and isFull='yes'";
                        OdbcCommand cmd1 = new OdbcCommand(update1, con);
                        cmd1.ExecuteNonQuery();
                        con.Close();

                    }
                }
                minrank++;
            }

            con.Open();
            OdbcDataAdapter daact = new OdbcDataAdapter("Select min(rank)from section_tbl where status='active'and level='" + cmbLevel.Text + "'and isFull='no'", con);
            DataTable dtact = new DataTable();
            daact.Fill(dtact);
            con.Close();
            if (dtact.Rows.Count >0)
            {
                con.Open();
                OdbcDataAdapter daID = new OdbcDataAdapter("Select id from section_tbl where status='active'and level='" + cmbLevel.Text + "'and isFull='no' and rank='"+dtact.Rows[0].ItemArray[0].ToString()+"'", con);
                DataTable dtID = new DataTable();
                daID.Fill(dtID);
                con.Close();
                if (dtID.Rows.Count > 0)
                {
                    con.Open();
                    string update1 = "Update section_tbl set status='" + "active" + "'where id='" + dtID.Rows[0].ItemArray[0].ToString() + "'";
                    OdbcCommand cmd1 = new OdbcCommand(update1, con);
                    cmd1.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    string update11 = "Update section_tbl set status='" + "inactive" + "'where level='" + cmbLevel.Text + "'and id<>'" + dtID.Rows[0].ItemArray[0].ToString() + "'";
                    OdbcCommand cmd11 = new OdbcCommand(update11, con);
                    cmd11.ExecuteNonQuery();
                    con.Close();

                }

            }
        



            MessageBox.Show("rank successfully updated", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            if (btnClr.Text == "Clear")
            {
                btnUpd.Enabled = false;
                btnRem.Enabled = false;
                btnUpd.Text = "Update";
                cmbRank.SelectedIndex = -1;
            }
            else
            {
               
                btnClr.Text = "Clear";
                btnUpd.Text = "Update";
                //primarykey = lblKey.Text;
                cmbRank.Text = orgrank;
                cmbRank.Enabled = false;
            }
        }

        private void btnRem_Click(object sender, EventArgs e)
        {
            con.Open();
            string update = "Update section_tbl set rank='" + "" + "'where id='" + id + "'";
            OdbcCommand cmdUpdate = new OdbcCommand(update, con);
            cmdUpdate.ExecuteNonQuery();
            con.Close();
            setupView();

            btnRem.Enabled = false;
            btnUpd.Text = "Update";
            btnUpd.Enabled = true;
            MessageBox.Show("rank successfully removed", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnAssRoom_Click(object sender, EventArgs e)
        {
            frmAssignRoom asrom = new frmAssignRoom();
            this.Dispose();
            asrom.asromlog = priorlog;
            asrom.VISITED = VISITED;
            asrom.Show();
        }
    }
}
