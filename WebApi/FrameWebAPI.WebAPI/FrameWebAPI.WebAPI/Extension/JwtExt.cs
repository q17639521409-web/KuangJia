using Autofac.Core;
using FrameWebAPI.Model.Enum;
using FrameWebAPI.Service.Extensions;
using FrameWebAPI.Service.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FrameWebAPI.WebAPI.Extension
{
    public static class JwtExt
    {
        public static readonly string Issuer = AppSetting.App("Audience", "Issuer");
        public static readonly string Audience = AppSetting.App("Audience", "Audience");
        public static readonly string Audience_Secret_File = AppSetting.App("Audience", "Secret");
        /// <summary>
        /// 添加JWT服务(net6以上使用)
        /// </summary>
        /// <param name="builder"></param>
        public static void AddJwtExt(this WebApplicationBuilder builder)
        {

            builder.Services. AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
               ).AddJwtBearer(options =>
               {

                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true, //是否验证Issuer
                       ValidIssuer = Issuer, //发行人IssuerJwtExt
                       ValidateAudience = true, //是否验证Audience
                       ValidAudience = Audience, //订阅人Audience
                       ValidateIssuerSigningKey = true, //是否验证SecurityKey
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Audience_Secret_File!)), //SecurityKey
                       ValidateLifetime = true, //是否验证失效时间
                       ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
                       RequireExpirationTime = true,
                   };

                   options.Events = new JwtBearerEvents
                   {
                       // 处理未授权访问
                       OnChallenge = delegate (JwtBearerChallengeContext context)
                       {
                           context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                           return Task.CompletedTask;
                       },

                       // 处理 JWT 认证失败
                       OnAuthenticationFailed = delegate (AuthenticationFailedContext context)
                       {
                           JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                           string text = context.Request.Headers["Authorization"].GetCString().Replace("Bearer ", "");

                           // 检查 Token 是否有效
                           if (text.GetIsNotEmptyOrNull() && jwtSecurityTokenHandler.CanReadToken(text))
                           {
                               JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(text);

                               // 检查签发者（Issuer）
                               if (jwtSecurityToken.Issuer != Issuer)
                               {
                                   context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                               }

                               // 检查受众（Audience）
                               if (jwtSecurityToken.Audiences.FirstOrDefault() != Audience)
                               {
                                   context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                               }
                           }

                           // 处理 Token 过期错误
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers.Add("Token-Expired", "true");
                           }

                           return Task.CompletedTask;
                       }
                   };

               }
               );
            // builder.Services.AddAuthorization(options =>
            //     {
            //         /***    "Client" 策略要求用户必须拥有 "Client" 角色才能访问相关资源。
            //         "Admin" 策略要求用户必须拥有 "Admin" 角色才能访问相关资源。
            //         "SystemOrAdmin" 策略要求用户必须拥有 "Admin" 或者 "System" 角色之一才能访问相关资源。***/
            //         options.AddPolicy("User", policy => policy.RequireRole("User").Build());
            //         options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            //         options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("Admin", "User"));
            //     });


        }
        /// <summary>
        /// 添加JWT服务（net5使用）
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddAuthentication_JWTSetup(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            string audience_Secret_String = AppSetting.App("Audience", "SecretFile");// AppSecretConfig.Audience_Secret_String;
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audience_Secret_String));
            string Issuer = AppSetting.App("Audience", "Issuer");
            string Audience = AppSetting.App("Audience", "Audience");
            new SigningCredentials(symmetricSecurityKey, "HS256");
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30.0),
                RequireExpirationTime = true
            };
            services.AddAuthentication(delegate (AuthenticationOptions o)
            {
                o.DefaultScheme = "Bearer";
            }).AddJwtBearer(delegate (JwtBearerOptions o)
            {
                o.TokenValidationParameters = tokenValidationParameters;
                o.Events = new JwtBearerEvents
                {
                    OnChallenge = delegate (JwtBearerChallengeContext context)
                    {
                        context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = delegate (AuthenticationFailedContext context)
                    {
                        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                        string text = context.Request.Headers["Authorization"].GetCString().Replace("Bearer ", "");
                        if (text.GetIsNotEmptyOrNull() && jwtSecurityTokenHandler.CanReadToken(text))
                        {
                            JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(text);
                            if (jwtSecurityToken.Issuer != Issuer)
                            {
                                context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                            }
                            if (jwtSecurityToken.Audiences.FirstOrDefault() != Audience)
                            {
                                context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                            }
                        }
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

    }
     
}
