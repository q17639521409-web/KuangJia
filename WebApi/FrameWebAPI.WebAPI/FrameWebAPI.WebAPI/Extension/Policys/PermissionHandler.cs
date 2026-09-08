using FrameWebAPI.Model.Basic;
using FrameWebAPI.Service.Extensions;
using FrameWebAPI.Service.Helper;
using FrameWebAPI.Service.HttpHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace FrameWebAPI.WebAPI.Extension.Policys
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _accessor;

        //private readonly IRedisBasketRepository _cache;

        public IAuthenticationSchemeProvider Schemes { get; set; }

        public PermissionHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            Schemes = schemes;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            _ = 2;
            try
            {
                HttpContext httpContext = _accessor.HttpContext;
                if (httpContext != null)
                {
                    httpContext.Request.Path.Value.ToLower();
                    if (!AppSetting.App("Authentication", "Enabled").GetCBool())
                    {
                        context.Succeed(requirement);
                    }
                    else if (httpContext.Request.Headers.ContainsKey("x-token"))
                    {
                        //验证
                        //string cString = httpContext.Request.Headers["x-user"].GetCString();
                        //string token = httpContext.Request.Headers["x-token"].GetCString();
                        //string cString2 = httpContext.Request.Headers["x-corp"].GetCString();
                        //string cString3 = httpContext.Request.Headers["x-domain"].GetCString();
                        //if (AppSetting.App("Authentication", "RemoteAuthentication").GetCBool())
                        //{
                        //    string cString4 = AppSetting.App("Authentication", "RemoteUrl").GetCString();
                        //    new ResultModel();
                        //    ResultModel resultModel = ((!(AppSetting.App("Authentication", "Type").GetCString().ToLower() == "progress")) ? HttpHelper.GetApi<ResultModel>(cString4, "", "userUid=" + cString + "&token=" + token) : ((ResultModel)JsonHelper.JsonToObject(new HttpRequestHelper
                        //    {
                        //        Uri = cString4,
                        //        Type = HttpType.POST,
                        //        BodyType = BodyType.Json,
                        //        Body = new Dictionary<string, object>
                        //    {
                        //        { "corp", cString2 },
                        //        { "domain", cString3 },
                        //        { "userid", cString },
                        //        { "sessionid", token }
                        //    }
                        //    }.Request(), typeof(ResultModel))));
                        //    SerilogHelper.WriteLog("PermissionHandler", "HandleRequirementAsync", "rm", JsonHelper.ObjectToJson(resultModel));
                        //    if (resultModel.Code != 10000)
                        //    {
                        //        context.Fail();
                        //    }
                        //    else
                        //    {
                        //        context.Succeed(requirement);
                        //    }
                        //    return;
                        //}
                        //TokenModelJwt tmj = JwtHelper.SerializeJwt(token);
                        //if ((await _cache.GetValue(tmj.Uid.GetCString())).GetCString().Trim('"') != token)
                        //{
                        //    context.Fail();
                        //    return;
                        //}
                        //if (tmj.Expiration < DateTime.Now)
                        //{
                        //    if ((await _cache.GetValue(tmj.Uid.GetCString() + "_Expiration")).GetCString().Trim('"').GetCDate() < DateTime.Now)
                        //    {
                        //        context.Fail();
                        //        return;
                        //    }
                        //    httpContext.Response.Headers["X-RefreshToken"] = JwtHelper.IssueJwt(tmj);
                        //}
                        //await _cache.Set(tmj.Uid.GetCString() + "_Expiration", DateTime.Now.AddSeconds(7200.0).ToString(), TimeSpan.FromMinutes(120.0));
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                        }
                }
                else
                {
                    context.Fail();
                }
            }
            catch (Exception ex)
            {
                SerilogHelper.WriteErrorLog("PermissionHandler", "HandleRequirementAsync", ex);
                context.Fail();
            }
        }
    }
}
