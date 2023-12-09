using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Model;
using Common;
using SqlSugar;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SQLDAL
{
    public partial class S_User
    {
        SqlSugarClient db = SqlSugarHelper.GetSugarClient();
        public bool Login(string account, string password)
        {
            bool login = false;
            var SIUser = db.Queryable<DSS_3_8_User>().Where(it => it.Account == account && it.Password == password && it.Grade == "Stu").First();
            var SIBios = db.Queryable<DSS_3_8_BIOS>().Where(it => it.Account == account).First();
            if (SIUser != null && SIBios != null)
            {
                login = true;
                UserHelper.user = SIUser;
                UserHelper.bios = SIBios;
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
