using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Homeclick.Models
{
    public enum CategoryTypes { Material = 3, Model = 2, Typology = 1, LifeStype = 4, Collection = 5, Masonry = 15, Postcat = 16, ProjectType = 18 }

    public static class ModelHelper
    {
        public static string GetTableName<T>() where T :class
        {
            using (var db = new vinabits_homeclickEntities())
            {
                var result = db.GetTableName<T>();
                return result;
            }
        }

        static IList<T> GetListByQuery<T>(string query)
        {
            using (var db = new vinabits_homeclickEntities())
            {
                var result = db.Database.SqlQuery<T>(query).ToList();
                return result;
            }
        }

        public static IList<Category> GetCategoriesOfObject(CategoryTypes type, int objectId)
        {
            var referenceTableName = "Category_" + type.ToString() + "_Link";

            var query = string.Format(@"select c.* 
                                        from[{0}] pc
                                            join[Category] c on c.Id = pc.CategoryId and c.Category_typeId = '{1}'
                                        where pc.{2}Id = '{3}'", referenceTableName, (int)type, type.ToString() ,objectId);
            var result = GetListByQuery<Category>(query);
            return result;
        }

        public static IList<T> GetObjectListByCategory<T>(int categoryId) where T : class
        {
            var objectTableName = GetTableName<T>();
            var referenceTableName = "Category_" + objectTableName + "_Link";

            var query = string.Format(@"select a.* from
                                        [{0}] a
                                            join [{1}] b on b.ChildId = a.Id
                                        where b.ParentId = '{2}'", objectTableName, referenceTableName, categoryId);
            var result = GetListByQuery<T>(query);
            return result;
        }

        public static IList<T> getParents<T>(string parentTableName, string childrenTableName, int childId)
        {
            var referenceTableName = parentTableName + "_" + childrenTableName + "_Link";
            var query = string.Format(@"select a.* from
                                        [{0}] a
                                            join [{1}] b on b.ParentId = a.Id
                                        where b.ChildId = '{2}'", parentTableName, referenceTableName, childId);
            var result = GetListByQuery<T>(query);
            return result;
        }

        public static IList<T> getChildren<T>(string parentTableName, string childrenTableName, int parentId)
        {
            var referenceTableName = parentTableName + "_" + childrenTableName + "_Link";
            var query = string.Format(@"select a.* from
                                        [{0}] a
                                            join [{1}] b on b.ChildId = a.Id
                                        where b.ParentId = '{2}'", childrenTableName, referenceTableName, parentId);
            var result = GetListByQuery<T>(query);
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


        public static IList<Category> GetProductCategories(CategoryTypes type, int productId)
        {
            using (var db = new vinabits_homeclickEntities())
            {
                var query = string.Format(@"select c.* 
                                        from[ProductDanhSachProducts_CategoryDanhSachCategories] pc
                                            join[Category] c on c.Id = pc.DanhSachCategories and c.Category_typeId = '{0}'
                                        where pc.DanhSachProducts = '{1}'", (int)type, productId);

                var categories = db.Database.SqlQuery<Category>(query).ToList();

                return categories;
            }
        }
    }
}