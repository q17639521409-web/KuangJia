using Autofac.Core;
using System.Text;

namespace FrameWebAPI.WebAPI.Extension
{
    public static class NewtoJsonExt
    {
        public static void AddNewJsonExt(this IServiceCollection service)
        {
            //service.AddControllers().AddNewtonsoftJson(options =>
            //{
            //    option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;//忽略循环引用（使接口正常返回DataTable）
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //});  
        }
    }
}
