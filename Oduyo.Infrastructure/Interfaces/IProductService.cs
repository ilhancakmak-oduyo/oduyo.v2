using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(CreateProductDto dto);
        Task<Product> UpdateProductAsync(int productId, UpdateProductDto dto);
        Task<bool> DeleteProductAsync(int productId);
        Task<Product> GetProductByIdAsync(int productId);
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetActiveProductsAsync();
    }
}