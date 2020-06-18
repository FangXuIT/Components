using Opc.Ua;
using Opc.Ua.Configuration;
using PLC.Drive.S7.NetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool IsRuning { private set; get; }

        private CollectorServer()
        {

        }

        public CollectorServer(ICollectorStore _store)
        {
            InstanceList = new List<ScanInstance>();

            application = new ApplicationInstance();
            application.ApplicationType = ApplicationType.Server;
            application.ConfigSectionName = "ZEQP.OpcUAServer";

            store = _store;
        }

        public async Task StartAsync()
        {
            await StartOpcUAServerAsync();
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

                        ScanInstance instance = new ScanInstance(ext, logic, inter);
                        InstanceList.Add(instance);
                    }
                    ext.LogicGroups.Add(logic);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task StartOpcUAServerAsync()
        {
            await application.LoadApplicationConfiguration(false);
            await application.CheckApplicationInstanceCertificate(false, 0);
            await application.Start(new OpcUAServer(this));
        }

        private async Task StartScanAsync()
        {
            foreach(var ins in InstanceList)
            {
                await ins.StartAsync();
            }
        }
    }
}
