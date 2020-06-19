using System;
using System.Linq;
using System.Text;

namespace Terminal.Collector.IStore.Entites
{
    ///<summary>
    ///PLC配置
    ///</summary>
    public partial class PB_Plc
    {
           public PB_Plc(){


           }
           /// <summary>
           /// Desc:PLC控制器ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long Id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:IP地址
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Ip {get;set;}

           /// <summary>
           /// Desc:端口
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Port {get;set;}

           /// <summary>
           /// Desc:机架
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Rack {get;set;}

           /// <summary>
           /// Desc:插槽
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Slot {get;set;}

           /// <summary>
           /// Desc:CPU类型
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string CpuType {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

           /// <summary>
           /// Desc:创建人ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long CreatorId {get;set;}

           /// <summary>
           /// Desc:删除状态：0=未删除；1＝已删除；
           /// Default:
           /// Nullable:False
           /// </summary>           
           public byte Deleted {get;set;}

    }
}
