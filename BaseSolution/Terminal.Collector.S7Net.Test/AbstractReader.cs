using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.S7Net.Test
{
    public abstract class AbstractReader
    {
        /// <summary>
        /// 数据项目
        /// </summary>
        public Dictionary<string, DataItem> Items { set; get; }

        public abstract void Read(Plc _plc);
    }
}
