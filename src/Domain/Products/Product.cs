using SharedKernel;

namespace Domain.Products;

public sealed class Product : Entity
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }

    public string Category { get; private set; }
    private Product()
    {
    }
    private Product(
        Guid id,
        string name,
        string description,
        decimal price,
        int stock,
        string category)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        Category = category;
    }

    public static Result<Product> Create(
    string name,
    string description,
    decimal price,
    int stock,
    string category)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Product>(
                Error.Problem(
                    "Product.Name.Empty",
                    "Product name cannot be empty"));
        }

        if (price <= 0)
        {
            return Result.Failure<Product>(
                Error.Problem(
                    "Product.Price.Invalid",
                    "Price must be greater than zero"));
        }

        if (stock < 0)
        {
            return Result.Failure<Product>(
                Error.Problem(
                    "Product.Stock.Invalid",
                    "Stock cannot be negative"));
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            return Result.Failure<Product>(
                Error.Problem(
                    "Product.Category.Empty",
                    "Category cannot be empty"));
        }

        var product = new Product(
            Guid.NewGuid(),
            name,
            description,
            price,
            stock,
            category);

        return Result.Success(product);
    }
    public Result ReduceStock(int quantity)
    {
        if (quantity <= 0)
        {
            return Result.Failure(
                Error.Problem(
                    "Product.InvalidQuantity",
                    "Quantity must be greater than zero"));
        }

        if (Stock < quantity)
        {
            return Result.Failure(
                Error.Conflict(
                    "Product.InsufficientStock",
                    "Not enough stock"));
        }

        Stock -= quantity;

        return Result.Success();
    }
}
