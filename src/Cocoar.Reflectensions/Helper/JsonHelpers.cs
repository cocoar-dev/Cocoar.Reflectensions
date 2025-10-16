using System;
using Cocoar.Reflectensions.Common;

namespace Cocoar.Reflectensions.Helper
{
    public static class JsonHelpers
    {

        private static IJson? _json = null;

        static JsonHelpers()
        {
            Initialize();
        }

        private static bool _initialized = false;
        static void Initialize()
        {

            if (_initialized)
            {
                return;
            }
            _initialized = true;

            var _jsonType = TypeHelper.FindType("Cocoar.Reflectensions.Json");

            if (_jsonType != null)
            {
                _json = (IJson)Activator.CreateInstance(_jsonType, new object[] { true })!;
            }

        }

        public static bool IsAvailable()
        {
            Initialize();
            return _json != null;
        }

        public static IJson? Json()
        {
            Initialize();

            return _json;
        }

    }

}
