
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
    
public partial class Cart
{

    public Cart()
    {

        this.Canvas = new HashSet<Canva>();

        this.Products = new HashSet<Product>();

    }


    public int Id { get; set; }

    public Nullable<int> user_id { get; set; }

    public Nullable<System.DateTime> created_date { get; set; }

    public Nullable<short> status { get; set; }

    public Nullable<int> UserId { get; set; }



    public virtual ICollection<Canva> Canvas { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<Product> Products { get; set; }

}

}
