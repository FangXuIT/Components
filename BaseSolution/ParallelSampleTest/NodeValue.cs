using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelSampleTest
{
    public class NodeValue
    {
        public DateTime Time { private set; get; }

        public string Value { private set; get; }

        public Dictionary<DateTime,string> Log { private set; get; }

        public NodeValue()
        {
            Log = new Dictionary<DateTime, string>();
            Time = DateTime.MinValue;
            Value = string.Empty;
        }

        public void Push(string value)
        {
            if (Time != DateTime.MinValue) Log.Add(Time, value);

            Value = value;
            Time = DateTime.Now;
        }
    }
}
