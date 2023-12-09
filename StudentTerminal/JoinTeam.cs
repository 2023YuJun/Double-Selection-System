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
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void JoinTeam_Load(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "所有队伍信息", "队伍名称查询", "队长名称查询", "课题名称查询" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var query = db.Queryable<DSS_3_8_BIOTEAM>();
            if (comboBox.Text == "队伍名称查询")
            {
                query = query.Where(it => it.YourTeam == textBox1.Text.Trim());
            }
            else if (comboBox.Text == "队长名称查询")
            {
                query = query.Where(it => it.TL == textBox1.Text.Trim());
            }
            else if (comboBox.Text == "课题名称查询")
            {
                query = query.Where(it => it.TopicName == textBox1.Text.Trim());
            }
            else if (comboBox.Text == "所有队伍信息") ;
            dataGridView.DataSource = query.Select(it => new
            {
                队伍名称 = it.YourTeam,
                队内人数 = it.Number,
                队长 = it.TL,
                队员一 = it.TM1,
                队员二 = it.TM2,
                队员三 = it.TM3,
                课题名 = it.TopicName
            }).ToList();

            #region 优化前
            //string cmd = "SELECT YourTeam as '队伍名称', Number as '队内人数', TL as '队长', TM1 as '队员一', TM2 as '队员二', TM3 as '队员三', TopicName as '课题名' FROM [DSS_3_8_BIOTEAM]";

            //if (comboBox.Text == "队伍名称查询" || comboBox.Text == "队长名称查询" || comboBox.Text == "课题名称查询")
            //{
            //    string filterColumn = "YourTeam";               // 默认过滤列
            //    string parameterName = "@FilterValue";          // 默认参数名

            //    if (comboBox.Text == "队长名称查询")
            //    {
            //        filterColumn = "TL";
            //    }
            //    else if (comboBox.Text == "课题名称查询")
            //    {
            //        filterColumn = "TopicName";
            //    }

            //    cmd += $" WHERE {filterColumn} = {parameterName}";

            //    SqlParameter[] parameters =
            //    {
            //        new SqlParameter(parameterName, SqlDbType.NVarChar, 50)
            //    };

            //    parameters[0].Value = textBox1.Text.Trim();

            //    System.Data.DataTable dt = new System.Data.DataTable();
            //    dt = SqlDbHelper.ExecuteDataTable(cmd, CommandType.Text, parameters);
            //    dataGridView.DataSource = dt;
            //}
            //else if (comboBox.Text == "所有队伍信息")
            //{
            //    System.Data.DataTable dt = new System.Data.DataTable();
            //    dt = SqlDbHelper.ExecuteDataTable(cmd);
            //    dataGridView.DataSource = dt;
            //}
            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
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

            if (!int.TryParse(teamSizeStr, out int teamSize) && teamSize >= 4)
            {
                MessageBox.Show("你无法加入该队伍");
            }
            else
            {
                var team = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.YourTeam == teamName).First();

                if (team != null)
                {
                    // 递增队伍人数
                    team.Number += 1;

                    // 分配学生至队员
                    if (team.TM1 == null)
                    {
                        team.TM1 = UserHelper.bios.StudentName;
                    }
                    else if (team.TM2 == null)
                    {
                        team.TM2 = UserHelper.bios.StudentName;
                    }
                    else if (team.TM3 == null)
                    {
                        team.TM3 = UserHelper.bios.StudentName;
                    }

                    // 更新数据库
                    var affectedRows = db.Updateable(team)
                                        .UpdateColumns(it => new { it.Number, it.TM1, it.TM2, it.TM3 })
                                        .ExecuteCommand();
                    DSS_3_8_BIOS dSS_3_8_BIOS = new DSS_3_8_BIOS();
                    dSS_3_8_BIOS.Duty = "队员";
                    dSS_3_8_BIOS.YourTeam = teamName;
                    int Update = db.Updateable(dSS_3_8_BIOS)
                                .Where(it => it.Account == UserHelper.bios.Account)
                                .UpdateColumns(it => new { it.Duty, it.YourTeam })
                                .ExecuteCommand();
                    if (affectedRows > 0 && Update > 0 ) 
                    {
                        UserHelper.bios.Duty = "队员";
                        UserHelper.bios.YourTeam = teamName;
                        MessageBox.Show("已成功加入队伍");
                    }
                }
            }
        }

        private bool HasTeam(string currentUser)
        {
            var HasTeamQuery = db.Queryable<DSS_3_8_BIOTEAM>().Where(it => it.TL == currentUser || it.TM1 == currentUser || it.TM2 == currentUser || it.TM3 == currentUser).First();
            return HasTeamQuery != null;
        }
    }
}
