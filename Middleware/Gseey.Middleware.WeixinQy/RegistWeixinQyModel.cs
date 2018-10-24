using Autofac;
using Autofac.Extras.DynamicProxy;
using Gseey.Framework.Common.AopIntercepor;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.Interfaces;
using Gseey.Middleware.WeixinQy.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.WeixinQy
{
    public class RegistWeixinQyModel : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LogInterceptor>();
            builder.RegisterType<ChannelConfigService>()
                .As<IChannelConfigService>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(LogInterceptor));
        }
    }
}
