using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelSampleTest
{
    public class DataGenerator
    {
        public List<NodeWatcher> WatcherList { set; get; }

        public int Count
        {
            get
            {
                return WatcherList.Count;
            }
        }

        public Random GeneratorRandom { set; get; }

        public DataGenerator()
        {
            WatcherList = new List<NodeWatcher>();
            GeneratorRandom = new Random();
        }

        public async Task StartAsync()
        {
            while(true)
            {
                var wtIndex = GeneratorRandom.Next(0, Count);
                var watcher = WatcherList[wtIndex];
                for (var idx = 0; idx < 4; idx++)
                {
                    var ndIndex = GeneratorRandom.Next(0, watcher.Nodes.Count);
                    var keys = new string[watcher.Nodes.Keys.Count];
                    watcher.Nodes.Keys.CopyTo(keys, 0);

                    await watcher.Nodes[keys[ndIndex]].WriteAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffffff"));
                }

                Thread.Sleep(GeneratorRandom.Next(500, 1000));
            }
        }
    }
}
