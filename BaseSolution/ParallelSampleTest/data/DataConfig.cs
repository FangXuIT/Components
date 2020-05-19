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


        public List<AreaDirectory> AreaDirectorys { private set; get; }

        public Random GlobalRandom { private set; get; }

        private DataConfig()
        {
            GlobalRandom = new Random();
            AreaDirectorys = new List<AreaDirectory>();

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
                var adPath = string.Format("{0}data\\{1}\\", BaseDirectory, idx);
                var ad = new AreaDirectory(adPath);
                ad.DataFilePaths=await InitDataDirectoryAsync(adPath);

                AreaDirectorys.Add(ad);
            }
        }

        private async Task<List<string>> InitDataDirectoryAsync(string ad)
        {
            if (Directory.Exists(ad)) Directory.Delete(ad, true);
            Directory.CreateDirectory(ad);

            var result = new List<string>();
            for (var idy = 1; idy <= 100; idy++)
            {
                var fp = string.Format("{0}{1}.txt", ad, idy);
                await InitDataFileAsync(fp);

                result.Add(fp);
            }

            return result;
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
