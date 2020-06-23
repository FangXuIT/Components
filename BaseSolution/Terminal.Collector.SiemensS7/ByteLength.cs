using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.SiemensS7
{
    public class ByteLength
    {
        public static int Bit = 1;

        public static int Date = 2;

        public static int Int16, Int32, WORD, Counter = 4;

        public static int Int64, DWORD, Real, DateTime = 8;

        public static int Timer = 12;
    }
}
