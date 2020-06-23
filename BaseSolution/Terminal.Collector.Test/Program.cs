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
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            //-------------- Create and connect the client
            var client = new S7Client();
            int result1 = client.ConnectTo("192.168.11.95", 0, 1);
            if (result1 == 0)
            {
                Console.WriteLine("Connected to 192.168.11.95");
            }
            else
            {
                Console.WriteLine(client.ErrorText(result1));
                Console.ReadKey();
                return;
            }

            //ReadDB1005(client);
            //Console.WriteLine("----------------");
            ReadInput(client);

            //client.Disconnect();



            Console.ReadLine();
        }

        static void ReadDB1005(S7Client client)
        {
            byte[] Buffer = new byte[65536];
            var res = client.DBRead(1005, 0, 1024, Buffer);
            if(res>0)
            {
                Console.WriteLine(client.ErrorText(res));
            }
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

            var CPH = S7.GetStringAt(Buffer, 180, Encoding.GetEncoding("GB2312"));
            Console.WriteLine(string.Format("一号线-车牌号:{0}", CPH));
        }

        static void ReadInput(S7Client client)
        {
            var Buffer = new byte[1024];
            client.ReadArea(S7Consts.S7AreaPE, 7, 0, 1, S7Consts.S7WLByte, Buffer);

            var val1 = S7.GetBitAt(Buffer, 0, 1);
            Console.WriteLine(string.Format("val1={0}", val1));

            val1 = S7.GetBitAt(Buffer, 0, 2);
            Console.WriteLine(string.Format("val2={0}", val1));

            val1 = S7.GetBitAt(Buffer, 0, 3);
            Console.WriteLine(string.Format("val3={0}", val1));

            val1 = S7.GetBitAt(Buffer, 0, 4);
            Console.WriteLine(string.Format("val4={0}", val1));

            val1 = S7.GetBitAt(Buffer, 0, 5);
            Console.WriteLine(string.Format("val5={0}", val1));

            Buffer = new byte[20];
            client.ReadArea(S7Consts.S7AreaPE, 10, 0, 1, 1, Buffer);
            val1 = S7.GetBitAt(Buffer, 0, 7);
            Console.WriteLine(string.Format("val6={0}", val1));

            Buffer = new byte[4];
            client.ReadArea(S7Consts.S7AreaPE, 10, 0, 2, 2, Buffer);
            val1 = S7.GetBitAt(Buffer, 0, 0);
            Console.WriteLine(string.Format("val7={0}", val1));

            val1 = S7.GetBitAt(Buffer, 0, 2);
            Console.WriteLine(string.Format("val8={0}", val1));
        }

        static byte[] intToBytes(int value)
        {
            byte[] src = new byte[4];
            src[3] = (byte)((value >> 24) & 0xFF);
            src[2] = (byte)((value >> 16) & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);
            src[0] = (byte)(value & 0xFF);
            return src;
        }

        static byte[] DateToByte(DateTime date)
        {
            int year = date.Year - 2000;
            if (year < 0 || year > 127)
                return new byte[4];
            int month = date.Month;
            int day = date.Day;
            int date10 = year * 512 + month * 32 + day;
            return BitConverter.GetBytes((ushort)date10);
        }
        
        static void ByteLength()
        {
            var result = BitConverter.GetBytes(65535);//Int, DInt,WORD,Counter
            Console.WriteLine(string.Format("Int16 Int32 WORD:{0}", result.Length));

            result = BitConverter.GetBytes(21474836479);//LInt, DWORD
            Console.WriteLine(string.Format("Int64 DWORD:{0}", result.Length));

            result = BitConverter.GetBytes(true);//Bit
            Console.WriteLine(string.Format("Bit:{0}", result.Length));

            result = BitConverter.GetBytes(1.64e+009);//Real
            Console.WriteLine(string.Format("Real:{0}", result.Length));

            result = System.Text.Encoding.Default.GetBytes("万灵杰");//String
            Console.WriteLine(string.Format("String:{0}", result.Length));

            result = DateToByte(DateTime.Now);//Date
            Console.WriteLine(string.Format("Date:{0}", result.Length));

            result = BitConverter.GetBytes(DateTime.Now.Ticks);//DateTime
            Console.WriteLine(string.Format("DateTime:{0}", result.Length));

            Console.WriteLine("Timer:12");
        }
    }
}
