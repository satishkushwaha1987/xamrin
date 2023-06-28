using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace CHBackOffice.ApiServices
{
    class CustomEqualityComparer<T> : EqualityComparer<T>
    {
        private ConcurrentDictionary<Type, List<PropertyInfo>> _properties = new ConcurrentDictionary<Type, List<PropertyInfo>>();

        public override bool Equals(T x, T y)
        {
            try
            {
                if (!typeof(T).IsArray)
                    return EqualityComparer<T>.Default.Equals(x, y);
                else
                {
                    if (x == null && y == null) return true;
                    if ((x != null && y == null) || (x == null && y != null)) return false;

                    var array1 = x as Array;
                    var array2 = y as Array;

                    bool hasDifferences = array1.Length != array2.Length;

                    if (!hasDifferences)
                    {
                        var allProperties = GetProperties(typeof(T).GetElementType());
                        for (int i = 0; i < array1.Length; i++)
                        {
                            foreach (var t in allProperties)
                            {
                                try
                                {
                                    var val1 = t.GetValue(array1.GetValue(i));
                                    var val2 = t.GetValue(array2.GetValue(i));
                                    hasDifferences = hasDifferences || !val1.Equals(val2);
                                    if (hasDifferences) break;
                                }
                                catch { }
                            }
                            if (hasDifferences) break;
                        }
                    }

                    return !hasDifferences;
                }
            }
            catch 
            { 

            }
            return false;
        }

        List<PropertyInfo> GetProperties(Type t)
        {
            if (_properties.ContainsKey(t))
                return _properties[t];
            else
            {
                _properties.TryAdd(t, new List<PropertyInfo>(t.GetProperties()));
                return _properties[t];
            }
        }

        public override int GetHashCode(T obj)
        {
            return EqualityComparer<T>.Default.GetHashCode(obj);
        }

        public static new EqualityComparer<T> Default = new CustomEqualityComparer<T>();
    }
}
