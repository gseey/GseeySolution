namespace Gseey.Framework.Common.Middlewares
{
    using Gseey.Framework.Common.Helpers;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ExceptionHandlerMiddleWare" />
    /// </summary>
    public class ExceptionHandlerMiddleWare
    {
        /// <summary>
        /// Defines the next
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleWare"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/></param>
        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// The Invoke
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                HandleException(context, ex);
            }
        }

        /// <summary>
        /// The HandleException
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/></param>
        /// <param name="ex">The ex<see cref="Exception"/></param>
        private static void HandleException(HttpContext context, Exception ex)
        {
            if (ex == null)
                return;
            //记录日志
            ex.WriteExceptionLog("捕获全局未处理异常", isShowConsole: true);
        }
    }
}
