using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.WeixinQy.Middlewares
{
    public class InterceptMiddlware
    {
        private readonly RequestDelegate _next;

        public InterceptMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            PreProceed(context);
            await _next(context);
            PostProceed(context);
        }

        private void PreProceed(HttpContext context)
        {
            Console.WriteLine($"{DateTime.Now} middleware invoke preproceed");
        }

        private void PostProceed(HttpContext context)
        {
            Console.WriteLine($"{DateTime.Now} middleware invoke postproceed");
        }
    }
}
