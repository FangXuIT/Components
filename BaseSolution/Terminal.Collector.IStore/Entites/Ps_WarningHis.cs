﻿using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace Terminal.Collector.IStore.Entites
{
    ///<summary>
    ///
    ///</summary>
    public partial class Ps_WarningHis
    {
           public Ps_WarningHis(){


           }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>        
        [SugarColumn(IsPrimaryKey = true)] //是主键, 还是标识列
        public long Id {get;set;}

           /// <summary>
           /// Desc:设备Id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int EquipmentId {get;set;}

           /// <summary>
           /// Desc:报警Id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int WarningId {get;set;}

           /// <summary>
           /// Desc:报警日期
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime WarningDate {get;set;}

           /// <summary>
           /// Desc:报警开始时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime StartTime {get;set;}

           /// <summary>
           /// Desc:报警结束时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? EndTime {get;set;}

    }
}
