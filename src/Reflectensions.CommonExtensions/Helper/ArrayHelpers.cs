using System;
using System.Linq;

namespace Cocoar.Reflectensions.Common.Helper
{
	public class ArrayHelpers
	{
		public static T[] ConcatArrays<T>(params T[][] arrays)
		{
            var result = new T[arrays.Sum(arr => arr.Length)];
			int offset = 0;
			for (int i = 0; i < arrays.Length; i++)
			{
				var arr = arrays[i];
				Array.Copy(arr, 0, result, offset, arr.Length);
				offset += arr.Length;
			}
			return result;
		}

		public static T[] ConcatArrays<T>(T[] arr1, T[] arr2)
		{
            var result = new T[arr1.Length + arr2.Length];
			Array.Copy(arr1, 0, result, 0, arr1.Length);
			Array.Copy(arr2, 0, result, arr1.Length, arr2.Length);
			return result;
		}

		public static T[] SubArray<T>(T[] arr, int start, int length)
		{
            var result = new T[length];
			Array.Copy(arr, start, result, 0, length);
			return result;
		}

		public static T[] SubArray<T>(T[] arr, int start)
		{
            return SubArray(arr, start, arr.Length - start);
		}
	}
}