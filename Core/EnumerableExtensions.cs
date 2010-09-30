using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public static void Add<T, TK, TV>(this IDictionary<TK, List<TV>> dictionary, IEnumerable<T> list, Func<T, TK> keySelector, Func<T, TV> valueSelector)
        {
            lock (dictionary)
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
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// Divides an enumerable into equal parts and performs an action on those parts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="parts"></param>
        /// <param name="action"></param>
        public static void DivvyUp<T>(this IEnumerable<T> enumerable, int parts, Action<IEnumerable<T>, int, int> action)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var actions = new List<Action>();

            if (parts == 0)
                parts = 1;

            int count = enumerable.Count();
            int itemsPerPart = count / parts;

            if (itemsPerPart == 0)
                itemsPerPart = 1;

            for (int i = 0; i < parts; i++)
            {
                var collection = enumerable
                    .Skip(i * itemsPerPart)
                    .Take(i == parts - 1 ? count : itemsPerPart);

                int j = i; // access to modified closure safety
                actions.Add(() => action(collection, j, itemsPerPart));
            }

            Parallel.Invoke(actions.ToArray());
        }

        /// <summary>
        /// Divides an enumerable into equal parts and performs an action on those parts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="parts"></param>
        /// <param name="action"></param>
        public static void DivvyUp<T>(this IEnumerable<T> enumerable, int parts, Action<IEnumerable<T>> action)
        {
            DivvyUp(enumerable, parts, (subset, i, j) => action(subset));
        }

        #endregion
    }
}
