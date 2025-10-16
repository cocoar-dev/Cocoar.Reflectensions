using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cocoar.Reflectensions.Helper;
using Cocoar.Reflectensions.Internal;

namespace Cocoar.Reflectensions.ExtensionMethods
{
    public static class TypeExtensions
    {

        #region Check Type

        public static bool IsNumericType(this Type type)
        {
            return TypeLookupHelper.NumericTypes.Contains(type);
        }

        public static bool IsGenericTypeOf(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }

        public static bool IsGenericTypeOf<T>(this Type type)
        {
            return IsGenericTypeOf(type, typeof(T));
        }

        public static bool IsNullableType(this Type type)
        {
            return IsGenericTypeOf(type, typeof(Nullable<>));
        }

        public static bool IsEnumerableType(this Type type)
        {

            if (type == typeof(string))
                return false;

            if (IsDictionaryType(type))
                return false;

            return type.GetInterfaces().Contains(typeof(IEnumerable));
        }

        public static bool IsDictionaryType(this Type type)
        {
            return IsGenericTypeOf(type, typeof(IDictionary)) || 
                   IsGenericTypeOf(type, typeof(IDictionary<,>)) || 
                   ImplementsInterface(type, typeof(IDictionary)) || 
                   ImplementsInterface(type, typeof(IDictionary<,>));
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            if (interfaceType.IsGenericType || interfaceType.IsGenericTypeDefinition)
            {
                interfaceType = interfaceType.GetGenericTypeDefinition();
            }
            return type.GetInterfaces()
                .Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i)
                .Contains(interfaceType);
        }

        public static bool ImplementsInterface<T>(this Type type)
        {
            return ImplementsInterface(type, typeof(T));
        }

        public static bool InheritFromClass<T>(this Type type)
        {
            return type.InheritFromClassLevel<T>() > 0;
        }

        public static bool InheritFromClass(this Type type, string from)
        {
            return InheritFromClassLevel(type, from) > 0;
        }

        public static bool InheritFromClass(this Type type, Type from)
        {
            return InheritFromClassLevel(type, from) > 0;
        }

        public static bool IsImplicitCastableTo<T>(this Type type)
        {
            return IsImplicitCastableTo(type, typeof(T));
        }

        public static bool IsImplicitCastableTo(this Type type, Type to)
        {

            if (IsNumericType(type) && IsNumericType(to))
            {
                if (TypeLookupHelper.ImplicitNumericConversionsTable[type].Contains(to))
                    return true;
            }

            if (ImplementsInterface(type, to) || InheritFromClass(type, to))
            {
                return true;
            }

            if (GetImplicitCastMethodTo(type, to) != null)
                return true;

            if (IsNullableType(to))
            {
                var underlingType = Nullable.GetUnderlyingType(to);
                if (underlingType != null && IsImplicitCastableTo(type, underlingType))
                {
                    return true;
                }
            }

            return false;

        }

        public static bool Equals<T>(this Type type)
        {
            return type == typeof(T);
        }

        public static bool NotEquals<T>(this Type type)
        {
            return !Equals<T>(typeof(T));
        }

