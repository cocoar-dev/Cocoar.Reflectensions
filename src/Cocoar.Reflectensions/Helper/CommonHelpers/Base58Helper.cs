using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Cocoar.Reflectensions.Helper {

	public static class Base58Helper
	{
		public const int CheckSumSizeInBytes = 4;

		public static byte[] AddCheckSum(byte[] data)
		{
            byte[] checkSum = GetCheckSum(data);
			byte[] dataWithCheckSum = ArrayHelpers.ConcatArrays(data, checkSum);
			return dataWithCheckSum;
		}

		public static byte[]? VerifyAndRemoveCheckSum(byte[] data)
		{
            byte[] result = ArrayHelpers.SubArray(data, 0, data.Length - CheckSumSizeInBytes);
			byte[] givenCheckSum = ArrayHelpers.SubArray(data, data.Length - CheckSumSizeInBytes);
			byte[] correctCheckSum = GetCheckSum(result);
			if (givenCheckSum.SequenceEqual(correctCheckSum))
				return result;
			else
				return null;
		}

		private const string Digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

		public static string Encode(byte[] data)
		{
			BigInteger intData = 0;
			for (int i = 0; i < data.Length; i++)
			{
				intData = intData * 256 + data[i];
			}

			string result = "";
			while (intData > 0)
			{
				int remainder = (int)(intData % 58);
				intData /= 58;
				result = Digits[remainder] + result;
			}

			for (int i = 0; i < data.Length && data[i] == 0; i++)
			{
				result = '1' + result;
			}
			return result;
		}

		public static string EncodeWithCheckSum(byte[] data)
		{
            return Encode(AddCheckSum(data));
		}

		public static byte[] Decode(string s)
		{
			BigInteger intData = 0;
			for (int i = 0; i < s.Length; i++)
			{
				int digit = Digits.IndexOf(s[i]);
				if (digit < 0)
					throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Invalid Base58 character `{0}` at position {1}", s[i], i));
				intData = intData * 58 + digit;
			}

			int leadingZeroCount = s.TakeWhile(c => c == '1').Count();
			var leadingZeros = Enumerable.Repeat((byte)0, leadingZeroCount);
			var bytesWithoutLeadingZeros =
				intData.ToByteArray()
				.AsEnumerable()
				.Reverse()
				.SkipWhile(b => b == 0);
			var result = leadingZeros.Concat(bytesWithoutLeadingZeros).ToArray();
			return result;
		}

		public static byte[] DecodeWithCheckSum(string s)
		{
            var dataWithCheckSum = Decode(s);
			var dataWithoutCheckSum = VerifyAndRemoveCheckSum(dataWithCheckSum);
			if (dataWithoutCheckSum == null)
				throw new FormatException("Base58 checksum is invalid");
			return dataWithoutCheckSum;
		}

		private static byte[] GetCheckSum(byte[] data)
		{
            using var sha256 = SHA256.Create();
			byte[] hash1 = sha256.ComputeHash(data);
			byte[] hash2 = sha256.ComputeHash(hash1);

			var result = new byte[CheckSumSizeInBytes];
			Buffer.BlockCopy(hash2, 0, result, 0, result.Length);

			return result;
		}
	}
}