using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class A_User
    {
        SQLDAL.A_User a_user = new SQLDAL.A_User();
        public bool Login(string account, string password)
        {
            return a_user.Login(account, password);
        }
    }
}
