using System;
using System.Collections.Generic;

namespace Cocoar.Reflectensions.Internal
{
    internal static class TypeHelperCache
    {
        internal static Dictionary<string, Type?> TypeFromString { get; } = new();
        internal static object TypeFromStringLock { get; } = new();

        internal static List<TypeInheritanceLevel> InheritanceList { get; } = new();
        internal static object InheritanceListLock { get; } = new();
    }
}
