using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using VCMS.Lib.Models.Business;

namespace VCMS.Lib.Models
{
    public  class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("vinabits_homeclickEntities", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Orders_Products> Orders_Products { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Post_Details> Post_Details { get; set; }
        public virtual DbSet<Post_Product> Post_Products { get; set; }

        public virtual DbSet<Product_Variant> Product_Variants { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Category_detail> Category_details { get; set; }
        public virtual DbSet<Category_Type> Category_types { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Product_detail> Product_details { get; set; }
        public virtual DbSet<Product_type> Product_types { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Category
            //--------------------------------------
            modelBuilder.Entity<Category_Type>().HasMany(e => e.Categories)
                .WithOptional(e => e.Category_Type)
                .HasForeignKey(o => o.Category_TypeId);

            //Category
            //--------------------------------------
            modelBuilder.Entity<Category>().HasMany(e => e.Category_details)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Category>().HasMany(e => e.CategoryParents)
                .WithMany(e => e.CategoryChildren)
                .Map(cs =>
                {
                    cs.MapLeftKey("ChildId");
                    cs.MapRightKey("ParentId");
                    cs.ToTable("Category_Category_Link");
                });    
            //--------------------------------------

            //Product
            //--------------------------------------
            modelBuilder.Entity<Product>().HasMany(e => e.Product_detail)
                    .WithRequired(e => e.Product)
                    .WillCascadeOnDelete();


            modelBuilder.Entity<Product>().HasMany(e => e.Categories)
                .WithMany(e => e.Products)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Category_Link");
                });

            modelBuilder.Entity<Product>().HasMany(e => e.Product_Variants)
                .WithMany(e => e.Products)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Product_Variants_Link");
                });

            modelBuilder.Entity<Product>().HasMany(e => e.Files)
                .WithMany(e => e.Products)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Files_Link");
                });
            //--------------------------------------

            //Product Variant
            //--------------------------------------
            modelBuilder.Entity<Product_Variant>()
                .HasMany(o => o.Children)
                .WithOptional(o => o.Parent)
                .HasForeignKey(o => o.ParentId);

            modelBuilder.Entity<Product_Variant>()
                .HasMany(e => e.Files)
                .WithMany(e => e.Product_Variants)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Variants_Files_Link");
                });
            modelBuilder.Entity<Product_Variant>()
                .HasMany(e => e.Categories)
                .WithMany(e => e.ProductVariants)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Variants_Category_Link");
                });
            //--------------------------------------

            //Files
            //--------------------------------------
            modelBuilder.Entity<File>()
                .HasMany(e => e.Categories)
                .WithMany(e => e.Files)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Files_Category_Link");
                });
            //--------------------------------------

            //Post
            //--------------------------------------
            modelBuilder.Entity<Post>()
                .HasMany(o => o.Post_Details)
                .WithOptional(o => o.Post)
                .HasForeignKey(o => o.PostId);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Post_Products)
                .WithRequired(o => o.Post)
                .HasForeignKey(o => o.PostId);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Categories)
                .WithMany(e => e.Posts)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Posts_Category_Link");
                });

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Files)
                .WithMany(e => e.Posts)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Posts_Files_Link");
                });
            //--------------------------------------

            modelBuilder.Entity<Post_Product>()
                .HasRequired(o => o.Product)
                .WithMany(o => o.Post_Products)
                .HasForeignKey(o => o.ProductId);

            //----------------------------------------
            modelBuilder.Entity<Order>()
                .HasMany(e => e.Orders_Products)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            //modelBuilder.Configurations.Add(new CategoryEntityConfiguration());
            //modelBuilder.Configurations.Add(new ProductEntityConfiguration());
        }
    }
}
