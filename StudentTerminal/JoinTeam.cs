using Common;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Vml.Office;
using Model;
using SQLDAL;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentTerminal
{
    public partial class JoinTeam : Form
    {
        public JoinTeam()
        {
            InitializeComponent();
        }
        private int currentPage = 1;
        private int totalPages = 1;
        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        static int teampersonsize = int.Parse(db.Queryable<DSS_3_8_ChoiceSetting>().First().TeamPersonSize);
        private void JoinTeam_Load(object sender, EventArgs e)
        {
            LoadDGV();
            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "所有队伍信息", "队伍名称查询", "成员名称查询", "课题名称查询" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDGV();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow;
            string? teamName = null;
            string? teamSizeStr = null;
            if (dataGridView.DataSource != null)
            {
                selectedRow = dataGridView.SelectedRows[0];
                teamName = selectedRow.Cells["队伍名称"].Value.ToString();
                teamSizeStr = selectedRow.Cells["队内人数"].Value.ToString();
            }
            else
            {
                MessageBox.Show("信息选择错误，无法加入队伍");
                return;
            }


            if (HasTeam(UserHelper.bios.StudentName))
            {
                MessageBox.Show("你已有所在队伍，无法加入其他队伍");
                return;
            }

            if (!int.TryParse(teamSizeStr, out int teamSize) && teamSize >= teampersonsize)
            {
                MessageBox.Show("你无法加入该队伍，人数已达上限");
                return;
            }
            
            var team = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TeamName == teamName).First();
            if (team != null)
            {
                // 递增队伍人数
                team.Number = (1 + int.Parse(team.Number)).ToString();
                var affectedRows = db.Updateable(team)
                                    .Where(it => it.TeamID == team.TeamID)
                                    .UpdateColumns(it => new { it.Number })
                                    .ExecuteCommand();

                var whitetext = db.Queryable<DSS_3_8_Choice>()
                                .Where(it => it.Tag == team.TeamID.ToString() && (it.ChoiceName == null || it.ChoiceName == ""))
                                .OrderBy(it => it.ChoiceID)
                                .ToList()
                                .FirstOrDefault();

                int Update1 = 0;

                if (whitetext != null)
                {
                    whitetext.ChoiceType = "TM";
                    whitetext.ChoiceName = UserHelper.bios.StudentName;

                    Update1 = db.Updateable(whitetext)
                        .Where(it => it.ChoiceID == whitetext.ChoiceID) 
                        .ExecuteCommand();
                }
                DSS_3_8_BIOS dSS_3_8_BIOS = new DSS_3_8_BIOS();
                dSS_3_8_BIOS.Duty = "队员";
                dSS_3_8_BIOS.YourTeam = teamName;
                int Update = db.Updateable(dSS_3_8_BIOS)
                            .Where(it => it.Account == UserHelper.bios.Account)
                            .UpdateColumns(it => new { it.Duty, it.YourTeam })
                            .ExecuteCommand();
                if (affectedRows > 0 && Update > 0 && Update1 > 0)
                {
                    UserHelper.bios.Duty = "队员";
                    UserHelper.bios.YourTeam = teamName;
                    MessageBox.Show("已成功加入队伍");
                }
            }
            
        }

        private bool HasTeam(string currentUser)
        {
            var checkTeamQuery = db
                .Queryable<DSS_3_8_Choice, DSS_3_8_BIOTEAM>((t1, t2) => t1.Tag == t2.TeamID.ToString())
                .Where("ChoiceType LIKE @ChoiceType", new { ChoiceType = "%TM%" })
                .Where(t1 => t1.ChoiceName == currentUser)
                .Any();             
            return checkTeamQuery;
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
    }
}
