
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ConnectionPools
{
    public class Connection
    {
        public Connection(DataBaseTypeEnum type)
        {
            IsUsed = false;
            DbConnection = CreateConnection(type);
        }

        /// <summary>
        /// 是否被使用
        /// </summary>
        public bool IsUsed { get; set; }

        public DbConnection DbConnection { get; }

        private DbConnection CreateConnection(DataBaseTypeEnum type)
        {
            DbConnection conn = null;
            switch (type)
            {
                case DataBaseTypeEnum.SqlService:
                    //conn = new SqlConnection();
                    break;
                case DataBaseTypeEnum.MyService:
                    conn = new MySqlConnection();
                    break;
                default:
                    break;
            }
            return conn;
        }
    }
}
