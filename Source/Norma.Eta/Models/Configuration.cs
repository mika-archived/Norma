using System.IO;

using Newtonsoft.Json;

using Norma.Eta.Extensions;
using Norma.Eta.Models.Configurations;
using Norma.Eta.Models.Enums;

namespace Norma.Eta.Models
{
    public class Configuration
    {
        public RootConfig Root { get; private set; }

        public Configuration()
        {
            Load();
            Save();
        }

        private void Load()
        {
            if (!File.Exists(NormaConstants.ConfigurationFile))
            {
                Root = new RootConfig();
                Migrate();
                return;
            }
            using (var sr = File.OpenText(NormaConstants.ConfigurationFile))
            {
                var serializer = new JsonSerializer();
                Root = (RootConfig) serializer.Deserialize(sr, typeof(RootConfig));
            }
            Migrate();
        }

        public void Save()
        {
            using (var sw = File.CreateText(NormaConstants.ConfigurationFile))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(sw, Root);
            }
        }

        private void Migrate()
        {
            if (Root.Browser == null)
                Root.Browser = new BrowserConfig();
            if (Root.Operation == null)
                Root.Operation = new OperationConfig();
            if (Root.Others == null)
                Root.Others = new OthersConfig();
            if (Root.Internal == null)
                Root.Internal = new InternalConfig();

            if (string.IsNullOrWhiteSpace(Root.LastViewedChannelStr))
                Root.LastViewedChannelStr = Root.LastViewedChannel.ToUrlString();

            // Version 1.5
            if (Root.Operation.VideoQuality != VideoQuality.Auto)
                Root.Others.IsEnabledExperimentalFeatures = true;

            ApplyDefaultNgWords();
        }

        // 規定の NG ワード
        // https://github.com/nakayuki805/AbemaTVChromeExtension のものに、いくつか追加
        private void ApplyDefaultNgWords()
        {
            var words = Root.Operation.MuteKeywords;
            words.AddIfNotExists(new MuteKeyword("^@.*", true)); // @Mention
            words.AddIfNotExists(new MuteKeyword("^#.*", true)); // #Hashtag
            words.AddIfNotExists(new MuteKeyword("^http(s)?://", true)); // http://url.com
            words.AddIfNotExists(new MuteKeyword("(.+)\\1{2,}", true)); // repeeet words
            words.AddIfNotExists(new MuteKeyword("^.$", true)); // 1
            words.AddIfNotExists(new MuteKeyword("\n", true)); // New line
            // by Norma
            words.AddIfNotExists(new MuteKeyword("^.*は.*\\s.*は.*$", true));
            words.AddIfNotExists(new MuteKeyword("k", false));
        }
    }
}