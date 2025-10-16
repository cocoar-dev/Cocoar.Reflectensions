# Cocoar.Reflectensions Quick Start Guide

Get up and running with Cocoar.Reflectensions in minutes!

## 🚀 Installation

```bash
# Core library (type helpers and reflection)
dotnet add package Cocoar.Reflectensions

# Common extensions (string, enum, array helpers)
dotnet add package Cocoar.Reflectensions.CommonExtensions

# Method invocation helpers
dotnet add package Cocoar.Reflectensions.Invoke

# JSON utilities (Newtonsoft.Json)
dotnet add package Cocoar.Reflectensions.Json

# Dynamic/expandable objects
dotnet add package Cocoar.Reflectensions.ExpandableObject
```

## 💡 5-Minute Tutorial

### 1. Parse Complex Type Names

```csharp
using Cocoar.Reflectensions;

// Parse a complex generic type from a string
var type = TypeHelper.FindType("Dictionary<string, List<int>>");
// Returns: typeof(Dictionary<string, List<int>>)

// Works with custom mappings too
var mapping = new Dictionary<string, string> { ["number"] = "double" };
var type2 = TypeHelper.FindType("List<number>", mapping);
// Returns: typeof(List<double>)
```

### 2. Fluent Reflection Queries

```csharp
using Cocoar.Reflectensions;

// Find methods fluently
var calculateMethods = typeof(Calculator)
    .GetMethods()
    .WithName("Calculate")
    .WithReturnType<double>()
    .WithParametersOfType(typeof(int), typeof(int));

// Check type relationships
var isDict = type.IsGenericTypeOf<Dictionary<,>>();
var inherits = type.InheritFromClass<BaseClass>();
var implements = type.ImplementsInterface<IDisposable>();
```

### 3. Smart Object Conversion

```csharp
using doob.Reflectensions;

// Convert objects intelligently
string dateStr = "2021-03-21T15:50:17+00:00";
DateTime date = dateStr.Reflect().To<DateTime>();

// With fallback values
object maybeNumber = "not a number";
int number = maybeNumber.Reflect().To<int>(42); // Returns 42

// Try pattern
if (value.Reflect().TryTo<DateTime>(out var result))
{
    Console.WriteLine($"Date: {result}");
}
```

### 4. Dynamic Method Invocation

```csharp
using doob.Reflectensions;

// Invoke methods by name
var calculator = new Calculator();
var result = InvokeHelper.InvokeMethod(calculator, "Add", 10, 20);
// Automatically finds and calls the Add method

// Works with type conversion
var result2 = InvokeHelper.InvokeMethod(calculator, "Add", "10", "20");
// Converts strings to ints automatically
```

### 5. Property Access

```csharp
using doob.Reflectensions;

var person = new Person { Name = "John", Age = 30 };

// Get property value
var name = person.Reflect().GetPropertyValue<string>("Name");

// Set property value
person.Reflect().SetPropertyValue("Age", 31);
```

## 🎯 Common Patterns

### Pattern 1: Type Discovery from Configuration

```csharp
// Configuration-driven type loading
public class PluginLoader
{
    public IPlugin LoadPlugin(string typeName)
    {
        var type = TypeHelper.FindType(typeName);
        if (type == null || !type.ImplementsInterface<IPlugin>())
        {
            throw new InvalidOperationException($"Invalid plugin type: {typeName}");
        }
        
        return (IPlugin)Activator.CreateInstance(type);
    }
}
```

### Pattern 2: Dynamic Method Discovery

```csharp
// Find and invoke methods based on attributes
public class CommandExecutor
{
    public void ExecuteCommands(object target)
    {
        var methods = target.GetType()
            .GetMethods()
            .WithAttribute<CommandAttribute>()
            .WithParametersOfType();
        
        foreach (var method in methods)
        {
            method.Invoke(target, null);
        }
    }
}
```

### Pattern 3: Type-Safe Configuration

```csharp
// Parse type names from configuration
public class ServiceFactory
{
    public T Create<T>(string typeName) where T : class
    {
        var type = TypeHelper.FindType(typeName);
        
        if (!type.ImplementsInterface<T>())
        {
            throw new ArgumentException($"{typeName} doesn't implement {typeof(T).Name}");
        }
        
        return (T)Activator.CreateInstance(type);
    }
}
```

### Pattern 4: Smart Type Conversion

```csharp
// Convert loosely-typed data to strongly-typed objects
public class DataConverter
{
    public T Convert<T>(Dictionary<string, object> data, string key, T defaultValue = default)
    {
        if (!data.TryGetValue(key, out var value))
        {
            return defaultValue;
        }
        
        if (value.Reflect().TryTo<T>(out var result))
        {
            return result;
        }
        
        return defaultValue;
    }
}
```

## 📚 Next Steps

- 📖 Read the full [README](README.md) for complete API reference
- 🔍 Explore [REFLECTENSIONS_VALUE_ANALYSIS.md](REFLECTENSIONS_VALUE_ANALYSIS.md) to understand the library's design
- 📝 Check [CHANGELOG.md](CHANGELOG.md) for version history
- 🐛 Report issues on [GitHub](https://github.com/doob-at/Reflectensions/issues)

## 💡 Tips & Tricks

### Tip 1: Performance
Cache type lookups when using `TypeHelper.FindType()` repeatedly:
```csharp
private static readonly ConcurrentDictionary<string, Type> _typeCache = new();

public Type GetType(string name)
{
    return _typeCache.GetOrAdd(name, TypeHelper.FindType);
}
```

### Tip 2: Custom Type Mappings
Create reusable type mappings for your domain:
```csharp
private static readonly Dictionary<string, string> TypeScriptToC# = new()
{
    ["string"] = "System.String",
    ["number"] = "System.Double",
    ["boolean"] = "System.Boolean",
    ["any"] = "System.Object",
    ["Date"] = "System.DateTime"
};
```

### Tip 3: Method Filtering
Combine filters for precise queries:
```csharp
var methods = type.GetMethods()
    .WithName("Process")
    .WithReturnType<Task>()
    .WithAttribute<AuthorizeAttribute>()
    .WithParametersOfType(typeof(string));
```

### Tip 4: Null Safety
Always check for null when using `FindType`:
```csharp
var type = TypeHelper.FindType(typeName);
if (type == null)
{
    throw new TypeLoadException($"Cannot find type: {typeName}");
}
```

## 🆘 Common Issues

### Issue: Type Not Found
```csharp
// ❌ Problem: Type not in loaded assemblies
var type = TypeHelper.FindType("MyNamespace.MyClass");
// Returns null

// ✅ Solution: Ensure assembly is loaded
Assembly.Load("MyAssembly");
var type = TypeHelper.FindType("MyNamespace.MyClass");
```

### Issue: Generic Type Parsing
```csharp
// ❌ Problem: Incorrect generic syntax
var type = TypeHelper.FindType("Dictionary<string, int>");
// May fail depending on assembly loading

// ✅ Solution: Use fully qualified names
var type = TypeHelper.FindType("System.Collections.Generic.Dictionary<System.String, System.Int32>");
```

### Issue: Type Conversion Fails
```csharp
// ❌ Problem: No fallback
var number = "not a number".Reflect().To<int>();
// Throws exception

// ✅ Solution: Use Try pattern or default value
if ("not a number".Reflect().TryTo<int>(out var result))
{
    // Use result
}
// OR
var number = "not a number".Reflect().To<int>(42); // Returns 42
```

---

**Happy coding with Reflectensions!** 🎉
