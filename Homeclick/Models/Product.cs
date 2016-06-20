
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
    
public partial class Product
{

    public Product()
    {

        this.Product_detail = new HashSet<Product_detail>();

        this.Categories = new HashSet<Category>();

        this.Tags = new HashSet<Tag>();

        this.Carts = new HashSet<Cart>();

        this.Wishlists = new HashSet<Wishlist>();

    }


    public int Id { get; set; }

    public string name { get; set; }

    public string content { get; set; }

    public Nullable<System.DateTime> created_date { get; set; }

    public Nullable<System.DateTime> updated_date { get; set; }

    public Nullable<short> status { get; set; }

    public Nullable<int> Product_type_id { get; set; }

    public Nullable<int> author_id { get; set; }

    public Nullable<int> Product_typeId { get; set; }

    public Nullable<int> UserId { get; set; }

    public Nullable<bool> featured { get; set; }

    public string image { get; set; }

    public string excerpt { get; set; }



    public virtual ICollection<Product_detail> Product_detail { get; set; }

    public virtual Product_type Product_type { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<Category> Categories { get; set; }

    public virtual ICollection<Tag> Tags { get; set; }

    public virtual ICollection<Cart> Carts { get; set; }

    public virtual ICollection<Wishlist> Wishlists { get; set; }

}

}
