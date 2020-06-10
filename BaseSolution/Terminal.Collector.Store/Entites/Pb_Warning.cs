using System;
using System.Linq;
using System.Text;

namespace Terminal.Collector.Store.Entites
{
    ///<summary>
    ///
    ///</summary>
    public partial class Pb_Warning
    {
           public Pb_Warning(){


           }
           /// <summary>
           /// Desc:报警Id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

           /// <summary>
           /// Desc:设备Id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int EquipmentId {get;set;}

           /// <summary>
           /// Desc:报警
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Warning {get;set;}

    }
}
