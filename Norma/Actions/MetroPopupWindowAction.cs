using System.Windows;
using System.Windows.Media;

using MetroRadiance.Chrome;
using MetroRadiance.UI.Controls;

using Prism.Interactivity;

namespace Norma.Actions
{
    internal class MetroPopupWindowAction : PopupWindowAction
    {
        protected override Window CreateWindow()
        {
            var window = new MetroWindow
            {
                Style = new Style(),
                FontFamily = new FontFamily("Segoe UI"),
                UseLayoutRounding = true
            };
            TextOptions.SetTextFormattingMode(window, TextFormattingMode.Display);
            WindowChrome.SetInstance(window, new WindowChrome());
            return window;
        }
    }
}