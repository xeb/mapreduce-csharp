using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Extensions;

namespace Kockerbeck.MapReduce.Tests
{
    public class FileCharacterTests
    {
        readonly Dictionary<char,List<int>> _output;

        public FileCharacterTests()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var di = new DirectoryInfo(String.Format(@"{0}\App_Data\", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            // ReSharper restore AssignNullToNotNullAttribute

            var fileSearchData = new Dictionary<FileInfo, string>();
            di.GetFiles().ToList().ForEach(f => fileSearchData.Add(f, File.ReadAllText(f.FullName)));

            _output = MapReduce.Execute(Map, Reduce, fileSearchData);
        }

        [Fact]
        public void MapReduce_Finds_All_Characters()
        {
            Assert.Equal(68, _output.Keys.Count);
        }

        [Fact]
        public void MapReduce_Finds_Most_Common_Character()
        {
            var ignoreCharacters = new[] {' '};
            var mostCommonMatch = _output
                .Where(kv => !ignoreCharacters.Contains(kv.Key))
                .OrderByDescending(kv => kv.Value.Sum())
                .FirstOrDefault();

            Assert.Equal('i', mostCommonMatch.Key);
            Assert.Equal(2941, mostCommonMatch.Value.Sum());
        }

        [Theory,
        InlineData('L', 14),
        InlineData('e', 2863),
        InlineData('V', 27),]
        public void MapReduce_Finds_Keywords_In_Flat_Files(char character, int times)
        {
            Assert.NotEmpty(_output);
            Assert.Contains(character, _output.Keys);
            Assert.Equal(times, _output[character][0]);
        }

        /// <summary>
        /// The main Map function
        /// </summary>
        /// <param name="document"></param>
        /// <param name="text"></param>
        public static List<KeyValuePair<char, int>> Map(FileInfo document, string text)
        {
            return text.Select(item => new KeyValuePair<char, int>(item, 1)).ToList();
        }

        /// <summary>
        /// The main reduce function
        /// </summary>
        /// <param name="character"></param>
        /// <param name="characterCounts"></param>
        public static List<int> Reduce(char character, List<int> characterCounts)
        {
            if (characterCounts == null) return new List<int>();

            var result = new List<int> { 0 };

            foreach (var value in characterCounts)
            {
                result[0] += value;
            }

            return result;
        }
    }
}
