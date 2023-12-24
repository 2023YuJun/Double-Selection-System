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
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.Drawing.Printing;

namespace StudentTerminal
{
    public partial class CreateTeam : Form
    {
        public CreateTeam()
        {
            InitializeComponent();
        }
        private int currentPage = 1;
        private int totalPages = 1;
        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        static int teampersonsize = int.Parse(db.Queryable<DSS_3_8_ChoiceSetting>().First().TeamPersonSize);
        static int teamchoicesize = int.Parse(db.Queryable<DSS_3_8_ChoiceSetting>().First().TeamChoiceSize);
        int nonEmptyTextBoxCount;
        bool IsStuOrTea = true;
        Panel[] PTP = new Panel[teampersonsize - 1];
        Panel[] PTC = new Panel[teamchoicesize];
        TextBox[] TBTP = new TextBox[teampersonsize - 1];
        TextBox[] TBTC = new TextBox[teamchoicesize];
        private void CreateTeam_Load(object sender, EventArgs e)
        {
            var userteam = db
               .Queryable<DSS_3_8_Choice, DSS_3_8_BIOTEAM>((t1, t2) => t1.Tag == t2.TeamID.ToString())
               .Where((t1, t2) => t1.ChoiceName == UserHelper.bios.StudentName).Select((t1, t2) => new
               {
                   Choice = t1,
                   BIOTEAM = t2
               })
               .ToList()
               .FirstOrDefault();
            //判断是否有队伍
            if (userteam == null)
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(new object[] { "所有学生信息", "学生学号搜索", "学生姓名搜索", "所有导师信息", "导师职工号搜索", "导师姓名搜索" });
                LoadDGV();
                button8_Click(sender, e);
                button7_Click(sender, e);
            }
            else
            {
                MessageBox.Show("你已创建（或加入）队伍，无法创建队伍");
                panel1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDGV();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //检查是否有重复名
            bool TBTPhasDuplicates = TBTP.Where(textBox => !string.IsNullOrWhiteSpace(textBox.Text)) // 过滤掉空白或空值
                        .GroupBy(textBox => textBox.Text)
                        .Any(group => group.Count() > 1);
            bool TBTChasDuplicates = TBTC.Where(textBox => !string.IsNullOrWhiteSpace(textBox.Text))
                        .GroupBy(textBox => textBox.Text)
                        .Any(group => group.Count() > 1);
            if (TBTPhasDuplicates)
            {
                MessageBox.Show("组员重复！");
                return;
            }
            if (TBTChasDuplicates)
            {
                MessageBox.Show("导师重复！");
                return;
            }
            // 检查用户是否已经创建或加入队伍
            if (HasTeam(UserHelper.bios.StudentName))
            {
                MessageBox.Show("你已创建（或加入）队伍，无法创建队伍！");
                return;
            }
            List<string> textBoxes = new List<string>();
            //检查队员是否已经存在其他队伍中
            foreach (TextBox textBox in TBTP)
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                    textBoxes.Add(textBox.Text);
            }
            nonEmptyTextBoxCount = textBoxes.Count();
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

