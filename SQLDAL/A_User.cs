using Common;
using Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDAL
{
    public class A_User
    {
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        public bool Login(string account, string password)
        {
            bool login = false;
            var SIUser = db.Queryable<DSS_3_8_User>().Where(it => it.Account == account && it.Password == password && it.Grade == "Adm").First();
            if (SIUser != null)
            {
                login = true;
                UserHelper.user = SIUser;
            }
            return login;

        }
    }
}
