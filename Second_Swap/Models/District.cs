using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class District
{
    public int Id { get; set; }

    public int? ProvinceId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();
}