        private void button7_Click(object sender, EventArgs e)
        {
            panel7.Controls.Clear();
            if (PTP.Any(panel => panel == null))
            {
                for (int i = teampersonsize - 1; i > 0; i--)
                {
                    Panel panel = new Panel();
                    Label label = new Label();
                    TextBox textBox = new TextBox();
                    panel.Dock = DockStyle.Top;
                    panel.Size = new Size(250, 30);
                    panel.Name = "panel" + (i + 100).ToString();
                    label.TextAlign = ContentAlignment.MiddleLeft;
                    label.Font = new System.Drawing.Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
                    label.Text = "队员" + i.ToString();
                    label.Name = "label" + (i + 100).ToString();
                    label.Location = new System.Drawing.Point(20, 3);
                    label.Size = new Size(80, 25);
                    textBox.BackColor = System.Drawing.Color.White;
                    textBox.BorderStyle = BorderStyle.None;
                    textBox.Font = new System.Drawing.Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
                    textBox.Name = "textBox" + (i + 100).ToString();
                    textBox.Multiline = true;
                    textBox.ReadOnly = true;
                    textBox.Location = new Point(100, 3);
                    textBox.Size = new Size(120, 25);
                    textBox.Click += TextBox_Click_Stu;
                    textBox.DoubleClick += TextBox_Click_Stu_DoubleClick;
                    label.Tag = i - 1;
                    textBox.Tag = i - 1;
                    panel.Tag = i - 1;
                    panel.Controls.Add(label);
                    panel.Controls.Add(textBox);
                    TBTP[i - 1] = textBox;
                    PTP[i - 1] = panel;
                    panel7.Controls.Add(panel);
                }
            }
            else
            {
                for (int i = teampersonsize - 1; i > 0; i--)
                {
                    panel7.Controls.Add(PTP[i - 1]);
                }
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            panel7.Controls.Clear();
            if (PTC.Any(panel => panel == null))
            {
                for (int i = teamchoicesize; i > 0; i--)
                {
                    Panel panel = new Panel();
                    Label label = new Label();
                    TextBox textBox = new TextBox();
                    panel.Dock = DockStyle.Top;
                    panel.Size = new Size(250, 30);
                    panel.Name = "panel" + (i + 200).ToString();
                    label.TextAlign = ContentAlignment.MiddleLeft;
                    label.Font = new System.Drawing.Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
                    label.Text = "导师" + i.ToString();
                    label.Name = "label" + (i + 200).ToString();
                    label.Location = new System.Drawing.Point(20, 3);
                    label.Size = new Size(80, 25);
                    textBox.BackColor = System.Drawing.Color.White;
                    textBox.BorderStyle = BorderStyle.None;
                    textBox.Font = new System.Drawing.Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
                    textBox.Name = "textBox" + (i + 200).ToString();
                    textBox.Multiline = true;
                    textBox.ReadOnly = true;
                    textBox.Location = new Point(100, 3);
                    textBox.Size = new Size(120, 25);
                    textBox.Click += TextBox_Click_Tea;
                    textBox.DoubleClick += TextBox_Click_Tea_DoubleClick;
                    label.Tag = i - 1;
                    textBox.Tag = i - 1;
                    panel.Tag = i - 1;
                    panel.Controls.Add(label);
                    panel.Controls.Add(textBox);
                    TBTC[i - 1] = textBox;
                    PTC[i - 1] = panel;
                    panel7.Controls.Add(panel);
                }
            }
            else
            {
                for (int i = teamchoicesize; i > 0; i--)
                {
                    panel7.Controls.Add(PTC[i - 1]);
                }
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TeamName == textBox3.Text).First() != null)
            {
                textBox3.Clear();
                MessageBox.Show("已有相同队伍名称，请重新命名");
            }
        }
        private void textBox6_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox6.Text == "文件名") textBox6.Clear();
        }
        private void textBox7_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox7.Text == "文件下载路径") textBox7.Clear();
        }

        private bool HasTeam(string currentUser)
        {
            var checkTeamQuery = db
                .Queryable<DSS_3_8_Choice, DSS_3_8_BIOTEAM>((t1, t2) => t1.Tag == t2.TeamID.ToString())
                .Where("ChoiceType LIKE @ChoiceType", new { ChoiceType = "%TM%" })
                .Where(t1 => t1.ChoiceName == currentUser)
                .Any();             // 使用 Any() 检查是否存在符合条件的项
            return checkTeamQuery;
        }

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox5.Text);
        }

        private bool InsertTeamInformation()
        {

            DSS_3_8_BIOTEAM dSS_3_8_BIOTEAM = new DSS_3_8_BIOTEAM();
            dSS_3_8_BIOTEAM.TeamName = textBox3.Text.Trim();
            dSS_3_8_BIOTEAM.Number = (nonEmptyTextBoxCount + 1).ToString();
            dSS_3_8_BIOTEAM.TopicName = textBox4.Text.Trim();
            dSS_3_8_BIOTEAM.TopicIntroduction = textBox5.Text.Trim();
            dSS_3_8_BIOTEAM.FileName = textBox6.Text.Trim();
            dSS_3_8_BIOTEAM.FileDownloadPath = textBox7.Text.Trim();
            int Insert = db.Insertable(dSS_3_8_BIOTEAM).ExecuteCommand();


            var tag = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TeamName == textBox3.Text.Trim()).First().TeamID;
            DSS_3_8_Choice dSS_3_8_Choice = new DSS_3_8_Choice();
            dSS_3_8_Choice.ChoiceType = "TM*";
            dSS_3_8_Choice.ChoiceName = UserHelper.bios.StudentName;
            dSS_3_8_Choice.Tag = tag.ToString();
            db.Insertable(dSS_3_8_Choice).ExecuteCommand();
            foreach (var textbox in TBTP)
            {
                dSS_3_8_Choice.ChoiceType = "TM";
                dSS_3_8_Choice.ChoiceName = textbox.Text.Trim();
                dSS_3_8_Choice.Tag = tag.ToString();
                db.Insertable(dSS_3_8_Choice).ExecuteCommand();
                db.Updateable<DSS_3_8_BIOS>()
                    .SetColumns(it => new DSS_3_8_BIOS { Duty = "队员", YourTeam = textBox3.Text.Trim() })
                    .Where(it => it.StudentName == textbox.Text)
                    .ExecuteCommand();
                
            }
            foreach (var textbox in TBTC)
            {
                dSS_3_8_Choice.ChoiceType = "TC";
                dSS_3_8_Choice.ChoiceName = textbox.Text.Trim();
                dSS_3_8_Choice.Tag = tag.ToString();
                db.Insertable(dSS_3_8_Choice).ExecuteCommand();
                
            }

            DSS_3_8_BIOS dSS_3_8_BIOS = new DSS_3_8_BIOS();
            dSS_3_8_BIOS.YourTeam = textBox3.Text.Trim();
            dSS_3_8_BIOS.Duty = "队长";
            int Update = db.Updateable(dSS_3_8_BIOS)
                .Where(it => it.StudentName == UserHelper.bios.StudentName)
                .UpdateColumns(it => new { it.Duty, it.YourTeam })
                .ExecuteCommand();
            return Insert > 0 && Update > 0;
        }

        #region DGV分页
        private void LoadDGV(int currentPage = 1)
        {
            // 按钮点击事件中的查询逻辑
            var query1 = db.Queryable<DSS_3_8_BIOS>();
            var query2 = db.Queryable<DSS_3_8_BIOT>();

            if (comboBox.Text == "所有学生信息" || comboBox.Text == "学生学号搜索" || comboBox.Text == "学生姓名搜索" || comboBox.Text == "")
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
                else
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

            // 执行分页查询
            int visibleRowCount = dataGridView.Height / dataGridView.RowTemplate.Height;
            int rowCountPerPage = visibleRowCount; // 每页显示的行数与可见行数一致
            int totalCount = IsStuOrTea ? query1.Count() : query2.Count();
            totalPages = (int)Math.Ceiling((double)totalCount / rowCountPerPage);
            currentPage = Math.Min(Math.Max(1, currentPage), totalPages);

            if (IsStuOrTea)
            {
                var result = query1.ToPageList(currentPage, rowCountPerPage);
                dataGridView.DataSource = result.Select(it => new
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
                var result = query2.ToPageList(currentPage, rowCountPerPage);
                dataGridView.DataSource = result.Select(it => new
                {
                    职工号 = it.Account,
                    导师姓名 = it.TeacherName,
                    性别 = it.Sex,
                    院系 = it.Faculties,
                    专业方向 = it.Specialty
                }).ToList();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentPage = 1; // 跳转到第一页，更新 currentPage
            LoadDGV(currentPage);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--; // 上一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++; // 下一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            currentPage = totalPages; // 跳转到最后一页，更新 currentPage
            LoadDGV(currentPage);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                int pageNumber;
                if (int.TryParse(textBox2.Text, out pageNumber))
                {
                    currentPage = Math.Max(1, Math.Min(totalPages, pageNumber)); // 更新 currentPage
                    LoadDGV(currentPage);
                }
                else
                {
                    MessageBox.Show("页数无效");
                }
            }
        }
        #endregion

        #region 选入逻辑
        private void TextBox_Click_Stu(object sender, EventArgs e)
        {
            TextBox clickedTextBox = (TextBox)sender;
            int index = (int)clickedTextBox.Tag;

            if (IsStuOrTea)
            {
                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
                string input = selectedRow.Cells["学生姓名"].Value.ToString();
                TBTP[index].Clear();
                TBTP[index].Text = input;
            }
            else
            {
                MessageBox.Show("不能选入！");
            }
        }
        private void TextBox_Click_Tea(object sender, EventArgs e)
        {
            TextBox clickedTextBox = (TextBox)sender;
            int index = (int)clickedTextBox.Tag;

            if (!IsStuOrTea)
            {
                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
                string input = selectedRow.Cells["导师姓名"].Value.ToString();
                TBTC[index].Clear();
                TBTC[index].Text = input;
            }
            else
            {
                MessageBox.Show("不能选入！");
            }
        }
        private void TextBox_Click_Stu_DoubleClick(object sender, EventArgs e)
        {
            TextBox clickedTextBox = (TextBox)sender;
            int index = (int)clickedTextBox.Tag;
            TBTP[index].Clear();
        }
        private void TextBox_Click_Tea_DoubleClick(object sender, EventArgs e)
        {
            TextBox clickedTextBox = (TextBox)sender;
            int index = (int)clickedTextBox.Tag;
            TBTC[index].Clear();
        }
        #endregion
    }
}
