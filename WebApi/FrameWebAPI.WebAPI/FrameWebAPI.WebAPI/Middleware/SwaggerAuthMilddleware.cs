using FrameWebAPI.Service.Extensions;
using FrameWebAPI.Service.Helper;
using System.Net.Http.Headers;
using System.Text;

namespace FrameWebAPI.WebAPI.Middleware
{
    public class SwaggerAuthMilddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerAuthMilddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/index.html") || !AppSetting.App("Startup", "Swagger", "Auth", "Enabled").GetCBool())
            {
                await _next(context).ConfigureAwait(continueOnCapturedContext: false);
                return;
            }
            string text = context.Request.Headers["Authorization"];
            if (text != null && text.StartsWith("Basic "))
            {
                byte[] bytes = Convert.FromBase64String(AuthenticationHeaderValue.Parse(text).Parameter);
                string[] array = Encoding.UTF8.GetString(bytes).Split(':');
                string text2 = array[0];
                string text3 = array[1];
                //验证
                if (text2.Equals(AppSetting.App("Startup", "Swagger", "Auth", "UserName").GetCString()) && text3.Equals(AppSetting.App("Startup", "Swagger", "Auth", "Passwrod").GetCString()))
                {
                    await _next(context).ConfigureAwait(continueOnCapturedContext: false);
                    return;
                }
            }
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = 401;
        }
    }
}
