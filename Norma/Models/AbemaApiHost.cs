using System;
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

        private string _token;
        public static AbemaApiHost Instance => _instance ?? (_instance = new AbemaApiHost());

        private AbemaApiHost()
        {
            _abemaTv = new AbemaTv();
            _token = "";
        }

        public async void Initialize()
        {
            var devId = Guid.NewGuid().ToString();
            var secret = await _abemaTv.Preload.GetSecretKey(devId);
            var user = await _abemaTv.Users.Verify(applicationKeySecret => secret, deviceId => devId);
            _token = user.Token;
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