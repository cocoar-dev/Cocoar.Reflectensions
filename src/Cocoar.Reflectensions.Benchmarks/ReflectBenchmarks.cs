using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Cocoar.Reflectensions.ExtensionMethods;

namespace Cocoar.Reflectensions.Benchmarks;

/// <summary>
/// Benchmarks for the Reflect() wrapper pattern to measure allocation and boxing overhead.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ReflectBenchmarks
{
    // Test data
    private const int IntValue = 12345;
    private const string StringValue = "12345";
    private readonly object _boxedInt = 12345;
    private readonly DateTime _dateTime = new DateTime(2025, 1, 17);

    #region Value Type Conversions (Boxing Scenarios)

    [Benchmark(Description = "int.Reflect().To<string>()")]
    public string IntToString_ViaReflect()
    {
        return IntValue.Reflect().To<string>()!;
    }

    [Benchmark(Description = "int.ToString() [Baseline]")]
    public string IntToString_Direct()
    {
        return IntValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    [Benchmark(Description = "int.Reflect().To<long>()")]
    public long IntToLong_ViaReflect()
    {
        return IntValue.Reflect().To<long>();
    }

    [Benchmark(Description = "int cast to long [Baseline]")]
    public long IntToLong_Direct()
    {
        return (long)IntValue;
    }

    #endregion

    #region Reference Type Conversions

    [Benchmark(Description = "string.Reflect().To<int>()")]
    public int StringToInt_ViaReflect()
    {
        return StringValue.Reflect().To<int>();
    }

    [Benchmark(Description = "int.Parse(string) [Baseline]")]
    public int StringToInt_Direct()
    {
        return int.Parse(StringValue, System.Globalization.CultureInfo.InvariantCulture);
    }

    [Benchmark(Description = "string.Reflect().To<DateTime>()")]
    public DateTime StringToDateTime_ViaReflect()
    {
        return "2025-01-17".Reflect().To<DateTime>();
    }

    [Benchmark(Description = "DateTime.Parse(string) [Baseline]")]
    public DateTime StringToDateTime_Direct()
    {
        return DateTime.Parse("2025-01-17", System.Globalization.CultureInfo.InvariantCulture);
    }

    #endregion

    #region Object (Already Boxed) Conversions

    [Benchmark(Description = "object.Reflect().To<int>()")]
    public int ObjectToInt_ViaReflect()
    {
        return _boxedInt.Reflect().To<int>();
    }

    [Benchmark(Description = "object cast to int [Baseline]")]
    public int ObjectToInt_Direct()
    {
        return (int)_boxedInt;
    }

    #endregion

    #region Boolean Conversions

    [Benchmark(Description = "int.Reflect().ToBoolean()")]
    public bool IntToBoolean_ViaReflect()
    {
        return IntValue.Reflect().ToBoolean();
    }

    [Benchmark(Description = "int != 0 [Baseline]")]
    public bool IntToBoolean_Direct()
    {
        return IntValue != 0;
    }

    [Benchmark(Description = "string.Reflect().ToBoolean()")]
    public bool StringToBoolean_ViaReflect()
    {
        return "true".Reflect().ToBoolean();
    }

    [Benchmark(Description = "bool.Parse(string) [Baseline]")]
    public bool StringToBoolean_Direct()
    {
        return bool.Parse("true");
    }

    #endregion

    #region Wrapper Creation Only

    [Benchmark(Description = "int.Reflect() [wrapper only]")]
    public IObjectReflection IntReflect_WrapperOnly()
    {
        return IntValue.Reflect();
    }

    [Benchmark(Description = "string.Reflect() [wrapper only]")]
    public IObjectReflection StringReflect_WrapperOnly()
    {
        return StringValue.Reflect();
    }

    [Benchmark(Description = "DateTime.Reflect() [wrapper only]")]
    public IObjectReflection DateTimeReflect_WrapperOnly()
    {
        return _dateTime.Reflect();
    }

    #endregion

    #region Nullable Conversions

    [Benchmark(Description = "string.Reflect().To<int?>()")]
    public int? StringToNullableInt_ViaReflect()
    {
        return StringValue.Reflect().To<int?>();
    }

    [Benchmark(Description = "int.TryParse() [Baseline]")]
    public int? StringToNullableInt_Direct()
    {
        return int.TryParse(StringValue, out var result) ? result : null;
    }

    #endregion
}
