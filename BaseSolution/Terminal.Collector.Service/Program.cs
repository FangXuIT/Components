using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Terminal.Collector.SiemensS7;
using Terminal.Collector.Store;

namespace Terminal.Collector.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            S7Client client = new S7Client();
            
            client.ConnectTo("192.168.30.80", 102, 0, 1);

            if(client.Connected)
            {
                var buffer = new byte[1005];
                int isize = 1;
                var result = client.DBGet(1006, buffer, ref isize);
                if (result == 0)
                {
                    var val = S7.GetIntAt(buffer, 142);
                    Console.WriteLine(val);
                }
                else
                {
                    Console.WriteLine(client.ErrorText(result));
                }
            }
            
            
            Console.ReadLine();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .UseWindowsService();
    }
}
