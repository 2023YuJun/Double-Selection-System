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
    public partial class TeaData : Form
    {
        public TeaData()
        {
            InitializeComponent();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // 切换Panel的可见性
            panel2.Visible = true;
            panel8.Visible = false;
        }


        private void TeaData_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "所有导师信息", "导师职工号搜索", "导师姓名搜索","带领队伍搜索" });
        }
        #region 导入保存页面
        //右箭头
        private void button1_Click(object sender, EventArgs e)
        {

        }

        //第二个右箭头
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //垃圾桶
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

        #region 信息查询页
        //查询
        private void button5_Click(object sender, EventArgs e)
        {

        }

        //导入数据
        private void button8_Click_1(object sender, EventArgs e)
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
    }
}
