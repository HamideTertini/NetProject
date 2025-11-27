using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Dtos;
using ProductApi.Entities;

namespace ProductApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllAsync(string? category, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var products = await query.ToListAsync();

            return products.Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CreatedAt = p.CreatedAt,
                InStock = p.StockQuantity > 0
            });
        }

        public async Task<ProductReadDto?> GetByIdAsync(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return null;

            return new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CreatedAt = p.CreatedAt,
                InStock = p.StockQuantity > 0
            };
        }

        public async Task<ProductReadDto> CreateAsync(ProductCreateDto dto)
        {
            var p = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(p);
            await _context.SaveChangesAsync();

            return new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CreatedAt = p.CreatedAt,
                InStock = p.StockQuantity > 0
            };
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return false;

            p.Name = dto.Name;
            p.Category = dto.Category;
            p.Price = dto.Price;
            p.StockQuantity = dto.StockQuantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return false;

            _context.Products.Remove(p);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
