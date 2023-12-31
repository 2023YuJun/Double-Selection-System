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
    public partial class AdmData : Form
    {
        public AdmData()
        {
            InitializeComponent();
        }
        private void AdmData_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "所有管理员信息", "管理员账号搜索", "管理员姓名搜索" });

        }
        #region 第一页面
        // 切换Panel的可见性
        private void button8_Click(object sender, EventArgs e)
        {

            panel2.Visible = true;
            panel8.Visible = false;
        }

        //查询
        private void button5_Click(object sender, EventArgs e)
        {

        }

        //删除
        private void button6_Click(object sender, EventArgs e)
        {

        }

        //撤销
        private void button10_Click(object sender, EventArgs e)
        {

        }

        //保存
        private void button7_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 第二页面
        //单条数据传入
        private void button1_Click(object sender, EventArgs e)
        {

        }

        //全部数据传入
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //单条数据删除
        private void button3_Click(object sender, EventArgs e)
        {

        }

        //导入文件
        private void button4_Click(object sender, EventArgs e)
        {

        }

        //保存
        private void button9_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region DGV分页

        private void button20_Click(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        #endregion
    }
}
