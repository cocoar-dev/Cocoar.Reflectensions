# Packages

Cocoar.Reflectensions is split into focused packages. Install only what you need.

## Core {#core}

### Cocoar.Reflectensions

The main library with all core functionality.

```bash
dotnet add package Cocoar.Reflectensions
```

| | |
|---|---|
| **NuGet** | [Cocoar.Reflectensions](https://www.nuget.org/packages/Cocoar.Reflectensions) |
| **Targets** | .NET 8.0, .NET Standard 2.0 |
| **Dependencies** | None |

**Includes:**

- `TypeHelper` — type parsing and resolution from strings
- Type extensions — `IsNumericType()`, `IsGenericTypeOf()`, `ImplementsInterface()`, etc.
- Object reflection — `Reflect()`, `To<T>()`, `TryTo<T>()`, `As<T>()`, property access
- Method/property/parameter extensions — fluent LINQ-style filtering
- String extensions — validation, conversion, encoding
- Enum extensions — `GetName()`, `TryFind()`
- DateTime extensions — Unix time, UTC conversion
- Array/dictionary extensions — `Concat()`, `SubArray()`, `Merge()`
- Assembly extensions — embedded resource reading
- Helpers — `Base58Helper`, `WildcardHelper`, `Enum<T>`, `ArrayHelpers`

## Invoke {#invoke}

### Cocoar.Reflectensions.Invoke

Dynamic method invocation with automatic parameter matching.

```bash
dotnet add package Cocoar.Reflectensions.Invoke
```

| | |
|---|---|
| **NuGet** | [Cocoar.Reflectensions.Invoke](https://www.nuget.org/packages/Cocoar.Reflectensions.Invoke) |
| **Targets** | .NET 8.0, .NET Standard 2.0 |
| **Dependencies** | Cocoar.Reflectensions |

**Includes:**

- `InvokeHelper` — invoke methods via `MethodInfo`
- Synchronous and asynchronous invocation
- Generic method invocation with type arguments
- Automatic parameter type conversion

## Expandable Object {#expandable-object}

### Cocoar.Reflectensions.ExpandableObject

Dynamic objects with dictionary-like behavior.

```bash
dotnet add package Cocoar.Reflectensions.ExpandableObject
```

| | |
|---|---|
| **NuGet** | [Cocoar.Reflectensions.ExpandableObject](https://www.nuget.org/packages/Cocoar.Reflectensions.ExpandableObject) |
| **Targets** | .NET 8.0, .NET Standard 2.0 |
| **Dependencies** | Cocoar.Reflectensions |

**Includes:**

- `ExpandableObject` — dynamic object wrapper
- Initialize from objects, dictionaries, or empty
- `INotifyPropertyChanged` support
- Implicit conversion from `Dictionary<string, object?>`

## Which Package Do I Need?

| Use case | Package |
|----------|---------|
| Type parsing, type checking, type conversion | Core |
| Fluent method/property filtering | Core |
| String validation and conversion | Core |
| Dynamic method invocation via MethodInfo | Core + Invoke |
| Dynamic objects with dictionary behavior | Core + ExpandableObject |
| Everything | All three |

## Compatibility

All packages target:

- **.NET 8.0** — recommended for new projects
- **.NET Standard 2.0** — compatible with:
  - .NET Core 2.0+
  - .NET 5+
  - .NET Framework 4.6.1+
  - Mono 5.4+
  - Xamarin.iOS 10.14+
  - Xamarin.Android 8.0+
