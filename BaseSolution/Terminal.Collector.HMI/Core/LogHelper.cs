using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;

namespace Terminal.Collector.HMI.Core
{
    public class LogHelper
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
            Errors = new ObservableCollection<Log>();
        }

        #endregion

        public ObservableCollection<Log> Errors { private set; get; }

        private readonly object _errorLock = new object();
        public void Error(string msg)
        {
            lock (_errorLock)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    Limit();
                    Errors.Insert(0, new Log(msg));
                }));
            }
        }

        private void Limit()
        {
            if (Errors.Count > 50)
            {
                Errors.RemoveAt(Errors.Count - 1);
            }
        }
    }

    public class Log
    {
        private Log()
        {
        }

        public Log(string msg)
        {
            Time = DateTime.Now;
            Content = msg;
        }

        /// <summary>
        /// 日志记录时间
        /// </summary>
        public DateTime Time { set; get; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { set; get; }
    }
}
