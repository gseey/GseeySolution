using Gseey.Framework.Common.Helpers;
using System;
using System.Threading;

namespace Gseey.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var e1 = ConfigHelper.Get("test:t0");

            RedisHelper redisHelper = new RedisHelper(0);
            var redisKey = "test";
            var r1 = redisHelper.StringSet(redisKey, "2131232");
            var r2 = redisHelper.StringGet(redisKey);

            Console.WriteLine("Hello World!");

            Console.ReadKey();
        }
    }
}
