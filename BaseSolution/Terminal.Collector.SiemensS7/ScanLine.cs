using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Collector.SiemensS7
{
    /// <summary>
    /// 扫描管理道
    /// </summary>
    public class ScanLine
    {
        public S7Client Client { private set; get; }

        /// <summary>
        /// Ip
        /// </summary>
        public string Ip { private set; get; }

        /// <summary>
        /// Port
        /// </summary>
        public int Port { private set; get; }

        /// <summary>
        /// Slot
        /// </summary>
        public short Slot { private set; get; }

        /// <summary>
        /// Rack
        /// </summary>
        public short Rack { private set; get; }

        /// <summary>
        /// 扫描状态
        /// </summary>
        public bool IsRuning { private set; get; }

        public List<Reader> ReaderList { private set; get; }

        private ScanLine()
        { 
        }


        public ScanLine(string ip,int port=102,short slot=0,short rack=1)
        {
            Ip = ip;
            Port = port;
            Slot = slot;
            Rack = rack;

            Client = new S7Client();
            ReaderList = new List<Reader>();
        }

        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <returns></returns>
        public int Connect()
        {
            if(!Client.Connected)
            {
                return Client.ConnectTo(Ip, Port, Rack, Slot);
            }
            return 0;
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
        public void RegistReader(Reader item)
        {
            bool exist= (from u in ReaderList where u.DBNumber == item.DBNumber select u.DBNumber).Count()>0;
            if(!exist)
            {
                ReaderList.Add(item);                
            }
        }

        /// <summary>
        /// 移除数据读执行者
        /// </summary>
        /// <param name="DBNumber"></param>
        public void RemoveReader(int DBNumber)
        {
            var reader = (from u in ReaderList where u.DBNumber == DBNumber select u).First();
            if(reader!=null)
            {
                ReaderList.Remove(reader);
                reader = null;
            }
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
                    if (Connect() == 0)
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
