using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelSampleTest.data
{
    public class AreaDirectory
    {
        public string Path { private set; get; }

        public List<string> DataFilePaths { set; get; }

        private AreaDirectory() 
        { }

        public AreaDirectory(string path)
        {
            DataFilePaths = new List<string>();
            Path = path;
        }
    }
}
