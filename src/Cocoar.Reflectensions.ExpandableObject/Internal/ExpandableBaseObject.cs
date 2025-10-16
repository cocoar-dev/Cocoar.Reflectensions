using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cocoar.Reflectensions.ExtensionMethods;

namespace Cocoar.Reflectensions.Internal
{
    /// <summary>
    /// !!! do not use directly !!! -> use ExpandableObject
    /// </summary>
    public abstract partial class ExpandableBaseObject : DynamicObject, INotifyPropertyChanged
    {

        private readonly object _instance;
        private readonly Type _instanceType;

        private PropertyInfo[]? _instancePropertyInfo;
        private PropertyInfo[] InstancePropertyInfo =>
            _instancePropertyInfo ??= _instance
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
                .Where(p => p.DeclaringType != typeof(ExpandableBaseObject))
                .ToArray();

        private Dictionary<string, object?> __properties = new(StringComparer.OrdinalIgnoreCase);


        protected ExpandableBaseObject()
        {
            _instance = this;
            _instanceType = _instance.GetType();

        }


        protected ExpandableBaseObject(object? instance)
        {
            _instance = instance ?? this;
            _instanceType = _instance.GetType();


        }


        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach (var prop in GetProperties())
            {
                yield return prop.Key;
            }
        }


        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {

            if (__properties.Keys.Contains(binder.Name))
            {
                result = __properties[binder.Name];
                return true;
            }


            try
            {
                if (TryGetProperty(_instance, binder.Name, out result))
                {
                    return true;
                }
            }
            catch
            {
            }

            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            try
            {
                if (TrySetProperty(_instance, binder.Name, value))
                {
                    RaisePropertyChanged(binder.Name);
                    return true;
                }
            }
            catch
            {
            }

            if (__properties.TryGetValue(binder.Name, out var oldValue))
            {
                if (oldValue == null || value == null || value.GetType().InheritFromClass(oldValue.GetType()))
                {
                    __properties[binder.Name] = value;
                }
                else
                {
                    if (value.Reflect().TryTo(oldValue.GetType(), out var convValue))
                    {
                        __properties[binder.Name] = convValue;
                    }
                    else
                    {
                        throw new Exception($"Type Mismatch");
                    }
                    
                }

            }
            else
            {
                __properties[binder.Name] = value;
            }
            
            RaisePropertyChanged(binder.Name);
            return true;
        }


        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {

            try
            {
                if (TryInvokeMethod(_instance, binder.Name, args, out result))
                {
                    return true;
                }
            }
            catch
            {
                // ignored
            }


            result = null;
            return false;
        }


        protected bool TryGetProperty(object instance, string name, out object? result)
        {

            var memberInfo = _instanceType
                .GetMember(name, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.IgnoreCase).FirstOrDefault();

            if (memberInfo?.MemberType == MemberTypes.Property)
            {
                result = ((PropertyInfo)memberInfo).GetValue(instance, null);
                return true;
            }

            result = null;
            return false;
        }


        protected bool TrySetProperty(object instance, string name, object? value)
        {

            var memberInfo = _instanceType
                .GetMember(name, BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.IgnoreCase).FirstOrDefault();

            if (memberInfo?.MemberType == MemberTypes.Property)
            {
                var propInfo = (PropertyInfo)memberInfo;
                if (value != null)
                {

                    if (propInfo.PropertyType != value.GetType())
                    {
                        value.Reflect().TryTo(propInfo.PropertyType, out value);
                    }
                }
                propInfo.SetValue(instance, value, null);
                RaisePropertyChanged(name);
                return true;
            }

            return false;
        }


        protected bool TryInvokeMethod(object instance, string name, object?[]? args, out object? result)
        {

            var memberInfo = _instanceType
                .GetMember(name, BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.IgnoreCase).FirstOrDefault();

            if (memberInfo?.MemberType == MemberTypes.Method)
            {
                result = ((MethodInfo)memberInfo).Invoke(instance, args);
                return true;
            }

            result = null;
            return false;
        }


