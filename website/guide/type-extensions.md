# Type Extensions

Extension methods on `System.Type` for checking type characteristics, relationships, and operator availability.

## Type Checking

### IsNumericType

Returns `true` for all numeric types (`int`, `long`, `float`, `double`, `decimal`, `byte`, `short`, `uint`, `ulong`, `ushort`, `sbyte`):

```csharp
typeof(int).IsNumericType();     // true
typeof(decimal).IsNumericType(); // true
typeof(string).IsNumericType();  // false
```

### IsNullableType

Checks if the type is `Nullable<T>`:

```csharp
typeof(int?).IsNullableType();    // true
typeof(int).IsNullableType();     // false
typeof(string).IsNullableType();  // false
```

### IsEnumerableType

Checks if the type implements `IEnumerable` (excluding `string`):

```csharp
typeof(List<int>).IsEnumerableType();  // true
typeof(int[]).IsEnumerableType();      // true
typeof(string).IsEnumerableType();     // false
```

### IsDictionaryType

Checks if the type implements `IDictionary<,>`:

```csharp
typeof(Dictionary<string, int>).IsDictionaryType();  // true
typeof(List<int>).IsDictionaryType();                 // false
```

### IsStatic

Checks if a type is static (abstract + sealed):

```csharp
typeof(Math).IsStatic();       // true
typeof(string).IsStatic();     // false
```

## Type Relationships

### IsGenericTypeOf

Checks if a type is a specific open generic type:

```csharp
typeof(List<int>).IsGenericTypeOf<List<>>();           // true
typeof(Dictionary<string, int>).IsGenericTypeOf<Dictionary<,>>(); // true
typeof(List<int>).IsGenericTypeOf<Dictionary<,>>();    // false
```

### ImplementsInterface

Checks if a type implements a given interface:

```csharp
typeof(List<int>).ImplementsInterface<IEnumerable>();     // true
typeof(List<int>).ImplementsInterface<IDisposable>();      // false
typeof(Dictionary<string, int>).ImplementsInterface<IDictionary<string, int>>(); // true
```

### InheritFromClass

Checks if a type inherits from a given class — by type, generic, or name:

```csharp
typeof(MyDerived).InheritFromClass<MyBase>();       // true
typeof(MyDerived).InheritFromClass(typeof(MyBase));  // true
typeof(MyDerived).InheritFromClass("MyBase");        // true (name match)
```

### InheritFromClassLevel

Returns the inheritance depth between a type and an ancestor. Returns `0` if no relationship exists:

```csharp
// class A { }
// class B : A { }
// class C : B { }
typeof(C).InheritFromClassLevel<A>();  // 2
typeof(B).InheritFromClassLevel<A>();  // 1
typeof(A).InheritFromClassLevel<A>();  // 0 (same type)
```

You can limit search depth with `maximumLevel`:

```csharp
typeof(C).InheritFromClassLevel<A>(maximumLevel: 1);  // 0 (not found within 1 level)
```

### IsImplicitCastableTo

Checks if a type can be implicitly cast to another type (including numeric widening):

```csharp
typeof(int).IsImplicitCastableTo<long>();    // true
typeof(int).IsImplicitCastableTo<double>();  // true
typeof(long).IsImplicitCastableTo<int>();    // false
```

## Attribute Checking

### HasAttribute

Checks if a type is decorated with a specific attribute:

```csharp
typeof(MyClass).HasAttribute<SerializableAttribute>();    // true/false
typeof(MyClass).HasAttribute<ObsoleteAttribute>(inherit: true); // check base types too
```

## Operator Methods

### GetImplicitOperatorMethods / GetExplicitOperatorMethods

Retrieves all implicit or explicit conversion operator methods defined on a type:

```csharp
var implicits = typeof(MyType).GetImplicitOperatorMethods();
var explicits = typeof(MyType).GetExplicitOperatorMethods();
```

### GetImplicitCastMethodTo / GetExplicitCastMethodTo

Finds a specific conversion operator to a target type:

```csharp
MethodInfo? cast = typeof(MyType).GetImplicitCastMethodTo<string>();
if (cast != null)
{
    string result = (string)cast.Invoke(null, new object[] { myInstance })!;
}
```

## Equals / NotEquals

Type-safe equality checks:

```csharp
typeof(string).Equals<string>();    // true
typeof(string).NotEquals<int>();    // true
```

## Interface Discovery

### GetDirectInterfaces

Returns only the interfaces directly implemented by a type, excluding those inherited from base types or other interfaces:

```csharp
// interface IA { }
// interface IB : IA { }
// class Base : IA { }
// class Derived : Base, IB { }

typeof(Derived).GetDirectInterfaces();
// Returns: [IB] — excludes IA (inherited via Base and IB)

typeof(Base).GetDirectInterfaces();
// Returns: [IA]
```

## Filtering Collections of Types

When working with `IEnumerable<Type>`, use the filtering extensions:

```csharp
var types = assembly.GetTypes();

// Filter by attribute
var serializable = types.WithAttribute<SerializableAttribute>();

// Filter by attribute, returning the attribute instance
var withAttrs = types.WithAttributeExpanded<ObsoleteAttribute>();
foreach (var (type, attr) in withAttrs)
{
    Console.WriteLine($"{type.Name}: {attr.Message}");
}

// Filter generic types
var lists = types.WhichIsGenericTypeOf<List<>>();

// Filter by inheritance
var derived = types.WhichInheritFromClass<BaseClass>();
```
