using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class Province
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();
}
