# Changelog

All notable changes to Reflectensions will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.2]

### Fixed
- **`TypeExtensions.IsImplicitCastableTo()`** — Did not recognize that every type is implicitly assignable to `object`. Interface types (e.g. `IAsyncEnumerable<T>`) failed the check because they have no `BaseType` in .NET metadata, causing `InheritFromClass<object>()` to return `false`. This made `InvokeHelper.InvokeMethodAsync<object>()` throw for any method returning an interface type.

---

## [1.0.1] - 2025-03-23

### Fixed
- **`TypeExtensions.NotEquals<T>()`** — Method ignored the `this Type type` parameter and always compared `typeof(T) == typeof(T)`, returning `false` in all cases. Now correctly compares the source type against `typeof(T)`.

---

## [1.0.0] - 2025-01-17

### 🎉 First Official Release under Cocoar Organization

This is the first stable release of Cocoar.Reflectensions, representing a complete modernization and rebrand of the former doob.Reflectensions library.

### Added
- ✨ **Nullable reference types** enabled across all projects with full annotation
- 📊 **Comprehensive benchmarking infrastructure** with BenchmarkDotNet
- ⚡ **Performance optimizations** with automatic hot paths for common conversions
- 📚 **Complete XML documentation** for IntelliSense support
- 🧪 **529 passing tests** (379 test methods) with CI/CD validation
- 📖 **QUICK_START.md** guide for new users
- 🔧 **Centralized build configuration** via Directory.Build.props

### Changed
- 🏢 **BREAKING**: Rebranded from `doob.Reflectensions.*` to `Cocoar.Reflectensions.*`
  - Updated all namespaces, package IDs, and assembly names
  - Updated GitHub URLs from `github.com/doob-at` to `github.com/cocoar-dev`
- ⬆️ **Target Frameworks**: .NET 8.0 + .NET Standard 2.0 (dropped .NET 7.0 and .NET Framework 4.7.2)
- 📦 **Simplified package structure**: Merged common extensions into core package
  - `Cocoar.Reflectensions.CommonExtensions` merged into `Cocoar.Reflectensions`
- ✂️ **Removed JSON package**: No longer provide JSON serialization wrappers
  - Applications should use System.Text.Json or Newtonsoft.Json directly
  - Removes unnecessary abstraction layer
- 🔄 **Zero external dependencies**: Completely self-contained library
- 📝 **Updated all documentation** to reflect new branding and structure

### Removed
- ❌ **Cocoar.Reflectensions.Json** - JSON serialization wrappers removed
  - Reason: Unnecessary abstraction, apps should use System.Text.Json directly
- ❌ **Cocoar.Reflectensions.CommonExtensions** - Merged into core package
  - All common extensions (string, enum, array, etc.) now in main package
- ❌ **Cocoar.Reflectensions.CodeDefinition** - Experimental project with no tests
- ❌ **Cocoar.Reflectensions.AspNetCore** - Experimental project with no tests

### Fixed
- 🐛 Fixed `Buffer.BlockCopy` usage with generic types in `ArrayHelpers` and `ArrayExtensions`
- 🔒 Fixed deprecated `SHA256Managed` usage in `Base58Helper` (now uses `SHA256.Create()`)
- 🔧 Fixed `Array.Reverse()` issue in `Base58Helper` (added `.AsEnumerable()`)
- 🧪 Fixed timezone-dependent DateTime test for cross-platform CI compatibility
- 🚫 Fixed benchmarks project being packaged to NuGet (IsPackable=false)

### Performance
- ⚡ Implemented zero-cost abstraction with automatic hot paths:
  - `int.Reflect().To<string>()` ~80ns (was ~653ns) - 8x faster via ToString()
  - `string.Reflect().To<int>()` ~118ns (was ~155ns) - 1.3x faster via int.Parse()
  - Generic fallback paths remain available for all other conversions

### Migration from doob.Reflectensions 6.x

**Package References:**
```xml
<!-- Before -->
<PackageReference Include="doob.Reflectensions" Version="6.4.2" />
<PackageReference Include="doob.Reflectensions.CommonExtensions" Version="6.4.2" />
<PackageReference Include="doob.Reflectensions.Json" Version="6.4.2" />

<!-- After -->
<PackageReference Include="Cocoar.Reflectensions" Version="1.0.0" />
<!-- CommonExtensions merged into core - no separate package needed -->
<!-- Json removed - use System.Text.Json or Newtonsoft.Json directly -->
```

**Namespace Updates:**
```csharp
// Before
using doob.Reflectensions;
using doob.Reflectensions.ExtensionMethods;
using doob.Reflectensions.Json;

// After
using Cocoar.Reflectensions;
using Cocoar.Reflectensions.ExtensionMethods;
// Json namespace removed - use System.Text.Json
```

**JSON Migration:**
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

### Technical Details
- All 529 tests pass on .NET 8.0 (379 test methods with InlineData)
- Zero build warnings with nullable reference types enabled
- Clean, professional codebase ready for production use
- CI/CD pipeline with GitHub Actions
- Apache 2.0 License

---

## [6.4.2] - Previous Release

Last version under the `doob.Reflectensions` branding. See git history for details.
