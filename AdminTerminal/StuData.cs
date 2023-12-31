using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.InkML;
using SqlSugar;
using System;
using Model;
using SQLDAL;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Bibliography;
using Common;

namespace AdminTerminal
{
    public partial class StuData : Form
    {
        public StuData()
        {
            InitializeComponent();
        }

        List<List<string>> dataList = new List<List<string>>();
        private const string connectionString = "Server=bzmtxh,top,2433;database=Assign2021;uid=stu;pwd=12344321";

        private void button8_Click(object sender, EventArgs e)
        {
            // 切换Panel的可见性
            panel2.Visible = true;
            panel8.Visible = false;
        }

        //查询
        private void button5_Click(object sender, EventArgs e)
        {

        }


        //导入文件
        private void button4_Click(object sender, EventArgs e)
        {
            if (readfile())
            {
                dataGridView1.DataSource = dataList;
                MessageBox.Show("文件读取成功");
            }
            else
            {
                MessageBox.Show("文件读取失败");
            }

        }
        //保存导入
        private void button10_Click(object sender, EventArgs e)
        {
            void ImportExcelToDatabase(string excelFilePath)
            {
                try
                {
                    using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(excelFilePath, false))
                    {
                        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                        IEnumerable<Sheet> sheets = workbookPart.Workbook.Descendants<Sheet>();

                        // Assuming you are working with the first sheet, change as needed
                        Sheet sheet = sheets.FirstOrDefault();

                        if (sheet != null)
                        {
                            WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                            IEnumerable<Row> rows = worksheetPart.Worksheet.Descendants<Row>();

                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                foreach (Row row in rows)
                                {
                                    IEnumerable<Cell> cells = row.Elements<Cell>();

                                    // Replace with your column indices
                                    string column1Value = GetCellValue(cells, 1, worksheetPart);
                                    string column2Value = GetCellValue(cells, 2, worksheetPart);

                                    // Modify the query based on your table structure
                                    string insertQuery = $"INSERT INTO YourTable (Column1, Column2) VALUES ('{column1Value}', '{column2Value}')";

                                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                    {
                                        command.ExecuteNonQuery();
                                    }
                                }

                                MessageBox.Show("Excel 数据上传成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Excel 文件不包含工作表！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发生错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetCellValue(IEnumerable<Cell> cells, int columnIndex, WorksheetPart worksheetPart)
        {
            Cell cell = cells.FirstOrDefault(c => GetColumnIndex(c.CellReference) == columnIndex);

            if (cell != null)
            {
                string cellValue = cell.InnerText;

                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    int sharedStringIndex = int.Parse(cell.InnerText);
                    SharedStringTablePart sharedStringTablePart = worksheetPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

                    if (sharedStringTablePart != null)
                    {
                        return sharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(sharedStringIndex).InnerText;
                    }
                }

                return cellValue;
            }

            return null;
        }

        private int GetColumnIndex(string cellReference)
        {
            // Extract the column index from the cell reference
            string columnName = new string(cellReference.Where(char.IsLetter).ToArray());
            int columnIndex = 0;

            foreach (char c in columnName)
            {
                columnIndex = columnIndex * 26 + (c - 'A' + 1);
            }

            return columnIndex;
        }





        //单条数据插入
        private void button1_Click(object sender, EventArgs e)
        {

        }

        //全部数据插入
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //删除单条数据
        private void button3_Click(object sender, EventArgs e)
        {

        }

        //导入文件
        public bool readfile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel file|*.xls;*.xlsx";
            openFileDialog1.Title = "Select a Excel File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                string excelConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={FileName};Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"";

                // 连接到 Excel 文件
                using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
                {
                    excelConnection.Open();

                    // 选择要导入的 Excel 表格及列
                    string query = "SELECT * FROM [Sheet1$] WHERE Faculties=@Faculties AND Specialty=@Specialty AND Grade=@Grade ";        // Sheet1 是 Excel 中的工作表名称
                    OleDbCommand cmd = new OleDbCommand(query, excelConnection);
                    cmd.Parameters.AddWithValue("@Faculties", College_comboBox.Text);
                    cmd.Parameters.AddWithValue("@Specialty", Specilaty_comboBox.Text);
                    cmd.Parameters.AddWithValue("@Grade", Grade_comboBox.Text);
                    using (System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            List<string> rowData = new List<string>();

                            foreach (DataColumn column in dataTable.Columns)
                            {
                                rowData.Add(row[column].ToString());
                            }

                            dataList.Add(rowData);
                        }

                    }
                }

                return true;
            }
            else
            {
                return false;
            }

        }


        private void StuData_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "所有学生信息", "学生学号搜索", "学生姓名搜索",
                "队伍名称","导师姓名搜索所带领学生", "导师职工号搜索所带领学生" });
            College_comboBox.Items.AddRange(new object[] { "第二临床医学院", "医学检验学院", "护理学院",
                "药学院","公共卫生学院", "人文与管理学院","生物医学工程学院","外国语学院" });
            Grade_comboBox.Items.AddRange(new object[] { "2020级", "2021级", "2022级", "2023级" });
            Specilaty_comboBox.Items.AddRange(new object[] { "信息管理与信息系统", "生物医学工程学院",
                "智能医学工程", "数据科学与大数据","信息资源管理" });
        }


    }
}

