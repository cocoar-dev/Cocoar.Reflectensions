using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cocoar.Reflectensions.Exceptions;
using Cocoar.Reflectensions.Helper;
using Cocoar.Reflectensions.Internal;

namespace Cocoar.Reflectensions.ExtensionMethods
{
    /// <summary>
    /// Provides extension methods for object reflection and type conversion operations.
    /// </summary>
    public static class ObjectReflectionExtensions
    {

        /// <summary>
        /// Determines whether the object's value equals any of the provided values.
        /// </summary>
        /// <param name="objectReflection">The object reflection wrapper.</param>
        /// <param name="equalsTo">The values to compare against.</param>
        /// <returns>True if the object equals any of the provided values; otherwise, false.</returns>
        public static bool EqualsToAnyOf(this IObjectReflection objectReflection, params object[] equalsTo)
        {
            var value = objectReflection.GetValue();

            foreach (var trueValue in equalsTo)
            {
                if (value?.Equals(trueValue) == true)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts the object to a boolean value with optional custom true values.
        /// </summary>
        /// <param name="objectReflection">The object reflection wrapper.</param>
        /// <param name="trueValues">Optional custom values that should be considered true.</param>
        /// <returns>The boolean representation of the object.</returns>
        /// <remarks>
        /// If no custom true values are provided, uses standard conversions (bool, int != 0, "yes", "true", etc.).
        /// </remarks>
        public static bool ToBoolean(this IObjectReflection objectReflection, params object[] trueValues)
        {
            var value = objectReflection.GetValue();

            if (value == null)
                return false;

            if (trueValues.Any())
            {
                return EqualsToAnyOf(objectReflection, trueValues);
            }


            if (value is bool boolValue)
                return boolValue;



            var str = value.ToString();
            if (int.TryParse(str, out var numb))
            {
                return numb > 0;
            }

            if (bool.TryParse(str, out var ret))
            {
                return ret;
            }


            if (str?.ToLower(CultureInfo.InvariantCulture) == "yes")
            {
                return true;
            }

            return ret;
        }


        public static bool IsImplicitCastableTo(this IObjectReflection objectReflection, Type type)
        {
            return objectReflection.GetType().IsImplicitCastableTo(type);
        }
        
        public static bool IsImplicitCastableTo<T>(this IObjectReflection objectReflection)
        {
            return objectReflection.GetType().IsImplicitCastableTo<T>();
        }


        public static bool TryTo(this IObjectReflection objectReflection, Type type, out object? outValue)
        {



            if (type == typeof(bool))
            {
                outValue = ToBoolean(objectReflection);
                return true;
            }



            if (TryAs(objectReflection, type, out outValue))
            {
                return true;
            }


            if (type.IsNullableType())
            {
                var nt = Nullable.GetUnderlyingType(type);
                if (nt == null)
                {
                    outValue = null;
                    return false;
                }

                type = nt;
            }


            var value = objectReflection.GetValue();

            if (type == typeof(Guid))
            {
                if (value is string str)
                {
                    if(Guid.TryParse(str, out var g))
                    {
                        outValue = g;
                        return true;
                    }
                    else
                    {
                        outValue = null;
                        return false;
                    }
                }
            }

            if (type.IsNumericType())
            {
                try
                {
                    outValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                    return true;
                }
                catch
                {
                }
            }

            if (value!.GetType().ImplementsInterface<IConvertible>() && type.ImplementsInterface<IConvertible>())
            {
                try
                {
                    outValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                    return true;
                }
                catch
                {
                }
            }


            var method = value.GetType().GetImplicitCastMethodTo(type);

            if (method != null)
            {
                outValue = method.Invoke(null, new[] {
                    value
                });
                return true;
            }

            outValue = null;
            return false;

        }

        public static bool TryTo<T>(this IObjectReflection objectReflection, out T? outValue)
        {

            var result = TryTo(objectReflection, typeof(T), out var _outValue);

            outValue = _outValue != null ? (T)_outValue : default;
            return result;
        }

        public static object? To(this IObjectReflection objectReflection, Type type, object? defaultValue)
        {

            var result = TryTo(objectReflection, type, out var outValue);
            if (result)
                return outValue;

            if (defaultValue != null)
            {
                return defaultValue;
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;

        }

        public static object? To(this IObjectReflection objectReflection, Type type)
        {

            var result = TryTo(objectReflection, type, out var outValue);
            if (result)
                return outValue;

            throw new InvalidCastException($"Can't cast object of Type '{objectReflection.GetType()}' to '{type}'.");
        }

        public static T? To<T>(this IObjectReflection objectReflection, T? defaultValue)
        {
            return (T?)To(objectReflection, typeof(T), defaultValue);
        }


        public static  T? To<T>(this IObjectReflection objectReflection)
        {
            return (T?)To(objectReflection, typeof(T));
        }

        #region Hot Path Overloads for Generic ObjectReflection<T> - Auto-optimized conversions

        /// <summary>
        /// Hot path for int.Reflect().To&lt;string&gt;() - uses ToString() directly instead of complex conversion.
        /// </summary>
        public static TResult? To<TResult>(this ObjectReflection<int> objectReflection)
        {
            // Hot path: int to string
            if (typeof(TResult) == typeof(string))
            {
                return (TResult)(object)objectReflection.GetTypedValue().ToString(CultureInfo.InvariantCulture);
            }
            
            // Hot path: int to long
            if (typeof(TResult) == typeof(long))
            {
                return (TResult)(object)(long)objectReflection.GetTypedValue();
            }
            
            // Hot path: int to double
            if (typeof(TResult) == typeof(double))
            {
                return (TResult)(object)(double)objectReflection.GetTypedValue();
            }
            
            // Hot path: int to decimal
            if (typeof(TResult) == typeof(decimal))
            {
                return (TResult)(object)(decimal)objectReflection.GetTypedValue();
            }
            
            // Fall back to general conversion for other types
            return ((IObjectReflection)objectReflection).To<TResult>();
        }

        /// <summary>
        /// Hot path for long.Reflect().To&lt;string&gt;() - uses ToString() directly instead of complex conversion.
        /// </summary>
        public static TResult? To<TResult>(this ObjectReflection<long> objectReflection)
        {
            // Hot path: long to string
            if (typeof(TResult) == typeof(string))
            {
                return (TResult)(object)objectReflection.GetTypedValue().ToString(CultureInfo.InvariantCulture);
            }
            
            // Hot path: long to int (with range check)
            if (typeof(TResult) == typeof(int))
            {
                var value = objectReflection.GetTypedValue();
                if (value >= int.MinValue && value <= int.MaxValue)
                {
                    return (TResult)(object)(int)value;
                }
            }
            
            // Hot path: long to double
            if (typeof(TResult) == typeof(double))
            {
                return (TResult)(object)(double)objectReflection.GetTypedValue();
            }
            
            // Fall back to general conversion
            return ((IObjectReflection)objectReflection).To<TResult>();
        }

        /// <summary>
        /// Hot path for double.Reflect().To&lt;string&gt;() - uses ToString() directly instead of complex conversion.
        /// </summary>
        public static TResult? To<TResult>(this ObjectReflection<double> objectReflection)
        {
            // Hot path: double to string
            if (typeof(TResult) == typeof(string))
            {
                return (TResult)(object)objectReflection.GetTypedValue().ToString(CultureInfo.InvariantCulture);
            }
            
            // Hot path: double to int (with truncation)
            if (typeof(TResult) == typeof(int))
            {
                var value = objectReflection.GetTypedValue();
                if (value >= int.MinValue && value <= int.MaxValue)
                {
                    return (TResult)(object)(int)value;
                }
            }
            
            // Fall back to general conversion
            return ((IObjectReflection)objectReflection).To<TResult>();
        }

        /// <summary>
        /// Hot path for string.Reflect().To&lt;int&gt;() - uses Parse() directly instead of complex conversion.
        /// </summary>
        public static TResult? To<TResult>(this ObjectReflection<string> objectReflection)
        {
            var str = objectReflection.GetTypedValue();
            
            // Hot path: string to int
            if (typeof(TResult) == typeof(int))
            {
                if (int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intResult))
                {
                    return (TResult)(object)intResult;
                }
            }
            
            // Hot path: string to long
            if (typeof(TResult) == typeof(long))
            {
                if (long.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longResult))
                {
                    return (TResult)(object)longResult;
                }
            }
            
            // Hot path: string to double
            if (typeof(TResult) == typeof(double))
            {
                if (double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleResult))
                {
                    return (TResult)(object)doubleResult;
                }
            }
            
            // Hot path: string to bool
            if (typeof(TResult) == typeof(bool))
            {
                if (bool.TryParse(str, out var boolResult))
                {
                    return (TResult)(object)boolResult;
                }
            }
            
            // Hot path: string to DateTime
            if (typeof(TResult) == typeof(DateTime))
            {
                if (DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateResult))
                {
                    return (TResult)(object)dateResult;
                }
            }
            
