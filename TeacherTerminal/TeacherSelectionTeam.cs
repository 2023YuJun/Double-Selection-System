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
using static System.Net.WebRequestMethods;

namespace TeacherTerminal
{
    public partial class TeacherSelectionTeam : Form
    {
        public TeacherSelectionTeam()
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
            LoadChoice();
            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "所有队伍信息", "队伍名称搜索", "课题名称搜索", "成员名称搜索" });
            LoadDGV();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDGV();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DSS_3_8_Choice dSS_3_8_Choice = new DSS_3_8_Choice();
                bool TBTChasDuplicates = TBTC.Where(textBox => !string.IsNullOrWhiteSpace(textBox.Text))
                        .GroupBy(textBox => textBox.Text)
                        .Any(group => group.Count() > 1);
                if(TBTChasDuplicates )
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
                MessageBox.Show("已成功带领队伍");
            }
            catch (Exception ex)
            {
                MessageBox.Show("队伍带领失败，错误原因：" + ex.Message);
            }

        }

        #region DGV分页
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
            var result = query.Select((t1,t2) => new
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
                    textBox.Multiline = true;
                    textBox.ReadOnly = true;
                    textBox.Location = new Point(20, 33);
                    textBox.Size = new Size(200, 25);
                    textBox.Click += TextBox_Click_Team;
                    textBox.DoubleClick += TextBox_DoubleClick;
                    label.Tag = i-1;
                    textBox.Tag = i-1;
                    panel.Tag = i-1;
                    panel.Controls.Add(label);
                    panel.Controls.Add(textBox);
                    TBTC[i-1] = textBox;
                    PTC[i-1] = panel;
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
    }
}
