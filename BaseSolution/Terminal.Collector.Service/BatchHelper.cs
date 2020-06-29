using org.apache.zookeeper.data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Terminal.Collector.IStore;
using Terminal.Collector.IStore.Entites;

namespace Terminal.Collector.Service
{
    public class BatchHelper
    {
        private ICollectorStore _store;

        public ConcurrentDictionary<string, Ps_Batch> LineBatch { private set; get; }

        private string line1_key = "Line1";
        private string line2_key = "Line2";
        private string line4_key = "Line4";
        private string line5_key = "Line5";

        private BatchHelper()
        {
        }

        public BatchHelper(ICollectorStore store)
        {
            _store = store;
            if (LineBatch != null) LineBatch.Clear();
            else LineBatch = new ConcurrentDictionary<string, Ps_Batch>();

            var batch = new Ps_Batch();
            batch.Status = 1;
            LineBatch.TryAdd(line1_key, batch);

            batch = new Ps_Batch();
            batch.Status = 1;
            LineBatch.TryAdd(line2_key, batch);

            batch = new Ps_Batch();
            batch.Status = 1;
            LineBatch.TryAdd(line4_key, batch);

            batch = new Ps_Batch();
            batch.Status = 1;
            LineBatch.TryAdd(line5_key, batch);
        }

