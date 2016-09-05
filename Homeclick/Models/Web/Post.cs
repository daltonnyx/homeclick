namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            Post_detail = new HashSet<Post_detail>();
            Post1 = new HashSet<Post>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string title { get; set; }

        [StringLength(100)]
        public string alias { get; set; }

        public string content { get; set; }

        public int? author_id { get; set; }

        public DateTime? created_date { get; set; }

        public DateTime? updated_date { get; set; }

        public int? parent_id { get; set; }

        public short? status { get; set; }

        public int? UserId { get; set; }

        public int? PostParentId { get; set; }

        public bool? featured { get; set; }

        [StringLength(100)]
        public string image { get; set; }

        public string excerpt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post_detail> Post_detail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Post1 { get; set; }

        public virtual Post Post2 { get; set; }

        public virtual User User { get; set; }
    }
}
