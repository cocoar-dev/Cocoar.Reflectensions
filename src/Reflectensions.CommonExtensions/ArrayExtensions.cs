using System;

namespace Cocoar.Reflectensions.Common
{
    public static class ArrayExtensions
    {
        public static T[] Concat<T>(this T[] arr1, T[] arr2)
        {
            var result = new T[arr1.Length + arr2.Length];
            Array.Copy(arr1, 0, result, 0, arr1.Length);
            Array.Copy(arr2, 0, result, arr1.Length, arr2.Length);
            return result;
        }

        public static T[] SubArray<T>(this T[] arr, int start, int length)
        {
            var result = new T[length];
            Array.Copy(arr, start, result, 0, length);
            return result;
        }

        public static T[] SubArray<T>(this T[] arr, int start)
        {
            return SubArray(arr, start, arr.Length - start);
        }

    }
}
