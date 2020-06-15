using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Terminal.Collector.IStore;
using Terminal.Collector.IStore.Models;
using Terminal.Collector.Store.Entites;

namespace Terminal.Collector.Store
{
    public class CollectorStoreImple : ICollectorStore
    {
        public Task<List<PlcModel>> GetPlcListAsync()
        {
            using(var db=DBContext.Client())
            {
                return db.Queryable<PB_Plc>().Where(p => p.Deleted == 0)
                    .Select(s => new PlcModel()
                    {
                        CpuType = s.CpuType,
                        Id = s.Id,
                        Ip = s.Ip,
                        Name = s.Name,
                        Port = s.Port,
                        Rack = (short)s.Rack,
                        Slot = (short)s.Slot
                    }).ToListAsync();
            }
        }

        public Task<List<TargetModel>> GetTargetListAsync()
        {
            using (var db = DBContext.Client())
            {
                return db.Queryable<PB_Tag>().Where(p => p.Deleted == 0)
                    .Select(s => new TargetModel()
                    {
                        DB = s.DB,
                        DataType = s.DataType,
                        BitAdr = s.BitAdr,
                        Count = s.Count,
                        Id = s.Id,
                        Interval = s.Interval,
                        IsStoreTarget = s.SaveHistory == 1,
                        Name = s.Tag,
                        PlcId = s.PlcId,
                        StartByteAdr = s.StartByteAdr,
                        VarType = s.VarType,
                        Address = s.Address,
                        OpcNodeType = s.OpcNodeType,
                        Tag = s.Tag
                    }).ToListAsync();
            }
        }

        public Task SaveMultTargetValues<T>(List<T> data)
        {
            throw new NotImplementedException();
        }

        public Task SaveTargetValue<T>(T data)
        {
            throw new NotImplementedException();
        }
    }
}
