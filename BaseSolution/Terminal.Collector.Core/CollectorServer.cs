﻿using Coldairarrow.Util;
using Opc.Ua;
using Opc.Ua.Configuration;
using PLC.Drive.S7.NetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Collector.Core.Data;
using Terminal.Collector.Core.OpcUA;
using Terminal.Collector.Core.Scan;
using Terminal.Collector.Core.Util;
using Terminal.Collector.IStore;

namespace Terminal.Collector.Core
{
    public sealed class CollectorServer
    {
        public List<ScanInstance> InstanceList { private set; get; }

        private ApplicationInstance application;

        private ICollectorStore store;

        public bool PushDataToRedis { set; get; }

        public bool EnabledOpcUA { set; get; }

        public bool IsRuning { private set; get; }

        private DataCenter ins;

        private CollectorServer()
        {

        }

        public CollectorServer(ICollectorStore _store)
        {
            InstanceList = new List<ScanInstance>();
            store = _store;
            ins = DataCenter.Instance;
        }

        public async Task StartAsync()
        {
            if(EnabledOpcUA)
            {
                await StartOpcUAServerAsync();
            }

            await StartScanAsync();
            IsRuning = true;
        }

        public async Task StopAsync()
        {
            await Task.Run(() =>
            {
                application.Stop();
                foreach (var ins in InstanceList)
                {
                    ins.Close();
                }
                TerminalClient.Instance.Close();
                IsRuning = false;
            });
        }

        public async Task InitScanServerAsync()
        {
            var targets = await store.GetTargetListAsync();

            foreach (var ext in TerminalClient.Instance.ChannelList)
            {
                try
                {
                    //PLC实例下所有节点
                    var nodes = (from u in targets
                                 where u.PlcId == ext.Id
                                 select new TargetNode(u.Address, u.Name, u.Id, u.Interval, u.IsStoreTarget)
                                 {
                                     DataType = (PLC.Drive.S7.NetCore.DataType)u.DataType,
                                     OpcNodeType= u.OpcNodeType,
                                     VarType = (VarType)u.VarType,
                                     DB = u.DB,
                                     StartByteAdr = u.StartByteAdr,
                                     BitAdr = (byte)u.BitAdr,
                                     Count = u.Count
                                 }).ToList();

                    foreach (var node in nodes)
                    {
                        ext.Nodes.Add(node.Key, node);
                    }

                    //逻辑分组
                    LogicGroup logic = new LogicGroup(ext.Name);
                    logic.IntervalList = (from u in nodes select u.Interval).Distinct().ToList();

                    foreach (var inter in logic.IntervalList)
                    {
                        var keys = (from u in nodes where u.Interval == inter select u.Key).ToList();
                        logic.TargetNodeIdList.Add(inter, keys);

                        ScanInstance instance = new ScanInstance(ext, logic, inter,store);
                        InstanceList.Add(instance);
                    }
                    ext.LogicGroups.Add(logic);
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.Error("InitScanServerAsync:" + ex.Message);
                }
            }
        }

        private async Task StartOpcUAServerAsync()
        {
            try
            {
                application = new ApplicationInstance();
                application.ApplicationType = ApplicationType.Server;
                application.ConfigSectionName = "ZEQP.OpcUAServer";

                await application.LoadApplicationConfiguration(false);
                await application.CheckApplicationInstanceCertificate(false, 0);
                await application.Start(new OpcUAServer(this));
            }
            catch(Exception ex)
            {
                LogHelper.Instance.Error("StartOpcUAServerAsync:" + ex.Message);
            }
        }

        private async Task StartScanAsync()
        {
            foreach(var ins in InstanceList)
            {
                try
                {
                    await ins.StartAsync();
                }
                catch(Exception ex)
                {
                    LogHelper.Instance.Error("StartScanAsync:" + ex.Message);
                }
            }
        }
    }
}
