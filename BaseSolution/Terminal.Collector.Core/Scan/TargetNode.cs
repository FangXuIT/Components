﻿using Opc.Ua;
using Opc.Ua.Server;
using PLC.Drive.S7.NetCore.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Terminal.Collector.Core.Data;
using Terminal.Collector.Core.Util;

namespace Terminal.Collector.Core.Scan
{
    /// <summary>
    /// 数据采集节点
    /// </summary>
    public class TargetNode : DataItem
    {
        /// <summary>
        /// 采集点唯一标识(Target Address)
        /// </summary>
        public string Key { set; get; }

        /// <summary>
        /// 采集点名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// Opc数据类型
        /// </summary>
        public string OpcNodeType { set; get; }

        /// <summary>
        /// 是否保留历史数据
        /// </summary>
        public bool IsStoreTarget { set; get; }

        /// <summary>
        /// 扫描频率(默认值：1000,单位：毫秒,值为0时不自动进行扫描)
        /// </summary>
        public int Interval { set; get; }

        /// <summary>
        /// 数据更新时间
        /// </summary>
        public System.DateTime Timestamp { set; get; }

        /// <summary>
        ///采集点配置编号
        /// </summary>
        public Int64 ConfigId { private set; get; }

        /// <summary>
        /// 基础实例对象
        /// </summary>
        public BaseObjectState Trigger { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ushort NamespaceIndex { set; get; }

        public PropertyState OpcNode { set; get; }

        /// <summary>
        /// 服务系统上下文
        /// </summary>
        public ServerSystemContext SystemContext { set; get; }

        private TargetNode()
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="_key">采集点唯一标识(Target Address)</param>
        /// <param name="_name">采集点名称</param>
        /// <param name="_configId">采集点配置编号</param>
        /// <param name="_interval">扫描频率(默认值：1000,单位：毫秒)</param>
        /// <param name="_isStoreTarget">是否保留历史数据（默认：false）</param>
        public TargetNode(string _key,string _name,Int64 _configId,int _interval=1000,bool _isStoreTarget=false)
            : base()
        {
            Key = _key;
            Name = _name;
            ConfigId = _configId;
            Interval = _interval;
            IsStoreTarget = _isStoreTarget;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public void FlushValue(object value, System.DateTime time)
        {
            try
            {
                Timestamp = time;
                if(OpcNode!=null)
                {
                    OpcNode.Value = DataTypeHelper.ParseOpcUAValue(OpcNode.DataType, value);
                    OpcNode.StatusCode = StatusCodes.Good;
                    OpcNode.Timestamp = time;
                    OpcNode.ClearChangeMasks(SystemContext, false);
                    if (OpcNode.Value == null)
                    {
                        LogHelper.Instance.Info(string.Format("{0} value is null!", Name));
                    }
                }

                RedisHelper.Set(Key, value);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(string.Format("FlushValueAsync {0}:{1}", Name, ex.Message));
            }
        }
    }
}
