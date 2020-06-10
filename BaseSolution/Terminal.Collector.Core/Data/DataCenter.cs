using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Collector.Core.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DataCenter
    {
        //定义一个用于保存静态变量的实例
        private static DataCenter instance = null;

        //定义一个保证线程同步的标识
        private static readonly object locker = new object();

        //构造函数为私有，使外界不能创建该类的实例
        private DataCenter()
        {

        }

        public static DataCenter Instance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        DataCenter instance = new DataCenter();
                    }
                }
            }
            return instance;
        }

        public async Task AddQueueAsync(TargetData data)
        {
            await Task.Delay(100);
            //...
        }

        public async Task SaveQueueAsync()
        {
            await Task.Delay(100);
            //...
        }
    }
}
