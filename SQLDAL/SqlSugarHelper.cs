using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using SqlSugar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDAL
{
    public class SqlSugarHelper
    {
        #region 数据库的连接字符串
        private static IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        public static string? Connstring = configuration["AppSettings:DBConnectionString"];
        public static string Connectionstring
        {
            get { return Connstring; }
            set { Connstring = value; }
        }
        #endregion

        #region 获取数据库连接
        public static SqlSugarClient GetSugarClient()
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Connstring,
                DbType = SqlSugar.DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            return db;
        }
        #endregion

        #region 生成数据库表的实体类(注意不要随便使用！)
        public static void CreateClassFile()
        {
            // 建立数据库连接
            string connectionString = Connstring; // 替换成数据库连接字符串

            // 实例化 SugarClient
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = SqlSugar.DbType.SqlServer, // 根据数据库类型更改
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute // 设定初始化主键和自增列信息的方式
            });

            db.DbFirst.IsCreateAttribute().CreateClassFile("D:\\编程练习\\Double-Selection-System\\Model", "Model");
            // 第一个参数是指定生成实体类的命名空间，第二个参数是生成实体类的文件夹路径
        }
        #endregion

        #region 数据查询 指定实体类 传入字典（字段名：值）暂时还用不了
        public static List<T> Queryable<T>(Dictionary<string, object>? conditions = null) where T : class, new()
        {
            using (SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Connstring,
                DbType = SqlSugar.DbType.SqlServer,
                IsAutoCloseConnection = true
            }))
            {
                var query = db.Queryable<T>();
                
                if (conditions != null && conditions.Count > 0)
                {
                    // 根据传入的条件动态构建查询
                    foreach (var condition in conditions)
                    {
                        query = query.Where( it => condition.Key == condition.Value);
                    }
                }
                return query.ToList();
            }
        }
        #endregion


    }
}
