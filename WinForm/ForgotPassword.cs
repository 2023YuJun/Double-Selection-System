using Common;
using Model;
using SQLDAL;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void button1_Click(object sender, EventArgs e)
        {
            bool UpdateAffirm = false;
            DSS_3_8_User dSS_3_8_User = new DSS_3_8_User();
            dSS_3_8_User.SecretKey = textBox1.Text;
            dSS_3_8_User.Password = textBox3.Text;
            var SIUser = db.Queryable<DSS_3_8_User>().Where(it => it.Account == textBox1.Text.Trim()).First();
            #region 密码确认判断
            if (!string.IsNullOrEmpty(SIUser.SecretKey))
            {
                if (SIUser.SecretKey != textBox2.Text)
                {
                    UpdateAffirm = false;
                    TipLabel.Text = "密钥错误";
                    TipLabel.Visible = true;
                    return;
                }
                else UpdateAffirm = true;

                if (!IsInputValid(textBox3.Text))
                {
                    UpdateAffirm = false;
                    TipLabel.Text = "请输入英文与数字的组合，且字符数不小于6位高于18位";
                    TipLabel.Visible = true;
                    return;
                }
                else UpdateAffirm = true;

                if (string.IsNullOrEmpty(textBox3.Text) || (textBox4.Text != textBox3.Text))
                {
                    UpdateAffirm = false;
                    TipLabel.Text = "密码确认错误";
                    TipLabel.Visible = true;
                    return;
                }
                else UpdateAffirm = true;
            }
            else
            {
                MessageBox.Show("你没有设置过密钥，无法修改密码，如果使用初始账号和密码无法登录，请联系管理员");
            }
            if (UpdateAffirm)
            {
                db.Updateable(dSS_3_8_User).UpdateColumns(it => new { it.SecretKey, it.Password }).Where(it => it.Account == UserHelper.user.Account).ExecuteCommand();
                MessageBox.Show("密码修改成功！");
                this.Close();
            }
            else return;
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static bool IsInputValid(string input)
        {
            // 正则表达式：匹配至少一个数字和至少一个字母，且不能有中文，长度不少于6位不高于18位
            string pattern = @"^(?=.*[0-9])(?=.*[a-zA-Z])(?!.*[\u4e00-\u9fa5]).{6,18}$";

            return Regex.IsMatch(input, pattern);
        }
        #region enter转到下一个文本框
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') textBox2.Focus();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') textBox3.Focus();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') textBox4.Focus();
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') button1_Click(sender, e);
        }

        #endregion
    }
}
