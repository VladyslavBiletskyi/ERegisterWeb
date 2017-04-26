using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using MVCClient.Models;
using MVCClient.Properties;
using Newtonsoft.Json;

namespace MVCClient.Util
{
    public class HttpClientEngine
    {
        private static HttpClient client;
        public static string AccessToken = "";

        private static HttpClient Client
        {
            get
            {
                if (client != null)
                {
                    if (!String.IsNullOrEmpty(AccessToken))
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", AccessToken);
                    }
                    return client;
                }
                else
                {
                    client = CreateClient(AccessToken);
                    return client;
                }
            }
        }

        private static HttpClient CreateClient(string accessToken = "")
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }

        public static object Get(string urlPart, Type castType = null)
        {
            var response = Client.GetAsync(Resources.HostName + urlPart).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException("Ошибка при выполнении запроса");
            }
            string responseString = response.Content.ReadAsStringAsync().Result;
            var json = JsonConvert.DeserializeObject(responseString,castType);
            return json;
        }

        public static async Task<object> Post(string urlPart, object data, Type castType = null)
        {
            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = Client.PostAsync(Resources.HostName + urlPart, byteContent).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException("Ошибка при выполнении запроса");
            }
            string responseString = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject(responseString, castType);
            return json;
        }
        public static async Task<object> Token(List<KeyValuePair<string,string>> data)
        {
            var myContent = new FormUrlEncodedContent(data);
            var response = Client.PostAsync(Resources.HostName + "token", myContent).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException("Ошибка при выполнении запроса");
            }
            string responseString = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<Dictionary<string,string>>(responseString);
            return json;
        }
    }
}