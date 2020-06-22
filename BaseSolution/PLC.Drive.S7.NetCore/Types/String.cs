using System.Text.RegularExpressions;

namespace PLC.Drive.S7.NetCore.Types
{
    /// <summary>
    /// Contains the methods to convert from S7 strings to C# strings
    /// </summary>
    public class String
    {
        /// <summary>
        /// Converts a string to <paramref name="reservedLength"/> of bytes, padded with 0-bytes if required.
        /// </summary>
        /// <param name="value">The string to write to the PLC.</param>
        /// <param name="reservedLength">The amount of bytes reserved for the <paramref name="value"/> in the PLC.</param>
        public static byte[] ToByteArray(string value, int reservedLength)
        {
            var length = value?.Length;
            if (length > reservedLength) length = reservedLength;
            var bytes = new byte[reservedLength];

            if (length == null || length == 0) return bytes;

            System.Text.Encoding.ASCII.GetBytes(value, 0, length.Value, bytes, 0);

            return bytes;
        }
        
        /// <summary>
        /// Converts S7 bytes to a string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FromByteArray(byte[] bytes)
        {
            var value = System.Text.Encoding.GetEncoding("gb2312").GetString(bytes).Trim();
            value = FilterChar(value);

            if (value.Length>0) value = value.Substring(0, 7);

            return value;
        }

        #region public static string FilterChar(string inputValue) 过滤特殊字符，保留中文，字母，数字，和-
        /// <summary>
        /// 过滤特殊字符，保留中文，字母，数字，和-
        /// </summary>
        /// <param name="inputValue">输入字符串</param>
        /// <remarks>发件和收件详细地址有这种情况：“仓场路40-73号迎园新村四坊69号202室”，这种带有-的特殊字符不需要过滤掉</remarks>
        /// <returns></returns>
        public static string FilterChar(string inputValue)
        {
            // return Regex.Replace(inputValue, "[`~!@#$^&*()=|{}':;',\\[\\].<>/?~！@#￥……&*（）&mdash;|{}【】；‘’，。/*-+]+", "", RegexOptions.IgnoreCase);
            if (Regex.IsMatch(inputValue, "[A-Za-z0-9\u4e00-\u9fa5-]+"))
            {
                return Regex.Match(inputValue, "[A-Za-z0-9\u4e00-\u9fa5-]+").Value;
            }
            return "";
        }
        #endregion

    }
}
