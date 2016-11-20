using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Common
{
    public static class Helper
    {
        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        public static List<T> FindCommon<T>(List<IEnumerable<T>> lists)
        {
            Dictionary<T, int> map = new Dictionary<T, int>();
            int listCount = 0; // number of lists

            foreach (IEnumerable<T> list in lists)
            {
                listCount++;
                foreach (T item in list)
                {
                    // Item encountered, increment count
                    int currCount;
                    if (!map.TryGetValue(item, out currCount))
                        currCount = 0;

                    currCount++;
                    map[item] = currCount;
                }
            }

            List<T> result = new List<T>();
            foreach (KeyValuePair<T, int> kvp in map)
            {
                // Items whose occurrence count is equal to the number of lists are common to all the lists
                if (kvp.Value == listCount)
                    result.Add(kvp.Key);
            }

            return result;
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> someTypes, int maxCount)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            Dictionary<double, T> randomSortTable = new Dictionary<double, T>();

            foreach (T someType in someTypes)
                randomSortTable[random.NextDouble()] = someType;

            return randomSortTable.OrderBy(KVP => KVP.Key).Take(maxCount).Select(KVP => KVP.Value);
        }

        public static object GetEntityFieldValue(this object entityObj, string propertyName)
        {
            var pro = entityObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).First(x => x.Name == propertyName);
            return pro.GetValue(entityObj, null);

        }

        public static IEnumerable<PropertyInfo> GetManyRelatedEntityNavigatorProperties(object entityObj)
        {
            var props = entityObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanWrite && x.GetGetMethod().IsVirtual && x.PropertyType.IsGenericType == true);
            return props;
        }

        public static bool HasAnyRelation(object entityObj)
        {

            var collectionProps = GetManyRelatedEntityNavigatorProperties(entityObj);


            foreach (var item in collectionProps)
            {
                var collectionValue = GetEntityFieldValue(entityObj, item.Name);
                if (collectionValue != null && collectionValue is IEnumerable)
                {
                    var col = collectionValue as IEnumerable;
                    if (col.GetEnumerator().MoveNext())
                    {
                        return true;
                    }

                }
            }
            return false;
        }
    }

    public static class ObjectToDictionaryHelper
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }
    }
}
