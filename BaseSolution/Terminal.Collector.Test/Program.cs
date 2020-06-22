using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sharp7;

namespace Sharp7Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //-------------- Create and connect the client
            var client = new S7Client();
            int result = client.ConnectTo("192.168.11.95", 0, 1);
            if (result == 0)
            {
                Console.WriteLine("Connected to 192.168.11.95");
            }
            else
            {
                Console.WriteLine(client.ErrorText(result));
                Console.ReadKey();
                return;
            }

            byte[] Buffer = new byte[65536];
            client.DBRead(7, 0, 65536, Buffer);

            var val1 = S7.GetBitAt(Buffer, 0, 5);
            Console.WriteLine(string.Format("val1={0}", val1));
            //while(true)
            //{
            //    byte[] Buffer = new byte[65536];
            //    client.DBRead(1006, 0, 65536, Buffer);
            //    int Line5_LLJS = S7.GetIntAt(Buffer, 48);
            //    Console.WriteLine(string.Format("Line5_LLJS={0}", Line5_LLJS));

            //    int Line5_SDZCBS= S7.GetIntAt(Buffer, 36);
            //    Console.WriteLine(string.Format("Line5_SDZCBS={0}", Line5_SDZCBS));

            //    Thread.Sleep(1000);
            //}

            client.Disconnect();


            //int CXNCCD = S7.GetIntAt(Buffer, 40);
            //Console.WriteLine(string.Format("CXNCCD={0}", CXNCCD));

            //int CKGD = S7.GetIntAt(Buffer, 6);
            //Console.WriteLine(string.Format("CKGD={0}", CKGD));

            //int SDZCBS = S7.GetIntAt(Buffer, 36);
            //Console.WriteLine(string.Format("SDZCBS={0}", SDZCBS));

            //int JHZQCS = S7.GetIntAt(Buffer, 34);
            //Console.WriteLine(string.Format("JHZQCS={0}", JHZQCS));

            //int DQZCSKZL = S7.GetIntAt(Buffer, 176);
            //Console.WriteLine(string.Format("DQZCSKZL={0}", DQZCSKZL));

            //byte[] Buffer2 = new byte[65536];
            //client.ABRead(0, 65536, Buffer2);
            //int ZCSJ = S7.GetDIntAt(Buffer2, 0);
            //Console.WriteLine(string.Format("ZCSJ={0}", ZCSJ));



            Console.ReadLine();
        }
    }
}
