# Changelog

All notable changes to Reflectensions will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [7.0.0] - 2025-10-16

### 🎉 Major Modernization Release

This release represents a comprehensive modernization of the Reflectensions library, bringing it up to date with the latest .NET ecosystem standards and best practices.

### Added
- ✨ Support for .NET 8.0 across all projects
- 📚 Comprehensive README with examples and use cases
- 📋 CHANGELOG for tracking version history
- 🏷️ Enhanced package metadata and documentation

### Removed
- ⚠️ **Reflectensions.CodeDefinition** - Removed experimental project (no tests, no usage in production)
- ⚠️ **Reflectensions.AspNetCore** - Removed experimental project (no tests, no usage in production)

### Rebranded
- 🏢 **BREAKING**: Renamed all namespaces from `doob.Reflectensions.*` to `Cocoar.Reflectensions.*`
- 🏢 **BREAKING**: Renamed all package IDs from `doob.Reflectensions.*` to `Cocoar.Reflectensions.*`
- 🏢 Updated company name to "Cocoar"
- 🏢 Updated repository URLs: `github.com/doob-at` → `github.com/cocoar-dev`
- 🏢 Updated all assembly names to `Cocoar.Reflectensions.*`

### Changed
- ⬆️ **BREAKING**: Updated target frameworks from .NET 7.0 to .NET 8.0
- ⬆️ **BREAKING**: Dropped .NET Framework 4.7.2 support (use .NET Standard 2.0 for legacy compatibility)
  - .NET Standard 2.0 supports .NET Framework 4.6.1+ if needed
- ⬆️ Updated `LangVersion` to `latest` across all projects for modern C# features
- ⬆️ Updated NuGet package dependencies:
  - `Microsoft.NET.Test.Sdk` 17.8.0 → 17.11.1
  - `xunit` 2.6.3 → 2.9.2
  - `xunit.analyzers` 1.7.0 → 1.16.0
  - `xunit.runner.visualstudio` 2.5.5 → 2.8.2
  - `xunit.runner.console` 2.6.3 → 2.9.2
  - `Nuke.Common` 7.0.6 → 8.1.2
  - `GitVersion.Tool` 5.12.0 → 6.0.2

### Fixed
- 🐛 Fixed `Buffer.BlockCopy` usage with generic types in `ArrayHelpers` and `ArrayExtensions`
  - Replaced with `Array.Copy` which properly handles generic type arrays
  - Resolves compilation errors in .NET 8.0
- 🔒 Fixed deprecated `SHA256Managed` usage in `Base58Helper`
  - Updated to use `SHA256.Create()` with proper disposal pattern
  - Improves security and removes obsolete API warnings
- 🔧 Fixed `Array.Reverse()` issue in `Base58Helper`
  - Added `.AsEnumerable()` before `.Reverse()` to use LINQ extension instead of in-place reversal
  - Resolves compilation error with method chaining

### Technical Details
- All 169 tests pass successfully on .NET 8.0
- Build succeeds with no errors (105 nullable reference warnings remain for future improvement)
- Maintains full backward compatibility - no breaking API changes
- .NET Standard 2.0 target ensures compatibility with older frameworks if needed

### Migration Notes

**From v6.x to v7.0:**

#### For Existing Users (doob.Reflectensions users):
1. **Update package names** - All packages have been renamed from `doob.Reflectensions.*` to `Cocoar.Reflectensions.*`
   ```xml
   <!-- Before -->
   <PackageReference Include="doob.Reflectensions" Version="6.x" />
   
   <!-- After -->
   <PackageReference Include="Cocoar.Reflectensions" Version="7.0.0" />
   ```

2. **Update using statements** - Replace `doob.Reflectensions` with `Cocoar.Reflectensions` in your code
   ```csharp
   // Before
   using doob.Reflectensions;
   using doob.Reflectensions.ExtensionMethods;
   
   // After
   using Cocoar.Reflectensions;
   using Cocoar.Reflectensions.ExtensionMethods;
   ```

3. **Update type name strings** - If you have hardcoded type names as strings, update them
   ```csharp
   // Before
   TypeHelper.FindType("doob.Reflectensions.Tests.MyClass")
   
   // After
   TypeHelper.FindType("Cocoar.Reflectensions.Tests.MyClass")
   ```

4. **Ensure .NET 8.0 compatibility** - Your project should target .NET 8.0, .NET Standard 2.0+, or .NET Framework 4.6.1+

5. **No code logic changes required** - All APIs remain compatible; only namespaces and package names changed

#### For New Users:
- Simply install `Cocoar.Reflectensions.*` packages from NuGet
- Target .NET 8.0 or .NET Standard 2.0+ compatible runtime

### Notes
This modernization follows the recommendation from the [Value Analysis](REFLECTENSIONS_VALUE_ANALYSIS.md) document (Option 1: Full Modernization). The library is now ready for:
- Modern .NET development
- Public NuGet release
- Community contributions
- Long-term maintenance

---

## [6.x] - Previous Releases

Historical releases targeting .NET 7.0 and earlier. See git history for details.
