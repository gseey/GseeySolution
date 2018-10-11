using Gseey.Framework.Common.Helpers;
using System;
using System.Text;
using System.Threading;

namespace Gseey.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var h1 = HttpHelper.GetHtml("http://esf.fang.com/chushou/3_417109133.htm?channel=2,2&psid=1_1_70");
            var h2 = HttpHelper.GetHtmlAsync("https://www.cnblogs.com/sunxucool/p/4180375.html").Result;

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
