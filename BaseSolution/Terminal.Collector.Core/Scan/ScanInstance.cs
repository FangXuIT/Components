using Coldairarrow.Util;
using PLC.Drive.S7.NetCore;
using PLC.Drive.S7.NetCore.Types;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Collector.IStore;
using Terminal.Collector.IStore.Entites;

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
        private ICollectorStore store;

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

        public Int64 ErrorCount { private set; get; }

        /// <summary>
        /// 扫描状态
        /// </summary>
        public bool ScanEnabled { private set; get; }

        private ScanInstance()
        {
        }

        public ScanInstance(Channel _channel, LogicGroup _group,int _interval, ICollectorStore _store)
        {
            ScanEnabled = false;
            Channel = _channel;
            Group = _group;
            Interval = _interval;
            store = _store;
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
            ErrorCount = 0;
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
                    try
                    {
                        if (page.Value.Count == 1)
                        {
                            var item = page.Value[0];
                            item.Value = TerminalClient.Instance.Read(Channel.Id, item);
                        }
                        else
                        {
                            TerminalClient.Instance.ReadMultipleVars(Channel.Id, page.Value);
                        }
                    }
                    catch(Exception ex)
                    {
                        ErrorCount += 1;
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
                                    await FlushCascadeRelationNodes(time, data, node);
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

        private async Task FlushCascadeRelationNodes(System.DateTime time, DataItem data, TargetNode node)
        {
            if (CascadeConfig.Instance.Relations.ContainsKey(node.Key))
            {
                var rel = CascadeConfig.Instance.Relations[node.Key];
                if (rel.OldValue != data.Value)
                {
                    if (rel.LimitValue != data.Value)
                    {
                        foreach (var relkey in rel.CascadeTargetKey)
                        {
                            try
                            {
                                var target = Channel.Nodes[relkey];
                                target.Value = TerminalClient.Instance.Read(Channel.Id, target);
                                await target.FlushValueAsync(target.Value, time);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Instance.Error(string.Format("FlushCascadeRelationNodes {0}:{1}", relkey, ex.Message));
                                ErrorCount += 1;
                            }
                        }

                        rel.OldValue = data.Value;
                        await SaveBatchDataAsync(node.Key, rel);
                    }
                }
            }
        }

        private async Task SaveBatchDataAsync(string refKey,CascadeRelation rel)
        {
            try
            {
                var dtNow = System.DateTime.Now;
                if (refKey.EndsWith(".ZCSJ"))
                {//装车开始时间
                    var TruckLicense = Channel.Nodes[rel.PrefixTarget + ".CPH"].Value.ToString().Trim();
                    var entity = await store.GetBatchAsync(p => p.LineId == rel.LineId && p.Status == 0 && p.TruckLicense.Trim() == TruckLicense);

                    bool hasRecord = true;
                    if(entity==null)
                    {
                        entity = new Ps_Batch();
                        entity.Id = Convert.ToInt64(System.DateTime.Now.ToString("yyMMddHHmmssfff") + new Random().Next(0, 9999).ToString());
                        entity.LineId = rel.LineId;
                        entity.LoadingWeight = 0;
                        entity.SnatchCount = 0;
                        entity.Status = 0;
                        entity.StartTime = dtNow;
                        entity.ProduceDate = Convert.ToInt32(dtNow.ToString("yyyyMMdd"));

                        hasRecord = false;
                    }                    

                    entity.TruckSizeWidth = Convert.ToInt32(Channel.Nodes[rel.PrefixTarget + ".CXNCKD"].Value);        //车辆车厢内侧宽度
                    entity.TruckSizeLong = Convert.ToInt32(Channel.Nodes[rel.PrefixTarget + ".CXNCCD"].Value);    //车辆车厢内侧长度
                    entity.TruckSizeHeight = Convert.ToInt32(Channel.Nodes[rel.PrefixTarget + ".CKGD"].Value);   //车箱大小（高）
                    entity.TruckLicense = Channel.Nodes[rel.PrefixTarget + ".CPH"].Value.ToString();             //车牌号
                    entity.StackType = Convert.ToInt32(Channel.Nodes[rel.PrefixTarget + ".DXSZ"].Value);         //垛型设置
                    entity.PlanPackages = Convert.ToInt32(Channel.Nodes[rel.PrefixTarget + ".SDZCBS"].Value);    //设定装车包数

                    if (hasRecord)
                        await store.UpdateBatchAsync(entity);
                    else
                        await store.InsertBatchAsync(entity);
                }
                else if (refKey.EndsWith(".ZCWB"))
                {//装车完毕
                    var TruckLicense = Channel.Nodes[rel.PrefixTarget + ".CPH"].Value.ToString().Trim();

                    var entity = await store.GetBatchAsync(p => p.LineId == rel.LineId && p.Status == 0 && p.TruckLicense.Trim() == TruckLicense);
                    entity.Status = 1;
                    entity.EndTime = dtNow;

                    entity.RealPackages = Convert.ToInt32(Channel.Nodes[rel.PrefixTarget + ".JQRSJZQCS"].Value);        //机器人实际抓取次数
                    entity.LoadingWeight = Convert.ToInt32(Channel.Nodes[rel.PrefixTarget + ".DQZCZL"].Value);          //当前装车重量
                    entity.SnatchCount = Convert.ToInt32(Channel.Nodes[rel.PrefixTarget + ".JQRSJZQBS"].Value);         //机器人实际抓取包数
                }
            }
            catch(Exception ex)
            {
                LogHelper.Instance.Error(string.Format("SaveBatchDataAsync {0}:{1}", refKey, ex.Message));
            }
        }
    }
}
