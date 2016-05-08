using System.IO;

using Newtonsoft.Json;

using Norma.Models.Config;

namespace Norma.Models
{
    internal class Configuration
    {
        private static Configuration _instance;
        public static Configuration Instance => _instance ?? (_instance = new Configuration());

        public ConfigRoot Root { get; private set; }

        private Configuration()
        {

        }

        public void Load()
        {
            if (!File.Exists(NormaConstants.ConfigurationFile))
            {
                Root = new ConfigRoot();
                return;
            }
            using (var sr = File.OpenText(NormaConstants.ConfigurationFile))
            {
                var serializer = new JsonSerializer();
                Root = (ConfigRoot) serializer.Deserialize(sr, typeof(ConfigRoot));
            }
        }

        public void Save()
        {
            using (var sw = File.CreateText(NormaConstants.ConfigurationFile))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(sw, Root);
            }
        }
    }
}