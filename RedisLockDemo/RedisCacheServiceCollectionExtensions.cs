using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace RedisLockDemo
{
    public static class RedisCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(services.GetGetConfigString());
            });

            //distributed cache 以下兩種擇一使用
            //方法一
            services.AddOptions<RedisCacheOptions>()
                .Configure<IServiceProvider>((options, sp) =>
                {
                    options.Configuration = services.GetGetConfigString();
                    options.InstanceName = "RedisShakeHand";
                });

            services.AddSingleton<IDistributedCache, RedisCache>();

            //方法二
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = services.GetGetConfigString();
            //    options.InstanceName = "RedisShakeHand";
            //});

            return services;
        }

        private static string GetGetConfigString(this IServiceCollection services)
        {
            return "127.0.0.1:6379,syncTimeout=8000";
        }
    }
}