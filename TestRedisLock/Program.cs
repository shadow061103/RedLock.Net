using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestRedisLock
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Task.Run(() =>
            {
                Test.CheckReceive("123");
            });
            Task.Run(() =>
            {
                Test.CheckReceive("456");
            });

            Console.ReadKey();
        }
    }

    internal class Test
    {
        public static async Task CheckReceive(string membercode)
        {
            var existingConnectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");

            var multiplexers = new List<RedLockMultiplexer>
                            { existingConnectionMultiplexer};
            var redlockFactory = RedLockFactory.Create(multiplexers);

            var resource = "lockkey";
            var expiry = TimeSpan.FromSeconds(30);

            using (var redLock = await redlockFactory.CreateLockAsync(resource, expiry)) // there are also non async Create() methods
            {
                // make sure we got the lock
                if (redLock.IsAcquired)
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: lock start. at {DateTime.Now}");
                    Thread.Sleep(5 * 1000);
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: lock end.at {DateTime.Now}");
                }
                else
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}:Not get the locker");
                }
            }
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: done.");
        }
    }
}