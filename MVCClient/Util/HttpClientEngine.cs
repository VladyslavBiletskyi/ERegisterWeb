﻿using System;
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
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }

        public static object Get(string urlPart)
        {
            var response = Client.GetAsync(Resources.HostName + urlPart).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<object>(responseString);          
        }

        public static async Task<object> Post(string urlPart, object data)
        {
            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = Client.PostAsync(Resources.HostName + urlPart, byteContent).Result;
            string ans = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(ans);
        }
    }
}