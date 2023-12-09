using Common;
using SQLDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Word = Microsoft.Office.Interop.Word;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Diagnostics;
using SqlSugar;
using Model;

namespace StudentTerminal
{
    public partial class InTheTeam : Form
    {
        public InTheTeam()
        {
            InitializeComponent();
        }
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        int nonEmptyTextBoxCount;
        private void InTheTeam_Load(object sender, EventArgs e)
        {
            UserHelper.bioteam = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TL == UserHelper.bios.StudentName || it.TM1 == UserHelper.bios.StudentName || it.TM2 == UserHelper.bios.StudentName || it.TM3 == UserHelper.bios.StudentName).First();
            
            //判断是否有队伍
            if (UserHelper.bioteam!=null)
            {
                //加载过程需要看是不是队长，组员的界面不一样
                if (UserHelper.bioteam.TL == UserHelper.bios.StudentName)
                {
                    button3.Visible = true;
                    button4.Visible = true;
                    button4.Enabled = false;
                    button6.Visible = true;
                    button7.Visible = false;
                    textBox2.Text = UserHelper.bioteam.TM1;
                    textBox3.Text = UserHelper.bioteam.TM2;
                    textBox4.Text = UserHelper.bioteam.TM3;
                }
                else
                {
                    button3.Visible = false;
                    button4.Visible = false;
                    button6.Visible = false;
                    button7.Visible = true;
                    if(UserHelper.bioteam.TM1 == UserHelper.bios.StudentName)
                    {
                        textBox2.Text = UserHelper.bioteam.TL;
                        textBox3.Text = UserHelper.bioteam.TM2;
                        textBox4.Text = UserHelper.bioteam.TM3;
                    }
                    else if(UserHelper.bioteam.TM2 == UserHelper.bios.StudentName)
                    {
                        textBox2.Text = UserHelper.bioteam.TL;
                        textBox3.Text = UserHelper.bioteam.TM1;
                        textBox4.Text = UserHelper.bioteam.TM3;
                    }
                    else
                    {
                        textBox2.Text = UserHelper.bioteam.TL;
                        textBox3.Text = UserHelper.bioteam.TM1;
                        textBox4.Text = UserHelper.bioteam.TM2;
                    }
                }

                List<TextBox> textBoxes = new List<TextBox> { textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10 };
                foreach (var textBox in textBoxes)
                {
                    textBox.ReadOnly = true;
                }
                textBox10.Text = UserHelper.bioteam.YourTeam;
                textBox5.Text = UserHelper.bioteam.V1;
                textBox6.Text = UserHelper.bioteam.V2;
                textBox7.Text = UserHelper.bioteam.V3;
                textBox8.Text = UserHelper.bioteam.TopicName;
                textBox9.Text = UserHelper.bioteam.TopicIntroduction;
                textBox11.Text = UserHelper.bioteam.FileName;
                textBox12.Text = UserHelper.bioteam.FileDownloadPath;
            }
            else
            {
                MessageBox.Show("你尚未加入队伍，无法查看信息");
                panel1.Visible = false;
            }
            
