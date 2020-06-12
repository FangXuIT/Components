using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core.Scan
{
    /// <summary>
    /// 采集节点逻辑分组
    /// </summary>
    public class LogicGroup
    {
        /// <summary>
        /// 逻辑分组唯一标识
        /// </summary>
        public string Key { private set; get; }

        /// <summary>
        /// 逻辑分组名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 逻辑分组下的扫描频率列表
        /// </summary>
        public List<int> IntervalList { set; get; }

        /// <summary>
        /// 逻辑分组的采集节点列表
        /// key＝扫描频率
        /// value＝采集点唯一标识（Target Address）
        /// </summary>
        public Dictionary<int,List<string>> TargetNodeIdList { set; get; }

        private LogicGroup()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_name">逻辑分组名称</param>
        public LogicGroup(string _name)
        {
            Key = Guid.NewGuid().ToString();
            Name = _name;
            IntervalList = new List<int>();
            TargetNodeIdList = new Dictionary<int, List<string>>();
        }
    }
}
