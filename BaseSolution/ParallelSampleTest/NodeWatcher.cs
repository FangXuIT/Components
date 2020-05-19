using ParallelSampleTest.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ParallelSampleTest
{
    public class NodeWatcher
    {
        public Dictionary<string,Node> Nodes { private set; get; }

        private NodeWatcher()
        {
        }

        public NodeWatcher(string watchPath)
        {
            Nodes = new Dictionary<string, Node>();

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = watchPath;
            watcher.Changed += NodeValue_Changed;
            watcher.EnableRaisingEvents = true;
        }

        public async Task Regist(string nodePath)
        {
            if(!Nodes.ContainsKey(nodePath))
            {
                var node = new Node(nodePath);
                await node.ReadAsync();

                Nodes.Add(nodePath, node);
            }
        }

        public async Task Regist(List<string> nodePaths)
        {
            foreach(var path in nodePaths)
            {
                await Regist(path);
            }
        }

        private void NodeValue_Changed(object sender, FileSystemEventArgs e)
        {
            if(Nodes.ContainsKey(e.FullPath))
            {
                Nodes[e.FullPath].ReadAsync();
            }
        }
    }
}
