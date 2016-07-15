using System.IO;

using Newtonsoft.Json;

using Norma.Eta.Models.Configurations;

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

            // for 1.4.x
            Root.LastViewedChannelStr = Root.LastViewedChannel.ToUrlString();
        }
    }
}