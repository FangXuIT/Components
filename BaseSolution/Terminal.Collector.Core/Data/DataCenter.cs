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

        private DataCenter()
        {
            successed = false;
            store = new CollectorStoreImple();
            Lines = new Dictionary<int, LineManager>();


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
            KeyLineRelation.Add("PLC1.Line1.ZCZT", LineID);          //装车状态
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
            KeyLineRelation.Add("PLC1.Line2.ZCZT", LineID);          //装车状态
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
            KeyLineRelation.Add("PLC2.Line4.ZCZT", LineID);          //装车状态
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
            KeyLineRelation.Add("PLC2.Line5.ZCZT", LineID);          //装车状态
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
                case "ZCZT"://装车状态
                    ZCZTChanged(lineId, value, dtNow);
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

            int ival = 0;
            switch (simpleKey)
            {
                case "CXNCKD":
                    //车辆车厢内侧宽度
                    ival = Convert.ToInt32(value);
                    if (ival >= 0) line.Item.TruckSizeWidth = Convert.ToInt32(value);
                    break;
                case "CXNCCD":
                    //车辆车厢内侧长度
                    ival = Convert.ToInt32(value);
                    if (ival >= 0) line.Item.TruckSizeLong = Convert.ToInt32(value);
                    break;
                case "CKGD":
                    //车箱大小（高）
                    ival = Convert.ToInt32(value);
                    if (ival >= 0) line.Item.TruckSizeHeight = Convert.ToInt32(value);
                    break;
                case "CPH":
                    //车牌号
                    if(string.IsNullOrWhiteSpace(line.Item.TruckLicense))
                    {
                        line.Item.TruckLicense = value.ToString();
                        store.InsertBatchAsync(line.Item);
                    }
                    line.Item.TruckLicense = value.ToString();
                    break;
                case "DXSZ":
                    //垛型设置
                    line.Item.StackType = Convert.ToInt32(value);                    
                    break;
                case "SDZCBS":
                    //设定装车包数
                    line.Item.PlanPackages = Convert.ToInt32(value);
                    break;
                case "JQRSJZQCS":
                    //机器人实际抓取次数
                    line.Item.RealPackages = Convert.ToInt32(value);
                    break;
                case "DQZCZL":
                    //当前装车重量
                    line.Item.LoadingWeight = Convert.ToInt32(value);
                    break;
                case "JQRSJZQBS":
                    //机器人实际抓取包数
                    line.Item.SnatchCount = Convert.ToInt32(value);
                    break;
            }
        }

        /// <summary>
        /// 装车状态发生变化
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="value"></param>
        private void ZCZTChanged(int lineId, object value, DateTime dtNow)
        {
            var line = Lines[lineId];
            var ZCZT = Convert.ToBoolean(value);
            if (line.ZCZT!=ZCZT)
            {//装车开始
                if(ZCZT)
                {//开始装车
                    if(line.Item.Id>0 && line.Item.Status==0)
                    {
                        line.Item.Status = 1;
                        line.Item.EndTime = DateTime.Now;

                        store.UpdateBatchAsync(line.Item);
                    }

                    line.Item = new Ps_Batch();
                    line.Item.Id = Convert.ToInt64(System.DateTime.Now.ToString("yyMMddHHmmssfff") + new Random().Next(0, 9999).ToString());
                    line.Item.LineId = lineId;
                    line.Item.TruckLicense = "";
                    line.Item.StartTime = dtNow;
                    line.Item.ProduceDate = Convert.ToInt32(dtNow.ToString("yyyyMMdd"));
                    line.Item.Status = 0;
                    line.Item.PlanPackages = 0;
                    line.Item.RealPackages = 0;
                    line.Item.LoadingWeight = 0;
                    line.Item.StackType = 0;
                    line.Item.SnatchCount = 0;
                }
                else
                {//结束装车
                    line.Item.Status = 1;
                    line.Item.EndTime = DateTime.Now;

                    store.UpdateBatchAsync(line.Item);
                }
            }
            line.ZCZT = ZCZT;
        }
    }
}
