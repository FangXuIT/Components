using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core
{
    /// <summary>
    /// 数据采集通道
    /// </summary>
    public class Channel: Plc,IDisposable
    {
        public string Name { private set; get; }

        public Channel(string name,CpuType cpu, string ip, short rack, short slot)
            :base(cpu, ip, rack, slot)
        {
            Name = name;
        }

        public Channel(string name, CpuType cpu, string ip, int port, short rack, short slot)
            : base(cpu, ip, port, rack, slot)
        {
            Name = name;
        }

        public void Dispose()
        {
            try
            {
                if (this.IsConnected)
                {
                    this.Close();
                }
            }
            catch(Exception ex)
            {
            }
        }
    }
}
