# Changelog

All notable changes to Reflectensions will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - Unreleased

### Fixed
- **`TypeExtensions.NotEquals<T>()`** — Method ignored the `this Type type` parameter and always compared `typeof(T) == typeof(T)`, returning `false` in all cases. Now correctly compares the source type against `typeof(T)`.

---

## [1.0.0] - 2025-01-17

### First Official Release under Cocoar Organization

This is the first stable release of Cocoar.Reflectensions, representing a complete modernization and rebrand of the former doob.Reflectensions library.

### Added
- **Nullable reference types** enabled across all projects with full annotation
- **Comprehensive benchmarking infrastructure** with BenchmarkDotNet
- **Performance optimizations** with automatic hot paths for common conversions
- **Complete XML documentation** for IntelliSense support
- **529 passing tests** (379 test methods) with CI/CD validation
- **QUICK_START.md** guide for new users
- **Centralized build configuration** via Directory.Build.props

### Changed
- **BREAKING**: Rebranded from `doob.Reflectensions.*` to `Cocoar.Reflectensions.*`
  - Updated all namespaces, package IDs, and assembly names
  - Updated GitHub URLs from `github.com/doob-at` to `github.com/cocoar-dev`
- **Target Frameworks**: .NET 8.0 + .NET Standard 2.0 (dropped .NET 7.0 and .NET Framework 4.7.2)
- **Simplified package structure**: Merged common extensions into core package
  - `Cocoar.Reflectensions.CommonExtensions` merged into `Cocoar.Reflectensions`
- **Removed JSON package**: No longer provide JSON serialization wrappers
  - Applications should use System.Text.Json or Newtonsoft.Json directly
  - Removes unnecessary abstraction layer
- **Zero external dependencies**: Completely self-contained library
- **Updated all documentation** to reflect new branding and structure

### Removed
- **Cocoar.Reflectensions.Json** — JSON serialization wrappers removed (use System.Text.Json directly)
- **Cocoar.Reflectensions.CommonExtensions** — Merged into core package
- **Cocoar.Reflectensions.CodeDefinition** — Experimental project with no tests
- **Cocoar.Reflectensions.AspNetCore** — Experimental project with no tests

### Fixed
- Fixed `Buffer.BlockCopy` usage with generic types in `ArrayHelpers` and `ArrayExtensions`
- Fixed deprecated `SHA256Managed` usage in `Base58Helper` (now uses `SHA256.Create()`)
- Fixed `Array.Reverse()` issue in `Base58Helper` (added `.AsEnumerable()`)
- Fixed timezone-dependent DateTime test for cross-platform CI compatibility
- Fixed benchmarks project being packaged to NuGet (IsPackable=false)

### Performance
- Implemented zero-cost abstraction with automatic hot paths:
  - `int.Reflect().To<string>()` ~80ns (was ~653ns) — 8x faster via ToString()
  - `string.Reflect().To<int>()` ~118ns (was ~155ns) — 1.3x faster via int.Parse()
  - Generic fallback paths remain available for all other conversions

### Migration from doob.Reflectensions 6.x

See the [Migration Guide](/guide/migration) for detailed instructions.

---

## [6.4.2] - Previous Release

Last version under the `doob.Reflectensions` branding. See git history for details.
