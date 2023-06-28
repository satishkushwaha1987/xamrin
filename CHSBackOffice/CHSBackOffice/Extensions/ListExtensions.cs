using CHSBackOffice.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CHSBackOffice.Extensions
{
    public static class IListExtensions
    {
        public static void SyncList<T>(
            this IList<T> This,
            IEnumerable sourceList,
            Func<object, T> selector,
            Func<object, T, bool> areEqual,
            Action<object, T> updateExisting,
            bool dontRemove = false)
        {
            try
            {
                var sourceListOfType = sourceList.OfType<Object>().ToList();
                foreach (T dest in This.ToList())
                {
                    var match = sourceListOfType.FirstOrDefault(p => areEqual(p, dest));
                    if (match != null)
                    {
                        if (updateExisting != null)
                            updateExisting(match, dest);
                    }
                    else if (!dontRemove)
                    {
                        This.Remove(dest);
                    }
                }

                sourceListOfType.Where(x => !This.Any(p => areEqual(x, p)))
                    .ToList().ForEach(p =>
                    {
                        if (This.Count >= sourceListOfType.IndexOf(p))
                            This.Insert(sourceListOfType.IndexOf(p), selector(p));
                        else
                        {
                            var result = selector(p);
                            if (!EqualityComparer<T>.Default.Equals(result, default(T)))
                                This.Add(result);
                        }
                    });
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public static List<T> ToList<T>(this T[] source)
        {
            try
            {
                if (source != null)
                {
                    List<T> dest = new List<T>();
                    foreach (var item in source)
                    {
                        dest.Add((T)item);
                    }
                    return dest;
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }
    }
}
