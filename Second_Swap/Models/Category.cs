using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Avatar { get; set; }

    public string? Name { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreateDay { get; set; }

    public string? Description { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual ICollection<Brand> Brands { get; } = new List<Brand>();

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
