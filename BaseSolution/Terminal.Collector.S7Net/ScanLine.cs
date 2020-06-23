using S7.Net;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Collector.S7Net
{
    /// <summary>
    /// 扫描管理道
    /// </summary>
    public class ScanLine
    {
        /// <summary>
        /// Plc
        /// </summary>
        public Plc Client { private set; get; }

        public List<AbstractReader> ReaderList { private set; get; }

        /// <summary>
        /// 扫描状态
        /// </summary>
        public bool IsRuning { private set; get; }


        private ScanLine()
        { 
        }

        public ScanLine(CpuType type,string ip,int port=102,short slot=0,short rack=1)
        {
            ReaderList = new List<AbstractReader>();
            Client = new Plc(type, ip, port, slot, rack);
        }

        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            if(!Client.IsConnected)
            {
                Client.Open();
            }
            return Client.IsConnected;
        }

        /// <summary>
        /// 开始扫描
        /// </summary>
        /// <param name="interval">扫描间隔(默认值：1000,单位：毫秒)</param>
        public void Start(int interval=1000)
        {
            if (IsRuning) return;

            if (interval < 500) interval = 500;

            Run(interval);
        }

        /// <summary>
        /// 停止扫描
        /// </summary>
        public void Stop()
        {
            IsRuning = false;
        }

        /// <summary>
        /// 注册数据读执行者
        /// </summary>
        /// <param name="item"></param>
        public void RegistReader(AbstractReader item)
        {
            ReaderList.Add(item);
        }

        /// <summary>
        /// 开始扫描
        /// </summary>
        /// <param name="interval"></param>
        private void Run(int interval = 1000)
        {
            IsRuning = true;
            Task.Run(() =>
            {
                while (IsRuning)
                {
                    if (Connect())
                    {
                        foreach (var reader in ReaderList)
                        {
                            reader.Read(Client);
                        }
                    }

                    Thread.Sleep(interval);
                };
            });
        }
    }
}
