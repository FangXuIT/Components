using org.apache.zookeeper.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.ThreadTimer
{
    public class Executer
    {
        public bool IsRuning { set; get; }

        public void Start()
        {
            if(!IsRuning)
            {
                IsRuning = true;
                Scan();
            }
        }

        public void Stop()
        {
            IsRuning = false;
        }

        public void Scan()
        {
            Task.Run(() =>
            {
                while(IsRuning)
                {
                    Console.WriteLine("Scan :{0}", DateTime.Now.ToString("HH:mm:ss"));
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Task Closed.");
            });
        }
    }
}
