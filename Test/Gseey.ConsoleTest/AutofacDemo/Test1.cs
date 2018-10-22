using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gseey.ConsoleTest.AutofacDemo
{
    public interface ITest1
    {

        void TestMethod();

        string TestMethod2(int input);
    }

    //[Intercept(typeof(TestIinterceptor))]
    public class Test1 : ITest1
    {
        public void TestMethod()
        {
            Console.WriteLine("in method!!!!!!");
        }

        public string TestMethod2(int input)
        {
            Console.WriteLine("input:"+input);

            return Guid.NewGuid().ToString();
        }
    }

    public class TestIinterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Calling method {0} with parameters {1}... ",
                invocation.Method.Name,
                string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
            Console.WriteLine("=======before=============");

            invocation.Proceed();

            Console.WriteLine("=======end=============");
            Console.WriteLine("Done: result was {0}.", invocation.ReturnValue);
        }
    }

}
