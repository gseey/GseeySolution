namespace Gseey.Framework.Common.AopIntercepor
{
    using Castle.DynamicProxy;
    using Gseey.Framework.Common.Helpers;
    using System;

    /// <summary>
    /// Defines the <see cref="RedisInterceptor" />
    /// </summary>
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

            var value = RedisHelper.StringGet(redisKey);
            if (!string.IsNullOrEmpty(value))
            {
                invocation.ReturnValue = value;
            }
            else
            {
                var redisValue = "fdsfdsfsfsdfsd";
                RedisHelper.StringSet(redisKey, redisValue);
                invocation.ReturnValue = redisValue;
            }
        }
    }
}
