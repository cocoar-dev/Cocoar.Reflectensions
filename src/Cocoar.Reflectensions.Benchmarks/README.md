# Reflectensions Performance Benchmarks

This project contains comprehensive performance benchmarks for the Reflectensions library using BenchmarkDotNet.

## Running Benchmarks

### Option 1: Interactive menu
```bash
cd src/Cocoar.Reflectensions.Benchmarks
dotnet run -c Release
# Then enter: 1, 2, 3, 4, or 0
```

### Option 2: Command line argument (CI/CD friendly)
```bash
# Run specific benchmark by number
dotnet run -c Release -- 1    # Reflect benchmarks
dotnet run -c Release -- 2    # TypeHelper benchmarks
dotnet run -c Release -- 3    # InvokeHelper benchmarks
dotnet run -c Release -- 4    # ALL benchmarks

# Or by name
dotnet run -c Release -- reflect
dotnet run -c Release -- type
dotnet run -c Release -- invoke
dotnet run -c Release -- all
```

### Option 3: BenchmarkDotNet filters (advanced)
```bash
dotnet run -c Release -- 1 --filter "*IntToString*"
dotnet run -c Release -- all --filter "*Wrapper*"
```

## Benchmark Categories

### 1. ReflectBenchmarks
Measures the overhead of the `.Reflect()` wrapper pattern:
- **Boxing overhead** for value types (int, DateTime, etc.)
- **Allocation overhead** for ObjectReflection wrapper
- **Type conversion performance** compared to direct casts
- **Nullable conversions** performance

**Key scenarios:**
- `int.Reflect().To<string>()` vs `int.ToString()`
- `string.Reflect().To<int>()` vs `int.Parse()`
- Wrapper-only overhead

### 2. TypeHelperBenchmarks
Measures the unique value proposition - parsing complex type names:
- Simple type resolution
- **Generic type parsing** (e.g., `"Dictionary<string, int>"`)
- **Nested generics** (e.g., `"Dictionary<string, List<Tuple<int, string>>>"`)
- Array types
- **Caching effectiveness**

### 3. InvokeHelperBenchmarks
Measures dynamic method invocation overhead:
- Sync method invocation
- **Async method invocation**
- Comparison with direct MethodInfo.Invoke()
- Comparison with direct method calls (absolute baseline)

## Expected Results (Before Optimization)

Based on the current architecture (class-based ObjectReflection):

| Scenario | Time (ns) | Allocations | Notes |
|----------|-----------|-------------|-------|
| `int.Reflect().To<string>()` | ~80 | 80 B | Boxing + wrapper allocation |
| `int.ToString()` | ~20 | 32 B | Just string allocation |
| `string.Reflect().To<int>()` | ~100 | 48 B | Wrapper + parsing |
| `int.Parse()` | ~50 | 0 B | Direct parsing |
| Wrapper only (int) | ~25 | 48 B | ObjectReflection + boxing |
| Wrapper only (string) | ~15 | 40 B | ObjectReflection only |

## After Optimization Goals

With generic struct approach (`ObjectReflection<T>`):

| Scenario | Expected Improvement |
|----------|---------------------|
| `int.Reflect().To<string>()` | ~50% faster, 50% less allocation |
| Wrapper only (int) | ~80% faster, **zero allocation** |
| Wrapper only (string) | ~60% faster, **zero allocation** |

## Interpreting Results

### Memory Diagnostics
- **Gen0**: Number of Gen 0 garbage collections
- **Allocated**: Total bytes allocated on managed heap
- Look for **zero allocation** scenarios after optimization

### Performance Metrics
- **Mean**: Average execution time
- **Error**: Standard error
- **StdDev**: Standard deviation
- **Ratio**: Comparison to baseline (1.00 = same speed)

### What to look for
1. **High allocation numbers** on Reflect() calls indicate optimization opportunity
2. **Gen0 collections** should be zero for hot path scenarios
3. **Ratio > 2.0** means the operation is 2x slower than baseline
4. **Ratio < 0.5** means the operation is 2x faster than baseline

## Results Location

After running benchmarks, find detailed results in:
```
BenchmarkDotNet.Artifacts/results/
```

Files include:
- `*-report.html` - Interactive HTML report
- `*-report-github.md` - Markdown summary
- `*-measurements.csv` - Raw data

## Next Steps

1. **Run baseline benchmarks** (current implementation)
2. **Implement optimizations** (generic struct approach)
3. **Re-run benchmarks** to measure improvement
4. **Compare results** side-by-side

## Notes

- Benchmarks run in Release mode with optimizations enabled
- Multiple warmup and measurement iterations ensure accuracy
- Results may vary by CPU, .NET version, and system load
- Always run benchmarks on the target deployment environment for production decisions
