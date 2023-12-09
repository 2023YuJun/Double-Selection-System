using Model;
using SQLDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForm;

namespace AdminTerminal
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private void Loginbtn_Click(object sender, EventArgs e)
        {
            /*该btn作用为登录按钮，主要检索是否为本系统用户*/
            if (Accounttb.Text.Trim() == "" || Passwordtb.Text.Trim() == "")
            {
                Tiplab.Visible = true;
                if (Accounttb.Text.Trim() == "")
                    Accounttb.Focus();
                else Passwordtb.Focus();
                return;
            }
            BLL.A_User a_user = new BLL.A_User();
            if (a_user.Login(Accounttb.Text.Trim(), Passwordtb.Text.Trim()))
            {
                AdminTerminal.LexitRequested = false;
                this.Close();
            }
            else
            {
                Tiplab.Text = "The user name or password is incorrect";
                Tiplab.Visible = true;
                Passwordtb.Text = "";
                Passwordtb.Focus();
            }

        }


        //回车跳转
        private void Accounttb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                Passwordtb.Focus();
            }
        }

        private void Passwordtb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                Loginbtn_Click(sender, e);
            }
        }

        private void Fotgotlab_Click(object sender, EventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.ShowDialog();
        }
    }
}
