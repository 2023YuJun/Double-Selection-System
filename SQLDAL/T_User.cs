using Common;
using Model;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDAL
{
    public class T_User
    {
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        public bool Login(string account, string password)
        {
            bool login = false;
            var SIUser = db.Queryable<DSS_3_8_User>().Where(it => it.Account == account && it.Password == password && it.Grade == "Tea").First();
            var SIBiot = db.Queryable<DSS_3_8_BIOT>().Where(it => it.Account == account).First();
            if (SIUser != null && SIBiot != null)
            {
                login = true;
                UserHelper.user = SIUser;
                UserHelper.biot = SIBiot;
            }
            return login;

        }

        public bool Time()
        {
            bool time = false;
            DateTime SysBeginTime;
            DateTime SysEndTime;
            DateTime NowTime = DateTime.Now;
            JObject jsonObj = JObject.Parse(SqlDbHelper.json);
            var SysTime = jsonObj["AppSettings"]["SystemTimeSetting"] as JObject;
            if (SysTime != null && SysTime.HasValues)
            {
                string BeginTime = SysTime["BeginTime"].ToString();
                string EndTime = SysTime["EndTime"].ToString();
                SysBeginTime = DateTime.ParseExact(BeginTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                SysEndTime = DateTime.ParseExact(EndTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                if (NowTime >= SysBeginTime && NowTime <= SysEndTime)
                {
                    time = true;
                }
            }
            else
            {
                time = true;
            }
            return time;
        }
    }
}
