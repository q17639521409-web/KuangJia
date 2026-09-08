using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace FrameWebAPI.WebAPI.Extension.Policys
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public List<PermissionItem> Permissions { get; set; }

        public string DeniedAction { get; set; }

        public string ClaimType { internal get; set; }

        public string LoginPath { get; set; } = "/Api/Login";


        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public PermissionRequirement(string deniedAction, List<PermissionItem> permissions, string claimType, string issuer, string audience, SigningCredentials signingCredentials, TimeSpan expiration)
        {
            ClaimType = claimType;
            DeniedAction = deniedAction;
            Permissions = permissions;
            Issuer = issuer;
            Audience = audience;
            Expiration = expiration;
            SigningCredentials = signingCredentials;
        }
    }
}
