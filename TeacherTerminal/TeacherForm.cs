using Common;
using Newtonsoft.Json.Linq;
using SQLDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForm;

namespace TeacherTerminal
{
    public partial class TeacherForm : Form
    {
        Ftool ftool = new Ftool();
        bool btn1 = false;
        bool btn2 = false;
        bool btn3 = false;
        bool time = false;
        public TeacherForm()
        {
            InitializeComponent();
        }

        private void TeacherForm_Load(object sender, EventArgs e)
        {
            TeaTimer.Start();
        }
        private void TeacherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TeacherTerminal.FexitRequested = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //账号设置
            Ftool.Cheak_click(button1, ftool.C_btn);
            ftool.C_btn = button1;
            BtnTimer1.Start();
            if (panel8.Size != panel8.MinimumSize)
            {
                BtnTimer2.Start();
            }
            if (panel9.Size != panel9.MinimumSize)
            {
                BtnTimer3.Start();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //个人信息管理
            Ftool.Cheak_click(button2, ftool.C_btn);
            ftool.C_btn = button2;
            BIOT bIOT = new BIOT();
            Ftool.Showform(panel4, bIOT);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //学生信息查询
            Ftool.Cheak_click(button3, ftool.C_btn);
            ftool.C_btn = button3;
            SearchS searchS = new SearchS();
            Ftool.Showform(panel4, searchS);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //毕业设计组队
            TeaTimer.Start();
            if (!time)
            {
                MessageBox.Show("导师选择功能已关闭");
                if (panel8.Size != panel8.MinimumSize)
                {
                    BtnTimer2.Start();
                }
                return;
            }
            Ftool.Cheak_click(button4, ftool.C_btn);
            ftool.C_btn = button4;
            BtnTimer2.Start();
            if (panel5.Size != panel5.MinimumSize)
            {
                BtnTimer1.Start();
            }
            if (panel9.Size != panel9.MinimumSize)
            {
                BtnTimer3.Start();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //系统消息
            Ftool.Cheak_click(button5, ftool.C_btn);
            ftool.C_btn = button5;
            WinForm.Message message = new WinForm.Message();
            Ftool.Showform(panel4, message);
            //BtnTimer3.Start();
            //if (panel8.Size != panel8.MinimumSize)
            //{
            //    BtnTimer2.Start();
            //}
            //if (panel5.Size != panel5.MinimumSize)
            //{
            //    BtnTimer1.Start();
            //}
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //帮助
            Ftool.Cheak_click(button6, ftool.C_btn);
            ftool.C_btn = button6;
            HelpT helpT = new HelpT();
            Ftool.Showform(panel4, helpT);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //修改密码
            Ftool.Cheak_click(button7, ftool.C_btn);
            ftool.C_btn = button7;
            WinForm.NewPassword newPassword = new WinForm.NewPassword();
            newPassword.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //退出登录
            Ftool.Cheak_click(button8, ftool.C_btn);
            ftool.C_btn = button8;
            TeacherTerminal.LexitRequested = false;
            this.Close();
            TeacherTerminal.FexitRequested = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //选择带领队伍
            Ftool.Cheak_click(button9, ftool.C_btn);
            ftool.C_btn = button9;
            TeacherSelectionTeam teacherSelectionTeam = new TeacherSelectionTeam();
            Ftool.Showform(panel4, teacherSelectionTeam);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            //当前所带队伍
            Ftool.Cheak_click(button10, ftool.C_btn);
            ftool.C_btn = button10;
            CurrentLeadTeam currentLeadTeam = new CurrentLeadTeam();
            Ftool.Showform(panel4, currentLeadTeam);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //收件箱
            Ftool.Cheak_click(button12, ftool.C_btn);
            ftool.C_btn = button12;
            WinForm.Message message = new WinForm.Message();
            Ftool.Showform(panel4, message);
        }
        private void button13_Click(object sender, EventArgs e)
        {
            //发件箱
            Ftool.Cheak_click(button13, ftool.C_btn);
            ftool.C_btn = button13;
            WinForm.Message message = new WinForm.Message();
            Ftool.Showform(panel4, message);
        }
        private void BtnTimer1_Tick(object sender, EventArgs e)
        {
            //账号设置计时器
            if (!btn1)
            {
                panel5.Height += 10;
                if (panel5.Height == panel5.MaximumSize.Height)
                {
                    btn1 = true;
                    BtnTimer1.Stop();
                }

            }
            else
            {
                panel5.Height -= 10;
                if (panel5.Height == panel5.MinimumSize.Height)
                {
                    btn1 = false;
                    BtnTimer1.Stop();
                }

            }
        }

        private void BtnTimer2_Tick(object sender, EventArgs e)
        {
            //组队计时器
            int x1 = panel9.Location.X;
            int y1 = panel9.Location.Y;
            int x2 = panel10.Location.X;
            int y2 = panel10.Location.Y;
            if (!btn2)
            {
                panel8.Height += 10;
                panel9.Location = new Point(x1, y1 += 10);
                panel10.Location = new Point(x2, y2 += 10);
                if (panel8.Height == panel8.MaximumSize.Height)
                {
                    btn2 = true;
                    BtnTimer2.Stop();
                }

            }
            else
            {
                panel8.Height -= 10;
                panel9.Location = new Point(x1, y1 -= 10);
                panel10.Location = new Point(x2, y2 -= 10);
                if (panel8.Height == panel8.MinimumSize.Height)
                {
                    btn2 = false;
                    BtnTimer2.Stop();
                }

            }
        }

        private void BtnTimer3_Tick(object sender, EventArgs e)
        {
            int x2 = panel10.Location.X;
            int y2 = panel10.Location.Y;
            if (!btn3)
            {
                panel9.Height += 10;
                panel10.Location = new Point(x2, y2 += 10);
                if (panel9.Height == panel9.MaximumSize.Height)
                {
                    btn3 = true;
                    BtnTimer3.Stop();
                }

            }
            else
            {
                panel9.Height -= 10;
                panel10.Location = new Point(x2, y2 -= 10);
                if (panel9.Height == panel9.MinimumSize.Height)
                {
                    btn3 = false;
                    BtnTimer3.Stop();
                }

            }
        }

        private void TeaTimer_Tick(object sender, EventArgs e)
        {
            DateTime TeaBeginTime;
            DateTime TeaEndTime;
            DateTime NowTime = DateTime.Now;
            JObject jsonObj = JObject.Parse(SqlDbHelper.json);
            var TeaTime = jsonObj["AppSettings"]["TeaTimeSetting"] as JObject;
            if (TeaTime != null && TeaTime.HasValues)
            {
                bool found = false;
                string existingFieldName = null;

                foreach (var item in TeaTime)
                {
                    var field = (JObject)item.Value;

                    if (field["Faculties"]?.ToString() == "学院")
                    {
                        if (field["ProfessionalDirection"]?.ToString() == "专业")
                        {
                            found = true;
                            existingFieldName = item.Key;
                            break;
                        }
                        else if (field["ProfessionalDirection"]?.ToString() == UserHelper.bios.Specialty)
                        {
                            found = true;
                            existingFieldName = item.Key;
                            break;
                        }
                    }
                    else if (field["Faculties"]?.ToString() == UserHelper.bios.Faculties &&
                        field["ProfessionalDirection"]?.ToString() == UserHelper.bios.Specialty)
                    {
                        found = true;
                        existingFieldName = item.Key;
                        break;
                    }
                }
                if (found)
                {
                    var existingField = TeaTime[existingFieldName];
                    string BeginTime = existingField["BeginTime"]?.ToString();
                    string EndTime = existingField["EndTime"]?.ToString();
                    TeaBeginTime = DateTime.ParseExact(BeginTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    TeaEndTime = DateTime.ParseExact(EndTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    if (NowTime >= TeaBeginTime && NowTime <= TeaEndTime)
                    {
                        time = true;
                    }
                }
                else
                {
                    time = true;
                }
            }
            else
            {
                time = true;
            }
            if (!time)
            {
                TeaTimer.Stop();
            }
        }
    }
}
