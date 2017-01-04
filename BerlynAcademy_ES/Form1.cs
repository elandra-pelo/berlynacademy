using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BerlynAcademy_ES
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(49, 79, 142);
           
        }

        private void btnStd_MouseEnter(object sender, EventArgs e)
        {
            btnStd.ForeColor = Color.Gainsboro;
        }

        private void btnStd_MouseLeave(object sender, EventArgs e)
        {
            btnStd.ForeColor = Color.White;
        }

        private void btnEmp_MouseEnter(object sender, EventArgs e)
        {
            btnEmp.ForeColor = Color.Gainsboro;
        }

        private void btnEmp_MouseLeave(object sender, EventArgs e)
        {
            btnEmp.ForeColor = Color.White;
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

        private void btnEmp_Click(object sender, EventArgs e)
        {
            frmEmpLogin frm1 = new frmEmpLogin();
            this.Hide();
            frm1.Show();
        }

        private void btnStd_Click(object sender, EventArgs e)
        {
            frmAdmission admit = new frmAdmission(); 
            this.Hide();
            admit.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
