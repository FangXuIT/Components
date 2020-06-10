using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.IStore.Models
{
    public class PlcModel
    {
        public Int64 Id { set; get; }

        public string Name { set; get; }

        public string CpuType { set; get; }

        public string Ip { set; get; }

        public int Port { set; get; }

        public short Slot { set; get; }

        public short Rack { set; get; }


    }
}
