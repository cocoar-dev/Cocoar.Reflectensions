using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Cocoar.Reflectensions.ExtensionMethods;

namespace Cocoar.Reflectensions.Benchmarks;

/// <summary>
/// Benchmarks comparing fast-path methods vs generic To{T}() conversions.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class FastPathBenchmarks
{
    private const int IntValue = 12345;
    private const string StringValue = "12345";
    private const long LongValue = 123456789L;

    #region int to string

    [Benchmark(Description = "int.Reflect().To<string>() [With Hot Path]")]
    public string IntToString_WithHotPath()
    {
        return IntValue.Reflect().To<string>()!;
    }

    [Benchmark(Description = "int.ToString() [Baseline]")]
    public string IntToString_Direct()
    {
        return IntValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    #endregion

    #region int to long

    [Benchmark(Description = "int.Reflect().To<long>() [With Hot Path]")]
    public long IntToLong_WithHotPath()
    {
        return IntValue.Reflect().To<long>();
    }

    [Benchmark(Description = "(long)int [Baseline]")]
    public long IntToLong_Direct()
    {
        return (long)IntValue;
    }

    #endregion

    #region string to int

    [Benchmark(Description = "string.Reflect().To<int>() [With Hot Path]")]
    public int StringToInt_WithHotPath()
    {
        return StringValue.Reflect().To<int>();
    }

    [Benchmark(Description = "int.Parse(string) [Baseline]")]
    public int StringToInt_Direct()
    {
        return int.Parse(StringValue, System.Globalization.CultureInfo.InvariantCulture);
    }

    #endregion

    #region long to string

    [Benchmark(Description = "long.Reflect().To<string>() [With Hot Path]")]
    public string LongToString_WithHotPath()
    {
        return LongValue.Reflect().To<string>()!;
    }

    [Benchmark(Description = "long.ToString() [Baseline]")]
    public string LongToString_Direct()
    {
        return LongValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    #endregion
}
