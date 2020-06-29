using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private ILogger<Worker> _logger;
        private ICollectorStore _store;
        private string ConnectionString;
        private string RedisConnectionString;

        private BatchHelper _helper;

        private List<ScanLine> Lines;
        private Dictionary<string, object> Data;

        public Worker(ILogger<Worker> logger,IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;            
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(10000);
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Data = new Dictionary<string, object>();
            var _configuration = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();

            ConnectionString = _configuration["Configurations:ConnectionString"];
            RedisConnectionString = _configuration["Configurations:Redis"];

            _store = new CollectorStoreImple(ConnectionString);
            RedisHelper.Initialization(new CSRedis.CSRedisClient(RedisConnectionString));
            _helper = new BatchHelper(_store);
            
            await LoadLinesAsync();
            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Data.Clear();
            foreach (var line in Lines)
            {
                line.Stop();
            }
            Lines.Clear();
            Lines = null;

            _helper = null;
            _store = null;

            await base.StopAsync(cancellationToken);
        }

        #region --私有成员--
        private async Task LoadLinesAsync()
        {
            Lines = new List<ScanLine>();            
            var plcs = await _store.GetPlcListAsync();
            var targets = await _store.GetTargetListAsync();

            int iPageSize = 10;

            foreach(var plc in plcs)
            {
                var line = new ScanLine(DataTypeHelper.GetPlcType(plc.CpuType), plc.Ip, plc.Port, plc.Slot, plc.Rack);
                line.PlcId = plc.Id;
                line.RunHandler += Line_RunHandler;
                line.StatusChangeHandler += Line_StatusChangeHandler;

                var nomaltgs = (from u in targets where u.PlcId == plc.Id && u.VarType != 7 select u).ToList();
                var stringtgs= (from u in targets where u.PlcId == plc.Id && u.VarType == 7 select u).ToList();

                RegistReader(line, nomaltgs, iPageSize);
                RegistStringReader(line, stringtgs); 
                Lines.Add(line);
                line.Start(1000);
            }
        }

        private void Line_StatusChangeHandler(object sender, StatusChangeEventArgs e)
        {
            RedisHelper.PublishAsync("Collector_Status", string.Format("{0}={1}", e.PlcID, e.Status));
        }

        private void RegistReader(ScanLine line, List<TargetModel> targets, int pageSize)
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
            else if (targets.Count % pageSize == 0)
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
                foreach (var target in data)
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

        private void LogError(string Message)
        {
            RedisHelper.PublishAsync("Collector_Error", string.Format("{0}  {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Message));
        }
        #endregion

        #region --事件处理--
        /// <summary>
        /// PLC当次采数完成(Line Run Event)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Line_RunHandler(object sender, RunEventArgs e)
        {
            RunHandlerAsync(e);            
        }

        /// <summary>
        /// 批次采数完成(Reader Read Event)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reader_ReadHandler(object sender, ReadEventArgs e)
        {
            ReadHandlerAsync(e);
        }

        private async Task ReadHandlerAsync(ReadEventArgs arg)
        {
            if(arg.Result)
            {
                StringBuilder sbSendValue = new StringBuilder();
                foreach (var data in arg.Data)
                {
                    if(!Data.ContainsKey(data.Key))
                    {
                        Data.Add(data.Key, data.Value);
                    }
                    else
                    {
                        Data[data.Key] = data.Value;
                    }
                    sbSendValue.AppendFormat("{0}={1};", data.Key, data.Value);
                    await RedisHelper.SetAsync(data.Key, data.Value);
                }

                if(sbSendValue.Length>0) await RedisHelper.PublishAsync("Collector_Value", sbSendValue.ToString());

                sbSendValue.Clear();
                sbSendValue = null;
            }
            else
            {
                LogError(string.Format("读取超时：{0}。", arg.ErrorMsg));
            }

            arg.Dispose();
            arg = null;
        }

        private async Task RunHandlerAsync(RunEventArgs arg)
        {
            if (arg.Result)
            {
                try
                {

                    _helper.SaveBatchData(arg.PlcId, Data);

                    await RedisHelper.SetAsync(string.Format("PLC{0}_ST", arg.PlcId), arg.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    await RedisHelper.SetAsync(string.Format("PLC{0}_ED", arg.PlcId), arg.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    await RedisHelper.SetAsync(string.Format("PLC{0}_TS", arg.PlcId), (arg.EndTime - arg.StartTime).TotalMinutes);

                }
                catch (Exception ex)
                {
                    LogError(string.Format("保存历史数据出错：{0}。", ex.Message));
                }
            }
            else
            {
                LogError(string.Format("{1}# PLC长时间无反应或连接已断开。", arg.PlcId));
            }
        }
        #endregion
    }
}
