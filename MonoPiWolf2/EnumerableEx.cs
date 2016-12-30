using System;
using System.Collections.Generic;

namespace MonoPiWolf2
{
    public static class EnumerableEx
    {
        public static void EagerEach<T>(this IEnumerable<T> enumerable, Action<int, T> action)
        {
            var index = 0;
            foreach (var item in enumerable)
            {
                action(index, item);
                index++;
            }
        }
        public static void EagerEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}