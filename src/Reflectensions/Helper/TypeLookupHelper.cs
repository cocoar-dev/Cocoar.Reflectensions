using System;
using System.Collections.Generic;
using System.Linq;

namespace Cocoar.Reflectensions.Helper
{
    public static class TypeLookupHelper
    {
        public static IReadOnlyDictionary<string, string> TypeKeywordTable { get; } = new Dictionary<string, string>
        {
            // Value Types
            ["bool"] = "System.Boolean",
            ["byte"] = "System.Byte",
            ["sbyte"] = "System.SByte",
            ["char"] = "System.Char",
            ["decimal"] = "System.Decimal",
            ["double"] = "System.Double",
            ["float"] = "System.Single",
            ["int"] = "System.Int32",
            ["uint"] = "System.UInt32",
            ["long"] = "System.Int64",
            ["ulong"] = "System.UInt64",
            ["short"] = "System.Int16",
            ["ushort"] = "System.UInt16",

            // Reference Types
            ["object"] = "System.Object",
            ["string"] = "System.String",
            ["dynamic"] = "System.Object"
        };

        public static IReadOnlyDictionary<Type, List<Type>> ImplicitNumericConversionsTable { get; } = new Dictionary<Type, List<Type>> {
            { typeof(sbyte),   new List<Type> { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(byte),    new List<Type> { typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(short),   new List<Type> { typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(ushort),  new List<Type> { typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(int),     new List<Type> { typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(uint),    new List<Type> { typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(long),    new List<Type> { typeof(float), typeof(double), typeof(decimal) } },
            { typeof(char),    new List<Type> { typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(float),   new List<Type> { typeof(double) } },
            { typeof(ulong),   new List<Type> { typeof(float), typeof(double), typeof(decimal) } },
            { typeof(double),  new List<Type>() },
            { typeof(decimal), new List<Type>() }
        };

        public static IReadOnlyList<Type> NumericTypes => ImplicitNumericConversionsTable.Keys.ToList();


    }
}
