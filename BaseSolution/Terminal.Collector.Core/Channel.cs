using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal.Collector.Core.Scan;

namespace Terminal.Collector.Core
{
    /// <summary>
    /// 数据采集通道
    /// </summary>
    public class Channel: Plc,IDisposable
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
        public Channel(Int64 id,string name,CpuType cpu, string ip, short rack, short slot)
            :base(cpu, ip, rack, slot)
        {
            Instances = new ConcurrentBag<ScanInstance>();
            Id = id;
            Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">采集通道编号</param>
        /// <param name="name">采集通道名称</param>
        /// <param name="cpu">PLC类型</param>
        /// <param name="ip">IP地址</param>
        /// <param name="port">IP地址端口</param>
        /// <param name="rack">机架号</param>
        /// <param name="slot">槽号</param>
        public Channel(Int64 id, string name, CpuType cpu, string ip, int port, short rack, short slot)
            : base(cpu, ip, port, rack, slot)
        {
            Instances = new ConcurrentBag<ScanInstance>();
            Id = id;
            Name = name;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (this.IsConnected)
            {
                this.Close();
            }
        }
    }
}
