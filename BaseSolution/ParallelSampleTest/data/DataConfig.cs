using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ParallelSampleTest.data
{
    public sealed class DataConfig
    {
        private static readonly DataConfig _instance = null;

        public string BaseDirectory { private set; get; }

        public List<string> AreaDirectorys { private set; get; }

        public List<string> DataFilePaths { private set; get; }

        public Random GlobalRandom { private set; get; }

        private DataConfig()
        {
            GlobalRandom = new Random();
            AreaDirectorys = new List<string>();
            DataFilePaths = new List<string>();

            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        static DataConfig()
        {
            _instance = new DataConfig();
        }

        public static DataConfig Instance
        {
            get
            {
                return _instance;
            }
        }

        public async Task InitDataAsync()
        {
            
            for (var idx = 1; idx <= 50; idx++)
            {
                var ad = string.Format("{0}\\data\\{1}\\", BaseDirectory, idx);
                AreaDirectorys.Add(ad);

                await InitDataDirectoryAsync(ad);
            }
        }

        private async Task InitDataDirectoryAsync(string ad)
        {
            if (Directory.Exists(ad)) Directory.Delete(ad, true);
            Directory.CreateDirectory(ad);

            for (var idy = 1; idy <= 100; idy++)
            {
                var fp = string.Format("{0}{1}.txt", ad, idy);
                DataFilePaths.Add(fp);

                await InitDataFileAsync(fp);
            }
        }

        private async Task InitDataFileAsync(string filePath)
        {
            using(var fs1= new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using(var sw= new StreamWriter(fs1))
                {
                    await sw.WriteLineAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fffffff"));
                }
            }            
        }
    }
}
