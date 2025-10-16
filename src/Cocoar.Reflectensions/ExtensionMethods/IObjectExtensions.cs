using Cocoar.Reflectensions.Internal;

namespace Cocoar.Reflectensions.ExtensionMethods
{
    public static class IObjectExtensions
    {

        public static IObjectReflection Reflect(this IObjectReflection reflectionObject)
        {
            return reflectionObject;
        }

        public static IObjectReflection Reflect(this object reflectionObject)
        {
            return new ObjectReflection(reflectionObject);
        }

    }
}
