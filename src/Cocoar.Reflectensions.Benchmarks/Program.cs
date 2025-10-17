using BenchmarkDotNet.Running;
using Cocoar.Reflectensions.Benchmarks;

// Check for command line arguments
string? choice = args.Length > 0 ? args[0] : null;

// If no argument provided, show interactive menu
if (choice == null)
{
    Console.WriteLine("Reflectensions Performance Benchmarks");
    Console.WriteLine("======================================");
    Console.WriteLine();
    Console.WriteLine("Choose a benchmark to run:");
    Console.WriteLine("1. Reflect() API Benchmarks (boxing/allocation overhead)");
    Console.WriteLine("2. TypeHelper Benchmarks (type parsing performance)");
    Console.WriteLine("3. InvokeHelper Benchmarks (dynamic invocation overhead)");
    Console.WriteLine("4. Fast Path Benchmarks (optimized conversions)");
    Console.WriteLine("5. Run ALL benchmarks");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.WriteLine("Tip: You can also pass the choice as argument:");
    Console.WriteLine("     dotnet run -c Release -- 1");
    Console.WriteLine();
    Console.Write("Enter choice: ");
    choice = Console.ReadLine();
}
else
{
    Console.WriteLine($"Running benchmark option: {choice}");
    Console.WriteLine();
}

switch (choice)
{
    case "1":
    case "reflect":
    case "Reflect":
        Console.WriteLine("Running Reflect() API Benchmarks...");
        BenchmarkRunner.Run<ReflectBenchmarks>();
        break;
        
    case "2":
    case "type":
    case "TypeHelper":
        Console.WriteLine("Running TypeHelper Benchmarks...");
        BenchmarkRunner.Run<TypeHelperBenchmarks>();
        break;
        
    case "3":
    case "invoke":
    case "InvokeHelper":
        Console.WriteLine("Running InvokeHelper Benchmarks...");
        BenchmarkRunner.Run<InvokeHelperBenchmarks>();
        break;
        
    case "4":
    case "fast":
    case "fastpath":
    case "FastPath":
        Console.WriteLine("Running Fast Path Benchmarks...");
        BenchmarkRunner.Run<FastPathBenchmarks>();
        break;
        
    case "5":
    case "all":
    case "All":
        Console.WriteLine("Running ALL benchmarks...");
        BenchmarkRunner.Run<ReflectBenchmarks>();
        BenchmarkRunner.Run<TypeHelperBenchmarks>();
        BenchmarkRunner.Run<InvokeHelperBenchmarks>();
        BenchmarkRunner.Run<FastPathBenchmarks>();
        break;
        
    case "0":
    case "exit":
        Console.WriteLine("Exiting...");
        return;
        
    default:
        Console.WriteLine($"Invalid choice '{choice}'. Valid options: 1, 2, 3, 4, 5, or 0");
        Console.WriteLine("Running Reflect benchmarks by default...");
        BenchmarkRunner.Run<ReflectBenchmarks>();
        break;
}

Console.WriteLine();
Console.WriteLine("Benchmark complete! Check the BenchmarkDotNet.Artifacts folder for detailed results.");

