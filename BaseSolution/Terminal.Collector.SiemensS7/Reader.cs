using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.SiemensS7
{
    public abstract class Reader
    {
        /// <summary>
        /// DB块
        /// </summary>
        public int DBNumber { set; get; }

        /// <summary>
        /// 开始位置(默认:0)
        /// </summary>
        public int Start { set; get; }

        /// <summary>
        /// 读取大小(默认:1024)
        /// </summary>
        public int Size { set; get; }

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="client"></param>
        public abstract void Read(S7Client client);
    }
}
