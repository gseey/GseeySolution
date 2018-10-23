using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Gseey.Framework.Common.AopIntercepor;
using Gseey.Framework.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gseey.ConsoleTest.AutofacDemo
{
    public class TestObj
    {
        public int MyProperty { get; set; }

        public string MyProperty2 { get; set; }
    }

    public interface ITest1
    {

        void TestMethod();

        string TestMethod2(int input);

        TestObj TestMethod3(int input1, string input2);
        TestObj TestMethod4(TestObj obj);
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
            Console.WriteLine("input:" + input);

            return Guid.NewGuid().ToString();
        }

        public TestObj TestMethod3(int input1, string input2)
        {
            Console.WriteLine("input1:" + input1);

            var obj = new TestObj
            {
                MyProperty = input1,
                MyProperty2 = input2
            };
            return obj;
        }

        public TestObj TestMethod4(TestObj obj)
        {
            Console.WriteLine("in method!!!!!!");
            if (obj == null)
                obj = new TestObj
                {
                    MyProperty = 111111,
                    MyProperty2 = "2222222"
                };
            return obj;
        }
    }

    public class TestIinterceptor : BaseInterceptor
    {
        public override void PostProceed(IInvocation invocation)
        {
            Console.WriteLine("=======end=============");
            Console.WriteLine("{0}:拦截{1}方法{2}后,", DateTime.Now.ToString("O"), invocation.InvocationTarget.GetType().BaseType, invocation.Method.Name);
            Console.WriteLine("Done: result was {0}.", invocation.ReturnValue.ToJson());
        }

        public override void PreProceed(IInvocation invocation)
        {
            Console.WriteLine("{0}:拦截{1}方法{2}前,", DateTime.Now.ToString("O"), invocation.InvocationTarget.GetType().BaseType, invocation.Method.Name);
            Console.WriteLine("Calling method {0} with parameters {1}... ",
                invocation.Method.Name,
                invocation.Arguments.ToJson()
                );
            Console.WriteLine("=======before=============");
        }
    }

    public class TestDemo
    {
        public static void Test()
        {
            //AutofacHelper.Register<ITest1, Test1>();
            AutofacHelper.Register<ITest1, Test1, TestIinterceptor>();
            var it1 = AutofacHelper.Resolve<ITest1>();
            it1.TestMethod();
            Console.WriteLine("--------");
            var ss2 = it1.TestMethod2(1212);
            Console.WriteLine("--------");
            var ss3 = it1.TestMethod3(1212, "fsafasfasfsaf");
            Console.WriteLine("--------");
            var ss4 = it1.TestMethod4(null);
            Console.WriteLine("--------");
            var ss5 = it1.TestMethod4(new TestObj { MyProperty = 2132131, MyProperty2 = "afljlfajflsa" });
            Console.WriteLine("--------");

            Console.WriteLine(ss2);
            Console.WriteLine(ss3);
            Console.WriteLine(ss4);
            Console.WriteLine(ss5);
        }
    }
}
