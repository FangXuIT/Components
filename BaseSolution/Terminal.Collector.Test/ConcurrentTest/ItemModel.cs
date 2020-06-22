using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Test.ConcurrentTest
{
    public class ItemModel
    {
        public Int64 ID { set; get; }

        public string Name { set; get; }

        public int Value { set; get; }

        public bool Finished { set; get; }
    }
}
