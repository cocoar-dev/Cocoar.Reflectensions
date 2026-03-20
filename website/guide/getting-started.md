# Getting Started

## Installation

Install the core package via NuGet:

```bash
dotnet add package Cocoar.Reflectensions
```

For dynamic method invocation or expandable objects, add the specialized packages:

```bash
dotnet add package Cocoar.Reflectensions.Invoke
dotnet add package Cocoar.Reflectensions.ExpandableObject
```

::: tip
The core package has **zero external dependencies** and targets both .NET 8.0 and .NET Standard 2.0.
:::

## Quick Examples

### 1. Parse Type Names from Strings

Resolve complex generic types from human-readable strings:

```csharp
using Cocoar.Reflectensions;

// Parse generic types from strings
var type = TypeHelper.FindType("Dictionary<string, List<int>>");
// Returns: typeof(Dictionary<string, List<int>>)

// Works with CLR syntax too
var type2 = TypeHelper.FindType(
    "System.Collections.Generic.Dictionary`2[System.String, System.Int32]");

// Custom type mappings (e.g. TypeScript to C#)
var mapping = new Dictionary<string, string> { ["number"] = "double" };
var type3 = TypeHelper.FindType("List<number>", mapping);
// Returns: typeof(List<double>)
```

### 2. Query Reflection with Fluent Extensions

Filter methods, check type relationships, and inspect types using chainable extension methods:

```csharp
using Cocoar.Reflectensions;

// Filter methods fluently
var methods = typeof(Calculator).GetMethods()
    .WithName("Calculate")
    .WithReturnType<double>()
    .WithParametersOfType(typeof(int), typeof(int));

// Check type relationships
bool isDict    = type.IsGenericTypeOf<Dictionary<,>>();
bool inherits  = type.InheritFromClass<BaseClass>();
bool implements = type.ImplementsInterface<IDisposable>();
bool castable  = type.IsImplicitCastableTo<string>();
```

### 3. Convert Objects with Smart Type Conversion

Convert between types intelligently using the `Reflect()` entry point:

```csharp
using Cocoar.Reflectensions;

// Parse a date string
string dateStr = "2021-03-21T15:50:17+00:00";
DateTime date = dateStr.Reflect().To<DateTime>();

// With fallback value
object maybeNumber = "not a number";
int number = maybeNumber.Reflect().To<int>(42); // Returns 42

// Try pattern for safe conversion
if (value.Reflect().TryTo<DateTime>(out var result))
{
    Console.WriteLine($"Parsed: {result}");
}
```

## Next Steps

- [Why Reflectensions?](/guide/why-cocoar-reflectensions) — what problems this library solves
- [Type Parsing](/guide/type-parsing) — full type resolution reference
- [Type Extensions](/guide/type-extensions) — all type checking methods
- [Smart Type Conversion](/guide/type-conversion) — conversion strategies in detail
- [API Overview](/reference/api) — complete API reference
