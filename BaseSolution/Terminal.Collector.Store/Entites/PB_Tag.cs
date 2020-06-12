using System;
using System.Linq;
using System.Text;

namespace Terminal.Collector.Store.Entites
{
    ///<summary>
    ///Tag点表
    ///</summary>
    public partial class PB_Tag
    {
           public PB_Tag(){


           }
           /// <summary>
           /// Desc:采集点ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long Id {get;set;}

           /// <summary>
           /// Desc:PLC控制器ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long PlcId {get;set;}

           /// <summary>
           /// Desc:Tag点
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Tag {get;set;}

           /// <summary>
           /// Desc:Tag点名
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string TagName {get;set;}

           /// <summary>
           /// Desc:存储类型	Counter = 28,	Timer = 29,	Input = 129,	Output = 130,	Memory = 131,	DataBlock = 132
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int DataType {get;set;}

           /// <summary>
           /// Desc:变量类型	        Bit = 0,	        Byte = 1,	        Word = 2,	        DWord = 3,	        Int = 4,	        DInt = 5,	        Real = 6,	        String = 7,	        StringEx = 8,	        Timer = 9,	        Counter = 10,	        DateTime = 11
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int VarType {get;set;}

           /// <summary>
           /// Desc:存储数据块
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int DB {get;set;}

           /// <summary>
           /// Desc:偏移量
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int StartByteAdr {get;set;}

           /// <summary>
           /// Desc:Bit类型的索引位
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int BitAdr {get;set;}

           /// <summary>
           /// Desc:长度
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Count {get;set;}

           /// <summary>
           /// Desc:扫描频率
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Interval {get;set;}

           /// <summary>
           /// Desc:读写方式（枚举1 只读 2 只写 3 读写）
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? RWType {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Address {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string OpcNodeType {get;set;}

           /// <summary>
           /// Desc:是否保存历史数据
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int SaveHistory {get;set;}

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
