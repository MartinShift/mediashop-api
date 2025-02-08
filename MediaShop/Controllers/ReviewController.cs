using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaShop.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize]
public class ReviewController(IReviewService reviewService) : ControllerBase
{
    private readonly IReviewService _reviewService = reviewService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReview(int id)
    {
        return Ok(await _reviewService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewDto)
    {
        return Ok(await _reviewService.CreateAsync(reviewDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateReview([FromBody] ReviewDto reviewDto)
    {
        return Ok(await _reviewService.UpdateAsync(reviewDto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        await _reviewService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("product")]
    public async Task<IActionResult> GetReviewsByProduct([FromQuery] int productId)
    {
        return Ok(await _reviewService.GetByProductIdAsync(productId));
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetReviewsByUser([FromQuery] int userId)
    {
        return Ok(await _reviewService.GetByUserIdAsync(userId));
    }
}
