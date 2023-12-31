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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
    }
}
