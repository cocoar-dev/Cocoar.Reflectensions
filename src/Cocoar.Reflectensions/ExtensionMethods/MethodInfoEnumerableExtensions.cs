using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cocoar.Reflectensions.ExtensionMethods
{
    public static class MethodInfoEnumerableExtensions
    {

        public static IEnumerable<MethodInfo> WithName(this IEnumerable<MethodInfo> methodInfos, string name, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            return methodInfos.Where(m => m.HasName(name, stringComparison));
        }

        public static IEnumerable<MethodInfo> WithReturnType<T>(this IEnumerable<MethodInfo> methodInfos)
        {
            return WithReturnType(methodInfos, typeof(T));
        }

        public static IEnumerable<MethodInfo> WithReturnType(this IEnumerable<MethodInfo> methodInfos, Type returnType)
        {
            return methodInfos.Where(m => m.HasReturnType(returnType));
        }

        public static IEnumerable<MethodInfo> WithParametersLengthOf(this IEnumerable<MethodInfo> methodInfos, int length)
        {
            return methodInfos.Where(m => m.HasParametersLengthOf(length));
        }

        public static IEnumerable<MethodInfo> WithParametersOfType(this IEnumerable<MethodInfo> methodInfos, params Type[] types)
        {
            return methodInfos.Where(m => m.HasParametersOfType(types));
        }

        public static IEnumerable<MethodInfo> WithGenericArgumentsLengthOf(this IEnumerable<MethodInfo> methodInfos, int length)
        {
            return methodInfos.Where(m => m.HasGenericArgumentsLengthOf(length));
        }

        public static IEnumerable<MethodInfo> WithGenericArgumentsOfType(this IEnumerable<MethodInfo> methodInfos, params Type[] types)
        {
            return methodInfos.Where(m => m.HasGenericArguments(types));
        }

        public static IEnumerable<MethodInfo> WithAttribute<T>(this IEnumerable<MethodInfo> methodInfos, bool inherit = false) where T : Attribute
        {
            return methodInfos.Where(m => m.HasAttribute<T>(inherit));
        }

        public static IEnumerable<MethodInfo> WithAttribute(this IEnumerable<MethodInfo> methodInfos, Type attributeType, bool inherit = false)
        {
            return methodInfos.Where(m => m.HasAttribute(attributeType, inherit));
        }

        public static IEnumerable<(MethodInfo MethodInfo, T Attribute)> WithAttributeExpanded<T>(this IEnumerable<MethodInfo> methodInfos, bool inherit = false) where T : Attribute
        {

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return WithAttribute<T>(methodInfos, inherit).Select(t => (t, t.GetCustomAttribute<T>()));
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

        }

        public static IEnumerable<(MethodInfo MethodInfo, Attribute Attribute)> WithAttributeExpanded(this IEnumerable<MethodInfo> methodInfos, Type attributeType, bool inherit = false)
        {

            if (!attributeType.InheritFromClass<Attribute>())
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' be an Attribute Type!");
            }

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return WithAttribute(methodInfos, attributeType, inherit).Select(t => (t, t.GetCustomAttribute(attributeType)));
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

        }

    }
}
