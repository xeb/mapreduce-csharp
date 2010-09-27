using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Kockerbeck.MapReduce.Tests
{
    public class EnumerableExtensionTests
    {
        [Fact]
        public void DivvyUp_Splits_List_Of_Two()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            list.DivvyUp(2, subset =>
            {
                Assert.NotEmpty(subset);
                Assert.Equal(4, subset.Count());
            });
        }
        
        [Fact]
        public void DivvyUp_Splits_List_Of_Three()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            list.DivvyUp(3, subset =>
            {
                Assert.NotEmpty(subset);
                Assert.Equal(3, subset.Count());
            });
        }

        [Fact]
        public void DivvyUp_Splits_Uneven_Lists()
        {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            list.DivvyUp(2, (subset, i, j) =>
            {
                Assert.NotEmpty(subset);

                if (i == 0)
                    Assert.Equal(4, subset.Count());
                if (i == 1)
                    Assert.Equal(5, subset.Count());
            });
        }

        [Fact]
        public void DivvyUp_Waits_For_All_Actions_To_Complete()
        {
            int i = 0;
            var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            list.DivvyUp(9, j => i++);
            Assert.Equal(9, i);
        }
    }
}