            dataGridView.Visible = false;
            panel3.Visible = false;
            panel13.Location = new System.Drawing.Point(20, 135);

            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "所有学生信息", "学生学号搜索", "学生姓名搜索", "所有导师信息", "导师职工号搜索", "导师姓名搜索" });
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //选择搜索
            bool IsStuOrTea = true;
            var query1 = db.Queryable<DSS_3_8_BIOS>();
            var query2 = db.Queryable<DSS_3_8_BIOT>();
            if (comboBox.Text == "所有学生信息" || comboBox.Text == "学生学号搜索" || comboBox.Text == "学生姓名搜索")
            {
                IsStuOrTea = true;
                if (comboBox.Text == "学生学号搜索")
                {
                    query1 = query1.Where(it => it.Account == textBox1.Text.Trim() && it.Account != UserHelper.user.Account);
                }
                else if (comboBox.Text == "学生姓名搜索")
                {
                    query1 = query1.Where(it => it.StudentName == textBox1.Text.Trim() && it.Account != UserHelper.user.Account);
                }
                else if (comboBox.Text == "所有学生信息")
                {
                    query1 = query1.Where(it => it.Account != UserHelper.user.Account);
                }
            }
            else
            {
                IsStuOrTea = false;
                if (comboBox.Text == "导师职工号搜索")
                {
                    query2 = query2.Where(it => it.Account == textBox1.Text.Trim());
                }
                else if (comboBox.Text == "导师姓名搜索")
                {
                    query2 = query2.Where(it => it.TeacherName == textBox1.Text.Trim());
                }
                else if (comboBox.Text == "所有导师信息")
                {
                    ;
                }
            }
            if (IsStuOrTea)
            {
                dataGridView.DataSource = query1.Select(it => new {
                    学号 = it.Account,
                    学生姓名 = it.StudentName,
                    性别 = it.Sex,
                    学院 = it.Faculties,
                    专业 = it.Specialty,
                    年级 = it.Grade,
                    班级 = it.Class
                }).ToList();
            }
            else
            {
                dataGridView.DataSource = query2.Select(it => new {
                    职工号 = it.Account,
                    导师姓名 = it.TeacherName,
                    性别 = it.Sex,
                    院系 = it.Faculties,
                    专业方向 = it.ProfessionalDirection
                }).ToList();
            }

        }

        #region 文本框点击传入
        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox2, new TextBox[] { textBox3, textBox4 }, "学生姓名");
        }

        private void textBox3_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox3, new TextBox[] { textBox2, textBox4 }, "学生姓名");
        }

        private void textBox4_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox4, new TextBox[] { textBox2, textBox3 }, "学生姓名");
        }

        private void textBox5_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox5, new TextBox[] { textBox6, textBox7 }, "导师姓名");
        }

        private void textBox6_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox6, new TextBox[] { textBox5, textBox7 }, "导师姓名");
        }

        private void textBox7_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox_MouseDown(textBox7, new TextBox[] { textBox5, textBox6 }, "导师姓名");
        }
        private void textBox11_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox11.Text == "文件名") textBox11.Clear();
        }
        private void textBox12_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox12.Text == "文件下载路径") textBox12.Clear();
        }
        #endregion


        private void button3_Click(object sender, EventArgs e)
        {
            //队伍修改信息
            List<TextBox> textBoxes = new List<TextBox> { textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10 };

            foreach (var textBox in textBoxes)
            {
                textBox.ReadOnly = false;
            }
            button4.Enabled = true;
            button3.Enabled = false;
            dataGridView.Visible = true;
            panel3.Visible = true;
            panel13.Location = new System.Drawing.Point(380, 165);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //重新上传队伍数据
            button3.Enabled = true;
            button4.Enabled = false;
            dataGridView.Visible = false;
            panel3.Visible = false;
            panel13.Location = new System.Drawing.Point(20, 135);
            if (ValidateInput())
            {
                // 插入新的队伍信息
                //检查队员是否已经存在其他队伍中
                string[] textBoxes = { textBox2.Text, textBox3.Text, textBox4.Text }; 
                nonEmptyTextBoxCount = textBoxes.Count(text => !string.IsNullOrWhiteSpace(text));
                foreach (string text in textBoxes)
                {
                    if (!InTeam(text) && HasTeam(text))
                    {
                        MessageBox.Show(text + "同学已加入（或创建）队伍，无法加入你的队伍！");
                        return;
                    }
                }
                if (UpdateTeamInformation())
                {
                    MessageBox.Show("队伍信息修改成功！");
                }
                else
                {
                    MessageBox.Show("队伍信息修改失败！");
                }
            }
            else
            {
                MessageBox.Show("队伍名称/课题名称/课题简介不能为空！");
            }
        }
        

        private void button6_Click(object sender, EventArgs e)
        {
            //将当前队伍删除
            if (db.Deleteable<DSS_3_8_BIOTEAM>(it => it.TL == UserHelper.bios.StudentName).ExecuteCommand() > 0)
            {
                MessageBox.Show("队伍删除成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("队伍信息删除出错！");
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            //组员退出队伍
            var rasult = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TM1 == UserHelper.bios.StudentName || it.TM2 == UserHelper.bios.StudentName || it.TM3 == UserHelper.bios.StudentName).ToList();
            if (rasult[0].TM1== UserHelper.bios.StudentName)
            {
                DSS_3_8_BIOTEAM dSS_3_8_BIOTEAM = new DSS_3_8_BIOTEAM();
                dSS_3_8_BIOTEAM.TM1 = "暂无";
                db.Updateable(dSS_3_8_BIOTEAM).UpdateColumns(it => new { it.TM1 }).ExecuteCommand();
            }
            else if (rasult[0].TM2 == UserHelper.bios.StudentName)
            {
                DSS_3_8_BIOTEAM dSS_3_8_BIOTEAM = new DSS_3_8_BIOTEAM();
                dSS_3_8_BIOTEAM.TM2 = "暂无";
                db.Updateable(dSS_3_8_BIOTEAM).UpdateColumns(it => new { it.TM2 }).ExecuteCommand();
            }
            else if (rasult[0].TM3 == UserHelper.bios.StudentName)
            {
                DSS_3_8_BIOTEAM dSS_3_8_BIOTEAM = new DSS_3_8_BIOTEAM();
                dSS_3_8_BIOTEAM.TM3 = "暂无";
                db.Updateable(dSS_3_8_BIOTEAM).UpdateColumns(it => new { it.TM3 }).ExecuteCommand();
            }
            DSS_3_8_BIOS dSS_3_8_BIOS = new DSS_3_8_BIOS();
            dSS_3_8_BIOS.Duty = "暂无";
            int Update = db.Updateable(dSS_3_8_BIOS)
                        .Where(it => it.Account == UserHelper.bios.Account)
                        .UpdateColumns(it => new { it.Duty })
                        .ExecuteCommand();
            MessageBox.Show("你已退出队伍");
            this.Close();
        }
       
        private void TextBox_MouseDown(TextBox textBox, TextBox[] otherTextBoxes, string targetColumnName)
        {
            //所选信息是否为该类信息
            bool col = false;
            DataGridViewRow selectedRow;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name == targetColumnName)
                {
                    col = true;
                    break;
                }
            }

            if (col)
            {
                selectedRow = dataGridView.SelectedRows[0];
                string? input = selectedRow.Cells[targetColumnName].Value.ToString();

                if (!string.IsNullOrEmpty(input) && !otherTextBoxes.Any(tb => tb.Text == input))
                {
                    textBox.Clear();
                    textBox.Text = input;
                }
            }
            else
            {
                MessageBox.Show("不能选入！");
            }
        }

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(textBox10.Text) && !string.IsNullOrWhiteSpace(textBox8.Text) && !string.IsNullOrWhiteSpace(textBox9.Text);
        }

        private bool HasTeam(string currentUser)
        {
            var HasTeamQuery = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TL == currentUser || it.TM1 == currentUser || it.TM2 == currentUser || it.TM3 == currentUser).First();
            return HasTeamQuery != null;
        }

        private bool InTeam(string currentUser)
        {
            var InTeamQuery = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TL == currentUser || it.TM1 == currentUser || it.TM2 == currentUser || it.TM3 == currentUser).First();
            if (InTeamQuery.YourTeam == UserHelper.bioteam.YourTeam)  return true; 

            return false;
        }
        private bool UpdateTeamInformation()
        {
            DSS_3_8_BIOTEAM dSS_3_8_BIOTEAM = new DSS_3_8_BIOTEAM();
            dSS_3_8_BIOTEAM.YourTeam = textBox10.Text.Trim();
            dSS_3_8_BIOTEAM.Number = (nonEmptyTextBoxCount + 1).ToString();
            dSS_3_8_BIOTEAM.TL = UserHelper.bios.StudentName;
            dSS_3_8_BIOTEAM.TM1 = textBox2.Text.Trim();
            dSS_3_8_BIOTEAM.TM2 = textBox3.Text.Trim();
            dSS_3_8_BIOTEAM.TM3 = textBox4.Text.Trim();
            dSS_3_8_BIOTEAM.TopicName = textBox8.Text.Trim();
            dSS_3_8_BIOTEAM.TopicIntroduction = textBox9.Text.Trim();
            dSS_3_8_BIOTEAM.FileName = textBox11.Text.Trim();
            dSS_3_8_BIOTEAM.FileDownloadPath = textBox12.Text.Trim();
            dSS_3_8_BIOTEAM.V1 = textBox5.Text.Trim();
            dSS_3_8_BIOTEAM.V2 = textBox6.Text.Trim();
            dSS_3_8_BIOTEAM.V3 = textBox7.Text.Trim();

            List<string> teamMembers = new List<string> { textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim() };

            DSS_3_8_BIOS dSS_3_8_BIOS = new DSS_3_8_BIOS();

            foreach (var member in teamMembers)
            {
                if (!UserHelper.bioteam.TM1.Contains(member) && !UserHelper.bioteam.TM2.Contains(member) && !UserHelper.bioteam.TM3.Contains(member))
                {
                    dSS_3_8_BIOS.Duty = "暂无";
                    dSS_3_8_BIOS.YourTeam = "暂无";
                    db.Updateable(dSS_3_8_BIOS)
                        .Where(it => it.StudentName == member)
                        .UpdateColumns(it => new { it.Duty, it.YourTeam })
                        .ExecuteCommand();
                }
            }

            int update1 = db.Updateable(dSS_3_8_BIOS)
                .SetColumns(it => new DSS_3_8_BIOS { Duty = "队员", YourTeam = textBox10.Text.Trim() })
                .Where(it => teamMembers.Contains(it.StudentName))
                .ExecuteCommand();

            int update2 = db.Updateable(dSS_3_8_BIOTEAM)
                .Where(it => it.TL == UserHelper.bios.StudentName)
                .ExecuteCommand();

            return update2 > 0 && update1 > 0;
        

            //DSS_3_8_BIOTEAM dSS_3_8_BIOTEAM = new DSS_3_8_BIOTEAM();
            //dSS_3_8_BIOTEAM.YourTeam = textBox10.Text.Trim();
            //dSS_3_8_BIOTEAM.Number = (nonEmptyTextBoxCount + 1).ToString();
            //dSS_3_8_BIOTEAM.TL = UserHelper.bios.StudentName;
            //dSS_3_8_BIOTEAM.TM1 = textBox2.Text.Trim();
            //dSS_3_8_BIOTEAM.TM2 = textBox3.Text.Trim();
            //dSS_3_8_BIOTEAM.TM3 = textBox4.Text.Trim();
            //dSS_3_8_BIOTEAM.TopicName = textBox8.Text.Trim();
            //dSS_3_8_BIOTEAM.TopicIntroduction = textBox9.Text.Trim();
            //dSS_3_8_BIOTEAM.FileName = textBox11.Text.Trim();
            //dSS_3_8_BIOTEAM.FileDownloadPath = textBox12.Text.Trim();
            //dSS_3_8_BIOTEAM.V1 = textBox5.Text.Trim();
            //dSS_3_8_BIOTEAM.V2 = textBox6.Text.Trim();
            //dSS_3_8_BIOTEAM.V3 = textBox7.Text.Trim();

            ////判断原来的值是否还在
            //bool puttm1 = false;
            //bool puttm2 = false;
            //bool puttm3 = false;
            //if (UserHelper.bioteam.TM1 == textBox2.Text.Trim())
            //{
            //    puttm1 = true;
            //}
            //else if(UserHelper.bioteam.TM1 == textBox3.Text.Trim())
            //{
            //    puttm1 = true;
            //}
            //else if(UserHelper.bioteam.TM1 == textBox4.Text.Trim())
            //{
            //    puttm1 = true;
            //}
            //if (UserHelper.bioteam.TM2 == textBox2.Text.Trim())
            //{
            //    puttm2 = true;
            //}
            //else if (UserHelper.bioteam.TM2 == textBox3.Text.Trim())
            //{
            //    puttm2 = true;
            //}
            //else if (UserHelper.bioteam.TM2 == textBox4.Text.Trim())
            //{
            //    puttm2 = true;
            //}
            //if (UserHelper.bioteam.TM3 == textBox2.Text.Trim())
            //{
            //    puttm3 = true;
            //}
            //else if (UserHelper.bioteam.TM3 == textBox3.Text.Trim())
            //{
            //    puttm3 = true;
            //}
            //else if (UserHelper.bioteam.TM3 == textBox4.Text.Trim())
            //{
            //    puttm3 = true;
            //}

            //DSS_3_8_BIOS dSS_3_8_BIOS = new DSS_3_8_BIOS();
            //if (!puttm1)
            //{
            //    dSS_3_8_BIOS.Duty = "暂无";
            //    dSS_3_8_BIOS.YourTeam = "暂无";
            //    int Update1 = db.Updateable(dSS_3_8_BIOS)
            //                    .Where(it => it.StudentName == UserHelper.bioteam.TM1)
            //                    .UpdateColumns(it => new { it.Duty, it.YourTeam })
            //                    .ExecuteCommand();
            //}
            //if (!puttm2)
            //{
            //    dSS_3_8_BIOS.Duty = "暂无";
            //    dSS_3_8_BIOS.YourTeam = "暂无";
            //    int Update1 = db.Updateable(dSS_3_8_BIOS)
            //                    .Where(it => it.StudentName == UserHelper.bioteam.TM2)
            //                    .UpdateColumns(it => new { it.Duty, it.YourTeam })
            //                    .ExecuteCommand();
            //}
            //if (!puttm3)
            //{
            //    dSS_3_8_BIOS.Duty = "暂无";
            //    dSS_3_8_BIOS.YourTeam = "暂无";
            //    int Update1 = db.Updateable(dSS_3_8_BIOS)
            //                    .Where(it => it.StudentName == UserHelper.bioteam.TM3)
            //                    .UpdateColumns(it => new { it.Duty, it.YourTeam })
            //                    .ExecuteCommand();
            //}

            //dSS_3_8_BIOS.Duty = "队员";
            //int update1 = db.Updateable(dSS_3_8_BIOS)
            //    .Where(it => it.StudentName == textBox2.Text.Trim() || it.StudentName == textBox3.Text.Trim() || it.StudentName == textBox4.Text.Trim())
            //    .UpdateColumns(it => new { it.Duty })
            //    .ExecuteCommand();



            //int update2 = db.Updateable(dSS_3_8_BIOTEAM).Where(it => it.TL == UserHelper.bios.StudentName).ExecuteCommand();

            //return update1 > 0 && update2 > 0;
        }


    }
}
