using Microsoft.AspNetCore.Mvc;
using ProductApi.Dtos;
using ProductApi.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;


namespace ProductApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        
      [HttpGet]
public async Task<IActionResult> GetAll(
    [FromQuery] string? category,
    [FromQuery] decimal? minPrice,
    [FromQuery] decimal? maxPrice,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? sortBy = null,
    [FromQuery] string sortOrder = "asc")
{
     // Kjo është vetëm për TEST per me testu Global Exception Middleware
   // throw new Exception("Test exception from GetAll");

    if (page <= 0) page = 1;
    if (pageSize <= 0) pageSize = 10;

    var products = await _service.GetAllAsync(category, minPrice, maxPrice);

    IEnumerable<ProductReadDto> query = products;

    if (!string.IsNullOrWhiteSpace(sortBy))
    {
        bool desc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

        switch (sortBy.ToLower())
        {
            case "name":
                query = desc
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name);
                break;

            case "price":
                query = desc
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price);
                break;

            case "category":
                query = desc
                    ? query.OrderByDescending(p => p.Category)
                    : query.OrderBy(p => p.Category);
                break;

            case "createdat":
                query = desc
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt);
                break;

            default:
                break;
        }
    }

    var totalCount = query.Count();
    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

    var items = query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    return Ok(new
    {
        message = "Products retrieved successfully.",
        page,
        pageSize,
        totalCount,
        totalPages,
        data = items
    });
}

        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new
                {
                    message = $"Product with id {id} was not found."
                });
            }

            return Ok(new
            {
                message = "Product retrieved successfully.",
                data = product
            });
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto dto)
        {
            
            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Category) ||
                dto.Price <= 0)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = new[]
                    {
                        "Name, Category dhe Price janë të detyrueshme.",
                        "Price duhet të jetë > 0."
                    }
                });
            }

            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new
                {
                    message = "Product created successfully.",
                    data = created
                });
        }

        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ProductUpdateDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success)
            {
                return NotFound(new
                {
                    message = $"Cannot update. Product with id {id} was not found."
                });
            }

            return Ok(new
            {
                message = "Product updated successfully."
            });
        }

       
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound(new
                {
                    message = $"Cannot delete. Product with id {id} was not found."
                });
            }

            return Ok(new
            {
                message = "Product deleted successfully."
            });
        }
    }
}
