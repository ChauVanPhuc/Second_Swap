using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? BuyerId { get; set; }

    public string? MethodPayment { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? Message { get; set; }

    public string? PaymentToken { get; set; }

    public DateTime? TokenExpiration { get; set; }

    public virtual User? Buyer { get; set; }

    public virtual Product? Product { get; set; }
}
