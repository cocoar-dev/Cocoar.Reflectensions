# String Extensions

The core package includes a rich set of string extension methods for validation, conversion, and manipulation.

## Validation

Check string content type without parsing:

```csharp
"42".IsNumeric();         // true
"42".IsInt();             // true
"42".IsLong();            // true
"3.14".IsDouble();        // true
"true".IsBoolean();       // true
"2021-03-21".IsDateTime();// true

"192.168.1.1".IsValidIp();           // true
"test@example.com".IsValidEmailAddress(); // true
"SGVsbG8=".IsBase64Encoded();        // true

"550e8400-e29b-41d4-a716-446655440000".IsGuid(); // true

"hello".IsLowerCase();    // true
"HELLO".IsUpperCase();    // true
```

## Type Conversion

Convert strings directly to typed values:

```csharp
int n       = "42".ToInt();
long l      = "42".ToLong();
double d    = "3.14".ToDouble();
float f     = "3.14".ToFloat();
decimal dec = "3.14".ToDecimal();
bool b      = "true".ToBoolean();
DateTime dt = "2021-03-21".ToDateTime();
Guid id     = "550e8400-e29b-41d4-a716-446655440000".ToGuid();
```

### Nullable Variants

Return `null` instead of throwing on invalid input:

```csharp
int? n       = "not a number".ToNullableInt();       // null
long? l      = "not a number".ToNullableLong();      // null
double? d    = "invalid".ToNullableDouble();          // null
float? f     = "invalid".ToNullableFloat();           // null
decimal? dec = "invalid".ToNullableDecimal();          // null
DateTime? dt = "invalid".ToNullableDateTime();         // null
```

## String Manipulation

### Trimming

```csharp
"  hello  ".Trim();            // "hello" (standard)
"  hello  ".TrimToNull();      // "hello"
"   ".TrimToNull();            // null (whitespace-only becomes null)
"".TrimToNull();               // null
```

### Null / Empty Checks

```csharp
string? s = null;
s.IsNullOrEmpty();       // true
s.IsNullOrWhiteSpace();  // true
"".IsNullOrEmpty();      // true
" ".IsNullOrWhiteSpace();// true
```

### ToNull

Converts empty or whitespace strings to `null`:

```csharp
"".ToNull();       // null
"   ".ToNull();    // null
"hello".ToNull();  // "hello"
```

### RemoveEmptyLines

Removes empty lines from multi-line strings:

```csharp
"line1\n\nline2\n\nline3".RemoveEmptyLines();
// "line1\nline2\nline3"
```

### RemoveEnd

Removes a suffix from a string:

```csharp
"HelloWorld".RemoveEnd("World");  // "Hello"
```

## Wildcard Matching

Match strings against wildcard patterns (`*`, `?`, `+`):

```csharp
"hello world".Match("hello*");                      // true (case-insensitive by default)
"test.txt".Match("*.txt");                           // true
"abc".Match("a?c");                                  // true
"Hello".Match("hello", ignoreCase: false);           // false
"test.txt".Match("*.txt", invert: true);             // false (inverts result)
```

## Encoding

### Base64

```csharp
string encoded = "Hello".EncodeToBase64();     // "SGVsbG8="
string decoded = "SGVsbG8=".DecodeFromBase64(); // "Hello"
```

### Base58

```csharp
string encoded = "Hello".EncodeToBase58();
string decoded = encoded.DecodeFromBase58();
```

## Splitting

Split with cleaner syntax:

```csharp
"a,b,,c".Split(",");  // ["a", "b", "", "c"]
```
