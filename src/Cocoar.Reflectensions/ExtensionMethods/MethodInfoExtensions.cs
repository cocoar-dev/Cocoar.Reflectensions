using System;
using System.Linq;
using System.Reflection;

namespace Cocoar.Reflectensions.ExtensionMethods
{
    public static class MethodInfoExtensions
    {

        public static bool HasName(this MethodInfo methodInfo, string name, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            return methodInfo.Name.Equals(name, stringComparison);
        }

        public static bool HasParametersLengthOf(this MethodInfo methodInfo, int parameterLength, bool includeOptional = false)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var methodInfoParameters = methodInfo.GetParameters();

            if (methodInfoParameters.Length < parameterLength)
            {
                return false;
            }

            if (methodInfoParameters.Length == parameterLength)
            {
                return true;
            }

            return includeOptional && methodInfoParameters.Skip(parameterLength).All(p => p.IsOptional);
        }

        public static bool HasParametersOfType(this MethodInfo methodInfo, Type[] types)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!HasParametersLengthOf(methodInfo, types.Length, true))
                return false;

            var match = true;
            var methodParameters = methodInfo.GetParameters();
            for (var i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i].ParameterType != types[i])
                {
                    match = false;
                    break;
                }
            }

            return match;
        }

        public static bool HasAttribute<T>(this MethodInfo methodInfo, bool inherit = false) where T : Attribute
        {
            return HasAttribute(methodInfo, typeof(T), inherit);
        }

        public static bool HasAttribute(this MethodInfo methodInfo, Type attributeType, bool inherit = false)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!attributeType.InheritFromClass<Attribute>())
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' has to be an Attribute Type!");
            }

            return methodInfo.GetCustomAttribute(attributeType, inherit) != null;
        }

        public static bool HasReturnType<T>(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }
            return methodInfo.ReturnType.Equals<T>();
        }

        public static bool HasReturnType(this MethodInfo methodInfo, Type returnType)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (returnType == null)
            {
                throw new ArgumentNullException(nameof(returnType));
            }

            return methodInfo.ReturnType == returnType;
        }

        public static bool HasGenericArgumentsLengthOf(this MethodInfo methodInfo, int genericArgumentsLength)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var methodInfoGenericArguments = methodInfo.GetGenericArguments();

            return methodInfoGenericArguments.Length == genericArgumentsLength;
        }


        public static bool HasGenericArguments(this MethodInfo methodInfo, Type[] types)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!HasGenericArgumentsLengthOf(methodInfo, types.Length))
                return false;

            var match = true;
            var methodParameters = methodInfo.GetGenericArguments();
            for (var i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i] != types[i])
                {
                    match = false;
                    break;
                }
            }

            return match;
        }

    }
}
