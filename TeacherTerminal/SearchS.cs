using Common;
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
        private int currentPage = 1;
        private int totalPages = 1;
        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void button1_Click(object sender, EventArgs e)
        {
            LoadDGV();
        }

        private void SearchT_Load(object sender, EventArgs e)
        {
            LoadDGV();
            comboBox.Items.Clear();
            comboBox.Items.AddRange(new object[] { "所有学生信息", "学生学号搜索", "学生姓名搜索", "未组队学生信息" });
        }

        #region DGV分页
        private void LoadDGV(int currentPage = 1)
        {
            // 按钮点击事件中的查询逻辑
            var query = db.Queryable<DSS_3_8_BIOS>();
            if (comboBox.Text == "所有学生信息")
            {
                ;
            }
            else if (comboBox.Text == "学生学号搜索")
            {
                query = query.Where(it => it.Account == textBox.Text.Trim());
            }
            else if (comboBox.Text == "学生姓名搜索")
            {
                query = query.Where(it => it.StudentName == textBox.Text.Trim());
            }
            else if (comboBox.Text == "未组队学生信息")
            {
                query = query.Where(it => it.Duty == "暂无" || it.Duty == null || it.Duty == "");
            }
            else
            {
                ;
            }

            // 执行分页查询
            int visibleRowCount = dataGridView.Height / dataGridView.RowTemplate.Height;
            int rowCountPerPage = visibleRowCount; // 每页显示的行数与可见行数一致
            int totalCount = query.Count();
            totalPages = (int)Math.Ceiling((double)totalCount / rowCountPerPage);
            currentPage = Math.Min(Math.Max(1, currentPage), totalPages);
            var result = query.ToPageList(currentPage, rowCountPerPage);
            dataGridView.DataSource = result.Select(it => new
            {
                学号 = it.Account,
                学生姓名 = it.StudentName,
                性别 = it.Sex,
                学院 = it.Faculties,
                专业 = it.Specialty,
                年级 = it.Grade,
                班级 = it.Class,
                所在队伍 = it.YourTeam,
                队内职务 = it.Duty,
                指导老师 = it.Instructor
            }).ToList();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentPage = 1; // 跳转到第一页，更新 currentPage
            LoadDGV(currentPage);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--; // 上一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++; // 下一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            currentPage = totalPages; // 跳转到最后一页，更新 currentPage
            LoadDGV(currentPage);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                int pageNumber;
                if (int.TryParse(textBox2.Text, out pageNumber))
                {
                    currentPage = Math.Max(1, Math.Min(totalPages, pageNumber)); // 更新 currentPage
                    LoadDGV(currentPage);
                }
                else
                {
                    MessageBox.Show("页数无效");
                }
            }
        }
        #endregion

    }
}
