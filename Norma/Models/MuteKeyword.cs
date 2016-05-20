namespace Norma.Models
{
    internal class MuteKeyword
    {
        public bool IsRegex { get; set; }

        public string Keyword { get; set; }

        //
        public MuteKeyword() {}

        public MuteKeyword(string keyword, bool isRegex)
        {
            Keyword = keyword;
            IsRegex = isRegex;
        }
    }
}