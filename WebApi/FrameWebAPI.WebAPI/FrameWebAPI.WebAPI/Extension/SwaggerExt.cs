using Autofac.Core;
using FrameWebAPI.Model.Enum;
using FrameWebAPI.Service.Extensions;
using FrameWebAPI.Service.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Configuration;
using System.Text;

namespace FrameWebAPI.WebAPI.Extension
{
    public static class SwaggerExt
    {
         
        /// <summary>
        /// 添加Swagger生成服务   
        /// </summary>
        /// <param name="service"></param>
        public static void AddSwaggerExt(this IServiceCollection service)
        {
            //配置 JWT Bearer 授权
            //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //{
            //    Description = "JWT Authorization header using the Bearer scheme",
            //    Name = "Authorization",
            //    In = ParameterLocation.Header,
            //    Type = SecuritySchemeType.Http,
            //    Scheme = "bearer"
            //});

            #region 请求头
            if (service == null)
            {
                throw new ArgumentNullException("services");
            }
            _ = AppContext.BaseDirectory;
            // builder.Services.AddSingleton(new JwtHelper(builder.Configuration));
            string ApiName = AppSetting.App("Startup", "Swagger", "ApiName").GetCString();
            string XmlName = AppSetting.App("Startup", "Swagger", "XmlName").GetCString();
            string projectName = AppSetting.App("Startup", "Swagger", "ProjectName").GetCString();

            service.AddSwaggerGen(options =>
            {
                #region 配置swagger

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = EnumApiVerson.v1.GetCString(),
                    Title = ApiName + " 接口文档",
                    Description = ApiName + " RESTful API",
                    Contact = new OpenApiContact
                    {
                        Name = projectName + "项目",
                        Email = "@qq.com"
                    }
                });
                options.OrderActionsBy((ApiDescription o) => o.RelativePath);//Swagger 文档中的 API 规则排序

                string path = XmlName ?? "";//FrameWebAPI.WebAPI.xml
                string filePath = Path.Combine(AppContext.BaseDirectory, path);
                options.IncludeXmlComments(filePath, includeControllerXmlComments: true);

                List<string> list = AppSetting.App<string>(new string[3] { "Startup", "Swagger", "Xmls" });
                if (list != null)
                {
                    foreach (string item in list)
                    {
                        string text = Path.Combine(AppContext.BaseDirectory, item);
                        if (File.Exists(text))
                        {
                            options.IncludeXmlComments(text, includeControllerXmlComments: true);
                        }
                    }
                }
                #endregion


                options.AddSecurityDefinition("x-token", new OpenApiSecurityScheme
                {
                    Description = "在下框中输入请求头中添加Jwt授权Token：Bearer Token",
                    Name = "x-token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Token"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "x-token"
                            }
                        },
                                new string[0]
                    } });
                options.AddSecurityDefinition("x-user", new OpenApiSecurityScheme
                {
                    Description = "用户ID",
                    Name = "x-user",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "x-user"
                            }
                        },
                                new string[0]
                    } });
                options.AddSecurityDefinition("x-corp", new OpenApiSecurityScheme
                {
                    Description = "公司ID",
                    Name = "x-corp",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "x-corp"
                            }
                        },
                                new string[0]
                    } });
                options.AddSecurityDefinition("x-domain", new OpenApiSecurityScheme
                {
                    Description = "域ID",
                    Name = "x-domain",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "x-domain"
                            }
                        },
                                new string[0]
                    } });
                options.AddSecurityDefinition("x-language", new OpenApiSecurityScheme
                {
                    Description = "语言ID",
                    Name = "x-language",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "x-language"
                            }
                        },
                                new string[0]
                    } });
                options.CustomSchemaIds((Type o) => o.FullName);
            });
            #endregion

        }
    }
}
