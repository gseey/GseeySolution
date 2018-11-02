using Gseey.Framework.Common.Extensions;
using Gseey.Framework.Common.Helpers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Framework.Common.Attributes
{
    public class ExceptionlessLogAttribute : Attribute, IAsyncActionFilter
    {
        private string _moudleName { get; set; }
        private string _methodName { get; set; }
        /// <summary>
        /// 构造日志类型
        /// </summary>
        /// <param name="moudle">模块名称</param>
        /// <param name="method">方法名称</param>
        public ExceptionlessLogAttribute(string moudle, string method)
        {
            _moudleName = moudle;
            _methodName = method;
        }
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var actiondescriptor = ((ControllerActionDescriptor)context.ActionDescriptor);
            //var _eLLog = context.HttpContext.RequestServices.GetService(typeof(IExceptionlessLogService)) as IExceptionlessLogService;
            //var model = context.ActionArguments.ToJson();
            //_eLLog.AddSource($"[{_moudleName}]{_methodName}{string.Format("{0:yyyy年MM月dd日 HH:mm:ss}", DateTime.Now)}")
            //    .AddMessage("参数内容:" + model ?? "")
            //    .AddTag(actiondescriptor.ControllerName)
            //    .AddTag(actiondescriptor.ActionName)
            //    .AddTag(_moudleName)
            //    .AddTag(_methodName)
            //    .AddTag(string.Format("{0:yyyy-MM-dd}", DateTime.Now))
            //    .AddSubmitInfo();

            await next.Invoke();
        }
    }
}
