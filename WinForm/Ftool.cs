using Common;
using SQLDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Text.Json;

namespace WinForm
{
    public class Ftool
    {
        /// <summary>
        /// 这个类主要作用是集成出一些WinForm项目中的窗体会经常调用的函数
        /// </summary>
        /// 

        public Button? C_btn {  get; set; } = null;
        public static void Cheak_click(Button btn1,Button? btn2 = null)
        {
            //切换侧边栏颜色

            btn1.BackColor = Color.FromArgb(211, 226, 244);
            btn1.ForeColor = Color.Black;
            if (btn2 != null && btn1!=btn2)
            {
                btn2.BackColor = Color.FromArgb(45, 90, 152);
                btn2.ForeColor = Color.White;
            }
        }

        public static void Showform(Panel C_panel,Form form)
        {
            //让子窗体可以在父窗体的panel组件中显示

            C_panel.Controls.Clear();       //清除panel里面的其他窗体
            form.TopLevel = false;      //将该子窗体设置成非顶级控件 窗体本身为顶级控件
            form.FormBorderStyle = FormBorderStyle.None;        //将该子窗体的边框去掉
            form.Dock = DockStyle.Fill;     //设置子窗体随容器大小自动调整
            form.Parent =C_panel;      //设置mdi父容器为当前窗口 this.splitContainer1.Panel2：要显示的panel
            form.Show();        //子窗体显示
        }

        public static void LoadComboBoxData(string tableName ,string columnName, ComboBox comboBox)
        {
            //设置下拉框选项,通过查询列获得不重复的值
            string connstring = SqlDbHelper.Connstring;
            string distinctQuery = $"SELECT DISTINCT {columnName} FROM {tableName}";

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(distinctQuery, conn))
                {
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        comboBox.Items.Clear();
                        while (sdr.Read())
                        {
                            comboBox.Items.Add(sdr[columnName].ToString());
                        }
                    }
                }
            }
        }

        public static bool PostMessage(string SenderID, string MessageType, string? TextContent)
        {
            //消息发送
            string MessageinsertQuery = "INSERT INTO [3_8_Messaging] (SenderID, RecipientID, MessageType, TextContent, Timestamp) " +
                                "VALUES (@SenderID, @RecipientID, @MessageType, @TextContent, @Timestamp)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@SenderID", SqlDbType.NVarChar, 50),
                new SqlParameter("@RecipientID", SqlDbType.NVarChar, 50),
                new SqlParameter("@MessageType", SqlDbType.NVarChar, 50),
                new SqlParameter("@TextContent", SqlDbType.NVarChar, 50),
                new SqlParameter("@Timestamp", SqlDbType.NVarChar, 50),
            };

            parameters[0].Value = UserHelper.user.Account;
            parameters[1].Value = SenderID;
            parameters[2].Value = MessageType;
            parameters[3].Value = TextContent;
            parameters[4].Value = DateTime.Now;

            return SqlDbHelper.ExecuteNonQuery(MessageinsertQuery, CommandType.Text, parameters) > 0;
        }

        
    }
}
