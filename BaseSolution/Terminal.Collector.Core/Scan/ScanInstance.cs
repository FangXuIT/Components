﻿using PLC.Drive.S7.NetCore;
using PLC.Drive.S7.NetCore.Types;
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

        public Dictionary<VarType,Dictionary<int, List<DataItem>>> DataList { private set; get; }

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
            DataList = new Dictionary<VarType, Dictionary<int, List<DataItem>>>();

            var keys = Group.TargetNodeIdList.GetValueOrDefault(Interval);
            foreach (var key in keys)
            {
                var node = Channel.Nodes[key];

                if (!DataList.ContainsKey(node.VarType))
                {
                    var pageData = new Dictionary<int, List<DataItem>>();
                    pageData.Add(1, new List<DataItem>());
                    pageData[1].Add((DataItem)Channel.Nodes[key]);

                    DataList.Add(node.VarType, pageData);
                }
                else
                {
                    if(node.VarType== VarType.String || node.VarType==VarType.StringEx)
                    {
                        var iPageIndex = DataList[node.VarType].Keys.Last();
                        if (DataList[node.VarType][iPageIndex].Count >= 1)
                        {
                            var iNewPage = iPageIndex + 1;
                            DataList[node.VarType].Add(iNewPage, new List<DataItem>());
                            DataList[node.VarType][iNewPage].Add((DataItem)Channel.Nodes[key]);
                        }
                        else
                        {
                            DataList[node.VarType][iPageIndex].Add((DataItem)Channel.Nodes[key]);
                        }
                    }
                    else
                    {
                        var iPageIndex = DataList[node.VarType].Keys.Last();
                        if (DataList[node.VarType][iPageIndex].Count >= 10)
                        {
                            var iNewPage = iPageIndex + 1;
                            DataList[node.VarType].Add(iNewPage, new List<DataItem>());
                            DataList[node.VarType][iNewPage].Add((DataItem)Channel.Nodes[key]);
                        }
                        else
                        {
                            DataList[node.VarType][iPageIndex].Add((DataItem)Channel.Nodes[key]);
                        }
                    }
                    
                }
            }
        }

        /// <summary>
        /// 开始扫描
        /// </summary>
        public async Task StartAsync()
        {
            await TerminalClient.Instance.ConnectAsync(Channel);
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
            foreach(var dic in DataList)
            {
                foreach(var page in dic.Value)
                {
                    if(page.Value.Count==1)
                    {
                        var item = page.Value[0];
                        item.Value = TerminalClient.Instance.Read(Channel.Id, item);
                    }
                    else
                    {
                        TerminalClient.Instance.ReadMultipleVars(Channel.Id, page.Value);
                    }                    
                }
            }
            FlushValueAsync();
        }

        private async Task FlushValueAsync()
        {
            var time = System.DateTime.UtcNow;
            try
            {
                foreach(var dir in DataList)
                {
                    foreach(var page in dir.Value)
                    {
                        foreach (var data in page.Value)
                        {
                            foreach (var node in Channel.Nodes.Values)
                            {
                                if (node.DataType == data.DataType
                                    && node.VarType == data.VarType
                                    && node.BitAdr == data.BitAdr
                                    && node.Count == data.Count
                                    && node.DB == data.DB
                                    && node.StartByteAdr == data.StartByteAdr)
                                {
                                    await node.FlushValueAsync(data.Value, time);
                                    
                                    if(CascadeConfig.Instance.Relations.ContainsKey(node.Key))
                                    {
                                        var rel = CascadeConfig.Instance.Relations[node.Key];
                                        if (rel.OldValue!=data.Value)
                                        {
                                            if(rel.LimitValue != data.Value)
                                            {
                                                foreach(var relkey in rel.CascadeTargetKey)
                                                {
                                                    var target = Channel.Nodes[relkey];
                                                    TerminalClient.Instance.Read(Channel.Id, target);
                                                }
                                            }
                                            rel.OldValue = data.Value;
                                        }
                                    }
                                }
                            }
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
