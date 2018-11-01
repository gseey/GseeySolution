namespace Gseey.Framework.Common.AopIntercepor
{
    using Castle.DynamicProxy;

    /// <summary>
    /// AOP基类
    /// </summary>
    public abstract class BaseInterceptor : IInterceptor
    {
        /// <summary>
        /// 注入后执行方法
        /// </summary>
        /// <param name="invocation"></param>
        public abstract void PostProceed(IInvocation invocation);

        /// <summary>
        /// 注入前执行方法
        /// </summary>
        /// <param name="invocation"></param>
        public abstract void PreProceed(IInvocation invocation);

        /// <summary>
        /// 执行aop注入
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            PreProceed(invocation);

            invocation.Proceed();

            PostProceed(invocation);
        }
    }
}
