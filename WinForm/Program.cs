using Common;
using Model;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;

namespace WinForm
{
    internal static class Program
    {
        //判断是通过什么方式进行窗体切换
        public static bool LexitRequested = false;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //窗体循环
            //while (!LexitRequested)
            //{
            //    LoginForm loginForm = new LoginForm();
            //    loginForm.ShowDialog();
            //    LexitRequested = true;
            //    if (UserHelper.Grade == "Stu")
            //    {

            //        //获取学生姓名，因为监测要用
            //        string getname = "select StudentName from [3_8_BIOS] where Account = @Account";
            //        SqlParameter[] param1 =
            //        {
            //            new SqlParameter("@Account", SqlDbType.NVarChar,50),
            //        };
            //        param1[0].Value = UserHelper.Account;
            //        SqlDataReader? sdr1 = null;
            //        sdr1 = SqlDbHelper.ExecuteReader(getname, CommandType.Text, param1);
            //        if (sdr1.Read())
            //        {
            //            UserHelper.StuName = sdr1["StudentName"].ToString();
            //        }

            //        StudentForm studentForm = new StudentForm();
            //        studentForm.ShowDialog();

            //    }
            //    else if (UserHelper.Grade == "Tea")
            //    {
            //        TeacherForm teacherForm = new TeacherForm();
            //        teacherForm.ShowDialog();
            //    }
            //    else if (UserHelper.Grade == "Adm")
            //    {
            //        AdminForm adminForm = new AdminForm();
            //        adminForm.ShowDialog();
            //    }
            //    UserHelper.Grade = "";
            //}


            //调试用的
            //Application.Run(new LoginForm());
            //Application.Run(new StudentForm());
            //Application.Run(new TeacherForm());
            //Application.Run(new AdminForm());
        }
    }
}