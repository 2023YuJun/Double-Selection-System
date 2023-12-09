using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
namespace SQLDAL
{
    public class SqlDbHelper
    {
        private static IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        public static string? Connstring = configuration["AppSettings:DBConnectionString"];
        public static string T1 = "[3_8_User]";
        public static string T2 = "[3_8_BIOS]";
        public static string T3 = "[3_8_BIOT]";
        public static string T4 = "[3_8_BIOTEAM]";
        public static string T5 = "[3_8_Messaging]";
        public static string T6 = "[3_8_TC]";

        // 读取现有的配置文件内容
        public static string ConfigFilePath = "D:\\编程练习\\Double-Selection-System\\SQLDAL\\appsettings.json";
        public static string json = File.ReadAllText(ConfigFilePath);

        //获取数据库连接字符串
        public static string Connectionstring
        {
            get { return Connstring; }
            set { Connstring = value; }
        }

        //非连接式查询，获取多条查询数据，通过适配器直接将查询语句中获取的数据填充到表格容器中
        public static DataTable ExecuteDataTable(string commandText, CommandType commandType, SqlParameter[]? parameters)
        {
            DataTable data = new DataTable();
            using (SqlConnection conn = new SqlConnection(Connstring))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(data);
                }
            }
            return data;
        }
        //重载
        public static DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, CommandType.Text, null);
        }
        public static DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return ExecuteDataTable(commandText, commandType, null);
        }


        //连接式查询，返回一个 SqlDataReader 对象，来逐行读取查询结果，并访问每一行中的各个列的数据。
        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType, SqlParameter[]? parameters)
        {
            SqlConnection conn = new SqlConnection(Connstring);
            SqlCommand cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            conn.Open();
            //CommandBehavior.CloseConnection指示Reader对象关闭时，关闭与其关联的Connection对象
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        //重载
        public static SqlDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text, null);
        }
        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return ExecuteReader(commandText, commandType, null);
        }


        //检索单值,返回cmd查询结果的第一行第一列的值这个返回值的数据类型通常是 SQL 查询结果的标量值（scalar value），例如，一个聚合函数的结果或某一列的单个值。
        public static object ExecuteScalar(string commandText, CommandType commandType, SqlParameter[]? parameters)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(Connstring))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    conn.Open();
                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }
        //重载
        public static object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, CommandType.Text, null);
        }
        public static object ExecuteScalar(string commandText, CommandType commandType)
        {
            return ExecuteScalar(commandText, commandType, null);
        }

        
        //增删改
        public static int ExecuteNonQuery(string commandText, CommandType commandType, SqlParameter[]? parameters)
        {
            int count = 0;
            using(SqlConnection conn = new SqlConnection(Connstring))
            {
                using (SqlCommand cmd = new SqlCommand(commandText,conn))
                {
                    cmd.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach(SqlParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    conn.Open();
                    count = cmd.ExecuteNonQuery();
                }
            }
            return count;
        }
        //重载
        public static int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, null);
        }
        public static int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, commandType, null);
        }

        public static void ImportExcel(string commandText, CommandType commandType, string FileName)
        {
            string excelConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={FileName};Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"";

            // 连接到 Excel 文件
            using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
            {
                excelConnection.Open();

                OleDbCommand cmd = new OleDbCommand(commandText, excelConnection);
                cmd.CommandType = commandType;
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    // 将 Excel 数据插入到 SQL Server 数据库

                    using (SqlConnection sqlConnection = new SqlConnection(Connstring))
                    {
                        sqlConnection.Open();

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                        {
                            bulkCopy.DestinationTableName = "Contact"; // 设置目标表
                            bulkCopy.WriteToServer(dataSet.Tables[0]);
                        }
                    }
                }
            }
        }
    }
}