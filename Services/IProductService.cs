using ProductApi.Dtos;

namespace ProductApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDto>> GetAllAsync(string? category, decimal? minPrice, decimal? maxPrice);
        Task<ProductReadDto?> GetByIdAsync(int id);
        Task<ProductReadDto> CreateAsync(ProductCreateDto dto);
        Task<bool> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
