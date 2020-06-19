using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Collector.Core.Util;
using Terminal.Collector.IStore;
using Terminal.Collector.Store;
using PLC.Drive.S7.NetCore;
using PLC.Drive.S7.NetCore.Types;

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

            var list = store.GetPlcListAsync().Result;

            foreach(var entity in list)
            {
                var channel = new Channel(entity.Id, entity.Name);
                channel.CpuType = entity.CpuType;
                channel.Slot = entity.Slot;
                channel.Rack = entity.Rack;
                channel.Port = entity.Port;
                channel.IP = entity.Ip;
                channel.IsAvailable = false;
                channel.IsConnected = false;

                ChannelList.Add(channel);
            }
        }

        public async Task ConnectAsync()
        {
            foreach(var channel in ChannelList)
            {
                await ConnectAsync(channel);
            }
        }

        public async Task ConnectAsync(Int64 Id)
        {
            var channel = (from u in ChannelList where u.Id == Id select u).FirstOrDefault();
            await ConnectAsync(channel);
        }

        public async Task ConnectAsync(Channel channel)
        {
            if (channel == null) return;

            if (!PlcDic.ContainsKey(channel.Id))
            {
                try
                {
                    var _p = new Plc(DataTypeHelper.GetPlcType(channel.CpuType), channel.IP, channel.Port, channel.Rack, channel.Slot);
                    await _p.OpenAsync();

                    PlcDic.Add(channel.Id, _p);

                    channel.IsAvailable = true;
                    channel.IsConnected = true;//PlcDic[channel.Id].IsConnected;
                }
                catch (Exception ex)
                {
                    channel.Message = ex.ToString();
                    channel.IsAvailable = false;
                    channel.IsConnected = false;
                }
            }
            else
            {
                try
                {
                    await PlcDic[channel.Id].OpenAsync();
                    channel.IsAvailable = true;
                    channel.IsConnected = true;// PlcDic[channel.Id].IsConnected;
                }
                catch (Exception ex)
                {
                    channel.Message = ex.ToString();
                    channel.IsAvailable = false;
                    channel.IsConnected = false;
                }
            }
        }

        public object Read(Int64 plcId, DataItem node)
        {
            if(PlcDic.ContainsKey(plcId))
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
            else
            {
                return null;
            }
        }

        public void ReadMultipleVars(Int64 plcId, List<DataItem> nodes)
        {
            if(PlcDic.ContainsKey(plcId))
            {
                var plc = PlcDic[plcId];
                if (plc.IsConnected)
                {
                    plc.ReadMultipleVars(nodes);
                }
            }
        }

        public void Write(Int64 plcId, TargetNode node)
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
