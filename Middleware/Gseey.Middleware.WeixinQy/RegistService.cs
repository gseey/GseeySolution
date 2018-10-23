using Autofac;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.Interfaces;
using Gseey.Middleware.WeixinQy.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.WeixinQy
{
    public class RegistModel : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //base.Load(builder);
            //var autofacHelper = new AutofacHelper(builder);
            //autofacHelper.Register<IChannelConfigService, ChannelConfigService>();
            builder.RegisterType<ChannelConfigService>().As<IChannelConfigService>();
        }
    }
}
