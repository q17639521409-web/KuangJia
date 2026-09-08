using FrameWebAPI.WebAPI.Extension;

namespace FrameWebAPI.WebAPI.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public static class MiddlewareHelpers
    {
        public static IApplicationBuilder UseJwtTokenAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtTokenAuth>(Array.Empty<object>());
        }

        //public static IApplicationBuilder UseIPLogMildd(this IApplicationBuilder app)
        //{
        //    return app.UseMiddleware<IPLogMildd>(Array.Empty<object>());
        //}

        //public static IApplicationBuilder UseRecordAccessLogsMildd(this IApplicationBuilder app)
        //{
        //    return app.UseMiddleware<RecordAccessLogsMildd>(Array.Empty<object>());
        //}

        public static IApplicationBuilder UseOptionsReuestMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<OptionsRequestMildd>(Array.Empty<object>());
        }
    }
}
