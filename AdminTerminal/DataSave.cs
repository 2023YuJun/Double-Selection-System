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
    public partial class DataSave : Form
    {
        public DataSave()
        {
            InitializeComponent();
        }
        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void DataSave_Load(object sender, EventArgs e)
        {
            LoadTableNamesToComboBox();

        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string tableName = comboBox1.SelectedItem.ToString(); ; // 替换成你的表名 // 查询表格信息并将结果加载到 DataGridView 中
            string sql = $"SELECT * FROM {tableName}"; System.Data.DataTable dataTable = db.Ado.GetDataTable(sql); // 将查询结果绑定到 DataGridView
            dataGridView1.DataSource = dataTable;
        }
        private void LoadTableNamesToComboBox()
        {
            try
            {
                // 获取数据库中的所有表格名称
                List<string> tableNames = db.DbMaintenance.GetTableInfoList().Select(t => t.Name).ToList();
                tableNames = tableNames.Where(s => s.StartsWith("DSS_3_8_")).ToList();

                // 将表格名称填入 ComboBox
                comboBox1.DataSource = tableNames;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取要导出的表格数据
                DataTable table = (DataTable)dataGridView1.DataSource;

                if (table != null && table.Rows.Count > 0)
                {
                    // 获取保存路径
                    string directoryPath = textBox1.Text;

                    // 判断路径是否为空
                    if (string.IsNullOrEmpty(directoryPath))
                    {
                        MessageBox.Show("请指定保存路径！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 确保目录存在
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // 获取选中表格的名称
                    string selectedTableName = comboBox1.SelectedItem.ToString();

                    // 构建 Excel 文件路径
                    string excelFilePath = Path.Combine(directoryPath, $"{selectedTableName}.xlsx");

                    // 创建 Excel 文件
                    CreateExcelFile(excelFilePath, table);

                    MessageBox.Show("导出成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("没有可导出的数据！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //导出全部表为Excel
        }
        private void CreateExcelFile(string filePath, DataTable dataTable)
        {
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{filePath}';Extended Properties='Excel 12.0 Xml;HDR=YES;'";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                // 创建 Excel 表格
                using (OleDbCommand createTableCommand = new OleDbCommand())
                {
                    createTableCommand.Connection = connection;
                    createTableCommand.CommandText = $"CREATE TABLE [Sheet1] ({GenerateTableColumns(dataTable)})";
                    createTableCommand.ExecuteNonQuery();
                }

                // 插入数据
                using (OleDbCommand insertCommand = new OleDbCommand())
                {
                    insertCommand.Connection = connection;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        insertCommand.CommandText = $"INSERT INTO [Sheet1] VALUES ({GenerateRowValues(row)})";
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private string GenerateTableColumns(DataTable dataTable)
        {
            string columns = "";
            foreach (DataColumn column in dataTable.Columns)
            {
                columns += $"[{column.ColumnName}] TEXT, ";
            }
            return columns.TrimEnd(' ', ',');
        }

        private string GenerateRowValues(DataRow row)
        {
            string values = "";
            foreach (var item in row.ItemArray)
            {
                values += $"'{item.ToString().Replace("'", "''")}', ";
            }
            return values.TrimEnd(' ', ',');
        }

        
    }
}
