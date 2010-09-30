using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kockerbeck.MapReduce
{
    /// <summary>
    /// Very Simple MapReduce implementation in C#
    /// </summary>
    /// <remarks>
    /// Thanks to Stephan Brenner.  Refactored for C# 4.0 
    /// </remarks>
    public class MapReduce
    {
        public static int NumberOfCores = 4;

        public static Dictionary<T3, List<T4>> Execute<T1, T2, T3, T4>(Func<T1, T2, List<KeyValuePair<T3, T4>>> mapFunction,
            Func<T3, List<T4>, List<T4>> reduceFunction, Dictionary<T1, T2> input)
        {
            var result = new Dictionary<T3, List<T4>>();
            var maps = new Dictionary<T3, List<T4>>();

            input.DivvyUp(NumberOfCores, l => l.ForEach(kv => 
                maps.Add(mapFunction(kv.Key, kv.Value), i => i.Key, i => i.Value)));

            maps.DivvyUp(NumberOfCores, m => m.ForEach(map => 
                result.Add(reduceFunction(map.Key, map.Value), i => map.Key, i => i)));

            return result;
        }
    }
}