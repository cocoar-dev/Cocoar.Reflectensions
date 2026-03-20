# Cocoar.Reflectensions

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download)
[![.NET Standard 2.0](https://img.shields.io/badge/.NET%20Standard-2.0-blue)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![License: Apache 2.0](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://www.apache.org/licenses/LICENSE-2.0)
[![NuGet](https://img.shields.io/nuget/v/Cocoar.Reflectensions.svg)](https://www.nuget.org/packages/Cocoar.Reflectensions/)

Advanced reflection utilities for .NET — type parsing, fluent reflection extensions, smart type conversion, and dynamic invocation. Zero external dependencies.

## Install

```shell
dotnet add package Cocoar.Reflectensions                    # Core library
dotnet add package Cocoar.Reflectensions.Invoke             # + Dynamic method invocation
dotnet add package Cocoar.Reflectensions.ExpandableObject   # + Dynamic expandable objects
```

You only need **Core** unless you need invocation or expandable objects. Targets .NET 8.0 and .NET Standard 2.0.

## Quick Start

```csharp
using Cocoar.Reflectensions;

// Parse complex type names from strings
var type = TypeHelper.FindType("Dictionary<string, List<int>>");

// Fluent reflection queries
var methods = typeof(MyClass).GetMethods()
    .WithName("Process")
    .WithReturnType<Task>()
    .WithParametersOfType(typeof(string));

// Smart type conversion — discovers implicit operators automatically
DateTime date = "2021-03-21T15:50:17+00:00".Reflect().To<DateTime>();
int number = "42".Reflect().To<int>();
Truck truck = camaro.Reflect().To<Truck>(); // uses implicit operator
```

## Key Features

- **Advanced Type Parsing** — Parse `Dictionary<string, List<int>>` from strings, with custom type mappings.
- **Fluent Reflection API** — Chainable LINQ-style extensions for types, methods, properties, and parameters.
- **Smart Type Conversion** — `Reflect().To<T>()` with automatic implicit operator detection, Parse/TryParse, IConvertible fallbacks, and optimized hot paths (8x faster for common conversions).
- **Zero-Cost Abstractions** — `ObjectReflection<T>` readonly structs with no heap allocations.
- **Dynamic Invocation** — Invoke methods via MethodInfo with automatic parameter type conversion and async support.
- **Expandable Objects** — Dynamic objects with dictionary-like behavior and `INotifyPropertyChanged`.
- **Zero Dependencies** — Completely self-contained, no third-party NuGet packages.

## Packages

| Package | Description |
|---------|-------------|
| **Cocoar.Reflectensions** | Core — type helpers, reflection extensions, type conversion, string/enum/DateTime/array utilities |
| **Cocoar.Reflectensions.Invoke** | Dynamic method invocation with parameter matching |
| **Cocoar.Reflectensions.ExpandableObject** | Dynamic objects with dictionary-like behavior |

## Migration from doob.Reflectensions

```xml
<!-- Before -->
<PackageReference Include="doob.Reflectensions" Version="6.4.2" />

<!-- After -->
<PackageReference Include="Cocoar.Reflectensions" Version="1.0.0" />
```

See the [Migration Guide](CHANGELOG.md) for namespace changes and removed packages.

## Contributing

Contributions welcome! See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

[Apache-2.0](LICENSE) — Copyright 2025 COCOAR e.U.
