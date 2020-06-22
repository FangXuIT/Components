using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using org.apache.zookeeper.data;
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

            while(true)
            {
                byte[] Buffer = new byte[65536];
                client.DBRead(1000, 12, 65536, Buffer);
                int SDZCCS = S7.GetDIntAt(Buffer, 0);
                Console.WriteLine(string.Format("PLC1.Line1.SDZCCS={0}", SDZCCS));
                Console.WriteLine("--------");
                Thread.Sleep(1000);

            }

            //ReadDB1005(client);


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

        static void ReadDB1005(S7Client client)
        {
            while (true)
            {
                byte[] Buffer = new byte[65536];
                client.DBRead(1005, 0, 65536, Buffer);
                var CXNCKD = S7.GetIntAt(Buffer, 42);
                Console.WriteLine(string.Format("一号线-车辆车厢内侧宽度:{0}", CXNCKD));

                var CXNCCD = S7.GetIntAt(Buffer, 40);
                Console.WriteLine(string.Format("一号线-车辆车厢内侧长度:{0}", CXNCCD));

                var CKGD = S7.GetIntAt(Buffer, 6);
                Console.WriteLine(string.Format("一号线-车框高度:{0}", CKGD));

                var SDZCBS = S7.GetIntAt(Buffer, 36);
                Console.WriteLine(string.Format("一号线-设定装车包数:{0}", SDZCBS));

                var JHZQCS = S7.GetIntAt(Buffer, 34);
                Console.WriteLine(string.Format("一号线-计划抓取次数:{0}", JHZQCS));

                var DQZCSKZL = S7.GetIntAt(Buffer, 176);
                Console.WriteLine(string.Format("一号线-当前装车刷卡重量:{0}", DQZCSKZL));

                var ZZCS = S7.GetDIntAt(Buffer, 138);
                Console.WriteLine(string.Format("一号线-总装车数:{0}", ZZCS));

                var DTZCS = S7.GetIntAt(Buffer, 142);
                Console.WriteLine(string.Format("一号线-当天装车数:{0}", DTZCS));

                var ZZCZL = S7.GetRealAt(Buffer, 158);
                Console.WriteLine(string.Format("一号线-总装车重量:{0}", ZZCZL));

                var DTZCZL = S7.GetRealAt(Buffer, 162);
                Console.WriteLine(string.Format("一号线-当天装车重量:{0}", DTZCZL));

                var JCSJHG = S7.GetBitAt(Buffer, 70, 2);
                Console.WriteLine(string.Format("一号线-检测数据合格:{0}", JCSJHG));

                var WMBXZHG = S7.GetBitAt(Buffer, 70, 3);
                Console.WriteLine(string.Format("一号线-尾门板X值合格:{0}", WMBXZHG));

                var DQZCZL = S7.GetDIntAt(Buffer, 0);
                Console.WriteLine(string.Format("一号线-当前装车重量:{0}", DQZCZL));

                var CPH = S7.GetStringAt(Buffer, 180);
                Console.WriteLine(string.Format("一号线-车牌号:{0}", CPH));

                Thread.Sleep(1000);
            }
        }

        static void ReadInput(S7Client client)
        {
            //Buffer = new byte[10];
            //client.ReadArea(S7Consts.S7AreaPE, 7, 0, 5, 5, Buffer);

            //var val1 = S7.GetBitAt(Buffer, 0, 1);
            //Console.WriteLine(string.Format("val1={0}", val1));

            //val1 = S7.GetBitAt(Buffer, 0, 2);
            //Console.WriteLine(string.Format("val2={0}", val1));

            //val1 = S7.GetBitAt(Buffer, 0, 3);
            //Console.WriteLine(string.Format("val3={0}", val1));

            //val1 = S7.GetBitAt(Buffer, 0, 4);
            //Console.WriteLine(string.Format("val4={0}", val1));

            //val1 = S7.GetBitAt(Buffer, 0, 5);
            //Console.WriteLine(string.Format("val5={0}", val1));

            //Buffer = new byte[2];
            //client.ReadArea(S7Consts.S7AreaPE, 10, 0, 1, 1, Buffer);
            //val1 = S7.GetBitAt(Buffer, 0, 7);
            //Console.WriteLine(string.Format("val6={0}", val1));

            //Buffer = new byte[4];
            //client.ReadArea(S7Consts.S7AreaPE, 10, 0, 2, 2, Buffer);
            //val1 = S7.GetBitAt(Buffer, 0, 0);
            //Console.WriteLine(string.Format("val7={0}", val1));

            //val1 = S7.GetBitAt(Buffer, 0, 2);
            //Console.WriteLine(string.Format("val8={0}", val1));
        }
    }
}
