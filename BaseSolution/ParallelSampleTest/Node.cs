using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ParallelSampleTest
{
    public class Node
    {
        public string NodeId { set; get; }

        public string DataFilePath { private set; get; }

        public NodeValue Value { private set; get; }

        private Node()
        {
        }

        public Node(string path)
        {
            NodeId = Guid.NewGuid().ToString();
            DataFilePath = path;
        }

        public async Task ReadAsync()
        {
            using (var fs = new FileStream(DataFilePath, FileMode.Open, FileAccess.Read))
            {
                if (Value == null) Value = new NodeValue();

                byte[] bytes = new byte[fs.Length];
                await fs.ReadAsync(bytes, 0, bytes.Length);

                Value.Push(Encoding.UTF8.GetString(bytes));

                Console.WriteLine("NodeId:{0}  ValueTime:{1}  Value:{2}", NodeId, Value.Time, Value.Value);
            }
        }

        public async Task WriteAsync(string value)
        {
            using (var fs1 = new FileStream(DataFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs1))
                {
                    await sw.WriteLineAsync(value);
                }
            }
        }
    }
}