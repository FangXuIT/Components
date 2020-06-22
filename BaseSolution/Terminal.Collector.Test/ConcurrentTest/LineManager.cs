using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Test.ConcurrentTest
{
    public class LineManager
    {
        public int LineID { private set; get; }

        public ConcurrentQueue<ItemModel> Items { private set; get; }

        private LineManager()
        {
        }

        public LineManager(int id)
        {
            LineID = id;
            Items = new ConcurrentQueue<ItemModel>();
        }

        public void Enqueue(ItemModel data)
        {
            Items.Enqueue(data);
        }

        public ItemModel Peek()
        {
            ItemModel result = null;
            Items.TryPeek(out result);
            return result;
        }

        public ItemModel Dequeue()
        {
            ItemModel result = null;
            Items.TryDequeue(out result);
            return result;
        }
    }


}
