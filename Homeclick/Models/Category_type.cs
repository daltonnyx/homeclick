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
    
    public partial class Category_type
    {
        public Category_type()
        {
            this.Categories = new HashSet<Category>();
        }
    
        public int Id { get; set; }
        public string name { get; set; }
        public string caption { get; set; }
        public Nullable<int> typeFor { get; set; }
        public Nullable<short> type_for { get; set; }
    
        public virtual ICollection<Category> Categories { get; set; }
    }
}
