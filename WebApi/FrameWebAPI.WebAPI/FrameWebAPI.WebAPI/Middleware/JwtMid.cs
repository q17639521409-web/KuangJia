using FrameWebAPI.Service.Helper;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FrameWebAPI.WebAPI.Middleware
{
    public static class JwtMid
    {
        /// <summary>
        /// Swagger接口文档
        /// </summary>
        /// <param name="application"></param>
        public static void UserSwaggerExt(this WebApplication application)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }
            application.UseMiddleware<SwaggerAuthMilddleware>(Array.Empty<object>());
            application.UseSwagger();
            application.UseSwaggerUI(delegate (SwaggerUIOptions c)
            {
                c.RoutePrefix = "";
                string ApiName = AppSetting.App("Startup", "Swagger", "ApiName");
                AppSetting.App<string>(new string[3] { "Startup", "Swagger", "Versions" }).ForEach(delegate (string version)
                {
                    string text = (string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..");
                    c.SwaggerEndpoint($"{text}/swagger/{version}/swagger.json", $"{ApiName} {version}");
                    //c.SwaggerEndpoint(text + "/swagger/" + version + "  ", ApiName + " " + version);
                });
            });
        }



    }
}
