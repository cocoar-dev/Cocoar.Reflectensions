using Cocoar.Reflectensions.Internal;

namespace Cocoar.Reflectensions.ExtensionMethods
{
    /// <summary>
    /// Provides extension methods for creating reflection wrappers around objects.
    /// </summary>
    public static class IObjectExtensions
    {

        /// <summary>
        /// Returns the reflection wrapper as-is (identity function).
        /// </summary>
        /// <param name="reflectionObject">The reflection wrapper.</param>
        /// <returns>The same reflection wrapper.</returns>
        public static IObjectReflection Reflect(this IObjectReflection reflectionObject)
        {
            return reflectionObject;
        }

        /// <summary>
        /// Wraps a value in a generic reflection wrapper (zero-cost for value types).
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>A generic <see cref="ObjectReflection{T}"/> wrapper with zero allocation.</returns>
        /// <remarks>
        /// This overload avoids boxing for value types and provides type safety.
        /// The generic struct lives on the stack with zero heap allocation.
        /// </remarks>
        /// <example>
        /// <code>
        /// int num = 123;
        /// var wrapped = num.Reflect(); // No boxing! Calls Reflect&lt;int&gt;(int)
        /// 
        /// string str = "hello";
        /// var wrapped2 = str.Reflect(); // Calls Reflect&lt;string&gt;(string)
        /// </code>
        /// </example>
        public static ObjectReflection<T> Reflect<T>(this T value)
        {
            return new ObjectReflection<T>(value);
        }

        /// <summary>
        /// Wraps an object in a non-generic reflection wrapper (for already-boxed values).
        /// </summary>
        /// <param name="reflectionObject">The object to wrap.</param>
        /// <returns>A non-generic <see cref="ObjectReflection"/> wrapper.</returns>
        /// <remarks>
        /// This overload is used when the compile-time type is 'object'.
        /// The struct wrapper lives on the stack with minimal allocation.
        /// </remarks>
        /// <example>
        /// <code>
        /// object obj = GetSomeObject();
        /// var wrapped = obj.Reflect(); // Uses non-generic overload
        /// 
        /// var value = wrapped.To&lt;int&gt;();
        /// </code>
        /// </example>
        public static ObjectReflection Reflect(this object reflectionObject)
        {
            return new ObjectReflection(reflectionObject);
        }

    }
}
