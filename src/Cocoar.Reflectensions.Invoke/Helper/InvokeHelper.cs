using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cocoar.Reflectensions.ExtensionMethods;

namespace Cocoar.Reflectensions.Helper
{

    /// <summary>
    /// Provides helper methods for dynamically invoking methods with support for async/await patterns.
    /// </summary>
    public static class InvokeHelper
    {

        /// <summary>
        /// Invokes a void method synchronously, automatically handling Task-returning methods.
        /// </summary>
        /// <param name="instance">The instance to invoke the method on, or null for static methods.</param>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> representing the method to invoke.</param>
        /// <param name="parameters">The parameters to pass to the method.</param>
        /// <remarks>
        /// If the method returns Task or Task&lt;T&gt;, this method will synchronously wait for completion.
        /// </remarks>
        /// <example>
        /// <code>
        /// var method = typeof(MyClass).GetMethod("DoWork");
        /// InvokeHelper.InvokeVoidMethod(myInstance, method, arg1, arg2);
        /// </code>
        /// </example>
        public static void InvokeVoidMethod(object? instance, MethodInfo methodInfo, params object[] parameters)
        {
            if (methodInfo.IsStatic)
            {
                instance = null;
            }
            else if (instance is null or Type)
            {
                instance = Activator.CreateInstance(methodInfo.ReflectedType!);
            }

            var enumerable = BuildParametersArray(methodInfo, parameters);
            var isTaskVoid = methodInfo.ReturnType == typeof(Task);
            var isTaskReturn = methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);

            if (isTaskVoid || isTaskReturn)
            {
                SimpleAsyncHelper.RunSync(() => (Task)methodInfo.Invoke(instance, enumerable)!);
                return;
            }

            methodInfo.Invoke(instance, enumerable);
        }
        
        /// <summary>
        /// Invokes a method and returns its result, with automatic type conversion and async/await support.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="instance">The instance to invoke the method on, or null for static methods.</param>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> representing the method to invoke.</param>
        /// <param name="parameters">The parameters to pass to the method.</param>
        /// <returns>The result of the method invocation, converted to type T.</returns>
        /// <remarks>
        /// Automatically handles Task&lt;T&gt; return types and performs type conversion if needed.
        /// </remarks>
        /// <example>
        /// <code>
        /// var method = typeof(Calculator).GetMethod("Add");
        /// int result = InvokeHelper.InvokeMethod&lt;int&gt;(calculator, method, 5, 3);
        /// </code>
        /// </example>
        public static T? InvokeMethod<T>(object? instance, MethodInfo methodInfo, params object[] parameters)
        {

            if (methodInfo.IsStatic)
            {
                instance = null;
            }
            else if (instance is null or Type)
            {
                instance = Activator.CreateInstance(methodInfo.ReflectedType!);
            }

            var enumerable = BuildParametersArray(methodInfo, parameters);
            var isTaskReturn = methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);

            var returnType = methodInfo.ReturnType;
            if (isTaskReturn)
            {
                returnType = methodInfo.ReturnType.GenericTypeArguments[0];
            }

            if (returnType.NotEquals<T>() && !returnType.InheritFromClass<T>() && !returnType.ImplementsInterface<T>() && !returnType.IsImplicitCastableTo<T>())
                throw new Exception($"Method returns a Type of '{methodInfo.ReturnType}' which is not implicitly castable to {typeof(T)}");

            T? returnObject;

            if (isTaskReturn)
            {

                returnObject = SimpleAsyncHelper.RunSync(() => ((Task)methodInfo.Invoke(instance, enumerable)!).CastToTaskOf<T?>());

            }
            else
            {
                returnObject = methodInfo.Invoke(instance, enumerable)!.Reflect().To<T?>();
            }

