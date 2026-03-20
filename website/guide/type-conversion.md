# Smart Type Conversion

Reflectensions provides intelligent type conversion through the `Reflect()` entry point. It automatically tries multiple conversion strategies in order, with optimized fast paths for common type pairs.

## Basic Conversion

### To\<T\>()

Converts a value to the target type, throwing if conversion fails:

```csharp
string dateStr = "2021-03-21T15:50:17+00:00";
DateTime date = dateStr.Reflect().To<DateTime>();

string numberStr = "42";
int number = numberStr.Reflect().To<int>();

int n = 123;
string s = n.Reflect().To<string>(); // "123"
```

### To\<T\>(T defaultValue)

Returns the default value if conversion fails:

```csharp
object value = "not a number";
int number = value.Reflect().To<int>(42); // 42

object maybeDate = "invalid";
DateTime date = maybeDate.Reflect().To<DateTime>(DateTime.MinValue);
```

### TryTo\<T\>()

The Try-pattern for safe conversion without exceptions:

```csharp
if (value.Reflect().TryTo<DateTime>(out var result))
{
    Console.WriteLine($"Parsed: {result}");
}
else
{
    Console.WriteLine("Conversion failed");
}
```

## Casting vs Converting

Reflectensions distinguishes between **casting** (`As<T>`) and **converting** (`To<T>`):

### As\<T\>() — Type Switching

`As<T>` attempts to switch the type without data transformation. It tries direct casting and reference conversion:

```csharp
object obj = "hello";
string? s = obj.Reflect().As<string>(); // "hello" — direct cast

object num = 42;
long? l = num.Reflect().As<long>(); // null — int is not long
```

### TryAs\<T\>()

```csharp
if (obj.Reflect().TryAs<string>(out var str))
{
    // Direct cast succeeded
}
```

### To\<T\>() — Smart Conversion

`To<T>` uses a multi-strategy conversion pipeline. In order, it tries:

1. **Bool special case** — if the target is `bool`, uses the `ToBoolean()` logic directly
2. **Direct cast** via `As<T>` (type identity, inheritance, interface)
3. **Nullable unwrapping** — if the target is `Nullable<T>`, converts to the underlying type
4. **Guid special case** — `Guid.TryParse` for string-to-Guid conversion
5. **Numeric conversion** — `Convert.ChangeType` when the target is a numeric type
6. **IConvertible** — `Convert.ChangeType` when both source and target implement `IConvertible`
7. **Implicit operator detection** — finds and invokes `op_Implicit` on source or target type

```csharp
// Uses int.Parse() internally
int n = "42".Reflect().To<int>();

// Uses Guid.TryParse() internally
Guid id = "550e8400-e29b-41d4-a716-446655440000".Reflect().To<Guid>();

// Uses ToString() internally
string s = 42.Reflect().To<string>();
```

## Automatic Implicit Operator Conversion

This is where Reflectensions really shines. When you define `implicit operator` on your types, `To<T>()` **discovers and uses them automatically** — no extra code needed.

### How It Works

When all other conversion strategies fail, `To<T>()` searches for an `op_Implicit` method on **both** the source type and the target type. This means it works regardless of which side defines the operator:

```csharp
public class Celsius
{
    public double Value { get; }
    public Celsius(double value) => Value = value;

    // Define implicit conversion TO Fahrenheit
    public static implicit operator Fahrenheit(Celsius c)
        => new(c.Value * 9.0 / 5.0 + 32);
}

public class Fahrenheit
{
    public double Value { get; }
    public Fahrenheit(double value) => Value = value;
}

// Reflectensions finds and uses the implicit operator automatically
var celsius = new Celsius(100);
Fahrenheit f = celsius.Reflect().To<Fahrenheit>();
// f.Value == 212
```

### Bidirectional Discovery

The key insight: `GetImplicitCastMethodTo` searches on **both** the source and target types. If the operator is defined on the target type (accepting the source type as parameter), it works just as well:

```csharp
public class Camaro { }

public class Truck : Car
{
    // Implicit conversion defined on the TARGET type
    public static implicit operator Truck(Camaro camaro)
        => new Truck();
}

// Works — found on Truck, even though we start from Camaro
var camaro = new Camaro();
Truck truck = camaro.Reflect().To<Truck>();
```

