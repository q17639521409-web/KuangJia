using System;
using FrameWebAPI.Service.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Abstractions;


namespace FrameWebAPI.Common.HttpContext
{
    public static class StaticHttpContext
    {
        private static IHttpContextAccessor _accessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext; 

        public static string IP => GetIP();

        public static string API => GetAPI();

        public static string User => GetUser();

        public static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private static string GetIP()
        {
            if (_accessor == null)
            {
                Console.WriteLine("IHttpContextAccessor未注册，请在startup中添加app.UseStaticHttpContext();");
                return "";
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

        private static string GetAPI()
        {
            if (_accessor == null)
            {
                Console.WriteLine("IHttpContextAccessor未注册，请在startup中添加app.UseStaticHttpContext();");
                return "";
            }
            if (_accessor.HttpContext == null || _accessor.HttpContext.Request == null)
            {
                return "";
            }
            return _accessor.HttpContext.Request.Path.GetCString().TrimEnd('/').ToLower();
        }

        private static string GetUser()
        {
            if (_accessor == null)
            {
                Console.WriteLine("IHttpContextAccessor未注册，请在startup中添加app.UseStaticHttpContext();");
                return "";
            }
            if (_accessor.HttpContext == null || _accessor.HttpContext.Request == null || _accessor.HttpContext.Request.Headers == null)
            {
                return "";
            }
            return _accessor.HttpContext.Request.Headers["x-user"].GetCString();
        }
    }
}
