using IdentityModel.Client;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Client
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

            Console.WriteLine("Hello World!");

            Console.Read();
        }

        static async Task Find()
        {
            HttpClient httpClient = new HttpClient();


            // 从元数据中发现端口
            var disco = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5000");

            // 请求以获得令牌
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
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
