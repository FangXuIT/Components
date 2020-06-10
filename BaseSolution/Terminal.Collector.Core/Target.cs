using Opc.Ua;
using Opc.Ua.Server;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Terminal.Collector.Core.Data;
using Terminal.Collector.Core.Util;

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

        /// <summary>
        /// 是否保留历史数据
        /// </summary>
        public bool IsStoreTarget { set; get; }

        /// <summary>
        /// 基础实例对象
        /// </summary>
        public BaseObjectState Trigger { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ushort NamespaceIndex { set; get; }

        /// <summary>
        /// 数据更新时间
        /// </summary>
        public System.DateTime Timestamp { set; get; }

        /// <summary>
        /// 服务系统上下文
        /// </summary>
        public ServerSystemContext SystemContext { set; get; }

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

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async Task FlushValue(object value, System.DateTime time)
        {
            Timestamp = time;
            if (Value != value)
            {
                //当值有变化时，修改OpcUA Node Value
                var browseName = new QualifiedName(Name, NamespaceIndex);
                var node = Trigger.FindChild(SystemContext, browseName) as PropertyState;
                if (node != null)
                {
                    node.Value = DataTypeHelper.ParseOpcUAValue(node.DataType, value);
                    node.StatusCode = StatusCodes.Good;
                    node.Timestamp = time;
                    node.ClearChangeMasks(SystemContext, false);
                }

                if (IsStoreTarget)
                {//如果需要执行化数据，则加入数据保存队列
                    TargetData data = new TargetData();
                    data.Time = time;
                    data.TargetId = Id;
                    data.Value = value;

                    await DataCenter.Instance().AddQueueAsync(data);
                }
            }
        }
    }
}
