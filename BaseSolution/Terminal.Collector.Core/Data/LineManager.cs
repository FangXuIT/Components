using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal.Collector.IStore.Entites;

namespace Terminal.Collector.Core.Data
{
    public class LineManager
    {
        public ConcurrentQueue<Ps_Batch> Items { private set; get; }

        /// <summary>
        /// 装车时间
        /// </summary>
        public int ZCSJ { set; get; }

        public LineManager()
        {
            Items = new ConcurrentQueue<Ps_Batch>();
        }

        public void Enqueue(Ps_Batch data)
        {
            Items.Enqueue(data);
        }

        /// <summary>
        /// 返回开头处的对象但不将其移除
        /// </summary>
        /// <returns></returns>
        public Ps_Batch Peek()
        {
            Ps_Batch result = null;
            Items.TryPeek(out result);
            return result;
        }

        /// <summary>
        /// 尝试移除并返回并发队列开头处的对象
        /// </summary>
        /// <returns></returns>
        public Ps_Batch Dequeue()
        {
            Ps_Batch result = null;
            Items.TryDequeue(out result);
            return result;
        }
    }
}
