using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("DSS_3_8_BIOS")]
    public partial class DSS_3_8_BIOS
    {
           public DSS_3_8_BIOS(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsIdentity=true)]
           public int StudentID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string StudentName {get;set;}

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
           public string Sex {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Faculties {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Specialty {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Grade {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Class {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N'暂无'
           /// Nullable:True
           /// </summary>           
           public string YourTeam {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N'暂无'
           /// Nullable:True
           /// </summary>           
           public string Duty {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N'暂无'
           /// Nullable:True
           /// </summary>           
           public string Instructor {get;set;}

    }
}
