using FrameWebAPI.WebAPI.Extension;
using FrameWebAPI.Service.Helper;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using System.Text;
using FrameWebAPI.WebAPI.Middleware;
using Autofac.Core;
using FrameWebAPI.WebAPI.Extension.SqlSugar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddSwaggerGen();

//.NET Core 在默认情况下是没有注册EncodeProvider
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

//注册AppSetting读取类
builder.Services.AddSingleton(new AppSetting(builder.Configuration));

//扩展方法添加JWT
builder.AddJwtExt();
builder.Services.AddAuthorizationSetup();
// SqlSugar ORM框架
//builder.Services.AddSqlsugarSetup();

// Swagger接口文档
builder.Services.AddSwaggerExt();
builder.Services.AddControllers();
 

//注册httpContext
builder.Services.AddHttpContextAccessor();

// 使用 Autofac 作为服务提供者工厂
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//AddSqlsugarSetup
builder.Services.AddSqlsugarSetup();

// 从配置文件中读取 DLL 路径 
var dlls = AppSetting.App<string>("Startup", "Autofac", "Dlls");

// 注册 Autofac 模块
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AutofacModuleRegister(dlls));
});

//跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
});

#region 注册

#endregion

var app = builder.Build();

//启用路由
app.UseRouting();

//注册context
//app.UseStaticHttpContexts();

// CORS跨域
app.UseCors(AppSetting.App(new string[] { "Startup", "Cors", "PolicyName" }));
 
//扩展方法使用Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UserSwaggerExt();
}

#region test
//using (var scope = app.Services.CreateScope())
//{
//    var service = scope.ServiceProvider.GetService<ICodeMstrService>();
//    if (service == null)
//    {
//        Console.WriteLine("无法解析 CodeMstrService");
//    }
//    else
//    {
//        Console.WriteLine("成功解析 CodeMstrService");
//    }
//}
#endregion
//使用JWT
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.Run();