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

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<File> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Files)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Files1)
                .WithOptional(e => e.AspNetUser1)
                .HasForeignKey(e => e.UpdateUserId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Category1)
                .WithOptional(e => e.Category2)
                .HasForeignKey(e => e.ParentCategoryId);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Files)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.FileTypeId);
        }
    }
}
