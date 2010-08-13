using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Kockerbeck.MapReduce.Tests
{
    public class MemoryTests
    {
        [Theory,
        InlineData("The", 6),
        InlineData("Dragon", 1), 
        InlineData("Bear", 2), 
        InlineData("Spaceship", 3)]
        public void MapReduce_Finds_Basic_Keywords_In_Dictionary(string word, int count)
        {
            var characterCount = word.Length*count;
            
            var input = new Dictionary<string, string>
                                {
                                    {"Dragonslayer", "Dragons ate some people"},
                                    {"War of the Ancients", "The dragon liked to eat bears"},
                                    {"War of the Ancients Part 2", "The multitude of Dragon could only eat One Big Bear"},
                                    {"Nature's Splendor", "The bear was at peace with his life.  He was Bear."},
                                    {"Mothership", "The Spaceship was not a mother ship"},
                                    {"Lost Viking", "The little Spaceship tried to get home"},
                                    {"Abduction", "The big Spaceship got some rednecks"},
                                };

            var output = MapReduce.Execute(Map, Reduce, input);

            Assert.Contains(word, output.Keys);
            Assert.Equal(count, output[word][0]);
            Assert.Equal(characterCount, output[word][1]);
        }
        
        /// <summary>
        /// The main Map function
        /// </summary>
        /// <param name="document"></param>
        /// <param name="words"></param>
        public static List<KeyValuePair<string, int>> Map(string document, string words)
        {
            var items = words.Split('\n',' ','.');
            return items.Select(item => new KeyValuePair<string, int>(item, 1)).ToList();
        }

        /// <summary>
        /// The main reduce function
        /// </summary>
        /// <param name="word"></param>
        /// <param name="words"></param>
        public static List<int> Reduce(string word, List<int> words)
        {
            if (words == null) return null;

            var result = new List<int> {0, 0};

            foreach (var value in words)
            {
                result[0] += value; // number of words
                result[1] += word.Length; // number of characters 
            }

            return result;
        }
    }
}