using Newtonsoft.Json.Linq;
using SQLDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlSugar;
using Model;

namespace AdminTerminal
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            SqlSugarClient db = SqlSugarHelper.GetSugarClient();
            var select = db
                .Queryable<DSS_3_8_TimeSetting>()
                .Select(it => new
                {
                    时限ID = it.TimeID,
                    时限类型 = it.TimeType,
                    学院 = it.Faculties,
                    专业 = it.Specialty,
                    年级 = it.Grade,
                    开始时间 = it.BeginTime,
                    结束时间 = it.BeginTime
                })
                .ToList();
            dataGridView1.DataSource = select;
        }

    }
}
