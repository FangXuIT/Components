using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Core
{
    public sealed class LogHelper
    {
        #region --单例--
        public static LogHelper Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            internal static readonly LogHelper instance;

            // 静态构造方法，保证线程安全、延迟加载，且只执行一次
            static Nested()
            {
                instance = new LogHelper();
            }
        }

        private LogHelper()
        {
            Logs = new ConcurrentQueue<LogInfo>();
        }

        #endregion

        public ConcurrentQueue<LogInfo> Logs { private set; get; }
        private int MaxCount = 100;

        public void Error(string content)
        {
            Error(DateTime.Now, content);
        }

        public void Error(DateTime time, string content)
        {
            Logs.Enqueue(new LogInfo(time, "error", content));
            while(Logs.Count>MaxCount)
            {
                LogInfo item = null;
                Logs.TryDequeue(out item);
                if(item!=null)
                {
                    item.Msg = string.Empty;
                    item = null;
                }                
            }
        }

        public void Info(string content)
        {
            Info(DateTime.Now, content);
        }

        public void Info(DateTime time, string content)
        {
            Logs.Enqueue(new LogInfo(time, "info", content));
            while (Logs.Count > MaxCount)
            {
                LogInfo item = null;
                Logs.TryDequeue(out item);
                if (item != null)
                {
                    item.Msg = string.Empty;
                    item = null;
                }
            }
        }
    }

    public class LogInfo
    {
        public DateTime LogTime { set; get; }

        public string Type { set; get; }

        public string Msg { set; get; }

        private LogInfo()
        { }

        public LogInfo(DateTime _time,string _type,string _msg)
        {
            LogTime = _time;
            Type = _type;
            Msg = _msg;
        }
    }
}
