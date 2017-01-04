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
    public partial class frmFee : Form
    {
        OdbcConnection con = new OdbcConnection("DRIVER={MySQL ODBC 3.51 DRIVER};USER=root;SERVER=localhost;PWD=leebert;DB=berlyn");
        public string feelog,primarykey,selectedfee,VISITED,activeSY;
        public DataView dvFee;

        public frmFee()
        {
            InitializeComponent();
        }

        private void frmFee_Load(object sender, EventArgs e)
        {
            //pnlType.BackColor = Color.FromArgb(0, 0, 25);

            //this.BackColor = Color.FromArgb(49, 79, 142);
            lblLogger.Text = feelog;
            lblLoggerPosition.Text = "Admin";
            //btnHome.Text = "          " + feelog;
            btnFee.BackColor = Color.LightGreen;
           
            pnlnotify.Visible = false;
            setupLevelList();
            setupDept();
            GetActiveSchoolYear();
            
            if (VISITED.Contains("Student fee") == false)
            {
                VISITED += "   Student fee";
            }
        }

      
        public void setupDept()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select deptname from department_tbl", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                cmbDept.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbDept.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                }
                cmbDept.Items.Add("All Department");
            }
        }

        public void GetActiveSchoolYear()
        {
            con.Open();
            OdbcDataAdapter dasy = new OdbcDataAdapter("Select*from schoolyear_tbl where status='" + "Active" + "'", con);
            DataTable dtssy = new DataTable();
            dasy.Fill(dtssy);
            con.Close();
            if (dtssy.Rows.Count > 0)
            { activeSY = dtssy.Rows[0].ItemArray[1].ToString(); }
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
                cmbLevel.Items.Add("All levels");
            }
        }

        public void updateannual_thruregi(string level, double reg)
        {
            if (txtAmt.Text != "")
            {
                double amt = Convert.ToDouble(txtAmt.Text);
                //double orgamount = Convert.ToDouble(selectedfee);
                double newreg = 0;
                double tf_amt = 0;
                double misc_amt = 0;
                double anlm = 0;

                con.Open();
                OdbcDataAdapter datf = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Tuition fee' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dttf = new DataTable();
                datf.Fill(dttf);
                con.Close();

                con.Open();
                OdbcDataAdapter dami = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dtmi = new DataTable();
                dami.Fill(dtmi);
                con.Close();

                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dttf.Rows.Count > 0)
                {
                    tf_amt = Convert.ToDouble(dttf.Rows[0].ItemArray[2].ToString());
                }

                if (dtmi.Rows.Count > 0)
                {
                    misc_amt = Convert.ToDouble(dtmi.Rows[0].ItemArray[2].ToString());
                }

                if (dt.Rows.Count > 0)
                {
                    newreg = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                    
                }


                anlm = tf_amt + misc_amt + newreg;
                string amount = "";
                if (anlm >= 1000)//b4 amt
                {
                    amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(anlm));
                }
                if (anlm < 1000)
                {
                    amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(anlm));
                }

                con.Open();
                string upd = "Update fee_tbl set amount='" + amount + "'where fee LIKE 'Annual payment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                OdbcCommand cmdupd = new OdbcCommand(upd, con);
                cmdupd.ExecuteNonQuery();
                con.Close();
            }

        }

        public void updateannual_thrumisc(string level, double mis)
        {
            if (txtAmt.Text != "")
            {
                double amt = Convert.ToDouble(txtAmt.Text);
                double orgamount = Convert.ToDouble(selectedfee);
                double misc = 0;
                double tf_amt = 0;
                double reg_amt = 0;
                double anlm = 0;

                con.Open();
                OdbcDataAdapter datf = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Tuition fee' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dttf = new DataTable();
                datf.Fill(dttf);
                con.Close();

                con.Open();
                OdbcDataAdapter darf = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dtrf = new DataTable();
                darf.Fill(dtrf);
                con.Close();

                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dttf.Rows.Count > 0)
                {
                    tf_amt = Convert.ToDouble(dttf.Rows[0].ItemArray[2].ToString());
                }

                if (dtrf.Rows.Count > 0)
                {
                    reg_amt = Convert.ToDouble(dtrf.Rows[0].ItemArray[2].ToString());
                }

                if (dt.Rows.Count > 0)
                {
                    misc = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                   
                }

                anlm= reg_amt + tf_amt + misc;
                string amount = "";
                if (anlm >= 1000)
                {
                    amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(anlm));
                }
                if (anlm < 1000)
                {
                    amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(anlm));
                }

                con.Open();
                string upd = "Update fee_tbl set amount='" + amount + "'where fee LIKE 'Annual payment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                OdbcCommand cmdupd = new OdbcCommand(upd, con);
                cmdupd.ExecuteNonQuery();
                con.Close();
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Clear")
            {
                txtFee.Clear();
                txtAmt.Clear();
                txtFee.Enabled = true;
                txtAmt.Enabled = true;
                lblKey.Text = "";
                

                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                if (cmbType.Text != "Assessment")
                {
                    btnAdd.Enabled = true;
                }
                else
                {
                    btnAdd.Enabled = false;
                }
                btnUpdate.Text = "Update";
            }
            else
            {
                if (cmbType.Text != "Assessment")
                {
                    btnDelete.Enabled = true;
                }
                
                btnClear.Text = "Clear";
                btnUpdate.Text = "Update";

                //primarykey = lblKey.Text;
                setup_retrieve(primarykey);


                txtFee.Enabled = false;
                txtAmt.Enabled = false;
            }


            if (dgvSearch.Rows.Count >= 1)
            {
                dgvSearch.Rows[0].Selected = true;
            }

            txtSearch.Focus();
        }

        public void setup_retrieve(string thekey)
        {
            if (cmbType.Text == "Assessment")
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where id='" + thekey + "'and SY='" + activeSY + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                    txtFee.Text = dt.Rows[0].ItemArray[1].ToString();
                    txtAmt.Text = dt.Rows[0].ItemArray[2].ToString();

                    btnAdd.Enabled = false;
                }
            }


            if (cmbType.Text == "Registration breakdown")
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from registrationfee_tbl where id='" + thekey + "'and SY='" + activeSY + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                    txtFee.Text = dt.Rows[0].ItemArray[1].ToString();
                    txtAmt.Text = dt.Rows[0].ItemArray[2].ToString();

                    btnAdd.Enabled = false;
                }
            }



            if (cmbType.Text == "Miscellaneous breakdown")
            {
                con.Open();
                OdbcDataAdapter da = new OdbcDataAdapter("Select*from miscellaneousfee_tbl where id='" + thekey + "'and SY='" + activeSY + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    lblKey.Text = dt.Rows[0].ItemArray[0].ToString();
                    txtFee.Text = dt.Rows[0].ItemArray[1].ToString();
                    txtAmt.Text = dt.Rows[0].ItemArray[2].ToString();

                    btnAdd.Enabled = false;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete?", "Fee maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtFee.Text == "" || txtAmt.Text == "" || cmbType.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Fee maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    setup_delete();
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;

                    txtFee.Clear();
                    txtAmt.Clear();
                   
                    txtFee.Enabled = true;
                    txtAmt.Enabled = true;

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
            double checkdigit = Convert.ToDouble(txtAmt.Text);
            string lev = cmbLevel.Text;
            string level = cmbDept.Text;
            /*if (lev == "Kinder")
            {
                level = "Pre-elementary";
            }
            else if (lev == "Grade 1" || lev == "Grade 2" || lev == "Grade 3" || lev == "Grade 4" || lev == "Grade 5" || lev == "Grade 6")
            {
                level = "Grade 1-6";
            }
            else if (lev == "Grade 7" || lev == "Grade 8" || lev == "Grade 9" || lev == "Grade 10")
            {
                level = "Grade 7-10";
            }
            else
            {
                level = cmbLevel.Text;
            }*/

            if (cmbType.Text == "Assessment")
            {
                con.Open();
                string deleteFee = "Delete from fee_tbl where id='" + primarykey + "'and SY='" + activeSY + "'";
                OdbcCommand cmdDeleteFee = new OdbcCommand(deleteFee, con);
                cmdDeleteFee.ExecuteNonQuery();
                con.Close();

                if (txtAmt.Text != "")
                {
                    double amt = Convert.ToDouble(txtAmt.Text);
                    double anl = 0;
                    double regi = 0;
                    double misc = 0;
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Annual payment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);

                    OdbcDataAdapter da2 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    con.Close();

                    if (dt1.Rows.Count > 0)
                    {
                        regi = Convert.ToDouble(dt1.Rows[0].ItemArray[2].ToString());
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        misc = Convert.ToDouble(dt2.Rows[0].ItemArray[2].ToString());
                    }

                    if (dt.Rows.Count > 0)
                    {
                        anl = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                        double newamt = anl -amt;
                        double regAndMiscSum = regi+ misc;
                        double getMonthlyAmt = newamt - regAndMiscSum;
                        double computedMontlyAmt = getMonthlyAmt / 10;
                        double newUponEnr_Amt = regAndMiscSum + computedMontlyAmt;
                        double ttnfee = getMonthlyAmt;

                        string amount = "";
                        if (newamt >= 1000)
                        {
                            amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(newamt));
                        }
                        if (newamt < 1000)
                        {
                            amount = String.Format(("{0:0.00#}"), Convert.ToDouble(newamt));
                        }

                        string monthly = "";
                        if (computedMontlyAmt >= 1000)
                        {
                            monthly = String.Format(("{0:0,###.00#}"), computedMontlyAmt);
                        }
                        if (computedMontlyAmt < 1000)
                        {
                            monthly = String.Format(("{0:0.00#}"), computedMontlyAmt);
                        }

                        string upon = "";
                        if (newUponEnr_Amt >= 1000)
                        {
                            upon = String.Format(("{0:0,###.00#}"), newUponEnr_Amt);
                        } if (newUponEnr_Amt < 1000)
                        {
                            upon = String.Format(("{0:0.00#}"), newUponEnr_Amt);
                        }

                        string ttn = "";
                        if (ttnfee >= 1000)
                        {
                            ttn = String.Format(("{0:0,###.00#}"), ttnfee);
                        } if (ttnfee < 1000)
                        {
                            ttn = String.Format(("{0:0.00#}"), ttnfee);
                        }

                        con.Open();
                        string upd = "Update fee_tbl set amount='" + amount + "'where fee LIKE 'Annual payment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd = new OdbcCommand(upd, con);
                        cmdupd.ExecuteNonQuery();

                        string upd1 = "Update fee_tbl set amount='" + monthly + "'where fee LIKE 'Monthly installment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd1 = new OdbcCommand(upd1, con);
                        cmdupd1.ExecuteNonQuery();

                        string upd2 = "Update fee_tbl set amount='" + upon + "'where fee LIKE 'Upon enrollment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd2 = new OdbcCommand(upd2, con);
                        cmdupd2.ExecuteNonQuery();

                        string upd3 = "Update fee_tbl set amount='" + ttn + "'where fee LIKE 'Tuition fee' and type='fee' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd3 = new OdbcCommand(upd3, con);
                        cmdupd3.ExecuteNonQuery();
                        con.Close();
                    }
                }

                setupview_fee(cmbDept.Text);
            }
            if (cmbType.Text == "Registration breakdown")
            {
                con.Open();
                string deleteFee = "Delete from registrationfee_tbl where id='" + primarykey + "'and SY='" + activeSY + "'";
                OdbcCommand cmdDeleteFee = new OdbcCommand(deleteFee, con);
                cmdDeleteFee.ExecuteNonQuery();
                con.Close();

                if (txtAmt.Text != "")
                {
                    double amt = Convert.ToDouble(txtAmt.Text);
                    double reg = 0;
                    double orgamount= 0;
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        reg = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                        orgamount = reg;
                        reg -= amt;
                        string amount = "";
                        if (reg >= 1000)
                        {
                            amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(reg));
                        }
                        if (reg < 1000)
                        {
                            amount = String.Format(("{0:0.00#}"), Convert.ToDouble(reg));
                        }
                 
                        con.Open();
                        string upd = "Update fee_tbl set amount='" + amount + "'where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd = new OdbcCommand(upd, con);
                        cmdupd.ExecuteNonQuery();
                        con.Close();
                        updateannual_thruregi(level,orgamount);
                        updateamount_REGIMISC(level);
                    }
                }
                setupview_regfee(cmbDept.Text);
            }
            if (cmbType.Text == "Miscellaneous breakdown")
            {
                con.Open();
                string deleteFee = "Delete from miscellaneousfee_tbl where id='" + primarykey + "'and SY='" + activeSY + "'";
                OdbcCommand cmdDeleteFee = new OdbcCommand(deleteFee, con);
                cmdDeleteFee.ExecuteNonQuery();
                con.Close();

                if (txtAmt.Text != "")
                {
                    double amt = Convert.ToDouble(txtAmt.Text);
                    double mis = 0;
                    double orgamount = 0;
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        mis = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                        orgamount = mis;
                        mis -= amt;

                        string amount = "";
                        if (mis>= 1000)
                        {
                            amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(mis));
                        }
                        if (mis < 1000)
                        {
                            amount = String.Format(("{0:0.00#}"), Convert.ToDouble(mis));
                        }
                  
                        con.Open();
                        string upd = "Update fee_tbl set amount='" + amount + "'where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd = new OdbcCommand(upd, con);
                        cmdupd.ExecuteNonQuery();
                        con.Close();
                        updateannual_thrumisc(level,orgamount);
                        updateamount_REGIMISC(level);
                    }
                }

                setupview_miscfee(cmbDept.Text);
            }

            btnAdd.Enabled = false;
            lblKey.Text = "";
            
            MessageBox.Show("fee successfully deleted", "Fee maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Update")
            {
                txtFee.Enabled = true;
                txtAmt.Enabled = true;

                btnUpdate.Text = "Save";
                btnDelete.Enabled = false;
                btnClear.Text = "Cancel";
            }
            else
            {
                if (txtFee.Text == "" || txtAmt.Text == "" || cmbType.Text=="")
                {
                    MessageBox.Show("fill out required fields.", "Fee maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
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
            double checkdigit = Convert.ToDouble(txtAmt.Text);
            string lev = cmbLevel.Text;
            string level = cmbDept.Text;
            /*if (lev == "Kinder")
            {
                level = "Pre-elementary";
            }
            else if (lev == "Grade 1" || lev == "Grade 2" || lev == "Grade 3" || lev == "Grade 4" || lev == "Grade 5" || lev == "Grade 6")
            {
                level = "Grade 1-6";
            }
            else if (lev == "Grade 7" || lev == "Grade 8" || lev == "Grade 9" || lev == "Grade 10")
            {
                level = "Grade 7-10";
            }
            else
            {
                level = cmbLevel.Text;
            }*/

            if (cmbType.Text == "Assessment")
            {
                string amount = "";
                if (checkdigit >= 1000)
                {
                    amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(txtAmt.Text));
                }
                if (checkdigit < 1000)
                {
                    amount = String.Format(("{0:0.00#}"), Convert.ToDouble(txtAmt.Text));
                }
                con.Open();
                string updatefee = "Update fee_tbl set fee='" + txtFee.Text + "',amount='" + amount + "'where id='" + primarykey + "'and SY='" + activeSY + "'";
                OdbcCommand cmdUpdatefee = new OdbcCommand(updatefee, con);
                cmdUpdatefee.ExecuteNonQuery();
                con.Close();

                if (txtAmt.Text != "")
                {
                    double amt = Convert.ToDouble(txtAmt.Text);
                    double orgamount = Convert.ToDouble(selectedfee);
                    double anl = 0;
                    double tf_amt = 0;
                    double reg_amt = 0;
                    double misc_amt = 0;

                  
                    con.Open();

                    OdbcDataAdapter daTF0 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Tuition fee' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dtTF0 = new DataTable();
                    daTF0.Fill(dtTF0);

                    OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt0 = new DataTable();
                    da0.Fill(dt0);

                    OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt01 = new DataTable();
                    da01.Fill(dt01);
                    con.Close();

                    if (dtTF0.Rows.Count > 0)
                    {
                        tf_amt = Convert.ToDouble(dtTF0.Rows[0].ItemArray[2].ToString());
                    }

                    if (dt0.Rows.Count > 0)
                    {
                        reg_amt = Convert.ToDouble(dt0.Rows[0].ItemArray[2].ToString());
                    }

                    if (dt01.Rows.Count > 0)
                    {
                        misc_amt = Convert.ToDouble(dt01.Rows[0].ItemArray[2].ToString());
                    }

                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Annual payment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        anl = reg_amt + misc_amt + tf_amt;
                        double computedMonthlyAmt = tf_amt / 10;
                        double newUponEnr_Amt = reg_amt + misc_amt + computedMonthlyAmt;
                       
                        string amountanl = "";
                        if (anl >= 1000)
                        {
                            amountanl = String.Format(("{0:0,###.00#}"), Convert.ToDouble(anl));
                        }
                        if (anl < 1000)
                        {
                            amountanl = String.Format(("{0:0.00#}"), Convert.ToDouble(anl));
                        }

                        string monthly = "";
                        if (computedMonthlyAmt >= 1000)
                        {
                            monthly = String.Format(("{0:0,###.00#}"), computedMonthlyAmt);
                        }
                        if (computedMonthlyAmt < 1000)
                        {
                            monthly = String.Format(("{0:0.00#}"), computedMonthlyAmt);
                        }

                        string upon = "";
                        if (newUponEnr_Amt >= 1000)
                        {
                            upon = String.Format(("{0:0,###.00#}"), newUponEnr_Amt);
                        } if (newUponEnr_Amt < 1000)
                        {
                            upon = String.Format(("{0:0.00#}"), newUponEnr_Amt);
                        }

                       /* string ttn = "";
                        if (ttnfee >= 1000)
                        {
                            ttn = String.Format(("{0:0,###.00#}"), ttnfee);
                        } if (ttnfee < 1000)
                        {
                            ttn = String.Format(("{0:0.00#}"), ttnfee);
                        }*/

                       
                        con.Open();
                        string upd = "Update fee_tbl set amount='" + amountanl + "'where fee LIKE 'Annual payment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd = new OdbcCommand(upd, con);
                        cmdupd.ExecuteNonQuery();

                        string upd1 = "Update fee_tbl set amount='" + monthly + "'where fee LIKE 'Monthly installment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd1 = new OdbcCommand(upd1, con);
                        cmdupd1.ExecuteNonQuery();

                        string upd2 = "Update fee_tbl set amount='" + upon + "'where fee LIKE 'Upon enrollment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd2 = new OdbcCommand(upd2, con);
                        cmdupd2.ExecuteNonQuery();
                        con.Close();

                       /* string upd3 = "Update fee_tbl set amount='" + ttn + "'where fee LIKE 'Tuition fee' and type='fee' and level='" + level + "'";
                        OdbcCommand cmdupd3 = new OdbcCommand(upd3, con);
                        cmdupd3.ExecuteNonQuery();*/

                        txtFee.Enabled = false; txtAmt.Enabled = false;
                    }
                }

                setupview_fee(level);
            }
            if (cmbType.Text == "Registration breakdown")
            {
                string amount = "";
                if (checkdigit >= 1000)
                {
                    amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(txtAmt.Text));
                }
                if (checkdigit < 1000)
                {
                    amount = String.Format(("{0:0.00#}"), Convert.ToDouble(txtAmt.Text));
                }
                con.Open();
                string updatefee = "Update registrationfee_tbl set fee='" + txtFee.Text + "',amount='" + amount + "'where id='" + primarykey + "'and SY='" + activeSY + "'";
                OdbcCommand cmdUpdatefee = new OdbcCommand(updatefee, con);
                cmdUpdatefee.ExecuteNonQuery();
                con.Close();

                if (txtAmt.Text != "")
                {
                    double amt = Convert.ToDouble(txtAmt.Text);
                    double currentamt = Convert.ToDouble(selectedfee);
                    double reg = 0;
                    double orgamount = 0;
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        reg = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                        orgamount = reg;
                        reg -= currentamt;
                        reg += amt;
                        string amountreg = "";

                        if (reg >= 1000)
                        {
                            amountreg = String.Format(("{0:0,###.00#}"), Convert.ToDouble(reg));
                        }
                        if (reg < 1000)
                        {
                            amountreg = String.Format(("{0:0.00#}"), Convert.ToDouble(reg));
                        }
                       
                        con.Open();
                        string upd = "Update fee_tbl set amount='" + amountreg + "'where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd = new OdbcCommand(upd, con);
                        cmdupd.ExecuteNonQuery();
                        con.Close();
                    }

                    updateannual_thruregi(level, orgamount);
                    updateamount_REGIMISC(level);//new added code
                }
               
                setupview_regfee(level);
            }
            if (cmbType.Text == "Miscellaneous breakdown")
            {
                string amount = "";
                if (checkdigit >= 1000)
                {
                    amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(txtAmt.Text));
                }
                if (checkdigit < 1000)
                {
                    amount = String.Format(("{0:0.00#}"), Convert.ToDouble(txtAmt.Text));
                }
                con.Open();
                string updatefee = "Update miscellaneousfee_tbl set fee='" + txtFee.Text + "',amount='" + amount + "'where id='" + primarykey + "'and SY='" + activeSY + "'";
                OdbcCommand cmdUpdatefee = new OdbcCommand(updatefee, con);
                cmdUpdatefee.ExecuteNonQuery();
                con.Close();

                if (txtAmt.Text != "")
                {
                    double amt = Convert.ToDouble(txtAmt.Text);
                    double currentamt = Convert.ToDouble(selectedfee);
                    double orgamount = 0;
                    double mis = 0;
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        mis = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                        orgamount = mis;
                        mis -= currentamt;
                        mis += amt;
                        string amountmis = "";

                        if (mis >= 1000)
                        {
                            amountmis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(mis));
                        }
                        if (mis < 1000)
                        {
                            amountmis = String.Format(("{0:0.00#}"), Convert.ToDouble(mis));
                        }
                        con.Open();
                        string upd = "Update fee_tbl set amount='" + amountmis + "'where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'";
                        OdbcCommand cmdupd = new OdbcCommand(upd, con);
                        cmdupd.ExecuteNonQuery();
                        con.Close();
                    }

                    updateannual_thrumisc(level, orgamount);
                    updateamount_REGIMISC(level);//new added code
                }

                setupview_miscfee(level);
            }

            btnAdd.Enabled = false;
           
            btnClear.Text = "Clear";
            MessageBox.Show("fee successfully updated", "Fee maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtFee.Text == "" || txtAmt.Text == "" || cmbType.Text=="")
            {
                MessageBox.Show("fill out required fields.", "Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                setupAddFee();
            }
        }

        public void updateamount_registration(string glev)
        {
            if (txtAmt.Text != "")
            {
                double amt = Convert.ToDouble(txtAmt.Text);
                double reg = 0;
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + glev + "'and SY='" + activeSY + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    reg = Convert.ToDouble(dt1.Rows[0].ItemArray[2].ToString());
                    reg += amt;
                    string amount = "";
                    if (reg >= 1000)
                    {
                        amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(reg));
                    }
                    if (reg < 1000)
                    {
                        amount = String.Format(("{0:0.00#}"), Convert.ToDouble(reg));
                    }
             
                    con.Open();
                    string upd = "Update fee_tbl set amount='" + amount + "'where fee LIKE 'Registration' and type='fee' and level='" + glev + "'and SY='" + activeSY + "'";
                    OdbcCommand cmdupd = new OdbcCommand(upd, con);
                    cmdupd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void updateamount_StudFee(string level)
        {
            if (txtAmt.Text != "")
            {
                double amt = Convert.ToDouble(txtAmt.Text);
                double anl = 0;
                double reg_amt = 0;
                double misc_amt = 0;
                double tf_amt = 0;

                con.Open();

                OdbcDataAdapter daTF0 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Tuition fee' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dtTF0 = new DataTable();
                daTF0.Fill(dtTF0);

                OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dt0 = new DataTable();
                da0.Fill(dt0);

                OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dt01 = new DataTable();
                da01.Fill(dt01);

                OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Annual payment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dtTF0.Rows.Count > 0)
                {
                    tf_amt = Convert.ToDouble(dtTF0.Rows[0].ItemArray[2].ToString());
                }

                if (dt0.Rows.Count > 0)
                {
                    reg_amt = Convert.ToDouble(dt0.Rows[0].ItemArray[2].ToString());
                }

                if (dt01.Rows.Count > 0)
                {
                    misc_amt = Convert.ToDouble(dt01.Rows[0].ItemArray[2].ToString());
                }

                if (dt1.Rows.Count > 0)
                {
                    anl = reg_amt + misc_amt + tf_amt;
                    double computedMontlyAmt = tf_amt / 10;
                    double newUponEnr_Amt = reg_amt + misc_amt + computedMontlyAmt;
                   
                    string amount = "";
                    if (amt >= 1000){
                        amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(anl));} 
                    if (amt < 1000){
                        amount = String.Format(("{0:0.00#}"), Convert.ToDouble(anl)); }

                    string monthly = "";
                    if (computedMontlyAmt >= 1000){
                        monthly = String.Format(("{0:0,###.00#}"), computedMontlyAmt);}
                    if (computedMontlyAmt< 1000) {
                        monthly = String.Format(("{0:0.00#}"), computedMontlyAmt);}

                    string upon = "";
                    if (newUponEnr_Amt >= 1000){
                        upon = String.Format(("{0:0,###.00#}"), newUponEnr_Amt);
                    }if (newUponEnr_Amt < 1000) {
                        upon = String.Format(("{0:0.00#}"), newUponEnr_Amt);}

                    /*string ttn = "";
                    if (ttnfee >= 1000)
                    {
                        ttn = String.Format(("{0:0,###.00#}"), ttnfee);
                    } if (ttnfee < 1000)
                    {
                        ttn = String.Format(("{0:0.00#}"), ttnfee);
                    }*/

                    con.Open();
                    string upd = "Update fee_tbl set amount='" + amount + "'where fee LIKE 'Annual payment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                    OdbcCommand cmdupd = new OdbcCommand(upd, con);
                    cmdupd.ExecuteNonQuery();

                    string upd1 = "Update fee_tbl set amount='" + monthly + "'where fee LIKE 'Monthly installment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                    OdbcCommand cmdupd1 = new OdbcCommand(upd1, con);
                    cmdupd1.ExecuteNonQuery();

                    string upd2 = "Update fee_tbl set amount='" + upon + "'where fee LIKE 'Upon enrollment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                    OdbcCommand cmdupd2 = new OdbcCommand(upd2, con);
                    cmdupd2.ExecuteNonQuery();

                    /*string upd3 = "Update fee_tbl set amount='" + ttn + "'where fee LIKE 'Tuition fee' and type='fee' and level='" + level + "'";
                    OdbcCommand cmdupd3 = new OdbcCommand(upd3, con);
                    cmdupd3.ExecuteNonQuery();*/

                    con.Close();
                }
            }
        }

        public void updateamount_REGIMISC(string level)
        {
            if (txtAmt.Text != "")
            {
                double anl = 0;
                double reg_amt = 0;
                double misc_amt = 0;
                double tf_amt = 0;

                con.Open();

                OdbcDataAdapter daTF0 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Tuition fee' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dtTF0 = new DataTable();
                daTF0.Fill(dtTF0);

                OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dt0 = new DataTable();
                da0.Fill(dt0);

                OdbcDataAdapter da01 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dt01 = new DataTable();
                da01.Fill(dt01);

                OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Annual payment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dtTF0.Rows.Count > 0)
                {
                    tf_amt = Convert.ToDouble(dtTF0.Rows[0].ItemArray[2].ToString());
                }

                if (dt0.Rows.Count > 0)
                {
                    reg_amt = Convert.ToDouble(dt0.Rows[0].ItemArray[2].ToString());
                }

                if (dt01.Rows.Count > 0)
                {
                    misc_amt = Convert.ToDouble(dt01.Rows[0].ItemArray[2].ToString());
                }

                if (dt1.Rows.Count > 0)
                {
                    anl = reg_amt + misc_amt + tf_amt;
                    double computedMontlyAmt = tf_amt / 10;
                    double newUponEnr_Amt = reg_amt + misc_amt + computedMontlyAmt;
                   
                    string monthly = "";
                    if (computedMontlyAmt >= 1000)
                    {
                        monthly = String.Format(("{0:0,###.00#}"), computedMontlyAmt);
                    }
                    if (computedMontlyAmt < 1000)
                    {
                        monthly = String.Format(("{0:0.00#}"), computedMontlyAmt);
                    }

                    string upon = "";
                    if (newUponEnr_Amt >= 1000)
                    {
                        upon = String.Format(("{0:0,###.00#}"), newUponEnr_Amt);
                    } if (newUponEnr_Amt < 1000)
                    {
                        upon = String.Format(("{0:0.00#}"), newUponEnr_Amt);
                    }

                  
                    con.Open();

                    string upd1 = "Update fee_tbl set amount='" + monthly + "'where fee LIKE 'Monthly installment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                    OdbcCommand cmdupd1 = new OdbcCommand(upd1, con);
                    cmdupd1.ExecuteNonQuery();

                    string upd2 = "Update fee_tbl set amount='" + upon + "'where fee LIKE 'Upon enrollment' and type='payment' and level='" + level + "'and SY='" + activeSY + "'";
                    OdbcCommand cmdupd2 = new OdbcCommand(upd2, con);
                    cmdupd2.ExecuteNonQuery();

                  
                    con.Close();
                }
            }
        }

        public void updateamount_misc(string level)
        {
            if (txtAmt.Text != "")
            {
                double amt = Convert.ToDouble(txtAmt.Text);
                double mis = 0;
                con.Open();
                OdbcDataAdapter da1 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                con.Close();

                if (dt1.Rows.Count > 0)
                {
                    mis = Convert.ToDouble(dt1.Rows[0].ItemArray[2].ToString());
                    mis += amt;
                    string amount = "";
                    if (mis >= 1000)
                    {
                        amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(mis));
                    }
                    if (mis < 1000)
                    {
                        amount = String.Format(("{0:0.00#}"), Convert.ToDouble(mis));
                    }
                    con.Open();
                    string upd = "Update fee_tbl set amount='" + amount + "'where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'";
                    OdbcCommand cmdupd = new OdbcCommand(upd, con);
                    cmdupd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void setupAddFee()
        {
            double checkdigit = Convert.ToDouble(txtAmt.Text);
            string lev = cmbLevel.Text;
            string level = cmbDept.Text;
            /*if (lev == "Kinder")
            {
                level = "Pre-elementary";
            }
            else if (lev == "Grade 1" || lev == "Grade 2" || lev == "Grade 3" || lev == "Grade 4" || lev == "Grade 5" || lev == "Grade 6")
            {
                level = "Grade 1-6";
            }
            else if (lev == "Grade 7" || lev == "Grade 8" || lev == "Grade 9" || lev == "Grade 10")
            {
                level = "Grade 7-10";
            }
            else
            {
                level = cmbLevel.Text;
            }*/

            if (cmbType.Text == "Assessment")
            {
                if (cmbDept.Text == "All Department")
                {
                    string amount = "";
                    if (checkdigit >= 1000)
                    {
                        amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(txtAmt.Text));
                    }
                    if (checkdigit < 1000)
                    {
                        amount = String.Format(("{0:0.00#}"), Convert.ToDouble(txtAmt.Text));
                    }

                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select deptname from department_tbl", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string gdept = dt.Rows[i].ItemArray[0].ToString();
                             
                            con.Open();
                            string addfee = "Insert Into fee_tbl(fee,amount,level,type,SY)values('" + txtFee.Text + "','" + amount + "','" + gdept + "','" + "fee" + "','" + activeSY + "')";
                            OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                            cmdAddfee.ExecuteNonQuery();
                            con.Close();
                            updateamount_StudFee(gdept);
                        }
                    }
                }
                else
                {
                    string amount = "";
                    if (checkdigit >= 1000)
                    {
                        amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(txtAmt.Text));
                    }
                    if (checkdigit < 1000)
                    {
                        amount = String.Format(("{0:0.00#}"), Convert.ToDouble(txtAmt.Text));
                    }
                    con.Open();
                    string addfee = "Insert Into fee_tbl(fee,amount,level,type,SY)values('" + txtFee.Text + "','" + amount + "','" + level + "','" + "fee" + "','" + activeSY + "')";
                    OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                    cmdAddfee.ExecuteNonQuery();
                    con.Close();

                    if (txtAmt.Text != "")
                    {
                        updateamount_StudFee(level);
                    }
                    //updateamount_StudFee(level);
                }

                setupview_fee(level);
            }
            if (cmbType.Text == "Registration breakdown")
            {
                if (cmbDept.Text == "All Department")
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select deptname from department_tbl", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    string amount = "";
                    if (checkdigit >= 1000)
                    {
                        amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(txtAmt.Text));
                    }
                    if (checkdigit < 1000)
                    {
                        amount = String.Format(("{0:0.00#}"), Convert.ToDouble(txtAmt.Text));
                    }

                    if (dt.Rows.Count > 0)
                    {
                        double orgamount = 0;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string gdept = dt.Rows[i].ItemArray[0].ToString();

                            con.Open();
                            string addfee = "Insert Into registrationfee_tbl(fee,amount,level,SY)values('" + txtFee.Text + "','" + amount + "','" + gdept + "','" + activeSY + "')";
                            OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                            cmdAddfee.ExecuteNonQuery();
                            con.Close();

                            updateamount_registration(gdept);
                            updateamount_REGIMISC(gdept);
                            //updateannual_thruregi(gdept, orgamount);
                        }

                        con.Open();
                        OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and SY='" + activeSY + "'", con);
                        DataTable dt0 = new DataTable();
                        da0.Fill(dt0);
                        con.Close();

                        if (dt0.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt0.Rows.Count; i++)
                            {
                                orgamount = Convert.ToDouble(dt0.Rows[i].ItemArray[2].ToString());
                                updateannual_thruregi(dt0.Rows[i].ItemArray[3].ToString(), orgamount);
                            }
                        }
                    }
                }
                else
                {
                    string amount = "";
                    if (checkdigit >= 1000)
                    {
                        amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(txtAmt.Text));
                    }
                    if (checkdigit < 1000)
                    {
                        amount = String.Format(("{0:0.00#}"), Convert.ToDouble(txtAmt.Text));
                    }
                    con.Open();
                    string addfee = "Insert Into registrationfee_tbl(fee,amount,level,SY)values('" + txtFee.Text + "','" + amount + "','" + level + "','" + activeSY + "')";
                    OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                    cmdAddfee.ExecuteNonQuery();
                    con.Close();

                    if (txtAmt.Text != "")
                    {
                        double amt = Convert.ToDouble(txtAmt.Text);
                        double reg = 0;
                        double orgprice = 0;
                        con.Open();
                        OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        con.Close();

                        if (dt.Rows.Count > 0)
                        {
                            reg = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                            orgprice = reg;
                            reg += amt;
                            string amountreg = "";
                            if (reg >= 1000)
                            {
                                amountreg = String.Format(("{0:0,###.00#}"), Convert.ToDouble(reg));
                            }
                            if (reg < 1000)
                            {
                                amountreg = String.Format(("{0:0.00#}"), Convert.ToDouble(reg));
                            }
                            con.Open();
                            string upd = "Update fee_tbl set amount='" + amountreg + "'where fee LIKE 'Registration' and type='fee' and level='" + level + "'and SY='" + activeSY + "'";
                            OdbcCommand cmdupd = new OdbcCommand(upd, con);
                            cmdupd.ExecuteNonQuery();
                            con.Close();
                            updateannual_thruregi(level, orgprice);
                            updateamount_REGIMISC(level);
                        }//DITO PO
                    } 
                }

                setupview_regfee(level);
            }
            if (cmbType.Text == "Miscellaneous breakdown")
            {
                if (cmbDept.Text == "All Department")
                {
                    con.Open();
                    OdbcDataAdapter da = new OdbcDataAdapter("Select deptname from department_tbl", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    string amount = "";
                    if (checkdigit >= 1000)
                    {
                        amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(txtAmt.Text));
                    }
                    if (checkdigit < 1000)
                    {
                        amount = String.Format(("{0:0.00#}"), Convert.ToDouble(txtAmt.Text));
                    }

                    if (dt.Rows.Count > 0)
                    {
                        double orgamount = 0;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string gdept = dt.Rows[i].ItemArray[0].ToString();

                            con.Open();
                            string addfee = "Insert Into miscellaneousfee_tbl(fee,amount,level,SY)values('" + txtFee.Text + "','" + amount + "','" + gdept + "','" + activeSY + "')";
                            OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                            cmdAddfee.ExecuteNonQuery();
                            con.Close();
                            updateamount_misc(gdept);
                            //updateannual_thrumisc(gdept, orgamount);
                            updateamount_REGIMISC(gdept);

                        }

                        con.Open();
                        OdbcDataAdapter da0 = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and SY='" + activeSY + "'", con);
                        DataTable dt0 = new DataTable();
                        da0.Fill(dt0);
                        con.Close();

                        if (dt0.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt0.Rows.Count; i++)
                            {
                                orgamount = Convert.ToDouble(dt0.Rows[i].ItemArray[2].ToString());
                                updateannual_thrumisc(dt0.Rows[i].ItemArray[3].ToString(), orgamount);
                            }
                           
                        }
                    }
                }
                else
                {
                    string amount = "";
                    if (checkdigit >= 1000)
                    {
                        amount = String.Format(("{0:0,###.00#}"), Convert.ToDouble(txtAmt.Text));
                    }
                    if (checkdigit < 1000)
                    {
                        amount = String.Format(("{0:0.00#}"), Convert.ToDouble(txtAmt.Text));
                    }
                    con.Open();
                    string addfee = "Insert Into miscellaneousfee_tbl(fee,amount,level,SY)values('" + txtFee.Text + "','" + amount + "','" + level + "','" + activeSY + "')";
                    OdbcCommand cmdAddfee = new OdbcCommand(addfee, con);
                    cmdAddfee.ExecuteNonQuery();
                    con.Close();

                    if (txtAmt.Text != "")
                    {
                        double amt = Convert.ToDouble(txtAmt.Text);
                        double mis = 0;
                        double orgprice = 0;
                        con.Open();
                        OdbcDataAdapter da = new OdbcDataAdapter("Select*from fee_tbl where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'", con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        con.Close();

                        if (dt.Rows.Count > 0)
                        {
                            mis = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                            orgprice = mis;
                            mis += amt;
                            string amountmis = "";
                            if (mis >= 1000)
                            {
                                amountmis = String.Format(("{0:0,###.00#}"), Convert.ToDouble(mis));
                            }
                            if (mis< 1000)
                            {
                                amountmis = String.Format(("{0:0.00#}"), Convert.ToDouble(mis));
                            }
                            con.Open();
                            string upd = "Update fee_tbl set amount='" + amountmis + "'where fee LIKE 'Miscellaneous' and type='fee' and level='" + level + "'and SY='" + activeSY + "'";
                            OdbcCommand cmdupd = new OdbcCommand(upd, con);
                            cmdupd.ExecuteNonQuery();
                            con.Close();
                            updateannual_thrumisc(level, orgprice);
                            updateamount_REGIMISC(level);
                        }

                    }
                }

                setupview_miscfee(level);
            }

            btnAdd.Enabled = false;
            
            MessageBox.Show("fee successfully added", "Fee maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSearch.Focus();
        }

        public void setupview_allfee()
        {
            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select distinct id,fee as 'Fee',amount as 'Amount', level as 'Level' from fee_tbl where type='fee' and SY='" + activeSY + "' order by fee ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            dvFee = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvFee;
                dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                dgvSearch.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSearch.Columns[0].Width = 0;
                dgvSearch.Columns[1].Width = 200;
                dgvSearch.Columns[2].Width = 150;
                dgvSearch.Columns[3].Width = 125;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }

            lblResult.Text = "number of fee: " + dgvSearch.Rows.Count.ToString();
        }

        public void setupview_fee(string level)
        {
           
            /*string level = "";
            if (lev == "Kinder")
            {
                level = "Pre-elementary";
            }
            else if (lev == "Grade 1" || lev == "Grade 2" || lev == "Grade 3" || lev == "Grade 4" || lev == "Grade 5" || lev == "Grade 6")
            {
                level = "Grade 1-6";
            }
            else if (lev == "Grade 7" || lev == "Grade 8" || lev == "Grade 9" || lev == "Grade 10")
            {
                level = "Grade 7-10";
            }
            else
            {
                level = cmbLevel.Text;
            }*/

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id,fee as 'Fee',amount as 'Amount' from fee_tbl where level='" + level + "'and type='fee' and SY='" + activeSY + "' order by fee ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dvFee = new DataView(dt);
            dgvSearch.DataSource = dvFee;

            dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
            dgvSearch.Columns[0].Width = 0;
            dgvSearch.Columns[1].Width = 300;
            dgvSearch.Columns[2].Width = 175;
            dgvSearch.Columns[1].DefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
            dgvSearch.Columns[2].DefaultCellStyle.Font = new Font("Arial",11,FontStyle.Bold);
            dgvSearch.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
               // dgvSearch.DataSource = null;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }

            lblResult.Text = "number of fee: " + dgvSearch.Rows.Count.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dvFee.RowFilter = string.Format("Fee LIKE '%{0}%'", txtSearch.Text);
            dgvSearch.DataSource = dvFee;

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

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                pnlnotify.Visible = true;
            }

            txtFee.Clear();
            txtAmt.Clear();
            txtFee.Enabled = true;
            txtAmt.Enabled = true;
            
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            btnUpdate.Text = "Update";

            if (cmbType.Text == "Registration breakdown")
            {
                setupview_regfee(cmbLevel.Text);
            }
            if (cmbType.Text == "Miscellaneous breakdown")
            {
                setupview_miscfee(cmbLevel.Text);
            }
            if (cmbType.Text == "Assessment")
            {
                setupview_fee(cmbLevel.Text);
            }
        
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
           // primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();

            if (dgvSearch.Rows.Count <= 0)
            {
                return;
            }

            dgvSearch.SelectedRows[0].Cells[0].Style.ForeColor = Color.Silver;
            string feename = "";
            string feeamt = "";
          
            string lev = cmbLevel.Text;

            string levdep = "";

            con.Open();
            OdbcDataAdapter dadep = new OdbcDataAdapter("Select department from level_tbl where level='" + lev + "'", con);
            DataTable dtdep = new DataTable();
            dadep.Fill(dtdep);
            con.Close();
            if (dtdep.Rows.Count > 0)
            {
                levdep = dtdep.Rows[0].ItemArray[0].ToString();
            }

           
            if (dgvSearch.SelectedRows[0].Cells[0].Value.ToString() != "")
            {
                primarykey = dgvSearch.SelectedRows[0].Cells[0].Value.ToString();
            }
            if (dgvSearch.SelectedRows[0].Cells[1].Value.ToString()!="")
            {
                feename = dgvSearch.SelectedRows[0].Cells[1].Value.ToString();
            }
            if (dgvSearch.SelectedRows[0].Cells[2].Value.ToString() != "")
            {
                feeamt = dgvSearch.SelectedRows[0].Cells[2].Value.ToString();
                selectedfee = feeamt;
            }

            txtFee.Enabled = false;
            txtAmt.Enabled = false;

            if (cmbType.Text != "Assessment")
            {
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                if (feename.Contains("TUITION FEE") == true)
                {
                    btnUpdate.Enabled = true;
                }
                else
                {
                    btnUpdate.Enabled = false;
                }
            }

            setup_retrieve(primarykey);  
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LOGOUT();
            frmEmpLogin home = new frmEmpLogin();
            this.Hide();
            home.Show();
        }

        private void btnAbt_Click(object sender, EventArgs e)
        {
            frmAboutMaintenance abtform = new frmAboutMaintenance();
            this.Hide();
            abtform.amlog = feelog;
            abtform.Show();
        }

        private void btnAudittrail_Click(object sender, EventArgs e)
        {
            frmAudit audform = new frmAudit();
            this.Hide();
            audform.auditlogger = feelog;
            audform.Show();
        }

        private void btnActivity_Click(object sender, EventArgs e)
        {
            frmActivity actform = new frmActivity();
            this.Hide();
            actform.actlog = feelog;
            actform.Show();
        }

        private void btnReq_Click(object sender, EventArgs e)
        {
            frmRequirement reqform = new frmRequirement();
            this.Hide();
            reqform.reqlog = feelog;
            reqform.VISITED = VISITED;
            reqform.Show();
        }

        private void btnSched_Click(object sender, EventArgs e)
        {
            frmSched sf = new frmSched();
            this.Hide();
            sf.schedlog = feelog;
            sf.VISITED = VISITED;
            sf.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom roomform = new frmRoom();
            this.Hide();
            roomform.logger = feelog;
            roomform.VISITED = VISITED;
            roomform.Show();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            frmSection secform = new frmSection();
            this.Hide();
            secform.secwholog = feelog;
            secform.VISITED = VISITED;
            secform.Show();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            frmSubject subform = new frmSubject();
            this.Hide();
            subform.wholog = feelog;
            subform.VISITED = VISITED;
            subform.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmMaintenance maine = new frmMaintenance();
            this.Hide();
            maine.adminlog = feelog;
            maine.VISITED = VISITED;
            maine.Show();
        }

        private void txtAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (Char.IsLetter(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            frmDiscount discform = new frmDiscount();
            this.Hide();
            discform.disclog = feelog;
            discform.VISITED = VISITED;
            discform.Show();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            frmBackup buf = new frmBackup();
            this.Hide();
            buf.backlog = feelog;
            buf.Show();
        }

        private void frmFee_FormClosing(object sender, FormClosingEventArgs e)
        {
            LOGOUT();
            frmEmpLogin hf = new frmEmpLogin();
            this.Hide();
            hf.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
        }

        private void btnSY_Click(object sender, EventArgs e)
        {
            frmSchoolYear symaintenance = new frmSchoolYear();
            symaintenance.sylog = feelog;
            symaintenance.VISITED = VISITED;
            symaintenance.Show();
            this.Hide();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmLevel levmain = new frmLevel();
            this.Hide();
            levmain.levlog = feelog;
            levmain.VISITED = VISITED;
            levmain.Show();
        }

        private void btnFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty facmain = new frmFaculty();
            facmain.facmlog = feelog;
            facmain.VISITED = VISITED;
            facmain.Show();
            this.Hide();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnAdmMain_Click(object sender, EventArgs e)
        {
            frmHomeMaintenance hm = new frmHomeMaintenance();
            hm.adminlog = feelog;
            hm.VISITED = VISITED;
            this.Hide();
            hm.Show();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFee.Clear();
            txtAmt.Clear();
            txtFee.Enabled = true;
            txtAmt.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            btnUpdate.Text = "Update";


            if (cmbType.Text == "Assessment")
            {
                setupview_fee(cmbDept.Text);
                btnAdd.Enabled = false;
            }
            if (cmbType.Text == "Registration breakdown")
            {
                setupview_regfee(cmbDept.Text);
                btnAdd.Enabled = true;
               
            }
            if (cmbType.Text == "Miscellaneous breakdown")
            {
                setupview_miscfee(cmbDept.Text);
                btnAdd.Enabled = true;
            }
           
        }

        public void setupview_regfee(string level)
        {

            /*string level = "";
            if (lev == "Kinder")
            {
                level = "Pre-elementary";
            }
            else if (lev == "Grade 1" || lev == "Grade 2" || lev == "Grade 3" || lev == "Grade 4" || lev == "Grade 5" || lev == "Grade 6")
            {
                level = "Grade 1-6";
            }
            else if (lev == "Grade 7" || lev == "Grade 8" || lev == "Grade 9" || lev == "Grade 10")
            {
                level = "Grade 7-10";
            }
            else
            {
                level = cmbLevel.Text;
            }*/

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id,fee as 'Fee',amount as 'Amount' from registrationfee_tbl where level='" + level + "'and SY='" + activeSY + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            dvFee = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvFee;

                dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                dgvSearch.Columns[0].Width = 0;
                dgvSearch.Columns[1].Width = 300;
                dgvSearch.Columns[2].Width = 175;
                dgvSearch.Columns[1].DefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Regular);
                dgvSearch.Columns[2].DefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Regular);
                dgvSearch.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }

            lblResult.Text = "no. of fees included in registration: " + dgvSearch.Rows.Count.ToString();
        }


        public void setupview_miscfee(string level)
        {

            /*string level = "";
            if (lev == "Kinder")
            {
                level = "Pre-elementary";
            }
            else if (lev == "Grade 1" || lev == "Grade 2" || lev == "Grade 3" || lev == "Grade 4" || lev == "Grade 5" || lev == "Grade 6")
            {
                level = "Grade 1-6";
            }
            else if (lev == "Grade 7" || lev == "Grade 8" || lev == "Grade 9" || lev == "Grade 10")
            {
                level = "Grade 7-10";
            }
            else
            {
                level = cmbLevel.Text;
            }*/

            con.Open();
            OdbcDataAdapter da = new OdbcDataAdapter("Select id,fee as 'Fee',amount as 'Amount' from miscellaneousfee_tbl where level='" + level + "'and SY='" + activeSY + "' order by fee ASC", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            con.Close();

            dvFee = new DataView(dt);

            if (dt.Rows.Count > 0)
            {
                pnlnotify.Visible = false;
                dgvSearch.DataSource = null;
                dgvSearch.DataSource = dvFee;

                dgvSearch.Columns[0].DefaultCellStyle.ForeColor = Color.White;
                dgvSearch.Columns[0].Width = 0;
                dgvSearch.Columns[1].Width = 300;
                dgvSearch.Columns[2].Width = 175;
                dgvSearch.Columns[1].DefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Regular);
                dgvSearch.Columns[2].DefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Regular);
                dgvSearch.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else
            {
                dgvSearch.DataSource = null;
                pnlnotify.Visible = true;
                lblnote.Text = "no items found...";
            }

            lblResult.Text = "no. of fees included in miscellaneous: " + dgvSearch.Rows.Count.ToString();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff stfform = new frmStaff();
            this.Dispose();
            stfform.stflog = feelog;
            stfform.VISITED = VISITED;
            stfform.Show();
        }

        private void btnStud_Click(object sender, EventArgs e)
        {
            frmStudent stdform = new frmStudent();
            this.Dispose();
            stdform.stdlog = feelog;
            stdform.VISITED = VISITED;
            stdform.Show();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            frmDepartment deptmainte = new frmDepartment();
            this.Dispose();
            deptmainte.deplog = feelog;
            deptmainte.VISITED = VISITED;
            deptmainte.Show();
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count <= 0)
            {
                pnlnotify.Visible = true;
            }

            txtFee.Clear();
            txtAmt.Clear();
            txtFee.Enabled = true;
            txtAmt.Enabled = true;

            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnUpdate.Text = "Update";
            if (cmbType.Text == "")
            {
                pnlnotify.Visible = false;
            }

            if (cmbType.Text == "Registration breakdown")
            {
                setupview_regfee(cmbDept.Text);
                btnAdd.Enabled = true;
            }
            if (cmbType.Text == "Miscellaneous breakdown")
            {
                setupview_miscfee(cmbDept.Text);
                btnAdd.Enabled = true;
            }
            if (cmbType.Text == "Assessment")
            {
                setupview_fee(cmbDept.Text);
                btnAdd.Enabled = false;
            }
            
        }  
    }
}
