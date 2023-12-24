using Common;
using Model;
using Newtonsoft.Json.Linq;
using SQLDAL;
using SqlSugar;
using WinForm;

namespace StudentTerminal
{
    public partial class StudentForm : Form
    {
        Ftool ftool = new Ftool();
        bool btn1 = false;
        bool btn2 = false;
        bool btn3 = false;
        bool time = false;
        public StudentForm()
        {
            InitializeComponent();
        }
        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void StudentForm_Load(object sender, EventArgs e)
        {
            StuTimer.Start();
        }
        private void StudentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StudentTerminal.FexitRequested = true;
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
            BIOS bIOS = new BIOS();
            Ftool.Showform(panel4, bIOS);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //学生信息查询
            Ftool.Cheak_click(button3, ftool.C_btn);
            ftool.C_btn = button3;
            SearchT searchT = new SearchT();
            Ftool.Showform(panel4, searchT);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //毕业设计组队
            StuTimer.Start();
            if (!time)
            {
                MessageBox.Show("学生组队功能已关闭");
                if(panel8.Size != panel8.MinimumSize)
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
            HelpS helpS = new HelpS();
            Ftool.Showform(panel4, helpS);

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
            StudentTerminal.LexitRequested = false;
            this.Close();
            StudentTerminal.FexitRequested = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //创建队伍
            Ftool.Cheak_click(button9, ftool.C_btn);
            ftool.C_btn = button9;
            CreateTeam createTeam = new CreateTeam();
            Ftool.Showform(panel4, createTeam);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            //加入队伍
            Ftool.Cheak_click(button10, ftool.C_btn);
            ftool.C_btn = button10;
            JoinTeam joinTeam = new JoinTeam();
            Ftool.Showform(panel4, joinTeam);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            //所在队伍
            Ftool.Cheak_click(button11, ftool.C_btn);
            ftool.C_btn = button11;
            InTheTeam inTheTeam = new InTheTeam();
            Ftool.Showform(panel4, inTheTeam);
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
                    button7.Enabled = true;
                    button8.Enabled = true;
                    BtnTimer1.Stop();
                }

            }
            else
            {
                panel5.Height -= 10;
                if (panel5.Height == panel5.MinimumSize.Height)
                {
                    btn1 = false;
                    button7.Enabled = false;
                    button8.Enabled = false;
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

        private void StuTimer_Tick(object sender, EventArgs e)
        {
            var faculties = UserHelper.bios.Faculties;
            var specialty = UserHelper.bios.Specialty;
            var grade = UserHelper.bios.Grade;

            var currentTime = DateTime.Now;

            var timeSettings = db.Queryable<DSS_3_8_TimeSetting>()
                .Where(t => t.TimeType == "StuTeam")
                .ToList();
            if(timeSettings.Count > 0)
            {
                foreach (var setting in timeSettings)
                {
                    if ((setting.Faculties == "学院" && setting.Specialty == "专业" && setting.Grade == "年级") ||
                        (setting.Faculties == faculties && setting.Specialty == specialty && setting.Grade == grade))
                    {
                        DateTime beginTime = DateTime.ParseExact(setting.BeginTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime endTime = DateTime.ParseExact(setting.EndTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                        if (currentTime >= beginTime && currentTime <= endTime)
                        {
                            // 时间范围内
                            time = true;
                            break;
                        }
                        else
                        {
                            time = false;
                            break;
                        }
                    }
                    else
                    {
                        time = true;
                    }
                }
            }
            else
            {
                time = true;
            }
            if (!time)
            {
                StuTimer.Stop();
            }
        }


    }
}