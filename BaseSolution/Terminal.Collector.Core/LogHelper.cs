using System;
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
            Logs = new List<LogInfo>();
        }

        #endregion
        private readonly object listLock = new object();

        public List<LogInfo> Logs { private set; get; }
        private int MaxCount = 500;

        public void Error(string content)
        {
            Error(DateTime.Now, content);
        }

        public void Error(DateTime time, string content)
        {
            try
            {
                lock (listLock)
                {
                    Logs.Insert(0, new LogInfo(time, "error", content));
                    if (Logs.Count > MaxCount)
                    {
                        Logs.RemoveRange(Logs.Count - 101, 100);
                    }
                }
            }
            catch(Exception ex)
            { }
        }

        public void Info(string content)
        {
            Info(DateTime.Now, content);
        }

        public void Info(DateTime time, string content)
        {
            try
            {
                lock (listLock)
                {
                    Logs.Insert(0, new LogInfo(time, "info", content));

                    if (Logs.Count > MaxCount)
                    {
                        Logs.RemoveRange(Logs.Count - 101, 100);
                    }
                }
            }
            catch(Exception ex)
            {

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
