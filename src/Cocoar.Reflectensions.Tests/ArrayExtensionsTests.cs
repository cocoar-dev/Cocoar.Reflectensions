using Xunit;

namespace Cocoar.Reflectensions.Tests
{
    public class ArrayExtensionsTests
    {
        [Fact]
        public void Concat_TwoArrays_ReturnsCombinedArray()
        {
            var arr1 = new[] { 1, 2, 3 };
            var arr2 = new[] { 4, 5, 6 };

            var result = arr1.Concat(arr2);

            Assert.Equal(6, result.Length);
            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6 }, result);
        }

        [Fact]
        public void Concat_EmptyArrays_ReturnsEmptyArray()
        {
            var arr1 = new int[] { };
            var arr2 = new int[] { };

            var result = arr1.Concat(arr2);

            Assert.Empty(result);
        }

        [Fact]
        public void Concat_FirstArrayEmpty_ReturnsSecondArray()
        {
            var arr1 = new int[] { };
            var arr2 = new[] { 1, 2, 3 };

            var result = arr1.Concat(arr2);

            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void Concat_SecondArrayEmpty_ReturnsFirstArray()
        {
            var arr1 = new[] { 1, 2, 3 };
            var arr2 = new int[] { };

            var result = arr1.Concat(arr2);

            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void SubArray_WithStartAndLength_ReturnsCorrectSubArray()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };

            var result = arr.SubArray(1, 3);

            Assert.Equal(new[] { 2, 3, 4 }, result);
        }

        [Fact]
        public void SubArray_WithStartOnly_ReturnsRemainingElements()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };

            var result = arr.SubArray(2);

            Assert.Equal(new[] { 3, 4, 5 }, result);
        }

        [Fact]
        public void SubArray_StartAtZero_ReturnsCorrectSubArray()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };

            var result = arr.SubArray(0, 3);

            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void SubArray_StartAtBeginning_ReturnsEntireArray()
        {
            var arr = new[] { 1, 2, 3 };

            var result = arr.SubArray(0);

            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void SubArray_WithStringArray_WorksCorrectly()
        {
            var arr = new[] { "a", "b", "c", "d" };

            var result = arr.SubArray(1, 2);

            Assert.Equal(new[] { "b", "c" }, result);
        }
    }
}
