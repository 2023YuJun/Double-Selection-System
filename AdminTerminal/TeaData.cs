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
using System.Data.OleDb;

namespace AdminTerminal
{
    public partial class TeaData : Form
    {
        public TeaData()
        {
            InitializeComponent();
        }
        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private BindingList<DSS_3_8_BIOT> deletedRowsList = new BindingList<DSS_3_8_BIOT>();
        private BindingList<DSS_3_8_BIOT> bindingList;
        bool columnsMatch = false;
        private int currentPage = 1;
        private int totalPages = 1;
        private void TeaData_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "所有导师信息", "导师职工号搜索", "导师姓名搜索","带领队伍搜索" });
            LoadDGV();
        }
        #region 导入保存页面

        //第一个右箭头
        private void button1_Click(object sender, EventArgs e)
        {
            if (!columnsMatch) return;
            // 如果 DataGridView2 没有列标题，复制 DataGridView1 的列标题过来
            if (dataGridView3.ColumnCount == 0)
            {
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    dataGridView3.Columns.Add((DataGridViewColumn)col.Clone());
                }
            }

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView2.Rows[selectedRowIndex];

                // 检查单元格所在行对应列的元素是否等同于 comboBox 的元素
                string faculty = College_comboBox.Text;
                string Specialty = Specialty_comboBox.Text;

                string selectedFaculty = selectedRow.Cells["Faculties"].Value?.ToString();
                string selectedSpecialty = selectedRow.Cells["Specialty"].Value?.ToString();

                if ((faculty != "All" && selectedFaculty != faculty) ||
                    (Specialty != "All" && selectedSpecialty != Specialty))
                {
                    MessageBox.Show("所选行的元素与限制中的元素不匹配，请重新选择行或更改限制的值。");
                    return;
                }

                bool rowExists = false;
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (row.Cells.Cast<DataGridViewCell>().All(c => c.Value != null && c.Value.ToString() == selectedRow.Cells[c.ColumnIndex].Value?.ToString()))
                    {
                        rowExists = true;
                        break;
                    }
                }

                if (!rowExists)
                {
                    object[] values = new object[selectedRow.Cells.Count];
                    for (int i = 0; i < selectedRow.Cells.Count; i++)
                    {
                        values[i] = selectedRow.Cells[i].Value;
                    }
                    dataGridView3.Rows.Add(values);
                }
            }
        }

        //第二个右箭头
        private void button2_Click(object sender, EventArgs e)
        {
            if (!columnsMatch) return;
            // 清空 DataGridView3
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();

            // 复制 DataGridView2 内容到 DataGridView3
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                dataGridView3.Columns.Add(column.Clone() as DataGridViewColumn);
            }

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                bool rowShouldCopy = true;

                // 检查单元格所在行对应列的元素是否等同于 comboBox 的元素
                string faculty = College_comboBox.Text;
                string Specialty = Specialty_comboBox.Text;

                string rowFaculty = row.Cells["Faculties"].Value?.ToString();
                string rowSpecialty = row.Cells["Specialty"].Value?.ToString();

                if ((faculty != "All" && rowFaculty != faculty) ||
                    (Specialty != "All" && rowSpecialty != Specialty))
                {
                    rowShouldCopy = false;
                }

                if (rowShouldCopy)
                {
                    object[] rowData = new object[row.Cells.Count];
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        rowData[i] = row.Cells[i].Value;
                    }
                    dataGridView3.Rows.Add(rowData);
                }
            }
        }

        //垃圾桶
        private void button3_Click(object sender, EventArgs e)
        {
            if (!columnsMatch) return;
            if (dataGridView3.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView3.SelectedCells[0].RowIndex;
                dataGridView3.Rows.RemoveAt(rowIndex);
            }
        }

        //导入文件
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                string connectionString = "";
                if (Path.GetExtension(filePath).Equals(".xls"))
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                }
                else if (Path.GetExtension(filePath).Equals(".xlsx"))
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
                }

                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        DataTable dataTable = new DataTable();

                        // 获取 Excel 文件的第一个工作表名
                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = schemaTable?.Rows?[0]?["TABLE_NAME"]?.ToString();

                        if (!string.IsNullOrEmpty(sheetName))
                        {
                            string query = "SELECT * FROM [" + sheetName + "]";
                            using (System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(query, connection))
                            {
                                adapter.Fill(dataTable);
                            }
                        }

                        connection.Close();

                        // 检查列名是否匹配
                        columnsMatch = CheckColumnNames(dataTable.Columns);

                        if (columnsMatch)
                        {
                            dataGridView2.DataSource = dataTable;

                            // 获取 Faculties、Specialty 和 Specialty 列的不重复项
                            List<string> faculties = GetDistinctValues(dataTable, "Faculties");
                            List<string> specialties = GetDistinctValues(dataTable, "Specialty");

                            // 在不重复项列表中添加 "All" 选项作为第一项
                            faculties.Insert(0, "All");
                            specialties.Insert(0, "All");

                            // 将不重复项分别绑定到 ComboBox 控件
                            College_comboBox.DataSource = faculties;
                            Specialty_comboBox.DataSource = specialties;

                        }
                        else
                        {
                            MessageBox.Show("Excel 表格的列标题与数据库表的列不匹配，请检查表格数据");
                        }
                    }
                }
            }
        }

        //保存
        private void button9_Click(object sender, EventArgs e)
        {
            if (!columnsMatch) return;
            try
            {
                List<DSS_3_8_BIOT> data = new List<DSS_3_8_BIOT>();
                List<DSS_3_8_User> users = new List<DSS_3_8_User>();
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DSS_3_8_BIOT item = new DSS_3_8_BIOT
                        {
                            TeacherName = Convert.ToString(row.Cells["TeacherName"].Value),
                            Sex = Convert.ToString(row.Cells["Sex"].Value),
                            Account = Convert.ToString(row.Cells["Account"].Value),
                            Faculties = Convert.ToString(row.Cells["Faculties"].Value),
                            Specialty = Convert.ToString(row.Cells["Specialty"].Value),
                            LeadTeam = Convert.ToString(row.Cells["LeadTeam"].Value),
                            PersonalDeeds = Convert.ToString(row.Cells["PersonalDeeds"].Value),
                            Contact = Convert.ToString(row.Cells["Contact"].Value),
                        };
                        data.Add(item);
                    }
                    string account = Convert.ToString(row.Cells["Account"].Value);
                    if (!db.Queryable<DSS_3_8_User>().Any(u => u.Account == account))
                    {
                        // 如果 DSS_3_8_User 表中不存在相同 Account 的记录，则创建新的 DSS_3_8_User 对象并添加到列表中
                        DSS_3_8_User user = new DSS_3_8_User
                        {
                            Account = account,
                            Password = "1234",
                            SecretKey = "null",
                            Grade = "Tea"
                            // 添加属性赋值
                        };
                        users.Add(user);
                    }
                }

                if (data.Any())
                {
                    // 插入数据到数据库
                    db.Insertable(data).ExecuteCommand();

                    if (users.Any())
                    {
                        db.Insertable(users).ExecuteCommand();
                    }
                    // 检查表是否存在，避免出现异常
                    if (db.DbMaintenance.IsAnyTable("DSS_3_8_BIOT"))
                    {
                        // 在数据库中删除重复项
                        db.Ado.ExecuteCommand("WITH cte AS (SELECT *, ROW_NUMBER() OVER (PARTITION BY TeacherName, Account, Contact ORDER BY Account) AS rn FROM DSS_3_8_BIOT) DELETE FROM cte WHERE rn > 1");

                        MessageBox.Show("数据保存成功");
                    }
                    else
                    {
                        MessageBox.Show("表 DSS_3_8_BIOT 不存在！");
                    }
                }
                else
                {
                    MessageBox.Show("DataGridView2 中没有数据可供插入！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败：" + ex.Message);
            }
        }
        #endregion

        #region 信息查询页
        // 切换Panel的可见性
        private void button8_Click(object sender, EventArgs e)
        {

            panel2.Visible = true;
            panel8.Visible = false;
        }
        //查询
        private void button5_Click(object sender, EventArgs e)
        {
            LoadDGV();
        }

        //删除
        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                // 将被删除的行数据保存到 DataTable
                SaveDeletedRowDataToBindingList(dataGridView1.Rows[rowIndex]);

                dataGridView1.Rows.RemoveAt(rowIndex);
            }
        }

        //撤销
        private void button10_Click(object sender, EventArgs e)
        {
            if (deletedRowsList.Count > 0)
            {
                // 获取最后一个被删除的数据
                DSS_3_8_BIOT lastDeletedRow = deletedRowsList[deletedRowsList.Count - 1];
                bindingList.Add(lastDeletedRow);
                // 重新设置 DataGridView 的数据源
                dataGridView3.DataSource = null;
                dataGridView3.DataSource = bindingList;

                // 从 BindingList 中移除最后一行数据
                deletedRowsList.RemoveAt(deletedRowsList.Count - 1);
            }
            else
            {
                MessageBox.Show("没有可以恢复的数据");
            }
        }

        //保存
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    int TeacherID = Convert.ToInt32(row.Cells["TeacherID"].Value);
                    string TeacherName = row.Cells["TeacherName"].Value?.ToString();
                    string Sex = row.Cells["Sex"].Value?.ToString();
                    string Account = row.Cells["Account"].Value?.ToString();
                    string Faculties = row.Cells["Faculties"].Value?.ToString();
                    string Specialty = row.Cells["Specialty"].Value?.ToString();
                    string LeadTeam = row.Cells["LeadTeam"].Value?.ToString();
                    string PersonalDeeds = row.Cells["PersonalDeeds"].Value?.ToString();
                    string Contact = row.Cells["Contact"].Value?.ToString();

                    // 更新数据到数据库表 DSS_3_8_BIOT 中
                    // 假设这里是使用 SQLSugar 进行更新操作的示例
                    db.Updateable<DSS_3_8_BIOT>()
                        .SetColumns(it => new DSS_3_8_BIOT()
                        {
                            TeacherID = TeacherID,
                            TeacherName = Account,
                            Sex = Sex,
                            Account = Account,
                            Faculties = Faculties,
                            Specialty = Specialty,
                            LeadTeam = LeadTeam,
                            PersonalDeeds = PersonalDeeds,
                            Contact = Contact,
                        })
                        .Where(t => t.TeacherID == TeacherID)
                        .ExecuteCommand();
                }

                MessageBox.Show("数据已成功更新到数据库表 DSS_3_8_BIOT");
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新数据时出现错误：" + ex.Message);
            }
        }
        #endregion

        #region DGV分页
        private void LoadDGV(int currentPage = 1)
        {
            var query = db.Queryable<DSS_3_8_BIOT>();
            if (comboBox1.Text == "导师职工号搜索")
            {
                query = query.Where(t => t.Account == textBox1.Text.Trim());
            }
            else if (comboBox1.Text == "导师姓名搜索")
            {
                query = query.Where(t => t.TeacherName == textBox1.Text.Trim());
            }
            else if (comboBox1.Text == "带领队伍搜索")
            {
                query = query.Where(t => t.LeadTeam == textBox1.Text.Trim());
            }

            // 执行分页查询
            int visibleRowCount = dataGridView1.Height / dataGridView1.RowTemplate.Height;
            int rowCountPerPage = visibleRowCount; // 每页显示的行数与可见行数一致
            int totalCount = query.Count();
            totalPages = (int)Math.Ceiling((double)totalCount / rowCountPerPage);
            currentPage = Math.Min(Math.Max(1, currentPage), totalPages);

            // 执行查询并将结果转换为 BindingList<T>
            List<DSS_3_8_BIOT> resultList = query.ToPageList(currentPage, rowCountPerPage).ToList();
            bindingList = new BindingList<DSS_3_8_BIOT>(resultList);
            // 将 BindingList 设置为 DataGridView 的数据源
            dataGridView1.DataSource = bindingList;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            currentPage = 1; // 跳转到第一页，更新 currentPage
            LoadDGV(currentPage);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--; // 上一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++; // 下一页，更新 currentPage
                LoadDGV(currentPage);
            }
        }

        private void button23_Click(object sender, EventArgs e)
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

        // 将删除的行数据保存到 BindingList<DSS_3_8_BIOT> 中
        private void SaveDeletedRowDataToBindingList(DataGridViewRow deletedRow)
        {
            DSS_3_8_BIOT deletedData = new DSS_3_8_BIOT();

            // 复制被删除行的数据到 deletedData 对象
            foreach (DataGridViewCell cell in deletedRow.Cells)
            {
                // 根据 DataGridView 中的列顺序将值添加到 deletedData 对象中
                int columnIndex = cell.ColumnIndex;
                if (columnIndex == 0) deletedData.TeacherID = Convert.ToInt32(cell.Value);
                else if (columnIndex == 1) deletedData.TeacherName = cell.Value?.ToString();
                else if (columnIndex == 2) deletedData.Account = cell.Value?.ToString();
                else if (columnIndex == 3) deletedData.Sex = cell.Value?.ToString();
                else if (columnIndex == 4) deletedData.Faculties = cell.Value?.ToString();
                else if (columnIndex == 5) deletedData.Specialty = cell.Value?.ToString();
                else if (columnIndex == 6) deletedData.LeadTeam = cell.Value?.ToString();
                else if (columnIndex == 7) deletedData.PersonalDeeds = cell.Value?.ToString();
                else if (columnIndex == 8) deletedData.Contact = cell.Value?.ToString();
            }

            // 将删除的行数据添加到 BindingList
            deletedRowsList.Add(deletedData);
        }
        // 检查列名是否与数据库表的列名匹配
        private bool CheckColumnNames(DataColumnCollection columns)
        {
            List<string> databaseColumnNames = db.DbMaintenance.GetColumnInfosByTableName("DSS_3_8_BIOT")
                            .Select(c => c.DbColumnName)
                            .Where(name => !name.Contains("ID"))
                            .ToList();

            // 获取 Excel 表格的列名
            List<string> excelColumnNames = columns.Cast<DataColumn>()
                             .Select(column => column.ColumnName)
                             .ToList();

            // 检查 Excel 表格的列名是否与数据库表的列名一一对应
            return databaseColumnNames.SequenceEqual(excelColumnNames);
        }

        // 获取列的不重复项
        private List<string> GetDistinctValues(DataTable dataTable, string columnName)
        {
            List<string> distinctValues = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                string value = row[columnName]?.ToString();
                if (!string.IsNullOrEmpty(value) && !distinctValues.Contains(value))
                {
                    distinctValues.Add(value);
                }
            }

            return distinctValues;
        }
    }
}