        public async Task SaveBatchData(Int64 plcId, Dictionary<string,object> readData)
        {
            DateTime dtNow = DateTime.Now;
            if(plcId==1)
            {
                #region --1号线--                
                string line_zt = "PLC1.Line1.ZCZT";             //装车状态
                string line_chp = "PLC1.Line1.CPH";             //车牌号
                string line_sdzcbs = "PLC1.Line1.SDZCBS";       //设定装车包数
                string line_skzl = "PLC1.Line1.DQZCSKZL";       //刷卡重量
                string line_dxsz = "PLC1.Line1.DXSZ";           //垛型设置
                string line_jhzqcs = "PLC1.Line1.JHZQCS";       //计划抓取次数
                string line_sjzqbs = "PLC1.Line1.JQRSJZQBS";    //实际抓取包数
                string truck_width = "PLC1.Line1.CXNCKD";       //车箱宽
                string truck_heigth = "PLC1.Line1.CXNCCD";      //车箱高
                string truck_long = "PLC1.Line1.CKGD";          //车箱长

                await SaveLineBatchData(readData, dtNow, line1_key, line_zt, line_chp, line_sdzcbs, line_skzl, line_dxsz, line_jhzqcs, line_sjzqbs, truck_width, truck_heigth, truck_long, 1);
                #endregion

                #region --2号线--
                line_zt = "PLC1.Line2.ZCZT";        //装车状态
                line_chp = "PLC1.Line2.CPH";        //车牌号
                line_sdzcbs = "PLC1.Line2.SDZCBS";  //设定装车包数
                line_skzl = "PLC1.Line2.DQZCSKZL";  //刷卡重量
                line_dxsz = "PLC1.Line2.DXSZ";      //垛型设置
                line_jhzqcs = "PLC1.Line2.JHZQCS";  //计划抓取次数
                line_sjzqbs = "PLC1.Line2.JQRSJZQBS";    //实际抓取包数
                truck_width = "PLC1.Line2.CXNCKD";       //车箱宽
                truck_heigth = "PLC1.Line2.CXNCCD";      //车箱高
                truck_long = "PLC1.Line2.CKGD";          //车箱长

                await SaveLineBatchData(readData, dtNow, line2_key, line_zt, line_chp, line_sdzcbs, line_skzl, line_dxsz, line_jhzqcs, line_sjzqbs, truck_width, truck_heigth, truck_long, 2);
                #endregion
            }
            else if(plcId==2)
            {
                #region --4号线--
                string line_zt = "PLC2.Line4.ZCZT";         //装车状态
                string line_chp = "PLC2.Line4.CPH";        //车牌号
                string line_sdzcbs = "PLC2.Line4.SDZCBS";  //设定装车包数
                string line_skzl = "PLC2.Line4.DQZCSKZL";  //刷卡重量
                string line_dxsz = "PLC2.Line4.DXSZ";      //垛型设置
                string line_jhzqcs = "PLC2.Line4.JHZQCS";  //计划抓取次数
                string line_sjzqbs = "PLC2.Line4.JQRSJZQBS";    //实际抓取包数
                string truck_width = "PLC2.Line4.CXNCKD";       //车箱宽
                string truck_heigth = "PLC2.Line4.CXNCCD";      //车箱高
                string truck_long = "PLC2.Line4.CKGD";          //车箱长

                await SaveLineBatchData(readData, dtNow, line4_key, line_zt, line_chp, line_sdzcbs, line_skzl, line_dxsz, line_jhzqcs, line_sjzqbs, truck_width, truck_heigth, truck_long, 4);
                #endregion

                #region --5号线--
                line_zt = "PLC2.Line5.ZCZT";        //装车状态
                line_chp = "PLC2.Line5.CPH";        //车牌号
                line_sdzcbs = "PLC2.Line5.SDZCBS";  //设定装车包数
                line_skzl = "PLC2.Line5.DQZCSKZL";  //刷卡重量
                line_dxsz = "PLC2.Line5.DXSZ";      //垛型设置
                line_jhzqcs = "PLC2.Line5.JHZQCS";  //计划抓取次数
                line_sjzqbs = "PLC2.Line5.JQRSJZQBS";    //实际抓取包数
                truck_width = "PLC2.Line5.CXNCKD";       //车箱宽
                truck_heigth = "PLC2.Line5.CXNCCD";      //车箱高
                truck_long = "PLC2.Line5.CKGD";          //车箱长

                await SaveLineBatchData(readData, dtNow, line5_key, line_zt, line_chp, line_sdzcbs, line_skzl, line_dxsz, line_jhzqcs, line_sjzqbs, truck_width, truck_heigth, truck_long, 5);
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readData">读取数据</param>
        /// <param name="dtNow">当前时间</param>
        /// <param name="line_zt">装车状态</param>
        /// <param name="line_chp">车牌号</param>
        /// <param name="line_sdzcbs">设定装车包数</param>
        /// <param name="line_skzl">刷卡重量</param>
        /// <param name="line_dxsz">垛型设置</param>
        /// <param name="line_jhzqcs">计划抓取次数</param>
        /// <param name="line_sjzqbs">实际抓取包数</param>
        /// <param name="line_id">产线编号</param>
        /// <returns></returns>
        private async Task SaveLineBatchData(Dictionary<string, object> readData
            , DateTime dtNow
            , string line_key
            , string line_zt
            , string line_chp
            , string line_sdzcbs
            , string line_skzl
            , string line_dxsz
            , string line_jhzqcs
            , string line_sjzqbs
            , string truck_width
            , string truck_heigth
            , string truck_long
            , int line_id)
        {
            if (LineBatch[line_key].Status==0)
            {//当前正在装车中
                if (Convert.ToInt32(readData[line_sjzqbs]) > 0) LineBatch[line_key].RealPackages = Convert.ToInt32(readData[line_sjzqbs]);
                if (Convert.ToInt32(readData[line_jhzqcs])>0) LineBatch[line_key].SnatchCount = Convert.ToInt32(readData[line_jhzqcs]);

                if (LineBatch[line_key].LoadingWeight == 0) LineBatch[line_key].LoadingWeight = Convert.ToDecimal(readData[line_skzl]);
                if (LineBatch[line_key].PlanPackages == 0) LineBatch[line_key].PlanPackages = Convert.ToInt32(readData[line_sdzcbs]);

                if (LineBatch[line_key].TruckSizeWidth == 0) LineBatch[line_key].TruckSizeWidth = Convert.ToInt32(readData[truck_width]);
                if (LineBatch[line_key].TruckSizeHeight == 0) LineBatch[line_key].TruckSizeHeight = Convert.ToInt32(readData[truck_heigth]);
                if (LineBatch[line_key].TruckSizeLong == 0) LineBatch[line_key].TruckSizeLong = Convert.ToInt32(readData[truck_long]);
                if(LineBatch[line_key].StackType==0) LineBatch[line_key].StackType = Convert.ToInt32(readData[line_dxsz]);

                if (Convert.ToBoolean(readData[line_zt]) == false)
                {//结束装车
                    LineBatch[line_key].Status = 1;
                    LineBatch[line_key].RealPackages = LineBatch[line_key].PlanPackages;                    
                    LineBatch[line_key].EndTime = dtNow;
                    await _store.UpdateBatchAsync(LineBatch[line_key]);
                }
            }
            else
            {//当前车道装车完毕
                var cph = readData[line_chp].ToString();
                if (Convert.ToBoolean(readData[line_zt]) == true && !string.IsNullOrWhiteSpace(cph))
                {
                    if (cph== LineBatch[line_key].TruckLicense
                        && LineBatch[line_key].Status==1
                        && LineBatch[line_key].StartTime>=DateTime.Now.AddMinutes(-30))
                    {//继续装车
                        LineBatch[line_key].Status = 0;
                        await _store.UpdateBatchAsync(LineBatch[line_key]);
                    }
                    else
                    {//开始装车
                        LineBatch[line_key] = null;
                        LineBatch[line_key] = new Ps_Batch();
                        LineBatch[line_key].Id = Convert.ToInt64(System.DateTime.Now.ToString("yyMMddHHmmssfff") + new Random().Next(0, 9999).ToString());
                        LineBatch[line_key].ProduceDate = Convert.ToInt32(dtNow.ToString("yyyyMMdd"));
                        LineBatch[line_key].LineId = line_id;
                        LineBatch[line_key].StartTime = dtNow;                        
                        LineBatch[line_key].Status = 0;

                        LineBatch[line_key].TruckLicense = cph;
                        LineBatch[line_key].PlanPackages = Convert.ToInt32(readData[line_sdzcbs]);
                        LineBatch[line_key].LoadingWeight = Convert.ToDecimal(readData[line_skzl]);
                        LineBatch[line_key].StackType = Convert.ToInt32(readData[line_dxsz]);
                        LineBatch[line_key].SnatchCount = Convert.ToInt32(readData[line_jhzqcs]);

                        LineBatch[line_key].TruckSizeWidth = Convert.ToInt32(readData[truck_width]);
                        LineBatch[line_key].TruckSizeHeight = Convert.ToInt32(readData[truck_heigth]);
                        LineBatch[line_key].TruckSizeLong = Convert.ToInt32(readData[truck_long]);
                    }

                    await _store.InsertBatchAsync(LineBatch[line_key]);
                }
            }
        }
    }
}
