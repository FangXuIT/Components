using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.S7Net
{
    public class DataTypeHelper
    {
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
            switch(type)
            {
                case 0:result = VarType.Bit;
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
                case 28:result = DataType.Counter;
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
