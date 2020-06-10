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

        public object ParseTargetValue(NodeId dataTypeId, string value)
        {
            if (dataTypeId == DataTypeIds.Int16)
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return Convert.ToInt16(value);
            }
            else if (dataTypeId == DataTypeIds.Int32)
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return Convert.ToInt32(value);
            }
            else if (dataTypeId == DataTypeIds.Int64)
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return Convert.ToInt64(value);
            }
            else if (dataTypeId == DataTypeIds.Float)
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return float.Parse(value);
            }
            else if (dataTypeId == DataTypeIds.Double)
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return Convert.ToDouble(value);
            }
            else if (dataTypeId == DataTypeIds.Boolean)
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return Convert.ToBoolean(value);
            }
            else if (dataTypeId == DataTypeIds.Byte)
            {
                if (string.IsNullOrWhiteSpace(value)) return null;
                else return Convert.ToByte(value);
            }
            else
            {
                return value;
            }
        }
    }
}
