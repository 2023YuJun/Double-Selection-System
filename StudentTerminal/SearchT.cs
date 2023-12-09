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

namespace StudentTerminal
{
    public partial class SearchT : Form
    {
        public SearchT()
        {
            InitializeComponent();
        }
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void button1_Click(object sender, EventArgs e)
        {
            var SIBIOT = db.Queryable<DSS_3_8_BIOT>();
            if(comboBox.Text == "导师职工号搜索")
            {
                SIBIOT = SIBIOT.Where(it => it.Account == textBox.Text.Trim());
            }
            else if (comboBox.Text == "导师姓名搜索")
            {
                SIBIOT = SIBIOT.Where(it => it.TeacherName == textBox.Text.Trim());
            }
            var show = SIBIOT.First();
            if (show != null)
            {
                textBox1.Text = show.TeacherName;
                textBox2.Text = show.Sex;
                textBox3.Text = show.Account;
                textBox4.Text = show.Contact;
                textBox5.Text = show.Faculties;
                textBox6.Text = show.Specialty;
                textBox7.Text = show.LeadTeam;
                textBox8.Text = show.PersonalDeeds;
            }
            else
            {
                MessageBox.Show("查找出错！");
            }

            #region 优化前
            //string cmd = string.Empty;
            //SqlParameter[] parameters = null;
            //if (comboBox.Text == "导师职工号搜索")
            //{
            //    cmd = "select * from [DSS_3_8_BIOT] where Account=@Account";
            //    parameters = new SqlParameter[]
            //    {
            //        new SqlParameter("@Account", SqlDbType.NVarChar, 50)
            //        {
            //            Value = textBox.Text.Trim()
            //        }
            //    };
            //}
            //else if (comboBox.Text == "导师姓名搜索")
            //{
            //    cmd = "select * from [DSS_3_8_BIOT] where TeacherName=@TeacherName";
            //    parameters = new SqlParameter[]
            //    {
            //        new SqlParameter("@TeacherName", SqlDbType.NVarChar, 50)
            //        {
            //            Value = textBox.Text.Trim()
            //        }
            //    };
            //}
            //SqlDataReader reader = SqlDbHelper.ExecuteReader(cmd, CommandType.Text, parameters);
            //if (reader.Read())
            //{
            //    textBox1.Text = reader["TeacherName"].ToString();
            //    textBox2.Text = reader["Sex"].ToString();
            //    textBox3.Text = reader["Account"].ToString();
            //    textBox4.Text = reader["Contact"].ToString();
            //    textBox5.Text = reader["Faculties"].ToString();
            //    textBox6.Text = reader["ProfessionalDirection"].ToString();
            //    textBox7.Text = reader["LeadTeam"].ToString();
            //    textBox8.Text = reader["PersonalDeeds"].ToString();
            //}
            //else
            //{
            //    MessageBox.Show("查找出错！");
            //}
            #endregion
        }

            private void SearchT_Load(object sender, EventArgs e)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "导师职工号搜索", "导师姓名搜索" });
        }
    }
}
