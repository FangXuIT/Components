using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(10000);

            //Console.WriteLine("Hello World!");
            var path = "E:\\工作目录\\git\\Components\\BaseSolution\\ParallelSampleTest\\bin\\Debug\\netcoreapp3.1\\data\\9\\4.txt";
            while (true)
            {
                var value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffffff");
                try
                {
                    ModifyAsync(path, value).Wait();
                    Console.WriteLine("写入值：{0}", value);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Thread.Sleep(1000);
            }
        }

        static async Task ModifyAsync(string path,string value)
        {
            using (var fs1 = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs1))
                {
                    await sw.WriteLineAsync(value);
                }
            }
        }
    }
}
