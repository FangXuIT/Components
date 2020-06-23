using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;
using Terminal.Collector.IStore;
using Terminal.Collector.IStore.Models;
using Terminal.Collector.SiemensS7;
using Terminal.Collector.Store;

namespace Terminal.Collector.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private ICollectorStore _store;
        private string ConnectionString;
        private string RedisConnectionString;

        private List<ScanLine> Lines;
        private Dictionary<string, List<TargetModel>> Targets;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(500);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Targets = new Dictionary<string, List<TargetModel>>();

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

            Targets.Clear();
            Targets = null;

            await base.StopAsync(cancellationToken);
        }

        private async Task LoadLinesAsync()
        {
            Lines = new List<ScanLine>();
            var plcs = await _store.GetPlcListAsync();
            var targets = await _store.GetTargetListAsync();

            foreach(var plc in plcs)
            {
                AreaRegist(targets, plc, 132);
                AreaRegist(targets, plc, 129);
            }
        }

        private void AreaRegist(List<TargetModel> targets, PlcModel plc,int dataType)
        {
            var line = new ScanLine(plc.Ip, plc.Port, plc.Slot, plc.Rack);
            var dbs = (from u in targets where u.PlcId == plc.Id && u.Interval > 0 && u.DataType == dataType select u.DB)
                .Distinct()
                .ToList();

            foreach (var db in dbs)
            {
                var dicValue = (from u in targets
                                where u.PlcId == plc.Id && u.DB == db && u.DataType == dataType
                                && u.Interval > 0 
                                select u)
                                .ToList();

                if(dataType == 132)
                {//DBReader DB
                    var reader = new DBReader(db);
                    reader.ReadHandler += Reader_ReadHandler;
                    Targets.Add(reader.ID, dicValue);
                    line.RegistReader(reader);
                }
                else if(dataType == 129)
                {//PEReader  Input
                    var reader = new PEReader(db);
                    reader.ReadHandler += Reader_ReadHandler;
                    Targets.Add(reader.ID, dicValue);
                    line.RegistReader(reader);
                }
            }

            line.Connect();
            line.Start(1000);
            Lines.Add(line);
        }

        private void Reader_ReadHandler(object sender, ReadEventArgs e)
        {
            ReadHandlerAsync(e);
        }

        private async Task ReadHandlerAsync(ReadEventArgs arg)
        {
            if(arg.Result==0 || arg.Result==5)
            {
                if (Targets.ContainsKey(arg.ReadID))
                {
                    foreach (var target in Targets[arg.ReadID])
                    {
                        object value = ParseValue(target, arg.Data);
                        if (value != null)
                        {
                            await RedisHelper.SetAsync(target.Address, ParseValue(target, arg.Data));
                        }
                    }
                }
            }
            else
            {
                _logger.LogDebug(string.Format("DB{0} Read Error Code:{1}", arg.DBNumber, arg.Result));
            }
            arg.Dispose();
            arg = null;
        }

        private object ParseValue(TargetModel target,byte[] data)
        {
            try
            {
                switch (target.VarType)
                {
                    case 0: return S7.GetBitAt(data, target.StartByteAdr, target.BitAdr);
                    case 1: return S7.GetByteAt(data, target.StartByteAdr);
                    case 2: return S7.GetWordAt(data, target.StartByteAdr);
                    case 3: return S7.GetDWordAt(data, target.StartByteAdr);
                    case 4: return S7.GetIntAt(data, target.StartByteAdr);
                    case 5: return S7.GetDIntAt(data, target.StartByteAdr);
                    case 6: return S7.GetRealAt(data, target.StartByteAdr);
                    case 7: return S7.GetStringAt(data, target.StartByteAdr, Encoding.GetEncoding("GB2312"));
                    case 11: return S7.GetDateTimeAt(data, target.StartByteAdr);
                    default: return null;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
