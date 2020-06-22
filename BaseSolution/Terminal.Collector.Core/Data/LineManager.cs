using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal.Collector.IStore.Entites;

namespace Terminal.Collector.Core.Data
{
    public class LineManager
    {
        public Ps_Batch Item { set; get; }

        /// <summary>
        /// 装车状态
        /// </summary>
        public bool ZCZT { set; get; }

        public string TruckLicense { set; get; }

        public LineManager()
        {
            ZCZT = false;
            Item = new Ps_Batch();
        }
    }
}
