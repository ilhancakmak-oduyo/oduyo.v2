using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(int productId, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new InvalidOperationException("Ürün bulunamadı.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> GetActiveProductsAsync()
        {
            return await _context.Products.Where(p => p.IsActive).ToListAsync();
        }
    }
}