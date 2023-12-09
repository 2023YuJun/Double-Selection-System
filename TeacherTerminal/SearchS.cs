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

namespace TeacherTerminal
{
    public partial class SearchS : Form
    {
        public SearchS()
        {
            InitializeComponent();
        }
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void button1_Click(object sender, EventArgs e)
        {
            var SIBIOS = db.Queryable<DSS_3_8_BIOS>();
            if (comboBox.Text == "所有学生信息")
            {
                ;
            }
            else if (comboBox.Text == "学生学号搜索")
            {
                SIBIOS = SIBIOS.Where(it => it.Account == textBox.Text.Trim());
            }
            else if (comboBox.Text == "学生姓名搜索")
            {
                SIBIOS = SIBIOS.Where(it => it.StudentName == textBox.Text.Trim());
            }
            else if (comboBox.Text == "未组队学生信息")
            {
                SIBIOS = SIBIOS.Where(it => it.Duty == "暂无");
            }
            dataGridView1.DataSource = SIBIOS.ToList();

            #region 优化前
            //string cmd = string.Empty;
            //SqlParameter[] parameters = null;
            //if (comboBox.Text == "所有学生信息")
            //{
            //    cmd = "select * from [DSS_3_8_BIOS]";
            //}
            //else if (comboBox.Text == "学生学号搜索")
            //{
            //    cmd = "select * from [DSS_3_8_BIOS] where Account=@Account";
            //    parameters = new SqlParameter[]
            //    {
            //        new SqlParameter("@Account", SqlDbType.NVarChar, 50)
            //        {
            //            Value = textBox.Text.Trim()
            //        }
            //    };
            //}
            //else if (comboBox.Text == "学生姓名搜索")
            //{
            //    cmd = "select * from [DSS_3_8_BIOS] where StudentName=@StudentName";
            //    parameters = new SqlParameter[]
            //    {
            //        new SqlParameter("@StudentName", SqlDbType.NVarChar, 50)
            //        {
            //            Value = textBox.Text.Trim()
            //        }
            //    };
            //}
            //else if (comboBox.Text == "未组队学生信息")
            //{
            //    cmd = "select * from [DSS_3_8_BIOS] where Duty = @Duty";
            //    parameters = new SqlParameter[]
            //    {
            //        new SqlParameter("@Duty", SqlDbType.NVarChar, 50)
            //        {
            //            Value = "暂无"
            //        }
            //    };

            //}
            //DataTable dataTable = new DataTable();
            //dataTable = SqlDbHelper.ExecuteDataTable(cmd, CommandType.Text, parameters);
            //dataGridView1.DataSource = dataTable;
            #endregion
        }

        private void SearchT_Load(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "所有学生信息", "学生学号搜索", "学生姓名搜索", "未组队学生信息" });
        }
    }
}
