using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using VCMS.Lib.Models;
using System.Net;

namespace Homeclick.Controllers
{
    public class SanPhamController : Controller
    {
        public  CategoryTypes CategoryType { get { return CategoryTypes.Model; } }

        public ApplicationDbContext db = new ApplicationDbContext();

        public virtual ActionResult _Sidebar()
        {
            var categories = db.Categories.Where(c => c.CategoryTypeId == (int)CategoryType).ToList();
            return PartialView(categories);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category(int category_id)
        {
            return View();
        }

        public ActionResult ListCategory()
        {
            List<Category> listcategory = db.Categories.ToList();
            return View(listcategory);
        }

        public ActionResult CanvasList(int? room, int? type)
        {
            IList<Product> products = (from product in db.Products
                                       where product.Status == true
                                      select product).ToList<Product>();

            if(room != null)
            {
                products = (from product in products
                            where product.Categories.Select<Category,int>(c => c.Id).ToList<int>().Contains(room.Value)
                            select product).ToList<Product>();
            }
            if(type != null)
            {
                products = (from product in products
                            where product.Categories.Select<Category,int>(c => c.Id).ToList<int>().Contains(type.Value)
                            select product).ToList<Product>();
            }
            return PartialView(products);
        }

        public ActionResult CategoryProduct(int? categoryid)
        {
            List<Product> listproduct;
            if (categoryid == null)
            {
                listproduct = db.Products.ToList();
            }
            else
            {
                Category category = db.Categories.Where(n => n.Id == categoryid) as Category;
                listproduct = db.Products.Where(n => n.Categories.Contains(category)).ToList();
            }
            return View(listproduct);
        }

        public List<Product> FilterByCategory(string[] args)
        {
            var products = db.Products.ToList();
            foreach (var arg in args)
            {
                int number;
                if (int.TryParse(arg,out number))
                    products = products.Where(o => o.Categories.Select(e => e.Id).ToList().Contains(number)).ToList();
            }
            return products;
        }

        /// <summary>
        /// Get product via ajax call
        /// </summary>
        /// <param name="ids">categories id</param>
        /// <returns></returns>
        public JsonResult ProductsJson(string[] ids)
        {
            var list = this.FilterByCategory(ids);
            var json = new List<object>();
            foreach (var item in list)
            {
                var details = item.DetailsToDictionary();
                var typo = item.Categories.FirstOrDefault(o => o.CategoryTypeId == (int)CategoryTypes.Typology);

                var materialList = new List<object>();
                var tList = new List<Product>();
                tList.Add(item);
                var materials = GetCMaterialOfProducts(tList);

                foreach (var material in materials)
                {
                    materialList.Add(new
                    {
                        id = material.Id
                    });
                }

                json.Add(new
                {
                    id = item.Id,
                    name = item.Name,
                    image = item.Image.FullFileName,
                    value = Convert.ToInt32(details["gia"]),
                    materials = materialList,
                    typo = typo.Id,
                    sale = item.CurrentSale != null ? item.CurrentSale.Percent : 0
                });
            }
            return Json(json, JsonRequestBehavior.AllowGet);     
        }

        /// <summary>
        /// taking all of the material relating to the category
        /// </summary>
        /// <param name="typo_id">Id of the category</param>
        /// <param name="model_id">If 'category_id' is -1 and 'model_id' has been set, this will getting materials contained in the model</param>
        /// <returns></returns>       
        public JsonResult GetMetarialsJson(string[] ids)
        {
            var resuilt = new List<object>();
            var products = this.FilterByCategory(ids);
            var cMaterials = GetCMaterialOfProducts(products);

            foreach (var cMaterial in cMaterials)
            {
                resuilt.Add(new
                {
                    id = cMaterial.Id,
                    name = cMaterial.Name
                });
            }
            return Json(resuilt, JsonRequestBehavior.AllowGet);
        }

        private List<Category> GetCMaterialOfProducts(IEnumerable<Product> products)
        {
            var pMaterials = new List<Product_Variant>();
            var cMaterials = new List<Category>();
            foreach (var product in products)
            {
                pMaterials = pMaterials
                    .Concat(product.Materials.ToList())
                    .Distinct()
                    .ToList();
            }
            foreach (var pMaterial in pMaterials)
            {
                cMaterials = cMaterials
                    .Concat(pMaterial.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Material))
                    .Distinct()
                    .ToList();
            }
            return cMaterials;
        }

        public ViewResult Product_Detail(int? id)
        {
            var product = db.Products.Find(id);
            ViewBag.Title = product.Name;
            return View(product);
        }

        public PartialViewResult AjaxProductDetail(int id)
        {
            var product = db.Products.Find(id);
            return PartialView(product);
        }

        public ActionResult GetImages(int? optionId)
        {
            var result = new List<string>();
            if (optionId != null)
            {
                var option = db.Product_Options.Find(optionId);
                if (option != null)
                {
                    result.Add(option.PreviewImage.FullFileName);
                    result.AddRange(option.Files.Select(o => o.Id + o.Extension));
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOptionData(int? optionId)
        {
            var option = db.Product_Options.Find(optionId);
            dynamic result = null;
            if (option != null)
            {
                var images = new List<string>();
                images.Add(option.PreviewImage.FullFileName);
                images.AddRange(option.Files.Select(o => o.Id + o.Extension));
                var quantity = int.Parse(option.Product_Options_Details.FirstOrDefault(o => o.Name == ProductDetailTypes.Quantity).Value ?? "0");
                result = new { images = images, quantity = quantity };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}