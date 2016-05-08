using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Norma.Gamma;
using Norma.Gamma.Models;

namespace Norma.Models
{
    // 出来る限り叩かない
    public class AbemaApiHost
    {
        private static AbemaApiHost _instance;
        private readonly AbemaTv _abemaTv;

        public static AbemaApiHost Instance => _instance ?? (_instance = new AbemaApiHost());

        private AbemaApiHost()
        {
            _abemaTv = string.IsNullOrWhiteSpace(Configuration.Instance.Root.AccessToken)
                ? new AbemaTv()
                : new AbemaTv(Configuration.Instance.Root.AccessToken);
        }

        public async void Initialize()
        {
            if (string.IsNullOrWhiteSpace(_abemaTv.AccessToken))
                await GetToken();
            else
            {
                try
                {
                    var token = await _abemaTv.Root.Token(osName => "pc", osVersion => "1.0",
                                                          osLang => "ja", osTimezone => "Asia/Tokyo");
                    Debug.WriteLine($"AccessToken is valid : {token.Token}");
                }
                catch
                {
                    _abemaTv.AccessToken = "";
                    await GetToken();
                }
            }
        }

        private async Task GetToken()
        {
            var devId = Guid.NewGuid().ToString();
            var secret = await _abemaTv.Preload.GetSecretKey(devId);
            var user = await _abemaTv.Users.Verify(applicationKeySecret => secret, deviceId => devId);
            _abemaTv.AccessToken = user.Token;
            Configuration.Instance.Root.AccessToken = user.Token;
        }

        public async Task<Comments> Comments(string slotId)
            => await _abemaTv.Root.Comments(slotId, limit => 100);

        public async Task<Media> MediaOfCurrent()
        {
            var today = DateTime.Today.ToString("yyyyMMdd");
            var tomorrow = DateTime.Today.AddDays(1).ToString("yyyyMMdd");
            return await _abemaTv.Root.Media(dateFrom => today, dateTo => tomorrow);
        }
    }
}