using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class Ward
{
    public int Id { get; set; }

    public int? DistrictId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();
}
