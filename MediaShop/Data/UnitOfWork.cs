using MediaShop.Data.Interfaces;

namespace MediaShop.Data;

public class UnitOfWork(IUserRepository userRepository, IRoleRepository roleRepository, IProductRepository productRepository, ICategoryRepository categoryRepository, IReviewRepository reviewRepository, IOrderRepository orderRepository, MediaShopContext context) : IUnitOfWork
{
    public IUserRepository UserRepository { get; } = userRepository;

    public IRoleRepository RoleRepository { get; } = roleRepository;

    public IProductRepository ProductRepository { get; } = productRepository;

    public ICategoryRepository CategoryRepository { get; } = categoryRepository;

    public IReviewRepository ReviewRepository { get; } = reviewRepository;

    public IOrderRepository OrderRepository { get; } = orderRepository;

    private MediaShopContext _context { get; } = context;

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
