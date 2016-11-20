namespace VCMS.Lib.Models
{
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Core.Objects.DataClasses;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class File : BaseModel
    {
        public File()
        {
            Categories = new HashSet<Category>();
        }

        public string Id { get; set; }
        
        [Display(Name = "FileName", ResourceType = typeof(Strings))]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Extension { get; set; }

        public long Size { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Product_Option> Product_Options { get; set; }

        [Display(Name = "FileType", ResourceType = typeof(Strings))]
        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Project> ProjectPreviews { get; set; }
    }

    public partial class File
    {
        public FileTypes FileType
        {
            get
            {
                var category = Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.FileType).FirstOrDefault();
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

        public string GetThumb()
        {
            return "";
        }
    }


    public enum FileTypes {Default = -1, Image = 74, Other = 75 }

    public enum FileGroups { Default = -1, ProductImage = 78, ProductVariantImage = 79, Other = 80 }
}
