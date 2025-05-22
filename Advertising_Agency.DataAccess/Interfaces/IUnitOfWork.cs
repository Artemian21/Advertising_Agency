namespace Advertising_Agency.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IDiscountRepository Discounts { get; }
        IOrderItemRepository OrderItems { get; }
        IOrderRepository Orders { get; }
        IServiceRepository Services { get; }
        IUserRepository Users { get; }

        void Dispose();
        Task<int> SaveChangesAsync();
    }
}