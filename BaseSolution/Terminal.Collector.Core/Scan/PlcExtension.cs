using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core.Scan
{
    /// <summary>
    /// PLC Extension
    /// </summary>
    public class PlcExtension : Plc
    {
        /// <summary>
        ///Plc编号 (唯一标识)
        /// </summary>
        public Int64 Id { private set; get; }

        /// <summary>
        /// Plc名称
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// Plc下的逻辑分组
        /// </summary>
        public List<LogicGroup> LogicGroups { set; get; }

        /// <summary>
        /// 采集节点列表
        /// </summary>
        public Dictionary<string,TargetNode> Nodes { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">采集通道编号</param>
        /// <param name="name">采集通道名称</param>
        /// <param name="cpu">PLC类型</param>
        /// <param name="ip">IP地址</param>
        /// <param name="rack">机架号</param>
        /// <param name="slot">槽号</param>
        public PlcExtension(Int64 id, string name, CpuType cpu, string ip, short rack, short slot)
            : base(cpu, ip, rack, slot)
        {
            Id = id;
            Name = name;
            LogicGroups = new List<LogicGroup>();
            Nodes = new Dictionary<string, TargetNode>();
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
        public PlcExtension(Int64 id, string name, CpuType cpu, string ip, int port, short rack, short slot)
            : base(cpu, ip, port, rack, slot)
        {
            Id = id;
            Name = name;
            LogicGroups = new List<LogicGroup>();
            Nodes = new Dictionary<string, TargetNode>();
        }
    }
}
