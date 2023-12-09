using Common;
using BLL;
using Model;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;


namespace StudentTerminal
{
    internal static class StudentTerminal
    {
        //判断是通过什么方式进行窗体切换
        public static bool LexitRequested = true;
        public static bool FexitRequested = true;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            while (true)
            {
                LexitRequested = true;
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
                if(!LexitRequested)
                {
                    StudentForm studentForm = new StudentForm();
                    studentForm.ShowDialog();
                }
                else { return; }
                if (FexitRequested) { return; }
                UserHelper.user = null;
                UserHelper.bios = null;
                UserHelper.biot = null;
                UserHelper.bioteam = null;
                UserHelper.tc = null;
            }
        }
    }
}