using MongoDB.Driver;
using MongoDbCrud.Api.Models;
using MongoDbCrud.Api.DTOs;

namespace MongoDbCrud.Api.Services;

public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _products;

    public ProductService(DatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _products = database.GetCollection<Product>(settings.CollectionName);
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _products.Find(_ => true).ToListAsync();
        return products.Select(MapToDto).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(string id)
    {
        var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        return product != null ? MapToDto(product) : null;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Category = dto.Category,
            InStock = dto.InStock,
            CreatedAt = DateTime.UtcNow
        };

        await _products.InsertOneAsync(product);
        return MapToDto(product);
    }

    public async Task<bool> UpdateAsync(string id, CreateProductDto dto)
    {
        var update = Builders<Product>.Update
            .Set(p => p.Name, dto.Name)
            .Set(p => p.Price, dto.Price)
            .Set(p => p.Category, dto.Category)
            .Set(p => p.InStock, dto.InStock);

        var result = await _products.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _products.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    private static ProductDto MapToDto(Product product) => new()
    {
        Id = product.Id!,
        Name = product.Name,
        Price = product.Price,
        Category = product.Category,
        InStock = product.InStock,
        CreatedAt = product.CreatedAt
    };
}
