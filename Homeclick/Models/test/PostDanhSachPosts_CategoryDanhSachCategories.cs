//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Homeclick.Models.test
{
    using System;
    using System.Collections.Generic;
    
    public partial class PostDanhSachPosts_CategoryDanhSachCategories
    {
        public Nullable<int> DanhSachCategories { get; set; }
        public Nullable<int> DanhSachPosts { get; set; }
        public int OID { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Post Post { get; set; }
    }
}