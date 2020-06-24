using S7.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal.Collector.S7Net
{
    public class RunEventArgs : EventArgs
    {
        public Int64 PlcId { set; get; }

        public bool Result { set; get; }

        public System.DateTime StartTime { set; get; }

        public System.DateTime EndTime { set; get; }
    }

    /// <summary>
    /// 扫描管理道
    /// </summary>
    public class ScanLine
    {
        public Int64 PlcId { set; get; }

        /// <summary>
        /// Plc
        /// </summary>
        public Plc Client { private set; get; }

        public List<AbstractReader> ReaderList { private set; get; }

        /// <summary>
        /// 扫描状态
        /// </summary>
        public bool IsRuning { private set; get; }

        /// <summary>
        /// 读取完成
        /// </summary>
        /// <param name="sender">Reader</param>
        /// <param name="e">ReadEventArgs</param>
        public delegate void RunEventHandler(object sender, RunEventArgs e);

        /// <summary>
        /// 读取完成事件
        /// </summary>
        public event RunEventHandler RunHandler;


        private ScanLine()
        { 
        }

        public ScanLine(CpuType type,string ip,int port=102,short slot=0,short rack=1)
        {
            ReaderList = new List<AbstractReader>();
            Client = new Plc(type, ip, port, slot, rack);
            Client.ReadTimeout = 5000;
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
            Client.Close();
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
                    RunEventArgs e = new RunEventArgs();
                    e.PlcId = PlcId;
                    e.StartTime = DateTime.Now;
                    e.Result = true;

                    foreach (var reader in ReaderList)
                    {
                        if (Connect())
                        {
                            reader.Read(Client);
                        }
                        else
                        {
                            e.Result = false;
                        }
                    } 

                    e.EndTime = DateTime.Now;
                    if (this.RunHandler != null)
                    {
                        this.RunHandler(this, e);
                    }
                    Thread.Sleep(interval);
                };
            });
        }
    }
}
