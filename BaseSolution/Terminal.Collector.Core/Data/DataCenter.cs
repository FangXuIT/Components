using PLC.Drive.S7.NetCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Collector.IStore;
using Terminal.Collector.IStore.Entites;
using Terminal.Collector.IStore.Models;
using Terminal.Collector.Store;

namespace Terminal.Collector.Core.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DataCenter
    {
        #region --单例--
        public static DataCenter Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            internal static readonly DataCenter instance;

            // 静态构造方法，保证线程安全、延迟加载，且只执行一次
            static Nested()
            {
                instance = new DataCenter();
            }
        }
        #endregion

        private ICollectorStore store;
        private bool successed;        

        /// <summary>
        /// 产线装车实时数据
        /// </summary>
        public Dictionary<int,LineManager> Lines { private set; get; }

        /// <summary>
        /// 采集节点与产线关系
        /// </summary>
        public Dictionary<string,int> KeyLineRelation { private set; get; }

        /// <summary>
        /// 待保存的数据列表
        /// </summary>
        public ConcurrentQueue<Ps_Batch> Data { private set; get; }

        /// <summary>
        /// 扫描频率(默认值：60000,单位：毫秒)
        /// </summary>
        public int Interval { set; get; }
        /// <summary>
        /// 扫描定时器
        /// </summary>
        private System.Threading.Timer timerScan;
        private AutoResetEvent autoEvent;

        private DataCenter()
        {
            successed = false;
            Interval = 60000;
            store = new CollectorStoreImple();
            Lines = new Dictionary<int, LineManager>();
            Data = new ConcurrentQueue<Ps_Batch>();


            InitKeyLineRelation();
            LoadLinesAsync().Wait();
        }

        private void InitKeyLineRelation()
        {
            KeyLineRelation = new Dictionary<string, int>();
            int LineID = 0;
            #region --一号线--
            LineID = 1;
            KeyLineRelation.Add("PLC1.Line1.CXNCKD", LineID);         //车辆车厢内侧宽度
            KeyLineRelation.Add("PLC1.Line1.CXNCCD", LineID);         //车辆车厢内侧长度
            KeyLineRelation.Add("PLC1.Line1.CKGD", LineID);           //车箱大小（高）
            KeyLineRelation.Add("PLC1.Line1.CPH", LineID);            //车牌号
            KeyLineRelation.Add("PLC1.Line1.DXSZ", LineID);           //垛型设置
            KeyLineRelation.Add("PLC1.Line1.SDZCBS", LineID);         //设定装车包数
            KeyLineRelation.Add("PLC1.Line1.ZCSJ", LineID);          //装车时间
            KeyLineRelation.Add("PLC1.Line1.JQRSJZQCS", LineID);     //机器人实际抓取次数
            KeyLineRelation.Add("PLC1.Line1.DQZCZL", LineID);        //当前装车重量
            KeyLineRelation.Add("PLC1.Line1.JQRSJZQBS", LineID);     //机器人实际抓取包数
            KeyLineRelation.Add("PLC1.Line1.ZCWB", LineID);          //装车完毕
            #endregion
            #region --二号线--
            LineID = 2;
            KeyLineRelation.Add("PLC1.Line2.CXNCKD", LineID);         //车辆车厢内侧宽度
            KeyLineRelation.Add("PLC1.Line2.CXNCCD", LineID);         //车辆车厢内侧长度
            KeyLineRelation.Add("PLC1.Line2.CKGD", LineID);           //车箱大小（高）
            KeyLineRelation.Add("PLC1.Line2.CPH", LineID);            //车牌号
            KeyLineRelation.Add("PLC1.Line2.DXSZ", LineID);           //垛型设置
            KeyLineRelation.Add("PLC1.Line2.SDZCBS", LineID);         //设定装车包数
            KeyLineRelation.Add("PLC1.Line2.ZCSJ", LineID);          //装车时间
            KeyLineRelation.Add("PLC1.Line2.JQRSJZQCS", LineID);     //机器人实际抓取次数
            KeyLineRelation.Add("PLC1.Line2.DQZCZL", LineID);        //当前装车重量
            KeyLineRelation.Add("PLC1.Line2.JQRSJZQBS", LineID);     //机器人实际抓取包数
            KeyLineRelation.Add("PLC1.Line2.ZCWB", LineID);          //装车完毕
            #endregion
            #region --四号线--
            LineID = 4;
            KeyLineRelation.Add("PLC2.Line4.CXNCKD", LineID);         //车辆车厢内侧宽度
            KeyLineRelation.Add("PLC2.Line4.CXNCCD", LineID);         //车辆车厢内侧长度
            KeyLineRelation.Add("PLC2.Line4.CKGD", LineID);           //车箱大小（高）
            KeyLineRelation.Add("PLC2.Line4.CPH", LineID);            //车牌号
            KeyLineRelation.Add("PLC2.Line4.DXSZ", LineID);           //垛型设置
            KeyLineRelation.Add("PLC2.Line4.SDZCBS", LineID);         //设定装车包数
            KeyLineRelation.Add("PLC2.Line4.ZCSJ", LineID);          //装车时间
            KeyLineRelation.Add("PLC2.Line4.JQRSJZQCS", LineID);     //机器人实际抓取次数
            KeyLineRelation.Add("PLC2.Line4.DQZCZL", LineID);        //当前装车重量
            KeyLineRelation.Add("PLC2.Line4.JQRSJZQBS", LineID);     //机器人实际抓取包数
            KeyLineRelation.Add("PLC2.Line4.ZCWB", LineID);          //装车完毕
            #endregion
            #region --五号线--
            LineID = 5;
            KeyLineRelation.Add("PLC2.Line5.CXNCKD", LineID);         //车辆车厢内侧宽度
            KeyLineRelation.Add("PLC2.Line5.CXNCCD", LineID);         //车辆车厢内侧长度
            KeyLineRelation.Add("PLC2.Line5.CKGD", LineID);           //车箱大小（高）
            KeyLineRelation.Add("PLC2.Line5.CPH", LineID);            //车牌号
            KeyLineRelation.Add("PLC2.Line5.DXSZ", LineID);           //垛型设置
            KeyLineRelation.Add("PLC2.Line5.SDZCBS", LineID);         //设定装车包数
            KeyLineRelation.Add("PLC2.Line5.ZCSJ", LineID);          //装车时间
            KeyLineRelation.Add("PLC2.Line5.JQRSJZQCS", LineID);     //机器人实际抓取次数
            KeyLineRelation.Add("PLC2.Line5.DQZCZL", LineID);        //当前装车重量
            KeyLineRelation.Add("PLC2.Line5.JQRSJZQBS", LineID);     //机器人实际抓取包数
            KeyLineRelation.Add("PLC2.Line5.ZCWB", LineID);          //装车完毕
            #endregion
        }

        private async Task LoadLinesAsync()
        {
            var entities = await store.GetLineListAsync();
            foreach (var line in entities)
            {
                Lines.Add(line.Id, new LineManager());
            }
            successed = true;            
        }

        public void StartSaveData()
        {
            try
            {
                autoEvent = new AutoResetEvent(false);
                timerScan = new System.Threading.Timer(new TimerCallback(TimeCall), autoEvent, 0, Interval);
            }
            catch(Exception ex)
            {
                LogHelper.Instance.Error(string.Format("DataCenter StartSaveData:", ex.Message));
            }
        }

        /// <summary>
        /// 定时存储数据
        /// </summary>
        /// <param name="stateInfo"></param>
        void TimeCall(Object stateInfo)
        {
            TimeCallHandle();
        }

        private async Task TimeCallHandle()
        {
            if (Data.Count > 0)
            {
                List<Ps_Batch> list = new List<Ps_Batch>();
                while (Data.Count > 0)
                {
                    Ps_Batch batch = null;
                    Data.TryDequeue(out batch);

                    if (batch != null) list.Add(batch);
                }

                if (list != null && list.Count > 0)
                {
                    try
                    {
                        await store.UpdateBatchAsync(list);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.Error(string.Format("UpdateBatchAsync:{0}", ex.Message));
                    }
                }
                list.Clear();
                list = null;
            }
        }

        public void SetValue(string key,object value,DateTime dtNow)
        {
            if(KeyLineRelation.ContainsKey(key))
            {
                int idx = key.LastIndexOf(".") + 1;
                string simpleKey = key.Substring(idx, key.Length - idx);

                ModifyBatchValue(KeyLineRelation[key], simpleKey, value, dtNow);
            }
        }

        private void ModifyBatchValue(int lineId,string simpleKey,object value, DateTime dtNow)
        {
            if (value == null) return;

            switch(simpleKey)
            {
                case "ZCSJ"://装车时间
                    ZCSJChanged(lineId, value, dtNow);
                    break;
                case "ZCWB"://装车完毕
                    ZCWBChanged(lineId, value, dtNow);
                    break;
                default:
                    ModifyBatchProperty(lineId, simpleKey, value, dtNow);
                    break;
            }
        }

        private readonly object insertLock = new object();
        private readonly object updateLock = new object();

        /// <summary>
        /// 装车其他属性发生变化
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="simpleKey"></param>
        /// <param name="value"></param>
        private void ModifyBatchProperty(int lineId, string simpleKey, object value, DateTime dtNow)
        {
            var line = Lines[lineId];
            var batch = line.Peek();
            if(batch==null)
            {
                lock (insertLock)
                {
                    batch = line.Peek();
                    if (batch == null)
                    {
                        Ps_Batch old = null;

                        old = store.GetBatchAsync(p => p.LineId == lineId
                                      && p.TruckLicense == value.ToString()
                                      && p.Status == 0
                                      && p.StartTime >= DateTime.Now.AddMinutes(30)).Result;


                        if (old == null)
                        {
                            TruckStartLoading(lineId, line, dtNow);
                        }
                        else
                        {
                            line.Enqueue(old);
                        }
                    }
                    batch = line.Peek();
                }
            }

            int ival = 0;
            switch (simpleKey)
            {
                case "CXNCKD":
                    //车辆车厢内侧宽度
                    ival = Convert.ToInt32(value);
                    if (ival > 0) batch.TruckSizeWidth = Convert.ToInt32(value);
                    break;
                case "CXNCCD":
                    //车辆车厢内侧长度
                    ival = Convert.ToInt32(value);
                    if (ival > 0) batch.TruckSizeLong = Convert.ToInt32(value);
                    break;
                case "CKGD":
                    //车箱大小（高）
                    ival = Convert.ToInt32(value);
                    if (ival > 0) batch.TruckSizeHeight = Convert.ToInt32(value);
                    break;
                case "CPH":
                    //车牌号
                    string truck = batch.TruckLicense;
                    if (truck == "loading")
                    {
                        lock (updateLock)
                        {
                            truck = batch.TruckLicense;
                            batch.TruckLicense = value.ToString();
                            if (truck == "loading" && !string.IsNullOrWhiteSpace(batch.TruckLicense))
                            {
                                Ps_Batch entity = new Ps_Batch();
                                entity.Id = batch.Id;
                                entity.EndTime = batch.EndTime;
                                entity.LineId = batch.LineId;
                                entity.LoadingWeight = batch.LoadingWeight;
                                entity.PicBehind = batch.PicBehind;
                                entity.PicFront = batch.PicFront;
                                entity.PlanPackages = batch.PlanPackages;
                                entity.ProduceDate = batch.ProduceDate;
                                entity.Product = batch.Product;
                                entity.RealPackages = batch.RealPackages;
                                entity.SnatchCount = batch.SnatchCount;
                                entity.StackType = batch.StackType;
                                entity.StartTime = batch.StartTime;
                                entity.Status = batch.Status;
                                entity.TruckLicense = batch.TruckLicense;
                                entity.TruckSizeHeight = batch.TruckSizeHeight;
                                entity.TruckSizeLong = batch.TruckSizeLong;
                                entity.TruckSizeWidth = batch.TruckSizeWidth;
                                List<Ps_Batch> data = new List<Ps_Batch>();
                                data.Add(entity);
                                store.UpdateBatchAsync(data).Wait();
                            }
                        }
                    }            
                    break;
                case "DXSZ":
                    //垛型设置
                    batch.StackType = Convert.ToInt32(value);                    
                    break;
                case "SDZCBS":
                    //设定装车包数
                    batch.PlanPackages = Convert.ToInt32(value);
                    break;
                case "JQRSJZQCS":
                    //机器人实际抓取次数
                    batch.RealPackages = Convert.ToInt32(value);
                    break;
                case "DQZCZL":
                    //当前装车重量
                    batch.LoadingWeight = Convert.ToInt32(value);
                    break;
                case "JQRSJZQBS":
                    //机器人实际抓取包数
                    batch.SnatchCount = Convert.ToInt32(value);
                    break;
            }
        }

        /// <summary>
        /// 装车时间发生变化
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="value"></param>
        private void ZCSJChanged(int lineId, object value, DateTime dtNow)
        {
            var line = Lines[lineId];
            var zcsj = Convert.ToInt32(value);
            if (zcsj == 0)
            {//装车开始
                TruckStartLoading(lineId, line, dtNow);
            }
            //装车时间有时为负数,所以取绝对值
            line.ZCSJ = System.Math.Abs(zcsj);
        }

        /// <summary>
        /// 装车完毕发生变化
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="value"></param>
        private void ZCWBChanged(int lineId, object value, DateTime dtNow)
        {
            var line = Lines[lineId];
            if (Convert.ToBoolean(value))
            {
                var batch = line.Dequeue();
                if (batch != null)
                {
                    batch.Status = 1;
                    batch.EndTime = dtNow;
                    Data.Enqueue(batch);
                }
            }
        }

        /// <summary>
        /// 开始装车
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="dtNow"></param>
        /// <param name="line"></param>
        private void TruckStartLoading(int lineId, LineManager line, DateTime dtNow)
        {
            var batch = line.Dequeue();

            var append = new Ps_Batch();
            append.Id= Convert.ToInt64(System.DateTime.Now.ToString("yyMMddHHmmssfff") + new Random().Next(0, 9999).ToString());
            append.LineId = lineId;
            append.TruckLicense = "loading";
            append.StartTime = dtNow;
            append.ProduceDate = Convert.ToInt32(dtNow.ToString("yyyyMMdd"));
            append.Status = 0;
            append.PlanPackages = 0;
            append.RealPackages = 0;
            append.LoadingWeight = 0;
            append.StackType = 0;
            append.SnatchCount = 0;
            line.Enqueue(append);

            store.InsertBatchAsync(append).Wait();

            if (batch != null)
            {
                batch.Status = 1;
                batch.EndTime = dtNow;
                Data.Enqueue(batch);
            }
        }
    }
}
