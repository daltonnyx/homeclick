namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Product_Options")]
    public partial class Product_Option : BaseModel
    {
        public Product_Option()
        {
            Product_Options_Details = new HashSet<Product_Options_Details>();
            Post_Products = new HashSet<Post_Product>();
        }

        public new int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(128)]
        public string PreviewImageId { get; set; }

        public int? ProductId { get; set; }

        public bool Status { get; set; }

        [ForeignKey("PreviewImageId")]
        public virtual File PreviewImage { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<Product_Options_Details> Product_Options_Details { get; set; }

        public virtual ICollection<Product_Variant> Product_Variants { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<Post_Product> Post_Products { get; set; }
    }

    public partial class Product_Option : BaseModel
    {
        public Dictionary<string, string> Details
        {
            get
            {
                var dic = Product_Options_Details.ToDictionary(o => o.Name, o => o.Value);
                return dic;
            }
       }
    }
}
