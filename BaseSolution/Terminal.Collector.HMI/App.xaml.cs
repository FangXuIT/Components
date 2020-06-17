using System;
using System.Reflection;
using System.Threading;
using System.Windows;

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
        }

        static App()
        {
            // Ensure other instances of this application are not running.
            singleInstanceWatcher = new Semaphore(
                0, // Initial count.
                1, // Maximum count.
                Assembly.GetExecutingAssembly().GetName().Name,
                out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("已有一个实例在运行!");
                Environment.Exit(-2);
            }
        }
    }
}
