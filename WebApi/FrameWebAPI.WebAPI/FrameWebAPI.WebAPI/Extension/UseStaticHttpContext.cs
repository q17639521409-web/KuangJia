using FrameWebAPI.Common.HttpContext;

namespace FrameWebAPI.WebAPI.Extension
{
    public static class UseStaticHttpContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticHttpContexts(this IApplicationBuilder app)
        {
            StaticHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }
    }
}
