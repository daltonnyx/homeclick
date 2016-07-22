using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public enum CategoryTypes { Material = 3, Model = 2, Typology = 1, LifeStype = 4, Collection = 5, Masonry = 15, Postcat = 16 }

    public static class ModelHelper
    {
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

        public static IList<Category> GetCategoryParents(int categoryId)
        {
            using (var db = new vinabits_homeclickEntities())
            {
                var query = string.Format(@"select c.*
                                        from Category c
                                        join CategoriesLink cl on cl.ChildId = '{0}'
                                        where c.Id = cl.ParentId", categoryId);

                var categories = db.Database.SqlQuery<Category>(query).ToList();
                return categories;
            }
        }

        public static IList<Category> GetCategoryChilds(int categoryId)
        {
            using (var db = new vinabits_homeclickEntities())
            {
                var query = string.Format(@"select c.*
                                        from Category c
                                        join CategoriesLink cl on cl.ParentId = '{0}'
                                        where c.Id = cl.ChildId", categoryId);

                var categories = db.Database.SqlQuery<Category>(query).ToList();
                return categories;
            }
        }
    }
}