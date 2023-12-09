using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("DSS_3_8_Guidance")]
    public partial class DSS_3_8_Guidance
    {
           public DSS_3_8_Guidance(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsIdentity=true)]
           public int GuidanceID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Account {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TeacherName {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N'暂无'
           /// Nullable:True
           /// </summary>           
           public string Faculties {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N'暂无'
           /// Nullable:True
           /// </summary>           
           public string Specialty {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N'暂无'
           /// Nullable:True
           /// </summary>           
           public string Grade {get;set;}

    }
}
