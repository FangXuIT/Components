using Opc.Ua;
using Opc.Ua.Configuration;
using System;
using System.Collections.Generic;
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
        public List<PlcExtension> PlcList { private set; get; }

        public List<ScanInstance> InstanceList { private set; get; }

        private ApplicationInstance application;

        private ICollectorStore store;

        private CollectorServer()
        {

        }

        public CollectorServer(ICollectorStore _store)
        {
            PlcList = new List<PlcExtension>();
            InstanceList = new List<ScanInstance>();

            application = new ApplicationInstance();
            application.ApplicationType = ApplicationType.Server;
            application.ConfigSectionName = "ZEQP.OpcUAServer";

            store = _store;
        }

        public async Task InitScanServerAsync()
        {
            var plcs = await store.GetPlcListAsync();
            var targets = await store.GetTargetListAsync();

            foreach(var plc in plcs)
            {
                PlcExtension ext;
                
                try
                {
                    //PLC实例
                    if (plc.Port > 0) ext = new PlcExtension(plc.Id, plc.Name, DataTypeHelper.GetPlcType(plc.CpuType), plc.Ip, plc.Port, plc.Rack, plc.Slot);
                    else ext = new PlcExtension(plc.Id, plc.Name, DataTypeHelper.GetPlcType(plc.CpuType), plc.Ip, plc.Rack, plc.Slot);

                    //PLC实例下所有节点
                    var nodes = (from u in targets where u.PlcId == plc.Id select new TargetNode(u.Address,u.Name,u.Id,u.Interval,u.IsStoreTarget) { }).ToList();
                    foreach(var node in nodes)
                    {
                        ext.Nodes.Add(node.Key, node);
                    }

                    //逻辑分组
                    LogicGroup logic = new LogicGroup(plc.Name);
                    logic.IntervalList = (from u in nodes select u.Interval).Distinct().ToList();
                    
                    foreach(var inter in logic.IntervalList)
                    {
                        var keys = (from u in nodes where u.Interval == inter select u.Key).ToList();
                        logic.TargetNodeIdList.Add(inter, keys);

                        ScanInstance instance = new ScanInstance(ext, logic, inter);
                        InstanceList.Add(instance);
                    }
                    ext.LogicGroups.Add(logic);
                    await ext.OpenAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task StartOpcUAServerAsync()
        {
            try
            {
                await application.LoadApplicationConfiguration(false);
                await application.CheckApplicationInstanceCertificate(false, 0);
                await application.Start(new OpcUAServer(this));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void StartScan()
        {
            foreach(var ins in InstanceList)
            {
                ins.Start();
            }
        }

        public void Stop()
        {
            application.Stop();
            foreach (var ins in InstanceList)
            {
                ins.Close();
            }
        }
    }
}
