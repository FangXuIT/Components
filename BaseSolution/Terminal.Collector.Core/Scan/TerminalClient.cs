using Opc.Ua;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Collector.Core.Util;
using Terminal.Collector.IStore;
using Terminal.Collector.Store;

namespace Terminal.Collector.Core.Scan
{
    public class TerminalClient:IDisposable
    {
        public static TerminalClient Instance
        {
            get { return Nested.instance; }
        }

        /// <summary>
        /// Plcs
        /// </summary>
        public Dictionary<Int64, Plc> PlcDic { private set; get; }

        public List<Channel> ChannelList { private set; get; }

        private ICollectorStore store;

        // 禁止外部实例化
        private TerminalClient() 
        {
            store = new CollectorStoreImple();
            PlcDic = new Dictionary<Int64, Plc>();
            ChannelList = new List<Channel>();

            var plcs = store.GetPlcListAsync().Result;

            foreach(var plc in plcs)
            {
                var _p = new Plc(DataTypeHelper.GetPlcType(plc.CpuType), plc.Ip, plc.Port, plc.Rack, plc.Slot);
                _p.Open();

                PlcDic.Add(plc.Id, _p);
                ChannelList.Add(new Channel(plc.Id, plc.Name));
            }
        }

        public object Read(Int64 plcId, TargetNode node)
        {
            try
            {
                var plc = PlcDic[plcId];
                if (node.BitAdr == 0)
                {
                    return plc.Read(node.DataType, node.DB, node.StartByteAdr, node.VarType, node.Count);
                }
                else
                {
                    return plc.Read(node.DataType, node.DB, node.StartByteAdr, node.VarType, node.Count, node.BitAdr);
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<object> ReadAsync(Int64 plcId, TargetNode node)
        {
            try
            {
                var plc = PlcDic[plcId];
                if (node.BitAdr == 0)
                {
                    return await plc.ReadAsync(node.DataType, node.DB, node.StartByteAdr, node.VarType, node.Count);
                }
                else
                {
                    return await plc.ReadAsync(node.DataType, node.DB, node.StartByteAdr, node.VarType, node.Count, node.BitAdr);
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public void ReadMultipleVars(Int64 plcId, List<DataItem> nodes)
        {
            try
            {
                var plc = PlcDic[plcId];
                plc.ReadMultipleVars(nodes);
            }
            catch(Exception ex)
            {

            }
        }

        public async Task<List<DataItem>> ReadMultipleVarsAsync(Int64 plcId, List<DataItem> nodes)
        {
            try
            {
                var plc = PlcDic[plcId];

                return await plc.ReadMultipleVarsAsync(nodes);
            }
            catch(Exception ex)
            {
                return new List<DataItem>();
            }
        }

        public void Write(Int64 plcId, TargetNode node)
        {
            try
            {
                var plc = PlcDic[plcId];

                DataItem data = new DataItem();
                data.BitAdr = node.BitAdr;
                data.Count = node.Count;
                data.DataType = node.DataType;
                data.DB = node.DB;
                data.StartByteAdr = node.StartByteAdr;
                data.Value = node.Value;
                data.VarType = node.VarType;

                plc.Write(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task WriteAsync(Int64 plcId, TargetNode node)
        {
            try
            {
                var plc = PlcDic[plcId];

                DataItem data = new DataItem();
                data.BitAdr = node.BitAdr;
                data.Count = node.Count;
                data.DataType = node.DataType;
                data.DB = node.DB;
                data.StartByteAdr = node.StartByteAdr;
                data.Value = node.Value;
                data.VarType = node.VarType;

                await plc.WriteAsync(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Close()
        {
            foreach (var item in PlcDic.Values)
            {
                item.Close();
            }
        }

        public void Dispose()
        {
            Close();
        }

        private class Nested
        {
            internal static readonly TerminalClient instance;

            // 静态构造方法，保证线程安全、延迟加载，且只执行一次
            static Nested()
            {
                instance = new TerminalClient();
            }
        }
    }
}
