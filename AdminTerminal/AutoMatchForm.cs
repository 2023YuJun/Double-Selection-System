using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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

    public partial class AutoMatchForm : Form
    {
        public AutoMatchForm()
        {
            InitializeComponent();
        }
        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        static int teachoicesize = int.Parse(db.Queryable<DSS_3_8_ChoiceSetting>().First().TeaChoiceSize);
        static int teamchoicesize = int.Parse(db.Queryable<DSS_3_8_ChoiceSetting>().First().TeamChoiceSize);
        static List<(List<string>, List<int>)> teaminitscore = new List<(List<string>, List<int>)>();
        static List<(List<string>, List<int>)> teainitscore = new List<(List<string>, List<int>)>();
        static List<List<string>> groupscore = new List<List<string>>();
        private void AutoMatchForm_Load(object sender, EventArgs e)
        {
            var result1 = db.Queryable<DSS_3_8_Choice>()
                            .Where(it => it.ChoiceType == "TC" && it.ChoiceName != "")
                            .Select(it => new DGV1
                            {
                                TeamID = it.Tag,
                                TeacherName = it.ChoiceName,
                                TeacherID = SqlFunc.Subqueryable<DSS_3_8_BIOT>().Where(t => t.TeacherName == it.ChoiceName).Select(t => t.TeacherID)
                            })
                            .ToList();
            foreach (var item in result1)
            {
                if (item.TeacherID == 0 || item.TeacherID == null)
                {
                    var teacher = db.Queryable<DSS_3_8_BIOT>().Where(t => t.TeacherName.StartsWith($"{item.TeacherName}")).First();
                    if (teacher != null)
                    {
                        item.TeacherID = teacher.TeacherID;
                    }
                }
            }
            var result2 = db.Queryable<DSS_3_8_Choice, DSS_3_8_BIOTEAM>((t1, t2) => new JoinQueryInfos(
                            JoinType.Left, t1.ChoiceName == t2.TeamName))
                            .Where((t1, t2) => t1.ChoiceType == "CT")
                            .OrderBy((t1, t2) => t1.Tag)
                            .OrderBy((t1, t2) => t1.ChoiceID)
                            .Select((t1, t2) => new
                            {
                                TeacherID = t1.Tag,
                                TeamName = t1.ChoiceName,
                                TeamTD = t2.TeamID
                            })
                            .ToList();
            dataGridView1.DataSource = result1;
            dataGridView2.DataSource = result2;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Score();
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            var allTeams = db.Queryable<DSS_3_8_BIOTEAM>().ToList();
            foreach (var team in allTeams)
            {
                db.Updateable<DSS_3_8_BIOTEAM>()
                            .SetColumns(it => new DSS_3_8_BIOTEAM { Instructor = "" })
                            .Where(it => it.TeamID == team.TeamID)
                            .ExecuteCommand();
            }

            var allTeachers = db.Queryable<DSS_3_8_BIOT>().ToList();
            foreach (var teacher in allTeachers)
            {
                db.Updateable<DSS_3_8_BIOT>()
                            .SetColumns(it => new DSS_3_8_BIOT { LeadTeam = "" })
                            .Where(it => it.TeacherID == teacher.TeacherID)
                            .ExecuteCommand();
            }

            DataTable dataTable = new DataTable();
            dataGridView3.DataSource = dataTable;
            

        }
        private void Score()
        {
            var distinctTCTag = db.Queryable<DSS_3_8_Choice>()
                .Where(t => t.ChoiceType == "TC")
                .GroupBy(t => t.Tag)
                .Select(t => t.Tag)
                .ToList();
            var distinctCTTag = db.Queryable<DSS_3_8_Choice>()
                .Where(t => t.ChoiceType == "CT")
                .GroupBy(t => t.Tag)
                .Select(t => t.Tag)
                .ToList();
            var result1 = db.Queryable<DSS_3_8_Choice>()
                .Where(t => t.ChoiceType == "TC")
                .OrderBy(t => t.Tag)
                .OrderBy(t => t.ChoiceID)
                .ToList();
            var result2 = db.Queryable<DSS_3_8_Choice>()
                .Where(t => t.ChoiceType == "CT")
                .OrderBy(t => t.Tag)
                .OrderBy(t => t.ChoiceID)
                .ToList();

            // 初始化teaminitscore和teainitscore列表
            foreach (var tag in distinctTCTag)
            {
                var choices = result1.Where(c => c.Tag == tag).ToList();
                var tagList = new List<string> { tag };
                var intList = new List<int>();

                for (int i = 0; i < teamchoicesize; i++)
                {
                    var choiceName = i < choices.Count ? choices[i].ChoiceName : ""; // 如果为空则设置为空
                    tagList.Add(choiceName);
                    intList.Add(choiceName != "" ? teamchoicesize - i : 0); //如果choiceName不为空，则减1
                }

                teaminitscore.Add((tagList, intList));
            }

            foreach (var tag in distinctCTTag)
            {
                var choices = result2.Where(c => c.Tag == tag).ToList();
                var tagList = new List<string> { tag };
                var intList = new List<int>();

                for (int i = 0; i < teachoicesize; i++)
                {
                    var choiceName = i < choices.Count ? choices[i].ChoiceName : "";
                    tagList.Add(choiceName);
                    intList.Add(choiceName != "" ? teachoicesize - i : 0);
                }

                teainitscore.Add((tagList, intList));
            }


            foreach (var team in teaminitscore)
            {
                foreach (var tea in teainitscore)
                {
                    double tempScore = 0.0;
                    List<string> matchResult = new List<string>();

                    for (int i = 1; i < team.Item1.Count; i++) // 循环遍历每个choicename元素，除了Tag
                    {
                        // 在DSS_3_8_BIOT和DSS_3_8_BIOTEAM中查找choicename对应的id
                        string? teamChoiceID = db.Queryable<DSS_3_8_BIOT>()
                            .Where(t => t.TeacherName == team.Item1[i])
                            .Select(t => t.TeacherID.ToString())
                            .ToList()
                            .FirstOrDefault();

                        string? teaChoiceID = db.Queryable<DSS_3_8_BIOTEAM>()
                            .Where(t => t.TeamName == tea.Item1[i])
                            .Select(t => t.TeamID.ToString())
                            .ToList()
                            .FirstOrDefault();

                        // Compare IDs (as strings) and Tags
                        double teamScore = 0.0;
                        double teaScore = 0.0;

                        if (teamChoiceID == tea.Item1[0])
                        {
                            teamScore = team.Item2[i - 1];
                        }
                        else
                        {
                            teamScore = 0;
                        }
                        if (teaChoiceID == team.Item1[0])
                        {
                            teaScore = tea.Item2[i - 1] * 1.1;
                        }
                        else
                        {
                            teaScore = 0;
                        }
                        double combinedScore = teamScore + teaScore;

                        if (combinedScore > tempScore)
                        {
                            tempScore = combinedScore;
                        }
                    }
                    matchResult.Add(team.Item1[0]);
                    matchResult.Add(tea.Item1[0]);
                    matchResult.Add(tempScore.ToString());

                    groupscore.Add(matchResult);
                }
            }
            // 基于tempScore(每个列表元素的索引2)对groupscore进行排序
            groupscore = groupscore.OrderByDescending(x => double.Parse(x[2])).ToList();

            //展示下所有队伍和老师匹配得分
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TeamID", typeof(string));
            dataTable.Columns.Add("TeacherID", typeof(string));
            dataTable.Columns.Add("Score", typeof(string));
            foreach (var item in groupscore)
            {
                dataTable.Rows.Add(item[0], item[1], item[2]);
            }
            dataGridView3.DataSource = dataTable;

            foreach (var item in groupscore)
            {
                int teamID = int.Parse(item[0]);
                int teacherID = int.Parse(item[1]);

                // 检查队伍是否已经选择了一名教师，并且没有达到教师选择的限制
                var team = db.Queryable<DSS_3_8_BIOTEAM>().Where(t => t.TeamID == teamID).First();
                var teacher = db.Queryable<DSS_3_8_BIOT>().Where(t => t.TeacherID == teacherID).First();
                var leadteam = teacher.LeadTeam.Split(' ');
                if (!string.IsNullOrEmpty(team.Instructor))
                {
                    continue; //如果队伍已经选择了教师，或者队伍选择人数已经达到限制，跳过
                }

                //检查是否达到教师选择限制
                if (leadteam.Count() >= teachoicesize)
                {
                    continue;
                }

                //更新团队的Instructor属性
                db.Updateable<DSS_3_8_BIOTEAM>()
                    .SetColumns(it => new DSS_3_8_BIOTEAM { Instructor = teacher.TeacherName })
                    .Where(it => it.TeamID == team.TeamID)
                    .ExecuteCommand();

                //更新教师的LeadTeam属性
                teacher.LeadTeam = (string.IsNullOrEmpty(teacher.LeadTeam)) ? team.TeamName : teacher.LeadTeam + " " + team.TeamName;
                db.Updateable<DSS_3_8_BIOT>()
                    .SetColumns(it => new DSS_3_8_BIOT { LeadTeam = teacher.LeadTeam })
                    .Where(it => it.TeacherID == teacher.TeacherID)
                    .ExecuteCommand();
            }

            //找到没有Instructor的队伍
            var teamsWithoutInstructor = db.Queryable<DSS_3_8_BIOTEAM>().Where(t => string.IsNullOrEmpty(t.Instructor)).ToList();

            if (teamsWithoutInstructor.Count == 0)
            {
                return;
            }
            else
            {
                foreach (var team in teamsWithoutInstructor)
                {
                    //寻找那些没有达到团队选择极限的老师
                    var teachers = db.Queryable<DSS_3_8_BIOT>().ToList();

                    if (teachers.Count == 0)
                    {
                        return;
                    }

                    foreach (var teacher in teachers)
                    {
                        var teacherTeams = teacher.LeadTeam.Split(' ');
                        if (teacherTeams.Length >= teachoicesize)
                        {
                            continue;
                        }
                        db.Updateable<DSS_3_8_BIOTEAM>()
                            .SetColumns(it => new DSS_3_8_BIOTEAM { Instructor = teacher.TeacherName })
                            .Where(it => it.TeamID == team.TeamID)
                            .ExecuteCommand();

                        teacher.LeadTeam = (string.IsNullOrEmpty(teacher.LeadTeam)) ? team.TeamName : teacher.LeadTeam + " " + team.TeamName;
                        db.Updateable<DSS_3_8_BIOT>()
                            .SetColumns(it => new DSS_3_8_BIOT { LeadTeam = teacher.LeadTeam })
                            .Where(it => it.TeacherID == teacher.TeacherID)
                            .ExecuteCommand();
                        break;
                    }
                }
            }
            Update_Stu();
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
        private void timer1_Tick(object sender, EventArgs e)
        {
            int dgvWidth = this.Width / 3;
            dataGridView1.Location = new System.Drawing.Point(dgvWidth * 0, 0);
            dataGridView3.Location = new System.Drawing.Point(dgvWidth * 1, 0);
            dataGridView2.Location = new System.Drawing.Point(dgvWidth * 2, 0);
            dataGridView1.Width = dgvWidth;
            dataGridView2.Width = dgvWidth;
            dataGridView3.Width = dgvWidth;
        }

    }

    public class DGV1
    {
        public string? TeamID { get; set; }
        public string? TeacherName { get; set; }
        public int? TeacherID { get; set; }
    }
}
