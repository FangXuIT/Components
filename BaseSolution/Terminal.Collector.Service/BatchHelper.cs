using org.apache.zookeeper.data;
using System;
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

        public Dictionary<string, Boolean> CurrentStatus { private set; get; }
        
        private BatchHelper()
        {
        }

        public BatchHelper(ICollectorStore store)
        {
            _store = store;
            if (CurrentStatus! != null) CurrentStatus.Clear();
            else CurrentStatus = new Dictionary<string, bool>();

            CurrentStatus.Add("PLC1.Line1.ZCZT", false);
            CurrentStatus.Add("PLC1.Line2.ZCZT", false);
            CurrentStatus.Add("PLC2.Line4.ZCZT", false);
            CurrentStatus.Add("PLC2.Line5.ZCZT", false);
        }

        public async Task SaveBatchData(Int64 plcId, Dictionary<string,object> readData)
        {
            DateTime dtNow = DateTime.Now;
            if(plcId==1)
            {
                #region --1号线--
                string line_key = "PLC1.Line1.ZCZT";
                string line_chp = "PLC1.Line1.CPH";        //车牌号
                string line_sdzcbs = "PLC1.Line1.SDZCBS";  //设定装车包数
                string line_skzl = "PLC1.Line1.DQZCSKZL";  //刷卡重量
                string line_dxsz = "PLC1.Line1.DXSZ";      //垛型设置
                string line_jhzqcs = "PLC1.Line1.JHZQCS";  //计划抓取次数
                int line_id = 1;
                await SaveLineBatchData(readData, dtNow, line_key, line_chp, line_sdzcbs, line_skzl, line_dxsz, line_jhzqcs, line_id);
                #endregion

                #region --2号线--
                line_key = "PLC1.Line2.ZCZT";
                line_chp = "PLC1.Line2.CPH";        //车牌号
                line_sdzcbs = "PLC1.Line2.SDZCBS";  //设定装车包数
                line_skzl = "PLC1.Line2.DQZCSKZL";  //刷卡重量
                line_dxsz = "PLC1.Line2.DXSZ";      //垛型设置
                line_jhzqcs = "PLC1.Line2.JHZQCS";  //计划抓取次数
                line_id = 2;
                await SaveLineBatchData(readData, dtNow, line_key, line_chp, line_sdzcbs, line_skzl, line_dxsz, line_jhzqcs, line_id);
                #endregion
            }
            else if(plcId==2)
            {
                #region --4号线--
                string line_key = "PLC2.Line4.ZCZT";
                string line_chp = "PLC2.Line4.CPH";        //车牌号
                string line_sdzcbs = "PLC2.Line4.SDZCBS";  //设定装车包数
                string line_skzl = "PLC2.Line4.DQZCSKZL";  //刷卡重量
                string line_dxsz = "PLC2.Line4.DXSZ";      //垛型设置
                string line_jhzqcs = "PLC2.Line4.JHZQCS";  //计划抓取次数
                int line_id = 4;
                await SaveLineBatchData(readData, dtNow, line_key, line_chp, line_sdzcbs, line_skzl, line_dxsz, line_jhzqcs, line_id);
                #endregion

                #region --5号线--
                line_key = "PLC2.Line5.ZCZT";
                line_chp = "PLC2.Line5.CPH";        //车牌号
                line_sdzcbs = "PLC2.Line5.SDZCBS";  //设定装车包数
                line_skzl = "PLC2.Line5.DQZCSKZL";  //刷卡重量
                line_dxsz = "PLC2.Line5.DXSZ";      //垛型设置
                line_jhzqcs = "PLC2.Line5.JHZQCS";  //计划抓取次数
                line_id = 5;
                await SaveLineBatchData(readData, dtNow, line_key, line_chp, line_sdzcbs, line_skzl, line_dxsz, line_jhzqcs, line_id);
                #endregion
            }
        }

        private async Task SaveLineBatchData(Dictionary<string, object> readData
            , DateTime dtNow
            , string line_key
            , string line_chp
            , string line_sdzcbs
            , string line_skzl
            , string line_dxsz
            , string line_jhzqcs
            , int line_id)
        {
            if (CurrentStatus[line_key])
            {//当前正在装车中
                if (Convert.ToBoolean(readData[line_key]) == false)
                {//结束装车
                    CurrentStatus[line_key] = false;
                    var list = await _store.GetBatchListAsync(p => p.LineId == line_id && p.Status == 0);
                    foreach (var item in list)
                    {
                        item.Status = line_id;
                        item.EndTime = dtNow;

                        if (Convert.ToInt32(readData[line_key]) > 0)
                        {
                            item.RealPackages = Convert.ToInt32(readData[line_key]);
                        }
                    }
                    await _store.UpdateBatchListAsync(list);
                }
            }
            else
            {//当前车道装车完毕
                var cph = readData[line_chp].ToString();
                if (Convert.ToBoolean(readData[line_key]) == true && !string.IsNullOrWhiteSpace(cph))
                {//开始装车
                    CurrentStatus[line_key] = true;

                    Ps_Batch entity = new Ps_Batch();
                    entity.Id = Convert.ToInt64(System.DateTime.Now.ToString("yyMMddHHmmssfff") + new Random().Next(0, 9999).ToString());
                    entity.ProduceDate = Convert.ToInt32(dtNow.ToString("yyyyMMdd"));
                    entity.LineId = line_id;
                    entity.StartTime = dtNow;
                    entity.Status = 0;

                    entity.TruckLicense = cph;
                    entity.PlanPackages = Convert.ToInt32(readData[line_sdzcbs]);
                    entity.LoadingWeight = Convert.ToDecimal(readData[line_skzl]);
                    entity.StackType = Convert.ToInt32(readData[line_dxsz]);
                    entity.SnatchCount = Convert.ToInt32(readData[line_jhzqcs]);

                    await _store.InsertBatchAsync(entity);
                }
            }
        }
    }
}
