using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core.Data
{
    public class TargetData
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { set; get; }

        /// <summary>
        /// 采集点编号
        /// </summary>
        public Int64 TargetId{ set; get; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { set; get; }
    }
}
