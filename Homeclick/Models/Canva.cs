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
    
    public partial class Canva
    {
        public int Id { get; set; }
        public Nullable<int> room_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public string json_data { get; set; }
        public Nullable<int> cart_id { get; set; }
        public Nullable<int> RoomId { get; set; }
        public Nullable<int> CartId { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual Cart Cart { get; set; }
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
