using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisLockDemo
{
    public class RedisConnectionFactory
    {
        private static readonly Lazy<ConnectionMultiplexer> Connection;

        static RedisConnectionFactory()
        {
            var connectionString = "192.168.249.151:6379,192.168.249.152:6379,192.168.249.153:6379,192.168.249.154:6379,192.168.249.155:6379,192.168.249.156:6379,password=Pxpay@sit";
            var options = ConfigurationOptions.Parse(connectionString);
            Connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
        }

        public static ConnectionMultiplexer GetConnection() => Connection.Value;

        public static RedLockFactory RedisLockFactory
        {
            get
            {
                var multiplexers = new List<RedLockMultiplexer> { GetConnection() };
                return RedLockFactory.Create(multiplexers);
            }
        }
    }
}