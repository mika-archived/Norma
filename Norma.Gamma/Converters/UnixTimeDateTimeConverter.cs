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
            return new DateTime(1970, 1, 1).AddSeconds(ms).AddHours(9);
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}