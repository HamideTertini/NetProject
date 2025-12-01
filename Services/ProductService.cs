using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Dtos;
using ProductApi.Entities;
using ProductApi.Models;

namespace ProductApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProductReadDto>> GetAllAsync(
            string? category,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize,
            string? sortBy,
            string sortOrder)
        {
            var query = _context.Products
                .AsNoTracking()
                .AsQueryable();

           
            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category == category);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            bool desc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "name" => desc ? query.OrderByDescending(p => p.Name)
                                   : query.OrderBy(p => p.Name),

                    "price" => desc ? query.OrderByDescending(p => p.Price)
                                    : query.OrderBy(p => p.Price),

                    "category" => desc ? query.OrderByDescending(p => p.Category)
                                       : query.OrderBy(p => p.Category),

                    "createdat" => desc ? query.OrderByDescending(p => p.CreatedAt)
                                        : query.OrderBy(p => p.CreatedAt),

                    _ => query
                };
            }
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => ToReadDto(p))
                .ToListAsync();

            return new PagedResult<ProductReadDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = items
            };
        }

        public async Task<ProductReadDto?> GetByIdAsync(int id)
        {
            var p = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return null;

            return ToReadDto(p);
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

            return ToReadDto(p);
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

        private static ProductReadDto ToReadDto(Product p)
        {
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
    }
}
