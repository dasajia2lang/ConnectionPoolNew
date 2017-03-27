using System;
using System.Collections.Generic;
using System.Data.Common;

namespace ConnectionPools
{
    public class ConnectionPools
    {

        private volatile static ConnectionPools _pools=null;

        private static object _lockObj = new object();
        private ConnectionPools(DataBaseTypeEnum type, int maxLength)
        {
            DataBaseType = type;
            MaxLength = maxLength;
            InitPools(type,maxLength);
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseTypeEnum DataBaseType { get;}

        /// <summary>
        /// 连接池最大连接个数
        /// </summary>
        public int MaxLength { get; }

        /// <summary>
        /// 连接池
        /// </summary>
        /// 
        private Connection[] _connections;
        public Connection[] Connections => _connections;

        /// <summary>
        /// 尝试获取连接
        /// </summary>
        /// <returns></returns>
        public Connection TryGetConnection()
        {
            lock (_lockObj)
            {
                for (int i = 0; i < MaxLength; i++)
                {
                    var con=_connections[i];
                    if (!con.IsUsed)
                    {
                        con.IsUsed = true;
                        return con;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 初始化连接池
        /// </summary>
        private void InitPools(DataBaseTypeEnum type, int maxLength)
        {
            if (_connections == null)
            {
                _connections = new Connection[maxLength];
            }
            for (int i = 0; i < MaxLength; i++)
            {
                var conn = new Connection(DataBaseType);
                _connections.SetValue(conn, i);
            }
        }

        /// <summary>
        /// 还回连接
        /// </summary>
        /// <returns></returns>
        public bool FreeConnection(Connection conn)
        {
            conn.IsUsed=false;
            return true;
        }

        /// <summary>
        /// 单例模式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public ConnectionPools CreateInstance(DataBaseTypeEnum type,int maxLength)
        {
            if (_pools == null)
            {
                lock (_lockObj)
                {
                    if (_pools == null)
                    {
                        return new ConnectionPools(type, maxLength);
                    }
                }
            }
            return _pools;
        }
    }
}
