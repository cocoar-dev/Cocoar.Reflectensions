using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cocoar.Reflectensions.ExtensionMethods
{
    public static class TypeEnumerableExtensions
    {
        #region Linq IEnumerable<Type>

        public static IEnumerable<Type> WithAttribute<T>(this IEnumerable<Type> types, bool inherit = false) where T : Attribute
        {
            return types.Where(m => m.HasAttribute<T>(inherit));
        }

        public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attributeType, bool inherit = false)
        {
            return types.Where(m => m.HasAttribute(attributeType, inherit));
        }

        public static IEnumerable<KeyValuePair<Type, T>> WithAttributeExpanded<T>(this IEnumerable<Type> types, bool inherit = false) where T : Attribute
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return WithAttribute<T>(types, inherit).Select(t => new KeyValuePair<Type, T>(t, t.GetCustomAttribute<T>()));
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public static IEnumerable<KeyValuePair<Type, Attribute>> WithAttributeExpanded(this IEnumerable<Type> types, Type attributeType, bool inherit = false)
        {

            if (!attributeType.InheritFromClass<Attribute>())
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' be an Attribute Type!");
            }

#pragma warning disable CS8604 // Possible null reference argument.
            return WithAttribute(types, attributeType, inherit).Select(t => new KeyValuePair<Type, Attribute>(t, t.GetCustomAttribute(attributeType)));
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public static IEnumerable<Type> WhichIsGenericTypeOf(this IEnumerable<Type> types, Type of)
        {
            return types.Where(t => t.IsGenericTypeOf(of));
        }

        public static IEnumerable<Type> WhichIsGenericTypeOf<T>(this IEnumerable<Type> types)
        {
            return types.Where(t => t.IsGenericTypeOf<T>());
        }

        public static IEnumerable<Type> WhichInheritFromClass(this IEnumerable<Type> types, Type from)
        {
            return types.Where(t => t.InheritFromClass(from));
        }
        public static IEnumerable<Type> WhichInheritFromClass<T>(this IEnumerable<Type> types)
        {
            return types.Where(t => t.InheritFromClass<T>());
        }

        #endregion

    }
}
