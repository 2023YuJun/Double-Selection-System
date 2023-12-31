using Model;
using SQLDAL;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
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

        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private BindingList<DSS_3_8_BIOADM> deletedRowsList = new BindingList<DSS_3_8_BIOADM>();
        private BindingList<DSS_3_8_BIOADM> bindingList;
        bool columnsMatch = false;
        private int currentPage = 1;
        private int totalPages = 1;
        private void AdmData_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "所有管理员信息", "管理员账号搜索", "管理员姓名搜索" });
            LoadDGV();
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
                DSS_3_8_BIOADM lastDeletedRow = deletedRowsList[deletedRowsList.Count - 1];
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
                    int AdmID = Convert.ToInt32(row.Cells["AdmID"].Value);
                    string AdmName = row.Cells["AdmName"].Value?.ToString();
                    string Account = row.Cells["Account"].Value?.ToString();
                    string Password = row.Cells["Password"].Value?.ToString();
                    string Grade = row.Cells["Grade"].Value?.ToString();
                    string Contact = row.Cells["Contact"].Value?.ToString();

                    // 更新数据到数据库表 DSS_3_8_BIOADM 中
                    // 假设这里是使用 SQLSugar 进行更新操作的示例
                    db.Updateable<DSS_3_8_BIOADM>()
                        .SetColumns(it => new DSS_3_8_BIOADM()
                        {
                            AdmID = AdmID,
                            Account = Account,
                            AdmName = AdmName,
                            Password = Password,
                            Contact = Contact,
                            Grade = Grade,
                        })
                        .Where(t => t.AdmID == AdmID)
                        .ExecuteCommand();
                }

                MessageBox.Show("数据已成功更新到数据库表 DSS_3_8_BIOADM");
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新数据时出现错误：" + ex.Message);
            }
        }
        #endregion

        #region 第二页面
        //单条数据传入
        private void button1_Click(object sender, EventArgs e)
        {
            if (!columnsMatch) return;
            // 如果 DataGridView3 没有列标题，复制 DataGridView1 的列标题过来
            if (dataGridView3.ColumnCount == 0)
            {
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    dataGridView3.Columns.Add((DataGridViewColumn)col.Clone());
                }
            }

            if (dataGridView2.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView2.Rows[selectedRowIndex];

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

        //全部数据传入
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

                object[] rowData = new object[row.Cells.Count];
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    rowData[i] = row.Cells[i].Value;
                }
                dataGridView3.Rows.Add(rowData);

            }
        }

        //单条数据删除
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
                List<DSS_3_8_BIOADM> data = new List<DSS_3_8_BIOADM>();
                List<DSS_3_8_User> users = new List<DSS_3_8_User>();
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DSS_3_8_BIOADM item = new DSS_3_8_BIOADM
                        {
                            AdmName = Convert.ToString(row.Cells["AdmName"].Value),
                            Account = Convert.ToString(row.Cells["Account"].Value),
                            Password = Convert.ToString(row.Cells["Password"].Value),
                            Grade = Convert.ToString(row.Cells["Grade"].Value),
                            Contact = Convert.ToString(row.Cells["Contact"].Value)
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
                            Password = "12345",
                            SecretKey = null,
                            Grade = "Adm"
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
                    if (db.DbMaintenance.IsAnyTable("DSS_3_8_BIOADM"))
                    {
                        // 在数据库中删除重复项
                        db.Ado.ExecuteCommand("WITH cte AS (SELECT *, ROW_NUMBER() OVER (PARTITION BY AdmName, Account, Contact ORDER BY Account) AS rn FROM DSS_3_8_BIOADM) DELETE FROM cte WHERE rn > 1");

                        MessageBox.Show("数据保存成功");
                    }
                    else
                    {
                        MessageBox.Show("表 DSS_3_8_BIOADM 不存在！");
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

        #region DGV分页
        private void LoadDGV(int currentPage = 1)
        {
            var query = db.Queryable<DSS_3_8_BIOADM>();
            if (comboBox1.Text == "管理员账号搜索")
            {
                query = query.Where(t => t.Account == textBox1.Text.Trim());
            }
            else if (comboBox1.Text == "管理员姓名搜索")
            {
                query = query.Where(t => t.AdmName == textBox1.Text.Trim());
            }

            // 执行分页查询
            int visibleRowCount = dataGridView1.Height / dataGridView1.RowTemplate.Height;
            int rowCountPerPage = visibleRowCount; // 每页显示的行数与可见行数一致
            int totalCount = query.Count();
            totalPages = (int)Math.Ceiling((double)totalCount / rowCountPerPage);
            currentPage = Math.Min(Math.Max(1, currentPage), totalPages);

            // 执行查询并将结果转换为 BindingList<T>
            List<DSS_3_8_BIOADM> resultList = query.ToPageList(currentPage, rowCountPerPage).ToList();
            bindingList = new BindingList<DSS_3_8_BIOADM>(resultList);
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


        // 将删除的行数据保存到 BindingList<DSS_3_8_BIOADM> 中
        private void SaveDeletedRowDataToBindingList(DataGridViewRow deletedRow)
        {
            DSS_3_8_BIOADM deletedData = new DSS_3_8_BIOADM();

            // 复制被删除行的数据到 deletedData 对象
            foreach (DataGridViewCell cell in deletedRow.Cells)
            {
                // 根据 DataGridView 中的列顺序将值添加到 deletedData 对象中
                int columnIndex = cell.ColumnIndex;
                if (columnIndex == 0) deletedData.AdmID = Convert.ToInt32(cell.Value);
                else if (columnIndex == 1) deletedData.AdmName = cell.Value?.ToString();
                else if (columnIndex == 2) deletedData.Account = cell.Value?.ToString();
                else if (columnIndex == 3) deletedData.Password = cell.Value?.ToString();
                else if (columnIndex == 4) deletedData.Grade = cell.Value?.ToString();
                else if (columnIndex == 5) deletedData.Contact = cell.Value?.ToString();
            }

            // 将删除的行数据添加到 BindingList
            deletedRowsList.Add(deletedData);
        }
        // 检查列名是否与数据库表的列名匹配
        private bool CheckColumnNames(DataColumnCollection columns)
        {
            List<string> databaseColumnNames = db.DbMaintenance.GetColumnInfosByTableName("DSS_3_8_BIOADM")
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
    }
}
