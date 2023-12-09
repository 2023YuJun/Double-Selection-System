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

namespace TeacherTerminal
{
    public partial class CurrentLeadTeam : Form
    {
        public CurrentLeadTeam()
        {
            InitializeComponent();
        }
        bool isnulldgv = true;
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void TeacherSelectionTeam_Load(object sender, EventArgs e)
        {
            var SITC = db.Queryable<DSS_3_8_TC>().Where(it => it.Account == UserHelper.user.Account).ToList();
            if (!(SITC.Count > 0))
            {
                panel16.Visible = false;
                MessageBox.Show("您还没有带领队伍，无法查看当前带领队伍");
            }
            else
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(new object[] { "所有队伍信息", "队伍名称搜索", "课题名称搜索", "学生姓名搜索" });
                panel1.Visible = false;
                panel9.Visible = true;
                button4.Enabled = false;
                var teams = new string[] { SITC[0].T1, SITC[0].T2, SITC[0].T3, SITC[0].T4, SITC[0].T5 };
                var StList = new List<List<DSS_3_8_BIOTEAM>>();
                foreach (var team in teams)
                {
                    var St = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.YourTeam == team).ToList();
                    StList.Add(St);
                }

                var textBoxes = new TextBox[] {
                        textBox7, textBox8, textBox9, textBox10, textBox11, textBox12,
                        textBox13, textBox14, textBox15, textBox16, textBox17, textBox18,
                        textBox19, textBox20, textBox21, textBox22, textBox23, textBox24,
                        textBox25, textBox26, textBox27, textBox28, textBox29, textBox30,
                        textBox31, textBox32, textBox33, textBox34, textBox35, textBox36
                };
                foreach (var textBox in textBoxes)
                {
                    textBox.ReadOnly = true;
                }
                foreach (var St in StList)
                {
                    var index = StList.IndexOf(St);
                    if (St.Count != 0)
                    {
                        textBoxes[index * 6].Text = St[0].TopicIntroduction;
                        textBoxes[index * 6 + 1].Text = St[0].TopicName;
                        textBoxes[index * 6 + 2].Text = St[0].TM3;
                        textBoxes[index * 6 + 3].Text = St[0].TM2;
                        textBoxes[index * 6 + 4].Text = St[0].TM1;
                        textBoxes[index * 6 + 5].Text = St[0].TL;
                    }
                }
            }


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
            dataGridView1.DataSource = query.Select(it => new
            {
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

            try
            {
                db.Deleteable<DSS_3_8_TC>(it => it.Account == UserHelper.user.Account).ExecuteCommand();
                MessageBox.Show("已成功删除所带领队伍");
                panel16.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel9.Visible = false;
            button4.Enabled = true;
            button3.Enabled = false;

            var TC = db.Queryable<DSS_3_8_TC>().Where(it => it.Account == UserHelper.user.Account).ToList();
            textBox2.Text = TC[0].T1;
            textBox3.Text = TC[0].T2;
            textBox4.Text = TC[0].T3;
            textBox5.Text = TC[0].T4;
            textBox6.Text = TC[0].T5;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            DSS_3_8_TC dSS_3_8_TC = new DSS_3_8_TC();
            dSS_3_8_TC.Account = UserHelper.user.Account;
            dSS_3_8_TC.TeacherName = UserHelper.biot.TeacherName;
            dSS_3_8_TC.T1 = textBox2.Text;
            dSS_3_8_TC.T2 = textBox3.Text;
            dSS_3_8_TC.T3 = textBox4.Text;
            dSS_3_8_TC.T4 = textBox5.Text;
            dSS_3_8_TC.T5 = textBox6.Text;
            db.Updateable(dSS_3_8_TC).Where(it => it.Account == UserHelper.user.Account).ExecuteCommand();
            MessageBox.Show("您已成功修改所带领队伍！");

            button4.Enabled = false;
            button3.Enabled = true;
            panel1.Visible = false;
            panel9.Visible = true;

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
