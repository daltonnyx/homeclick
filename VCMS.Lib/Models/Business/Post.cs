namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    public partial class Post : BaseModel
    {
        public Post()
        {
            Post_Details = new HashSet<Post_Details>();
            Post_ProductOptions = new HashSet<Post_Product>();
            Files = new HashSet<File>();
            //Rooms = new HashSet<Room>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(128)]
        public string Title { get; set; }

        [StringLength(128)]
        public string Alias { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public bool Status { get; set; }

        public bool Featured { get; set; }

        [StringLength(128)]
        public string ImageId { get; set; }

        public string Excerpt { get; set; }

        public int? PostTypeId { get; set; }

        public int? RoomId { get; set; }

        public DateTime? PostDate { get; set; }

        [ForeignKey("ImageId")]
        public virtual File ImageFile { get; set; }

        public virtual Room Room { get; set; }

        //public virtual ICollection<Room> Rooms { get; set; }

        public virtual ICollection<Post_Details> Post_Details { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Post_Product> Post_ProductOptions { get; set; }

        //public virtual ICollection<Product_Option_Set> ProductOptionSets { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }

    public partial class Post
    {
        [NotMapped]
        public Dictionary<string, bool> SelectedCategories { get; set; }

        [NotMapped]
        public Dictionary<string, int> ProductOptionSets { get; set; }

        [NotMapped]
        public string[] SlideImages { get; set; }
        /*
        public PostTypes PostType
        {
            get
            {
                var category = Categories.FirstOrDefault();
                switch (category.CategoryTypeId)
                {
                    case (int)PostTypes.Collection:
                        return PostTypes.Collection;
                    case (int)PostTypes.Blog:
                        return PostTypes.Blog;
                }
                return PostTypes.Default;
            }
        }
        */
    }

    public class PostEntityConfiguration : EntityTypeConfiguration<Post>
    {
        public PostEntityConfiguration()
        {
            this.HasMany(o => o.Post_Details)
                            .WithOptional(o => o.Post)
                            .HasForeignKey(o => o.PostId);

            this.HasMany(e => e.Post_ProductOptions)
                .WithOptional(o => o.Post)
                .HasForeignKey(o => o.PostId);

            this.HasOptional(e => e.Room)
                .WithMany(e => e.Collections).HasForeignKey(e => e.RoomId);

            this.HasMany(e => e.Files)
                .WithMany(e => e.Posts)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Posts_Files_Link");
                });
            /*
            this.HasMany(e => e.ProductOptionSets)
                .WithMany(e => e.Posts)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Posts_Product_Option_Sets_Link");
                });
                */
        }
    }

    public enum PostTypes { Default = -1, Collection = 5, Blog = 26}
}
