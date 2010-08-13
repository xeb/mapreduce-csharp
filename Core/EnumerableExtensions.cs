using System;
using System.Linq;
using System.Collections.Generic;

namespace Kockerbeck.MapReduce
{
    public static class EnumerableExtensions
    {
        #region -- IDictionary --

        /// <summary>
        /// Adds an Enumerable to a Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="list"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        public static void Add<T, TK, TV>(this Dictionary<TK, List<TV>> dictionary, IEnumerable<T> list, Func<T, TK> keySelector, Func<T, TV> valueSelector)
        {
            foreach (var item in list)
            {
                var key = keySelector(item);

                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = new List<TV>();
                }

                dictionary[key].Add(valueSelector(item));
            }
        }

        #endregion
        
        #region -- IEnumberable --

        /// <summary>
        /// Iterates over an Enumerable
        /// </summary>
        /// <remarks>
        /// why wasn't this part of LINQ?
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach(var item in enumerable)
            {
                action(item);
            }
        }

        #endregion
    }
}
