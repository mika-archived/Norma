namespace Norma.Gamma
{
    public static class EndPoints
    {
        private const string Base = "https://api.abema.io";

        private const string Version = "v1";

        private static readonly string EndPoint = $"{Base}/{Version}/";

        public static readonly string Users = EndPoint + "users";

        public static readonly string UsersShow = EndPoint + "users/{0}";

        public static readonly string Token = EndPoint + "media/token";

        public static readonly string Media = EndPoint + "media";

        public static readonly string Mime = EndPoint + "tracks/mime";

        public static readonly string SlotAudicence = EndPoint + "slotAudience";

        public static readonly string Comments = EndPoint + "slots/{0}/comments";
    }
}