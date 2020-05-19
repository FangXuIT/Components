using ParallelSampleTest.data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelSampleTest
{
    class Program
    {
        private static readonly Stopwatch sw = new Stopwatch();

        static void Main(string[] args)
        {
            Console.WriteLine(">>程序启动");
            Console.WriteLine(">>开始初始化测试数据");
            DataConfig.Instance.InitDataAsync().Wait();
            Console.WriteLine(">>测试数据初始化完成。");

            
            Console.WriteLine(">>注册节点监听");
            List<NodeWatcher> nwList = new List<NodeWatcher>();
            foreach(var ad in DataConfig.Instance.AreaDirectorys)
            {
                NodeWatcher nw = new NodeWatcher(ad.Path);
                nw.Regist(ad.DataFilePaths).Wait();

                nwList.Add(nw);
            }            
            Console.WriteLine(">>节点监听中");

            Console.Read();
        }
    }
}
