using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class User
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public int? RoleId { get; set; }

    public DateTime? CreateDay { get; set; }

    public bool? Status { get; set; }

    public string? Avatar { get; set; }

    public DateTime? BirthDay { get; set; }

    public int? AddressId { get; set; }

    public string? Gender { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Message> MessageReceiverNavigations { get; } = new List<Message>();

    public virtual ICollection<Message> MessageSenderNavigations { get; } = new List<Message>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Product> Products { get; } = new List<Product>();

    public virtual Role? Role { get; set; }
}
