# Code Coverage

This document describes how to run and analyze code coverage for Cocoar.Reflectensions.

## Quick Start

Run tests with code coverage:

```powershell
./run-coverage.ps1
```

This will:
1. Run all tests with code coverage collection
2. Generate an HTML coverage report
3. Display a summary in the console

## View Results

After running, open the HTML report:
- **Location**: `TestResults/CoverageReport/index.html`
- Shows line-by-line coverage for all source files
- Highlights covered (green) and uncovered (red) code

## Current Coverage Status

As of the latest run, the coverage metrics are:
- **Line Coverage**: 36.7%
- **Branch Coverage**: 37.3%
- **Method Coverage**: 28.9%

### Coverage by Assembly

| Assembly | Coverage |
|----------|----------|
| Cocoar.Reflectensions | 35.1% |
| Cocoar.Reflectensions.ExpandableObject | 28.1% |
| Cocoar.Reflectensions.Invoke | 58.6% |

## Areas Needing Tests

Based on current coverage, these areas need additional tests:

### Zero Coverage (Priority)
- `ActionExtensions` - 0%
- `ArrayExtensions` - 0%
- `AssemblyExtensions` - 0%
- `ClaimExtensions` - 0%
- `DateTimeExtensions` - 0%
- `ArrayHelpers` - 0%
- `Base58Helper` - 0%
- `WildcardHelper` - 0%
- `StringExtensions` - 0%
- `MethodBaseExtensions` - 0%
- `ParameterInfoEnumerableExtensions` - 0%
- `ParameterInfoExtensions` - 0%
- `PropertyInfoExtensionMethods` - 0%
- `TypeEnumerableExtensions` - 0%
- `ExpandableObjectPropBag` - 0%

### Low Coverage (< 50%)
- `MethodInfoEnumerableExtensions` - 23%
- `MethodInfoExtensions` - 33.3%
- `ExpandableBaseObject` - 28.1%
- `ExpandableObject` - 40.6%
- `ObjectReflection` - 45.4%
- `IObjectExtensions` - 50%

### Well Covered (>= 90%)
- `InternalStringExtensions` - 91.6%
- `SimpleAsyncHelper` - 100%
- `TypeLookupHelper` - 100%
- `TypeHelperCache` - 100%
- `TypeInheritanceLevel` - 100%
- `EnumExtensions` - 100%
- `TaskExtensions` - 100%
- `IDictionaryExtensions` (ExtensionMethods) - 100%

## Configuration

Coverage is configured in `coverlet.runsettings`:
- **Format**: OpenCover XML
- **Excludes**: Test assemblies and xunit internals
- **Source Link**: Enabled for source code navigation

## CI Integration

The code coverage results can be integrated into CI/CD pipelines:

```yaml
# Example GitHub Actions workflow
- name: Run Tests with Coverage
  run: ./run-coverage.ps1

- name: Upload Coverage Report
  uses: codecov/codecov-action@v3
  with:
    files: TestResults/**/coverage.opencover.xml
    flags: unittests
```

## Tools Used

- **coverlet.collector**: Cross-platform code coverage library
- **ReportGenerator**: Converts coverage results to HTML reports

## Goals

Target coverage levels:
- **Line Coverage**: ≥ 80%
- **Branch Coverage**: ≥ 75%
- **Method Coverage**: ≥ 70%

We should prioritize:
1. Core functionality (type operations, casting, reflection helpers)
2. Public APIs that users directly interact with
3. Error handling paths
4. Edge cases and boundary conditions
