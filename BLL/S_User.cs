namespace BLL
{
    public class S_User
    {
        SQLDAL.S_User s_user = new SQLDAL.S_User();
        public bool Login(string account, string password)
        {
            return s_user.Login(account, password);
        }

        public bool Time()
        {
            return s_user.Time();
        }
    }
}