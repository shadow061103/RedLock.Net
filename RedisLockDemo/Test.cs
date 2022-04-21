using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RedisLockDemo
{
    public class Test
    {
        private static object _lock = new object();

        public static void CheckReceiveBet(string membercode)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: received.");
            lock (_lock)
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: lock start. at {DateTime.Now}");
                Thread.Sleep(5 * 1000);
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: lock end.at {DateTime.Now}");
            }

            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: done.");
        }

        public static void CheckReceiveBet2(string membercode)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: received.");
            var resource = $"lockkey";//resource lock key
            var expiry = TimeSpan.FromSeconds(30);//lock key expire 時間
            var wait = TimeSpan.FromSeconds(10);//放棄重試時間
            var retry = TimeSpan.FromSeconds(1);//重試間隔時間

            //傳入 resource lock key , expiry, wait, retry
            using (var redLock = RedisConnectionFactory.RedisLockFactory.CreateLockAsync(resource, expiry, wait, retry).Result)
            {
                // 確定取得 lock 所有權
                if (redLock.IsAcquired)
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: lock start. at {DateTime.Now}");
                    Thread.Sleep(5 * 1000);
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: lock end.at {DateTime.Now}");
                }
                else
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:Not get the locker");
            }
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}_{membercode}: done.");
        }
    }
}