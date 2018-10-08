using Gseey.Framework.Common.Helpers;
using System;
using System.Threading;

namespace Gseey.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = 0;
            while (count<1000)
            {
                Thread.Sleep(1000);
                var confingValue = ConfigHelper.Get("test:t1:t2", "fsfsdfsfsfsdf");

                Console.WriteLine(confingValue);

                var connectionStr = ConfigHelper.GetConnectionString("t2");

                Console.WriteLine(connectionStr);
            }

            Console.WriteLine("Hello World!");

            Console.ReadKey();
        }
    }
}