        public object? this[string key]
        {
            get
            {

                if (TryGetProperty(_instance, key, out var result))
                {
                    return result;
                }

                if (__properties.TryGetValue(key, out var prop))
                {
                    return prop;
                }

                throw new KeyNotFoundException(key);
            }
            set
            {

                if (!TrySetProperty(_instance, key, value))
                {
                    __properties[key] = value;
                }
                RaisePropertyChanged(key);
            }
        }


        public IEnumerable<KeyValuePair<string, object?>> GetProperties(bool includeInstanceProperties = true)
        {

            if (includeInstanceProperties)
            {
                foreach (var prop in InstancePropertyInfo)
                {
                    yield return new KeyValuePair<string, object?>(prop.Name, prop.GetValue(_instance, null));
                }
            }

            foreach (var key in __properties.Keys)
            {
                yield return new KeyValuePair<string, object?>(key, __properties[key]);
            }

        }

        public bool Contains(KeyValuePair<string, object?> item)
        {
            return Contains(item, true);
        }

        public bool Contains(KeyValuePair<string, object?> item, bool includeInstanceProperties)
        {
            var res = __properties.ContainsKey(item.Key);
            if (res)
            {
                return true;
            }


            if (includeInstanceProperties)
            {
                foreach (var prop in InstancePropertyInfo)
                {
                    if (prop.Name == item.Key)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ContainsKey(string key)
        {
            return ContainsKey(key, true);
        }

        public bool ContainsKey(string key, bool includeInstanceProperties)
        {

            var res = __properties.ContainsKey(key);
            if (res)
            {
                return true;
            }

            if (includeInstanceProperties)
            {
                foreach (var prop in InstancePropertyInfo)
                {
                    if (prop.Name == key)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        public bool IsInstanceProperty(string key)
        {

            foreach (var prop in InstancePropertyInfo)
            {
                if (prop.Name == key)
                    return true;
            }

            return false;
        }


        public IEnumerable<object?> GetValues()
        {
            return GetProperties().Select(p => p.Value);
        }

        public IEnumerable<string> GetKeys()
        {
            return GetProperties().Select(p => p.Key);
        }

        public T? GetValue<T>(string key)
        {
            if (TryGetProperty(_instance, key, out var result))
            {
                return result == null ? default : result.Reflect().To<T?>();
            }

            if (__properties.TryGetValue(key, out var prop))
            {
                return prop == null ? default : prop.Reflect().To<T>();
            }

            throw new KeyNotFoundException(key);
        }

        public object? GetValue(string key)
        {
            return GetValue<object>(key);
        }

        public object? GetValueOrDefault(string key, object? defaultValue = default)
        {
            return GetValueOrDefault<object>(key, defaultValue);
        }

        public T? GetValueOrDefault<T>(string key, T? defaultValue = default)
        {
            if (TryGetProperty(_instance, key, out var result))
            {
                return (T?)result;
            }

            if (__properties.TryGetValue(key, out var prop))
            {
                return (T?)prop;
            }

            return defaultValue;
        }

        public List<T?> GetValuesOrDefault<T>(string key)
        {
            var list = new List<T?>();

            if (!TryGetProperty(_instance, key, out var result))
            {
                if (!__properties.TryGetValue(key, out result))
                {
                    return list;
                }
            }

            if (result is List<T?> resultList)
            {
                return resultList;
            }

            if (result is IEnumerable ienum)
            {
                foreach (var o in ienum)
                {
                    if (o.Reflect().TryTo<T>(out var t))
                    {
                        list.Add(t);
                    }
                    else
                    {
                        if (o.Reflect().TryTo(out t))
                        {
                            list.Add(t);
                        }
                    }

                }
            }

            return list;
        }




#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        protected virtual bool SetPropertyChanged<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        protected virtual bool SetPropertyChanged<T>(ref T storage, T value, Action? onChanged, [CallerMemberName] string propertyName = null)
        {
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            //TODO: when we remove the old OnPropertyChanged method we need to uncomment the below line
            //OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
