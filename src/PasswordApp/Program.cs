using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PasswordApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Task.Run(async () =>
            {
                await Find();

            });

            Task.WaitAll(task);

            Console.Read();
        }

        static async Task Find()
        {
            HttpClient httpClient = new HttpClient();


            // 从元数据中发现端口
            var disco = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5000");

            // 请求以获得令牌
            var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",
                UserName="Admin",
                Password= "123456",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            // 调用API
            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await httpClient.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
