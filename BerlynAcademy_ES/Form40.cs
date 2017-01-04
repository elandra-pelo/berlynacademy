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
    public partial class frmASS : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public DataView dv,dvcom,dvcurr,dvnot,dvnotcom,dvnotcurr,dvnotroom;
        public frmSectioning secfrm;
        public string activeSY;
        public frmASS()
        {
            InitializeComponent();
        }

        private void frmASS_Load(object sender, EventArgs e)
        {
            setupview_active();
            setupview_notactive();
            GetActiveSchoolYear();
        }

        public void GetActiveSchoolYear()
        {
            con.Open();
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select syformat from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);
            con.Close();
            if (dtssy.Rows.Count > 0)
            {
                activeSY = dtssy.Rows[0].ItemArray[0].ToString();
            }
           
        }

        public void setupview_active()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("select section_tbl.level as 'Level',section_tbl.section as 'Section',roomallocation_tbl.name as 'Room' from section_tbl left join roomallocation_tbl on section_tbl.level=roomallocation_tbl.grade and section_tbl.section=roomallocation_tbl.section and section_tbl.level=roomallocation_tbl.grade where status='" + "active" + "' and isFull='no' ORDER BY Level ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
       
            DataTable dtcombi = new DataTable();
            DataTable dtcurrnum = new DataTable();

            if(dt.Rows.Count>0)
            {
               
                dtcombi.Columns.Add();
                dtcombi.Columns[0].ColumnName = "Capacity";
                dtcurrnum.Columns.Add();
                dtcurrnum.Columns[0].ColumnName = "Current no. of students";
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    con.Open();
                    OdbcDataAdapter da1 = new OdbcDataAdapter("select capacity as 'Capacity' from room_tbl where name='"+dt.Rows[x].ItemArray[2].ToString()+"'", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    con.Close();
                    if (dt1.Rows.Count > 0)
                    {
                        //dv.Table.rows
                        dtcombi.Rows.Add(dt1.Rows[0].ItemArray[0].ToString());
                       
                    }
                 
                    con.Open();
                    OdbcDataAdapter da2 = new OdbcDataAdapter("Select count(fname)from stud_tbl where section='" + dt.Rows[x].ItemArray[1].ToString() + "' and level='" + dt.Rows[x].ItemArray[0].ToString()+"'and status='Active'", con);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    con.Close();
                    if (dt2.Rows.Count > 0)
                    {
                       
                        //dv.Table.rows
                        dtcurrnum.Rows.Add(dt2.Rows[0].ItemArray[0].ToString());

                    }
                    
                }
               
                dv = new DataView(dt);
                dgvSearch.DataSource = dv;
                dvcom = new DataView(dtcombi);
                dgvAppend.DataSource = dvcom;
                dvcurr = new DataView(dtcurrnum);
                dgvCurr.DataSource = dvcurr;

                dgvSearch.Columns[0].Width = 170;
                dgvSearch.Columns[1].Width = 170;
                dgvSearch.Columns[2].Width = 170;
                dgvAppend.Columns[0].Width = 170;
                dgvCurr.Columns[0].Width = 247;

                bool istherenoactive=false;
                con.Open();
                OdbcDataAdapter dafind= new OdbcDataAdapter("select level from level_tbl", con);
                DataTable dtfind = new DataTable();
                dafind.Fill(dtfind);
                con.Close();
                if (dtfind.Rows.Count > 0)
                {
                   
                    string[] getlev = new string[dtfind.Rows.Count];
                    for (int x = 0; x < dtfind.Rows.Count; x++)
                    {
                        con.Open();
                        OdbcDataAdapter daa = new OdbcDataAdapter("select level from section_tbl where status='" + "active" + "'and level='"+dtfind.Rows[x].ItemArray[0].ToString()+"'", con);
                        DataTable dtt = new DataTable();
                        daa.Fill(dtt);
                        con.Close();
                        if (dtt.Rows.Count <= 0)
                        {
                            istherenoactive = true;
                            lblWarning.Text = lblWarning.Text + " " + dtfind.Rows[x].ItemArray[0].ToString();
                        }
                    }

                    if (istherenoactive == true)
                    {
                        lblWarning.Visible = true;
                        lblw1.Visible = true;
                    }
                }
            }
        }

        public void setupview_notactive()
        {
           int seccapacity=0;
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("select level as 'Level',section as 'Section' from section_tbl where status='" + "inactive" + "' ORDER BY Level ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvnot = new DataView(dt);
            DataTable dtntcombi = new DataTable();
            DataTable dtntcurrnum = new DataTable();
            DataTable dtntroom = new DataTable();
            dgvnot1.DataSource = dvnot;
            dgvnot1.Columns[0].Width = 170;
            dgvnot1.Columns[1].Width = 170;
            dtntcurrnum.Columns.Add();
            dtntcurrnum.Columns[0].ColumnName = "Status";
            dtntroom.Columns.Add();
            dtntroom.Columns[0].ColumnName = "Room";
            dtntcombi.Columns.Add();
            dtntcombi.Columns[0].ColumnName = "Capacity";

            if (dt.Rows.Count > 0)
            {
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    con.Open();
                    OdbcDataAdapter da22 = new OdbcDataAdapter("select name from roomallocation_tbl where grade='" + dt.Rows[x].ItemArray[0].ToString() + "'and section='" + dt.Rows[x].ItemArray[1].ToString() + "'", con);
                    DataTable dt22 = new DataTable();
                    da22.Fill(dt22);
                    con.Close();
                    if (dt22.Rows.Count > 0)
                    {
                        dtntroom.Rows.Add(dt22.Rows[0].ItemArray[0].ToString());

                        con.Open();
                        OdbcDataAdapter da1 = new OdbcDataAdapter("select capacity as 'Capacity' from room_tbl where name='" + dt22.Rows[0].ItemArray[0].ToString() + "'", con);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        con.Close();
                        if (dt1.Rows.Count > 0)
                        {
                            dtntcombi.Rows.Add(dt1.Rows[0].ItemArray[0].ToString());
                            seccapacity = Convert.ToInt32(dt1.Rows[0].ItemArray[0].ToString());
                        }
                    }

                    con.Open();
                    OdbcDataAdapter da2 = new OdbcDataAdapter("Select count(fname)from stud_tbl where section='" + dt.Rows[x].ItemArray[1].ToString() + "' and level='" + dt.Rows[x].ItemArray[0].ToString() + "'and status='Active'", con);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    con.Close();
                    if (dt2.Rows.Count > 0)
                    {
                        int rowcnt = Convert.ToInt32(dt2.Rows[0].ItemArray[0].ToString());
                      
                        if (rowcnt==seccapacity)
                        {
                            dtntcurrnum.Rows.Add("Full");
                        }
                        else
                        {
                            dtntcurrnum.Rows.Add("Waiting");
                        }
                    }
                }

                dvnotcom = new DataView(dtntcombi);
                dgvNotAppend.DataSource = dvnotcom;
                dvnotcurr = new DataView(dtntcurrnum);
                dgvNotCurr.DataSource = dvnotcurr;
                dvnotroom = new DataView(dtntroom);
                dgvNotRoom.DataSource = dvnotroom;
                dgvNotAppend.Columns[0].Width = 170;
                dgvNotRoom.Columns[0].Width = 170;
                dgvNotCurr.Columns[0].Width = 247;
            }
               
        }

        private void frmASS_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

        private void dgvnot1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblWarning_Click(object sender, EventArgs e)
        {

        }
    }
}
