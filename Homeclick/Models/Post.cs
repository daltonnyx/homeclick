//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Post
    {
        public Post()
        {
            this.Post_detail = new HashSet<Post_detail>();
            this.Post1 = new HashSet<Post>();
            this.Categories = new HashSet<Category>();
        }
    
        public int Id { get; set; }
        public string title { get; set; }
        public string alias { get; set; }
        public string content { get; set; }
        public Nullable<int> author_id { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
        public Nullable<int> parent_id { get; set; }
        public Nullable<short> status { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> PostParentId { get; set; }
        public Nullable<bool> featured { get; set; }
        public string image { get; set; }
        public string excerpt { get; set; }
    
        public virtual ICollection<Post_detail> Post_detail { get; set; }
        public virtual ICollection<Post> Post1 { get; set; }
        public virtual Post Post2 { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
