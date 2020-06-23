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
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.DInt;
            item.DB = 1006;
            item.BitAdr = 0;
            item.Count = 1;
            item.StartByteAdr = 138;
            reader.Items.Add("PLC2.Line5.ZZCS", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Int;
            item.DB = 1006;
            item.StartByteAdr = 142;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.DTZCS", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Real;
            item.DB = 1006;
            item.StartByteAdr = 158;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.ZZCZL", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Real;
            item.DB = 1006;
            item.StartByteAdr = 162;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.DTZCZL", item);

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

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Word;
            item.DB = 1006;
            item.StartByteAdr = 42;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.CXNCKD", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Word;
            item.DB = 1006;
            item.StartByteAdr = 40;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.CXNCCD", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Word;
            item.DB = 1006;
            item.StartByteAdr = 446;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.CKGD", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Word;
            item.DB = 1001;
            item.StartByteAdr = 12;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.SDZCCS", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Word;
            item.DB = 1006;
            item.StartByteAdr = 36;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.SDZCBS", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Word;
            item.DB = 200;
            item.StartByteAdr = 484;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("PLC2.Line5.DXSZ", item);

            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.Word;
            item.DB = 1006;
            item.StartByteAdr = 34;
            item.BitAdr = 0;
            item.Count = 1;
            reader.Items.Add("Line5.JHZQCS", item);

            item = new DataItem();
            item.DataType = DataType.Input;
            item.VarType = VarType.Bit;
            item.DB = 9;
            item.StartByteAdr = 0;
            item.BitAdr = 1;
            item.Count = 1;
            reader.Items.Add("Line5.ZQ1CYL", item);

            reader.ReadHandler += Reader_ReadHandler;
            line.RegistReader(reader);

            StringReader sreader = new StringReader();
            item = new DataItem();
            item.DataType = DataType.DataBlock;
            item.VarType = VarType.StringEx;
            item.DB = 1006;
            item.StartByteAdr = 180;
            item.BitAdr = 0;
            item.Count = 100;
            sreader.Items.Add("PLC2.Line5.CPH", item);

            sreader.ReadHandler += Reader_ReadHandler;
            line.RegistReader(sreader);

            line.Start(1000);
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
