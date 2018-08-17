using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class HttpHelper
    {
        private static HttpClient _client;
        public static HttpClient Client => _client;

        static HttpHelper()
        {
            _client = new HttpClient();
            _client.Timeout = new TimeSpan(0, 0, 20);
            //_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<string> GetAsync(string url)
        {
            //HttpResponseMessage msg = await _client.GetAsync(new Uri(url)).ConfigureAwait(false);
            //var content = await msg.Content.ReadAsStringAsync().ConfigureAwait(false);
            //return content;

            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;//请求类型
            request.RequestUri = new Uri(url);//请求地址
            HttpResponseMessage response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        /// <summary>
        /// 发送Post请求，入参为json格式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static async Task<string> PostJsonAsync(string url, string jsonData)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;//请求类型
            request.RequestUri = new Uri(url);
            request.Content = new StringContent(jsonData);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            HttpResponseMessage response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public static async Task<string> PostAsync(string url, IDictionary<string, string> jsonData)
        {
            //HttpContent requestContent = new StringContent(jsonData);
            //_client.DefaultRequestHeaders.Add("contentType", "application/json");
            //HttpResponseMessage msg = await _client.PostAsync(url, requestContent).ConfigureAwait(false);
            //var content = await msg.Content.ReadAsStringAsync().ConfigureAwait(false);
            //return content;
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;//请求类型
            httpRequestMessage.RequestUri = new Uri(url);
            httpRequestMessage.Content = new FormUrlEncodedContent(jsonData);
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            var content = await SendAsync(httpRequestMessage);
            return content;
        }

        public static async Task<string> SendAsync(HttpRequestMessage req)
        {
            HttpResponseMessage msg = await _client.SendAsync(req);
            var content = await msg.Content.ReadAsStringAsync();
            return content;
        }
    }
}
