using System;

namespace Cocoar.Reflectensions.Common.Helper
{
        public class Enum<T> where T : struct, Enum, IConvertible
        {

            public static T Find(string value, T ifNotFound = default(T))
            {

                if (TryFind(value, out var found))
                {
                    return found;
                }

                return ifNotFound;

            }

            public static T Find(string value, bool ignoreCase, T ifNotFound = default(T))
            {

                if (TryFind(value, ignoreCase, out var found))
                {
                    return found;
                }

                return ifNotFound;

            }

            public static bool TryFind(string value, out T result)
            {

                var found = EnumExtensions.TryFind(typeof(T), value, out var _result);
                if (found)
                {
                    result = (T)_result!;
                }
                else
                {
                    result = default;
                }

                return found;
            }

            public static bool TryFind(string value, bool ignoreCase, out T result)
            {
                var found = EnumExtensions.TryFind(typeof(T), value, ignoreCase, out var _result);
                if (found)
                {
                    result = (T)_result!;
                }
                else
                {
                    result = default;
                }

                return found;
            }
        }
    
}
