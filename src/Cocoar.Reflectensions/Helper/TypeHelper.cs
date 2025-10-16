using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cocoar.Reflectensions.Internal;

namespace Cocoar.Reflectensions.Helper
{
    public static class TypeHelper
    {
        
        /// <summary>
        /// Find a type from string
        /// </summary>
        /// <param name="typeName">textual representation of the type</param>
        /// <returns></returns>
        public static Type? FindType(string typeName)
        {
            return FindType(typeName, new Dictionary<string, string>());
        }


        /// <summary>
        /// Find a type by name
        /// </summary>
        /// <param name="typeName">textual representation of the type</param>
        /// <param name="customTypeMapping">custom mapping for types, like 'number' => 'double'</param>
        /// <returns></returns>
        public static Type? FindType(string typeName, IDictionary<string, string> customTypeMapping)
        {

            if (customTypeMapping.TryGetValue(typeName, out var m))
            {
                typeName = m;
            }

            lock (TypeHelperCache.TypeFromStringLock)
            {

                if (TypeHelperCache.TypeFromString.TryGetValue(typeName, out var foundType))
                {
                    return foundType;
                }
                
                var origTypeName = typeName;

                typeName = NormalizeTypeName(typeName, customTypeMapping);



                foundType = Type.GetType(typeName, false, true);


                if (foundType == null)
                {
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                    foreach (var assembly in assemblies)
                    {
                        foundType = Type.GetType($"{typeName}, {assembly.FullName}", false, false);
                        if (foundType != null)
                        {
                            break;
                        }

                    }

                    if (foundType == null)
                    {
                        foreach (var assembly in assemblies)
                        {
                            foundType = Type.GetType($"{typeName}, {assembly.FullName}", false, true);
                            if (foundType != null)
                            {
                                break;
                            }
                        }
                    }
                }

                TypeHelperCache.TypeFromString.Add(origTypeName, foundType);

                return foundType;
            }

        }


        /// <summary>
        /// Normalizes complex type names to runtime friendly type names
        /// </summary>
        /// <param name="typeName">textual representation of the type</param>
        /// <returns></returns>
        public static string NormalizeTypeName(string typeName)
        {
            return NormalizeTypeName(typeName, new Dictionary<string, string>());
        }


        /// <summary>
        /// Normalizes complex type names to runtime friendly type names
        /// </summary>
        /// <param name="typeName">textual representation of the type</param>
        /// <param name="customTypeMapping">custom mapping for types, like 'number' => 'double'</param>
        /// <returns>string</returns>
        public static string NormalizeTypeName(string typeName, IDictionary<string, string> customTypeMapping)
        {

            if (customTypeMapping.TryGetValue(typeName, out var m))
            {
                typeName = m;
            }

            var arrayDepth = 0;
            while (typeName.EndsWith("[]"))
            {
                arrayDepth++;
                typeName = typeName.Remove(typeName.Length - 2);
            }

            var isGeneric = typeName.Contains("`") || typeName.Contains("<");

            if (!isGeneric)
            {

                if (TypeLookupHelper.TypeKeywordTable.TryGetValue(typeName, out var tn))
                {
                    typeName = tn;
                }

                if (!typeName.Contains("."))
                {
                    typeName = $"System.{typeName}";
                }
            }
            else
            {

                var regex = new Regex(@"((?<modifier>[a-z]+)?\s)?((?<kind>[a-z]+)\s)?(?<fqn>[a-zA-Z0-9_\.]+)([`|$][\d+|\?])?([\[|<](?<genericArguments>.*)[\]|>])?");

                var match = regex.Match(typeName);

                if (!match.Success)
                    throw new Exception($"can't parse '{typeName}' to TypeDefinition");

                var fqn = match.Groups["fqn"].Value;

                var genericArguments = match.Groups["genericArguments"].Value.Trim().SplitGenericArguments();
                var gens = genericArguments.Select(ga => NormalizeTypeName(ga, customTypeMapping)).ToArray();

                typeName = $"{fqn}`{gens.Length}[{String.Join(", ", gens)}]";
            }

            typeName = $"{typeName}{"[]".Repeat(arrayDepth)}";

            return typeName;
        }


        public static bool HasInspectableBaseType(Type type)
        {
            var baseType = type.BaseType;
            if (baseType == null)
                return false;

            if (baseType == typeof(object))
                return false;

            if (baseType == typeof(ValueType))
                return false;

            return true;
        }
    }
}
