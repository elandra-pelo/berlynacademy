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
    public partial class frmMaintenance : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");

        public DataView dvStudent;
        public DataView dvAdmin, dvFaculty, dvCashier, dvRegistrar, dvPrincipal;
        public string adminlog, primarykey,newStudentNumber,firstdigit,seconddigit,thirddigit,fourthdigit,VISITED;
        public bool isinserted,ispassones,ispasstenths,ispasshunths,ispassmax,isnospace;
        public int lstEnye = 1, fnmEnye = 1, mnmEnye = 1;
       
        public frmMaintenance()
        {
            InitializeComponent();
        }

        private void frmMaintenance_Load(object sender, EventArgs e)
        {
            
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);
            btnUser.BackColor = Color.LightGreen;
            lblLogger.Text = adminlog;
            lblLoggerPosition.Text = "Admin";
            cmbUserType.Text = "Administrator";
            setupdays_forAdmin();
            setupyears_forAdmin();

            //this.BackColor = Color.FromArgb(49, 79, 142);
          
            //btnHome.Text = "          "+adminlog;

            if (VISITED.Contains("User") == false)
            {
                VISITED += "   User";
            }
            //setupview_forAdmin();
        }

        public void setupStudnum()
        {
            string current = "";
            string yr = "";

            yr = DateTime.Now.Year.ToString();


            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select*from studno_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0].ItemArray[1].ToString() == "9998")
                {
                    lblnotemax.Visible = true;
                }
                else
                {
                    lblnotemax.Visible = false;
                }

                firstdigit = dt.Rows[0].ItemArray[1].ToString().Substring(0, 1);
                seconddigit = dt.Rows[0].ItemArray[1].ToString().Substring(1, 1);
                thirddigit = dt.Rows[0].ItemArray[1].ToString().Substring(2, 1);
                fourthdigit = dt.Rows[0].ItemArray[1].ToString().Substring(3, 1);

                //INTEGER VALUE
                int firstval = Convert.ToInt32(firstdigit);
                int secval = Convert.ToInt32(seconddigit);
                int thirdval = Convert.ToInt32(thirddigit);
                int fourthval = Convert.ToInt32(fourthdigit);

                int convertfourth = Convert.ToInt32(fourthdigit);
              
                int combione = Convert.ToInt32(thirddigit + fourthdigit);
                int combitwo = Convert.ToInt32(seconddigit + thirddigit + fourthdigit);
                int combithree = Convert.ToInt32(firstdigit + seconddigit + thirddigit + fourthdigit);

                //FOR STUDENT NUMBER 00001 UP TO 00009
                if ((firstdigit == "0" && seconddigit == "0" && thirddigit == "0") && (fourthval >= 0 && fourthval <= 9))
                {
                  
                    if (convertfourth <=11)
                    {
                        if (convertfourth == 0)
                        {
                            convertfourth = 1;
                        }
                        newStudentNumber = yr + "-" + firstdigit + seconddigit + thirddigit + convertfourth.ToString();
                        convertfourth++;
                        current = firstdigit + seconddigit + thirddigit + convertfourth.ToString();

                        if (convertfourth == 10)
                        {
                            ispassones = true;
                        }
                    }
                    if (convertfourth ==2 && ispassones==true)
                    {
                        thirddigit = "1";
                        fourthdigit = "0";
                        combione = Convert.ToInt32(thirddigit+fourthdigit);
                    }
                }

                //FOR STUDENT NUMBER 00010 UP TO 00099
                if ((firstdigit == "0" && seconddigit == "0") && (combione >= 10 && combione <=99))
                {
                    
                    if (combione <=99)
                    {
                       
                        newStudentNumber = yr + "-" + firstdigit + seconddigit + combione.ToString();
                        combione++;
                        current = firstdigit + seconddigit + combione.ToString();
                      

                        if (combione == 100)
                        {
                            ispasstenths = true;
                        }
                    }
                  
                    if (combione==11 && ispasstenths==true)
                    {
                        firstdigit = "0";
                        //seconddigit = "1";
                        //thirddigit = "0";
                        //fourthdigit = "0";
                        combione = 0;
                        combitwo = 100;
                        //combitwo = Convert.ToInt32(seconddigit + thirddigit + fourthdigit);
                        newStudentNumber = yr + "-"+firstdigit+combitwo.ToString();
                    }
                   
                }

                //FOR STUDENT NUMBER 00100 UP TO 00999
                if ((firstdigit == "0") && (combitwo >= 100 && combitwo <= 999))
                {
                    
                    if (combitwo <=999)
                    {
                       
                            newStudentNumber = yr + "-" + firstdigit + combitwo.ToString();
                            combitwo++;
                            current = firstdigit + combitwo.ToString();
                            
                            if (combitwo == 1000)
                            {
                                ispasshunths = true;
                            }
                
                    }
                    if (combitwo ==101 && ispasshunths==true)
                    {
                        firstdigit = "1";
                        seconddigit = "0";
                        thirddigit = "0";
                        fourthdigit = "0";
                       
                        combithree = Convert.ToInt32(firstdigit+seconddigit + thirddigit + fourthdigit);
                    }

                }

                //FOR STUDENT NUMBER 01000 UP TO 09999
                if ((firstdigit != "0") && (combithree >= 1000 && combithree <=9999))
                {
                        if (combithree <=9998)
                        {
                            newStudentNumber = yr + "-" + combithree.ToString();
                            combithree++;
                            current = combithree.ToString();
                           
                            if (combithree == 9999)
                            {
                                isnospace = true;
                                ispassmax = true;
                               
                            }
                        }
                        if (combithree ==9999 && isinserted==true)
                        {
                            MessageBox.Show("add operation could not perform!"+"\nmaximum student number was reached.","User maintenance",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;   
                        }
                }
               

                

                if (isinserted == true)
                {
                    con.Open();
                    string updatecurrent = "Update studno_tbl set current='" + current + "'";
                    OdbcCommand cmd = new OdbcCommand(updatecurrent, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    isinserted = false;
                }
            }
        }
        
        private void cmbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUserType.Text == "Administrator")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    pnlnotify.Visible = true;
                }
                btnN1.Enabled = true; btnN2.Enabled = true; btnN3.Enabled = true;
                pnlUserAdmin.Location = new Point(6, 58);
                setupview_forAdmin();
                lblStdFilter.Visible = false;
                cmbStudFilter.Visible = false;
                cmbStdFilterChoice.Visible = false;
                pnlUserAdmin.Visible = true;
                pnlUserFaculty.Visible = false;
                pnlUserEmployee.Visible = false;
                pnlUserStud.Visible = false;
                btnAdmUpdate.Enabled = false;
                btnAdmDelete.Enabled = false;
                btnFacAdd.Enabled = true;
                lblUserNo.Text = "Admin no:";
                lblKey.Text = "";
                toolTip1.SetToolTip(txtSearch, "search admin lastname");
                setupenableinput_forAdmin();
                setupclear_forAdmin();
                //setupdays_forAdmin();
                //setupyears_forAdmin();
               
            }
            if (cmbUserType.Text == "Faculty")
            {

                if (dgvSearch.Rows.Count <= 0)
                {
                    pnlnotify.Visible = true;
                }
                btnFFirst.Enabled = true; btnFMid.Enabled = true; btnFLast.Enabled = true;
                pnlUserAdmin.Visible = false;
                pnlUserFaculty.Visible = true;
                pnlUserEmployee.Visible = false;
                pnlUserStud.Visible = false;
                pnlUserFaculty.Location = new Point(6, 58);
                btnFacUpdate.Enabled = false;
                btnFacDelete.Enabled = false;
                btnFacAdd.Enabled = true;
                lblStdFilter.Visible = false;
                cmbStudFilter.Visible = false;
                cmbStdFilterChoice.Visible = false;
                setupview_forFaculty();
                setupenableinput_forFaculty();
                setupclear_forFaculty();
                setupdays_forFaculty();
                setupyears_forFaculty();

                lblUserNo.Text = "Faculty no:";
                lblKey.Text = "";
                toolTip1.SetToolTip(txtSearch, "search faculty lastname");
                //setupview_forFacultyAcct();
            }
            if (cmbUserType.Text == "Cashier" || cmbUserType.Text == "Registrar" || cmbUserType.Text == "Principal")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    pnlnotify.Visible = true;
                }
                btnEFirst.Enabled = true; btnEMidl.Enabled = true; btnELast.Enabled = true;
                lblStdFilter.Visible = false;
                cmbStudFilter.Visible = false;
                cmbStdFilterChoice.Visible = false;
                pnlUserEmployee.Visible = true;
                pnlUserAdmin.Visible = false;
                pnlUserFaculty.Visible = false;
                pnlUserStud.Visible = false;
                pnlUserEmployee.Location = new Point(6, 58);
                btnEmpUpdate.Enabled = false;
                btnEmpDelete.Enabled = false;
                btnEmpAdd.Enabled = true;
                setupenableinput_forEmployee();
                setupclear_forEmployee();

                lblKey.Text = "";
              
                if (cmbUserType.Text == "Cashier")
                {
                    lblUserNo.Text ="Cashier no:";
                    setupview_forCashier();
                    toolTip1.SetToolTip(txtSearch, "search cashier lastname");
                }
                if (cmbUserType.Text == "Registrar")
                {
                    lblUserNo.Text = "Registrar no:";
                    setupview_forRegistrar();
                    toolTip1.SetToolTip(txtSearch, "search registrar lastname");
                }
                if (cmbUserType.Text == "Principal")
                {
                    lblUserNo.Text = "Principal no:";
                    setupview_forPrincipal();
                    toolTip1.SetToolTip(txtSearch, "search principal lastname");
                }
            }
            if (cmbUserType.Text == "Student")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    pnlnotify.Visible = true;
                }

                btnSFirst.Enabled = true; btnSMid.Enabled = true; btnSLast.Enabled = true;

                lblStdFilter.Visible = true;
                cmbStudFilter.Visible = true;
                cmbStdFilterChoice.Visible = true;
                pnlUserStud.Visible = true;
                pnlUserStud.Location = new Point(6, 58);
                pnlUserAdmin.Visible = false;
                pnlUserEmployee.Visible = false;
                pnlUserFaculty.Visible = false;
    
                btnStdUpdate.Enabled = false;
                btnStdDelete.Enabled = false;
                btnStdAdd.Enabled = true;

                setupenableinput_forStudent();
                setupclear_forStudent();

                lblUserNo.Text = "Student no:";
                lblKey.Text = "";
                cmbStudFilter.Text = "All students";
                toolTip1.SetToolTip(txtSearch, "search student lastname");
             
                setupview_forStudent();
                setupStudnum();
               
            }
        }

        private void lnkUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pnlUser.Visible = true;
        }

        private void btnAdmClear_Click(object sender, EventArgs e)
        {
            if (btnAdmClear.Text == "Clear")
            {
                setupclear_forAdmin();
                btnAdmUpdate.Enabled = false;
                btnAdmDelete.Enabled = false;
                btnAdmAdd.Enabled = true;
                btnAdmUpdate.Text = "Update";
            }
            else
            {
                btnAdmDelete.Enabled = true;
                btnAdmClear.Text = "Clear";
                btnAdmUpdate.Text = "Update";

                primarykey = lblKey.Text;
                setupretrieveddata_forAdmin(primarykey);
                setupdisableinput_forAdmin();
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
        }

         private void dgvSearch_Click(object sender, EventArgs e)
        {
            primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            if (cmbUserType.Text == "Administrator")
            {
                if (dgvSearch.Rows.Count <= 0)
                {
                    return;
                }
                btnN1.Enabled = false; btnN2.Enabled = false; btnN3.Enabled = false;
                string fn = "";
                string ln = "";
                string mn = "";
             
                if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
                {
                    ln = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                }
                if (dgvSearch.SelectedRows[0].Cells[1].Value.ToString() != "")
                {
                    fn = dgvSearch.SelectedRows[0].Cells[1].Value.ToString();
                }
                if (dgvSearch.SelectedRows[0].Cells[2].Value.ToString() != "")
                {
                    mn = dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
                }

               
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select adminnum from admin_tbl where firstname='" + fn + "'and middlename='" + mn + "'and lastname='" + ln + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    primarykey = dt.Rows[0].ItemArray[0].ToString();
                }

                setupdisableinput_forAdmin();
                btnAdmUpdate.Enabled = true;
                btnAdmDelete.Enabled = true;

                setupretrieveddata_forAdmin(primarykey);
            }
            if (cmbUserType.Text == "Faculty")
            {
                btnFFirst.Enabled = false; btnFMid.Enabled = false; btnFLast.Enabled = false;
                if (dgvSearch.Rows.Count <= 0)
                {
                    return;
                }
                setupdisableinput_forFaculty();
                btnFacUpdate.Enabled = true;
                btnFacDelete.Enabled = true;

                if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
                {
                    primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                }
                 
                setupretrieveddata_forFaculty(primarykey);
               
            }
            if (cmbUserType.Text == "Cashier" || cmbUserType.Text == "Registrar" || cmbUserType.Text == "Principal")
            {
                btnEFirst.Enabled = false; btnEMidl.Enabled = false; btnELast.Enabled = false;
                if (dgvSearch.Rows.Count <=0)
                {
                    return;
                }
                setupdisableinput_forEmployee();
                btnEmpUpdate.Enabled = true;
                btnEmpDelete.Enabled = true;

                if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
                {
                     primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                }
               
                setupretrieveddata_forEmployee(primarykey);
               
            }
            if (cmbUserType.Text == "Student")
            {

                btnSFirst.Enabled = false; btnSMid.Enabled = false; btnSLast.Enabled = false;
                if (dgvSearch.Rows.Count <= 0)
                {
                    return;
                }

                if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
                {
                    primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
                }
               
                setupdisableinput_forStudent();
                btnStdUpdate.Enabled = true;
                btnStdDelete.Enabled = true;
              
                setupretrieveddata_forStudent(primarykey);
            }
         }


         public void inputcheck_forMaintenance(string result,TextBox txt)
         {
             if (result=="valid")
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

         private void btnAdmAdd_Click(object sender, EventArgs e)
         {
             if (txtAdmFname.Text == "" || txtAdmLast.Text == "" || cmbAdmGen.Text == "" || txtAdmUser.Text == "" || txtAdmPass.Text == "")
             {
                 MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK , MessageBoxIcon.Warning);
                 return;
             }
             if (cmbAdmDay.Text != "")
             {
                 if (cmbAdmMonth.Text == "Feb" && Convert.ToInt32(cmbAdmDay.Text) > 28)
                 {
                     MessageBox.Show("day of birth is out of range.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
             }

             int current = Convert.ToInt32(DateTime.Now.Year);
             int birthyear = Convert.ToInt32(cmbAdmYears.Text);
             int age = current - birthyear;

             if (age < 18)
             {
                 MessageBox.Show("admin too young.", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;
             }
             if (txtAdmMidl.Text != "")
             {
                 if (txtAdmMidl.TextLength < 2)
                 {
                     inputcheck_forMaintenance("invalid", txtAdmMidl);
                 }
                 else
                 {
                     inputcheck_forMaintenance("valid", txtAdmMidl);
                 }
             }
             if (txtAdmCon.Text != "")
             {
                 if ((txtAdmCon.TextLength != 11)&&(txtAdmCon.TextLength !=7))
                 {
                    
                     inputcheck_forMaintenance("invalid", txtAdmCon);
                 }
                 else 
                 {
                     inputcheck_forMaintenance("valid", txtAdmCon);
                 }
             }
             if (txtAdmUser.Text != "")
             {
                 if (txtAdmUser.TextLength < 6)
                 {
                    
                     inputcheck_forMaintenance("invalid", txtAdmUser);
                 }
                 else
                 {
                     inputcheck_forMaintenance("valid", txtAdmUser);
                 }
             }
             if (txtAdmPass.Text != "")
             {
                 if (txtAdmPass.TextLength < 6)
                 {
                   
                     inputcheck_forMaintenance("invalid", txtAdmPass);
                 }
                 else
                 { 
                     inputcheck_forMaintenance("valid", txtAdmPass);
                 }
             }
             if (((txtAdmUser.TextLength > 5) && (txtAdmPass.TextLength > 5)) && (((txtAdmCon.Text != "") && ((txtAdmCon.TextLength == 11) || (txtAdmCon.TextLength == 7))) || (txtAdmCon.Text == "")))
             {
                 con.Open();
                 OdbcDataAdapter da = new OdbcDataAdapter("Select*from admin_tbl where username='" + txtAdmUser.Text + "'or password='" + txtAdmPass.Text + "'", con);
                 DataTable dt = new DataTable();
                 da.Fill(dt);
                 con.Close();
                 if (dt.Rows.Count > 0)
                 {
                     MessageBox.Show("username or password not unique.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 else
                 {
                     if (txtAdmMidl.Text != "")
                     {
                         if (txtAdmMidl.TextLength > 1)
                         {
                             setupaddoperation_forAdmin();
                         }
                     }
                     else
                     {
                         setupaddoperation_forAdmin();
                     }   
                 }
             }
         }

         private void btnAdmUpdate_Click(object sender, EventArgs e)
         {
            
             if (btnAdmUpdate.Text == "Update")
             {
                 btnN1.Enabled = true; btnN2.Enabled = true; btnN3.Enabled = true;
                 setupenableinput_forAdmin();
                 btnAdmUpdate.Text = "Save";
                 btnAdmDelete.Enabled = false;
                 btnAdmClear.Text = "Cancel";
             }
             else
             {
                 if (txtAdmFname.Text == "" || txtAdmLast.Text == "" || cmbAdmGen.Text == "" || cmbAdmDay.Text=="" || cmbAdmMonth.Text=="" || cmbAdmYears.Text=="" || txtAdmUser.Text == "" || txtAdmPass.Text == "")
                 {
                     MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 if (cmbAdmDay.Text != "")
                 {
                     if (cmbAdmMonth.Text == "Feb" && Convert.ToInt32(cmbAdmDay.Text) > 28)
                     {
                         MessageBox.Show("day of birth is out of range.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         return;
                     }
                 }
                 int current = Convert.ToInt32(DateTime.Now.Year);
                 int birthyear = Convert.ToInt32(cmbAdmYears.Text);
                 int age = current - birthyear;

                 if (age < 18)
                 {
                     MessageBox.Show("admin too young.", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 if (txtAdmMidl.Text != "")
                 {
                     if (txtAdmMidl.TextLength < 2)
                     {
                         inputcheck_forMaintenance("invalid", txtAdmMidl);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtAdmMidl);
                     }
                 }
                 if (txtAdmCon.Text != "")
                 {
                     if ((txtAdmCon.TextLength != 11) && (txtAdmCon.TextLength != 7))
                     {
                         
                         inputcheck_forMaintenance("invalid", txtAdmCon);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtAdmCon);
                     }
                 }
                 if (txtAdmUser.Text != "")
                 {
                     if (txtAdmUser.TextLength < 6)
                     {
                        
                         inputcheck_forMaintenance("invalid", txtAdmUser);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtAdmUser);
                     }
                 }
                 if (txtAdmPass.Text != "")
                 {
                     if (txtAdmPass.TextLength < 6)
                     {
                         
                         inputcheck_forMaintenance("invalid", txtAdmPass);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtAdmPass);
                     }
                 }
                 if (((txtAdmUser.TextLength > 5) && (txtAdmPass.TextLength > 5)) && (((txtAdmCon.Text != "") && ((txtAdmCon.TextLength == 11) || (txtAdmCon.TextLength == 7))) || (txtAdmCon.Text == "")))
                 {
                     con.Open();
                     OdbcDataAdapter da = new OdbcDataAdapter("Select*from admin_tbl where adminnum<>'" + primarykey + "'and username='" + txtAdmUser.Text + "'and password='" + txtAdmPass.Text + "'", con);
                     DataTable dt = new DataTable();
                     da.Fill(dt);
                     con.Close();
                     if (dt.Rows.Count > 0)
                     {
                         MessageBox.Show("username or password not unique.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         return;
                     }
                     else
                     {
                         if (txtAdmMidl.Text != "")
                         {
                             if (txtAdmMidl.TextLength > 1)
                             {
                                 setupsaveoperation_forAdmin();
                                 btnAdmUpdate.Text = "Update";
                                 btnAdmClear.Text = "Clear";
                                 btnAdmUpdate.Enabled = false;
                                 btnAdmDelete.Enabled = false;
                                 if (dgvSearch.Rows.Count >= 1)
                                 {
                                     dgvSearch.Rows[0].Selected = true;
                                 }
                             }
                         }
                         else
                         {
                             setupsaveoperation_forAdmin();
                             btnAdmUpdate.Text = "Update";
                             btnAdmClear.Text = "Clear";
                             btnAdmUpdate.Enabled = false;
                             btnAdmDelete.Enabled = false;
                             if (dgvSearch.Rows.Count >= 1)
                             {
                                 dgvSearch.Rows[0].Selected = true;
                             }
                         }  
                     }
                 }
             }
         }

         private void btnAdmDelete_Click(object sender, EventArgs e)
         {
           
             if (MessageBox.Show("Do you really want to delete?", "User maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
             {
                 if (txtAdmFname.Text == "" || txtAdmLast.Text == ""|| cmbAdmMonth.Text == "" || cmbAdmDay.Text == "" ||
                 cmbAdmYears.Text == "" || cmbAdmGen.Text == "" || txtAdmUser.Text == "" || txtAdmPass.Text == "")
                 {
                     MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 if (cmbAdmDay.Text != "")
                 {
                     if (cmbAdmMonth.Text == "Feb" && Convert.ToInt32(cmbAdmDay.Text) > 28)
                     {
                         MessageBox.Show("day of birth is out of range.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         return;
                     }
                 }
                 if (txtAdmMidl.Text != "")
                 {
                     if (txtAdmMidl.TextLength <2)
                     {

                         inputcheck_forMaintenance("invalid", txtAdmMidl);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtAdmMidl);
                     }
                 }
                 if (txtAdmCon.Text != "")
                 {
                     if ((txtAdmCon.TextLength != 11) && (txtAdmCon.TextLength != 7))
                     {
                        
                         inputcheck_forMaintenance("invalid", txtAdmCon);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtAdmCon);
                     }
                 }
                 if (txtAdmUser.Text != "")
                 {
                     if (txtAdmUser.TextLength < 6)
                     {
                         
                         inputcheck_forMaintenance("invalid", txtAdmUser);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtAdmUser);
                     }
                 }
                 if (txtAdmPass.Text != "")
                 {
                     if (txtAdmPass.TextLength < 6)
                     {
                        
                         inputcheck_forMaintenance("invalid", txtAdmPass);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtAdmPass);
                     }
                 }
                 if (((txtAdmUser.TextLength > 5) && (txtAdmPass.TextLength > 5))&& (((txtAdmCon.Text != "") && ((txtAdmCon.TextLength == 11) || (txtAdmCon.TextLength == 7))) || (txtAdmCon.Text == "")))
                 {
                     if (txtAdmMidl.Text != "")
                     {
                         if (txtAdmMidl.TextLength > 1)
                         {
                             setupdeleteoperation_forAdmin();
                             btnAdmUpdate.Enabled = false;
                             btnAdmDelete.Enabled = false;
                             setupclear_forAdmin();
                             setupenableinput_forAdmin();
                             btnAdmAdd.Enabled = true;
                             if (dgvSearch.Rows.Count >= 1)
                             {
                                 dgvSearch.Rows[0].Selected = true;
                             }
                         }
                     }
                     else
                     {
                         setupdeleteoperation_forAdmin();
                         btnAdmUpdate.Enabled = false;
                         btnAdmDelete.Enabled = false;
                         setupclear_forAdmin();
                         setupenableinput_forAdmin();
                         btnAdmAdd.Enabled = true;
                         if (dgvSearch.Rows.Count >= 1)
                         {
                             dgvSearch.Rows[0].Selected = true;
                         }
                     } 
                 }
             }
             else
             {
                 return;
             }
         }

         private void txtSearch_TextChanged(object sender, EventArgs e)
         {
             if (dgvSearch.Rows.Count>=1)
             {
                 dgvSearch.Rows[0].Selected = true;
             }
             if (cmbUserType.Text == "Administrator")
             {
                 dvAdmin.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
                 dgvSearch.DataSource = dvAdmin;      
                 toolTip1.SetToolTip(txtSearch, "search admin lastname");

                 if (dgvSearch.Rows.Count > 0 && txtSearch.Text != "")
                 {
                     pnlnotify.Visible = false;
                 }
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
             if (cmbUserType.Text == "Faculty")
             {
                 dvFaculty.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
                 dgvSearch.DataSource = dvFaculty;
                 toolTip1.SetToolTip(txtSearch, "search faculty lastname");

                 if (dgvSearch.Rows.Count > 0 && txtSearch.Text != "")
                 {
                     pnlnotify.Visible = false;
                 }
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
             if (cmbUserType.Text == "Student")
             {
                 dvStudent.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
                 dgvSearch.DataSource = dvStudent;
                 toolTip1.SetToolTip(txtSearch, "search student lastname");
                 if (dgvSearch.Rows.Count > 0 && txtSearch.Text != "")
                 {
                     pnlnotify.Visible = false;
                 }
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
             if (cmbUserType.Text == "Cashier")
             {
                 dvCashier.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
                 dgvSearch.DataSource = dvCashier;
                 toolTip1.SetToolTip(txtSearch, "search cashier lastname");

                 if (dgvSearch.Rows.Count > 0 && txtSearch.Text != "")
                 {
                     pnlnotify.Visible = false;
                 }
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
             if (cmbUserType.Text == "Registrar")
             {
                 dvRegistrar.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
                 dgvSearch.DataSource = dvRegistrar;
                 toolTip1.SetToolTip(txtSearch, "search registrar lastname");

                 if (dgvSearch.Rows.Count > 0 && txtSearch.Text != "")
                 {
                     pnlnotify.Visible = false;
                 }
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
             if (cmbUserType.Text == "Principal")
             {
                 dvPrincipal.RowFilter = string.Format("Lastname LIKE '%{0}%'", txtSearch.Text);
                 dgvSearch.DataSource = dvPrincipal;
                 toolTip1.SetToolTip(txtSearch, "search principal lastname");

                 if (dgvSearch.Rows.Count > 0 && txtSearch.Text != "")
                 {
                     pnlnotify.Visible = false;
                 }
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

         private void txtAdmCon_KeyPress(object sender, KeyPressEventArgs e)
         {
             char ch = e.KeyChar;
             if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
             {
                 e.Handled = true;
             }
         }

         private void cmbAdmMonth_SelectedIndexChanged(object sender, EventArgs e)
         {
          
         }

         private void cmbFacGrd_SelectedIndexChanged(object sender, EventArgs e)
         {

         }

         private void btnFacClear_Click(object sender, EventArgs e)
         {
             if (btnFacClear.Text == "Clear")
             {
                 setupclear_forFaculty();
                 btnFacUpdate.Enabled = false;
                 btnFacDelete.Enabled = false;
                 btnFacAdd.Enabled = true;
                 btnFacUpdate.Text = "Update";
             }
             else
             {
                 btnFacDelete.Enabled = true;
                 btnFacClear.Text = "Clear";
                 btnFacUpdate.Text = "Update";

                 primarykey = lblKey.Text;
                 setupretrieveddata_forFaculty(primarykey);
                 setupdisableinput_forFaculty();
             }

       
             if (dgvSearch.Rows.Count >= 1)
             {
                 dgvSearch.Rows[0].Selected = true;
             }

             txtSearch.Focus();
         }

         private void cmbFacSubj_SelectedIndexChanged(object sender, EventArgs e)
         {

         }

         private void btnFacAdd_Click(object sender, EventArgs e)
         {

             if (txtFacFname.Text == "" || txtFacLast.Text == "" || txtFacUser.Text == "" || txtFacPass.Text == "")
             {
                 MessageBox.Show("fill out required field.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;
             }
             if (txtFacUser.Text != "")
             {
                 if (txtFacUser.TextLength < 6)
                 {
                    
                     inputcheck_forMaintenance("invalid", txtFacUser);
                 }
                 else
                 {
                     inputcheck_forMaintenance("valid", txtFacUser);
                 }
             }
             if (txtFacMidl.Text != "")
             {
                 if (txtFacMidl.TextLength < 2)
                 {
                     inputcheck_forMaintenance("invalid", txtFacMidl);
                 }
                 else
                 {
                     inputcheck_forMaintenance("valid", txtFacMidl);
                 }
             }
             if (txtFacPass.Text != "")
             {
                 if (txtFacPass.TextLength < 6)
                 {
                     inputcheck_forMaintenance("invalid", txtFacPass);
                 }
                 else
                 {
                     inputcheck_forMaintenance("valid", txtFacPass);
                 }
             }
             if ((txtFacUser.TextLength > 5) && (txtFacPass.TextLength > 5))
             {
                 con.Open();
                 OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno<>'"+primarykey+"' and username='" + txtFacUser.Text + "'and password='" + txtFacPass.Text + "'", con);
                 DataTable dt = new DataTable();
                 da.Fill(dt);
                 con.Close();
                 if (dt.Rows.Count > 0)
                 {
                     MessageBox.Show("username or password not unique.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 else
                 {
                     if (txtFacMidl.Text != "")
                     {
                         if (txtFacMidl.TextLength > 1)
                         {
                             setupaddoperation_forFaculty();
                         }
                     }
                     else
                     {
                         setupaddoperation_forFaculty();
                     }  
                 }
                
             }
         }

         private void txtFacCon_KeyPress(object sender, KeyPressEventArgs e)
         {
             char ch = e.KeyChar;
             if (!Char.IsDigit(ch) && ch != 8 && ch != 36)
             {
                 e.Handled = true;
             }
         }

         private void cmbFacMonth_SelectedIndexChanged(object sender, EventArgs e)
         {
             computeAge();
         }

         private void btnFacUpdate_Click(object sender, EventArgs e)
         {
            
             if (btnFacUpdate.Text == "Update")
             {
                 btnFFirst.Enabled = true; btnFMid.Enabled = true; btnFLast.Enabled = true;
                 setupenableinput_forFaculty();
                 btnFacUpdate.Text = "Save";
                 btnFacDelete.Enabled = false;
                 btnFacClear.Text = "Cancel";
             }
             else
             {

                 if (txtFacFname.Text == "" || txtFacLast.Text == "" ||txtFacUser.Text == "" || txtFacPass.Text == "")
                 {
                     MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 if (txtFacMidl.Text != "")
                 {
                     if (txtFacMidl.TextLength < 2)
                     {

                         inputcheck_forMaintenance("invalid", txtFacMidl);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtFacMidl);
                     }
                 }
                 if (txtFacUser.Text != "")
                 {
                     if (txtFacUser.TextLength < 6)
                     {
                        
                         inputcheck_forMaintenance("invalid", txtFacUser);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtFacUser);
                     }
                 }
                 if (txtFacPass.Text != "")
                 {
                     if (txtFacPass.TextLength < 6)
                     {
                        
                         inputcheck_forMaintenance("invalid", txtFacPass);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtFacPass);
                     }
                 }
                 if ((txtFacUser.TextLength > 5) && (txtFacPass.TextLength > 5))
                 {
                     con.Open();
                     OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno<>'"+primarykey+"' and username='" + txtFacUser.Text + "'and password='" + txtFacPass.Text + "'", con);
                     DataTable dt = new DataTable();
                     da.Fill(dt);
                     con.Close();
                     if (dt.Rows.Count > 0)
                     {
                         MessageBox.Show("username or password not unique.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         return;
                     }
                     else
                     {
                         if (txtFacMidl.Text != "")
                         {
                             if (txtFacMidl.TextLength > 1)
                             {
                                 setupsaveoperation_forFaculty();
                                 btnFacUpdate.Text = "Update";
                                 btnFacUpdate.Enabled = false;
                                 btnFacDelete.Enabled = false;
                                 if (dgvSearch.Rows.Count >= 1)
                                 {
                                     dgvSearch.Rows[0].Selected = true;
                                 }
                             }
                         }
                         else
                         {
                             setupsaveoperation_forFaculty();
                             btnFacUpdate.Text = "Update";
                             btnFacUpdate.Enabled = false;
                             btnFacDelete.Enabled = false;
                             if (dgvSearch.Rows.Count >= 1)
                             {
                                 dgvSearch.Rows[0].Selected = true;
                             }
                         } 
                     }
                 }
             }
         }

         private void btnFacDelete_Click(object sender, EventArgs e)
         {
             
             if (MessageBox.Show("Do you really want to delete?", "User maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
             {
                 if (txtFacFname.Text == "" || txtFacLast.Text == "" || txtFacUser.Text == "" || txtFacPass.Text == "")
                 {
                     MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 if (txtFacUser.Text != "")
                 {
                     if (txtFacUser.TextLength < 6)
                     {
                         txtFacUser.Focus();
                         inputcheck_forMaintenance("invalid", txtFacUser);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtFacUser);
                     }
                 }
                 if (txtFacPass.Text != "")
                 {
                     if (txtFacPass.TextLength < 6)
                     {
                         txtFacPass.Focus();
                         inputcheck_forMaintenance("invalid", txtFacPass);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtFacPass);
                     }
                 }
                 if ((txtFacUser.TextLength >= 5) && (txtFacPass.TextLength >= 5))
                 {
                     setupdeleteoperation_forFaculty();
                     btnFacUpdate.Enabled = false;
                     btnFacDelete.Enabled = false;
                     setupclear_forFaculty();
                     setupenableinput_forFaculty();
                     btnFacAdd.Enabled = true;
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


         private void btnEmpAdd_Click(object sender, EventArgs e)
         {

             if (txtEmpFname.Text == "" || txtEmpLast.Text == ""|| txtEmpUser.Text == "" || txtEmpPass.Text == "")
             {
                 MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;
             }
             if (txtEmpMidl.Text != "")
             {
                 if (txtEmpMidl.TextLength < 2)
                 {

                     inputcheck_forMaintenance("invalid", txtEmpMidl);
                 }
                 else
                 {
                     inputcheck_forMaintenance("valid", txtEmpMidl);
                 }
             }
             if (txtEmpUser.Text != "")
             {
                 if (txtEmpUser.TextLength < 6)
                 {
                   
                     inputcheck_forMaintenance("invalid", txtEmpUser);
                 }
                 else
                 {
                     inputcheck_forMaintenance("valid", txtEmpUser);
                 }
             }
             if (txtEmpPass.Text != "")
             {
                 if (txtEmpPass.TextLength < 6)
                 {
                    
                     inputcheck_forMaintenance("invalid", txtEmpPass);
                 }
                 else
                 {
                     inputcheck_forMaintenance("valid", txtEmpPass);
                 }
             }
             if ((txtEmpUser.TextLength > 5) && (txtEmpPass.TextLength > 5))
             {
                 con.Open();
                 OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where username='" + txtEmpUser.Text + "'or password='" + txtEmpPass.Text + "'", con);
                 DataTable dt = new DataTable();
                 da.Fill(dt);
                 con.Close();
                 if (dt.Rows.Count > 0)
                 {
                     MessageBox.Show("username or password not unique.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 else
                 {
                     if (txtEmpMidl.Text != "")
                     {
                         if (txtEmpMidl.TextLength > 1)
                         {
                             setupaddoperation_forEmployee();
                         }
                     }
                     else
                     {
                         setupaddoperation_forEmployee();
                     }
                 }
                 
             }
         }

         private void btnEmpUpdate_Click(object sender, EventArgs e)
         {
            
             if (btnEmpUpdate.Text == "Update")
             {
                 btnEFirst.Enabled = true; btnEMidl.Enabled = true; btnELast.Enabled = true;
                 setupenableinput_forEmployee();
                 btnEmpUpdate.Text = "Save";
                 btnEmpDelete.Enabled = false;
                 btnEmpClear.Text = "Cancel";
             }
             else
             {


                 if (txtEmpFname.Text == "" || txtEmpLast.Text == "" || txtEmpUser.Text == "" || txtEmpPass.Text == "")
                 {
                     MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 if (txtEmpMidl.Text != "")
                 {
                     if (txtEmpMidl.TextLength < 2)
                     {

                         inputcheck_forMaintenance("invalid", txtEmpMidl);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtEmpMidl);
                     }
                 }
                 if (txtEmpUser.Text != "")
                 {
                     if (txtEmpUser.TextLength < 6)
                     {
                        
                         inputcheck_forMaintenance("invalid", txtEmpUser);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtEmpUser);
                     }
                 }
                 if (txtEmpPass.Text != "")
                 {
                     if (txtEmpPass.TextLength < 6)
                     {
                        
                         inputcheck_forMaintenance("invalid", txtEmpPass);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtEmpPass);
                     }
                 }
                 if ((txtEmpUser.TextLength > 5) && (txtEmpPass.TextLength > 5))
                 {
                     con.Open();
                     OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno<>'"+primarykey+"'and username='" + txtEmpUser.Text + "'and password='" + txtEmpPass.Text + "'", con);
                     DataTable dt = new DataTable();
                     da.Fill(dt);
                     con.Close();
                     if (dt.Rows.Count > 0)
                     {
                         MessageBox.Show("username or password not unique.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         return;
                     }
                     else
                     {
                         if (txtEmpMidl.Text != "")
                         {
                             if (txtEmpMidl.TextLength > 1)
                             {
                                 setupsaveoperation_forEmployee();
                                 btnEmpUpdate.Text = "Update";
                                 btnEmpUpdate.Enabled = false;
                                 btnEmpDelete.Enabled = false;
                                 if (dgvSearch.Rows.Count >= 1)
                                 {
                                     dgvSearch.Rows[0].Selected = true;
                                 }
                             }
                         }
                         else
                         {
                             setupsaveoperation_forEmployee();
                             btnEmpUpdate.Text = "Update";
                             btnEmpUpdate.Enabled = false;
                             btnEmpDelete.Enabled = false;
                             if (dgvSearch.Rows.Count >= 1)
                             {
                                 dgvSearch.Rows[0].Selected = true;
                             }
                         }
                     }
                 }
             }
         }

         private void btnEmpDelete_Click(object sender, EventArgs e)
         {
             if (MessageBox.Show("Do you really want to delete?", "User maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
             {

                 if (txtEmpFname.Text == "" || txtEmpLast.Text == "" || txtEmpUser.Text == "" || txtEmpPass.Text == "")
                 {
                     MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 if (txtEmpMidl.Text != "")
                 {
                     if (txtEmpMidl.TextLength < 2)
                     {

                         inputcheck_forMaintenance("invalid", txtEmpMidl);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtEmpMidl);
                     }
                 }
                 if (txtEmpUser.Text != "")
                 {
                     if (txtEmpUser.TextLength < 6)
                     {
                       
                         inputcheck_forMaintenance("invalid", txtEmpUser);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtEmpUser);
                     }
                 }
                 if (txtEmpPass.Text != "")
                 {
                     if (txtEmpPass.TextLength < 6)
                     {
                        
                         inputcheck_forMaintenance("invalid", txtEmpPass);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtEmpPass);
                     }
                 }
                 if (((txtEmpUser.TextLength > 5) && (txtEmpPass.TextLength > 5)))
                 {
                     if (txtEmpMidl.Text != "")
                     {
                         if (txtEmpMidl.TextLength > 1)
                         {
                             setupdeleteoperation_forEmployee();
                             btnEmpUpdate.Enabled = false;
                             btnEmpDelete.Enabled = false;
                             setupclear_forEmployee();
                             setupenableinput_forEmployee();
                             btnEmpAdd.Enabled = true;
                             if (dgvSearch.Rows.Count >= 1)
                             {
                                 dgvSearch.Rows[0].Selected = true;
                             }
                         }
                     }
                     else
                     {
                         setupdeleteoperation_forEmployee();
                         btnEmpUpdate.Enabled = false;
                         btnEmpDelete.Enabled = false;
                         setupclear_forEmployee();
                         setupenableinput_forEmployee();
                         btnEmpAdd.Enabled = true;
                         if (dgvSearch.Rows.Count >= 1)
                         {
                             dgvSearch.Rows[0].Selected = true;
                         }
                     }
                 }
             }
             else
             {
                 return;
             }
         }

         private void btnEmpClear_Click(object sender, EventArgs e)
         {
             if (btnEmpClear.Text == "Clear")
             {
                 setupclear_forEmployee();
                 btnEmpUpdate.Enabled = false;
                 btnEmpDelete.Enabled = false;
                 btnEmpAdd.Enabled = true;
                 btnEmpUpdate.Text = "Update";
             }
             else
             {
                 btnEmpDelete.Enabled = true;
                 btnEmpClear.Text = "Clear";
                 btnEmpUpdate.Text = "Update";

                 primarykey = lblKey.Text;
                 setupretrieveddata_forEmployee(primarykey);
                 setupdisableinput_forEmployee();
             }

           
             if (dgvSearch.Rows.Count >= 1)
             {
                 dgvSearch.Rows[0].Selected = true;
             }
             txtSearch.Focus();
         }

         private void txtEmpCon_KeyPress(object sender, KeyPressEventArgs e)
         {
             char ch = e.KeyChar;
             if (!Char.IsDigit(ch) && ch != 8 && ch != 36)
             {
                 e.Handled = true;
             }
         }

         private void btnStdAdd_Click(object sender, EventArgs e)
         {
             if ((txtStdFname.Text == "" || txtStdLname.Text == ""))
             {
                 MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;
             }
             
            if (isnospace==false)
            {
                setupaddoperation_forStudent();
            }
            
         }

         private void btnStdUpdate_Click(object sender, EventArgs e)
         {
             if (btnStdUpdate.Text == "Update")
             {
                 btnSFirst.Enabled = true; btnSMid.Enabled = true; btnSLast.Enabled = true;
                 setupenableinput_forStudent();
                 btnStdUpdate.Text = "Save";
                 btnStdDelete.Enabled = false;
                 btnStdClear.Text = "Cancel";
             }
             else
             {
                 if (txtStdFname.Text == "" || txtStdLname.Text == "")
                 {
                     MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
                 if (txtStdMname.Text != "")
                 {
                     if (txtStdMname.TextLength < 2)
                     {
                         inputcheck_forMaintenance("invalid", txtStdMname);
                     }
                     else
                     {
                         inputcheck_forMaintenance("valid", txtStdMname);
                     }
                 }
                if(txtUsernameStd.Text!="")
                {
                    if (txtUsernameStd.TextLength < 6)
                    {
                        inputcheck_forMaintenance("invalid", txtUsernameStd);
                    }
                    else
                    {
                        inputcheck_forMaintenance("valid", txtUsernameStd);
                    }
                }
                if (txtPasswordStd.Text != "")
                {
                    if (txtPasswordStd.TextLength < 6)
                    {
                        inputcheck_forMaintenance("invalid", txtPasswordStd);
                    }
                    else
                    {
                        inputcheck_forMaintenance("valid", txtPasswordStd);
                    }
                }
                if (txtUsernameStd.Text != "" && txtPasswordStd.Text != "")
                {
                     con.Open();
                     OdbcDataAdapter da = new OdbcDataAdapter("Select*from studacct_tbl where username='" + txtUsernameStd.Text + "'and password='" + txtPasswordStd.Text + "'and studno<>'"+primarykey+"'", con);
                     DataTable dt = new DataTable();
                     da.Fill(dt);
                     con.Close();
                     if (dt.Rows.Count > 0)
                     {
                         MessageBox.Show("username or password not unique.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         return;
                     }    
                }
                if ((txtStdFname.Text!="" && txtStdLname.Text!=""))
                {
                    if (txtUsernameStd.Text != "" && txtPasswordStd.Text != "")
                    {
                        if (txtUsernameStd.TextLength > 5 && txtPasswordStd.TextLength > 5)
                        {
                            if (txtStdMname.Text != "")
                            {
                                if (txtStdMname.TextLength > 1)
                                {
                                    setupsaveoperation_forStudent();
                                    btnStdUpdate.Text = "Update";
                                    btnStdUpdate.Enabled = false;
                                    btnStdDelete.Enabled = false;
                                    if (dgvSearch.Rows.Count >= 1)
                                    {
                                        dgvSearch.Rows[0].Selected = true;
                                    } 
                                }
                            }
                            else
                            {
                                setupsaveoperation_forStudent();
                                btnStdUpdate.Text = "Update";
                                btnStdUpdate.Enabled = false;
                                btnStdDelete.Enabled = false;
                                if (dgvSearch.Rows.Count >= 1)
                                {
                                    dgvSearch.Rows[0].Selected = true;
                                } 
                            }  
                        }
                    }
                }
                if ((txtStdFname.Text != "" && txtStdLname.Text != ""))
                {
                    if (txtUsernameStd.Text == "" && txtPasswordStd.Text == "")
                    {
                        if (txtStdMname.Text != "")
                        {
                            if (txtStdMname.TextLength > 1)
                            {
                                setupsaveoperation_forStudent();
                                btnStdUpdate.Text = "Update";
                                btnStdUpdate.Enabled = false;
                                btnStdDelete.Enabled = false;
                                if (dgvSearch.Rows.Count >= 1)
                                {
                                    dgvSearch.Rows[0].Selected = true;
                                }
                            }
                        }
                        else
                        {
                            setupsaveoperation_forStudent();
                            btnStdUpdate.Text = "Update";
                            btnStdUpdate.Enabled = false;
                            btnStdDelete.Enabled = false;
                            if (dgvSearch.Rows.Count >= 1)
                            {
                                dgvSearch.Rows[0].Selected = true;
                            }
                        } 
                    }
                }

               }
         }

         private void btnStdDelete_Click(object sender, EventArgs e)
         {

             if (MessageBox.Show("Do you really want to delete?", "User maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
             {
                 if (txtStdFname.Text == "" || txtStdLname.Text == "")
                 {
                     MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
          
                setupdeleteoperation_forStudent();
                btnStdUpdate.Enabled = false;
                btnStdDelete.Enabled = false;
                setupclear_forStudent();
                setupenableinput_forStudent();
                btnStdAdd.Enabled = true;
                if (dgvSearch.Rows.Count >= 1)
                {
                    dgvSearch.Rows[0].Selected = true;
                }
             }
             else
             {
                 return;
             }
         }

         private void btnStdClear_Click(object sender, EventArgs e)
         {
             if (btnStdClear.Text == "Clear")
             {
                 setupclear_forStudent();
                 btnStdUpdate.Enabled = false;
                 btnStdDelete.Enabled = false;
                 btnStdAdd.Enabled = true;
                 btnStdUpdate.Text = "Update";
             }
             else
             {
                 btnStdDelete.Enabled = true;
                 btnStdClear.Text = "Clear";
                 btnStdUpdate.Text = "Update";
                
                 primarykey = lblKey.Text;
                 setupretrieveddata_forStudent(primarykey);
                 setupdisableinput_forStudent();
             }

          
             if (dgvSearch.Rows.Count >= 1)
             {
                 dgvSearch.Rows[0].Selected = true;
             }

             txtSearch.Focus();
         }

         private void txtStdPGCon_KeyPress(object sender, KeyPressEventArgs e)
         {
             char ch = e.KeyChar;
             if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
             {
                 e.Handled = true;
             }
         }

         private void txtStdCon_KeyPress(object sender, KeyPressEventArgs e)
         {
             char ch = e.KeyChar;
             if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
             {
                 e.Handled = true;
             }
         }

         /************************************** METHODS FOR USER MAINTENANCE > ADMIN***********************************************************
         ***************************************************************************************************************************************/

        public void setupyears_forAdmin()
        {
            int start=1970;
            int current = Convert.ToInt32(DateTime.Now.Year);

            while (current >= start)
            {
                cmbAdmYears.Items.Add(current);
                current--;
            }
        }

        public void setupdays_forAdmin()
        {
            cmbAdmDay.Items.Clear();

            int start = 1;
            while (start <=31)
            {
                if (start < 10)
                {
                    cmbAdmDay.Items.Add("0" + start);
                }
                else
                {
                    cmbAdmDay.Items.Add(start);
                }
                start++;
            }
        }

        public void setupclear_forAdmin()
        {
            txtAdmFname.Clear();
            txtAdmMidl.Clear();
            txtAdmLast.Clear();
         
            cmbAdmMonth.SelectedIndex = -1;
            cmbAdmDay.SelectedIndex = -1;
            cmbAdmYears.SelectedIndex = -1;
            cmbAdmGen.SelectedIndex = -1;
            txtAdmCon.Clear();
          
            txtAdmUser.Clear();
            txtAdmPass.Clear();
            lblKey.Text = "";
            

            setupenableinput_forAdmin();
        }

         public void setupdisableinput_forAdmin()
        {
            txtAdmFname.Enabled=false;
            txtAdmMidl.Enabled=false;
            txtAdmLast.Enabled=false;
            btnN1.Enabled = false;
            btnN2.Enabled = false;
            btnN3.Enabled = false;
            cmbAdmMonth.Enabled=false;
            cmbAdmDay.Enabled=false;
            cmbAdmYears.Enabled=false;
            cmbAdmGen.Enabled=false;
            txtAdmCon.Enabled=false;
           
            txtAdmUser.Enabled=false;
            txtAdmPass.Enabled=false;
            btnN1.Enabled = false;
            btnN2.Enabled = false;
            btnN3.Enabled = false;
        }

         public void setupenableinput_forAdmin()
         {
             txtAdmFname.Enabled = true;
             txtAdmMidl.Enabled = true;
             txtAdmLast.Enabled = true;
             btnN1.Enabled = true;
             btnN2.Enabled = true;
             btnN3.Enabled = true;
             cmbAdmMonth.Enabled = true;
             cmbAdmDay.Enabled = true;
             cmbAdmYears.Enabled = true;
             cmbAdmGen.Enabled = true;
             txtAdmCon.Enabled = true;
             btnN1.Enabled = true;
             btnN2.Enabled = true;
             btnN3.Enabled = true;
            
             txtAdmUser.Enabled = true;
             txtAdmPass.Enabled = true;
         }

         public void setupaddoperation_forAdmin()
         {
             string concatBday = cmbAdmMonth.Text + " " + cmbAdmDay.Text + " " + cmbAdmYears.Text;
             int current = Convert.ToInt32(DateTime.Now.Year);
             int birthyear = Convert.ToInt32(cmbAdmYears.Text);
             int age = current - birthyear;

             con.Open();
             string addAdmin = "Insert Into admin_tbl(firstname,middlename,lastname,birthdate,age,gender,contactno,username,password)values('"+txtAdmFname.Text+"','"+
             txtAdmMidl.Text+"','"+txtAdmLast.Text+"','"+concatBday+"','"+age+"','"+cmbAdmGen.Text+"','"+txtAdmCon.Text+"','"+txtAdmUser.Text+"','"+txtAdmPass.Text+"')";
             OdbcCommand cmdAddAdmin = new OdbcCommand(addAdmin, con);
             cmdAddAdmin.ExecuteNonQuery();
             con.Close();

             btnAdmAdd.Enabled = false;
             setupview_forAdmin();
             MessageBox.Show("admin successfully added", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

             txtAdmCon.BackColor = Color.White;
             txtAdmUser.BackColor = Color.White;
             txtAdmMidl.BackColor = Color.White;
             txtAdmPass.BackColor = Color.White;
             txtSearch.Focus();
             
         }

         public void setupsaveoperation_forAdmin()
         {
             string bday = cmbAdmMonth.Text+" "+cmbAdmDay.Text+" "+cmbAdmYears.Text;
              int current = Convert.ToInt32(DateTime.Now.Year);
             int birthyear = Convert.ToInt32(cmbAdmYears.Text);
             int age = current - birthyear;

             con.Open();
             string updateAdmin = "Update admin_tbl set firstname='" + txtAdmFname.Text + "',middlename='" + txtAdmMidl.Text + "',lastname='" + txtAdmLast.Text + "',birthdate='" + 
                 bday + "',age='" + age + "',gender='" + cmbAdmGen.Text + "',contactno='" + txtAdmCon.Text + "',username='" +
                 txtAdmUser.Text + "',password='" + txtAdmPass.Text + "'where adminnum='"+lblKey.Text+"'";
             OdbcCommand cmdUpdateAdmin = new OdbcCommand(updateAdmin,con);
             cmdUpdateAdmin.ExecuteNonQuery();
             con.Close();

             btnAdmAdd.Enabled = false;
             setupview_forAdmin();
             btnAdmClear.Text = "Clear";
             MessageBox.Show("admin successfully updated", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

             txtAdmCon.BackColor = Color.White;
             txtAdmMidl.BackColor = Color.White;
             txtAdmUser.BackColor = Color.White;
             txtAdmPass.BackColor = Color.White;
             txtSearch.Focus();
         }

         public void setupdeleteoperation_forAdmin()
         {
             con.Open();
             string deleteAdmin = "Delete from admin_tbl where adminnum='" + lblKey.Text + "'";
             OdbcCommand cmdDeleteAdmin = new OdbcCommand(deleteAdmin, con);
             cmdDeleteAdmin.ExecuteNonQuery();
             con.Close();

             btnAdmAdd.Enabled = false;
             setupview_forAdmin();
             MessageBox.Show("admin successfully deleted", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

             txtAdmCon.BackColor = Color.White;
             txtAdmMidl.BackColor = Color.White;
             txtAdmUser.BackColor = Color.White;
             txtAdmPass.BackColor = Color.White;
             txtSearch.Focus();
         }

         public void setupretrieveddata_forAdmin(string thekey)
         {
             con.Open();
             OdbcDataAdapter da = new OdbcDataAdapter("Select*from admin_tbl where adminnum='" + thekey + "'", con);
             DataTable dt = new DataTable();
             da.Fill(dt);
             con.Close();

             if (dt.Rows.Count > 0)
             {
                 lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                 txtAdmFname.Text = dt.Rows[0].ItemArray[1].ToString();
                 txtAdmMidl.Text = dt.Rows[0].ItemArray[2].ToString();
                 txtAdmLast.Text = dt.Rows[0].ItemArray[3].ToString();
                 //txtAdmAdd.Text = dt.Rows[0].ItemArray[4].ToString();
                 cmbAdmMonth.Text = dt.Rows[0].ItemArray[5].ToString().Substring(0, 3).ToString();//0 start of string 3 the length 
                 cmbAdmDay.Text = dt.Rows[0].ItemArray[5].ToString().Substring(4, 2).ToString();
                 cmbAdmYears.Text = dt.Rows[0].ItemArray[5].ToString().Substring(7, 4).ToString();
                 cmbAdmGen.Text = dt.Rows[0].ItemArray[7].ToString();
                 txtAdmCon.Text = dt.Rows[0].ItemArray[8].ToString();
                 //txtAdmEmail.Text = dt.Rows[0].ItemArray[9].ToString();
                 txtAdmUser.Text = dt.Rows[0].ItemArray[10].ToString();
                 txtAdmPass.Text = dt.Rows[0].ItemArray[11].ToString();

                 btnAdmAdd.Enabled = false;
             }
         }
        
         public void setupview_forAdmin()
         {
             dgvSearch.DataSource = null;

             con.Open();
             OdbcDataAdapter daAdmin = new OdbcDataAdapter("Select lastname as 'Lastname',firstname as 'Firstname',middlename as 'Middlename' from admin_tbl order by lastname ASC", con);
             DataTable dtadmin = new DataTable();
             daAdmin.Fill(dtadmin);
             con.Close();
             dvAdmin = new DataView(dtadmin);
             dgvSearch.DataSource = dvAdmin;

             //dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             dgvSearch.Columns[0].Width = 155;
             dgvSearch.Columns[1].Width = 155;
             dgvSearch.Columns[2].Width = 155;
            
             lblResult.Text = "number of admin: " + dgvSearch.Rows.Count.ToString();
         }

        /**********************************************END METHODS FOR USER MAINTENANCE > ADMIN*************************************************
         ***************************************************************************************************************************************/




         /************************************** METHODS FOR USER MAINTENANCE > FACULTY *********************************************************
         ***************************************************************************************************************************************/

         public void setupyears_forFaculty()
         {
             int start = 1970;
             int current = Convert.ToInt32(DateTime.Now.Year);

             while (current >= start)
             {
                 //cmbFacYear.Items.Add(current);
                 current--;
             }
         }

         public void setupdays_forFaculty()
         {
             //cmbFacDay.Items.Clear();

             int start = 1;
             while (start <= 31)
             {
                 if (start < 10)
                 {
                     //cmbFacDay.Items.Add("0" + start);
                 }
                 else
                 {
                     //cmbFacDay.Items.Add(start);
                 }
                 start++;
             }
         }

         public void setupretrieveddata_forFaculty(string thekey)
         {
             con.Open();
             OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno='" + thekey + "'", con);
             DataTable dt = new DataTable();
             da.Fill(dt);
             con.Close();

             if (dt.Rows.Count > 0)
             {
                 lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                 txtFacFname.Text = dt.Rows[0].ItemArray[1].ToString();
                 txtFacMidl.Text = dt.Rows[0].ItemArray[2].ToString();
                 txtFacLast.Text = dt.Rows[0].ItemArray[3].ToString();
                 cmbFacGrd.Text = dt.Rows[0].ItemArray[14].ToString();
                 cmbFacAdv.Text = dt.Rows[0].ItemArray[15].ToString();
                 txtFacUser.Text = dt.Rows[0].ItemArray[16].ToString();
                 txtFacPass.Text = dt.Rows[0].ItemArray[17].ToString();

                 btnFacAdd.Enabled = false;
             }
         }

         public void setupview_forFacultyAcct()
         {
             dgvSearch.DataSource = null;

             con.Open();
             OdbcDataAdapter daFaculty = new OdbcDataAdapter("Select lname as 'Lastname',fname as 'Firstname',mname as 'Middlename' gender as 'Gender' from user_tbl order by lname ASC", con);
             DataTable dtfaculty = new DataTable();
             daFaculty.Fill(dtfaculty);
             con.Close();
             dvFaculty = new DataView(dtfaculty);
             dgvSearch.DataSource = dvFaculty;

             dgvSearch.Columns[0].Width = 130;
             dgvSearch.Columns[1].Width = 130;
             dgvSearch.Columns[2].Width = 130;
             dgvSearch.Columns[3].Width = 90;
           
             lblResult.Text = "number of faculty: " + dgvSearch.Rows.Count.ToString();
         }

         public void setupview_forFaculty()
         {
             dgvSearch.DataSource = null;

             con.Open();
             OdbcDataAdapter daFaculty = new OdbcDataAdapter("Select empno, lastname as 'Lastname',firstname as 'Firstname',middlename as 'Middlename' from employees_tbl where position='" + "faculty" + "'order by lastname ASC", con);
             DataTable dtfaculty = new DataTable();
             daFaculty.Fill(dtfaculty);
             con.Close();
             dvFaculty = new DataView(dtfaculty);
             dgvSearch.DataSource = dvFaculty;

             dgvSearch.Columns[0].Width = 0;
             dgvSearch.Columns[1].Width = 155;
             dgvSearch.Columns[2].Width = 155;
             dgvSearch.Columns[3].Width = 155;
             dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
             lblResult.Text = "number of faculty: " + dgvSearch.Rows.Count.ToString();
         }

         public void setupdisableinput_forFaculty()
         {
             txtFacFname.Enabled = false;
             txtFacMidl.Enabled = false;
             txtFacLast.Enabled = false;
            
             txtFacUser.Enabled = false;
             txtFacPass.Enabled = false;
            
             cmbFacGrd.Enabled = false;
             cmbFacAdv.Enabled = false;
             btnFFirst.Enabled = false;
             btnFMid.Enabled = false;
             btnFLast.Enabled = false;
            
         }

         public void setupenableinput_forFaculty()
         {
             txtFacFname.Enabled = true;
             txtFacMidl.Enabled = true;
             txtFacLast.Enabled = true;
           
             txtFacUser.Enabled = true;
             txtFacPass.Enabled = true;
           
             cmbFacGrd.Enabled = true;
             cmbFacAdv.Enabled = true;

             btnFFirst.Enabled = true;
             btnFMid.Enabled = true;
             btnFLast.Enabled = true;
         
         }

         public void setupclear_forFaculty()
         {
             txtFacFname.Clear();
             txtFacMidl.Clear();
             txtFacLast.Clear();
         
             txtFacUser.Clear();
             txtFacPass.Clear();
            
             cmbFacGrd.SelectedIndex = -1;
             cmbFacAdv.SelectedIndex = -1;
            
             lblKey.Text = "";

             setupenableinput_forFaculty();
         }

       
         public void setupaddoperation_forFaculty()
         {
             con.Open();
             string addFaculty2 = "Insert Into employees_tbl(firstname,middlename,lastname,address,birthdate,age,gender,civilstatus,contactnum,email,schoolgrad,position,subject,grade,advisory,username,password,educattainment)values('" + txtFacFname.Text + "','" +
             txtFacMidl.Text + "','" + txtFacLast.Text + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" +
             "" + "','" + "" + "','" + "faculty" + "','" + ""+ "','" + "" + "','" + "" + "','" + txtFacUser.Text + "','" + txtFacPass.Text + "','" + "" + "')";

             OdbcCommand cmdAddFaculty2 = new OdbcCommand(addFaculty2, con);
             cmdAddFaculty2.ExecuteNonQuery();
             con.Close();

             btnFacAdd.Enabled = false;
             setupview_forFaculty();
             //setupview_forFacultyAcct();
             MessageBox.Show("faculty successfully added", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

             txtFacMidl.BackColor = Color.White;
             txtFacUser.BackColor = Color.White;
             txtFacPass.BackColor = Color.White;
             txtSearch.Focus();

         }

         public void setupsaveoperation_forFaculty()
         {
           
             con.Open();
             string updateFaculty2 = "Update employees_tbl set firstname='" + txtFacFname.Text + "',middlename='" + txtFacMidl.Text + "',lastname='" + txtFacLast.Text + "',position='" + "faculty" + "',username='" +
                 txtFacUser.Text + "',password='" + txtFacPass.Text + "'where empno='" + primarykey + "'";
             OdbcCommand cmdUpdateFaculty2 = new OdbcCommand(updateFaculty2, con);
             cmdUpdateFaculty2.ExecuteNonQuery();
             con.Close();

             btnFacAdd.Enabled = false;
             setupview_forFaculty();
             //setupview_forFacultyAcct();
             btnFacClear.Text = "Clear";
             MessageBox.Show("faculty successfully updated", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

             txtFacMidl.BackColor = Color.White;
             txtFacUser.BackColor = Color.White;
             txtFacPass.BackColor = Color.White;
             txtSearch.Focus();
         }

         public void setupdeleteoperation_forFaculty()
         {
             con.Open();
             string deleteFaculty = "Delete from employees_tbl where empno='" + primarykey + "'";
             OdbcCommand cmdDeleteFaculty = new OdbcCommand(deleteFaculty, con);
             cmdDeleteFaculty.ExecuteNonQuery();
             con.Close();

             btnFacAdd.Enabled = false;
             setupview_forFaculty();
             //setupview_forFacultyAcct();
             MessageBox.Show("faculty successfully deleted", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

             txtFacMidl.BackColor = Color.White;
             txtFacUser.BackColor = Color.White;
             txtFacPass.BackColor = Color.White;
             txtSearch.Focus();
         }

        /******************************************** END METHODS FOR USER MAINTENANCE > FACULTY **********************************************
        ***************************************************************************************************************************************/




        /************************************** METHODS FOR USER MAINTENANCE > EMPLOYEE *******************************************************
        ***************************************************************************************************************************************/

         public void setupretrieveddata_forEmployee(string thekey)
         {
             con.Open();
             OdbcDataAdapter da = new OdbcDataAdapter("Select*from employees_tbl where empno='" + thekey + "'", con);
             DataTable dt = new DataTable();
             da.Fill(dt);
             con.Close();
             if (dt.Rows.Count > 0)
             {
                 lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                 txtEmpFname.Text = dt.Rows[0].ItemArray[1].ToString();
                 txtEmpMidl.Text = dt.Rows[0].ItemArray[2].ToString();
                 txtEmpLast.Text = dt.Rows[0].ItemArray[3].ToString();
                 txtEmpUser.Text = dt.Rows[0].ItemArray[16].ToString();
                 txtEmpPass.Text = dt.Rows[0].ItemArray[17].ToString();

                 btnEmpAdd.Enabled = false;
             }
         }

         public void setupview_forCashier()
         {
             dgvSearch.DataSource = null;

             con.Open();
             OdbcDataAdapter daCashier = new OdbcDataAdapter("Select empno,lastname as 'Lastname',firstname as 'Firstname',middlename as 'Middlename' from employees_tbl where position='cashier' order by lastname ASC", con);
             DataTable dtCashier = new DataTable();
             daCashier.Fill(dtCashier);
             con.Close();
             dvCashier = new DataView(dtCashier);
             dgvSearch.DataSource = dvCashier;

            // dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             dgvSearch.Columns[0].Width = 0;
             dgvSearch.Columns[1].Width = 155;
             dgvSearch.Columns[2].Width = 155;
             dgvSearch.Columns[3].Width = 155;
             dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
             lblResult.Text = "number of cashier: " + dgvSearch.Rows.Count.ToString();
         }

         public void setupview_forRegistrar()
         {
             dgvSearch.DataSource = null;

             con.Open();
             OdbcDataAdapter daRegistrar = new OdbcDataAdapter("Select empno,lastname as 'Lastname',firstname as 'Firstname',middlename as 'Middlename' from employees_tbl where position='registrar' order by lastname ASC", con);
             DataTable dtRegistrar = new DataTable();
             daRegistrar.Fill(dtRegistrar);
             con.Close();
             dvRegistrar = new DataView(dtRegistrar);
             dgvSearch.DataSource = dvRegistrar;

             //dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             dgvSearch.Columns[0].Width = 0;
             dgvSearch.Columns[1].Width = 155;
             dgvSearch.Columns[2].Width = 155;
             dgvSearch.Columns[3].Width = 155;
             dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
             lblResult.Text = "number of registrar: " + dgvSearch.Rows.Count.ToString();
         }

         public void setupview_forPrincipal()
         {
             dgvSearch.DataSource = null;

             con.Open();
             OdbcDataAdapter daPrincipal = new OdbcDataAdapter("Select empno,lastname as 'Lastname',firstname as 'Firstname',middlename as 'Middlename' from employees_tbl where position='principal' order by lastname ASC", con);
             DataTable dtPrincipal = new DataTable();
             daPrincipal.Fill(dtPrincipal);
             con.Close();
             dvPrincipal= new DataView(dtPrincipal);
             dgvSearch.DataSource = dvPrincipal;

             //dgvSearch.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             dgvSearch.Columns[0].Width = 0;
             dgvSearch.Columns[1].Width = 155;
             dgvSearch.Columns[2].Width = 155;
             dgvSearch.Columns[3].Width = 155;
             dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
             lblResult.Text = "number of principal: " + dgvSearch.Rows.Count.ToString();
         }

         public void setupdisableinput_forEmployee()
         {
             txtEmpFname.Enabled = false;
             txtEmpMidl.Enabled = false;
             txtEmpLast.Enabled = false;
             txtEmpUser.Enabled = false;
             txtEmpPass.Enabled = false;
             btnEFirst.Enabled = false;
             btnEMidl.Enabled = false;
             btnELast.Enabled = false;
         }

         public void setupenableinput_forEmployee()
         {
             txtEmpFname.Enabled = true;
             txtEmpMidl.Enabled = true;
             txtEmpLast.Enabled = true;
             txtEmpUser.Enabled = true;
             txtEmpPass.Enabled = true;
             btnEFirst.Enabled = true;
             btnEMidl.Enabled = true;
             btnELast.Enabled = true;
         }

         public void setupclear_forEmployee()
         {
             txtEmpFname.Clear();
             txtEmpMidl.Clear();
             txtEmpLast.Clear();
             txtEmpUser.Clear();
             txtEmpPass.Clear();
             lblKey.Text = "";

             setupenableinput_forEmployee();
         }

         public void setupaddoperation_forEmployee()
         {
             string position = "";
             if (cmbUserType.Text == "Cashier")
             {
                 position = "cashier";
             }
             if (cmbUserType.Text == "Registrar")
             {
                 position = "registrar";
             }
             if (cmbUserType.Text == "Principal")
             {
                 position = "principal";
             }

             
             con.Open();
             string addEmployee = "Insert Into employees_tbl(firstname,middlename,lastname,position,username,password)values('" + txtEmpFname.Text + "','" +
             txtEmpMidl.Text + "','" + txtEmpLast.Text + "','" + position + "','" + txtEmpUser.Text + "','" + txtEmpPass.Text + "')";

             OdbcCommand cmdAddEmployee = new OdbcCommand(addEmployee, con);
             cmdAddEmployee.ExecuteNonQuery();
             con.Close();

             btnEmpAdd.Enabled = false;
           
             if (cmbUserType.Text == "Cashier")
             {
                 setupview_forCashier();
             }
             if (cmbUserType.Text == "Registrar")
             {
                 setupview_forRegistrar();
             }
             if (cmbUserType.Text == "Principal")
             {
                 setupview_forPrincipal();
             }

             MessageBox.Show(position+" successfully added", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
             txtEmpMidl.BackColor = Color.White;
             txtEmpUser.BackColor = Color.White;
             txtEmpPass.BackColor = Color.White;
             txtSearch.Focus();

         }

         public void setupsaveoperation_forEmployee()
         {
             string position = "";
             if (cmbUserType.Text == "Cashier")
             {
                 position = "cashier";
             }
             if (cmbUserType.Text == "Registrar")
             {
                 position = "registrar";
             }
             if (cmbUserType.Text == "Principal")
             {
                 position = "principal";
             }

             con.Open();
             string updateEmployee = "Update employees_tbl set firstname='" + txtEmpFname.Text + "',middlename='" + txtEmpMidl.Text + "',lastname='" + txtEmpLast.Text + "',position='" + position + "',username='" +
                 txtEmpUser.Text + "',password='" + txtEmpPass.Text + "'where empno='" + lblKey.Text + "'";
             OdbcCommand cmdUpdateEmployee = new OdbcCommand(updateEmployee, con);
             cmdUpdateEmployee.ExecuteNonQuery();
             con.Close();

             btnEmpAdd.Enabled = false;

             if (cmbUserType.Text == "Cashier")
             {
                 setupview_forCashier();
             }
             if (cmbUserType.Text == "Registrar")
             {
                 setupview_forRegistrar();
             }
             if (cmbUserType.Text == "Principal")
             {
                 setupview_forPrincipal();
             }
             btnEmpClear.Text = "Clear";
             MessageBox.Show(position+" successfully updated", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);

             txtEmpMidl.BackColor = Color.White;
             txtEmpUser.BackColor = Color.White;
             txtEmpPass.BackColor = Color.White;
             txtSearch.Focus();
         }

         public void setupdeleteoperation_forEmployee()
         {
             string position = "";
             if (cmbUserType.Text == "Cashier")
             {
                 position = "cashier";
             }
             if (cmbUserType.Text == "Registrar")
             {
                 position = "registrar";
             }
             if (cmbUserType.Text == "Principal")
             {
                 position = "principal";
             }

             con.Open();
             string deleteEmployee = "Delete from employees_tbl where empno='" + lblKey.Text + "'";
             OdbcCommand cmdDeleteEmployee = new OdbcCommand(deleteEmployee, con);
             cmdDeleteEmployee.ExecuteNonQuery();
             con.Close();

             btnEmpAdd.Enabled = false;

             if (cmbUserType.Text == "Cashier")
             {
                 setupview_forCashier();
             }
             if (cmbUserType.Text == "Registrar")
             {
                 setupview_forRegistrar();
             }
             if (cmbUserType.Text == "Principal")
             {
                 setupview_forPrincipal();
             }
             MessageBox.Show(position+ " successfully deleted", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
             txtEmpMidl.BackColor = Color.White;
             txtEmpUser.BackColor = Color.White;
             txtEmpPass.BackColor = Color.White;
             txtSearch.Focus();
         }



         /************************************** METHODS FOR USER MAINTENANCE > STUDENT *********************************************************
         ***************************************************************************************************************************************/

         

         public void setupretrieveddata_forStudent(string thekey)
         {
             con.Open();
             OdbcDataAdapter da = new OdbcDataAdapter("Select*from stud_tbl where studno='" + thekey + "'", con);
             DataTable dt = new DataTable();
             da.Fill(dt);
             con.Close();

             if (dt.Rows.Count > 0)
             {

                 lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                 txtStdFname.Text = dt.Rows[0].ItemArray[1].ToString();
                 txtStdMname.Text = dt.Rows[0].ItemArray[2].ToString();
                 txtStdLname.Text = dt.Rows[0].ItemArray[3].ToString();
               
                 btnStdAdd.Enabled = false;
             }

             txtUsernameStd.Text = "";
             txtPasswordStd.Text = "";

             con.Open();
             OdbcDataAdapter daa = new OdbcDataAdapter("Select*from studacct_tbl where studno='" + thekey + "'", con);
             DataTable dtt = new DataTable();
             daa.Fill(dtt);
             con.Close();

             if (dtt.Rows.Count > 0)
             {
                 txtUsernameStd.Text = dtt.Rows[0].ItemArray[1].ToString();
                 txtPasswordStd.Text = dtt.Rows[0].ItemArray[2].ToString();
             }
             else
             {
                 con.Open();
                 string addAcct = "Insert Into studacct_tbl(studno,username,password)values('" +
                     thekey + "','" +txtUsernameStd.Text + "','" + txtPasswordStd.Text + "')";
                 OdbcCommand cmdAddAcct = new OdbcCommand(addAcct, con);
                 cmdAddAcct.ExecuteNonQuery();
                 con.Close();

                 con.Open();
                 OdbcDataAdapter daaa = new OdbcDataAdapter("Select*from studacct_tbl where studno='" + thekey + "'", con);
                 DataTable dttt = new DataTable();
                 daaa.Fill(dttt);
                 con.Close();

                 if (dttt.Rows.Count > 0)
                 {
                     txtUsernameStd.Text = dttt.Rows[0].ItemArray[1].ToString();
                     txtPasswordStd.Text = dttt.Rows[0].ItemArray[2].ToString();
                 }
             }
         }

         public void setupview_forStudent()
         {
             dgvSearch.DataSource = null;

             con.Open();
             OdbcDataAdapter daStudent = new OdbcDataAdapter("Select studno,lname as 'Lastname',fname as 'Firstname',mname as 'Middlename' from stud_tbl order by lname ASC", con);
             DataTable dtStudent = new DataTable();
             daStudent.Fill(dtStudent);
             con.Close();
             dvStudent = new DataView(dtStudent);
             dgvSearch.DataSource = dvStudent;

             dgvSearch.Columns[0].Width = 0;
             dgvSearch.Columns[1].Width = 155;
             dgvSearch.Columns[2].Width = 155;
             dgvSearch.Columns[3].Width = 155;
             dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
             lblResult.Text = "number of students: " + dgvSearch.Rows.Count.ToString();
         }

         public void setupdisableinput_forStudent()
         {
             txtStdFname.Enabled = false;
             txtStdMname.Enabled = false;
             txtStdLname.Enabled = false;
             txtUsernameStd.Enabled = false;
             txtPasswordStd.Enabled = false;
             btnSFirst.Enabled = false;
             btnSMid.Enabled = false;
             btnSLast.Enabled = false;
         }

         public void setupenableinput_forStudent()
         {
             txtStdFname.Enabled = true;
             txtStdMname.Enabled = true;
             txtStdLname.Enabled = true;
             txtUsernameStd.Enabled = true;
             txtPasswordStd.Enabled = true;
             btnSFirst.Enabled = true;
             btnSMid.Enabled = true;
             btnSLast.Enabled = true;
         }

         public void setupclear_forStudent()
         {
             txtStdFname.Clear();
             txtStdMname.Clear();
             txtStdLname.Clear();
             txtUsernameStd.Clear();
             txtPasswordStd.Clear();
       
             lblKey.Text = "";

             setupenableinput_forStudent();
         }

         public void setupaddoperation_forStudent()
         {
             if (isnospace == false)
             {
                 setupStudnum();
             }
             else
             {
                 firstdigit = "9"; seconddigit = "9"; thirddigit = "9"; fourthdigit = "9"; 
             }
             
             //THIS IS WHERE THE INFORMATION WILL STORE TO THE STUD_TBL IN DATABASE
             con.Open(); 
             string addStudent = "Insert Into stud_tbl(studno,fname,mname,lname)values('" +
                 newStudentNumber+"','"+txtStdFname.Text + "','" + txtStdMname.Text + "','" + txtStdLname.Text + "')";

             OdbcCommand cmdAddStudent = new OdbcCommand(addStudent, con);
             cmdAddStudent.ExecuteNonQuery();
             con.Close();

             //THIS IS WHERE THE THE STUDENT WILL CREATE A RECORD TO THE TABLE IN DATABASE BASED ON THEIR LEVEL
            
             btnStdAdd.Enabled = false;
             setupview_forStudent();
             MessageBox.Show("student successfully added", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
             isinserted = true;
             
             setupStudnum();
             txtStdMname.BackColor = Color.White;
             txtUsernameStd.BackColor = Color.White;
             txtPasswordStd.BackColor = Color.White;
             txtSearch.Focus();

         }

         public void setupsaveoperation_forStudent()
         {
           
             con.Open();
             string updateStudent = "Update stud_tbl set fname='" + txtStdFname.Text + "',mname='" + txtStdMname.Text + "',lname='" + txtStdLname.Text + "' where studno='" + primarykey + "'";
             OdbcCommand cmdUpdateStudent = new OdbcCommand(updateStudent, con);
             cmdUpdateStudent.ExecuteNonQuery();
             con.Close();

             con.Open();
             string updateacct = "Update studacct_tbl set username='" + txtUsernameStd.Text + "',password='" + txtPasswordStd.Text + "'where studno='" + primarykey + "'";
             OdbcCommand cmdacct = new OdbcCommand(updateacct,con);
             cmdacct.ExecuteNonQuery();
             con.Close();

             btnStdAdd.Enabled = false;
             setupview_forStudent();
             btnStdClear.Text = "Clear";
             MessageBox.Show("student successfully updated", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
             txtStdMname.BackColor = Color.White;
             txtUsernameStd.BackColor = Color.White;
             txtPasswordStd.BackColor = Color.White;
             txtSearch.Focus();
         }

         public void setupdeleteoperation_forStudent()
         {
             con.Open();
             string deleteStudent = "Delete from stud_tbl where studno='" + lblKey.Text + "'";
             OdbcCommand cmdDeleteStudent = new OdbcCommand(deleteStudent, con);
             cmdDeleteStudent.ExecuteNonQuery();
             con.Close();

             btnStdAdd.Enabled = false;
             setupview_forStudent();
             MessageBox.Show("student successfully deleted", "User maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
             txtStdMname.BackColor = Color.White;
             txtUsernameStd.BackColor = Color.White;
             txtPasswordStd.BackColor = Color.White;
             txtSearch.Focus();
         }

     
         private void txtAdmMidl_TextChanged(object sender, EventArgs e)
         {

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

        
         private void btnSub_Click(object sender, EventArgs e)
         {
             frmSubject subjmaintenance = new frmSubject();
             this.Dispose();
             subjmaintenance.wholog = adminlog;
             subjmaintenance.VISITED = VISITED;
             subjmaintenance.Show();
            
         }

         private void btnRoom_Click(object sender, EventArgs e)
         {
             frmRoom roommaintenance = new frmRoom();
             this.Dispose();
             roommaintenance.logger = adminlog;
             roommaintenance.VISITED = VISITED;
             roommaintenance.Show();

         
         }

         private void btnAbout_Click(object sender, EventArgs e)
         {
             frmAboutMaintenance am = new frmAboutMaintenance();
             this.Dispose();
             am.amlog = adminlog;
             am.Show();
            
         }

         private void btnAudit_Click(object sender, EventArgs e)
         {
             frmAudit auditform = new frmAudit();
             this.Dispose();
             auditform.auditlogger = adminlog;
             auditform.Show();
         }

         private void btnReq_Click(object sender, EventArgs e)
         { 
             frmRequirement reqform = new frmRequirement();
             this.Dispose();
             reqform.reqlog = adminlog;
             reqform.VISITED = VISITED;
             reqform.Show();
         }

         private void btnAct_Click(object sender, EventArgs e)
         {
             frmActivity actform = new frmActivity();
             this.Dispose();
             actform.actlog = adminlog;
             actform.Show();
         }

         private void button6_Click(object sender, EventArgs e)
         {
             frmFee feeform = new frmFee();
             this.Dispose();
             feeform.feelog = adminlog;
             feeform.VISITED = VISITED;
             feeform.Show();
         }

         private void btnDisc_Click(object sender, EventArgs e)
         {
             frmDiscount discform = new frmDiscount();
             this.Dispose();
             discform.disclog = adminlog;
             discform.VISITED = VISITED;
             discform.Show();
         }

         private void btnSched_Click(object sender, EventArgs e)
         {
             frmSched sf = new frmSched();
             this.Dispose();
             sf.schedlog = adminlog;
             sf.VISITED = VISITED;
             sf.Show();
         }

         private void btnBackup_Click(object sender, EventArgs e)
         {
             frmBackup buf = new frmBackup();
             this.Dispose();
             buf.backlog = adminlog;
             buf.Show();
         }

         private void frmMaintenance_FormClosing(object sender, FormClosingEventArgs e)
         {
             LOGOUT();
             frmEmpLogin home = new frmEmpLogin();
             this.Dispose();
             home.Show();
         }

         private void button1_Click(object sender, EventArgs e)
         {
             txtStdFname.Text = "a";
             txtStdLname.Text = "b";
            

         }

         private void label63_Click(object sender, EventArgs e)
         {

         }

         private void btnSettings_Click(object sender, EventArgs e)
         {
         }

         private void cmbStudFilter_SelectedIndexChanged(object sender, EventArgs e)
         {
             setupChoicesFilterStudent(cmbStudFilter.Text);
         }

         public void setupChoicesFilterStudent(string filtertype)
         {
             
             if (filtertype == "Level")
             {
                 cmbStdFilterChoice.Enabled = true;
                 cmbStdFilterChoice.Items.Clear();
                 cmbStdFilterChoice.Items.Add("Kinder");
                 cmbStdFilterChoice.Items.Add("Grade 1");
                 cmbStdFilterChoice.Items.Add("Grade 2");
                 cmbStdFilterChoice.Items.Add("Grade 3");
                 cmbStdFilterChoice.Items.Add("Grade 4");
                 cmbStdFilterChoice.Items.Add("Grade 5");
                 cmbStdFilterChoice.Items.Add("Grade 6");
                 cmbStdFilterChoice.Items.Add("Grade 7");
                 cmbStdFilterChoice.Items.Add("Grade 8");
                 cmbStdFilterChoice.Items.Add("Grade 9");
                 cmbStdFilterChoice.Items.Add("Grade 10");
             }
             if (filtertype == "Section")
             {
                 con.Open();
                 OdbcDataAdapter da = new OdbcDataAdapter("Select section from section_tbl", con);
                 DataTable dt = new DataTable();
                 da.Fill(dt);
                 con.Close();
                 if (dt.Rows.Count > 0)
                 {
                     cmbStdFilterChoice.Enabled = true;
                     cmbStdFilterChoice.Items.Clear();

                     for (int i = 0; i < dt.Rows.Count; i++)
                     {
                         cmbStdFilterChoice.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                     }
                 }
             }
             if (filtertype == "Gender")
             {
                 cmbStdFilterChoice.Enabled = true;
                 cmbStdFilterChoice.Items.Clear();
                 cmbStdFilterChoice.Items.Add("Male");
                 cmbStdFilterChoice.Items.Add("Female");
             }
             if (filtertype == "All students")
             {
                 cmbStdFilterChoice.Enabled = false;
                 cmbStdFilterChoice.Items.Clear();
                 setupview_forStudent();
             }
         }

         private void cmbStdFilterChoice_SelectedIndexChanged(object sender, EventArgs e)
         {
             if (cmbStudFilter.Text == "Level")
             {
                 con.Open();
                 OdbcDataAdapter daStudent = new OdbcDataAdapter("Select studno,lname as 'Lastname',fname as 'Firstname',mname as 'Middlename'from stud_tbl where level='"+cmbStdFilterChoice.Text+"'", con);
                 DataTable dtStudent = new DataTable();
                 daStudent.Fill(dtStudent);
                 dvStudent = new DataView(dtStudent);
                 dgvSearch.DataSource = dvStudent;
                 con.Close();
                 
                 lblResult.Text = "number of students from "+cmbStdFilterChoice.Text+" : " + dgvSearch.Rows.Count.ToString();
             }
             else if (cmbStudFilter.Text == "Gender")
             {
                 con.Open();
                 OdbcDataAdapter daStudent = new OdbcDataAdapter("Select studno,lname as 'Lastname',fname as 'Firstname',mname as 'Middlename'from stud_tbl where gender='" + cmbStdFilterChoice.Text + "'", con);
                 DataTable dtStudent = new DataTable();
                 daStudent.Fill(dtStudent);
                 dvStudent = new DataView(dtStudent);
                 dgvSearch.DataSource = dvStudent;
                 con.Close();
                 
                 lblResult.Text = "number of "+cmbStdFilterChoice.Text+" students: "+ dgvSearch.Rows.Count.ToString();
             }
             else
             {
                 con.Open();
                 OdbcDataAdapter daStudent = new OdbcDataAdapter("Select studno,lname as 'Lastname',fname as 'Firstname',mname as 'Middlename'from stud_tbl where section='" + cmbStdFilterChoice.Text + "'", con);
                 DataTable dtStudent = new DataTable();
                 daStudent.Fill(dtStudent);
                 dvStudent = new DataView(dtStudent);
                 dgvSearch.DataSource = dvStudent;
                 con.Close();
                
                 lblResult.Text = "number of students from section "+cmbStdFilterChoice.Text+" : " + dgvSearch.Rows.Count.ToString();
             }
            
         }

         private void cmbFacYear_SelectedIndexChanged(object sender, EventArgs e)
         {
             computeAge();
         }

         public void computeAge()
         {
             
         }

       

        
         private void cmbFacDay_SelectedIndexChanged(object sender, EventArgs e)
         {
             computeAge();
         }

         private void cmbStdMonth_SelectedIndexChanged(object sender, EventArgs e)
         {
             
         }

         private void cmbStdDay_SelectedIndexChanged(object sender, EventArgs e)
         {
            
         }

         private void cmbStdYear_SelectedIndexChanged(object sender, EventArgs e)
         {
            
         }

         private void cmbEmpMonth_SelectedIndexChanged(object sender, EventArgs e)
         {
             
         }

         private void cmbEmpDay_SelectedIndexChanged(object sender, EventArgs e)
         {
             
         }

         private void cmbEmpYear_SelectedIndexChanged(object sender, EventArgs e)
         {
             
         }

         private void btnUser_Click(object sender, EventArgs e)
         {
             return;
         }

         private void btnSY_Click(object sender, EventArgs e)
         {
             
             frmSchoolYear symaintenance = new frmSchoolYear();
             this.Dispose();
             symaintenance.sylog = adminlog;
             symaintenance.VISITED = VISITED;
             symaintenance.Show();
             
         }

         private void btnLevel_Click(object sender, EventArgs e)
         {
             frmLevel levmain = new frmLevel();
             this.Dispose();
             levmain.levlog = adminlog;
             levmain.VISITED = VISITED;
             levmain.Show();
         }

         private void btnFaculty_Click(object sender, EventArgs e)
         {
             frmFaculty facmain = new frmFaculty();
             this.Dispose();
             facmain.facmlog = adminlog;
             facmain.VISITED = VISITED;
             facmain.Show();
          
         }

         private void btnAdmMain_Click(object sender, EventArgs e)
         {
             frmHomeMaintenance hm = new frmHomeMaintenance();
             this.Dispose();
             hm.adminlog = adminlog;
             hm.VISITED = VISITED;
             hm.Show();
         }

         private void dgvSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
         {

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

         private void btnSection_Click(object sender, EventArgs e)
         {
             frmSection section = new frmSection();
             this.Dispose();
             section.secwholog =adminlog;
             section.VISITED = VISITED;
             section.Show();
         }

         private void btnStaff_Click(object sender, EventArgs e)
         {
             frmStaff stfform = new frmStaff();
             this.Dispose();
             stfform.stflog = adminlog;
             stfform.VISITED = VISITED;
             stfform.Show();
         }

         private void btnStud_Click(object sender, EventArgs e)
         {
             frmStudent stdform = new frmStudent();
             this.Dispose();
             stdform.stdlog = adminlog;
             stdform.VISITED = VISITED;
             stdform.Show();
         }

         private void btnN1_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtAdmFname.Text != "")
             {
                 if (txtAdmFname.TextLength == 1)
                 {
                     string last = txtAdmFname.Text.Substring(0, txtAdmFname.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtAdmFname.Text.Substring(0, txtAdmFname.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtAdmFname.Text;
                     }

                 }
                 else
                 {
                     string last = txtAdmFname.Text.Substring(txtAdmFname.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtAdmFname.Text.Substring(0, txtAdmFname.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtAdmFname.Text.Substring(0, txtAdmFname.TextLength);
                     }
                 }
             }

             if (fnmEnye == 1)
             {
                 if (txtAdmFname.Text != "")
                 {
                     txtAdmFname.Text = orgtext + "Ñ";
                     fnmEnye += 1;
                 }
                 else
                 {
                     txtAdmFname.Text = "Ñ";
                     fnmEnye += 1;
                 }
             }
             else
             {
                 if (txtAdmFname.Text != "")
                 {
                     txtAdmFname.Text = orgtext + "ñ";
                     fnmEnye -= 1;
                 }
                 else
                 {
                     txtAdmFname.Text = "ñ";
                     fnmEnye -= 1;
                 }
             }

             txtAdmFname.Focus();
             txtAdmFname.SelectionStart = txtAdmFname.Text.Length;
         }

         private void btnN2_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtAdmMidl.Text != "")
             {
                 if (txtAdmMidl.TextLength == 1)
                 {
                     string last = txtAdmMidl.Text.Substring(0, txtAdmMidl.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtAdmMidl.Text.Substring(0, txtAdmMidl.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtAdmMidl.Text;
                     }

                 }
                 else
                 {
                     string last = txtAdmMidl.Text.Substring(txtAdmMidl.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtAdmMidl.Text.Substring(0, txtAdmMidl.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtAdmMidl.Text.Substring(0, txtAdmMidl.TextLength);
                     }
                 }
             }

             if (mnmEnye == 1)
             {
                 if (txtAdmMidl.Text != "")
                 {
                     txtAdmMidl.Text = orgtext + "Ñ";
                     mnmEnye += 1;
                 }
                 else
                 {
                     txtAdmMidl.Text = "Ñ";
                     mnmEnye += 1;
                 }
             }
             else
             {
                 if (txtAdmMidl.Text != "")
                 {
                     txtAdmMidl.Text = orgtext + "ñ";
                     mnmEnye -= 1;
                 }
                 else
                 {
                     txtAdmMidl.Text = "ñ";
                     mnmEnye -= 1;
                 }
             }

             txtAdmMidl.Focus();
             txtAdmMidl.SelectionStart = txtAdmMidl.Text.Length;
         }

         private void btnN3_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtAdmLast.Text != "")
             {
                 if (txtAdmLast.TextLength == 1)
                 {
                     string last = txtAdmLast.Text.Substring(0, txtAdmLast.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtAdmLast.Text.Substring(0, txtAdmLast.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtAdmLast.Text;
                     }

                 }
                 else
                 {
                     string last = txtAdmLast.Text.Substring(txtAdmLast.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtAdmLast.Text.Substring(0, txtAdmLast.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtAdmLast.Text.Substring(0, txtAdmLast.TextLength);
                     }
                 }
             }

             if (lstEnye == 1)
             {
                 if (txtAdmLast.Text != "")
                 {
                     txtAdmLast.Text = orgtext + "Ñ";
                     lstEnye += 1;
                 }
                 else
                 {
                     txtAdmLast.Text = "Ñ";
                     lstEnye += 1;
                 }
             }
             else
             {
                 if (txtAdmLast.Text != "")
                 {
                     txtAdmLast.Text = orgtext + "ñ";
                     lstEnye -= 1;
                 }
                 else
                 {
                     txtAdmLast.Text = "ñ";
                     lstEnye -= 1;
                 }
             }

             txtAdmLast.Focus();
             txtAdmLast.SelectionStart = txtAdmLast.Text.Length;
         }

         private void btnEFirst_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtEmpFname.Text != "")
             {
                 if (txtEmpFname.TextLength == 1)
                 {
                     string last = txtEmpFname.Text.Substring(0, txtEmpFname.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtEmpFname.Text.Substring(0, txtEmpFname.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtEmpFname.Text;
                     }

                 }
                 else
                 {
                     string last = txtEmpFname.Text.Substring(txtEmpFname.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtEmpFname.Text.Substring(0, txtEmpFname.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtEmpFname.Text.Substring(0, txtEmpFname.TextLength);
                     }
                 }
             }

             if (fnmEnye == 1)
             {
                 if (txtEmpFname.Text != "")
                 {
                     txtEmpFname.Text = orgtext + "Ñ";
                     fnmEnye += 1;
                 }
                 else
                 {
                     txtEmpFname.Text = "Ñ";
                     fnmEnye += 1;
                 }
             }
             else
             {
                 if (txtEmpFname.Text != "")
                 {
                     txtEmpFname.Text = orgtext + "ñ";
                     fnmEnye -= 1;
                 }
                 else
                 {
                     txtEmpFname.Text = "ñ";
                     fnmEnye -= 1;
                 }
             }

             txtEmpFname.Focus();
             txtEmpFname.SelectionStart = txtEmpFname.Text.Length;
         }

         private void btnEMidl_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtEmpMidl.Text != "")
             {
                 if (txtEmpMidl.TextLength == 1)
                 {
                     string last = txtEmpMidl.Text.Substring(0, txtEmpMidl.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtEmpMidl.Text.Substring(0, txtEmpMidl.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtEmpMidl.Text;
                     }

                 }
                 else
                 {
                     string last = txtEmpMidl.Text.Substring(txtEmpMidl.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtEmpMidl.Text.Substring(0, txtEmpMidl.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtEmpMidl.Text.Substring(0, txtEmpMidl.TextLength);
                     }
                 }
             }

             if (mnmEnye == 1)
             {
                 if (txtEmpMidl.Text != "")
                 {
                     txtEmpMidl.Text = orgtext + "Ñ";
                     mnmEnye += 1;
                 }
                 else
                 {
                     txtEmpMidl.Text = "Ñ";
                     mnmEnye += 1;
                 }
             }
             else
             {
                 if (txtEmpMidl.Text != "")
                 {
                     txtEmpMidl.Text = orgtext + "ñ";
                     mnmEnye -= 1;
                 }
                 else
                 {
                     txtEmpMidl.Text = "ñ";
                     mnmEnye -= 1;
                 }
             }

             txtEmpMidl.Focus();
             txtEmpMidl.SelectionStart = txtEmpMidl.Text.Length;
         }

         private void btnELast_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtEmpLast.Text != "")
             {
                 if (txtEmpLast.TextLength == 1)
                 {
                     string last = txtEmpLast.Text.Substring(0, txtEmpLast.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtEmpLast.Text.Substring(0, txtEmpLast.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtEmpLast.Text;
                     }

                 }
                 else
                 {
                     string last = txtEmpLast.Text.Substring(txtEmpLast.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtEmpLast.Text.Substring(0, txtEmpLast.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtEmpLast.Text.Substring(0, txtEmpLast.TextLength);
                     }
                 }
             }

             if (lstEnye == 1)
             {
                 if (txtEmpLast.Text != "")
                 {
                     txtEmpLast.Text = orgtext + "Ñ";
                     lstEnye += 1;
                 }
                 else
                 {
                     txtEmpLast.Text = "Ñ";
                     lstEnye += 1;
                 }
             }
             else
             {
                 if (txtEmpLast.Text != "")
                 {
                     txtEmpLast.Text = orgtext + "ñ";
                     lstEnye -= 1;
                 }
                 else
                 {
                     txtEmpLast.Text = "ñ";
                     lstEnye -= 1;
                 }
             }

             txtEmpLast.Focus();
             txtEmpLast.SelectionStart = txtEmpLast.Text.Length;
         }

         private void btnFFirst_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtFacFname.Text != "")
             {
                 if (txtFacFname.TextLength == 1)
                 {
                     string last = txtFacFname.Text.Substring(0, txtFacFname.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtFacFname.Text.Substring(0, txtFacFname.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtFacFname.Text;
                     }

                 }
                 else
                 {
                     string last = txtFacFname.Text.Substring(txtFacFname.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtFacFname.Text.Substring(0, txtFacFname.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtFacFname.Text.Substring(0, txtFacFname.TextLength);
                     }
                 }
             }

             if (fnmEnye == 1)
             {
                 if (txtFacFname.Text != "")
                 {
                     txtFacFname.Text = orgtext + "Ñ";
                     fnmEnye += 1;
                 }
                 else
                 {
                     txtFacFname.Text = "Ñ";
                     fnmEnye += 1;
                 }
             }
             else
             {
                 if (txtFacFname.Text != "")
                 {
                     txtFacFname.Text = orgtext + "ñ";
                     fnmEnye -= 1;
                 }
                 else
                 {
                     txtFacFname.Text = "ñ";
                     fnmEnye -= 1;
                 }
             }

             txtFacFname.Focus();
             txtFacFname.SelectionStart = txtFacFname.Text.Length;
         }

         private void btnFMid_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtFacMidl.Text != "")
             {
                 if (txtFacMidl.TextLength == 1)
                 {
                     string last = txtFacMidl.Text.Substring(0, txtFacMidl.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtFacMidl.Text.Substring(0, txtFacMidl.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtFacMidl.Text;
                     }

                 }
                 else
                 {
                     string last = txtFacMidl.Text.Substring(txtFacMidl.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtFacMidl.Text.Substring(0, txtFacMidl.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtFacMidl.Text.Substring(0, txtFacMidl.TextLength);
                     }
                 }
             }

             if (mnmEnye == 1)
             {
                 if (txtFacMidl.Text != "")
                 {
                     txtFacMidl.Text = orgtext + "Ñ";
                     mnmEnye += 1;
                 }
                 else
                 {
                     txtFacMidl.Text = "Ñ";
                     mnmEnye += 1;
                 }
             }
             else
             {
                 if (txtFacMidl.Text != "")
                 {
                     txtFacMidl.Text = orgtext + "ñ";
                     mnmEnye -= 1;
                 }
                 else
                 {
                     txtFacMidl.Text = "ñ";
                     mnmEnye -= 1;
                 }
             }

             txtFacMidl.Focus();
             txtFacMidl.SelectionStart = txtFacMidl.Text.Length;
         }

         private void btnFLast_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtFacLast.Text != "")
             {
                 if (txtFacLast.TextLength == 1)
                 {
                     string last = txtFacLast.Text.Substring(0, txtFacLast.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtFacLast.Text.Substring(0, txtFacLast.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtFacLast.Text;
                     }

                 }
                 else
                 {
                     string last = txtFacLast.Text.Substring(txtFacLast.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtFacLast.Text.Substring(0, txtFacLast.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtFacLast.Text.Substring(0, txtFacLast.TextLength);
                     }
                 }
             }

             if (lstEnye == 1)
             {
                 if (txtFacLast.Text != "")
                 {
                     txtFacLast.Text = orgtext + "Ñ";
                     lstEnye += 1;
                 }
                 else
                 {
                     txtFacLast.Text = "Ñ";
                     lstEnye += 1;
                 }
             }
             else
             {
                 if (txtFacLast.Text != "")
                 {
                     txtFacLast.Text = orgtext + "ñ";
                     lstEnye -= 1;
                 }
                 else
                 {
                     txtFacLast.Text = "ñ";
                     lstEnye -= 1;
                 }
             }

             txtFacLast.Focus();
             txtFacLast.SelectionStart = txtFacLast.Text.Length;
         }

         private void btnSFirst_Click(object sender, EventArgs e)
         {
             string orgtext = "";
             if (txtStdFname.Text != "")
             {
                 if (txtStdFname.TextLength == 1)
                 {
                     string last = txtStdFname.Text.Substring(0, txtStdFname.TextLength);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtStdFname.Text.Substring(0, txtStdFname.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtStdFname.Text;
                     }

                 }
                 else
                 {
                     string last = txtStdFname.Text.Substring(txtStdFname.TextLength - 1, 1);

                     if (last == "Ñ" || last == "ñ")
                     {
                         orgtext = txtStdFname.Text.Substring(0, txtStdFname.TextLength - 1);
                     }
                     else
                     {
                         orgtext = txtStdFname.Text.Substring(0, txtStdFname.TextLength);
                     }
                 }
             }

             if (fnmEnye == 1)
             {
                 if (txtStdFname.Text != "")
                 {
                     txtStdFname.Text = orgtext + "Ñ";
                     fnmEnye += 1;
                 }
                 else
                 {
                     txtStdFname.Text = "Ñ";
                     fnmEnye += 1;
                 }
             }
             else
             {
                 if (txtStdFname.Text != "")
                 {
                     txtStdFname.Text = orgtext + "ñ";
                     fnmEnye -= 1;
                 }
                 else
                 {
                     txtStdFname.Text = "ñ";
                     fnmEnye -= 1;
                 }
             }

             txtStdFname.Focus();
             txtStdFname.SelectionStart = txtStdFname.Text.Length;
         }

         private void btnSMid_Click(object sender, EventArgs e)
         {
             string orgtext = "";
            if (txtStdMname.Text != "")
            {
                if (txtStdMname.TextLength == 1)
                {
                    string last =txtStdMname.Text.Substring(0, txtStdMname.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtStdMname.Text.Substring(0, txtStdMname.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtStdMname.Text;
                    }

                }
                else
                {
                    string last = txtStdMname.Text.Substring(txtStdMname.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtStdMname.Text.Substring(0, txtStdMname.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtStdMname.Text.Substring(0, txtStdMname.TextLength);
                    }
                }
            }

            if (mnmEnye == 1)
            {
                if (txtStdMname.Text != "")
                {
                    txtStdMname.Text = orgtext + "Ñ";
                    mnmEnye += 1;
                }
                else
                {
                    txtStdMname.Text = "Ñ";
                    mnmEnye += 1;
                }
            }
            else
            {
                if (txtStdMname.Text != "")
                {
                    txtStdMname.Text = orgtext + "ñ";
                    mnmEnye -= 1;
                }
                else
                {
                    txtStdMname.Text = "ñ";
                    mnmEnye -= 1;
                }
            }

            txtStdMname.Focus();
            txtStdMname.SelectionStart = txtStdMname.Text.Length;
        }

        private void btnSLast_Click(object sender, EventArgs e)
        {
            string orgtext = "";
            if (txtStdLname.Text != "")
            {
                if (txtStdLname.TextLength == 1)
                {
                    string last = txtStdLname.Text.Substring(0, txtStdLname.TextLength);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtStdLname.Text.Substring(0, txtStdLname.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtStdLname.Text;
                    }

                }
                else
                {
                    string last = txtStdLname.Text.Substring(txtStdLname.TextLength - 1, 1);

                    if (last == "Ñ" || last == "ñ")
                    {
                        orgtext = txtStdLname.Text.Substring(0, txtStdLname.TextLength - 1);
                    }
                    else
                    {
                        orgtext = txtStdLname.Text.Substring(0, txtStdLname.TextLength);
                    }
                }
            }

            if (lstEnye == 1)
            {
                if (txtStdLname.Text != "")
                {
                    txtStdLname.Text = orgtext + "Ñ";
                    lstEnye += 1;
                }
                else
                {
                    txtStdLname.Text = "Ñ";
                    lstEnye += 1;
                }
            }
            else
            {
                if (txtStdLname.Text != "")
                {
                    txtStdLname.Text = orgtext + "ñ";
                    lstEnye -= 1;
                }
                else
                {
                    txtStdLname.Text = "ñ";
                    lstEnye -= 1;
                }
            }

            txtStdLname.Focus();
            txtStdLname.SelectionStart = txtStdLname.Text.Length;
        }

        private void pnlUserStud_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = adminlog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }
         

       
         
        
        

        /******************************************** END METHODS FOR USER MAINTENANCE > STUDENT **********************************************
        ***************************************************************************************************************************************/

    }
}
