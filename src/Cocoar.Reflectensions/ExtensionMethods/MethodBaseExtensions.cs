using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cocoar.Reflectensions.ExtensionMethods {
    public static class MethodBaseExtensions {

        public static bool IsExtensionMethod(this MethodBase methodInfo) {
            return methodInfo.IsDefined(typeof(ExtensionAttribute), true);
        }
    }
}
