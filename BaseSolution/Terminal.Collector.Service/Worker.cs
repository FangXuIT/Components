using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Opc.Ua.Configuration;
using Terminal.Collector.Core;
using Terminal.Collector.Core.OpcUA;
using Terminal.Collector.Store;

namespace Terminal.Collector.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private CollectorServer _collector;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _collector = new CollectorServer(new CollectorStoreImple());
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(true)
            {
                Console.WriteLine("Check Status:"+DateTime.Now.ToString("HH:mm:ss"));
                await Task.Delay(60000);
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _collector.InitScanServerAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                await _collector.StartOpcUAServerAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                _collector.StartScan();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Run(() => { _collector.Stop(); });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
