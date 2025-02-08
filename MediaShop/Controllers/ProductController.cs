using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaShop.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return Ok(await _productService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        return Ok(await _productService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto, IFormFile? media, IFormFile? preview)
    {
        
        return Ok(await _productService.CreateAsync(productDto, media, preview));
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateProduct([FromForm] ProductDto productDto, IFormFile? media, IFormFile? preview)
    {
        return Ok(await _productService.UpdateAsync(productDto, media, preview));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        return Ok(await _productService.GetByUserIdAsync(userId));
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFiltered([FromQuery] ProductFilterDto filter)
    {
        return Ok(await _productService.GetFilteredAsync(filter));
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured()
    {
        return Ok(await _productService.GetFeaturedAsync());
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecent()
    {
        return Ok(await _productService.GetRecentAsync());
    }

    [HttpGet("{id}/detail")]
    public async Task<IActionResult> GetProductDetail(int id)
    {
        return Ok(await _productService.GetProductDetailAsync(id));
    }
}
