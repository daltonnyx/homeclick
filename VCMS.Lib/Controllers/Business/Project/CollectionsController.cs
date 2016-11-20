using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Datatables;

namespace VCMS.Lib.Controllers
{
    public class CollectionsController : PostsController
    {
        public IEnumerable<Project> GetProjects()
        {
            return db.Projects;
        }

        public IEnumerable<Product> GetProducts()
        {
            return db.Products;
        }

        public override IQueryable<Category> GetCategories(bool parentOnly = false)
        {
            var result = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Collection);
            if (parentOnly)
                return result.Where(o => o.CategoryParentId == null);
            return result;
        }

        public override ActionResult Edit(int id)
        {
            ViewData[ConstantKeys.PROJECTS] = GetProjects();
            ViewData[ConstantKeys.PRODUCTS] = GetProducts();
            return base.Edit(id);
        }

        public override ActionResult Edit(Post model)
        {
                db.Post_Products.RemoveRange(db.Post_Products.Where(o => o.PostId == model.Id));
                foreach (var item in model.ProductOptionSets)
                {
                    if (item.Value > 0)
                    {
                        var productOptionSet = new Post_Product
                        {
                            PostId = model.Id,
                            ProductOptionId = int.Parse(item.Key),
                            Quantity = item.Value,
                            CreateUserId = User.Identity.GetUserId(),
                            CreateTime = DateTime.UtcNow,
                        };
                        db.Post_Products.Add(productOptionSet);
                        model.Post_ProductOptions.Add(productOptionSet);
                    }
                }

            ViewData[ConstantKeys.PROJECTS] = GetProjects();
            ViewData[ConstantKeys.PRODUCTS] = GetProducts();
            return base.Edit(model);
        }
    }
}
