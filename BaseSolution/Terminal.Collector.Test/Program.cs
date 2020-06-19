using System;
using System.Security.Cryptography.X509Certificates;
using Terminal.Collector.Store;
using System.Linq;
using PLC.Drive.S7.NetCore;

namespace Terminal.Collector.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var store = new CollectorStoreImple();

            var plcList = store.GetPlcListAsync().Result;
            var targetList = store.GetTargetListAsync().Result;
            foreach(var entity in plcList)
            {
                try
                {
                    var _p = new Plc(DataTypeHelper.GetPlcType(entity.CpuType), entity.Ip, entity.Port, entity.Rack, entity.Slot);
                    _p.Open();

                    var _ts = (from u in targetList where u.PlcId == entity.Id && u.VarType==7 select u).ToList();
                    
                    foreach(var tag in _ts)
                    {
                        try
                        {
                            var type = DataTypeHelper.GetVarType(tag.VarType);
                            object obj = _p.Read(DataTypeHelper.GetDataType(tag.DataType), tag.DB, tag.StartByteAdr, type, tag.Count, (byte)tag.BitAdr);
                            string message = string.Format(@"{0}={1}", tag.Address, DataTypeHelper.ParseVarValue(type, obj, tag.Count));
                            Console.WriteLine(message);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(string.Format("{0}:取值错误", tag.Address));
                        }
                    }
                    _p.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(string.Format("无法连接上PLC:{0},ID={1}.", entity.Name, entity.Id));
                }

                Console.WriteLine("处理完毕!");
                Console.ReadLine();
            }
        }
    }
}
