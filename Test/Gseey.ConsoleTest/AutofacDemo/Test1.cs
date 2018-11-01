namespace Gseey.ConsoleTest.AutofacDemo
{
    using Castle.DynamicProxy;
    using Gseey.Framework.Common.AopIntercepor;
    using Gseey.Framework.Common.Helpers;
    using System;

    /// <summary>
    /// Defines the <see cref="TestObj" />
    /// </summary>
    public class TestObj
    {
        /// <summary>
        /// Gets or sets the MyProperty
        /// </summary>
        public int MyProperty { get; set; }

        /// <summary>
        /// Gets or sets the MyProperty2
        /// </summary>
        public string MyProperty2 { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ITest1" />
    /// </summary>
    public interface ITest1
    {
        /// <summary>
        /// The TestMethod
        /// </summary>
        void TestMethod();

        /// <summary>
        /// The TestMethod2
        /// </summary>
        /// <param name="input">The input<see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        string TestMethod2(int input);

        /// <summary>
        /// The TestMethod3
        /// </summary>
        /// <param name="input1">The input1<see cref="int"/></param>
        /// <param name="input2">The input2<see cref="string"/></param>
        /// <returns>The <see cref="TestObj"/></returns>
        TestObj TestMethod3(int input1, string input2);

        /// <summary>
        /// The TestMethod4
        /// </summary>
        /// <param name="obj">The obj<see cref="TestObj"/></param>
        /// <returns>The <see cref="TestObj"/></returns>
        TestObj TestMethod4(TestObj obj);
    }

    //[Intercept(typeof(TestIinterceptor))]
    /// <summary>
    /// Defines the <see cref="Test1" />
    /// </summary>
    public class Test1 : ITest1
    {
        /// <summary>
        /// The TestMethod
        /// </summary>
        public void TestMethod()
        {
            Console.WriteLine("in method!!!!!!");
        }

        /// <summary>
        /// The TestMethod2
        /// </summary>
        /// <param name="input">The input<see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string TestMethod2(int input)
        {
            Console.WriteLine("input:" + input);

            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The TestMethod3
        /// </summary>
        /// <param name="input1">The input1<see cref="int"/></param>
        /// <param name="input2">The input2<see cref="string"/></param>
        /// <returns>The <see cref="TestObj"/></returns>
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

        /// <summary>
        /// The TestMethod4
        /// </summary>
        /// <param name="obj">The obj<see cref="TestObj"/></param>
        /// <returns>The <see cref="TestObj"/></returns>
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

    /// <summary>
    /// Defines the <see cref="TestIinterceptor" />
    /// </summary>
    public class TestIinterceptor : BaseInterceptor
    {
        /// <summary>
        /// The PostProceed
        /// </summary>
        /// <param name="invocation">The invocation<see cref="IInvocation"/></param>
        public override void PostProceed(IInvocation invocation)
        {
            Console.WriteLine("=======end=============");
            Console.WriteLine("{0}:拦截{1}方法{2}后,", DateTime.Now.ToString("O"), invocation.InvocationTarget.GetType().BaseType, invocation.Method.Name);
            Console.WriteLine("Done: result was {0}.", invocation.ReturnValue.ToJson());
        }

        /// <summary>
        /// The PreProceed
        /// </summary>
        /// <param name="invocation">The invocation<see cref="IInvocation"/></param>
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

    /// <summary>
    /// Defines the <see cref="TestDemo" />
    /// </summary>
    public class TestDemo
    {
        /// <summary>
        /// The Test
        /// </summary>
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
