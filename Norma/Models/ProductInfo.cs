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

        public static string Description => GetAssemblyInfo<AssemblyDescriptionAttribute>().Description;

        public static string Company => GetAssemblyInfo<AssemblyCompanyAttribute>().Company;

        public static string Copyright => GetAssemblyInfo<AssemblyCopyrightAttribute>().Copyright;

        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static ReleaseType ReleaseType => ReleaseType.Preview;

        public static string Support => "https://github.com/fuyuno/Norma";

        public static List<Library> Libraries => new List<Library>
        {
            new CEF(),
            new CEFSharp(),
            new CommonServiceLocator(),
            new LibMetroRadiance(),
            new NewtonsoftJson(),
            new PrismLibrary(),
            new ReactiveProperty(),
            new RxNET(),
            new Unity()
        };

        private static T GetAssemblyInfo<T>() where T : Attribute
            => (T) Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
    }
}