using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.Service.Helper
{
    public class AppSetting
    {
        static IConfiguration Configuration { get; set; }
        static string ContentPath { get; set; }

        public AppSetting()
        {
            string path = "AppSetting.json";
            Configuration = new ConfigurationBuilder().SetBasePath(ContentPath).Add(new JsonConfigurationSource
            {
                Path = path,
                Optional = false,
                ReloadOnChange = true
            }).Build();
        }
        public AppSetting(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 获取指定属性值
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string App(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    if (Configuration != null)
                    {
                        return Configuration[string.Join(":", sections)];
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> App<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }


        //public static string app(params string[] sections)
        //{
        //    try
        //    {
        //        if (sections.Any())
        //        {
        //            if (Configuration != null)
        //            {
        //                return Configuration[string.Join(":", sections)];
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return "";
        //}


        public static List<T> app<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }

    }
}
