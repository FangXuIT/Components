using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.SiemensS7
{
    public class PEReader : Reader
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID { private set; get; }

        /// <summary>
        /// 读取完成
        /// </summary>
        /// <param name="sender">Reader</param>
        /// <param name="e">ReadEventArgs</param>
        public delegate void ReadEventHandler(object sender, ReadEventArgs e);

        /// <summary>
        /// 读取完成事件
        /// </summary>
        public event ReadEventHandler ReadHandler;

        private PEReader()
        {
        }

        public PEReader(int db, int start = 0, int size = 1024)
        {
            ID = Guid.NewGuid().ToString("n");
            DBNumber = db;
            Start = start;
            Size = size;
        }

        public override void Read(S7Client client)
        {
            if (this.ReadHandler != null)
            {
                ReadEventArgs e = new ReadEventArgs();
                e.ReadID = ID;
                e.DBNumber = DBNumber;
                e.StartTime = DateTime.Now;
                e.Data = new byte[Size];
                e.Result = client.ReadArea(S7Consts.S7AreaPE, DBNumber, Start, Size, S7Consts.S7WLByte, e.Data);
                e.EndTime = DateTime.Now;

                this.ReadHandler(this, e);
            }
        }
    }
}
