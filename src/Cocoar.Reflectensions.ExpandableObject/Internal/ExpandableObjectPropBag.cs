using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cocoar.Reflectensions.Internal
{
    internal class ExpandableObjectPropBag
    {
        internal readonly object _instance;
        internal readonly Type _instanceType;

        private PropertyInfo[]? _instancePropertyInfo;
        internal PropertyInfo[] InstancePropertyInfo =>
            _instancePropertyInfo ??= _instance
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
                .Where(p => p.DeclaringType != typeof(ExpandableBaseObject))
                .ToArray();


        // ReSharper disable once InconsistentNaming
        internal Dictionary<string, object?> __properties = new(StringComparer.OrdinalIgnoreCase);

        internal ExpandableObjectPropBag()
        {
            _instance = this;
            _instanceType = _instance.GetType();

        }


        internal ExpandableObjectPropBag(object? instance)
        {
            _instance = instance ?? this;
            _instanceType = _instance.GetType();


        }
    }
}
