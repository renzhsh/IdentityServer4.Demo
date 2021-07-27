using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Storage
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

        public static IEnumerable<Client> Clients =>
             new List<Client> { };
    }
}
