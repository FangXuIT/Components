using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal.Collector.Core.Scan
{
    public class CollectorPool
    {
        private ConcurrentBag<ScanInstance> Instances { set; get; }

        public CollectorPool()
        {
            Instances = new ConcurrentBag<ScanInstance>();
        }

        /// <summary>
        /// 注册扫描采集点
        /// </summary>
        /// <param name="_channel">采集通道</param>
        /// <param name="_target">采集数据点</param>
        /// <returns></returns>
        //public ScanInstance Regist(Channel _channel,Target _target)
        //{
        //    if (_target.Interval <= 0) return null;

        //    ScanInstance result = null;
        //    bool hasIns = false;

        //    foreach(var ins in Instances.Where(p=>p.Channel.Id ==_channel.Id))
        //    {
        //        if (_target.Interval == ins.Interval)
        //        {
        //            if (!ins.Exist(_target))
        //            {
        //                ins.DataItems.TryAdd(_target.Id, _target);
        //            }

        //            result = ins;
        //            hasIns = true;
        //        }
        //        else
        //        {
        //            if (ins.Exist(_target))
        //            {
        //                Target _remove;
        //                ins.DataItems.TryRemove(_target.Id, out _remove);
        //                if (ins.DataItems.Count == 0)
        //                {
        //                    ins.Close();
        //                    Instances.TakeWhile(p => p == ins);
        //                }
        //            }
        //        }
        //    }

        //    if(!hasIns)
        //    {
        //        ScanInstance ins = new ScanInstance(_channel);
        //        ins.Interval = _target.Interval;
        //        ins.DataItems.TryAdd(_target.Id, _target);                
        //        Instances.Add(ins);

        //        result = ins;
        //    }

        //    return result;
        //}

        public void Start()
        {
            foreach(var item in Instances)
            {
                if(!item.ScanEnabled)
                {
                    item.Start();
                }
            }
        }

        public void Pause()
        {
            foreach (var item in Instances)
            {
                if (item.ScanEnabled)
                {
                    item.Pause();
                }
            }
        }

        public void Close()
        {
            foreach (var item in Instances)
            {
                if (item.ScanEnabled)
                {
                    item.Close();
                }
            }
        }
    }
}
