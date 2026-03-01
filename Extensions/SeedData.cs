using MongoDbCrud.Api.DTOs;
using MongoDbCrud.Api.Services;

namespace MongoDbCrud.Api.Extensions;

public static class SeedData
{
    public static async Task SeedDatabase(IProductService productService)
    {
        var existingProducts = await productService.GetAllAsync();
        if (existingProducts.Any())
        {
            Console.WriteLine("Database already has data. Skipping seed.");
            return;
        }

        Console.WriteLine("Seeding database with sample data...");

        var sampleProducts = new List<CreateProductDto>
        {
            new()
            {
                Name = "Gaming Laptop",
                Price = 1299.99m,
                Category = "Electronics",
                InStock = true
            },
            new()
            {
                Name = "Wireless Mouse",
                Price = 29.99m,
                Category = "Accessories",
                InStock = true
            },
            new()
            {
                Name = "Mechanical Keyboard",
                Price = 89.99m,
                Category = "Accessories",
                InStock = true
            },
            new()
            {
                Name = "USB-C Hub",
                Price = 45.50m,
                Category = "Accessories",
                InStock = false
            },
            new()
            {
                Name = "27\" Monitor",
                Price = 299.99m,
                Category = "Electronics",
                InStock = true
            }
        };

        foreach (var product in sampleProducts)
        {
            await productService.CreateAsync(product);
            Console.WriteLine($"Created: {product.Name}");
        }

        Console.WriteLine("Sample data seeded successfully!");
    }
}
