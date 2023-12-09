using SQLDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using Model;
using SqlSugar;

namespace StudentTerminal
{
    public partial class CreateTeam : Form
    {
        public CreateTeam()
        {
            InitializeComponent();
        }
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        int nonEmptyTextBoxCount;

        private void CreateTeam_Load(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "所有学生信息", "学生学号搜索", "学生姓名搜索", "所有导师信息", "导师职工号搜索", "导师姓名搜索" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                dataGridView.DataSource = query1.Select(it => new
                {
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
                dataGridView.DataSource = query2.Select(it => new
                {
                    职工号 = it.Account,
                    导师姓名 = it.TeacherName,
                    性别 = it.Sex,
                    院系 = it.Faculties,
                    专业方向 = it.Specialty
                }).ToList();
            }

            #region 优化前
            //string cmd = string.Empty;
            //SqlParameter[] parameters = null;

            //if (comboBox.Text == "所有学生信息")
            //{
            //    cmd = "select Account as '学号', StudentName as '学生姓名', Sex as '性别', Faculties as '学院', Specialty as '专业',Grade as '年级',Class as '班级' from [DSS_3_8_BIOS] ";
            //}
            //else if (comboBox.Text == "所有导师信息")
            //{
            //    cmd = "select Account as '职工号', TeacherName as '导师姓名', Sex as '性别', Faculties as '院系',ProfessionalDirection as '专业方向' from [DSS_3_8_BIOT] ";
            //}
            //else if (comboBox.Text == "学生学号搜索")
            //{
            //    cmd = "select Account as '学号', StudentName as '学生姓名', Sex as '性别', Faculties as '学院', Specialty as '专业',Grade as '年级',Class as '班级' from [DSS_3_8_BIOS] where Account=@Account";
            //    parameters = new SqlParameter[]
            //    {
            //        new SqlParameter("@Account", SqlDbType.NVarChar, 50)
            //        {
            //            Value = textBox1.Text.Trim()
            //        }
            //    };
            //}
            //else if (comboBox.Text == "学生姓名搜索")
            //{
            //    cmd = "select Account as '学号', StudentName as '学生姓名', Sex as '性别', Faculties as '学院', Specialty as '专业',Grade as '年级',Class as '班级' from [DSS_3_8_BIOS] where StudentName=@StudentName";
            //    parameters = new SqlParameter[]
            //    {
            //        new SqlParameter("@StudentName", SqlDbType.NVarChar, 50)
            //        {
            //            Value = textBox1.Text.Trim()
            //        }
            //    };
            //}
            //else if (comboBox.Text == "导师职工号搜索")
            //{
            //    cmd = "select Account as '职工号', TeacherName as '导师姓名', Sex as '性别', Faculties as '院系',ProfessionalDirection as '专业方向' from [DSS_3_8_BIOT] where Account=@Account";
            //    parameters = new SqlParameter[]
            //    {
            //        new SqlParameter("@Account", SqlDbType.NVarChar, 50)
            //        {
            //            Value = textBox1.Text.Trim()
            //        }
            //    };
            //}
            //else if (comboBox.Text == "导师姓名搜索")
            //{
            //    cmd = "select Account as '职工号', TeacherName as '导师姓名', Sex as '性别', Faculties as '院系',ProfessionalDirection as '专业方向' from [DSS_3_8_BIOT] where TeacherName=@TeacherName";
            //    parameters = new SqlParameter[]
            //    {
            //        new SqlParameter("@TeacherName", SqlDbType.NVarChar, 50)
            //        {
            //            Value = textBox1.Text.Trim()
            //        }
            //    };
            //}

            //if (!string.IsNullOrEmpty(cmd))
            //{
            //    System.Data.DataTable dt = SqlDbHelper.ExecuteDataTable(cmd, CommandType.Text, parameters);
            //    //去除自己的信息
            //    if (comboBox.Text == "所有学生信息" || comboBox.Text == "学生学号搜索" || comboBox.Text == "学生姓名搜索")
            //    {
            //        for (int i = dt.Rows.Count - 1; i >= 0; i--)
            //        {
            //            if (dt.Rows[i]["学生姓名"].ToString() == UserHelper.bios.StudentName)
            //            {
            //                dt.Rows.RemoveAt(i);
            //            }
            //        }
            //    }
            //    dataGridView.DataSource = dt;
            //}
            #endregion

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

            // 检查用户是否已经创建或加入队伍
            if (HasTeam(UserHelper.bios.StudentName))
            {
                MessageBox.Show("你已创建（或加入）队伍，无法创建队伍！");
                return;
            }
            //检查队员是否已经存在其他队伍中
            string[] textBoxes = { textBox2.Text, textBox3.Text, textBox4.Text }; // 假设你要检查的文本框存储在数组中
            nonEmptyTextBoxCount = textBoxes.Count(text => !string.IsNullOrWhiteSpace(text));
            foreach (string text in textBoxes)
            {
                if (HasTeam(text))
                {
                    MessageBox.Show(text + "同学已加入（或创建）队伍，无法加入你的队伍！");
                    return;
                }
            }
            // 验证输入是否有效
            if (ValidateInput())
            {
                // 插入新的队伍信息

                if (InsertTeamInformation())
                {
                    MessageBox.Show("队伍创建成功！");
                }
                else
                {
                    MessageBox.Show("队伍创建失败！");
                }
            }
            else
            {
                MessageBox.Show("队伍名称/课题名称/课题简介不能为空！");
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TeamName == textBox10.Text).First() != null)
            {
                textBox10.Clear();
                MessageBox.Show("已有相同队伍名称，请重新命名");
            }
        }
        private void TextBox_MouseDown(TextBox textBox, TextBox[] otherTextBoxes, string targetColumnName)
        {
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

        private bool HasTeam(string currentUser)
        {
            var checkTeamQuery = db
                .Queryable<DSS_3_8_Choice, DSS_3_8_BIOTEAM>((t1, t2) => t1.Tag == t2.TeamID.ToString())
                .Where("ChoiceType LIKE @ChoiceType", new { ChoiceType = "%TM%" })
                .Where(it => it.ChoiceName==currentUser)
                .ToList();
                
            return checkTeamQuery != null;
            #region 优化前
            //string checkTeamQuery = "SELECT COUNT(*) FROM [DSS_3_8_BIOTEAM] WHERE TL = @CurrentUser OR TM1 = @CurrentUser OR TM2 = @CurrentUser OR TM3 = @CurrentUser";
            //SqlParameter[] currentUserParam = { new SqlParameter("@CurrentUser", SqlDbType.NVarChar, 50) };
            //currentUserParam[0].Value = currentUser;

            //int teamCount = (int)SqlDbHelper.ExecuteScalar(checkTeamQuery, CommandType.Text, currentUserParam);

            //return teamCount > 0;
            #endregion
        }

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(textBox10.Text) && !string.IsNullOrWhiteSpace(textBox8.Text) && !string.IsNullOrWhiteSpace(textBox9.Text);
        }

