using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core.Scan
{
    /// <summary>
    /// PLC Extension
    /// </summary>
    public class Channel
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
        /// Slot
        /// </summary>
        public short Slot { set; get; }

        /// <summary>
        /// Rack
        /// </summary>
        public short Rack { set; get; }

        /// <summary>
        /// CPU
        /// </summary>
        public string CpuType { set; get; }

        /// <summary>
        /// Port
        /// </summary>
        public int Port { set; get; }

        /// <summary>
        /// IP
        /// </summary>
        public string IP { set; get; }

        /// <summary>
        /// IsAvailable
        /// </summary>
        public bool IsAvailable { set; get; }

        /// <summary>
        /// IsConnected
        /// </summary>
        public bool IsConnected { set; get; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Message { set; get; }

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
        public Channel(Int64 id, string name)
        {
            Id = id;
            Name = name;
            LogicGroups = new List<LogicGroup>();
            Nodes = new Dictionary<string, TargetNode>();
        }
    }
}
