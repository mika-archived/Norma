using System;

using Newtonsoft.Json;

namespace Norma.Gamma.Models
{
    [AppVersion("1.0.42")]
    public class Program : IEquatable<Program>
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("episode")]
        public Episode Episode { get; set; }

        [JsonProperty("credit")]
        public Credit Credit { get; set; }

        [JsonProperty("series")]
        public Series Series { get; set; }

        [JsonProperty("providedInfo")]
        public ProvidedInfo ProvidedInfo { get; set; }

        #region Implementation of IEquatable<Program>

        public bool Equals(Program other) => Id == other.Id;

        #endregion

        #region Overrides of Object

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode() => Id.GetHashCode();

        #endregion
    }
}