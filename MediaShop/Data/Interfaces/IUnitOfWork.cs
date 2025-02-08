namespace MediaShop.Data.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }

    IRoleRepository RoleRepository { get; }

    IProductRepository ProductRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    IReviewRepository ReviewRepository { get; }

    IOrderRepository OrderRepository { get; }

    Task SaveChangesAsync();
}
