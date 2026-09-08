using FrameWebAPI.Service.Extensions;
using FrameWebAPI.Service.Helper;

namespace FrameWebAPI.WebAPI.Middleware
{
    /// <summary>
    /// 请求头
    /// </summary>
    public class OptionsRequestMildd
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<OptionsRequestMildd> _logger;

        public OptionsRequestMildd(RequestDelegate next, ILogger<OptionsRequestMildd> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.ToUpper() == "OPTIONS")
            {
                if (!AppSetting.App("Startup", "Cors", "EnableAllIPs").GetCBool() 
                    && Array.IndexOf(AppSetting.App("Startup", "Cors", "IPs").GetCString().Split(',')
                    , context.Request.Headers["Origin"].GetCString()) == -1)
                {
                    context.Response.StatusCode = 403;
                    return;
                }
                context.Response.StatusCode = 200;
                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
                context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,OPTIONS,GET,PUT,DELETE");
                context.Response.Headers.Add("Access-Control-Allow-Headers", context.Request.Headers["Access-Control-Request-Headers"]);
            }
            else
            {
                await _next(context);
            }
        }
    }
}
