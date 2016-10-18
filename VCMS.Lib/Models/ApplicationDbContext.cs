using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using VCMS.Lib.Models;

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

        public virtual DbSet<CustomField_Enums> CustomField_Enums { get; set; }
        public virtual DbSet<CustomField> CustomFields { get; set; }

        public virtual DbSet<User_Detail> User_Details { get; set; }

        public virtual DbSet<Canva> Canvas { get; set; }
        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }

        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Project_Details> Project_Details { get; set; }
        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Slide> Slides { get; set; }

        public virtual DbSet<Product_Option> Product_Options { get; set; }
        public virtual DbSet<Product_Options_Details> Product_Options_Details { get; set; }

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

            //ApplicationUser
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Details)
                .WithOptional(e => e.User);

            //Category
            //--------------------------------------
            modelBuilder.Entity<Category_Type>().HasMany(e => e.Categories)
                .WithOptional(e => e.CategoryType)
                .HasForeignKey(o => o.CategoryTypeId);

            //Category
            //--------------------------------------
            modelBuilder.Entity<Category>().HasMany(e => e.Children)
                .WithOptional(e => e.CategoryParent);

            modelBuilder.Entity<Category>().HasMany(e => e.Projects)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.CategoryId);

            modelBuilder.Entity<Category>().HasMany(e => e.Slides)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.CategoryId);

            modelBuilder.Entity<Category>().HasMany(e => e.Category_details)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Category>().HasMany(e => e.CustomFields)
                .WithOptional(e => e.Category);

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

            modelBuilder.Entity<Product>().HasMany(e => e.Product_Options)
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
                .HasMany(e => e.Categories)
                .WithMany(e => e.ProductVariants)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Variants_Category_Link");
                });

            modelBuilder.Entity<Product_Variant>()
                .HasMany(e => e.Product_Options)
                .WithMany(e => e.Product_Variants)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Variants_Product_Options_Link");
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
                .HasMany(e => e.Post_ProductOptions)
                .WithOptional(o => o.Post)
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
                .HasRequired(o => o.ProductOption)
                .WithMany(o => o.Post_Products)
                .HasForeignKey(o => o.ProductOptionId);


            //----------------------------------------
            modelBuilder.Entity<Order>()
                .HasMany(e => e.Orders_Products)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            //Sale
            //----------------------------------------
            modelBuilder.Entity<Sale>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Sales)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Sale_Product_Link");
                });

            //Product_Options
            //----------------------------------------
            modelBuilder.Entity<Product_Option>()
                .HasMany(e => e.Product_Options_Details)
                .WithOptional(e => e.Product_Options)
                .HasForeignKey(e => e.ProductOptionId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Product_Option>()
                .HasMany(e => e.Files)
                .WithMany(e => e.Product_Options)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Options_Files_Link");
                });

            //Projects
            //----------------------------------------
            modelBuilder.Entity<Project>()
            .HasMany(e => e.Departments)
            .WithOptional(e => e.Project)
            .WillCascadeOnDelete();

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Project_Details)
                .WithOptional(e => e.Project)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Project>()
                .HasOptional(e => e.PreviewImage)
                .WithMany(e => e.ProjectPreviews);

            modelBuilder.Entity<Project>()
            .HasMany(e => e.Files)
            .WithMany(e => e.Projects)
            .Map(cs =>
            {
                cs.MapLeftKey("ParentId");
                cs.MapRightKey("ChildId");
                cs.ToTable("Projects_Files_Link");
            });

            //Cities
            //----------------------------------------
            modelBuilder.Entity<City>()
                .HasMany(o => o.Districts)
                .WithOptional(o => o.City);

            //Districts
            //----------------------------------------
            modelBuilder.Entity<District>()
                .HasMany(o => o.Projects)
                .WithOptional(o => o.District);

            //Department
            //----------------------------------------
            modelBuilder.Entity<Department>()
                .HasMany(e => e.Floors)
                .WithOptional(e => e.Department);

            //Floor
            //----------------------------------------
            modelBuilder.Entity<Floor>()
                .HasMany(e => e.Rooms)
                .WithOptional(e => e.Floor);

            //Room
            //----------------------------------------
            modelBuilder.Entity<Room>()
                .HasMany(e => e.Canvas)
                .WithOptional(e => e.Room);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.Collections)
                .WithMany(e => e.Rooms).Map( cs =>
                    {
                        cs.MapLeftKey("ParentId");
                        cs.MapRightKey("ChildId");
                        cs.ToTable("Rooms_Posts_Link");
                    }
                );
            //Customfields
            //----------------------------------------
            modelBuilder.Entity<CustomField>()
                .HasMany(e => e.CustomField_Enums)
                .WithOptional(e => e.CustomField);

            //modelBuilder.Configurations.Add(new CategoryEntityConfiguration());
            //modelBuilder.Configurations.Add(new ProductEntityConfiguration());
        }
    }
}
