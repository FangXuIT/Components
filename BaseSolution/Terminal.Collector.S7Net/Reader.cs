﻿using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Terminal.Collector.S7Net
{
    public class ReadEventArgs : EventArgs, IDisposable
    {
        public string ReadID { set; get; }

        public System.DateTime StartTime { set; get; }

        public System.DateTime EndTime { set; get; }

        public bool Result { set; get; }

        public string ErrorMsg { set; get; }

        public Dictionary<string,object> Data { set; get; }

        public void Dispose()
        {
            ReadID = string.Empty;
            ErrorMsg = string.Empty;
            Data.Clear();
        }
    }

    public class Reader: AbstractReader
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

        public Reader()
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
                    _plc.ReadMultipleVars(Items.Values.ToList());
                    e.Result = true;
                }
                catch
                {
                    //出现异常断开连接(超时),并休眠1秒
                    _plc.Close();
                    e.Result = false;
                    e.ErrorMsg = "由于连接方在一段时间后没有正确答复或连接的主机没有反应，连接尝试失败";
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
    }
}
