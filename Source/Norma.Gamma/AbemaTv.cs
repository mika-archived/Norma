using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Norma.Gamma.Api;
using Norma.Gamma.Converters;
using Norma.Gamma.Extensions;

namespace Norma.Gamma
{
    public class AbemaTv
    {
        public string AccessToken { get; set; }

        public Preload Preload => new Preload(this);

        public RootApi Root => new RootApi(this);

        public Users Users => new Users(this);

        public AbemaTv(string accessToken)
        {
            ServicePointManager.Expect100Continue = false;
            AccessToken = accessToken;
        }

        public AbemaTv()
        {
            ServicePointManager.Expect100Continue = false;
        }

        #region Synchronous POST

        public T Post<T>(string url, params Expression<Func<string, object>>[] parameters)
        {
            var param = parameters
                .Select(w => new KeyValuePair<string, object>(w.Parameters[0].Name, w.Compile().Invoke(null)))
                .ToList();
            return Post<T>(url, param);
        }

        private T Post<T>(string url, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            try
            {
                Debug.WriteLine("POST:" + url);

                var httpClient = new HttpClient(new AbemaAuthorizationHandler(this));
                var convedParams = parameters.Select(w => new KeyValuePair<string, object>(
                                                         w.Key,
                                                         w.Value is bool
                                                             ? w.Value.ToString().ToLower()
                                                             : w.Value?.ToString()))
                                             .ToList();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter> {new KeyValuePairConverter()}
                };
                var content = new StringContent2(JsonConvert.SerializeObject(convedParams, settings), Encoding.UTF8,
                                                 "application/json");

                var response = httpClient.PostAsync(url, content).Result;
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return default(T);
        }

        #endregion

        #region Asynchronous POST

        public async Task<T> PostAsync<T>(string url, params Expression<Func<string, object>>[] parameters)
        {
            var param = parameters
                .Select(w => new KeyValuePair<string, object>(w.Parameters[0].Name, w.Compile().Invoke(null)))
                .ToList();
            return await PostAsync<T>(url, param).Stay();
        }

        private async Task<T> PostAsync<T>(string url, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            try
            {
                Debug.WriteLine("POST:" + url);

                var httpClient = new HttpClient(new AbemaAuthorizationHandler(this));
                var convedParams = parameters.Select(w => new KeyValuePair<string, object>(
                                                         w.Key,
                                                         w.Value is bool
                                                             ? w.Value.ToString().ToLower()
                                                             : w.Value?.ToString()))
                                             .ToList();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter> {new KeyValuePairConverter()}
                };
                var content = new StringContent2(JsonConvert.SerializeObject(convedParams, settings), Encoding.UTF8,
                                                 "application/json");

                var response = await httpClient.PostAsync(url, content).Stay();
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().Stay();
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return default(T);
        }

        #endregion

        #region Synchronous GET

        public T Get<T>(string url, params Expression<Func<string, object>>[] parameters)
        {
            var param = parameters
                .Select(w => new KeyValuePair<string, object>(w.Parameters[0].Name, w.Compile().Invoke(null)))
                .ToList();
            return Get<T>(url, param);
        }

        private T Get<T>(string url, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            try
            {
                if (parameters != null)
                    url += "?" + string.Join("&", parameters.Select(w => $"{w.Key}={w.Value}"));
                Debug.WriteLine("GET :" + url);

                var httpClient = new HttpClient(new AbemaAuthorizationHandler(this));
                var response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return default(T);
        }

        #endregion

        #region Asynchronous GET

        public async Task<T> GetAsync<T>(string url, params Expression<Func<string, object>>[] parameters)
        {
            var param = parameters
                .Select(w => new KeyValuePair<string, object>(w.Parameters[0].Name, w.Compile().Invoke(null)))
                .ToList();
            return await GetAsync<T>(url, param).Stay();
        }

        private async Task<T> GetAsync<T>(string url, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            try
            {
                if (parameters != null)
                    url += "?" + string.Join("&", parameters.Select(w => $"{w.Key}={w.Value}"));
                Debug.WriteLine("GET :" + url);

                var httpClient = new HttpClient(new AbemaAuthorizationHandler(this));
                var response = await httpClient.GetAsync(url).Stay();
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().Stay();
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return default(T);
        }

        #endregion
    }
}