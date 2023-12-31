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
using Model;

namespace AdminTerminal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        private void button1_Click(object sender, EventArgs e)
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

                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 如果 DataGridView2 没有列标题，复制 DataGridView1 的列标题过来
            if (dataGridView2.ColumnCount == 0)
            {
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    dataGridView2.Columns.Add((DataGridViewColumn)col.Clone());
                }
            }

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];

                bool rowExists = false;
                foreach (DataGridViewRow row in dataGridView2.Rows)
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
                    dataGridView2.Rows.Add(values);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 清空 DataGridView2
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            // 复制 DataGridView1 内容到 DataGridView2
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                dataGridView2.Columns.Add(column.Clone() as DataGridViewColumn);
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                object[] rowData = new object[row.Cells.Count];
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    rowData[i] = row.Cells[i].Value;
                }
                dataGridView2.Rows.Add(rowData);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView2.SelectedCells[0].RowIndex;
                dataGridView2.Rows.RemoveAt(rowIndex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<DSS_3_8_TESTINSERT> data = new List<DSS_3_8_TESTINSERT>();

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DSS_3_8_TESTINSERT item = new DSS_3_8_TESTINSERT
                        {
                            // 根据实际的列名获取值，确保属性名称与实际列名一致
                            Year = Convert.ToString(row.Cells["Year"].Value),
                            Major = Convert.ToString(row.Cells["Major"].Value),
                            ID = Convert.ToString(row.Cells["ID"].Value),
                            Name = Convert.ToString(row.Cells["Name"].Value),
                            // 其他属性根据需要添加...
                        };
                        data.Add(item);
                    }
                }

                if (data.Any())
                {
                    // 插入数据到数据库
                    db.Insertable(data).ExecuteCommand();

                    // 检查表是否存在，避免出现异常
                    if (db.DbMaintenance.IsAnyTable("DSS_3_8_TESTINSERT"))
                    {
                        // 在数据库中删除重复项
                        db.Ado.ExecuteCommand("WITH cte AS (SELECT *, ROW_NUMBER() OVER (PARTITION BY Year, Major, ID, Name ORDER BY Year) AS rn FROM DSS_3_8_TESTINSERT) DELETE FROM cte WHERE rn > 1");
                        MessageBox.Show("数据保存成功");
                    }
                    else
                    {
                        MessageBox.Show("表 DSS_3_8_TESTINSERT 不存在！");
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
    }
}
