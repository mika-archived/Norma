using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Norma.Gamma.Extensions;

namespace Norma.Gamma.Api
{
    // Reference: https://github.com/fushihara/abema-tvguide/blob/master/abema-onair-schedule/AbemaApi.cs
    public class Preload : AbemaTvApiImpl
    {
        private static readonly Regex MainjsRegex = new Regex("c=r\\(l\\),f=\"(.+?)\"");

        public Preload(AbemaTv abemaTv) : base(abemaTv)
        {

        }

        public string GetSecretKey(string deviceId)
        {
            var wc = new WebClient();
            var str = wc.DownloadString("https://abema.tv/main.js");
            var match = MainjsRegex.Match(str);
            if (!match.Success)
                throw new Exception("Cannot get secret key from abematv. Please wait update.");
            return ComputeSecretKey(match.Groups[1].Value, deviceId);
        }

        public async Task<string> GetSecretKeyAsync(string deviceId)
        {
            // もし main.js から取れなくなったら、 Chrome LocalStorage から取得するようにする。
            var wc = new WebClient();
            var str = await wc.DownloadStringTaskAsync("https://abema.tv/main.js").Stay();
            var match = MainjsRegex.Match(str);
            if (!match.Success)
                throw new Exception("Cannot get secret key from abematv. Please wait update.");
            return ComputeSecretKey(match.Groups[1].Value, deviceId);
        }

        private string ComputeSecretKey(string secretKey, string deviceId)
        {
            var time = DateTime.Now;
            time = time.AddMinutes(-time.Minute).AddSeconds(-time.Second).AddHours(1).ToUniversalTime();

            var hmacsha256 = new HMACSHA256(GetByteChars(secretKey));
            var hash = hmacsha256.ComputeHash(GetByteChars(secretKey));
            for (var i = 0; i < time.Month; i++)
                hash = hmacsha256.ComputeHash(hash);
            hash = hmacsha256.ComputeHash(GetByteChars(ComputeB64(hash) + deviceId));
            for (var i = 0; i < time.Day % 5; i++)
                hash = hmacsha256.ComputeHash(hash);
            hash = hmacsha256.ComputeHash(GetByteChars(ComputeB64(hash) + new DateTimeOffset(time).ToUnixTimeSeconds()));
            for (var i = 0; i < time.Hour % 5; i++)
                hash = hmacsha256.ComputeHash(hash);
            return ComputeB64(hash);
        }

        private string ComputeB64(byte[] text)
        {
            return Convert.ToBase64String(text)
                          .Replace("=", "")
                          .Replace("+", "-")
                          .Replace("/", "_");
        }

        private byte[] GetByteChars(string text) => Encoding.ASCII.GetBytes(text);
    }
}