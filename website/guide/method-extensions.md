# Method Extensions

Reflectensions provides fluent, LINQ-style filtering for `MethodInfo`, `PropertyInfo`, and `ParameterInfo` collections.

## Filtering Methods

### WithName

Filter methods by name:

```csharp
var methods = typeof(MyClass).GetMethods()
    .WithName("Calculate");

// Case-insensitive
var methods2 = typeof(MyClass).GetMethods()
    .WithName("calculate", StringComparison.OrdinalIgnoreCase);
```

### WithReturnType

Filter by return type:

```csharp
var methods = typeof(MyClass).GetMethods()
    .WithReturnType<double>();

// Non-generic overload
var methods2 = typeof(MyClass).GetMethods()
    .WithReturnType(typeof(Task));
```

### WithParametersOfType

Filter by exact parameter types:

```csharp
var methods = typeof(MyClass).GetMethods()
    .WithParametersOfType(typeof(string), typeof(int));
```

### WithParametersLengthOf

Filter by parameter count:

```csharp
var methods = typeof(MyClass).GetMethods()
    .WithParametersLengthOf(2);

// Include optional parameters in count
var methods2 = typeof(MyClass).GetMethods()
    .WithParametersLengthOf(3, includeOptional: true);
```

### WithAttribute

Filter methods decorated with a specific attribute:

```csharp
var methods = typeof(MyClass).GetMethods()
    .WithAttribute<ObsoleteAttribute>();

// Include inherited attributes
var methods2 = typeof(MyClass).GetMethods()
    .WithAttribute<MyAttribute>(inherit: true);
```

### WithAttributeExpanded

Filter and return both the method and the attribute instance:

```csharp
var methods = typeof(MyClass).GetMethods()
    .WithAttributeExpanded<ObsoleteAttribute>();

foreach (var (method, attr) in methods)
{
    Console.WriteLine($"{method.Name}: {attr.Message}");
}
```

### WithGenericArgumentsLengthOf / WithGenericArgumentsOfType

Filter generic methods:

```csharp
// Methods with exactly 1 generic argument
var methods = typeof(MyClass).GetMethods()
    .WithGenericArgumentsLengthOf(1);

// Methods with specific generic argument types
var methods2 = typeof(MyClass).GetMethods()
    .WithGenericArgumentsOfType(typeof(string));
```

## Chaining Filters

All filters are composable — chain them for precise queries:

```csharp
var handlers = typeof(MyController).GetMethods()
    .WithName("Handle")
    .WithReturnType<Task>()
    .WithParametersOfType(typeof(HttpRequest))
    .WithAttribute<AuthorizeAttribute>();
```

## Single Method Checks

Each filter has a corresponding single-method check:

```csharp
MethodInfo method = ...;

bool nameMatch  = method.HasName("Calculate");
bool paramMatch = method.HasParametersLengthOf(2);
bool typeMatch  = method.HasParametersOfType(new[] { typeof(int), typeof(int) });
bool attrMatch  = method.HasAttribute<TestAttribute>();
bool retMatch   = method.HasReturnType<double>();
```

## Extension Method Detection

Check if a method is an extension method:

```csharp
MethodInfo method = ...;
bool isExtension = method.IsExtensionMethod();
// true if declared in static class with 'this' first parameter
```

## Property Extensions

### IsIndexerProperty

```csharp
PropertyInfo prop = ...;
bool isIndexer = prop.IsIndexerProperty();
```

### IsPublic

```csharp
bool isPublic = prop.IsPublic();
```

### Filtering Properties

```csharp
var indexers = typeof(MyClass).GetProperties()
    .WhichIsIndexerProperty();
```

## Parameter Extensions

### Filtering Parameters

```csharp
MethodInfo method = ...;
var parameters = method.GetParameters();

// Filter by name (ignoreCase is required)
var named = parameters.WithName("id", ignoreCase: true);

// Filter by type
var strings = parameters.WithTypeOf<string>();

// Exclude types
var nonStrings = parameters.WithoutTypeOf<string>();

// Filter by attribute name (string-based matching)
var validated = parameters.WithAttribute("Required");
var notValidated = parameters.WithoutAttribute("Required");
```

### Checking Parameters

```csharp
ParameterInfo param = ...;

// Check by attribute type
bool hasAttr = param.HasAttribute(typeof(RequiredAttribute));

// Check by attribute name (string matching)
bool hasAttrByName = param.HasAttribute("Required");
```
