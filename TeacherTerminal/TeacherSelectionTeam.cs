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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TeacherTerminal
{
    public partial class TeacherSelectionTeam : Form
    {
        public TeacherSelectionTeam()
        {
            InitializeComponent();
        }
        bool isnulldgv = true;
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void TeacherSelectionTeam_Load(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "所有队伍信息", "队伍名称搜索", "课题名称搜索", "学生姓名搜索" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var query = db.Queryable<DSS_3_8_BIOTEAM>();

            if (comboBox.Text == "队伍名称搜索")
            {
                query = query.Where(it => it.YourTeam == textBox1.Text);
            }
            else if (comboBox.Text == "课题名称搜索")
            {
                query = query.Where(it => it.TopicName == textBox1.Text);
            }
            else if (comboBox.Text == "学生姓名搜索")
            {
                var searchText = textBox1.Text;
                query = query.Where(it => it.TL == searchText || it.TM1 == searchText || it.TM2 == searchText || it.TM3 == searchText);
            }
            dataGridView1.DataSource = query.Select(it => new {
                队伍名称 = it.YourTeam,
                课题名称 = it.TopicName,
                课题简介 = it.TopicIntroduction,
                队伍人数 = it.Number,
                队长 = it.TL,
                队员一 = it.TM1,
                队员二 = it.TM2,
                队员三 = it.TM3,
                毕设文件 = it.FileName,
                毕设文件下载路径 = it.FileDownloadPath
            }).ToList();
            if (query.ToList().Count == 0)
            {
                isnulldgv = true;
            }
            else
            {
                isnulldgv = false;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            var SITC = db.Queryable<DSS_3_8_TC>().Where(it => it.TeacherName == UserHelper.biot.TeacherName).ToList();
            if (SITC.Count > 0)
            {
                MessageBox.Show("您已经有带领队伍了，无法再次创建带领，如需修改请到“当前带领队伍”界面进行修改");
                return;
            }
            else
            {
                DSS_3_8_TC dSS_3_8_TC = new DSS_3_8_TC();
                dSS_3_8_TC.Account = UserHelper.user.Account;
                dSS_3_8_TC.TeacherName = UserHelper.biot.TeacherName;
                dSS_3_8_TC.T1 = textBox2.Text;
                dSS_3_8_TC.T2 = textBox3.Text;
                dSS_3_8_TC.T3 = textBox4.Text;
                dSS_3_8_TC.T4 = textBox5.Text;
                dSS_3_8_TC.T5 = textBox6.Text;
                db.Insertable(dSS_3_8_TC).ExecuteCommand();
                MessageBox.Show("您已成功带领以上队伍！");
            }
        }

        #region 文本框点击传入
        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox2, new TextBox[] { textBox3, textBox4, textBox5, textBox6 });
        }

        private void textBox3_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox3, new TextBox[] { textBox2, textBox4, textBox5, textBox6 });
        }

        private void textBox4_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox4, new TextBox[] { textBox3, textBox2, textBox5, textBox6 });
        }

        private void textBox5_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox5, new TextBox[] { textBox3, textBox4, textBox2, textBox6 });
        }

        private void textBox6_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox6, new TextBox[] { textBox3, textBox4, textBox5, textBox2 });
        }
        private void TextBox_MouseDown(TextBox textBox, TextBox[] otherTextBoxes)
        {
            DataGridViewRow selectedRow;
            string? input = null;
            if (!isnulldgv)
            {
                selectedRow = dataGridView1.SelectedRows[0];
                input = selectedRow.Cells["队伍名称"].Value.ToString();
            }
            else
            {
                MessageBox.Show("无效信息导入！");
                return;
            }
            if (!string.IsNullOrEmpty(input) && !otherTextBoxes.Any(tb => tb.Text == input))
            {
                textBox.Clear();
                textBox.Text = input;
            }
        }
        #endregion
        
    }
}
