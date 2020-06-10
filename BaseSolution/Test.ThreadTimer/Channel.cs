using System;
using System.Collections.Concurrent;
using Test.ThreadTimer.Scan;

namespace Test.ThreadTimer
{
    /// <summary>
    /// 数据采集通道
    /// </summary>
    public class Channel
    {
        /// <summary>
        ///采集通道编号 (唯一标识)
        /// </summary>
        public Int64 Id { private set; get; }

        /// <summary>
        /// 采集通道名称
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// 扫描配置项列表
        /// </summary>
        public ConcurrentBag<ScanInstance> Instances { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">采集通道编号</param>
        /// <param name="name">采集通道名称</param>
        /// <param name="cpu">PLC类型</param>
        /// <param name="ip">IP地址</param>
        /// <param name="rack">机架号</param>
        /// <param name="slot">槽号</param>
        public Channel(Int64 id,string name)
        {
            Instances = new ConcurrentBag<ScanInstance>();
            Id = id;
            Name = name;
        }
    }
}
