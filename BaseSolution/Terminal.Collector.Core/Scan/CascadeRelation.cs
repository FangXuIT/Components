using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core.Scan
{
    public class CascadeRelation
    {
        /// <summary>
        /// Plc编号
        /// </summary>
        public Int64 PlcId { private set; get; }

        public Int64 LineId { private set; get; }

        public object LimitValue { set; get; }

        public object OldValue { set; get; }

        /// <summary>
        /// 级联刷新点位标识
        /// </summary>
        public List<string> CascadeTargetKey { set; get; }

        private CascadeRelation()
        {
        }

        public CascadeRelation(Int64 _plcId,Int64 _lineId)
        {
            CascadeTargetKey = new List<string>();
            PlcId = _plcId;
            LineId = _lineId;
        }
    }
}
