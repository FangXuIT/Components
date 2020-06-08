using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core
{
    /// <summary>
    /// 数据采集点
    /// </summary>
    public class Target: DataItem
    {
        public string Name { set; get; }
    }
}
