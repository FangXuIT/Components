using S7.Net.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Collector.Core.Scan
{
    /// <summary>
    /// 扫描实例
    /// </summary>
    public class ScanInstance
    {
        /// <summary>
        /// 扫描定时器
        /// </summary>
        private System.Threading.Timer timerScan;
        private AutoResetEvent autoEvent;

        /// <summary>
        /// Plc
        /// </summary>
        public PlcExtension Channel { private set; get; }

        public LogicGroup Group { private set; get; }

        /// <summary>
        /// 扫描频率(默认值：1000,单位：毫秒)
        /// </summary>
        public int Interval { set; get; }

        /// <summary>
        /// 扫描状态
        /// </summary>
        public bool ScanEnabled { private set; get; }

        private ScanInstance()
        {
        }

        public ScanInstance(PlcExtension _channel, LogicGroup _group,int _interval)
        {
            ScanEnabled = false;
            Channel = _channel;
            Group = _group;
            Interval = _interval;
        }

        /// <summary>
        /// 开始扫描
        /// </summary>
        public void Start()
        {
            if(!ScanEnabled)
            {
                ScanEnabled = true;
                if (timerScan == null)
                {
                    autoEvent = new AutoResetEvent(false);
                    timerScan = new System.Threading.Timer(new TimerCallback(TimeCall), autoEvent, 0, Interval);
                }
                else
                {
                    timerScan.Change(0, Interval);
                }
            }
        }

        /// <summary>
        /// 暂停扫描
        /// </summary>
        public void Pause()
        {
            ScanEnabled = false;
            timerScan.Change(0, -1);
        }

        /// <summary>
        /// 关闭扫描
        /// </summary>
        public void Close()
        {
            ScanEnabled = false;
            timerScan.Change(0, -1);
            timerScan.Dispose();
        }

        /// <summary>
        /// 定时读取
        /// </summary>
        /// <param name="stateInfo"></param>
        void TimeCall(Object stateInfo)
        {
            if(this.Channel.IsAvailable && this.Channel.IsConnected)
            {
                var keys = Group.TargetNodeIdList.GetValueOrDefault(Interval);
                if (keys == null)
                {
                    foreach (var key in keys)
                    {
                        var node = Channel.Nodes[key];
                        ReadItemValueAsync(node);
                    }
                }
            }
        }

        private async Task ReadItemValueAsync(TargetNode node)
        {
            var time = System.DateTime.UtcNow;
            var value = await this.Channel.ReadAsync(node.DataType, node.DB, node.StartByteAdr, node.VarType, node.Count, node.BitAdr);
            await node.FlushValueAsync(value, time);
        }
    }
}
