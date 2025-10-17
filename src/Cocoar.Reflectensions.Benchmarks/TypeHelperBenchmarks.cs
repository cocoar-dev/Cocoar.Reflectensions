using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Cocoar.Reflectensions.Helper;

namespace Cocoar.Reflectensions.Benchmarks;

/// <summary>
/// Benchmarks for TypeHelper.FindType() - the unique value proposition of Reflectensions.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class TypeHelperBenchmarks
{
    // Test data
    private const string SimpleType = "System.String";
    private const string GenericType = "Dictionary<string, int>";
    private const string ComplexGenericType = "Dictionary<string, List<Tuple<int, string>>>";
    private const string ArrayType = "System.Int32[]";
    private const string MultidimensionalArrayType = "System.Int32[,]";

    [Benchmark(Description = "FindType(\"System.String\")")]
    public Type? FindSimpleType()
    {
        return TypeHelper.FindType(SimpleType);
    }

    [Benchmark(Description = "Type.GetType(\"System.String\") [Baseline]")]
    public Type? GetSimpleType_Direct()
    {
        return Type.GetType(SimpleType);
    }

    [Benchmark(Description = "FindType(\"Dictionary<string, int>\")")]
    public Type? FindGenericType()
    {
        return TypeHelper.FindType(GenericType);
    }

    [Benchmark(Description = "FindType(\"Dictionary<string, List<Tuple<int, string>>>\")")]
    public Type? FindComplexGenericType()
    {
        return TypeHelper.FindType(ComplexGenericType);
    }

    [Benchmark(Description = "FindType(\"System.Int32[]\")")]
    public Type? FindArrayType()
    {
        return TypeHelper.FindType(ArrayType);
    }

    [Benchmark(Description = "FindType(\"System.Int32[,]\")")]
    public Type? FindMultidimensionalArrayType()
    {
        return TypeHelper.FindType(MultidimensionalArrayType);
    }

    [Benchmark(Description = "FindType (cached) - 2nd call")]
    public Type? FindTypeCached()
    {
        // First call to populate cache
        TypeHelper.FindType(SimpleType);
        
        // Second call - should be cached
        return TypeHelper.FindType(SimpleType);
    }

    [Benchmark(Description = "NormalizeTypeName(\"Dictionary<string, int>\")")]
    public string NormalizeGenericTypeName()
    {
        return TypeHelper.NormalizeTypeName(GenericType);
    }
}
