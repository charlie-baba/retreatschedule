using RetreatSchedule.Poco.Paystack;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace RetreatSchedule.Services
{
    public class RestClient
    {
        public static async Task<T> GetJsonAsync<T>(string authorization, string baseUrl, string path)
        {
            HttpClient client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrWhiteSpace(authorization))
                {
                    client.DefaultRequestHeaders.Add("Authorization", authorization);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                        "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                }

                T response = default(T);
                HttpResponseMessage resp = await client.GetAsync(path);
                if (resp.IsSuccessStatusCode)
                    response = await resp.Content?.ReadAsAsync<T>();

                return response;
            }
            finally
            {
                client.Dispose();
            }
            
        }

        public static async Task<T> PostJsonAsync<T>(string authorization, string baseUrl, string path, Object req)
        {
            HttpClient client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrWhiteSpace(authorization))
                {
                    client.DefaultRequestHeaders.Add("Authorization", authorization);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                        "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                }

                T response = default(T);
                HttpResponseMessage resp = await client.PostAsJsonAsync(path, req);
                if (resp.IsSuccessStatusCode)
                    response = await resp.Content?.ReadAsAsync<T>();

                return response;
            }
            finally
            {
                client.Dispose();
            }

        }
    }
}
