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

        
  

     // Kjo është vetëm për TEST per me testu Global Exception Middleware
   // throw new Exception("Test exception from GetAll");

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
    if (page <= 0) page = 1;
    if (pageSize <= 0) pageSize = 10;

    var result = await _service.GetAllAsync(
        category, minPrice, maxPrice,
        page, pageSize,
        sortBy, sortOrder
    );

    return Ok(new
    {
        message = "Products retrieved successfully.",
        page = result.Page,
        pageSize = result.PageSize,
        totalCount = result.TotalCount,
        totalPages = result.TotalPages,
        data = result.Items
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
public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
{
    if (!ModelState.IsValid)
    {
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return BadRequest(new
        {
            message = "Validation failed.",
            errors
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
public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
{
    if (!ModelState.IsValid)
    {
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return BadRequest(new
        {
            message = "Validation failed.",
            errors
        });
    }

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
