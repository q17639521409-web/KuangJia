using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using FrameWebAPI.IService.DB;
using FrameWebAPI.Service.DB;
using FrameWebAPI.WebAPI.Models.Models;
using SqlSugar;

namespace FrameWebAPI.WebAPI.Extension
{
    /// <summary>
    /// autofac注入
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {
        readonly List<string> _dlls;

        public AutofacModuleRegister(List<string> dlls)
        {
            _dlls = dlls;
        }

        protected override void Load(ContainerBuilder builder)
        {
            string baseDirectory = AppContext.BaseDirectory;
            List<Type> list = new List<Type>();
            foreach (string dll in _dlls)
            {
                string text = Path.Combine(baseDirectory, dll);
                if (!File.Exists(text))
                {
                    throw new Exception(dll + "丢失，检查AppSetting.json中Autofac配置；如果项目的bin目录下没有该文件，检查是否有引用项目");
                    //throw new Exception("FrameWebAPI.Service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。");
                }
                //实例化IOC工厂类
                //builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
                Assembly assembly = Assembly.LoadFrom(text);

                builder.RegisterGeneric(typeof(DbContext<>))
               .As(typeof(IDbContext<>))
               .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(assembly)
                    .AsImplementedInterfaces()
                    .InstancePerDependency()
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(list.ToArray());
            }
        }
    }
}
