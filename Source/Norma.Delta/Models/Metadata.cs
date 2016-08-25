namespace Norma.Delta.Models
{
    // Wordpress の設定がこんな感じで保存してた
    public class Metadata
    {
        public static string LastSyncTimeKey => "last_sync_time";

        public int MetadataId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}