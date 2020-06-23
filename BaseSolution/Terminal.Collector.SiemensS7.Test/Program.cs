using System;
using Terminal.Collector.Store;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace Terminal.Collector.SiemensS7.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new S7Client();
            int result1 = client.ConnectTo("192.168.11.95", 102, 0, 1);
            if (result1 == 0)
            {
                Console.WriteLine("Connected to 192.168.11.95");
                DBReader reader = new DBReader(1005);
                reader.ReadHandler += Reader_ReadHandler;
                
                while(true)
                {
                    if(client.Connected)
                    {
                        DateTime dtStart = DateTime.Now;
                        reader.Read(client);
                        DateTime dtEnd = DateTime.Now;
                        Console.WriteLine(string.Format("读取耗时:{0}毫秒",(dtEnd - dtStart).TotalMilliseconds));
                    }
                    else
                    {
                        client.Connect();
                    }
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine(client.ErrorText(result1));
                Console.ReadKey();
                return;
            }

            Console.ReadLine();
        }

        private static void Reader_ReadHandler(object sender, ReadEventArgs e)
        {
            Task.Run(() =>
            {
                if(e.Result==0 || e.Result==5)
                {
                    var CXNCKD = S7.GetIntAt(e.Data, 42);
                    Console.WriteLine(string.Format("一号线-车辆车厢内侧宽度:{0}", CXNCKD));

                    var CXNCCD = S7.GetIntAt(e.Data, 40);
                    Console.WriteLine(string.Format("一号线-车辆车厢内侧长度:{0}", CXNCCD));

                    var CKGD = S7.GetIntAt(e.Data, 6);
                    Console.WriteLine(string.Format("一号线-车框高度:{0}", CKGD));

                    var SDZCBS = S7.GetIntAt(e.Data, 36);
                    Console.WriteLine(string.Format("一号线-设定装车包数:{0}", SDZCBS));

                    var JHZQCS = S7.GetIntAt(e.Data, 34);
                    Console.WriteLine(string.Format("一号线-计划抓取次数:{0}", JHZQCS));

                    var DQZCSKZL = S7.GetIntAt(e.Data, 176);
                    Console.WriteLine(string.Format("一号线-当前装车刷卡重量:{0}", DQZCSKZL));

                    var ZZCS = S7.GetDIntAt(e.Data, 138);
                    Console.WriteLine(string.Format("一号线-总装车数:{0}", ZZCS));

                    var DTZCS = S7.GetIntAt(e.Data, 142);
                    Console.WriteLine(string.Format("一号线-当天装车数:{0}", DTZCS));

                    var ZZCZL = S7.GetRealAt(e.Data, 158);
                    Console.WriteLine(string.Format("一号线-总装车重量:{0}", ZZCZL));

                    var DTZCZL = S7.GetRealAt(e.Data, 162);
                    Console.WriteLine(string.Format("一号线-当天装车重量:{0}", DTZCZL));

                    var JCSJHG = S7.GetBitAt(e.Data, 70, 2);
                    Console.WriteLine(string.Format("一号线-检测数据合格:{0}", JCSJHG));

                    var WMBXZHG = S7.GetBitAt(e.Data, 70, 3);
                    Console.WriteLine(string.Format("一号线-尾门板X值合格:{0}", WMBXZHG));

                    var DQZCZL = S7.GetDIntAt(e.Data, 0);
                    Console.WriteLine(string.Format("一号线-当前装车重量:{0}", DQZCZL));

                    var CPH = S7.GetStringAt(e.Data, 180, Encoding.GetEncoding("GB2312"));
                    Console.WriteLine(string.Format("一号线-车牌号:{0}", CPH));
                }
                e.Dispose();
                e = null;
            });
        }
    }
}
