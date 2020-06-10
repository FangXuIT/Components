using System;
using System.Runtime.CompilerServices;
using Test.ThreadTimer.Scan;

namespace Test.ThreadTimer
{
    class Program
    {
        static void Main(string[] args)
        {
            CollectorPool pool = new CollectorPool();
            int idx = 1;
            while (true)
            {
                var key = Console.ReadKey().Key.ToString();
                switch(key)
                {
                    case "r":
                    case "R":
                        Console.WriteLine("");
                        Console.WriteLine("------------------------------");
                        Regist(pool, idx);
                        idx += 1;
                        break;
                }
            }
        }

        static void Regist(CollectorPool pool,int idx)
        {
            Channel channel = new Channel(idx, string.Format("{0}#号PLC", idx));

            for (int idy = 0; idy < 82; idy++)
            {
                Target target = new Target(idy, string.Format("{0}#采集点", idy));
                pool.Regist(channel, target);
            }
        }
    }
}
