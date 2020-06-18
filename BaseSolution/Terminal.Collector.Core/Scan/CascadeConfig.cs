using System.Collections.Generic;

namespace Terminal.Collector.Core.Scan
{
    public sealed class CascadeConfig
    {
        #region --单例--
        public static CascadeConfig Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            internal static readonly CascadeConfig instance;

            // 静态构造方法，保证线程安全、延迟加载，且只执行一次
            static Nested()
            {
                instance = new CascadeConfig();
            }
        }

        private CascadeConfig()
        {
            Relations = new Dictionary<string, CascadeRelation>();
            #region --一号线--
            var rel = new CascadeRelation(1,1);
            
            rel.CascadeTargetKey.Add("PLC1.Line1.CXNCKD");      //车辆车厢内侧宽度
            rel.CascadeTargetKey.Add("PLC1.Line1.CXNCCD");      //车辆车厢内侧长度
            rel.CascadeTargetKey.Add("PLC1.Line1.CKGD");        //车箱大小（高）
            rel.CascadeTargetKey.Add("PLC1.Line1.CPH");         //车牌号
            rel.CascadeTargetKey.Add("PLC1.Line1.DXSZ");        //垛型设置
            rel.CascadeTargetKey.Add("PLC1.Line1.SDZCBS");      //设定装车包数
            //当装车时间,由0转为非0时
            rel.LimitValue = 0;
            Relations.Add("PLC1.Line1.ZCSJ", rel);

            rel = new CascadeRelation(1, 1);
            rel.CascadeTargetKey.Add("PLC1.Line1.JQRSJZQCS");   //机器人实际抓取次数
            rel.CascadeTargetKey.Add("PLC1.Line1.DQZCZL");      //当前装车重量
            rel.CascadeTargetKey.Add("PLC1.Line1.JQRSJZQBS");   //机器人实际抓取包数

            //当装车完毕,由false转为true时
            rel.LimitValue = false;
            Relations.Add("PLC1.Line1.ZCWB", rel);
            #endregion

            #region --二号线--
            rel = new CascadeRelation(1, 2);
            rel.CascadeTargetKey.Add("PLC1.Line2.CXNCKD");      //车辆车厢内侧宽度
            rel.CascadeTargetKey.Add("PLC1.Line2.CXNCCD");      //车辆车厢内侧长度
            rel.CascadeTargetKey.Add("PLC1.Line2.CKGD");        //车箱大小（高）
            rel.CascadeTargetKey.Add("PLC1.Line2.CPH");         //车牌号
            rel.CascadeTargetKey.Add("PLC1.Line2.DXSZ");        //垛型设置
            rel.CascadeTargetKey.Add("PLC1.Line2.SDZCBS");      //设定装车包数
            //一号线-当装车时间,由0转为非0时
            rel.LimitValue = 0;
            Relations.Add("PLC1.Line2.ZCSJ", rel);

            rel = new CascadeRelation(1, 2);
            rel.CascadeTargetKey.Add("PLC1.Line2.JQRSJZQCS");   //机器人实际抓取次数
            rel.CascadeTargetKey.Add("PLC1.Line2.DQZCZL");      //当前装车重量
            rel.CascadeTargetKey.Add("PLC1.Line2.JQRSJZQBS");   //机器人实际抓取包数

            //当装车完毕,由false转为true时
            rel.LimitValue = false;
            Relations.Add("PLC1.Line2.ZCWB", rel);
            #endregion

            #region --四号线--
            rel = new CascadeRelation(2, 4);
            rel.CascadeTargetKey.Add("PLC2.Line4.CXNCKD");      //一车辆车厢内侧宽度
            rel.CascadeTargetKey.Add("PLC2.Line4.CXNCCD");      //车辆车厢内侧长度
            rel.CascadeTargetKey.Add("PLC2.Line4.CKGD");        //车箱大小（高）
            rel.CascadeTargetKey.Add("PLC2.Line4.CPH");         //车牌号
            rel.CascadeTargetKey.Add("PLC2.Line4.DXSZ");        //垛型设置
            rel.CascadeTargetKey.Add("PLC2.Line4.SDZCBS");      //设定装车包数
            //当装车时间,由0转为非0时
            rel.LimitValue = 0;
            Relations.Add("PLC2.Line4.ZCSJ", rel);

            rel = new CascadeRelation(2, 4);
            rel.CascadeTargetKey.Add("PLC2.Line4.JQRSJZQCS");   //机器人实际抓取次数
            rel.CascadeTargetKey.Add("PLC2.Line4.DQZCZL");      //当前装车重量
            rel.CascadeTargetKey.Add("PLC2.Line4.JQRSJZQBS");   //机器人实际抓取包数

            //当装车完毕,由false转为true时
            rel.LimitValue = false;
            Relations.Add("PLC2.Line4.ZCWB", rel);
            #endregion

            #region --五号线--
            rel = new CascadeRelation(2, 5);
            rel.CascadeTargetKey.Add("PLC2.Line5.CXNCKD");      //车辆车厢内侧宽度
            rel.CascadeTargetKey.Add("PLC2.Line5.CXNCCD");      //车辆车厢内侧长度
            rel.CascadeTargetKey.Add("PLC2.Line5.CKGD");        //车箱大小（高）
            rel.CascadeTargetKey.Add("PLC2.Line5.CPH");         //车牌号
            rel.CascadeTargetKey.Add("PLC2.Line5.DXSZ");        //垛型设置
            rel.CascadeTargetKey.Add("PLC2.Line5.SDZCBS");      //设定装车包数
            //当装车时间,由0转为非0时
            rel.LimitValue = 0;
            Relations.Add("PLC2.Line5.ZCSJ", rel);

            rel = new CascadeRelation(2, 5);
            rel.CascadeTargetKey.Add("PLC2.Line5.JQRSJZQCS");   //机器人实际抓取次数
            rel.CascadeTargetKey.Add("PLC2.Line5.DQZCZL");      //当前装车重量
            rel.CascadeTargetKey.Add("PLC2.Line5.JQRSJZQBS");   //机器人实际抓取包数

            //当装车完毕,由false转为true时
            rel.LimitValue = false;
            Relations.Add("PLC2.Line5.ZCWB", rel);
            #endregion
        }
        #endregion

        public Dictionary<string,CascadeRelation> Relations { private set; get; }
    }
}
