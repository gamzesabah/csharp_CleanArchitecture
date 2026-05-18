using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Dtos;

public sealed class ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public string Category { get; init; }
}
