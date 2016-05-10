using System;
using System.Reflection;

namespace Norma.Models
{
    internal static class ProductInfo
    {
        public static string Name => GetAssemblyInfo<AssemblyTitleAttribute>().Title;

        public static string Description => GetAssemblyInfo<AssemblyDescriptionAttribute>().Description;

        public static string Company => GetAssemblyInfo<AssemblyCompanyAttribute>().Company;

        public static string Copyright => GetAssemblyInfo<AssemblyCopyrightAttribute>().Copyright;

        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string Support => "https://github.com/fuyuno/Norma";

        private static T GetAssemblyInfo<T>() where T : Attribute
            => (T) Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
    }
}