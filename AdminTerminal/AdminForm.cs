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

namespace AdminTerminal
{
    public partial class AdminForm : Form
    {
        Ftool ftool = new Ftool();
        bool btn1 = false;
        bool btn2 = false;
        bool btn3 = false;
        bool btn4 = false;
        public AdminForm()
        {
            InitializeComponent();
        }
        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AdminTerminal.FexitRequested = true;
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
            if (panel6.Size != panel6.MinimumSize)
            {
                BtnTimer4.Start();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //个人信息管理
            Ftool.Cheak_click(button2, ftool.C_btn);
            ftool.C_btn = button2;
            BtnTimer4.Start();
            if (panel8.Size != panel8.MinimumSize)
            {
                BtnTimer2.Start();
            }
            if (panel9.Size != panel9.MinimumSize)
            {
                BtnTimer3.Start();
            }
            if (panel5.Size != panel5.MinimumSize)
            {
                BtnTimer1.Start();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //系统时限设置
            Ftool.Cheak_click(button3, ftool.C_btn);
            ftool.C_btn = button3;
            TimeSetting timeSetting = new TimeSetting();
            Ftool.Showform(panel4, timeSetting);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //毕业设计组队
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
            if (panel6.Size != panel6.MinimumSize)
            {
                BtnTimer4.Start();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //系统消息
            Ftool.Cheak_click(button5, ftool.C_btn);
            ftool.C_btn = button5;
            WinForm.Message message = new WinForm.Message();
            Ftool.Showform(panel4, message);

            /// <summary>
            /// 暂时先雪藏了，后面要扩展功能再搞
            /// BtnTimer3.Start();
            /// if (panel8.Size != panel8.MinimumSize)
            /// {
            ///     BtnTimer2.Start();
            /// }
            /// if (panel5.Size != panel5.MinimumSize)
            /// {
            ///     BtnTimer1.Start();
            /// }
            /// if (panel6.Size != panel6.MinimumSize)
            /// {
            ///     BtnTimer4.Start();
            /// }
            /// </summary>

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //帮助
            Ftool.Cheak_click(button6, ftool.C_btn);
            ftool.C_btn = button6;
            HelpA helpA = new HelpA();
            Ftool.Showform(panel4, helpA);

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
            AdminTerminal.LexitRequested = false;
            this.Close();
            AdminTerminal.FexitRequested = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //自动分配
            Ftool.Cheak_click(button9, ftool.C_btn);
            ftool.C_btn = button9;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            //手动调整
            Ftool.Cheak_click(button10, ftool.C_btn);
            ftool.C_btn = button10;
        }
        private void button11_Click(object sender, EventArgs e)
        {
            //组队规则
            Ftool.Cheak_click(button11, ftool.C_btn);
            ftool.C_btn = button11;
            TeamRuleSettingForm teamRuleSettingForm = new TeamRuleSettingForm();
            Ftool.Showform(panel4, teamRuleSettingForm);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            Ftool.Cheak_click(button12, ftool.C_btn);
            ftool.C_btn = button12;
            WinForm.Message message = new WinForm.Message();
            Ftool.Showform(panel4, message);
        }
        private void button13_Click(object sender, EventArgs e)
        {
            Ftool.Cheak_click(button13, ftool.C_btn);
            ftool.C_btn = button13;
            WinForm.Message message = new WinForm.Message();
            Ftool.Showform(panel4, message);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            //学生信息管理

        }

        private void button15_Click(object sender, EventArgs e)
        {
            //导师信息管理
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //队伍信息管理
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

        private void BtnTimer4_Tick(object sender, EventArgs e)
        {
            int x3 = panel7.Location.X;
            int y3 = panel7.Location.Y;
            int x4 = panel8.Location.X;
            int y4 = panel8.Location.Y;
            int x1 = panel9.Location.X;
            int y1 = panel9.Location.Y;
            int x2 = panel10.Location.X;
            int y2 = panel10.Location.Y;

            if (!btn4)
            {
                panel6.Height += 10;
                panel7.Location = new Point(x3, y3 += 10);
                panel8.Location = new Point(x4, y4 += 10);
                panel9.Location = new Point(x1, y1 += 10);
                panel10.Location = new Point(x2, y2 += 10);
                if (panel6.Height == panel6.MaximumSize.Height)
                {
                    btn4 = true;
                    BtnTimer4.Stop();
                }

            }
            else
            {
                panel6.Height -= 10;
                panel7.Location = new Point(x3, y3 -= 10);
                panel8.Location = new Point(x4, y4 -= 10);
                panel9.Location = new Point(x1, y1 -= 10);
                panel10.Location = new Point(x2, y2 -= 10);
                if (panel6.Height == panel6.MinimumSize.Height)
                {
                    btn4 = false;
                    BtnTimer4.Stop();
                }

            }
        }


    }
}
