using FrameWebAPI.Service.Helper;
using FrameWebAPI.WebAPI.Extension.Policys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FrameWebAPI.WebAPI.Extension
{
    public static class AuthorizationSetup
    {
        public static readonly string Issuer = AppSetting.App("Audience", "Issuer");
        public static readonly string Audience = AppSetting.App("Audience", "Audience");
        public static readonly string Audience_Secret_File = AppSetting.App("Audience", "Secret");
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {

            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            services.AddAuthorization(delegate (AuthorizationOptions options)
            {
                options.AddPolicy("Client", delegate (AuthorizationPolicyBuilder policy)
                {
                    policy.RequireRole("Client").Build();
                });
                options.AddPolicy("Admin", delegate (AuthorizationPolicyBuilder policy)
                {
                    policy.RequireRole("Admin").Build();
                });
                options.AddPolicy("SystemOrAdmin", delegate (AuthorizationPolicyBuilder policy)
                {
                    policy.RequireRole("Admin", "System");
                });
                options.AddPolicy("A_S_O", delegate (AuthorizationPolicyBuilder policy)
                {
                    policy.RequireRole("Admin", "System", "Others");
                });
            });
            string issuer = AppSetting.App("Audience", "Issuer");
            string audience = AppSetting.App("Audience", "Audience");
            string audience_Secret_String = Audience_Secret_File;
            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audience_Secret_String)), "HS256");
            List<PermissionItem> permissions = new List<PermissionItem>();
            PermissionRequirement permissionRequirement = new PermissionRequirement("/api/denied", permissions, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", issuer, audience, signingCredentials, TimeSpan.FromSeconds(3600.0));
            services.AddAuthorization(delegate (AuthorizationOptions options)
            {
                options.AddPolicy("Permission", delegate (AuthorizationPolicyBuilder policy)
                {
                    policy.Requirements.Add(permissionRequirement);
                });
            });
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);
        }
    } 
}
