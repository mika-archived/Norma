using System;
using System.Windows.Input;

using Norma.Eta.Properties;

namespace Norma.Eta.Models.Enums
{
    public enum PostKey
    {
        /// <summary>
        ///     Post: Enter / New Line: Shift + Enter
        /// </summary>
        EnterOnly,

        /// <summary>
        ///     Post: Ctrl + Enter / New Line: Enter
        /// </summary>
        CtrlEnter,

        /// <summary>
        ///     Post: Shift + Enter / New Line: Enter
        /// </summary>
        ShiftEnter
    }

    public static class PostKeyExt
    {
        public static bool IsMatchShortcut(this PostKey keyType, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return false;
            switch (keyType)
            {
                case PostKey.EnterOnly:
                    return true;

                case PostKey.CtrlEnter:
                    return Keyboard.Modifiers.HasFlag(ModifierKeys.Control);

                case PostKey.ShiftEnter:
                    return Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);

                default:
                    throw new ArgumentOutOfRangeException(nameof(keyType), keyType, null);
            }
        }

        public static string ToLocaleString(this PostKey obj)
        {
            return (string) typeof(Resources).GetProperty(obj.ToString()).GetValue(null);
        }
    }
}