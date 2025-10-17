using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Cocoar.Reflectensions.Helper;
using System.Reflection;

namespace Cocoar.Reflectensions.Benchmarks;

/// <summary>
/// Benchmarks for InvokeHelper - dynamic method invocation overhead.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class InvokeHelperBenchmarks
{
    private readonly TestClass _instance = new();
    private readonly MethodInfo _addMethod;
    private readonly MethodInfo _addAsyncMethod;

    public InvokeHelperBenchmarks()
    {
        _addMethod = typeof(TestClass).GetMethod(nameof(TestClass.Add))!;
        _addAsyncMethod = typeof(TestClass).GetMethod(nameof(TestClass.AddAsync))!;
    }

    [Benchmark(Description = "InvokeHelper.InvokeMethod<int>()")]
    public int InvokeMethod_ViaHelper()
    {
        return InvokeHelper.InvokeMethod<int>(_instance, _addMethod, 5, 3);
    }

    [Benchmark(Description = "MethodInfo.Invoke() [Baseline]")]
    public int InvokeMethod_Direct()
    {
        return (int)_addMethod.Invoke(_instance, new object[] { 5, 3 })!;
    }

    [Benchmark(Description = "Direct method call [Absolute Baseline]")]
    public int InvokeMethod_NoReflection()
    {
        return _instance.Add(5, 3);
    }

    [Benchmark(Description = "InvokeHelper.InvokeMethodAsync<int>()")]
    public async Task<int> InvokeMethodAsync_ViaHelper()
    {
        return await InvokeHelper.InvokeMethodAsync<int>(_instance, _addAsyncMethod, 5, 3);
    }

    [Benchmark(Description = "Direct async method call [Baseline]")]
    public async Task<int> InvokeMethodAsync_NoReflection()
    {
        return await _instance.AddAsync(5, 3);
    }

    #region Test Class

    public class TestClass
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public async Task<int> AddAsync(int a, int b)
        {
            await Task.Delay(1);
            return a + b;
        }

        public void VoidMethod()
        {
            // No-op
        }

        public async Task VoidMethodAsync()
        {
            await Task.Delay(1);
        }
    }

    #endregion
}
