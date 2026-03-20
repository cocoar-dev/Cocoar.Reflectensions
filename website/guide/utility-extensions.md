# Utility Extensions

The core package includes extensions for enums, DateTime, arrays, dictionaries, assemblies, and tasks.

## Enum Extensions

### GetName

Get the display name of an enum value. Uses `[EnumMember]` or `[Description]` attributes if present, otherwise falls back to the member name:

```csharp
public enum Status
{
    [EnumMember(Value = "in_progress")]
    InProgress,

    [Description("Completed successfully")]
    Done,

    Pending
}

Status.InProgress.GetName(); // "in_progress"
Status.Done.GetName();       // "Completed successfully"
Status.Pending.GetName();    // "Pending"
```

### TryFind

Find an enum value from a string, checking member names and attribute values:

```csharp
// Non-generic — accepts Type parameter, returns object
if (EnumExtensions.TryFind(typeof(Status), "in_progress", out var status))
{
    var s = (Status)status!; // s == Status.InProgress
}

// Case-insensitive
EnumExtensions.TryFind(typeof(Status), "PENDING", ignoreCase: true, out var s2);
```

### Generic Enum Helper

For a cleaner generic API, use `Enum<T>`:

```csharp
using Cocoar.Reflectensions.Helper;

// Find enum value from string (returns default if not found)
Status status = Enum<Status>.Find("InProgress");

// Try find
if (Enum<Status>.TryFind("Done", out var result))
{
    // use result
}

// Case-insensitive
if (Enum<Status>.TryFind("done", ignoreCase: true, out var result2))
{
    // use result2
}
```

## DateTime Extensions

### Unix Time Conversion

```csharp
DateTime now = DateTime.UtcNow;

// To Unix time
long unixMs = now.ToUnixTimeMilliseconds();

// From Unix time
DateTime fromMs = DateTimeExtensions.FromUnixTimeMilliseconds(unixMs);
DateTime fromSec = DateTimeExtensions.FromUnixTimeSeconds(1616345417);
```

### Kind Management

```csharp
DateTime dt = DateTime.Now;

DateTime utc   = dt.ToUtc();                    // Converts to UTC
DateTime local = dt.ToLocal();                   // Converts to local
DateTime withKind = dt.SetKind(DateTimeKind.Utc); // Sets kind without conversion
DateTime asUtc   = dt.SetIsUtc();                // Sets kind to UTC
DateTime asLocal = dt.SetIsLocal();              // Sets kind to Local
```

## Array Extensions

### Concat

Concatenate two arrays:

```csharp
int[] a = [1, 2, 3];
int[] b = [4, 5, 6];
int[] combined = a.Concat(b); // [1, 2, 3, 4, 5, 6]
```

### SubArray

Extract a portion of an array:

```csharp
int[] arr = [1, 2, 3, 4, 5];

int[] sub1 = arr.SubArray(1, 3);  // [2, 3, 4] (start index, length)
int[] sub2 = arr.SubArray(2);     // [3, 4, 5] (from index to end)
```

### ArrayHelpers

Static helpers for array operations:

```csharp
using Cocoar.Reflectensions.Helper;

int[] combined = ArrayHelpers.ConcatArrays(array1, array2);
int[] sub = ArrayHelpers.SubArray(source, startIndex, length);
```

## Dictionary Extensions

### Merge

Merge two dictionaries, with the second overwriting duplicates:

```csharp
var dict1 = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
var dict2 = new Dictionary<string, int> { ["b"] = 3, ["c"] = 4 };

var merged = dict1.Merge(dict2);
// { "a": 1, "b": 3, "c": 4 }
```

### Typed Value Access

Extensions on `IDictionary<string, object>` for type-safe value retrieval:

```csharp
var data = new Dictionary<string, object>
{
    ["count"] = 42,
    ["name"] = "Alice"
};

// GetValueAs — uses As<T> (direct cast)
int count = data.GetValueAs<int>("count");
string name = data.GetValueAs<string>("name", "default");

// TryGetValueAs — safe variant
if (data.TryGetValueAs<int>("count", out var n))
{
    // n == 42
}

// GetValueTo — uses To<T> (smart conversion)
string countStr = data.GetValueTo<string>("count"); // "42"

// TryGetValueTo — safe variant
if (data.TryGetValueTo<string>("count", out var s))
{
    // s == "42"
}
```

::: tip
`GetValueAs` uses direct casting (`As<T>`), while `GetValueTo` uses smart conversion (`To<T>`) with implicit operators and Parse methods. Use `As` when the types match, `To` when conversion is needed.
:::

## Assembly Extensions

Read embedded resources from assemblies:

```csharp
using Cocoar.Reflectensions;

var assembly = typeof(MyClass).Assembly;

// Read as stream
Stream? stream = assembly.ReadResourceAsStream("MyNamespace.data.json");

// Read as string
string? content = assembly.ReadResourceAsString("MyNamespace.template.html");

// Read as byte array
byte[]? bytes = assembly.ReadResourceAsByteArray("MyNamespace.image.png");
```

## Task Extensions

### CastToTaskOf

Convert a `Task` to `Task<T>` using reflection (useful when the runtime type is known but the compile-time type is not):

```csharp
Task untypedTask = GetSomeTask(); // actually returns Task<string>
Task<string> typedTask = untypedTask.CastToTaskOf<string>();
string result = await typedTask;
```

## Wildcard Helper

Pattern matching with wildcard syntax:

```csharp
using Cocoar.Reflectensions.Helper;

// Convert wildcard to regex
string regex = WildcardHelper.WildcardToRegex("*.txt");

// Match with wildcards
bool match = WildcardHelper.Match("hello.txt", "*.txt"); // true

// Check if string contains wildcards
bool hasWild = WildcardHelper.ContainsWildcard("test*"); // true
```

Supported wildcards: `*` (any characters), `?` (single character), `+` (one or more characters).

## Base58 Helper

Encode and decode byte arrays in Base58 format:

```csharp
using Cocoar.Reflectensions.Helper;

byte[] data = Encoding.UTF8.GetBytes("Hello");

// Basic encode/decode
string encoded = Base58Helper.Encode(data);
byte[] decoded = Base58Helper.Decode(encoded);

// With SHA256 checksum
string withCheck = Base58Helper.EncodeWithCheckSum(data);
byte[] verified  = Base58Helper.DecodeWithCheckSum(withCheck);
```

## Action Extensions

Extension methods on `Action<T>` that auto-instantiate parameters:

```csharp
using Cocoar.Reflectensions;

// Creates a new MyConfig instance, passes it to the action, returns it
Action<MyConfig> configure = config =>
{
    config.Setting = "value";
};
MyConfig result = configure.InvokeAction();

// With multiple parameters
Action<MyConfig, MyOptions> setup = (config, options) =>
{
    config.Name = "App";
    options.Timeout = 30;
};
var (config, options) = setup.InvokeAction();
```