            // Fall back to general conversion
            return ((IObjectReflection)objectReflection).To<TResult>();
        }

        /// <summary>
        /// Hot path for DateTime.Reflect().To&lt;string&gt;() - uses ToString() directly instead of complex conversion.
        /// </summary>
        public static TResult? To<TResult>(this ObjectReflection<DateTime> objectReflection)
        {
            // Hot path: DateTime to string
            if (typeof(TResult) == typeof(string))
            {
                return (TResult)(object)objectReflection.GetTypedValue().ToString(CultureInfo.InvariantCulture);
            }
            
            // Fall back to general conversion
            return ((IObjectReflection)objectReflection).To<TResult>();
        }

        /// <summary>
        /// Hot path for bool.Reflect().To&lt;string&gt;() - uses ToString() directly instead of complex conversion.
        /// </summary>
        public static TResult? To<TResult>(this ObjectReflection<bool> objectReflection)
        {
            // Hot path: bool to string
            if (typeof(TResult) == typeof(string))
            {
                return (TResult)(object)objectReflection.GetTypedValue().ToString(CultureInfo.InvariantCulture);
            }
            
            // Hot path: bool to int
            if (typeof(TResult) == typeof(int))
            {
                return (TResult)(object)(objectReflection.GetTypedValue() ? 1 : 0);
            }
            
            // Fall back to general conversion
            return ((IObjectReflection)objectReflection).To<TResult>();
        }

