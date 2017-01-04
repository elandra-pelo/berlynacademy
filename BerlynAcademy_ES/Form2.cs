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
    public partial class frmEmpLogin : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=sa;DB=berlyn");
        public frmMaintenance main;
        public frmHomeMaintenance hm;
       
        public char pchar;
        public bool isclickback,waslog;
        public static int trycount = 0;
        public int tick;

        public frmEmpLogin()
        {
            InitializeComponent();
        }

        private void frmEmpLogin_Load(object sender, EventArgs e)
        {
           
            this.BackColor = Color.FromArgb(25, 25, 25);
            //pnlpassview.BackColor = Color.FromArgb(39, 69, 132);
            txtUser.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            trycount = 0;

            if (MessageBox.Show("Do you really want to close?", "close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            pnlpassview.Visible = false;
            LOGIN();
        }

     
        public void LOGIN()
        {
            
            pnlpassview.Visible = false;
            if (txtUser.Text != "" && txtPass.Text != "")
            {
                waslog = true;
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where username='" + txtUser.Text + "' AND password='" + txtPass.Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from admin_tbl where username='" + txtUser.Text + "'and password='" + txtPass.Text + "'", con);
                DataTable dt0 = new DataTable();
                da0.Fill(dt0);

                con.Close();

                if (trycount < 4)
                {
                    if (dt.Rows.Count > 0)
                    {
                        string name = dt.Rows[0].ItemArray[1].ToString() + " " + dt.Rows[0].ItemArray[2].ToString() + " " + dt.Rows[0].ItemArray[3].ToString();
                        string posi = dt.Rows[0].ItemArray[12].ToString();
                        string gen = dt.Rows[0].ItemArray[7].ToString();
                        string prefix = "";
                       
                        string loggerAccessCode = "";
                        string date = DateTime.Now.ToLongDateString();
                        string logintime = DateTime.Now.ToString("hh:mm tt");

                        con.Open();
                        OdbcDataAdapter daAccessCode = new OdbcDataAdapter("Select accesscode from useraccesslevel_tbl where position LIKE'" + posi + "'", con);
                        DataTable dtAccessCode = new DataTable();
                        daAccessCode.Fill(dtAccessCode);
                        con.Close();

                        if (dtAccessCode.Rows.Count > 0)
                        {
                            loggerAccessCode = dtAccessCode.Rows[0].ItemArray[0].ToString();
                        }

                        if (gen == "Male")
                        {
                            prefix = "Mr. ";
                        }
                        else {
                            prefix = "Ms. ";
                        }

                        con.Open();
                        string setLog = "Insert Into audittrail_tbl(name,position,date,login)values('" + name + "','"+posi+"','" + date + "','" + logintime + "')";
                        OdbcCommand cmd = new OdbcCommand(setLog, con);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        if (posi == "faculty")
                        {
                           
                            frmEmpMain empmain = new frmEmpMain();
                            this.Hide();
                            empmain.faclog = dt.Rows[0].ItemArray[1].ToString();
                            empmain.TheFacultyName = name;
                            empmain.CO = prefix+name;
                            empmain.accesscode = loggerAccessCode;
                            empmain.emptype = "Faculty";
                            empmain.Show();
                        }
                        if (posi == "principal")
                        {
                            frmPrincipalMain prinmain = new frmPrincipalMain();
                            this.Hide();
                            prinmain.prinlog = dt.Rows[0].ItemArray[1].ToString();
                            prinmain.accesscode = loggerAccessCode;
                            prinmain.co = prefix + name;
                            prinmain.emptype = "Principal";
                            prinmain.Show();
                        }
                        if (posi == "cashier")
                        {
                            frmCashierMain cashmain = new frmCashierMain();
                            this.Hide();
                            cashmain.cashlog = dt.Rows[0].ItemArray[1].ToString();
                            cashmain.CO = prefix+name;
                            cashmain.accesscode = loggerAccessCode;
                            cashmain.emptype = "Cashier";
                            cashmain.Show();
                        }
                        if (posi == "registrar")
                        {
                            frmRegistrarMain regmain = new frmRegistrarMain();
                            this.Hide();
                            regmain.reglog = dt.Rows[0].ItemArray[1].ToString();
                            regmain.co = prefix + name;
                            regmain.accesscode = loggerAccessCode;
                            regmain.emptype = "Registrar";
                            regmain.Show();
                        }
                    }
                    else if (dt0.Rows.Count > 0)
                    {
                        string name = dt0.Rows[0].ItemArray[1].ToString() + " " + dt0.Rows[0].ItemArray[2].ToString() + " " + dt0.Rows[0].ItemArray[3].ToString();
                        string posi = "Administrator";
                        string date = DateTime.Today.ToLongDateString();
                        string logintime = DateTime.Now.ToString("hh:mm tt");

                        con.Open();
                        string setLog = "Insert Into audittrail_tbl(name,position,date,login)values('" + name + "','" + posi + "','" + date + "','" + logintime + "')";
                        OdbcCommand cmd = new OdbcCommand(setLog, con);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        //main = new frmMaintenance();
                        hm = new frmHomeMaintenance();
                       
                        this.Hide();
                  
                        hm.adminlog = dt0.Rows[0].ItemArray[1].ToString();
                        hm.Show();
                    }
                    else
                    {
                       
                        MessageBox.Show("Invalid username or password.", "Employee login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUser.Clear(); txtPass.Clear();
                        txtUser.Focus();
                        waslog = false;
                        trycount++;
                        return;
                    }
                }
                else
                {
                   
                    waslog = false;
                    if (MessageBox.Show("You entered invalid username and password" + "\n 5 times, would you like to continue?", "Employee login",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        
                        trycount = 0;
                    }
                    else
                    {

                    }

                   
                    txtUser.Clear(); txtPass.Clear();
                    txtUser.Focus();

                }
            }
            else
            {
                return;
            }
           
           
        }

        private void lnkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to close?", "close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                return;
            }
        }

       

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
           if (txtPass.Text=="")
            {
                pnlpassview.Visible = false;
           }
        }

        private void tmrv_Tick(object sender, EventArgs e)
        {
         
            if (tick++ >= 6)
            {
                pnlpassview.Visible = false;
                tick = 0;
                tmrv.Enabled = false;
            }
            
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //pnlpassview.Visible = false;
                LOGIN();
               
            }
            /*if (e.KeyCode == Keys.Back)
            {
                pnlpassview.Visible = false;

            }
            else
            {
                if (txtPass.TextLength != 15 && waslog==false && e.KeyCode!=Keys.Enter)
                {
                    pnlpassview.Visible = true;
                }
            }*/

        }

        /*private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            pchar = e.KeyChar;

            LBLI.Text = pchar.ToString();
            tmrv.Enabled = true;

            if (txtPass.TextLength == 15)
            {
                pnlpassview.Visible = false;
            }
        }*/

        
        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (txtUser.Text != "")
            {
                lbltit1.Visible = false;
            }
            else
            {
                lbltit1.Visible = true;
            }
        }

        private void txtPass_TextChanged_1(object sender, EventArgs e)
        {
            if (txtPass.Text != "")
            {
                lbltit2.Visible = false;
            }
            else
            {
                lbltit2.Visible = true;
            }
        }

        private void lbltit1_Click(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        private void lbltit2_Click(object sender, EventArgs e)
        {
            txtPass.Focus();
        }

      
    }
}
