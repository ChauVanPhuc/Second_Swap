using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LastUpdated { get; set; }

    public int? CategoryId { get; set; }

    public string? Avatar { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
