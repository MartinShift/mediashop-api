using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaShop.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        return Ok(await _categoryService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        return Ok(await _categoryService.GetByIdAsync(id));
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
    {
        return Ok(await _categoryService.CreateAsync(categoryDto));
    }
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto categoryDto)
    {
        return Ok(await _categoryService.UpdateAsync(categoryDto));
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("name")]
    public async Task<IActionResult> GetCategoryByName([FromQuery] string name)
    {
        return Ok(await _categoryService.GetByNameAsync(name));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCategories([FromQuery] string query)
    {
        return Ok(await _categoryService.SearchAsync(query));
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTopCategories()
    {
        return Ok(await _categoryService.GetTopCategoriesAsync());
    }
}
