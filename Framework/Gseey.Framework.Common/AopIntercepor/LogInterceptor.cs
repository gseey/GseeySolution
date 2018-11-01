namespace Gseey.Framework.Common.AopIntercepor
{
    using Castle.DynamicProxy;
    using Gseey.Framework.Common.Helpers;
    using System;

    /// <summary>
    /// Defines the <see cref="LogInterceptor" />
    /// </summary>
    public class LogInterceptor : BaseInterceptor
    {
        /// <summary>
        /// Defines the GuidStr
        /// </summary>
        private readonly string GuidStr = new Guid().ToString().Replace("-", "");

        /// <summary>
        /// 注入后执行方法
        /// </summary>
        /// <param name="invocation"></param>
        public override void PostProceed(IInvocation invocation)
        {
            //LogHelper.RunLog(string.Format("info"), logLevel: LogHelper.LogLevelEnum.Warn);

            var msg = string.Format("{0}.{1}_{2}", invocation.InvocationTarget, invocation.Method.Name, invocation.Arguments.ToJson());

            var result = string.Format("==========={2}===========\n{0}\n{1}", msg, invocation.ReturnValue.ToJson(), GuidStr);

            //LogHelper.RunLog(result, folderName: "LogInterceptor");
            Console.WriteLine(result);
        }

        /// <summary>
        /// 注入前执行方法
        /// </summary>
        /// <param name="invocation"></param>
        public override void PreProceed(IInvocation invocation)
        {
            LogHelper.RunLog(string.Format("info"), logLevel: LogHelper.LogLevelEnum.Warn);

            var msg = string.Format("==========={3}===========\n{0}.{1}_{2}", invocation.InvocationTarget, invocation.Method.Name, invocation.Arguments.ToJson(), GuidStr);

            //LogHelper.RunLog(msg, folderName: "LogInterceptor");
            Console.WriteLine(msg);
        }
    }
}
