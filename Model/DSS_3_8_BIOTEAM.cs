using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("DSS_3_8_BIOTEAM")]
    public partial class DSS_3_8_BIOTEAM
    {
           public DSS_3_8_BIOTEAM(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsIdentity=true)]
           public int TeamID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TeamName {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Number {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N'暂无'
           /// Nullable:True
           /// </summary>           
           public string TopicName {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N'暂无'
           /// Nullable:True
           /// </summary>           
           public string TopicIntroduction {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FileName {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string FileDownloadPath {get;set;}

           /// <summary>
           /// Desc:
           /// Default:N'暂无'
           /// Nullable:True
           /// </summary>           
           public string Instructor {get;set;}

    }
}
