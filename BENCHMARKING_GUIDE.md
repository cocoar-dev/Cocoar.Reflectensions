# Benchmarking Guide - Performance Optimization Journey

This guide explains how to benchmark Reflectensions before and after implementing the proposed optimizations.

## 🎯 Goal

Measure the actual performance impact of converting from class-based to struct-based generic wrappers to eliminate boxing and heap allocations.

## 📊 Running Benchmarks

### Quick Start (Interactive)

```bash
cd src/Cocoar.Reflectensions.Benchmarks
dotnet run -c Release
# Then choose: 1, 2, 3, 4, or 0
```

### Quick Start (Automated/CI)

```bash
cd src/Cocoar.Reflectensions.Benchmarks

# Run specific benchmark by number
dotnet run -c Release -- 1    # Reflect() API benchmarks
dotnet run -c Release -- 2    # TypeHelper benchmarks  
dotnet run -c Release -- 3    # InvokeHelper benchmarks
dotnet run -c Release -- 4    # ALL benchmarks

# Or by friendly name
dotnet run -c Release -- reflect
dotnet run -c Release -- type
dotnet run -c Release -- invoke
dotnet run -c Release -- all
```

### Advanced Options

```bash
# Run with BenchmarkDotNet filters
dotnet run -c Release -- 1 --filter "*IntToString*"

# Export results in multiple formats
dotnet run -c Release -- all --exporters json,html,csv

# Run with specific framework
dotnet run -c Release -f net8.0 -- 1
```

### CI/CD Integration

Perfect for automated pipelines:

```yaml
# GitHub Actions example
- name: Run Reflect Benchmarks
  run: |
    cd src/Cocoar.Reflectensions.Benchmarks
    dotnet run -c Release -- 1 --exporters json
    
- name: Upload Results
  uses: actions/upload-artifact@v3
  with:
    name: benchmark-results
    path: src/Cocoar.Reflectensions.Benchmarks/BenchmarkDotNet.Artifacts/
```

## 📈 Understanding Results

### Key Metrics

| Metric | What it means | Target |
|--------|---------------|--------|
| **Mean** | Average execution time | Lower is better |
| **Allocated** | Bytes allocated on heap | **Zero for hot paths** |
| **Gen0** | Garbage collections | **Zero for hot paths** |
| **Ratio** | Compared to baseline | < 1.0 means faster |

### Example Output

```
|              Method |     Mean |   Error |  StdDev | Ratio | Allocated |
|-------------------- |---------:|--------:|--------:|------:|----------:|
| int.Reflect().To    |  82.3 ns | 1.2 ns  | 1.1 ns  | 4.12  |      80 B | ⚠️ Current
| int.ToString()      |  20.0 ns | 0.3 ns  | 0.2 ns  | 1.00  |      32 B | ✅ Baseline
```

**What this tells us:**
- `.Reflect().To()` is **4.12x slower** than direct call
- Allocates **80 bytes** (40B wrapper + 40B boxing)
- **Optimization opportunity!** Should be near baseline speed

## 🎯 Baseline Expectations (Current Implementation)

Based on architecture analysis, we expect:

### ReflectBenchmarks (Current)

| Scenario | Expected Time | Expected Allocation | Issue |
|----------|---------------|---------------------|-------|
| `int.Reflect()` wrapper | ~25 ns | 48 B | Boxing (32B) + ObjectReflection (16B) |
| `string.Reflect()` wrapper | ~15 ns | 40 B | ObjectReflection allocation |
| `int.Reflect().To<string>()` | ~80 ns | 80 B | Boxing + wrapper + conversion |
| `string.Reflect().To<int>()` | ~100 ns | 48 B | Wrapper + parsing overhead |

### After Generic Struct Optimization

| Scenario | Target Time | Target Allocation | Improvement |
|----------|-------------|-------------------|-------------|
| `int.Reflect()` wrapper | ~5 ns | **0 B** | ~5x faster, zero allocation |
| `string.Reflect()` wrapper | ~5 ns | **0 B** | ~3x faster, zero allocation |
| `int.Reflect().To<string>()` | ~30 ns | **32 B** | ~2.5x faster, only string alloc |
| `string.Reflect().To<int>()` | ~60 ns | **0 B** | ~1.5x faster, zero allocation |

## 📋 Benchmark Checklist

### Phase 1: Baseline (Current Implementation)

