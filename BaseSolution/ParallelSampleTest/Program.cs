using ParallelSampleTest.data;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelSampleTest
{
    class Program
    {
        private static readonly Stopwatch Watch = new Stopwatch();

        static void Main(string[] args)
        {
            Watch.Start();
            Console.WriteLine("程序启动");
            Console.WriteLine("开始初始化测试数据");

            DataConfig.Instance.InitDataAsync();

            Console.WriteLine("初始化完成。");

            Console.ReadLine();
        }
    }
}
