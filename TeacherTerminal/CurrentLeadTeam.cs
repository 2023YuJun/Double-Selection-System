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
        private int currentPage = 1;
        private int totalPages = 1;
        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        static int teachoicesize = int.Parse(db.Queryable<DSS_3_8_ChoiceSetting>().First().TeaChoiceSize);
        Panel[] PTC = new Panel[teachoicesize];
        TextBox[] TBTC = new TextBox[teachoicesize];
        private void TeacherSelectionTeam_Load(object sender, EventArgs e)
        {
            var SITC = db.Queryable<DSS_3_8_Choice>().Where(it => it.Tag == UserHelper.biot.TeacherID.ToString() && it.ChoiceType == "CT").ToList();
            if (!(SITC.Count > 0))
            {
                panel16.Visible = false;
                MessageBox.Show("您还没有带领队伍，无法查看当前带领队伍");
            }
            else
            {
                LoadTeamInformation();
                LoadChoice();
                comboBox.Items.Clear();
                comboBox.Items.AddRange(new object[] { "所有队伍信息", "队伍名称搜索", "课题名称搜索", "成员名称搜索" });
                panel1.Visible = false;
                panel9.Visible = true;
                button4.Enabled = false;
                LoadDGV();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDGV();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //删除带领
            try
            {
                DialogResult result = MessageBox.Show("你确定要执行此操作吗？\n这将会导致失去带领的所有队伍！", "确认", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    ;
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
                db.Deleteable<DSS_3_8_Choice>().Where(it => it.Tag == UserHelper.biot.TeacherID.ToString()).ExecuteCommand();
                MessageBox.Show("已成功删除所带领队伍");
                panel16.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除带领队伍失败，错误原因：" + ex.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //修改带领
            LoadChoice();
            panel1.Visible = true;
            panel9.Visible = false;
            button4.Enabled = true;
            button3.Enabled = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //确认带领
            try
            {
                db.Deleteable<DSS_3_8_Choice>().Where(it => it.Tag == UserHelper.biot.TeacherID.ToString()).ExecuteCommand();
                DSS_3_8_Choice dSS_3_8_Choice = new DSS_3_8_Choice();
                bool TBTChasDuplicates = TBTC.Where(textBox => !string.IsNullOrWhiteSpace(textBox.Text))
                        .GroupBy(textBox => textBox.Text)
                        .Any(group => group.Count() > 1);
                if (TBTChasDuplicates)
                {
                    MessageBox.Show("队伍重复！");
                    return;
                }
                foreach (var textbox in TBTC)
                {
                    if (!string.IsNullOrWhiteSpace(textbox.Text))
                    {
                        dSS_3_8_Choice.ChoiceType = "CT";
                        dSS_3_8_Choice.ChoiceName = textbox.Text.Trim();
                        dSS_3_8_Choice.Tag = UserHelper.biot.TeacherID.ToString();
                        db.Insertable(dSS_3_8_Choice).ExecuteCommand();
                    }
                }
                MessageBox.Show("您已成功修改所带领队伍！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改带领队伍失败，错误原因：" + ex.Message);
                return;
            }
            button4.Enabled = false;
            button3.Enabled = true;
            panel1.Visible = false;
            panel9.Visible = true;

        }

        #region DGV分页
        private void button5_Click(object sender, EventArgs e)
        {
            currentPage = 1; // 跳转到第一页，更新 currentPage
            LoadDGV(currentPage);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--; // 上一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++; // 下一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button8_Click(object sender, EventArgs e)
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

        private void LoadDGV(int currentPage = 1)
        {
            var query = db.Queryable<DSS_3_8_Choice, DSS_3_8_BIOTEAM>((t1, t2) => t1.Tag == t2.TeamID.ToString());
            if (comboBox.Text == "队伍名称搜索")
            {
                query = query.Where((t1, t2) => t2.TeamName == textBox1.Text.Trim() && t1.ChoiceType != "TC");
            }
            else if (comboBox.Text == "课题名称搜索")
            {
                query = query.Where((t1, t2) => t2.TopicName == textBox1.Text.Trim() && t1.ChoiceType != "TC");
            }
            else if (comboBox.Text == "成员名称搜索")
            {
                query = query.Where((t1, t2) => t1.ChoiceName == textBox1.Text.Trim() && t1.ChoiceType != "TC");
            }
            else
            {
                query = query.Where("ChoiceType LIKE @ChoiceType", new { ChoiceType = "%TM%" });
            }
            // 执行分页查询
            int visibleRowCount = dataGridView.Height / dataGridView.RowTemplate.Height;
            int rowCountPerPage = visibleRowCount; // 每页显示的行数与可见行数一致
            int totalCount = query.Count();
            totalPages = (int)Math.Ceiling((double)totalCount / rowCountPerPage);
            currentPage = Math.Min(Math.Max(1, currentPage), totalPages);
            var result = query.Select((t1, t2) => new
            {
                队伍ID = t2.TeamID,
                队伍名称 = t2.TeamName,
                队内人数 = t2.Number,
                成员类型 = t1.ChoiceType,
                成员名称 = t1.ChoiceName,
                课题名 = t2.TopicName
            }).ToPageList(currentPage, rowCountPerPage);
            dataGridView.DataSource = result.ToList();
        }
        #endregion

        #region 创建选入组
        private void LoadChoice()
        {
            panel4.Controls.Clear();
            var CT = db.Queryable<DSS_3_8_Choice>().Where(it => it.Tag == UserHelper.biot.TeacherID.ToString() && it.ChoiceType == "CT").ToList();
            if (PTC.Any(panel => panel == null))
            {
                for (int i = teachoicesize; i > 0; i--)
                {
                    Panel panel = new Panel();
                    Label label = new Label();
                    TextBox textBox = new TextBox();
                    panel.Dock = DockStyle.Top;
                    panel.Size = new Size(250, 60);
                    panel.Name = "panel" + (i + 100).ToString();
                    label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    label.Font = new System.Drawing.Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
                    label.Text = "队伍" + i.ToString();
                    label.Name = "label" + (i + 100).ToString();
                    label.Location = new System.Drawing.Point(20, 3);
                    label.Size = new Size(80, 25);
                    textBox.BackColor = System.Drawing.Color.White;
                    textBox.BorderStyle = BorderStyle.None;
                    textBox.Font = new System.Drawing.Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
                    textBox.Name = "textBox" + (i + 100).ToString();
                    if (CT.Count >= i)
                    {
                        textBox.Text = CT[i - 1].ChoiceName;
                    }
                    textBox.Multiline = true;
                    textBox.ReadOnly = true;
                    textBox.Location = new Point(20, 33);
                    textBox.Size = new Size(200, 25);
                    textBox.Click += TextBox_Click_Team;
                    textBox.DoubleClick += TextBox_DoubleClick;
                    label.Tag = i - 1;
                    textBox.Tag = i - 1;
                    panel.Tag = i - 1;
                    panel.Controls.Add(label);
                    panel.Controls.Add(textBox);
                    TBTC[i - 1] = textBox;
                    PTC[i - 1] = panel;
                    panel4.Controls.Add(panel);
                }
            }
            else
            {
                for (int i = teachoicesize; i > 0; i--)
                {
                    panel4.Controls.Add(PTC[i - 1]);
                }
            }
        }
        private void TextBox_Click_Team(object sender, EventArgs e)
        {
            TextBox clickedTextBox = (TextBox)sender;
            int index = (int)clickedTextBox.Tag;

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
            string input = selectedRow.Cells["队伍名称"].Value.ToString();
            TBTC[index].Clear();
            TBTC[index].Text = input;

        }
        private void TextBox_DoubleClick(object sender, EventArgs e)
        {
            TextBox clickedTextBox = (TextBox)sender;
            int index = (int)clickedTextBox.Tag;
            TBTC[index].Clear();
        }
        #endregion

        #region 加载带领队伍信息
        private void LoadTeamInformation()
        {
            var teamname = db.Queryable<DSS_3_8_Choice>().Where(it => it.Tag == UserHelper.biot.TeacherID.ToString() && it.ChoiceType == "CT").ToList();

            for (int i = 0; i < teamname.Count; i++)
            {
                var bioteam = db.Queryable<DSS_3_8_BIOTEAM>()
                                .Where(it => it.TeamName == teamname[i].ChoiceName)
                                .First();
                var choice = db.Queryable<DSS_3_8_Choice>()
                                .Where(it => it.Tag == bioteam.TeamID.ToString())
                                .Where("ChoiceType LIKE @ChoiceType", new { ChoiceType = "%TM%" })
                                .ToList();

                Panel panel = new Panel();
                panel.Name = "panel" + (i + 100).ToString();
                panel.Location = new Point(0, 0);
                panel.Size = new Size(810, 200);
                panel.Dock = DockStyle.Top;

                Label label1 = new Label();
                label1.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
                label1.Location = new Point(5, 5);
                label1.Name = "label1" + (i + 100).ToString();
                label1.Size = new Size(80, 25);
                label1.Text = "队伍" + (i + 1).ToString();
                label1.TextAlign = ContentAlignment.MiddleLeft;

                Label label2 = new Label();
                label2.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
                label2.Location = new Point(5, 40);
                label2.Name = "label2" + (i + 100).ToString();
                label2.Size = new Size(65, 20);
                label2.Text = "队长：";
                label2.TextAlign = ContentAlignment.MiddleLeft;

                Label label3 = new Label();
                label3.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
                label3.Location = new Point(200, 40);
                label3.Name = "label3" + (i + 100).ToString();
                label3.Size = new Size(65, 20);
                label3.Text = "队员：";
                label3.TextAlign = ContentAlignment.MiddleLeft;

                Label label4 = new Label();
                label4.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
                label4.Location = new Point(5, 76);
                label4.Name = "labe4" + (i + 100).ToString();
                label4.Size = new Size(100, 20);
                label4.Text = "课题名称：";
                label4.TextAlign = ContentAlignment.MiddleLeft;

                Label label5 = new Label();
                label5.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
                label5.Location = new Point(5, 110);
                label5.Name = "label5" + (i + 100).ToString();
                label5.Size = new Size(100, 20);
                label5.Text = "课题简介：";
                label5.TextAlign = ContentAlignment.MiddleLeft;

                TextBox textBox1 = new TextBox();
                textBox1.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
                textBox1.Location = new Point(75, 34);
                textBox1.Multiline = true;
                textBox1.ReadOnly = true;
                textBox1.Name = "textBox1" + (i + 100).ToString();
                textBox1.Size = new Size(120, 28);
                textBox1.Text = choice.Where(it => it.ChoiceType == "TM*").First().ChoiceName;

                TextBox textBox2 = new TextBox();
                textBox2.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
                textBox2.Location = new Point(270, 34);
                textBox2.Multiline = true;
                textBox2.ReadOnly = true;
                textBox2.Name = "textBox2" + (i + 100).ToString();
                textBox2.Size = new Size(460, 28);
                string TM = "";
                foreach (var item in choice)
                {
                    if (item.ChoiceType == "TM")
                    {
                        TM += item.ChoiceName + "   ";
                    }
                }
                textBox2.Text = TM;

                TextBox textBox3 = new TextBox();
                textBox3.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
                textBox3.Location = new Point(110, 73);
                textBox3.Multiline = true;
                textBox3.ReadOnly = true;
                textBox3.Name = "textBox3" + (i + 100).ToString();
                textBox3.Size = new Size(700, 28);
                textBox3.Text = bioteam.TopicName;

                TextBox textBox4 = new TextBox();
                textBox4.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
                textBox4.Location = new Point(110, 107);
                textBox4.Multiline = true;
                textBox4.ReadOnly = true;
                textBox4.Name = "textBox4" + (i + 100).ToString();
                textBox4.Size = new Size(700, 90);
                textBox4.Text = bioteam.TopicIntroduction;

                panel.Controls.Add(label1);
                panel.Controls.Add(label2);
                panel.Controls.Add(label3);
                panel.Controls.Add(label4);
                panel.Controls.Add(label5);
                panel.Controls.Add(textBox1);
                panel.Controls.Add(textBox2);
                panel.Controls.Add(textBox3);
                panel.Controls.Add(textBox4);

                panel9.Controls.Add(panel);
            }
        }

        #endregion
    }
}