### Checking Castability Before Converting

You can check whether an implicit conversion exists without performing it:

```csharp
// On types
bool canConvert = typeof(Camaro).IsImplicitCastableTo<Truck>();    // true
bool canReverse = typeof(Truck).IsImplicitCastableTo<Camaro>();     // false

// On values
bool canConvert2 = camaro.Reflect().IsImplicitCastableTo<Truck>(); // true
```

### Discovering Operator Methods

Inspect all implicit or explicit operators defined on a type:

```csharp
// List all implicit operators
var implicits = typeof(Truck).GetImplicitOperatorMethods();
foreach (var method in implicits)
{
    var param = method.GetParameters()[0].ParameterType;
    Console.WriteLine($"{param.Name} -> {method.ReturnType.Name}");
}
// Output: "Camaro -> Truck"

// List all explicit operators
var explicits = typeof(MyType).GetExplicitOperatorMethods();

// Find a specific conversion
MethodInfo? cast = typeof(Camaro).GetImplicitCastMethodTo<Truck>();
MethodInfo? explicit_ = typeof(MyType).GetExplicitCastMethodTo<string>();
```

### Numeric Widening

For numeric types, Reflectensions maintains a built-in [implicit conversion table](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/numeric-conversions) that mirrors C# language rules. This means `IsImplicitCastableTo` works correctly for all numeric widening conversions without needing to search for `op_Implicit`:

```csharp
typeof(int).IsImplicitCastableTo<long>();      // true (widening)
typeof(int).IsImplicitCastableTo<double>();     // true (widening)
typeof(int).IsImplicitCastableTo<decimal>();    // true (widening)
typeof(long).IsImplicitCastableTo<int>();       // false (narrowing)
typeof(float).IsImplicitCastableTo<double>();   // true
typeof(double).IsImplicitCastableTo<float>();   // false
typeof(byte).IsImplicitCastableTo<int>();       // true
typeof(char).IsImplicitCastableTo<int>();       // true
```

::: info
The conversion pipeline order matters. Direct casts and `IConvertible` are tried before implicit operators, so operator conversion only kicks in when standard .NET mechanisms don't apply — exactly the right fallback for your custom domain types.
:::

## Non-Generic Overloads

All conversion methods have non-generic overloads that accept a `Type` parameter:

```csharp
Type targetType = typeof(DateTime);

object? result = value.Reflect().To(targetType);
object? result2 = value.Reflect().To(targetType, defaultValue);
bool success = value.Reflect().TryTo(targetType, out object? result3);
```

## Performance: Hot Paths

Common conversions are optimized with dedicated fast paths that bypass the generic pipeline:

| Conversion | Performance | Mechanism |
|-----------|-------------|-----------|
| `int` to `string` | ~80ns | Direct `ToString()` |
| `string` to `int` | ~118ns | Direct `int.Parse()` |
| `long` to `string` | ~80ns | Direct `ToString()` |
| `string` to `long` | ~118ns | Direct `long.Parse()` |
| `double` to `string` | ~80ns | Direct `ToString()` |
| `string` to `double` | ~118ns | Direct `double.Parse()` |
| `bool` to `string` | ~80ns | Direct `ToString()` |

These fast paths are selected automatically based on the source and target types. All other conversions fall back to the generic pipeline.

::: tip
The `int.Reflect().To<string>()` hot path is **8x faster** than the generic fallback path.
:::

## Real-World Patterns

### Configuration Parsing

```csharp
public T GetConfigValue<T>(Dictionary<string, object> config, string key, T fallback)
{
    if (!config.TryGetValue(key, out var value))
        return fallback;

    return value.Reflect().TryTo<T>(out var result) ? result : fallback;
}
```

### Loose Data Normalization

```csharp
// Convert loosely-typed API responses
var response = new Dictionary<string, object>
{
    ["count"] = "42",
    ["enabled"] = "true",
    ["timestamp"] = "2021-03-21T15:50:17Z"
};

int count = response["count"].Reflect().To<int>();
bool enabled = response["enabled"].Reflect().To<bool>();
DateTime ts = response["timestamp"].Reflect().To<DateTime>();
```