        public static bool HasAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            return HasAttribute(type, typeof(T));
        }

        public static bool HasAttribute(this Type type, Type attributeType, bool inherit = false)
        {
            if (!InheritFromClass<Attribute>(attributeType))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' has to be an Attribute Type!");
            }

            return type.GetCustomAttribute(attributeType, inherit) != null;
        }

        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }


        #endregion


        #region Get Operator Methods


        public static IEnumerable<MethodInfo> GetImplicitOperatorMethods(this Type type, bool throwOnError = true)
        {

            if (type == null)
            {
                if (throwOnError)
                {
                    throw new ArgumentNullException(nameof(type));
                }
                return new List<MethodInfo>();
            }

            return type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.HasName("op_Implicit"));
        }
        public static IEnumerable<MethodInfo> GetExplicitOperatorMethods(this Type type, bool throwOnError = true)
        {
            if (type == null)
            {
                if (throwOnError)
                {
                    throw new ArgumentNullException(nameof(type));
                }
                return new List<MethodInfo>();
            }

            return type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.HasName("op_Explicit"));
        }

        public static IEnumerable<MethodInfo> GetImplicitOperatorMethods<T>(bool throwOnError = true)
        {
            return GetImplicitOperatorMethods(typeof(T), throwOnError);
        }

        public static IEnumerable<MethodInfo> GetExplicitOperatorMethods<T>(bool throwOnError = true)
        {
            return GetExplicitOperatorMethods(typeof(T), throwOnError);
        }

        public static MethodInfo? GetImplicitCastMethodTo<T>(this Type fromType)
        {
            return GetImplicitCastMethodTo(fromType, typeof(T));
        }
        public static MethodInfo? GetImplicitCastMethodTo(this Type fromType, Type toType)
        {

            return GetImplicitOperatorMethods(fromType, false).WithReturnType(toType).WithParametersOfType(fromType).FirstOrDefault() ??
                     GetImplicitOperatorMethods(toType, false).WithReturnType(toType).WithParametersOfType(fromType).FirstOrDefault();

        }

        public static MethodInfo? GetExplicitCastMethodTo<T>(this Type fromType)
        {
            return GetExplicitCastMethodTo(fromType, typeof(T));
        }
        public static MethodInfo? GetExplicitCastMethodTo(this Type fromType, Type toType)
        {
            return GetExplicitOperatorMethods(fromType, false).WithReturnType(toType).WithParametersOfType(fromType).FirstOrDefault() ??
                   GetExplicitOperatorMethods(toType, false).WithReturnType(toType).WithParametersOfType(fromType).FirstOrDefault();
        }

        #endregion


        public static int InheritFromClassLevel(this Type type, Type from, int? maximumLevel = null)
        {
            
            lock (TypeHelperCache.InheritanceListLock)
            {
                var exists = TypeHelperCache.InheritanceList.FirstOrDefault(item => item?.Type == type && item?.From == from);
                if (exists != null)
                    return exists.Level;

                var level = 0;
                bool found = type == from;

                if (!found)
                {
                    var lookupType = type;

                    if (lookupType.IsGenericType)
                    {
                        found = lookupType.GetGenericTypeDefinition() == from;
                    }

                    while (lookupType != null && !found && (!maximumLevel.HasValue || level >= maximumLevel.Value))
                    {
                        level++;

                        found = lookupType.BaseType == from;
                        if (found)
                        {
                            break;
                        }
                        lookupType = lookupType.BaseType;
                        if (lookupType?.IsGenericType == true)
                        {
                            found = lookupType.GetGenericTypeDefinition() == from;
                        }

                    }
                }
                

                if (!found)
                    level = -1;

                TypeHelperCache.InheritanceList.Add(new(type, from, level));
                return level;
            }

        }

        public static int InheritFromClassLevel(this Type type, string from, int? maximumLevel = null)
        {
            var fromType = TypeHelper.FindType(from);
            if (fromType == null)
            {
                throw new TypeLoadException($"Can't find Type '{from}'");
            }

            return InheritFromClassLevel(type, fromType, maximumLevel);
        }

        public static int InheritFromClassLevel<T>(this Type type, int? maximumLevel = null)
        {
            return InheritFromClassLevel(type, typeof(T), maximumLevel);
        }


        public static IEnumerable<Type> GetDirectInterfaces(this Type type)
        {
            var allInterfaces = new List<Type>();
            var childInterfaces = new List<Type>();

            
            foreach (var i in type.GetInterfaces())
            {
                allInterfaces.Add(i);
                foreach (var ii in i.GetInterfaces())
                    childInterfaces.Add(ii);
            }

            if (TypeHelper.HasInspectableBaseType(type))
            {
                foreach (var baseTypeInterface in type.BaseType?.GetInterfaces() ?? Array.Empty<Type>())
                {
                    childInterfaces.Add(baseTypeInterface);
                }
            }

            return allInterfaces.Except(childInterfaces);
        }

        

    }
}
