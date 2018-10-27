using Autofac;
using Autofac.Extras.DynamicProxy;
using Gseey.Framework.Common.AopIntercepor;
using Gseey.Middleware.Weixin.Services;
using Gseey.Middleware.Weixin.Services.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin
{
    public class RegistWeixinModel : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LogInterceptor>();
            builder.RegisterType<MessageHandlerService>()
                .As<IMessageHandlerService>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(LogInterceptor));
        }
    }
}
