using System;

using Newtonsoft.Json;

namespace Norma.Gamma.Converters
{
    internal class UnixTimeDateTimeConverter : JsonConverter
    {
        #region Overrides of JsonConverter

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            var ms = (long) reader.Value;
            return ms.ToString().Length <= 10
                ? new DateTime(1970, 1, 1).AddHours(9).AddSeconds(ms)
                : new DateTime(1970, 1, 1).AddHours(9).AddMilliseconds(ms);
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}