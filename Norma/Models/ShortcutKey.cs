using Norma.Eta.Models;

namespace Norma.Models
{
#pragma warning disable CS0659 // 型は Object.Equals(object o) をオーバーライドしますが、Object.GetHashCode() をオーバーライドしません

    internal class ShortcutKey
#pragma warning restore CS0659 // 型は Object.Equals(object o) をオーバーライドしますが、Object.GetHashCode() をオーバーライドしません
    {
        public string Display { get; }

        public PostKey PostKey { get; }

        public ShortcutKey(PostKey key)
        {
            Display = key.ToLocaleString();
            PostKey = key;
        }

        #region Overrides of Object

#pragma warning disable 659

        public override bool Equals(object obj)
#pragma warning restore 659
        {
            var shortcutKey = obj as ShortcutKey;
            return PostKey == shortcutKey?.PostKey;
        }

        #endregion
    }
}