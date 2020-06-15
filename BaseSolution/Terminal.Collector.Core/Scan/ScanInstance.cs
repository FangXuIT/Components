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
        public Channel Channel { private set; get; }

        public LogicGroup Group { private set; get; }

        public List<DataItem> DataList { private set; get; }

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

        public ScanInstance(Channel _channel, LogicGroup _group,int _interval)
        {
            ScanEnabled = false;
            Channel = _channel;
            Group = _group;
            Interval = _interval;
            DataList = new List<DataItem>();

            var keys = Group.TargetNodeIdList.GetValueOrDefault(Interval);
            foreach (var key in keys)
            {
                var node = Channel.Nodes[key];
                DataList.Add((DataItem)Channel.Nodes[key]);
            }
        }

        /// <summary>
        /// 开始扫描
        /// </summary>
        public void Start()
        {
            if (!ScanEnabled && Interval>0)
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
            TerminalClient.Instance.ReadMultipleVars(Channel.Id, (from u in DataList where u.VarType == S7.Net.VarType.Bit select u).ToList());
            TerminalClient.Instance.ReadMultipleVars(Channel.Id, (from u in DataList where u.VarType == S7.Net.VarType.Word select u).ToList());
            TerminalClient.Instance.ReadMultipleVars(Channel.Id, (from u in DataList where u.VarType == S7.Net.VarType.Int select u).ToList());
            TerminalClient.Instance.ReadMultipleVars(Channel.Id, (from u in DataList where u.VarType == S7.Net.VarType.DInt select u).ToList());
            TerminalClient.Instance.ReadMultipleVars(Channel.Id, (from u in DataList where u.VarType == S7.Net.VarType.Real select u).ToList());
            TerminalClient.Instance.ReadMultipleVars(Channel.Id, (from u in DataList where u.VarType == S7.Net.VarType.String select u).ToList());
            FlushValueAsync();
        }

        private async Task FlushValueAsync()
        {
            var time = System.DateTime.UtcNow;
            try
            {
                foreach(var data in DataList)
                {
                    foreach(var node in Channel.Nodes.Values)
                    {
                        if(node.DataType==data.DataType
                            && node.VarType==data.VarType
                            && node.BitAdr==data.BitAdr
                            && node.Count==data.Count
                            && node.DB==data.DB
                            && node.StartByteAdr==data.StartByteAdr)
                        {
                            await node.FlushValueAsync(data.Value, time);
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
