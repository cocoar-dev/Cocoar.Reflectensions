# Migration from doob.Reflectensions

Cocoar.Reflectensions v1.0.0 is the successor to `doob.Reflectensions` v6.4.2, with a new organization, simplified package structure, and zero external dependencies.

## Package References

```xml
<!-- Before -->
<PackageReference Include="doob.Reflectensions" Version="6.4.2" />
<PackageReference Include="doob.Reflectensions.CommonExtensions" Version="6.4.2" />
<PackageReference Include="doob.Reflectensions.Json" Version="6.4.2" />

<!-- After -->
<PackageReference Include="Cocoar.Reflectensions" Version="1.0.0" />
<!-- CommonExtensions merged into core — no separate package needed -->
<!-- Json removed — use System.Text.Json or Newtonsoft.Json directly -->
```

## Namespace Updates

```csharp
// Before
using doob.Reflectensions;
using doob.Reflectensions.ExtensionMethods;
using doob.Reflectensions.Json;

// After
using Cocoar.Reflectensions;
using Cocoar.Reflectensions.ExtensionMethods;
// Json namespace removed — use System.Text.Json
```

## Package Changes

| Old Package | Action | New Package |
|---|---|---|
| `doob.Reflectensions` | Renamed | `Cocoar.Reflectensions` |
| `doob.Reflectensions.Invoke` | Renamed | `Cocoar.Reflectensions.Invoke` |
| `doob.Reflectensions.ExpandableObject` | Renamed | `Cocoar.Reflectensions.ExpandableObject` |
| `doob.Reflectensions.CommonExtensions` | **Merged** into core | `Cocoar.Reflectensions` |
| `doob.Reflectensions.Json` | **Removed** | Use `System.Text.Json` directly |
| `doob.Reflectensions.CodeDefinition` | **Removed** | — |
| `doob.Reflectensions.AspNetCore` | **Removed** | — |

## JSON Migration

The JSON wrapper has been removed. Replace with `System.Text.Json`:

```csharp
// Before (doob.Reflectensions.Json)
using doob.Reflectensions.Json;
var json = Json.Converter.ToJson(obj);
var obj = Json.Converter.ToObject<T>(json);

// After (System.Text.Json)
using System.Text.Json;
var json = JsonSerializer.Serialize(obj);
var obj = JsonSerializer.Deserialize<T>(json);
```

## Type Name Strings

If you have hardcoded type name strings referencing the old namespace, update them:

```csharp
// Before
TypeHelper.FindType("doob.Reflectensions.Tests.MyClass")

// After
TypeHelper.FindType("Cocoar.Reflectensions.Tests.MyClass")
```

## Target Framework Changes

| Before | After |
|---|---|
| .NET 7.0 | .NET 8.0 |
| .NET Framework 4.7.2 | — (removed) |
| .NET Standard 2.0 | .NET Standard 2.0 (kept) |

If you were targeting .NET Framework 4.7.2 directly, you can still use Reflectensions through .NET Standard 2.0 compatibility (.NET Framework 4.6.1+).

## What's New in v1.0.0

- **Zero external dependencies** — Newtonsoft.Json dependency removed
- **Nullable reference types** enabled across all projects
- **Performance optimizations** — hot paths for common conversions (8x faster for int to string)
- **529 passing tests** with CI/CD validation
- **Full XML documentation** for IntelliSense