        #endregion


        public static bool TryAs(this IObjectReflection objectReflection, Type type, out object? outValue)

        {
            var value = objectReflection.GetValue();

            if (value == null)
            {
                outValue = null;
                return false;
            }

            var t = value.GetType();

            if (t == type)
            {
                outValue = value;
                return true;
            }

            if (t.ImplementsInterface(type) || t.InheritFromClass(type))
            {
                outValue = value;
                return true;
            }


            if (type.IsNullableType())
            {
                var underlingType = Nullable.GetUnderlyingType(type);
                if (underlingType != null && new ObjectReflection(value).TryAs(underlingType, out var innerValue))
                {
                    outValue = innerValue;
                    return true;
                }
            }

            outValue = null;
            return false;

        }

        public static bool TryAs<T>(this IObjectReflection objectReflection, out T? outValue)
        {
            var result = TryAs(objectReflection, typeof(T), out var _outValue);

            outValue = _outValue != null ? (T)_outValue : default;
            return result;
        }

        public static object? As(this IObjectReflection objectReflection, Type type)
        {

            var result = TryAs(objectReflection, type, out var outValue);
            return result ? outValue : null;

        }

        public static T? As<T>(this IObjectReflection objectReflection)
        {

            var result = TryAs<T>(objectReflection, out var outValue);
            return result ? outValue : default;

        }


        public static object? GetPropertyValue(this IObjectReflection objectReflection, string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            return GetPropertyValue<object>(objectReflection, name, bindingFlags);
        }

        public static T? GetPropertyValue<T>(this IObjectReflection objectReflection, string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            if (objectReflection.GetValue() == null)
                throw new ArgumentNullException();

            var parts = name.Split('.');

            var currentObject = objectReflection.GetValue();

            var processedPaths = new List<string>();
            foreach (var part in parts)
            {

                processedPaths.Add(part);
                var currentPropertyInfo = currentObject?.GetType().GetProperty(part, bindingFlags);


                if (currentPropertyInfo == null)
                    throw new PropertyNotFoundException($"Path not found '{string.Join(".", processedPaths)}'");

                currentObject = currentPropertyInfo.GetValue(currentObject);

                if (currentObject == null)
                    throw new PropertyNotFoundException($"Path not found '{string.Join(".", processedPaths)}'");

            }

            return currentObject!.Reflect().To<T>();

        }

        public static void SetPropertyValue(this IObjectReflection objectReflection, string path, object value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            var parts = path.Split('.');

            var currentObject = objectReflection.GetValue();
            var processedPaths = new List<string>();

            for (var i = 0; i < parts.Length; i++)
            {
                if (currentObject == null)
                    break;

                var part = parts[i];
                var isLast = i == parts.Length - 1;

                processedPaths.Add(part);
                var currentPropertyInfo = currentObject.GetType().GetProperty(part, bindingFlags);


                if (currentPropertyInfo == null)
                    throw new PropertyNotFoundException($"Path not found '{string.Join(".", processedPaths)}'");

                if (isLast)
                {
                    currentPropertyInfo.SetValue(currentObject, value);
                    return;
                }
                else
                {
                    currentObject = currentPropertyInfo.GetValue(currentObject);
                }

            }

        }



    }
}