            return returnObject;
        }
        public static object InvokeMethod(object? instance, MethodInfo methodInfo, params object[] parameters)
        {

            if (methodInfo.IsStatic)
            {
                instance = null;
            }
            else if (instance is null or Type)
            {
                instance = Activator.CreateInstance(methodInfo.ReflectedType!);
            }

            var enumerable = BuildParametersArray(methodInfo, parameters);
            var isTaskReturn = methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);

            var returnType = methodInfo.ReturnType;
            if (isTaskReturn)
            {
                returnType = methodInfo.ReturnType.GenericTypeArguments[0];
            }

            object? returnObject;

            if (isTaskReturn)
            {

                returnObject = SimpleAsyncHelper.RunSync(() => ((Task)methodInfo.Invoke(instance, enumerable)!).CastToTaskOf<object>());

            }
            else
            {
                returnObject = methodInfo.Invoke(instance, enumerable);
            }

            return returnObject!;
        }
        
        /// <summary>
        /// Asynchronously invokes a void method, automatically handling both synchronous and Task-returning methods.
        /// </summary>
        /// <param name="instance">The instance to invoke the method on, or null for static methods.</param>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> representing the method to invoke.</param>
        /// <param name="parameters">The parameters to pass to the method.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// Synchronous methods will be wrapped in Task.Run, while async methods are awaited directly.
        /// </remarks>
        public static async Task InvokeVoidMethodAsync(object? instance, MethodInfo methodInfo, params object[] parameters)
        {

            if (methodInfo.IsStatic)
            {
                instance = null;
            }
            else if (instance is null or Type)
            {
                instance = Activator.CreateInstance(methodInfo.ReflectedType!);
            }

            var enumerable = BuildParametersArray(methodInfo, parameters);
            var isTaskVoid = methodInfo.ReturnType == typeof(Task);
            var isTaskReturn = methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);

            if (isTaskVoid || isTaskReturn)
            {
                await (Task)methodInfo.Invoke(instance, enumerable)!;
                return;
            }

            await Task.Run(() => methodInfo.Invoke(instance, enumerable));

        }
        
        /// <summary>
        /// Asynchronously invokes a method and returns its result, with automatic type conversion.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="instance">The instance to invoke the method on, or null for static methods.</param>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> representing the method to invoke.</param>
        /// <param name="parameters">The parameters to pass to the method.</param>
        /// <returns>A task representing the asynchronous operation, containing the method result converted to type T.</returns>
        /// <remarks>
        /// Handles both synchronous methods (wrapped in Task.Run) and async Task&lt;T&gt; methods.
        /// Performs automatic type conversion if needed.
        /// </remarks>
        public static async Task<T?> InvokeMethodAsync<T>(object? instance, MethodInfo methodInfo, params object[] parameters)
        {

            if (methodInfo.IsStatic)
            {
                instance = null;
            }
            else if (instance is null or Type)
            {
                instance = Activator.CreateInstance(methodInfo.ReflectedType!);
            }

            var enumerable = BuildParametersArray(methodInfo, parameters);
            var isTaskVoid = methodInfo.ReturnType == typeof(Task);
            if (isTaskVoid)
            {
                await (Task)methodInfo.Invoke(instance, enumerable)!;
                return default;
            }

            var isTaskReturn = methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);

            var returnType = methodInfo.ReturnType;
            if (isTaskReturn)
            {
                returnType = methodInfo.ReturnType.GenericTypeArguments[0];
            }

            if (returnType.NotEquals<T>() && !returnType.InheritFromClass<T>() && !returnType.ImplementsInterface<T>() && !returnType.IsImplicitCastableTo<T>())
                throw new Exception($"Method returns a Type of '{methodInfo.ReturnType}' which is not implicitly castable to {typeof(T)}");

            object returnObject;

            if (isTaskReturn)
            {

                var task = (Task)methodInfo.Invoke(instance, enumerable)!;
                await task;
                var resultProperty = typeof(Task<>).MakeGenericType(methodInfo.ReturnType.GetGenericArguments().First()).GetProperty("Result")!;
                returnObject = resultProperty.GetValue(task)!;
            }
            else
            {
                returnObject = await Task.Run(() => methodInfo.Invoke(instance, enumerable)!);
            }

            return returnObject.Reflect().To<T?>();

        }
        public static async Task<object> InvokeMethodAsync(object? instance, MethodInfo methodInfo, params object[] parameters)
        {

            if (methodInfo.IsStatic)
            {
                instance = null;
            }
            else if (instance is null or Type)
            {
                instance = Activator.CreateInstance(methodInfo.ReflectedType!);
            }

            var enumerable = BuildParametersArray(methodInfo, parameters);
            var isTaskVoid = methodInfo.ReturnType == typeof(Task);
            if (isTaskVoid)
            {
                await (Task)methodInfo.Invoke(instance, enumerable)!;
                return default!;
            }

            var isTaskReturn = methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);

            object? returnObject;

            if (isTaskReturn)
            {

                var task = (Task)methodInfo.Invoke(instance, enumerable)!;
                await task;
                var resultProperty = typeof(Task<>).MakeGenericType(methodInfo.ReturnType.GetGenericArguments().First()).GetProperty("Result")!;
                returnObject = resultProperty.GetValue(task)!;
            }
            else
            {
                returnObject = await Task.Run(() => methodInfo.Invoke(instance, enumerable)!);
            }

            return returnObject!;

        }

        #region Generic Methods

        #region Void

        public static void InvokeGenericVoidMethod(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters)
        {
            methodInfo = methodInfo.MakeGenericMethod(genericArguments.ToArray());
            InvokeVoidMethod(instance, methodInfo, parameters);
        }

        public static void InvokeGenericVoidMethod<TArg>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            InvokeGenericVoidMethod(instance, methodInfo, new[] { typeof(TArg) }, parameters);
        }

        public static void InvokeGenericVoidMethod<TArg1, TArg2>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            InvokeGenericVoidMethod(instance, methodInfo, new[] { typeof(TArg1), typeof(TArg2) }, parameters);
        }

        public static void InvokeGenericVoidMethod<TArg1, TArg2, TArg3>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            InvokeGenericVoidMethod(instance, methodInfo, new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) }, parameters);
        }

        #endregion

        #region WithReturn

        public static object? InvokeGenericMethod(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters)
        {
            methodInfo = methodInfo.MakeGenericMethod(genericArguments.ToArray());
            return InvokeMethod(instance, methodInfo, parameters);
        }

        public static TResult? InvokeGenericMethod<TResult>(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters)
        {
            methodInfo = methodInfo.MakeGenericMethod(genericArguments.ToArray());
            return InvokeMethod<TResult>(instance, methodInfo, parameters);
        }

        public static TResult? InvokeGenericMethod<TArg, TResult>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            return InvokeGenericMethod<TResult>(instance, methodInfo, new[] { typeof(TArg) }, parameters);
        }

        public static TResult? InvokeGenericMethod<TArg1, TArg2, TResult>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            return InvokeGenericMethod<TResult>(instance, methodInfo, new[] { typeof(TArg1), typeof(TArg2) }, parameters);
        }

        public static TResult? InvokeGenericMethod<TArg1, TArg2, TArg3, TResult>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            return InvokeGenericMethod<TResult>(instance, methodInfo, new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) }, parameters);
        }

        #endregion

        #region Task

        public static Task InvokeGenericVoidMethodAsync(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters)
        {
            methodInfo = methodInfo.MakeGenericMethod(genericArguments.ToArray());
            return InvokeVoidMethodAsync(instance, methodInfo, parameters);
        }

        public static Task InvokeGenericVoidMethodAsync<TArg>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            return InvokeGenericVoidMethodAsync(instance, methodInfo, new[] { typeof(TArg) }, parameters);
        }

        public static Task InvokeGenericVoidMethodAsync<TArg1, TArg2>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            return InvokeGenericVoidMethodAsync(instance, methodInfo, new[] { typeof(TArg1), typeof(TArg2) }, parameters);
        }

        public static Task InvokeGenericVoidMethodAsync<TArg1, TArg2, TArg3>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            return InvokeGenericVoidMethodAsync(instance, methodInfo, new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) }, parameters);
        }

        #endregion

        #region WithTaskOfT

        public static Task<object> InvokeGenericMethodAsync(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters)
        {
            methodInfo = methodInfo.MakeGenericMethod(genericArguments.ToArray());
            return InvokeMethodAsync(instance, methodInfo, parameters);
        }

        public static Task<TResult?> InvokeGenericMethodAsync<TResult>(object instance, MethodInfo methodInfo, IEnumerable<Type> genericArguments, params object[] parameters)
        {
            methodInfo = methodInfo.MakeGenericMethod(genericArguments.ToArray());
            return InvokeMethodAsync<TResult>(instance, methodInfo, parameters);
        }

        public static Task<TResult?> InvokeGenericMethodAsync<TArg, TResult>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            return InvokeGenericMethodAsync<TResult>(instance, methodInfo, new[] { typeof(TArg) }, parameters);
        }

        public static Task<TResult?> InvokeGenericMethodAsync<TArg1, TArg2, TResult>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            return InvokeGenericMethodAsync<TResult>(instance, methodInfo, new[] { typeof(TArg1), typeof(TArg2) }, parameters);
        }

        public static Task<TResult?> InvokeGenericMethodAsync<TArg1, TArg2, TArg3, TResult>(object instance, MethodInfo methodInfo, params object[] parameters)
        {
            return InvokeGenericMethodAsync<TResult>(instance, methodInfo, new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) }, parameters);
        }

        #endregion

        #endregion

        
        private static object?[] BuildParametersArray(MethodInfo methodInfo, IEnumerable<object> parameters)
        {
            var enumerable = parameters as object[] ?? parameters.ToArray();

            var pa = methodInfo.GetParameters().Select((p, i) => {

                if (enumerable.Length > i)
                {

                    return enumerable[i]?.Reflect().To(p.ParameterType);
                }

                return p.DefaultValue;
            }).ToArray();

            return pa;
        }


    }

}