        private bool InsertTeamInformation()
        {

            DSS_3_8_BIOTEAM dSS_3_8_BIOTEAM = new DSS_3_8_BIOTEAM();
            dSS_3_8_BIOTEAM.TeamName = textBox10.Text.Trim();
            dSS_3_8_BIOTEAM.Number = (nonEmptyTextBoxCount + 1).ToString();
            dSS_3_8_BIOTEAM.TopicName = textBox8.Text.Trim();
            dSS_3_8_BIOTEAM.TopicIntroduction = textBox9.Text.Trim();
            dSS_3_8_BIOTEAM.FileName = textBox11.Text.Trim();
            dSS_3_8_BIOTEAM.FileDownloadPath = textBox12.Text.Trim();
            int Insert = db.Insertable(dSS_3_8_BIOTEAM).ExecuteCommand();

            DSS_3_8_BIOS dSS_3_8_BIOS = new DSS_3_8_BIOS();
            dSS_3_8_BIOS.Duty = "队员";
            dSS_3_8_BIOS.YourTeam = textBox10.Text.Trim();
            int Update1 = db.Updateable(dSS_3_8_BIOS)
                .Where(it => it.StudentName == textBox2.Text.Trim() || it.StudentName == textBox3.Text.Trim() || it.StudentName == textBox4.Text.Trim())
                .UpdateColumns(it => new { it.Duty, it.YourTeam })
                .ExecuteCommand();
            dSS_3_8_BIOS.Duty = "队长";
            int Update2 = db.Updateable(dSS_3_8_BIOS)
                .Where(it => it.StudentName == UserHelper.bios.StudentName)
                .UpdateColumns(it => new { it.Duty, it.YourTeam })
                .ExecuteCommand();
            return Insert > 0 && Update1 > 0 && Update2 > 0;
            #region 优化前
            //string TeaminsertQuery = "INSERT INTO [DSS_3_8_BIOTEAM] (YourTeam, Number, TL, TM1, TM2, TM3, TopicName, TopicIntroduction, FileName, FileDownloadPath, V1, V2, V3) " +
            //                            "VALUES (@YourTeam, @Number, @TL, @TM1, @TM2, @TM3, @TopicName, @TopicIntroduction, @FileName, @FileDownloadPath, @V1, @V2, @V3)";

            //SqlParameter[] parameters =
            //{
            //    new SqlParameter("@YourTeam", SqlDbType.NVarChar, 50),
            //    new SqlParameter("@Number", SqlDbType.NVarChar, 50),
            //    new SqlParameter("@TL", SqlDbType.NVarChar, 50),
            //    new SqlParameter("@TM1", SqlDbType.NVarChar, 50),
            //    new SqlParameter("@TM2", SqlDbType.NVarChar, 50),
            //    new SqlParameter("@TM3", SqlDbType.NVarChar, 50),
            //    new SqlParameter("@TopicName", SqlDbType.NVarChar, -1),
            //    new SqlParameter("@TopicIntroduction", SqlDbType.NVarChar, -1),
            //    new SqlParameter("@FileName", SqlDbType.NVarChar, -1),
            //    new SqlParameter("@FileDownloadPath", SqlDbType.NVarChar, -1),
            //    new SqlParameter("@V1", SqlDbType.NVarChar, 50),
            //    new SqlParameter("@V2", SqlDbType.NVarChar, 50),
            //    new SqlParameter("@V3", SqlDbType.NVarChar, 50),
            //};

            //parameters[0].Value = textBox10.Text.Trim();
            //parameters[1].Value = nonEmptyTextBoxCount + 1;
            //parameters[2].Value = UserHelper.bios.StudentName;
            //parameters[3].Value = textBox2.Text.Trim();
            //parameters[4].Value = textBox3.Text.Trim();
            //parameters[5].Value = textBox4.Text.Trim();
            //parameters[6].Value = textBox8.Text.Trim();
            //parameters[7].Value = textBox9.Text.Trim();
            //parameters[8].Value = textBox11.Text.Trim();
            //parameters[9].Value = textBox12.Text.Trim();
            //parameters[10].Value = textBox5.Text.Trim();
            //parameters[11].Value = textBox6.Text.Trim();
            //parameters[12].Value = textBox7.Text.Trim();

            //return SqlDbHelper.ExecuteNonQuery(TeaminsertQuery, CommandType.Text, parameters) > 0;
            #endregion
        }

    }
}
