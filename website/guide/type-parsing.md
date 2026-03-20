# Type Parsing

`TypeHelper` resolves .NET types from human-readable string representations. It handles generics, arrays, C# keywords, and custom type mappings — with thread-safe caching for performance.

## FindType

Resolves a type name string to a `System.Type`:

```csharp
using Cocoar.Reflectensions;

// C# keywords
var stringType = TypeHelper.FindType("string");    // typeof(string)
var intType    = TypeHelper.FindType("int");        // typeof(int)
var dynamicType = TypeHelper.FindType("dynamic");   // typeof(object)

// Fully qualified names
var dateType = TypeHelper.FindType("System.DateTime");

// Generic types
var listType = TypeHelper.FindType("List<string>");
var dictType = TypeHelper.FindType("Dictionary<string, int>");

// Nested generics
var complex = TypeHelper.FindType(
    "Dictionary<string, List<Tuple<int, string>>>");

// CLR backtick syntax
var clrType = TypeHelper.FindType(
    "System.Collections.Generic.Dictionary`2[System.String, System.Int32]");

// Arrays
var arrayType   = TypeHelper.FindType("int[]");
var jaggedType  = TypeHelper.FindType("int[][]");
```

Returns `null` if the type cannot be found in any loaded assembly.

## Custom Type Mappings

Map external type systems (e.g. TypeScript) to C# types:

```csharp
var mapping = new Dictionary<string, string>
{
    ["number"]  = "double",
    ["boolean"] = "bool",
    ["any"]     = "object",
    ["Date"]    = "System.DateTime"
};

var type = TypeHelper.FindType("Dictionary<string, number>", mapping);
// Resolves to: typeof(Dictionary<string, double>)
```

This is useful for code generation, plugin systems, or any scenario where type names come from a different language or configuration format.

## NormalizeTypeName

Converts a type name string to the CLR-compatible format without resolving it:

```csharp
var input  = "Dictionary<string, List<int>>";
var output = TypeHelper.NormalizeTypeName(input);
// "System.Collections.Generic.Dictionary`2[System.String, System.Collections.Generic.List`1[System.Int32]]"
```

This is the first step of `FindType`. Use it when you need the normalized string but not the actual `Type` object.

```csharp
// With custom mapping
var mapping = new Dictionary<string, string> { ["number"] = "double" };
var normalized = TypeHelper.NormalizeTypeName("List<number>", mapping);
// "System.Collections.Generic.List`1[System.Double]"
```

## Supported Formats

| Format | Example |
|--------|---------|
| C# keywords | `string`, `int`, `double`, `bool`, `dynamic`, `object` |
| Simple names | `Guid`, `DateTime`, `TimeSpan` |
| Fully qualified | `System.Collections.Generic.List\`1` |
| Generic (angle brackets) | `List<string>`, `Dictionary<string, int>` |
| Generic (CLR backtick) | `Dictionary\`2[System.String, System.Int32]` |
| Nested generics | `Dictionary<string, List<Tuple<int, string>>>` |
| Arrays | `int[]`, `int[][]`, `System.Object[][][]` |
| Mixed formats | `Dictionary<string, System.Collections.Generic.List\`1[System.Int32]>` |

::: info Namespace Inference
If a type name has no namespace (e.g. `Guid[]`), TypeHelper assumes it is in the `System` namespace.
:::

## Caching

Type lookups are cached with thread-safe locking. The first call resolves and caches the type; subsequent calls return instantly.

```csharp
// First call: resolves + caches
var type1 = TypeHelper.FindType("Dictionary<string, int>");

// Second call: cache hit (~0ns overhead)
var type2 = TypeHelper.FindType("Dictionary<string, int>");
```

## HasInspectableBaseType

Checks whether a type has a base type worth inspecting (i.e., not `object`, `ValueType`, or `Enum`):

```csharp
TypeHelper.HasInspectableBaseType(typeof(MyDerivedClass)); // true
TypeHelper.HasInspectableBaseType(typeof(object));          // false
TypeHelper.HasInspectableBaseType(typeof(int));             // false (ValueType)
```
