using Common;
using Model;
using SQLDAL;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeacherTerminal
{
    public partial class BIOT : Form
    {
        public BIOT()
        {
            InitializeComponent();
        }
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();

        private void BIOT_Load(object sender, EventArgs e)
        {
            var biot = db.Queryable<DSS_3_8_BIOT>().Where(it => it.Account == UserHelper.user.Account).First();
            try
            {
                if (biot != null)
                {
                    textBox1.Text = biot.TeacherName;
                    textBox2.Text = biot.Sex;
                    textBox3.Text = biot.Account;
                    textBox4.Text = biot.Contact;
                    textBox5.Text = biot.Faculties;
                    textBox6.Text = biot.Specialty;
                    textBox7.Text = biot.LeadTeam;
                    textBox8.Text = biot.PersonalDeeds;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            button2.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 8; i++)
            {
                TextBox textBox = Controls.Find("textBox" + i, true).FirstOrDefault() as TextBox;
                if (textBox != null)
                {
                    textBox.ReadOnly = false;
                    textBox.BackColor = Color.White;
                }
            }
            textBox1.ReadOnly = true;
            textBox3.ReadOnly = true;
            button2.Enabled = true;
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var update = db.Queryable<DSS_3_8_BIOT>().Where(it => it.Account == UserHelper.user.Account).First();
            update.Sex = textBox2.Text.Trim();
            update.Faculties = textBox5.Text.Trim();
            update.Specialty = textBox6.Text.Trim();
            update.LeadTeam = textBox7.Text.Trim();
            update.PersonalDeeds = textBox8.Text.Trim();
            update.Contact = textBox4.Text.Trim();

            var affectedRows = db.Updateable(update)
                                        .UpdateColumns(it => new { it.Sex, it.Faculties, it.Specialty, it.LeadTeam, it.PersonalDeeds, it.Contact })
                                        .ExecuteCommand();
            if (affectedRows > 0)
            {
                MessageBox.Show("修改成功！");
                button2.Enabled = false;
                button1.Enabled = true;
                for (int i = 1; i <= 8; i++)
                {
                    TextBox textBox = Controls.Find("textBox" + i, true).FirstOrDefault() as TextBox;
                    if (textBox != null)
                    {
                        textBox.ReadOnly = true;
                        textBox.BackColor = Color.FromArgb(211, 226, 244);
                    }
                }
            }
            else
            {
                MessageBox.Show("修改失败！");
            }

        }
    }
}
