using Cocoar.Reflectensions.Helper;
using Xunit;

namespace Cocoar.Reflectensions.Tests.HelperTests
{
    public class ArrayHelpersTests
    {
        [Fact]
        public void ConcatArrays_TwoArrays_CombinesCorrectly()
        {
            var arr1 = new[] { 1, 2, 3 };
            var arr2 = new[] { 4, 5, 6 };

            var result = ArrayHelpers.ConcatArrays(arr1, arr2);

            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6 }, result);
        }

        [Fact]
        public void ConcatArrays_MultipleArrays_CombinesAll()
        {
            var arr1 = new[] { 1, 2 };
            var arr2 = new[] { 3, 4 };
            var arr3 = new[] { 5, 6 };

            var result = ArrayHelpers.ConcatArrays(arr1, arr2, arr3);

            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6 }, result);
        }

        [Fact]
        public void ConcatArrays_EmptyArrays_ReturnsEmpty()
        {
            var arr1 = new int[] { };
            var arr2 = new int[] { };

            var result = ArrayHelpers.ConcatArrays(arr1, arr2);

            Assert.Empty(result);
        }

        [Fact]
        public void ConcatArrays_OneEmptyArray_ReturnsOther()
        {
            var arr1 = new int[] { };
            var arr2 = new[] { 1, 2, 3 };

            var result = ArrayHelpers.ConcatArrays(arr1, arr2);

            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void ConcatArrays_ByteArrays_WorksCorrectly()
        {
            var arr1 = new byte[] { 1, 2, 3 };
            var arr2 = new byte[] { 4, 5, 6 };

            var result = ArrayHelpers.ConcatArrays(arr1, arr2);

            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6 }, result);
        }

        [Fact]
        public void SubArray_WithStartAndLength_ReturnsCorrectSegment()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };

            var result = ArrayHelpers.SubArray(arr, 1, 3);

            Assert.Equal(new[] { 2, 3, 4 }, result);
        }

        [Fact]
        public void SubArray_WithStartOnly_ReturnsRemainingElements()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };

            var result = ArrayHelpers.SubArray(arr, 2);

            Assert.Equal(new[] { 3, 4, 5 }, result);
        }

        [Fact]
        public void SubArray_StartAtZero_ReturnsCorrectSegment()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };

            var result = ArrayHelpers.SubArray(arr, 0, 3);

            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void SubArray_StartAtBeginning_ReturnsAll()
        {
            var arr = new[] { 1, 2, 3 };

            var result = ArrayHelpers.SubArray(arr, 0);

            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void SubArray_ByteArray_WorksCorrectly()
        {
            var arr = new byte[] { 1, 2, 3, 4, 5 };

            var result = ArrayHelpers.SubArray(arr, 1, 3);

            Assert.Equal(new byte[] { 2, 3, 4 }, result);
        }

        [Fact]
        public void ConcatArrays_SingleArray_ReturnsOriginal()
        {
            var arr = new[] { 1, 2, 3 };

            var result = ArrayHelpers.ConcatArrays(arr);

            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void ConcatArrays_FourArrays_CombinesAll()
        {
            var arr1 = new[] { 1 };
            var arr2 = new[] { 2 };
            var arr3 = new[] { 3 };
            var arr4 = new[] { 4 };

            var result = ArrayHelpers.ConcatArrays(arr1, arr2, arr3, arr4);

            Assert.Equal(new[] { 1, 2, 3, 4 }, result);
        }
    }
}
