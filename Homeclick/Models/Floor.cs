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
    
    public partial class Floor
    {
        public Floor()
        {
            this.Rooms = new HashSet<Room>();
        }
    
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<int> block_id { get; set; }
        public string structure_link { get; set; }
        public Nullable<int> DepartmentId { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
