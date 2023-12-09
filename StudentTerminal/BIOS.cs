using Common;
using DocumentFormat.OpenXml.Wordprocessing;
using Model;
using SQLDAL;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentTerminal
{
    public partial class BIOS : Form
    {
        public BIOS()
        {
            InitializeComponent();
        }
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        //private void BIOS_Load(object sender, EventArgs e)
        //{
        //    string cmd = "select * from [3_8_BIOS] where Account=@Account";
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@Account", SqlDbType.NVarChar,50)
        //    };
        //    parameters[0].Value = UserHelper.Account;
        //    SqlDataReader? sdr = null;
        //    sdr = SqlDbHelper.ExecuteReader(cmd, CommandType.Text, parameters);
        //    try
        //    {
        //        if (sdr.Read())
        //        {
        //            textBox1.Text = sdr["StudentName"].ToString();
        //            textBox2.Text = sdr["Sex"].ToString();
        //            textBox3.Text = sdr["Account"].ToString();
        //            textBox4.Text = sdr["Faculties"].ToString();
        //            textBox5.Text = sdr["Specialty"].ToString();
        //            textBox6.Text = sdr["Grade"].ToString();
        //            textBox7.Text = sdr["Class"].ToString();
        //            textBox8.Text = sdr["YourTeam"].ToString();
        //            textBox9.Text = sdr["Duty"].ToString();
        //            textBox10.Text = sdr["Instructor"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        #region sugar用法
        private void BIOS_Load(object sender, EventArgs e)
        {
            #region 通过字典动态传入查询条件，但是没搞好，不知道怎么做先放一下
            //var dic = new Dictionary<string, object>
            //{
            //    { "Account", UserHelper.Account }
            //};
            //var result = SqlSugarHelper.Queryable<DSS_3_8_BIOS>(dic);
            //var result = db.Ado.SqlQuery<dynamic>("select * from [3_8_BIOS] where Account=@Account", new { Account = UserHelper.Account });
            #endregion

            var result = db.Queryable<DSS_3_8_BIOS>().Where(it => it.Account == UserHelper.user.Account).ToList();
            
            if (result != null && result.Count > 0)
            {
                var row = result[0];
                textBox1.Text = row.StudentName.ToString();
                textBox2.Text = row.Sex.ToString();
                textBox3.Text = row.Account.ToString();
                textBox4.Text = row.Faculties.ToString();
                textBox5.Text = row.Specialty.ToString();
                textBox6.Text = row.Grade.ToString();
                textBox7.Text = row.Class.ToString();
                textBox8.Text = row.YourTeam.ToString();
                textBox9.Text = row.Duty.ToString();
                textBox10.Text = row.Instructor.ToString();
            }
        }
        #endregion
    }
}
