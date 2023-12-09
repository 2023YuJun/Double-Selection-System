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

namespace WinForm
{
    public partial class Message : Form
    {
        public Message()
        {
            InitializeComponent();
        }
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void Message_Load(object sender, EventArgs e)
        {
            var latestRecords = db.Queryable<DSS_3_8_Messaging>().OrderByDescending(it => it.MessageID).Take(20).ToList();
            for (int i = latestRecords.Count - 1; i > 0; i--)
            {
                TextBox textBox = new TextBox();
                textBox.Text = latestRecords[i].MessageType + "\r\n" + latestRecords[i].Timestamp;             // 设置文本框显示的内容
                textBox.Dock = DockStyle.Top;               // 设置文本框停靠方式，从顶部开始堆叠
                textBox.Multiline = true;
                textBox.ScrollBars = ScrollBars.None;
                textBox.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
                textBox.Size = new Size(220, 80);
                textBox.ReadOnly = true;
                textBox.BackColor = Color.FromArgb(211, 226, 244);

                // 在这里创建闭包以保持 latestRecords[i] 的引用
                void TextBox_Click(object sender, EventArgs e)
                {
                    TextBox clickedTextBox = (TextBox)sender;
                    int index = (int)clickedTextBox.Tag; // 获取保存的索引
                    richTextBox1.Text = latestRecords[index].TextContent;
                }
                textBox.Click += TextBox_Click;
                textBox.Tag = i; // 保存索引值

                // 将文本框添加到面板中
                panel1.Controls.Add(textBox);
            }
        }

    }
}
