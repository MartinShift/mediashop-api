using AutoMapper;
using MediaShop.Business.Interfaces;
using MediaShop.Business.Models;
using MediaShop.Data.Entities;
using MediaShop.Data.Interfaces;
using MediaShop.Exceptions;

namespace MediaShop.Business.Services;

public class OrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger) : IOrderService
{
    private IUnitOfWork _unitOfWork { get; } = unitOfWork;

    private IMapper _mapper { get; } = mapper;

    private ILogger<OrderService> _logger { get; } = logger;

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all orders");
        var orders = await _unitOfWork.OrderRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
    public async Task<OrderDto> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting order with id {id}");
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
        return _mapper.Map<OrderDto>(order);
    }
    public async Task<OrderDto> CreateAsync(OrderDto orderDto)
    {
        _logger.LogInformation($"Creating order with id {orderDto.Id}");
        var order = _mapper.Map<Order>(orderDto);
        await _unitOfWork.OrderRepository.CreateAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<OrderDto>(order);
    }
    public async Task<OrderDto> UpdateAsync(OrderDto orderDto)
    {
        _logger.LogInformation($"Updating order with id {orderDto.Id}");
        var order = _mapper.Map<Order>(orderDto);
        var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(order.Id) ?? throw new NotFoundException("order not found");
        existingOrder.Status = order.Status;
        _unitOfWork.OrderRepository.Update(existingOrder);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<OrderDto>(order);
    }
    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation($"Deleting order with id {id}");
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(id) ?? throw new NotFoundException("order not found");
        _unitOfWork.OrderRepository.Delete(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<OrderDto>> GetByUserIdAsync(int userId)
    {
        _logger.LogInformation($"Getting orders by user id {userId}");
        var orders = await _unitOfWork.OrderRepository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetPendingOrdersAsync(int userId)
    {
        _logger.LogInformation($"Getting pending orders by user id {userId}");
        return (await _unitOfWork.OrderRepository.GetPendingAsync(userId)).Select(_mapper.Map<OrderDto>);
    }
}
