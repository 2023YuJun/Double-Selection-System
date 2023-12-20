using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using SQLDAL;
using SqlSugar;

namespace AdminTerminal
{
    public partial class TeamRuleSettingForm : Form
    {
        public TeamRuleSettingForm()
        {
            InitializeComponent();
        }
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void TeamRuleSettingForm_Load(object sender, EventArgs e)
        {
            var choicesetting = db.Queryable<DSS_3_8_ChoiceSetting>().OrderBy(it => it.LimitID).Take(1).First();
            if (choicesetting == null)
            {
                DSS_3_8_ChoiceSetting dSS_3_8_ChoiceSetting = new DSS_3_8_ChoiceSetting();
                dSS_3_8_ChoiceSetting.TeamPersonSize = "4";
                dSS_3_8_ChoiceSetting.TeamChoiceSize = "3";
                dSS_3_8_ChoiceSetting.TeaChoiceSize = "5";
                db.Insertable(dSS_3_8_ChoiceSetting).ExecuteCommand();
            }
            choicesetting = db.Queryable<DSS_3_8_ChoiceSetting>().First();
            textBox1.Text = choicesetting.TeamPersonSize;
            textBox2.Text = choicesetting.TeamChoiceSize;
            textBox3.Text = choicesetting.TeaChoiceSize;
            button2.Enabled = false;
            button1.Enabled = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button1.Enabled = false;
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取第一行数据的 LimitID 值
                var firstRowLimitID = db.Queryable<DSS_3_8_ChoiceSetting>().OrderBy(it => it.LimitID).Take(1).ToList().FirstOrDefault()?.LimitID;

                // 更新表的第一行数据
                var firstRow = db.Updateable<DSS_3_8_ChoiceSetting>().Where(it => it.LimitID == firstRowLimitID).SetColumns(it => new DSS_3_8_ChoiceSetting()
                {
                    TeamPersonSize = textBox1.Text,
                    TeamChoiceSize = textBox2.Text,
                    TeaChoiceSize = textBox3.Text
                    // 更新的其他列和值
                }).ExecuteCommand();
                if (firstRow > 0)
                {
                    MessageBox.Show("组队规则保存成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            button2.Enabled = false;
            button1.Enabled = true;
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var choicesetting = db.Queryable<DSS_3_8_ChoiceSetting>().First();
            string MessageText = $"""
                组队规则消息

                    学生组队人数上限为：{choicesetting.TeamPersonSize}
                    队伍选择导师上限为：{choicesetting.TeamChoiceSize}
                    导师选择队伍上限为：{choicesetting.TeaChoiceSize}
                """;
            string NowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DSS_3_8_Messaging dSS_3_8_Messaging = new DSS_3_8_Messaging();
            dSS_3_8_Messaging.MessageType = "组队规则消息";
            dSS_3_8_Messaging.TextContent = MessageText;
            dSS_3_8_Messaging.Timestamp = NowTime;
            try
            {
                if (db.Insertable(dSS_3_8_Messaging).ExecuteCommand() > 0)
                {
                    MessageBox.Show("消息发送成功");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
