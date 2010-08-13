using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Extensions;

namespace Kockerbeck.MapReduce.Tests
{
    public class FileWordTests
    {
        [Theory,
        InlineData("Lorem", 3),
        InlineData("et", 77),
        InlineData("Cicero", 1),]
        public void MapReduce_Finds_Keywords_In_Flat_Files(string word, int times)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var di = new DirectoryInfo(String.Format(@"{0}\App_Data\", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            // ReSharper restore AssignNullToNotNullAttribute

            var fileSearchData = new Dictionary<FileInfo, string>();
            di.GetFiles().ToList().ForEach(f => fileSearchData.Add(f, File.ReadAllText(f.FullName)));

            var output = MapReduce.Execute(Map, Reduce, fileSearchData);

            Assert.NotEmpty(output);
            Assert.Contains(word, output.Keys);
            Assert.Equal(times, output[word][0]);
        }
        
        /// <summary>
        /// The main Map function
        /// </summary>
        /// <param name="document"></param>
        /// <param name="text"></param>
        public static List<KeyValuePair<string, int>> Map(FileInfo document, string text)
        {
            var items = text.Split('\n', ' ', '.', ',','\r');
            return items.Select(item => new KeyValuePair<string, int>(item, 1)).ToList();
        }

        /// <summary>
        /// The main reduce function
        /// </summary>
        /// <param name="word"></param>
        /// <param name="wordCounts"></param>
        public static List<int> Reduce(string word, List<int> wordCounts)
        {
            if (wordCounts == null) return null;

            var result = new List<int> { 0 };
            
            foreach (var value in wordCounts)
            {
                result[0] += value;
            }

            return result;
        }
    }
}
