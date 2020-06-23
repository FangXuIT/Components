using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;
using S7.Net.Types;
using Terminal.Collector.IStore;
using Terminal.Collector.IStore.Models;
using Terminal.Collector.S7Net;
using Terminal.Collector.Store;

namespace Terminal.Collector.Service
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private ICollectorStore _store;
        private string ConnectionString;
        private string RedisConnectionString;

        private List<ScanLine> Lines;

        public Worker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(500);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var _configuration = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();

            ConnectionString = _configuration["Configurations:ConnectionString"];
            RedisConnectionString = _configuration["Configurations:ConnectionString"];

            _store = new CollectorStoreImple(ConnectionString);
            RedisHelper.Initialization(new CSRedis.CSRedisClient(RedisConnectionString));


            await LoadLinesAsync();
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach(var line in Lines)
            {
                line.Stop();
            }
            Lines.Clear();
            Lines = null;
            _store = null;

            await base.StopAsync(cancellationToken);
        }

        private async Task LoadLinesAsync()
        {
            Lines = new List<ScanLine>();
            var plcs = await _store.GetPlcListAsync();
            var targets = await _store.GetTargetListAsync();

            int iPageSize = 10;

            foreach(var plc in plcs)
            {
                var line = new ScanLine(DataTypeHelper.GetPlcType(plc.CpuType), plc.Ip, plc.Port, plc.Slot, plc.Rack);
                line.Connect();

                var nomaltgs = (from u in targets where u.PlcId == plc.Id && u.VarType != 7 select u).ToList();
                var stringtgs= (from u in targets where u.PlcId == plc.Id && u.VarType == 7 select u).ToList();

                RegistReader(line, nomaltgs, iPageSize);
                RegistStringReader(line, stringtgs); 
                Lines.Add(line);
                line.Start(1000);
            }
        }

        private void RegistReader(ScanLine line, List<TargetModel> targets,int pageSize)
        {
            int iPageCount = 0;
            if (targets.Count == 0)
            {
                iPageCount = 0;
            }
            else if (targets.Count <= pageSize)
            {
                iPageCount = 1;
            }
            else if (targets.Count% pageSize == 0)
            {
                iPageCount = targets.Count / pageSize;
            }
            else
            {
                iPageCount = targets.Count / pageSize;
                iPageCount += 1;
            }

            int currentPage = 1;
            while (currentPage <= iPageCount)
            {
                var reader = new Reader();
                reader.ReadHandler += Reader_ReadHandler;
                var data = (targets.Skip((currentPage - 1) * pageSize).Take(pageSize)).ToList();
                foreach(var target in data)
                {
                    DataItem item = new DataItem();
                    item.DataType = DataTypeHelper.GetDataType(target.DataType);
                    item.VarType = DataTypeHelper.GetVarType(target.VarType);
                    item.DB = target.DB;
                    item.StartByteAdr = target.StartByteAdr;
                    item.BitAdr = System.BitConverter.GetBytes(target.BitAdr)[0];
                    item.Count = target.Count;                    
                    reader.Items.Add(target.Address, item);
                }

                line.RegistReader(reader);
                currentPage += 1;
            }
        }

        private void RegistStringReader(ScanLine line, List<TargetModel> targets)
        {
            var reader = new StringReader();
            reader.ReadHandler += Reader_ReadHandler;
            foreach (var target in targets)
            {
                DataItem item = new DataItem();
                item.DataType = DataTypeHelper.GetDataType(target.DataType);
                item.VarType = DataTypeHelper.GetVarType(target.VarType);
                item.DB = target.DB;
                item.StartByteAdr = target.StartByteAdr;
                item.BitAdr = System.BitConverter.GetBytes(target.BitAdr)[0];
                item.Count = target.Count;
                reader.Items.Add(target.Address, item);
            }

            line.RegistReader(reader);            
        }

        private void Reader_ReadHandler(object sender, ReadEventArgs e)
        {
            ReadHandlerAsync(e);
        }

        private async Task ReadHandlerAsync(ReadEventArgs arg)
        {
            if(arg.Result)
            {
                //_logger.LogInformation(string.Format("{0}={1}", arg.ReadID, arg.Data.Count));
                foreach (var data in arg.Data)
                {
                    if (data.Value != null)
                    {
                        await RedisHelper.SetAsync(data.Key, data.Value);
                    }
                }
            }
            else
            {
               //_logger.LogError(arg.ErrorMsg);
            }

            arg.Dispose();
            arg = null;
        }
    }
}
