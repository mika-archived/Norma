using System;
using System.Collections.Generic;
using System.Reflection;

using Norma.Models.Libraries;

using LibMetroRadiance = Norma.Models.Libraries.MetroRadiance;

namespace Norma.Models
{
    internal static class ProductInfo
    {
        public static string Name => GetAssemblyInfo<AssemblyTitleAttribute>().Title;

        public static Lazy<string> Description
            => new Lazy<string>(() => GetAssemblyInfo<AssemblyDescriptionAttribute>().Description);

        public static Lazy<string> Company
            => new Lazy<string>(() => GetAssemblyInfo<AssemblyCompanyAttribute>().Company);

        public static string Copyright => GetAssemblyInfo<AssemblyCopyrightAttribute>().Copyright;

        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static ReleaseType ReleaseType => ReleaseType.Release;

        public static string Support => "https://github.com/fuyuno/Norma";

        public static Lazy<List<Library>> Libraries => new Lazy<List<Library>>(() => new List<Library>
        {
            new CEF(),
            new CEFSharp(),
            new CommonServiceLocator(),
            new DesktopToast(),
            new EntityFramework(),
            new HardcodetWpfNotifyIcon(),
            new LibMetroRadiance(),
            new NewtonsoftJson(),
            new NotificationsExtensions(),
            new PrismLibrary(),
            new ReactiveProperty(),
            new RxNET(),
            new SqLiteCodeFirst(),
            new SqLiteProvider(),
            new Unity()
        });

        private static T GetAssemblyInfo<T>() where T : Attribute
            => (T) Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
    }
}