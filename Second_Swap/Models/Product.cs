using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public double? Price { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreateDay { get; set; }

    public int? CreateBy { get; set; }

    public int? CategoryId { get; set; }

    public bool? Need { get; set; }

    public int? BrandId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? CreateByNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<ProductImage> ProductImages { get; } = new List<ProductImage>();
}
