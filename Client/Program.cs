using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test1();
            //Test2();
            RunDemo().Wait();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        private static async Task RunDemo()
        {
            var client = new HttpClient();

            // discover endpoints from metadata
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:8001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }


            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = disco.TokenEndpoint,

                ClientId = "client1",
                ClientSecret = "123qwe",

                //Scope = "api1,AuthServerApi",
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            Console.WriteLine("访问Api1");
            // call api
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:8003/api/values");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }


            Console.WriteLine("访问Api2");
            response = await client.GetAsync("http://localhost:8004/api/values");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }







        /// <summary>
        /// 使用客户端认证控制API访问
        /// </summary>
        public static async void Test1()
        {
            //从元数据中发现端口
            //设置 new DiscoveryClient().Policy.RequireHttps=false;关闭https检查，默认非localhost是有https检查得
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            // 请求令牌
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            SendGet(tokenResponse.AccessToken);
        }

        /// <summary>
        /// 使用密码认证方式控制API访问
        /// </summary>
        private static async void Test2()
        {
            ////` 从元数据中发现客户端
            //var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            //// 请求令牌
            //var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            //var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("bob", "password", "api1");//使用用户名密码

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                UserName = "bob",
                Password = "password",
                Scope = "api1",
                ClientId = "ro.client",
                ClientSecret = "secret",
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            SendGet(tokenResponse.AccessToken);
        }

        private static async void SendGet(string token)
        {
            Console.WriteLine("调用api");
            // 调用api
            var client = new HttpClient();
            client.SetBearerToken(token);

            var response = await client.GetAsync("http://localhost:5001/api/identity");  //identity
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

        public static async void Test3()
        {
            var client = new HttpClient();

            //// discover endpoints from metadata
            //var disco = await client.GetDiscoveryDocumentAsync("http://localhost:8001");
            //if (disco.IsError)
            //{
            //    Console.WriteLine(disco.Error);
            //    return;
            //}


            //// request token
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            //{
            //    Address = disco.TokenEndpoint,

            //    ClientId = "client1",
            //    ClientSecret = "123qwe",

            //    Scope = "api1",
            //});

            //if (tokenResponse.IsError)
            //{
            //    Console.WriteLine(tokenResponse.Error);
            //    return;
            //}

            //Console.WriteLine(tokenResponse.Json);  tokenResponse.AccessToken
            string token = "f3cdc974fc5fab48e013225f764cff3326319205b7623635756ab9a97c79c754";
            // call api
            client.SetBearerToken(token);

            var response = await client.GetAsync("http://localhost:8003/api/values");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }

        }
    }
}
