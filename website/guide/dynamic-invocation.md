# Dynamic Invocation

The `Cocoar.Reflectensions.Invoke` package provides helpers for invoking methods dynamically via `MethodInfo` — with automatic parameter type conversion and async support.

## Installation

```bash
dotnet add package Cocoar.Reflectensions.Invoke
```

## InvokeHelper

All InvokeHelper methods accept a `MethodInfo` to identify the target method.

### Synchronous Invocation

#### InvokeMethod\<T\>

Invoke a method and return a typed result:

```csharp
using Cocoar.Reflectensions.Helper;

var calculator = new Calculator();
MethodInfo method = typeof(Calculator).GetMethod("Add")!;

int result = InvokeHelper.InvokeMethod<int>(calculator, method, 10, 20);
```

#### InvokeVoidMethod

Invoke a method with no return value:

```csharp
MethodInfo method = typeof(MyService).GetMethod("Initialize")!;
InvokeHelper.InvokeVoidMethod(myService, method, config);
```

::: info
If the method returns `Task` or `Task<T>`, the void variants synchronously wait for completion.
:::

### Asynchronous Invocation

#### InvokeMethodAsync\<T\>

Invoke an async method and await the result:

```csharp
MethodInfo method = typeof(MyService).GetMethod("FetchDataAsync")!;
var result = await InvokeHelper.InvokeMethodAsync<string>(
    myService, method, "https://api.example.com");
```

#### InvokeVoidMethodAsync

Invoke an async void method:

```csharp
MethodInfo method = typeof(MyService).GetMethod("ProcessAsync")!;
await InvokeHelper.InvokeVoidMethodAsync(myService, method, data);
```

### Generic Method Invocation

Invoke generic methods by passing `MethodInfo` and generic type arguments:

```csharp
MethodInfo method = typeof(MyService).GetMethod("Serialize")!;

// Invoke with explicit generic arguments
var result = InvokeHelper.InvokeGenericMethod<string>(
    myService,
    method,
    new[] { typeof(MyClass) },  // generic type arguments
    myObject);

// Void generic method
MethodInfo registerMethod = typeof(MyService).GetMethod("Register")!;
InvokeHelper.InvokeGenericVoidMethod(
    myService,
    registerMethod,
    new[] { typeof(IMyService), typeof(MyServiceImpl) });

// Async generic method
MethodInfo deserializeMethod = typeof(MyService).GetMethod("DeserializeAsync")!;
var result2 = await InvokeHelper.InvokeGenericMethodAsync<string>(
    myService,
    deserializeMethod,
    new[] { typeof(MyClass) },
    jsonString);
```

There are also convenience overloads that accept generic type arguments as type parameters instead of an array:

```csharp
// Type args as generic parameters (1-3 args supported)
var result = InvokeHelper.InvokeGenericMethod<MyClass, string>(
    myService, method, myObject);
// Equivalent to: InvokeGenericMethod<string>(instance, method, [typeof(MyClass)], ...)
```

## Automatic Parameter Conversion

InvokeHelper automatically converts parameters to match the method signature using the same smart conversion pipeline as `Reflect().To<T>()`:

```csharp
public class Calculator
{
    public int Add(int a, int b) => a + b;
}

MethodInfo method = typeof(Calculator).GetMethod("Add")!;

// String arguments are auto-converted to int
var result = InvokeHelper.InvokeMethod<int>(
    new Calculator(), method, "10", "20");
// Returns: 30
```

## Static Method Invocation

Pass `null` as the instance to invoke static methods:

```csharp
MethodInfo method = typeof(Math).GetMethod("Max", new[] { typeof(int), typeof(int) })!;
var result = InvokeHelper.InvokeMethod<int>(null, method, 10, 20);
// Returns: 20
```
