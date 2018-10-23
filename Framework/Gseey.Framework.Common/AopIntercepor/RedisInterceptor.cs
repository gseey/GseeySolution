using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Gseey.Framework.Common.Helpers;

namespace Gseey.Framework.Common.AopIntercepor
{
    public class RedisInterceptor : BaseInterceptor
    {
        /// <summary>
        /// 注入后执行方法
        /// </summary>
        /// <param name="invocation"></param>
        public override void PostProceed(IInvocation invocation)
        {
        }

        /// <summary>
        /// 注入前执行方法
        /// </summary>
        /// <param name="invocation"></param>
        public override void PreProceed(IInvocation invocation)
        {
            var redisKey = string.Format("{0}.{1}_{2}", invocation.InvocationTarget, invocation.Method.Name, invocation.Arguments.ToJson());
            Console.WriteLine(redisKey);

            var redisHelper = new RedisHelper();
            var value = redisHelper.StringGet(redisKey);
            if (!string.IsNullOrEmpty(value))
            {
                invocation.ReturnValue = value;
            }
            else
            {
                var redisValue = "fdsfdsfsfsdfsd";
                redisHelper.StringSet(redisKey, redisValue);
                invocation.ReturnValue = redisValue;
            }
        }
    }
}
