using System;
using System.Linq;
using System.Text;

namespace Terminal.Collector.IStore.Entites
{
    ///<summary>
    ///
    ///</summary>
    public partial class Ps_Batch
    {
        public Ps_Batch() {


        }
        /// <summary>
        /// Desc:主键
        /// Default:
        /// Nullable:False
        /// </summary>           
        public long Id { get; set; }

        /// <summary>
        /// Desc:装车线
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int LineId { get; set; }

        /// <summary>
        /// Desc:装车日期
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int ProduceDate { get; set; }

        /// <summary>
        /// Desc:车牌
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string TruckLicense { get; set; }

        /// <summary>
        /// Desc:车箱大小（长）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? TruckSizeLong { get; set; }

        /// <summary>
        /// Desc:车箱大小（宽）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? TruckSizeWidth { get; set; }

        /// <summary>
        /// Desc:车箱大小（高）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? TruckSizeHeight { get; set; }

        /// <summary>
        /// Desc:品种
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Product { get; set; }

        /// <summary>
        /// Desc:装车开始时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Desc:装车结束时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Desc:计划装车包数
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int PlanPackages { get; set; }

        /// <summary>
        /// Desc:实际装车包数
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int RealPackages { get; set; }

        /// <summary>
        /// Desc:装车重量（吨）
        /// Default:0.000
        /// Nullable:False
        /// </summary>           
        public decimal LoadingWeight { get; set; }

        /// <summary>
        /// Desc:垛型
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int StackType { get; set; }

        /// <summary>
        /// Desc:抓取次数
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int SnatchCount { get; set; }

        /// <summary>
        /// Desc:前摄像照片
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string PicFront { get; set; }

        /// <summary>
        /// Desc:后摄像照片
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string PicBehind { get; set; }

        /// <summary>
        /// 状态:0=装车中;1=装车完成;
        /// </summary>
        public int Status { set; get; }

    }
}
