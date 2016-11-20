namespace VCMS.Lib.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    public partial class MenuItem : BaseModel
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string ImageId {get;set;}

        public string Icon { get; set; }

        public int? CategoryId { get; set; }

        public int? ParentId { get; set; }

        public bool Status { get; set; }

        public Category Category { get; set; }

        public MenuItem Parent { get; set; }

        [ForeignKey("ImageId")]
        public File Image { get; set; }

        public ICollection<MenuItem> Children { get; set; }
    }

    /// <summary>
    /// Model Configuration
    /// </summary>
    public class MenuItemEntityConfiguration : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemEntityConfiguration()
        {
            this.HasOptional(e => e.Parent)
                .WithMany(e => e.Children);

            this.HasOptional(e => e.Category)
                .WithMany(e => e.MenuItems);
        }
    }

}
