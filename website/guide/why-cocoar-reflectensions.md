# Why Cocoar.Reflectensions?

.NET reflection is powerful, but the raw APIs are verbose and error-prone. Reflectensions makes reflection code **readable**, **maintainable**, and **efficient**.

## The Problem

Consider finding all methods named `Process` that return `Task` and accept a `string` parameter:

**Raw .NET Reflection:**

```csharp
var methods = typeof(MyService).GetMethods(BindingFlags.Public | BindingFlags.Instance)
    .Where(m => m.Name == "Process")
    .Where(m => m.ReturnType == typeof(Task)
                || (m.ReturnType.IsGenericType
                    && m.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)))
    .Where(m => {
        var parameters = m.GetParameters();
        return parameters.Length == 1 && parameters[0].ParameterType == typeof(string);
    })
    .ToList();
```

**With Reflectensions:**

```csharp
var methods = typeof(MyService).GetMethods()
    .WithName("Process")
    .WithReturnType<Task>()
    .WithParametersOfType(typeof(string));
```

Same result. One-third the code. Self-documenting.

## Type Resolution from Strings

Resolving generic type names from configuration or user input requires significant boilerplate with raw .NET:

**Raw .NET:**

```csharp
// Good luck parsing "Dictionary<string, List<int>>" by hand
var typeName = "System.Collections.Generic.Dictionary`2[" +
    "[System.String, System.Private.CoreLib]," +
    "[System.Collections.Generic.List`1[" +
        "[System.Int32, System.Private.CoreLib]" +
    "], System.Private.CoreLib]" +
    "], System.Private.CoreLib]";
var type = Type.GetType(typeName);
```

**With Reflectensions:**

```csharp
var type = TypeHelper.FindType("Dictionary<string, List<int>>");
```

Reflectensions normalizes human-readable type names to CLR-compatible format, resolves them across loaded assemblies, and caches the result for performance.

## Type Conversion

Converting between types in .NET involves a cascade of checks and fallbacks:

**Raw .NET:**

```csharp
object value = "42";
int result;

if (value is int i)
    result = i;
else if (value is string s && int.TryParse(s, out var parsed))
    result = parsed;
else if (value is IConvertible convertible)
    result = convertible.ToInt32(null);
else
    result = defaultValue;
```

**With Reflectensions:**

```csharp
int result = value.Reflect().To<int>(defaultValue);
```

The library automatically tries direct casting, implicit operators, `Parse`/`TryParse`, and registered conversion extensions — with fast paths for common type pairs.

## Type Checking

Checking type relationships often requires nested conditions:

**Raw .NET:**

```csharp
bool isNumeric = type == typeof(int) || type == typeof(long)
    || type == typeof(float) || type == typeof(double)
    || type == typeof(decimal) || type == typeof(byte)
    || type == typeof(short) || type == typeof(uint)
    || type == typeof(ulong) || type == typeof(ushort)
    || type == typeof(sbyte);

bool isDictionary = type.GetInterfaces()
    .Any(i => i.IsGenericType
        && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
```

**With Reflectensions:**

```csharp
bool isNumeric    = type.IsNumericType();
bool isDictionary = type.IsDictionaryType();
```

## Implicit Operator Conversion — Automatically

This is one of the most powerful features. When your types define `implicit operator`, Reflectensions **finds and uses them automatically** during conversion — no configuration, no registration, no manual lookup.

**Raw .NET:**

```csharp
// You have to know the operator exists, find it, and invoke it
var method = typeof(Truck).GetMethods(BindingFlags.Public | BindingFlags.Static)
    .FirstOrDefault(m => m.Name == "op_Implicit"
        && m.ReturnType == typeof(Truck)
        && m.GetParameters().Length == 1
        && m.GetParameters()[0].ParameterType == typeof(Camaro));

Truck? truck = method != null
    ? (Truck)method.Invoke(null, new object[] { camaro })!
    : null;
```

**With Reflectensions:**

```csharp
Truck truck = camaro.Reflect().To<Truck>();
```

One line. Reflectensions searches for `op_Implicit` on **both** the source and target types, so it works regardless of which side defines the operator. Combined with the numeric widening table and `IConvertible` fallbacks, `To<T>()` handles virtually any conversion scenario you throw at it.

See the [Smart Type Conversion](/guide/type-conversion#automatic-implicit-operator-conversion) guide for full details.

## What You Get

| Feature | Raw .NET | Reflectensions |
|---------|----------|----------------|
| Type parsing from strings | Manual CLR syntax | `TypeHelper.FindType("Dict<string, int>")` |
| Method filtering | Nested LINQ with GetParameters | `.WithName().WithReturnType<T>()` |
| Type checking | Long boolean chains | `.IsNumericType()`, `.IsDictionaryType()` |
| Object conversion | Try/catch cascades | `.Reflect().To<T>(fallback)` |
| Implicit operator conversion | Manual op_Implicit lookup + invoke | `.Reflect().To<T>()` — automatic |
| Operator discovery | Search op_Implicit manually | `.GetImplicitCastMethodTo<T>()` |
| Property access by name | GetProperty + GetValue + null checks | `.Reflect().GetPropertyValue<T>("Name")` |

## Zero Dependencies

Reflectensions has no external NuGet dependencies. The core library is entirely self-contained, built on .NET 8.0 and .NET Standard 2.0 for maximum compatibility.
