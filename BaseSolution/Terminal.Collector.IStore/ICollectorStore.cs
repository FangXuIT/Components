using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Terminal.Collector.IStore.Entites;
using Terminal.Collector.IStore.Models;

namespace Terminal.Collector.IStore
{
    public interface ICollectorStore
    {
        Task<List<PlcModel>> GetPlcListAsync();

        Task<List<TargetModel>> GetTargetListAsync();

        Task InsertBatchAsync(Ps_Batch data);

        Task UpdateBatchAsync(Ps_Batch data);

        Task<Ps_Batch> GetBatchAsync(Expression<Func<Ps_Batch, bool>> expression);
    }
}
