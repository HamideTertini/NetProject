using ProductApi.Dtos;
 using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IProductService
    {
       

Task<PagedResult<ProductReadDto>> GetAllAsync(
    string? category,
    decimal? minPrice,
    decimal? maxPrice,
    int page,
    int pageSize,
    string? sortBy,
    string sortOrder);

        Task<ProductReadDto?> GetByIdAsync(int id);
        Task<ProductReadDto> CreateAsync(ProductCreateDto dto);
        Task<bool> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
