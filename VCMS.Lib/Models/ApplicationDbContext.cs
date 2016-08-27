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

            modelBuilder.Configurations.Add(new CategoryEntityConfiguration());
            modelBuilder.Configurations.Add(new ProductEntityConfiguration());
        }
    }
}
