using System.Threading.Tasks;

namespace Cocoar.Reflectensions.ExtensionMethods
{
    public static class TaskExtensions
    {
        public static async Task<TResult?> CastToTaskOf<TResult>(this Task task) =>
            await task.ContinueWith(t => 
                t.Reflect().GetPropertyValue<TResult>("Result")
            );


    }
}
