# Expandable Objects

The `Cocoar.Reflectensions.ExpandableObject` package provides dynamic objects with dictionary-like behavior and property change notifications.

## Installation

```bash
dotnet add package Cocoar.Reflectensions.ExpandableObject
```

## Creating Expandable Objects

### Empty Object

```csharp
using Cocoar.Reflectensions;

var obj = new ExpandableObject();
```

### From an Existing Object

Copies all public properties:

```csharp
var person = new Person { Name = "Alice", Age = 30 };
var obj = new ExpandableObject(person);
```

### From a Dictionary

```csharp
var dict = new Dictionary<string, object?>
{
    ["Name"] = "Alice",
    ["Age"] = 30
};
var obj = new ExpandableObject(dict);
```

### Implicit Conversion from Dictionary

```csharp
ExpandableObject obj = new Dictionary<string, object?>
{
    ["Name"] = "Alice",
    ["Age"] = 30
};
```

## Dynamic Property Access

ExpandableObject inherits from `DynamicObject`, so you can use `dynamic` to access properties:

```csharp
dynamic obj = new ExpandableObject();
obj.Name = "Alice";
obj.Age = 30;

Console.WriteLine(obj.Name); // "Alice"
Console.WriteLine(obj.Age);  // 30
```

## Dictionary-Like Operations

### Reading Values

```csharp
var obj = new ExpandableObject(new { Name = "Alice", Age = 30 });

// Get value with type conversion (throws KeyNotFoundException if missing)
string name = obj.GetValue<string>("Name");
int age = obj.GetValue<int>("Age");

// Get with default (returns default if missing)
string email = obj.GetValueOrDefault<string>("Email", "none");
```

### Writing Values

Use `dynamic` access to set values:

```csharp
dynamic d = obj;
d.Name = "Bob";
d.NewProperty = "dynamic value";
```

### Querying

```csharp
// Check if property exists
bool has = obj.ContainsKey("Name"); // true

// Get all keys
IEnumerable<string> keys = obj.GetKeys();

// Get all values
IEnumerable<object?> values = obj.GetValues();

// Get all properties as key-value pairs
var properties = obj.GetProperties();
```

### Instance vs Dynamic Properties

Check whether a property comes from the original object type or was added dynamically:

```csharp
var person = new Person { Name = "Alice" };
var obj = new ExpandableObject(person);

dynamic d = obj;
d.DynamicProp = "value";

bool isInstance = obj.IsInstanceProperty("Name");       // true
bool isDynamic  = obj.IsInstanceProperty("DynamicProp"); // false
```

## Property Change Notifications

ExpandableObject implements `INotifyPropertyChanged`:

```csharp
var obj = new ExpandableObject();
obj.PropertyChanged += (sender, e) =>
{
    Console.WriteLine($"Property changed: {e.PropertyName}");
};

dynamic d = obj;
d.Name = "Alice"; // Prints: "Property changed: Name"
```

## IDictionary Support

ExpandableObject supports `IDictionary` for interoperability:

```csharp
var dict = new Hashtable { ["key"] = "value" };
var obj = new ExpandableObject(dict);
```
