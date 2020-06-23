using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Terminal.Collector.IStore;
using Terminal.Collector.IStore.Models;
using Terminal.Collector.IStore.Entites;
using Coldairarrow.Util;
using System.Runtime.InteropServices;
using System.Linq;
using System.Linq.Expressions;

namespace Terminal.Collector.Store
{
    public class CollectorStoreImple : ICollectorStore
    {
        public string ConnectionString { set; get; }

        public CollectorStoreImple(string _connectionString = "server=192.168.11.90;user id=root;password=ABCabc123;persistsecurityinfo=True;database=zeqp_hlsn;SslMode=none;")
        {
            ConnectionString = _connectionString;
        }

        public async Task<List<Pb_Line>> GetLineListAsync()
        {
            using (var db = DBContext.Client())
            {
                return await db.Queryable<Pb_Line>().ToListAsync();
            }
        }

        public async Task<List<PlcModel>> GetPlcListAsync()
        {
            using(var db=DBContext.Client(ConnectionString))
            {
                return await db.Queryable<PB_Plc>().Where(p => p.Deleted == 0)
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

        public async Task<List<TargetModel>> GetTargetListAsync()
        {
            using (var db = DBContext.Client(ConnectionString))
            {
                return await db.Queryable<PB_Tag>().Where(p => p.Deleted == 0)
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

        public async Task<Ps_Batch> GetBatchAsync(Expression<Func<Ps_Batch, bool>> expression)
        {
            using (var db = DBContext.Client(ConnectionString))
            {
                return await db.Queryable<Ps_Batch>().Where(expression).FirstAsync();
            }
        }

        public async Task InsertBatchAsync(Ps_Batch data)
        {
            using(var db = DBContext.Client(ConnectionString))
            {
                await db.Insertable(data).ExecuteCommandAsync();
            }
        }

        public async Task UpdateBatchAsync(Ps_Batch data)
        {
            using (var db = DBContext.Client(ConnectionString))
            {
                await db.Updateable(data).ExecuteCommandAsync();
            }
        }
    }
}
