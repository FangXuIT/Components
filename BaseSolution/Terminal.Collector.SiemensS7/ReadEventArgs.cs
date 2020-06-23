using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.SiemensS7
{
    public class ReadEventArgs: EventArgs,IDisposable
    {
        public string ReadID { set; get; }

        public int DBNumber { set; get; }

        public DateTime StartTime { set; get; }

        public DateTime EndTime { set; get; }

        public int Result { set; get; }

        public byte[] Data { set; get; }

        public void Dispose()
        {
            ReadID = string.Empty;
            Data = null;
        }
    }
}
