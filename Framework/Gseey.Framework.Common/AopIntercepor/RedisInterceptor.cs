using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注入前执行方法
        /// </summary>
        /// <param name="invocation"></param>
        public override void PreProceed(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
