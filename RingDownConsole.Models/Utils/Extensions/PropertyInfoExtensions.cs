using System.Collections.Generic;
using System.Reflection;

namespace RingDownConsole.Utils.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string GetString(this PropertyInfo property, object obj)
        {
            return property.GetValue(obj)?.ToString() ?? string.Empty;
        }

        public static IEnumerable<string> GetList(this PropertyInfo property, object obj)
        {
            return (IEnumerable<string>) property.GetValue(obj) ?? new List<string>();
        }
    }
}
