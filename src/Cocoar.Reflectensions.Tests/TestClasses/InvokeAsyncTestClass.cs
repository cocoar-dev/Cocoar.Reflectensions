using System;
using System.Linq;
using System.Threading.Tasks;
using Cocoar.Reflectensions.ExtensionMethods;
using Cocoar.Reflectensions.Helper;

namespace Cocoar.Reflectensions.Tests.TestClasses
{
    public class InvokeAsyncTestClass
    {


        public async Task<object> GetNameAsync() {
            var methodInfo = typeof(InvokeAsyncTestClass).GetMethods()
                .WithName(nameof(GetNameAsyncAsObject)).First();
            var generic = methodInfo.MakeGenericMethod(typeof(string));
            var res = await InvokeHelper.InvokeMethodAsync(this, generic, Array.Empty<object>());
            return res;
        }

        public Task<T> GetNameAsyncAsObject<T>() {
            return Task.FromResult<T>(default!);
        }
    }
}
