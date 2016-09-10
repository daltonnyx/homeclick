namespace VCMS.Lib.Models
{
    using Business;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class File : BaseModel
    {
        public File()
        {
            Categories = new HashSet<Category>();
        }
        
        [Display(Name = "FileName", ResourceType = typeof(Strings))]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Extension { get; set; }


        public long Size { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Product_Variant> Product_Variants { get; set; }

        [Display(Name = "FileType", ResourceType = typeof(Strings))]
        public virtual ICollection<Category> Categories { get; set; }
    }

    public partial class File
    {
        public FileTypes FileType
        {
            get
            {
                var category = Categories.Where(o => o.Category_typeId == (int)CategoryTypes.FileType).FirstOrDefault();
                switch(category.Id)
                {
                    case (int)FileTypes.Image:
                        return FileTypes.Image;
                    case (int)FileTypes.Other:
                        return FileTypes.Other;
                    default:
                        return FileTypes.Default;
                }
            }
        }

        public string FullFileName
        {
            get
            {
                return Id + Extension;
            }
        }
    }


    public enum FileTypes {Default = -1, Image = 74, Other = 75 }

    public enum FileGroups { Default = -1, ProductImage = 78, ProductVariantImage = 79, Other = 80 }
}
