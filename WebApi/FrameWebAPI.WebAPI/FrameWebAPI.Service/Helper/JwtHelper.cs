using FrameWebAPI.Service.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FrameWebAPI.Service.Helper
{
    /// <summary>
    /// JWT颁发通行证
    /// </summary>
    public class JwtHelper
    {
        #region  颁发

        public static readonly string Issuer = AppSetting.App("Audience", "Issuer");
        public static readonly string Audience = AppSetting.App("Audience", "Audience");
        public static readonly string Audience_Secret_File = AppSetting.App("Audience", "Secret");

        public static string IssueJwt(TokenModelJwt tokenModel)
        {
            string text = AppSetting.App("Audience", "Issuer");
            string value = AppSetting.App("Audience", "Audience");
            string audience_Secret_String = Audience_Secret_File;
            List<Claim> list = new List<Claim>
        {
            new Claim("jti", tokenModel.Uid.ToString()),
            new Claim("iat", $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
            new Claim("nbf", $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
            new Claim("exp", $"{new DateTimeOffset(tokenModel.IsPermanent.GetCBool() ? DateTime.MaxValue.ToUniversalTime() : DateTime.Now.AddSeconds(3600.0)).ToUnixTimeSeconds()}"),
            new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration", tokenModel.IsPermanent.GetCBool() ? DateTime.MaxValue.ToString() : DateTime.Now.AddSeconds(3600.0).ToString()),
            new Claim("iss", text),
            new Claim("aud", value)
        };
            list.AddRange(from s in tokenModel.Role.Split(',')
                          select new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", s));
            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(audience_Secret_String)), "HS256");
            SigningCredentials signingCredentials2 = signingCredentials;
            JwtSecurityToken token = new JwtSecurityToken(text, null, list, null, null, signingCredentials2);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            TokenModelJwt result = new TokenModelJwt();
            if (jwtStr.GetIsNotEmptyOrNull() && jwtSecurityTokenHandler.CanReadToken(jwtStr))
            {
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtStr);
                jwtSecurityToken.Payload.TryGetValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var value);
                jwtSecurityToken.Payload.TryGetValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration", out var value2);
                result = new TokenModelJwt
                {
                    Uid = jwtSecurityToken.Id.GetCString(),
                    Role = ((value != null) ? value.GetCString() : ""),
                    Expiration = value2.GetCDate(),
                    IsPermanent = (value2.GetCDate().Date == DateTime.MaxValue.GetCDate().Date)
                };
            }
            return result;
        }
        #endregion
    }

    public class TokenModelJwt
    {
        public string Uid { get; set; }

        public string Role { get; set; }

        public DateTime Expiration { get; set; }

        public bool IsPermanent { get; set; }
        /// <summary>
        /// 过期时间 (分钟)
        /// </summary>
        public int PastDueTime { get; set; }
    }
}
