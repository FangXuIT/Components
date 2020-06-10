using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core.Util
{
    public class DataTypeHelper
    {
        public static object ParseOpcUAValue(NodeId dataTypeId, object value)
        {
            if (value == null) return null;

            if (dataTypeId == DataTypeIds.Int16)
            {
                return Convert.ToInt16(value);
            }
            else if (dataTypeId == DataTypeIds.Int32)
            {
                return Convert.ToInt32(value);
            }
            else if (dataTypeId == DataTypeIds.Int64)
            {
                return Convert.ToInt64(value);
            }
            else if (dataTypeId == DataTypeIds.Float)
            {
                return float.Parse(value.ToString());
            }
            else if (dataTypeId == DataTypeIds.Double)
            {
                return Convert.ToDouble(value);
            }
            else if (dataTypeId == DataTypeIds.Boolean)
            {
                return Convert.ToBoolean(value);
            }
            else if (dataTypeId == DataTypeIds.Byte)
            {
                return Convert.ToByte(value);
            }
            else
            {
                return value.ToString();
            }
        }

        public static NodeId GetDataTypeId(string dataType)
        {
            NodeId result;
            switch (dataType.ToLower().Trim())
            {
                case "long":
                    result = DataTypeIds.Int32;
                    break;
                case "string":
                    result = DataTypeIds.String;
                    break;
                case "float":
                    result = DataTypeIds.Float;
                    break;
                case "boolean":
                    result = DataTypeIds.Boolean;
                    break;
                case "char":
                    result = DataTypeIds.String;
                    break;
                case "byte":
                    result = DataTypeIds.Byte;
                    break;
                case "short":
                    result = DataTypeIds.Int16;
                    break;
                case "llong":
                    result = DataTypeIds.Int64;
                    break;
                case "double":
                    result = DataTypeIds.Double;
                    break;
                case "date":
                    result = DataTypeIds.Date;
                    break;
                default:
                    result = DataTypeIds.String;
                    break;
            }

            return result;
        }
    }
}
