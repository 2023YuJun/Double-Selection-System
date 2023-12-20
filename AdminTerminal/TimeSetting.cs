using SQLDAL;
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
using WinForm;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using Model;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace AdminTerminal
{
    public partial class TimeSetting : Form
    {
        public TimeSetting()
        {
            InitializeComponent();
        }

        SqlSugarClient db = SqlSugarHelper.GetSugarClient();

        private void TimeSetting_Load(object sender, EventArgs e)
        {
            string T1 = "DSS_3_8_BIOS";
            string T2 = "DSS_3_8_BIOT";
            string C1 = "Faculties";
            string C2 = "Specialty";
            string C3 = "Grade";
            Ftool.LoadComboBoxData(T1, C1, comboBox1);
            Ftool.LoadComboBoxData(T1, C2, comboBox2);
            Ftool.LoadComboBoxData(T1, C3, comboBox3);
            Ftool.LoadComboBoxData(T2, C1, comboBox4);
            Ftool.LoadComboBoxData(T2, C2, comboBox5);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string BeginTime = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string EndTime = dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");
            DSS_3_8_TimeSetting dSS_3_8_TimeSetting = new DSS_3_8_TimeSetting();
            dSS_3_8_TimeSetting.BeginTime = BeginTime;
            dSS_3_8_TimeSetting.EndTime = EndTime;
            dSS_3_8_TimeSetting.TimeType = "StuTeam";
            dSS_3_8_TimeSetting.Faculties = comboBox1.Text;
            dSS_3_8_TimeSetting.Specialty = comboBox2.Text;
            dSS_3_8_TimeSetting.Grade = comboBox3.Text;
            try
            {
                int insert;
                int update;
                update = db
                    .Updateable(dSS_3_8_TimeSetting)
                    .Where(it => it.Faculties==dSS_3_8_TimeSetting.Faculties && it.Specialty==dSS_3_8_TimeSetting.Specialty && it.Grade==dSS_3_8_TimeSetting.Grade && it.TimeType==dSS_3_8_TimeSetting.TimeType)
                    .UpdateColumns(it => new { it.BeginTime, it.EndTime })
                    .ExecuteCommand();
                if(update == 0)
                {
                    insert = db.Insertable(dSS_3_8_TimeSetting).ExecuteCommand();
                }
                else
                {
                    insert = 0;
                }
                if (insert > 0 || update > 0)
                {
                    MessageBox.Show("学生组队时限设置成功");
                }
                else
                {
                    MessageBox.Show("学生组队时限设置失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("学生组队时限设置失败，错误原因："+ex.Message);
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int delete;
                delete = db.Deleteable<DSS_3_8_TimeSetting>(it => it.Faculties==comboBox1.Text && it.Specialty==comboBox2.Text && it.Grade==comboBox3.Text && it.TimeType == "StuTeam").ExecuteCommand();
                if (delete > 0)
                {
                    MessageBox.Show("学生组队时限删除成功");
                }
                else
                {
                    MessageBox.Show("学生组队时限删除失败，不存在该时限");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("学生组队时限删除失败，错误原因："+ex.Message);
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            string BeginTime = dateTimePicker3.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string EndTime = dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm:ss");
            DSS_3_8_TimeSetting dSS_3_8_TimeSetting = new DSS_3_8_TimeSetting();
            dSS_3_8_TimeSetting.BeginTime = BeginTime;
            dSS_3_8_TimeSetting.EndTime = EndTime;
            dSS_3_8_TimeSetting.TimeType = "TeaSelect";
            dSS_3_8_TimeSetting.Faculties = comboBox4.Text;
            dSS_3_8_TimeSetting.Specialty = comboBox5.Text;
            dSS_3_8_TimeSetting.Grade = null;
            try
            {
                int insert;
                int update;
                update = db
                    .Updateable(dSS_3_8_TimeSetting)
                    .Where(it => it.Faculties == dSS_3_8_TimeSetting.Faculties && it.Specialty == dSS_3_8_TimeSetting.Specialty && it.TimeType == dSS_3_8_TimeSetting.TimeType)
                    .UpdateColumns(it => new { it.BeginTime, it.EndTime })
                    .ExecuteCommand();
                if (update == 0)
                {
                    insert = db.Insertable(dSS_3_8_TimeSetting).ExecuteCommand();
                }
                else
                {
                    insert = 0;
                }
                if (insert > 0 || update > 0)
                {
                    MessageBox.Show("导师选择时限设置成功");
                }
                else
                {
                    MessageBox.Show("导师选择时限设置失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导师选择时限设置失败，错误原因：" + ex.Message);
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int delete;
                delete = db.Deleteable<DSS_3_8_TimeSetting>(it => it.Faculties == comboBox4.Text && it.Specialty == comboBox5.Text && it.TimeType == "TeaSelect").ExecuteCommand();
                if (delete > 0)
                {
                    MessageBox.Show("导师选择时限删除成功");
                }
                else
                {
                    MessageBox.Show("导师选择时限删除失败，不存在该时限");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导师选择时限删除失败，错误原因：" + ex.Message);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string BeginTime = dateTimePicker5.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string EndTime = dateTimePicker6.Value.ToString("yyyy-MM-dd HH:mm:ss");
            DSS_3_8_TimeSetting dSS_3_8_TimeSetting = new DSS_3_8_TimeSetting();
            dSS_3_8_TimeSetting.BeginTime = BeginTime;
            dSS_3_8_TimeSetting.EndTime = EndTime;
            dSS_3_8_TimeSetting.TimeType = "SysTime";
            dSS_3_8_TimeSetting.Faculties = null;
            dSS_3_8_TimeSetting.Specialty = null;
            dSS_3_8_TimeSetting.Grade = null;
            try
            {
                int insert;
                int update;
                update = db
                    .Updateable(dSS_3_8_TimeSetting)
                    .Where(it => it.TimeType==dSS_3_8_TimeSetting.TimeType)
                    .UpdateColumns(it => new { it.BeginTime, it.EndTime })
                    .ExecuteCommand();
                if (update == 0)
                {
                    insert = db.Insertable(dSS_3_8_TimeSetting).ExecuteCommand();
                }
                else
                {
                    insert = 0;
                }
                if (insert > 0 || update > 0)
                {
                    MessageBox.Show("系统时限设置成功");
                }
                else
                {
                    MessageBox.Show("系统时限设置失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("系统时限设置失败，错误原因：" + ex.Message);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int delete;
                delete = db.Deleteable<DSS_3_8_TimeSetting>(it => it.TimeType == "SysTime").ExecuteCommand();
                if (delete > 0)
                {
                    MessageBox.Show("系统时限删除成功");
                }
                else
                {
                    MessageBox.Show("系统时限删除失败，不存在该时限");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("系统时限删除失败，错误原因：" + ex.Message);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm(); 
            configForm.ShowDialog();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            string MessageText = """
                系统时限消息


                """;
            string? middlestring = null;
            var selectStu = db.Queryable<DSS_3_8_TimeSetting>().Where(it => it.TimeType == "StuTeam").ToList();
            var selectTea = db.Queryable<DSS_3_8_TimeSetting>().Where(it => it.TimeType == "TeaSelect").ToList();
            var selectSys = db.Queryable<DSS_3_8_TimeSetting>().Where(it => it.TimeType == "SysTime").ToList();
            if (selectStu.Count > 0)
            {
                MessageText += """
                    学生组队时限信息：
                    
                    """;
                middlestring = null;
                foreach (var items in selectStu)
                {
                    middlestring +=
                        "学院：" + items.Faculties
                        + "\n专业：" + items.Specialty
                        + "\n年级：" + items.Grade
                        + "\n开始时间：" + items.BeginTime
                        + "\n结束时间" + items.EndTime
                        + "\n" + "\n";
                }
                MessageText += middlestring;
            }
            if (selectTea.Count > 0)
            {
                MessageText += """
                    导师选择时限信息：
                    
                    """;
                middlestring = null;
                foreach (var items in selectTea)
                {
                    middlestring += 
                        "学院：" + items.Faculties
                        + "\n专业：" + items.Specialty
                        + "\n开始时间：" + items.BeginTime
                        + "\n结束时间" + items.EndTime
                        + "\n" + "\n";
                }
                MessageText += middlestring;
            }
            if(selectSys.Count > 0)
            {
                MessageText += """
                    系统时限信息：
                    
                    """;
                middlestring = null;
                foreach (var items in selectSys)
                {
                    middlestring += 
                        "开始时间：" + items.BeginTime
                        + "\n结束时间" + items.EndTime;
                }
                MessageText += middlestring;
            }



            string NowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DSS_3_8_Messaging dSS_3_8_Messaging = new DSS_3_8_Messaging();
            dSS_3_8_Messaging.MessageType = "时限消息";
            dSS_3_8_Messaging.TextContent = MessageText;
            dSS_3_8_Messaging.Timestamp = NowTime;
            if(db.Insertable(dSS_3_8_Messaging).ExecuteCommand() > 0)
            {
                MessageBox.Show("消息发送成功");
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            // 检查选择的值是否在下拉框的选项中
            if (!comboBox.Items.Contains(comboBox.Text))
            {
                // 如果选择的值不在选项中，将下拉框文本设置为特定字符串
                comboBox.Text = "学院";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (!comboBox.Items.Contains(comboBox.Text))
            {
                comboBox.Text = "专业";
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (!comboBox.Items.Contains(comboBox.Text))
            {
                comboBox.Text = "年级";
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (!comboBox.Items.Contains(comboBox.Text))
            {
                comboBox.Text = "学院";
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (!comboBox.Items.Contains(comboBox.Text))
            {
                comboBox.Text = "专业";
            }
        }

    }
}
