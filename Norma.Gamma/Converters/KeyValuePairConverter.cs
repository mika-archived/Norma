using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Norma.Gamma.Converters
{
    internal class KeyValuePairConverter : JsonConverter
    {
        #region Overrides of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var items = (IEnumerable<KeyValuePair<string, string>>) value;
            writer.WriteStartObject();
            foreach (var item in items)
            {
                writer.WritePropertyName(item.Key);
                writer.WriteValue(item.Value);
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            var hoge = typeof(IEnumerable<KeyValuePair<string, string>>).IsAssignableFrom(objectType);
            return hoge;
        }

        #endregion
    }
}