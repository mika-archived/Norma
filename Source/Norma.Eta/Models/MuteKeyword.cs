using Newtonsoft.Json;

namespace Norma.Eta.Models
{
    public class MuteKeyword
    {
        public bool IsRegex { get; set; }

        public string Keyword { get; set; }

        [JsonIgnore]
        public string DisplayKeyword => Keyword.Replace("\n", "\\n");

        //
        public MuteKeyword() {}

        public MuteKeyword(string keyword, bool isRegex)
        {
            Keyword = keyword;
            IsRegex = isRegex;
        }

        #region Overrides of Object

#pragma warning disable 659

        public override bool Equals(object obj)
#pragma warning restore 659
        {
            var item = obj as MuteKeyword;
            return IsRegex == item?.IsRegex && Keyword == item.Keyword;
        }

        #endregion
    }
}