- [ ] Run ReflectBenchmarks
- [ ] Save results to `baseline-results/`
- [ ] Note allocation hotspots
- [ ] Identify optimization priorities

### Phase 2: Struct Conversion

1. [ ] Convert `ObjectReflection` class → `readonly struct`
2. [ ] Run benchmarks again
3. [ ] Compare with baseline
4. [ ] **Expected: ~40% improvement**

### Phase 3: Generic Overloads

1. [ ] Add `ObjectReflection<T>` struct
2. [ ] Add `Reflect<T>()` extension
3. [ ] Run benchmarks
4. [ ] Compare with Phase 2
5. [ ] **Expected: ~80% improvement for value types**

### Phase 4: Validation

- [ ] All tests still pass
- [ ] No breaking changes
- [ ] Allocations near zero
- [ ] Performance meets targets

## 🔍 Analyzing Specific Scenarios

### Value Type Boxing (Critical)

```csharp
// Current: Boxing happens here
int num = 123;
var wrapped = num.Reflect(); // ❌ Boxes int → object (32 bytes)
                             // ❌ Allocates ObjectReflection (40 bytes)
```

**Benchmark focus:**
- `IntReflect_WrapperOnly` - Measures pure boxing/allocation cost
- `IntToString_ViaReflect` - Measures total overhead

**Target after optimization:** 0 bytes allocated

### String Conversions (Important)

```csharp
// Current: Just wrapper allocation
string str = "123";
var wrapped = str.Reflect(); // ❌ Allocates ObjectReflection (40 bytes)
```

**Benchmark focus:**
- `StringReflect_WrapperOnly` - Measures wrapper cost
- `StringToInt_ViaReflect` - Measures conversion overhead

**Target after optimization:** 0 bytes allocated for wrapper

### Type Parsing (Your Unique Value)

```csharp
// This is your library's superpower!
var type = TypeHelper.FindType("Dictionary<string, List<int>>");
```

**Benchmark focus:**
- Cache effectiveness
- Complex generic parsing speed

**Target:** Keep performance excellent (already good!)

## 📊 Comparing Results

### Save Baseline

```bash
# Run and save baseline
dotnet run -c Release > baseline-results.txt

# Or use BenchmarkDotNet exporters
dotnet run -c Release --exporters json
# Results saved to BenchmarkDotNet.Artifacts/results/
```

### After Optimization

```bash
# Run again after changes
dotnet run -c Release > optimized-results.txt

# Compare
diff baseline-results.txt optimized-results.txt
```

### Use BenchmarkDotNet Compare

```bash
# Generate comparison report
dotnet run -c Release --filter "*ReflectBenchmarks*" --join

# This creates a side-by-side comparison
```

## 🎓 Learning from Results

### ✅ Success Indicators

- **Zero allocations** for wrapper creation
- **Gen0 collections = 0** for hot paths
- **Ratio < 1.5** compared to direct operations
- **No performance regression** in any scenario

### ⚠️ Red Flags

- **Increased allocations** after optimization
- **Slower than baseline** in common scenarios
- **High StdDev** (> 10% of Mean) indicates instability
- **Gen0 > 0** for simple operations

## 📝 Reporting Results

When reporting benchmark results:

1. **Environment Info**
   - CPU model
   - .NET version (`dotnet --version`)
   - OS version

2. **Comparison Table**
   ```markdown
   | Scenario | Before | After | Improvement |
   |----------|--------|-------|-------------|
   | int.Reflect() | 25 ns, 48 B | 5 ns, 0 B | 5x faster, zero alloc |
   ```

3. **Key Findings**
   - Biggest improvements
   - Any regressions
   - Unexpected results

## 🚀 Next Steps

After running baseline benchmarks:

1. **Review ARCHITECTURE_IMPROVEMENT_PROPOSAL.md**
2. **Implement Phase 1**: Struct conversion
3. **Re-benchmark and compare**
4. **Implement Phase 2**: Generic overloads
5. **Final benchmark validation**
6. **Update documentation with actual results**

## 💡 Tips

- **Run multiple times** - First run warms up JIT
- **Close other applications** - Reduce system noise
- **Use Release mode** - Debug mode gives misleading results
- **Check for regressions** - Some scenarios might get slower
- **Focus on hot paths** - Optimize common cases first

---

**Ready to start?**

```bash
cd src/Cocoar.Reflectensions.Benchmarks
dotnet run -c Release
```

Choose option **1** (Reflect benchmarks) to see the current overhead!
