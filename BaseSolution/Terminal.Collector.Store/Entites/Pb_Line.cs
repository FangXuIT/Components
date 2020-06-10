using System;
using System.Linq;
using System.Text;

namespace Terminal.Collector.Store.Entites
{
    ///<summary>
    ///
    ///</summary>
    public partial class Pb_Line
    {
           public Pb_Line(){


           }
           /// <summary>
           /// Desc:生产线ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

           /// <summary>
           /// Desc:生产线
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Line {get;set;}

    }
}
