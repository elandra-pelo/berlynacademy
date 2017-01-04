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
    public partial class frmRoom : Form
    {
        public string logger,primarykey,selectedroom,selectedtype,selectedcapacity,roomalcid,VISITED;
        public DataView dvRoom,dvSetRoom;
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public frmRoom()
        {
            InitializeComponent();
        }

        private void frmRoom_Load(object sender, EventArgs e)
        {
            cmbOperation.Text = "Add room";
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);

            //this.BackColor = Color.FromArgb(49, 79, 142);
            lblLogger.Text = logger;
            lblLoggerPosition.Text = "Admin";
            //btnHome.Text = "          " + logger;
            btnRoom.BackColor = Color.LightGreen;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            pnlnotify.Visible = false;

            if (VISITED.Contains("Room") == false)
            {
                VISITED += "   Room";
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

        private void cmbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOperation.Text == "Add room")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    pnlnotify.Visible = true;
                }

                cmbLevel.SelectedIndex = -1;
                cmbSection.SelectedIndex = -1;
                txtCapacity.Clear();
                
                lblKey.Text = "";
                toolTip1.SetToolTip(txtSearch, "search room");
                setupview_room();

                pnlAddRoom.Visible = true;
               // pnlAddRoom.Location = new Point(6, 57);
                pnlSetRoom.Visible = false;
            }
            if (cmbOperation.Text == "Assign room")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    pnlnotify.Visible = true;
                }
                setupLevelList();
                txtRoom.Clear();
                btnAdd.Enabled = true;
                cmbType.SelectedIndex = -1;

                lblKey.Text = "";
                toolTip1.SetToolTip(txtSearch, "search room");
                setupview_roomallocation();

                btnSet.Enabled = false;
                btnRemove.Enabled = false;

                pnlAddRoom.Visible = false;
                pnlSetRoom.Visible = true;
                pnlSetRoom.Location = new Point(6, 57);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtRoom.Text == "" || cmbType.Text == ""|| txtCapacity.Text=="")
            {
                MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                con.Open();
                OdbcDataAdapter dac = new OdbcDataAdapter("Select*from section_tbl where IsFull='" + "no" + "'", con);
                DataTable dtc = new DataTable();
                dac.Fill(dtc);
                con.Close();
                if (dtc.Rows.Count > 0)
                {
                    MessageBox.Show("Adding not allowed some room is not full.", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from room_tbl where name='" + txtRoom.Text + "'and name<>'" + selectedroom + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("name already exists.", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                setupAddRoom();
            }
        }

        public void setupAddRoom()
        {
            con.Open();
            string addRoom = "Insert Into room_tbl(name,type,capacity)values('" + txtRoom.Text + "','" + cmbType.Text + "','"+txtCapacity.Text+"')";

            OdbcCommand cmdAddRoom = new OdbcCommand(addRoom, con);
            cmdAddRoom.ExecuteNonQuery();


            OdbcDataAdapter daget = new OdbcDataAdapter("Select*from room_tbl where name='" + txtRoom.Text + "'and type='room'", con);
            DataTable dtget = new DataTable();
            daget.Fill(dtget);

            if (dtget.Rows.Count > 0)
            {
                string addRoomtoallocatetbl = "Insert Into roomallocation_tbl(id,name,type,grade,section)values('" + dtget.Rows[0].ItemArray[0].ToString() + "','" + txtRoom.Text + "','" + cmbType.Text + "','','')";

                OdbcCommand cmdAddRoomtoallocatetbl = new OdbcCommand(addRoomtoallocatetbl, con);
                cmdAddRoomtoallocatetbl.ExecuteNonQuery();
            }
            con.Close();

            btnAdd.Enabled = false;
            setupview_room();
            MessageBox.Show("room successfully added", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        public void setupview_room()
        {
            con.Open();
            
            OdbcDataAdapter da = new OdbcDataAdapter("Select name as 'Room',type as 'Type',capacity as 'Capacity' from room_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            OdbcDataAdapter da0 = new OdbcDataAdapter("Select count(type) from room_tbl where type='Room'", con);
            DataTable dt0 = new DataTable();
            da0.Fill(dt0);

            OdbcDataAdapter da1 = new OdbcDataAdapter("Select count(type) from room_tbl where type='Computer Laboratory'", con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            con.Close();
            dvRoom = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvRoom;

                dgvSearch.Columns[0].Width = 240;
                dgvSearch.Columns[1].Width = 165;
                dgvSearch.Columns[2].Width = 70;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }

            lblResult.Text = "number of allocation: " + dgvSearch.Rows.Count.ToString();




            if (dt0.Rows.Count > 0 && dt1.Rows.Count > 0)
            {
                lblTyperes.Text = "Room: " + dt0.Rows[0].ItemArray[0].ToString() + "  Computer Laboratory: " + dt1.Rows[0].ItemArray[0].ToString();
            }
            else if (dt0.Rows.Count > 0 && dt1.Rows.Count < 0)
            {
                lblTyperes.Text = "Room: " + dt0.Rows[0].ItemArray[0].ToString() + "  Computer Laboratory: 0";
            }
            else if (dt0.Rows.Count < 0 && dt1.Rows.Count > 0)
            {
                lblTyperes.Text = "Room: 0" + "  Computer Laboratory: " + dt1.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                lblTyperes.Text = "Room: 0" + "  Computer Laboratory: 0";
            }
        }

        public void setupview_roomallocation()
        {
            con.Open();

            OdbcDataAdapter da = new OdbcDataAdapter("Select name as 'Room',grade as 'Grade',section as 'Section' from roomallocation_tbl where type='Room'", con);
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
                lblTyperes.Text="Occupied room: " + dtoccupied.Rows[0].ItemArray[0].ToString();
            }
            else if (dtfree.Rows.Count > 0 && dtoccupied.Rows.Count < 0)
            {
                lblResult.Text = "Free room: " + dtfree.Rows[0].ItemArray[0].ToString();
                lblTyperes.Text="Occupied room: 0";
            }
            else if (dtfree.Rows.Count < 0 && dtoccupied.Rows.Count > 0)
            {
                MessageBox.Show("");
                lblResult.Text = "Free room: 0";
                lblTyperes.Text="Occupied room: " + dtoccupied.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                lblResult.Text = "Free room: 0";
                lblTyperes.Text="Occupied room: 0";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            /*if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }*/


            if (cmbOperation.Text == "Add room")
            {
                dvRoom.RowFilter = string.Format("Room LIKE '%{0}%'", txtSearch.Text);
                dgvSearch.DataSource = dvRoom;

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
            if (cmbOperation.Text == "Assign room")
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
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Clear")
            {
                txtRoom.Clear();
                txtCapacity.Clear();
                cmbType.SelectedIndex = -1;
                txtRoom.Enabled = true;
                txtCapacity.Enabled = true;
                cmbType.Enabled = true;
                lblKey.Text = "";

                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnAdd.Enabled = true;
                btnUpdate.Text = "Update";
            }
            else
            {
                btnDelete.Enabled = true;
                btnClear.Text = "Clear";
                btnUpdate.Text = "Update";

                primarykey = lblKey.Text;
                setup_retrieve(primarykey);


                txtRoom.Enabled = false;
                cmbType.Enabled = false;
                txtCapacity.Enabled = false;
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
        }

        public void setup_retrieve(string thekey)
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from room_tbl where id='" + thekey + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                txtRoom.Text = dt.Rows[0].ItemArray[1].ToString();
                cmbType.Text = dt.Rows[0].ItemArray[2].ToString();
                txtCapacity.Text = dt.Rows[0].ItemArray[3].ToString();
                selectedroom = txtRoom.Text;
                selectedtype = cmbType.Text;
                selectedcapacity = txtCapacity.Text;
                btnAdd.Enabled = false;
            }
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            if (cmbOperation.Text == "Add room")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    return;
                }
                txtRoom.Enabled = false;
                cmbType.Enabled = false;
                txtCapacity.Enabled = false;

                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;

                string room = "";
                string type = "";
                string capa = "";
                if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
                {
                    room = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                }
                if (dgvSearch.SelectedRows[0].Cells[1].Value.ToString() != "")
                {
                    type = dgvSearch.SelectedRows[0].Cells[1].Value.ToString();
                }
                if (dgvSearch.SelectedRows[0].Cells[2].Value.ToString() != "")
                {
                    capa = dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
                }
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select id from room_tbl where name='" + room + "'and type='" + type + "'and capacity='" + capa + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    primarykey = dt.Rows[0].ItemArray[0].ToString();
                }
                setup_retrieve(primarykey);  
            }
            if (cmbOperation.Text == "Assign room")
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

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maintenance = new frmMaintenance();
            maintenance.adminlog = logger;
            maintenance.VISITED = VISITED;
            maintenance.Show();
            this.Hide();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection section = new frmSection();
            section.secwholog = logger;
            section.VISITED = VISITED;
            section.Show();
            this.Hide();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subjmaintenance = new frmSubject();
            subjmaintenance.wholog = logger;
            subjmaintenance.VISITED = VISITED;
            subjmaintenance.Show();
            this.Hide();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Update")
            {
                txtRoom.Enabled = true;
                cmbType.Enabled = true;
                txtCapacity.Enabled = true;

                btnUpdate.Text = "Save";
                btnDelete.Enabled = false;
                btnClear.Text = "Cancel";
            }
            else
            {
                if (txtRoom.Text == "" || cmbType.Text == ""|| txtCapacity.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from room_tbl where name='" + txtRoom.Text + "'and name<>'" + selectedroom + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("name already exists.", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    setup_save();
                    btnUpdate.Text = "Update";
                    btnClear.Text = "Clear";
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
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
            string updateroom = "Update room_tbl set name='" + txtRoom.Text + "',type='" + cmbType.Text + "',capacity='"+txtCapacity.Text+"'where id='" + lblKey.Text + "'";
            OdbcCommand cmdUpdateRoom = new OdbcCommand(updateroom, con);
            cmdUpdateRoom.ExecuteNonQuery();

            //string updateroomtoallocationtbl = "Update roomallocation_tbl set name='" + txtRoom.Text + "',type='" + cmbType.Text + "',grade='',section=''where id='" + lblKey.Text + "'";
            //OdbcCommand cmdUpdateRoomtoallocationtbl = new OdbcCommand(updateroomtoallocationtbl, con);
            //cmdUpdateRoomtoallocationtbl.ExecuteNonQuery();
            //con.Close();
            string updateroomtoallocationtbl = "Update roomallocation_tbl set name='" + txtRoom.Text + "',type='" + cmbType.Text + "'where id='" + lblKey.Text + "'";
            OdbcCommand cmdUpdateRoomtoallocationtbl = new OdbcCommand(updateroomtoallocationtbl, con);
            cmdUpdateRoomtoallocationtbl.ExecuteNonQuery();

            string update3 = "Update facultysched_tbl set room='" + txtRoom.Text + "'where room='" + selectedroom + "'";
            OdbcCommand cmdUpdate3 = new OdbcCommand(update3, con);
            cmdUpdate3.ExecuteNonQuery();

            string update4 = "Update schedule_tbl set room='" + txtRoom.Text + "'where room='" + selectedroom + "'";
            OdbcCommand cmdUpdate4 = new OdbcCommand(update4, con);
            cmdUpdate4.ExecuteNonQuery();

            con.Close();

            btnAdd.Enabled = false;
            setupview_room();
            btnClear.Text = "Clear";
            MessageBox.Show("room successfully updated", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete?", "Room maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtRoom.Text == "" || cmbType.Text == ""|| txtCapacity.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    setup_delete();
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;

                    txtRoom.Clear();
                    cmbType.SelectedIndex = -1;

                    txtRoom.Enabled = true;
                    cmbType.Enabled = true;
                    txtCapacity.Enabled = true;

                    btnAdd.Enabled = true;
                    if (dgvSearch.Rows.Count >= 1)
                    {
                        dgvSearch.Rows[0].Selected = true;
                    }
                }
            }
            else
            {
                return;
            }
        }

        public void setup_delete()
        {
            con.Open();
            string deleteRoom = "Delete from room_tbl where id='" + lblKey.Text + "'";
            OdbcCommand cmdDeleteRoom = new OdbcCommand(deleteRoom, con);
            cmdDeleteRoom.ExecuteNonQuery();

            string deleteRoomtoallocationtbl = "Delete from roomallocation_tbl where id='" + lblKey.Text + "'";
            OdbcCommand cmdDeleteRoomtoallocationtbl = new OdbcCommand(deleteRoomtoallocationtbl, con);
            cmdDeleteRoomtoallocationtbl.ExecuteNonQuery();
            con.Close();

            btnAdd.Enabled = false;
            lblKey.Text = "";
            setupview_room();
            MessageBox.Show("room successfully deleted", "Room maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
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
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from roomallocation_tbl where type='Room' and id='"+roomalcid+"'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0].ItemArray[4].ToString() == "")
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("Select*from roomallocation_tbl where section='"+cmbSection.Text+"' and grade='"+cmbLevel.Text+"'",con);
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
                            DialogResult res = MessageBox.Show("section already assigned,"+"\nwould you like to replace?" + "", "Room maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance am = new frmAboutMaintenance();
            am.amlog = logger;
            am.Show();
            this.Hide();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            frmFee feeform = new frmFee();
            this.Hide();
            feeform.feelog = logger;
            feeform.VISITED = VISITED;
            feeform.Show();
        }

        private void btnAud_Click(object sender, EventArgs e)
        {
            frmAudit auditform = new frmAudit();
            this.Hide();
            auditform.auditlogger = logger;
            auditform.Show();
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Hide();
            actform.actlog = logger;
            actform.Show();
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Hide();
            discform.disclog = logger;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqform = new frmRequirement();
            this.Hide();
            reqform.reqlog = logger;
            reqform.VISITED = VISITED;
            reqform.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched sf = new frmSched();
            this.Hide();
            sf.schedlog = logger;
            sf.VISITED = VISITED;
            sf.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Hide();
            buf.backlog = logger;
            buf.Show();
        }

        private void frmRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf= new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
           
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupSectionsPerLevel(cmbLevel.Text);
        }

        public void setupLevels()
        {
            cmbLevel.Items.Clear();
            cmbLevel.Items.Add("Kinder");
            cmbLevel.Items.Add("Grade 1");
            cmbLevel.Items.Add("Grade 2");
            cmbLevel.Items.Add("Grade 3");
            cmbLevel.Items.Add("Grade 4");
            cmbLevel.Items.Add("Grade 5");
            cmbLevel.Items.Add("Grade 6");
            cmbLevel.Items.Add("Grade 7");
            cmbLevel.Items.Add("Grade 8");
            cmbLevel.Items.Add("Grade 9");
            cmbLevel.Items.Add("Grade 10");
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

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = logger;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Hide();
            levmain.levlog = logger;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = logger;
            facmain.VISITED = VISITED;
            facmain.Show();
            this.Hide();
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = logger;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void pnlUser_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = logger;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = logger;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = logger;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void btnAsgClr_Click(object sender, EventArgs e)
        {
            setupLevelList();
            setupSectionsPerLevel(cmbLevel.Text);
        }

        private void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      

        
    }
}
