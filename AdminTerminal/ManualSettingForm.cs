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

namespace AdminTerminal
{
    public partial class ManualSettingForm : Form
    {
        public ManualSettingForm()
        {
            InitializeComponent();
        }
        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        static int teachoicesize = int.Parse(db.Queryable<DSS_3_8_ChoiceSetting>().First().TeaChoiceSize);
        static int teamchoicesize = int.Parse(db.Queryable<DSS_3_8_ChoiceSetting>().First().TeamChoiceSize);
        private void ManualSettingForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        dataGridView2.Rows.Remove(row);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count >= teachoicesize)
            {
                MessageBox.Show("已达到最大带领限制");
                return;
            }

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                List<string> teamNames = new List<string>();
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    teamNames.Add(row.Cells["队伍名称"].Value.ToString());
                }

                string selectedTeamName = selectedRow.Cells["队伍名称"].Value.ToString();

                if (teamNames.Contains(selectedTeamName))
                {
                    MessageBox.Show("不能添加重复的队伍");
                    return;
                }

                DataRow newRow = ((DataTable)dataGridView2.DataSource).NewRow();
                for (int i = 0; i < selectedRow.Cells.Count; i++)
                {
                    newRow[i] = selectedRow.Cells[i].Value;
                }

                ((DataTable)dataGridView2.DataSource).Rows.Add(newRow);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentCell != null)
            {
                string teacherName = dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells["姓名"].Value.ToString();
                string teacherID = dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells["teaID"].Value.ToString();
                int teaID = int.Parse(teacherID);
                DataTable dt = (DataTable)dataGridView2.DataSource;
                List<string> teamNames = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    int teamID = Convert.ToInt32(row["TeamID"]);
                    db.Updateable<DSS_3_8_BIOTEAM>()
                            .SetColumns(it => new DSS_3_8_BIOTEAM { Instructor = teacherName })
                            .Where(it => it.TeamID == teamID)
                            .ExecuteCommand();
                    string teamName = row["队伍名称"].ToString();
                    teamNames.Add(teamName);
                }

                string combinedTeamNames = string.Join(" ", teamNames);

                db.Updateable<DSS_3_8_BIOT>()
                            .SetColumns(it => new DSS_3_8_BIOT { LeadTeam = combinedTeamNames })
                            .Where(it => it.TeacherID == teaID)
                            .ExecuteCommand();
            }
            LoadData();
            Update_Stu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string MessageText = """
                组队结果消息：

                """;
            string middlestring = "";
            var teacher = db.Queryable<DSS_3_8_BIOT>().Where(t => string.IsNullOrEmpty(t.LeadTeam)).ToList();
            foreach (var t in teacher)
            {
                middlestring += t.TeacherName + "老师带领队伍：\n" + t.LeadTeam + "\n";
            }
            MessageText += middlestring;
            string NowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DSS_3_8_Messaging dSS_3_8_Messaging = new DSS_3_8_Messaging();
            dSS_3_8_Messaging.MessageType = "组队结果消息";
            dSS_3_8_Messaging.TextContent = MessageText;
            dSS_3_8_Messaging.Timestamp = NowTime;
            if (db.Insertable(dSS_3_8_Messaging).ExecuteCommand() > 0)
            {
                MessageBox.Show("消息发送成功");
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var leadTeamValue = dataGridView3.Rows[e.RowIndex].Cells["带领队伍"].Value?.ToString();
                DataTable dt = new DataTable();
                dt.Columns.Add("TeamID", typeof(int));
                dt.Columns.Add("队伍名称", typeof(string));
                dt.Columns.Add("队内人数", typeof(int));
                dt.Columns.Add("课题名", typeof(string));
                dt.Columns.Add("课题简介", typeof(string));
                dt.Columns.Add("文件名", typeof(string));
                dt.Columns.Add("文件下载路径", typeof(string));
                dt.Columns.Add("指导老师", typeof(string));
                if (!string.IsNullOrEmpty(leadTeamValue))
                {
                    var teamnames = leadTeamValue.Split(' ');

                    

                    foreach (var item in teamnames)
                    {
                        var result = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TeamName == item).First();
                        if (result != null)
                        {
                            // Assuming property names - replace them with actual properties
                            dt.Rows.Add(result.TeamID, result.TeamName, result.Number, result.TopicName, result.TopicIntroduction, result.FileName, result.FileDownloadPath, result.Instructor);
                        }
                    }

                    
                }
                dataGridView2.DataSource = dt;
            }
        }

        private void LoadData()
        {
            var result1 = db.Queryable<DSS_3_8_BIOTEAM>()
                .Select(it => new
                {
                    TeamID = it.TeamID,
                    队伍名称 = it.TeamName,
                    队内人数 = it.Number,
                    课题名 = it.TopicName,
                    课题简介 = it.TopicIntroduction,
                    文件名 = it.FileName,
                    文件下载路径 = it.FileDownloadPath,
                    指导老师 = it.Instructor
                })
                .ToList();

            var result2 = db.Queryable<DSS_3_8_BIOT>()
                .Select(it => new
                {
                    TeaID = it.TeacherID,
                    姓名 = it.TeacherName,
                    性别 = it.Sex,
                    工号 = it.Account,
                    院系 = it.Faculties,
                    专业 = it.Specialty,
                    带领队伍 = it.LeadTeam,
                    联系方式 = it.Contact
                }).ToList();

            dataGridView1.DataSource = result1;
            dataGridView3.DataSource = result2;

        }
        private void Update_Stu()
        {
            // 查询获取所有 Instructor 为空的队伍的 TeamID
            var emptyInstructorTeams = db.Queryable<DSS_3_8_BIOTEAM>()
                .Where(t => string.IsNullOrEmpty(t.Instructor))
                .Select(t => t.TeamID)
                .ToList();

            // 如果 Instructor 为空的队伍存在
            if (emptyInstructorTeams.Any())
            {
                foreach (var teamID in emptyInstructorTeams)
                {
                    // 将队伍中所有学生的 Instructor 设置为空
                    string id = teamID.ToString();
                    var studentNames = db.Queryable<DSS_3_8_Choice>()
                        .Where(c => c.Tag == id && c.ChoiceType.StartsWith("TM"))
                        .Select(c => c.ChoiceName)
                        .ToList();

                    var studentsToUpdate = db.Queryable<DSS_3_8_BIOS>()
                        .Where(s => s.Instructor != "" && studentNames.Contains(s.StudentName))
                        .ToList();

                    foreach (var student in studentsToUpdate)
                    {
                        student.Instructor = "";
                        db.Updateable<DSS_3_8_BIOS>()
                            .SetColumns(it => new DSS_3_8_BIOS { Instructor = student.Instructor })
                            .Where(s => s.StudentName == student.StudentName)
                            .ExecuteCommand();
                    }
                }
            }

            // 查询获取所有 Instructor 不为空的队伍的 TeamID 和 Instructor 的值
            var instructorTeams = db.Queryable<DSS_3_8_BIOTEAM>()
                .Where(t => !string.IsNullOrEmpty(t.Instructor))
                .Select(t => new { t.TeamID, t.Instructor })
                .ToList();

            foreach (var team in instructorTeams)
            {
                // 查询获取 DSS_3_8_Choice 表中符合条件的 ChoiceName 的值
                string teamID = team.TeamID.ToString();
                var choiceNames = db.Queryable<DSS_3_8_Choice>()
                    .Where(c => c.Tag == teamID && c.ChoiceType.StartsWith("TM"))
                    .Select(c => c.ChoiceName)
                    .ToList();

                // 更新 DSS_3_8_BIOS 表中的 Instructor 属性为查到的 Instructor 值
                foreach (var choiceName in choiceNames)
                {
                    var student = db.Queryable<DSS_3_8_BIOS>()
                        .Where(s => s.StudentName == choiceName)
                        .First();

                    if (student != null)
                    {
                        db.Updateable<DSS_3_8_BIOS>()
                            .SetColumns(it => new DSS_3_8_BIOS { Instructor = team.Instructor })
                            .Where(s => s.StudentName == choiceName)
                            .ExecuteCommand();
                    }
                }
            }



        }
    }
}
