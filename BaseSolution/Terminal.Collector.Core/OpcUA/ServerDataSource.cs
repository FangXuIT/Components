using Opc.Ua;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Terminal.Collector.Core.OpcUA
{
    public class ServerDataSource
    {
        public BaseObjectState Trigger { set; get; }

        public ushort NamespaceIndex { set; get; }

        public ServerSystemContext SystemContext { set; get; }

        public Dictionary<string, string> CurrentData { private set; get; }
        public List<string> Keys { private set; get; }

        public ServerDataSource()
        {
            //manager = new TargetsManager();
            //var targetDir = manager.GetAllTargets();

            //if (CurrentData == null) CurrentData = new Dictionary<string, string>();
            //else CurrentData.Clear();

            //Keys = new List<string>();

            //foreach (var item in targetDir)
            //{
            //    CurrentData.Add(item.Key, string.Empty);
            //    Keys.Add(item.Key);
            //}
        }
    }
}
