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
    public partial class frmAssignRoom : Form
    {
        public string asromlog, VISITED, roomalcid;
        public DataView dvSetRoom;
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public frmAssignRoom()
        {
            InitializeComponent();
        }

        private void frmAssignRoom_Load(object sender, EventArgs e)
        {

            lblLogger.Text = asromlog;
            lblLoggerPosition.Text = "Admin";
            btnAssRoom.BackColor = Color.LightGreen;
            if (VISITED.Contains("Assign room") == false)
            {
                VISITED += "   Assign room";

            }
            pnlnotify.Visible = false;
            setupLevelList();
            cmbFilter.Text = "Occupied";
            //setupview_roomallocation();
        }

        private void btnAssRoom_Click(object sender, EventArgs e)
        {
            return;
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

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupSectionsPerLevel(cmbLevel.Text);
        }

        public void setupSectionsPerLevel(string lev)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select section from section_tbl where level='" + lev + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            cmbSection.Items.Clear();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSection.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
            }
        }

        public void setupview_roomallocation()
        {
            con.Open();

            OdbcDataAdapter da = new OdbcDataAdapter("Select name as 'Room',grade as 'Grade',section as 'Section' from roomallocation_tbl where type='Room' and section<>''", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            OdbcDataAdapter dafree = new OdbcDataAdapter("Select count(section) from roomallocation_tbl where type='Room' and section=''", con);
            DataTable dtfree = new DataTable();
            dafree.Fill(dtfree);

            OdbcDataAdapter daoccupied = new OdbcDataAdapter("Select count(section) from roomallocation_tbl where type='Room' and section<>''", con);
            DataTable dtoccupied = new DataTable();
            daoccupied.Fill(dtoccupied);

            con.Close();
            dvSetRoom = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvSetRoom;

                dgvSearch.Columns[0].Width = 185;
                dgvSearch.Columns[1].Width = 130;
                dgvSearch.Columns[2].Width = 160;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }



            if (dtfree.Rows.Count > 0 && dtoccupied.Rows.Count > 0)
            {
                lblResult.Text = "Free room: " + dtfree.Rows[0].ItemArray[0].ToString();
                lblTyperes.Text = "Occupied room: " + dtoccupied.Rows[0].ItemArray[0].ToString();
            }
            else if (dtfree.Rows.Count > 0 && dtoccupied.Rows.Count < 0)
            {
                lblResult.Text = "Free room: " + dtfree.Rows[0].ItemArray[0].ToString();
                lblTyperes.Text = "Occupied room: 0";
            }
            else if (dtfree.Rows.Count < 0 && dtoccupied.Rows.Count > 0)
            {
                MessageBox.Show("");
                lblResult.Text = "Free room: 0";
                lblTyperes.Text = "Occupied room: " + dtoccupied.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                lblResult.Text = "Free room: 0";
                lblTyperes.Text = "Occupied room: 0";
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (cmbLevel.Text == "" || cmbSection.Text == "")
            {
                MessageBox.Show("fill out required fields.", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from roomallocation_tbl where type='Room' and id='" + roomalcid + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0].ItemArray[4].ToString() == "")
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select*from roomallocation_tbl where section='" + cmbSection.Text + "' and grade='" + cmbLevel.Text + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            MessageBox.Show("section already assigned to room " + dtt.Rows[0].ItemArray[1].ToString(), "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {

                            con.Open();
                            string updateroomtoallocationtbl0 = "Update roomallocation_tbl set grade='" + cmbLevel.Text + "',section='" + cmbSection.Text + "' where id='" + roomalcid + "'";
                            OdbcCommand cmdUpdateRoomtoallocationtbl0 = new OdbcCommand(updateroomtoallocationtbl0, con);
                            cmdUpdateRoomtoallocationtbl0.ExecuteNonQuery();
                            con.Close();

                            btnRemove.Enabled = false;
                            btnSet.Enabled = false;
                            lblKey.Text = "";
                            setupview_roomallocation();
                            MessageBox.Show("room successfully occupied", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            txtSearch.Focus();
                        }
                    }
                    else
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select*from roomallocation_tbl where section='" + cmbSection.Text + "' and grade='" + cmbLevel.Text + "'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count > 0)
                        {
                            DialogResult res = MessageBox.Show("section already assigned," + "\nwould you like to replace?" + "", "Room maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (res == DialogResult.Yes)
                            {
                                con.Open();
                                string updateroomtoallocationtbl1 = "Update roomallocation_tbl set grade='" + cmbLevel.Text + "',section='" + cmbSection.Text + "' where id='" + roomalcid + "'";
                                OdbcCommand cmdUpdateRoomtoallocationtbl1 = new OdbcCommand(updateroomtoallocationtbl1, con);
                                cmdUpdateRoomtoallocationtbl1.ExecuteNonQuery();
                                con.Close();

                                btnRemove.Enabled = false;
                                btnSet.Enabled = false;
                                lblKey.Text = "";
                                setupview_roomallocation();
                                MessageBox.Show("class successfully replaced", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                txtSearch.Focus();
                            }
                            else if (res == DialogResult.No)
                            {
                                return;
                            }
                            else if (res == DialogResult.OK)
                            {
                                return;
                            }
                            else
                            {
                                lblKey.Text = "";
                                btnSet.Enabled = false;
                                btnRemove.Enabled = false;
                                cmbLevel.SelectedIndex = -1;
                                cmbSection.SelectedIndex = -1;
                                dgvSearch.Rows[0].Selected = true;
                            }
                        }
                        else
                        {
                            con.Open();
                            string updateroomtoallocationtbl1 = "Update roomallocation_tbl set grade='" + cmbLevel.Text + "',section='" + cmbSection.Text + "' where id='" + roomalcid + "'";
                            OdbcCommand cmdUpdateRoomtoallocationtbl1 = new OdbcCommand(updateroomtoallocationtbl1, con);
                            cmdUpdateRoomtoallocationtbl1.ExecuteNonQuery();
                            con.Close();

                            btnRemove.Enabled = false;
                            btnSet.Enabled = false;
                            lblKey.Text = "";
                            setupview_roomallocation();
                            MessageBox.Show("class successfully replaced", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            txtSearch.Focus();
                        }
                    }
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            con.Open();
            string updateroomtoallocationtbl = "Update roomallocation_tbl set grade='',section='' where id='" + roomalcid + "'";
            OdbcCommand cmdUpdateRoomtoallocationtbl = new OdbcCommand(updateroomtoallocationtbl, con);
            cmdUpdateRoomtoallocationtbl.ExecuteNonQuery();
            con.Close();

            btnRemove.Enabled = false;
            btnSet.Enabled = false;
            lblKey.Text = "";
            setupview_roomallocation();
            MessageBox.Show("room is available for allocation", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnAsgClr_Click(object sender, EventArgs e)
        {
            setupLevelList();
            setupSectionsPerLevel(cmbLevel.Text);
            btnSet.Enabled = false;
            btnRemove.Enabled = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvSetRoom.RowFilter = string.Format("Room LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvSetRoom;

            if (dgvSearch.Rows.Count > 0 && txtSearch.Text == "")
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

            string roomalc = "";
            string grde = "";
            string sect = "";
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
            {
                roomalc = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }
            if (dgvSearch.SelectedRows[0].Cells[1].Value.ToString() != "")
            {
                grde = dgvSearch.SelectedRows[0].Cells[1].Value.ToString();
                cmbLevel.Text = grde;
            }
            if (dgvSearch.SelectedRows[0].Cells[2].Value.ToString() != "")
            {
                sect = dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
                cmbSection.Text = sect;
            }
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id from roomallocation_tbl where name='" + roomalc + "'and grade='" + grde + "'and section='" + sect + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                roomalcid = dt.Rows[0].ItemArray[0].ToString();
            }
            //lblKey.Text = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();

            if (dgvSearch.SelectedRows[0].Cells[2].Value.ToString() == "")
            {
                btnSet.Enabled = true;
                btnRemove.Enabled = false;
            }
            else
            {
                btnSet.Enabled = false;
                btnRemove.Enabled = true;
            }
        }

        private void btnStudStat_Click(object sender, EventArgs e)
        {
            frmStudentStats stform = new frmStudentStats();
            this.Dispose();
            stform.statlog = asromlog;
            stform.VISITED = VISITED;
            stform.Show();
        }

        private void btnEdays_Click(object sender, EventArgs e)
        {
            frmEnrollmentDays eform = new frmEnrollmentDays();
            this.Dispose();
            eform.edlog = asromlog;
            eform.VISITED = VISITED;
            eform.Show();
        }

        private void btncoa_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Dispose();
            actform.actlog = asromlog;
            actform.VISITED = VISITED;
            actform.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abm = new frmAboutMaintenance();
            this.Dispose();
            abm.amlog = asromlog;
            abm.VISITED = VISITED;
            abm.Show();
        }

        private void btnPriority_Click(object sender, EventArgs e)
        {
            frmPrioritySec priorsec = new frmPrioritySec();
            this.Dispose();
            priorsec.priorlog = asromlog;
            priorsec.VISITED = VISITED;
            priorsec.Show();
        }

        private void btnHomeMainte_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            this.Dispose();
            hm.adminlog = asromlog;
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
            string setOut = "Update audittrail_tbl set logout='" + time + "',visited='" + VISITED + "'Where logout='" + def + "'";
            OdbcCommand cmd = new OdbcCommand(setOut, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void frmAssignRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Dispose();
            hf.Show();
        }



        private void dgvSearch_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dgvSearch.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
            }
            else
            {
                pnlnotify.Visible = true;
            }
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "Occupied")
            {
                setupview_roomallocation();
               
            }
            else
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select name as 'Room',grade as 'Grade',section as 'Section' from roomallocation_tbl where type='Room' and Section=''", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataView dvOcc = new DataView(dt);
                con.Close();
                dgvSearch.DataSource = dvOcc;
                if (dt.Rows.Count > 0)
                {
                    btnRemove.Enabled = true;
                    pnlnotify.Visible = false;
                    dgvSearch.DataSource = null;
                    dgvSearch.DataSource = dvOcc;

                    dgvSearch.Columns[0].Width = 185;
                    dgvSearch.Columns[1].Width = 130;
                    dgvSearch.Columns[2].Width = 160;
                }
                else
                {
                    btnRemove.Enabled = false;
                    dgvSearch.DataSource = null;
                    pnlnotify.Visible = true;
                    lblnote.Text = "no items found...";
                }
            }
        }
    }
}
