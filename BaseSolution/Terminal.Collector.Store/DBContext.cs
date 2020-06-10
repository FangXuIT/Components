using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.Store
{
    public class DBContext
    {

        public static SqlSugarClient Client()
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=10.76.20.194;user id=root;password=ABCabc123;persistsecurityinfo=True;database=zeqp_hlsn;SslMode=none;",
                DbType = DbType.MySql,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            });
        }
    }
}
