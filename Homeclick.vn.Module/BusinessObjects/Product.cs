using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.Model;
using DevExpress.Data.Filtering;
using System.Text.RegularExpressions;

namespace Homeclick.vn.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Sản phẩm"),DefaultProperty("name")]
    [TypeConverter("ProductTypeConverter")]
    public partial class Product
    {

        //public static string[] defaultDetail = { "product_gallery", "product_price" };
        public Product(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }

        [DevExpress.Xpo.Association, Browsable(false)]
        public IList<DanhSachProductsDanhSachCarts> ProductToCartLinks
        {
            get
            {
                return GetList<DanhSachProductsDanhSachCarts>("ProductToCartLinks");
            }
        }

        [ManyToManyAlias("ProductToCartLinks", "DanhSachCarts")]
        public IList<Cart> Carts
        {
            get
            {
                return GetList<Cart>("Carts");
            }
        }

        [DevExpress.Xpo.Association, Browsable(false)]
        public IList<DanhSachProductsyDanhSachCategories> ProductToCategoryLinks
        {
            get
            {
                return GetList<DanhSachProductsyDanhSachCategories>("ProductToCategoryLinks");
            }
        }

        [ManyToManyAlias("ProductToCategoryLinks", "DanhSachCategories")]
        public IList<Category> Categories
        {
            get
            {
                return GetList<Category>("Categories");
            }
        }


        [Browsable(false), DevExpress.Xpo.Association]
        public IList<ProductsLayoutsLinkTable> ProductLayoutLink
        {
            get { return GetList<ProductsLayoutsLinkTable>("ProductLayoutLink"); }

        }


        [ManyToManyAlias("ProductLayoutLink", "Layout")]
        public IList<ProjectLayout> Layouts
        {
            get { return GetList<ProjectLayout>("Layouts"); }
        }


        [NonPersistent]
        public ProductEnum StatusEnum
        {
            get { return (ProductEnum)this.status; }
            set { this.status = (short)value; }
        }

        private bool isNewObject = false;
        protected override void OnSaving()
        {
            if (this.Session.IsNewObject(this))
            { 
                isNewObject = true;
                this.created_date = DateTime.Now;
            }
            this.updated_date = DateTime.Now;
            base.OnSaving();
            
        }



        protected override void OnSaved()
        {
            base.OnSaved();
            if (!this.isNewObject)
                return;
            using (UnitOfWork uow = new UnitOfWork(this.Session.DataLayer))
            {
                Product_detail productDetail = new Product_detail(uow);
                productDetail.name = "gallery";
                productDetail.caption = "Gallery";
                productDetail.typeEnum = TypeEnum.multiupload;
                productDetail.value = string.Empty;
                productDetail.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail.Save();

                Product_detail productDetail2 = new Product_detail(uow);
                productDetail2.name = "gia";
                productDetail2.caption = "Giá";
                productDetail2.typeEnum = TypeEnum.str;
                productDetail2.value = string.Empty;    
                productDetail2.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail2.Save();

                Product_detail productDetail3 = new Product_detail(uow);
                productDetail3.name = "xuat_xu";
                productDetail3.caption = "Xuất xứ";
                productDetail3.typeEnum = TypeEnum.str;
                productDetail3.value = string.Empty;
                productDetail3.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail3.Save();

                Product_detail productDetail4 = new Product_detail(uow);
                productDetail4.name = "bao_hanh";
                productDetail4.caption = "Bảo hành";
                productDetail4.typeEnum = TypeEnum.str;
                productDetail4.value = string.Empty;
                productDetail4.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail4.Save();

                Product_detail productDetail5 = new Product_detail(uow);
                productDetail5.name = "kich_thuoc";
                productDetail5.caption = "Kích thước";
                productDetail5.typeEnum = TypeEnum.str;
                productDetail5.value = string.Empty;
                productDetail5.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail5.Save();

                Product_detail productDetail6 = new Product_detail(uow);
                productDetail6.name = "dat_hang";
                productDetail6.caption = "Đặt hàng";
                productDetail6.typeEnum = TypeEnum.str;
                productDetail6.value = string.Empty;
                productDetail6.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail6.Save();

                Product_detail productDetail7 = new Product_detail(uow);
                productDetail7.name = "image";
                productDetail7.caption = "Hình ảnh";
                productDetail7.typeEnum = TypeEnum.upload;
                productDetail7.value = string.Empty;
                productDetail7.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail7.Save();

                Product_detail productDetail8 = new Product_detail(uow);
                productDetail8.name = "data";
                productDetail8.caption = "Vector Image";
                productDetail8.typeEnum = TypeEnum.upload;
                productDetail8.value = string.Empty;
                productDetail8.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail8.Save();

                Product_detail productDetail9 = new Product_detail(uow);
                productDetail9.name = "zdata";
                productDetail9.caption = "Z-data";
                productDetail9.typeEnum = TypeEnum.str;
                productDetail9.value = string.Empty;
                productDetail9.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail9.Save();

                Product_detail productDetail10 = new Product_detail(uow);
                productDetail10.name = "scale";
                productDetail10.caption = "Scale";
                productDetail10.typeEnum = TypeEnum.str;
                productDetail10.value = "0";
                productDetail10.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail10.Save();

                Product_detail productDetail11 = new Product_detail(uow);
                productDetail11.name = "initScale";
                productDetail11.caption = "Tỉ lệ";
                productDetail11.typeEnum = TypeEnum.str;
                productDetail11.value = "0.18";
                productDetail10.ProductId = uow.FindObject<Product>(new BinaryOperator("Id", this.Id));
                productDetail11.Save();

                if (this.excerpt == string.Empty)
                {
                    string[] words = HtmlHelper.StripTagsCharArray(this.content).Split(' ');
                    string the_excerpt = string.Empty;
                    for (int i = 0; i < 50; i++)
                    {
                        the_excerpt += words[i];
                        if (i < 50)
                            the_excerpt += " ";
                    }
                    this.excerpt = the_excerpt;
                }
                uow.CommitChanges();
            }
        }
    }


    public enum ProductEnum
    {
        draft= 0,
        published = 1,
        trashed = 9,
    }

    public static class HtmlHelper
    {
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
    public class ProductTypeConverter : TypeConverter
    {
        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is String)
            {
                Regex regexr = new Regex(@"\d+(?=\)$)");
                string id = regexr.Match((string)value).ToString();
                Session session = new Session();
                Product child = session.GetObjectByKey<Product>(Convert.ToInt32(id));

                return child;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override Object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, Type destinationType)
        {
            if (destinationType == typeof(String))
            {
                Product childObject = (Product)value;

                return childObject.name + "(" + childObject.Id.ToString() + ")";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
