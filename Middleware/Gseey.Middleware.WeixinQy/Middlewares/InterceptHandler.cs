using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.WeixinQy.Middlewares
{
    public static class InterceptHandler
    {
        public static IApplicationBuilder UseInterceptMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<InterceptMiddlware>();
        }
    }
}
