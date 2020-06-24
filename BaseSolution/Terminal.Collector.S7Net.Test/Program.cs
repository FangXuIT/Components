using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;

namespace Terminal.Collector.S7Net.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            StartServer();
            Console.ReadLine();
        }

        static void StartServer()
        {            
            Console.WriteLine("Hello World!");

            ScanLine line = new ScanLine(CpuType.S71200, "192.168.11.95", 103, 0, 1);
            line.Connect();

            Reader reader = new Reader();

            DataItem item = new DataItem();
            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Bit;
            item.DB = 51;
            item.StartByteAdr = 32;
            item.BitAdr = 5;
            item.Count = 1;
            reader.Items.Add("PLC2.Line4.ZCZT", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Bit;
            item.DB = 51;
            item.StartByteAdr = 48;
            item.BitAdr = 7;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.ZCZT", item);

            reader.ReadHandler += Reader_ReadHandler;
            line.RegistReader(reader);

            //StringReader sreader = new StringReader();
            //item = new DataItem();
            //item.DataType = DataType.DataBlock;
            //item.VarType = VarType.StringEx;
            //item.DB = 1006;
            //item.StartByteAdr = 180;
            //item.BitAdr = 0;
            //item.Count = 100;
            //sreader.Items.Add("PLC2.Line5.CPH", item);

            //sreader.ReadHandler += Reader_ReadHandler;
            //line.RegistReader(sreader);

            line.Start(2000);
        }

        private static void Reader_ReadHandler(object sender, ReadEventArgs e)
        {
            Console.WriteLine(string.Format("*******本次耗时:{0}毫秒*******", (e.EndTime - e.StartTime).TotalMilliseconds));
            if (e.Result)
            {
                foreach (var item in e.Data)
                {
                    if(item.Key.EndsWith(".CPH"))
                    {
                        Console.WriteLine(string.Format("{0}={1}", item.Key, item.Value));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0}={1}", item.Key, item.Value));
                    }
                    
                }
            }
            else
            {
                Console.WriteLine(e.ErrorMsg);
            }
            
        }

        static void Read(Plc _plc)
        {
            List<DataItem> list = new List<DataItem>();

            

            System.DateTime dtStart = System.DateTime.Now;
            _plc.ReadMultipleVars(list);
            System.DateTime dtEnd = System.DateTime.Now;
            Console.WriteLine(string.Format("耗时:{0}毫秒", (dtEnd - dtStart).TotalMilliseconds));

            foreach (var data in list)
            {
                Console.WriteLine(data.Value);
            }
        }
    }
}
