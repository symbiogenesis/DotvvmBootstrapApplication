using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace DotvvmBootstrapApplication.Utils.Extensions
{
    public static class DbContextExtensions
    {
        private static readonly Dictionary<Type, PropertyInfo> _tables = new Dictionary<Type, PropertyInfo>();

        public static DbSet<T> GetTable<T>(this DbContext context) where T : class
        {
            if(!_tables.ContainsKey(typeof(T)))
            {
                var property = context
                   .GetType()
                   .GetProperties()
                   .First(p => p.PropertyType == typeof(DbSet<T>));

                _tables.Add(typeof(T), property);
            }

            return (DbSet<T>) _tables[typeof(T)].GetValue(context);
        }

        public static string SplitCsvAndGetFirst(this string csvList)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return null;

            return csvList
                .TrimEnd(',')
                .Split(',')
                .First()
                .Trim();
        }
    }
}