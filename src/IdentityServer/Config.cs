using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Config
    {
        /// <summary>
        /// 定义API资源
        /// </summary>
        public static IEnumerable<ApiResource> ApiResources =>
             new List<ApiResource>
              {
                    new ApiResource("api1", "我的第一个API")
              };

        /// <summary>
        /// 定义资源范围
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope()
                {
                    Name="api1"
                }
            };

        /// <summary>
        /// 定义访问的资源客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> Clients =>
             new List<Client>
              {
                    new Client()
                    {
                      ClientId = "client",
                      ClientSecrets =
                      {
                            new Secret("secret".Sha256())
                      },
                      AllowedGrantTypes = GrantTypes.ClientCredentials,
                      AllowedScopes = { "api1"}
                    },
                    new Client()
                    {
                      ClientId = "ro.client",
                      ClientSecrets =
                      {
                            new Secret("secret".Sha256())
                      },
                      AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                      AllowedScopes = { "api1"}
                    },
                    new Client  // OIDC 隐式流客户端（MVC）
                    {
                        ClientId = "mvc",
                        ClientName = "Mvc 客户端",
                        AllowedGrantTypes = GrantTypes.Implicit,
                        RequireConsent = true, // 授权确认页面
                        RedirectUris = { "http://localhost:5002/signin-oidc" },// 登录后重定向到的地址
                        AllowedScopes = new List<string>
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile
                        }
                    }
               };

        /// <summary>
        /// 标准的Identity Scope
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> IdentityResources =>
             new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                };

        public static List<TestUser> TestUsers =>
             new List<TestUser> {
               new TestUser {
                   Username = "Admin",
                   Password = "123456",
                   SubjectId = "001",
                   IsActive = true
               }
            };
    }
}
