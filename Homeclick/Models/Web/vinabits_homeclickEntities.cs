namespace Homeclick.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class vinabits_homeclickEntities : DbContext
    {
        public vinabits_homeclickEntities()
            : base("name=vinabits_homeclickEntities")
        {

        }

        public virtual DbSet<Canva> Canvas { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Category_detail> Category_detail { get; set; }
        public virtual DbSet<Category_type> Category_type { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Post_detail> Post_detail { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Product_detail> Product_detail { get; set; }
        public virtual DbSet<Product_type> Product_type { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectItem> ProjectItems { get; set; }
        public virtual DbSet<ProjectLayout_Collection> ProjectLayout_Collections { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Category>()
            //    .HasMany(e => e.Category_detail)
            //    .WithOptional(e => e.Category)
            //    .HasForeignKey(d => d.CategoryId)
            //    .WillCascadeOnDelete();

            modelBuilder.Entity<Category_detail>()
                .HasOptional(c => c.Category)
                .WithMany(c => c.Category_detail)
                .HasForeignKey(c => c.CategoryId)
                .WillCascadeOnDelete();


            modelBuilder.Entity<Department>()
                .HasMany(e => e.Department1)
                .WithOptional(e => e.Department2)
                .HasForeignKey(e => e.ParentDepartmentId);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Post_detail)
                .WithOptional(e => e.Post)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Post1)
                .WithOptional(e => e.Post2)
                .HasForeignKey(e => e.PostParentId);

            modelBuilder.Entity<Project>()
                .Property(e => e.Apartments)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Project>()
                .HasOptional(p => p.Category)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<ProjectItem>()
                .HasMany(e => e.ProjectItem1)
                .WithOptional(e => e.ProjectItem2)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<ProjectItem>()
                .HasOptional(p => p.Category)
                .WithMany(c => c.ProjectItems)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<ProjectItem>()
                .HasMany(e => e.ProjectLayout_Collection)
                .WithOptional(e => e.ProjectItem)
                .HasForeignKey(e => e.LayoutId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Projects)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Projects1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.UpdatedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ProjectItems)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ProjectItems1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.UpdatedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ProjectLayout_Collection)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ProjectLayout_Collection1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.UpdatedBy);
        }

    }
}
