using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Terminal.Collector.IStore.Models;

namespace Terminal.Collector.IStore
{
    public interface ICollectorStore
    {
        Task<List<PlcModel>> GetPlcListAsync();

        Task<List<TargetModel>> GetTargetListAsync();

        Task SaveTargetValue<T>(T data);

        Task SaveMultTargetValues<T>(List<T> data);
    }
}
