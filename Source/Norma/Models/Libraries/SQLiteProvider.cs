namespace Norma.Models.Libraries
{
    internal class SqLiteProvider : Library
    {
        #region Overrides of Library

        public override string Name => "System.Data.SQLite";
        public override string Url => "https://system.data.sqlite.org";

        public override string License => @"
Public Domain
";

        #endregion
    }
}