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

        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Category_detail> Category_details { get; set; }
        public virtual DbSet<Category_type> Category_types { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Product_detail> Product_details { get; set; }
        public virtual DbSet<Product_type> Product_types { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Category
            //--------------------------------------
            modelBuilder.Entity<Category>().HasMany(e => e.Category_detail)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Category>().HasMany(e => e.Files)
                .WithOptional(e => e.FileType)
                .HasForeignKey(e => e.FileTypeId);

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
                    .WithOptional(e => e.Product)
                    .WillCascadeOnDelete();

            modelBuilder.Entity<Product>().HasMany(e => e.Categories)
                .WithMany(e => e.Products)
                .Map(cs =>
                {
                    cs.MapLeftKey("ChildId");
                    cs.MapRightKey("ParentId");
                    cs.ToTable("Category_Product_Link");
                });
            //--------------------------------------


            //modelBuilder.Configurations.Add(new CategoryEntityConfiguration());
            //modelBuilder.Configurations.Add(new ProductEntityConfiguration());
        }
    }
}
