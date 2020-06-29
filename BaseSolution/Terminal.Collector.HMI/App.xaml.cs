using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using Terminal.Collector.HMI.Core;

namespace Terminal.Collector.HMI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Semaphore singleInstanceWatcher;
        private static bool createdNew;

        public App()
        {
            RedisHelper.Initialization(new CSRedis.CSRedisClient(ConfigManager.Configuration["Configurations:Redis"]));
        }

        static App()
        {
            // Ensure other instances of this application are not running.
            //singleInstanceWatcher = new Semaphore(
            //    0, // Initial count.
            //    1, // Maximum count.
            //    Assembly.GetExecutingAssembly().GetName().Name,
            //    out createdNew);

            //if (!createdNew)
            //{
            //    MessageBox.Show("已有一个实例在运行!");
            //    Environment.Exit(-2);
            //}
        }
    }
}
