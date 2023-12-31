using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("DSS_3_8_BIOADM")]
    public partial class DSS_3_8_BIOADM
    {
           public DSS_3_8_BIOADM(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsIdentity=true)]
           public int AdmID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string AdmName {get;set;}

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
           public string Password {get;set;}

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
           public string Contact {get;set;}

    }
}
