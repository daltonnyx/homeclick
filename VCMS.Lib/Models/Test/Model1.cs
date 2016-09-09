namespace VCMS.Lib.Models.Test
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Product_Variants> Product_Variants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product_Variants>()
                .HasMany(e => e.Product_Variants1)
                .WithOptional(e => e.Product_Variants2)
                .HasForeignKey(e => e.ParentId);
        }
    }
}
