using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
