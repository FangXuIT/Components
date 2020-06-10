using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminal.Collector.Core.Scan
{
    public sealed class CollectorPool
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
        public bool Regist(Channel _channel,Target _target)
        {
            bool result = false;
            bool hasIns = false;

            foreach(var ins in Instances.Where(p=>p.Channel.Id ==_channel.Id))
            {
                if (_target.Interval == ins.Interval)
                {
                    if (!ins.Exist(_target))
                    {
                        result = ins.DataItems.TryAdd(_target.Id, _target);
                    }
                    else
                    {
                        result = true;
                    }
                    hasIns = true;
                }
                else
                {
                    if (ins.Exist(_target))
                    {
                        Target _remove;
                        ins.DataItems.TryRemove(_target.Id, out _remove);
                        if (ins.DataItems.Count == 0)
                        {
                            ins.Close();
                            Instances.TakeWhile(p => p == ins);
                        }
                    }
                }
            }

            if(!hasIns)
            {
                ScanInstance ins = new ScanInstance(_channel);
                ins.Interval = _target.Interval;
                result = ins.DataItems.TryAdd(_target.Id, _target);
                ins.Start();
                Instances.Add(ins);
            }

            return result;
        }
    }
}
