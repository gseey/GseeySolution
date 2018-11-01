using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Gseey.Framework.Common.Middlewares;
using Gseey.Middleware.Weixin;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Gseey.Apis.Weixin
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //// Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddSwaggerGen(c =>
            //{
            //    //c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            //    c.SwaggerDoc("v1", new Info
            //    {
            //        Version = "v1",
            //        Title = "微信api",
            //        Description = "微信api",
            //        TermsOfService = "Gseey",
            //        Contact = new Contact
            //        {
            //            Name = "Gaos",
            //            Email = string.Empty,
            //            Url = "www.gseey.com"
            //        },
            //        License = new License
            //        {
            //            Name = "Use under LICX",
            //            Url = "https://example.com/license"
            //        }
            //    });

            //    // Set the comments path for the Swagger JSON and UI.
            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    c.IncludeXmlComments(xmlPath);
            //});

            var builder = new ContainerBuilder();
            //注册微信服务
            builder.RegisterModule<RegistWeixinModel>();

            builder.Populate(services);
            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseStaticFiles();
            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            //// specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GseeyApi");
            //    c.RoutePrefix = string.Empty;
            //});


            //异常处理中间件
            app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));

            app.UseMvc();

            app.UseHttpsRedirection();
        }
    }
}
