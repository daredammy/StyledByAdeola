using System.Linq;

namespace StyledByAdeola.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order, bool newOrder);
    }
}