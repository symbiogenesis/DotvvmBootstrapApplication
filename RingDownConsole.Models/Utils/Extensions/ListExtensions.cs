using System.Collections.Generic;

namespace RingDownConsole.Utils.Extensions
{
    public static class ListExtensions
    {
        public static void MoveItemAtIndexToFront<T>(this IList<T> list, int index)
        {
            T item = list[index];
            list.RemoveAt(index);
            list.Insert(0, item);
        }
    }
}
