using Microsoft.AspNetCore.Mvc;
using ProductApi.Dtos;
using ProductApi.Services;
using Microsoft.AspNetCore.Authorization;

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
            [FromQuery] decimal? maxPrice)
        {
            var products = await _service.GetAllAsync(category, minPrice, maxPrice);

            return Ok(new
            {
                message = "Products retrieved successfully.",
                count = products.Count(),
                data = products
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
