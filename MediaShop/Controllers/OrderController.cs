using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaShop.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        return Ok(await _orderService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        return Ok(await _orderService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
    {
        return Ok(await _orderService.CreateAsync(orderDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder([FromBody] OrderDto orderDto)
    {
        return Ok(await _orderService.UpdateAsync(orderDto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _orderService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetOrdersByUser([FromQuery] int userId)
    {
        return Ok(await _orderService.GetByUserIdAsync(userId));
    }

    [HttpGet("{userId}/pending")]
    public async Task<IActionResult> GetPendingOrders(int userId)
    {
        return Ok(await _orderService.GetPendingOrdersAsync(userId));
    }
}
