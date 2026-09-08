using FrameWebAPI.Service.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.Common.HttpContext
{
    public class HttpContextCore
    {
        private readonly IHttpContextAccessor _accessor;

        //private readonly ILogger<HttpContextCore> _logger;

        public string IP => GetIP();

        public string API => GetAPI();

        public string User => GetUser();

        public HttpContextCore(IHttpContextAccessor accessor
            //, ILogger<HttpContextCore> logger
            )
        {
            _accessor = accessor;
            //_logger = logger;
        }

        private string GetIP()
        {
            if (_accessor == null)
            {
                return "IHttpContextAccessor未注册，请在startup中添加app.UseStaticHttpContext();";
            }
            if (_accessor.HttpContext == null || _accessor.HttpContext.Request == null || _accessor.HttpContext.Request.Headers == null)
            {
                return "";
            }
            if (_accessor.HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
            {
                return _accessor.HttpContext.Request.Headers["X-Real-IP"].GetCString();
            }
            if (_accessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return _accessor.HttpContext.Request.Headers["X-Forwarded-For"].GetCString();
            }
            string cString = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].GetCString();
            if (string.IsNullOrEmpty(cString))
            {
                cString = _accessor.HttpContext.Connection.RemoteIpAddress.GetCString();
            }
            return cString;
        }

        private string GetAPI()
        {
            if (_accessor == null)
            {
                return "IHttpContextAccessor未注册，请在startup中添加app.UseStaticHttpContext();";
            }
            if (_accessor.HttpContext == null || _accessor.HttpContext.Request == null)
            {
                return "";
            }
            return _accessor.HttpContext.Request.Path.GetCString().TrimEnd('/').ToLower();
        }

        private string GetUser()
        {
            if (_accessor == null)
            {
                return "IHttpContextAccessor未注册，请在startup中添加app.UseStaticHttpContext();";
            }
            if (_accessor.HttpContext == null || _accessor.HttpContext.Request == null || _accessor.HttpContext.Request.Headers == null)
            {
                return "";
            }
            return _accessor.HttpContext.Request.Headers["x-user"].GetCString();
        }
    }
}
