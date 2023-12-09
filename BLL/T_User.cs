using SQLDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class T_User
    {
        SQLDAL.T_User t_user = new SQLDAL.T_User();
        public bool Login(string account, string password)
        {
            return t_user.Login(account, password);
        }
        public bool Time()
        {
            return t_user.Time();
        }
    }
}
