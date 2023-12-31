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
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace StudentTerminal
{
    public partial class InTheTeam : Form
    {
        public InTheTeam()
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
        bool FirstLoad = false;
        Panel[] PTP = new Panel[teampersonsize - 1];
        Panel[] PTC = new Panel[teamchoicesize];
        TextBox[] TBTP = new TextBox[teampersonsize - 1];
        TextBox[] TBTC = new TextBox[teamchoicesize];

        private void InTheTeam_Load(object sender, EventArgs e)
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
            if (userteam != null)
            {
                var choice = userteam.Choice;
                var bioteam = userteam.BIOTEAM;
                panel3.Visible = false;
                dataGridView.Visible = false;
                panel4.Location = new System.Drawing.Point(20, 100);
                panel6.Visible = false;

                //加载过程需要看是不是队长，组员的界面不一样
                if (choice.ChoiceType == "TM*")
                {
                    button3.Enabled = false;
                    button5.Visible = false;
                }
                else
                {
                    button2.Visible = false;
                    button3.Visible = false;
                    button4.Visible = false;
                }
                FirstLoad = true;
                textBox3.Text = bioteam.TeamName;
                button11_Click(sender, e);
                button10_Click(sender, e);
                foreach (var item in TBTP)
                {
                    item.Enabled = false;
                }
                foreach (var item in TBTC)
                {
                    item.Enabled = false;
                }
                textBox4.Text = bioteam.TopicName;
                textBox5.Text = bioteam.TopicIntroduction;
                textBox6.Text = bioteam.FileName;
                textBox7.Text = bioteam.FileDownloadPath;

                List<TextBox> textBoxes = new List<TextBox> { textBox3, textBox4, textBox5, textBox6, textBox7 };
                foreach (var textBox in textBoxes)
                {
                    textBox.ReadOnly = true;
                }
            }
            else
            {
                MessageBox.Show("你尚未加入队伍，无法查看信息");
                panel1.Visible = false;
            }

            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "所有学生信息", "学生学号搜索", "学生姓名搜索", "所有导师信息", "导师职工号搜索", "导师姓名搜索" });
        }
        private void button1_Click(object sender, EventArgs e)
        {
            LoadDGV();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //队伍修改信息
            List<TextBox> textBoxes = new List<TextBox> { textBox3, textBox4, textBox5, textBox6, textBox7 };

            foreach (var textBox in textBoxes)
            {
                textBox.ReadOnly = false;
            }
            foreach (var item in TBTP)
            {
                item.Enabled = true;
            }
            foreach (var item in TBTC)
            {
                item.Enabled = true;
            }
            button3.Enabled = true;
            button2.Enabled = false;
            dataGridView.Visible = true;
            panel3.Visible = true;
            panel4.Location = new System.Drawing.Point(568, 115);
            panel6.Visible = true;
            LoadDGV();
        }
        private void button3_Click(object sender, EventArgs e)
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

            //重新上传队伍数据
            if (ValidateInput())
            {
                // 插入新的队伍信息
                //检查队员是否已经存在其他队伍中
                List<string> textBoxes = new List<string>();
                foreach (TextBox textBox in TBTP)
                {
                    if (!string.IsNullOrWhiteSpace(textBox.Text))
                        textBoxes.Add(textBox.Text);
                }
                nonEmptyTextBoxCount = textBoxes.Count();
                foreach (string text in textBoxes)
                {
                    if (HasOtherTeam(text))
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
                    return;
                }
            }
            else
            {
                MessageBox.Show("队伍名称/课题名称/课题简介不能为空！");
                return;
            }
            List<TextBox> BaseTextBoxes = new List<TextBox> { textBox3, textBox4, textBox5, textBox6, textBox7 };

            foreach (var textBox in BaseTextBoxes)
            {
                textBox.ReadOnly = true;
            }
            foreach (var item in TBTP)
            {
                item.Enabled = false;
            }
            foreach (var item in TBTC)
            {
                item.Enabled = false;
            }
            button3.Enabled = false;
            button2.Enabled = true;
            dataGridView.Visible = false;
            panel3.Visible = false;
            panel4.Location = new System.Drawing.Point(20, 100);
            panel6.Visible = false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //将当前队伍删除
            DialogResult result = MessageBox.Show("你确定要执行此操作吗？\n这将会导致所有队员失去队伍！", "确认", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                ;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            try
            {
                var userteam = db
                       .Queryable<DSS_3_8_Choice, DSS_3_8_BIOTEAM>((t1, t2) => t1.Tag == t2.TeamID.ToString())
                       .Where((t1, t2) => t1.ChoiceName == UserHelper.bios.StudentName).Select((t1, t2) => new
                       {
                           Choice = t1,
                           BIOTEAM = t2
                       })
                       .First();
                var choice = userteam.Choice;
                var bioteam = userteam.BIOTEAM;
                var TM = db.Queryable<DSS_3_8_Choice>()
                .Where(it => it.Tag == choice.Tag)
                .Where("ChoiceType LIKE @ChoiceType", new { ChoiceType = "%TM%" })
                .ToList();
                var delete1 = db.Deleteable<DSS_3_8_Choice>().Where(it => it.Tag == choice.Tag).ExecuteCommand();
                var delete2 = db.Deleteable<DSS_3_8_BIOTEAM>().Where(it => it.TeamID == bioteam.TeamID).ExecuteCommand();
                foreach (var choicename in TM.Select(it => it.ChoiceName))
                {
                    db.Updateable<DSS_3_8_BIOS>()
                    .SetColumns(it => new DSS_3_8_BIOS { Duty = "" })
                    .Where(it => it.StudentName == choicename)
                    .ExecuteCommand();
                }
                MessageBox.Show("队伍删除成功");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("队伍删除失败，错误原因：" + ex.Message);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //组员退出队伍
            try
            {
                var userteam = db
               .Queryable<DSS_3_8_Choice, DSS_3_8_BIOTEAM>((t1, t2) => t1.Tag == t2.TeamID.ToString())
               .Where((t1, t2) => t1.ChoiceName == UserHelper.bios.StudentName).Select((t1, t2) => new
               {
                   Choice = t1,
                   BIOTEAM = t2
               })
               .First();
                var choice = userteam.Choice;
                var bioteam = userteam.BIOTEAM;
                db.Deleteable<DSS_3_8_Choice>()
                    .Where(it => it.ChoiceName == UserHelper.bios.StudentName)
                    .ExecuteCommand();
                db.Updateable<DSS_3_8_BIOTEAM>()
                    .SetColumns(it => new DSS_3_8_BIOTEAM { Number = (int.Parse(bioteam.Number) - 1).ToString() })
                    .Where(it => it.TeamID == bioteam.TeamID)
                    .ExecuteCommand();
                db.Updateable<DSS_3_8_BIOS>()
                    .SetColumns(it => new DSS_3_8_BIOS { Duty = "" })
                    .Where(it => it.Account == UserHelper.user.Account)
                    .ExecuteCommand();
                MessageBox.Show("你已退出队伍");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("退出队伍失败，错误原因：" + ex.Message);
            }

        }
        private void button10_Click(object sender, EventArgs e)
        {
            panel7.Controls.Clear();
            var Tag = db.Queryable<DSS_3_8_Choice>().Where(it => it.ChoiceName == UserHelper.bios.StudentName).First().Tag;
            var TM = db.Queryable<DSS_3_8_Choice>()
                .Where(it => it.Tag == Tag)
                .Where("ChoiceType LIKE @ChoiceType", new { ChoiceType = "%TM%" })
                .ToList();
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
                    if (TM.Count > i && TM[i].ChoiceType != "TM*")
                    {
                        textBox.Text = TM[i].ChoiceName;
                    }
                    textBox.Multiline = true;
                    textBox.ReadOnly = true;
                    textBox.Location = new System.Drawing.Point(100, 3);
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
        private void button11_Click(object sender, EventArgs e)
        {
            panel7.Controls.Clear();
            var Tag = db.Queryable<DSS_3_8_Choice>().Where(it => it.ChoiceName == UserHelper.bios.StudentName).First().Tag;
            var TC = db.Queryable<DSS_3_8_Choice>()
                .Where(it => it.Tag == Tag && it.ChoiceType == "TC")
                .ToList();
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
                    if (TC.Count >= i)
                    {
                        textBox.Text = TC[i - 1].ChoiceName;
                    }
                    textBox.Multiline = true;
                    textBox.ReadOnly = true;
                    textBox.Location = new System.Drawing.Point(100, 3);
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
            if (FirstLoad == false && db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TeamName == textBox3.Text).First() != null)
            {
                textBox3.Clear();
                MessageBox.Show("已有相同队伍名称，请重新命名");
            }
            else
            {
                FirstLoad = false;
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
        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox5.Text);
        }

        private bool HasOtherTeam(string currentUser)
        {
            var Tag = db.Queryable<DSS_3_8_Choice>().Where(it => it.ChoiceName == UserHelper.bios.StudentName).First().Tag;
            var checkTeamQuery = db
                .Queryable<DSS_3_8_Choice, DSS_3_8_BIOTEAM>((t1, t2) => t1.Tag == t2.TeamID.ToString())
                .Where("ChoiceType LIKE @ChoiceType", new { ChoiceType = "%TM%" })
                .Where(t1 => t1.ChoiceName == currentUser && t1.Tag != Tag)
                .Any();                         
            return checkTeamQuery;
        }

        private bool UpdateTeamInformation()
        {
            DSS_3_8_BIOTEAM dSS_3_8_BIOTEAM = new DSS_3_8_BIOTEAM();
            dSS_3_8_BIOTEAM.TeamName = textBox3.Text.Trim();
            dSS_3_8_BIOTEAM.Number = (nonEmptyTextBoxCount + 1).ToString();
            dSS_3_8_BIOTEAM.TopicName = textBox4.Text.Trim();
            dSS_3_8_BIOTEAM.TopicIntroduction = textBox5.Text.Trim();
            dSS_3_8_BIOTEAM.FileName = textBox6.Text.Trim();
            dSS_3_8_BIOTEAM.FileDownloadPath = textBox7.Text.Trim();
            int updatebioteam = db.Updateable(dSS_3_8_BIOTEAM).Where(it => it.TeamName == textBox3.Text.Trim()).ExecuteCommand();

            var Tag = db.Queryable<DSS_3_8_Choice>().Where(it => it.ChoiceName == UserHelper.bios.StudentName).First().Tag;
            var oldTM = db.Queryable<DSS_3_8_Choice>().Where(it => it.Tag == Tag && it.ChoiceType == "TM").ToList();
            DSS_3_8_Choice dSS_3_8_Choice = new DSS_3_8_Choice();
            int update1 = 0, insert1 = 0, insert2 = 0;
            //更改不同的职位
            foreach (var choice in oldTM)
            {
                bool foundMatch = false;
                foreach (TextBox textBox in TBTP)
                {
                    if (textBox != null && choice.ChoiceName == textBox.Text)
                    {
                        foundMatch = true;
                        break;
                    }
                }
                if (!foundMatch)
                {
                    // 找到不同的 ChoiceName 在 BIOS 表中进行修改
                    var biosEntity = db.Queryable<DSS_3_8_BIOS>().Where(it => it.StudentName == choice.ChoiceName).First();
                    if (biosEntity != null)
                    {
                        update1 = db.Updateable<DSS_3_8_BIOS>()
                                    .SetColumns(it => new DSS_3_8_BIOS { Duty = "" })
                                    .Where(it => it.StudentName == choice.ChoiceName)
                                    .ExecuteCommand();
                    }
                }
                else
                {
                    update1 = 1;
                }
            }

            //删除重新插入
            var deleteold = db.Deleteable<DSS_3_8_Choice>()
                .Where(it => it.Tag == Tag && it.ChoiceName != UserHelper.bios.StudentName)
                .ExecuteCommand();
            foreach (var textbox in TBTP)
            {
                dSS_3_8_Choice.ChoiceType = "TM";
                dSS_3_8_Choice.ChoiceName = textbox.Text.Trim();
                dSS_3_8_Choice.Tag = Tag;
                insert1 = db.Insertable(dSS_3_8_Choice).ExecuteCommand();
                db.Updateable<DSS_3_8_BIOS>()
                            .SetColumns(it => new DSS_3_8_BIOS { Duty = "队员", YourTeam = textBox3.Text.Trim() })
                            .Where(it => it.StudentName == textbox.Text)
                            .ExecuteCommand();
                
            }
            foreach (var textbox in TBTC)
            {
                dSS_3_8_Choice.ChoiceType = "TC";
                dSS_3_8_Choice.ChoiceName = textbox.Text.Trim();
                dSS_3_8_Choice.Tag = Tag;
                insert2 = db.Insertable(dSS_3_8_Choice).ExecuteCommand();
                
            }

            return updatebioteam > 0 && update1 > 0  && insert1 > 0 && insert2 > 0;

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

        private void button6_Click(object sender, EventArgs e)
        {
            currentPage = 1; // 跳转到第一页，更新 currentPage
            LoadDGV(currentPage);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--; // 上一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++; // 下一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button9_Click(object sender, EventArgs e)
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
                    MessageBox.Show("请输入有效的页数！");
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
