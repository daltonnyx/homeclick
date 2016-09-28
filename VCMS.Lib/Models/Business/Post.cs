namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Post : BaseModel
    {
        public Post()
        {
            Post_Details = new HashSet<Post_Details>();
            Categories = new HashSet<Category>();
            Post_ProductOptions = new HashSet<Post_Product>();
            Files = new HashSet<File>();
        }

        [Key]
        public new int Id { get; set; }

        [StringLength(128)]
        public string Title { get; set; }

        [StringLength(128)]
        public string Alias { get; set; }

        public string Content { get; set; }

        public bool Status { get; set; }

        public bool Featured { get; set; }

        [StringLength(128)]
        public string ImageId { get; set; }

        public string Excerpt { get; set; }

        public DateTime? PostDate { get; set; }

        [ForeignKey("ImageId")]
        public virtual File ImageFile { get; set; }

        public virtual ICollection<Post_Details> Post_Details { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Post_Product> Post_ProductOptions { get; set; }

        public virtual ICollection<File> Files { get; set; }

    }

    public partial class Post
    {
        public PostTypes PostType
        {
            get
            {
                var category = Categories.FirstOrDefault();
                switch (category.Category_TypeId)
                {
                    case (int)PostTypes.Collection:
                        return PostTypes.Collection;
                    case (int)PostTypes.Blog:
                        return PostTypes.Blog;
                }
                return PostTypes.Default;
            }
        }
    }

    public enum PostTypes { Default = -1, Collection = 5, Blog = 26}
}
