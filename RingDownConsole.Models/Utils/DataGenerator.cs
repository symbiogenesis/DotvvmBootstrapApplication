using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using RingDownConsole.Interfaces;
using RingDownConsole.Models;

namespace RingDownConsole.Utils
{
    public static class DataGenerator
    {
        private static int _numItems;
        private static Random _rnd;

        public static IList<Status> Statuses { get; private set; }
        public static IList<Location> Locations { get; private set; }
        public static IList<LocationStatus> LocationStatuses { get; private set; }

        public static void Generate(int count = 10)
        {
            _numItems = count;
            _rnd = new Random((int) DateTime.Now.Ticks);

            Statuses = Generate<Status>().ToList();
            Locations = Generate<Location>().ToList();
            LocationStatuses = Generate<LocationStatus>().ToList();
        }

        private static IEnumerable<T> Generate<T>() where T : class, IIdentifiable, new()
        {
            var properties = typeof(T).GetProperties().Where(p => !p.CustomAttributes.Any(a => a.AttributeType != typeof(RequiredAttribute)));

            for (var i = 0; i < _numItems; i++)
            {
                var item = new T() { Id = i };

                foreach (var property in properties)
                {
                    switch (property.PropertyType.Name)
                    {
                        case "String":
                            if (property.Name.EndsWith("Num"))
                                property.SetValue(item, RandomString());
                            else
                                property.SetValue(item, LoremIpsum());
                            break;
                        case "DateTime":
                            property.SetValue(item, RandomDate());
                            break;
                        case "Boolean":
                            property.SetValue(item, _rnd.Next() % 2 == 0);
                            break;
                    }
                }

                yield return item;
            }
        }

        private static string RandomString(int size = 0)
        {
            if (size == 0)
                size = _rnd.Next(15);

            var builder = new StringBuilder();
            char ch;
            for (var i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * _rnd.NextDouble()) + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private static string LoremIpsum(int minWords = 3, int maxWords = 10, int minSentences = 1, int maxSentences = 2, int numParagraphs = 1)
        {
            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
        "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            var numSentences = rand.Next(maxSentences - minSentences) + minSentences + 1;
            var numWords = rand.Next(maxWords - minWords) + minWords + 1;

            var result = new StringBuilder();

            for (var p = 0; p < numParagraphs; p++)
            {
                for (var s = 0; s < numSentences; s++)
                {
                    for (var w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        result.Append(words[rand.Next(words.Length)]);
                    }
                    result.Append(". ");
                }
            }

            return result.ToString();
        }

        public static DateTime RandomDate()
        {
            var start = new DateTime(2017, 6, 1);
            var range = (DateTime.Today - start).Days;
            var output = start.AddDays(_rnd.Next(range)).AddMinutes(_rnd.Next(range));

            if (output >= DateTime.UtcNow)
                return RandomDate();

            return output;
        }

        public static T GetRandomItem<T>(this IEnumerable<T> items) where T : IIdentifiable
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var newIndex = _rnd.Next(0, items.Count() - 1);

            return items.ToArray()[newIndex];
        }

        public static IEnumerable<T> GetRandomItems<T>(this IEnumerable<T> items, int count) where T : IIdentifiable
        {
            if (items == null)
                throw new ArgumentNullException();

            var output = new HashSet<T>();

            for (var i = 0; i < count; i++)
            {
                var newIndex = _rnd.Next(0, items.Count() - 1);
                var newItem = items.ToArray()[newIndex];
                output.Add(newItem);
            }

            return output;
        }

        public static IEnumerable<T> GetRandomItems<T>(this IEnumerable<T> items, int max, bool variableAmount) where T : IIdentifiable
        {
            var num = variableAmount ? _rnd.Next(1, max) : max;

            return GetRandomItems(items, num);
        }
    }
}
