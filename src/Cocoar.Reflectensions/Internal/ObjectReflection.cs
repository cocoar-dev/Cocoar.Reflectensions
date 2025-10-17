using System;
using System.Collections.Generic;

namespace Cocoar.Reflectensions.Internal
{
    /// <summary>
    /// Non-generic struct wrapper for objects (for already-boxed values or when type is unknown).
    /// Zero heap allocation - lives on stack.
    /// </summary>
    public readonly struct ObjectReflection : IObjectReflection
    {
        private readonly object? _value;

        internal ObjectReflection(object? value)
        {
            _value = value;
        }

        public object? GetValue() => _value;

        public override bool Equals(object? obj)
        {
            return _value?.Equals(obj) ?? obj == null;
        }

        public override int GetHashCode()
        {
            return _value?.GetHashCode() ?? 0;
        }

        public override string? ToString()
        {
            return _value?.ToString();
        }

        public new Type GetType()
        {
            return _value?.GetType() ?? typeof(object);
        }

        public static bool operator ==(ObjectReflection first, ObjectReflection second)
        {
            return Equals(first._value, second._value);
        }

        public static bool operator !=(ObjectReflection first, ObjectReflection second)
        {
            return !Equals(first._value, second._value);
        }
    }

    /// <summary>
    /// Generic struct wrapper for objects - zero-cost abstraction.
    /// Avoids boxing for value types and provides type safety.
    /// </summary>
    /// <typeparam name="T">The type of the wrapped value.</typeparam>
    public readonly struct ObjectReflection<T> : IObjectReflection
    {
        private readonly T _value;

        internal ObjectReflection(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the wrapped value without boxing (if T is a value type).
        /// </summary>
        public T GetTypedValue() => _value;

        /// <summary>
        /// Gets the value as object (may box value types).
        /// </summary>
        public object? GetValue() => _value;

        public override bool Equals(object? obj)
        {
            return _value?.Equals(obj) ?? obj == null;
        }

        public override int GetHashCode()
        {
            return _value?.GetHashCode() ?? 0;
        }

        public override string? ToString()
        {
            return _value?.ToString();
        }

        public new Type GetType()
        {
            return typeof(T);
        }

        public static bool operator ==(ObjectReflection<T> first, ObjectReflection<T> second)
        {
            return EqualityComparer<T>.Default.Equals(first._value, second._value);
        }

        public static bool operator !=(ObjectReflection<T> first, ObjectReflection<T> second)
        {
            return !EqualityComparer<T>.Default.Equals(first._value, second._value);
        }

        /// <summary>
        /// Implicit conversion to non-generic ObjectReflection for compatibility.
        /// </summary>
        public static implicit operator ObjectReflection(ObjectReflection<T> generic)
        {
            return new ObjectReflection(generic._value);
        }
    }
}
