using System.Collections.Generic;

namespace FrameWebAPI.WebAPI.Extension
{
    ///
    public static class HttpContextSetup
    {
        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<IHttpContextCore, HttpContextCore>();
            //services.AddScoped<IUser, AspNetUser>();
        }
    }
}
