using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.IStore.Models
{
    public class TargetModel
    {
        /// <summary>
        ///采集点编号 (唯一标识)
        /// </summary>
        public Int64 Id { set; get; }

        /// <summary>
        /// Plc编号
        /// </summary>
        public Int64 PlcId { set; get; }

        /// <summary>
        /// 采集点名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 扫描频率(默认值：1000,单位：毫秒)
        /// </summary>
        public int Interval { set; get; }

        /// <summary>
        /// 是否保留历史数据
        /// </summary>
        public bool IsStoreTarget { set; get; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 变量类型
        /// </summary>
        public int VarType { get; set; }

        /// <summary>
        /// DB块
        /// </summary>
        public int DB { get; set; }
        public int StartByteAdr { get; set; }
        public byte BitAdr { get; set; }
        public int Count { get; set; }
    }
}
