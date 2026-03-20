# Reflection Wrapper

Reflectensions avoids polluting `object` with extension methods. Instead, all object-level operations go through a lightweight wrapper accessed via `.Reflect()`.

## The Reflect() Entry Point

Call `.Reflect()` on any object to access reflection extensions:

```csharp
using Cocoar.Reflectensions;

var value = "hello";
var reflected = value.Reflect();

// Now use reflection extensions
DateTime date = "2021-03-21".Reflect().To<DateTime>();
int number = 42.Reflect().To<string>().Length;
```

There is also a generic version that preserves type information:

```csharp
// Generic — avoids boxing for value types
int n = 42;
var reflected = n.Reflect();  // ObjectReflection<int>
```

## Zero-Cost Abstraction

Both `ObjectReflection` and `ObjectReflection<T>` are **readonly structs**. They live on the stack with no heap allocations:

```csharp
// No heap allocation — ObjectReflection<int> is a struct
var reflected = 42.Reflect();
string s = reflected.To<string>(); // "42"
```

The generic variant `ObjectReflection<T>` avoids boxing entirely for value types. When you call `.Reflect()` on an `int`, the value stays unboxed throughout the conversion pipeline.

## ObjectReflection vs ObjectReflection\<T\>

| | `ObjectReflection` | `ObjectReflection<T>` |
|---|---|---|
| Created by | `.Reflect()` on `object` | `.Reflect()` on typed values |
| Boxing | Value is boxed | No boxing for value types |
| Use case | Runtime-typed values | Compile-time typed values |
| Both are | readonly struct | readonly struct |

`ObjectReflection<T>` implicitly converts to `ObjectReflection`, so all extension methods work with both:

```csharp
// Generic wrapper — no boxing
ObjectReflection<int> generic = 42.Reflect();

// Implicit conversion to non-generic
ObjectReflection nonGeneric = generic;
```

## Property Access

Read and write properties by name:

```csharp
var person = new Person { Name = "Alice", Age = 30 };

// Get property value
string name = person.Reflect().GetPropertyValue<string>("Name");

// Set property value
person.Reflect().SetPropertyValue("Age", 31);
```

`GetPropertyValue` supports custom `BindingFlags`:

```csharp
var value = obj.Reflect().GetPropertyValue<string>(
    "InternalProp",
    BindingFlags.NonPublic | BindingFlags.Instance);
```

`SetPropertyValue` supports nested paths — it navigates through object properties to set deeply nested values.

## Equality Helpers

### EqualsToAnyOf

Check if a value equals any of the provided values:

```csharp
string status = "active";
bool match = status.Reflect().EqualsToAnyOf("active", "enabled", "on");
// true
```

### ToBoolean

Convert a value to `bool` using custom true-value definitions:

```csharp
object value = "yes";
bool result = value.Reflect().ToBoolean("yes", "true", "1", "on");
// true
```

## Implicit Cast Checking

Check if a value's runtime type can be implicitly cast to a target type:

```csharp
object value = 42;
bool canCast = value.Reflect().IsImplicitCastableTo<long>();   // true
bool canCast2 = value.Reflect().IsImplicitCastableTo<string>(); // false
```
