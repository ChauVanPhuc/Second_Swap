using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class Address
{
    public int Id { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public int? WardsId { get; set; }

    public virtual District? District { get; set; }

    public virtual Province? Province { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();

    public virtual Ward? Wards { get; set; }
}
