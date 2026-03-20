# API Overview

Complete reference of all public types, extension methods, and helpers in Cocoar.Reflectensions.

## Packages

| Package | Namespace | Description |
|---------|-----------|-------------|
| [Cocoar.Reflectensions](/reference/packages#core) | `Cocoar.Reflectensions` | Core library — type parsing, reflection extensions, type conversion, utilities |
| [Cocoar.Reflectensions.Invoke](/reference/packages#invoke) | `Cocoar.Reflectensions.Helper` | Dynamic method invocation with parameter matching |
| [Cocoar.Reflectensions.ExpandableObject](/reference/packages#expandable-object) | `Cocoar.Reflectensions` | Dynamic objects with dictionary-like behavior |

## TypeHelper

Static helper for parsing and resolving types from strings.

```csharp
public static class TypeHelper
{
    // Resolve type from string
    static Type? FindType(string typeName);
    static Type? FindType(string typeName, IDictionary<string, string> customTypeMapping);

    // Normalize type name to CLR format
    static string NormalizeTypeName(string typeName);
    static string NormalizeTypeName(string typeName, IDictionary<string, string> customTypeMapping);

    // Check base type
    static bool HasInspectableBaseType(Type type);
}
```

## ObjectReflection

Readonly struct wrappers for object reflection. Created via `.Reflect()`.

```csharp
// Non-generic wrapper (boxes value types)
public readonly struct ObjectReflection : IObjectReflection
{
    object? GetValue();
}

// Generic wrapper (zero-cost for value types)
public readonly struct ObjectReflection<T> : IObjectReflection
{
    T GetTypedValue();
    object? GetValue();
    static implicit operator ObjectReflection(ObjectReflection<T> value);
}
```

## Extension Methods — Entry Point

```csharp
public static class IObjectExtensions
{
    // Generic — no boxing for value types
    static ObjectReflection<T> Reflect<T>(this T value);

    // Non-generic — for object-typed values
    static ObjectReflection Reflect(this object obj);
}
```

## Extension Methods — Type

```csharp
public static class TypeExtensions
{
    // Type checking
    static bool IsNumericType(this Type type);
    static bool IsGenericTypeOf(this Type type, Type genericType);
    static bool IsGenericTypeOf<T>(this Type type);
    static bool IsNullableType(this Type type);
    static bool IsEnumerableType(this Type type);
    static bool IsDictionaryType(this Type type);
    static bool IsStatic(this Type type);
    static bool Equals<T>(this Type type);
    static bool NotEquals<T>(this Type type);

    // Relationships
    static bool ImplementsInterface(this Type type, Type interfaceType);
    static bool ImplementsInterface<T>(this Type type);
    static bool InheritFromClass<T>(this Type type);
    static bool InheritFromClass(this Type type, Type from);
    static bool InheritFromClass(this Type type, string from);
    static int InheritFromClassLevel(this Type type, Type from, int? maximumLevel = null);
    static int InheritFromClassLevel(this Type type, string from, int? maximumLevel = null);
    static int InheritFromClassLevel<T>(this Type type, int? maximumLevel = null);

    // Castability
    static bool IsImplicitCastableTo<T>(this Type type);
    static bool IsImplicitCastableTo(this Type type, Type to);

    // Attributes
    static bool HasAttribute<T>(this Type type, bool inherit = false) where T : Attribute;
    static bool HasAttribute(this Type type, Type attributeType, bool inherit = false);

    // Operator methods
    static IEnumerable<MethodInfo> GetImplicitOperatorMethods(this Type type, bool throwOnError = true);
    static IEnumerable<MethodInfo> GetImplicitOperatorMethods<T>(bool throwOnError = true);
    static IEnumerable<MethodInfo> GetExplicitOperatorMethods(this Type type, bool throwOnError = true);
    static IEnumerable<MethodInfo> GetExplicitOperatorMethods<T>(bool throwOnError = true);
    static MethodInfo? GetImplicitCastMethodTo<T>(this Type fromType);
    static MethodInfo? GetImplicitCastMethodTo(this Type fromType, Type toType);
    static MethodInfo? GetExplicitCastMethodTo<T>(this Type fromType);
    static MethodInfo? GetExplicitCastMethodTo(this Type fromType, Type toType);

    // Interface discovery
    static IEnumerable<Type> GetDirectInterfaces(this Type type);
}
```

## Extension Methods — ObjectReflection

```csharp
public static class ObjectReflectionExtensions
{
    // Equality
    static bool EqualsToAnyOf(this IObjectReflection obj, params object[] equalsTo);
    static bool ToBoolean(this IObjectReflection obj, params object[] trueValues);

    // Castability
    static bool IsImplicitCastableTo(this IObjectReflection obj, Type type);
    static bool IsImplicitCastableTo<T>(this IObjectReflection obj);

    // Type switching (As)
    static bool TryAs(this IObjectReflection obj, Type type, out object? outValue);
    static bool TryAs<T>(this IObjectReflection obj, out T? outValue);
    static object? As(this IObjectReflection obj, Type type);
    static T? As<T>(this IObjectReflection obj);

    // Smart conversion (To)
    static bool TryTo(this IObjectReflection obj, Type type, out object? outValue);
    static bool TryTo<T>(this IObjectReflection obj, out T? outValue);
    static object? To(this IObjectReflection obj, Type type);
    static object? To(this IObjectReflection obj, Type type, object? defaultValue);
    static T? To<T>(this IObjectReflection obj);
    static T? To<T>(this IObjectReflection obj, T? defaultValue);

    // Hot path overloads for ObjectReflection<int>, <long>, <double>, <string>, <DateTime>, <bool>
    static TResult? To<TResult>(this ObjectReflection<int> obj);
    static TResult? To<TResult>(this ObjectReflection<long> obj);
    static TResult? To<TResult>(this ObjectReflection<double> obj);
    static TResult? To<TResult>(this ObjectReflection<string> obj);
    static TResult? To<TResult>(this ObjectReflection<DateTime> obj);
    static TResult? To<TResult>(this ObjectReflection<bool> obj);

    // Property access
    static object? GetPropertyValue(this IObjectReflection obj, string name,
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance);
    static T? GetPropertyValue<T>(this IObjectReflection obj, string name,
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance);
    static void SetPropertyValue(this IObjectReflection obj, string path, object value,
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance);
}
```

## Extension Methods — MethodInfo

```csharp
public static class MethodInfoExtensions
{
    static bool HasName(this MethodInfo m, string name,
        StringComparison comparison = StringComparison.CurrentCulture);
    static bool HasParametersLengthOf(this MethodInfo m, int length, bool includeOptional = false);
    static bool HasParametersOfType(this MethodInfo m, Type[] types);
    static bool HasAttribute<T>(this MethodInfo m, bool inherit = false) where T : Attribute;
    static bool HasAttribute(this MethodInfo m, Type attributeType, bool inherit = false);
    static bool HasReturnType<T>(this MethodInfo m);
    static bool HasReturnType(this MethodInfo m, Type returnType);
    static bool HasGenericArgumentsLengthOf(this MethodInfo m, int length);
    static bool HasGenericArguments(this MethodInfo m, Type[] types);
}

public static class MethodInfoEnumerableExtensions
{
    static IEnumerable<MethodInfo> WithName(this IEnumerable<MethodInfo> methods, string name,
        StringComparison comparison = StringComparison.CurrentCulture);
    static IEnumerable<MethodInfo> WithReturnType<T>(this IEnumerable<MethodInfo> methods);
    static IEnumerable<MethodInfo> WithReturnType(this IEnumerable<MethodInfo> methods, Type type);
    static IEnumerable<MethodInfo> WithParametersLengthOf(this IEnumerable<MethodInfo> methods,
        int length, bool includeOptional = false);
    static IEnumerable<MethodInfo> WithParametersOfType(this IEnumerable<MethodInfo> methods,
        params Type[] types);
    static IEnumerable<MethodInfo> WithGenericArgumentsLengthOf(this IEnumerable<MethodInfo> methods,
        int length);
    static IEnumerable<MethodInfo> WithGenericArgumentsOfType(this IEnumerable<MethodInfo> methods,
        params Type[] types);
    static IEnumerable<MethodInfo> WithAttribute<T>(this IEnumerable<MethodInfo> methods,
        bool inherit = false) where T : Attribute;
    static IEnumerable<(MethodInfo, T)> WithAttributeExpanded<T>(this IEnumerable<MethodInfo> methods,
        bool inherit = false) where T : Attribute;
}

public static class MethodBaseExtensions
{
    static bool IsExtensionMethod(this MethodBase method);
}
```

## Extension Methods — Type Collections

```csharp
public static class TypeEnumerableExtensions
{
    static IEnumerable<Type> WithAttribute<T>(this IEnumerable<Type> types,
        bool inherit = false) where T : Attribute;
    static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types,
        Type attributeType, bool inherit = false);
    static IEnumerable<KeyValuePair<Type, T>> WithAttributeExpanded<T>(
        this IEnumerable<Type> types, bool inherit = false) where T : Attribute;
    static IEnumerable<KeyValuePair<Type, Attribute>> WithAttributeExpanded(
        this IEnumerable<Type> types, Type attributeType, bool inherit = false);
    static IEnumerable<Type> WhichIsGenericTypeOf(this IEnumerable<Type> types, Type of);
    static IEnumerable<Type> WhichIsGenericTypeOf<T>(this IEnumerable<Type> types);
    static IEnumerable<Type> WhichInheritFromClass(this IEnumerable<Type> types, Type from);
    static IEnumerable<Type> WhichInheritFromClass<T>(this IEnumerable<Type> types);
}
```

## Extension Methods — PropertyInfo

```csharp
public static class PropertyInfoExtensionMethods
{
    static bool IsIndexerProperty(this PropertyInfo prop);
    static bool IsPublic(this PropertyInfo prop);
    static IEnumerable<PropertyInfo> WhichIsIndexerProperty(this IEnumerable<PropertyInfo> props);
}
```

## Extension Methods — ParameterInfo

```csharp
public static class ParameterInfoExtensions
{
    static bool HasAttribute(this ParameterInfo param, Type attributeType, bool inherit = false);
    static bool HasAttribute(this ParameterInfo param, string attributeName, bool inherit = false);
}

public static class ParameterInfoEnumerableExtensions
{
    static IEnumerable<ParameterInfo> WithName(this IEnumerable<ParameterInfo> params,
        string name, bool ignoreCase);
    static IEnumerable<ParameterInfo> WithTypeOf(this IEnumerable<ParameterInfo> params, Type type);
    static IEnumerable<ParameterInfo> WithTypeOf<T>(this IEnumerable<ParameterInfo> params);
    static IEnumerable<ParameterInfo> WithoutTypeOf(this IEnumerable<ParameterInfo> params, Type type);
    static IEnumerable<ParameterInfo> WithoutTypeOf<T>(this IEnumerable<ParameterInfo> params);
    static IEnumerable<ParameterInfo> WithAttribute(this IEnumerable<ParameterInfo> params,
        string attributeName);
    static IEnumerable<ParameterInfo> WithoutAttribute(this IEnumerable<ParameterInfo> params,
        string attributeName);
}
```

## Extension Methods — String

```csharp
public static class StringExtensions
{
    // Validation
    static bool IsNullOrWhiteSpace(this string? value);
    static bool IsNumeric(this string? value);
    static bool IsInt(this string? value);
    static bool IsLong(this string? value);
    static bool IsDouble(this string? value);
    static bool IsDateTime(this string? value);
    static bool IsBoolean(this string? value);
    static bool IsValidIp(this string? value);
    static bool IsBase64Encoded(this string? value);
    static bool IsLowerCase(this string? value);
    static bool IsUpperCase(this string? value);
    static bool IsValidEmailAddress(this string? value);
    static bool IsGuid(this string? value);

    // Conversion
    static int ToInt(this string value);
    static long ToLong(this string value);
    static double ToDouble(this string value);
    static float ToFloat(this string value);
    static decimal ToDecimal(this string value);
    static bool ToBoolean(this string value);
    static DateTime ToDateTime(this string value);
    static Guid ToGuid(this string value);
    static int? ToNullableInt(this string? value);
    static long? ToNullableLong(this string? value);
    static double? ToNullableDouble(this string? value);
    static float? ToNullableFloat(this string? value);
    static decimal? ToNullableDecimal(this string? value);
    static DateTime? ToNullableDateTime(this string? value);

    // Manipulation
    static string? TrimToNull(this string? value);
    static string? ToNull(this string? value);
    static string RemoveEmptyLines(this string value);
    static string RemoveEnd(this string value, string end);

    // Encoding
    static string EncodeToBase64(this string value);
    static string DecodeFromBase64(this string value);
    static string EncodeToBase58(this string value);
    static string DecodeFromBase58(this string value);

    // Pattern matching
    static bool Match(this string value, string matchString,
        bool ignoreCase = true, bool invert = false);
}
```

## Extension Methods — Enum

```csharp
public static class EnumExtensions
{
    static string GetName(this Enum value);
    static bool TryFind(Type enumType, string value, out object? result);
    static bool TryFind(Type enumType, string value, bool ignoreCase, out object? result);
}
```

## Extension Methods — DateTime

```csharp
public static class DateTimeExtensions
{
    static long ToUnixTimeMilliseconds(this DateTime dateTime);
    static DateTime FromUnixTimeMilliseconds(this long milliseconds);
    static DateTime FromUnixTimeSeconds(this long seconds);
    static DateTime ToUtc(this DateTime dateTime);
    static DateTime ToLocal(this DateTime dateTime);
    static DateTime SetKind(this DateTime dateTime, DateTimeKind kind);
    static DateTime SetIsUtc(this DateTime dateTime);
    static DateTime SetIsLocal(this DateTime dateTime);
}
```

## Extension Methods — Collections

```csharp
// Merge (in Cocoar.Reflectensions.ExtensionMethods namespace)
public static class IDictionaryExtensions
{
    static T Merge<T>(this T dict, T mergeWith) where T : IDictionary;
}

// Typed value access (in Cocoar.Reflectensions namespace)
public static class IDictionaryExtensions
{
    static T? GetValueAs<T>(this IDictionary<string, object> dictionary, string key, T? orDefault = default);
    static bool TryGetValueAs<T>(this IDictionary<string, object> dictionary, string key, out T? value);
    static T? GetValueTo<T>(this IDictionary<string, object> dictionary, string key, T? orDefault = default);
    static bool TryGetValueTo<T>(this IDictionary<string, object> dictionary, string key, out T? value);
}

public static class ArrayExtensions
{
    static T[] Concat<T>(this T[] first, T[] second);
    static T[] SubArray<T>(this T[] source, int startIndex, int length);
    static T[] SubArray<T>(this T[] source, int startIndex);
}
```

## Extension Methods — Assembly

```csharp
public static class AssemblyExtensions
{
    static Stream? ReadResourceAsStream(this Assembly assembly, string resourceName);
    static string? ReadResourceAsString(this Assembly assembly, string resourceName);
    static byte[]? ReadResourceAsByteArray(this Assembly assembly, string resourceName);
}
```

## Extension Methods — Task

```csharp
public static class TaskExtensions
{
    static Task<TResult> CastToTaskOf<TResult>(this Task task);
}
```

## Extension Methods — Claims

```csharp
public static class ClaimExtensions
{
    static IEnumerable<Claim> GetClaimsByType(this IEnumerable<Claim> claims, string type);
    static Claim? GetFirstClaimByType(this IEnumerable<Claim> claims, string type);
    static string? GetFirstClaimValueByType(this IEnumerable<Claim> claims, string type);
    static IEnumerable<string> GetClaimValuesByType(this IEnumerable<Claim> claims, string type);
    static IEnumerable<Claim> RemoveClaimsByType(this IEnumerable<Claim> claims, string type);
}
```

## Extension Methods — Action

```csharp
public static class ActionExtensions
{
    static T InvokeAction<T>(this Action<T> action, T? instance = default);
    static (T1, T2) InvokeAction<T1, T2>(this Action<T1, T2> action,
        T1? firstInstance = default, T2? secondInstance = default);
    static (T1, T2, T3) InvokeAction<T1, T2, T3>(this Action<T1, T2, T3> action,
        T1? firstInstance = default, T2? secondInstance = default, T3? thirdInstance = default);
}
```

## Helpers

```csharp
public static class ArrayHelpers
{
    static T[] ConcatArrays<T>(params T[][] arrays);
    static T[] ConcatArrays<T>(T[] first, T[] second);
    static T[] SubArray<T>(T[] source, int startIndex, int length);
}

public static class Base58Helper
{
    static string Encode(byte[] data);
    static byte[] Decode(string encoded);
    static string EncodeWithCheckSum(byte[] data);
    static byte[] DecodeWithCheckSum(string encoded);
}

public static class WildcardHelper
{
    static string WildcardToRegex(string pattern);
    static bool Match(string input, string pattern, bool caseSensitive = true);
    static bool ContainsWildcard(string pattern);
}

public class Enum<T> where T : struct, Enum, IConvertible
{
    static T Find(string value, T ifNotFound = default);
    static T Find(string value, bool ignoreCase, T ifNotFound = default);
    static bool TryFind(string value, out T result);
    static bool TryFind(string value, bool ignoreCase, out T result);
}
```

## InvokeHelper (Invoke package)

```csharp
public static class InvokeHelper
{
    // Synchronous
    static void InvokeVoidMethod(object? instance, MethodInfo method, params object[] parameters);
    static T? InvokeMethod<T>(object? instance, MethodInfo method, params object[] parameters);
    static object InvokeMethod(object? instance, MethodInfo method, params object[] parameters);

    // Asynchronous
    static Task InvokeVoidMethodAsync(object? instance, MethodInfo method, params object[] parameters);
    static Task<T?> InvokeMethodAsync<T>(object? instance, MethodInfo method, params object[] parameters);
    static Task<object> InvokeMethodAsync(object? instance, MethodInfo method, params object[] parameters);

    // Generic methods (MethodInfo + type arguments)
    static void InvokeGenericVoidMethod(object instance, MethodInfo method,
        IEnumerable<Type> genericArguments, params object[] parameters);
    static void InvokeGenericVoidMethod<TArg>(object instance, MethodInfo method,
        params object[] parameters);
    static void InvokeGenericVoidMethod<TArg1, TArg2>(object instance, MethodInfo method,
        params object[] parameters);
    static void InvokeGenericVoidMethod<TArg1, TArg2, TArg3>(object instance, MethodInfo method,
        params object[] parameters);

    static object? InvokeGenericMethod(object instance, MethodInfo method,
        IEnumerable<Type> genericArguments, params object[] parameters);
    static TResult? InvokeGenericMethod<TResult>(object instance, MethodInfo method,
        IEnumerable<Type> genericArguments, params object[] parameters);
    static TResult? InvokeGenericMethod<TArg, TResult>(object instance, MethodInfo method,
        params object[] parameters);
    static TResult? InvokeGenericMethod<TArg1, TArg2, TResult>(object instance, MethodInfo method,
        params object[] parameters);
    static TResult? InvokeGenericMethod<TArg1, TArg2, TArg3, TResult>(object instance, MethodInfo method,
        params object[] parameters);

    // Async generic methods
    static Task InvokeGenericVoidMethodAsync(object instance, MethodInfo method,
        IEnumerable<Type> genericArguments, params object[] parameters);
    static Task<object> InvokeGenericMethodAsync(object instance, MethodInfo method,
        IEnumerable<Type> genericArguments, params object[] parameters);
    static Task<TResult?> InvokeGenericMethodAsync<TResult>(object instance, MethodInfo method,
        IEnumerable<Type> genericArguments, params object[] parameters);
    static Task<TResult?> InvokeGenericMethodAsync<TArg, TResult>(object instance, MethodInfo method,
        params object[] parameters);
    static Task<TResult?> InvokeGenericMethodAsync<TArg1, TArg2, TResult>(object instance, MethodInfo method,
        params object[] parameters);
    static Task<TResult?> InvokeGenericMethodAsync<TArg1, TArg2, TArg3, TResult>(object instance,
        MethodInfo method, params object[] parameters);
}
```

## ExpandableObject (ExpandableObject package)

```csharp
public class ExpandableObject : ExpandableBaseObject
{
    ExpandableObject();
    ExpandableObject(object? source);
    ExpandableObject(IDictionary dictionary);
    ExpandableObject(IDictionary<string, object?> dictionary);

    static implicit operator ExpandableObject(Dictionary<string, object?> dictionary);
}

public abstract class ExpandableBaseObject : DynamicObject, INotifyPropertyChanged
{
    T? GetValue<T>(string key);
    object? GetValue(string key);
    T? GetValueOrDefault<T>(string key, T? defaultValue = default);
    object? GetValueOrDefault(string key, object? defaultValue = default);
    IEnumerable<string> GetKeys();
    IEnumerable<object?> GetValues();
    IEnumerable<KeyValuePair<string, object?>> GetProperties();
    bool ContainsKey(string key);
    bool IsInstanceProperty(string key);

    event PropertyChangedEventHandler? PropertyChanged;
}
```
