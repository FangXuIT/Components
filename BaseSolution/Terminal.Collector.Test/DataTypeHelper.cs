﻿using PLC.Drive.S7.NetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Test
{
    public class DataTypeHelper
    {
        public static object ParseVarValue(VarType type, object value, int length)
        {
            if (value == null) return null;

            object result = null;
            switch (type)
            {
                case VarType.Bit:
                    result = Convert.ToBoolean(value);
                    break;
                case VarType.Byte:
                    result = Convert.ToByte(value);
                    break;
                case VarType.Counter:
                    result = Convert.ToInt32(value);
                    break;
                case VarType.DateTime:
                    result = Convert.ToDateTime(value);
                    break;
                case VarType.DInt:
                    result = Convert.ToInt32(value);
                    break;
                case VarType.DWord:
                    result = Convert.ToInt32(value);
                    break;
                case VarType.Int:
                    result = Convert.ToInt16(value);
                    break;
                case VarType.Real:
                    result = float.Parse(value.ToString());
                    break;
                case VarType.String:
                    result = value.ToString();
                    break;
                case VarType.StringEx:
                    result = value.ToString();
                    break;
                case VarType.Timer:
                    result = value.ToString();
                    break;
                case VarType.Word:
                    result = Convert.ToInt16(value);
                    break;

            }
            return result;
        }

        public static CpuType GetPlcType(string type)
        {
            var result = CpuType.S71500;
            switch (type)
            {
                case "S7-1500":
                    result = CpuType.S71500;
                    break;
                case "S7-1200":
                    result = CpuType.S71200;
                    break;
                case "S7-200":
                    result = CpuType.S7200;
                    break;
                case "S7-300":
                    result = CpuType.S7300;
                    break;
                case "S7-400":
                    result = CpuType.S7400;
                    break;

            }
            return result;
        }

        public static VarType GetVarType(int type)
        {
            VarType result = VarType.Int;
            switch (type)
            {
                case 0:
                    result = VarType.Bit;
                    break;
                case 1:
                    result = VarType.Byte;
                    break;
                case 2:
                    result = VarType.Word;
                    break;
                case 3:
                    result = VarType.DWord;
                    break;
                case 4:
                    result = VarType.Int;
                    break;
                case 5:
                    result = VarType.DInt;
                    break;
                case 6:
                    result = VarType.Real;
                    break;
                case 7:
                    result = VarType.String;
                    break;
                case 8:
                    result = VarType.StringEx;
                    break;
                case 9:
                    result = VarType.Timer;
                    break;
                case 10:
                    result = VarType.Counter;
                    break;
                case 11:
                    result = VarType.DateTime;
                    break;
            }
            return result;
        }

        public static DataType GetDataType(int type)
        {
            DataType result = DataType.DataBlock;

            switch (type)
            {
                case 28:
                    result = DataType.Counter;
                    break;
                case 29:
                    result = DataType.Timer;
                    break;
                case 129:
                    result = DataType.Input;
                    break;
                case 130:
                    result = DataType.Output;
                    break;
                case 131:
                    result = DataType.Memory;
                    break;
                case 132:
                    result = DataType.DataBlock;
                    break;
            }

            return result;
        }
    }
}
