using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cocoar.Reflectensions.ExtensionMethods;

public static class ParameterInfoEnumerableExtensions {


    public static IEnumerable<ParameterInfo> WithName(this IEnumerable<ParameterInfo> parameterInfos, string name,
        bool ignoreCase) {

        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return parameterInfos.Where(p => p.Name?.Equals(name, stringComparison) == true);
    }

    public static IEnumerable<ParameterInfo> WithTypeOf(this IEnumerable<ParameterInfo> parameterInfos, Type type) {
        return parameterInfos.Where(p => p.ParameterType == type);
    }

    public static IEnumerable<ParameterInfo> WithoutTypeOf(this IEnumerable<ParameterInfo> parameterInfos, Type type) {
        return parameterInfos.Where(p => p.ParameterType != type);
    }

    public static IEnumerable<ParameterInfo> WithTypeOf<T>(this IEnumerable<ParameterInfo> parameterInfos) {
        return parameterInfos.WithTypeOf(typeof(T));
    }
    public static IEnumerable<ParameterInfo> WithoutTypeOf<T>(this IEnumerable<ParameterInfo> parameterInfos) {
        return parameterInfos.WithoutTypeOf(typeof(T));
    }

    public static IEnumerable<ParameterInfo> WithAttribute(this IEnumerable<ParameterInfo> parameterInfos, string attributeName) {
        return parameterInfos
            .Where(p => p.HasAttribute(attributeName));
    }

    public static IEnumerable<ParameterInfo> WithoutAttribute(this IEnumerable<ParameterInfo> parameterInfos, string attributeName) {
        return parameterInfos
            .Where(p => !p.HasAttribute(attributeName));
    }


        
}