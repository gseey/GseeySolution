using Gseey.Framework.Common.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Framework.Common.Middlewares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            this.next = next;
        }

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

        private static void HandleException(HttpContext context, Exception ex)
        {
            if (ex == null)
                return;
            //记录日志
            ex.WriteExceptionLog("捕获全局未处理异常", isShowConsole: true);

            var location = ConfigHelper.Get("ErrorPage", "/");
            context.Response.Redirect(location);
        }
    }
}
