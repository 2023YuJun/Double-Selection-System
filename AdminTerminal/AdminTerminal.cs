using Common;
using Model;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;

namespace AdminTerminal
{
    internal static class AdminTerminal
    {
        //�ж���ͨ��ʲô��ʽ���д����л�
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
            //while (true)
            //{
            //    LexitRequested = true;
            //    LoginForm loginForm = new LoginForm();
            //    loginForm.ShowDialog();
            //    if (!LexitRequested)
            //    {
            //        AdminForm adminForm = new AdminForm();
            //        adminForm.ShowDialog();
            //    }
            //    else { return; }
            //    if (FexitRequested) { return; }
            //}
            DataSave dataSave = new DataSave();
            dataSave.ShowDialog();
        }
    }
}