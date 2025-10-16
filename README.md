# Cocoar.Reflectensions

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download)
[![.NET Standard 2.0](https://img.shields.io/badge/.NET%20Standard-2.0-blue)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![License: Apache 2.0](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://www.apache.org/licenses/LICENSE-2.0)
[![NuGet](https://img.shields.io/nuget/v/Cocoar.Reflectensions.svg)](https://www.nuget.org/packages/Cocoar.Reflectensions/)

**Cocoar.Reflectensions** is a comprehensive .NET library that simplifies working with C# Reflection and Type operations. It provides powerful utilities for type parsing, reflection queries, type conversion, and dynamic invocation - making reflection code more readable, maintainable, and efficient.

## 🎯 Why Reflectensions?

- **🔍 Advanced Type Parsing**: Parse complex generic type names from strings (e.g., `Dictionary<string, List<int>>`)
- **💡 Fluent API**: Chainable extension methods for querying types, methods, and properties
- **🔄 Smart Type Conversion**: Intelligent object type conversion with fallback mechanisms
- **⚡ Dynamic Invocation**: Simplified method invocation with automatic parameter matching
- **🌐 Cross-Platform**: Supports .NET 8.0 and .NET Standard 2.0

## 📦 Packages

Reflectensions is split into focused packages:

| Package | Description | Use When |
|---------|-------------|----------|
| **Cocoar.Reflectensions** | Core library with type helpers, reflection extensions, and common utilities | Working with types, reflection, and type conversion |
| **Cocoar.Reflectensions.Invoke** | Method invocation helpers | Dynamically calling methods |
| **Cocoar.Reflectensions.ExpandableObject** | Dynamic object support | Creating dynamic/expandable objects |

## 🚀 Quick Start

```bash
# Core library (includes type helpers, reflection, and common extensions)
dotnet add package Cocoar.Reflectensions

# Method invocation helpers
dotnet add package Cocoar.Reflectensions.Invoke

# Dynamic/expandable objects
dotnet add package Cocoar.Reflectensions.ExpandableObject
```

## ✨ Key Features

### 1. Advanced Type Resolution ⭐

Parse complex type names from strings, including generics, arrays, and custom mappings:

```csharp
using Cocoar.Reflectensions;

// Parse complex generic types
var type1 = TypeHelper.FindType("Dictionary<string, List<int>>");
var type2 = TypeHelper.FindType("System.Collections.Generic.Dictionary`2[System.String, System.Int32]");

// Custom type mapping (optional - useful for code generation scenarios)
// For example, mapping TypeScript types to C# for Monaco editor definitions
var mapping = new Dictionary<string, string> 
{ 
    ["number"] = "double",
    ["boolean"] = "bool"
};
var type3 = TypeHelper.FindType("Dictionary<string, number>", mapping);
// Resolves to: Dictionary<string, double>
```

**Supported Formats:**
- Normal type names: `System.String`, `System.DateTime`, `CustomNamespace.MyClass`
- C# keywords: `string`, `double`, `dynamic`, `int`
- Arrays: `int[]`, `int[][]`, `System.Object[][][]`
- Generic types: `List<string>`, `Dictionary<string, int>`
- Nested generics: `Dictionary<string, List<Dictionary<int, double>>>`
- Mixed formats and custom mappings

### 2. Fluent Reflection Extensions 🔗

Make reflection code more readable with chainable LINQ-style queries:

```csharp
// Filter methods fluently
var methods = typeof(MyClass).GetMethods()
    .WithName("Calculate")
    .WithReturnType<double>()
    .WithParametersOfType(typeof(int), typeof(bool))
    .WithAttribute<ObsoleteAttribute>();

// Check type relationships
if (type.IsGenericTypeOf<Dictionary<,>>()) { }
if (type.InheritFromClass<BaseClass>()) { }
if (type.ImplementsInterface<IMyInterface>()) { }
if (type.IsImplicitCastableTo<string>()) { }
```

### 3. Smart Type Conversion 🔄

Convert objects between types intelligently:

```csharp
// Simple conversions
var dateString = "2021-03-21T15:50:17+00:00";
DateTime date = dateString.Reflect().To<DateTime>();

// With fallback
object value = "not a number";
int number = value.Reflect().To<int>(42); // Returns 42

// Try pattern
if (value.Reflect().TryTo<DateTime>(out var result))
{
    // Use result
}
```

### 4. Dynamic Method Invocation ⚡

Simplify method invocation with automatic parameter matching:

```csharp
// Invoke by name
var result = instance.Invoke("MethodName", arg1, arg2);

// Invoke with automatic type conversion
var result = InvokeHelper.InvokeMethod(instance, "Calculate", "42", "true");
// Automatically converts string args to int and bool
```

### 5. Property & Field Helpers 🎯

Work with properties and fields easily:

```csharp
// Get property value by name
var value = obj.Reflect().GetPropertyValue<string>("PropertyName");

// Set property value
obj.Reflect().SetPropertyValue("PropertyName", newValue);

// Check property characteristics
if (propertyInfo.IsIndexerProperty()) { }
if (propertyInfo.IsPublic()) { }
```

## 📚 Detailed API Reference

# Typehelper

## NormalizeTypeName

```csharp
public static string NormalizeTypeName(string typeName);
public static string NormalizeTypeName(string typeName, IDictionary<string, string> customTypeMapping);
```

Normalizing complex type names to runtime friendly C# type names.
following flavours are supported  
* Normal Type Names: `System.String`, `System.DateTime`, `CustomNamespace.MyClass`...
* C# keywords: `string`, `double`, `dynamic`, `int` ...
* Arrays: `int[]`, `int[][]`, `System.Object[][][]`...
* Custom Type mappings, for example if you want to normalize `number` to `double`
* Generic Type Names, and also nested Generic Type Names
  * ```System.Collections.Generic.Dictionary`2[System.String, System.Object]```
  * ```System.Collections.Generic.List<string>```
  * ```System.Collections.Generic.Dictionary<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<number>>, System.Collections.Generic.IReadOnlyCollection<Guid[]>>```
* Mixed flavours from this list

If a typename has no namespace, like just `Guid[]` in the above list. It is assumed that this type is in the System namespace.  
  
Examples:  
```csharp
var a = "System.Collections.Generic.Dictionary`2[string, dynamic]";
TypeHelper.NormalizeTypeName(a);
// System.Collections.Generic.Dictionary`2[System.String, System.Object]


var b = "System.Collections.Generic.List<string>";
TypeHelper.NormalizeTypeName(b);
// System.Collections.Generic.List`1[System.String]


var customTypeMapping = new Dictionary<string, string>
{
    ["number"] = "double"
};
var c = "System.Collections.Generic.Dictionary<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<number>>, System.Collections.Generic.IReadOnlyCollection<Guid[]>>";
TypeHelper.NormalizeTypeName(c,customTypeMapping);
// System.Collections.Generic.Dictionary`2[System.Collections.Generic.Dictionary`2[System.String, System.Collections.Generic.List`1[System.Double]], System.Collections.Generic.IReadOnlyCollection`1[System.Guid[]]]
```



## FindType

```csharp
public static Type? FindType(string typename);
public static Type? FindType(string typename, IDictionary<string, string> customTypeMapping);

```

Uses the above 'NormalizeTypeName' Methods to at first normalizes the type name, and then find the C# Type in the loaded Assemblies.

With the customTypeMapping parameter, its possible to modify the normalization behaviour.  
I use this for example in another library to map TypeScript Syntax to C# Types.
```csharp
var cMap = new Dictionary<string, string>
{
    ["number"] = "double"
};
var typeString = "System.Collections.Generic.Dictionary<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<number>>, System.Collections.Generic.IReadOnlyCollection`1[Guid[]]>";
var type = TypeHelper.FindType(typeString, cMap);
// which resolves to a type of
// Dictionary<Dictionary<string, List<double>>, IReadOnlyCollection<Guid[]>>

```

## MethodBase Extensions

```csharp
public static bool IsExtensionMethod(this MethodBase methodInfo);
```

Currently just one Method which tells you if the Method is an ExtensionMethod.  
Which means that the Method is declared in a static class, and the first parameter of this Method has the `this` modifier.  


## IEnumerable\<MethodInfo\> Extensions

```csharp
public static IEnumerable<MethodInfo> WithName(this IEnumerable<MethodInfo> methodInfos, string name, StringComparison stringComparison = StringComparison.CurrentCulture);
public static IEnumerable<MethodInfo> WithReturnType<T>(this IEnumerable<MethodInfo> methodInfos);
public static IEnumerable<MethodInfo> WithReturnType(this IEnumerable<MethodInfo> methodInfos, Type returnType);
public static IEnumerable<MethodInfo> WithParametersOfType(this IEnumerable<MethodInfo> methodInfos, params Type[] types);
public static IEnumerable<MethodInfo> WithAttribute<T>(this IEnumerable<MethodInfo> methodInfos, bool inherit = false);
public static IEnumerable<MethodInfo> WithAttribute(this IEnumerable<MethodInfo> methodInfos, Type attributeType, bool inherit = false);
public static IEnumerable<(MethodInfo MethodInfo, T Attribute)> WithAttributeExpanded<T>(this IEnumerable<MethodInfo> methodInfos, bool inherit = false) where T : Attribute;
public static IEnumerable<(MethodInfo MethodInfo, Attribute Attribute)> WithAttributeExpanded(this IEnumerable<MethodInfo> methodInfos, Type attributeType, bool inherit = false)
```

## MethodInfo Extensions

```csharp
public static bool HasName(this MethodInfo methodInfo, string name, StringComparison stringComparison = StringComparison.CurrentCulture);
public static bool HasParametersLengthOf(this MethodInfo methodInfo, int parameterLength, bool includeOptional = false);
public static bool HasParametersOfType(this MethodInfo methodInfo, Type[] types);
public static bool HasAttribute<T>(this MethodInfo methodInfo, bool inherit = false) where T : Attribute;
public static bool HasAttribute(this MethodInfo methodInfo, Type attributeType, bool inherit = false);
public static bool HasReturnType<T>(this MethodInfo methodInfo);
public static bool HasReturnType(this MethodInfo methodInfo, Type returnType);
```


## PropertyInfo Extensions

```csharp
public static bool IsIndexerProperty(this PropertyInfo propertyInfo);
public static bool IsPublic(this PropertyInfo propertyInfo);
public static IEnumerable<PropertyInfo> WhichIsIndexerProperty(this IEnumerable<PropertyInfo> propertyInfos);
```

## IEnumerable\<Type\> Extensions

```csharp
public static IEnumerable<Type> WithAttribute<T>(this IEnumerable<Type> types, bool inherit = false) where T : Attribute;
public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attributeType, bool inherit = false);
public static IEnumerable<KeyValuePair<Type, T>> WithAttributeExpanded<T>(this IEnumerable<Type> types, bool inherit = false) where T : Attribute;
public static IEnumerable<KeyValuePair<Type, Attribute>> WithAttributeExpanded(this IEnumerable<Type> types, Type attributeType, bool inherit = false);
public static IEnumerable<Type> WhichIsGenericTypeOf(this IEnumerable<Type> types, Type of);
public static IEnumerable<Type> WhichIsGenericTypeOf<T>(this IEnumerable<Type> types);
public static IEnumerable<Type> WhichInheritFromClass(this IEnumerable<Type> types, Type from);
public static IEnumerable<Type> WhichInheritFromClass<T>(this IEnumerable<Type> types);
```

## Type Extensions

```csharp
#region Check Type
public static bool IsNumericType(this Type type);
public static bool IsGenericTypeOf(this Type type, Type genericType);
public static bool IsGenericTypeOf<T>(this Type type);
public static bool IsNullableType(this Type type);
public static bool IsEnumerableType(this Type type);
public static bool IsDictionaryType(this Type type);
public static bool ImplementsInterface(this Type type, Type interfaceType);
public static bool ImplementsInterface<T>(this Type type);
public static bool InheritFromClass<T>(this Type type);
public static bool InheritFromClass(this Type type, string from);
public static bool InheritFromClass(this Type type, Type from);
public static bool IsImplicitCastableTo<T>(this Type type);
public static bool IsImplicitCastableTo(this Type type, Type to);
public static bool Equals<T>(this Type type);
public static bool NotEquals<T>(this Type type);
public static bool HasAttribute<T>(this Type type, bool inherit = false) where T : Attribute;
public static bool HasAttribute(this Type type, Type attributeType, bool inherit = false);
public static bool IsStatic(this Type type);
#endregion

#region Get Operator Methods
public static IEnumerable<MethodInfo> GetImplicitOperatorMethods(this Type type, bool throwOnError = true);
public static IEnumerable<MethodInfo> GetExplicitOperatorMethods(this Type type, bool throwOnError = true);
public static IEnumerable<MethodInfo> GetImplicitOperatorMethods<T>(bool throwOnError = true);
public static IEnumerable<MethodInfo> GetExplicitOperatorMethods<T>(bool throwOnError = true);
public static MethodInfo? GetImplicitCastMethodTo<T>(this Type fromType);
public static MethodInfo? GetImplicitCastMethodTo(this Type fromType, Type toType);
public static MethodInfo? GetExplicitCastMethodTo<T>(this Type fromType);
public static MethodInfo? GetExplicitCastMethodTo(this Type fromType, Type toType);
#endregion

public static int InheritFromClassLevel(this Type type, Type from, int? maximumLevel = null);
public static int InheritFromClassLevel(this Type type, string from, int? maximumLevel = null);
public static int InheritFromClassLevel<T>(this Type type, int? maximumLevel = null);

```

## Object Extensions

ExtensionMethods for the `object` type are special.  
To prevent that those ExtensionMethods appears on all types which inherit from `object`(which are almost all...) there is ONE special Method called `Reflect()`.  

```csharp
public static ObjectReflection Reflect(this object reflectionObject);
```

From there you have access to the ExtensionMethods which are avsailable to `object` types.

```csharp
public static bool EqualsToAnyOf(this ObjectReflection objectReflection, params object[] equalsTo);
public static bool ToBoolean(this ObjectReflection objectReflection, params object[] trueValues);
public static bool IsImplicitCastableTo(this ObjectReflection objectReflection, Type type);
public static bool IsImplicitCastableTo<T>(this ObjectReflection objectReflection);

// tries to switch the type of an object
public static bool TryAs(this ObjectReflection objectReflection, Type type, out object? outValue);
public static bool TryAs<T>(this ObjectReflection objectReflection, out T? outValue);
public static object? As(this ObjectReflection objectReflection, Type type);
public static T? As<T>(this ObjectReflection objectReflection);

// tries to cast on object
// - use TryAs Methods to switch type if possible
// - uses an ExtensinMehtod from this lib to `convert` the object, fro example `string.ToGuid()`
// - search for a suitable implicit operator Method
public static bool TryTo(this ObjectReflection objectReflection, Type type, out object? outValue);
public static object? To(this ObjectReflection objectReflection, Type type, object? defaultValue);
public static object? To(this ObjectReflection objectReflection, Type type);
public static bool TryTo<T>(this ObjectReflection objectReflection, out T? outValue);
public static  T? To<T>(this ObjectReflection objectReflection, T? defaultValue);
public static  T? To<T>(this ObjectReflection objectReflection);

public static object? GetPropertyValue(this ObjectReflection objectReflection, string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance);
public static T? GetPropertyValue<T>(this ObjectReflection objectReflection, string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance);
public static void SetPropertyValue(this ObjectReflection objectReflection, string path, object value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance);

```


```csharp

var dtString = "2021-03-21T15:50:17+00:00";
DateTime date = dtString.Reflect().To<DateTime>();

```

## 🛠️ Real-World Use Cases

### Plugin/Extensibility Systems
```csharp
// Load types from configuration
var pluginType = TypeHelper.FindType(config.PluginTypeName);
var instance = Activator.CreateInstance(pluginType);
```

### Dynamic Configuration
```csharp
// Type-driven configuration
var processorType = TypeHelper.FindType(settings.ProcessorType);
var processor = (IProcessor)Activator.CreateInstance(processorType);
```

### Code Generation
```csharp
// Parse TypeScript-style types to C#
var mapping = new Dictionary<string, string>
{
    ["number"] = "double",
    ["boolean"] = "bool",
    ["any"] = "object"
};
var csharpType = TypeHelper.FindType(typeScriptType, mapping);
```

### Testing & Mocking
```csharp
// Find methods with specific attributes
var testMethods = typeof(TestClass).GetMethods()
    .WithAttribute<TestAttribute>()
    .WithReturnType<Task>();
```

## 🏗️ Architecture

Cocoar.Reflectensions is designed with simplicity and modularity in mind:

- **Core Library**: Type resolution, reflection extensions, and common utilities (strings, enums, arrays, dates) all in one package
- **Specialized Packages**: Domain-specific functionality (Invoke, ExpandableObject)
- **Zero Dependencies**: Completely dependency-free - no third-party packages required
- **Multi-Targeting**: Supports .NET 8.0 and .NET Standard 2.0 for broad compatibility

## 📋 Requirements

- **.NET 8.0** or later (recommended)
- **.NET Standard 2.0** compatible runtime (.NET Core 2.0+, .NET 5+, .NET Framework 4.6.1+)

## 🔄 Migration from doob.Reflectensions

This is the first release under the Cocoar organization. If you're migrating from `doob.Reflectensions` (v6.4.2):

1. **Update Package References**: Change from `doob.Reflectensions.*` to `Cocoar.Reflectensions.*`
   ```xml
   <!-- Before -->
   <PackageReference Include="doob.Reflectensions" Version="6.4.2" />
   
   <!-- After -->
   <PackageReference Include="Cocoar.Reflectensions" Version="1.0.0" />
   ```

2. **Update Using Statements**: Change namespace imports
   ```csharp
   // Before
   using doob.Reflectensions;
   
   // After
   using Cocoar.Reflectensions;
   ```

3. **Update Type Name Strings**: If you have hardcoded type names
   ```csharp
   // Before
   TypeHelper.FindType("doob.Reflectensions.Tests.MyClass")
   
   // After
   TypeHelper.FindType("Cocoar.Reflectensions.Tests.MyClass")
   ```

**What's New in v1.0.0:**
- ✅ Rebranded from doob.Reflectensions to Cocoar.Reflectensions
- ✅ Modernized to .NET 8.0 + .NET Standard 2.0
- ✅ **Zero dependencies** - removed Newtonsoft.Json (was the only external dependency)
- ✅ Simplified from 7 projects to 4 projects
- ✅ Fixed circular dependencies
- ✅ Merged common extensions into core package for simplicity
- ✅ Comprehensive documentation
- ✅ CI/CD with GitHub Actions
- ✅ All 166 tests passing
- ✅ Apache 2.0 License

## 🤝 Contributing

Contributions are welcome! We follow the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md).

Ways to contribute:
- 🐛 Bug reports and fixes
- 💡 Feature requests and implementations
- 📖 Documentation improvements
- 🔧 Code reviews and discussions

Please read our [Contributing Guidelines](CONTRIBUTING.md) before submitting PRs.

## 📄 License

Copyright 2025 COCOAR e.U.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this project except in compliance with the License.
You may obtain a copy of the License at:

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

See the [LICENSE](LICENSE) file for the full license text.

## 🔒 Security

For information about security policies and reporting vulnerabilities, see our [Security Policy](SECURITY.md).

## 🙏 Acknowledgments

**Created and maintained by** [Bernhard Windisch](https://github.com/windischb) at [COCOAR e.U.](https://github.com/cocoar-dev)

Based on the solid foundation of the original doob.Reflectensions library (v6.4.2).

Special thanks to all contributors and users who have helped improve this library!

## 📞 Support & Links

- **GitHub**: [https://github.com/cocoar-dev/Cocoar.Reflectensions](https://github.com/cocoar-dev/Cocoar.Reflectensions)
- **Issues**: [Report a bug or request a feature](https://github.com/cocoar-dev/Cocoar.Reflectensions/issues)
- **NuGet**: [Browse packages](https://www.nuget.org/packages?q=Cocoar.Reflectensions)
- **Documentation**: [Full documentation](https://github.com/cocoar-dev/Cocoar.Reflectensions/tree/develop/docs)

---

**Built with ❤️ by COCOAR e.U.**

---

**Made with ❤️ for the .NET Community**
