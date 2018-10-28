using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Gseey.Framework.Common.Helpers;

namespace Gseey.Framework.Common.AopIntercepor
{
    public class LogInterceptor : BaseInterceptor
    {
        private readonly string GuidStr = new Guid().ToString().Replace("-", "");

        /// <summary>
        /// 注入后执行方法
        /// </summary>
        /// <param name="invocation"></param>
        public override void PostProceed(IInvocation invocation)
        {
            var msg = string.Format("{0}.{1}_{2}", invocation.InvocationTarget, invocation.Method.Name, invocation.Arguments.ToJson());

            var result = string.Format("==========={2}===========\n{0}\n{1}", msg, invocation.ReturnValue.ToJson(), GuidStr);

            LogHelper.RunLog(result, folderName: "LogInterceptor");
        }

        /// <summary>
        /// 注入前执行方法
        /// </summary>
        /// <param name="invocation"></param>
        public override void PreProceed(IInvocation invocation)
        {
            var msg = string.Format("==========={3}===========\n{0}.{1}_{2}", invocation.InvocationTarget, invocation.Method.Name, invocation.Arguments.ToJson(), GuidStr);

            LogHelper.RunLog(msg, folderName: "LogInterceptor");
        }
    }
}
