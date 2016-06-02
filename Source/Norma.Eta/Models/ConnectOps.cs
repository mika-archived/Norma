using System.IO;

using Newtonsoft.Json;

using Norma.Eta.Models.Operations;

namespace Norma.Eta.Models
{
    // ops.json を介して通信
    // セキュリティ的にどうなんじゃろ
    // TODO: Server(Norma.exe), Client(Norma.Ipsilon.exe) みたいな感じで実装したい。
    public class ConnectOps
    {
        public IOperation Operation { get; private set; }

        public void Load()
        {
            if (!File.Exists(NormaConstants.OpsFile))
                return;
            using (var sr = File.OpenText(NormaConstants.OpsFile))
                Operation = JsonConvert.DeserializeObject<IOperation>(sr.ReadToEnd());
        }

        public void Save(IOperation operation)
        {
            using (var sw = File.CreateText(NormaConstants.OpsFile))
                sw.WriteLine(JsonConvert.SerializeObject(operation));
        }
    }
}