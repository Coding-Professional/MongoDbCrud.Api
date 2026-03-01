using MongoDbCrud.Api.DTOs;

namespace MongoDbCrud.Api.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(string id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<bool> UpdateAsync(string id, CreateProductDto dto);
    Task<bool> DeleteAsync(string id);
}
