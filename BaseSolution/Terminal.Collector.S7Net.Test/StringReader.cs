using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Terminal.Collector.S7Net.Test
{
    public class StringReader : AbstractReader
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID { private set; get; }

        /// <summary>
        /// 读取完成
        /// </summary>
        /// <param name="sender">Reader</param>
        /// <param name="e">ReadEventArgs</param>
        public delegate void ReadEventHandler(object sender, ReadEventArgs e);

        /// <summary>
        /// 读取完成事件
        /// </summary>
        public event ReadEventHandler ReadHandler;

        public StringReader()
        {
            ID = Guid.NewGuid().ToString("n");
            Items = new Dictionary<string, DataItem>();
        }

        public override void Read(Plc _plc)
        {
            if (this.ReadHandler != null)
            {
                ReadEventArgs e = new ReadEventArgs();
                e.ReadID = ID;
                e.StartTime = System.DateTime.Now;
                try
                {
                    foreach(var dic in Items)
                    {
                        var buffer = _plc.ReadBytes(dic.Value.DataType, dic.Value.DB, dic.Value.StartByteAdr, dic.Value.Count);
                        dic.Value.Value = FilterChar(System.Text.Encoding.GetEncoding("GB2312").GetString(buffer).Trim());
                        buffer = null;
                    }
                    e.Result = true;
                }
                catch (Exception ex)
                {
                    e.Result = false;
                    e.ErrorMsg = ex.Message;
                }
                e.EndTime = System.DateTime.Now;
                e.Data = new Dictionary<string, object>();
                foreach (var dic in Items)
                {
                    e.Data.Add(dic.Key, dic.Value.Value);
                }

                this.ReadHandler(this, e);
            }
        }

        /// <summary>
        /// 过滤特殊字符，保留中文，字母，数字，和-
        /// </summary>
        /// <param name="inputValue">输入字符串</param>
        /// <remarks>发件和收件详细地址有这种情况：“仓场路40-73号迎园新村四坊69号202室”，这种带有-的特殊字符不需要过滤掉</remarks>
        /// <returns></returns>
        private string FilterChar(string inputValue)
        {
            // return Regex.Replace(inputValue, "[`~!@#$^&*()=|{}':;',\\[\\].<>/?~！@#￥……&*（）&mdash;|{}【】；‘’，。/*-+]+", "", RegexOptions.IgnoreCase);
            if (Regex.IsMatch(inputValue, "[A-Za-z0-9\u4e00-\u9fa5-]+"))
            {
                return Regex.Match(inputValue, "[A-Za-z0-9\u4e00-\u9fa5-]+").Value;
            }
            return "";
        }
    }
}
