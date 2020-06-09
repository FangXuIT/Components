using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Test.ThreadTimer
{
    public class ToolTime
    {
        private Timer timeScan;
        private AutoResetEvent autoEvent;

        private int idx = 0;

        public void Start()
        {
            if(timeScan==null)
            {
                autoEvent = new AutoResetEvent(false);
                timeScan = new Timer(new TimerCallback(Timer_Expand), autoEvent, 0, 1000);
            }
            else
            {
                timeScan.Change(0, 1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Pause()
        {
            timeScan.Change(0, -1);
        }

        void Timer_Expand(object obj)
        {
            idx += 1;
            Console.WriteLine(string.Format("{0}===>{1}", DateTime.Now.ToString("HH:mm:ss.fff"),idx));
        }
    }
}
