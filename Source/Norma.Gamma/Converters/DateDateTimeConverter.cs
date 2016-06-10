using System;
using System.Linq;

using Newtonsoft.Json;

namespace Norma.Gamma.Converters
{
    internal class DateDateTimeConverter : JsonConverter
    {
        private DateTime ParseToDateTime(string text)
        {
            return new DateTime(int.Parse(text.Substring(0, 4)), int.Parse(text.Substring(4, 2)),
                                int.Parse(text.Substring(6, 2)));
        }

        #region Overrides of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var time = value as DateTime?;
            if (!time.HasValue)
                return;
            writer.WriteValue($"{time.Value.Year}{time.Value.Month.ToString("D2")}{time.Value.Day.ToString("D2")}");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
                return ParseToDateTime((string) reader.Value);
            var obj = (string[]) serializer.Deserialize(reader, typeof(string[]));
            return obj.Select(ParseToDateTime).ToArray();
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}