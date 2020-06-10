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
    public class ScanInstance
    {
        /// <summary>
        /// 扫描定时器
        /// </summary>
        private System.Threading.Timer timerScan;
        private AutoResetEvent autoEvent;

        /// <summary>
        /// 扫描通道
        /// </summary>
        public Channel Channel { private set; get; }

        /// <summary>
        /// 扫描频率(默认值：1000,单位：毫秒)
        /// </summary>
        public int Interval { set; get; }

        /// <summary>
        /// 扫描采集点列表
        /// </summary>
        public ConcurrentDictionary<Int64,Target> DataItems { set; get; }

        public ScanInstance(Channel _channel)
        {
            Channel = _channel;
            Interval = 1000;
            DataItems = new ConcurrentDictionary<Int64, Target>();
        }

        /// <summary>
        /// 是否存在指定采集点
        /// </summary>
        /// <param name="_target"></param>
        /// <returns></returns>
        public bool Exist(Target _target)
        {
            return DataItems.ContainsKey(_target.Id);
        }

        /// <summary>
        /// 开始扫描
        /// </summary>
        public void Start()
        {
            if(timerScan==null)
            {
                autoEvent = new AutoResetEvent(false);
                timerScan = new System.Threading.Timer(new TimerCallback(TimeCall), autoEvent, 0, Interval);
            }
            else
            {
                timerScan.Change(0, Interval);
            }
        }

        /// <summary>
        /// 暂停扫描
        /// </summary>
        public void Pause()
        {
            timerScan.Change(0, -1);
        }

        /// <summary>
        /// 关闭扫描
        /// </summary>
        public void Close()
        {
            timerScan.Change(0, -1);
            timerScan.Dispose();
        }

        /// <summary>
        /// 定时读取
        /// </summary>
        /// <param name="stateInfo"></param>
        void TimeCall(Object stateInfo)
        {
            TimeCallBackAsync();
        }

        async Task TimeCallBackAsync()
        {
            if (this.Channel.IsConnected)
            {
                foreach (var item in DataItems.Values)
                {
                    ReadItemValueAsync(item);
                }
            }
        }

        async Task ReadItemValueAsync(Target item)
        {
            item.Value = await this.Channel.ReadAsync(item.DataType, item.DB, item.StartByteAdr, item.VarType, item.Count, item.BitAdr);
        }
    }
}
