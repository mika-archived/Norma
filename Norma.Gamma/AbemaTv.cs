using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Norma.Gamma
{
    public class AbemaTv
    {
        public string SecretKey { get; private set; }

        public string AccessToken { get; private set; }

        public AbemaTv(string secretKey)
        {
            SecretKey = secretKey;
        }

        public AbemaTv()
        {
            // Nothing to do
        }

        public async Task<T> GetAsync<T>(string url, params Expression<Func<string, object>>[] parameters)
        {
            var param = parameters
                .Select(w => new KeyValuePair<string, object>(w.Parameters[0].Name, w.Compile().Invoke(null)))
                .ToList();
            return await GetAsync<T>(url, param);
        }

        private async Task<T> GetAsync<T>(string url, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            if (parameters != null)
                url += "?" + string.Join("&", parameters.Select(w => $"{w.Key}={w.Value}"));
            Debug.WriteLine("GET :" + url);

            var httpClient = new HttpClient(new AbemaAuthorizationHandler(this));
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public async Task<T> PostAsync<T>(string url, params Expression<Func<string, object>>[] parameters)
        {
            var param = parameters
                .Select(w => new KeyValuePair<string, object>(w.Parameters[0].Name, w.Compile().Invoke(null)))
                .ToList();
            return await PostAsync<T>(url, param);
        }

        private async Task<T> PostAsync<T>(string url, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            Debug.WriteLine("POST:" + url);

            var httpClient = new HttpClient(new AbemaAuthorizationHandler(this));
            var convedParams = parameters.Select(w => new KeyValuePair<string, string>(
                                                     w.Key,
                                                     w.Value is bool ? w.Value.ToString().ToLower() : w.Value.ToString()));
            HttpContent content = new StringContent(JsonConvert.SerializeObject(convedParams));

            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}