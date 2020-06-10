using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core
{
    /// <summary>
    /// 数据采集点
    /// </summary>
    public class Target: DataItem
    {
        /// <summary>
        ///采集点编号 (唯一标识)
        /// </summary>
        public Int64 Id { private set; get; }

        /// <summary>
        /// 采集点名称
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// 扫描频率(默认值：1000,单位：毫秒)
        /// </summary>
        public int Interval { set; get; }

        private Target()
        {
            Interval = 1000;
        }

        public Target(Int64 id,string name)
            :base()
        {
            Id = id;
            Name = name;
            Interval = 1000;
        }
    }
}
